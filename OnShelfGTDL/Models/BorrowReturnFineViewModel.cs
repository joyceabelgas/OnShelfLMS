using System.IO;

namespace OnShelfGTDL.Models
{
    public class BorrowReturnFineViewModel
    {
        public int? ReturnID { get; set; }
        public List<FineTypeModel> FineList { get; set; } = new List<FineTypeModel>();
    }
}
