namespace OnShelfGTDL.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        public string LRN { get; set; }
        public string? Grade { get; set; }
        public string? Section { get; set; }
        public string? Adviser { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string MemberType { get; set; } // "Student" or "Teacher"
    }
}
