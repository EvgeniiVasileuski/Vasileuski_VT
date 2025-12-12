using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Extensions;
using Vasileuski.UI.Services;

namespace Vasileuski.UI.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ITeamService _teamService;

        public FavoritesController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int teamId)
        {
            var teamResponse = await _teamService.GetTeamByIdAsync(teamId);

            if (!teamResponse.Success)
            {
                TempData["ErrorMessage"] = "Команда не найдена";
                return RedirectToAction("Index", "Team");
            }

            var favorites = HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();

            if (!favorites.Contains(teamId))
            {
                favorites.Add(teamId);
                HttpContext.Session.Set("Favorites", favorites);
                TempData["SuccessMessage"] = $"Команда '{teamResponse.Data.Name}' добавлена в избранное!";
            }
            else
            {
                TempData["InfoMessage"] = "Эта команда уже в избранном";
            }

            return RedirectToAction("Index", "Team");
        }
        [HttpPost]
        public IActionResult ClearFavorites()
        {
            HttpContext.Session.Remove("Favorites");
            TempData["SuccessMessage"] = "Все команды удалены из избранного";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RemoveFromFavorites(int teamId)
        {
            var favorites = HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();
            favorites.Remove(teamId);
            HttpContext.Session.Set("Favorites", favorites);

            TempData["SuccessMessage"] = "Команда удалена из избранного";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var favorites = HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();
            var teams = new List<Team>();

            foreach (var teamId in favorites)
            {
                var teamResponse = await _teamService.GetTeamByIdAsync(teamId);
                if (teamResponse.Success)
                {
                    teams.Add(teamResponse.Data);
                }
            }

            return View(teams);
        }
    }
}