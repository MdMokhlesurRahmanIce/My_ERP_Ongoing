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
    public interface iBillOfMaterialMgt 
    {  
        IEnumerable<vmBOM> GetItemDetailByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal);
        IEnumerable<object> GetDetailListByBOMID(vmCmnParameters objcmnParam, vmCmnParameters objcmnParam1, Int64 bomID, out int recordsTotal); 
        IEnumerable<vmBOM> GetFinishingByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal);
        IEnumerable<vmBOM> GetItemDetailFDying(vmCmnParameters objcmnParam, Int32 itemTypeID, Int32 itemGroupID, out int recordsTotal);
        IEnumerable<vmBOM> GetItemDetailFSizing(vmCmnParameters objcmnParam, Int32 itemTypeID, Int32 itemGroupID, out int recordsTotal);

        IEnumerable<vmBOM> GetBOMMasterList(vmCmnParameters objcmnParam, out int recordsTotal);

        string SaveNUpdateBOM( PrdBOMMaster objPrdBOMMaster,  List<PrdBOMDying> lstPrdBOMDying, List<PrdBOMSize> lstPrdBOMSize, int menuID);

        string DeleteBOM(vmCmnParameters objcmnParam, Int64 bomID);
        
    }
}
