namespace OnShelfGTDL.Models
{
    public class EBook
    {
        public int EbookID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Authors { get; set; }
        public string? Description { get; set; }
        public string EbookFilePath { get; set; } = string.Empty;

        // 🧠 Image stored in database
        public byte[]? CoverImage { get; set; }
        public string? CoverImageType { get; set; }

        public DateTime? DateUploaded { get; set; } = DateTime.Now;
        public string? UploadedBy { get; set; }
    }
}
