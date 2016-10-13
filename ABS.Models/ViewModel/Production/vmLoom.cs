using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public  class vmLoom
    {
        public Int64? MachineConfigID { get; set; }// this property will get Value Form PrdWeavingMachinConfig table 
        public string MachineName { set; get; }  // this property will get Value Form cmnItemmaster table 
    }
}
