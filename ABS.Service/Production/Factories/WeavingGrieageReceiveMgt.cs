using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class WeavingGrieageReceiveMgt : iWeavingGrieageReceiveMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmWeavingGriage> GFactory_VM_WeavingMacheGrieage = null;
        private iGenericFactory_EF<PrdWeavingMRRMaster> GFactory_WeavingMRRMaster = null;

        public vmWeavingGriage GetWeavingMachines(vmCmnParameters objcmnParam)
        {
            GFactory_VM_WeavingMacheGrieage = new preWeavingMachineGriage_VM();

            vmWeavingGriage _vmWeavingMachineGrieage = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("MachineConfiqID", objcmnParam.id);
                    spQuery = "[Get_PrdWeavingMachineByIdMCID]";
                    _vmWeavingMachineGrieage = GFactory_VM_WeavingMacheGrieage.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmWeavingMachineGrieage;
        }

        public int SaveWeavingGriage(PrdWeavingMRRMaster model, vmCmnParameters objcmnParam)
        {
            int result = 0;
            using (TransactionScope transaction = new TransactionScope())
            {
                GFactory_WeavingMRRMaster = new PrdWeavingMRRMaster_EF();
                string CustomNo = string.Empty, WeavingMRRNo=string.Empty;
                try
                {
                    if (model.WeavingMRRID == 0)
                    {
                        Int64 NextId = GFactory_WeavingMRRMaster.getMaxVal_int64("WeavingMRRID", "PrdWeavingMRRMaster");
                        CustomNo = GFactory_WeavingMRRMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        if (CustomNo == null || CustomNo == "")
                        {
                            WeavingMRRNo = NextId.ToString();
                        }
                        else
                        {
                            WeavingMRRNo = CustomNo;
                        }

                        model.WeavingMRRID = NextId;
                        model.WeavingMRRNo = WeavingMRRNo;
                        model.IsIssued = true;
                        model.IsFinishid = false;
                        model.IsReceived = false;
                        model.DepartmentID = objcmnParam.DepartmentID;
                        model.StatusID = objcmnParam.loggeduser;
                        model.CompanyID = objcmnParam.loggedCompany;
                        model.CreateBy = objcmnParam.loggeduser;
                        model.CreateOn = DateTime.Now;
                        model.CreatePc = HostService.GetIP();

                        GFactory_WeavingMRRMaster.Insert(model);
                        GFactory_WeavingMRRMaster.Save();
                    }
                    else
                    {
                        PrdWeavingMRRMaster _objWeavingMRRMaster = GFactory_WeavingMRRMaster.GetAll().Where(x => x.WeavingMRRID == model.WeavingMRRID).FirstOrDefault();
                        _objWeavingMRRMaster.DoffingNo = model.DoffingNo;
                        _objWeavingMRRMaster.MachineConfigID = model.MachineConfigID;
                        _objWeavingMRRMaster.ItemID = model.ItemID;
                        _objWeavingMRRMaster.SetID = model.SetID;
                        _objWeavingMRRMaster.SizeMRRID = model.SizeMRRID;
                        _objWeavingMRRMaster.UnitID = model.UnitID;
                        _objWeavingMRRMaster.ShiftID = model.ShiftID;
                        _objWeavingMRRMaster.OperatorID = model.OperatorID;
                        _objWeavingMRRMaster.Qty = model.Qty;                        
                        _objWeavingMRRMaster.Remarks = model.Remarks;

                        _objWeavingMRRMaster.UpdateOn = DateTime.Now;
                        _objWeavingMRRMaster.UpdatePc = HostService.GetIP();
                        _objWeavingMRRMaster.UpdateBy = objcmnParam.loggeduser;

                        GFactory_WeavingMRRMaster.Update(_objWeavingMRRMaster);
                        GFactory_WeavingMRRMaster.Save();
                    }
                    transaction.Complete();
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                }                
            }

            return result;
        }
        public int DeleteWeavingGriageById(vmCmnParameters objcmnParam)
        {
            int result = 0;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    GFactory_WeavingMRRMaster = new PrdWeavingMRRMaster_EF();
                    PrdWeavingMRRMaster _objWeavingMRRMaster = GFactory_WeavingMRRMaster.GetAll().Where(x => x.WeavingMRRID == objcmnParam.id).FirstOrDefault();
                    _objWeavingMRRMaster.IsDeleted = true;
                    GFactory_WeavingMRRMaster.Update(_objWeavingMRRMaster);
                    GFactory_WeavingMRRMaster.Save();

                    transaction.Complete();
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                } 
            }
            return result;

        }        

        public List<vmWeavingGriage> WeavingGriageDetails(vmCmnParameters objcmnParam, out int recordsTotal)
        {

            GFactory_VM_WeavingMacheGrieage = new preWeavingMachineGriage_VM();

            List<vmWeavingGriage> _objWeavingGriages = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("PageNo", objcmnParam.pageNumber);
                    ht.Add("RowCountPerPage", objcmnParam.pageSize);
                    ht.Add("IsPaging", objcmnParam.IsPaging);


                    spQuery = "[Get_PrdWeavingGriageDetails]";
                    _objWeavingGriages = GFactory_VM_WeavingMacheGrieage.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.PrdWeavingMRRMasters.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objWeavingGriages;
        }
        public vmWeavingGriage GetWeavingGriageDetailsById(vmCmnParameters objcmnParam)
        {
            GFactory_VM_WeavingMacheGrieage = new preWeavingMachineGriage_VM();

            vmWeavingGriage _objWeavingGriage = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("WeavingMRRID", objcmnParam.id);
                    spQuery = "[Get_PrdWeavingGriageDetailsByID]";
                    _objWeavingGriage = GFactory_VM_WeavingMacheGrieage.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objWeavingGriage;
        }


    }
}
