using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmFinishingInspactionDetail
    {
        public int? BeamNo { get; set; }
        public long? RollNo { get; set; }
        public long? ItemID { get; set; }
        public string ItemName { get; set; }
        public string QANo { get; set; }
        public decimal? GreigeLength { get; set; }
        public int? Piece { get; set; }
        public int? DefectPoint { get; set; }
        public string DefectPointNo { get; set; }
        public decimal? GrossWt { get; set; }
        public decimal? NetWt { get; set; }
        public string Remarks { get; set; }
        public string ModelState { get; set; }
        public long? InspactionDateilID { get; set; }
        public string ModelStatus { get; set; }
        public long? InspactionID { get; set; }
        public long? SizeMRRID { get; set; }
        public long? FinishingMRRID { get; set; }
        public int? PlateID { get; set; }
        public int? UnitID { get; set; }
        public string PlateNo { get; set; }
        public string SizeBeamNo { get; set; }
        public decimal? FiniWidth { get; set; }
        public int? DoffingNo { get; set; }
        public int? ItemGradeID { get; set; }
        public bool IsNotDeliverable { get; set; }
        public long? SetID { get; set; }
        public string SetNo { get; set; }        
        public string ProDate { get; set; }
        public DateTime? ProductionDate { get; set; }        
        public long? QADetailID { get; set; }
        public long? QAID { get; set; }        
        public decimal? LPercent { get; set; }
        public decimal? WPercent { get; set; }
        public DateTime? QADate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
