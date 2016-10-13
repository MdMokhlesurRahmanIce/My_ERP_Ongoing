using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iYarnMgt
    {
        string SaveYarn(CmnItemMaster model);

        List<VmItemMater> GetAllYarn(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);

        int DeleteYarn(CmnItemMaster model);

        VmItemMater GetYarn(int id);

        int UpdateYarn(CmnItemMaster model);

        bool CheckItemCode(string ItemName);
    }
}
