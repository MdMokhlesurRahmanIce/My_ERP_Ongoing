using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmItemMaster
    {
       public Int64? ItemID { set; get; }
        public Int64? ItemTypeID { set; get; }
        public Int64? ItemGroupID { set; get; }
        public Int64? MachineSetupID { set; get; }

        public string ItemName { set; get; }
        public decimal? Speed { set; get; }
        public decimal? Moiture { set; get; }
        public decimal? KGPreMin { set; get; }
        public string ArticalNo { set; get; }
        public long UnitID { set; get; }
       public string UOMName { set; get; }

        public int RowNum { get; set; }
        public decimal Quantity { get; set; }
        public decimal UOM { get; set; }
        public long BatchID { get; set; }
        public long SupplierID { get; set; }
        public List<vmCmnBatch> Batch { get; set; }
        public List<vmBallInfo> Supplier { get; set; }
    }
}
