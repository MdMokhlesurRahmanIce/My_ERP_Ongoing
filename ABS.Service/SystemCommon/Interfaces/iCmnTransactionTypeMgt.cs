using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iCmnTransactionTypeMgt
    {
        IEnumerable<CmnTransactionType> GetTransactionTypes(int? pageNumber, int? pageSize, int? IsPaging);
             
    }
}
