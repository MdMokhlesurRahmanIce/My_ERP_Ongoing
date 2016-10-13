using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmCmnParameters
    {
        public int loggeduser { get; set; }
        public int loggedCompany { get; set; }
        public int? selectedCompany { get; set; }
        public int menuId { get; set; }
        public int? tTypeId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int? IsPaging { get; set; }
        public int? id { get; set; }
        public bool? IsTrue { get; set; }
        public int? ItemID { get; set; }
        public int? ItemType { get; set; }
        public int? ItemGroup { get; set; }
        public int? DepartmentID { get; set; }
        public int? SelectedDepartmentID { get; set; }
        public string ParamName { get; set; }
        public int? UserType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int SITypeID { set; get; }
        public long? SIID { set; get; } 
        public string serachItemName { get; set; }
    }
}
