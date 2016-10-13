using ABS.Models;
//using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iSystemCommonDDL
    {
        IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging);
        List<CmnCompany> GetCompanyForDropDownList();
        IEnumerable<vmCmnModule> GetModulesForDropDown(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmCmnModule> GetModulesForDropDown(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnStatu> GetStatusForDropDown(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnMenu> GetMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnMenu> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging, int? ModuleID);
        IEnumerable<CmnMenuType> GetMenuTypeForDropDown(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmCmnOrganogram> GetOrganogramForDropDown(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmUserGroup> GetUserGroupForDropDownList(int? companyID, int? loggedUser, int? userType, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmUserType> GetUserTypeForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmUser> GetUserForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmItemGroup> GetItemGroupsByTypeID(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId);

        List<AccACDetail> GetLedger(vmCmnParameters objcmnParam, Int32 acc1Id); 

        List<vmItemType> GetItemTypes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);
        List<vmUnit> GetAllUnit(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmColor> GetAllColor(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmSize> GetAllSizes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmBrand> GetBrands(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);

        List<vmModel> GetModels(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmYarn> GetYarns(int? pageNumber, int? pageSize, int? IsPaging, int? GetYarns);
        List<vmWarp> GeWarps(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID);
        List<vmBuyer> GetBuyers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmBuyer> GetSuppliers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmBuyer> GetBuyerReffs(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        //List<vmFinishingType> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID);
        List<vmWarp> GetWefts(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID);
        List<vmMachine> GetMachines(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmFinishingWeight> GetFinishWeights(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID);
        List<vmCountry> GetCountry(int CompanyID, int pageNumber, int pageSize, int IsPaging);
        List<vmState> GetState(int CountryID, int CompanyID, int pageNumber, int pageSize, int IsPaging);
        List<vmCity> GetCity(int StateID, int CompanyID, int pageNumber, int pageSize, int IsPaging);
        List<vmDepartment> GetDepartment(int CompanyID, int pageNumber, int pageSize, int IsPaging);
        List<vmDesignation> GetDesignation(int CompanyID, int pageNumber, int pageSize, int IsPaging);

        List<vmItemLot> GetLotsForYarn(int id);
        List<vmCmnUserWiseCompany> GetUserCompany(int userID, int companyID, int useLoggedID);
        List<vmGroup> GetItemGroupParenteList(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId);

        List<vmBranch> GetBranchDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);

        List<vmTeam> GetTemsByDepartmentID(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID);

        List<vmFinishProcess> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID);

        List<vmFinishProcess> GetFinishProcessByItem(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item);
        List<VmItemMater> GetDevelopmentNo(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item);
        List<vmCompany> GetCompanyForMutl(int? pageNumber, int? pageSize, int? isPaging);
        List<vmCompany> GetCompanyPermissionListByUserID(int? userID);
        List<vmCoating> GetCoatingByTypeID(int? pageNumber, int? pageSize, int? isPaging, int? companyID, int? cTypeID);
        List<vmOverdyed> GetOverdyed(int? pageNumber, int? pageSize, int? isPaging, int? companyID);
    }
}