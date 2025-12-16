// Vasileuski.UI/Services/ApiCategoryService.cs
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.UI.Models;

namespace Vasileuski.UI.Services
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Существующие методы
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                _logger.LogInformation("Запрос категорий через API");

                var response = await _httpClient.GetAsync("api/categories");

                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                    var mappedCategories = categories?.Select(MapDtoToCategory).ToList() ?? new List<Category>();
                    return ResponseData<List<Category>>.Ok(mappedCategories);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка API: {StatusCode} - {Error}", response.StatusCode, errorContent);
                    return ResponseData<List<Category>>.Error($"Ошибка API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категорий через API");
                return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
                    if (category != null)
                    {
                        return ResponseData<Category>.Ok(MapDtoToCategory(category));
                    }
                }

                return ResponseData<Category>.Error($"Категория с ID {id} не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категории по ID");
                return ResponseData<Category>.Error($"Ошибка: {ex.Message}");
            }
        }

        // Новые методы для CRUD операций

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category, IFormFile? formFile)
        {
            try
            {
                var createDto = new CreateCategoryDto
                {
                    Name = category.Name,
                    Description = category.Description
                };

                if (formFile != null)
                {
                    using var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(createDto.Name ?? ""), "Name");

                    if (!string.IsNullOrEmpty(createDto.Description))
                    {
                        formData.Add(new StringContent(createDto.Description), "Description");
                    }

                    formData.Add(new StreamContent(formFile.OpenReadStream()), "ImageFile", formFile.FileName);

                    var response = await _httpClient.PostAsync("api/categories", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var createdCategory = await response.Content.ReadFromJsonAsync<CategoryDto>();
                        if (createdCategory != null)
                        {
                            return ResponseData<Category>.Ok(MapDtoToCategory(createdCategory), "Категория успешно создана");
                        }
                    }
                }
                else
                {
                    var response = await _httpClient.PostAsJsonAsync("api/categories", createDto);

                    if (response.IsSuccessStatusCode)
                    {
                        var createdCategory = await response.Content.ReadFromJsonAsync<CategoryDto>();
                        if (createdCategory != null)
                        {
                            return ResponseData<Category>.Ok(MapDtoToCategory(createdCategory), "Категория успешно создана");
                        }
                    }
                }

                return ResponseData<Category>.Error("Ошибка при создании категории");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории через API");
                return ResponseData<Category>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category, IFormFile? formFile)
        {
            try
            {
                var updateDto = new UpdateCategoryDto
                {
                    Name = category.Name,
                    Description = category.Description,
                    ExistingImage = category.Image
                };

                if (formFile != null)
                {
                    using var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(updateDto.Name ?? ""), "Name");

                    if (!string.IsNullOrEmpty(updateDto.Description))
                    {
                        formData.Add(new StringContent(updateDto.Description), "Description");
                    }

                    if (!string.IsNullOrEmpty(updateDto.ExistingImage))
                    {
                        formData.Add(new StringContent(updateDto.ExistingImage), "ExistingImage");
                    }

                    formData.Add(new StreamContent(formFile.OpenReadStream()), "ImageFile", formFile.FileName);

                    var response = await _httpClient.PutAsync($"api/categories/{id}", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var updatedCategory = await response.Content.ReadFromJsonAsync<CategoryDto>();
                        if (updatedCategory != null)
                        {
                            return ResponseData<Category>.Ok(MapDtoToCategory(updatedCategory), "Категория успешно обновлена");
                        }
                    }
                }
                else
                {
                    var response = await _httpClient.PutAsJsonAsync($"api/categories/{id}", updateDto);

                    if (response.IsSuccessStatusCode)
                    {
                        var updatedCategory = await response.Content.ReadFromJsonAsync<CategoryDto>();
                        if (updatedCategory != null)
                        {
                            return ResponseData<Category>.Ok(MapDtoToCategory(updatedCategory), "Категория успешно обновлена");
                        }
                    }
                }

                return ResponseData<Category>.Error("Ошибка при обновлении категории");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении категории через API");
                return ResponseData<Category>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return ResponseData<bool>.Ok(true, "Категория успешно удалена");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка удаления категории: {StatusCode} - {Error}", response.StatusCode, errorContent);
                    return ResponseData<bool>.Error($"Ошибка при удалении категории: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении категории через API");
                return ResponseData<bool>.Error($"Ошибка: {ex.Message}");
            }
        }

        private Category MapDtoToCategory(CategoryDto dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                NormalizedName = dto.NormalizedName,
                Description = dto.Description,
                Image = dto.Image,
                CreatedAt = dto.CreatedAt
            };
        }
    }

    // DTO классы для API
    

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImage { get; set; }
    }
}