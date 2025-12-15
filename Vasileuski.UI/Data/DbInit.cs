using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Vasileuski.UI.Data
{
    public static class DbInit
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Создаем роль администратора, если её нет
                var adminRoleName = "admin";
                if (!await roleManager.RoleExistsAsync(adminRoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));
                    Console.WriteLine("Роль 'admin' создана");
                }

                // Создаем пользователя администратора, если его нет
                var adminEmail = "admin@vasileuski.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Администратор Системы",
                        RegistrationDate = DateTime.UtcNow,
                        IsVerified = true,
                        Points = 1000,
                        Level = 10
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (result.Succeeded)
                    {
                        Console.WriteLine("Пользователь администратора создан");

                        // Добавляем пользователя в роль администратора
                        await userManager.AddToRoleAsync(adminUser, adminRoleName);

                        // Добавляем claim "role" со значением "admin"
                        await userManager.AddClaimAsync(adminUser, new Claim("role", "admin"));

                        // Добавляем дополнительные claims для администратора
                        await userManager.AddClaimAsync(adminUser, new Claim("permission", "all"));
                        await userManager.AddClaimAsync(adminUser, new Claim("is_admin", "true"));

                        Console.WriteLine($"Администратор создан: {adminEmail} / Admin123!");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"Ошибка создания администратора: {errors}");
                    }
                }
                else
                {
                    // Проверяем, есть ли у пользователя claim "role" со значением "admin"
                    var existingClaims = await userManager.GetClaimsAsync(adminUser);
                    var roleClaim = existingClaims.FirstOrDefault(c => c.Type == "role" && c.Value == "admin");

                    if (roleClaim == null)
                    {
                        await userManager.AddClaimAsync(adminUser, new Claim("role", "admin"));
                        Console.WriteLine("Claim 'role' добавлен существующему пользователю");
                    }

                    // Проверяем, есть ли у пользователя роль admin
                    if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                    {
                        await userManager.AddToRoleAsync(adminUser, adminRoleName);
                        Console.WriteLine("Пользователь добавлен в роль 'admin'");
                    }
                }

                // Создаем тестового пользователя (опционально)
                var testEmail = "user@vasileuski.com";
                var testUser = await userManager.FindByEmailAsync(testEmail);

                if (testUser == null)
                {
                    testUser = new ApplicationUser
                    {
                        UserName = testEmail,
                        Email = testEmail,
                        EmailConfirmed = true,
                        FullName = "Тестовый Пользователь",
                        RegistrationDate = DateTime.UtcNow,
                        IsVerified = false,
                        Points = 100,
                        Level = 1
                    };

                    var result = await userManager.CreateAsync(testUser, "User123!");

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Тестовый пользователь создан: {testEmail} / User123!");
                    }
                }

                Console.WriteLine("Инициализация базы данных завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            }
        }

        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Создаем стандартные роли
                string[] roleNames = { "admin", "manager", "user" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                        Console.WriteLine($"Роль '{roleName}' создана");
                    }
                }

                // Создаем администратора, если он не существует
                var adminEmail = "admin@vasileuski.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Системный Администратор",
                        RegistrationDate = DateTime.UtcNow,
                        LastLoginDate = DateTime.UtcNow,
                        Country = "Россия",
                        City = "Москва",
                        IsOnline = true,
                        IsVerified = true,
                        Points = 10000,
                        Level = 100,
                        PreferredLanguage = "ru-RU",
                        Theme = "dark"
                    };

                    var createResult = await userManager.CreateAsync(adminUser, "Admin@12345");

                    if (createResult.Succeeded)
                    {
                        // Добавляем в роль администратора
                        await userManager.AddToRoleAsync(adminUser, "admin");

                        // Добавляем claim "role" со значением "admin"
                        await userManager.AddClaimAsync(adminUser, new Claim("role", "admin"));

                        // Добавляем дополнительные claims
                        var claims = new[]
                        {
                            new Claim("is_admin", "true"),
                            new Claim("permission_level", "999"),
                            new Claim("full_access", "true")
                        };

                        foreach (var claim in claims)
                        {
                            await userManager.AddClaimAsync(adminUser, claim);
                        }

                        Console.WriteLine("Администратор создан успешно");
                    }
                    else
                    {
                        var errors = string.Join("\n", createResult.Errors.Select(e => e.Description));
                        Console.WriteLine($"Ошибка создания администратора: {errors}");
                    }
                }
                else
                {
                    // Обновляем claims для существующего администратора
                    var claims = await userManager.GetClaimsAsync(adminUser);

                    if (!claims.Any(c => c.Type == "role" && c.Value == "admin"))
                    {
                        await userManager.AddClaimAsync(adminUser, new Claim("role", "admin"));
                        Console.WriteLine("Claim 'role' добавлен существующему администратору");
                    }

                    if (!await userManager.IsInRoleAsync(adminUser, "admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "admin");
                        Console.WriteLine("Существующий администратор добавлен в роль 'admin'");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации данных: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}