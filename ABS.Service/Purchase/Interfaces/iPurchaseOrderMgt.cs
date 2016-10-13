using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Purchase;
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
    public interface iPurchaseOrderMgt
    {
        IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging);
        IEnumerable<vmChallan> GetItemDetailByStatementNo(vmCmnParameters objcmnParam, Int64 StatementID);   
        IEnumerable<vmChallan> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode);
        IEnumerable<CmnCombo> GetOrderType(vmCmnParameters objcmnParam, string ComboType);

        IEnumerable<CmnCombo> GetMoneyTrnsType(vmCmnParameters objcmnParam, string ComboType);
        IEnumerable<vmTermsCondition> GetTermCondition(vmCmnParameters objcmnParam, out int recordsTotal);

        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        //// IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  GetSPRNo(objcmnParam, out recordsTotal)
        IEnumerable<InvRequisitionMaster> GetSPRNo(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<PurchaseQuotationMaster> GetStatementNo(vmCmnParameters objcmnParam, out int recordsTotal);
     
        IEnumerable<CmnUOM> GetPackingUnit(vmCmnParameters objcmnParam, out int recordsTotal); 
        IEnumerable<CmnUOM> GetWeightUnit(vmCmnParameters objcmnParam, out int recordsTotal); 
        IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, out int recordsTotal);
        string SaveUpdatePOMasterNdetails(PurchasePOMaster itemMaster, List<PurchasePODetail> itemDetails, int menuID, ArrayList fileNames,List<vmTermsCondition> termdetail );
        IEnumerable<vmChallan> GetPODetailByPOID(vmCmnParameters objcmnParam, Int64 poID, out int recordsTotal);
        IEnumerable<vmChallan> GetPOMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
        CmnDocumentPath GetUploadPath();
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id);

    }
}
