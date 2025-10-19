using System.ComponentModel.DataAnnotations;

namespace OnShelfGTDL.Models
{
    public class BookModel
    {
        [Required]
        public string ISNB { get; set; }

        [Required]
        public string BookName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string AuthorsName { get; set; }

        public string BookShelf { get; set; }

        public string Copyright { get; set; }

        public int StockQuantity { get; set; }

        public string PublicationName { get; set; }

        public string Description { get; set; }
        // Correct type for file upload
        public IFormFile BooksImage { get; set; }
    }
}
