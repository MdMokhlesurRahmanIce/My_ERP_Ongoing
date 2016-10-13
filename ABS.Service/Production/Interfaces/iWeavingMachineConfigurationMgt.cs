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
    public interface iWeavingMachineConfigurationMgt
    {
        string SaveWeavingMachineConfi(PrdWeavingMachinConfig model, vmCmnParameters objcmnParam);
        List<vmWeavingLine> GetWeavingMachineConfigurations(vmCmnParameters objcmnParam, out int recordsTotal);
        //vmWeavingLine GetWMachineConfById(vmCmnParameters objcmnParam);
        string DeleteWeavingMachineConfig(vmCmnParameters objcmnParam);
    }
}
