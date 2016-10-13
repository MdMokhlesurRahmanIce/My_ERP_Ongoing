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
   public interface iDyingOperationSetupMgt
    {
        int SaveOperationSetup( List<PrdDyingOperationSetup> masterList, UserCommonEntity commonEntity);
        IEnumerable<vmPrdDyingOperationSetup> GetAllOperationSetup(vmCmnParameters objcmnParam, out int recordsTotal);
        string DeleteChemicalOperation(vmCmnParameters objcmnParam);
    }
}
