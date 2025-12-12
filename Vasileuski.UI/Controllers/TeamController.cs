using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Repositories;
using Vasileuski.UI.Models;
using System.Linq;

namespace Vasileuski.UI.Controllers
{
    public class TeamController : Controller
    {
        /// <summary>
        /// Главная страница со списком всех команд
        /// </summary>
        public IActionResult Index()
        {
            // Получаем все команды и сортируем по очкам (по убыванию)
            var teams = SampleDataRepository.GetTeams()
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ToList();

            // Получаем все категории для фильтра
            var categories = SampleDataRepository.GetCategories();

            // Передаем в ViewBag для использования в представлении
            ViewBag.Categories = categories;

            return View(teams);
        }

        /// <summary>
        /// Фильтрация команд по категории (по NormalizedName)
        /// </summary>
        public IActionResult ByCategory(string categoryName)
        {
            // Находим категорию по нормализованному имени
            var category = SampleDataRepository.GetCategories()
                .FirstOrDefault(c => c.NormalizedName == categoryName);

            if (category == null)
            {
                // Если категория не найдена, возвращаем на главную
                return RedirectToAction("Index");
            }

            // Получаем команды только этой категории и сортируем
            var teams = SampleDataRepository.GetTeams()
                .Where(t => t.CategoryId == category.Id)
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ToList();

            var categories = SampleDataRepository.GetCategories();

            // Передаем информацию о текущей категории
            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = category;

            return View("Index", teams);
        }

        /// <summary>
        /// Статистика команд (с использованием ViewModel)
        /// </summary>
        public IActionResult Statistics()
        {
            var teams = SampleDataRepository.GetTeams();
            var categories = SampleDataRepository.GetCategories();

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
                Categories = categories.ToList(),
                Teams = teams.ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Детальная информация о конкретной команде
        /// </summary>
        public IActionResult Details(int id)
        {
            var team = SampleDataRepository.GetTeams()
                .FirstOrDefault(t => t.Id == id);

            if (team == null)
            {
                // Если команда не найдена
                TempData["ErrorMessage"] = $"Команда с ID {id} не найдена";
                return RedirectToAction("Index");
            }

            // Получаем категорию команды, если она есть
            if (team.CategoryId.HasValue)
            {
                var category = SampleDataRepository.GetCategories()
                    .FirstOrDefault(c => c.Id == team.CategoryId.Value);
                ViewBag.Category = category;
            }

            return View(team);
        }

        /// <summary>
        /// Тестовый метод для проверки работы API/JSON
        /// </summary>
        [HttpGet]
        public IActionResult ApiTest()
        {
            var teams = SampleDataRepository.GetTeams()
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Points,
                    t.Location,
                    Category = t.CategoryId.HasValue
                        ? SampleDataRepository.GetCategories()
                            .FirstOrDefault(c => c.Id == t.CategoryId.Value)?.Name
                        : null
                })
                .ToList();

            return Json(teams);
        }

        /// <summary>
        /// Пример математических операций - сумма очков всех команд
        /// </summary>
        [HttpGet]
        public IActionResult CalculateTotalPoints()
        {
            var teams = SampleDataRepository.GetTeams();
            var totalPoints = teams.Sum(t => t.Points);

            ViewBag.TotalPoints = totalPoints;
            ViewBag.TeamCount = teams.Count;
            ViewBag.AveragePoints = teams.Any()
                ? Math.Round(teams.Average(t => t.Points), 2)
                : 0;

            return View();
        }

        /// <summary>
        /// Постраничный вывод команд
        /// </summary>
        [HttpGet]
        public IActionResult Paged(int page = 1, int pageSize = 3)
        {
            var allTeams = SampleDataRepository.GetTeams()
                .OrderByDescending(t => t.Points)
                .ToList();

            var totalCount = allTeams.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Проверка границ страницы
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedTeams = allTeams
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = page < totalPages;

            return View(pagedTeams);
        }

        /// <summary>
        /// Фильтр команд по количеству очков
        /// </summary>
        [HttpGet]
        public IActionResult FilterByPoints(int minPoints = 0, int maxPoints = 100)
        {
            var teams = SampleDataRepository.GetTeams()
                .Where(t => t.Points >= minPoints && t.Points <= maxPoints)
                .OrderByDescending(t => t.Points)
                .ToList();

            var categories = SampleDataRepository.GetCategories();

            ViewBag.Categories = categories;
            ViewBag.MinPoints = minPoints;
            ViewBag.MaxPoints = maxPoints;
            ViewBag.FilteredCount = teams.Count;

            return View("Index", teams);
        }

        /// <summary>
        /// Таблица лидеров (топ-N команд)
        /// </summary>
        [HttpGet]
        public IActionResult Leaderboard(int top = 5)
        {
            var teams = SampleDataRepository.GetTeams()
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .Take(top)
                .ToList();

            ViewBag.TopCount = top;

            return View(teams);
        }

        /// <summary>
        /// Тестовый метод для проверки связи Team-Category
        /// </summary>
        [HttpGet]
        public IActionResult TestRelationship()
        {
            var teams = SampleDataRepository.GetTeams();
            var categories = SampleDataRepository.GetCategories();

            var teamCategoryInfo = teams.Select(t => new
            {
                TeamName = t.Name,
                TeamPoints = t.Points,
                CategoryName = t.CategoryId.HasValue
                    ? categories.FirstOrDefault(c => c.Id == t.CategoryId.Value)?.Name
                    : "Без категории",
                CategoryNormalizedName = t.CategoryId.HasValue
                    ? categories.FirstOrDefault(c => c.Id == t.CategoryId.Value)?.NormalizedName
                    : null
            }).ToList();

            return View(teamCategoryInfo);
        }
    }
}