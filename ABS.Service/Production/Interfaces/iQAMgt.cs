using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
   public interface iQAMgt
    {
       List<vmFinishingInspactionDetail> GetInspactionDetailsByIDAndDates(vmCmnParameters objcmnParam);
       List<vmFinishingInspactionDetail> GetQAMasterList(vmCmnParameters objcmnParam, out int recordsTotal);
       string SaveUpdateQAMasterDetail(vmFinishingInspactionDetail Master, List<vmFinishingInspactionDetail> Detail, vmCmnParameters objcmnParam);
       string DeleteUpdateQAMasterDetail(vmCmnParameters objcmnParam);
    }
}
