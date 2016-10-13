using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmPrdDyingMachinePart
    {
        public long MachinePartID { get; set; }
        public long MachineID { get; set; }
        public string MachineName { get; set; }
        public string MachinePartNo { get; set; }
        public string MachinePartName { get; set; }
    }
}
