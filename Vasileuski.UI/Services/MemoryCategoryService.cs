using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.Domain.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Сервис для работы с категориями в памяти с поддержкой CRUD
    /// </summary>
    public class MemoryCategoryService : ICategoryService
    {
        private List<Category> _categories;
        private readonly IWebHostEnvironment _environment;

        public MemoryCategoryService(IWebHostEnvironment environment)
        {
            _environment = environment;
            SetupData();
        }

        /// <summary>
        /// Наполнение данных категориями
        /// </summary>
        private void SetupData()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Футбол",
                    NormalizedName = "football",
                    Description = "Футбольные лиги и турниры",
                    Image = "/images/categories/football.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Category
                {
                    Id = 2,
                    Name = "Баскетбол",
                    NormalizedName = "basketball",
                    Description = "Баскетбольные лиги и турниры",
                    Image = "/images/categories/basketball.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Category
                {
                    Id = 3,
                    Name = "Хоккей",
                    NormalizedName = "hockey",
                    Description = "Хоккейные лиги и турниры",
                    Image = "/images/categories/hockey.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Category
                {
                    Id = 4,
                    Name = "Теннис",
                    NormalizedName = "tennis",
                    Description = "Теннисные турниры и ассоциации",
                    Image = "/images/categories/tennis.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Category
                {
                    Id = 5,
                    Name = "Волейбол",
                    NormalizedName = "volleyball",
                    Description = "Волейбольные лиги и турниры",
                    Image = "/images/categories/volleyball.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                }
            };
        }

        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var result = new ResponseData<List<Category>>
            {
                Data = _categories.OrderBy(c => c.Name).ToList(),
                Success = true
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// Получение категории по ID
        /// </summary>
        public Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Task.FromResult(ResponseData<Category>.Error($"Категория с ID {id} не найдена"));
            }

            return Task.FromResult(ResponseData<Category>.Ok(category));
        }

        /// <summary>
        /// Создание новой категории
        /// </summary>
        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category, IFormFile? imageFile)
        {
            // Валидация
            var validationResult = ValidateCategory(category);
            if (!validationResult.IsValid)
            {
                return ResponseData<Category>.Error(string.Join("; ", validationResult.Errors));
            }

            // Проверка уникальности имени
            var existsResponse = await CategoryExistsAsync(category.Name);
            if (existsResponse.Success && existsResponse.Data)
            {
                return ResponseData<Category>.Error($"Категория с именем '{category.Name}' уже существует");
            }

            // Генерация нормализованного имени
            category.NormalizedName = StringHelper.ToKebabCase(category.Name);

            // Сохранение изображения
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = await SaveImageAsync(imageFile);
                category.Image = $"/images/categories/{fileName}";
            }
            else
            {
                category.Image = "/images/categories/default.png";
            }

            // Установка ID и временных меток
            category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
            category.CreatedAt = DateTime.UtcNow;

            _categories.Add(category);

            return ResponseData<Category>.Ok(category, "Категория успешно создана");
        }

        /// <summary>
        /// Обновление существующей категории
        /// </summary>
        public async Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category, IFormFile? imageFile)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return ResponseData<Category>.Error($"Категория с ID {id} не найдена");
            }

            // Валидация
            var validationResult = ValidateCategory(category);
            if (!validationResult.IsValid)
            {
                return ResponseData<Category>.Error(string.Join("; ", validationResult.Errors));
            }

            // Проверка уникальности имени (исключая текущую категорию)
            var existsResponse = await CategoryExistsAsync(category.Name, id);
            if (existsResponse.Success && existsResponse.Data)
            {
                return ResponseData<Category>.Error($"Категория с именем '{category.Name}' уже существует");
            }

            // Обновление полей
            existingCategory.Name = category.Name;
            existingCategory.NormalizedName = StringHelper.ToKebabCase(category.Name);
            existingCategory.Description = category.Description;

            // Обновление изображения, если загружено новое
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = await SaveImageAsync(imageFile);
                existingCategory.Image = $"/images/categories/{fileName}";
            }
            else if (!string.IsNullOrEmpty(category.Image))
            {
                existingCategory.Image = category.Image;
            }

            return ResponseData<Category>.Ok(existingCategory, "Категория успешно обновлена");
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        public Task<ResponseData<bool>> DeleteCategoryAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return Task.FromResult(ResponseData<bool>.Error($"Категория с ID {id} не найдена"));
            }

            // Проверка, есть ли связанные команды
            // В реальном приложении здесь нужно проверить зависимости
            // Например: var teamsCount = _teamService.GetTeamsCountByCategory(id);

            _categories.Remove(category);
            return Task.FromResult(ResponseData<bool>.Ok(true, "Категория успешно удалена"));
        }

        /// <summary>
        /// Получение категории по нормализованному имени
        /// </summary>
        public Task<ResponseData<Category>> GetCategoryByNormalizedNameAsync(string normalizedName)
        {
            var category = _categories.FirstOrDefault(c =>
                c.NormalizedName.Equals(normalizedName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                return Task.FromResult(ResponseData<Category>.Error($"Категория с именем '{normalizedName}' не найдена"));
            }

            return Task.FromResult(ResponseData<Category>.Ok(category));
        }

        /// <summary>
        /// Проверка существования категории с указанным именем
        /// </summary>
        public Task<ResponseData<bool>> CategoryExistsAsync(string name, int? excludeId = null)
        {
            var normalizedName = StringHelper.ToKebabCase(name);
            var exists = _categories.Any(c =>
                c.NormalizedName.Equals(normalizedName, StringComparison.OrdinalIgnoreCase) &&
                (!excludeId.HasValue || c.Id != excludeId.Value));

            return Task.FromResult(ResponseData<bool>.Ok(exists));
        }

        /// <summary>
        /// Сохранение изображения категории
        /// </summary>
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return string.Empty;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "categories");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генерация уникального имени файла
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }

        /// <summary>
        /// Валидация категории
        /// </summary>
        private ValidationResult ValidateCategory(Category category)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                result.AddError("Название категории обязательно для заполнения");
            }
            else if (category.Name.Length > 100)
            {
                result.AddError("Название категории не должно превышать 100 символов");
            }

            if (category.Description?.Length > 500)
            {
                result.AddError("Описание не должно превышать 500 символов");
            }

            return result;
        }

        /// <summary>
        /// Получение статистики по категориям
        /// </summary>
        public Task<ResponseData<object>> GetStatisticsAsync()
        {
            var stats = new
            {
                TotalCategories = _categories.Count,
                LatestCategory = _categories.OrderByDescending(c => c.CreatedAt).FirstOrDefault()?.Name,
                OldestCategory = _categories.OrderBy(c => c.CreatedAt).FirstOrDefault()?.Name
            };

            return Task.FromResult(ResponseData<object>.Ok(stats));
        }

        /// <summary>
        /// Получение категорий с пагинацией
        /// </summary>
        public Task<ResponseData<PagedResponseData<Category>>> GetCategoriesPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = _categories.Count;
            var pagedCategories = _categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResponse = new PagedResponseData<Category>
            {
                Data = pagedCategories,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Success = true
            };

            return Task.FromResult(ResponseData<PagedResponseData<Category>>.Ok(pagedResponse));
        }
    }
}