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

    public class CmnOrganogramMgt : iCmnOrganogramMgt
    {
        private iGenericFactory<vmCmnOrganogram> GenericFactoryFor_Organogram = null;
       
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public vmCmnOrganogram GetOrganogramByID(int? id)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            vmCmnOrganogram objOrganogram = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("OrganogramID", id);

                spQuery = "[Get_CmnOrganogramSingle]";
                objOrganogram = GenericFactoryFor_Organogram.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objOrganogram;
        }

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnOrganogram> GetOrganograms(int? CompanyID, int? loggedUserID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            IEnumerable<vmCmnOrganogram> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", CompanyID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[Get_CmnOrganogram]";
                objMenues = GenericFactoryFor_Organogram.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMenues;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveOrganogram(CmnOrganogram model)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            int result = 0;
            try
            {
                //model.IsBranch = true;
                //model.IsDepartment = true;
                //model.ProcessOutput = "Nothing";
                Hashtable ht = new Hashtable();
                ht.Add("OrganogramID", model.OrganogramID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("OrganogramName", model.OrganogramName);
                ht.Add("ParentID", model.ParentID);
                //ht.Add("IsCostCenter", model.IsCostCenter);
                //ht.Add("IsDefault", model.IsDefault);
                //ht.Add("StatusID", model.StatusID);
                ht.Add("IsBranch", model.IsBranch);
                ht.Add("IsDepartment", model.IsDepartment);
                ht.Add("ProcessOutput", model.ProcessOutput);
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CreateBy", model.CreateBy);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", HostService.GetIP());
                ht.Add("UpdateBy", model.UpdateBy);
                ht.Add("UpdateOn", model.UpdateOn);
                ht.Add("UpdatePc", model.UpdatePc);
                ht.Add("IsDeleted", model.IsDeleted);
                ht.Add("DeleteBy", model.DeleteBy);
                ht.Add("DeleteOn", model.DeleteOn);
                ht.Add("DeletePc", model.DeletePc);
                string spQuery = "[Set_CmnOrganogram]";
                result = GenericFactoryFor_Organogram.ExecuteCommand(spQuery, ht);
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
        public int UpdateOrganogram(CmnOrganogram model)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("OrganogramID", model.OrganogramID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("OrganogramName", model.OrganogramName);
                ht.Add("ParentID", model.ParentID);
                //ht.Add("IsCostCenter", model.IsCostCenter);
                //ht.Add("IsDefault", model.IsDefault);
                //ht.Add("StatusID", model.StatusID);
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CreateBy", model.CreateBy);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", HostService.GetIP());               
                ht.Add("UpdateBy", 1);
                ht.Add("UpdateOn", DateTime.Now);
                ht.Add("UpdatePc", HostService.GetIP());
                ht.Add("IsDeleted", model.IsDeleted);
                ht.Add("DeleteBy", model.DeleteBy);
                ht.Add("DeleteOn", model.DeleteOn);
                ht.Add("DeletePc", model.DeletePc);
                string spQuery = "[Put_CmnOrganogram]";
                result = GenericFactoryFor_Organogram.ExecuteCommand(spQuery, ht);
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
        public int DeleteOrganogram(int? ModuleID)
        {
            GenericFactoryFor_Organogram = new vmCmnOrganogram_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("OrganogramID", ModuleID);

                string spQuery = "[Delete_CmnOrganogram]";
                result = GenericFactoryFor_Organogram.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
