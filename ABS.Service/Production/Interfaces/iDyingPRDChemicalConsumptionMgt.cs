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
    public interface iDyingPRDChemicalConsumptionMgt
    {
        int SaveChemicalConsumption(PrdDyingConsumptionMaster master, List<PrdDyingConsumptionDetail> detailsList, UserCommonEntity commonEntity);
        List<vmPrdDyingConsumptionMaster> GetAllConsumption(vmCmnParameters objcmnParam, out int recordsTotal);
        List<vmPrdDyingConsumptionDetail> GetChemicalConsumptionByID(int? companyID, int? loggedUser, int? ConsumptionID);
        IEnumerable<object> GetChemicalConsumptionDetailsByID(int? companyID, int? loggedUser, int? ConsumptionID);
        vmPrdDyingConsumptionMaster GetConsumptionByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ConsumptionID);
        Int64 DeleteConsumption(vmCmnParameters commonEntity);
    }
}
