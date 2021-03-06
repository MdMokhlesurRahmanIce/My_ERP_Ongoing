﻿using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnMenuPermissionMgt
    { 
        int SaveMenuPermission(List<CmnMenuPermission> listModel);
        IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParams(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID);
        IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParamsUser(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID);
    }
}
