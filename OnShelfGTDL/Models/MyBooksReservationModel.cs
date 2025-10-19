namespace OnShelfGTDL.Models
{
    public class MyBooksReservationModel
    {
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string Category { get; set; }
        public string AuthorsName { get; set; }
        public string BookShelf { get; set; }
        public byte[] BookImage { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
