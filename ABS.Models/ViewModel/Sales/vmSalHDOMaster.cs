using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmSalHDOMaster
    {
        public long LUserID { get; set; }
        public long LCompanyID { get; set; }
        public long LMenuID { get; set; }
        public long LTransactionTypeID { get; set; }

        public long HDOID { get; set; }
        public string HDONo { get; set; }
        public string DeliveredTo { get; set; }

        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string BuyerName { get; set; }
        public string DecUserName { get; set; }

        public long? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime? HDODate { get; set; }
        public long? LCID { get; set; }
        public long? PIID { get; set; }
        public string LCNo { get; set; }
        public string AdoNo { get; set; }
        public decimal? AdoQty { get; set; }
        public string B2BLCNo { get; set; }
        public DateTime? B2BLCDate { get; set; }
        public string UpNo { get; set; }
        public string Beneficiary { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAllApproved { get; set; }
        public string Status { get; set; }
        public int? DODetailCount { get; set; }
    }
}
