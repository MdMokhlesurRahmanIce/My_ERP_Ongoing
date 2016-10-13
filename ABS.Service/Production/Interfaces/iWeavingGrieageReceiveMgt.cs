using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
   public interface iWeavingGrieageReceiveMgt
    {
       vmWeavingGriage GetWeavingMachines(vmCmnParameters objcmnParam);
       int SaveWeavingGriage(PrdWeavingMRRMaster model, vmCmnParameters objcmnParam);
       List<vmWeavingGriage> WeavingGriageDetails(vmCmnParameters objcmnParam, out int recordsTotal);
       vmWeavingGriage GetWeavingGriageDetailsById(vmCmnParameters objcmnParam);
       int DeleteWeavingGriageById(vmCmnParameters objcmnParam);




       
    }
}
