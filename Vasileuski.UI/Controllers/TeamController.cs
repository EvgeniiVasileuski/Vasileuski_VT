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

        public TeamController(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Главная страница со списком всех команд
        /// </summary>
        public async Task<IActionResult> Index(string? category)
        {
            // Получаем команды через сервис
            var teamsResponse = await _teamService.GetTeamListAsync(category);

            if (!teamsResponse.Success)
                return NotFound(teamsResponse.ErrorMessage);

            // Получаем категории для фильтра
            var categoriesResponse = await _categoryService.GetCategoryListAsync();

            if (categoriesResponse.Success)
            {
                ViewBag.Categories = categoriesResponse.Data;

                // Передаем текущую категорию для выделения в фильтре
                if (!string.IsNullOrEmpty(category))
                {
                    var currentCategory = categoriesResponse.Data?
                        .FirstOrDefault(c => c.NormalizedName == category);
                    ViewBag.CurrentCategory = currentCategory;
                }
            }


            return View(teamsResponse.Data);
        }
        
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