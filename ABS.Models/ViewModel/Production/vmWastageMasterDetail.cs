using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmWastageMasterDetail
    {
        //Master
        public long? WastageID { get; set; }
        public long? DepartmentID { get; set; }
        public string WastageNo { get; set; }
        public DateTime? WastageDate { get; set; }
        public string Remarks { get; set; }
        //Detail
        public long? WastageDetailID { get; set; }
        public int? UnitID { get; set; }
        public string UOMName { get; set; }
        public long? ItemID { get; set; }
        public decimal? Qty { get; set; }

        public string ModelState { get; set; }
    }
}
