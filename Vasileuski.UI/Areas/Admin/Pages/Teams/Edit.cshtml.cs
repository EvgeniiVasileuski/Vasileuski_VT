using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Services;

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
            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            var response = await _teamService.UpdateTeamAsync(Team.Id, Team);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Ошибка обновления");
                await LoadCategoriesAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task LoadCategoriesAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Success && categoriesResponse.Data != null)
            {
                Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
            }
            else
            {
                Categories = new SelectList(new List<Category>(), "Id", "Name");
            }
        }
    }
}