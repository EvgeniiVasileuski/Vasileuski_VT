using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vasileuski.UI.Services;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Areas.Admin.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _categoryService.CreateCategoryAsync(Category, ImageFile);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Ошибка при создании категории");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}