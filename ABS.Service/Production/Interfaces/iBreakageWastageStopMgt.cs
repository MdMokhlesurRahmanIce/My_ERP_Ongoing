using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iBreakageWastageStopMgt
    {
        IEnumerable<PrdBWSlist> GetBWSInfo(vmCmnParameters cmnParam, out int recordsTotal);
        string SaveUpdateBWS(PrdBWSlist itemMaster, vmCmnParameters cmnParam);
        string DeleteUpdatePrdBWSlist(vmCmnParameters objcmnParam);
    }
}
