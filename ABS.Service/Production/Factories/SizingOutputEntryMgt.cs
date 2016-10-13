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
    public class SizingOutputEntryMgt //: iBallWarpingMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdSizingMRRMaster> GenericFactory_EF_PrdSizingMRRMaster = null;
        private iGenericFactory_EF<PrdSizingMRRDetail> GenericFactory_EF_PrdSizingMRRDetail = null;
        private iGenericFactory_EF<PrdSizingMRRMachineStopM> GenericFactory_EF_PrdSizingMRRMachineStopM = null;
        private iGenericFactory_EF<PrdSizingMRRMachineStop> GenericFactory_EF_PrdSizingMRRMachineStop = null;
        private iGenericFactory_EF<PrdSizingMRRBreakageM> GenericFactory_EF_PrdSizingMRRBreakageM = null;
        private iGenericFactory_EF<PrdSizingMRRBreakage> GenericFactory_EF_PrdSizingMRRBreakage = null;
        private iGenericFactory<vmBallWarpingInformation> GenericFactory_BallWarpingInformation_GF = null;
        private iGenericFactory<vmPrdSizingMRRMaster> GenericFactory_vmPrdSizingMRRMaster_VM = null;

        public vmBallWarpingInformation GetSetInformation(vmCmnParameters objcmnParam)
        {
            GenericFactory_BallWarpingInformation_GF = new vmBallWarpingInformation_GF();
            vmBallWarpingInformation objSetInformation = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("SetID", objcmnParam.id);
                ht.Add("MRRID", objcmnParam.ItemType); //SizeMRRID is applicable when retrieve sizemrrmaster data

                spQuery = "[Get_SetInformation]";
                objSetInformation = GenericFactory_BallWarpingInformation_GF.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetInformation;
        }

        public IEnumerable<vmPrdSizingMRRMaster> GetSizingMRRMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdSizingMRRMaster_VM = new PrdSizingMRRMaster_VM();
            IEnumerable<vmPrdSizingMRRMaster> SizingMaster = null;
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

                    spQuery = "[Get_SizingMRRMaster]";
                    SizingMaster = GenericFactory_vmPrdSizingMRRMaster_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdSizingMRRMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//SizingMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return SizingMaster;
        }

        public IEnumerable<vmPrdSizingMRRMaster> GetSizingDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdSizingMRRMaster_VM = new PrdSizingMRRMaster_VM();
            IEnumerable<vmPrdSizingMRRMaster> ListSizingDetail = null;
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
                    ht.Add("SizeMRRID", objcmnParam.id);

                    spQuery = "[Get_SizingDetailByID]";
                    ListSizingDetail = GenericFactory_vmPrdSizingMRRMaster_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListSizingDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListSizingDetail;
        }

        public IEnumerable<vmPrdSizingMRRMaster> GetStopDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdSizingMRRMaster_VM = new PrdSizingMRRMaster_VM();
            IEnumerable<vmPrdSizingMRRMaster> ListStopDetail = null;
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
                    ht.Add("SizeMRRID", objcmnParam.id);

                    spQuery = "[Get_StopDetailByID]";
                    ListStopDetail = GenericFactory_vmPrdSizingMRRMaster_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListStopDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListStopDetail;
        }

        public IEnumerable<vmPrdSizingMRRMaster> GetBreakageDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdSizingMRRMaster_VM = new PrdSizingMRRMaster_VM();
            IEnumerable<vmPrdSizingMRRMaster> ListBreakageDetail = null;
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
                    ht.Add("SizeMRRID", objcmnParam.id);

                    spQuery = "[Get_BreakageDetailByID]";
                    ListBreakageDetail = GenericFactory_vmPrdSizingMRRMaster_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListBreakageDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListBreakageDetail;
        }

        public string SaveUpdateSizing(vmPrdSizingMRRMaster itemMaster, List<vmPrdSizingMRRMaster> MainDetail, List<vmPrdSizingMRRMaster> MachinStopDetail, List<vmPrdSizingMRRMaster> BreakageTypeDetail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty; long MainMasterId = 0, StopMasterId = 0, BreakMasterId = 0,
                MainDetailId = 0, MainFirstDigit = 0, MainOtherDigits = 0, StopDetailId = 0, StopFirstDigit = 0, StopOtherDigits = 0,
                BreakDetailId = 0, BreakFirstDigit = 0, BreakOtherDigits = 0, SetCustomCodeID = 0;
                int SMainRowNum = 0, StopRowNum = 0, BreakageRowNum = 0, UMainRowNum = 0,
                    LastRowNum = 0, StopCount = 0, BreakCount = 0; string SizeMRRNo = "";
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_EF_PrdSizingMRRMaster = new PrdSizingMRRMaster_EF();
                GenericFactory_EF_PrdSizingMRRDetail = new PrdSizingMRRDetail_EF();
                GenericFactory_EF_PrdSizingMRRMachineStopM = new PrdSizingMRRMachineStopM_EF();
                GenericFactory_EF_PrdSizingMRRMachineStop = new PrdSizingMRRMachineStop_EF();
                GenericFactory_EF_PrdSizingMRRBreakageM = new PrdSizingMRRBreakageM_EF();
                GenericFactory_EF_PrdSizingMRRBreakage = new PrdSizingMRRBreakage_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdSizingMRRMaster();
                var DetailItemMain = new List<PrdSizingMRRDetail>();
                var MasterItemMachineStop = new List<PrdSizingMRRMachineStopM>();
                var DetailItemMachineStop = new List<PrdSizingMRRMachineStop>();
                var MasterItemBreakageType = new List<PrdSizingMRRBreakageM>();
                var DetailItemBreakageType = new List<PrdSizingMRRBreakage>();

                var UDetailItemMain = new List<PrdSizingMRRDetail>();
                var UMasterItemMachineStop = new List<PrdSizingMRRMachineStopM>();
                var UDetailItemMachineStop = new List<PrdSizingMRRMachineStop>();
                var UMasterItemBreakageType = new List<PrdSizingMRRBreakageM>();
                var UDetailItemBreakageType = new List<PrdSizingMRRBreakage>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmPrdSizingMRRMaster item = new vmPrdSizingMRRMaster();
                vmPrdSizingMRRMaster itemStop = new vmPrdSizingMRRMaster();
                vmPrdSizingMRRMaster itemBreak = new vmPrdSizingMRRMaster();
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
                        if (itemMaster.SizeMRRID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MainMasterId = Convert.ToInt16(GenericFactory_EF_PrdSizingMRRMaster.getMaxID("PrdSizingMRRMaster"));
                            MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRDetail.getMaxID("PrdSizingMRRDetail"));
                            MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                            MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************
                            CustomNo = GenericFactory_EF_PrdSizingMRRMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                SizeMRRNo = MainMasterId.ToString();
                            }
                            else
                            {
                                SizeMRRNo = CustomNo;
                            }

                            MasterItem = new PrdSizingMRRMaster
                            {
                                SIzeMRRID = MainMasterId,
                                Description = itemMaster.Description,
                                ItemID = (long)itemMaster.ItemID,
                                MachineID = itemMaster.MachineID,
                                SetID = itemMaster.SetID,
                                SizeMRRDate = (DateTime)itemMaster.SizeMRRDate,
                                SizeMRRNo = SizeMRRNo,
                                IsIssued=false,

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

                                if (StopRowNum > 0)
                                {
                                    StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                    if (StopCount > 0)
                                    {
                                        StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdSizingMRRMachineStopM.getMaxID("PrdSizingMRRMachineStopM"));
                                        StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRMachineStop.getMaxID("PrdSizingMRRMachineStop"));
                                        StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                        StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmPrdSizingMRRMaster S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                        {
                                            var DetailStop = new PrdSizingMRRMachineStop
                                            {
                                                SizeMachineStopID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                SizeMachineStopMasterID = StopMasterId,
                                                Description = S.Description,
                                                MachineID = S.MachineID,
                                                ShiftID = (int)S.ShiftID,
                                                StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                StopDate = (DateTime)itemMaster.SizeMRRDate,
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
                                                var MasterStop = new PrdSizingMRRMachineStopM
                                                {
                                                    SizeMachineStopMasterID = StopMasterId,
                                                    TotalStop = (int)item.TotalStop,
                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemMachineStop.Add(MasterStop);
                                                GenericFactory_EF_PrdSizingMRRMachineStopM.updateMaxID("PrdSizingMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                GenericFactory_EF_PrdSizingMRRMachineStop.updateMaxID("PrdSizingMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
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
                                        BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdSizingMRRBreakageM.getMaxID("PrdSizingMRRBreakageM"));
                                        BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRBreakage.getMaxID("PrdSizingMRRBreakage"));
                                        BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                        BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmPrdSizingMRRMaster m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                        {
                                            var DetailBreak = new PrdSizingMRRBreakage
                                            {
                                                SizeBreakageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                SizeBreakageMasterID = BreakMasterId,
                                                BreakageDate = (DateTime)itemMaster.SizeMRRDate,
                                                BreakageType = m.BreakageType,
                                                NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                BreakageID = (int)m.BWSID,
                                                MachineID = m.MachineID,
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
                                                var MasterBreak = new PrdSizingMRRBreakageM
                                                {
                                                    SizeBreakageMasterID = BreakMasterId,
                                                    TotalBCBreakage = (int)item.TotalBCBreakage,
                                                    TotalHSBreakage = (int)item.TotalHSBreakage,

                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemBreakageType.Add(MasterBreak);
                                                GenericFactory_EF_PrdSizingMRRBreakageM.updateMaxID("PrdSizingMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                GenericFactory_EF_PrdSizingMRRBreakage.updateMaxID("PrdSizingMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                            }
                                            LastRowNum = LastRowNum + 1;
                                        }
                                    }
                                }

                                //string ds = item.BeginTime.ToString();

                                var Detailitem = new PrdSizingMRRDetail
                                {
                                    SizeMRRDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                    SizeMRRID = MainMasterId,
                                    BeamLength = item.BeamLength,
                                    BeamQualityID = (int)item.BeamQualityID,
                                    StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime),
                                    EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime),
                                    MachineSpeed = item.MachineSpeed,
                                    OperatorID = item.OperatorID,
                                    OutputUnitID = item.OutputUnitID,
                                    OverallStretch = item.OverallStretch,
                                    Remarks = item.Description,
                                    ShiftEngineerID = item.ShiftEngineerID,
                                    ShiftID = item.ShiftID,
                                    SizeBreakageMasterID = BreakMasterId,
                                    SizeMachineStopMasterID = StopMasterId,
                                    SqueezingActual = item.SqueezingActual,
                                    SqueezingSTD = item.SqueezingSTD,
                                    TotalBCBreakage = item.TotalBCBreakage,
                                    TotalHSBreakage = item.TotalHSBreakage,
                                    TotalStop = item.TotalStop,

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
                                var MasterAll = GenericFactory_EF_PrdSizingMRRMaster.GetAll().Where(x => x.SIzeMRRID == itemMaster.SizeMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var DetailAll = GenericFactory_EF_PrdSizingMRRDetail.GetAll().Where(x => x.SizeMRRID == itemMaster.SizeMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var MachineMasterAll = GenericFactory_EF_PrdSizingMRRMachineStopM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var MachineDetailAll = GenericFactory_EF_PrdSizingMRRMachineStop.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageMasterAll = GenericFactory_EF_PrdSizingMRRBreakageM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageDetailAll = GenericFactory_EF_PrdSizingMRRBreakage.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                //*************************************End Get Data From Related Table to Update*********************************                            

                                MasterItem = MasterAll.First(x => x.SIzeMRRID == itemMaster.SizeMRRID);
                                MasterItem.Description = itemMaster.Description;
                                MasterItem.ItemID = (long)itemMaster.ItemID;
                                MasterItem.MachineID = itemMaster.MachineID;
                                MasterItem.SetID = itemMaster.SetID;
                                MasterItem.SizeMRRDate = (DateTime)itemMaster.SizeMRRDate;
                                MasterItem.TransactionTypeID = objcmnParam.tTypeId;
                                MasterItem.CompanyID = objcmnParam.loggedCompany;
                                MasterItem.UpdateBy = objcmnParam.loggeduser;
                                MasterItem.UpdateOn = DateTime.Now;
                                MasterItem.UpdatePc = HostService.GetIP();

                                for (int i = 0; i < UMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").ToList()[i];

                                    if (StopRowNum > 0)
                                    {
                                        StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                        if (StopCount > 0)
                                        {
                                            LastRowNum = 1;
                                            foreach (vmPrdSizingMRRMaster S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                foreach (PrdSizingMRRMachineStop st in MachineDetailAll.Where(x => x.SizeMachineStopID == S.SizeMachineStopID && (S.ModelState == "Update" || S.ModelState == "Delete")))
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
                                                        st.MachineID = S.MachineID;
                                                        st.ShiftID = (int)S.ShiftID;
                                                        st.StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime);
                                                        st.StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime);
                                                        st.StopDate = (DateTime)itemMaster.SizeMRRDate;
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
                                                    StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRMachineStop.getMaxID("PrdSizingMRRMachineStop"));
                                                    StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                                    StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));

                                                    var SDetailStop = new PrdSizingMRRMachineStop
                                                    {
                                                        SizeMachineStopID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                        SizeMachineStopMasterID = (long)item.SizeMachineStopMasterID,
                                                        Description = S.Description,
                                                        MachineID = S.MachineID,
                                                        ShiftID = (int)S.ShiftID,
                                                        StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                        StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                        StopDate = (DateTime)itemMaster.SizeMRRDate,
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
                                                    GenericFactory_EF_PrdSizingMRRMachineStop.updateMaxID("PrdSizingMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits)));
                                                }


                                                if (StopCount == LastRowNum)
                                                {
                                                    foreach (PrdSizingMRRMachineStopM sm in MachineMasterAll.Where(x => x.SizeMachineStopMasterID == item.SizeMachineStopMasterID))
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
                                            foreach (vmPrdSizingMRRMaster m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                            {
                                                foreach (PrdSizingMRRBreakage bd in BreakageDetailAll.Where(x => x.SizeBreakageID == m.SizeBreakageID && (m.ModelState == "Update" || m.ModelState == "Delete")))
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
                                                        bd.BreakageDate = (DateTime)itemMaster.SizeMRRDate;
                                                        bd.BreakageType = m.BreakageType;
                                                        bd.NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage;
                                                        bd.BreakageID = (int)m.BWSID;
                                                        bd.MachineID = m.MachineID;
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
                                                        BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRBreakage.getMaxID("PrdSizingMRRBreakage"));
                                                        BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                                        BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                                    }

                                                    var SDetailBreak = new PrdSizingMRRBreakage
                                                    {
                                                        SizeBreakageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                        SizeBreakageMasterID = (long)item.SizeBreakageMasterID,
                                                        BreakageDate = (DateTime)itemMaster.SizeMRRDate,
                                                        BreakageType = m.BreakageType,
                                                        NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                        BreakageID = (int)m.BWSID,
                                                        MachineID = m.MachineID,
                                                        Description = m.Description,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    DetailItemBreakageType.Add(SDetailBreak);
                                                    GenericFactory_EF_PrdSizingMRRBreakage.updateMaxID("PrdSizingMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits)));
                                                }

                                                if (BreakCount == LastRowNum)
                                                {
                                                    foreach (PrdSizingMRRBreakageM bm in BreakageMasterAll.Where(x => x.SizeBreakageMasterID == item.SizeBreakageMasterID))
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
                                                            bm.TotalBCBreakage = (int)item.TotalBCBreakage;
                                                            bm.TotalHSBreakage = (int)item.TotalHSBreakage;

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

                                    foreach (PrdSizingMRRDetail d in DetailAll.Where(d => d.SizeMRRID == item.SizeMRRID && d.SizeMRRDetailID == item.SizeMRRDetailID))
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
                                            d.BeamLength = item.BeamLength;
                                            d.BeamQualityID = (int)item.BeamQualityID;
                                            d.StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime);
                                            d.EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime);
                                            d.MachineSpeed = item.MachineSpeed;
                                            d.OperatorID = item.OperatorID;
                                            d.OutputUnitID = item.OutputUnitID;
                                            d.OverallStretch = item.OverallStretch;
                                            d.Remarks = item.Description;
                                            d.ShiftEngineerID = item.ShiftEngineerID;
                                            d.ShiftID = item.ShiftID;
                                            d.SqueezingActual = item.SqueezingActual;
                                            d.SqueezingSTD = item.SqueezingSTD;
                                            d.TotalBCBreakage = item.TotalBCBreakage;
                                            d.TotalHSBreakage = item.TotalHSBreakage;
                                            d.TotalStop = item.TotalStop;

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
                                MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRDetail.getMaxID("PrdSizingMRRDetail"));
                                MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                                MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                                StopFirstDigit = 0; StopOtherDigits = 0; BreakFirstDigit = 0; BreakOtherDigits = 0;
                                //***********************************************End Generate Master & Detail ID*****************************************                                
                                for (int i = 0; i < SMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Save").ToList()[i];

                                    if (StopRowNum > 0)
                                    {
                                        StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                        if (StopCount > 0)
                                        {
                                            StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdSizingMRRMachineStopM.getMaxID("PrdSizingMRRMachineStopM"));
                                            StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRMachineStop.getMaxID("PrdSizingMRRMachineStop"));
                                            StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                            StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                            LastRowNum = 1;
                                            foreach (vmPrdSizingMRRMaster S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                var DetailStop = new PrdSizingMRRMachineStop
                                                {
                                                    SizeMachineStopID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                    SizeMachineStopMasterID = StopMasterId,
                                                    Description = S.Description,
                                                    MachineID = S.MachineID,
                                                    ShiftID = (int)S.ShiftID,
                                                    StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                    StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                    StopDate = (DateTime)itemMaster.SizeMRRDate,
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
                                                    var MasterStop = new PrdSizingMRRMachineStopM
                                                    {
                                                        SizeMachineStopMasterID = StopMasterId,
                                                        TotalStop = (int)item.TotalStop,
                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    MasterItemMachineStop.Add(MasterStop);
                                                    GenericFactory_EF_PrdSizingMRRMachineStopM.updateMaxID("PrdSizingMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                    GenericFactory_EF_PrdSizingMRRMachineStop.updateMaxID("PrdSizingMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
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
                                            BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdSizingMRRBreakageM.getMaxID("PrdSizingMRRBreakageM"));
                                            BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdSizingMRRBreakage.getMaxID("PrdSizingMRRBreakage"));
                                            BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                            BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                            LastRowNum = 1;
                                            foreach (vmPrdSizingMRRMaster m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                            {
                                                var DetailBreak = new PrdSizingMRRBreakage
                                                {
                                                    SizeBreakageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                    SizeBreakageMasterID = BreakMasterId,
                                                    BreakageDate = (DateTime)itemMaster.SizeMRRDate,
                                                    BreakageType = m.BreakageType,
                                                    NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                    BreakageID = (int)m.BWSID,
                                                    MachineID = m.MachineID,
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
                                                    var MasterBreak = new PrdSizingMRRBreakageM
                                                    {
                                                        SizeBreakageMasterID = BreakMasterId,
                                                        TotalBCBreakage = (int)item.TotalBCBreakage,
                                                        TotalHSBreakage = (int)item.TotalHSBreakage,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    MasterItemBreakageType.Add(MasterBreak);
                                                    GenericFactory_EF_PrdSizingMRRBreakageM.updateMaxID("PrdSizingMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                    GenericFactory_EF_PrdSizingMRRBreakage.updateMaxID("PrdSizingMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    //string ds = item.BeginTime.ToString();

                                    var Detailitem = new PrdSizingMRRDetail
                                    {
                                        SizeMRRDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                        SizeMRRID = itemMaster.SizeMRRID,
                                        BeamLength = item.BeamLength,
                                        BeamQualityID = (int)item.BeamQualityID,
                                        StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime),
                                        EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime),
                                        MachineSpeed = item.MachineSpeed,
                                        OperatorID = item.OperatorID,
                                        OutputUnitID = item.OutputUnitID,
                                        OverallStretch = item.OverallStretch,
                                        Remarks = item.Description,
                                        ShiftEngineerID = item.ShiftEngineerID,
                                        ShiftID = item.ShiftID,
                                        SizeBreakageMasterID = BreakMasterId,
                                        SizeMachineStopMasterID = StopMasterId,
                                        SqueezingActual = item.SqueezingActual,
                                        SqueezingSTD = item.SqueezingSTD,
                                        TotalBCBreakage = item.TotalBCBreakage,
                                        TotalHSBreakage = item.TotalHSBreakage,
                                        TotalStop = item.TotalStop,

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

                        if (itemMaster.SizeMRRID > 0)
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdSizingMRRMaster.Update(MasterItem);
                                GenericFactory_EF_PrdSizingMRRMaster.Save();
                            }
                            if (UMasterItemMachineStop != null && UMasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStopM.UpdateList(UMasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStopM.Save();
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStopM.Save();
                            }
                            if (UDetailItemMachineStop != null && UDetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStop.UpdateList(UDetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStop.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStop.Save();
                            }
                            if (UMasterItemBreakageType != null && UMasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakageM.UpdateList(UMasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakageM.Save();
                            }
                            if (MasterItemBreakageType != null && MasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakageM.Save();
                            }
                            if (UDetailItemBreakageType != null && UDetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakage.UpdateList(UDetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakage.Save();
                            }
                            if (DetailItemBreakageType != null && DetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakage.Save();
                            }
                            if (UDetailItemMain != null && UDetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRDetail.UpdateList(UDetailItemMain.ToList());
                                GenericFactory_EF_PrdSizingMRRDetail.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRDetail.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdSizingMRRDetail.Save();
                                GenericFactory_EF_PrdSizingMRRDetail.updateMaxID("PrdSizingMRRDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }
                        }
                        else
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdSizingMRRMaster.Insert(MasterItem);
                                GenericFactory_EF_PrdSizingMRRMaster.Save();
                                GenericFactory_EF_PrdSizingMRRMaster.updateMaxID("PrdSizingMRRMaster", Convert.ToInt64(MainMasterId));
                                GenericFactory_EF_PrdSizingMRRMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStopM.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdSizingMRRMachineStop.Save();
                            }
                            if (MasterItemBreakageType != null && MasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakageM.Save();
                            }
                            if (DetailItemBreakageType != null && DetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdSizingMRRBreakage.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdSizingMRRDetail.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdSizingMRRDetail.Save();
                                GenericFactory_EF_PrdSizingMRRDetail.updateMaxID("PrdSizingMRRDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
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

        public string DeletePrdSizingMasterDetail(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmPrdSizingMRRMaster_VM = new PrdSizingMRRMaster_VM();
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
                ht.Add("SizeMRRID", objcmnParam.id);

                spQuery = "[Delete_PrdSizingMRRMasterDetailByID]";
                result = GenericFactory_vmPrdSizingMRRMaster_VM.ExecuteCommandString(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }
    }
}
