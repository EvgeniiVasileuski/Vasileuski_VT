using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Helpers;

namespace Vasileuski.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций (уже делается в Program.cs, но можно и здесь)
            //await context.Database.MigrateAsync();

            // Проверяем, есть ли уже данные в базе
            if (await context.Categories.AnyAsync())
            {
                Console.WriteLine("База данных уже содержит данные. Пропускаем инициализацию.");
                return;
            }

            Console.WriteLine("Начало заполнения базы данных начальными данными...");

            // Создаем категории
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Футбол",
                    NormalizedName = "football",
                    Description = "Футбольные лиги и турниры",
                    Image = "/images/categories/football.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Category
                {
                    Name = "Баскетбол",
                    NormalizedName = "basketball",
                    Description = "Баскетбольные лиги и турниры",
                    Image = "/images/categories/basketball.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Category
                {
                    Name = "Хоккей",
                    NormalizedName = "hockey",
                    Description = "Хоккейные лиги и турниры",
                    Image = "/images/categories/hockey.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Category
                {
                    Name = "Теннис",
                    NormalizedName = "tennis",
                    Description = "Теннисные турниры и ассоциации",
                    Image = "/images/categories/tennis.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Category
                {
                    Name = "Волейбол",
                    NormalizedName = "volleyball",
                    Description = "Волейбольные лиги и турниры",
                    Image = "/images/categories/volleyball.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            Console.WriteLine($"Добавлено {categories.Count} категорий.");

            // Получаем ID созданных категорий
            var footballCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == "football");
            var basketballCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == "basketball");
            var hockeyCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == "hockey");
            var tennisCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == "tennis");
            var volleyballCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == "volleyball");

            // Создаем команды
            var teams = new List<Team>
            {
                // Футбольные команды
                new Team
                {
                    Name = "Спартак Москва",
                    Description = "Один из самых популярных футбольных клубов России",
                    Points = 52,
                    CategoryId = footballCategory?.Id,
                    Image = "/images/teams/spartak.png",
                    Location = "Москва, Россия",
                    FoundedYear = 1922,
                    HeadCoach = "Гильермо Абаскаль",
                    Captain = "Георгий Джикия",
                    Stadium = "Открытие Банк Арена",
                    Wins = 16,
                    Losses = 4,
                    Draws = 4,
                    GoalsFor = 48,
                    GoalsAgainst = 25,
                    Position = 1,
                    Colors = "Красно-белые",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Team
                {
                    Name = "Зенит Санкт-Петербург",
                    Description = "Действующий чемпион России",
                    Points = 50,
                    CategoryId = footballCategory?.Id,
                    Image = "/images/teams/zenit.png",
                    Location = "Санкт-Петербург, Россия",
                    FoundedYear = 1925,
                    HeadCoach = "Сергей Семак",
                    Captain = "Дмитрий Чистяков",
                    Stadium = "Газпром Арена",
                    Wins = 15,
                    Losses = 5,
                    Draws = 4,
                    GoalsFor = 52,
                    GoalsAgainst = 22,
                    Position = 2,
                    Colors = "Сине-бело-голубые",
                    CreatedAt = DateTime.UtcNow.AddDays(-29),
                    UpdatedAt = DateTime.UtcNow.AddDays(-29)
                },
                new Team
                {
                    Name = "Локомотив Москва",
                    Description = "Один из старейших футбольных клубов России",
                    Points = 48,
                    CategoryId = footballCategory?.Id,
                    Image = "/images/teams/lokomotiv.png",
                    Location = "Москва, Россия",
                    FoundedYear = 1922,
                    HeadCoach = "Михаил Галактионов",
                    Captain = "Дмитрий Баринов",
                    Stadium = "РЖД Арена",
                    Wins = 14,
                    Losses = 6,
                    Draws = 4,
                    GoalsFor = 45,
                    GoalsAgainst = 28,
                    Position = 3,
                    Colors = "Красно-зеленые",
                    CreatedAt = DateTime.UtcNow.AddDays(-28),
                    UpdatedAt = DateTime.UtcNow.AddDays(-28)
                },
                
                // Баскетбольные команды
                new Team
                {
                    Name = "Лос-Анджелес Лейкерс",
                    Description = "Легендарный баскетбольный клуб из Лос-Анджелеса",
                    Points = 42,
                    CategoryId = basketballCategory?.Id,
                    Image = "/images/teams/lakers.png",
                    Location = "Лос-Анджелес, США",
                    FoundedYear = 1947,
                    HeadCoach = "Дарвин Хэм",
                    Captain = "ЛеБрон Джеймс",
                    Stadium = "Крайпл-центр",
                    Wins = 43,
                    Losses = 39,
                    Draws = 0,
                    GoalsFor = 11640,
                    GoalsAgainst = 11560,
                    Position = 7,
                    Colors = "Фиолетовый, золотой, черный",
                    CreatedAt = DateTime.UtcNow.AddDays(-27),
                    UpdatedAt = DateTime.UtcNow.AddDays(-27)
                },
                new Team
                {
                    Name = "Голден Стэйт Уорриорз",
                    Description = "Один из самых успешных клубов последнего десятилетия",
                    Points = 44,
                    CategoryId = basketballCategory?.Id,
                    Image = "/images/teams/warriors.png",
                    Location = "Сан-Франциско, США",
                    FoundedYear = 1946,
                    HeadCoach = "Стив Керр",
                    Captain = "Стефен Карри",
                    Stadium = "Чейз-центр",
                    Wins = 44,
                    Losses = 38,
                    Draws = 0,
                    GoalsFor = 11800,
                    GoalsAgainst = 11700,
                    Position = 6,
                    Colors = "Синий, желтый",
                    CreatedAt = DateTime.UtcNow.AddDays(-26),
                    UpdatedAt = DateTime.UtcNow.AddDays(-26)
                },
                
                // Хоккейные команды
                new Team
                {
                    Name = "СКА Санкт-Петербург",
                    Description = "Один из ведущих хоккейных клубов России",
                    Points = 108,
                    CategoryId = hockeyCategory?.Id,
                    Image = "/images/teams/ska.png",
                    Location = "Санкт-Петербург, Россия",
                    FoundedYear = 1946,
                    HeadCoach = "Роман Ротенберг",
                    Captain = "Василий Подколзин",
                    Stadium = "Ледовый дворец",
                    Wins = 36,
                    Losses = 12,
                    Draws = 0,
                    GoalsFor = 182,
                    GoalsAgainst = 110,
                    Position = 1,
                    Colors = "Красный, синий",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Team
                {
                    Name = "ЦСКА Москва",
                    Description = "Легендарный хоккейный клуб",
                    Points = 102,
                    CategoryId = hockeyCategory?.Id,
                    Image = "/images/teams/cska.png",
                    Location = "Москва, Россия",
                    FoundedYear = 1946,
                    HeadCoach = "Сергей Фёдоров",
                    Captain = "Никита Нестеров",
                    Stadium = "ЦСКА Арена",
                    Wins = 34,
                    Losses = 14,
                    Draws = 0,
                    GoalsFor = 175,
                    GoalsAgainst = 115,
                    Position = 2,
                    Colors = "Красно-синие",
                    CreatedAt = DateTime.UtcNow.AddDays(-24),
                    UpdatedAt = DateTime.UtcNow.AddDays(-24)
                },
                
                // Теннис (индивидуальные спортсмены)
                new Team
                {
                    Name = "Новак Джокович",
                    Description = "Сербский теннисист, бывшая первая ракетка мира",
                    Points = 9850,
                    CategoryId = tennisCategory?.Id,
                    Image = "/images/teams/djokovic.png",
                    Location = "Белград, Сербия",
                    HeadCoach = "Горан Иванишевич",
                    Wins = 55,
                    Losses = 7,
                    Draws = 0,
                    Position = 1,
                    Colors = "Синий, белый",
                    CreatedAt = DateTime.UtcNow.AddDays(-23),
                    UpdatedAt = DateTime.UtcNow.AddDays(-23)
                },
                
                // Волейбольные команды
                new Team
                {
                    Name = "Зенит Казань",
                    Description = "Один из сильнейших волейбольных клубов России",
                    Points = 68,
                    CategoryId = volleyballCategory?.Id,
                    Image = "/images/teams/zenit_kazan.png",
                    Location = "Казань, Россия",
                    FoundedYear = 2000,
                    HeadCoach = "Алексей Вербов",
                    Captain = "Максим Михайлов",
                    Stadium = "Баскет-холл",
                    Wins = 22,
                    Losses = 4,
                    Draws = 0,
                    GoalsFor = 1850,
                    GoalsAgainst = 1650,
                    Position = 1,
                    Colors = "Синий, белый",
                    CreatedAt = DateTime.UtcNow.AddDays(-22),
                    UpdatedAt = DateTime.UtcNow.AddDays(-22)
                }
            };

            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            Console.WriteLine($"Добавлено {teams.Count} команд.");
            Console.WriteLine("Заполнение базы данных завершено успешно.");
        }
    }
}