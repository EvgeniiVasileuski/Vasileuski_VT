using Microsoft.AspNetCore.Mvc;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.ViewComponents
{
    public class TeamCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Team team)
        {
            return View(team);
        }
    }
}