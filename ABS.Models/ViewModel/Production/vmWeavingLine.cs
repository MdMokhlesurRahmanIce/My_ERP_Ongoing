using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmWeavingLine
    {
        public Int64 MachineConfigID { get; set; }
        public int LineID { get; set; }
        public Int64 MachineID { set; get; }
        public string MachineConfigNo { get; set; }
        public string LineName { set; get; }
        public string ItemName { get; set; }        
        public string Remarks { get; set; }
        public int DepartmentID { get; set; }
        public string OrganogramName { get; set; }
    }
}
