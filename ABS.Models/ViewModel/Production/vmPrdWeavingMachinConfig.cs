using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdWeavingMachinConfig
    {
         
        public Int64 MachineConfigID { get; set; }
        public string MachineConfigNo { get; set; }
        public Int64? MachineID { get; set; }
        public int? DepartmentID { get; set; }
        public int? LineID { get; set; }
        public string Remarks { get; set; }
         
        public bool IsBook { get; set; }
         
        public bool IsCorrupted { get; set; }
         
        public bool IsMaintenance { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? BookingDate { get; set; }
         
        public int CompanyID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
         
        public bool IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
    }
}
