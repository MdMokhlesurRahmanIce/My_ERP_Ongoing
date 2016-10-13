using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnMenuPermissionMgt : iCmnMenuPermissionMgt
    {
        private iGenericFactory<vmCmnMenuPermission> GenericFactoryFor_MenuPermission = null;
        //private iGenericFactory<vmCmnMenuPermission> GenericFactoryFor_MenuPermission1 = null;
        private iGenericFactory_EF<CmnMenuPermission> GenericFactoryForEF_MenuPermission = null;
        

        #region Get Menu Permission Data
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        /// (int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID);
        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParams(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, 
            int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID)
        {
            IEnumerable<vmCmnMenuPermission> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ComapnyID", companyID);
                ht.Add("loggedUser", loggedUser);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                ht.Add("ModuleID", pModuleID);
                ht.Add("UserGroupID", pUserGroupID);
                ht.Add("UserID", pUserID);
                ht.Add("OrgannogramID", pOrgannogramID);

                spQuery = "[Get_CmnMenuPermissionByParam]";
                //objMenues = GenericFactoryFor_MenuPermission1.ExecuteQuery(spQuery, ht);
                //objMenues = GenericFactoryFor_MenuPermission1.ExecuteQuery(spQuery, ht);
                objMenues = new vmCmnMenuPermission_GF().ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }
        #endregion Get Menu Permission Data

        #region Get Menu Permission Data
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        /// (int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID);
        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParamsUser(int? companyID, int? loggedUser, int? pageNumber, int? pageSize,
            int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int? pOrgannogramID)
        {
            IEnumerable<vmCmnMenuPermission> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ComapnyID", companyID);
                ht.Add("loggedUser", loggedUser);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                ht.Add("ModuleID", pModuleID);
                ht.Add("UserGroupID", pUserGroupID);
                ht.Add("UserID", pUserID);
                ht.Add("OrgannogramID", pOrgannogramID);

                spQuery = "[Get_CmnMenuPermissionByParamUser]";
                //objMenues = GenericFactoryFor_MenuPermission1.ExecuteQuery(spQuery, ht);
                objMenues = new vmCmnMenuPermission_GF().ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }
        #endregion Get Menu Permission Data

        #region SaveMenuPermission
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public int SaveMenuPermission(List<CmnMenuPermission> listModel)
        {
            GenericFactoryForEF_MenuPermission = new CmnMenuPermission_EF();
            List<CmnMenuPermission> listUpdateModel = new List<CmnMenuPermission>();
            List<CmnMenuPermission> listInsertModel = new List<CmnMenuPermission>();
            int result = 0;
            try
            {
                int NextId = GenericFactoryForEF_MenuPermission.getMaxVal_int("MenuPermissionID", "CmnMenuPermission");
                using (var transaction = new TransactionScope())
                {
                    foreach (CmnMenuPermission item in listModel)
                    {
                        item.MenuID = item.MenuID <= 0 ? null : item.MenuID;
                        item.UserID = item.UserID <= 0 ? null : item.UserID;
                        item.UserGroupID = item.UserGroupID <= 0 ? null : item.UserGroupID;
                        item.CompanyID = item.CompanyID <= 0 ? null : item.CompanyID;
                        item.StatusID = item.StatusID <= 0 ? null : item.StatusID;
                        item.OrganogramID = item.OrganogramID <= 0 ? null : item.OrganogramID;
                        item.IsDeleted = false;
                        if (item.MenuPermissionID == 0 && (item.EnableView == true || item.EnableInsert == true || item.EnableUpdate == true || item.EnableDelete == true))
                        {
                            item.MenuPermissionID = NextId++;
                            listInsertModel.Add(item);
                        }
                        else if (item.MenuPermissionID > 0) listUpdateModel.Add(item);
                        else item.MenuID = -1;

                    }
                    if (listInsertModel.Count() > 0)
                        GenericFactoryForEF_MenuPermission.InsertList(listInsertModel);
                    if (listUpdateModel.Count()>0)
                        GenericFactoryForEF_MenuPermission.UpdateList(listUpdateModel);
                    GenericFactoryForEF_MenuPermission.Save();
                    transaction.Complete();
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = -101;
            }
            finally
            {
                result = 1;
            }
          
            return result;
        }
        #endregion SaveMenuPermission

    }
}
