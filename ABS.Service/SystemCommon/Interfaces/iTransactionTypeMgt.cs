using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iTransactionTypeMgt
    {

        int SaveItemGroup(CmnTransactionType model);
        int UpdateItemGroup(CmnTransactionType model);
        int DeleteItemGroup(CmnTransactionType model);
        List<vmItemGroup> GetAllTransactionTypes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);
        vmItemGroup GetTransactionTypeByID(int id);
    }
}
