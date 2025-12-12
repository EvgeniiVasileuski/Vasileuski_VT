using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Сервис для работы с командами в памяти
    /// </summary>
    public class MemoryTeamService : ITeamService
    {
        private List<Team> _teams;
        private List<Category> _categories;

        public MemoryTeamService(ICategoryService categoryService)
        {
            // Получаем категории через сервис категорий
            _categories = categoryService.GetCategoryListAsync()
                .Result
                .Data ?? new List<Category>();

            SetupData();
        }

        /// <summary>
        /// Наполнение данных командами
        /// </summary>
        private void SetupData()
        {
            // Находим ID категорий по нормализованным именам
            var footballCategory = _categories.Find(c => c.NormalizedName.Equals("football"));
            var basketballCategory = _categories.Find(c => c.NormalizedName.Equals("basketball"));
            var hockeyCategory = _categories.Find(c => c.NormalizedName.Equals("hockey"));
            var tennisCategory = _categories.Find(c => c.NormalizedName.Equals("tennis"));

            _teams = new List<Team>
            {
                // Футбольные команды
                new Team
                {
                    Id = 1,
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
                    Colors = "Красно-белые"
                },
                new Team
                {
                    Id = 2,
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
                    Colors = "Сине-бело-голубые"
                },
                new Team
                {
                    Id = 3,
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
                    Colors = "Красно-зеленые"
                },

                // Баскетбольные команды
                new Team
                {
                    Id = 4,
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
                    Colors = "Фиолетовый, золотой, черный"
                },
                new Team
                {
                    Id = 5,
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
                    Colors = "Синий, желтый"
                },

                // Хоккейные команды
                new Team
                {
                    Id = 6,
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
                    Colors = "Красный, синий"
                },
                new Team
                {
                    Id = 7,
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
                    Colors = "Красно-синие"
                },

                // Теннис (индивидуальные спортсмены представлены как команды из 1 человека)
                new Team
                {
                    Id = 8,
                    Name = "Новак Джокович",
                    Description = "Сербский теннисист, бывшая первая ракетка мира",
                    Points = 9850,
                    CategoryId = tennisCategory?.Id,
                    Image = "/images/teams/djokovic.png",
                    Location = "Белград, Сербия",
                    FoundedYear = null,
                    HeadCoach = "Горан Иванишевич",
                    Captain = "",
                    Stadium = "",
                    Wins = 55,
                    Losses = 7,
                    Draws = 0,
                    GoalsFor = 0,
                    GoalsAgainst = 0,
                    Position = 1,
                    Colors = "Синий, белый"
                }
            };
        }

        /// <summary>
        /// Получение списка всех команд с фильтрацией по категории
        /// </summary>
        public Task<ResponseData<List<Team>>> GetTeamListAsync(string? category)
        {
            IEnumerable<Team> teams = _teams;

            // Фильтрация по категории, если указана
            if (!string.IsNullOrEmpty(category))
            {
                // Находим категорию по нормализованному имени
                var categoryObj = _categories.FirstOrDefault(c =>
                    c.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase));

                if (categoryObj != null)
                {
                    teams = teams.Where(t => t.CategoryId == categoryObj.Id);
                }
            }

            // Сортировка по очкам (по убыванию)
            teams = teams.OrderByDescending(t => t.Points)
                        .ThenByDescending(t => t.GoalDifference);

            var result = ResponseData<List<Team>>.Ok(teams.ToList());
            return Task.FromResult(result);
        }

        /// <summary>
        /// Поиск команды по Id
        /// </summary>
        public Task<ResponseData<Team>> GetTeamByIdAsync(int id)
        {
            var team = _teams.FirstOrDefault(t => t.Id == id);

            if (team == null)
            {
                return Task.FromResult(ResponseData<Team>.Error($"Команда с ID {id} не найдена"));
            }

            return Task.FromResult(ResponseData<Team>.Ok(team));
        }

        /// <summary>
        /// Обновление команды (заглушка - будет реализовано позже)
        /// </summary>
        public Task UpdateTeamAsync(int id, Team team, IFormFile? formFile)
        {
            // Заглушка - в реальном приложении здесь будет логика обновления
            return Task.CompletedTask;
        }

        /// <summary>
        /// Удаление команды (заглушка - будет реализовано позже)
        /// </summary>
        public Task DeleteTeamAsync(int id)
        {
            // Заглушка - в реальном приложении здесь будет логика удаления
            return Task.CompletedTask;
        }

        /// <summary>
        /// Создание команды (заглушка - будет реализовано позже)
        /// </summary>
        public Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile)
        {
            // Заглушка - в реальном приложении здесь будет логика создания
            team.Id = _teams.Max(t => t.Id) + 1;
            _teams.Add(team);

            return Task.FromResult(ResponseData<Team>.Ok(team));
        }
    }
}