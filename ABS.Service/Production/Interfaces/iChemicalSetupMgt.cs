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
   public interface iChemicalSetupMgt
    {
        int SaveChemicalPrepartion(PrdDyingChemicalSetup master, List<PrdDyingChemicalSetupDetail> Details, UserCommonEntity commonEntity);
        List<vmChemicalSetup> GetChemicalSetupList(vmCmnParameters objcmnParam, out int recordsTotal);
        List<vmChemicalSetupDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID);
        string DeleteChemicalPreparationMasterDetail(vmCmnParameters objcmnParam);

    }
}
