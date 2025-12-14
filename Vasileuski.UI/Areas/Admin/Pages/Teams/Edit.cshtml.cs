using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vasileuski.UI.Services;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Areas.Admin.Pages.Teams
{
    public class EditModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public EditModel(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

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
            LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return Page();
            }

            await _teamService.UpdateTeamAsync(Team.Id, Team, ImageFile);
            return RedirectToPage("./Index");
        }

        private void LoadCategories()
        {
            var categoriesResponse = _categoryService.GetCategoryListAsync().Result;
            var categories = categoriesResponse.Data ?? new List<Category>();
            Categories = new SelectList(categories, "Id", "Name");
        }
    }
}