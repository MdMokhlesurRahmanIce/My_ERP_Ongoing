using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.Sample.Interfaces;
using System.Collections;



namespace ABS.Service.Sample.Factories
{

    public class EmpMasterMgtGFactory : GenericFactory<dbSampleEntities, EmpMaster> { }


    public class EmpMasterMgt:iEmpMasterMgt
    {
        private iGenericFactory<EmpMaster> GenericFactoryFor_Emp = null;

        public EmpMasterMgt()
        {
            GenericFactoryFor_Emp = new EmpMasterMgtGFactory();
        }
        

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<EmpMaster> GetEmployee(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<EmpMaster> objEmployee = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_Employee]";
                objEmployee = GenericFactoryFor_Emp.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objEmployee;
        }


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public EmpMaster GetEmployeeByID(int? id)
        {
            EmpMaster objEmployee = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("EmpMasterID", id);

                spQuery = "[Get_EmployeeSingle]";
                objEmployee = GenericFactoryFor_Emp.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objEmployee;
        }



        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveEmployee(EmpMaster model)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("EmpName", model.EmpName);        

                string spQuery = "[Set_Customer]";
                result = GenericFactoryFor_Emp.ExecuteCommand(spQuery, ht);
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
        public int UpdateEmployee(EmpMaster model)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CustomerID", model.EmpMasterID);
                ht.Add("EmpName", model.EmpName);           

                string spQuery = "[Put_Customer]";
                result = GenericFactoryFor_Emp.ExecuteCommand(spQuery, ht);
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
        public int DeleteEmployee(int? EmpMasterID)
        {
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("EmpMasterID", EmpMasterID);

                string spQuery = "[Delete_Customer]";
                result = GenericFactoryFor_Emp.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }









    }

}
