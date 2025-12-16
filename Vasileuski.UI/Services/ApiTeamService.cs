// Vasileuski.UI/Services/ApiTeamService.cs
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.UI.Models;

namespace Vasileuski.UI.Services
{
    public class ApiTeamService : ITeamService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiTeamService> _logger;

        public ApiTeamService(HttpClient httpClient, ILogger<ApiTeamService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ResponseData<List<Team>>> GetTeamListAsync(string? category)
        {
            try
            {
                _logger.LogInformation("Запрос команд с категорией: {Category}", category ?? "все");

                string url = "api/teams";
                if (!string.IsNullOrEmpty(category))
                {
                    url += $"?category={Uri.EscapeDataString(category)}";
                }

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Десериализуем ответ API
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<TeamDto>>>();

                    if (apiResponse != null && apiResponse.Success)
                    {
                        // Преобразуем TeamDto в Team (можно использовать AutoMapper)
                        var teams = apiResponse.Data?.Select(MapDtoToTeam).ToList() ?? new List<Team>();
                        return ResponseData<List<Team>>.Ok(teams);
                    }
                    else
                    {
                        return ResponseData<List<Team>>.Error(apiResponse?.Error ?? "Ошибка API");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка API: {StatusCode} - {Error}", response.StatusCode, errorContent);
                    return ResponseData<List<Team>>.Error($"Ошибка API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении команд через API");
                return ResponseData<List<Team>>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<Team>> GetTeamByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/teams/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();

                    if (apiResponse != null && apiResponse.Success && apiResponse.Data != null)
                    {
                        return ResponseData<Team>.Ok(MapDtoToTeam(apiResponse.Data));
                    }
                    else
                    {
                        return ResponseData<Team>.Error(apiResponse?.Error ?? "Команда не найдена");
                    }
                }
                else
                {
                    return ResponseData<Team>.Error($"Команда с ID {id} не найдена");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении команды по ID");
                return ResponseData<Team>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile)
        {
            try
            {
                // Создаем DTO для отправки
                var createDto = new CreateTeamDto
                {
                    Name = team.Name,
                    Description = team.Description,
                    Points = team.Points,
                    CategoryId = team.CategoryId,
                    Location = team.Location,
                    FoundedYear = team.FoundedYear,
                    HeadCoach = team.HeadCoach,
                    Captain = team.Captain,
                    Stadium = team.Stadium,
                    Wins = team.Wins,
                    Losses = team.Losses,
                    Draws = team.Draws,
                    GoalsFor = team.GoalsFor,
                    GoalsAgainst = team.GoalsAgainst,
                    Position = team.Position,
                    Colors = team.Colors
                };

                // Если есть файл, отправляем как FormData
                if (formFile != null)
                {
                    using var formData = new MultipartFormDataContent();

                    // Добавляем все поля из CreateTeamDto
                    formData.Add(new StringContent(createDto.Name ?? ""), "Name");

                    if (!string.IsNullOrEmpty(createDto.Description))
                    {
                        formData.Add(new StringContent(createDto.Description), "Description");
                    }

                    formData.Add(new StringContent(createDto.Points.ToString()), "Points");

                    if (createDto.CategoryId.HasValue)
                    {
                        formData.Add(new StringContent(createDto.CategoryId.Value.ToString()), "CategoryId");
                    }

                    if (!string.IsNullOrEmpty(createDto.Location))
                    {
                        formData.Add(new StringContent(createDto.Location), "Location");
                    }

                    if (createDto.FoundedYear.HasValue)
                    {
                        formData.Add(new StringContent(createDto.FoundedYear.Value.ToString()), "FoundedYear");
                    }

                    if (!string.IsNullOrEmpty(createDto.HeadCoach))
                    {
                        formData.Add(new StringContent(createDto.HeadCoach), "HeadCoach");
                    }

                    if (!string.IsNullOrEmpty(createDto.Captain))
                    {
                        formData.Add(new StringContent(createDto.Captain), "Captain");
                    }

                    if (!string.IsNullOrEmpty(createDto.Stadium))
                    {
                        formData.Add(new StringContent(createDto.Stadium), "Stadium");
                    }

                    formData.Add(new StringContent(createDto.Wins.ToString()), "Wins");
                    formData.Add(new StringContent(createDto.Losses.ToString()), "Losses");
                    formData.Add(new StringContent(createDto.Draws.ToString()), "Draws");
                    formData.Add(new StringContent(createDto.GoalsFor.ToString()), "GoalsFor");
                    formData.Add(new StringContent(createDto.GoalsAgainst.ToString()), "GoalsAgainst");
                    formData.Add(new StringContent(createDto.Position.ToString()), "Position");

                    if (!string.IsNullOrEmpty(createDto.Colors))
                    {
                        formData.Add(new StringContent(createDto.Colors), "Colors");
                    }

                    // Добавляем файл изображения
                    formData.Add(new StreamContent(formFile.OpenReadStream()), "ImageFile", formFile.FileName);

                    var response = await _httpClient.PostAsync("api/teams", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();
                        if (apiResponse != null && apiResponse.Success && apiResponse.Data != null)
                        {
                            return ResponseData<Team>.Ok(MapDtoToTeam(apiResponse.Data), "Команда успешно создана");
                        }
                    }
                }
                else
                {
                    // Без файла отправляем как JSON
                    var response = await _httpClient.PostAsJsonAsync("api/teams", createDto);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();
                        if (apiResponse != null && apiResponse.Success && apiResponse.Data != null)
                        {
                            return ResponseData<Team>.Ok(MapDtoToTeam(apiResponse.Data), "Команда успешно создана");
                        }
                    }
                }

                return ResponseData<Team>.Error("Ошибка при создании команды");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании команды через API");
                return ResponseData<Team>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task UpdateTeamAsync(int id, Team team, IFormFile? formFile)
        {
            try
            {
                var updateDto = new UpdateTeamDto
                {
                    Name = team.Name,
                    Description = team.Description,
                    Points = team.Points,
                    CategoryId = team.CategoryId,
                    Location = team.Location,
                    FoundedYear = team.FoundedYear,
                    HeadCoach = team.HeadCoach,
                    Captain = team.Captain,
                    Stadium = team.Stadium,
                    Wins = team.Wins,
                    Losses = team.Losses,
                    Draws = team.Draws,
                    GoalsFor = team.GoalsFor,
                    GoalsAgainst = team.GoalsAgainst,
                    Position = team.Position,
                    Colors = team.Colors,
                    ExistingImage = team.Image
                };

                if (formFile != null)
                {
                    using var formData = new MultipartFormDataContent();

                    // Добавляем все поля из UpdateTeamDto
                    formData.Add(new StringContent(updateDto.Name ?? ""), "Name");

                    if (!string.IsNullOrEmpty(updateDto.Description))
                    {
                        formData.Add(new StringContent(updateDto.Description), "Description");
                    }

                    formData.Add(new StringContent(updateDto.Points.ToString()), "Points");

                    if (updateDto.CategoryId.HasValue)
                    {
                        formData.Add(new StringContent(updateDto.CategoryId.Value.ToString()), "CategoryId");
                    }

                    if (!string.IsNullOrEmpty(updateDto.Location))
                    {
                        formData.Add(new StringContent(updateDto.Location), "Location");
                    }

                    if (updateDto.FoundedYear.HasValue)
                    {
                        formData.Add(new StringContent(updateDto.FoundedYear.Value.ToString()), "FoundedYear");
                    }

                    if (!string.IsNullOrEmpty(updateDto.HeadCoach))
                    {
                        formData.Add(new StringContent(updateDto.HeadCoach), "HeadCoach");
                    }

                    if (!string.IsNullOrEmpty(updateDto.Captain))
                    {
                        formData.Add(new StringContent(updateDto.Captain), "Captain");
                    }

                    if (!string.IsNullOrEmpty(updateDto.Stadium))
                    {
                        formData.Add(new StringContent(updateDto.Stadium), "Stadium");
                    }

                    formData.Add(new StringContent(updateDto.Wins.ToString()), "Wins");
                    formData.Add(new StringContent(updateDto.Losses.ToString()), "Losses");
                    formData.Add(new StringContent(updateDto.Draws.ToString()), "Draws");
                    formData.Add(new StringContent(updateDto.GoalsFor.ToString()), "GoalsFor");
                    formData.Add(new StringContent(updateDto.GoalsAgainst.ToString()), "GoalsAgainst");
                    formData.Add(new StringContent(updateDto.Position.ToString()), "Position");

                    if (!string.IsNullOrEmpty(updateDto.Colors))
                    {
                        formData.Add(new StringContent(updateDto.Colors), "Colors");
                    }

                    if (!string.IsNullOrEmpty(updateDto.ExistingImage))
                    {
                        formData.Add(new StringContent(updateDto.ExistingImage), "ExistingImage");
                    }

                    // Добавляем файл изображения
                    formData.Add(new StreamContent(formFile.OpenReadStream()), "ImageFile", formFile.FileName);

                    await _httpClient.PutAsync($"api/teams/{id}", formData);
                }
                else
                {
                    // Без файла отправляем как JSON
                    await _httpClient.PutAsJsonAsync($"api/teams/{id}", updateDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении команды через API");
                throw;
            }
        }

        public async Task DeleteTeamAsync(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"api/teams/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении команды через API");
                throw;
            }
        }

        private Team MapDtoToTeam(TeamDto dto)
        {
            return new Team
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Points = dto.Points,
                CategoryId = dto.CategoryId,
                Image = dto.Image,
                Location = dto.Location,
                FoundedYear = dto.FoundedYear,
                HeadCoach = dto.HeadCoach,
                Captain = dto.Captain,
                Stadium = dto.Stadium,
                Wins = dto.Wins,
                Losses = dto.Losses,
                Draws = dto.Draws,
                GoalsFor = dto.GoalsFor,
                GoalsAgainst = dto.GoalsAgainst,
                Position = dto.Position,
                Colors = dto.Colors,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Category = dto.Category != null ? new Category
                {
                    Id = dto.Category.Id,
                    Name = dto.Category.Name,
                    NormalizedName = dto.Category.NormalizedName,
                    Description = dto.Category.Description,
                    Image = dto.Category.Image,
                    CreatedAt = dto.Category.CreatedAt
                } : null
            };
        }
    }

    // DTO классы для API
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; }
        public int? CategoryId { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Position { get; set; }
        public string? Colors { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CategoryDto? Category { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TeamDto>? Teams { get; set; }
    }

    public class CreateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; } = 0;
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int GoalsFor { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int Position { get; set; } = 0;
        public string? Colors { get; set; }
    }

    public class UpdateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; }
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Position { get; set; }
        public string? Colors { get; set; }
        public string? ExistingImage { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public string? Message { get; set; }
    }
}