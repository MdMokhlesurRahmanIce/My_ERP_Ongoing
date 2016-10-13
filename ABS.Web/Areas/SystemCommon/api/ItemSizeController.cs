using ABS.Models;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/ItemSize")]
    public class ItemSizeController : ApiController
    {
        private iItemSizeMgt objItemSizeService = null;

        public ItemSizeController()
        {
            objItemSizeService = new ItemSizeMgt();            
        }

        #region ItemSizeSettings 

        [Route("GetItemSize/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnItemSize))]
        [HttpGet]
        public IEnumerable<CmnItemSize> GetItemSize(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemSize> objListItemSize = null;
            try
            {
                objListItemSize = objItemSizeService.GetItemSize(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListItemSize;
        }

        // GET: GetItemSizeById/1
        [Route("GetItemSizeById/{id:int}")]
        [ResponseType(typeof(CmnItemSize))]
        [HttpGet]
        public CmnItemSize GetItemSizeById(int id)
        {
            CmnItemSize objItemSize = null;
            try
            {
                objItemSize = objItemSizeService.GetItemSizeById(id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemSize;
        }

        // POST SaveUpdateItemSize
        [ResponseType(typeof(CmnItemSize))]
        [HttpPost]
        public HttpResponseMessage SaveUpdateItemSize(CmnItemSize model)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = objItemSizeService.SaveUpdateItemSize(model);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteItemSize/1
        [Route("DeleteItemSize/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteItemSize(int id)
        {
            int result = 0;
            try
            {
                result = objItemSizeService.DeleteItemSize(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion  ItemSizeSettings
    }
}
