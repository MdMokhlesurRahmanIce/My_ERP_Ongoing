
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/ProductionDDL")]
    public class ProductionDDLController : ApiController
    {
        private iProductionDDLMgt objDDLService = null;
        public ProductionDDLController()
        {
            this.objDDLService = new ProductionDDLMgt();
        }

        #region GetUOM DropDown
        [Route("GetUOM/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmProductionUOMDropDown))]
        [HttpGet]
        public IEnumerable<vmProductionUOMDropDown> GetUOM(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmProductionUOMDropDown> list = null;
            try
            {
                list = objDDLService.GetAllUnit(companyID, loggedUser, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetUOM DropDown

        #region GetColor DropDown
        [Route("GetColor/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmProductionColorDropDown))]
        [HttpGet]
        public IEnumerable<vmProductionColorDropDown> GetColor(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmProductionColorDropDown> list = null;
            try
            {
                list = objDDLService.GetAllColor(companyID, loggedUser, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetColor DropDown

        #region GetItem DropDown
        [Route("GetChemicalItem/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{GroupID:int}")]
        [ResponseType(typeof(vmProductinItemDropDown))]
        [HttpGet]
        public IEnumerable<vmProductinItemDropDown> GetChemicalItem(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? GroupID)
        {
            IEnumerable<vmProductinItemDropDown> list = null;
            try
            {
                list = objDDLService.GetItemMasterByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging, GroupID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetItem DropDown

        #region GetItem DropDown
        [Route("GetItemMasterByItemID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(vmProductinItemDropDown))]
        [HttpGet]
        public IEnumerable<vmProductinItemDropDown> GetItemMasterByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            IEnumerable<vmProductinItemDropDown> list = null;
            try
            {
                list = objDDLService.GetItemMasterByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetItem DropDown

        #region GetMachine DropDown

        [Route("GetShift/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmProductionDyingHRMShiftDropDown))]
        [HttpGet]
        public IEnumerable<vmProductionDyingHRMShiftDropDown> GetShift(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmProductionDyingHRMShiftDropDown> list = null;
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    list = objDDLService.GetShift(companyID, loggedUser, pageNumber, pageSize, IsPaging);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }

        [Route("GetDyingItemMachineByItemTypeGroup/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{ItemGroupID:int}")]
        [ResponseType(typeof(vmItemMaster))]
        [HttpGet]
        public IEnumerable<vmItemMaster> GetDyingItemMachineByItemTypeGroup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? ItemGroupID)
        {
            IEnumerable<vmItemMaster> list = null;
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    list = objDDLService.GetDyingItemMachineByItemTypeGroup(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemTypeID, ItemGroupID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }


        [Route("GetMachine/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DepartmentID:int}/{ItemID:int}")]
        [HttpGet]
        public List<vmPrdWeavingMachinConfig> GetMachine(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID, int? ItemID)
        {
            List<vmPrdWeavingMachinConfig> list = null;
            Models.ViewModel.SystemCommon.vmCmnParameters param = new Models.ViewModel.SystemCommon.vmCmnParameters();
            param.loggedCompany = companyID??1;
            param.loggeduser = loggedUser ?? 1;
            param.DepartmentID = DepartmentID ?? 1;
            param.ItemID = ItemID ?? 1;
            try
            {
                    list = new ProductionDDLMgt().GetMachine(param);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }


        [Route("GetPrdDyingMachineSetupByItemMachine/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}/{MechineID:int}")]
        [HttpGet]
        public vmPrdDyingMachineSetup GetPrdDyingMachineSetupByItemMachine(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? MechineID)
        {
            vmPrdDyingMachineSetup ent = null;
            try
            {
                ent = new ProductionDDLMgt().GetPrdDyingMachineSetupByItemMachine(companyID,loggedUser,pageNumber,pageSize,IsPaging,ItemID, MechineID).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ent;
        }

        #endregion  GetMachine DropDown

        #region GetSetNo DropDown
        [Route("GetDyingSetNoByItemID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(vmProductionDyingSetNoDropDown))]
        [HttpGet]
        public IEnumerable<vmProductionDyingSetNoDropDown> GetDyingSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            IEnumerable<vmProductionDyingSetNoDropDown> list = null;
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    list = objDDLService.GetDyingSetNoByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetSetNo DropDown

        #region GetSetNo DropDown
        [Route("GetSetNoByItemID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(vmProductinItemDropDown))]
        [HttpGet]
        public IEnumerable<vmProductionPrdSetSetupDDL> GetSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            IEnumerable<vmProductionPrdSetSetupDDL> list = null;
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    list = objDDLService.GetSetNoByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion GetSetNo DropDown

        #region Get ProductinDyingProcess DropDown
        [Route("GetDyingProcessByProcessID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ProcessID:int}")]
        [ResponseType(typeof(vmProductinDyingProcessDropDown))]
        [HttpGet]
        public IEnumerable<vmProductinDyingProcessDropDown> GetDyingProcessByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID)
        {
            IEnumerable<vmProductinDyingProcessDropDown> list = null;
            try
            {
                list = objDDLService.GetDyingProcessByProcessID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ProcessID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get ProductinDyingProcess DropDown

        #region Get PrdDyingOperation DropDown
        [Route("GetDyingOperationByProcessID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ProcessID:int}")]
        [ResponseType(typeof(vmPrdDyingOperationDropDown))]
        [HttpGet]
        public IEnumerable<vmPrdDyingOperationDropDown> GetDyingOperationByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID)
        {
            IEnumerable<vmPrdDyingOperationDropDown> list = null;
            try
            {

                list = objDDLService.GetDyingOperationByProcessID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ProcessID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get PrdDyingOperation DropDown

        #region Get Department By Comapny UserID
        [Route("GetDepartmentByCompayUserID/{companyID:int}/{loggedUser:int}")]
        [ResponseType(typeof(vmDepartment))]
        [HttpGet]
        public IEnumerable<vmDepartment> GetDepartmentByCompayUserID(int? companyID, int? loggedUser)
        {
            IEnumerable<vmDepartment> list = null;
            try
            {
                list = objDDLService.GetDepartmentByCompayUserID(companyID, loggedUser);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion  Get Department By Comapny UserID

        #region Get Machine Part DropDown
        [Route("GetMachinePartByMachineID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{MachineID:int}")]
        [ResponseType(typeof(vmPrdDyingMachinePart))]
        [HttpGet]
        public IEnumerable<vmPrdDyingMachinePart> GetMachinePartByMachineID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MachineID)
        {
            IEnumerable<vmPrdDyingMachinePart> list = null;
            try
            {

                list = objDDLService.GetMachinePartByMachineID(companyID, loggedUser, pageNumber, pageSize, IsPaging, MachineID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Machine Part DropDown

        #region Get Operation
        [Route("GetOperation/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmDyingChemicalConsumptionOperation))]
        [HttpGet]
        public IEnumerable<vmDyingChemicalConsumptionOperation> GetOperation(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmDyingChemicalConsumptionOperation> list = null;
            try
            {
                list = objDDLService.GetOperation(companyID, loggedUser, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Operation

        #region GetItem By ItemID 
        //This Method also have filter with itemGroup picked up by itemType
        [Route("GetItmeByItemType/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(vmProductinItemDropDown))]
        [HttpGet]
        public IEnumerable<vmItemMaster> GetItmeByItemType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            IEnumerable<vmItemMaster> list = null;
            try
            {
                list = objDDLService.GetItmeByItemType(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Item By ItemID 

        #region Get Batch 
        [Route("GetBatchByItemID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{BatchID:int}")]
        [ResponseType(typeof(vmCmnBatch))]
        [HttpGet]
        public IEnumerable<vmCmnBatch> GetBatchByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? BatchID)
        {
            IEnumerable<vmCmnBatch> list = null;
            try
            {
                list = objDDLService.GetBatchByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging, BatchID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Batch

        #region Get PrdFinishingType 

        [Route("GetPrdFinishingType/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmPrdFinishingType))]
        [HttpGet]
        public IEnumerable<vmPrdFinishingType> GetPrdFinishingType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmPrdFinishingType> list = null;
            try
            {
                list = objDDLService.GetPrdFinishingType(companyID, loggedUser, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        #endregion Get Batch

        #region Dyeing Set IsBall Complete

        [Route("GetDyeingSetAll/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmPrdSetSetup))]
        [HttpGet]
        public List<vmPrdSetSetup> GetDyeingSetAll(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    return objDDLService.GetDyeingSetAll(companyID, loggedUser, pageNumber, pageSize, IsPaging);
                } 
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        #endregion Dyeing Set IsDye Complete
        [Route("GetDyingMachineForChemicalProcess/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmPrdSetSetup))]
        [HttpGet]
        public object GetDyingMachineForChemicalProcess(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    var list= objDDLService.GetDyingMachineForChemicalProcess(companyID, loggedUser, pageNumber, pageSize, IsPaging);
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }


        [Route("GetDyingReferenceByItemID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(vmPrdSetSetup))]
        [HttpGet]
        public object GetDyingReferenceByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging,int? ItemID)
        {
            try
            {
                using (iProductionDDLMgt objDDLService = new ProductionDDLMgt())
                {
                    var list = objDDLService.GetDyingReferenceByItemID(companyID, loggedUser, pageNumber, pageSize, IsPaging,ItemID);
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }


        

    }
}