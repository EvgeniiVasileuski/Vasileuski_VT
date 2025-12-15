using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vasileuski.UI.Data;

namespace Vasileuski.UI.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminMenuViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var isAdmin = false;

            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                isAdmin = claims.Any(c => c.Type == "role" && c.Value == "admin");
            }

            return View(isAdmin);
        }
    }
}