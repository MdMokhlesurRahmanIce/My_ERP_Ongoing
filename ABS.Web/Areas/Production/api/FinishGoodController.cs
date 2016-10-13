using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
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

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/FinishGood")]
    public class FinishGoodController : ApiController
    {
        iSystemCommonDDL objSystemCommonDDDl = null;
        iFinishGood _objFinishGood = null;
        iProductionDDLMgt _objProductionDDL = null;

        public FinishGoodController()
        {
            objSystemCommonDDDl = new SystemCommonDDL();
            _objFinishGood = new CmnFinishGoodMgt();
        }

        [Route("GetYarns/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmYarn> GetYarns(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {

            List<vmYarn> yarns = null;

            try
            {
                yarns = objSystemCommonDDDl.GetYarns(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return yarns;
        }

        [Route("GeWarps/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ComapnyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmWarp> GeWarps(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID)
        {

            List<vmWarp> warps = null;

            try
            {
                warps = objSystemCommonDDDl.GeWarps(pageNumber, pageSize, IsPaging, ComapnyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return warps;
        }


        [Route("GetWefts/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ComapnyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmWarp> GetWefts(int? pageNumber, int? pageSize, int? IsPaging, int? ComapnyID)
        {

            List<vmWarp> warps = null;

            try
            {
                warps = objSystemCommonDDDl.GetWefts(pageNumber, pageSize, IsPaging, ComapnyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return warps;
        }



        [Route("GetBuyers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmBuyer> GetBuyers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {

            List<vmBuyer> Buyers = null;

            try
            {
                Buyers = objSystemCommonDDDl.GetBuyers(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Buyers;
        }


        [Route("GetBuyerReffs/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmBuyer> GetBuyerReffs(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {

            List<vmBuyer> Buyers = null;

            try
            {
                Buyers = objSystemCommonDDDl.GetBuyerReffs(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Buyers;
        }


        [Route("GetSuppliers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmBuyer> GetSuppliers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {

            List<vmBuyer> Suppliers = null;

            try
            {
                Suppliers = objSystemCommonDDDl.GetSuppliers(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Suppliers;
        }


        //[Route("GetFinishProcess/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompayID:int}")]
        //[ResponseType(typeof(CmnItemMaster))]
        //[HttpGet]
        //public List<vmFinishingType> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID)
        //{

        //    List<vmFinishingType> finsihingTypes = null;

        //    try
        //    {
        //        finsihingTypes = objSystemCommonDDDl.GetFinishProcess(pageNumber, pageSize, IsPaging, CompayID).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return finsihingTypes;
        //}


        [Route("GetItemGroups/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public List<vmItemGroup> GetItemGroups(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            List<vmItemGroup> itemParents = null;

            try
            {
                itemParents = objSystemCommonDDDl.GetItemGroupsByTypeID(pageNumber, pageSize, IsPaging, ItemTypeID, CompanyId).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemParents;
        }
        [Route("GetColors/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemColor))]
        [HttpGet]
        public List<vmColor> GetColors(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            List<vmColor> Colors = null;
            try
            {
                Colors = objSystemCommonDDDl.GetAllColor(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Colors;
        }

        [Route("GetLotsForYarn/{id:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public List<vmItemLot> GetLotsForYarn(int id)
        {
            List<vmItemLot> lots = null;
            try
            {
                lots = objSystemCommonDDDl.GetLotsForYarn(id).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lots;
        }


        [Route("GetFinishWeights/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemFinishingWeight))]
        [HttpGet]
        public List<vmFinishingWeight> GetFinishWeights(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            List<vmFinishingWeight> finishingWeights = null;
            try
            {
                finishingWeights = objSystemCommonDDDl.GetFinishWeights(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finishingWeights;
        }

        [Route("GetMachines/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{departmentId:int}")]
        [ResponseType(typeof(PrdWeavingMachinConfig))]
        [HttpGet]
        public List<WeavingMachine> GetMachines(int? pageNumber, int? pageSize, int? IsPaging, int? departmentId)
        {
            _objProductionDDL = new ProductionDDLMgt();
            List<WeavingMachine> _objWeavingMachines = null;
            try
            {
                _objWeavingMachines = _objProductionDDL.GetWeavingMachines(pageNumber, pageSize, IsPaging, departmentId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objWeavingMachines;
        }
        //[Route("GetMachines/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        //[ResponseType(typeof(CmnItemColor))]
        //[HttpGet]
        //public List<vmMachine> GetMachines(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        //{
        //    List<vmMachine> _Machines = null;
        //    try
        //    {
        //        _Machines = objSystemCommonDDDl.GetMachines(pageNumber, pageSize, IsPaging, CompanyID).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return _Machines;
        //}

        [HttpPost]
        public HttpResponseMessage SaveYarn(object[] data)
        {
            List<vmYarn> Yarns = JsonConvert.DeserializeObject<List<vmYarn>>(data[0].ToString());
            Int64 YarnId = 0;
            try
            {
                YarnId = _objFinishGood.SaveYarn(Yarns);
            }
            catch (Exception e)
            {
                e.ToString();

            }
            return Request.CreateResponse(HttpStatusCode.OK, YarnId);

        }

        [HttpPost]
        public HttpResponseMessage SaveFinishGood(object[] data)
        {
            CmnItemMaster _itemMaster = JsonConvert.DeserializeObject<CmnItemMaster>(data[0].ToString());
            List<vmFinishProcess> _finishing = JsonConvert.DeserializeObject<List<vmFinishProcess>>(data[1].ToString());

            string result = "";
            try
            {
                result = _objFinishGood.SaveFinishGood(_itemMaster, _finishing);
            }
            catch (Exception)
            {

                throw;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public IHttpActionResult GetFinishGoods(object[] data)
        {
            int recordsTotal = 0;
            List<vmFinishGood> objFinishGoods = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());


            try
            {
                objFinishGoods = _objFinishGood.GetFinishGoods(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objFinishGoods
            });
            //return _finishGoodes;
        }
        [HttpPost]
        public IHttpActionResult GetFabricDevelopmentList(object[] data)
        {
            int recordsTotal = 0;
            List<vmFinishGood> objFinishGoods = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objFinishGoods = _objFinishGood.GetFabricDevelopmentList(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objFinishGoods
            });
            //return _finishGoodes;
        }




        [ResponseType(typeof(CmnItemMaster))]
        [HttpPut]
        public HttpResponseMessage DeleteFinishGood(CmnItemMaster _itemMaster)
        {
            int result = 0;
            try
            {
                result = _objFinishGood.DeleteFinishGood(_itemMaster);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetFinishGoodById/{id:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmFinishGood GetFinishGoodById(int id)
        {
            vmFinishGood _objvmFinishGood = null;
            try
            {
                _objvmFinishGood = _objFinishGood.GetFinishGoodsById(id); //_objFinishGood.GetFinishGoodById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objvmFinishGood;
        }

        [Route("GetConsumptionInfoByItemID/{id:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmConsumption> GetConsumptionInfoByItemID(int id)
        {
            List<vmConsumption> _objVmConsuptionList = null;
            try
            {

                _objVmConsuptionList = _objFinishGood.GetConsumptionInfoByItemID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objVmConsuptionList;
        }
        [Route("GetFabricDevelopmentDetailById/{id:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmFinishGood GetFabricDevelopmentDetailById(int id)
        {
            vmFinishGood _objvmFinishGood = null;
            try
            {

                _objvmFinishGood = _objFinishGood.GetFabricDevelopmentDetailById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objvmFinishGood;
        }

        [Route("GetFabricDevelopmentDetailsByID/{id:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmFinishGood GetFabricDevelopmentDetailByID(int id)
        {
            vmFinishGood _objvmFinishGood = null;
            try
            {

                _objvmFinishGood = _objFinishGood.GetFabricDevelopmentDetailsByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objvmFinishGood;
        }
        [Route("GetYarnBYId/{yarnId:int}/{CompanyID:int}")]
        [ResponseType(typeof(RndYarnCR))]
        [HttpGet]
        public vmYarn GetYarnBYId(int? yarnId, int? CompanyID)
        {
            vmYarn _objYarnCR = null;
            try
            {

                _objYarnCR = _objFinishGood.GetYarnBYId(yarnId, CompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objYarnCR;
        }
        [Route("GetDoclistByItemID/{itemID:int}/{transactionID:int}")]
        [HttpGet]
        public List<vmRndDoc> GetDoclistByItemID(int itemID, int transactionID)
        {
            List<vmRndDoc> _objYarnCR = null;
            try
            {
                _objYarnCR = _objFinishGood.GetDoclistByItemID(itemID, transactionID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objYarnCR;
        }



        [ResponseType(typeof(CmnItemMaster))]
        [HttpPost]
        public HttpResponseMessage UpdateFinishGood(object[] data)
        {
            CmnItemMaster _itemMaster = JsonConvert.DeserializeObject<CmnItemMaster>(data[0].ToString());
            List<vmFinishProcess> _finishing = JsonConvert.DeserializeObject<List<vmFinishProcess>>(data[1].ToString());

            int result = 0;
            try
            {
                result = _objFinishGood.UpdateFinishGood(_itemMaster, _finishing);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        public HttpResponseMessage uploadedFile(object[] data)
        {
            List<vmRndDoc> _DocList = JsonConvert.DeserializeObject<List<vmRndDoc>>(data[0].ToString());
            int result = 0;
            if (_DocList.Count() == 0) return Request.CreateResponse(HttpStatusCode.OK, result); 
            try
            {
                result = _objFinishGood.uploadFiles(_DocList);
                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        [HttpPost]
        public int SaveConsumpiton(object[] data)
        {
            int result = 0;
            List<vmConsumption> _objvmConsumptions = JsonConvert.DeserializeObject<List<vmConsumption>>(data[0].ToString());
            ConsumptionMaster _objConsumptonMaster = JsonConvert.DeserializeObject<ConsumptionMaster>(data[1].ToString());
            try
            {
                result = _objFinishGood.SaveConsumpiton(_objvmConsumptions, _objConsumptonMaster);

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        [Route("GetFinishProcess/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompayID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmFinishProcess> GetFinishProcess(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID)
        {

            List<vmFinishProcess> finishProcesses = null;

            try
            {
                finishProcesses = objSystemCommonDDDl.GetFinishProcess(pageNumber, pageSize, IsPaging, CompayID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finishProcesses;
        }

        [Route("GetFinishProcessByItem/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompayID:int}/{Item:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmFinishProcess> GetFinishProcessByItem(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item)
        {

            List<vmFinishProcess> finishProcesses = null;

            try
            {
                finishProcesses = objSystemCommonDDDl.GetFinishProcessByItem(pageNumber, pageSize, IsPaging, CompayID, Item).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finishProcesses;
        }


        
        [Route("GetDevelopmentNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompayID:int}/{Item:int}")]
        [ResponseType(typeof(CmnItemMaster))]


        [HttpGet]
        public List<VmItemMater> GetDevelopmentNo(int? pageNumber, int? pageSize, int? IsPaging, int? CompayID, int? Item)
        {
            
            List<VmItemMater> developmentList = null;
            try
            {
                developmentList = objSystemCommonDDDl.GetDevelopmentNo(pageNumber, pageSize, IsPaging, CompayID, Item).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return developmentList;
        }


        
        [Route("GetAcDetailIDByGroupID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{GroupID:int}")]
        [ResponseType(typeof(vmItemGroup))]
        [HttpGet]
        public  vmItemGroup GetAcDetailIDByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? GroupID)
        {
            List<vmItemGroup> itemGroup = null;
            try
            {
                itemGroup = _objFinishGood.GetAcDetailIDByGroupID(pageNumber, pageSize, IsPaging, CompanyID, GroupID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemGroup.FirstOrDefault();
        }

        [Route("GetCoatingByTypeID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{CTypeID:int}")]       
        [HttpGet]
        public List<vmCoating> GetCoatingByTypeID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? CTypeID)
        {
            List<vmCoating> _coatingobj = null;
            try
            {
                _coatingobj = objSystemCommonDDDl.GetCoatingByTypeID(pageNumber, pageSize, IsPaging, CompanyID, CTypeID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _coatingobj;
        }

        [Route("GetOverdyed/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemOverDyed))]
        [HttpGet]
        public List<vmOverdyed> GetOverdyed(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            List<vmOverdyed> _overdyeds = null;
            try
            {
                _overdyeds = objSystemCommonDDDl.GetOverdyed(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _overdyeds;
        }

    }
}
