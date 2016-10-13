using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Models.ViewModel.Production
{
    public class vmSetMaster
    {
        public long SetMasterID { get; set; }  
        public string PINo { get; set; }
        public string ArticleNo { get; set; }
        public decimal PIItemlength { get; set; }
        public decimal NoOfBall { get; set; }
        public string SupplierName { get; set; }
        public string BuyerName { get; set; }
        public string RefSetNo { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime RefSetDate { get; set; }
        public string Remarks { get; set; }   
    
    }
}
