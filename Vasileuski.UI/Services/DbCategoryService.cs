using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.UI.Data;
using Vasileuski.Domain.Helpers;

namespace Vasileuski.UI.Services
{
    public class DbCategoryService : ICategoryService
    {
        private readonly AdminDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DbCategoryService(AdminDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return ResponseData<List<Category>>.Ok(categories);
            }
            catch (Exception ex)
            {
                return ResponseData<List<Category>>.Error($"Ошибка при получении категорий: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Teams)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return ResponseData<Category>.Error($"Категория с ID {id} не найдена");
                }

                return ResponseData<Category>.Ok(category);
            }
            catch (Exception ex)
            {
                return ResponseData<Category>.Error($"Ошибка при получении категории: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category, IFormFile? imageFile)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(category.Name))
                    return ResponseData<Category>.Error("Название категории обязательно");

                // Проверка уникальности
                if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
                    return ResponseData<Category>.Error($"Категория с именем '{category.Name}' уже существует");

                // Генерация нормализованного имени
                category.NormalizedName = StringHelper.ToKebabCase(category.Name);
                category.CreatedAt = DateTime.UtcNow;

                // Сохранение изображения
                if (imageFile != null && imageFile.Length > 0)
                {
                    category.Image = await SaveImageAsync(imageFile, "categories");
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return ResponseData<Category>.Ok(category, "Категория успешно создана");
            }
            catch (Exception ex)
            {
                return ResponseData<Category>.Error($"Ошибка при создании категории: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category, IFormFile? imageFile)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                    return ResponseData<Category>.Error($"Категория с ID {id} не найдена");

                // Валидация
                if (string.IsNullOrWhiteSpace(category.Name))
                    return ResponseData<Category>.Error("Название категории обязательно");

                // Проверка уникальности (исключая текущую)
                if (await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id))
                    return ResponseData<Category>.Error($"Категория с именем '{category.Name}' уже существует");

                // Обновление полей
                existingCategory.Name = category.Name;
                existingCategory.NormalizedName = StringHelper.ToKebabCase(category.Name);
                existingCategory.Description = category.Description;

                // Обновление изображения
                if (imageFile != null && imageFile.Length > 0)
                {
                    existingCategory.Image = await SaveImageAsync(imageFile, "categories");
                }
                else if (!string.IsNullOrEmpty(category.Image))
                {
                    existingCategory.Image = category.Image;
                }

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

                return ResponseData<Category>.Ok(existingCategory, "Категория успешно обновлена");
            }
            catch (Exception ex)
            {
                return ResponseData<Category>.Error($"Ошибка при обновлении категории: {ex.Message}");
            }
        }

        public async Task<ResponseData<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return ResponseData<bool>.Error($"Категория с ID {id} не найдена");

                // Проверка на наличие связанных команд
                var hasTeams = await _context.Teams.AnyAsync(t => t.CategoryId == id);
                if (hasTeams)
                    return ResponseData<bool>.Error("Невозможно удалить категорию, так как с ней связаны команды");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return ResponseData<bool>.Ok(true, "Категория успешно удалена");
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Error($"Ошибка при удалении категории: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> GetCategoryByNormalizedNameAsync(string normalizedName)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.NormalizedName == normalizedName);

                if (category == null)
                    return ResponseData<Category>.Error($"Категория с именем '{normalizedName}' не найдена");

                return ResponseData<Category>.Ok(category);
            }
            catch (Exception ex)
            {
                return ResponseData<Category>.Error($"Ошибка при получении категории: {ex.Message}");
            }
        }

        public async Task<ResponseData<bool>> CategoryExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var query = _context.Categories.Where(c => c.Name == name);

                if (excludeId.HasValue)
                    query = query.Where(c => c.Id != excludeId.Value);

                var exists = await query.AnyAsync();
                return ResponseData<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Error($"Ошибка при проверке существования категории: {ex.Message}");
            }
        }

        private async Task<string?> SaveImageAsync(IFormFile imageFile, string folder)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{folder}/{fileName}";
        }
    }
}
