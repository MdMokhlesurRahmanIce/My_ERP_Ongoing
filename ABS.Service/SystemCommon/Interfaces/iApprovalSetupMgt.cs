using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iApprovalSetupMgt 
    {
        string SaveUpdateApprovalSetup(CmnWorkFlowMaster workFlowMaster, List<CmnWorkFlowDetail> workFlowDetail);
        IEnumerable<vmCmnMenuPermission> GetApprovalSetupRecords();
        IEnumerable<vmCmnMenuPermission> GetApprovalDetailsByWorkFlowID(int workFlowID);
        List<vmTeam> GetTeamsUserByTemID(int? TeamID);
    } 
}
