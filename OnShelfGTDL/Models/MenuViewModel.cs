namespace OnShelfGTDL.Models
{
    public class MenuViewModel
    {
        public List<MenuItem> MenuItems { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
