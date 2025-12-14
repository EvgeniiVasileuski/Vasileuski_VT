using Microsoft.AspNetCore.Mvc;

namespace Vasileuski.UI.Components
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Получаем имя текущего контроллера и области
            var controller = Request.RouteValues["controller"]?.ToString()?.ToLower() ?? string.Empty;
            var area = Request.RouteValues["area"]?.ToString()?.ToLower() ?? string.Empty;
            var page = Request.RouteValues["page"]?.ToString()?.ToLower() ?? string.Empty;
            var action = Request.RouteValues["action"]?.ToString()?.ToLower() ?? string.Empty;

            // Передаем данные в представление
            ViewData["Controller"] = controller;
            ViewData["Area"] = area;
            ViewData["Page"] = page;
            ViewData["Action"] = action;

            // Также можно передать дополнительные данные, если нужно
            // Например, список пунктов меню из БД

            return View();
        }
    }
}