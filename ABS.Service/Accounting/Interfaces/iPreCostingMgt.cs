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
    public interface iPreCostingMgt  
    { 
   
        List<vmBOM> GetBomNArticleNo(vmCmnParameters objcmnParam);
        IEnumerable<vmBOM> GetDyingByBomID(vmCmnParameters objcmnParam, Int64 bomID, out int recordsTotal);
        IEnumerable<vmBOM> GetSizingByBomID(vmCmnParameters objcmnParam, Int64 bomID, out int recordsTotal);

        IEnumerable<vmBOM> GetItemDetailByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal);

        IEnumerable<object> GetDetailListByPrCostID(vmCmnParameters objcmnParam, Int64 costingID, out int recordsTotal);   
        IEnumerable<vmBOM> GetFinishingByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal);

        IEnumerable<vmBOM> GetYarnByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal);
        IEnumerable<vmBOM> GetCostMasterList(vmCmnParameters objcmnParam, out int recordsTotal); 


        string SaveNUpdatePreCosting(PrdPreCostingMaster objMaster, List<PrdPreCostingDying> lstDying, List<PrdPreCostingSize> lstSize, List<PrdPreCostingFinishing> lstFinish, List<PrdPreCostingYarn> lstYarn, List<PrdPreCostingDetail> lstDetail, int menuID);

        string DeletePreCosting(vmCmnParameters objcmnParam, Int64 costingID); 
        
    }
}
