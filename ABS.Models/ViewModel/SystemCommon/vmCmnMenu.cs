using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmCmnMenu
    {
        public int MenuID { get; set; }
        public string CustomCode { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string MenuPath { get; set; }

        public string ParentMenuPath { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> Sequence { get; set; }
        public Nullable<int> MenuTypeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
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
        public string StatusName { get; set; }
        public string MenuTypeName { get; set; }
        public string ParentMenuName { get; set; }
        public string MenuIconCss { get; set; }
        public string ParentMenuIconCss { get; set; }


    }

    public class vmBreadCrums
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }

    }

    public class vmApplicationTokenModel
    {
        public string MenuPath { get; set; }
        public int loggedCompanyID { get; set; }
        public int loggedUserID { get; set; }

    }


}
