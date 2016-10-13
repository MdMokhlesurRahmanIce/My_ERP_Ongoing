using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmFinishingPackingListMasterDetail
    {
        public long? PackingID { get; set; }
        public string PackingNo { get; set; }        
        public DateTime? PackingDate { get; set; }
        public long? PIID { get; set; }
        public string PINO { get; set; }
        //public long? LCID { get; set; }
        public string LCNo { get; set; }
        public string ExportLCNo { get; set; }
        public string CINO { get; set; }
        //public long? BuyerID { get; set; }
        public string BuyerName { get; set; }
        public long? FinishingMRRID { get; set; }
        public string FinishingMRRNo { get; set; }
        public long? ItemID { get; set; }
        public string ArticleNo { get; set; }
        public string Remarks { get; set; }

        public long? PackingDetailID { get; set; }
        public long? QADetailID { get; set; }
        public long? RollNo { get; set; }
        public string Shade { get; set; }
        public decimal? Length { get; set; }
        public decimal? GWeight { get; set; }
        public decimal? NWeight { get; set; }
        public decimal? Shipment { get; set; }
        public decimal? FiniWidth { get; set; }
        public decimal? WidthSr { get; set; }
        public string Description { get; set; }
        public decimal? WPercent { get; set; }
        public int? DefectPointID { get; set; }
        public string DefectPointNo { get; set; }
        public bool IsNotDeliverable { get; set; }
    }
}
