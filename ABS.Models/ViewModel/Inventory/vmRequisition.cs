using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmRequisition
    {
        public long IssueDetailID { get; set; }
        public Nullable<long> RequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public string ItemCode { get; set; }
        public string ManualRequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public long RequisitionTypeID { get; set; }
        public string RequisitionTypeName { get; set; }
        public long RequisitionBy { get; set; }
        public string RequisitionByName { get; set; }
        public long IssueID { get; set; }

        public string IssueNo { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public long UnitID { get; set; }
        public long LotID { get; set; }
        public long BatchID { get; set; }
        public string LotNo { get; set; }
        public string BatchNo { get; set; }
        public long ToCompanyID { get; set; }
        public string ToCompanyName { get; set; }
        public string ToDepartmentName { get; set; }
        public string FromDepartment { get; set; }
        public Nullable<DateTime> EstDate { get; set; }
        public int EstTime { get; set; }
        public bool IsUrgent { get; set; } 
        public long ToDepartmentID { get; set; }

        public long DepartmentID { get; set; }
        public string UOMName { get; set; }
        public Nullable<decimal> RequisitionQty { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> IssuedQty { get; set; }
        public Nullable<decimal> PendingIssueQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public decimal IssueQty { get; set; }

        public decimal CurrentRate { get; set; }
            
        public Nullable<decimal> IssueAmount { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public Nullable<long> CreateBy { get; set; }

        public Nullable<long> CompanyID { get; set; }
        public string OrganogramName { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Purpose { get; set; }


    }
}
