namespace OnShelfGTDL.Models
{
    public class BorrowBookView
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime EstimatedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string Status { get; set; } 
        public string? ApprovedBy { get; set; } 
        public DateTime? ApprovedDate { get; set; }
    }
}
