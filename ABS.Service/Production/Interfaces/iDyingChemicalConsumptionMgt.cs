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
    public interface iDyingChemicalConsumptionMgt
    {
        dynamic GetMachineSetupDetails(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MasterID, int? DetailsID);
        List<vmPrdDyingOperationSetup> GetOperationSetup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? CompanyID);
        int SaveMRRSet(List<vmProductionPrdSetSetupDDL> masterList,UserCommonEntity commonEntity);
        int SaveMRR(vmPrdDyingMRRMaster master, List<vmPrdDyingMRRDetail> details, UserCommonEntity commonEntity);
        vmPrdDyingMRRMaster GetProcessByID(int? companyID, int? loggedUser,int? DyingMRRID);
        List<vmPrdDyingMRRMaster> GetAllProcess(vmCmnParameters objcmnParam, out int recordsTotal);
        //List<vmPrdDyingMRRMaster> GetAllProcess(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmPrdDyingMRRDetail> GetProcessDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DyingMRRID);
        string DeleteChemicalProcessMasterDetail(vmCmnParameters objcmnParam);
    }
}
