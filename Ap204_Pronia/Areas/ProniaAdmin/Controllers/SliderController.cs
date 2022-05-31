using Ap204_Pronia.DAL;
using Ap204_Pronia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ap204_Pronia.Areas.ProniaAdmin.Controllers

{

    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            return View(slider);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            
            if (!ModelState.IsValid) return View();
            if (slider.Photo!=null)
            {
                if (!slider.Photo.ContentType.Contains("image"))
                {
                    return View();
                }
                if (slider.Photo.Length>1024*1024)
                {
                    return View();
                }
            }


            slider.Image = await slider.Photo.FileCreate(_env.WebRootPath, @"assets\images\website-images");
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Create()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Models.Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {

            Slider existedslider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (existedslider == null) return NotFound();
            if (id != slider.Id) return BadRequest();

            existedslider.Title = slider.Title;
            existedslider.Subtitle=slider.Subtitle;
            existedslider.Discount=slider.Discount;
            existedslider.Image=slider.Image;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Models.Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            Models.Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}
