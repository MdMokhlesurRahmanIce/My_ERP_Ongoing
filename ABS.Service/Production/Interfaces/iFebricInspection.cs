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
    public interface iFebricInspection
    {
        vmFabricInspection Get_FabricInspectionByStyle(vmCmnParameters objcmnParam);
        string SaveUpdateFebricInspection(PrdFinishingInspactionMaster _objFinishingInspactionMaster, List<vmFinishingInspactionDetail> _objNewInspactionDetails, vmCmnParameters objcmnParam);
        List<vmFabricInspectionMaster> FabricInspectionDetails(vmCmnParameters objcmnParam, out int recordsTotal);
        vmFabricInspectionMaster GetFebricInspectionByInspectionID(vmCmnParameters objcmnParam);
        List<vmFinishingInspactionDetail> GetFebricInspectionDetailsID(vmCmnParameters objcmnParam);
        string DeleteUpdateFabricInspectionMasterDetail(vmCmnParameters objcmnParam);
    }
}
