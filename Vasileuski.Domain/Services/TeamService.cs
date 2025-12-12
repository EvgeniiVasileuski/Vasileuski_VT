using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;
using Vasileuski.Domain.Repositories;

namespace Vasileuski.Domain.Services
{
    /// <summary>
    /// Сервис для работы с командами
    /// </summary>
    public class TeamService
    {
        /// <summary>
        /// Получить все команды
        /// </summary>
        public ResponseData<List<Team>> GetAllTeams()
        {
            try
            {
                var teams = SampleDataRepository.GetTeams();
                return ResponseData<List<Team>>.Ok(teams);
            }
            catch (Exception ex)
            {
                return ResponseData<List<Team>>.Error($"Ошибка при получении команд: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить команду по ID
        /// </summary>
        public ResponseData<Team> GetTeamById(int id)
        {
            try
            {
                var team = SampleDataRepository.GetTeams().FirstOrDefault(t => t.Id == id);
                
                if (team == null)
                {
                    return ResponseData<Team>.Error($"Команда с ID {id} не найдена");
                }

                return ResponseData<Team>.Ok(team);
            }
            catch (Exception ex)
            {
                return ResponseData<Team>.Error($"Ошибка при получении команды: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить команды по категории
        /// </summary>
        public ResponseData<List<Team>> GetTeamsByCategory(int categoryId)
        {
            try
            {
                var teams = SampleDataRepository.GetTeams()
                    .Where(t => t.CategoryId == categoryId)
                    .OrderByDescending(t => t.Points)
                    .ToList();

                return ResponseData<List<Team>>.Ok(teams);
            }
            catch (Exception ex)
            {
                return ResponseData<List<Team>>.Error($"Ошибка при получении команд по категории: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить статистику по очкам
        /// </summary>
        public ResponseData<object> GetPointsStatistics()
        {
            try
            {
                var teams = SampleDataRepository.GetTeams();
                
                var statistics = new
                {
                    TotalPoints = teams.Sum(t => t.Points),
                    AveragePoints = Math.Round(teams.Average(t => t.Points), 2),
                    MaxPoints = teams.Max(t => t.Points),
                    MinPoints = teams.Min(t => t.Points),
                    TopTeams = teams.OrderByDescending(t => t.Points).Take(5).Select(t => new
                    {
                        t.Name,
                        t.Points,
                        
                    })
                };

                return ResponseData<object>.Ok(statistics);
            }
            catch (Exception ex)
            {
                return ResponseData<object>.Error($"Ошибка при расчете статистики: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить команды с постраничным выводом
        /// </summary>
        public PagedResponseData<Team> GetTeamsPaged(int pageNumber, int pageSize)
        {
            try
            {
                var allTeams = SampleDataRepository.GetTeams();
                var totalCount = allTeams.Count;
                
                var pagedTeams = allTeams
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return PagedResponseData<Team>.Ok(pagedTeams, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return PagedResponseData<Team>.Error($"Ошибка при постраничном получении команд: {ex.Message}");
            }
        }
    }
}