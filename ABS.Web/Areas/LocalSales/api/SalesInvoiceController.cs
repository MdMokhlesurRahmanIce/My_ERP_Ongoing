using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.LocalSales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.LocalSales.Factories;
using ABS.Service.LocalSales.Interfaces;
using ABS.Service.Sales.Factories;
using ABS.Service.Sales.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.LocalSales.api
{
    [RoutePrefix("LocalSales/api/SalesInvoice")]
    public class SalesInvoiceController : ApiController
    {

        iCmnLocalSalesMgt objLocalSalesDDDl = null;
        private iGRRMgt objGRRService = null;
        private iBookingMgt objPIService = null;
        iStockEntryMgt objStockService = null;

        [Route("GetUsers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{UserTypeID:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet]
        public List<vmBuyer> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? UserTypeID)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            List<vmBuyer> Buyers = null;

            try
            {
                Buyers = objLocalSalesDDDl.GetBuyers(pageNumber, pageSize, IsPaging, CompanyID, UserTypeID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Buyers;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartmentDetails(object[] data)
        {
            objGRRService = new GRRMgt();
            IEnumerable<vmDepartment> ListDeptDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListDeptDetail = objGRRService.GetDepartmentParentList(objcmnParam.pageNumber, objcmnParam.pageSize, objcmnParam.IsPaging, objcmnParam.DepartmentID, objcmnParam.loggedCompany).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListDeptDetail
            });
            //return objDOMaster.ToList();
        }


        [Route("GetItemGroup/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{ItemType:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet]
        public List<vmItemGroup> GetItemGroup(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            List<vmItemGroup> ItemGroups = null;

            try
            {
                ItemGroups = objLocalSalesDDDl.GetItemGroup(pageNumber, pageSize, IsPaging, CompanyID, ItemType).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ItemGroups;
        }

        [Route("GetItemTypeWithoutFinsihGood/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{ItemType:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet]
        public List<vmItemType> GetItemTypeWithoutFinsihGood(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            List<vmItemType> ItemTypes = null;

            try
            {
                ItemTypes = objLocalSalesDDDl.GetItemTypeWithoutFinsihGood(pageNumber, pageSize, IsPaging, CompanyID, ItemType).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ItemTypes;
        }



        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemMasterByGroupID(object[] data)
        {
            objPIService = new BookingMgt();
            IEnumerable<vmItem> objPIItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                string groupId = data[1].ToString();
                objPIItemMaster = objPIService.GetItemMasterById(objcmnParam, groupId, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objPIItemMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemMasterByTypeID(object[] data)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            IEnumerable<vmItem> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                string TypeId = data[1].ToString();
                objItemMaster = objLocalSalesDDDl.GetItemMasterByTypeID(objcmnParam, TypeId, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objItemMaster
            });
        }


        [Route("GetArticleDetails/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{ItemID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmArticleDetail GetArticleDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            vmArticleDetail ArticleDetails = null;

            try
            {
                ArticleDetails = objLocalSalesDDDl.GetArticleDetails(pageNumber, pageSize, IsPaging, CompanyID, ItemID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ArticleDetails;
        }

        [Route("GetArticleDetailsforDemageSale/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{ItemID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmSLDemageSale GetArticleDetailsforDemageSale(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            vmSLDemageSale ArticleDetails = null;

            try
            {
                ArticleDetails = objLocalSalesDDDl.GetArticleDetailsforDemageSale(pageNumber, pageSize, IsPaging, CompanyID, ItemID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ArticleDetails;
        }
        [Route("GetGrades/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnItemGrade))]
        [HttpGet]
        public List<vmGrade> GetGrades(int? pageNumber, int? pageSize, int? IsPaging)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            List<vmGrade> _grades = null;

            try
            {
                _grades = objLocalSalesDDDl.GetGrades(pageNumber, pageSize, IsPaging);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _grades;
        }
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCurrentStock(object[] data)
        {
            vmLSCurrentStock slCurrentStock = null;
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            vmSLCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmSLCmnParameters>(data[0].ToString());
            try
            {
                slCurrentStock = objLocalSalesDDDl.GetCurrentStock(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                slCurrentStock
            });

        }
        [Route("GetLotNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging)
        {

            objStockService = new StockEntryMgt();
            IEnumerable<CmnLot> objListLot = null;
            try
            {
                objListLot = objStockService.GetLotNo(pageNumber, pageSize, IsPaging);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListLot.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveUpdateSalesInvoice(object[] data)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            string result = string.Empty;
            SalSalesInvoiceMaster Master = JsonConvert.DeserializeObject<SalSalesInvoiceMaster>(data[0].ToString());
            List<vmLSalesInvoiceDetail> SalesInvoiceDetails = JsonConvert.DeserializeObject<List<vmLSalesInvoiceDetail>>(data[1].ToString());
            List<vmLSalesInvoiceDetail> DeleteSalesInvoiceDetails = JsonConvert.DeserializeObject<List<vmLSalesInvoiceDetail>>(data[2].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                    result = objLocalSalesDDDl.SaveUpdateSalesInvoice(Master, SalesInvoiceDetails, DeleteSalesInvoiceDetails);
               // }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public IHttpActionResult GetSalesInvoiceDetails(object[] data)
        {
            int recordsTotal = 0;
            List<vmLSalesInvoiceMaster> SalesInvoicess = null;
             objLocalSalesDDDl = new CmnLocalSalesMgt();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {

                SalesInvoicess = objLocalSalesDDDl.GetSalesInvoiceDetails(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                SalesInvoicess
            });
            //return _finishGoodes;
        }

        [HttpPost]
        public IHttpActionResult LoadSalesListForUpdate(object[] data)
        {

            List<vmSLDemageSale> ArticleDetails = null;
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ArticleDetails = objLocalSalesDDDl.LoadSalesListForUpdate(objcmnParam);
                
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {

                ArticleDetails
            });
            //return _finishGoodes;
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteSalesInvoice(object[] data)
        {
            objLocalSalesDDDl = new CmnLocalSalesMgt();
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    result = objLocalSalesDDDl.DeleteSalesInvoice(objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }
    }
}
