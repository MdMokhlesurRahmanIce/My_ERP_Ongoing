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
    public interface iPIMgt
    {
        List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging);
        List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging);
        // IEnumerable<CmnItemMaster> GetItemMasterById(int Id);  
        IEnumerable<vmItem> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal);
        //IEnumerable<SalPIMaster> GetPIMasterByActivePI(Int64 activePI);
        //IEnumerable<SalPIDetail> GetPIDetailsByActivePI(Int64 activePI);
        IEnumerable<CmnCompany> GetPICompany(int userID);
        IEnumerable<vmPIMaster> GetBranchListByBankID(int Id);
        IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetPIStatus(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmPIMaster> GetBankAdvisingListByCompanyID(int Id);
        int DeleteSalPIMasterNSalPIDetail(int Id);
        string SaveUpdatePIItemMasterNdetails(SalPIMaster itemMaster, List<SalPIDetail> itemDetails, int menuID);
        IEnumerable<vmPIMaster> GetPIMasterByPIActive(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI);

        IEnumerable<CmnCombo> GetSalesItemConstructionType(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<CmnCombo> GetAcceptableQuantity(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<vmPIDetail> GetPIDailyData(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmPIDetail> GetPIMonthlyData(int? pageNumber, int? pageSize, int? IsPaging);


        IEnumerable<SalIncoterm> GetIncoterm(int? pageNumber, int? pageSize, int? IsPaging);

        //IEnumerable<SalBookingMaster> GetBookingList(int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<vmItem> GetBookingDetailByID(vmCmnParameters objcmnParam, string groupId);
        IEnumerable<SalBookingMaster> GetBookingList(int buyerID, int companyID, int? pageNumber, int? pageSize, int? isPaging);
    }
}
