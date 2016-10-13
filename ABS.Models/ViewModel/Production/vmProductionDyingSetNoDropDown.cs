using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmProductionDyingSetNoDropDown
    {
        public long DyingSetID { set; get; }
        public int DyingSetIDDetailID { set; get; }
        public int SetID { set; get; }
        public int PIID { set; get; }
        public int ItemID { set; get; }
        public string ArticleNo { set; get; }
        
        public string DyingPINo { set; get; }
        public string DyingSetNo { set; get; }
        public string BuyerName { set; get; }
        public string SupplierName { set; get; }
    }
}
