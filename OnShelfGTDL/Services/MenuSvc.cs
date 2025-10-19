using OnShelfGTDL.Interface;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Services
{
    public class MenuSvc : IMenuSvc
    {
        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Manage Users",
                    Url = "/Account/ManageUser",
                    Icon = "/images/manageusers.png",
                },
                new MenuItem
                {
                    Name = "Manage Books",
                    Url = "/Book/ManageBooks",
                    Icon = "/images/managebooks.png",
                    //Submenu = new List<MenuItem>
                    //{
                    //    new MenuItem { Name = "Add Book", Url = "/Books/Add" },
                    //    new MenuItem { Name = "Book Categories", Url = "/Books/Categories" }
                    //}
                },
                new MenuItem { Name = "Borrow Books", Url = "/Book/BorrowBooks", Icon = "/images/borrowbooks.png" },
                new MenuItem { Name = "Reports", Url = "/Reports", Icon = "/images/reports.png" },
                new MenuItem { Name = "Rules and Regulation", Url = "/Reports", Icon = "/images/rulesandregulations.png" },
                new MenuItem { Name = "Dashboard", Url = "/Reports", Icon = "/images/rulesandregulations.png" },
                new MenuItem
                {
                    Name = "Books",
                    Url = "/Book/ManageBooks",
                    Icon = "/images/managebooks.png",
                    Submenu = new List<MenuItem>
                    {
                        new MenuItem { Name = "Science", Url = "/Books/Add" },
                        new MenuItem { Name = "Social Science", Url = "/Books/Categories" },
                        new MenuItem { Name = "Literature", Url = "/Books/Categories" },
                        new MenuItem { Name = "English", Url = "/Books/Categories" },
                        new MenuItem { Name = "Mathematics", Url = "/Books/Categories" },
                        new MenuItem { Name = "Fiction", Url = "/Books/Categories" },
                        new MenuItem { Name = "Mapeh", Url = "/Books/Categories" },
                        new MenuItem { Name = "General Education", Url = "/Books/Categories" },
                        new MenuItem { Name = "PRVE", Url = "/Books/Categories" },
                        new MenuItem { Name = "TLE", Url = "/Books/Categories" },
                        new MenuItem { Name = "Filipino", Url = "/Books/Categories" }
                    }
                },
                new MenuItem { Name = "Borrowed Books", Url = "/Reports", Icon = "/images/rulesandregulations.png" },
                new MenuItem { Name = "Reserved Books", Url = "/Reports", Icon = "/images/rulesandregulations.png" }
            };
        }
    }
}
