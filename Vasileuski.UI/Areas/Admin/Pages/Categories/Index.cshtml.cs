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
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();
            if (response.Success)
            {
                Category = response.Data ?? new List<Category>();
            }
        }
    }
}