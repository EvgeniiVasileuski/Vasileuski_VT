using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.UI.Data;

namespace Vasileuski.UI.Services
{
    public class DbTeamService : ITeamService
    {
        private readonly AdminDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DbTeamService(AdminDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<ResponseData<List<Team>>> GetTeamListAsync(string? category)
        {
            try
            {
                IQueryable<Team> query = _context.Teams.Include(t => t.Category);

                if (!string.IsNullOrEmpty(category))
                {
                    // Нормализуем входной параметр
                    var normalizedInput = category.Trim().ToLower();

                    Console.WriteLine($"Фильтрация: '{category}' -> '{normalizedInput}'");

                    query = query.Where(t => t.Category != null &&
                        t.Category.NormalizedName.Trim().ToLower() == normalizedInput);
                }

                var teams = await query
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                    .ToListAsync();

                return ResponseData<List<Team>>.Ok(teams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetTeamListAsync: {ex.Message}");
                return ResponseData<List<Team>>.Error($"Ошибка при получении команд: {ex.Message}");
            }
        }
        //public async Task<ResponseData<List<Team>>> GetTeamListAsync(string? category)
        //{
        //    try
        //    {
        //        IQueryable<Team> query = _context.Teams.Include(t => t.Category);

        //        if (!string.IsNullOrEmpty(category))
        //        {
        //            // Добавляем логирование
        //            Console.WriteLine($"Фильтрация по категории: '{category}'");

        //            // Сравнение с игнорированием регистра
        //            query = query.Where(t => t.Category != null &&
        //                t.Category.NormalizedName.ToLower() == category.ToLower());
        //        }

        //        var teams = await query
        //            .OrderByDescending(t => t.Points)
        //            .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
        //            .ToListAsync();

        //        // Логируем результат
        //        Console.WriteLine($"Найдено команд: {teams.Count}");
        //        foreach (var team in teams)
        //        {
        //            Console.WriteLine($"- {team.Name}, Категория: {team.Category?.Name}");
        //        }

        //        return ResponseData<List<Team>>.Ok(teams);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Ошибка: {ex.Message}");
        //        return ResponseData<List<Team>>.Error($"Ошибка при получении команд: {ex.Message}");
        //    }
        //}

        public async Task<ResponseData<Team>> GetTeamByIdAsync(int id)
        {
            try
            {
                var team = await _context.Teams
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return ResponseData<Team>.Error($"Команда с ID {id} не найдена");
                }

                return ResponseData<Team>.Ok(team);
            }
            catch (Exception ex)
            {
                return ResponseData<Team>.Error($"Ошибка при получении команды: {ex.Message}");
            }
        }

        public async Task UpdateTeamAsync(int id, Team team, IFormFile? formFile)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam != null)
            {
                // Сохраняем файл, если он загружен
                if (formFile != null && formFile.Length > 0)
                {
                    existingTeam.Image = await SaveImageAsync(formFile, "teams");
                }

                // Обновляем свойства
                existingTeam.Name = team.Name;
                existingTeam.Description = team.Description;
                existingTeam.Points = team.Points;
                existingTeam.CategoryId = team.CategoryId;
                existingTeam.Location = team.Location;
                existingTeam.FoundedYear = team.FoundedYear;
                existingTeam.HeadCoach = team.HeadCoach;
                existingTeam.Captain = team.Captain;
                existingTeam.Stadium = team.Stadium;
                existingTeam.Wins = team.Wins;
                existingTeam.Losses = team.Losses;
                existingTeam.Draws = team.Draws;
                existingTeam.GoalsFor = team.GoalsFor;
                existingTeam.GoalsAgainst = team.GoalsAgainst;
                existingTeam.Position = team.Position;
                existingTeam.Colors = team.Colors;
                existingTeam.UpdatedAt = DateTime.UtcNow;

                _context.Teams.Update(existingTeam);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(team.Name))
                    return ResponseData<Team>.Error("Название команды обязательно");

                // Сохраняем изображение
                if (formFile != null && formFile.Length > 0)
                {
                    team.Image = await SaveImageAsync(formFile, "teams");
                }

                // Устанавливаем временные метки
                team.CreatedAt = DateTime.UtcNow;
                team.UpdatedAt = DateTime.UtcNow;

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                return ResponseData<Team>.Ok(team, "Команда успешно создана");
            }
            catch (Exception ex)
            {
                return ResponseData<Team>.Error($"Ошибка при создании команды: {ex.Message}");
            }
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