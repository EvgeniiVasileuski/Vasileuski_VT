using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vasileuski.API.Data;
using Vasileuski.API.DTOs;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Helpers;

namespace Vasileuski.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoriesController(ApiDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                NormalizedName = c.NormalizedName,
                Description = c.Description,
                Image = c.Image,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Teams)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound(new { error = $"Category with ID {id} not found" });

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                NormalizedName = category.NormalizedName,
                Description = category.Description,
                Image = category.Image,
                CreatedAt = category.CreatedAt,
                Teams = category.Teams?.Select(t => new TeamDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Points = t.Points,
                    Position = t.Position
                }).ToList()
            };

            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDTO categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                return BadRequest(new { error = "Category name is required" });

            // Проверка уникальности
            if (await _context.Categories.AnyAsync(c => c.Name == categoryDto.Name))
                return BadRequest(new { error = $"Category with name '{categoryDto.Name}' already exists" });

            var category = new Category
            {
                Name = categoryDto.Name,
                NormalizedName = StringHelper.ToKebabCase(categoryDto.Name),
                Description = categoryDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            if (categoryDto.ImageFile != null && categoryDto.ImageFile.Length > 0)
            {
                category.Image = await SaveImageAsync(categoryDto.ImageFile, "categories");
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                NormalizedName = category.NormalizedName,
                Description = category.Description,
                Image = category.Image,
                CreatedAt = category.CreatedAt
            });
        }

        private async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{folder}/{fileName}";
        }
    }
}