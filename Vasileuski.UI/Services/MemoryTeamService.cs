using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Services
{
    public class MemoryTeamService : ITeamService
    {
        private readonly List<Team> _teams = new List<Team>();
        private int _nextId = 1;

        public MemoryTeamService()
        {
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // ХОККЕЙНЫЕ КОМАНДЫ (CategoryId = 1)
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "СКА Санкт-Петербург",
                Description = "Один из ведущих хоккейных клубов России",
                Points = 108,
                Location = "Санкт-Петербург, Россия",
                FoundedYear = 1946,
                HeadCoach = "Роман Ротенберг",
                Captain = "Василий Подколзин",
                Stadium = "Ледовый дворец",
                Wins = 36,
                Draws = 0,
                Losses = 12,
                GoalsFor = 182,
                GoalsAgainst = 110,
                Position = 1,
                Colors = "#00008B,#FFFFFF",
                Image = "/images/teams/ska.png",
                CategoryId = 1, // Хоккей
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "ЦСКА Москва",
                Description = "Профессиональный хоккейный клуб",
                Points = 98,
                Location = "Москва, Россия",
                FoundedYear = 1946,
                HeadCoach = "Сергей Федоров",
                Captain = "Никита Нестеров",
                Stadium = "ЦСКА Арена",
                Wins = 32,
                Draws = 2,
                Losses = 14,
                GoalsFor = 165,
                GoalsAgainst = 105,
                Position = 2,
                Colors = "#FF0000,#0000FF",
                Image = "/images/teams/cska.png",
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Локомотив",
                Description = "Хоккейный клуб из Ярославля",
                Points = 85,
                Location = "Ярославль, Россия",
                FoundedYear = 1959,
                HeadCoach = "Игорь Никитин",
                Captain = "Станислав Бочаров",
                Stadium = "Арена 2000",
                Wins = 28,
                Draws = 1,
                Losses = 19,
                GoalsFor = 150,
                GoalsAgainst = 125,
                Position = 3,
                Colors = "#FF0000,#000000",
                Image = "/images/teams/lokomotiv.png",
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Спартак Москва",
                Description = "Один из старейших хоккейных клубов России",
                Points = 80,
                Location = "Москва, Россия",
                FoundedYear = 1946,
                HeadCoach = "Борис Михайлов",
                Captain = "Александр Хохлачев",
                Stadium = "ЛДС Спартак",
                Wins = 26,
                Draws = 2,
                Losses = 20,
                GoalsFor = 142,
                GoalsAgainst = 130,
                Position = 4,
                Colors = "#FF0000,#FFFFFF",
                Image = "/images/teams/spartak.png",
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // ФУТБОЛЬНЫЕ КОМАНДЫ (CategoryId = 2)
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Зенит",
                Description = "Футбольный клуб из Санкт-Петербурга",
                Points = 65,
                Location = "Санкт-Петербург, Россия",
                FoundedYear = 1925,
                HeadCoach = "Сергей Семак",
                Captain = "Денис Черышев",
                Stadium = "Газпром Арена",
                Wins = 20,
                Draws = 5,
                Losses = 13,
                GoalsFor = 120,
                GoalsAgainst = 85,
                Position = 5,
                Colors = "#0000FF,#FFFFFF",
                Image = "/images/teams/zenit.png",
                CategoryId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Спартак Москва (футбол)",
                Description = "Футбольный клуб из Москвы",
                Points = 60,
                Location = "Москва, Россия",
                FoundedYear = 1922,
                HeadCoach = "Гильермо Абаскаль",
                Captain = "Георгий Джикия",
                Stadium = "Открытие Банк Арена",
                Wins = 18,
                Draws = 6,
                Losses = 14,
                GoalsFor = 110,
                GoalsAgainst = 90,
                Position = 6,
                Colors = "#FF0000,#FFFFFF",
                Image = "/images/teams/spartak.png",
                CategoryId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // БАСКЕТБОЛЬНЫЕ КОМАНДЫ (CategoryId = 3)
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Лос-Анджелес Лейкерс",
                Description = "Профессиональный баскетбольный клуб из США",
                Points = 55,
                Location = "Лос-Анджелес, США",
                FoundedYear = 1947,
                HeadCoach = "Дарвин Хэм",
                Captain = "ЛеБрон Джеймс",
                Stadium = "Крипто-ком Арена",
                Wins = 25,
                Draws = 0,
                Losses = 25,
                GoalsFor = 115,
                GoalsAgainst = 115,
                Position = 7,
                Colors = "#552583,#FDB927",
                Image = "/images/teams/lakers.png",
                CategoryId = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Голден Стэйт Уорриорз",
                Description = "Профессиональный баскетбольный клуб из США",
                Points = 50,
                Location = "Сан-Франциско, США",
                FoundedYear = 1946,
                HeadCoach = "Стив Керр",
                Captain = "Стефен Карри",
                Stadium = "Чейз Центр",
                Wins = 22,
                Draws = 0,
                Losses = 28,
                GoalsFor = 112,
                GoalsAgainst = 118,
                Position = 8,
                Colors = "#1D428A,#FFC72C",
                Image = "/images/teams/warriors.png",
                CategoryId = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // ТЕННИС (CategoryId = 4) - обычно это индивидуальный вид спорта
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Новак Джокович",
                Description = "Профессиональный теннисист, серб",
                Points = 120,
                Location = "Белград, Сербия",
                FoundedYear = 1987,
                HeadCoach = "Горан Иванишевич",
                Captain = "Новак Джокович",
                Stadium = "Разные",
                Wins = 45,
                Draws = 0,
                Losses = 5,
                GoalsFor = 90,
                GoalsAgainst = 30,
                Position = 9,
                Colors = "#0C4076,#FFFFFF",
                Image = "/images/teams/djokovic.png",
                CategoryId = 4,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // ВОЛЕЙБОЛЬНЫЕ КОМАНДЫ (CategoryId = 5)
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Зенит (волейбол)",
                Description = "Волейбольный клуб из Казани",
                Points = 40,
                Location = "Казань, Россия",
                FoundedYear = 2000,
                HeadCoach = "Алексей Вербов",
                Captain = "Мэттью Андерсон",
                Stadium = "Центр волейбола Санкт-Петербург",
                Wins = 18,
                Draws = 2,
                Losses = 20,
                GoalsFor = 60,
                GoalsAgainst = 65,
                Position = 10,
                Colors = "#0000FF,#FFFFFF",
                Image = "/images/teams/zenit.png",
                CategoryId = 5,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // Добавим еще несколько команд для разнообразия
            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Барселона (футбол)",
                Description = "Испанский футбольный клуб",
                Points = 75,
                Location = "Барселона, Испания",
                FoundedYear = 1899,
                HeadCoach = "Хави",
                Captain = "Серхио Бускетс",
                Stadium = "Камп Ноу",
                Wins = 24,
                Draws = 3,
                Losses = 11,
                GoalsFor = 130,
                GoalsAgainst = 70,
                Position = 11,
                Colors = "#A50044,#004D98",
                Image = "/images/teams/zenit.png", // временно используем имеющееся
                CategoryId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            _teams.Add(new Team
            {
                Id = _nextId++,
                Name = "Чикаго Буллз (баскетбол)",
                Description = "Американский баскетбольный клуб",
                Points = 45,
                Location = "Чикаго, США",
                FoundedYear = 1966,
                HeadCoach = "Билли Донован",
                Captain = "Демар Дерозан",
                Stadium = "Юнайтед Центр",
                Wins = 20,
                Draws = 0,
                Losses = 30,
                GoalsFor = 105,
                GoalsAgainst = 115,
                Position = 12,
                Colors = "#CE1141,#000000",
                Image = "/images/teams/warriors.png", // временно используем имеющееся
                CategoryId = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
        

        // Исправленный метод - возвращает Task<ServiceResponse<IEnumerable<Team>>>
        public Task<ServiceResponse<IEnumerable<Team>>> GetTeamListAsync(string? category)
        {
            try
            {
                IEnumerable<Team> result = _teams;

                if (!string.IsNullOrEmpty(category))
                {
                    // Фильтрация по категории (если будет реализовано)
                    result = result.Where(t =>
                        t.Category != null &&
                        t.Category.NormalizedName == category);
                }

                return Task.FromResult(ServiceResponse<IEnumerable<Team>>.Ok(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<IEnumerable<Team>>.Error(
                    $"Ошибка при получении списка команд: {ex.Message}"));
            }
        }

        // Исправленный метод - возвращает Task<ServiceResponse<Team>>
        public Task<ServiceResponse<Team>> GetTeamByIdAsync(int id)
        {
            try
            {
                var team = _teams.FirstOrDefault(t => t.Id == id);

                if (team == null)
                {
                    return Task.FromResult(ServiceResponse<Team>.Error(
                        $"Команда с ID {id} не найдена"));
                }

                return Task.FromResult(ServiceResponse<Team>.Ok(team));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<Team>.Error(
                    $"Ошибка при получении команды: {ex.Message}"));
            }
        }

        // Остальные методы остаются без изменений
        public Task<ServiceResponse<Team>> CreateTeamAsync(Team team)
        {
            try
            {
                team.Id = _nextId++;
                team.CreatedAt = DateTime.UtcNow;
                team.UpdatedAt = DateTime.UtcNow;

                _teams.Add(team);

                return Task.FromResult(ServiceResponse<Team>.Ok(team, "Команда успешно создана"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<Team>.Error(
                    $"Ошибка при создании команды: {ex.Message}"));
            }
        }

        public Task<ServiceResponse<Team>> UpdateTeamAsync(int id, Team team)
        {
            try
            {
                var existingTeam = _teams.FirstOrDefault(t => t.Id == id);
                if (existingTeam == null)
                {
                    return Task.FromResult(ServiceResponse<Team>.Error(
                        $"Команда с ID {id} не найдена"));
                }

                // Обновляем свойства
                existingTeam.Name = team.Name;
                existingTeam.Description = team.Description;
                existingTeam.Points = team.Points;
                existingTeam.CategoryId = team.CategoryId;
                existingTeam.Image = team.Image;
                existingTeam.Location = team.Location;
                existingTeam.FoundedYear = team.FoundedYear;
                existingTeam.HeadCoach = team.HeadCoach;
                existingTeam.Captain = team.Captain;
                existingTeam.Stadium = team.Stadium;
                existingTeam.Wins = team.Wins;
                existingTeam.Draws = team.Draws;
                existingTeam.Losses = team.Losses;
                existingTeam.GoalsFor = team.GoalsFor;
                existingTeam.GoalsAgainst = team.GoalsAgainst;
                existingTeam.Position = team.Position;
                existingTeam.Colors = team.Colors;
                existingTeam.UpdatedAt = DateTime.UtcNow;

                return Task.FromResult(ServiceResponse<Team>.Ok(
                    existingTeam, "Команда успешно обновлена"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<Team>.Error(
                    $"Ошибка при обновлении команды: {ex.Message}"));
            }
        }

        public Task<ServiceResponse<bool>> DeleteTeamAsync(int id)
        {
            try
            {
                var team = _teams.FirstOrDefault(t => t.Id == id);
                if (team == null)
                {
                    return Task.FromResult(ServiceResponse<bool>.Error(
                        $"Команда с ID {id} не найдена"));
                }

                _teams.Remove(team);

                return Task.FromResult(ServiceResponse<bool>.Ok(true, "Команда успешно удалена"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<bool>.Error(
                    $"Ошибка при удалении команды: {ex.Message}"));
            }
        }
    }
}