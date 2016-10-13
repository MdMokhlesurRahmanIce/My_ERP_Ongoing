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
    public interface iStockInfoMgt 
    {  
       

        IEnumerable<vmStockMaster> GetAllStockItems(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
