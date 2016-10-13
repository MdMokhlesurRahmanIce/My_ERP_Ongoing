using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmDepartment
    {
        public int? OrganogramID { get; set; }
        public string CustomCode { get; set; }
        public string OrganogramName { get; set; }


        public int? ID { get; set; }
        public string Name { get; set; } 
        public List<vmDepartment> Children { set; get; } 
        public int? ParentID { get; set; }  
        public bool? collapsed {get; set;} 

    }
}
