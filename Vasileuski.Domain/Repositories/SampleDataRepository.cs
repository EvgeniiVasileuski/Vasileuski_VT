using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Helpers;

namespace Vasileuski.Domain.Repositories
{
    /// <summary>
    /// Репозиторий с тестовыми данными для спортивных команд
    /// </summary>
    public static class SampleDataRepository
    {
        /// <summary>
        /// Получить тестовые категории
        /// </summary>
        public static List<Category> GetCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Футбольная Премьер Лига",
                    NormalizedName = StringHelper.ToKebabCase("Футбольная Премьер Лига"),
                    Description = "Высший дивизион российского футбола",
                    Image = "/images/categories/premier-league.png"
                },
                new Category
                {
                    Id = 2,
                    Name = "НБА (Баскетбол)",
                    NormalizedName = "nba",
                    Description = "Национальная баскетбольная ассоциация (США)",
                    Image = "/images/categories/nba.png"
                },
                new Category
                {
                    Id = 3,
                    Name = "КХЛ (Хоккей)",
                    NormalizedName = "khl",
                    Description = "Континентальная хоккейная лига",
                    Image = "/images/categories/khl.png"
                },
                new Category
                {
                    Id = 4,
                    Name = "Теннис ATP Tour",
                    NormalizedName = "atp-tour",
                    Description = "Мировой тур Ассоциации теннисистов-профессионалов",
                    Image = "/images/categories/atp-tour.png"
                }
            };

            return categories;
        }

        /// <summary>
        /// Получить тестовые команды
        /// </summary>
        public static List<Team> GetTeams()
        {
            var teams = new List<Team>
            {
                // Футбольные команды (CategoryId = 1)
                new Team
                {
                    Id = 1,
                    Name = "Спартак Москва",
                    Description = "Один из самых популярных футбольных клубов России",
                    Points = 52,
                    CategoryId = 1,
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
                    Colors = "Красно-белые"
                },
                new Team
                {
                    Id = 2,
                    Name = "Зенит Санкт-Петербург",
                    Description = "Действующий чемпион России",
                    Points = 50,
                    CategoryId = 1,
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
                    Colors = "Сине-бело-голубые"
                },

                // Баскетбольные команды (CategoryId = 2)
                new Team
                {
                    Id = 3,
                    Name = "Лос-Анджелес Лейкерс",
                    Description = "Легендарный баскетбольный клуб из Лос-Анджелеса",
                    Points = 42,
                    CategoryId = 2,
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
                    Colors = "Фиолетовый, золотой, черный"
                },
                new Team
                {
                    Id = 4,
                    Name = "Голден Стэйт Уорриорз",
                    Description = "Один из самых успешных клубов последнего десятилетия",
                    Points = 44,
                    CategoryId = 2,
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
                    Colors = "Синий, желтый"
                },

                // Хоккейные команды (CategoryId = 3)
                new Team
                {
                    Id = 5,
                    Name = "СКА Санкт-Петербург",
                    Description = "Один из ведущих хоккейных клубов России",
                    Points = 108,
                    CategoryId = 3,
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
                    Colors = "Красный, синий"
                }
            };

            return teams;
        }

        /// <summary>
        /// Получить суммарные очки всех команд в категории
        /// </summary>
        public static int GetTotalPointsInCategory(int categoryId)
        {
            return GetTeams()
                .Where(t => t.CategoryId == categoryId)
                .Sum(t => t.Points);
        }

        /// <summary>
        /// Получить средние очки команд в категории
        /// </summary>
        public static double GetAveragePointsInCategory(int categoryId)
        {
            var teams = GetTeams().Where(t => t.CategoryId == categoryId).ToList();
            return teams.Any() ? teams.Average(t => t.Points) : 0;
        }
    }
}
