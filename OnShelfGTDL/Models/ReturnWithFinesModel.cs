using System.IO;

namespace OnShelfGTDL.Models
{
    public class ReturnWithFinesModel
    {
        public string UserID { get; set; }
        public string ISBN { get; set; }
        public int Id { get; set; }
        public List<FineModel> Fines { get; set; }
    }
}
