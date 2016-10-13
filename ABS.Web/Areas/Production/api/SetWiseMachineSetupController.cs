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
    [RoutePrefix("Production/api/SetWiseMachineSetup")]
    public class SetWiseMachineSetupController : ApiController
    {
        private SetWiseMachineSetupMgt objMachineSetService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public SetWiseMachineSetupController()
        {
            objMachineSetService = new SetWiseMachineSetupMgt();
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
        public IHttpActionResult GetMachine(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListMachine = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListMachine = objCmnDDLService.GetDetailsMachine(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListMachine
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDetailBox(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListBox = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListBox = objMachineSetService.GetDetailBox(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListBox
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDetailOperation(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListOperation = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListOperation = objCmnDDLService.GetDetailOperation(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListOperation
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetWiseMachineSetupMaster(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListSetWiseMachineMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSetWiseMachineMaster = objMachineSetService.GetSetWiseMachineSetupMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSetWiseMachineMaster
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetWiseMachineSetupMasterByID(object[] data)
        {
            vmItemSetSetup ListMasterByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMasterByID = objMachineSetService.GetSetWiseMachineSetupMasterByID(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMasterByID
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetWiseMachineSetupDetailByID(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListDetailByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListDetailByID = objMachineSetService.GetSetWiseMachineSetupDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListDetailByID
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveUpdateSetWiseMachineSetupMasterDetail(object[] data)
        {
            vmItemSetSetup Master = JsonConvert.DeserializeObject<vmItemSetSetup>(data[0].ToString());
            List<vmItemSetSetup> Detail = JsonConvert.DeserializeObject<List<vmItemSetSetup>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = string.Empty;
            try
            {
                result = objMachineSetService.SaveUpdateSetWiseMachineSetupMasterDetail(Master, Detail, objcmnParam);
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DelUpdateSetWiseMachineSetupMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objMachineSetService.DelUpdateSetWiseMachineSetupMasterDetail(objcmnParam);
                //}
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
