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
    public class CmnModuleMgt : iCmnModuleMgt
    {
        private iGenericFactory<vmCmnModule> GenericFactoryFor_Module = null;
        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company = null;

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
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
                                  where olt.IsDeleted==false
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
        public IEnumerable<vmCmnModule> GetModules(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            IEnumerable<vmCmnModule> objModules = null;
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
                objModules = GenericFactoryFor_Module.ExecuteQuery(spQuery, ht);
                
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objModules;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public vmCmnModule GetModuleByID(int? id)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            vmCmnModule objModule = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ModuleID", id);

                spQuery = "[Get_CmnModuleSingle]";
                objModule = GenericFactoryFor_Module.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objModule;
        }

        /// Static ID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveModule(CmnModule model)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ModuleID", model.ModuleID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("ModuleName", model.ModuleName);
                ht.Add("Description", model.Description);
                ht.Add("Sequence", model.Sequence);
                ht.Add("ImageURL", model.ImageURL);
                ht.Add("ModulePath", model.ModulePath);
                ht.Add("StatusID", 1);
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


                string spQuery = "[Set_CmnModule]";
                result = GenericFactoryFor_Module.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateModule(CmnModule model)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ModuleID", model.ModuleID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("ModuleName", model.ModuleName);
                ht.Add("Description", model.Description);
                ht.Add("Sequence", model.Sequence);
                ht.Add("ImageURL", model.ImageURL);
                ht.Add("ModulePath", model.ModulePath);
                ht.Add("StatusID", 1);
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CreateBy", 1);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", HostService.GetIP());
                ht.Add("UpdateBy", 1);
                ht.Add("UpdateOn", DateTime.Now);
                ht.Add("UpdatePc", HostService.GetIP());
                ht.Add("IsDeleted", model.IsDeleted);
                ht.Add("DeleteBy", model.DeleteBy);
                ht.Add("DeleteOn", model.DeleteOn);
                ht.Add("DeletePc", model.DeletePc);

                string spQuery = "[Put_CmnModule]";
                result = GenericFactoryFor_Module.ExecuteCommand(spQuery, ht);
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
        public int DeleteModule(int? ModuleID)
        {
            GenericFactoryFor_Module = new vmCmnModule_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ModuleID", ModuleID);

                string spQuery = "[Delete_CmnModule]";
                result = GenericFactoryFor_Module.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
