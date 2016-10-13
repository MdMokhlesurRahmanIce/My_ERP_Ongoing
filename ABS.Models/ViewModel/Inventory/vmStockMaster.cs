using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmStockMaster
    {
        public long SL { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public string Item { get; set; }
        public long UOMID { get; set; }
        public string UOMName { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public long LotNo { get; set; }
        public decimal ReceiveQty { get; set; }
        public decimal IssueQty { get; set; }
        public decimal CurrentStock { get; set; }
        public DateTime LastReceiveDate { get; set; }
        public DateTime LastIssueDate { get; set; } 

    }
}
