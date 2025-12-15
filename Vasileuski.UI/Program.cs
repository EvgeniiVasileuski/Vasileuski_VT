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

//// ���������� �������� � ���������
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

//// ����������� � ���������������
//builder.Services.AddControllersWithViews();

//// Razor Pages (��� Identity � Admin area)
//builder.Services.AddRazorPages();

//// ����������� ��������� ��������
//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<ITeamService, MemoryTeamService>();

//// IWebHostEnvironment ��� ��������
//builder.Services.AddSingleton<IWebHostEnvironment>(provider =>
//    provider.GetRequiredService<IWebHostEnvironment>());

//// Session � �����������
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.Cookie.Name = ".Vasileuski.Session";
//});

//builder.Services.AddResponseCaching();

//// ��������� ��������� IHttpContextAccessor
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();

//// ��������� ��������� HTTP ��������
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

//// ������������� ���������� HTTP ��� ������������
////app.UseHttpsRedirection();

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.UseSession();
//app.UseResponseCaching();

//// Map Razor Pages ��� Identity
//app.MapRazorPages();

//// Map ������������
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

//���� ������
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ��������� ��������� ���� ������ ��� ���������� (AdminDbContext)
var adminConnectionString = builder.Configuration.GetConnectionString("AdminConnection")
    ?? throw new InvalidOperationException("Connection string 'AdminConnection' not found.");

builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(adminConnectionString));



// �������
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

    // ��������� ������ (�����������)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // ��������� ������������
    options.User.RequireUniqueEmail = true;

    // ��������� ����������
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
    .AddRoles<IdentityRole>() // ��������� ��������� �����
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ���� �������
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("role", "admin"));
});


var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    // ����� ������ SeedData ����� �������� ����������
    await DbInit.SeedData(app);

    // �������������� �����
    await DbInit.SeedDefaultData(app.Services);
}
catch (Exception ex)
{
    Console.WriteLine($"������ ��� ������������� ���� ������: {ex.Message}");
}
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy =>
//        policy.RequireClaim("role", "admin"));

//    options.AddPolicy("AdminOrManager", policy =>
//        policy.RequireAssertion(context =>
//            context.User.HasClaim(c =>
//                (c.Type == "role" && (c.Value == "admin" || c.Value == "manager")) ||
//                (c.Type == "is_admin" && c.Value == "true")
//            )));
//});


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

// Map Razor Pages ��� Identity
app.MapRazorPages();

// Map ������������
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Team}/{action=Index}/{id?}");


app.Run();