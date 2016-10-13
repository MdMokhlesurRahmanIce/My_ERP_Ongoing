using ABS.Models;
using ABS.Models.ViewModel.Commercial;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Commercial.Interfaces
{
    public interface iDCMgt
    {
        string SaveUpdateDC(SalDCMaster itemMaster, List<SalDCDetail> itemDetails, vmCmnParameters objcmnParam);
        IEnumerable<vmSalDCMasterDetail> GetDCMaster(vmCmnParameters cmnParam, out int recordsTotal);
        IEnumerable<vmSalFDODetail> GetFDOQty(int id);
        IEnumerable<SalFDOMaster> GetAllFDONo(vmCmnParameters objcmnParam, out int recordsTotal);

        IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<Models.ViewModel.Commercial.vmSalDCMasterDetail> GetDCDailyData(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<Models.ViewModel.Commercial.vmSalDCMasterDetail> GetDCMonthlyData(int? pageNumber, int? pageSize, int? IsPaging);
    }
}
