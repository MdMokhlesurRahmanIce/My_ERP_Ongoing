using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sample;
using ABS.Service.Sample.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Sample.Factories
{

    public class ProductOutlet : GenericFactory_EF<dbSampleEntities, tbl_ProductOutlet> { }
    public class ProductType : GenericFactory_EF<dbSampleEntities, tbl_ProductType> { }
    public class Product : GenericFactory_EF<dbSampleEntities, tbl_Product> { }
    public class Sale : GenericFactory_EF<dbSampleEntities, tbl_Sales> { }
    public class SalesItem : GenericFactory_EF<dbSampleEntities, tbl_SalesItem> { }

    public class ItemSaleMgt : iItemSaleMgt
    {
        private iGenericFactory_EF<tbl_ProductOutlet> GenericFactoryFor_ProductOutlet = null;
        private iGenericFactory_EF<tbl_ProductType> GenericFactoryFor_ProductType = null;
        private iGenericFactory_EF<tbl_Product> GenericFactoryFor_Product = null;
        private iGenericFactory_EF<tbl_Sales> GenericFactoryFor_Sale = null;
        private iGenericFactory_EF<tbl_SalesItem> GenericFactoryFor_SalesItem = null;

        public ItemSaleMgt()
        {
            GenericFactoryFor_ProductOutlet = new ProductOutlet();
            GenericFactoryFor_ProductType = new ProductType();
            GenericFactoryFor_Product = new Product();
            GenericFactoryFor_Sale = new Sale();
            GenericFactoryFor_SalesItem = new SalesItem();
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<tbl_ProductOutlet> GetOutlet()
        {
            List<tbl_ProductOutlet> objSoldItems = null;
            string spQuery = string.Empty;
            try
            {
                //objCustomers = GenericFactoryFor_ProductOutlet.GetAll();
                var outlet = GenericFactoryFor_ProductOutlet.GetAll();
                objSoldItems = (from olt in outlet
                                orderby olt.OutletID descending
                                select new
                                {
                                    OutletID = olt.OutletID,
                                    OutletName = olt.OutletName

                                }).ToList().Select(x => new tbl_ProductOutlet
                                {
                                    OutletID = x.OutletID,
                                    OutletName = x.OutletName

                                }).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objSoldItems;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<tbl_ProductType> GetProductType(int? id)
        {
            List<tbl_ProductType> objProductType = null;
            string spQuery = string.Empty;
            try
            {
                //objProductType = GenericFactoryFor_ProductType.FindBy(t => t.OutletID == id);
                var productType = GenericFactoryFor_ProductType.FindBy(t => t.OutletID == id);
                objProductType = (from pty in productType
                                  orderby pty.TypeID descending
                                  select new
                                  {
                                      TypeID = pty.TypeID,
                                      TypeName = pty.TypeName

                                  }).ToList().Select(x => new tbl_ProductType
                                  {
                                      TypeID = x.TypeID,
                                      TypeName = x.TypeName

                                  }).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objProductType;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<tbl_Product> GetProduct(int? id)
        {
            List<tbl_Product> objProduct = null;
            string spQuery = string.Empty;
            try
            {
                //objCustomers = GenericFactoryFor_Product.FindBy(t => t.TypeID == id);
                var product = GenericFactoryFor_Product.FindBy(t => t.TypeID == id);
                objProduct = (from prd in product
                              orderby prd.ProductID descending
                              select new
                              {
                                  ProductID = prd.ProductID,
                                  ProductName = prd.ProductName,
                                  Price = prd.Price

                              }).ToList().Select(x => new tbl_Product
                              {
                                  ProductID = x.ProductID,
                                  ProductName = x.ProductName,
                                  Price = x.Price

                              }).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objProduct;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveSale(vmSales vmodel)
        {
            int result = 0, saleItemID = 0, saleID = 0;
            tbl_Sales sale = new tbl_Sales(); tbl_Product objItem = new tbl_Product();
            var items = new List<tbl_SalesItem>();
            string[] valueItems = null; string strItems = string.Empty;
            decimal price = 0; decimal vat = 25;

            try
            {
                strItems = vmodel.Items.ToString();
                valueItems = strItems.Split(',');
                if (strItems != "")
                {
                    //---for SalesMaster--------
                    saleID = GenericFactoryFor_Sale.getMaxVal_int("SaleID", "tbl_Sales");
                    sale.SaleID = saleID;
                    sale.SaleNo = "SL-" + saleID; ;
                    sale.SaleDate = DateTime.UtcNow;
                    sale.OutletID = vmodel.OutletID;
                    sale.TypeID = vmodel.TypeID;

                    //---for SalesDetails--------
                    saleItemID = GenericFactoryFor_SalesItem.getMaxVal_int("SaleItemID", "tbl_SalesItem");
                    for (int i = 0; i < valueItems.Length; i++)
                    {
                        int productID = Convert.ToInt32(valueItems[i]);
                        objItem = GenericFactoryFor_Product.FindBy(t => t.ProductID == productID).FirstOrDefault();
                        price = Convert.ToDecimal(objItem.Price);
                        var item = new tbl_SalesItem
                        {
                            SaleItemID = saleItemID,
                            ProductID = productID,
                            SaleID = saleID,
                            Price = price + vat
                        };

                        items.Add(item);
                        saleItemID++;

                    }
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (sale != null)
                        {
                            //GenericFactoryFor_Sale.Insert(sale);
                            //GenericFactoryFor_Sale.Save();
                        }

                        if (items != null)
                        {
                            GenericFactoryFor_SalesItem.InsertList(items.ToList());
                            GenericFactoryFor_SalesItem.Save();
                        }

                        transaction.Complete();
                        result = 1;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                result = 0;
                ErrorLog.Log(ex);
                ex.ToString(); 
            }

            return result;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public List<vmSoldItems> GetSoldItems()
        {
            List<vmSoldItems> objListSoldItems = null;
            try
            {
                var salesItem = GenericFactoryFor_SalesItem.GetAll();
                var product = GenericFactoryFor_Product.GetAll();
                var sale = GenericFactoryFor_Sale.GetAll();
                var outlet = GenericFactoryFor_ProductOutlet.GetAll();
                var productType = GenericFactoryFor_ProductType.GetAll();

                objListSoldItems = (from slt in salesItem
                                    join prd in product on slt.ProductID equals prd.ProductID
                                    join sal in sale on slt.SaleID equals sal.SaleID
                                    join olt in outlet on sal.OutletID equals olt.OutletID
                                    join pty in productType on sal.TypeID equals pty.TypeID
                                    orderby slt.SaleID descending
                                    select new
                                    {
                                        SaleNo = sal.SaleNo,
                                        OutletName = olt.OutletName,
                                        TypeName = pty.TypeName,
                                        ProductName = prd.ProductName,
                                        netPrice = (decimal)prd.Price,
                                        grossPrice = (decimal)slt.Price,
                                        SaleDate = (DateTime)sal.SaleDate
                                    }).ToList().Select(xt => new vmSoldItems
                                    {
                                        SaleNo = xt.SaleNo,
                                        OutletName = xt.OutletName,
                                        TypeName = xt.TypeName,
                                        ProductName = xt.ProductName,
                                        netPrice = (decimal)xt.netPrice,
                                        grossPrice = (decimal)xt.grossPrice,
                                        SaleDate = (DateTime)xt.SaleDate

                                    }).Take(3).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objListSoldItems;
        }
    }
}
