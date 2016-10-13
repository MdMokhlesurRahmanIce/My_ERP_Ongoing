using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.ErrorLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Utility.ErrorExLog
{
    public class ErrorMgtGFactory : GenericFactory<ERP_Entities, vmErrorStack> { }
    public class ErrorMgt : iErrorMgt
    {
        private iGenericFactory<vmErrorStack> GenericFactoryFor_Error = null;

        public ErrorMgt()
        {
            GenericFactoryFor_Error = new ErrorMgtGFactory();
        }


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmErrorStack> GetErrorLog(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmErrorStack> objErrorLog = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_ErrorLog]";
                objErrorLog = GenericFactoryFor_Error.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objErrorLog;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveErrorLog(vmErrorStack model)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("tblErrorLog", model);
                string spQuery = "[Set_AppErrorLog]";
                result = GenericFactoryFor_Error.ExecuteCommand(spQuery, ht);
            }
            catch (Exception ex)
            {
                result = 0;
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return result;
        }
    }
}
