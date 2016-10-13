using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Color")]
    public partial class ColorController : ApiController
    {
        private iColorMgt_EF objColorService = null;

        public ColorController()
        {
            objColorService = new ColorMgt_EF();
        }

        [HttpPost]
        public IHttpActionResult GetColor(object[] data)
        {
            IEnumerable<vmColor> objListColor = null;
            int recordsTotal = 0;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListColor = objColorService.GetColors(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                color = objListColor
            });
        }

        // GET: GetColorById/1
        [HttpPost]
        public IHttpActionResult GetColorById(object[] data)
        {
            CmnItemColor objColor = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objColor = objColorService.GetColorsById(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objColor
            });
            //return objColor;
        }

        // POST SaveUpdateColor
        [HttpPost]
        public IHttpActionResult SaveUpdateColor(object[] data)
        {
            string result = string.Empty;
            vmColor model = JsonConvert.DeserializeObject<vmColor>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    result = objColorService.SaveUpdateColors(model, objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public IHttpActionResult DeleteUpdateColor(object[] data)
        {
            string result = string.Empty;            
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    result = objColorService.DeleteUpdateColor(objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Json(new
            {
                result
            });
        }

        // DELETE DeleteColor/1
        [Route("DeleteColor/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteColor(int id)
        {
            int result = 0;
            try
            {
                result = objColorService.DeleteColors(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
