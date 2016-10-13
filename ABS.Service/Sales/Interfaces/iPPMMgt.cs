using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Sales.Interfaces
{
   public interface iPPMMgt
    {
       List<CmnUser> GetPIBuyer(int? pageNumber, int? pageSize, int? IsPaging);
    }
}
