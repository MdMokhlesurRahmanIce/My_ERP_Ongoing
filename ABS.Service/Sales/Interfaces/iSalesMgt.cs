using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Sales.Interfaces
{
    public interface iSalesMgt
    { 
        IEnumerable<tbl_Sales> GetSales(int? pageNumber, int? pageSize, int? IsPaging);
        int SaveUpdateSales(tbl_Sales model);
        IEnumerable<tbl_Sales> GetSalesById(int Id);
        int DeleteSales(int Id);
    }
}
