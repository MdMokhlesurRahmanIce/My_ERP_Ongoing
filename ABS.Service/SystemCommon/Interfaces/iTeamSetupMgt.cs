using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iTeamSetupMgt
    {
        int SaveTeam(vCmnUserTeam master, List<vCmnuserTeamDetail> Details, UserCommonEntity commonEntity);
        List<vCmnUserTeam> GetTeam(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vCmnuserTeamDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID);
        int DeleteTeam(vCmnUserTeam master, UserCommonEntity commonEntity);
    }
}
