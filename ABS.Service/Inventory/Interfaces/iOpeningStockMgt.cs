using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Service.Inventory.Interfaces
{
    public interface iOpeningStockMgt  
    {
        IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging);
        IEnumerable<vmChallan> GetItemDetailBySPRID(vmCmnParameters objcmnParam, Int64 SprID);
        IEnumerable<vmChallan> GetItemDetailByPOID(vmCmnParameters objcmnParam, Int64 POID);  
        IEnumerable<vmChallan> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode); 
        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        //// IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  GetSPRNo(objcmnParam, out recordsTotal)
        IEnumerable<InvRequisitionMaster> GetSPRNo(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<PurchasePOMaster> GetPONo(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<CmnAddressCountry> GetLocation(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<CmnUOM> GetPackingUnit(vmCmnParameters objcmnParam, out int recordsTotal); 
        IEnumerable<CmnUOM> GetWeightUnit(vmCmnParameters objcmnParam, out int recordsTotal); 
        IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, out int recordsTotal);
        string SaveUpdateChallanMasterNdetails(InvGrrMaster itemMaster, List<InvGrrDetail> itemDetails, int menuID, ArrayList fileNames);
        IEnumerable<vmChallan> GetGrrDetailByGrrID(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal); 
        IEnumerable<vmChallan> GetMrrMasterList(vmCmnParameters objcmnParam, out int recordsTotal);

        string SaveUpdateMrrMasterNdetails(InvMrrMaster mrrMaster, List<InvMrrDetail> mrrDetails, int menuID);

        string SaveLot(CmnLot objCmnLot);
        string SaveBatch(CmnBatch objCmnBatch);

        CmnDocumentPath GetUploadPath(int TransactionTypeID);
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id, int TransTypeID);

    }
}
