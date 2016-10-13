using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class vmWORKTRANSACTIONTEMP
    {
         
        public Int64 RecordID { get; set; }
        public string CustomCode { get; set; }
        public Int64? MenuID { get; set; }
        public Int64? WFDID { get; set; }
        public Int64? WFMID { get; set; }
        public Int64? TransactionID { get; set; }
        public DateTime? TransDate { get; set; }
        public int? StatusID { get; set; }
        public Int64? TarTeamID { get; set; }
        public Int64? TUserID { get; set; }
        public Int64? UserID { get; set; }
        public Int64? TeamID { get; set; }
        public int? EntrySequence { get; set; }
        public int? Sequence { get; set; }
        public int? BeginEndTrac { get; set; }
        public bool? IsTeam { get; set; }
        public Int64? AppDECBy { get; set; }
        public bool? IsDeclied { get; set; }
        public bool? IsDeclinedDisable { get; set; }
        public bool? IsApprove { get; set; }
        public string Bcomment { get; set; }
        public string Ccomment { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
        public Int64? OrganogramID { get; set; }
        public Int64? CompanyID { get; set; }
        public string Message { get; set; }
        public DateTime? MessageDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
         
        public bool IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
         
        public bool Transfer { get; set; }
    }
}
