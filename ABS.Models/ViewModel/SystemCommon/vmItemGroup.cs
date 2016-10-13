using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmItemGroup
    {
        public int ItemGroupID { get; set; }
        public int companyID { get; set; }
        public string ItemGroupName { get; set; }
        public string Type { set; get; }
        public string Parent { set; get; }
        public string IsActive { set; get; }

        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        public int TypeId { get; set; }
        public string CustomCode { get; set; }
        public string ACName { get; set; }
        public int? AcDetailID { get; set; }

    }
}
