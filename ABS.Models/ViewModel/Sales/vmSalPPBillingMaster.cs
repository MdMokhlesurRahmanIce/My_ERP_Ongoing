using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmSalPPBillingMaster
    {
        public long LUserID { get; set; }
        public long LCompanyID { get; set; }
        public long LMenuID { get; set; }
        public long LTransactionTypeID { get; set; }
        public long TabID { get; set; }

        #region ***********************Starts Documents Info Entry***************************
        public long CompanyID { get; set; }
        public long BuyerID { get; set; }
        public long LCID { get; set; }
        public decimal DeliveryQty { get; set; }
        public string DocumentsNo { get; set; }        
        public Nullable<DateTime> ShipmentDate { get; set; }
        public Nullable<DateTime> DocsSentDateParty { get; set; }
        public long BankID { get; set; }
        public Nullable<DateTime> SubmissionDatePartyBank { get; set; }
        public long ComboID { get; set; }
        public string RefNo { get; set; }
        public decimal DocumentValue { get; set; }
        public Nullable<DateTime> DocumentsDate { get; set; }
        public Nullable<DateTime> DocsRecieveDate { get; set; }
        public Nullable<DateTime> PartyAcceptanceDate { get; set; }
        public string RefBillNo { get; set; }
        public Nullable<DateTime> BankAcceptanceDate { get; set; }
        #endregion***********************End Documents Info Entry*****************************

        #region ***********************Starts Purchase Info Entry*****************************
        public long BillMasterId { get; set; }
        public decimal LIB { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal SugarPAD { get; set; }
        public decimal ReserveMargin { get; set; }        
        public Nullable<DateTime> PurchaseDate { get; set; }
        public decimal Discount { get; set; }
        public decimal Percentage { get; set; }
        public decimal LIBRateOfInterest { get; set; }          
        #endregion***********************End Purchase Info Entry******************************

        #region ***********************Starts Over Due Info Entry*****************************        
        public Nullable<DateTime> PaymentIssueDate { get; set; }
        public Nullable<DateTime> AdjustmentDate { get; set; }
        public long TotalODDaysParty { get; set; }
        public decimal TotalODInterestParty { get; set; }
        public decimal ODAdjustment { get; set; }
        public Nullable<DateTime> MaturityDate { get; set; }
        public decimal PaymentValue { get; set; }
        public decimal Shortfall { get; set; }        
        public Nullable<DateTime> PaymentRecievedDate { get; set; }
        public long TotalODDaysBank { get; set; }
        public decimal TotalODInterestBank { get; set; }
        #endregion***********************End Over Due Info Entry******************************
    }
}
