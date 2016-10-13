using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmGrr  
    {
        public long? GrrID { get; set; }
        public string GrrNo { get; set; }
        public Nullable<long> MrrQcID { get; set; }
        public Nullable<System.DateTime> GrrDate { get; set; }
        public long? MrrID { get; set; }
        public string MrrNo { get; set; }
        public Nullable<System.DateTime> MrrDate { get; set; } 

        public string Remarks { get; set; }
        public string Description { get; set; }
        public int? SupplierID { get; set; }
        public string ChallanNo { get; set; }
        public long? RequisitionID { get; set; }
        public string SprNo { get; set; }
        public long? PIID { get; set; }
        public long? CHID { get; set; }
        public long? POID { get; set; }
        public string PONO { get; set; } 
        public int? CurrencyID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> StatusBy { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public bool? IsMrrCompleted { get; set; }

        public bool? IsAccountsCompleted { get; set; }
        public int? CompanyID { get; set; }
        public int? FromCompanyID { get; set; }
        public string FromCompanyName { get; set; } 
        public int? DepartmentID { get; set; }
        public Nullable<long> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool? IsDeleted { get; set; }
        public Nullable<long> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public string SupplierName { get; set; }

        public string InvoiceNo { get; set; }

        public string PINO { get; set; }   
        public Nullable<System.DateTime> CHDate { get; set; } 
        public Nullable<System.DateTime> PIDate { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public Nullable<System.DateTime> SPRDate { get; set; }

        public long? DCID { get; set; }

        public int? MrrTypeID { get; set; }

        public int? TypeID { get; set; } 
        public int? ComboID { get; set; } 
 
        public string ComboName { get; set; }

        public long? IssueID { get; set; }
        public int? FromDepartmentID { get; set; } 
        public string IssueNo { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public string OrganogramName { get; set; }

        public string DepartmentName { get; set; } 
        public string CurrencyName { get; set; }

        public string ManualMRRNo { get; set; }
        public string MRRCertificateNo { get; set; } 
        public string QCCertificateNo { get; set; } 
        public string MrrQcNo { get; set; }

        public string LCNO { get; set; }
        public string RefCHNo { get; set; }
        public string UserFullName { get; set; } 
        public Nullable<System.DateTime> MrrQcDate { get; set; } 
        public int? UserID { get; set; }

        public int? RecordTotal { get; set; }

        //   For opening stock

        //public int? ItemID {get; set;}
        //public string ItemName {get; set;}
        //public string ItemCode {get; set;}
        //public decimal? CurrentRate {get; set;}
        //public decimal? CurrentStock {get; set;}
        //public decimal? CurrentValue {get; set;}
        //public decimal? ReceiveQty {get; set;}
        //public decimal? ReceiveValue {get; set;}
        //public Nullable<System.DateTime> LastReceiveDate { get; set; }
         
         
    }
}
