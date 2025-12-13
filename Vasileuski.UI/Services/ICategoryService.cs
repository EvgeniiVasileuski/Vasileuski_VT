using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;

using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Services
{
    public interface ICategoryService
    {
        // Используем ServiceResponse с IEnumerable<Category>
        Task<ServiceResponse<IEnumerable<Category>>> GetCategoryListAsync();
        Task<ServiceResponse<Category>> GetCategoryByIdAsync(int id);
    }
}