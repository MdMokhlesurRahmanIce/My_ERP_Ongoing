using ABS.Models;
using ABS.Models.ViewModel.Accounting;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Accounting.Interfaces
{
    public interface iFinishingProcessPriceMgt
    { 
        List<PrdFinishingProcess> GetFinishingProcess(vmCmnParameters objcmnParam); 
        IEnumerable<vmFinishingProcessPriceSetup> GetFinPricChngeGrdByFProcessID(vmCmnParameters objcmnParam, Int32 finishProcessID, out int recordsTotal);
 
        IEnumerable<vmFinishingProcessPriceSetup> GetFPPMasterList(vmCmnParameters objcmnParam, out int recordsTotal); 
 
        string SaveFinishProPricSetup(prdFinishingProcessPriceSetup objPrdFinishingProcessPriceSetup, int menuID);
        
    }
}
