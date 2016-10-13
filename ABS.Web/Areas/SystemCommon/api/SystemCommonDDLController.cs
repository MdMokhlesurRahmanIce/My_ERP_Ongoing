using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/SystemCommonDDL")]
    public class SystemCommonDDLController : ApiController
    {
        private iSystemCommonDDL objDDLService = null;
        public SystemCommonDDLController()
        {
            this.objDDLService = new SystemCommonDDL();
        }

        #region Get Company
        // GET: CompanyonDemand
        [Route("GetCompany/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCompany> objListCompany = null;
            try
            {
                objListCompany = objDDLService.GetCompanyForDropDownList().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }
        #endregion Get Company End
        #region Get Use Wise Company
        // GET: Wise Company
        [Route("GetUserCompany/{userID:int}/{companyID:int}/{useLoggedID:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmCmnUserWiseCompany> GetUserCompany(int userID, int companyID, int useLoggedID)
        {
            List<vmCmnUserWiseCompany> list = new List<vmCmnUserWiseCompany>();
            try
            {
                list = objDDLService.GetUserCompany(userID, companyID, useLoggedID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Use Wise Company
        #region GetModule 

        // GET: GetCustomers/0/10/0
        [Route("GetModules/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<vmCmnModule> GetModules(  int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnModule> objListModules = null;
            try
            {
                objListModules = objDDLService.GetModulesForDropDown(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListModules;
        }

        // GET: GetCustomers/0/10/0
        [Route("GetModule/{companyID:int}/{userID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<vmCmnModule> GetModule(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnModule> objListModules = null;
            try
            {
                objListModules = objDDLService.GetModulesForDropDown(companyID, userID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListModules;
        }
        // GET: GetCustomers/0/10/0
        [Route("GetModuleWithPermission/{companyID:int}/{userID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnModule> objListModules = null;
            try
            {
                objListModules = objDDLService.GetModuleWithPermission(companyID, userID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListModules;
        }

        #endregion GetModuleEnd

        #region GetStatus
        [Route("GetStatus/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnStatu))]
        [HttpGet]
        public IEnumerable<CmnStatu> GetStatus(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnStatu> listStatus = null;
            try
            {
                listStatus = objDDLService.GetStatusForDropDown(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listStatus;
        }
        #endregion GetStatus

        #region GetMenues
        [Route("GetParentMenuForDropDown/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ModuleID:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<CmnMenu> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging,int? ModuleID)
        {
            IEnumerable<CmnMenu> listMenues = null;
            try
            {
                listMenues = objDDLService.GetParentMenuForDropDown(pageNumber, pageSize, IsPaging,ModuleID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues;
        }

        [Route("GetMenues/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<CmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnMenu> listMenues = null;
            try
            {
                listMenues = objDDLService.GetMenuForDropDown(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues;
        }
        #endregion GetMenues

        #region GetMenuType
        [Route("GetMenuType/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnMenuType))]
        [HttpGet]
        public IEnumerable<CmnMenuType> GetMenuType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnMenuType> listMenuTypes = null;
            try
            {
                listMenuTypes = objDDLService.GetMenuTypeForDropDown(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenuTypes;
        }
        #endregion GetMenuType

        #region GetorganoGrame
        [Route("GetOrganogram/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCmnOrganogram))]
        [HttpGet]
        public IEnumerable<vmCmnOrganogram> GetOrganogram(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnOrganogram> listOrganogram = null;
            try
            {

                listOrganogram = objDDLService.GetOrganogramForDropDown(companyID, loggedUser, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listOrganogram;
        }
        #endregion GetorganoGrame

        #region Get User Group
        // GET: CompanyonDemand
        [Route("GetUserGroup/{companyID:int}/{loggedUser:int}/{userType:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmUserGroup> GetUserGroup(int? companyID, int? loggedUser,int ? userType, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmUserGroup> obj = null;
            try
            {
                obj = objDDLService.GetUserGroupForDropDownList(companyID, loggedUser, userType, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return obj;
        }
        #endregion Get User Group End

        #region Get User  
        // GET: CompanyonDemand
        [Route("GetUser/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmUser> GetUser(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmUser> obj = null;
            try
            {
                obj = objDDLService.GetUserForDropDownList(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return obj;
        }
        #endregion Get User Group End

        #region GetBatch
        [Route("GetAllBatch/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnBatch))]
        [HttpGet]
        public IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnBatch> listBatch = null;
            try
            {
                listBatch = objDDLService.GetAllBatch(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listBatch;
        }
        #endregion GetBatch

        #region GetMenuesLatest
        [Route("GetMenues/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<CmnMenu> GetMenues(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnMenu> listMenues = null;
            try
            {
                listMenues = objDDLService.GetMenuForDropDown(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues;
        }
        #endregion GetMenuesLatest

        #region Get Company
        // GET: CompanyonDemand
        [Route("GetCompany/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<CmnCompany> GetCompany(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCompany> objListCompany = null;
            try
            {
                objListCompany = objDDLService.GetCompanyForDropDownList().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }
        #endregion Get Company End

        #region GetBranch
        [Route("GetBranch/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCmnOrganogram))]
        [HttpGet]
        public IEnumerable<vmCmnOrganogram> GetBranch(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnOrganogram> listOrganogram = null;
            try
            {

                listOrganogram = objDDLService.GetOrganogramForDropDown(companyID, loggedUser, pageNumber, pageSize, IsPaging);
                listOrganogram = listOrganogram.Where(x => x.ParentID == null).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listOrganogram;
        }
        #endregion

        #region Country-State-City

        [Route("GetCountry/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCountry))]
        [HttpGet]
        public IEnumerable<vmCountry> GetCountry(int companyID, int loggedUser, int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCountry> CountryList = null;
            try
            {
                CountryList = objDDLService.GetCountry(companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return CountryList;
        }

        [Route("GetState/{countryID:int}/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmState))]
        [HttpGet]
        public IEnumerable<vmState> GetState(int countryID, int companyID, int loggedUser, int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmState> StateList = null;
            try
            {
                StateList = objDDLService.GetState(countryID, companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return StateList;
        }

        [Route("GetCity/{stateID:int}/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCity))]
        [HttpGet]
        public IEnumerable<vmCity> GetCity(int stateID, int companyID, int loggedUser, int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCity> CityList = null;
            try
            {
                CityList = objDDLService.GetCity(stateID, companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return CityList;
        }
        #endregion

        #region Designation-Department
        [Route("GetDepartment/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmDepartment))]
        [HttpGet]
        public IEnumerable<vmDepartment> GetDepartment(int companyID, int loggedUser, int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmDepartment> DepartmentList = null;
            try
            {
                DepartmentList = objDDLService.GetDepartment(companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return DepartmentList;
        }

        [Route("GetDesignation/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmDesignation))]
        [HttpGet]
        public IEnumerable<vmDesignation> GetDesignation(int companyID, int loggedUser, int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmDesignation> DesignationList = null;
            try
            {
                DesignationList = objDDLService.GetDesignation(companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return DesignationList;
        }
        #endregion

        [Route("GetCompanyForMutl/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmCompany> GetCompanyForMutl(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmCompany> objListCompany = null;
            try
            {
                objListCompany = objDDLService.GetCompanyForMutl(pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }
        [Route("GetCompanyPermissionListByUserID/{UserID:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmCompany> GetCompanyPermissionListByUserID(int? UserID)
        {
            List<vmCompany> objListCompany = null;
            try
            {
                objListCompany = objDDLService.GetCompanyPermissionListByUserID(UserID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }

    }
}
