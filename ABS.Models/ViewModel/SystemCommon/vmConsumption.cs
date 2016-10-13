using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class vmConsumption
    {
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemGroupName { set; get; }
        public string ArticleNo { set; get; }
        public decimal? TotalEnds { get; set; }
        public decimal? NoOfPick { get; set; }    
        public string Consturction { set; get; }
        public string GerigeEPIxPPI { set; get; }
        public string ColorName { get; set; }
        public decimal? FinishingWeigth { get; set; }
        public string LotNo { get; set; }
        public int? BeamRatio { set; get; }
        public string YarnSP { get; set; }
        public decimal? YarnCount { set; get; }
        public string YarnType { set; get; }
        public decimal? Formula { set; get; }
        public decimal? WeightPerUnit { set; get; }

        public long? YarnID { get; set; }
        public long? LotID { get; set; }
        public decimal? NoOFPic { set; get; }

        





    }
}
