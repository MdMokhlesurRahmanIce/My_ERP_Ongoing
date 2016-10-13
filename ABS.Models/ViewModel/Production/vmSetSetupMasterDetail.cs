using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmSetSetupMasterDetail
    {
        //Master
        public long? SetMasterID { get; set; }
        public decimal? PIItemlength { get; set; }
        public long? RefSetID { get; set; }
        public DateTime? RefSetDate { get; set; }

        //Detail
        public long? SetID { get; set; }
        public string SetNo { get; set; }
        public long? ItemID { get; set; }
        public string ArticleNo { get; set; }
        public int? ColorID { get; set; }
        public string ColorName { get; set; }
        public long? PIID { get; set; }
        public string PINO { get; set; }
        public decimal? SetLength { get; set; }
        public long? YarnID { get; set; }
        public long? WeftYarnID { get; set; }
        public string Weave { get; set; }
        public string YarnCount { get; set; }
        public string YarnRatio { get; set; }
        public string YarnRatioLot { get; set; }
        public long? SupplierID { get; set; }
        public string SupplierName { get; set; }
        public decimal? TotalEnds { get; set; }
        public decimal? MachineSpeed { get; set; }
        public decimal? EndsPerCreel { get; set; }
        public decimal? LeaseRepeat { get; set; }
        public int? NoOfBall { get; set; }
        public long? BuyerID { get; set; }
        public string BuyerName { get; set; }
        public DateTime? SetDate { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        //-----------For InternalIssue Details By IssueID
        public string IssBallDate { set; get; }
        public string IssBallRemark { get; set; }
        public string ReceivedDyDate { get; set; }
        public string ReceivedDyRemarks { get; set; }
        public string IssDyDate { get; set; }
        public string IssuDyRemarks { get; set; }
        public string ReceivedLCBDate { get; set; }
        public string ReceivedLCBRemarks { get; set; }
        public string ItemName { set; get; }

        public long? IssueID { get; set; }

        public int? StatusID { get; set; }
    }
}
