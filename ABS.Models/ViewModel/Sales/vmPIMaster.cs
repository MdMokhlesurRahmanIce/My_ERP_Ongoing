using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmPIMaster
    {
        public Int64 PIID { get; set; }
        public string PINO { get; set; }
        public int? TransactionTypeID { get; set; }
        public DateTime PIDate { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int? ShipmentID { get; set; } 
        public int? PayDay { get; set; }
        public long? BookingID { get; set; }
        public string BookingNo { get; set; }
        public int? ValidityID { get; set; }
        public short IncotermID { get; set; }
        public string IncotermName { get; set; }
        public string IncotermDescription { get; set; }
        public int? NegoDay { get; set; }
        public int? SightID { get; set; }
        public int? AcceptanceID { get; set; }
        public decimal? ODInterest { get; set; } 
        public int BuyerID { get; set; }
        public int EmployeeID { get; set; }
        public int? PartyID { get; set; }
        public int? SalesPersionPin { get; set; }
        public bool? IsLcCompleted { get; set; }
        public DateTime? LCCompletedDate { get; set; }
        public bool? IsDOCompleted { get; set; }
        public DateTime? DOCompletedDate { get; set; }
        public int CompanyID { get; set; }
        public string LCStatus { get; set; }
        public string HDOStatus { get; set; }
        public bool IsActive { get; set; }
        public int? StatusID { get; set; }
        public int? StatusBy { get; set; }
        public DateTime? StatusOn { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public int ComboID { get; set; }
        public string ComboName { get; set; }
        public string ComboType { get; set; }
        public string ComboNameShipment { get; set; }
        public string ComboNameValidity { get; set; }
        public string comboNamePIStatus { get; set; }
        public string ComboNameAcceptance { get; set; }
        public string ComboNameSight { get; set; }
        public string CompanyName { get; set; }
        public string BuyerFirstName { get; set; }
        public string SalesPersonFirstName { get; set; }
        public decimal? Discount { get; set; }

        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public string BankName { get; set; }

        public string BankShortName { get; set; }
        public int CompanyIDBankAdvise { get; set; }
        public bool? IsDefaultBankAdvising { get; set; }
        public bool? IsDefaultBankBranch { get; set; } 
        public string CustomCode { get; set; }

    }
}
