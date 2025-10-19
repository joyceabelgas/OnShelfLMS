namespace OnShelfGTDL.Models
{
    public class BorrowedBookWithFinesModel
    {
        public int ID { get; set; }

        public string Name { get; set; } // Concatenated Firstname MiddleName Lastname

        public string ISBN { get; set; }

        public decimal? TotalFines { get; set; } // nullable because SUM() can be null if no fine details

        public string? Status { get; set; } // nullable in case the join does not return Status
    }
}
