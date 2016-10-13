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
    public class WeavingMachineMgt : iWeavingMachineConfigurationMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_EFWeavingMachinConfig = null;
        private iGenericFactory<vmWeavingLine> GenericFactory_PrdWeavingLine_vm;

        public List<vmWeavingLine> GetWeavingMachineConfigurations(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_PrdWeavingLine_vm = new PrdWeavingLine_VM();
            List<vmWeavingLine> _objWeavingMachine = null;
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


                    spQuery = "[Get_PrdWeavingMachingConfig]";
                    _objWeavingMachine = GenericFactory_PrdWeavingLine_vm.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.PrdWeavingMachinConfigs.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objWeavingMachine;
        }

        public string SaveWeavingMachineConfi(PrdWeavingMachinConfig model, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EFWeavingMachinConfig = new PrdWeavingMachinConfig_EF();
                long MainId = 0; string CustomNo = string.Empty, ConfigNo = string.Empty;
                var WeavingMachine = new PrdWeavingMachinConfig();
                try
                {
                    if (model.MachineConfigID > 0)
                    {
                        WeavingMachine = GenericFactory_EFWeavingMachinConfig.GetAll().Where(x => x.MachineConfigID == model.MachineConfigID).FirstOrDefault();
                        WeavingMachine.MachineConfigNo = model.MachineConfigNo;
                        WeavingMachine.MachineID = model.MachineID;
                        WeavingMachine.LineID = model.LineID;
                        WeavingMachine.DepartmentID = model.DepartmentID;
                        WeavingMachine.Remarks = model.Remarks;

                        WeavingMachine.CompanyID = objcmnParam.loggedCompany;
                        WeavingMachine.UpdateBy = objcmnParam.loggeduser;
                        WeavingMachine.UpdateOn = DateTime.Now;
                        WeavingMachine.UpdatePc = HostService.GetIP();
                        //ConfigNo = WeavingMachine.MachineConfigNo;
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_EFWeavingMachinConfig.getMaxID("PrdWeavingMachinConfig"));
                        CustomNo = GenericFactory_EFWeavingMachinConfig.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        //if (CustomNo == null || CustomNo == "")
                        //{
                        //    ConfigNo = MainId.ToString();
                        //}
                        //else
                        //{
                        //    ConfigNo = CustomNo;
                        //}

                        WeavingMachine = new PrdWeavingMachinConfig()
                        {
                            MachineConfigID = (int)MainId,
                            MachineConfigNo = model.MachineConfigNo,//ConfigNo.ToString(),
                            MachineID = model.MachineID,
                            DepartmentID = model.DepartmentID,
                            LineID = model.LineID,
                            Remarks = model.Remarks,
                            IsBook = false,
                            IsCorrupted = false,
                            IsMaintenance = false,
                            IsDeleted = false,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }

                    if (model.MachineConfigID > 0)
                    {
                        GenericFactory_EFWeavingMachinConfig.Update(WeavingMachine);
                        GenericFactory_EFWeavingMachinConfig.Save();
                    }
                    else
                    {
                        GenericFactory_EFWeavingMachinConfig.Insert(WeavingMachine);
                        GenericFactory_EFWeavingMachinConfig.Save();
                        GenericFactory_EFWeavingMachinConfig.updateMaxID("PrdWeavingMachinConfig", Convert.ToInt64(MainId));
                        GenericFactory_EFWeavingMachinConfig.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                    }
                    transaction.Complete();
                    result = model.MachineConfigNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteWeavingMachineConfig(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EFWeavingMachinConfig = new PrdWeavingMachinConfig_EF();
                var WeavingMachine = new PrdWeavingMachinConfig();
                try
                {
                    WeavingMachine = GenericFactory_EFWeavingMachinConfig.GetAll().Where(x => x.MachineConfigID == objcmnParam.id).FirstOrDefault();
                    WeavingMachine.IsDeleted = true;
                    WeavingMachine.CompanyID = objcmnParam.loggedCompany;
                    WeavingMachine.DeleteBy = objcmnParam.loggeduser;
                    WeavingMachine.DeleteOn = DateTime.Now;
                    WeavingMachine.DeletePc = HostService.GetIP();

                    GenericFactory_EFWeavingMachinConfig.Update(WeavingMachine);
                    GenericFactory_EFWeavingMachinConfig.Save();

                    transaction.Complete();
                    result = WeavingMachine.MachineConfigNo;
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
