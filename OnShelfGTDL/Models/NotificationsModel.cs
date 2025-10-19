namespace OnShelfGTDL.Models
{
    public class NotificationsModel
    {
        public int? ID { get; set; }
        public string? UserID { get; set; }
        public string? Module { get; set; }
        public string? Method { get; set; }
        public string? Action { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? Date { get; set; } // Add this
    }
}
