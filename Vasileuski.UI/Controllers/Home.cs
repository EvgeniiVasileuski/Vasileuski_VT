using Microsoft.AspNetCore.Mvc;

namespace Vasileuski.UI.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
