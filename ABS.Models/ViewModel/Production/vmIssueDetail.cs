using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmIssueDetail
    {
        public Int64 SetID { get; set; }
        public string SetNo { get; set; }
        public string Count { get; set; }
        public string YarnLot { set; get; }
        public decimal? TotalENDs { get; set; }
        public string McNo { set; get; }
        public string LOGNO { get; set; }
        public string OperatorName { set; get; }
        public decimal? RopeNumber { set; get; }
        public int? CanID { set; get; }
        public int? BallID { set; get; }
        public string ArticalNo { get; set; }
        public string IssBallDate { set; get; }
        public string ReceivedDyDate { set; get; }
        public string IssDyDate { set; get; }
        public string ReceivedLCBDate { set; get; }
        public Int64? IssueID { set; get; }
        public string IssueNo { set; get; }
        public Int64? BalMRRID { get; set; }
        public int? OperatorID { get; set; }
        public Int64? IssueDetailID { set; get; }
        public Int64? MachineID { set; get; }
        public int CompanyID { set; get; }

        public string IsIssuedBall { set; get; }
        public string IsReceivedDy { set; get; }
        public string IsIssuedDy { set; get; }
        public string IsReceivedLCB { set; get; }


    }
}
