using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.Sample.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Service.Sample.Factories
{
    public class CustomerMgtGFactory : GenericFactory<dbSampleEntities, tbl_Customer> { }

    public class CustomerMgt : iCustomerMgt
    {
        private iGenericFactory<tbl_Customer> GenericFactoryFor_Customer = null;

        public CustomerMgt()
        {
            GenericFactoryFor_Customer = new CustomerMgtGFactory();
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<tbl_Customer> GetCustomers(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<tbl_Customer> objCustomers = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_Customers]";
                objCustomers = GenericFactoryFor_Customer.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCustomers;
        }


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public tbl_Customer GetCustomerByID(int? id)
        {
            tbl_Customer objCustomer = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CustomerID", id);

                spQuery = "[Get_CustomersSingle]";
                objCustomer = GenericFactoryFor_Customer.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCustomer;
        }


        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveCustomer(tbl_Customer model)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("Name", model.Name);
                ht.Add("Email", model.Email);
                ht.Add("Mobile", model.Mobile);

                string spQuery = "[Set_Customer]";
                result = GenericFactoryFor_Customer.ExecuteCommand(spQuery, ht);
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
        public int UpdateCustomer(tbl_Customer model)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CustomerID", model.CustomerID);
                ht.Add("Name", model.Name);
                ht.Add("Email", model.Email);
                ht.Add("Mobile", model.Mobile);

                string spQuery = "[Put_Customer]";
                result = GenericFactoryFor_Customer.ExecuteCommand(spQuery, ht);
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
        public int DeleteCustomer(int? CustomerID)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CustomerID", CustomerID);

                string spQuery = "[Delete_Customer]";
                result = GenericFactoryFor_Customer.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
