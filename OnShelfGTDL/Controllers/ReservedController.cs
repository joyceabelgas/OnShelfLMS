using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class ReservedController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public ReservedController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Reserved Books";
            var userID = HttpContext.Session.GetString("UserID");
            var books = await _dbHelper.GetMyReservation(userID);
            return View(books);
        }

        [HttpPost]
        public IActionResult CancelReservation(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return BadRequest("ISBN is required.");

            bool result = _dbHelper.CancelReservation(isbn);
            return Json(new { success = result });
        }

    }
}
