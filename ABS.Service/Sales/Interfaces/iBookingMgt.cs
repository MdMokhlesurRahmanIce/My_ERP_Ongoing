using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ABS.Service.Sales.Interfaces
{
    public interface iBookingMgt
    {
        List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging);
        List<CmnUser> GetBuyerReference(int? pageNumber, int? pageSize, int? IsPaging);
        //IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        // IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  
        IEnumerable<vmItem> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal);
        //IEnumerable<SalPIMaster> GetPIMasterByActivePI(Int64 activePI);
        //IEnumerable<SalBookingDetail> GetPIDetailsByActivePI(Int64 activePI);
        IEnumerable<CmnCompany> GetPICompany(int userID);
        int DeleteMasterDetail(int Id);
        string SaveUpdateBookingItemMasterNdetails(SalBookingMaster itemMaster, List<SalBookingDetail> itemDetails, int menuID);
        IEnumerable<vmBookingMaster> GetBookingMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBookingDetail> GetBookingDetail(Int64 activePI);
        IEnumerable<vmItemGroup> GetPISampleNo(int companyID, int? pageNumber, int? pageSize, int? isPaging);
    }
}
