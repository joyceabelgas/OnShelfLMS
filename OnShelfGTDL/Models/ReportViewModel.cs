namespace OnShelfGTDL.Models
{
    public class ReportViewModel
    {

        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Borrower { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public int? DaysOverdue { get; set; }
        public decimal? Fine { get; set; }
        public DateTime? DateReturned { get; set; }
        public string Status { get; set; }
    }
}
