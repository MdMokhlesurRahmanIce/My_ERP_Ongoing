using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iBallWarpingMgt
    {
        IEnumerable<PrdSetSetup> GetSetNo(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmBallWarpingInformation> GetSetInformation(int id, int companyID);
        IEnumerable<vmBallWarpingInformation> GetBallWarpingDetail(int? id);
        //long SaveMachineStop(List<vmBallMachineStopAndBrekage> Stops);
        IEnumerable<PrdBWSlist> GetLoadMachineStopCauses(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<PrdBWSlist> GetLoadMachineBrekages(int? pageNumber, int? pageSize, int? IsPaging);

        //long SaveMachineBreakages(List<vmBallMachineStopAndBrekage> Stops);

        string SaveUpdateBallMRR(PrdBallMRRMaster itemMaster, List<vmPrdBallMRRDetail> itemDetails);

        IEnumerable<vmBallWarpingInformation> GetBallWarpingMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBallWarpingInformation> GetBallWarpingDetailByID(vmCmnParameters cmnParam, out int recordsTotal);
        IEnumerable<vmBallWarpingInformation> GetBallWarpingMasterById(int id);
    }
}
