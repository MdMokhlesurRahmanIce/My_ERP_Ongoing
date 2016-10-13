using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.Sales.Interfaces;
//using ABS.Models.Sales;

using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;


namespace ABS.Service.Sales.Factories
{
    public class PartyMgt : iPartyMgt
    {

        //private ReadOnlyFactory <Party, SalesModelContainer> ComRFactoryADO;
        //private GenericFactory<Party, SalesModelContainer> ComWFactoryADO;

        //private ReadOnlyFactory_EF<Party, SalesModelContainer> ComRFactoryEF;
        //private GenericFactory_EF<Party, SalesModelContainer> ComWFactoryEF;
        

        public PartyMgt()
        {
            //this.ComRFactoryEF = new ReadOnlyFactory<Party, SalesModelContainer>(new SalesModelContainer());
            //this.ComWFactoryEF = new GenericFactory<tbl_Customer>(new MvcArchitectureEntities());
        }

        //       /// <summary>
        //       /// Get Data From Database
        //       /// <para>Use it when to retive data through a stored procedure</para>
        //       /// </summary>
        //       public IEnumerable<tbl_Customer> GetCustomers(int? pageNumber, int? pageSize, int? IsPaging)
        //       {
        //           IEnumerable<tbl_Customer> objCustomers = null;
        //           string spQuery = string.Empty;
        //           try
        //           {
        //               object[] parameters = {
        //                   pageNumber,
        //                   pageSize,
        //                   IsPaging
        //               };

        //               spQuery = "[Get_Customers] {0}, {1}, {2}";
        //               objCustomers = CommRFactory.ExecuteQuery(spQuery, parameters);
        //           }
        //           catch (Exception e)
        //           {
        //               e.ToString();
        //           }

        //           return objCustomers;
        //       }

        //       /// <summary>
        //       /// Save Data To Database
        //       /// <para>Use it when save data through a stored procedure</para>
        //       /// </summary>
        //       public int SaveCustomer(tbl_Customer model)
        //       {
        //           int result = 0;
        //           try
        //           {
        //               object[] parameters = {
        //                   model.Name,
        //                   model.Email,
        //                   model.Mobile
        //               };
        //               string spQuery = "[Set_Customer] {0},{1},{2}";
        //               result = CommGFactory.ExecuteCommand(spQuery, parameters);
        //           }
        //           catch (Exception e)
        //           {
        //               e.ToString();
        //           }

        //           return result;
        //       }

        //       /// <summary>
        //       /// Save Data To Database
        //       /// <para>Use it when save data through a stored procedure</para>
        //       /// </summary>
        //       public int UpdateCustomer(tbl_Customer model)
        //       {
        //           int result = 0;
        //           try
        //           {
        //               object[] parameters = {
        //                   model.CustomerID,
        //                   model.Name,
        //                   model.Email,
        //                   model.Mobile
        //               };
        //               string spQuery = "[Put_Customer] {0},{1},{2},{3}";
        //               result = CommGFactory.ExecuteCommand(spQuery, parameters);
        //           }
        //           catch (Exception e)
        //           {
        //               e.ToString();
        //           }

        //           return result;
        //       }

        //       /// <summary>
        //       /// Update Delete From Database
        //       /// <para>Use it when delete data through a stored procedure</para>
        //       /// </summary>
        //       public int DeleteCustomer(int? CustomerID)
        //       {
        //           int result = 0;
        //           try
        //           {
        //               object[] parameters = {
        //                   CustomerID
        //               };
        //               string spQuery = "[Delete_Customer] {0}";
        //               result = CommGFactory.ExecuteCommand(spQuery, parameters);
        //           }
        //           catch (Exception e)
        //           {
        //               e.ToString();
        //           }

        //           return result;
        //       }






    }
}
