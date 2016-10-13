using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/CodewiseMachineSetup")]
    public class CodewiseMachineSetupController : ApiController
    {
        private iCodewiseMachineSetupMgt objCodewiseMachineSetup = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public CodewiseMachineSetupController()
        {
            objCodewiseMachineSetup = new CodewiseMachineSetupMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetArticle(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListArticle = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListArticle = objCmnDDLService.GetArticle(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListArticle
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMachineSetupList(object[] data)
        {
            IEnumerable<PrdWeavingMachineSetup> masterDataList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                masterDataList = objCodewiseMachineSetup.GetMachineSetupList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                masterDataList
            });
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateCodewiseMachineSetup(object[] data)
        {
            PrdWeavingMachineSetup itemMaster = JsonConvert.DeserializeObject<PrdWeavingMachineSetup>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objCodewiseMachineSetup.SaveUpdateCodewiseMachineSetup(itemMaster, objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateWeavingMachineSetup(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objCodewiseMachineSetup.DeleteUpdateWeavingMachineSetup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                result
            });
        }

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetMachineSetupInfo(object[] data)
        //{
        //    IEnumerable<vmWeavingMachineSetup> objMachineSetupInfo = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        objMachineSetupInfo = objCodewiseMachineSetup.GetMachineSetupInfo(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        objMachineSetupInfo
        //    });
        //}
    }
}
