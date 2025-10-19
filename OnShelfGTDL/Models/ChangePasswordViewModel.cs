namespace OnShelfGTDL.Models
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; } // You can set this from session
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
