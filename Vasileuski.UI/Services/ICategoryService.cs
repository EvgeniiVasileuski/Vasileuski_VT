// Vasileuski.UI/Services/ICategoryService.cs
using Microsoft.AspNetCore.Http;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.UI.Models;

namespace Vasileuski.UI.Services
{
    public interface ICategoryService
    {
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        Task<ResponseData<Category>> CreateCategoryAsync(Category category, IFormFile? formFile);
        Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category, IFormFile? formFile);
        Task<ResponseData<bool>> DeleteCategoryAsync(int id);
    }
}