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

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return Page();
            }

            var result = await _teamService.CreateTeamAsync(Team, ImageFile);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Ошибка при создании команды");
                LoadCategories();
                return Page();
            }

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