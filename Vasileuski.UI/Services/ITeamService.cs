using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с командами
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// Получение списка всех команд
        /// </summary>
        /// <param name="category">Нормализованное имя категории для фильтрации</param>
        /// <returns></returns>
        public Task<ResponseData<List<Team>>> GetTeamListAsync(string? category);

        /// <summary>
        /// Поиск команды по Id
        /// </summary>
        /// <param name="id">Идентификатор команды</param>
        /// <returns>Найденную команду или null, если команда не найдена</returns>
        public Task<ResponseData<Team>> GetTeamByIdAsync(int id);

        /// <summary>
        /// Обновление команды
        /// </summary>
        /// <param name="id">Id изменяемой команды</param>
        /// <param name="team">Команда с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        //public Task UpdateTeamAsync(int id, Team team, IFormFile? formFile);

        ///// <summary>
        ///// Удаление команды
        ///// </summary>
        ///// <param name="id">Id удаляемой команды</param>
        ///// <returns></returns>
        ////public Task DeleteTeamAsync(int id);

        ///// <summary>
        ///// Создание команды
        ///// </summary>
        ///// <param name="team">Новая команда</param>
        ///// <param name="formFile">Файл изображения</param>
        ///// <returns>Созданную команду</returns>
        //public Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile);

        Task UpdateTeamAsync(int id, Team team, IFormFile? formFile);
        Task DeleteTeamAsync(int id);
        Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile);
    }
}