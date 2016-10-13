using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnModuleMgt
    {
        IEnumerable<vmCmnModule> GetModules(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging);
        vmCmnModule GetModuleByID(int? id);
        int SaveModule(CmnModule model);
        int UpdateModule(CmnModule model);
        int DeleteModule(int? ModuleID);
        List<CmnCompany> GetCompanyOnDemand();
    }
}
