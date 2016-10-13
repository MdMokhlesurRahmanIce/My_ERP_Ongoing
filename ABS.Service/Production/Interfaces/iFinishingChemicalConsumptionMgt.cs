using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iFinishingChemicalConsumptionMgt
    {
        string SaveUpdateChemConsumptionInfo(vmChemicalSetupMasterDetail itemMaster, List<vmChemicalSetupMasterDetail> itemDetails, vmCmnParameters objcmnParam);
        IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionMaster(vmCmnParameters cmnParam, out int recordsTotal);
        string DeleteFiniChemConsumptionMD(vmCmnParameters objcmnParam);
        IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionByID(vmCmnParameters objcmnParam);
    }
}
