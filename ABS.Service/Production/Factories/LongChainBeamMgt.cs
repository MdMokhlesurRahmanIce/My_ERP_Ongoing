using ABS.Data.BaseFactories;
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
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class LongChainBeamMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdLCBMRRMaster> GenericFactory_EF_PrdLCBMRRMaster = null;
        private iGenericFactory_EF<PrdLCBMRRDetail> GenericFactory_EF_PrdLCBMRRDetail = null;
        private iGenericFactory_EF<PrdLCBMRRMachineStopM> GenericFactory_EF_PrdLCBMRRMachineStopM = null;
        private iGenericFactory_EF<PrdLCBMRRMachineStop> GenericFactory_EF_PrdLCBMRRMachineStop = null;
        private iGenericFactory_EF<PrdLCBMRRBreakageM> GenericFactory_EF_PrdLCBMRRBreakageM = null;
        private iGenericFactory_EF<PrdLCBMRRBreakage> GenericFactory_EF_PrdLCBMRRBreakage = null;
        private iGenericFactory<vmPrdLCBMRRMasterDetail> GenericFactory_PrdLCBMRRMasterDetail_VM = null;

        public vmPrdLCBMRRMasterDetail GetSetInformation(vmCmnParameters objcmnParam)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            vmPrdLCBMRRMasterDetail objSetInformation = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("SetID", objcmnParam.id);
                ht.Add("LCBMRRID", objcmnParam.ItemType); //LCBMRRID is applicable when retrieve LCBmrrmaster data

                spQuery = "[Get_LCBMasterInfo]";
                objSetInformation = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetInformation;
        }

        public IEnumerable<vmPrdLCBMRRMasterDetail> GetLCBMRRMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            IEnumerable<vmPrdLCBMRRMasterDetail> LCBMaster = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
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

                    spQuery = "[Get_LCBMRRMaster]";
                    LCBMaster = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdLCBMRRMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count(); //LCBMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return LCBMaster;
        }

        public IEnumerable<vmPrdLCBMRRMasterDetail> GetLCBDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            IEnumerable<vmPrdLCBMRRMasterDetail> ListLCBDetail = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
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
                    ht.Add("LCBMRRID", objcmnParam.id);

                    spQuery = "[Get_LCBDetailByID]";
                    ListLCBDetail = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListLCBDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListLCBDetail;
        }

        public IEnumerable<vmPrdLCBMRRMasterDetail> GetStopDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            IEnumerable<vmPrdLCBMRRMasterDetail> ListStopDetail = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
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
                    ht.Add("LCBMRRID", objcmnParam.id);

                    spQuery = "[Get_LCBStopDetailByID]";
                    ListStopDetail = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListStopDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListStopDetail;
        }

        public IEnumerable<vmPrdLCBMRRMasterDetail> GetBreakageDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            IEnumerable<vmPrdLCBMRRMasterDetail> ListBreakageDetail = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
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
                    ht.Add("LCBMRRID", objcmnParam.id);

                    spQuery = "[Get_LCBBreakageDetailByID]";
                    ListBreakageDetail = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListBreakageDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListBreakageDetail;
        }

        public string SaveUpdateLCB(vmPrdLCBMRRMasterDetail itemMaster, List<vmPrdLCBMRRMasterDetail> MainDetail, List<vmPrdLCBMRRMasterDetail> MachinStopDetail, List<vmPrdLCBMRRMasterDetail> BreakageTypeDetail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty; long MainMasterId = 0, StopMasterId = 0, BreakMasterId = 0,
                MainDetailId = 0, MainFirstDigit = 0, MainOtherDigits = 0, StopDetailId = 0, StopFirstDigit = 0, StopOtherDigits = 0,
                BreakDetailId = 0, BreakFirstDigit = 0, BreakOtherDigits = 0;
                int SMainRowNum = 0, StopRowNum = 0, BreakageRowNum = 0, UMainRowNum = 0,
                    LastRowNum = 0, StopCount = 0, BreakCount = 0; string LCBMRRNo = "";
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_EF_PrdLCBMRRMaster = new PrdLCBMRRMaster_EF();
                GenericFactory_EF_PrdLCBMRRDetail = new PrdLCBMRRDetail_EF();
                GenericFactory_EF_PrdLCBMRRMachineStopM = new PrdLCBMRRMachineStopM_EF();
                GenericFactory_EF_PrdLCBMRRMachineStop = new PrdLCBMRRMachineStop_EF();
                GenericFactory_EF_PrdLCBMRRBreakageM = new PrdLCBMRRBreakageM_EF();
                GenericFactory_EF_PrdLCBMRRBreakage = new PrdLCBMRRBreakage_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdLCBMRRMaster();
                var DetailItemMain = new List<PrdLCBMRRDetail>();
                var MasterItemMachineStop = new List<PrdLCBMRRMachineStopM>();
                var DetailItemMachineStop = new List<PrdLCBMRRMachineStop>();
                var MasterItemBreakageType = new List<PrdLCBMRRBreakageM>();
                var DetailItemBreakageType = new List<PrdLCBMRRBreakage>();

                var UDetailItemMain = new List<PrdLCBMRRDetail>();
                var UMasterItemMachineStop = new List<PrdLCBMRRMachineStopM>();
                var UDetailItemMachineStop = new List<PrdLCBMRRMachineStop>();
                var UMasterItemBreakageType = new List<PrdLCBMRRBreakageM>();
                var UDetailItemBreakageType = new List<PrdLCBMRRBreakage>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmPrdLCBMRRMasterDetail item = new vmPrdLCBMRRMasterDetail();
                vmPrdLCBMRRMasterDetail itemStop = new vmPrdLCBMRRMasterDetail();
                vmPrdLCBMRRMasterDetail itemBreak = new vmPrdLCBMRRMasterDetail();
                //vmChemicalSetupMasterDetail items = new vmChemicalSetupMasterDetail();
                //***************************************End Create Model Instance to get Data***********************************


                SMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Save").Count());
                UMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").Count());
                StopRowNum = Convert.ToInt32(MachinStopDetail.Count());
                BreakageRowNum = Convert.ToInt32(BreakageTypeDetail.Count());




                //**************************************************Start Main Operation************************************************
                if (SMainRowNum > 0 || UMainRowNum > 0)
                {
                    try
                    {
                        if (itemMaster.LCBMRRID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MainMasterId = Convert.ToInt16(GenericFactory_EF_PrdLCBMRRMaster.getMaxID("PrdLCBMRRMaster"));
                            MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRDetail.getMaxID("PrdLCBMRRDetail"));
                            MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                            MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************
                            CustomNo = GenericFactory_EF_PrdLCBMRRMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                LCBMRRNo = MainMasterId.ToString();
                            }
                            else
                            {
                                LCBMRRNo = CustomNo;
                            }

                            MasterItem = new PrdLCBMRRMaster
                            {
                                LCBMRRID = MainMasterId,
                                Description = itemMaster.Description,
                                ItemID = (long)itemMaster.ItemID,
                                SetID = itemMaster.SetID,
                                LCBMRRDate = (DateTime)itemMaster.LCBMRRDate,
                                LCBMRRNo = LCBMRRNo,
                                PIID = itemMaster.PIID,
                                TransactionTypeID = objcmnParam.tTypeId,
                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false
                            };
                            for (int i = 0; i < SMainRowNum; i++)
                            {
                                item = MainDetail[i];
                                item.DDate = DateTime.Parse(item.DDateString);
                                if (StopRowNum > 0)
                                {
                                    StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                    if (StopCount > 0)
                                    {
                                        StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdLCBMRRMachineStopM.getMaxID("PrdLCBMRRMachineStopM"));
                                        StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRMachineStop.getMaxID("PrdLCBMRRMachineStop"));
                                        StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                        StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmPrdLCBMRRMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                        {
                                            var DetailStop = new PrdLCBMRRMachineStop
                                            {
                                                LCBMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                LCBMachineStopMasterID = StopMasterId,
                                                Description = S.Description,
                                                MachineID = item.MachineID,
                                                ShiftID = (int)S.ShiftID,
                                                StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                StopDate = (DateTime)item.DDate,
                                                StopID = (int)S.BWSID,
                                                StopInMin = S.StopInMin,
                                                IsNextDate = S.IsNextDate,

                                                CompanyID = objcmnParam.loggedCompany,
                                                CreateBy = objcmnParam.loggeduser,
                                                CreateOn = DateTime.Now,
                                                CreatePc = HostService.GetIP(),
                                                IsDeleted = false
                                            };
                                            DetailItemMachineStop.Add(DetailStop);
                                            StopOtherDigits++;

                                            if (StopCount == LastRowNum)
                                            {
                                                var MasterStop = new PrdLCBMRRMachineStopM
                                                {
                                                    LCBMachineStopMasterID = StopMasterId,
                                                    TotalStop = (int)item.TotalStop,
                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemMachineStop.Add(MasterStop);
                                                GenericFactory_EF_PrdLCBMRRMachineStopM.updateMaxID("PrdLCBMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                GenericFactory_EF_PrdLCBMRRMachineStop.updateMaxID("PrdLCBMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
                                            }
                                            LastRowNum = LastRowNum + 1;
                                        }
                                    }
                                }

                                if (BreakageRowNum > 0)
                                {
                                    BreakCount = Convert.ToInt32(BreakageTypeDetail.Where(b => b.SlNo == item.SlNo).Count());
                                    if (BreakCount > 0)
                                    {
                                        BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdLCBMRRBreakageM.getMaxID("PrdLCBMRRBreakageM"));
                                        BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRBreakage.getMaxID("PrdLCBMRRBreakage"));
                                        BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                        BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmPrdLCBMRRMasterDetail m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                        {
                                            var DetailBreak = new PrdLCBMRRBreakage
                                            {
                                                LCBBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                LCBBreakageMasterID = BreakMasterId,
                                                BreakageDate = (DateTime)item.DDate,
                                                NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                BreakageID = (int)m.BWSID,
                                                MachineID = item.MachineID,
                                                Description = m.Description,

                                                CompanyID = objcmnParam.loggedCompany,
                                                CreateBy = objcmnParam.loggeduser,
                                                CreateOn = DateTime.Now,
                                                CreatePc = HostService.GetIP(),
                                                IsDeleted = false
                                            };
                                            DetailItemBreakageType.Add(DetailBreak);
                                            BreakOtherDigits++;

                                            if (BreakCount == LastRowNum)
                                            {
                                                var MasterBreak = new PrdLCBMRRBreakageM
                                                {
                                                    LCBBreakageMasterID = BreakMasterId,
                                                    TotalBreakage = (int)item.TotalBreakage,
                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemBreakageType.Add(MasterBreak);
                                                GenericFactory_EF_PrdLCBMRRBreakageM.updateMaxID("PrdLCBMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                GenericFactory_EF_PrdLCBMRRBreakage.updateMaxID("PrdLCBMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                            }
                                            LastRowNum = LastRowNum + 1;
                                        }
                                    }
                                }

                                //string ds = item.BeginTime.ToString();

                                var Detailitem = new PrdLCBMRRDetail
                                {
                                    LCBMRRDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                    LCBMRRID = MainMasterId,
                                    SetLength = item.BeamLength,
                                    StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime),
                                    EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime),
                                    MachineSpeed = item.MachineSpeed,
                                    OperatorID = item.OperatorID,
                                    OutputUnitID = item.OutputUnitID,
                                    Remarks = item.Description,
                                    ShiftEngineerID = item.ShiftEngineerID,
                                    ShiftID = item.ShiftID,
                                    LCBBreakageMasterID = BreakMasterId,
                                    LCBMachineStopMasterID = StopMasterId,
                                    TotalBreakage = item.TotalBreakage,
                                    TotalStop = item.TotalStop,
                                    MachineID = item.MachineID,
                                    Date = (DateTime)item.DDate,

                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false
                                };
                                DetailItemMain.Add(Detailitem);
                                MainOtherDigits++;
                            }

                            //***************************************************End Save Operation************************************************
                        }
                        else
                        {
                            if (UMainRowNum > 0)
                            {
                                var MasterAll = GenericFactory_EF_PrdLCBMRRMaster.GetAll().Where(x => x.LCBMRRID == itemMaster.LCBMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var DetailAll = GenericFactory_EF_PrdLCBMRRDetail.GetAll().Where(x => x.LCBMRRID == itemMaster.LCBMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var MachineMasterAll = GenericFactory_EF_PrdLCBMRRMachineStopM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var MachineDetailAll = GenericFactory_EF_PrdLCBMRRMachineStop.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageMasterAll = GenericFactory_EF_PrdLCBMRRBreakageM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageDetailAll = GenericFactory_EF_PrdLCBMRRBreakage.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                //*************************************End Get Data From Related Table to Update*********************************                            

                                MasterItem = MasterAll.First(x => x.LCBMRRID == itemMaster.LCBMRRID);
                                MasterItem.Description = itemMaster.Description;
                                MasterItem.ItemID = (long)itemMaster.ItemID;
                                MasterItem.SetID = itemMaster.SetID;
                                MasterItem.LCBMRRDate = (DateTime)itemMaster.LCBMRRDate;
                                MasterItem.TransactionTypeID = objcmnParam.tTypeId;
                                MasterItem.CompanyID = objcmnParam.loggedCompany;
                                MasterItem.UpdateBy = objcmnParam.loggeduser;
                                MasterItem.UpdateOn = DateTime.Now;
                                MasterItem.UpdatePc = HostService.GetIP();

                                for (int i = 0; i < UMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").ToList()[i];
                                    item.DDate = DateTime.Parse(item.DDateString);
                                    if (StopRowNum > 0)
                                    {
                                        StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                        if (StopCount > 0)
                                        {
                                            LastRowNum = 1;
                                            foreach (vmPrdLCBMRRMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                foreach (PrdLCBMRRMachineStop st in MachineDetailAll.Where(x => x.LCBMachineStopID == S.LCBMachineStopID && (S.ModelState == "Update" || S.ModelState == "Delete")))
                                                {
                                                    if (S.ModelState == "Delete")
                                                    {
                                                        st.CompanyID = objcmnParam.loggedCompany;
                                                        st.DeleteBy = objcmnParam.loggeduser;
                                                        st.DeleteOn = DateTime.Now;
                                                        st.DeletePc = HostService.GetIP();
                                                        st.IsDeleted = true;
                                                    }
                                                    else
                                                    {
                                                        st.Description = S.Description;
                                                        st.MachineID = item.MachineID;
                                                        st.ShiftID = (int)S.ShiftID;
                                                        st.StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime);
                                                        st.StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime);
                                                        st.StopDate = (DateTime)item.DDate;
                                                        st.StopID = (int)S.BWSID;
                                                        st.StopInMin = S.StopInMin;
                                                        st.IsNextDate = S.IsNextDate;

                                                        st.CompanyID = objcmnParam.loggedCompany;
                                                        st.UpdateBy = objcmnParam.loggeduser;
                                                        st.UpdateOn = DateTime.Now;
                                                        st.UpdatePc = HostService.GetIP();
                                                    }

                                                    UDetailItemMachineStop.Add(st);
                                                };

                                                if (S.ModelState == "Save")
                                                {
                                                    StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRMachineStop.getMaxID("PrdLCBMRRMachineStop"));
                                                    StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                                    StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));

                                                    var SDetailStop = new PrdLCBMRRMachineStop
                                                    {
                                                        LCBMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                        LCBMachineStopMasterID = (long)item.LCBMachineStopMasterID,
                                                        Description = S.Description,
                                                        MachineID = item.MachineID,
                                                        ShiftID = (int)S.ShiftID,
                                                        StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                        StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                        StopDate = (DateTime)item.DDate,
                                                        StopID = (int)S.BWSID,
                                                        StopInMin = S.StopInMin,
                                                        IsNextDate = S.IsNextDate,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    DetailItemMachineStop.Add(SDetailStop);
                                                    GenericFactory_EF_PrdLCBMRRMachineStop.updateMaxID("PrdLCBMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits)));
                                                }


                                                if (StopCount == LastRowNum)
                                                {
                                                    foreach (PrdLCBMRRMachineStopM sm in MachineMasterAll.Where(x => x.LCBMachineStopMasterID == item.LCBMachineStopMasterID))
                                                    {
                                                        if (item.ModelState == "Delete")
                                                        {
                                                            sm.CompanyID = objcmnParam.loggedCompany;
                                                            sm.DeleteBy = objcmnParam.loggeduser;
                                                            sm.DeleteOn = DateTime.Now;
                                                            sm.DeletePc = HostService.GetIP();
                                                            sm.IsDeleted = true;
                                                        }
                                                        else
                                                        {
                                                            sm.TotalStop = (int)item.TotalStop;
                                                            sm.CompanyID = objcmnParam.loggedCompany;
                                                            sm.UpdateBy = objcmnParam.loggeduser;
                                                            sm.UpdateOn = DateTime.Now;
                                                            sm.UpdatePc = HostService.GetIP();
                                                        }
                                                        UMasterItemMachineStop.Add(sm);
                                                    };
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    if (BreakageRowNum > 0)
                                    {
                                        BreakCount = Convert.ToInt32(BreakageTypeDetail.Where(b => b.SlNo == item.SlNo).Count());
                                        if (BreakCount > 0)
                                        {
                                            LastRowNum = 1;
                                            foreach (vmPrdLCBMRRMasterDetail m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                            {
                                                foreach (PrdLCBMRRBreakage bd in BreakageDetailAll.Where(x => x.LCBBreackageID == m.LCBBreakageID && (m.ModelState == "Update" || m.ModelState == "Delete")))
                                                {
                                                    if (m.ModelState == "Delete")
                                                    {
                                                        bd.CompanyID = objcmnParam.loggedCompany;
                                                        bd.DeleteBy = objcmnParam.loggeduser;
                                                        bd.DeleteOn = DateTime.Now;
                                                        bd.DeletePc = HostService.GetIP();
                                                        bd.IsDeleted = true;
                                                    }
                                                    else
                                                    {
                                                        bd.BreakageDate = (DateTime)item.DDate;
                                                        bd.NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage;
                                                        bd.BreakageID = (int)m.BWSID;
                                                        bd.MachineID = item.MachineID;
                                                        bd.Description = m.Description;

                                                        bd.CompanyID = objcmnParam.loggedCompany;
                                                        bd.UpdateBy = objcmnParam.loggeduser;
                                                        bd.UpdateOn = DateTime.Now;
                                                        bd.UpdatePc = HostService.GetIP();
                                                    }
                                                    UDetailItemBreakageType.Add(bd);
                                                };

                                                if (m.ModelState == "Save")
                                                {
                                                    if (BreakOtherDigits == 0)
                                                    {
                                                        BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRBreakage.getMaxID("PrdLCBMRRBreakage"));
                                                        BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                                        BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                                    }

                                                    var SDetailBreak = new PrdLCBMRRBreakage
                                                    {
                                                        LCBBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                        LCBBreakageMasterID = (long)item.LCBBreakageMasterID,
                                                        BreakageDate = (DateTime)item.DDate,
                                                        NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                        BreakageID = (int)m.BWSID,
                                                        MachineID = item.MachineID,
                                                        Description = m.Description,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    DetailItemBreakageType.Add(SDetailBreak);
                                                    GenericFactory_EF_PrdLCBMRRBreakage.updateMaxID("PrdLCBMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits)));
                                                }

                                                if (BreakCount == LastRowNum)
                                                {
                                                    foreach (PrdLCBMRRBreakageM bm in BreakageMasterAll.Where(x => x.LCBBreakageMasterID == item.LCBBreakageMasterID))
                                                    {
                                                        if (item.ModelState == "Delete")
                                                        {
                                                            bm.CompanyID = objcmnParam.loggedCompany;
                                                            bm.DeleteBy = objcmnParam.loggeduser;
                                                            bm.DeleteOn = DateTime.Now;
                                                            bm.DeletePc = HostService.GetIP();
                                                            bm.IsDeleted = true;
                                                        }
                                                        else
                                                        {
                                                            bm.TotalBreakage = (int)item.TotalBreakage;

                                                            bm.CompanyID = objcmnParam.loggedCompany;
                                                            bm.UpdateBy = objcmnParam.loggeduser;
                                                            bm.UpdateOn = DateTime.Now;
                                                            bm.UpdatePc = HostService.GetIP();
                                                        }
                                                        UMasterItemBreakageType.Add(bm);
                                                    };
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    foreach (PrdLCBMRRDetail d in DetailAll.Where(d => d.LCBMRRID == item.LCBMRRID && d.LCBMRRDetailID == item.LCBMRRDetailID))
                                    {
                                        if (item.ModelState == "Delete")
                                        {
                                            d.CompanyID = objcmnParam.loggedCompany;
                                            d.DeleteBy = objcmnParam.loggeduser;
                                            d.DeleteOn = DateTime.Now;
                                            d.DeletePc = HostService.GetIP();
                                            d.IsDeleted = true;
                                        }
                                        else
                                        {
                                            d.SetLength = item.BeamLength;
                                            d.StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime);
                                            d.EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime);
                                            d.MachineSpeed = item.MachineSpeed;
                                            d.OperatorID = item.OperatorID;
                                            d.OutputUnitID = item.OutputUnitID;

                                            d.Remarks = item.Description;
                                            d.ShiftEngineerID = item.ShiftEngineerID;
                                            d.ShiftID = item.ShiftID;
                                            d.TotalBreakage = item.TotalBreakage;
                                            d.TotalStop = item.TotalStop;
                                            d.MachineID = item.MachineID;
                                            d.Date = (DateTime)item.DDate;

                                            d.CompanyID = objcmnParam.loggedCompany;
                                            d.UpdateBy = objcmnParam.loggeduser;
                                            d.UpdateOn = DateTime.Now;
                                            d.UpdatePc = HostService.GetIP();
                                        }
                                        UDetailItemMain.Add(d);
                                    }
                                }
                            }
                            if (SMainRowNum > 0)
                            {
                                MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRDetail.getMaxID("PrdLCBMRRDetail"));
                                MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                                MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                                StopFirstDigit = 0; StopOtherDigits = 0; BreakFirstDigit = 0; BreakOtherDigits = 0;
                                //***********************************************End Generate Master & Detail ID*****************************************                                
                                for (int i = 0; i < SMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Save").ToList()[i];
                                    item.DDate = DateTime.Parse(item.DDateString);
                                    if (StopRowNum > 0)
                                    {
                                        StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                        if (StopCount > 0)
                                        {
                                            StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdLCBMRRMachineStopM.getMaxID("PrdLCBMRRMachineStopM"));
                                            StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRMachineStop.getMaxID("PrdLCBMRRMachineStop"));
                                            StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                            StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                            LastRowNum = 1;
                                            foreach (vmPrdLCBMRRMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                var DetailStop = new PrdLCBMRRMachineStop
                                                {
                                                    LCBMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                    LCBMachineStopMasterID = StopMasterId,
                                                    Description = S.Description,
                                                    MachineID = item.MachineID,
                                                    ShiftID = (int)S.ShiftID,
                                                    StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                    StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                    StopDate = (DateTime)item.DDate,
                                                    StopID = (int)S.BWSID,
                                                    StopInMin = S.StopInMin,
                                                    IsNextDate = S.IsNextDate,

                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                DetailItemMachineStop.Add(DetailStop);
                                                StopOtherDigits++;

                                                if (StopCount == LastRowNum)
                                                {
                                                    var MasterStop = new PrdLCBMRRMachineStopM
                                                    {
                                                        LCBMachineStopMasterID = StopMasterId,
                                                        TotalStop = (int)item.TotalStop,
                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    MasterItemMachineStop.Add(MasterStop);
                                                    GenericFactory_EF_PrdLCBMRRMachineStopM.updateMaxID("PrdLCBMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                    GenericFactory_EF_PrdLCBMRRMachineStop.updateMaxID("PrdLCBMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    if (BreakageRowNum > 0)
                                    {
                                        BreakCount = Convert.ToInt32(BreakageTypeDetail.Where(b => b.SlNo == item.SlNo).Count());
                                        if (BreakCount > 0)
                                        {
                                            BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdLCBMRRBreakageM.getMaxID("PrdLCBMRRBreakageM"));
                                            BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdLCBMRRBreakage.getMaxID("PrdLCBMRRBreakage"));
                                            BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                            BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                            LastRowNum = 1;
                                            foreach (vmPrdLCBMRRMasterDetail m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                            {
                                                var DetailBreak = new PrdLCBMRRBreakage
                                                {
                                                    LCBBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                    LCBBreakageMasterID = BreakMasterId,
                                                    BreakageDate = (DateTime)item.DDate,
                                                    NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                    BreakageID = (int)m.BWSID,
                                                    MachineID = item.MachineID,
                                                    Description = m.Description,


                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                DetailItemBreakageType.Add(DetailBreak);
                                                BreakOtherDigits++;

                                                if (BreakCount == LastRowNum)
                                                {
                                                    var MasterBreak = new PrdLCBMRRBreakageM
                                                    {
                                                        LCBBreakageMasterID = BreakMasterId,
                                                        TotalBreakage = (int)item.TotalBreakage,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    MasterItemBreakageType.Add(MasterBreak);
                                                    GenericFactory_EF_PrdLCBMRRBreakageM.updateMaxID("PrdLCBMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                    GenericFactory_EF_PrdLCBMRRBreakage.updateMaxID("PrdLCBMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    //string ds = item.BeginTime.ToString();

                                    var Detailitem = new PrdLCBMRRDetail
                                    {
                                        LCBMRRDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                        LCBMRRID = itemMaster.LCBMRRID,
                                        SetLength = item.BeamLength,
                                        StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime),
                                        EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime),
                                        MachineSpeed = item.MachineSpeed,
                                        OperatorID = item.OperatorID,
                                        OutputUnitID = item.OutputUnitID,
                                        Remarks = item.Description,
                                        ShiftEngineerID = item.ShiftEngineerID,
                                        ShiftID = item.ShiftID,
                                        LCBBreakageMasterID = BreakMasterId,
                                        LCBMachineStopMasterID = StopMasterId,
                                        TotalBreakage = item.TotalBreakage,
                                        TotalStop = item.TotalStop,
                                        MachineID = item.MachineID,
                                        Date = (DateTime)item.DDate,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItemMain.Add(Detailitem);
                                    MainOtherDigits++;
                                }
                            }
                            //***********************************Start Get Data From Related Table to Update*********************************
                        }

                        if (itemMaster.LCBMRRID > 0)
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdLCBMRRMaster.Update(MasterItem);
                                GenericFactory_EF_PrdLCBMRRMaster.Save();
                            }
                            if (UMasterItemMachineStop != null && UMasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStopM.UpdateList(UMasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStopM.Save();
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStopM.Save();
                            }
                            if (UDetailItemMachineStop != null && UDetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStop.UpdateList(UDetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStop.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStop.Save();
                            }
                            if (UMasterItemBreakageType != null && UMasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakageM.UpdateList(UMasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakageM.Save();
                            }
                            if (MasterItemBreakageType != null && MasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakageM.Save();
                            }
                            if (UDetailItemBreakageType != null && UDetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakage.UpdateList(UDetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakage.Save();
                            }
                            if (DetailItemBreakageType != null && DetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakage.Save();
                            }
                            if (UDetailItemMain != null && UDetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRDetail.UpdateList(UDetailItemMain.ToList());
                                GenericFactory_EF_PrdLCBMRRDetail.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdLCBMRRDetail.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdLCBMRRDetail.Save();
                                GenericFactory_EF_PrdLCBMRRDetail.updateMaxID("PrdLCBMRRDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }
                        }
                        else
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdLCBMRRMaster.Insert(MasterItem);
                                GenericFactory_EF_PrdLCBMRRMaster.Save();
                                GenericFactory_EF_PrdLCBMRRMaster.updateMaxID("PrdLCBMRRMaster", Convert.ToInt64(MainMasterId));
                                GenericFactory_EF_PrdLCBMRRMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (MasterItemMachineStop != null)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStopM.Save();
                            }
                            if (DetailItemMachineStop != null)
                            {
                                GenericFactory_EF_PrdLCBMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdLCBMRRMachineStop.Save();
                            }
                            if (MasterItemBreakageType != null)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakageM.Save();
                            }
                            if (DetailItemBreakageType != null)
                            {
                                GenericFactory_EF_PrdLCBMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdLCBMRRBreakage.Save();
                            }
                            if (DetailItemMain != null)
                            {
                                GenericFactory_EF_PrdLCBMRRDetail.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdLCBMRRDetail.Save();
                                GenericFactory_EF_PrdLCBMRRDetail.updateMaxID("PrdLCBMRRDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }
                        }

                        transaction.Complete();
                        result = "1";
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        if (result == "")
                        {
                            result = "";
                        }
                    }
                    #region To check Validation Error
                    //catch (DbEntityValidationException e)
                    //{
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //        foreach (var ve in eve.ValidationErrors)
                    //        {
                    //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //                ve.PropertyName, ve.ErrorMessage);
                    //        }
                    //    }
                    //    throw;
                    //}
                    #endregion
                }
                else
                {
                    result = "";
                }
            }
            return result;
            //**************************************************End Main Operation************************************************            
        }

        public string DeletePrdLCBMasterDetail(vmCmnParameters objcmnParam)
        {
            GenericFactory_PrdLCBMRRMasterDetail_VM = new PrdLCBMRRMasterDetail_VM();
            string result = string.Empty;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("LCBMRRID", objcmnParam.id);

                spQuery = "[Delete_PrdLCBMRRMasterDetailByID]";
                result = GenericFactory_PrdLCBMRRMasterDetail_VM.ExecuteCommandString(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }
    }
}