namespace OnShelfGTDL.Models
{
    public class BorrowReturnViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public int OverDue { get; set; }
    }
}
