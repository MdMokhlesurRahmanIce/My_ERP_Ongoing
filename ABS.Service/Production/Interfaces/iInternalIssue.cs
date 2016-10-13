using ABS.Models.ViewModel.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
  public  interface iInternalIssue
    {

        vmSetSetupMasterDetail GetSetDetailsBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo);

        List<vmIssueDetail> GetIssueDetailBySetNO(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo);

        int SaveInternalIssue(List<vmIssueDetail> _objIssueDetails, vmInternalIssue _objInternalIssue);

        List<vmIssueDetail> GetInternalIssueDetial(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, bool IsIssuedBall, bool IsReceivedDy, bool IsIssuedDy, bool IsReceivedLCB);

        vmSetSetupMasterDetail GetSetDetailsByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID);

        List<vmIssueDetail> GetIssueDetailByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID);
    }
}
