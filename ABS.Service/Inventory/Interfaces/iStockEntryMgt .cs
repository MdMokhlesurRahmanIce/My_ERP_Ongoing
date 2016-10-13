using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Service.Inventory.Interfaces
{
    public interface iStockEntryMgt 
    {  
        IEnumerable<CmnItemMaster> GetFinishItemDescription(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging);
        int SaveStockEntry(InvStockMaster model);
        IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmStockMaster> GetItemList(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
