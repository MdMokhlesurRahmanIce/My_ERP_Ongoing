using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.LocalSales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;

namespace ABS.Service.LocalSales.Interfaces
{
   public interface iCmnLocalSalesMgt
    {
       List<vmBuyer> GetBuyers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? UserTypeID);

       List<vmItemGroup> GetItemGroup(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType);

      vmArticleDetail GetArticleDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID);

      List<vmGrade> GetGrades(int? pageNumber, int? pageSize, int? IsPaging);

      vmLSCurrentStock GetCurrentStock(vmSLCmnParameters objcmnParam);

      List<vmItemType> GetItemTypeWithoutFinsihGood(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType);

      IEnumerable<vmItem> GetItemMasterByTypeID(vmCmnParameters objcmnParam, string TypeId, out int recordsTotal);

      vmSLDemageSale GetArticleDetailsforDemageSale(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID);

      string SaveUpdateSalesInvoice(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails, List<vmLSalesInvoiceDetail> DeleteSalesInvoiceDetails);

      List<vmLSalesInvoiceMaster> GetSalesInvoiceDetails(vmCmnParameters objcmnParam, out int recordsTotal);

      string DeleteSalesInvoice(vmCmnParameters objcmnParam);

      List<vmSLDemageSale> LoadSalesListForUpdate(vmCmnParameters objcmnParam);
    }
}
