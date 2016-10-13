using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmIssueMaster
    {
        public long IssueID { get; set; }
        public string IssueNo { get; set; }
        public System.DateTime IssueDate { get; set; }
        public Nullable<long> IssueBy { get; set; }
        public string IssueByName { get; set; }
        public Nullable<long> RequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> FromDepartmentID { get; set; }
        public string FromDepartment { get; set; }
        public Nullable<int> ToDepartmentID { get; set; }
        public string ToDepartment { get; set; }
        public Nullable<int> FromCompanyID { get; set; }
        public string FromCompany { get; set; }

        public Nullable<int> ToCompanyID { get; set; }
        public string ToCompany { get; set; }

        public string Comments { get; set; }
        public bool IsLoan { get; set; }
       
    }
}
