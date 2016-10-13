using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdWeavingMachineConfigMasterDetail
    {
        public long? MachineConfigID { get; set; }
        public string MachineConfigNo { get; set; }
        public int? DepartmentID { get; set; }
        public string Remarks { get; set; }

        public long? MachineConfigDetailID { get; set; }
        public string Description { get; set; }
        public long? MachineID { get; set; }
        public long? recordsTotal { get; set; }
        public string OrganogramName { get; set; }

        //vmMntMachineMaintenanceOrder
        public long? MaintenanceID { get; set; }
        public string MaintenanceNo { get; set; }
        public int? EmployeeID { get; set; }
        public string Reason { get; set; }
        public bool IsMaintenance { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public bool IsReleased { get; set; }
        public int? MaintenanceEmployeeID { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseRemarks { get; set; }
    }
}
