namespace OnShelfGTDL.Models
{
    public class BorrowBookModel
    {

        // User Information
        public string UserID { get; set; }

        // Book Information
        public string ISBN { get; set; }
        // Borrow Information
        public DateTime BorrowDate { get; set; }
        public DateTime EstimatedReturnDate { get; set; }


    }
}
