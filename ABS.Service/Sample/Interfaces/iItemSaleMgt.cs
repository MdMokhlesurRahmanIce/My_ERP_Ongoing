using ABS.Models;
using ABS.Models.ViewModel.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Sample.Interfaces
{
    public interface iItemSaleMgt
    {
        List<tbl_ProductOutlet> GetOutlet();
        List<tbl_ProductType> GetProductType(int? id);
        List<tbl_Product> GetProduct(int? id);
        int SaveSale(vmSales vmodel);
        List<vmSoldItems> GetSoldItems();
    }
}
