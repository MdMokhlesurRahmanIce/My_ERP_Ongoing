using ABS.Models.ViewModel.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
   public interface iSizeBeamIssueMgt
    {

       vmSizeBeamIssue GetSetDeatailBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID);
       List<vmSizeBeamIssue> GetSizingMRRMasterDetailBySetID(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID);
       int SaveSizeBeamIssue(List<vmSizeBeamIssue> _objSizeIssueDetails, vmSizeBeamIssue _objSizeIssueMaster);

       List<vmSizeBeamIssue> GetSizeBeamIssueDetails(int? pageNumber, int? pageSize, int? IsPaging, int? LoginCompanyID);
       vmSizeBeamIssue GetSizeBeamIssuemasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID);
       List<vmSizeBeamIssue> GetSizingMRRMasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID);


       
    }
}
