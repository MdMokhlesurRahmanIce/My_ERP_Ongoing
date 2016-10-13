using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmHDODetail
    {
        public long HDODetailID { get; set; }        
        public long PIID { get; set; }
        public long ItemID { get; set; }
        public string PINO { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal RemainingQty { get; set; }
        public decimal HoldQty { get; set; }
        public decimal Amount { get; set; }
        public List<vmLot> LotNos { get; set; }
        public List<vmBatch> BatchNos { get; set; }
        public List<vmGrade> GradeList { get; set; }
        
    }
}
