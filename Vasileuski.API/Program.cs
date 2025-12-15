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

// Настройка базы данных
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(connectionString));

// Регистрация сервисов
builder.Services.AddScoped<ITeamService, TeamService>();

// Настройка CORS (если нужно обращаться из UI проекта)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        builder => builder
            .WithOrigins("http://localhost:5001", "http://localhost:7001")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Добавление поддержки IFormFile
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

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
app.UseStaticFiles(); // Для обслуживания изображений из wwwroot
app.UseCors("AllowUI");
app.UseAuthorization();
app.MapControllers();

// Создание базы данных при запуске (если не существует)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();