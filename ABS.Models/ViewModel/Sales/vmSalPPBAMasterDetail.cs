using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmSalPPBAMasterDetail
    {
        public long LUserID { get; set; }
        public long LCompanyID { get; set; }
        public long LMenuID { get; set; }
        public long LTransactionTypeID { get; set; }
        public long TabID { get; set; }

        #region ***********************Starts Adjustment Info Entry***************************
        //Master**********
        public long CompanyID { get; set; }
        public long BuyerID { get; set; }
        public long BillMasterId { get; set; }
        public decimal LIBAdjustmentAmount { get; set; }
        public decimal RestRealizedAmount { get; set; }
        public string PAD { get; set; }
        public Nullable<DateTime> AdjustmentDate { get; set; }
        public decimal RestRealizedAmtPercentage { get; set; }
        public decimal ConversionRateRealized { get; set; }
        public string ERQ { get; set; }        
        public string Remarks { get; set; }

        //Detail**********
        public long BADetailID { get; set; }
        public long BankChargeID { get; set; }
        public decimal ChargeAmount { get; set; }
        public string ComboName { get; set; }
        #endregion***********************End Adjustment Info Entry*****************************
    }
}
