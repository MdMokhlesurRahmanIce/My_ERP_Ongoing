using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Purchase
{
    public class vmComparativeStatement
    {
        public long CSID { get; set; }
        public string CSNo { get; set; }
        public Nullable<System.DateTime> CSDate { get; set; }
        public long QuotationID { get; set; }
        public string QuotationNo { get; set; }

        public decimal UnitPrice { get; set; }
        public Nullable<System.DateTime> QuotationDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<long> RequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> PartyID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string PartyName { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
     
    }
}
