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
    public interface iComparativeStatementMgt
    {
        IEnumerable<vmComparativeStatement> GetCSMaster(vmCmnParameters objcmnParam, out int recordsTotal);  
        IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging);
        IEnumerable<vmChallan> GetItemDetailByStatementNo(vmCmnParameters objcmnParam, Int64 StatementID);   
        IEnumerable<vmChallan> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode);
        IEnumerable<CmnCombo> GetOrderType(vmCmnParameters objcmnParam, string ComboType);
        IEnumerable<CmnCombo> GetMoneyTrnsType(vmCmnParameters objcmnParam, string ComboType);
        IEnumerable<vmComparativeStatement> GetQuotationRequisitionID(vmCmnParameters objcmnParam);
        IEnumerable<InvRequisitionMaster> GetSPR(vmCmnParameters objcmnParam);
        IEnumerable<vmQuotation> GetQuotationInfoDetail(int QuotationID, int CompanyID);
        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        //// IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  GetSPRNo(objcmnParam, out recordsTotal)
        IEnumerable<InvRequisitionMaster> GetSPRNo(vmCmnParameters objcmnParam, out int recordsTotal);  
        IEnumerable<CmnAddressCountry> GetLocation(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, out int recordsTotal);
        string SaveUpdateCSMasterNdetails(PurchaseCSMaster itemMaster, List<PurchaseCSDetail> itemDetails, int menuID);
        IEnumerable<vmChallan> GetPODetailByPOID(vmCmnParameters objcmnParam, Int64 poID, out int recordsTotal);
        IEnumerable<vmChallan> GetPOMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
        CmnDocumentPath GetUploadPath();
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id);

    }
}
