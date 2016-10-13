using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmBookingDetail
    {
        public long BookingDetailID { get; set; }
        public string PINO { get; set; }
        public string BuyerStyle { get; set; }
        public decimal? CuttableWidth { get; set; }
        public decimal? Quantity { get; set; }
        public long BookingID { get; set; }
        public long? ItemID { get; set; }
        public string Description { get; set; }
        public DateTime? DeliveryStartDate { get; set; }
        public DateTime? DeliveryFinishDate { get; set; }

        public string ArticleNo { get; set; }
        public string ItemName { get; set; }     
        public int CompanyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
