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
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}