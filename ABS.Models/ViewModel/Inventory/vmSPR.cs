using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmSPR
    {
        public string ArticleNo { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public long UOMID { get; set; }
        public string UOMName { get; set; }         
        public string MrrNo { get; set; }
        public DateTime MrrDate { get; set; }
        public decimal MRRQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal CurrentRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal transitQty { get; set; }
    }
}
