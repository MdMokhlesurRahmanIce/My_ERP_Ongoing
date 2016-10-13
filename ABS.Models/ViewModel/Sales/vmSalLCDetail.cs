using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmSalLCDetail
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Remarks { get; set; }
        public string CircularNo { get; set; }
        public string SalesContractNo { get; set; }
        public string IRCNo { get; set; }
        public decimal LCAmount { get; set; }
        public string GarmentsQTY { get; set; }
        public decimal ODInterest { get; set; }
        public int LCMasterBranch { get; set; }
        public int LCMasterBank { get; set; }
        public int LCOpenBranch { get; set; }
        public string LCOpenBranchName { get; set; }
        public string MasterBankName { get; set; }
        public string MasterBranchName { get; set; }
        public string SightName { get; set; }
        public int LCOpenBank { get; set; }
        public string UserFullName { get; set; }
        public string LCOpenBankName { get; set; }
        public string MasterLCNO { get; set; }
        public int DocPrepDays { get; set; }
        public string LCReferenceNo { get; set; }
        public string AmendmentNo { get; set; }
        public string LCNo { get; set; }
        public long LCID { get; set; }
        public string ExportLCNo { get; set; }
        public string MBillNo { get; set; }
        public Nullable<System.DateTime> ExportLCNoDate { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public Nullable<System.DateTime> RefNoDate { get; set; }
        public Nullable<System.DateTime> LCDate { get; set; }
        public Nullable<System.DateTime> AmendmentDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> CircularNoDate { get; set; }
        public long LCDetailID { get; set; }
        public long PIDetailID { get; set; }
        public string PINo { get; set; }
        public string CustomCode { get; set; }
        public long PIID { get; set; }
        public Nullable<System.DateTime> PIDate { get; set; }
        public string ShipmentName { get; set; }
        public decimal TtlAmount { get; set; }
        public int? BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string ReferenceNo { get; set; }
        public string VATRegNo { get; set; }
        public string CentralBankRefNo { get; set; }

        public int? Sight { get; set; }

        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string SerialNo { get; set; }
        public Nullable<bool> IsMasterLC { get; set; }
        public Nullable<long> MasterLCID { get; set; }
        public string OpeningBankAccount { get; set; }
        public Nullable<int> AdvisingBranch { get; set; }
        public Nullable<int> AdvisingBank { get; set; }
        public string HSCode { get; set; }

        public bool IsActive { get; set; }
        public bool IsHDOCompleted { get; set; }
    }
}
