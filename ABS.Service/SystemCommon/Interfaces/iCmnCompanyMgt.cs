using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnCompanyMgt
    {
        IEnumerable<CmnCompany> GetCompanies(int? pageNumber, int? pageSize, int? IsPaging);
        CmnCompany GetCompanyByID(int? id);
        int SaveCompany(CmnCompany model);
        int SaveCompanyParam(CmnCompany modelFirst, UserCommonEntity commonEntity);
        int UpdateCompany(CmnCompany model, UserCommonEntity commonEntity);
        int DeleteCompany(int? CompanyID, UserCommonEntity commonEntity);
       
    }
}
