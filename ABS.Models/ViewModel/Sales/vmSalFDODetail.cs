using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmSalFDODetail
    {
        //Master        
        public long? MHDOID { get; set; }
        public long? MPartyID { get; set; }
        public string MDeliveryTo { get; set; }
        public string FDONo { get; set; }
        public string MBillNo { get; set; }
        public DateTime? MBillDate { get; set; }
        public string TruckNo { get; set; }
        public string BuyerContactName { get; set; }
        public string BuyerContactPhoneNo { get; set; }
        public string DriverName { get; set; }
        public string DriverPhoneNo { get; set; }
        //Detail 
        public long? HDODetailID { get; set; }
        public int? PIID { get; set; }
        public int? LotId { get; set; }
        public int? BatchId { get; set; }
        public long? ItemGradeID { get; set; }
        public int? ItemID { get; set; }
        public int? PINO { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? RemainingQty { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }

        public decimal? Roll { get; set; }
        public decimal? GrossQuantityKg { get; set; }
        public decimal? QuantityYds { get; set; }
        public decimal? NetQuantityKg { get; set; }
        public long FDOMasterID { get; set; }
        public decimal RollNo { get; set; }
        public decimal QuantitYds { get; set; }
    }
}
