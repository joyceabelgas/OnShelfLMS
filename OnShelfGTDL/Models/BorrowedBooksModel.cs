namespace OnShelfGTDL.Models
{
    public class BorrowedBooksModel
    {
        public string ISNB { get; set; }
        public string BookName { get; set; }
        public string Category { get; set; }
        public string AuthorsName { get; set; }
        public string BookShelf { get; set; }
        public byte[] BookImage { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? OverdueDate { get; set; }
        public string Status { get; set; }
    }
}
