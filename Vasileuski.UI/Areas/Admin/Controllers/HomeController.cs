using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vasileuski.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")] // Только для администраторов
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}