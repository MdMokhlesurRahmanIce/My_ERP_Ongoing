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

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/PackingList")]
    public class PackingListController : ApiController
    {
        private PackingListMgt packingListService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public PackingListController()
        {
            packingListService = new PackingListMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPINO(object[] data)
        {
            IEnumerable<vmPI> ListPINo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListPINo = objCmnDDLService.GetAllPI(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListPINo
            });
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
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStyleNo(object[] data)
        {
            List<vmStyle> Styles = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                Styles = objCmnDDLService.GetStyleNoes(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                Styles
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPIBasedData(object[] data)
        {
            vmFinishingPackingListMasterDetail PIData = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                PIData = packingListService.GetPIBasedData(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                PIData
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPackingMasterList(object[] data)
        {
            List<vmFinishingPackingListMasterDetail> ListPMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListPMaster = packingListService.GetPackingMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListPMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPackingListDetailByID(object[] data)
        {
            List<vmFinishingPackingListMasterDetail> PDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                PDetail = packingListService.GetPackingListDetailByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                PDetail
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdatePackingListMasterDetail(object[] data)
        {
            vmFinishingPackingListMasterDetail Master = JsonConvert.DeserializeObject<vmFinishingPackingListMasterDetail>(data[0].ToString());
            List<vmFinishingPackingListMasterDetail> Detail = JsonConvert.DeserializeObject<List<vmFinishingPackingListMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = string.Empty;
            try
            {
                result = packingListService.SaveUpdatePackingListMasterDetail(Master, Detail, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdatePackingMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = packingListService.DeleteUpdatePackingMasterDetail(objcmnParam);
                //}
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
    }
}
