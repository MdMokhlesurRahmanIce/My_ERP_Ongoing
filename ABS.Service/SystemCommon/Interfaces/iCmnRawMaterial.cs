using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
   public interface  iCmnRawMaterial
    {

      // string SaveRowMaterial(CmnItemMaster model);

       string SaveRowMaterial(CmnItemMaster itemMaster, int accDetailID, int ACTypeID);

       List<vmFinishGood> GetAllRowMaterial(vmCmnParameters objcmnParam, out int recordsTotal);

       int DeleteRawMaterial(CmnItemMaster model);

       vmFinishGood GetRawMaterial(int id, int typeId, int companyID);

       int UpdateRawMaterial(CmnItemMaster model);
    }
}
