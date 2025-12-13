using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;

using Vasileuski.UI.Extensions;

namespace Vasileuski.UI.ViewComponents
{
    public class TeamCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Team team)
        {
            var favorites = HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();
            ViewBag.IsFavorite = favorites.Contains(team.Id);
            return View(team);
        }
    }
}