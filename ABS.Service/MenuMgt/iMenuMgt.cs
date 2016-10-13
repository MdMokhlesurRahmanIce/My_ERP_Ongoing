using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.MenuMgt
{
    public interface iMenuMgt
    {
        object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID);
        IEnumerable<vmNotification> GetNotificationInfo(int? companyID, int? loggedUser, int? ModuleID);
        List<NotificationEntity> GetNotificationInfoes(int? companyID, int? loggedUser, int? ModuleID);
        List<vmBreadCrums> GetBreadCrums(int? companyID);
        object GetMenuPermission(vmApplicationTokenModel menu);
        List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID);
    }

}
