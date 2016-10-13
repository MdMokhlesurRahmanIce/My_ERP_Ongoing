using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnModulePermissionMgt
    {
        int SaveModulePermission(CmnModulePermission model);
        List<vmModulePermission> GetAllModulePermission(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        int DeletePermission(int? ModuleID);
    }
}
