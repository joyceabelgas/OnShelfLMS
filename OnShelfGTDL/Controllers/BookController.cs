using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OnShelfGTDL.Interface;
using OnShelfGTDL.Models;
using System.Data;

namespace OnShelfGTDL.Controllers
{
    public class BookController : Controller
    {
        private readonly IMenuSvc _menuService;
        private readonly DatabaseHelper _dbHelper;
        private readonly EmailHelper _emailHelper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public BookController(IMenuSvc menuSvc, DatabaseHelper dbHelper, EmailHelper emailHelper, IWebHostEnvironment hostEnvironment)
        {
            _menuService = menuSvc;
            _dbHelper = dbHelper;
            _emailHelper = emailHelper;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ManageBooks()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["ModuleName"] = "Manage Books";
            List<BookModelView> books = _dbHelper.GetAllBooks();
            return View(books);
        }
        [HttpGet]
        public IActionResult LoadBooks()
        {
            List<BookModelView> books = _dbHelper.GetAllBooks();
            return Json(books);
        }

        [HttpPost]
        public IActionResult SaveBookInformation(BookModel model, IFormFile bookImage)
        {
            if (ModelState.Count > 0)
            {
                byte[] bookImageData = null;

                if (bookImage != null && bookImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        bookImage.CopyTo(memoryStream);
                        bookImageData = memoryStream.ToArray();
                    }
                }

                // Save the book record including image
                bool isSaved = _dbHelper.SaveBook(
                    model.ISNB,
                    model.BookName,
                    model.Category, 
                    model.AuthorsName,
                    model.BookShelf,
                    model.Copyright,
                    model.StockQuantity,
                    model.PublicationName,
                    model.Description,
                    bookImageData // pass as byte[] to your SaveBook method
                );

                return Json(new { success = isSaved, message = isSaved ? "Book saved successfully!" : "Failed to save book." });
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpPost]
        public IActionResult UpdateBookInformation(BookModel model, IFormFile bookPicture)
        {
            if (ModelState.Count > 0)
            {
                // Call your SaveBook method to store the book data in the database
                bool isSaved = _dbHelper.UpdateBook(
                    model.ISNB,
                    model.BookName,
                    model.Category,
                    model.AuthorsName,
                    model.BookShelf,
                    model.Copyright,
                    model.StockQuantity,
                    model.PublicationName,
                    model.Description,
                    bookPicture
                );

                if (isSaved)
                {
                    return Json(new { success = true, message = "Book saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save book." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpPost]
        public IActionResult DeleteBook(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return BadRequest("ISBN is required.");
            }

            bool isDeleted = _dbHelper.DeleteBook(isbn);

            if (isDeleted)
            {
                return Json(new { success = true, message = "Book deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete the book." });
            }
        }
        public IActionResult BorrowBooks()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new BorrowReturnFineViewModel
            {
                ReturnID = null,
                FineList = _dbHelper.GetFineTypes() // Fetch from DB
            };

            ViewData["ModuleName"] = "Borrow Books";
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveBorrowBooks(BorrowBookModel model)
        {
            if (ModelState.IsValid)
            {
                // Get UserID from Session
                var userID = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(userID))
                {
                    return Json(new { success = false, message = "User is not logged in." });
                }

                // Assign UserID to the model
                model.UserID = userID;

                // Call the updated database helper to get the result message
                string resultMessage = _dbHelper.SaveBorrowBooks(
                    model.UserID,
                    model.ISBN,
                    model.BorrowDate,
                    model.EstimatedReturnDate
                );

                // Return JSON response based on the result message
                if (resultMessage == "Book borrow record saved successfully!")
                {
                    return Json(new { success = true, message = resultMessage });
                }
                else
                {
                    return Json(new { success = false, message = resultMessage });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpPost]
        public IActionResult SaveReservationBooks(ReservationBooksModel model)
        {
            if (ModelState.IsValid)
            {
                // Get UserID from Session
                var userID = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(userID))
                {
                    return Json(new { success = false, message = "User is not logged in." });
                }

                // Assign UserID to the model
                model.UserID = userID;

                // Call the updated database helper to get the result message
                string resultMessage = _dbHelper.SaveReservationBooks(
                    model.UserID,
                    model.ISBN
                );

                // Return JSON response based on the result message
                if (resultMessage == "Book reserved successfully!")
                {
                    return Json(new { success = true, message = resultMessage });
                }
                else
                {
                    return Json(new { success = false, message = resultMessage });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }


        [HttpGet]
        public IActionResult GetUserList()
        {
            var users = _dbHelper.GetUsers(); // Your DB fetch method
            return Json(users);
        }

        [HttpGet]
        public IActionResult GetBooksList()
        {
            List<AllBookModel> books = _dbHelper.GetBooks();
            return Json(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookList(string category)
        {
            var books = await _dbHelper.GetBooksList(category);
            ViewBag.Category = category.ToUpper();
            ViewData["ModuleName"] = "Books";
            return View("GetBookList", books);
        }

        [HttpGet]
        public async Task<IActionResult> GetEBookList(string category)
        {
            var books = await _dbHelper.GetEBooksList(category);
            ViewBag.Category = category.ToUpper();
            ViewData["ModuleName"] = "E-Books";
            return View("GetEBookList", books);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            var book = await _dbHelper.GetBookByISBN(isbn);

            if (book == null)
                return NotFound();

            return Json(new
            {
                title = book.BookName,
                author = book.AuthorsName,
                isbn = book.ISBN,
                publicationYear = book.Copyright,
                category = book.Category,
                bookshelf = book.Bookshelf,
                stocks = book.Stocks,
                description = book.Description,
                imageUrl = book.BookImage != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(book.BookImage)}" : "/images/default-book.jpg"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowBook()
        {
            List<BorrowBookView> book = await _dbHelper.LoadBorrowBookView();
            return Json(book);
        }

        [HttpPost]
        public IActionResult UpdateBorrowBooks(int id, bool status)
        {
            if (ModelState.IsValid)
            {
                // Get UserID from Session
                var userID = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(userID))
                {
                    return Json(new { success = false, message = "User is not logged in." });
                }

                // Call the updated database helper to get the result message
                string resultMessage = _dbHelper.UpdateBookBorrowStatus(id, status, userID);

                // Return JSON response based on the result message
                if (resultMessage.Contains("successfuly updated"))
                {
                    return Json(new { success = true, message = resultMessage });
                }
                else
                {
                    return Json(new { success = false, message = resultMessage });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }
        [HttpPost]
        public async Task<IActionResult> ApproveBorrow(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest("Invalid request");
            }

            // Fetch email content from the database
            var (email, emailBody) = await _dbHelper.GetBorrowApprovalEmailBody(id);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(emailBody))
            {
                return BadRequest("Failed to fetch email details.");
            }

            // Send email
            bool emailSent = await _emailHelper.SendEmailAsync(email, "Library Book Borrow Approval", emailBody);

            if (!emailSent)
            {
                return StatusCode(500, "Error sending email.");
            }

            return Ok("Borrow request approved and email sent.");
        }
        [HttpGet]
        public async Task<IActionResult> MyShelf()
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
        public async Task<IActionResult> MyReservedBooks()  
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
        public IActionResult AddToShelf(string isbn)
        {
            try
            {
                var userID = HttpContext.Session.GetString("UserID");
                string message = _dbHelper.AddToMyShelf(userID, isbn);
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SaveFines([FromBody] ReturnWithFinesModel data)
        {
            try
            {
                // Create DataTable for TVP
                DataTable dt = new DataTable();
                dt.Columns.Add("UserID", typeof(string));
                dt.Columns.Add("FineType", typeof(string));
                dt.Columns.Add("Amount", typeof(decimal));

                foreach (var fine in data.Fines)
                {
                    dt.Rows.Add(data.UserID, fine.FineType, fine.Amount);
                }

                bool saved = _dbHelper.SaveFinesToDatabase(dt, data.UserID, data.ISBN, data.Id);

                return Json(new { success = saved });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [RequestSizeLimit(209_715_200)]
[RequestFormLimits(MultipartBodyLengthLimit = 209_715_200)]
        [HttpPost]
        public async Task<IActionResult> UploadEbook(IFormFile ebookFile, IFormFile ebookCoverImage, string title, string category, string authors, string description)
        {
            var ebookPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads/ebooks");
            var userID = HttpContext.Session.GetString("UserID");
            if (!Directory.Exists(ebookPath)) Directory.CreateDirectory(ebookPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ebookFile.FileName)}";
            string fullPath = Path.Combine(ebookPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await ebookFile.CopyToAsync(stream);
            }

            string dbFilePath = $"/uploads/ebooks/{fileName}";

            bool success = await _dbHelper.SaveEBookAsync(
                title,
                category,
                authors,
                description,
                dbFilePath,
                ebookCoverImage,
                userID ?? ""
            );

            return success ? Ok("E-Book saved successfully!") : BadRequest("Failed to save e-book.");
        }

        [HttpGet]
        public IActionResult Read(int id)
        {
            var ebook = _dbHelper.GetEBookById(id);
            if (ebook == null || string.IsNullOrEmpty(ebook.EbookFilePath))
            {
                return Content("PDF file not found.");
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ebook.EbookFilePath.TrimStart('/'));

            if (!System.IO.File.Exists(fullPath))
            {
                return Content("PDF file not found.");
            }

            return PhysicalFile(fullPath, "application/pdf");
        }



    }
}

