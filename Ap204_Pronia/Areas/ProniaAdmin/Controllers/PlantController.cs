using Ap204_Pronia.DAL;
using Ap204_Pronia.Exntention;
using Ap204_Pronia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Ap204_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantController : Controller
    {
        private AppDbContext _context;
        private  IWebHostEnvironment _env;

        public PlantController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env =env;

        }
        public async Task<IActionResult> Index()
        {
            List<Plant> plants = await _context.Plants.Include(p => p.PlantImages).ToListAsync();
            return View(plants);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid) return View();

            if (plant.MainImage == null || plant.AnotherImage == null)
            {
                ModelState.AddModelError("", "Please choose main image or another image");
                return View();
            }
            if (!plant.MainImage.IsOkay(1))
            {
                ModelState.AddModelError("MainImage", "Please choose  image file and max 1Mb ");
                return View();
            }
            foreach (var img in plant.AnotherImage)
            {
                if (!img.IsOkay(1))
                {
                    ModelState.AddModelError("AnotherImage", "Please choose  image file and min 1Mb ");
                    return View();
                }
            }


            plant.PlantImages = new List<PlantImage>();
            PlantImage mainImage = new PlantImage
            {
                ImagePath = await plant.MainImage.FileCreate(_env.WebRootPath,"assets/images/website-images"),
                IsMain=true,
                Plant=plant

            };
            plant.PlantImages.Add(mainImage);


            foreach (var img in plant.AnotherImage)
            {
                PlantImage anotherimage = new PlantImage
                {
                    ImagePath = await img.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                    IsMain = false,
                    Plant = plant

                };
                plant.PlantImages.Add(anotherimage);
            }
            plant.PlantCategories = new List<PlantCategory>();

            foreach (var id in plant.CategoryIds)
            {
                PlantCategory plantCategory = new PlantCategory
                {
                    Plant = plant,
                    CategoryId = id
                };
            }

            await _context.Plants.AddAsync(plant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            Plant plant =await _context.Plants.Include(p=>p.PlantImages).FirstOrDefaultAsync(p=>p.Id==id);
            return View(plant);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Edit(Plant plant ,int id)
        {

            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            Plant existed = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p=>p.Id==id);
            if (existed == null) return NotFound();

            if (plant.ImageIds==null && plant.AnotherImage ==null)
            {
                ModelState.AddModelError("", "You can not empty images");
                return View(existed);
            }

            List<PlantImage> removaebleImages = existed.PlantImages.Where(p => p.IsMain == false && !plant.ImageIds.Contains(p.Id)).ToList();

            existed.PlantImages.RemoveAll(p => removaebleImages.Any(ri => ri.Id == p.Id));

            foreach (var image in removaebleImages)
            {
                FileUtilities.FileDelete(_env.WebRootPath, @"assets\images\website-images", image.ImagePath);
            }

            foreach (var image in plant.AnotherImage)
            {
                PlantImage plantImage = new PlantImage
                {
                    ImagePath = await image.FileCreate(_env.WebRootPath,@"assets\images\website-images"),
                    IsMain = false,
                    PlantId = existed.Id
                };
                existed.PlantImages.Add(plantImage);
            }
            _context.Entry(existed).CurrentValues.SetValues(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            Plant plant = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == id);
            return View(plant);
        }
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            Plant plant = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == id);
            return View(plant);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePlant( Plant plant,int id)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            plant = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == id);
            if (plant == null) return NotFound();

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            PlantImage plantImage = new PlantImage();
            string Path = $"assets/images/website-images/{plantImage.ImagePath}";
            FileInfo file = new FileInfo(Path);
            if (file.Exists)
            {
                file.Delete();
            }
           
        }


    }
}
