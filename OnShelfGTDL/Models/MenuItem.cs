namespace OnShelfGTDL.Models
{
    public class MenuItem
    {
        public string Name { get; set; }           // Menu name (e.g., Manage Users, Reports)
        public string Icon { get; set; }           // Icon path
        public string Url { get; set; }            // Link URL
        public List<MenuItem>? Submenu { get; set; }  // List of submenus

        //public MenuItem()
        //{
        //    Submenus = new List<MenuItem>();
        //}
    }
}
