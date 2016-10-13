using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmInternalIssue
    {
       public Int64? IssueID { set; get; }
       public int TransactionTypeID { set; get; }
       public Int64 ItemID { set; get; }
       public Int64? SetID { get; set; }
       public int DepartmentID { get; set; }
       
       public int CompanyID { set; get; }   
       public int CreateBy { get; set; }
       public DateTime? IssBallDate { set; get; }
       public bool? IsIssuedBall { get; set; }
       public string IssBallRemarks { set; get; }
       public DateTime? ReceivedDyDate { set; get; }
       public bool? IsReceivedDy { get; set; }
       public string ReceivedDyRemarks { set; get; }
       public DateTime? IssDyDate { set; get; }
       public bool? IsIssuedDy { get; set; }
       public string IssDyRemarks { set; get; }
       public DateTime? ReceivedLCBDate { set; get; }
       public bool? IsReceivedLCB { get; set; }
       public string ReceivedLCBRemarks { set; get; }
       public Int64? BalMRRID { set; get; }

      
    }
}
