using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iOutputUnitMgt
    {
        string SaveUpdateOutputUnit(PrdOutputUnit itemMaster, vmCmnParameters objcmnParam);
        IEnumerable<PrdOutputUnit> GetOutputUnitInfo(vmCmnParameters cmnParam, out int recordsTotal);
        IEnumerable<CmnOrganogram> GetOutputName(vmCmnParameters objcmnParam);
        string DeleteUpdateOutPutList(vmCmnParameters objcmnParam);
    }
}
