using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Models;
using Vasileuski.UI.Services;
using Vasileuski.UI.Extensions;

namespace Vasileuski.UI.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public TeamController(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category)
        {
            var teamResponse = await _teamService.GetTeamListAsync(category);

            if (!teamResponse.Success)
                return NotFound(teamResponse.ErrorMessage);

            var teams = teamResponse.Data?.ToList() ?? new List<Team>();

            // Получаем категории
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Success && categoriesResponse.Data != null)
            {
                var categoriesList = categoriesResponse.Data.ToList();
                ViewBag.Categories = categoriesList;

                if (!string.IsNullOrEmpty(category))
                {
                    var currentCategory = categoriesResponse.Data?
                .FirstOrDefault(c => c.NormalizedName == category);
                    ViewBag.CurrentCategory = category; // NormalizedName
                    ViewBag.CurrentCategoryName = currentCategory?.Name; // Display name
                }
            }

            // Получаем избранное из сессии
            var favorites = HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();
            ViewBag.FavoritesCount = favorites.Count;
            // Статистика для отображения
            ViewBag.TotalTeams = teams.Count;
            ViewBag.TotalPoints = teams.Sum(t => t.Points);
            ViewBag.AveragePoints = teams.Any() ? teams.Average(t => t.Points) : 0;

            return View(teams);
        }

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

        public async Task<IActionResult> Statistics()
        {
            var teamsResponse = await _teamService.GetTeamListAsync(null);
            if (!teamsResponse.Success)
                return NotFound(teamsResponse.ErrorMessage);

            var teams = teamsResponse.Data?.ToList() ?? new List<Team>();

            // Получаем категории
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            var categories = categoriesResponse.Data?.ToList() ?? new List<Category>();

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