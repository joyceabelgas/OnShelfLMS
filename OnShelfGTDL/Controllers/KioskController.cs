using Microsoft.AspNetCore.Mvc;

namespace OnShelfGTDL.Controllers
{
    public class KioskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
