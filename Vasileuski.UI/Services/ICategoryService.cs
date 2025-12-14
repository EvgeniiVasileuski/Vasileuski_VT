using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;

namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с категориями
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        Task<ResponseData<List<Category>>> GetCategoryListAsync();

        /// <summary>
        /// Получение категории по ID
        /// </summary>
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Создание новой категории
        /// </summary>
        Task<ResponseData<Category>> CreateCategoryAsync(Category category, IFormFile? imageFile);

        /// <summary>
        /// Обновление существующей категории
        /// </summary>
        Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category, IFormFile? imageFile);

        /// <summary>
        /// Удаление категории
        /// </summary>
        Task<ResponseData<bool>> DeleteCategoryAsync(int id);

        /// <summary>
        /// Получение категории по нормализованному имени
        /// </summary>
        Task<ResponseData<Category>> GetCategoryByNormalizedNameAsync(string normalizedName);

        /// <summary>
        /// Проверка существования категории с указанным именем
        /// </summary>
        Task<ResponseData<bool>> CategoryExistsAsync(string name, int? excludeId = null);
    }
}