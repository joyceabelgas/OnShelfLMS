using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;
using System.Data;

namespace OnShelfGTDL.Controllers
{
    public class ReportsController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public ReportsController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpGet]
        public IActionResult LoadReport(string dateFilter, string status, string category)
        {
            DataTable dt = _dbHelper.LoadReport(dateFilter, status, category);

            // Optional: Convert to a strongly typed list if you want
            var list = dt.AsEnumerable().Select(row => new ReportViewModel
            {
                ISBN = row["ISBN"]?.ToString(),
                Title = row["Title"]?.ToString(),
                Borrower = row["Borrower"]?.ToString(),
                Status = row.Table.Columns.Contains("Status") ? row["Status"].ToString() : null,
                DueDate = row.Table.Columns.Contains("DueDate") ? Convert.ToDateTime(row["DueDate"]) : (DateTime?)null,
                BorrowedDate = row.Table.Columns.Contains("BorrowedDate") ? Convert.ToDateTime(row["BorrowedDate"]) : (DateTime?)null,
                DaysOverdue = row.Table.Columns.Contains("DaysOverdue") ? Convert.ToInt32(row["DaysOverdue"]) : (int?)null,
                Fine = row.Table.Columns.Contains("Fine") ? Convert.ToDecimal(row["Fine"]) : (decimal?)null,
                DateReturned = row.Table.Columns.Contains("DateReturned") ? Convert.ToDateTime(row["DateReturned"]) : (DateTime?)null
            }).ToList();

            return Json(list);
        }
    }
}
