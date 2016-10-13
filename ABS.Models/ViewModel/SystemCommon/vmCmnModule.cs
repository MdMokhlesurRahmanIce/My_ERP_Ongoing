using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmCmnModule
    {
        public int ModuleID { get; set; }
        public string CustomCode { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string ModulePath { get; set; }
        
        public Nullable<int> Sequence { get; set; }
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
    }
}
