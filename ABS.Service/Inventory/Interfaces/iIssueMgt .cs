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
    public interface iIssueMgt 
    {         
        IEnumerable<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging);
        string SaveIssueMasterDetails(InvIssueMaster IssueMaster, List<InvIssueDetail> IssueDetails, int menuID);

        List<InvRequisitionMaster> GetRequisitionMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<InvRequisitionMaster> GetRequisitionNo(int? pageNumber, int? pageSize, int? IsPaging, int CompanyID, int RequisitionTypeID);

        IEnumerable<vmRequisition> GetRequisitionItemList(int? pageNumber, int? pageSize, int? IsPaging, int? RequisitionID, int? MrrID);

        List<vmIssueMaster> GetIssueMasterList(vmCmnParameters objcmnParam, out int recordsTotal);

        vmIssueMaster GetIssueMasterByIssueId(int? IssueId, int CompanyID);

        IEnumerable<vmRequisition> GetIssueDetailByIssueId(int? IssueId, int CompanyID);

        List<InvMrrMaster> GetMRRList(int? pageNumber, int? pageSize, int? IsPaging);
        List<InvRChallanMaster> GetChallanList(int? pageNumber, int? pageSize, int? IsPaging);
        List<InvGrrMaster> GetGRRList(int? pageNumber, int? pageSize, int? IsPaging);
    }
}
