using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vasileuski.Domain.Models
{
    /// <summary>
    /// Класс для постраничного вывода данных
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    public class PagedResponseData<T> : ResponseData<List<T>>
    {
        /// <summary>
        /// Общее количество элементов
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        /// <summary>
        /// Есть ли предыдущая страница
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Есть ли следующая страница
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Успешный ответ с постраничными данными
        /// </summary>
        public static PagedResponseData<T> Ok(List<T> data, int totalCount, int currentPage, int pageSize)
        {
            return new PagedResponseData<T>
            {
                Data = data,
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Success = true
            };
        }

        /// <summary>
        /// Ошибка для постраничного запроса
        /// </summary>
        public new static PagedResponseData<T> Error(string message)
        {
            return new PagedResponseData<T>
            {
                ErrorMessage = message,
                Success = false,
                Data = new List<T>(),
                TotalCount = 0,
                CurrentPage = 1,
                PageSize = 0
            };
        }
    }
}
