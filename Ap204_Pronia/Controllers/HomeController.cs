using Ap204_Pronia.Models;
using Ap204_Pronia.DAL;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ap204_Pronia.ViewModels;

namespace Ap204_Pronia.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM model = new HomeVM
            {
                Sliders = await _context.Sliders.ToListAsync(),
                Customers = await _context.Customers.ToListAsync(),
                Plants = await _context.Plants.Include(p => p.PlantImages).ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View(model);
        }


    }
}
