using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnOrganogramMgt
    {
        IEnumerable<vmCmnOrganogram> GetOrganograms(int? CompanyID, int? loggedUserID, int? pageNumber, int? pageSize, int? IsPaging);
        vmCmnOrganogram GetOrganogramByID(int? id);
        int SaveOrganogram(CmnOrganogram model);
        int UpdateOrganogram(CmnOrganogram model);
        int DeleteOrganogram(int? OrganogramID);
     //   List<CmnCompany> GetCompanyOnDemand();
    }
}
