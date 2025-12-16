//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using Vasileuski.UI.Data;

//namespace Vasileuski.UI.ViewComponents
//{
//    public class AdminMenuViewComponent : ViewComponent
//    {
//        private readonly UserManager<ApplicationUser> _userManager;

//        public AdminMenuViewComponent(UserManager<ApplicationUser> userManager)
//        {
//            _userManager = userManager;
//        }

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var user = await _userManager.GetUserAsync(HttpContext.User);
//            var isAdmin = false;

//            if (user != null)
//            {
//                var claims = await _userManager.GetClaimsAsync(user);
//                isAdmin = claims.Any(c => c.Type == "role" && c.Value == "admin");
//            }

//            return View(isAdmin);
//        }
//    }
//}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Vasileuski.UI.Data;

namespace Vasileuski.UI.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;

        public AdminMenuViewComponent(
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);

                if (string.IsNullOrEmpty(userId))
                    return View(false);

                var cacheKey = $"IsAdmin_{userId}";

                if (!_cache.TryGetValue(cacheKey, out bool isAdmin))
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    if (user == null)
                        return View(false);

                    isAdmin = await IsUserAdminAsync(user);
                    _cache.Set(cacheKey, isAdmin, TimeSpan.FromMinutes(5));
                }

                return View(isAdmin);
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем false
                // _logger.LogError(ex, "Ошибка в AdminMenuViewComponent");
                return View(false);
            }
        }

        private async Task<bool> IsUserAdminAsync(ApplicationUser user)
        {
            // Проверяем несколько вариантов определения администратора
            var adminRoles = new[] { "Admin", "Administrator", "SuperAdmin", "Moderator" };

            foreach (var role in adminRoles)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                    return true;
            }

            // Проверка через claims
            var claims = await _userManager.GetClaimsAsync(user);

            // Проверяем стандартные claim типы
            var isAdminByClaim = claims.Any(c =>
                (c.Type == ClaimTypes.Role ||
                 c.Type == "role" ||
                 c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role") &&
                adminRoles.Contains(c.Value));

            if (isAdminByClaim)
                return true;

            // Проверка через кастомные permissions
            var adminPermissions = new[] { "AccessAdminPanel", "ManageUsers", "ManageContent" };
            var isAdminByPermission = claims.Any(c =>
                c.Type == "Permission" && adminPermissions.Contains(c.Value));

            return isAdminByPermission;
        }
    }
}