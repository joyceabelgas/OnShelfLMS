using System.ComponentModel.DataAnnotations;

namespace OnShelfGTDL.Models
{
    public class UserModel
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string MemberType { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        [Required]
        public string Address { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        [Required]
        public string Status { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
