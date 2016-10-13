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
    public interface iQCMgt
    {
        IEnumerable<InvRequisitionMaster> GetSPR(vmCmnParameters objcmnParam);
        List<CmnCombo> GetSPRPOLCType(int? pageNumber, int? pageSize, int? IsPaging);
        List<CmnCombo> GetChallanInvoiceReceiptTypes(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmSPRPOLCNo> GetSPRPOLCNoByID(vmCmnParameters objcmnParam, int SPRPOLCTypeID, out int recordsTotal);
        IEnumerable<vmChallanInvoiceReceipt> GetChallanInvoiceReceiptNoByID(vmCmnParameters objcmnParam, int CIRTypeID, out int recordsTotal);

        IEnumerable<InvMrrQcMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno);
        vmSPRPOLCNo GetSPRPOLCDateByNo(int SprpolcType, Int64 SPRPOLCNo);
        vmChallanInvoiceReceipt GetCIRDateByNo(int CIRType, Int64 CIRNo);
        //List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<vmQC> GetGRRNo(vmCmnParameters objcmnParam, Int32 TransTypeID, out int recordsTotal);

        List<vmQC> GetItemDetailByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal);
        string SaveUpdateQCMasterNdetails(InvMrrQcMaster qcMaster, List<InvMrrQcDetail> qcDetails, int menuID);

        List<vmQC> GetQCMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
        List<vmQC> GetQCDetailsListByQCMasterID(vmCmnParameters objcmnParam, Int64 id);
        CmnDocumentPath GetUploadPath(int TransTypeID);
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id, int TransTypeID);
    }
}
