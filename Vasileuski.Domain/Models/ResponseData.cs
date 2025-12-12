using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vasileuski.Domain.Models
{
    /// <summary>
    /// Класс, описывающий формат данных, получаемых от сервисов
    /// </summary>
    /// <typeparam name="T">Тип передаваемых данных</typeparam>
    public class ResponseData<T>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ResponseData()
        {
        }

        /// <summary>
        /// Запрашиваемые данные
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Признак успешного завершения запроса
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Сообщение в случае неуспешного завершения
        /// </summary>
        public string? ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Успешный ответ
        /// </summary>
        /// <param name="data">Передаваемые данные</param>
        /// <returns>Экземпляр ResponseData с данными</returns>
        public static ResponseData<T> Ok(T? data)
        {
            return new ResponseData<T> { Data = data };
        }

        /// <summary>
        /// Успешный ответ с сообщением
        /// </summary>
        /// <param name="data">Передаваемые данные</param>
        /// <param name="message">Сообщение об успехе</param>
        /// <returns>Экземпляр ResponseData с данными</returns>
        public static ResponseData<T> Ok(T? data, string message)
        {
            return new ResponseData<T>
            {
                Data = data,
                ErrorMessage = message
            };
        }

        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке</param>
        /// <returns>Экземпляр ResponseData с ошибкой</returns>
        public static ResponseData<T> Error(string message)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Success = false,
                Data = default
            };
        }

        /// <summary>
        /// Ошибка с данными (например, частично заполненными)
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке</param>
        /// <param name="data">Данные, которые всё же удалось получить</param>
        /// <returns>Экземпляр ResponseData с ошибкой и данными</returns>
        public static ResponseData<T> Error(string message, T? data)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Success = false,
                Data = data
            };
        }
    }
}
