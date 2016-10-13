using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnCustomCodeMgt
    {
        int SavCustomCode(CmnCustomCode model, List<CmnCustomCodeDetail> details);
        List<vmCmnCustomCode> GetAllCustomCode(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<CmnCustomCodeDetail> GetCustomCodeDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID);
        int UpdateMasterStatus(int MasterID);
        void Dispose();


    }
}
