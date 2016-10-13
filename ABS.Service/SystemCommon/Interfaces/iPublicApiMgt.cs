using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iPublicApiMgt
    {
        IEnumerable<vmItem> GetItemMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItem> GetItemMasterDeveloped(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItem> GetFinishedItemMaster(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
