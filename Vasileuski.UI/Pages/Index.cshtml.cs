using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Repositories;

namespace Vasileuski.UI.Pages
{
    public class IndexModel : PageModel
    {
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public int? SelectedCategoryId { get; set; }

        // Статистика
        public int TotalPoints { get; set; }
        public int TotalWins { get; set; }
        public double AveragePoints { get; set; }
        public int TeamCount { get; set; }

        public IActionResult OnGet(int? categoryId)
        {
            // Получаем все категории из репозитория
            var allCategories = SampleDataRepository.GetCategories();

            // Убираем дубликаты по Id
            Categories = allCategories
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .OrderBy(c => c.Id)
                .ToList();

            SelectedCategoryId = categoryId;

            // Получаем все команды
            var allTeams = SampleDataRepository.GetTeams();

            // Фильтруем команды по выбранной категории
            if (categoryId.HasValue)
            {
                Teams = allTeams
                    .Where(t => t.CategoryId == categoryId.Value)
                    .OrderByDescending(t => t.Points)
                    .ThenBy(t => t.Position)
                    .ToList();

                // Находим название выбранной категории
                var selectedCategory = Categories.FirstOrDefault(c => c.Id == categoryId.Value);
                if (selectedCategory != null)
                {
                    ViewData["SelectedCategory"] = selectedCategory.Name;
                    ViewData["PageTitle"] = $"Спортивные команды - {selectedCategory.Name}";
                }
            }
            else
            {
                // Все команды
                Teams = allTeams
                    .OrderByDescending(t => t.Points)
                    .ThenBy(t => t.Position)
                    .ToList();
                ViewData["PageTitle"] = "Спортивные команды";
            }

            // Рассчитываем статистику
            CalculateStatistics(allTeams);

            return Page();
        }

        private void CalculateStatistics(List<Team> teams)
        {
            TeamCount = teams.Count;
            TotalPoints = teams.Sum(t => t.Points);
            TotalWins = teams.Sum(t => t.Wins);
            AveragePoints = teams.Any() ? Math.Round(teams.Average(t => t.Points), 1) : 0;
        }
    }
}