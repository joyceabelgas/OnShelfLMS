namespace OnShelfGTDL.Models
{
    public class HomeViewModel
    {
        public List<HomeBookModel> MostBorrowed { get; set; } = new List<HomeBookModel>();
        public List<HomeBookModel> TodaysPick { get; set; } = new List<HomeBookModel>();
    }
}
