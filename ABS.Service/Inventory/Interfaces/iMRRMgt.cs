using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Inventory.Interfaces
{
    public interface iMRRMgt 
    {
        IEnumerable<vmRequisition> GetRequisitionItemList(int? RequisitionID);
        List<CmnCombo> GetMrrType(string mrrType, vmCmnParameters objcmnParam);
        List<vmGrr> GetSuppliers(vmCmnParameters objcmnParam);
        List<InvIssueMaster> GetIssueNo(vmCmnParameters objcmnParam);
        List<vmQC> GetSprLoanList(vmCmnParameters objcmnParam, Int32 loanTransTypeSpr);

        //List<CmnCombo> GetChallanInvoiceReceiptTypes(int? pageNumber, int? pageSize, int? IsPaging);
        //IEnumerable<vmSPRPOLCNo> GetSPRPOLCNoByID(vmCmnParameters objcmnParam, int SPRPOLCTypeID, out int recordsTotal); 
        //IEnumerable<vmChallanInvoiceReceipt> GetChallanInvoiceReceiptNoByID(vmCmnParameters objcmnParam, int CIRTypeID, out int recordsTotal);
        //vmSPRPOLCNo GetSPRPOLCDateByNo(int SprpolcType, Int64 SPRPOLCNo);
        //vmChallanInvoiceReceipt GetCIRDateByNo(int CIRType, Int64 CIRNo);
        List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmGrr> GetMasterInfoByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal);
        IEnumerable<vmQC> GetDetailInfoByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal);

        IEnumerable<vmQC> GetQCListByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal);

        IEnumerable<InvMrrQcMaster> GetQCList(vmCmnParameters objcmnParam, Int32 TransType, out int recordsTotal);
         object []  GetWherehouseList(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<vmQC> GetDetailInfoByQCID( vmCmnParameters objcmnParam, Int64 QCID, out int recordsTotal);
        IEnumerable<vmQC> GetDetailInfoByLoanSprID(vmCmnParameters objcmnParam, Int64 LoanSprID, out int recordsTotal);

        IEnumerable<vmQC> GetDetailInfoByIssueID(vmCmnParameters objcmnParam, Int64 IssueID, out int recordsTotal);

        IEnumerable<vmGrr> GetMrrMasterList(vmCmnParameters objcmnParam,  out int recordsTotal);
        IEnumerable<InvMrrMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno);
        IEnumerable<vmQC> GetMrrDetailsListByMrrID(vmCmnParameters objcmnParam, Int64 mrrID, out int recordsTotal);

        //List<vmQC> GetItemDetailByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal);
        string SaveUpdateMrrMasterNdetails(InvMrrMaster mrrMaster, List<InvMrrDetail> mrrDetails, int menuID);
        string SaveLot(CmnLot objCmnLot);
        string SaveBatch(CmnBatch objCmnBatch); 
        //List<vmQC> GetQCMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
        //List<vmQC> GetQCDetailsListByQCMasterID(Int64 id);
    }
}
