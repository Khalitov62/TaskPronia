using Microsoft.AspNetCore.Mvc;

namespace Ap204_Pronia.Areas.ProniaAdmin.Controllers
{
    public class DashbroadController : Controller
    {
        [Area("ProniaAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
