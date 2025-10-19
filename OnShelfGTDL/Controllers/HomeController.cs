using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Common;
using OnShelfGTDL.Interface;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenuSvc _menuService;
        private readonly DatabaseHelper _databaseHelper;
        public HomeController(IMenuSvc menuSvc, DatabaseHelper databaseHelper)
        {
            _menuService = menuSvc;
            _databaseHelper = databaseHelper;   
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ModuleName"] = "Home";
            var userName = HttpContext.Session.GetString("FullName");
            ViewData["Name"] = userName;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoadBooksData()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var result = await _databaseHelper.LoadHomeBooksAsync(userID);

            var formatted = new
            {
                mostBorrowed = result.MostBorrowed.Select(x => new
                {
                    isbn = x.ISBN,
                    bookName = x.BookName,
                    bookImage = x.BookImage != null
                        ? $"data:image/png;base64,{Convert.ToBase64String(x.BookImage)}"
                        : null
                }),
                todaysPick = result.TodaysPick.Select(x => new
                {
                    isbn = x.ISBN,
                    bookName = x.BookName,
                    bookImage = x.BookImage != null
                        ? $"data:image/png;base64,{Convert.ToBase64String(x.BookImage)}"
                        : null
                })
            };

            return Json(formatted);
        }

        [HttpGet]
        public async Task<IActionResult> CheckNotification()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            var message = await _databaseHelper.GetNotificationMessageAsync(userID, role);

            if (!string.IsNullOrEmpty(message))
            {
                return Json(new { hasNotification = true, message });
            }

            return Json(new { hasNotification = false });
        }

    }
}
