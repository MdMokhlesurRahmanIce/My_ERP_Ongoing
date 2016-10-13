using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.Sales.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;

namespace ABS.Service.Sales.Factories
{

    public class SalesMgt : iSalesMgt
    {
        private iGenericFactory_EF<tbl_Sales> GenericFactory_EF_Sale;
         
        public SalesMgt()
        {
            GenericFactory_EF_Sale = new tbl_Sales_EF();
        }

        public IEnumerable<tbl_Sales> GetSales(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<tbl_Sales> objSales = null;
            try
            {
                objSales = GenericFactory_EF_Sale.GetAll();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSales;
        }

        public IEnumerable<tbl_Sales> GetSalesById(int Id)
        {

            IEnumerable<tbl_Sales> objSales = null;
            try
            {
                objSales = GenericFactory_EF_Sale.FindBy(m => m.SaleID == Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSales;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public int SaveUpdateSales(tbl_Sales model)
        {
            int result = 0;
            if (model.SaleID > 0)
            {
                try
                {
                    GenericFactory_EF_Sale.Update(model);
                    GenericFactory_EF_Sale.Save();
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                }
            }
            else
            {
                int NextId = GenericFactory_EF_Sale.getMaxVal_int("SaleID", "tbl_Sales");

                try
                {
                    model.SaleID = NextId;
                    GenericFactory_EF_Sale.Insert(model);
                    GenericFactory_EF_Sale.Save();
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                }
            }
            return result;
        }

        public int DeleteSales(int Id)
        {
            int result = 0;
            try
            {
                GenericFactory_EF_Sale.Delete(m => m.SaleID == Id);
                GenericFactory_EF_Sale.Save();
                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return result;
        }
    }
}
