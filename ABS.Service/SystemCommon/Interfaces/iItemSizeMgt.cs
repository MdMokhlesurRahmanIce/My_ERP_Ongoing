using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iItemSizeMgt
    {
        IEnumerable<CmnItemSize> GetItemSize(int? pageNumber, int? pageSize, int? IsPaging);
        int SaveUpdateItemSize(CmnItemSize model);

        IEnumerable<CmnItemSize> GetItemSizeById(int Id);
        int DeleteItemSize(int Id);
    }
}
