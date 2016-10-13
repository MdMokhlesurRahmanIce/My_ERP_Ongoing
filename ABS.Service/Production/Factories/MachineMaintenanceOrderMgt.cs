using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
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
    public class MachineMaintenanceOrderMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_WeavingMachinConfig_EF = null;
        private iGenericFactory_EF<MntMachineMaintenanceOrder> GenericFactory_MntMachineMaintenanceOrder_EF = null;
        private iGenericFactory<vmPrdWeavingMachineConfigMasterDetail> GenericFactory_vmPrdWeavingMachineConfigMasterDetail = null;


        public List<vmPrdWeavingMachineConfigMasterDetail> GetMntMachineMaintenanceOrde(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdWeavingMachineConfigMasterDetail = new vmPrdWeavingMachineConfigMasterDetail_GF();
            List<vmPrdWeavingMachineConfigMasterDetail> _objWeavingMachine = null;
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
                    ht.Add("IsTrue", objcmnParam.IsTrue);


                    spQuery = "[Get_MntMachineMaintenanceOrder]";
                    _objWeavingMachine = GenericFactory_vmPrdWeavingMachineConfigMasterDetail.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.MntMachineMaintenanceOrders.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objWeavingMachine;
        }

        public string SaveUpdateMachineMaintenanceOrder(vmPrdWeavingMachineConfigMasterDetail model, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_MntMachineMaintenanceOrder_EF = new MntMachineMaintenanceOrder_EF();
                GenericFactory_WeavingMachinConfig_EF = new PrdWeavingMachinConfig_EF();
                long MainId = 0; string CustomNo = string.Empty, MaintenanceNo = string.Empty;
                var umodel = new MntMachineMaintenanceOrder();
                var uConfig = new PrdWeavingMachinConfig();

                try
                {
                    if (model.MaintenanceID > 0)
                    {
                        umodel = GenericFactory_MntMachineMaintenanceOrder_EF.GetAll().Where(x => x.MaintenanceID == model.MaintenanceID).FirstOrDefault();
                        umodel.MachineConfigID = model.MachineConfigID;
                        umodel.DepartmentID = model.DepartmentID;
                        umodel.EmployeeID = objcmnParam.loggeduser;
                        umodel.Reason = model.Reason;
                        umodel.MaintenanceDate = model.MaintenanceDate;

                        umodel.CompanyID = objcmnParam.loggedCompany;
                        umodel.UpdateBy = objcmnParam.loggeduser;
                        umodel.UpdateOn = DateTime.Now;
                        umodel.UpdatePc = HostService.GetIP();
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_MntMachineMaintenanceOrder_EF.getMaxID("MntMachineMaintenanceOrder"));
                        CustomNo = GenericFactory_MntMachineMaintenanceOrder_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        if (CustomNo == null || CustomNo == "")
                        {
                            MaintenanceNo = MainId.ToString();
                        }
                        else
                        {
                            MaintenanceNo = CustomNo;
                        }

                        umodel = new MntMachineMaintenanceOrder()
                        {
                            MaintenanceID = (int)MainId,
                            MaintenanceNo = MaintenanceNo,
                            MachineConfigID = model.MachineConfigID,
                            DepartmentID = model.DepartmentID,
                            EmployeeID = objcmnParam.loggeduser,
                            Reason=model.Reason,
                            IsMaintenance=true,
                            MaintenanceDate=model.MaintenanceDate,
                            IsDeleted = false,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }

                    uConfig = GenericFactory_WeavingMachinConfig_EF.GetAll().Where(x => x.MachineConfigID == model.MachineConfigID).FirstOrDefault();
                    uConfig.IsMaintenance = true;

                    if (model.MaintenanceID > 0)
                    {
                        GenericFactory_MntMachineMaintenanceOrder_EF.Update(umodel);
                        GenericFactory_MntMachineMaintenanceOrder_EF.Save();
                    }
                    else
                    {
                        GenericFactory_MntMachineMaintenanceOrder_EF.Insert(umodel);
                        GenericFactory_MntMachineMaintenanceOrder_EF.Save();
                        GenericFactory_MntMachineMaintenanceOrder_EF.updateMaxID("MntMachineMaintenanceOrder", Convert.ToInt64(MainId));
                        GenericFactory_MntMachineMaintenanceOrder_EF.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                    }

                    if (uConfig !=null)
                    {
                        GenericFactory_WeavingMachinConfig_EF.Update(uConfig);
                        GenericFactory_WeavingMachinConfig_EF.Save();
                    }
                    transaction.Complete();
                    result = umodel.MaintenanceNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdateMachineMaintenanceOrder(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_MntMachineMaintenanceOrder_EF = new MntMachineMaintenanceOrder_EF();
                var MMMO = new MntMachineMaintenanceOrder();
                try
                {
                    MMMO = GenericFactory_MntMachineMaintenanceOrder_EF.GetAll().Where(x => x.MaintenanceID == objcmnParam.id).FirstOrDefault();
                    MMMO.IsDeleted = true;
                    MMMO.CompanyID = objcmnParam.loggedCompany;
                    MMMO.DeleteBy = objcmnParam.loggeduser;
                    MMMO.DeleteOn = DateTime.Now;
                    MMMO.DeletePc = HostService.GetIP();

                    GenericFactory_MntMachineMaintenanceOrder_EF.Update(MMMO);
                    GenericFactory_MntMachineMaintenanceOrder_EF.Save();

                    transaction.Complete();
                    result = MMMO.MaintenanceNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }
    }
}
