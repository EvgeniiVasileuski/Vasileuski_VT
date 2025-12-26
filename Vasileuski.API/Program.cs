using Microsoft.EntityFrameworkCore;
using Vasileuski.API.Data;
using Vasileuski.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Vasileuski Sports API",
        Version = "v1",
        Description = "API for managing sports teams and categories"
    });
});

// Настройка базы данных с вашим SQL Server Express
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());

// Регистрация сервисов
builder.Services.AddScoped<ITeamService, TeamService>();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy
            .WithOrigins(
                "http://localhost:5001",  // UI HTTP порт
                "https://localhost:7001", // UI HTTPS порт
                "http://localhost:5000",  // UI HTTP альтернативный порт
                "https://localhost:7000"  // UI HTTPS альтернативный порт
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

// Создаем область для применения миграций и заполнения данных
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Проверяем подключение к базе данных
        Console.WriteLine("Проверка подключения к базе данных...");
        Console.WriteLine($"Строка подключения: {connectionString}");

        // Применяем миграции
        Console.WriteLine("Применение миграций...");
        context.Database.Migrate();

        // Заполняем начальными данными
        Console.WriteLine("Заполнение начальными данными...");
        await DbInitializer.SeedData(app);

        Console.WriteLine("База данных готова!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при настройке базы данных: {ex.Message}");
        Console.WriteLine($"Детали: {ex.InnerException?.Message}");
    }
}

// Настройка конвейера HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vasileuski API v1");
        c.RoutePrefix = "api-docs";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();