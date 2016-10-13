using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnTransactionTypesMgt : iCmnTransactionTypeMgt
    {
        private iGenericFactory<CmnTransactionType> GenericFactoryFor_Transaction = null;

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<CmnTransactionType> GetTransactionTypes(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Transaction = new CmnTransactionType_GF();
            IEnumerable<CmnTransactionType> objTransaction = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_CmnTransactionType]";
                objTransaction = GenericFactoryFor_Transaction.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objTransaction;
        }
    }
}
