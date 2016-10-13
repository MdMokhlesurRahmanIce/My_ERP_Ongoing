using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Sales.Interfaces
{
    public interface iDeliveryOrderMgt
    {
        IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging);
        SalHDOMaster GetDoNoByID(int? id);
        IEnumerable<CmnUser> GetBuyer(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<SalLCMaster> GetLCByID(int? id);
    }
}
