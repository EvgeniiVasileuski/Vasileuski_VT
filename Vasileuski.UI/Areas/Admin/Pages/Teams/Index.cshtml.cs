using Microsoft.AspNetCore.Mvc.RazorPages;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Services;

namespace Vasileuski.UI.Areas.Admin.Pages.Teams
{
    public class IndexModel : PageModel
    {
        private readonly ITeamService _teamService;

        public IndexModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IList<Team> Team { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var response = await _teamService.GetTeamListAsync(null);
            if (response.Success && response.Data != null)
            {
                Team = response.Data.ToList(); // Преобразуем IEnumerable в List
            }
            else
            {
                Team = new List<Team>();
            }
        }
    }
}