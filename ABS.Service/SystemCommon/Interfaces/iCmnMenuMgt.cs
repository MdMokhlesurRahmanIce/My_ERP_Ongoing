using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnMenuMgt
    {
        IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging);
        vmCmnMenu GetMenuByID(int? id);
        int SaveMenu(CmnMenu model);
        int UpdateMenu(CmnMenu model);
        int DeleteMenu(int? MenuID);
        List<CmnCompany> GetCompanyOnDemand();
        List<CmnModule> GetModuleOnDemand();
        
    }
}
