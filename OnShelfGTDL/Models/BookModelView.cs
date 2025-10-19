using System.ComponentModel.DataAnnotations;

namespace OnShelfGTDL.Models
{
    public class BookModelView
    {
        public string ISNB { get; set; }

        public string BookName { get; set; }
        public string Category { get; set; }

        public string AuthorsName { get; set; }

        public string BookShelf { get; set; }

        public string Copyright { get; set; }

        public int StockQuantity { get; set; }

        public string PublicationName { get; set; }

        public string Description { get; set; }

        public byte[] BookImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
