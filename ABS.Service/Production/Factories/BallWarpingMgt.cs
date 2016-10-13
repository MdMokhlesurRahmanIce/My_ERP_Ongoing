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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class BallWarpingMgt //: iBallWarpingMgt
    {
        private ERP_Entities _ctxCmn = null;
        //private iGenericFactory_EF<PrdBWSlist> GenericFactory_PrdBWSlist_EF = null;
        private iGenericFactory_EF<InvStockMaster> GenericFactory_EF_InvStockMaster = null;
        private iGenericFactory_EF<PrdBallMRRMaster> GenericFactory_EF_PrdBallMRRMaster = null;
        private iGenericFactory_EF<PrdBallMRRDetail> GenericFactory_EF_PrdBallMRRDetail = null;
        private iGenericFactory_EF<PrdBallMRRBreakage> GenericFactory_EF_PrdBallMRRBreakage = null;
        private iGenericFactory_EF<PrdBallMRRBreakageM> GenericFactory_EF_PrdBallMRRBreakageM = null;
        private iGenericFactory_EF<PrdBallMRRConsumption> GenericFactory_EF_PrdBallMRRConsumption = null;
        private iGenericFactory_EF<PrdBallMRRMachineStop> GenericFactory_EF_PrdBallMRRMachineStop = null;
        private iGenericFactory_EF<PrdBallMRRMachineStopM> GenericFactory_EF_PrdBallMRRMachineStopM = null;
        private iGenericFactory<vmBallWarpingInformation> GenericFactory_BallWarpingInformation_GF = null;
        private iGenericFactory<vmBallConsumption> GenericFactory_vmBallConsumption_GF = null;
        private iGenericFactory<vmBallMachineStopAndBrekage> GenericFactory_BallMachineStopAndBreakage_GF = null;
        private iGenericFactory<vmBallInfo> GenericFactory_vmBallInfo_GF = null;

        public IEnumerable<vmBallWarpingInformation> GetBallWarpingMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBallWarpingInformation> objBallMRRMaster = null;
            recordsTotal = 0;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    var PrdBallMRRMaster = _ctxCmn.PrdBallMRRMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList();
                    var PrdSetSetup = _ctxCmn.PrdSetSetups.ToList();//.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList();

                    objBallMRRMaster = (from bmrrm in PrdBallMRRMaster
                                        join ssu in PrdSetSetup on bmrrm.SetID equals ssu.SetID
                                        select new
                                        {
                                            SetID = bmrrm.SetID,
                                            BalMRRID = bmrrm.BalMRRID,
                                            BalMRRNo = bmrrm.BalMRRNo,
                                            BalMRRDate = bmrrm.BalMRRDate,
                                            //MachineSpeed = ssu.MachineSpeed,
                                            YarnRatioLot = ssu.YarnRatioLot,
                                            NoOfBall = ssu.NoOfBall,
                                            SetLength = ssu.SetLength

                                        }).Select(m => new vmBallWarpingInformation
                                        {
                                            SetID = m.SetID == null ? 0 : (long)m.SetID,
                                            BalMRRID = m.BalMRRID,
                                            BalMRRNo = m.BalMRRNo,
                                            BalMRRDate = m.BalMRRDate,
                                            //MachineSpeed = Convert.ToInt16(m.MachineSpeed),
                                            YarnRatioLot = m.YarnRatioLot,
                                            NoOfBall = m.NoOfBall == null ? 0 : (int)m.NoOfBall,
                                            SetLength = m.SetLength == null ? 0 : (int)m.SetLength
                                        }).ToList();

                    recordsTotal = objBallMRRMaster.Count();
                    objBallMRRMaster = objBallMRRMaster.OrderByDescending(x => x.BalMRRID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBallMRRMaster;
        }

        public IEnumerable<vmBallWarpingInformation> GetSetInformation(vmCmnParameters objcmnParam)
        {
            GenericFactory_BallWarpingInformation_GF = new vmBallWarpingInformation_GF();
            IEnumerable<vmBallWarpingInformation> objSetInformation = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("SetID", objcmnParam.id);
                ht.Add("MRRID", objcmnParam.ItemType);

                spQuery = "[Get_SetInformation]";
                objSetInformation = GenericFactory_BallWarpingInformation_GF.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetInformation;
        }

        public vmBallInfo GetItemWiseStock(vmBallInfo ItemInfo, vmCmnParameters objcmnParam)
        {
            GenericFactory_vmBallInfo_GF = new vmBallInfo_GF();
            vmBallInfo objItemStock = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("DepartmentID", objcmnParam.DepartmentID);
                ht.Add("ItemID", ItemInfo.ItemID);
                ht.Add("LotID", ItemInfo.LotID);
                ht.Add("SupplierID", ItemInfo.SupplierID);

                spQuery = "[Get_BallConsumptionItemWiseStock]";
                objItemStock = GenericFactory_vmBallInfo_GF.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemStock;
        }

        //public vmBallInfo GetItemWiseStock(vmBallInfo ItemInfo, vmCmnParameters objcmnParam)
        //{
        //    vmBallInfo objItemStock = null;
        //    using (ERP_Entities _ctxCmn = new ERP_Entities())
        //    {
        //        var totalStock = _ctxCmn.InvStockMasters.Where(x => x.ItemID == ItemInfo.ItemID && x.LotID == ItemInfo.LotID && x.DepartmentID == objcmnParam.DepartmentID && x.CompanyID == objcmnParam.loggedCompany && ItemInfo.SupplierID == null ? true : x.SupplierID == ItemInfo.SupplierID).Select(x => x.CurrentStock).Sum();

        //        objItemStock = (from s in _ctxCmn.InvStockMasters.Where(x => x.ItemID == ItemInfo.ItemID && x.LotID == ItemInfo.LotID && x.DepartmentID == objcmnParam.DepartmentID && x.CompanyID == objcmnParam.loggedCompany && ItemInfo.SupplierID == null ? true : x.SupplierID == ItemInfo.SupplierID)
        //                        select new
        //                        {
        //                            ItemID = s.ItemID == null ? 0 : s.ItemID,
        //                            UnitPrice = s.CurrentRate == null ? 0 : s.CurrentRate,
        //                            CurrentStock = totalStock == null ? 0 : (decimal)totalStock
        //                        }).Select(x => new vmBallInfo
        //                      {
        //                          ItemID = x.ItemID,
        //                          UnitPrice = x.UnitPrice,
        //                          CurrentStock = x.CurrentStock
        //                      }).FirstOrDefault();
        //    }

        //    return objItemStock;
        //}

        public IEnumerable<vmBallWarpingInformation> GetBallWarpingDetail(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBallWarpingInformation> objBallDetail = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    var Output = _ctxCmn.PrdOutputUnits.Where(x => x.ProcessID == objcmnParam.DepartmentID).ToList();
                    var Machines = _ctxCmn.PrdWeavingMachinConfigs.Where(x => x.DepartmentID == objcmnParam.DepartmentID && x.IsMaintenance == false && x.IsBook == false).ToList();
                    var Shift = _ctxCmn.HRMShifts.ToList();
                    var Operator = _ctxCmn.CmnUsers.ToList();
                    var DutyOfficer = _ctxCmn.CmnUsers.ToList();

                    objBallDetail = (from HD in Output
                                     select new vmBallWarpingInformation
                                     {
                                         OutputNos =
                                             (from L in Output
                                              //where L.OutputNo != null
                                              select new vmBallInfo { OutputID = L.OutputID, OutputNo = L.OutputNo }).ToList(),
                                         MachineNos =
                                            (from B in Machines
                                             //where B.ItemTypeID != null
                                             select new vmMachineNo { ItemID = B.MachineConfigID, ItemName = B.MachineConfigNo }).ToList(),

                                         ShiftNames =
                                        (from B in Shift
                                         select new vmShiftName { ShiftID = B.ShiftID, ShiftName = B.ShiftName }).ToList(),

                                         Operators =
                                       (from B in Operator
                                        where B.UserTypeID == 1
                                        select new vmOperator { UserID = B.UserID, UserFullName = B.UserFullName }).ToList(),

                                         DutyOfficers =
                                            (from B in Operator
                                             where B.UserTypeID == 1
                                             select new vmOperator { UserID = B.UserID, UserFullName = B.UserFullName }).ToList()

                                     }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objBallDetail;
        }

        public IEnumerable<vmBallWarpingInformation> GetBallWarpingMasterById(vmCmnParameters objcmnParam)
        {
            GenericFactory_BallWarpingInformation_GF = new vmBallWarpingInformation_GF();
            IEnumerable<vmBallWarpingInformation> objBWMaster = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("BMRRID", objcmnParam.id);

                spQuery = "[Get_BallWarpingMasterById]";
                objBWMaster = GenericFactory_BallWarpingInformation_GF.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBWMaster;
        }

        public IEnumerable<vmBallWarpingInformation> GetBallDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_BallWarpingInformation_GF = new vmBallWarpingInformation_GF();
            IEnumerable<vmBallWarpingInformation> ListBallDetail = null;
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
                    ht.Add("BallMRRID", objcmnParam.id);

                    spQuery = "[Get_BallWarpingDetailById]";
                    ListBallDetail = GenericFactory_BallWarpingInformation_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListBallDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListBallDetail;
        }

        public IEnumerable<vmBallMachineStopAndBrekage> GetStopDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_BallMachineStopAndBreakage_GF = new vmBallMachineStopAndBrekage_GF();
            IEnumerable<vmBallMachineStopAndBrekage> ListStopDetail = null;
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
                    ht.Add("BallMRRID", objcmnParam.id);

                    spQuery = "[Get_BallStopDetailByID]";
                    ListStopDetail = GenericFactory_BallMachineStopAndBreakage_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListStopDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListStopDetail;
        }

        public IEnumerable<vmBallMachineStopAndBrekage> GetBreakageDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_BallMachineStopAndBreakage_GF = new vmBallMachineStopAndBrekage_GF();
            IEnumerable<vmBallMachineStopAndBrekage> ListBreakageDetail = null;
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
                    ht.Add("BallMRRID", objcmnParam.id);

                    spQuery = "[Get_BallBreakageDetailByID]";
                    ListBreakageDetail = GenericFactory_BallMachineStopAndBreakage_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListBreakageDetail.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListBreakageDetail;
        }

        public IEnumerable<vmBallConsumption> GetBallConsumptionByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmBallConsumption_GF = new vmBallConsumption_GF();
            IEnumerable<vmBallConsumption> ListConsumption = null;
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
                    ht.Add("BallMRRID", objcmnParam.id);

                    spQuery = "[Get_BallConsumptionByID]";
                    ListConsumption = GenericFactory_vmBallConsumption_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListConsumption.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListConsumption;
        }

        public string SaveUpdateBallMRR(PrdBallMRRMaster itemMaster, List<vmBallWarpingInformation> MainDetail, List<vmBallMachineStopAndBrekage> MachineStopDetail,
            List<vmBallMachineStopAndBrekage> BreakageTypeDetail, List<vmBallConsumption> ConsumptionInfo, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty; long? MainMasterId = 0, BreakMasterId = 0, StopMasterId = 0, ConsumptionID = 0,
                MainDetailId = 0, MainFirstDigit = 0, MainOtherDigits = 0, StopDetailId = 0, StopFirstDigit = 0, StopOtherDigits = 0,
                BreakDetailId = 0, BreakFirstDigit = 0, BreakOtherDigits = 0;
                int SMainRowNum = 0, StopRowNum = 0, BreakageRowNum = 0, ConsumptionRowNum = 0, UMainRowNum = 0,
                    LastRowNum = 0, StopCount = 0, BreakCount = 0, ConsumptionCount = 0; string BalMRRNo = "";
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_EF_InvStockMaster = new InvStockMaster_EF();
                GenericFactory_EF_PrdBallMRRMaster = new PrdBallMRRMaster_EF();
                GenericFactory_EF_PrdBallMRRDetail = new PrdBallMRRDeatail_EF();
                GenericFactory_EF_PrdBallMRRMachineStop = new PrdBallMRRMachineStop_EF();
                GenericFactory_EF_PrdBallMRRMachineStopM = new PrdBallMRRMachineStopM_EF();
                GenericFactory_EF_PrdBallMRRBreakage = new PrdBallMRRBreakage_EF();
                GenericFactory_EF_PrdBallMRRBreakageM = new PrdBallMRRBreakageM_EF();
                GenericFactory_EF_PrdBallMRRConsumption = new PrdBallMRRConsumption_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdBallMRRMaster();
                var DetailItemMain = new List<PrdBallMRRDetail>();
                var MasterItemMachineStop = new List<PrdBallMRRMachineStopM>();
                var DetailItemMachineStop = new List<PrdBallMRRMachineStop>();
                var MasterItemBreakageType = new List<PrdBallMRRBreakageM>();
                var DetailItemBreakageType = new List<PrdBallMRRBreakage>();
                var ListPrdBallMRRConsumption = new List<PrdBallMRRConsumption>();

                var UDetailItemMain = new List<PrdBallMRRDetail>();
                var UMasterItemMachineStop = new List<PrdBallMRRMachineStopM>();
                var UDetailItemMachineStop = new List<PrdBallMRRMachineStop>();
                var UMasterItemBreakageType = new List<PrdBallMRRBreakageM>();
                var UDetailItemBreakageType = new List<PrdBallMRRBreakage>();
                var UListPrdBallMRRConsumption = new List<PrdBallMRRConsumption>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmBallWarpingInformation item = new vmBallWarpingInformation();
                vmBallMachineStopAndBrekage itemStop = new vmBallMachineStopAndBrekage();
                vmBallMachineStopAndBrekage itemBreak = new vmBallMachineStopAndBrekage();
                //***************************************End Create Model Instance to get Data***********************************

                SMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Save").Count());
                UMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Update").Count());
                StopRowNum = Convert.ToInt32(MachineStopDetail.Count());
                BreakageRowNum = Convert.ToInt32(BreakageTypeDetail.Count());
                ConsumptionRowNum = Convert.ToInt32(ConsumptionInfo.Count());

                //**************************************************Start Main Operation************************************************
                if (SMainRowNum > 0 || UMainRowNum > 0)
                {
                    try
                    {
                        if (itemMaster.BalMRRID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MainMasterId = Convert.ToInt16(GenericFactory_EF_PrdBallMRRMaster.getMaxID("PrdBallMRRMaster"));
                            MainDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRDetail.getMaxID("PrdBallMRRDetail"));
                            MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                            MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************
                            CustomNo = GenericFactory_EF_PrdBallMRRMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                BalMRRNo = MainMasterId.ToString();
                            }
                            else
                            {
                                BalMRRNo = CustomNo;
                            }

                            MasterItem = new PrdBallMRRMaster
                            {
                                BalMRRID = (long)MainMasterId,
                                Description = itemMaster.Description,
                                ItemID = (long)itemMaster.ItemID,
                                PIID = itemMaster.PIID,
                                SetID = itemMaster.SetID,
                                BalMRRDate = (DateTime)itemMaster.BalMRRDate,
                                BalMRRNo = BalMRRNo,

                                TransactionTypeID = objcmnParam.tTypeId,
                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false
                            };
                            for (int i = 0; i < SMainRowNum; i++)
                            {
                                ConsumptionID = 0; StopMasterId = 0; BreakMasterId = 0;
                                item = MainDetail[i];

                                if (StopRowNum > 0)
                                {
                                    StopCount = Convert.ToInt32(MachineStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                    if (StopCount > 0)
                                    {
                                        StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdBallMRRMachineStopM.getMaxID("PrdBallMRRMachineStopM"));
                                        StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRMachineStop.getMaxID("PrdBallMRRMachineStop"));
                                        StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                        StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmBallMachineStopAndBrekage S in MachineStopDetail.Where(x => x.SNo == item.SlNo))
                                        {
                                            var DetailStop = new PrdBallMRRMachineStop
                                            {
                                                BalMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                BalMachineStopMasterID = StopMasterId,
                                                Description = S.Description,
                                                MachineID = S.MachineID,
                                                ShiftID = (int)S.ShiftID,
                                                StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                StopDate = (DateTime)itemMaster.BalMRRDate,
                                                StopID = (int)S.BWSID,
                                                StopInMin = S.StopInMin,

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
                                                var MasterStop = new PrdBallMRRMachineStopM
                                                {
                                                    BalMachineStopMasterID = (long)StopMasterId,
                                                    TotalStop = (int)item.TotalStop,
                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemMachineStop.Add(MasterStop);
                                                GenericFactory_EF_PrdBallMRRMachineStopM.updateMaxID("PrdBallMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                GenericFactory_EF_PrdBallMRRMachineStop.updateMaxID("PrdBallMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
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
                                        BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdBallMRRBreakageM.getMaxID("PrdBallMRRBreakageM"));
                                        BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRBreakage.getMaxID("PrdBallMRRBreakage"));
                                        BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                        BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                        LastRowNum = 1;
                                        foreach (vmBallMachineStopAndBrekage m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                        {
                                            var DetailBreak = new PrdBallMRRBreakage
                                            {
                                                BallBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                BallBreackageMasterID = BreakMasterId,
                                                BreakageDate = (DateTime)itemMaster.BalMRRDate,
                                                NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                BreakageID = (int)m.BWSID,
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
                                                var MasterBreak = new PrdBallMRRBreakageM
                                                {
                                                    BallBreackageMasterID = (long)BreakMasterId,
                                                    TotalBreakage = (int)item.TotalBreakage,

                                                    CompanyID = objcmnParam.loggedCompany,
                                                    CreateBy = objcmnParam.loggeduser,
                                                    CreateOn = DateTime.Now,
                                                    CreatePc = HostService.GetIP(),
                                                    IsDeleted = false
                                                };
                                                MasterItemBreakageType.Add(MasterBreak);
                                                GenericFactory_EF_PrdBallMRRBreakageM.updateMaxID("PrdBallMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                GenericFactory_EF_PrdBallMRRBreakage.updateMaxID("PrdBallMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                            }
                                            LastRowNum = LastRowNum + 1;
                                        }
                                    }
                                }
                                if (ConsumptionRowNum > 0)
                                {
                                    ConsumptionCount = Convert.ToInt32(ConsumptionInfo.Where(b => b.SlNo == item.SlNo).Count());
                                    if (ConsumptionCount > 0)
                                    {
                                        ConsumptionID = Convert.ToInt16(GenericFactory_EF_PrdBallMRRConsumption.getMaxID("PrdBallMRRConsumption"));
                                        foreach (vmBallConsumption m in ConsumptionInfo.Where(x => x.SlNo == item.SlNo))
                                        {                                            
                                            var ConsumptionItem = new PrdBallMRRConsumption
                                            {
                                                BallConsumptionID = (long)ConsumptionID,
                                                YarnCountID = (int)m.YarnCountID,
                                                SupplierID = m.SupplierID,
                                                DepartmentID = objcmnParam.DepartmentID,
                                                LotID = m.LotID,
                                                LengthM = m.LengthM,
                                                LengthYds = m.LengthYds,
                                                Qty = m.Qty,
                                                Unit = m.Unit,
                                                UnitPrice = m.UnitPrice,
                                                Amount = m.Amount,
                                                Remarks = m.Remarks,
                                                TransactionTypeID=objcmnParam.tTypeId,
                                                IssueDate=itemMaster.BalMRRDate,

                                                CompanyID = objcmnParam.loggedCompany,
                                                CreateBy = objcmnParam.loggeduser,
                                                CreateOn = DateTime.Now,
                                                CreatePc = HostService.GetIP(),
                                                IsDeleted = false
                                            };
                                            ListPrdBallMRRConsumption.Add(ConsumptionItem);
                                            GenericFactory_EF_PrdBallMRRConsumption.updateMaxID("PrdBallMRRConsumption", Convert.ToInt64(ConsumptionID));

                                            ConsumptionRowNum = ConsumptionRowNum - 1;
                                        }
                                    }
                                }

                                var Detailitem = new PrdBallMRRDetail
                                {
                                    BalMRRDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                    BalMRRID = (long)MainMasterId,
                                    StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime),
                                    EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime),
                                    MachineSpeed = item.MachineSpeed,
                                    OperatorID = item.OperatorID,
                                    OutputUnitID = item.OutputUnitID,
                                    Remarks = item.Remarks,
                                    ShiftEngineerID = item.ShiftEngineerID,
                                    ShiftID = item.ShiftID,
                                    MachineID = item.MachineID,
                                    BallConsumptionID = ConsumptionID == 0 ? null : ConsumptionID,
                                    BallBreackageMasterID = BreakMasterId == 0 ? null : BreakMasterId,
                                    BalMachineStopID = StopMasterId == 0 ? null : StopMasterId,
                                    TotalBreakage = item.TotalBreakage,
                                    TotalStop = item.TotalStop,
                                    WarpingDate = DateTime.Parse(item.StrWarpingDate),
                                    SetLength = item.LengthPerBall,
                                    YarnCountID = item.YarnCountID,
                                    LotID = item.LotID,
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
                                var MasterAll = GenericFactory_EF_PrdBallMRRMaster.GetAll().Where(x => x.BalMRRID == itemMaster.BalMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var DetailAll = GenericFactory_EF_PrdBallMRRDetail.GetAll().Where(x => x.BalMRRID == itemMaster.BalMRRID && x.CompanyID == objcmnParam.loggedCompany);
                                var MachineMasterAll = GenericFactory_EF_PrdBallMRRMachineStopM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var MachineDetailAll = GenericFactory_EF_PrdBallMRRMachineStop.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageMasterAll = GenericFactory_EF_PrdBallMRRBreakageM.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var BreakageDetailAll = GenericFactory_EF_PrdBallMRRBreakage.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                var ConsumptionAll = GenericFactory_EF_PrdBallMRRConsumption.GetAll().Where(x => x.CompanyID == objcmnParam.loggedCompany);
                                //*************************************End Get Data From Related Table to Update*********************************                            

                                MasterItem = MasterAll.First(x => x.BalMRRID == itemMaster.BalMRRID);
                                MasterItem.Description = itemMaster.Description;
                                MasterItem.ItemID = (long)itemMaster.ItemID;
                                MasterItem.SetID = itemMaster.SetID;
                                MasterItem.PIID = itemMaster.PIID;
                                MasterItem.BalMRRDate = (DateTime)itemMaster.BalMRRDate;

                                MasterItem.TransactionTypeID = objcmnParam.tTypeId;
                                MasterItem.CompanyID = objcmnParam.loggedCompany;
                                MasterItem.UpdateBy = objcmnParam.loggeduser;
                                MasterItem.UpdateOn = DateTime.Now;
                                MasterItem.UpdatePc = HostService.GetIP();

                                for (int i = 0; i < UMainRowNum; i++)
                                {
                                    ConsumptionID = 0; StopMasterId = 0; BreakMasterId = 0;
                                    item = MainDetail.Where(x => x.ModelState == "Update").ToList()[i];

                                    if (item.BalMachineStopID != null)
                                    {
                                        StopMasterId = item.BalMachineStopID;
                                        if (StopRowNum > 0)
                                        {
                                            StopCount = Convert.ToInt32(MachineStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                            if (StopCount > 0)
                                            {
                                                LastRowNum = 1;
                                                foreach (vmBallMachineStopAndBrekage S in MachineStopDetail.Where(x => x.SNo == item.SlNo))
                                                {
                                                    foreach (PrdBallMRRMachineStop st in MachineDetailAll.Where(x => x.BalMachineStopID == S.BallMachineStopID && (S.ModelState == "Update" || S.ModelState == "Delete")))
                                                    {
                                                        if (S.ModelState == "Delete")
                                                        {
                                                            st.CompanyID = objcmnParam.loggedCompany;
                                                            st.DeleteBy = objcmnParam.loggeduser;
                                                            st.DeleteOn = DateTime.Now;
                                                            st.DeletePc = HostService.GetIP();
                                                        }
                                                        else
                                                        {
                                                            st.Description = S.Description;
                                                            st.MachineID = S.MachineID;
                                                            st.ShiftID = (int)S.ShiftID;
                                                            st.StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime);
                                                            st.StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime);
                                                            st.StopDate = (DateTime)itemMaster.BalMRRDate;
                                                            st.StopID = (int)S.BWSID;
                                                            st.StopInMin = S.StopInMin;

                                                            st.CompanyID = objcmnParam.loggedCompany;
                                                            st.UpdateBy = objcmnParam.loggeduser;
                                                            st.UpdateOn = DateTime.Now;
                                                            st.UpdatePc = HostService.GetIP();
                                                        }

                                                        UDetailItemMachineStop.Add(st);
                                                    };

                                                    if (S.ModelState == "Save")
                                                    {
                                                        StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRMachineStop.getMaxID("PrdBallMRRMachineStop"));
                                                        StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                                        StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));

                                                        var SDetailStop = new PrdBallMRRMachineStop
                                                        {
                                                            BalMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                            BalMachineStopMasterID = (long)item.BalMachineStopID,
                                                            Description = S.Description,
                                                            MachineID = S.MachineID,
                                                            ShiftID = (int)S.ShiftID,
                                                            StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                            StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                            StopDate = (DateTime)itemMaster.BalMRRDate,
                                                            StopID = (int)S.BWSID,
                                                            StopInMin = S.StopInMin,

                                                            CompanyID = objcmnParam.loggedCompany,
                                                            CreateBy = objcmnParam.loggeduser,
                                                            CreateOn = DateTime.Now,
                                                            CreatePc = HostService.GetIP(),
                                                            IsDeleted = false
                                                        };
                                                        UDetailItemMachineStop.Add(SDetailStop);
                                                        GenericFactory_EF_PrdBallMRRMachineStop.updateMaxID("PrdBallMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits)));
                                                    }

                                                    if (StopCount == LastRowNum)
                                                    {
                                                        foreach (PrdBallMRRMachineStopM sm in MachineMasterAll.Where(x => x.BalMachineStopMasterID == item.BalMachineStopID))
                                                        {
                                                            sm.TotalStop = (int)item.TotalStop;
                                                            sm.CompanyID = objcmnParam.loggedCompany;
                                                            sm.UpdateBy = objcmnParam.loggeduser;
                                                            sm.UpdateOn = DateTime.Now;
                                                            sm.UpdatePc = HostService.GetIP();

                                                            UMasterItemMachineStop.Add(sm);
                                                        };
                                                    }
                                                    LastRowNum = LastRowNum + 1;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (StopRowNum > 0)
                                        {
                                            StopCount = Convert.ToInt32(MachineStopDetail.Where(j => j.SNo == item.SlNo).Count());
                                            if (StopCount > 0)
                                            {
                                                StopMasterId = Convert.ToInt16(GenericFactory_EF_PrdBallMRRMachineStopM.getMaxID("PrdBallMRRMachineStopM"));
                                                StopDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRMachineStop.getMaxID("PrdBallMRRMachineStop"));
                                                StopFirstDigit = Convert.ToInt64(StopDetailId.ToString().Substring(0, 1));
                                                StopOtherDigits = Convert.ToInt64(StopDetailId.ToString().Substring(1, StopDetailId.ToString().Length - 1));
                                                LastRowNum = 1;
                                                foreach (vmBallMachineStopAndBrekage S in MachineStopDetail.Where(x => x.SNo == item.SlNo))
                                                {
                                                    var DetailStop = new PrdBallMRRMachineStop
                                                    {
                                                        BalMachineStopID = Convert.ToInt32(StopFirstDigit + "" + StopOtherDigits),
                                                        BalMachineStopMasterID = StopMasterId,
                                                        Description = S.Description,
                                                        MachineID = S.MachineID,
                                                        ShiftID = (int)S.ShiftID,
                                                        StartTime = TimeSpan.Parse(S.StartTime == null ? "0:00" : S.StartTime),
                                                        StopTime = TimeSpan.Parse(S.StopTime == null ? "0:00" : S.StopTime),
                                                        StopDate = (DateTime)itemMaster.BalMRRDate,
                                                        StopID = (int)S.BWSID,
                                                        StopInMin = S.StopInMin,

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
                                                        var MasterStop = new PrdBallMRRMachineStopM
                                                        {
                                                            BalMachineStopMasterID = (long)StopMasterId,
                                                            TotalStop = (int)item.TotalStop,
                                                            CompanyID = objcmnParam.loggedCompany,
                                                            CreateBy = objcmnParam.loggeduser,
                                                            CreateOn = DateTime.Now,
                                                            CreatePc = HostService.GetIP(),
                                                            IsDeleted = false
                                                        };
                                                        MasterItemMachineStop.Add(MasterStop);
                                                        GenericFactory_EF_PrdBallMRRMachineStopM.updateMaxID("PrdBallMRRMachineStopM", Convert.ToInt64(StopMasterId));
                                                        GenericFactory_EF_PrdBallMRRMachineStop.updateMaxID("PrdBallMRRMachineStop", Convert.ToInt64(StopFirstDigit + "" + (StopOtherDigits - 1)));
                                                    }
                                                    LastRowNum = LastRowNum + 1;
                                                }
                                            }
                                        }
                                    }

                                    if (item.BallBreackageMasterID != null)
                                    {
                                        BreakMasterId = item.BallBreackageMasterID;
                                        if (BreakageRowNum > 0)
                                        {
                                            BreakCount = Convert.ToInt32(BreakageTypeDetail.Where(b => b.SlNo == item.SlNo).Count());
                                            if (BreakCount > 0)
                                            {
                                                LastRowNum = 1;
                                                foreach (vmBallMachineStopAndBrekage m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                                {
                                                    foreach (PrdBallMRRBreakage bd in BreakageDetailAll.Where(x => x.BallBreackageID == m.BallBreakageID && m.ModelState == "Update"))
                                                    {
                                                        bd.BreakageDate = (DateTime)itemMaster.BalMRRDate;
                                                        bd.NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage;
                                                        bd.BreakageID = (int)m.BWSID;
                                                        bd.Description = m.Description;

                                                        bd.CompanyID = objcmnParam.loggedCompany;
                                                        bd.UpdateBy = objcmnParam.loggeduser;
                                                        bd.UpdateOn = DateTime.Now;
                                                        bd.UpdatePc = HostService.GetIP();

                                                        UDetailItemBreakageType.Add(bd);
                                                    };

                                                    if (m.ModelState == "Save")
                                                    {
                                                        if (BreakOtherDigits == 0)
                                                        {
                                                            BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRBreakage.getMaxID("PrdBallMRRBreakage"));
                                                            BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                                            BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                                        }

                                                        var SDetailBreak = new PrdBallMRRBreakage
                                                        {
                                                            BallBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                            BallBreackageMasterID = (long)item.BallBreackageMasterID,
                                                            BreakageDate = (DateTime)itemMaster.BalMRRDate,
                                                            NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                            BreakageID = (int)m.BWSID,
                                                            Description = m.Description,

                                                            CompanyID = objcmnParam.loggedCompany,
                                                            CreateBy = objcmnParam.loggeduser,
                                                            CreateOn = DateTime.Now,
                                                            CreatePc = HostService.GetIP(),
                                                            IsDeleted = false
                                                        };
                                                        UDetailItemBreakageType.Add(SDetailBreak);
                                                        GenericFactory_EF_PrdBallMRRBreakage.updateMaxID("PrdBallMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits)));
                                                    }

                                                    if (BreakCount == LastRowNum)
                                                    {
                                                        foreach (PrdBallMRRBreakageM bm in BreakageMasterAll.Where(x => x.BallBreackageMasterID == item.BallBreackageMasterID))
                                                        {
                                                            bm.TotalBreakage = (int)item.TotalBreakage;
                                                            bm.CompanyID = objcmnParam.loggedCompany;
                                                            bm.UpdateBy = objcmnParam.loggeduser;
                                                            bm.UpdateOn = DateTime.Now;
                                                            bm.UpdatePc = HostService.GetIP();

                                                            UMasterItemBreakageType.Add(bm);
                                                        };
                                                    }
                                                    LastRowNum = LastRowNum + 1;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (BreakageRowNum > 0)
                                        {
                                            BreakCount = Convert.ToInt32(BreakageTypeDetail.Where(b => b.SlNo == item.SlNo).Count());
                                            if (BreakCount > 0)
                                            {
                                                BreakMasterId = Convert.ToInt16(GenericFactory_EF_PrdBallMRRBreakageM.getMaxID("PrdBallMRRBreakageM"));
                                                BreakDetailId = Convert.ToInt64(GenericFactory_EF_PrdBallMRRBreakage.getMaxID("PrdBallMRRBreakage"));
                                                BreakFirstDigit = Convert.ToInt64(BreakDetailId.ToString().Substring(0, 1));
                                                BreakOtherDigits = Convert.ToInt64(BreakDetailId.ToString().Substring(1, BreakDetailId.ToString().Length - 1));
                                                LastRowNum = 1;
                                                foreach (vmBallMachineStopAndBrekage m in BreakageTypeDetail.Where(x => x.SlNo == item.SlNo))
                                                {
                                                    var DetailBreak = new PrdBallMRRBreakage
                                                    {
                                                        BallBreackageID = Convert.ToInt64(BreakFirstDigit + "" + BreakOtherDigits),
                                                        BallBreackageMasterID = BreakMasterId,
                                                        BreakageDate = (DateTime)itemMaster.BalMRRDate,
                                                        NoOfBreakage = m.NoOfBreakage == null ? 0 : (int)m.NoOfBreakage,
                                                        BreakageID = (int)m.BWSID,
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
                                                        var MasterBreak = new PrdBallMRRBreakageM
                                                        {
                                                            BallBreackageMasterID = (long)BreakMasterId,
                                                            TotalBreakage = (int)item.TotalBreakage,

                                                            CompanyID = objcmnParam.loggedCompany,
                                                            CreateBy = objcmnParam.loggeduser,
                                                            CreateOn = DateTime.Now,
                                                            CreatePc = HostService.GetIP(),
                                                            IsDeleted = false
                                                        };
                                                        MasterItemBreakageType.Add(MasterBreak);
                                                        GenericFactory_EF_PrdBallMRRBreakageM.updateMaxID("PrdBallMRRBreakageM", Convert.ToInt64(BreakMasterId));
                                                        GenericFactory_EF_PrdBallMRRBreakage.updateMaxID("PrdBallMRRBreakage", Convert.ToInt64(BreakFirstDigit + "" + (BreakOtherDigits - 1)));
                                                    }
                                                    LastRowNum = LastRowNum + 1;
                                                }
                                            }
                                        }
                                    }

                                    if (ConsumptionRowNum > 0)
                                    {
                                        //string lIp = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                                        ConsumptionCount = Convert.ToInt32(ConsumptionInfo.Where(b => b.BallConsumptionID == item.BallConsumptionID || b.SlNo == item.SlNo).Count());
                                        if (ConsumptionCount > 0)
                                        {
                                            foreach (vmBallConsumption m in ConsumptionInfo.Where(x => x.BallConsumptionID == item.BallConsumptionID || x.SlNo == item.SlNo))
                                            {
                                                foreach (PrdBallMRRConsumption MC in ConsumptionAll.Where(x => x.BallConsumptionID == m.BallConsumptionID && (m.ModelState == "Update")))
                                                {
                                                    ConsumptionID = m.BallConsumptionID;
                                                    MC.YarnCountID = (int)m.YarnCountID;
                                                    MC.SupplierID = m.SupplierID;
                                                    MC.DepartmentID = objcmnParam.DepartmentID;
                                                    MC.LotID = m.LotID;
                                                    MC.LengthM = m.LengthM;
                                                    MC.LengthYds = m.LengthYds;
                                                    MC.Qty = m.Qty;
                                                    MC.Unit = m.Unit;
                                                    MC.UnitPrice = m.UnitPrice;
                                                    MC.Amount = m.Amount;
                                                    MC.Remarks = m.Remarks;
                                                    MC.TransactionTypeID = objcmnParam.tTypeId;
                                                    MC.IssueDate = itemMaster.BalMRRDate;

                                                    MC.CompanyID = objcmnParam.loggedCompany;
                                                    MC.UpdateBy = objcmnParam.loggeduser;
                                                    MC.UpdateOn = DateTime.Now;
                                                    MC.UpdatePc = HostService.GetIP();
                                                    MC.IsDeleted = false;

                                                    UListPrdBallMRRConsumption.Add(MC);
                                                }

                                                if (m.ModelState == "Save")
                                                {
                                                    ConsumptionID = Convert.ToInt16(GenericFactory_EF_PrdBallMRRConsumption.getMaxID("PrdBallMRRConsumption"));
                                                    var ConsumptionItem = new PrdBallMRRConsumption
                                                    {
                                                        BallConsumptionID = (long)ConsumptionID,
                                                        YarnCountID = (int)m.YarnCountID,
                                                        SupplierID = m.SupplierID,
                                                        DepartmentID = objcmnParam.DepartmentID,
                                                        LotID = m.LotID,
                                                        LengthM = m.LengthM,
                                                        LengthYds = m.LengthYds,
                                                        Qty = m.Qty,
                                                        Unit = m.Unit,
                                                        Remarks = m.Remarks,

                                                        CompanyID = objcmnParam.loggedCompany,
                                                        CreateBy = objcmnParam.loggeduser,
                                                        CreateOn = DateTime.Now,
                                                        CreatePc = HostService.GetIP(),
                                                        IsDeleted = false
                                                    };
                                                    ListPrdBallMRRConsumption.Add(ConsumptionItem);
                                                    GenericFactory_EF_PrdBallMRRConsumption.updateMaxID("PrdBallMRRConsumption", Convert.ToInt64(ConsumptionID));
                                                }
                                            }
                                        }
                                    }

                                    foreach (PrdBallMRRDetail d in DetailAll.Where(d => d.BalMRRID == item.BalMRRID && d.BalMRRDetailID == item.BalMRRDetailID && item.ModelState == "Update"))
                                    {
                                        d.BallConsumptionID = ConsumptionID == 0 ? null : ConsumptionID;
                                        d.BallBreackageMasterID = BreakMasterId == 0 ? null : BreakMasterId;
                                        d.BalMachineStopID = StopMasterId == 0 ? null : StopMasterId;
                                        d.TotalStop = item.TotalStop;
                                        d.MachineID = item.MachineID;
                                        d.StartTime = TimeSpan.Parse(item.StartTime == null ? "0:00" : item.StartTime);
                                        d.EndTime = TimeSpan.Parse(item.StopTime == null ? "0:00" : item.StopTime);
                                        d.MachineSpeed = item.MachineSpeed;
                                        d.OperatorID = item.OperatorID;
                                        d.OutputUnitID = item.OutputUnitID;
                                        d.Remarks = item.Remarks;
                                        d.ShiftEngineerID = item.ShiftEngineerID;
                                        d.ShiftID = item.ShiftID;
                                        d.SetLength = item.LengthPerBall;
                                        d.TotalBreakage = item.TotalBreakage;
                                        d.TotalStop = item.TotalStop;
                                        d.WarpingDate = DateTime.Parse(item.StrWarpingDate);

                                        d.CompanyID = objcmnParam.loggedCompany;
                                        d.UpdateBy = objcmnParam.loggeduser;
                                        d.UpdateOn = DateTime.Now;
                                        d.UpdatePc = HostService.GetIP();

                                        UDetailItemMain.Add(d);
                                    }
                                }
                            }
                            //***********************************Start Get Data From Related Table to Update*********************************
                        }

                        if (itemMaster.BalMRRID > 0)
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdBallMRRMaster.Update(MasterItem);
                                GenericFactory_EF_PrdBallMRRMaster.Save();
                            }
                            if (UMasterItemMachineStop != null && UMasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStopM.UpdateList(UMasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStopM.Save();
                            }
                            if (MasterItemMachineStop != null && MasterItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStopM.Save();
                            }
                            if (UDetailItemMachineStop != null && UDetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStop.UpdateList(UDetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStop.Save();
                            }
                            if (DetailItemMachineStop != null && DetailItemMachineStop.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStop.Save();
                            }
                            if (UMasterItemBreakageType != null && UMasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRBreakageM.UpdateList(UMasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakageM.Save();
                            }
                            if (MasterItemBreakageType != null && MasterItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakageM.Save();
                            }
                            if (UDetailItemBreakageType != null && UDetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRBreakage.UpdateList(UDetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakage.Save();
                            }
                            if (DetailItemBreakageType != null && DetailItemBreakageType.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakage.Save();
                            }
                            if (UListPrdBallMRRConsumption != null && UListPrdBallMRRConsumption.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRConsumption.UpdateList(UListPrdBallMRRConsumption.ToList());
                                GenericFactory_EF_PrdBallMRRConsumption.Save();
                            }
                            if (ListPrdBallMRRConsumption != null && ListPrdBallMRRConsumption.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRConsumption.InsertList(ListPrdBallMRRConsumption.ToList());
                                GenericFactory_EF_PrdBallMRRConsumption.Save();
                            }
                            if (UDetailItemMain != null && UDetailItemMain.Count != 0)
                            {
                                GenericFactory_EF_PrdBallMRRDetail.UpdateList(UDetailItemMain.ToList());
                                GenericFactory_EF_PrdBallMRRDetail.Save();
                            }
                        }
                        else
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_PrdBallMRRMaster.Insert(MasterItem);
                                GenericFactory_EF_PrdBallMRRMaster.Save();
                                GenericFactory_EF_PrdBallMRRMaster.updateMaxID("PrdBallMRRMaster", Convert.ToInt64(MainMasterId));
                                GenericFactory_EF_PrdBallMRRMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (MasterItemMachineStop != null)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStopM.InsertList(MasterItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStopM.Save();
                            }
                            if (DetailItemMachineStop != null)
                            {
                                GenericFactory_EF_PrdBallMRRMachineStop.InsertList(DetailItemMachineStop.ToList());
                                GenericFactory_EF_PrdBallMRRMachineStop.Save();
                            }
                            if (MasterItemBreakageType != null)
                            {
                                GenericFactory_EF_PrdBallMRRBreakageM.InsertList(MasterItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakageM.Save();
                            }
                            if (DetailItemBreakageType != null)
                            {
                                GenericFactory_EF_PrdBallMRRBreakage.InsertList(DetailItemBreakageType.ToList());
                                GenericFactory_EF_PrdBallMRRBreakage.Save();
                            }
                            if (ListPrdBallMRRConsumption != null)
                            {
                                GenericFactory_EF_PrdBallMRRConsumption.InsertList(ListPrdBallMRRConsumption.ToList());
                                GenericFactory_EF_PrdBallMRRConsumption.Save();
                            }
                            if (DetailItemMain != null)
                            {
                                GenericFactory_EF_PrdBallMRRDetail.InsertList(DetailItemMain.ToList());
                                GenericFactory_EF_PrdBallMRRDetail.Save();
                                GenericFactory_EF_PrdBallMRRDetail.updateMaxID("PrdBallMRRDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
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
                }
                else
                {
                    result = "";
                }
            }
            return result;
            //**************************************************End Main Operation************************************************            
        }

        public string DeleteUpdateBallMrrMasterDetail(vmCmnParameters objcmnParam)
        {
            GenericFactory_BallWarpingInformation_GF = new vmBallWarpingInformation_GF();
            string result = string.Empty;
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
                    ht.Add("BalMRRID", objcmnParam.id);

                    spQuery = "[Delete_PrdBallWarpingMasterDetailByID]";
                    result = GenericFactory_BallWarpingInformation_GF.ExecuteCommandString(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }

    }
}
