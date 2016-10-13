using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmQuotation
    {
        public long QuotationDetailID { get; set; }      
        public long QuotationID { get; set; }
        public Nullable<int> TransportTypeID { get; set; }
        public string QuotationNo { get; set; }
        public Nullable<System.DateTime> QuotationDate { get; set; }

        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public Nullable<System.DateTime> SPRDate { get; set; }

        public Nullable<long> RequisitionID { get; set; }
        public string RequisitionNo { get; set; }

        public Nullable<long> ComboID { get; set; }
        public string ComboName { get; set; }

        public Nullable<int> UserID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string UserFullName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Currency { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public int LoadingLocationID { get; set; }
        public int DischargeLocationID { get; set; }

        public string UnitName { get; set; }
      
        public long Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public decimal FreightCharge { get; set; }
        public decimal FOBValue { get; set; }
        public string DischargeLocation { get; set; }
        public string LoadingLocation { get; set; }



    }
}
