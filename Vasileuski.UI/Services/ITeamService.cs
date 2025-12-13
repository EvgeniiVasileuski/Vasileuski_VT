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
       // Используем ServiceResponse с IEnumerable<Team> вместо List<Team>
        Task<ServiceResponse<IEnumerable<Team>>> GetTeamListAsync(string? category);
        Task<ServiceResponse<Team>> GetTeamByIdAsync(int id);
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
        //public Task DeleteTeamAsync(int id);

        ///// <summary>
        ///// Создание команды
        ///// </summary>
        ///// <param name="team">Новая команда</param>
        ///// <param name="formFile">Файл изображения</param>
        ///// <returns>Созданную команду</returns>
        //public Task<ResponseData<Team>> CreateTeamAsync(Team team, IFormFile? formFile);
        // Новые методы для CRUD
        // Новые методы для CRUD
        Task<ServiceResponse<Team>> CreateTeamAsync(Team team);
        Task<ServiceResponse<Team>> UpdateTeamAsync(int id, Team team);
        Task<ServiceResponse<bool>> DeleteTeamAsync(int id);
    }
}