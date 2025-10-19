using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class FineManagementController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public FineManagementController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Fine Management";
            return View();
        }
        [HttpGet]
        public JsonResult GetBorrowBook()
        {
            var data = _dbHelper.GetBorrowedBooksWithFines();
            return Json(data);
        }
        [HttpPost]
        public IActionResult UpdateFineStatus([FromBody] FineUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var userID = HttpContext.Session.GetString("UserID");
                model.UserId = userID;
                var result = _dbHelper.UpdateFineStatus(model.Id, model.UserId);
                return Json(new { success = result });
            }

            return Json(new { success = false });
        }
    }
}
