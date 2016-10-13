using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmCmnOrganogram
    {
        public int OrganogramID { get; set; }
        public string CustomCode { get; set; }
        public string OrganogramName { get; set; }
        public Nullable<int> ParentID { get; set; }

        //Deleted Column
        public Nullable<bool> IsCostCenter { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<int> StatusID { get; set; }
        //Deleted Column

        public Nullable<bool> IsBranch { get; set; }
        public Nullable<bool> IsDepartment { get; set; }
        public string ProcessOutput { get; set; }

        public int CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }

         public string CompanyName { get; set; }
        public string StatusName { get; set; }
        public string ParentName { get; set; }
      
    }
}
