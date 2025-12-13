using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Services;

namespace Vasileuski.UI.Areas.Admin.Pages.Teams
{
    public class CreateModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public CreateModel(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        public IActionResult OnGet()
        {
            LoadCategories();
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return Page();
            }

            // Устанавливаем даты создания/обновления
            Team.CreatedAt = DateTime.UtcNow;
            Team.UpdatedAt = DateTime.UtcNow;

            var response = await _teamService.CreateTeamAsync(Team);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                LoadCategories();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private void LoadCategories()
        {
            var categoriesResponse = _categoryService.GetCategoryListAsync().Result;
            if (categoriesResponse.Success)
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