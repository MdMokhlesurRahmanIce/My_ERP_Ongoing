using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iDefectTypeMgt
    {
        IEnumerable<PrdDefectList> GetDefectTypeInfo(vmCmnParameters cmnParam, out int recordsTotal);
        IEnumerable<PrdDefectType> GetDefectType(vmCmnParameters objcmnParam);
        string SaveUpdateDefectType(PrdDefectList itemMaster, vmCmnParameters objcmnParam);
        string DeleteUpdateDefectList(vmCmnParameters objcmnParam);
    }
}
