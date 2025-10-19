namespace OnShelfGTDL.Models
{
    public class HomeBookModel
    {
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public byte[] BookImage { get; set; }

        public string BookImageBase64 =>
            BookImage != null ? $"data:image/png;base64,{Convert.ToBase64String(BookImage)}" : null;
    }
}
