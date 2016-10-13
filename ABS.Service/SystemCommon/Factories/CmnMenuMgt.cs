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
using ABS.Service.AllServiceClasses;
using ABS.Utility;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnMenuMgt : iCmnMenuMgt
    {
        private iGenericFactory<vmCmnMenu> GenericFactoryFor_Menu = null;
        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company = null;
        private iGenericFactory_EF<CmnModule> GenericFactoryFor_Module = null;

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        /// 
        public List<CmnModule> GetModuleOnDemand()
        {
            GenericFactoryFor_Module = new CmnModule_EF();
            List<CmnModule> objModuleList = null;
            string spQuery = string.Empty;
            try
            {
                //objCustomers = GenericFactoryFor_ProductOutlet.GetAll();
                var module = GenericFactoryFor_Module.GetAll();
                objModuleList = (from olt in module
                                 orderby olt.ModuleID descending
                                 select new
                                 {
                                     ModuleID = olt.ModuleID,
                                     ModuleName = olt.ModuleName

                                 }).ToList().Select(x => new CmnModule
                                 {
                                     ModuleID = x.ModuleID,
                                     ModuleName = x.ModuleName

                                 }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objModuleList.OrderBy(x => x.ModuleID).ToList();
        }

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        /// 
        public List<CmnCompany> GetCompanyOnDemand()
        {
            GenericFactoryFor_Company = new CmnCompany_EF();
            List<CmnCompany> objCompanyList = null;
            string spQuery = string.Empty;
            try
            {
                //objCustomers = GenericFactoryFor_ProductOutlet.GetAll();
                var company = GenericFactoryFor_Company.GetAll();
                objCompanyList = (from olt in company
                                      // where olt.StatusID==1
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

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            IEnumerable<vmCmnMenu> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_CmnMenu]";
                objMenues = GenericFactoryFor_Menu.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public vmCmnMenu GetMenuByID(int? id)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            vmCmnMenu objMenu = new vmCmnMenu();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", id);
                spQuery = "[Get_CmnMenuSingle]";
                objMenu = GenericFactoryFor_Menu.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMenu;
        }

        /// Static ID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveMenu(CmnMenu model)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", model.MenuID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("MenuName", model.MenuName);
                ht.Add("ModuleID", model.ModuleID);
                ht.Add("MenuPath", model.MenuPath);
                ht.Add("ReportName", model.ReportName);
                ht.Add("ReportPath", model.ReportPath);
                ht.Add("ParentID", model.ParentID);
                ht.Add("Sequence", model.Sequence);
                ht.Add("MenuTypeID", model.MenuTypeID);
                ht.Add("StatusID", model.StatusID);
                ht.Add("MenuIconCss", model.MenuIconCss);
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CreateBy", 1);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", HostService.GetIP());
                ht.Add("UpdateBy", model.UpdateBy);
                ht.Add("UpdateOn", model.UpdateOn);
                ht.Add("UpdatePc", model.UpdatePc);
                ht.Add("IsDeleted", model.IsDeleted);
                ht.Add("DeleteBy", model.DeleteBy);
                ht.Add("DeleteOn", model.DeleteOn);
                ht.Add("DeletePc", model.DeletePc);

                string spQuery = "[Set_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// Static ID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateMenu(CmnMenu model)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", model.MenuID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("MenuName", model.MenuName);
                ht.Add("ModuleID", model.ModuleID);
                ht.Add("MenuPath", model.MenuPath);
                ht.Add("ReportName", model.ReportName);
                ht.Add("ReportPath", model.ReportPath);
                ht.Add("ParentID", model.ParentID);
                ht.Add("Sequence", model.Sequence);
                ht.Add("MenuTypeID", model.MenuTypeID);
                ht.Add("StatusID", model.StatusID);
                ht.Add("MenuIconCss", model.MenuIconCss);
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CreateBy", 1);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", HostService.GetIP());
                ht.Add("UpdateBy", model.UpdateBy);
                ht.Add("UpdateOn", model.UpdateOn);
                ht.Add("UpdatePc", model.UpdatePc);
                ht.Add("IsDeleted", model.IsDeleted);
                ht.Add("DeleteBy", model.DeleteBy);
                ht.Add("DeleteOn", model.DeleteOn);
                ht.Add("DeletePc", model.DeletePc);
                string spQuery = "[Put_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteMenu(int? MenuID)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", MenuID);

                string spQuery = "[Delete_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
