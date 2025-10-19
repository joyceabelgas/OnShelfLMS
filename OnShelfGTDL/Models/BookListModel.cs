namespace OnShelfGTDL.Models
{
    public class BookListModel
    {
        public string ISBN { get; set; }

        public string BookName { get; set; }
        public string AuthorsName { get; set; }
        public string Copyright { get; set; }
        public string Category { get; set; }
        public string Bookshelf { get; set; }
        public int Stocks { get; set; }
        public string Description { get; set; }
        public byte[] BookImage { get; set; }
    }
}
