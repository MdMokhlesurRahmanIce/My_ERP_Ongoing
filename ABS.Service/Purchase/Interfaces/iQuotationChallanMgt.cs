using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Purchase.Interfaces
{
    public interface iQuotationMgt  
    { 
        IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging);
        IEnumerable<vmChallan> GetItemDetailBySPRID(vmCmnParameters objcmnParam, Int64 SprID);
        IEnumerable<CmnOrganogram> GetDeptByCompanyID(vmCmnParameters  objcmnParam, int companyID);
        IEnumerable<CmnCompany> GetCompany(vmCmnParameters objcmnParam);

        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        //// IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  GetSPRNo(objcmnParam, out recordsTotal)
        IEnumerable<InvRequisitionMaster> GetSPRNo(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal);

        string GetDataBySuppplierID(vmCmnParameters objcmnParam);
        string SaveUpdateChallanMasterNdetails(InvRChallanMaster itemMaster, List<InvRChallanDetail> itemDetails, int menuID);

        IEnumerable<vmQuotation> GetQuotationMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        string SaveQuotationMasterDetails(PurchaseQuotationMaster Master, List<PurchaseQuotationDetail> Details, int menuID);

        IEnumerable<vmChallan> GetChallanDetailByChallanID(vmCmnParameters objcmnParam, Int64 challanID, out int recordsTotal); 
        
        ////IEnumerable<SalPIMaster> GetPIMasterByActivePI(Int64 activePI);
        //IEnumerable<SalPIDetail> GetPIDetailsByActivePI(Int64 activePI);
        //IEnumerable<CmnCompany> GetPICompany(int userID);
        //IEnumerable<vmPIMaster> GetBranchListByBankID(int Id);
        //IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging);
        //IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging);
        //IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging);
        //IEnumerable<vmPIMaster> GetBankAdvisingListByCompanyID(int Id);
        //int DeleteSalPIMasterNSalPIDetail(int Id);
        //string SaveUpdatePIItemMasterNdetails(SalPIMaster itemMaster, List<SalPIDetail> itemDetails, int menuID);
        IEnumerable<vmChallan> GetChallanMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmRequisitionDetails> GetRequisitonDetailByRequisitionID(int? RequisitionId, int CompanyId);
        //IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI);
    }
}
