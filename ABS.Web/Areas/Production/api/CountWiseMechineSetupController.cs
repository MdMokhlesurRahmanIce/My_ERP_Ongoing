using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
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
    [RoutePrefix("Production/api/CountWiseMechineSetup")]
    public class CountWiseMechineSetupController : ApiController
    {
        private CountWiseMechineSetupMgt objMachineSetService = null;

        public CountWiseMechineSetupController()
        {
            objMachineSetService = new CountWiseMechineSetupMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCount(object[] data)
        {
            IEnumerable<CmnItemCount> ListCount = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListCount = objMachineSetService.GetCount(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListCount
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMachinSetup(object[] data)
        {
            IEnumerable<PrdBallMachineSetup> ListMachineSet = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMachineSet = objMachineSetService.GetMachinSetup(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMachineSet
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMachinSetByID(object[] data)
        {
            IEnumerable<PrdBallMachineSetup> ListMachineSet = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMachineSet = objMachineSetService.GetMachinSetup(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMachineSet
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCountToCheck(object[] data)
        {
            IEnumerable<PrdBallMachineSetup> ListCountExist = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListCountExist = objMachineSetService.GetCountToCheck(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListCountExist
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveUpdateCountWiseMachineSetup(object[] data)
        {
            vmPrdBallMachineSetup model = JsonConvert.DeserializeObject<vmPrdBallMachineSetup>(data[0].ToString());
            string result = string.Empty;
            try
            {
                result = objMachineSetService.SaveUpdateCountWiseMachineSetup(model);
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
    }
}
