using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Interface;
using OnShelfGTDL.Models;
using Microsoft.AspNetCore.Hosting;

namespace OnShelfGTDL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMenuSvc _menuService;
        private readonly DatabaseHelper _dbHelper;
        private readonly IWebHostEnvironment _env;
        private readonly EmailHelper _emailHelper;
        public AccountController(IMenuSvc menuSvc, DatabaseHelper dbHelper, IWebHostEnvironment env, EmailHelper emailHelper)
        {
            _menuService = menuSvc;
            _dbHelper = dbHelper;
            _env = env;
            _emailHelper = emailHelper;

        }
        public IActionResult Login()
        {

            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (!string.IsNullOrEmpty(userID))
            {
                if (role == "Administrator")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else if (role == "Student" || role == "Teacher")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Optional fallback if role is unknown
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }


        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new { success = false, message = "Invalid input." });
            }

            var user = await _dbHelper.LoginUser(model.Username, model.Password);

            if (user != null && user.Message == "Account exists!")
            {
                HttpContext.Session.SetString("UserID", user.UserID);
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetString("Role", user.Role);

                return Json(new { success = true, role = user.Role });
            }

            return Json(new { success = false, message = user.Message });
        }


        [HttpGet]
        [Route("Account/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Login", "Account");
        }

        //[HttpPost]
        //[Route("Account/Logout")]
        //[Route("Logout")]
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    Response.Cookies.Delete(".AspNetCore.Cookies");
        //    return RedirectToAction("Login", "Account");
        //}

        [HttpPost]
        [Route("Account/SaveUserInformation")]
        public IActionResult SaveUserInformation(UserModel model)
        {
            //if (ModelState.IsValid)
            if (ModelState.Count > 0)
            {
                string userId = HttpContext.Session.GetString("UserID");

                // Pass data to your SaveUser method (updated to include picture data)
                bool isSaved = _dbHelper.SaveUser(
                    model.UserId,
                    model.MemberType,
                    model.FirstName,
                    model.MiddleName,
                    model.LastName,
                    model.Suffix,
                    model.Address,
                    model.EmailAddress,
                    model.MobileNumber,
                    model.Status,
                    userId);

                if (isSaved)
                {
                    return Json(new { success = true, message = "User saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save user." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet]
        [Route("Account/ManageTeacher")]
        public IActionResult ManageTeacher()
        {
            ViewData["ModuleName"] = "Manage Teacher";
            ViewData["UserID"] = HttpContext.Session.GetString("UserID");
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            List<UserViewModel> users = _dbHelper.GetAllUsers();
            return View(users);

        }

        [HttpGet]
        [Route("Account/ManageStudent")]
        public IActionResult ManageStudent()
        {
            ViewData["ModuleName"] = "Manage Student";
            ViewData["UserID"] = HttpContext.Session.GetString("UserID");
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            List<UserViewModel> users = _dbHelper.GetAllUsers();
            return View(users);

        }

        [HttpGet]
        [Route("Account/GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _dbHelper.GetAllUsers();
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        [HttpGet]
        [Route("Account/GetAllTeachers")]
        public IActionResult GetAllTeachers()
        {
            try
            {
                var users = _dbHelper.GetAllTeachers();
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [Route("Account/UpdateUserInformation")]
        public IActionResult UpdateUserInformation(UserModel model)
        {
            //if (ModelState.IsValid)
            if (ModelState.Count > 0)
            {

                // Pass data to your SaveUser method (updated to include picture data)
                bool isSaved = _dbHelper.UpdateUser(
                    model.UserId,
                    model.MemberType,
                    model.FirstName,
                    model.MiddleName,
                    model.LastName,
                    model.Suffix,
                    model.Address,
                    model.EmailAddress,
                    model.MobileNumber,
                    model.Status);

                if (isSaved)
                {
                    return Json(new { success = true, message = "User saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save user." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        public IActionResult Profile()
        {
            ViewData["ModuleName"] = "Profile";
            return View();
        }
        [HttpGet]
        public IActionResult GetProfileData()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return BadRequest("User ID not found in session.");
            }

            var user_info = _dbHelper.GetUserById(userID);

            if (user_info == null)
            {
                return NotFound("User data not found.");
            }

            return Json(user_info);
        }

      
        [HttpPost]
        public IActionResult Profile(UserInformationViewModel model, IFormFile profileImage)
        {
            if (model == null)
            {
                return BadRequest("Model is null.");
            }
            var userID = HttpContext.Session.GetString("UserID");
            model.UserID = userID;
            var success = _dbHelper.UpdateUserProfile(model, profileImage);
            return success ? Json(new { success = true }) : BadRequest("Failed to update.");
        }

        [HttpDelete]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                bool isDeleted = _dbHelper.DeleteUser(id);
                if (isDeleted)
                    return Ok();
                else
                    return BadRequest("User not found or could not be deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult UpdateUserStatus([FromBody] UserStatusUpdateModel model)
        {
            try
            {
                bool updated = _dbHelper.UpdateUserStatus(model.UserId, model.Status);
                if (updated)
                    return Ok();
                else
                    return BadRequest("Failed to update user status.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpPost]
        public IActionResult ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            var userId = HttpContext.Session.GetString("UserID");

            if (NewPassword != ConfirmPassword)
            {
                return Json(new { success = false, message = "New password and confirmation do not match." });
            }

            var currentPassFromDb = _dbHelper.GetCurrentPassword(userId); 

            if (CurrentPassword != currentPassFromDb)
            {
                return Json(new { success = false, message = "Current password is incorrect." });
            }

            bool result = _dbHelper.ChangePassword(userId, NewPassword);

            if (result)
                return Json(new { success = true, message = "Password changed successfully." });
            else
                return Json(new { success = false, message = "Failed to change password. Please try again." });
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            if (model.Password != model.ConfirmPassword)
            {
                return Json(new { success = false, message = "Passwords do not match." });
            }

            bool result = _dbHelper.RegisterUser(model, out string message);

            return Json(new { success = result, message });
        }
        [HttpPost]
        public async Task<IActionResult> OTPSend(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return BadRequest("Invalid request");
            }

            // Fetch email content from the database
            var (email, emailBody) = await _dbHelper.GetRegistrationOTPEmailBody(userID);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(emailBody))
            {
                return BadRequest("Failed to fetch email details.");
            }

            // Send email
            bool emailSent = await _emailHelper.SendEmailAsync(email, "Verify Your Email Address", emailBody);

            if (!emailSent)
            {
                return StatusCode(500, "Error sending email.");
            }

            return Ok("Borrow request approved and email sent.");
        }
        public IActionResult Otp(string email)
        {
            ViewBag.Email = email;
            return View();
        }
        [HttpPost]
        public IActionResult VerifyOtp(string email, string otp)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                return Json(new { success = false, message = "Email and OTP are required." });
            }

            bool isVerified = _dbHelper.VerifyOtp(email, otp, out string message);

            return Json(new
            {
                success = isVerified,
                message = message
            });
        }
        // POST: /Account/ResendOtp
        [HttpPost]
        public async Task<IActionResult> ResendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email is required." });
            }

            bool isResent = _dbHelper.ResendOtp(email);

            var (emailAdd, emailBody) = await _dbHelper.GetRegistrationOTPEmailBodyByEmail(email);

            // Send email
            bool emailSent = await _emailHelper.SendEmailAsync(email, "Verify Your Email Address", emailBody);

            if (!emailSent)
            {
                return StatusCode(500, "Error sending email.");
            }

            return Json(new
            {
                success = isResent,
                message = "Success"
            });
        }

    }
}
