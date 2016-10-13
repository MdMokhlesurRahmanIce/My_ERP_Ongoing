using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnUserMgt
    {
        vmAuthenticatedUser Get_CmnUserAuthentication(vmLoginUser model);
        vmRecoverUser Get_CmnUserRecovery(vmRecoverUser model);

        vmUser SaveUser(vmUser model, List<vmCompany> companyList);
        int SaveUserGroup(vmUserGroup model);
        int SaveUserType(vmUserType model);

        IEnumerable<vmUserGroup> GetUserGroup(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmUserType> GetUserType(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmUser> GetUser(vmCmnParameters objcmnParam, out int recordsTotal);
        vmUser GetUserByID(int? id, int? CompanyID, int? LoggedUser);

        int UpdateUserType(vmUserType model);
        int UpdateUserGroup(vmUserGroup model);

        int DeleteUser(int? id, int? CompanyID, int? LoggedUser);
        int DeleteUserGroup(int? id, int? CompanyID, int? LoggedUser);
        int DeleteUserType(int? id, int? CompanyID, int? LoggedUser);

        string getCurrentPassword(int companyID, int loggedUser);

        int ChangePassword(Models.CmnUserAuthentication model);
    }
}
