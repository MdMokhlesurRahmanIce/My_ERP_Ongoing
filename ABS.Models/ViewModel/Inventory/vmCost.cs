using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Models.ViewModel.Inventory
{
    public class vmCost
    {


        public Nullable<long> AccessmentCostID { get; set; }
        public string AccessmentNo { get; set; }
        public Nullable<System.DateTime> AccessmentDate { get; set; }
        public Nullable<long> POID { get; set; }
        public Nullable<long> RequisitionID { get; set; }
        public Nullable<long> CurrencyID { get; set; }
        public string CurrencyName { get; set; }

        public Nullable<long> CustomOfficeID { get; set; }
        public Nullable<long> CountryID { get; set; }
        public string CountryName { get; set; }

        public string AccessmentRefNo { get; set; }
        public Nullable<System.DateTime> AccessmentRefDate { get; set; }

        public string CustomRefNo { get; set; }
        public string DeclarantRefNo { get; set; }


        public Nullable<decimal> BondAmount { get; set; }
        public Nullable<decimal> BondDue { get; set; }

        public Nullable<decimal> SROAmount { get; set; }

        public Nullable<decimal> SRODue { get; set; }
        public string AccessmentDescription { get; set; }
        public string DocUrl { get; set; }


        public string RequisitionNo { get; set; }
        public string PONo { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<System.DateTime> RequisitionDate { get; set; }
        public string LCorVoucherorLcafNo { get; set; }


        public Nullable<long> AccessmentCostDetailID { get; set; }
        public Nullable<int> TaxID { get; set; }
        public Nullable<decimal> TaxValue { get; set; }
        public string TaxName { get; set; }


        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }


        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }


        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public Nullable<bool> IsDeleted { get; set; }


        //------------------ Clearing Cost -------------------------------


        public string ClearingNo { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<long> ClearingCostID { get; set; }

        public Nullable<long> ClearingCostDetailID { get; set; }
        public string ReceiptNo { get; set; }


        public Nullable<int> ConsumerChargeTypeID { get; set; }
        public Nullable<decimal> Amount { get; set; }


    }
}
