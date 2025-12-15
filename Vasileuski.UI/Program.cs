//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Vasileuski.UI.Data;
//using Vasileuski.UI.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddDbContext<AdminDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<ITeamService, MemoryTeamService>();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.Cookie.Name = ".Vasileuski.Session";
//});
//builder.Services.AddSingleton<IWebHostEnvironment>(provider =>
//    provider.GetRequiredService<IWebHostEnvironment>());

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();
//app.UseSession();
//app.UseResponseCaching();
//app.MapControllerRoute(
//    name: "admin",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Team}/{action=Index}/{id?}");
//app.MapRazorPages();


//app.Run();



//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Vasileuski.UI.Data;
//using Vasileuski.UI.Services;

//var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

//// Добавление сервисов в контейнер
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
//    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 6;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireLowercase = false;
//})
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//// Контроллеры с представлениями
//builder.Services.AddControllersWithViews();

//// Razor Pages (для Identity и Admin area)
//builder.Services.AddRazorPages();

//// Регистрация кастомных сервисов
//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<ITeamService, MemoryTeamService>();

//// IWebHostEnvironment для сервисов
//builder.Services.AddSingleton<IWebHostEnvironment>(provider =>
//    provider.GetRequiredService<IWebHostEnvironment>());

//// Session и кэширование
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.Cookie.Name = ".Vasileuski.Session";
//});

//builder.Services.AddResponseCaching();

//// Добавляем поддержку IHttpContextAccessor
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();

//// Настройка конвейера HTTP запросов
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//// Принудительно используем HTTP для тестирования
////app.UseHttpsRedirection();

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.UseSession();
//app.UseResponseCaching();

//// Map Razor Pages для Identity
//app.MapRazorPages();

//// Map контроллеров
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Team}/{action=Index}/{id?}");

//// Health check endpoint
//app.MapGet("/health", () => "Server is running");

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error: {ex.Message}");
//        Console.WriteLine($"StackTrace: {ex.StackTrace}");
//        throw;
//    }
//});

//Console.WriteLine("Application starting at: " + DateTime.Now);
//Console.WriteLine("Environment: " + app.Environment.EnvironmentName);

////app.Urls.Add("http://localhost:7001");
////app.Urls.Add("https://localhost:5001");

//app.Run();
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vasileuski.UI.Data;
using Vasileuski.UI.Services;

var builder = WebApplication.CreateBuilder(args);

//База данных
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Настройка контекста базы данных для приложения (AdminDbContext)
var adminConnectionString = builder.Configuration.GetConnectionString("AdminConnection")
    ?? throw new InvalidOperationException("Connection string 'AdminConnection' not found.");

builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(adminConnectionString));



// Сервисы
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//})
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Настройки пароля (опционально)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Настройки пользователя
    options.User.RequireUniqueEmail = true;

    // Настройки блокировки
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Ваши сервисы
//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<ITeamService, MemoryTeamService>();
builder.Services.AddScoped<ICategoryService, DbCategoryService>();
builder.Services.AddScoped<ITeamService, DbTeamService>();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
builder.Services.AddHttpContextAccessor();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Map Razor Pages для Identity
app.MapRazorPages();

// Map контроллеров
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Team}/{action=Index}/{id?}");


app.Run();