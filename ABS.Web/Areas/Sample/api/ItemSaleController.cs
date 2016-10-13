using ABS.Service.Sample.Interfaces;
using ABS.Service.Sample.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABS.Models;
using System.Web.Http.Description;
using System.Threading.Tasks;
using ABS.Models.ViewModel.Sample;

namespace ABS.Web.Areas.Sample.api
{
    [RoutePrefix("Sample/api/ItemSale")]
    public class ItemSaleController : ApiController
    {
        private iItemSaleMgt objItemService = null;

        public ItemSaleController()
        {
            this.objItemService = new ItemSaleMgt();
        }

        // GET: GetOutlet
        [Route("GetOutlet/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(tbl_ProductOutlet))]
        [HttpGet]
        public List<tbl_ProductOutlet> GetOutlet(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<tbl_ProductOutlet> objListOutlet = null;
            try
            {
                objListOutlet = objItemService.GetOutlet().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListOutlet;
        }

        // GET: GetItemType
        [Route("GetItemType/{id:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(tbl_ProductType))]
        [HttpGet]
        public List<tbl_ProductType> GetItemType(int? id, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<tbl_ProductType> objListType = null;
            try
            {
                objListType = objItemService.GetProductType(id).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListType;
        }

        // GET: GetItem
        [Route("GetItem/{id:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(tbl_Product))]
        [HttpGet]
        public List<tbl_Product> GetItem(int? id, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<tbl_Product> objListType = null;
            try
            {
                objListType = objItemService.GetProduct(id).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListType;
        }

        // POST Save
        //[Route("SaveSale/{outletID:int}/{typeID:int}")]
        [HttpPost]
        public HttpResponseMessage SaveSale(vmSales model)
        {
            int result = 0;
            try
            {
                result = objItemService.SaveSale(model);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: GetSoldItems
        [Route("GetSoldItems/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmSoldItems))]
        [HttpGet]
        public List<vmSoldItems> GetSoldItems(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmSoldItems> objListSoldItems = null;
            try
            {
                objListSoldItems = objItemService.GetSoldItems();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSoldItems;
        }
    }
}
