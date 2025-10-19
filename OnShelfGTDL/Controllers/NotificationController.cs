using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class NotificationController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public NotificationController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Notifications";
            var userID = HttpContext.Session.GetString("UserID");
            var notifications = await _dbHelper.GetMyNotification(userID);

            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Notifications";
            var userID = HttpContext.Session.GetString("UserID");
            var data = await _dbHelper.GetMyNotification(userID);
            return Json(data); 
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _dbHelper.MarkAsRead(id);
            return Json(new { success = true });
        }

    }
}
