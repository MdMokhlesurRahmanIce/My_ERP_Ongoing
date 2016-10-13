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
    public interface iSPRMgt 
    {
        IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging, int companyID);
        IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmUsers> GetAllSupplier(int? pageNumber, int? pageSize, int? IsPaging, int ItemId, int LoginCompanyID);
        IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging, int ItemId, int LoginCompanyID);
        IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging, int ItemID, int LoginCompanyID);
        IEnumerable<InvRequisitionMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno);
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id, int TransTypeID);
        IEnumerable<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? UserTypeID,int? CompanyID);

        IEnumerable<CmnTransactionType> GetAllRequisitionType(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<CmnCompany> GetAllCompany(int? pageNumber, int? pageSize, int? IsPaging);


        IEnumerable<CmnItemGroup> GetAllItemGroup(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<vmCmnOrganogram> GetDepartmentByCompanyID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);


        IEnumerable<CmnItemMaster> GetItemListByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? GroupID);

        IEnumerable<vmRequisition> LaodItemRateNUnit(int? pageNumber, int? pageSize, int? IsPaging, int? itemid);
        string SaveRequisitionMasterDetails(InvRequisitionMaster RequisitionMaster, List<vmRequisitionDetails> RequisitionDetails, UserCommonEntity commonEntity, ArrayList fileNames);
        string SaveSRequisitionMasterDetails(InvRequisitionMaster RequisitionMaster, List<vmRequisitionDetails> RequisitionDetails, UserCommonEntity commonEntity);
        CmnDocumentPath GetUploadPath(int TransactionTypeID);
        IEnumerable<vmRequisition> GetRequisitionMaster(vmCmnParameters objcmnParam, out int recordsTotal,int RequisitionTypeId);   
        IEnumerable<vmRequisition> GetRequisitionMasterSPR(vmCmnParameters objcmnParam, out int recordsTotal, int RequisitionTypeId);

        vmRequisition GetRequisitonMasterByRequisitionID(int? RequisitionId,int ComapnyId);
   
        IEnumerable<vmSPR> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode);

        IEnumerable<vmSPR> GetItmDetailByItemId(vmCmnParameters objcmnParam, string ItemID);

        IEnumerable<vmSPR> GetItmDetail(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<vmRequisitionDetails> GetRequisitonDetailByRequisitionID(int? RequisitionId, int CompanyId);

        string SprUpdateDelete(vmCmnParameters objcmnParam);
    }
}
