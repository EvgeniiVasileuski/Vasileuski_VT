using Microsoft.EntityFrameworkCore;
using Vasileuski.API.Data;
using Vasileuski.API.DTOs;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;

namespace Vasileuski.API.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeamService(ApiDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<ResponseData<List<TeamDTO>>> GetTeamsAsync(string? category = null)
        {
            try
            {
                IQueryable<Team> query = _context.Teams.Include(t => t.Category);

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(t => t.Category != null &&
                        t.Category.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                var teams = await query
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                    .ToListAsync();

                var teamDTOs = teams.Select(t => MapToDTO(t)).ToList();
                return ResponseData<List<TeamDTO>>.Ok(teamDTOs);
            }
            catch (Exception ex)
            {
                return ResponseData<List<TeamDTO>>.Error($"Error getting teams: {ex.Message}");
            }
        }

        public async Task<ResponseData<TeamDTO>> GetTeamByIdAsync(int id)
        {
            try
            {
                var team = await _context.Teams
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return ResponseData<TeamDTO>.Error($"Team with ID {id} not found");
                }

                return ResponseData<TeamDTO>.Ok(MapToDTO(team));
            }
            catch (Exception ex)
            {
                return ResponseData<TeamDTO>.Error($"Error getting team: {ex.Message}");
            }
        }

        public async Task<ResponseData<TeamDTO>> CreateTeamAsync(CreateTeamDTO teamDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(teamDto.Name))
                {
                    return ResponseData<TeamDTO>.Error("Team name is required");
                }

                var team = new Team
                {
                    Name = teamDto.Name,
                    Description = teamDto.Description,
                    Points = teamDto.Points,
                    CategoryId = teamDto.CategoryId,
                    Location = teamDto.Location,
                    FoundedYear = teamDto.FoundedYear,
                    HeadCoach = teamDto.HeadCoach,
                    Captain = teamDto.Captain,
                    Stadium = teamDto.Stadium,
                    Wins = teamDto.Wins,
                    Losses = teamDto.Losses,
                    Draws = teamDto.Draws,
                    GoalsFor = teamDto.GoalsFor,
                    GoalsAgainst = teamDto.GoalsAgainst,
                    Position = teamDto.Position,
                    Colors = teamDto.Colors,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (teamDto.ImageFile != null && teamDto.ImageFile.Length > 0)
                {
                    team.Image = await SaveImageAsync(teamDto.ImageFile, "teams");
                }

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                // Загружаем связанные данные
                await _context.Entry(team)
                    .Reference(t => t.Category)
                    .LoadAsync();

                return ResponseData<TeamDTO>.Ok(MapToDTO(team), "Team created successfully");
            }
            catch (Exception ex)
            {
                return ResponseData<TeamDTO>.Error($"Error creating team: {ex.Message}");
            }
        }

        public async Task<ResponseData<TeamDTO>> UpdateTeamAsync(int id, UpdateTeamDTO teamDto)
        {
            try
            {
                var team = await _context.Teams
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return ResponseData<TeamDTO>.Error($"Team with ID {id} not found");
                }

                if (string.IsNullOrWhiteSpace(teamDto.Name))
                {
                    return ResponseData<TeamDTO>.Error("Team name is required");
                }

                // Обновление свойств
                team.Name = teamDto.Name;
                team.Description = teamDto.Description;
                team.Points = teamDto.Points;
                team.CategoryId = teamDto.CategoryId;
                team.Location = teamDto.Location;
                team.FoundedYear = teamDto.FoundedYear;
                team.HeadCoach = teamDto.HeadCoach;
                team.Captain = teamDto.Captain;
                team.Stadium = teamDto.Stadium;
                team.Wins = teamDto.Wins;
                team.Losses = teamDto.Losses;
                team.Draws = teamDto.Draws;
                team.GoalsFor = teamDto.GoalsFor;
                team.GoalsAgainst = teamDto.GoalsAgainst;
                team.Position = teamDto.Position;
                team.Colors = teamDto.Colors;
                team.UpdatedAt = DateTime.UtcNow;

                // Обновление изображения
                if (teamDto.ImageFile != null && teamDto.ImageFile.Length > 0)
                {
                    team.Image = await SaveImageAsync(teamDto.ImageFile, "teams");
                }
                else if (!string.IsNullOrEmpty(teamDto.ExistingImage))
                {
                    team.Image = teamDto.ExistingImage;
                }

                _context.Teams.Update(team);
                await _context.SaveChangesAsync();

                return ResponseData<TeamDTO>.Ok(MapToDTO(team), "Team updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseData<TeamDTO>.Error($"Error updating team: {ex.Message}");
            }
        }

        public async Task<ResponseData<bool>> DeleteTeamAsync(int id)
        {
            try
            {
                var team = await _context.Teams.FindAsync(id);
                if (team == null)
                {
                    return ResponseData<bool>.Error($"Team with ID {id} not found");
                }

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();

                return ResponseData<bool>.Ok(true, "Team deleted successfully");
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Error($"Error deleting team: {ex.Message}");
            }
        }

        public async Task<ResponseData<List<TeamDTO>>> SearchTeamsAsync(string searchTerm)
        {
            try
            {
                var teams = await _context.Teams
                    .Include(t => t.Category)
                    .Where(t => t.Name.Contains(searchTerm) ||
                                t.Description.Contains(searchTerm) ||
                                t.Location.Contains(searchTerm) ||
                                t.HeadCoach.Contains(searchTerm))
                    .OrderByDescending(t => t.Points)
                    .ToListAsync();

                var teamDTOs = teams.Select(t => MapToDTO(t)).ToList();
                return ResponseData<List<TeamDTO>>.Ok(teamDTOs);
            }
            catch (Exception ex)
            {
                return ResponseData<List<TeamDTO>>.Error($"Error searching teams: {ex.Message}");
            }
        }

        public async Task<ResponseData<object>> GetStatisticsAsync()
        {
            try
            {
                var teams = await _context.Teams.ToListAsync();

                var stats = new
                {
                    TotalTeams = teams.Count,
                    TotalPoints = teams.Sum(t => t.Points),
                    AveragePoints = teams.Any() ? Math.Round(teams.Average(t => t.Points), 2) : 0,
                    MaxPoints = teams.Any() ? teams.Max(t => t.Points) : 0,
                    MinPoints = teams.Any() ? teams.Min(t => t.Points) : 0,
                    TotalWins = teams.Sum(t => t.Wins),
                    TotalDraws = teams.Sum(t => t.Draws),
                    TotalLosses = teams.Sum(t => t.Losses),
                    TotalGoalsFor = teams.Sum(t => t.GoalsFor),
                    TotalGoalsAgainst = teams.Sum(t => t.GoalsAgainst)
                };

                return ResponseData<object>.Ok(stats);
            }
            catch (Exception ex)
            {
                return ResponseData<object>.Error($"Error getting statistics: {ex.Message}");
            }
        }

        private TeamDTO MapToDTO(Team team)
        {
            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Points = team.Points,
                CategoryId = team.CategoryId,
                Image = team.Image,
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
                CreatedAt = team.CreatedAt,
                UpdatedAt = team.UpdatedAt,
                Category = team.Category != null ? new CategoryDTO
                {
                    Id = team.Category.Id,
                    Name = team.Category.Name,
                    NormalizedName = team.Category.NormalizedName,
                    Description = team.Category.Description,
                    Image = team.Category.Image,
                    CreatedAt = team.Category.CreatedAt
                } : null
            };
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