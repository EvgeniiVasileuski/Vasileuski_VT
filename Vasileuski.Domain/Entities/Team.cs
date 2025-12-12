using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Vasileuski.Domain.Interfaces;

namespace Vasileuski.Domain.Entities
{
    /// <summary>
    /// Спортивная команда/участник
    /// </summary>
    public class Team : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название команды
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание команды (история, достижения)
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Количество очков в текущем сезоне (ОБЯЗАТЕЛЬНОЕ свойство)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        /// <summary>
        /// Идентификатор категории/лиги
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Навигационное свойство к категории (nullable)
        /// </summary>
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Путь к изображению команды (эмблема)
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Город/страна базирования
        /// </summary>
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Год основания
        /// </summary>
        [Range(1800, 2100)]
        public int? FoundedYear { get; set; }

        /// <summary>
        /// Главный тренер
        /// </summary>
        [StringLength(100)]
        public string HeadCoach { get; set; } = string.Empty;

        /// <summary>
        /// Капитан команды
        /// </summary>
        [StringLength(100)]
        public string Captain { get; set; } = string.Empty;

        /// <summary>
        /// Стадион/арена
        /// </summary>
        [StringLength(150)]
        public string Stadium { get; set; } = string.Empty;

        /// <summary>
        /// Количество побед в сезоне
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Wins { get; set; }

        /// <summary>
        /// Количество поражений в сезоне
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Losses { get; set; }

        /// <summary>
        /// Количество ничьих (если применимо)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Draws { get; set; }

        /// <summary>
        /// Забитые голы/очки
        /// </summary>
        [Range(0, int.MaxValue)]
        public int GoalsFor { get; set; }

        /// <summary>
        /// Пропущенные голы/очки
        /// </summary>
        [Range(0, int.MaxValue)]
        public int GoalsAgainst { get; set; }

        /// <summary>
        /// Разница голов (GoalsFor - GoalsAgainst)
        /// </summary>
        public int GoalDifference => GoalsFor - GoalsAgainst;

        /// <summary>
        /// Место в турнирной таблице
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Position { get; set; }

        /// <summary>
        /// Форма команды (цвета)
        /// </summary>
        [StringLength(200)]
        public string Colors { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Team() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Team(string name, int points, string location)
        {
            Name = name;
            Points = points;
            Location = location;
        }

        /// <summary>
        /// Расчет процента побед
        /// </summary>
        public double GetWinPercentage()
        {
            var totalGames = Wins + Losses + Draws;
            if (totalGames == 0) return 0;
            return (double)Wins / totalGames * 100;
        }

        /// <summary>
        /// Добавление очков с проверкой
        /// </summary>
        public void AddPoints(int pointsToAdd)
        {
            if (pointsToAdd < 0)
                throw new ArgumentException("Нельзя добавить отрицательное количество очков");

            Points += pointsToAdd;
        }
    }
}