using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iCodewiseMachineSetupMgt
    {   
        IEnumerable<PrdWeavingMachineSetup> GetMachineSetupList(vmCmnParameters cmnParam, out int recordsTotal);
        //IEnumerable<Models.ViewModel.Production.vmWeavingMachineSetup> GetMachineSetupInfo(vmCmnParameters objcmnParam);
        string SaveUpdateCodewiseMachineSetup(PrdWeavingMachineSetup CodewiseMachineSetupInfo, vmCmnParameters objcmnParam);
        string DeleteUpdateWeavingMachineSetup(vmCmnParameters objcmnParam);
    }
}
