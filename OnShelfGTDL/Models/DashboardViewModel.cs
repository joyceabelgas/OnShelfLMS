namespace OnShelfGTDL.Models
{
    public class DashboardViewModel
    {
        public DashboardSummary Summary { get; set; }
        public List<UserListModel> Users { get; set; }
        public List<BookListsModel> Books { get; set; }
    }
}
