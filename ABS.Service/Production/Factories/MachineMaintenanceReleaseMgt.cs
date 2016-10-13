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
    public class MachineMaintenanceReleaseMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_WeavingMachinConfig_EF = null;
        private iGenericFactory_EF<MntMachineMaintenanceOrder> GenericFactory_MntMachineMaintenanceOrder_EF = null;
        private iGenericFactory<vmPrdWeavingMachineConfigMasterDetail> GenericFactory_vmPrdWeavingMachineConfigMasterDetail = null;



        public IEnumerable<vmPrdWeavingMachineConfigMasterDetail> GetMaintenanceMachine(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmPrdWeavingMachineConfigMasterDetail> objMMOMachine = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objMMOMachine = (from IM in _ctxCmn.MntMachineMaintenanceOrders.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false)
                                     where objcmnParam.IsTrue == false ? true : IM.IsMaintenance == objcmnParam.IsTrue
                                     orderby IM.MaintenanceID
                                     select new
                                     {
                                         MaintenanceID = IM.MaintenanceID,
                                         MaintenanceNo = IM.MaintenanceNo
                                     }).ToList().Select(x => new vmPrdWeavingMachineConfigMasterDetail
                                   {
                                       MaintenanceID = x.MaintenanceID,
                                       MaintenanceNo = x.MaintenanceNo
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMMOMachine;
        }

        public vmPrdWeavingMachineConfigMasterDetail GetMaintenanceMachineData(vmCmnParameters objcmnParam)
        {
            vmPrdWeavingMachineConfigMasterDetail objMMOMachineData = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objMMOMachineData = (from IM in _ctxCmn.MntMachineMaintenanceOrders.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false && x.MaintenanceID == objcmnParam.id)
                                         join MC in _ctxCmn.PrdWeavingMachinConfigs on IM.MachineConfigID equals MC.MachineConfigID
                                         join CO in _ctxCmn.CmnOrganograms on IM.DepartmentID equals CO.OrganogramID
                                         select new
                                         {
                                             MachineConfigID = IM.MachineConfigID,
                                             MachineConfigNo = MC.MachineConfigNo,
                                             DepartmentID = IM.DepartmentID,
                                             OrganogramName = CO.OrganogramName

                                         }).ToList().Select(x => new vmPrdWeavingMachineConfigMasterDetail
                                         {
                                             MachineConfigID = x.MachineConfigID,
                                             MachineConfigNo = x.MachineConfigNo,
                                             DepartmentID = x.DepartmentID,
                                             OrganogramName = x.OrganogramName
                                         }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMMOMachineData;
        }

        public string SaveUpdateMachineMaintenanceRelease(vmPrdWeavingMachineConfigMasterDetail model, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_MntMachineMaintenanceOrder_EF = new MntMachineMaintenanceOrder_EF();
                GenericFactory_WeavingMachinConfig_EF = new PrdWeavingMachinConfig_EF();                
                var umodel = new MntMachineMaintenanceOrder();
                var uConfig = new PrdWeavingMachinConfig();

                try
                {

                    umodel = GenericFactory_MntMachineMaintenanceOrder_EF.GetAll().Where(x => x.MaintenanceID == model.MaintenanceID).FirstOrDefault();
                    umodel.IsReleased = true;
                    umodel.IsMaintenance = false;
                    umodel.MaintenanceEmployeeID = objcmnParam.loggeduser;
                    umodel.ReleaseRemarks = model.ReleaseRemarks;
                    umodel.ReleaseDate = model.ReleaseDate;

                    uConfig = GenericFactory_WeavingMachinConfig_EF.GetAll().Where(x => x.MachineConfigID == model.MachineConfigID).FirstOrDefault();
                    uConfig.IsMaintenance = false;

                    if (umodel !=null)
                    {
                        GenericFactory_MntMachineMaintenanceOrder_EF.Update(umodel);
                        GenericFactory_MntMachineMaintenanceOrder_EF.Save();
                    }                    

                    if (uConfig != null)
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
    }
}
