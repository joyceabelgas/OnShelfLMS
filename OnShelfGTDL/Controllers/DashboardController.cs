using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public DashboardController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            var userName = HttpContext.Session.GetString("FullName");
            ViewData["ModuleName"] = "Dashboard";
            ViewData["Name"] = userName;

            var dashboardData = await _dbHelper.LoadDashboardAsync();

            return View(dashboardData);
        }
    }
}
