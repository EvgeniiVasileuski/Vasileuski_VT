using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Models;
using Vasileuski.UI.Services;


namespace Vasileuski.UI.Controllers
{
    public class TeamController : Controller
    {
        
            private readonly ITeamService _teamService;
            private readonly ICategoryService _categoryService;
        private readonly ILogger<TeamController> _logger;

        public TeamController(
            ITeamService teamService,
            ICategoryService categoryService,
            ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _categoryService = categoryService;
            _logger = logger;
        }

        //public async Task<IActionResult> Index(string? category)
        //{
        //    var teamResponse = await _teamService.GetTeamListAsync(category);

        //    if (!teamResponse.Success)
        //        return NotFound(teamResponse.ErrorMessage);

        //    var teams = teamResponse.Data?.ToList() ?? new List<Team>();

        //    // Получаем категории
        //    var categoriesResponse = await _categoryService.GetCategoryListAsync();
        //    if (categoriesResponse.Success)
        //    {
        //        ViewBag.Categories = categoriesResponse.Data;

        //        if (!string.IsNullOrEmpty(category))
        //        {
        //            var currentCategory = categoriesResponse.Data?
        //                .FirstOrDefault(c => c.NormalizedName == category);
        //            ViewBag.CurrentCategory = currentCategory?.Name;
        //        }
        //    }

        //    // Рассчитываем дополнительные статистики
        //    if (teams.Any())
        //    {
        //        ViewBag.TotalPoints = teams.Sum(t => t.Points);
        //        ViewBag.TotalWins = teams.Sum(t => t.Wins);
        //        ViewBag.AveragePosition = teams.Average(t => t.Position);
        //        ViewBag.LeaderTeam = teams.OrderBy(t => t.Position).First();
        //    }

        //    return View(teams);
        //}
        public async Task<IActionResult> Index(string? category)
        {
            try
            {
                _logger.LogInformation("Запрос списка команд с категорией: {Category}", category ?? "все");

                var teamResponse = await _teamService.GetTeamListAsync(category);

                if (!teamResponse.Success)
                {
                    _logger.LogWarning("Ошибка получения команд: {Error}", teamResponse.ErrorMessage);
                    TempData["ErrorMessage"] = teamResponse.ErrorMessage;
                    return View(new List<Team>());
                }

                var teams = teamResponse.Data?.ToList() ?? new List<Team>();
                _logger.LogInformation("Получено {Count} команд", teams.Count);

                // Получаем категории для фильтра
                var categoriesResponse = await _categoryService.GetCategoryListAsync();
                if (categoriesResponse.Success)
                {
                    ViewBag.Categories = categoriesResponse.Data;

                    if (!string.IsNullOrEmpty(category))
                    {
                        var currentCategory = categoriesResponse.Data?
                            .FirstOrDefault(c => c.NormalizedName != null &&
                                c.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase));

                        ViewBag.CurrentCategory = currentCategory?.Name;
                    }
                }

                // Рассчитываем статистики
                if (teams.Any())
                {
                    ViewBag.TotalPoints = teams.Sum(t => t.Points);
                    ViewBag.TotalWins = teams.Sum(t => t.Wins);
                    ViewBag.AveragePosition = teams.Average(t => t.Position);
                    ViewBag.LeaderTeam = teams.OrderBy(t => t.Position).First();
                }

                return View(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе Index");
                TempData["ErrorMessage"] = "Ошибка загрузки данных";
                return View(new List<Team>());
            }
        }
        //public async Task<IActionResult> Index(string? category)
        //{
        //    // Логируем полученный параметр
        //    Console.WriteLine($"=== TeamController.Index called with category: '{category}' ===");

        //    var teamResponse = await _teamService.GetTeamListAsync(category);

        //    if (!teamResponse.Success)
        //    {
        //        Console.WriteLine($"Ошибка: {teamResponse.ErrorMessage}");
        //        return NotFound(teamResponse.ErrorMessage);
        //    }

        //    var teams = teamResponse.Data?.ToList() ?? new List<Team>();

        //    Console.WriteLine($"Получено команд: {teams.Count}");

        //    // Получаем категории
        //    var categoriesResponse = await _categoryService.GetCategoryListAsync();
        //    if (categoriesResponse.Success)
        //    {
        //        ViewBag.Categories = categoriesResponse.Data;

        //        if (!string.IsNullOrEmpty(category))
        //        {
        //            // Ищем категорию по NormalizedName (без учета регистра)
        //            var currentCategory = categoriesResponse.Data?
        //                .FirstOrDefault(c => c.NormalizedName != null &&
        //                    c.NormalizedName.ToLower() == category.ToLower());

        //            Console.WriteLine($"Найдена текущая категория: {currentCategory?.Name}");
        //            ViewBag.CurrentCategory = currentCategory?.Name;

        //            // Также сохраняем NormalizedName для фильтра
        //            ViewBag.CurrentCategoryNormalizedName = category;
        //        }
        //        else
        //        {
        //            ViewBag.CurrentCategoryNormalizedName = null;
        //        }
        //    }

        //    // Рассчитываем дополнительные статистики
        //    if (teams.Any())
        //    {
        //        ViewBag.TotalPoints = teams.Sum(t => t.Points);
        //        ViewBag.TotalWins = teams.Sum(t => t.Wins);
        //        ViewBag.AveragePosition = teams.Average(t => t.Position);
        //        ViewBag.LeaderTeam = teams.OrderBy(t => t.Position).First();
        //    }

        //    return View(teams);
        //}

        //public async Task<IActionResult> Index(string? category)
        //{
        //    var teamResponse = await _teamService.GetTeamListAsync(category);

        //    if (!teamResponse.Success)
        //        return NotFound(teamResponse.ErrorMessage);

        //    return View(teamResponse.Data);
        //}

        /// <summary>
        /// Детальная информация о команде
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var response = await _teamService.GetTeamByIdAsync(id);

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }

        /// <summary>
        /// Статистика команд
        /// </summary>
        public async Task<IActionResult> Statistics()
        {
            var teamsResponse = await _teamService.GetTeamListAsync(null);
            var categoriesResponse = await _categoryService.GetCategoryListAsync();

            if (!teamsResponse.Success || !categoriesResponse.Success)
                return NotFound(teamsResponse.ErrorMessage ?? categoriesResponse.ErrorMessage);

            var teams = teamsResponse.Data ?? new List<Team>();
            var categories = categoriesResponse.Data ?? new List<Category>();

            // Находим команду с максимальным количеством очков
            var topTeam = teams.OrderByDescending(t => t.Points).FirstOrDefault();

            // Создаем ViewModel со статистикой
            var model = new StatisticsViewModel
            {
                TotalTeams = teams.Count,
                TotalPoints = teams.Sum(t => t.Points),
                AveragePoints = teams.Any() ? teams.Average(t => t.Points) : 0,
                MaxPoints = teams.Any() ? teams.Max(t => t.Points) : 0,
                MinPoints = teams.Any() ? teams.Min(t => t.Points) : 0,
                TopTeam = topTeam,
                Categories = categories,
                Teams = teams
            };

            return View(model);
        }
    }
}