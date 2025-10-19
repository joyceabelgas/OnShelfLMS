using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class MyShelfController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public MyShelfController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "My Shelf";
            var userID = HttpContext.Session.GetString("UserID");
            var books = await _dbHelper.GetMyShelf(userID);
            return View(books);
        }
        [HttpPost]
        public IActionResult RemoveFromShelf(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return BadRequest("ISBN is required.");

            bool result = _dbHelper.RemoveFromShelf(isbn);
            return Json(new { success = result });
        }
    }
}
