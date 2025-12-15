using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;
using Vasileuski.UI.Data;

namespace Vasileuski.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")] // Только для администраторов
    public class TeamsController : Controller
    {
        private readonly AdminDbContext _context;

        public TeamsController(AdminDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Teams
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams
                .Include(t => t.Category)
                .ToListAsync();
            return View(teams);
        }

        // GET: Admin/Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // Остальные методы CRUD...
    }
}