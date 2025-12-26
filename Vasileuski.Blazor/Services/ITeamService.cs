using Vasileuski.Blazor.Models;

namespace Vasileuski.Blazor.Services
{
    public interface ITeamService
    {
        event Action ListChanged;

        IEnumerable<TeamDTO> Teams { get; }
        int CurrentPage { get; }
        int TotalPages { get; }

        Task GetTeams(int pageNo = 1, int pageSize = 3);
    }
}