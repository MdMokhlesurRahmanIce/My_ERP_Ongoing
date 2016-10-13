using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmSelectedItemDataSetSetup
    {
        public long? YarnID { get; set; }
        public long? WeftYarnID { get; set; }
        public string Weave { get; set; }
        public decimal? Length { get; set; }
        public string YarnCount { get; set; }
        public string YarnRatio { get; set; }
        public string YarnRatioLot { get; set; }
        public decimal? TotalEnds { get; set; }
        public long? ColorID { get; set; }
        public string ColorName { get; set; }
        public int? BallNo { get; set; }
    }
}
