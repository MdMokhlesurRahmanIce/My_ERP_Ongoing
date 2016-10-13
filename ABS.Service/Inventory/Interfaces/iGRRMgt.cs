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
    public interface iGRRMgt
    {
      
        IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging);
        IEnumerable<vmChallan> GetItemDetailBySPRID(vmCmnParameters objcmnParam, Int64 SprID);

        IEnumerable<vmChallan> GetItemDetailFGrrByIssueID(vmCmnParameters objcmnParam, Int64 IssueID);


        IEnumerable<vmChallan> GetItemDetailByPOID(vmCmnParameters objcmnParam, Int64 POID);  
        IEnumerable<vmChallan> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode); 
        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        //// IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  GetSPRNo(objcmnParam, out recordsTotal)
        IEnumerable<vmChallan> GetSPRNo(vmCmnParameters objcmnParam, Int32 ReqTypeID, out int recordsTotal);
        IEnumerable<vmChallan> GetLoanReturnIssueNo(vmCmnParameters objcmnParam, Int32 ReqTypeID, out int recordsTotal);

        IEnumerable<InvGrrMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno);
        IEnumerable<InvGrrMaster> ChkDuplicateGrrNo(vmCmnParameters objcmnParam, string Mno);

        List<vmDepartment> GetDepartmentParentList(int? pageNumber, int? pageSize, int? IsPaging, int? loggedDeptID, int? CompanyId);  
  
        List<vmDepartment>GetLoggedDeptName(vmCmnParameters objcmnParam);

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
        IEnumerable<vmChallan> GetGrrMasterList(vmCmnParameters objcmnParam, bool IsSPR, out int recordsTotal);

        string SaveLot(CmnLot objCmnLot);
        string SaveBatch(CmnBatch objCmnBatch);

        CmnDocumentPath GetUploadPath(int TransactionTypeID);
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id, int TransTypeID);

    }
}
