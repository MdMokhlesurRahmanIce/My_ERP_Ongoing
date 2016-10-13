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
    public class WeavingLoomDataMgt //: iBallWarpingMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdWeavingLoomData> GenericFactory_EF_PrdWeavingLoomData = null;
        private iGenericFactory_EF<PrdWeavingLoomDetailData> GenericFactory_EF_PrdWeavingLoomDetailData = null;
        private iGenericFactory_EF<PrdWeavingLoomStop> GenericFactory_EF_PrdWeavingLoomStop = null;
        private iGenericFactory_EF<PrdWeavingLoomStopDetail> GenericFactory_EF_PrdWeavingLoomStopDetail = null;
        private iGenericFactory_EF<PrdWeavingMachineBook> GenericFactory_EF_PrdWeavingMachineBook = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_EF_PrdWeavingMachinConfig = null;
        private iGenericFactory<vmWeavingLoomDataMasterDetail> GenericFactory_vmWeavingLoomDataMasterDetail = null;

        public IEnumerable<vmWeavingLoomDataMasterDetail> GetDataToSetWeavingLoomDetail(vmCmnParameters objcmnParam)//, out int recordsTotal
        {
            IEnumerable<vmWeavingLoomDataMasterDetail> objLoomDetailList = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objLoomDetailList = (from MC in _ctxCmn.PrdWeavingMachinConfigs
                                         join MB in _ctxCmn.PrdWeavingMachineBooks on MC.MachineConfigID equals MB.MachineConfigID
                                         join CM in _ctxCmn.CmnItemMasters on MB.ItemID equals CM.ItemID
                                         join SM in _ctxCmn.PrdSizingMRRMasters on MB.SizeMRRID equals SM.SIzeMRRID
                                         join CL in _ctxCmn.CmnItemColors on CM.ItemColorID equals CL.ItemColorID
                                         where MC.CompanyID == objcmnParam.loggedCompany && MC.IsDeleted == false && MC.IsBook == true &&
                                         MB.CompanyID == objcmnParam.loggedCompany && MB.IsDeleted == false && MB.IsReleased == false &&
                                         objcmnParam.id == 0 ? true : MC.LineID == objcmnParam.id
                                         orderby MC.MachineConfigNo descending
                                         select new vmWeavingLoomDataMasterDetail
                                         {
                                             MachineConfigID = MC.MachineConfigID,
                                             MachineConfigNo = MC.MachineConfigNo,
                                             LineID = MC.LineID,
                                             SetID = MB.SetID,
                                             SizeMRRID = MB.SizeMRRID,
                                             SetNo = SM.SizeMRRNo,
                                             ItemID = CM.ItemID,
                                             ArticleNo = CM.ArticleNo,
                                             ItemColorID = CL.ItemColorID,
                                             ColorName = CL.ColorName
                                         }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLoomDetailList;
        }

        public IEnumerable<vmWeavingLoomDataMasterDetail> GetWeavingLoomDataMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmWeavingLoomDataMasterDetail = new vmWeavingLoomDataMasterDetail_VM();
            IEnumerable<vmWeavingLoomDataMasterDetail> LoomMaster = null;
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

                    spQuery = "[Get_WeavingLoomDataMaster]";
                    LoomMaster = GenericFactory_vmWeavingLoomDataMasterDetail.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdWeavingLoomDatas.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//LoomMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return LoomMaster;
        }

        public vmWeavingLoomDataMasterDetail GetWeavingLoomDataMasterByID(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmWeavingLoomDataMasterDetail = new vmWeavingLoomDataMasterDetail_VM();
            vmWeavingLoomDataMasterDetail SinglLoomMaster = null;
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
                    ht.Add("LoomRecordID", objcmnParam.id);

                    spQuery = "[Get_WeavingLoomDataMasterByID]";
                    SinglLoomMaster = GenericFactory_vmWeavingLoomDataMasterDetail.ExecuteQuerySingle(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return SinglLoomMaster;
        }

        public IEnumerable<vmWeavingLoomDataMasterDetail> GetWeavingLoomDataDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmWeavingLoomDataMasterDetail = new vmWeavingLoomDataMasterDetail_VM();
            IEnumerable<vmWeavingLoomDataMasterDetail> ListLoomDetail = null;
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
                    ht.Add("LoomRecordID", objcmnParam.id);

                    spQuery = "[Get_WeavingLoomDataDetailByID]";
                    ListLoomDetail = GenericFactory_vmWeavingLoomDataMasterDetail.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListLoomDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListLoomDetail;
        }

        public IEnumerable<vmWeavingLoomDataMasterDetail> GetStopDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmWeavingLoomDataMasterDetail = new vmWeavingLoomDataMasterDetail_VM();
            IEnumerable<vmWeavingLoomDataMasterDetail> ListStopDetail = null;
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
                    ht.Add("LoomRecordID", objcmnParam.id);

                    spQuery = "[Get_LoomStopDetailByID]";
                    ListStopDetail = GenericFactory_vmWeavingLoomDataMasterDetail.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListStopDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListStopDetail;
        }

        public string SaveUpdateWeavingLoom(vmWeavingLoomDataMasterDetail itemMaster, List<vmWeavingLoomDataMasterDetail> MainDetail, List<vmWeavingLoomDataMasterDetail> MachinStopDetail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty; long MainMasterId = 0, StopMasterId = 0,
                MainDetailId = 0, MainFirstDigit = 0, MainOtherDigits = 0, StopDetailId = 0, StopFirstDigit = 0, StopOtherDigits = 0;
                int MainRowNum = 0, SMainRowNum = 0, StopRowNum = 0, UMainRowNum = 0, LastRowNum = 0, StopCount = 0; string LoomRacordNo = "";
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_EF_PrdWeavingLoomData = new PrdWeavingLoomData_EF();
                GenericFactory_EF_PrdWeavingLoomDetailData = new PrdWeavingLoomDetailData_EF();
                GenericFactory_EF_PrdWeavingLoomStop = new PrdWeavingLoomStop_EF();
                GenericFactory_EF_PrdWeavingLoomStopDetail = new PrdWeavingLoomStopDetail_EF();
                GenericFactory_EF_PrdWeavingMachineBook = new PrdWeavingMachineBook_EF();
                GenericFactory_EF_PrdWeavingMachinConfig = new PrdWeavingMachinConfig_EF();

                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdWeavingLoomData();
                var DetailItemMain = new List<PrdWeavingLoomDetailData>();
                var MasterItemMachineStop = new List<PrdWeavingLoomStop>();
                var DetailItemMachineStop = new List<PrdWeavingLoomStopDetail>();

                var UDetailItemMain = new List<PrdWeavingLoomDetailData>();
                var UMasterItemMachineStop = new List<PrdWeavingLoomStop>();
                var UDetailItemMachineStop = new List<PrdWeavingLoomStopDetail>();
                var UpdateMachineBook = new List<PrdWeavingMachineBook>();
                var UpdateMachineConfig = new List<PrdWeavingMachinConfig>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmWeavingLoomDataMasterDetail item = new vmWeavingLoomDataMasterDetail();
                vmWeavingLoomDataMasterDetail itemStop = new vmWeavingLoomDataMasterDetail();
                //***************************************End Create Model Instance to get Data***********************************

                MainRowNum = Convert.ToInt32(MainDetail.Count());
                SMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Save").Count());
                UMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").Count());
                StopRowNum = Convert.ToInt32(MachinStopDetail.Count());

                //**************************************************Start Main Operation************************************************
                if (SMainRowNum > 0 || UMainRowNum > 0)
                {
                    try
                    {
                        if (itemMaster.LoomRecordID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MainMasterId = Convert.ToInt16(GenericFactory_EF_PrdWeavingLoomData.getMaxID("PrdWeavingLoomData"));
                            MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdWeavingLoomDetailData.getMaxID("PrdWeavingLoomDetailData"));
                            MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                            MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************
                            CustomNo = GenericFactory_EF_PrdWeavingLoomData.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                LoomRacordNo = MainMasterId.ToString();
                            }
                            else
                            {
                                LoomRacordNo = CustomNo;
                            }

                            MasterItem = new PrdWeavingLoomData
                            {
                                LoomRecordID = MainMasterId,
                                LoomRacordNo = LoomRacordNo,
                                //LineID = (int)itemMaster.LineID,
                                ShiftID = (int)itemMaster.ShiftID,
                                ProductionDate = (DateTime)itemMaster.ProductionDate,
                                Remarks = itemMaster.Remarks,
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
                                        StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdWeavingLoomStop.getMaxID("PrdWeavingLoomStop"));
                                        StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdWeavingLoomStopDetail.getMaxID("PrdWeavingLoomStopDetail"));
                                        StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                        StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmWeavingLoomDataMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                        {
                                            var DetailStop = new PrdWeavingLoomStopDetail
                                            {
                                                LoomStopDetailID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                LoomStopID = StopMasterId,
                                                StopDate = (DateTime)itemMaster.ProductionDate,
                                                ShiftID = (int)itemMaster.ShiftID,
                                                StopID = (int)S.BWSID,
                                                StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                StopInMin = S.StopInMin,
                                                Description = S.Description,
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
                                                var MasterStop = new PrdWeavingLoomStop
                                                {
                                                    LoomStopID = StopMasterId,
                                                    TotalStop = (int)item.TotalStop,
                                                    MachineConfigID = item.MachineConfigID,

                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemMachineStop.Add(MasterStop);
                                                GenericFactory_EF_PrdWeavingLoomStop.updateMaxID("PrdWeavingLoomStop", Convert.ToInt64(StopMasterId));
                                                GenericFactory_EF_PrdWeavingLoomStopDetail.updateMaxID("PrdWeavingLoomStopDetail", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
                                            }
                                            LastRowNum = LastRowNum + 1;
                                        }
                                    }
                                }

                                var Detailitem = new PrdWeavingLoomDetailData
                                {
                                    LoomRecordDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                    LoomRecordID = MainMasterId,
                                    MachineConfigID = item.MachineConfigID,
                                    SetID = item.SetID,
                                    SizeMRRID = item.SizeMRRID,
                                    LoomStopID = StopMasterId,
                                    ItemID = item.ItemID,
                                    ShiftID = item.ShiftID,
                                    WarpStop = item.WarpStop,
                                    WarpCMPX = item.WarpCMPX,
                                    WeftStop = item.WeftStop,
                                    WeftCMPX = item.WeftCMPX,
                                    OtherStop = item.OtherStop,
                                    StartATT = item.StartATT,
                                    RPM = item.RPM,
                                    Efficiency = item.Efficiency,
                                    RunTime = item.RunTime,
                                    Prodn = item.Prodn,
                                    OperatorID = (int)item.OperatorID,
                                    ShiftEngineerID = (int)item.ShiftEngineerID,
                                    Remarks = item.Remarks,
                                    IsReleased = item.IsReleased,

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
                                var MasterAll = GenericFactory_EF_PrdWeavingLoomData.GetAll().Where(x => x.LoomRecordID == itemMaster.LoomRecordID && x.CompanyID == objcmnParam.loggedCompany);
                                var DetailAll = GenericFactory_EF_PrdWeavingLoomDetailData.GetAll().Where(x => x.LoomRecordID == itemMaster.LoomRecordID && x.CompanyID == objcmnParam.loggedCompany);
                                var MachineMasterAll = GenericFactory_EF_PrdWeavingLoomStop.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var MachineDetailAll = GenericFactory_EF_PrdWeavingLoomStopDetail.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                //*************************************End Get Data From Related Table to Update*********************************                            

                                MasterItem = MasterAll.First(x => x.LoomRecordID == itemMaster.LoomRecordID);
                                //MasterItem.LineID = (int)itemMaster.LineID;
                                MasterItem.ShiftID = (int)itemMaster.ShiftID;
                                MasterItem.ProductionDate = (DateTime)itemMaster.ProductionDate;
                                MasterItem.Remarks = itemMaster.Remarks;

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
                                            foreach (vmWeavingLoomDataMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                foreach (PrdWeavingLoomStopDetail st in MachineDetailAll.Where(x => x.LoomStopDetailID == S.LoomStopDetailID && (S.ModelState == "Update" || S.ModelState == "Delete")))
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
                                                        st.ShiftID = (int)itemMaster.ShiftID;
                                                        st.StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime);
                                                        st.StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime);
                                                        st.StopDate = (DateTime)itemMaster.ProductionDate;
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
                                                    StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdWeavingLoomStopDetail.getMaxID("PrdWeavingLoomStopDetail"));
                                                    StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                                    StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));

                                                    var SDetailStop = new PrdWeavingLoomStopDetail
                                                    {
                                                        LoomStopDetailID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                        LoomStopID = (long)item.LoomStopID,
                                                        StopDate = (DateTime)itemMaster.ProductionDate,
                                                        ShiftID = (int)itemMaster.ShiftID,
                                                        StopID = (int)S.BWSID,
                                                        StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                        StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                        StopInMin = S.StopInMin,
                                                        Description = S.Description,
                                                        IsNextDate = S.IsNextDate,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    DetailItemMachineStop.Add(SDetailStop);
                                                    GenericFactory_EF_PrdWeavingLoomStopDetail.updateMaxID("PrdWeavingLoomStopDetail", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits)));
                                                }


                                                if (StopCount == LastRowNum)
                                                {
                                                    foreach (PrdWeavingLoomStop sm in MachineMasterAll.Where(x => x.LoomStopID == item.LoomStopID))
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
                                                            sm.MachineConfigID = item.MachineConfigID;
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

                                    foreach (PrdWeavingLoomDetailData d in DetailAll.Where(d => d.LoomRecordID == item.LoomRecordID && d.LoomRecordDetailID == item.LoomRecordDetailID))
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
                                            d.MachineConfigID = item.MachineConfigID;
                                            d.SetID = item.SetID;
                                            d.SizeMRRID = item.SizeMRRID;
                                            d.LoomStopID = item.LoomStopID;
                                            d.ItemID = item.ItemID;
                                            d.ShiftID = item.ShiftID;
                                            d.WarpStop = item.WarpStop;
                                            d.WarpCMPX = item.WarpCMPX;
                                            d.WeftStop = item.WeftStop;
                                            d.WeftCMPX = item.WeftCMPX;
                                            d.OtherStop = item.OtherStop;
                                            d.StartATT = item.StartATT;
                                            d.RPM = item.RPM;
                                            d.Efficiency = item.Efficiency;
                                            d.RunTime = item.RunTime;
                                            d.Prodn = item.Prodn;
                                            d.OperatorID = (int)item.OperatorID;
                                            d.ShiftEngineerID = (int)item.ShiftEngineerID;
                                            d.Remarks = item.Remarks;
                                            d.IsReleased = item.IsReleased;

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
                                MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdWeavingLoomDetailData.getMaxID("PrdWeavingLoomDetailData"));
                                MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                                MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                                //***********************************************End Generate Master & Detail ID*****************************************                                
                                for (int i = 0; i < SMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Save").ToList()[i];

                                    if (StopRowNum > 0)
                                    {
                                        StopCount = Convert.ToInt32(MachinStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                        if (StopCount > 0)
                                        {
                                            StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdWeavingLoomStop.getMaxID("PrdWeavingLoomStop"));
                                            StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdWeavingLoomStopDetail.getMaxID("PrdWeavingLoomStopDetail"));
                                            StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                            StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                            LastRowNum = 1;
                                            foreach (vmWeavingLoomDataMasterDetail S in MachinStopDetail.Where(x => x.SNo == item.SlNo))
                                            {
                                                var DetailStop = new PrdWeavingLoomStopDetail
                                                {
                                                    LoomStopDetailID = Convert.ToInt64(StopFirstDigit + "" + StopOtherDigits),
                                                    LoomStopID = StopMasterId,
                                                    StopDate = (DateTime)itemMaster.ProductionDate,
                                                    ShiftID = (int)itemMaster.ShiftID,
                                                    StopID = (int)S.BWSID,
                                                    StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                    StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                    StopInMin = S.StopInMin,
                                                    Description = S.Description,
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
                                                    var MasterStop = new PrdWeavingLoomStop
                                                    {
                                                        LoomStopID = StopMasterId,
                                                        TotalStop = (int)item.TotalStop,
                                                        MachineConfigID = item.MachineConfigID,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    MasterItemMachineStop.Add(MasterStop);
                                                    GenericFactory_EF_PrdWeavingLoomStop.updateMaxID("PrdWeavingLoomStop", Convert.ToInt64(StopMasterId));
                                                    GenericFactory_EF_PrdWeavingLoomStopDetail.updateMaxID("PrdWeavingLoomStopDetail", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
                                                }
                                                LastRowNum = LastRowNum + 1;
                                            }
                                        }
                                    }

                                    //string ds = item.BeginTime.ToString();

                                    var Detailitem = new PrdWeavingLoomDetailData
                                    {
                                        LoomRecordDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                        LoomRecordID = MainMasterId,
                                        MachineConfigID = item.MachineConfigID,
                                        SetID = item.SetID,
                                        SizeMRRID = item.SizeMRRID,
                                        LoomStopID = StopMasterId,
                                        ItemID = item.ItemID,
                                        ShiftID = item.ShiftID,
                                        WarpStop = item.WarpStop,
                                        WarpCMPX = item.WarpCMPX,
                                        WeftStop = item.WeftStop,
                                        WeftCMPX = item.WeftCMPX,
                                        OtherStop = item.OtherStop,
                                        StartATT = item.StartATT,
                                        RPM = item.RPM,
                                        Efficiency = item.Efficiency,
                                        RunTime = item.RunTime,
                                        Prodn = item.Prodn,
                                        OperatorID = (int)item.OperatorID,
                                        ShiftEngineerID = (int)item.ShiftEngineerID,
                                        Remarks = item.Remarks,
                                        IsReleased = item.IsReleased,

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

                        for (int s = 0; s < MainRowNum; s++)
                        {
                            item = MainDetail[s];
                            var UMBook = GenericFactory_EF_PrdWeavingMachineBook.GetAll().FirstOrDefault(x => x.MachineConfigID == item.MachineConfigID);
                            UMBook.IsReleased = item.IsReleased;
                            UMBook.ReleaseDate = item.IsReleased == true ? itemMaster.ProductionDate : DateTime.Parse("1900/01/01");
                            UpdateMachineBook.Add(UMBook);

                            var UMConfig = GenericFactory_EF_PrdWeavingMachinConfig.GetAll().FirstOrDefault(x => x.MachineConfigID == item.MachineConfigID);
                            UMConfig.IsBook = item.IsReleased == true ? false : true;
                            UMConfig.ReleaseDate = item.IsReleased == true ? itemMaster.ProductionDate : DateTime.Parse("1900/01/01");
                            UpdateMachineConfig.Add(UMConfig);
                        }

                        if (itemMaster.LoomRecordID > 0)
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdWeavingLoomData.Update(MasterItem);
                                GenericFactory_EF_PrdWeavingLoomData.Save();
                            }
                            if (UMasterItemMachineStop != null && UMasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStop.UpdateList(UMasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStop.Save();
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStop.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStop.Save();
                            }
                            if (UDetailItemMachineStop != null && UDetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStopDetail.UpdateList(UDetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStopDetail.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStopDetail.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStopDetail.Save();
                            }
                            if (UDetailItemMain != null && UDetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomDetailData.UpdateList(UDetailItemMain.ToList());
                                GenericFactory_EF_PrdWeavingLoomDetailData.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomDetailData.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdWeavingLoomDetailData.Save();
                                GenericFactory_EF_PrdWeavingLoomDetailData.updateMaxID("PrdWeavingLoomDetailData", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }
                        }
                        else
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdWeavingLoomData.Insert(MasterItem);
                                GenericFactory_EF_PrdWeavingLoomData.Save();
                                GenericFactory_EF_PrdWeavingLoomData.updateMaxID("PrdWeavingLoomData", Convert.ToInt64(MainMasterId));
                                GenericFactory_EF_PrdWeavingLoomData.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStop.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStop.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomStopDetail.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdWeavingLoomStopDetail.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdWeavingLoomDetailData.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdWeavingLoomDetailData.Save();
                                GenericFactory_EF_PrdWeavingLoomDetailData.updateMaxID("PrdWeavingLoomDetailData", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }
                        }
                        if (UpdateMachineBook != null && UpdateMachineBook.Count != 0)
                        {
                            GenericFactory_EF_PrdWeavingMachineBook.UpdateList(UpdateMachineBook.ToList());
                            GenericFactory_EF_PrdWeavingMachineBook.Save();
                        }
                        if (UpdateMachineConfig != null && UpdateMachineConfig.Count != 0)
                        {
                            GenericFactory_EF_PrdWeavingMachinConfig.UpdateList(UpdateMachineConfig.ToList());
                            GenericFactory_EF_PrdWeavingMachinConfig.Save();
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

        public string DeletePrdWeavingLoomMasterDetail(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmWeavingLoomDataMasterDetail = new vmWeavingLoomDataMasterDetail_VM();
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
                ht.Add("LoomRecordID", objcmnParam.id);

                spQuery = "[Delete_WeavingLoomDataMasterDetailByID]";
                result = GenericFactory_vmWeavingLoomDataMasterDetail.ExecuteCommandString(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }
    }
}
