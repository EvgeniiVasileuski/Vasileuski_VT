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
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            Category = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _categoryService.UpdateCategoryAsync(Category.Id, Category, ImageFile);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Ошибка при обновлении категории");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}