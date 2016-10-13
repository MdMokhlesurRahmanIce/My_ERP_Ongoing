using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
   public class vmRequisitionDetails
    {

        public long RequisitionDetailID { get; set; }
        public long RequisitionID { get; set; }
        public long StockTransitID { get; set; }
        public long GroupID { get; set; }
        public string GroupName { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }
        public Nullable<long> LotID { get; set; }
        public string LotNo { get; set; }
        public Nullable<long> BatchID { get; set; }
        public string BatchNo { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string UOMName { get; set; }
        public string MrrNo { get; set; }
        public Nullable<DateTime> MrrDate { get; set; }
        public DateTime RequisitionDate { get; set; }
        public decimal MRRQty { get; set; }       
        public decimal CurrentStock { get; set; }
        public string ArticleNo { get; set; }
        public decimal CurrentRate { get; set; }
        public string ModelState { get; set; }
    }
}
