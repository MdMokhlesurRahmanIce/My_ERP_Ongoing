using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnItemGroupMgt
    {

        int SaveItemGroup(CmnItemGroup model);
        int UpdateItemGroup(CmnItemGroup model);
        int DeleteItemGroup(CmnItemGroup model);
        List<vmItemGroup> GetAllItemGroups(vmCmnParameters objcmnParam, out int recordsTotal);
        vmItemGroup GetItemGroupByID(int? GID, int? CompanyID);

        int GetMaxByParentID(int? ParentID, int? CompanyID);

        int CheckItemGroupCode(int? GroupCode);
    }
}
