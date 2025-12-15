using Vasileuski.API.DTOs;
using Vasileuski.Domain.Models;

namespace Vasileuski.API.Services
{
    public interface ITeamService
    {
        Task<ResponseData<List<TeamDTO>>> GetTeamsAsync(string? category = null);
        Task<ResponseData<TeamDTO>> GetTeamByIdAsync(int id);
        Task<ResponseData<TeamDTO>> CreateTeamAsync(CreateTeamDTO teamDto);
        Task<ResponseData<TeamDTO>> UpdateTeamAsync(int id, UpdateTeamDTO teamDto);
        Task<ResponseData<bool>> DeleteTeamAsync(int id);
        Task<ResponseData<List<TeamDTO>>> SearchTeamsAsync(string searchTerm);
        Task<ResponseData<object>> GetStatisticsAsync();
    }
}