using ABS.Service.GlobalMgt.Factories;
using ABS.Service.GlobalMgt.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Dashboard")]
    public class DashboardController : ApiController
    {
        iGlobalMgt objGlobalTotal = null;

        public DashboardController()
        {
            objGlobalTotal = new GlobalMgt();
        }

        [Route("GetUserTotal/{companyId:int}")]
        [HttpGet]
        public IHttpActionResult GetUserTotal(int companyId)
        {
            int totalUser = 0;
            try
            {
                totalUser = Convert.ToInt32(objGlobalTotal.GetUserTotal());
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                totalUser
            });
        }
    }
}
