using System.Web.UI.WebControls;
using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Sales.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;
using System.Data.Entity;
//using ABS.Models.ViewModel.Production;

namespace ABS.Service.SystemCommon.Factories
{
    public class SystemCommonDDL : iSystemCommonDDL
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnUserGroup> GenericFactoryFor_UserGroup = null;
        private iGenericFactory_EF<CmnItemType> GenericFactoryFor_ItemType = null;
        private iGenericFactory_EF<CmnItemGroup> GenericFactoryFor_ItemGroup = null;
        private iGenericFactory_EF<CmnOrganogram> GenericFactoryForOrganogram = null;
        private iGenericFactory_EF<CmnBatch> GenericFactoryFor_Batch = null;
        private iGenericFactory<vmCmnModule> GenericFactoryFor_Module = null;

       

        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company = null;
        private iGenericFactory<CmnStatu> GenericFactoryFor_Status = null;
        private iGenericFactory<CmnMenu> GenericFactoryFor_Menu = null;
        private iGenericFactory<CmnMenuType> GenericFactoryFor_MenuType = null;
        private iGenericFactory<vmCmnOrganogram> GenericFactoryFor_Organogram = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_User = null;
        private iGenericFactory_EF<CmnUserType> GenericFactoryFor_UserType = null;
        private iGenericFactory_EF<CmnUOM> GenericFactoryFor_Unit = null;
        private iGenericFactory_EF<CmnItemColor> GenericFactoryFor_Color = null;
        private iGenericFactory_EF<CmnItemSize> GenericFactoryFor_Size = null;
        private iGenericFactory_EF<CmnItemBrand> GenericFactoryFor_Brand = null;
        private iGenericFactory_EF<CmnItemModel> GenericFactoryFor_Model = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactoryFor_ItemMaster = null;
        private iGenericFactory_EF<RndYarnCR> GenericFactoryFor_RndYarnCr = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_CmnUser = null;
        private iGenericFactory_EF<PrdFinishingType> GenericFactoryFor_FinishingType = null;
        private iGenericFactory_EF<CmnItemFinishingWeight> GenericFactoryForItemFinishingWeifht = null;
        private iGenericFactory_EF<CmnLot> GenericFactoryForlot = null;
        private iGenericFactory_EF<SalPIMaster> GenericFactoryForPI = null;

        #region GetBatch
        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Batch = new CmnBatch_EF();
            IEnumerable<CmnBatch> BatchList = null;

            try
            {
                BatchList = GenericFactoryFor_Batch.GetAll().Select(m => new CmnBatch { BatchID = m.BatchID, BatchNo = m.BatchNo, IsDeleted = m.IsDeleted }).Where(m => m.IsDeleted == false).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return BatchList;
        }
        #endregion

        #region GetCompany
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<CmnCompany> GetCompanyForDropDownList()
        {
            GenericFactoryFor_Company = new CmnCompany_EF();
            List<CmnCompany> objCompanyList = null;
            string spQuery = string.Empty;
            try
            {
                var company = GenericFactoryFor_Company.GetAll();
                objCompanyList = (from olt in company
                                      // where olt.StatusID == 1
                                  where olt.IsDeleted == false
                                  orderby olt.CompanyID descending
                                  select new
                                  {
                                      CompanyID = olt.CompanyID,
                                      CompanyName = olt.CompanyName

                                  }).ToList().Select(x => new CmnCompany
                                  {
                                      CompanyID = x.CompanyID,
                                      CompanyName = x.CompanyName

                                  }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCompanyList.OrderBy(x => x.CompanyID).ToList();
        }
        #endregion GetCompany End

        #region UserGroup
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<vmUserGroup> GetUserGroupForDropDownList(int? companyID, int? loggedUser, int? userType, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmUserGroup> objUserGroupList = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objUserGroupList = (from ug in _ctxCmn.CmnUserGroups
                                        where ug.CompanyID == companyID && ug.UserTypeID == userType
                                        orderby ug.UserGroupID descending
                                        select new
                                        {
                                            UserGroupID = ug.UserGroupID,
                                            GroupName = ug.GroupName

                                        }).ToList().Select(x => new vmUserGroup
                                        {
                                            UserGroupID = x.UserGroupID,
                                            GroupName = x.GroupName

                                        }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUserGroupList.ToList().OrderBy(x => x.UserGroupID).ToList();
        }
        #endregion userGroup End

        #region UserType
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<vmUserType> GetUserTypeForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_UserType = new CmnUserType_EF();
            List<vmUserType> objUserTypeList = null;
            string spQuery = string.Empty;
            try
            {
                var UserType = GenericFactoryFor_UserType.GetAll();
                objUserTypeList = (from ut in UserType
                                   where ut.CompanyID == companyID //&& ut.LoggedUser == loggedUser
                                   orderby ut.UserTypeID descending
                                   select new
                                   {
                                       UserTypeID = ut.UserTypeID,
                                       UserTypeName = ut.UserTypeName

                                   }).ToList().Select(x => new vmUserType
                                   {
                                       UserTypeID = x.UserTypeID,
                                       UserTypeName = x.UserTypeName

                                   }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUserTypeList.ToList();
        }
        #endregion GetCompany End

        #region User
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<vmUser> GetUserForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_User = new CmnUser_EF();
            List<vmUser> objUserList = null;
            string spQuery = string.Empty;
            try
            {
                var User = GenericFactoryFor_User.GetAll();
                objUserList = (from ug in User
                               where ug.UserTypeID == 1 //ug.CompanyID == companyID &&
                               orderby ug.UserID descending
                               select new
                               {
                                   UserID = ug.UserID,
                                   UserFullName = ug.UserFullName

                               }).ToList().Select(x => new vmUser
                               {
                                   UserID = x.UserID,
                                   UserFullName = x.UserFullName

                               }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUserList.ToList().OrderBy(x => x.UserID).ToList();
        }
        #endregion Getuser End

        #region Get Module
        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnModule> GetModulesForDropDown(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            List<vmCmnModule> listModule = new List<vmCmnModule>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("companyID", 0);
                ht.Add("userID", 0);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnModule]";
                listModule = GenericFactoryFor_Module.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listModule.OrderBy(x => x.ModuleID).ToList();
        }

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnModule> GetModulesForDropDown(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            List<vmCmnModule> listModule = new List<vmCmnModule>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("companyID", companyID);
                ht.Add("userID", userID);

                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnModule]";
                listModule = GenericFactoryFor_Module.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listModule.OrderBy(x => x.ModuleID).ToList();
        }

        public IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            List<vmCmnModule> listModule = new List<vmCmnModule>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("companyID", companyID);
                ht.Add("userID", userID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnModuleWithPermission]";
                listModule = GenericFactoryFor_Module.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listModule.OrderBy(x => x.ModuleID).ToList();
        }
        #endregion Get Module End

        #region Get Status
        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<CmnStatu> GetStatusForDropDown(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Status = new CmnStatu_GF();
            IEnumerable<CmnStatu> objStatus = null;
            List<CmnStatu> listStatus = new List<CmnStatu>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnStatus]";
                objStatus = GenericFactoryFor_Status.ExecuteQuery(spQuery, ht);
                listStatus = (from olt in objStatus
                              orderby olt.StatusID descending
                              select new
                              {
                                  StatusID = olt.StatusID,
                                  StatusName = olt.StatusName

                              }).ToList().Select(x => new CmnStatu
                              {
                                  StatusID = x.StatusID,
                                  StatusName = x.StatusName

                              }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listStatus.OrderBy(x => x.StatusID).ToList();
        }

        #endregion Get Status

        #region GetMenu
        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<CmnMenu> GetMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Menu = new CmnMenu_GF();
            //IEnumerable<CmnMenu> objMenues = null;
            List<CmnMenu> listMenues = new List<CmnMenu>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnMenu]";
                listMenues = GenericFactoryFor_Menu.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues.OrderBy(x => x.MenuName).ToList();
        }

        public IEnumerable<CmnMenu> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging, int? ModuleID)
        {
            GenericFactoryFor_Menu = new CmnMenu_GF();
            //IEnumerable<CmnMenu> objMenues = null;
            List<CmnMenu> listMenues = new List<CmnMenu>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                ht.Add("ModuleID", ModuleID);
                spQuery = "[Get_CmnMenuParent]";
                listMenues = GenericFactoryFor_Menu.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues.OrderBy(x => x.MenuName).ToList();
        }

        #endregion Get Menu End

        #region GetMenuType
        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<CmnMenuType> GetMenuTypeForDropDown(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_MenuType = new CmnMenuType_GF();
            List<CmnMenuType> listMenuTypes = new List<CmnMenuType>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnMenuType]";
                listMenuTypes = GenericFactoryFor_MenuType.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenuTypes.OrderBy(x => x.MenuTypeID).ToList();
        }

        #endregion Get Menu End

        #region GetOrganogram
        /// CompanyID Provided But Not in Use
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnOrganogram> GetOrganogramForDropDown(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            //Have to add companyID and loggeduser param in procedure
            List<vmCmnOrganogram> listOrganogram = new List<vmCmnOrganogram>();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", companyID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnOrganogram]";
                listOrganogram = GenericFactoryFor_Organogram.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return listOrganogram.OrderBy(x => x.OrganogramID).ToList();
        }

        #endregion GetOrganogram End

        #region GetItemTypeStart
        public List<vmItemType> GetItemTypes(int? pageNumber, int? pageSize, int? IsPaging, int? compayId)
        {
            List<vmItemType> cmnItemTypes = null;
            string spQuery = string.Empty;
            try
            {
                using (GenericFactoryFor_ItemType = new CmnItemType_EF())
                {
                    var ItemTypes = GenericFactoryFor_ItemType.GetAll();
                    cmnItemTypes = (from olt in ItemTypes
                                    where olt.CompanyID == compayId && olt.IsDeleted == false
                                    orderby olt.ItemTypeID descending
                                    select new vmItemType
                                    {
                                        ItemTypeID = olt.ItemTypeID,
                                        ItemTypeName = olt.ItemTypeName

                                    }).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return cmnItemTypes.OrderBy(x => x.ItemTypeID).ToList();
        }

        #endregion GetItemTypeEnd

        #region GetItemGroupParentesStart
        public List<vmItemGroup> GetItemGroupsByTypeID(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            List<vmItemGroup> cmnItemItemGroupParentes = null;
            string spQuery = string.Empty;
            try
            {
                GenericFactoryFor_ItemGroup = new CmnItemGroup_EF();
                var ItemTypeGroupes = GenericFactoryFor_ItemGroup.GetAll().Where(x => x.ItemTypeID == ItemTypeID && x.IsDeleted == false && x.CompanyID == CompanyId);
                cmnItemItemGroupParentes = (from olt in ItemTypeGroupes
                                            orderby olt.ItemTypeID descending
                                            select new vmItemGroup
                                            {
                                                ItemGroupID = olt.ItemGroupID,
                                                ItemGroupName = olt.ItemGroupName
                                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return cmnItemItemGroupParentes.OrderBy(x => x.ItemGroupID).ToList();
        }
        #endregion GetItemGroupParentesEnd

        #region GETAccACDetail
        public List<AccACDetail> GetLedger(vmCmnParameters objcmnParam, Int32 acc1Id)
        {
            List<AccACDetail> objAccACDetail = null;
            using (_ctxCmn = new ERP_Entities())
            {

                try
                {

                    //  objAccACDetail = (from acclist in _ctxCmn.AccACDetails.Where(m => m.IsActive == true && m.AC1Id == acc1Id).ToList() select  new AccACDetail{Id=acclist.Id, ACName = acclist.ACName, AC1Id=acclist.AC1Id, ACode = acclist.ACode} ).ToList();

                    objAccACDetail = (from acclist in _ctxCmn.AccACDetails.Where(m => m.IsActive == true).ToList() select new AccACDetail { Id = acclist.Id, ACName = acclist.ACName, AC1Id = acclist.AC1Id, ACode = acclist.ACode }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }

            }
            return objAccACDetail.OrderBy(x => x.Id).ToList();
        }
        #endregion GETAccACDetail


        #region RawMaterial
        /// No CompanyID Provided
        public List<vmUnit> GetAllUnit(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_Unit = new CmnUOM_EF();
            List<vmUnit> unites = null;
            try
            {
                List<CmnUOM> Allunit = GenericFactoryFor_Unit.GetAll().ToList();
                unites = (from olt in Allunit
                          where olt.IsDeleted == false && olt.CompanyID == CompanyID
                          orderby olt.UOMID descending
                          select new vmUnit
                          {
                              UOMID = olt.UOMID,
                              UOMName = olt.UOMName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return unites.OrderBy(x => x.UOMID).ToList();
        }

        public List<vmColor> GetAllColor(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID)
        {
            GenericFactoryFor_Color = new CmnItemColor_EF();
            List<vmColor> colors = null;

            try
            {
                List<CmnItemColor> AllColors = GenericFactoryFor_Color.GetAll().ToList();// when Condition apply then Face problem 
                colors = (from olt in AllColors
                          where olt.IsDeleted == false && olt.CompanyID == ComapnyID
                          orderby olt.ItemColorID descending
                          select new vmColor
                          {
                              ItemColorID = olt.ItemColorID,
                              ColorName = olt.ColorName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return colors.OrderBy(x => x.ItemColorID).ToList();
        }

        public List<vmItemLot> GetLotsForYarn(int id)
        {
            GenericFactoryForlot = new CmnLot_EF();
            List<vmItemLot> lots = null;
            try
            {

                List<CmnLot> allLot = GenericFactoryForlot.GetAll().Where(x => x.IsDeleted == false && x.ItemID == id).ToList();
                lots = (from olt in allLot
                        orderby olt.LotID descending
                        select new vmItemLot
                        {
                            LotID = olt.LotID,
                            LotNo = olt.LotNo

                        }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return lots.OrderBy(x => x.LotID).ToList();
        }


        public List<vmFinishingWeight> GetFinishWeights(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryForItemFinishingWeifht = new CmnItemFinishingWeight_EF();
            List<vmFinishingWeight> finishWeights = null;

            try
            {
                List<CmnItemFinishingWeight> AllFinishWeight = GenericFactoryForItemFinishingWeifht.GetAll().Where(x => x.IsDeleted == false && x.CompanyID == CompanyID).ToList();
                finishWeights = (from olt in AllFinishWeight
                                 orderby olt.FinishingWeigthID descending
                                 select new vmFinishingWeight
                                 {
                                     FinishingWeigthID = olt.FinishingWeigthID,
                                     FinishingWeigth = olt.FinishingWeigth

                                 }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return finishWeights.OrderBy(x => x.FinishingWeigthID).ToList();
        }

        public List<vmSize> GetAllSizes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_Size = new CmnItemSize_EF();
            List<vmSize> Sizes = null;

            try
            {
                List<CmnItemSize> AllSize = GenericFactoryFor_Size.GetAll().ToList();
                Sizes = (from olt in AllSize
                         where olt.IsDeleted == false && olt.CompanyID == CompanyID
                         orderby olt.SizeID descending
                         select new vmSize
                         {
                             SizeID = olt.SizeID,
                             SizeName = olt.SizeName

                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Sizes.OrderBy(x => x.SizeID).ToList();
        }

        public List<vmBrand> GetBrands(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_Brand = new CmnItemBrand_EF();
            List<vmBrand> Brands = null;

            try
            {
                List<CmnItemBrand> AllBrand = GenericFactoryFor_Brand.GetAll().ToList();
                Brands = (from olt in AllBrand
                          where olt.IsDeleted == false && olt.CompanyID == CompanyID
                          orderby olt.ItemBrandID descending
                          select new vmBrand
                          {
                              ItemBrandID = olt.ItemBrandID,
                              BrandName = olt.BrandName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Brands.OrderBy(x => x.ItemBrandID).ToList();

        }

        public List<vmModel> GetModels(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_Model = new CmnItemModel_EF();
            List<vmModel> Models = null;

            try
            {
                List<CmnItemModel> AllBrand = GenericFactoryFor_Model.GetAll().ToList();
                Models = (from olt in AllBrand
                          where olt.IsDeleted == false && olt.CompanyID == CompanyID
                          orderby olt.ItemModelID descending
                          select new vmModel
                          {
                              ItemModelID = olt.ItemModelID,
                              ModelName = olt.ModelName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Models.OrderBy(x => x.ItemModelID).ToList();
        }

        public List<vmYarn> GetYarns(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_ItemMaster = new CmnItemMaster_EF();
            List<vmYarn> yarns = null;

            try
            {
                List<CmnItemMaster> AllYarns = GenericFactoryFor_ItemMaster.GetAll().Where(x => x.ItemTypeID == 3 && x.CompanyID == CompanyID).ToList();
                yarns = (from olt in AllYarns
                         orderby olt.ItemID descending
                         select new vmYarn
                         {
                             ItemID = olt.ItemID,
                             ItemName = olt.ItemName

                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return yarns.OrderBy(x => x.ItemID).ToList();
        }

        public List<vmWarp> GeWarps(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID)
        {
            GenericFactoryFor_RndYarnCr = new RndYarn_EF();
            List<vmWarp> vmWarps = null;

            try
            {
                List<RndYarnCR> AllYarns = GenericFactoryFor_RndYarnCr.FindBy(x => x.IsDeleted == false && x.YarnType == "Warp" && x.CompanyID == ComapnyID).ToList();
                vmWarps = (from olt in AllYarns
                           orderby olt.YarnID descending
                           select new vmWarp
                           {
                               YarnID = olt.YarnID,
                               YarnCount = olt.YarnCount

                           }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return vmWarps.OrderBy(x => x.YarnID).ToList();
        }

        public List<vmWarp> GetWefts(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_RndYarnCr = new RndYarn_EF();
            List<vmWarp> vmWefts = null;

            try
            {
                List<RndYarnCR> AllYarns = GenericFactoryFor_RndYarnCr.FindBy(x => x.IsDeleted == false && x.YarnType == "Weft" && x.CompanyID == CompanyID).ToList();
                vmWefts = (from olt in AllYarns
                           orderby olt.YarnID descending
                           select new vmWarp
                           {
                               YarnID = olt.YarnID,
                               YarnCount = olt.YarnCount

                           }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return vmWefts.OrderBy(x => x.YarnID).ToList();
        }

        public List<vmBuyer> GetBuyers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_CmnUser = new CmnUser_EF();
            List<vmBuyer> Buyers = null;
            // UserTypeID 2 is BuyerTypeID
            try
            {
                List<CmnUser> AllUser = GenericFactoryFor_CmnUser.GetAll().Where(x => x.IsDeleted == false && x.UserTypeID == 2 && x.CompanyID == CompanyID).ToList();
                Buyers = (from olt in AllUser
                          orderby olt.UserID descending
                          select new vmBuyer
                          {
                              UserID = olt.UserID,
                              UserName = olt.UserFullName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Buyers;
        }

        public List<vmBuyer> GetBuyers(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_CmnUser = new CmnUser_EF();
            List<vmBuyer> Buyers = null;
            // UserTypeID 2 is BuyerTypeID
            try
            {
                List<CmnUser> AllUser = GenericFactoryFor_CmnUser.GetAll().Where(x => x.IsDeleted == false && x.UserTypeID == 2 && x.CompanyID == objcmnParam.loggedCompany).ToList();
                Buyers = (from olt in AllUser
                          orderby olt.UserID descending
                          select new vmBuyer
                          {
                              UserID = olt.UserID,
                              UserName = olt.UserFullName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Buyers;
        }

        public List<vmPIMaster> GetPI(vmCmnParameters objcmnParam)
        {
            GenericFactoryForPI = new SalPIMaster_EF();
            List<vmPIMaster> allPIsList = null;
            List<SalPIMaster> AllPI = GenericFactoryForPI.GetAll().Where(x => x.IsDeleted == false && x.CompanyID == objcmnParam.loggedCompany).ToList();
            try
            {
                allPIsList = (from olt in AllPI
                              orderby olt.PINO descending
                              select new vmPIMaster
                              {
                                  PIID = olt.PIID,
                                  PINO = olt.PINO

                              }).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return allPIsList;
        }

        public List<vmMachine> GetMachines(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_ItemMaster = new CmnItemMaster_EF();
            List<vmMachine> Machines = null;
            // ItemTypeID 4 is Fixed Asset
            // Department 11 is Weaving
            try
            {
                List<CmnItemMaster> allMechines = GenericFactoryFor_ItemMaster.GetAll().Where(x => x.IsDeleted == false && x.ItemTypeID == 4 && x.DepartmentID == 11 && x.CompanyID == CompanyID).ToList();
                Machines = (from olt in allMechines
                            orderby olt.ItemID descending
                            select new vmMachine
                            {
                                ItemId = olt.ItemID,
                                MachineName = olt.ItemName

                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Machines.OrderBy(x => x.ItemId).ToList();
        }
        #endregion

        #region Finishgood
        public List<vmBuyer> GetSuppliers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_CmnUser = new CmnUser_EF();
            List<vmBuyer> Buyers = null;
            // UserType ID 3 is Supplier ID
            try
            {
                List<CmnUser> AllUser = GenericFactoryFor_CmnUser.GetAll().Where(x => x.IsDeleted == false && x.UserTypeID == 3 && x.CompanyID == CompanyID).ToList();
                Buyers = (from olt in AllUser
                          orderby olt.UserID descending
                          select new
                          {
                              UserID = olt.UserID,
                              UserName = olt.UserFullName

                          }).ToList().Select(x => new vmBuyer
                          {
                              UserID = x.UserID,
                              UserName = x.UserName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Buyers.OrderBy(x => x.UserID).ToList();
        }

        public List<vmBuyer> GetBuyerReffs(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactoryFor_CmnUser = new CmnUser_EF();
            List<vmBuyer> Buyers = null;
            // UserTypeID 1 is BuyerRefferID
            try
            {
                List<CmnUser> AllUser = GenericFactoryFor_CmnUser.GetAll().Where(x => x.IsDeleted == false && x.UserTypeID == 4 && x.CompanyID == CompanyID).ToList();
                Buyers = (from olt in AllUser
                          orderby olt.UserID descending
                          select new vmBuyer
                          {
                              UserID = olt.UserID,
                              UserName = olt.UserFullName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Buyers.OrderBy(x => x.UserID).ToList();
        }
        //public List<vmFinishingType> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        //{
        //    GenericFactoryFor_FinishingType = new PrdFinishingType_EF();
        //    List<vmFinishingType> FinishIngTypes = null;

        //    try
        //    {
        //        List<PrdFinishingType> AllFinishProcess = GenericFactoryFor_FinishingType.GetAll().Where(x => x.IsDeleted == false && x.CompanyID == CompanyID).ToList();
        //        FinishIngTypes = (from olt in AllFinishProcess
        //                          orderby olt.FInishTypeID descending
        //                          select new vmFinishingType
        //                          {
        //                              FinishTypeID = olt.FInishTypeID,
        //                              FinishTypeName = olt.FInishTypeName

        //                          }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return FinishIngTypes.OrderBy(x => x.FinishTypeID).ToList();
        //}
        #endregion

        #region Country-State-City
        public List<vmCountry> GetCountry(int CompanyID, int pageNumber, int pageSize, int IsPaging)
        {
            List<vmCountry> CountryList = null;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    CountryList = (from c in _ctxCmn.CmnAddressCountries
                                   where c.IsDeleted == false && c.CompanyID == CompanyID
                                   orderby c.CountryID ascending
                                   select new vmCountry
                                   {
                                       CountryID = c.CountryID,
                                       CustomCode = c.CustomCode,
                                       CountryName = c.CountryName

                                   }).ToList();
                    //.Skip(pageNumber)
                    //.Take(pageSize).ToList();
                }


            }
            catch (Exception e)
            {
                e.ToString();
            }

            return CountryList;
        }

        public List<vmState> GetState(int CountryID, int CompanyID, int pageNumber, int pageSize, int IsPaging)
        {
            List<vmState> StateList = null;

            try
            {

                using (_ctxCmn = new ERP_Entities())
                {
                    StateList = (from c in _ctxCmn.CmnAddressStates
                                 where c.IsDeleted == false && c.CountryID == CountryID && c.CompanyID == CompanyID
                                 orderby c.CountryID ascending
                                 select new vmState
                                 {
                                     StateID = c.StateID,
                                     CustomCode = c.CustomCode,
                                     StateName = c.StateName

                                 }).ToList();
                    //.Skip(pageNumber)
                    //.Take(pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return StateList;
        }

        public List<vmCity> GetCity(int StateID, int CompanyID, int pageNumber, int pageSize, int IsPaging)
        {
            List<vmCity> CityList = null;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    CityList = (from c in _ctxCmn.CmnAddressCities
                                where c.IsDeleted == false && c.StateID == StateID && c.CompanyID == CompanyID
                                orderby c.StateID ascending
                                select new vmCity
                                {
                                    CityID = c.CityID,
                                    CustomCode = c.CustomCode,
                                    CityName = c.CityName

                                }).ToList();
                    //.Skip(pageNumber)
                    //.Take(pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return CityList;
        }

        #endregion

        #region Designation-Department
        public List<vmDepartment> GetDepartment(int CompanyID, int pageNumber, int pageSize, int IsPaging)
        {
            List<vmDepartment> DepartmentList = null;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    DepartmentList = (from c in _ctxCmn.CmnOrganograms
                                      where c.IsDeleted == false && c.CompanyID == CompanyID
                                      orderby c.OrganogramID ascending
                                      select new vmDepartment
                                      {
                                          OrganogramID = c.OrganogramID,
                                          CustomCode = c.CustomCode,
                                          OrganogramName = c.OrganogramName

                                      }).ToList();
                    //.Skip(pageNumber)
                    //.Take(pageSize).ToList();
                }


            }
            catch (Exception e)
            {
                e.ToString();
            }

            return DepartmentList;
        }

        public List<vmDesignation> GetDesignation(int CompanyID, int pageNumber, int pageSize, int IsPaging)
        {
            List<vmDesignation> DesignationList = null;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    DesignationList = (from c in _ctxCmn.CmnUserDesignations
                                       where c.IsDeleted == false && c.CompanyID == CompanyID
                                       orderby c.UserDesignationID ascending
                                       select new vmDesignation
                                       {
                                           UserDesignationID = c.UserDesignationID,
                                           CustomCode = c.CustomCode,
                                           DesignationName = c.DesignationName

                                       }).ToList();
                    //.Skip(pageNumber)
                    //.Take(pageSize).ToList();
                }


            }
            catch (Exception e)
            {
                e.ToString();
            }

            return DesignationList;
        }
        #endregion

        #region UserWise Company
        public List<vmCmnUserWiseCompany> GetUserCompany(int userID, int companyID, int useLoggedID)
        {
            List<vmCmnUserWiseCompany> list = new List<vmCmnUserWiseCompany>();
            try
            {
                using (ERP_Entities _ctx = new ERP_Entities())
                {
                    list = (from uwc in _ctx.CmnUserWiseCompanies
                            join cm in _ctx.CmnCompanies on uwc.CompanyID equals cm.CompanyID into companyGroup
                            from cg in companyGroup.DefaultIfEmpty()
                            where uwc.IsDeleted == false && cg.IsDeleted == false && uwc.UserID == userID
                            select new vmCmnUserWiseCompany
                            {
                                UserCompanyID = uwc.UserCompanyID
                                ,
                                UserID = uwc.UserID
                                ,
                                CompanyID = uwc.CompanyID
                                ,
                                CompanyName = cg.CompanyName
                            }).ToList();

                    if (list.Count() == 0)
                    {
                        list = (from u in _ctx.CmnUsers
                                join cm in _ctx.CmnCompanies on u.CompanyID equals cm.CompanyID into companyGroup
                                from cg in companyGroup.DefaultIfEmpty()
                                where u.IsDeleted == false && u.UserID == userID
                                select new vmCmnUserWiseCompany
                                {
                                    UserCompanyID = u.CompanyID
                                ,
                                    UserID = u.UserID
                                ,
                                    CompanyID = u.CompanyID
                                ,
                                    CompanyName = cg.CompanyName
                                }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }
        #endregion UserWise Company


        //children = (from cl in _ctx.CmnItemGroups
        //            where cl.ParentID == rl.ItemGroupID
        //            select new vmParent
        //            {
        //                ID = cl.ItemGroupID,
        //                Name = cl.ItemGroupName,
        //            }).ToList()

        public List<vmGroup> GetItemGroupParenteList(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            List<vmGroup> groups = null;
            try
            {
                ERP_Entities _ctx = new ERP_Entities();
                List<CmnItemGroup> itemGroups = _ctx.CmnItemGroups.ToList();
                groups = itemGroups
                            .Where(c => c.ItemTypeID == ItemTypeID && c.CompanyID == CompanyId && c.ParentID == null)
                            .Select(c => new vmGroup()
                            {
                                ID = c.ItemGroupID,
                                AcDetailID = c.AcDetailID,
                                Name = c.CustomCode + ", " + c.ItemGroupName,
                                ParentID = c.ParentID,
                                Children = GetChildren(itemGroups, c.ItemGroupID, (int)c.AcDetailID),
                                collapsed = true
                            }).ToList();


            }
            catch (Exception)
            {

                throw;
            }
            return groups;

        }
        private static List<vmGroup> GetChildren(List<CmnItemGroup> itemGroups, int GroupId, int acDetailID)
        {
            ERP_Entities _ctx = new ERP_Entities();
            List<vmGroup> groups = null;

            try
            {
                groups = itemGroups
                                .Where(c => c.ParentID == GroupId)
                                .Select(c => new vmGroup()
                                {
                                    ID = c.ItemGroupID,
                                    AcDetailID = acDetailID,
                                    Name = c.CustomCode + ", " + c.ItemGroupName,
                                    ParentID = c.ParentID,
                                    Children = GetChildren(itemGroups, c.ItemGroupID, acDetailID),
                                    collapsed = true
                                })
                                .ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return groups;
        }

        public List<vmBranch> GetBranchDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            List<vmBranch> branchs = null;
            try
            {
                ERP_Entities _ctx = new ERP_Entities();
                List<CmnOrganogram> organograms = _ctx.CmnOrganograms.ToList();
                branchs = organograms
                            .Where(c => c.CompanyID == CompanyId && c.ParentID == null)
                            .Select(c => new vmBranch()
                            {
                                ID = c.OrganogramID,
                                Name = c.OrganogramName,
                                ParentID = c.ParentID,
                                Children = GetBranchChildren(organograms, c.OrganogramID),
                                collapsed = true
                            }).ToList();


            }
            catch
            {

                throw;
            }
            return branchs;
        }

        private static List<vmBranch> GetBranchChildren(List<CmnOrganogram> organograms, int OrganogramID)
        {
            ERP_Entities _ctx = new ERP_Entities();
            return organograms
                            .Where(c => c.ParentID == OrganogramID)
                            .Select(c => new vmBranch()
                            {
                                ID = c.OrganogramID,
                                Name = c.OrganogramName,
                                ParentID = c.ParentID,
                                Children = GetBranchChildren(organograms, c.OrganogramID),
                                collapsed = true
                            })
                            .ToList();



        }

        public List<vmTeam> GetTemsByDepartmentID(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID)
        {
            List<vmTeam> tems = null;
            _ctxCmn = new ERP_Entities();
            try
            {


                tems = _ctxCmn.CmnUserTeams.Where(c => c.DepartmentID == DepartmentID).
                    Select(c => new vmTeam()
                    {
                        TeamID = c.TeamID,
                        TeamName = c.TeamName
                    }
                    ).ToList();
            }
            catch
            {

                tems = null;
            }
            return tems;
        }

        public List<vmFinishProcess> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID)
        {
            List<vmFinishProcess> finishProces = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    finishProces = _ctx.PrdFinishingProcesses.Where(c => c.CompanyID == CompayID && c.IsDeleted == false)
                        .Select(c =>
                            new vmFinishProcess
                            {


                                id = c.FinishingProcessID,
                                label = c.FinishingProcessName

                            }).ToList();

                }
                catch
                {
                    finishProces = null;

                }
                return finishProces;
            }


        }
        public List<vmFinishProcess> GetFinishProcessByItem(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item)
        {
            List<vmFinishProcess> finishProces = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    finishProces = _ctx.PrdFinishingTypes.Where(c => c.CompanyID == CompayID && c.IsDeleted == false && c.ItemID == Item)
                        .Select(c =>
                            new vmFinishProcess
                            {
                                id = c.FinishingProcessID ?? 0

                            }).ToList();

                }
                catch
                {
                    finishProces = null;

                }
                return finishProces;
            }

        }



        public List<VmItemMater> GetDevelopmentNo(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item)
        {
            List<VmItemMater> retunList = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    retunList = _ctx.CmnItemMasters.Where(c => c.CompanyID == CompayID && c.IsDeleted == false && c.IsDevelopmentComplete == Item)
                        .Select(c =>
                            new VmItemMater
                            {
                                ItemID = c.ItemID,
                                ArticleNo = c.ArticleNo
                            }).OrderByDescending(x => x.ItemID).Take(5).ToList();
                }
                catch
                {
                    retunList = null;

                }
                return retunList;
            }

        }
        public List<vmCompany> GetCompanyForMutl(int? pageNumber, int? pageSize, int? isPaging)
        {
            List<vmCompany> _companies = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                _companies = (from com in _ctx.CmnCompanies
                              where com.IsDeleted == false
                              select new vmCompany
                              {
                                  id = com.CompanyID,
                                  label = com.CompanyName
                              }).ToList();
            }
            return _companies;
        }
        public List<vmCompany> GetCompanyPermissionListByUserID(int? userID)
        {
            List<vmCompany> _companies = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                _companies = (from ucom in _ctx.CmnUserWiseCompanies
                              join com in _ctx.CmnCompanies on ucom.CompanyID equals com.CompanyID
                              where ucom.IsDeleted == false && ucom.UserID == userID
                              select new vmCompany
                              {
                                  id = com.CompanyID,
                                  label = com.CompanyName,
                                  IsDefult = ucom.IsDefault
                              }).ToList();
            }
            return _companies;
        }

        public List<vmCoating> GetCoatingByTypeID(int? pageNumber, int? pageSize, int? isPaging, int? companyID, int? cTypeID)
        {

            List<vmCoating> _coatings = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                _coatings = (from ucom in _ctx.CmnItemCoatings
                             where ucom.IsDeleted == false && ucom.CompanyID == companyID && ucom.CoatingTypeID == cTypeID
                             select new vmCoating
                             {
                                 CoatingID = ucom.CoatinglID,
                                 CoatingName = ucom.CoatingName
                             }).ToList();
            }



            return _coatings;
        }
        public List<vmOverdyed> GetOverdyed(int? pageNumber, int? pageSize, int? isPaging, int? companyID)
        {
            List<vmOverdyed> _vmOverdyeds = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                _vmOverdyeds = (from ucom in _ctx.CmnItemOverDyeds
                                where ucom.IsDeleted == false && ucom.CompanyID == companyID
                                select new vmOverdyed
                                {
                                    OverDyedlID = ucom.OverDyedlID,
                                    OverDyedName = ucom.OverDyedName
                                }).ToList();
            }



            return _vmOverdyeds;
        }

        public List<VmItemMater> GetArticleNo(vmCmnParameters cmnParameter)
        {
            List<VmItemMater> retunList = null;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    retunList = _ctx.CmnItemMasters.Where(c => c.IsDeleted == false && c.IsDevelopmentComplete==0)
                        .Select(c =>
                            new VmItemMater
                            {
                                ItemID = c.ItemID,
                                ArticleNo = c.ArticleNo
                            }).OrderByDescending(x => x.ItemID).ToList();
                }
                catch
                {
                    retunList = null;

                }
                return retunList;
            }

        }

    }
}





