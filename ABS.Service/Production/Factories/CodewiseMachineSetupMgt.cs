using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace ABS.Service.Production.Factories
{
    public class CodewiseMachineSetupMgt : iCodewiseMachineSetupMgt
    {
        private iGenericFactory_EF<PrdWeavingMachineSetup> GenericFactory_EF_PrdWeavingMachineSetup = null;
        //private iGenericFactory<vmWeavingMachineSetup> GenericFactory_WeavingMachineSetup_GF = null;

        public IEnumerable<PrdWeavingMachineSetup> GetMachineSetupList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_EF_PrdWeavingMachineSetup = new PrdWeavingMachineSetup_EF();
            IEnumerable<PrdWeavingMachineSetup> objCodewiseMachineSetupMaster = null;
            recordsTotal = 0;
            try
            {
                objCodewiseMachineSetupMaster = GenericFactory_EF_PrdWeavingMachineSetup.GetAll().Select(m => new PrdWeavingMachineSetup
                {
                    MachineSetupID = m.MachineSetupID,
                    ItemID = m.ItemID,
                    Selvedge = m.Selvedge,
                    Brackrest = m.Brackrest,
                    ShadeAngle = m.ShadeAngle,
                    SFHight = m.SFHight,
                    CompanyID = m.CompanyID
                }).Where(m => m.CompanyID == objcmnParam.loggedCompany).ToList();
                recordsTotal = objCodewiseMachineSetupMaster.Count();
                objCodewiseMachineSetupMaster = objCodewiseMachineSetupMaster.OrderBy(x => x.MachineSetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCodewiseMachineSetupMaster;
        }
        //public IEnumerable<vmWeavingMachineSetup> GetMachineSetupInfo(vmCmnParameters cmnParam)
        //{
        //    GenericFactory_WeavingMachineSetup_GF = new vmPrdWeavingMachineSetup_GF();
        //    IEnumerable<vmWeavingMachineSetup> objWeavingMachineSetup = null;
        //    using (ERP_Entities _ctxCmn = new ERP_Entities())
        //    {
        //        try
        //        {
        //            var CmnItemMaster = _ctxCmn.CmnItemMasters.ToList();
        //            var PrdWeavingMachineSetup = _ctxCmn.PrdWeavingMachineSetups.ToList();

        //            objWeavingMachineSetup = (from master in PrdWeavingMachineSetup
        //                                      join color in CmnItemMaster on master.ItemID equals color.ItemID// into leftColorGroup
        //                                      // from lcg in leftColorGroup.DefaultIfEmpty()

        //                                      where (master.MachineSetupID == cmnParam.id && master.IsDeleted == false)
        //                                      select new vmWeavingMachineSetup
        //                                      {
        //                                          MachineSetupID = master.MachineSetupID,
        //                                          ItemID = master.ItemID,
        //                                          Selvedge = master.Selvedge,
        //                                          Brackrest = master.Brackrest,
        //                                          ShadeAngle = master.ShadeAngle,
        //                                          SFHight = master.SFHight,
        //                                          CompanyID = master.CompanyID,
        //                                          ArticleNo = color.ArticleNo
        //                                      }).ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //    }
        //    return objWeavingMachineSetup;
        //}

        public string SaveUpdateCodewiseMachineSetup(PrdWeavingMachineSetup CodewiseMachineSetupInfo, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdWeavingMachineSetup = new PrdWeavingMachineSetup_EF();
                long MainId = 0; string CustomNo = string.Empty, DefectNo = string.Empty;
                var CodeMachineSetup = new PrdWeavingMachineSetup();
                try
                {
                    if (CodewiseMachineSetupInfo.MachineSetupID > 0)
                    {
                        CodeMachineSetup = GenericFactory_EF_PrdWeavingMachineSetup.GetAll().Where(x => x.MachineSetupID == CodewiseMachineSetupInfo.MachineSetupID).FirstOrDefault();
                        CodeMachineSetup.ItemID = CodewiseMachineSetupInfo.ItemID;
                        CodeMachineSetup.Selvedge = CodewiseMachineSetupInfo.Selvedge;
                        CodeMachineSetup.SFHight = CodewiseMachineSetupInfo.SFHight;
                        CodeMachineSetup.ShadeAngle = CodewiseMachineSetupInfo.ShadeAngle;
                        CodeMachineSetup.Brackrest = CodewiseMachineSetupInfo.Brackrest;
                        
                        CodeMachineSetup.CompanyID = objcmnParam.loggedCompany;
                        CodeMachineSetup.UpdateBy = objcmnParam.loggeduser;
                        CodeMachineSetup.UpdateOn = DateTime.Now;
                        CodeMachineSetup.UpdatePc = HostService.GetIP();
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_EF_PrdWeavingMachineSetup.getMaxID("PrdWeavingMachineSetup"));
                        CodeMachineSetup = new PrdWeavingMachineSetup()
                        {
                            MachineSetupID = (int)MainId,
                            ItemID = CodewiseMachineSetupInfo.ItemID,
                            Selvedge = CodewiseMachineSetupInfo.Selvedge,
                            SFHight = CodewiseMachineSetupInfo.SFHight,
                            ShadeAngle = CodewiseMachineSetupInfo.ShadeAngle,
                            Brackrest = CodewiseMachineSetupInfo.Brackrest,
                            IsDeleted = false,

                            IsActive=true,
                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }

                    if (CodewiseMachineSetupInfo.MachineSetupID > 0)
                    {
                        GenericFactory_EF_PrdWeavingMachineSetup.Update(CodeMachineSetup);
                        GenericFactory_EF_PrdWeavingMachineSetup.Save();
                    }
                    else
                    {
                        GenericFactory_EF_PrdWeavingMachineSetup.Insert(CodeMachineSetup);
                        GenericFactory_EF_PrdWeavingMachineSetup.Save();
                        GenericFactory_EF_PrdWeavingMachineSetup.updateMaxID("PrdWeavingMachineSetup", Convert.ToInt64(MainId));
                    }
                    transaction.Complete();
                    result = CodeMachineSetup.Selvedge.ToString();
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdateWeavingMachineSetup(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdWeavingMachineSetup = new PrdWeavingMachineSetup_EF();
                var WeavingMachineSetup = new PrdWeavingMachineSetup();
                try
                {
                    WeavingMachineSetup = GenericFactory_EF_PrdWeavingMachineSetup.GetAll().Where(x => x.MachineSetupID == objcmnParam.id).FirstOrDefault();
                    WeavingMachineSetup.IsDeleted = true;
                    WeavingMachineSetup.CompanyID = objcmnParam.loggedCompany;
                    WeavingMachineSetup.DeleteBy = objcmnParam.loggeduser;
                    WeavingMachineSetup.DeleteOn = DateTime.Now;
                    WeavingMachineSetup.DeletePc = HostService.GetIP();

                    GenericFactory_EF_PrdWeavingMachineSetup.Update(WeavingMachineSetup);
                    GenericFactory_EF_PrdWeavingMachineSetup.Save();

                    transaction.Complete();
                    result = WeavingMachineSetup.Selvedge.ToString();
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
