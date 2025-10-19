namespace OnShelfGTDL.Common
{
    public class CommonMenuItems
    {
        public string Name { get; set; }           // Menu name (e.g., Manage Users, Reports)
        public string Icon { get; set; }           // Icon path
        public string Url { get; set; }            // Link URL
        public List<CommonMenuItems> Submenus { get; set; }  // List of submenus

        public CommonMenuItems()
        {
            Submenus = new List<CommonMenuItems>();
        }
    }
}
