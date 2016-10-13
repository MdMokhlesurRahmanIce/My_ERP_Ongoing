using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
   public interface iFinishGood
    {
        Int64 SaveYarn(List<vmYarn> Yarns);

        string SaveFinishGood(CmnItemMaster _itemMaster, List<vmFinishProcess> _finishing);

        List<vmFinishGood> GetFinishGoods(vmCmnParameters objcmnParam, out int recordsTotal);

        int DeleteFinishGood(CmnItemMaster _itemMaster);

        vmFinishGood GetFinishGoodById(int id);
        vmFinishGood GetFinishGoodsById(int id);
        int UpdateFinishGood(CmnItemMaster _itemMaster, List<vmFinishProcess> _finishing);

        vmYarn GetYarnBYId(int? yarnId, int? CompanyID);

        vmFinishGood GetFabricDevelopmentDetailById(int id);
        vmFinishGood GetFabricDevelopmentDetailsByID(int id);
        List<vmFinishGood> GetFabricDevelopmentList(vmCmnParameters objcmnParam, out int recordsTotal);

        List<vmConsumption> GetConsumptionInfoByItemID(int id);

        int SaveConsumpiton(List<vmConsumption> _objvmConsumptions, ConsumptionMaster _objConsumptonMaster);
        int uploadFiles(List<vmRndDoc> _DocList);
        List<vmRndDoc> GetDoclistByItemID(int itemID, int transactionID);
        List<vmItemGroup> GetAcDetailIDByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? GroupID);
    }
}
