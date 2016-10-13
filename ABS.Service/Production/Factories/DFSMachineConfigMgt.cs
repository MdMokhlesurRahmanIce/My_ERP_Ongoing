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
    public class DFSMachineConfigMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmPrdWeavingMachineConfigMasterDetail> GenericFactory_vmPrdWeavingMachineConfigMasterDetail_GF = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_PrdWeavingMachinConfig_EF = null;
        private iGenericFactory_EF<PrdWeavingMachineConfigDetail> GenericFactory_PrdWeavingMachineConfigDetail_EF = null;

        public IEnumerable<vmPrdWeavingMachineConfigMasterDetail> GetWeavingMachineConfigMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdWeavingMachineConfigMasterDetail_GF = new vmPrdWeavingMachineConfigMasterDetail_GF();
            IEnumerable<vmPrdWeavingMachineConfigMasterDetail> WMCMaster = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_WeavingMachinConfigMaster]";
                WMCMaster = GenericFactory_vmPrdWeavingMachineConfigMasterDetail_GF.ExecuteQuery(spQuery, ht);

                recordsTotal = (int)WMCMaster.FirstOrDefault().recordsTotal;
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return WMCMaster;
        }

        public IEnumerable<vmPrdWeavingMachineConfigMasterDetail> GetWeavingMachineConfigById(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmPrdWeavingMachineConfigMasterDetail_GF = new vmPrdWeavingMachineConfigMasterDetail_GF();
            IEnumerable<vmPrdWeavingMachineConfigMasterDetail> WMCDetail = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("MachineConfigID", objcmnParam.id);

                spQuery = "[Get_WeavingMachinConfigDetailByID]";
                WMCDetail = GenericFactory_vmPrdWeavingMachineConfigMasterDetail_GF.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return WMCDetail;
        }

        public string SaveUpdateWeavingMasterDetail(vmPrdWeavingMachineConfigMasterDetail Master, List<vmPrdWeavingMachineConfigMasterDetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdWeavingMachinConfig_EF = new PrdWeavingMachinConfig_EF();
                GenericFactory_PrdWeavingMachineConfigDetail_EF = new PrdWeavingMachineConfigDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdWeavingMachinConfig();
                var DetailItem = new List<PrdWeavingMachineConfigDetail>();
                var DetailItems = new List<PrdWeavingMachineConfigDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmPrdWeavingMachineConfigMasterDetail item = new vmPrdWeavingMachineConfigMasterDetail();
                vmPrdWeavingMachineConfigMasterDetail items = new vmPrdWeavingMachineConfigMasterDetail();
                PrdWeavingMachineConfigDetail itemdel = new PrdWeavingMachineConfigDetail();
                //***************************************End Create Model Instance to get Data***********************************

                var SDetail = Detail.Where(x => x.MachineConfigDetailID == 0).ToList();
                var UDetail = Detail.Where(x => x.MachineConfigDetailID != 0).ToList();
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.MachineConfigID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_PrdWeavingMachinConfig_EF.getMaxID("PrdWeavingMachinConfig"));
                            DetailId = Convert.ToInt64(GenericFactory_PrdWeavingMachineConfigDetail_EF.getMaxID("PrdWeavingMachineConfigDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            MasterItem = new PrdWeavingMachinConfig
                            {
                                MachineConfigID = (int)MasterId,
                                MachineConfigNo = Master.MachineConfigNo,
                                Remarks = Master.Remarks,
                                DepartmentID = Master.DepartmentID,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false,
                                IsBook = false,
                                IsCorrupted = false,
                                IsMaintenance = false
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdWeavingMachineConfigDetail
                                {
                                    MachineConfigDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                    MachineConfigID = (int)MasterId,
                                    MachineID = (long)item.MachineID,
                                    Description = item.Description,

                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false
                                };
                                DetailItem.Add(Detailitem);
                                OtherDigits++;
                            }
                            //***************************************************End Save Operation************************************************
                        }
                        else
                        {
                            //***********************************Start Get Data From Related Table to Update*********************************
                            var MasterAll = GenericFactory_PrdWeavingMachinConfig_EF.GetAll().Where(x => x.MachineConfigID == Master.MachineConfigID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GenericFactory_PrdWeavingMachineConfigDetail_EF.GetAll().Where(x => x.MachineConfigID == Master.MachineConfigID && x.CompanyID == objcmnParam.loggedCompany).ToArray();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.MachineConfigID == Master.MachineConfigID);
                            MasterItem.MachineConfigNo = Master.MachineConfigNo;
                            MasterItem.DepartmentID = Master.DepartmentID;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < UDetail.Count; i++)
                            {
                                item = UDetail[i];
                                foreach (PrdWeavingMachineConfigDetail d in DetailAll.Where(d => d.MachineConfigID == Master.MachineConfigID && d.MachineConfigDetailID == item.MachineConfigDetailID))
                                {
                                    d.MachineID = (long)item.MachineID;
                                    d.Description = item.Description;

                                    d.CompanyID = objcmnParam.loggedCompany;
                                    d.UpdateBy = objcmnParam.loggeduser;
                                    d.UpdateOn = DateTime.Now;
                                    d.UpdatePc = HostService.GetIP();
                                    d.IsDeleted = false;

                                    DetailItem.Add(d);
                                    break;
                                }
                            }
                            if (SDetail != null && SDetail.Count != 0)
                            {
                                for (int i = 0; i < SDetail.Count; i++)
                                {
                                    item = SDetail[i];
                                    DetailId = Convert.ToInt64(GenericFactory_PrdWeavingMachineConfigDetail_EF.getMaxID("PrdWeavingMachineConfigDetail"));
                                    FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                                    OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                                    var Detailitems = new PrdWeavingMachineConfigDetail
                                    {
                                        MachineConfigDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                        MachineConfigID = (long)Master.MachineConfigID,
                                        MachineID = (long)item.MachineID,
                                        Description = item.Description,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItems.Add(Detailitems);
                                    GenericFactory_PrdWeavingMachineConfigDetail_EF.updateMaxID("PrdWeavingMachineConfigDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits)));
                                }
                            }

                            if (UDetail.Count < DetailAll.Count())
                            {
                                for (int i = 0; i < DetailAll.Count(); i++)
                                {
                                    itemdel = DetailAll[i];

                                    var delDetail = (from del in DetailItem.Where(x => x.MachineConfigDetailID == itemdel.MachineConfigDetailID) select del.MachineConfigDetailID).FirstOrDefault();
                                    if (delDetail != itemdel.MachineConfigDetailID)
                                    {
                                        var tem = DetailAll.FirstOrDefault(d => d.MachineConfigID == Master.MachineConfigID && d.MachineConfigDetailID == itemdel.MachineConfigDetailID);
                                        tem.CompanyID = objcmnParam.loggedCompany;
                                        tem.DeleteBy = objcmnParam.loggeduser;
                                        tem.DeleteOn = DateTime.Now;
                                        tem.DeletePc = HostService.GetIP();
                                        tem.IsDeleted = true;
                                        DetailItem.Add(tem);
                                    }
                                }
                            }
                            //***************************************************End Update Operation********************************************
                        }

                        if (Master.MachineConfigID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdWeavingMachinConfig_EF.Update(MasterItem);
                                GenericFactory_PrdWeavingMachinConfig_EF.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.UpdateList(DetailItem.ToList());
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.Save();
                            }
                            if (DetailItems != null && DetailItems.Count != 0)
                            {
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.InsertList(DetailItems.ToList());
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdWeavingMachinConfig_EF.Insert(MasterItem);
                                GenericFactory_PrdWeavingMachinConfig_EF.Save();
                                GenericFactory_PrdWeavingMachinConfig_EF.updateMaxID("PrdWeavingMachinConfig", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.InsertList(DetailItem.ToList());
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.Save();
                                GenericFactory_PrdWeavingMachineConfigDetail_EF.updateMaxID("PrdWeavingMachineConfigDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
                        }

                        transaction.Complete();
                        result = MasterItem.MachineConfigNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                        e.ToString();
                    }
                }
                else
                {
                    result = "";
                }
            }
            return result;
            //**************************************************End Main Operation************************************************
        }

        public string DeleteUpdateWeavingMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                var MasterItem = new PrdWeavingMachinConfig();
                var DetailItem = new List<PrdWeavingMachineConfigDetail>();

                //For Update Master Detail
                var MasterAll = GenericFactory_PrdWeavingMachinConfig_EF.GetAll().Where(x => x.MachineConfigID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_PrdWeavingMachineConfigDetail_EF.GetAll().Where(x => x.MachineConfigID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.MachineConfigID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdWeavingMachineConfigDetail d in DetailAll.Where(d => d.MachineConfigID == objcmnParam.id))
                    {
                        d.CompanyID = objcmnParam.loggedCompany;
                        d.DeleteBy = objcmnParam.loggeduser;
                        d.DeleteOn = DateTime.Now;
                        d.DeletePc = HostService.GetIP();
                        d.IsDeleted = true;

                        DetailItem.Add(d);
                    }

                    if (MasterItem != null)
                    {
                        GenericFactory_PrdWeavingMachinConfig_EF.Update(MasterItem);
                        GenericFactory_PrdWeavingMachinConfig_EF.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_PrdWeavingMachineConfigDetail_EF.UpdateList(DetailItem.ToList());
                        GenericFactory_PrdWeavingMachineConfigDetail_EF.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.MachineConfigNo;
                }
                catch (Exception e)
                {
                    result = "";
                    e.ToString();
                }
            }
            return result;
        }
    }
}
