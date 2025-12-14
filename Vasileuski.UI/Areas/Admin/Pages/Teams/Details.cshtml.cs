using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vasileuski.UI.Services;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Areas.Admin.Pages.Teams
{
    public class DetailsModel : PageModel
    {
        private readonly ITeamService _teamService;

        public DetailsModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public Team Team { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _teamService.GetTeamByIdAsync(id.Value);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            Team = response.Data;
            return Page();
        }
    }
}