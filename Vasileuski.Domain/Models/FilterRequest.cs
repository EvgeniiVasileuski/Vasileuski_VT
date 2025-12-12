using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vasileuski.Domain.Models
{
    /// <summary>
    /// Класс для параметров фильтрации
    /// </summary>
    public class FilterRequest
    {
        /// <summary>
        /// Строка поиска
        /// </summary>
        public string? SearchString { get; set; }

        /// <summary>
        /// Идентификатор категории для фильтрации
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Минимальное значение (для цены, очков и т.д.)
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// Максимальное значение (для цены, очков и т.д.)
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// Поле для сортировки
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Направление сортировки (asc/desc)
        /// </summary>
        public string? SortDirection { get; set; } = "asc";

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
