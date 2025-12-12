using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Vasileuski.Domain.Interfaces;

namespace Vasileuski.Domain.Entities
{
    /// <summary>
    /// Категория спортивного соревнования/лиги
    /// </summary>
    public class Category : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории (на русском)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Нормализованное имя для URL (на английском, kebab-case)
        /// Например: "premier-league", "nba", "champions-league"
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NormalizedName { get; set; } = string.Empty;

        /// <summary>
        /// Описание категории
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Путь к изображению категории (логотип лиги)
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Навигационное свойство для команд в этой категории
        /// Один ко многим: одна категория содержит много команд
        /// </summary>
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        /// <summary>
        /// Конструктор для удобства создания
        /// </summary>
        public Category() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Category(string name, string normalizedName, string description)
        {
            Name = name;
            NormalizedName = normalizedName;
            Description = description;
        }
    }
}