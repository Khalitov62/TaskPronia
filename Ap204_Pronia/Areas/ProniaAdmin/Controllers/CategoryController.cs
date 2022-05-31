using Ap204_Pronia.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ap204_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController : Controller
    {
        
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Models.Category> categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Models.Category category)
        {
            if (!ModelState.IsValid) return View();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            Models.Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Models.Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Category category)
        {

            Models.Category existedCategory = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (existedCategory == null) return NotFound();
            if (id != category.Id) return BadRequest();

            existedCategory.Name = category.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Models.Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            Models.Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
