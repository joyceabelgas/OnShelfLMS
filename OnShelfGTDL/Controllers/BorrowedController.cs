using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class BorrowedController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public BorrowedController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MyBorrowedBooks()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Borrowed Books";
            var userID = HttpContext.Session.GetString("UserID");
            var books = await _dbHelper.GetMyBorrowedBooks(userID);
            return View(books);
        }
        [HttpGet]
        public IActionResult GetBorrowReturnDetails(int id)
        {
            var data = _dbHelper.LoadBorrowBookReturn(id);
            if (data == null)
            {
                return NotFound();
            }
            return Json(data);
        }


        [HttpPost]
        public IActionResult ProcessReturn(int id, bool withFines)
        {
            try
            {
                _dbHelper.MarkBookAsReturned(id, withFines);
                return Json(new { success = true, message = "Book marked as returned." });
            }
            catch
            {
                return Json(new { success = false, message = "Error occurred." });
            }
        }

        [HttpPost]
        public IActionResult ReturnBook(int id)
        {
            try
            {
                _dbHelper.MarkBookAsReturned(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ReturnBookWFine(int id)
        {
            try
            {
                _dbHelper.MarkBookAsReturnedwFine(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public JsonResult GetFines()
        {
            var fines = _dbHelper.GetFineTypes();
            return Json(fines);
        }

        [HttpGet]
        public JsonResult GetFineAmount(int id)
        {
            var amount = _dbHelper.GetFineAmountById(id);
            return Json(amount);
        }

        [HttpPost]
        public IActionResult CancelBorrowedBook(int borrowId, string isbn)
        {
            var result = _dbHelper.CancelBorrowedBook(borrowId, isbn);
            return Json(new { success = result, message = result ? "Borrow canceled successfully." : "Failed to cancel borrow." });
        }


    }
}
