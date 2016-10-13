using ABS.Service.Production.Interfaces;
using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;
using System.Collections;
using ABS.Data.BaseInterfaces;
using ABS.Data.BaseFactories;
using ABS.Models.ViewModel.Production;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System.Web;
using ABS.Models.ViewModel.SystemCommon;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class DyingChemicalConsumptionMgt : iDyingChemicalConsumptionMgt
    {
        bool disposed = false;
        private ERP_Entities _ctxObj = null;
        private iGenericFactory_EF<PrdDyingMRRSet> GenericFactoryEF_PrdDyingMRRSet = null;
        private iGenericFactory_EF<PrdDyingMRRSetDetail> GenericFactoryEF_PrdDyingMRRSetDetail = null;
        private iGenericFactory_EF<PrdDyingMRRMaster> PrdDyingMRRMaster_IEF = null;
        private iGenericFactory_EF<PrdDyingMRRDetail> PrdDyingMRRDetail_IEF = null;
        private iGenericFactory<dynamic> objectMachineSetupDetails = null;

        public dynamic GetMachineSetupDetails(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MasterID, int? DetailsID)
        {
            dynamic result = null;
            try
            {
                string spQuery = string.Empty;
                using (_ctxObj = new ERP_Entities())
                {
                    var itemID = MasterID;
                    var machineID = DetailsID;
                    PrdDyingMachineSetup ent = (from machineSetup in _ctxObj.PrdDyingMachineSetups
                                                where machineSetup.ItemID == itemID
                                                 && machineSetup.MechineID == ((machineID == 0) ? machineSetup.MechineID : (long)machineID)
                                                select machineSetup).FirstOrDefault();

                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", companyID);
                    ht.Add("LoggedUser", loggedUser);
                    ht.Add("MASTERID", MasterID);
                    ht.Add("DETAILSID", ent.SetupID.ToString());
                    spQuery = "[GET_DYINGMACHINESETUP]";
                    using (objectMachineSetupDetails = new dynamic_EF())
                    {
                        result = objectMachineSetupDetails.ExecuteQueryObjectType(spQuery, ht);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                ex.ToString();
            }
            return 0;

        }
        public List<vmPrdDyingOperationSetup> GetOperationSetup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? filterID)
        {
            List<vmPrdDyingOperationSetup> returnList = new List<vmPrdDyingOperationSetup>();
            try
            {
                string spQuery = string.Empty;
                using (_ctxObj = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", companyID);
                    ht.Add("LoggedUser", loggedUser);
                    ht.Add("ItemID", ItemID);
                    spQuery = "[GET_DYINGOperationSETUP]";
                    using (vmPrdDyingOperationSetupGenericFactory obj = new vmPrdDyingOperationSetupGenericFactory())
                    {
                        returnList = obj.ExecuteQuery(spQuery, ht).ToList();
                    }
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return returnList;
        }


        #region Save 
        public PrdDyingMRRSet GenerateSetup(List<vmProductionPrdSetSetupDDL> masterList, UserCommonEntity commonEntity)
        {
            PrdDyingMRRSet mRRSet = new PrdDyingMRRSet();
            try
            {
                bool isDeleted = false;
                DateTime serverDate = DateTime.Now;
                List<vmPrdSetSetup> listSetup = new List<vmPrdSetSetup>();
                using (ERP_Entities context = new ERP_Entities())
                {
                    var dateQuery = context.Database.SqlQuery<DateTime>("SELECT getdate()");
                    serverDate = dateQuery.AsEnumerable().First();
                    foreach (vmProductionPrdSetSetupDDL item in masterList)
                    {
                        vmPrdSetSetup setupEntity = (from setup in context.PrdSetSetups
                                                     join pi in context.SalPIMasters on setup.PIID equals pi.PIID into leftPIgroup
                                                     from lfg in leftPIgroup.DefaultIfEmpty()
                                                     where setup.SetID == item.id
                                                     select new vmPrdSetSetup
                                                     {

                                                         SetID = setup.SetID,
                                                         SetNo = setup.SetNo,
                                                         SetMasterID = setup.SetMasterID,
                                                         ItemID = setup.ItemID,
                                                         YarnID = setup.YarnID,
                                                         YarnCount = setup.YarnCount,
                                                         YarnRatioLot = setup.YarnRatioLot,
                                                         YarnRatio = setup.YarnRatio,
                                                         SetTrackingNo = setup.SetTrackingNo,
                                                         NoOfBall = setup.NoOfBall,
                                                         SetLength = setup.SetLength,
                                                         MachineSpeed = setup.MachineSpeed,
                                                         TotalEnds = setup.TotalEnds,
                                                         Weave = setup.Weave,
                                                         EndsPerRope = setup.EndsPerRope,
                                                         EndsPerCreel = setup.EndsPerCreel,
                                                         LeaseReapet = setup.LeaseReapet,
                                                         PIID = setup.PIID,
                                                         PINo = lfg.PINO,
                                                         SupplierID = setup.SupplierID,
                                                         BuyerID = setup.BuyerID,
                                                         ColorID = setup.ColorID,
                                                         WeftYarnID = setup.WeftYarnID,
                                                         WarpYarnID = setup.WarpYarnID,
                                                         Description = setup.Description,
                                                         SetDate = setup.SetDate,
                                                         CompanyID = setup.CompanyID,
                                                         CreateBy = setup.CreateBy,
                                                         CreateOn = setup.CreateOn,
                                                         CreatePc = setup.CreatePc,
                                                         UpdateBy = setup.UpdateBy,
                                                         UpdateOn = setup.UpdateOn,
                                                         UpdatePc = setup.UpdatePc,
                                                         IsDeleted = setup.IsDeleted,
                                                         DeleteBy = setup.DeleteBy,
                                                         DeleteOn = setup.DeleteOn,
                                                         DeletePc = setup.DeletePc
                                                     }).FirstOrDefault();

                        listSetup.Add(setupEntity);
                    }
                }
                GenericFactoryEF_PrdDyingMRRSet = new PrdDyingMRRSet_EF();
                mRRSet.DyingSetID = Convert.ToInt16(GenericFactoryEF_PrdDyingMRRSet.getMaxID("PrdDyingMRRSet"));
                GenericFactoryEF_PrdDyingMRRSet.updateMaxID("PrdDyingMRRSet", Convert.ToInt64(mRRSet.DyingSetID));
                mRRSet.DyingPINo = null;
                bool firstRow = true;
                foreach (vmPrdSetSetup item in listSetup)
                {
                    mRRSet.ItemID = item.ItemID;
                    if (string.IsNullOrEmpty(mRRSet.DyingPINo))
                    {
                        mRRSet.DyingPINo = "0";
                    }

                    if (firstRow)
                    {
                        mRRSet.DyingSetNo = item.SetNo;
                        mRRSet.TotalSetLength = item.SetLength ?? 0;
                    }
                    else
                    {
                        mRRSet.DyingSetNo = mRRSet.DyingSetNo + "," + item.SetNo;
                        mRRSet.TotalSetLength = mRRSet.TotalSetLength + item.SetLength ?? 0;
                    }
                    mRRSet.DyingPINo = mRRSet.DyingPINo + mRRSet.DyingPINo ?? "0";
                    mRRSet.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    mRRSet.IsDeleted = isDeleted;
                    mRRSet.CreateBy = commonEntity.loggedUserID;
                    mRRSet.CreatePc = HostService.GetIP();
                    mRRSet.CreateOn = serverDate;
                    PrdDyingMRRSetDetail mrrSetDetails = new PrdDyingMRRSetDetail();
                    GenericFactoryEF_PrdDyingMRRSetDetail = new PrdDyingMRRSetDetail_EF();
                    mrrSetDetails.DyingSetIDDetailID = Convert.ToInt16(GenericFactoryEF_PrdDyingMRRSetDetail.getMaxID("PrdDyingMRRSetDetail"));
                    GenericFactoryEF_PrdDyingMRRSetDetail.updateMaxID("PrdDyingMRRSetDetail", Convert.ToInt64(mrrSetDetails.DyingSetIDDetailID));
                    mrrSetDetails.DyingSetID = mRRSet.DyingSetID;
                    mrrSetDetails.PIID = item.PIID;
                    mrrSetDetails.SetLength = item.SetLength ?? 0;
                    mrrSetDetails.SetID = item.SetID;
                    mrrSetDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    mrrSetDetails.IsDeleted = isDeleted;
                    mrrSetDetails.CreateBy = mRRSet.CreateBy;
                    mrrSetDetails.CreatePc = mRRSet.CreatePc;
                    mrrSetDetails.CreateOn = mRRSet.CreateOn;
                    mRRSet.PrdDyingMRRSetDetails.Add(mrrSetDetails);
                    firstRow = false;
                }
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }
            return mRRSet;
        }
        public int SaveMRRSet(List<vmProductionPrdSetSetupDDL> masterList, UserCommonEntity commonEntity)
        {
            int result = (int)ResponseMessage.Error;
            try
            {
                PrdDyingMRRSet entity = GenerateSetup(masterList, commonEntity);
                GenericFactoryEF_PrdDyingMRRSet.Insert(entity);
                GenericFactoryEF_PrdDyingMRRSet.Save();
                result = (int)entity.DyingSetID;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                result = (int)ResponseMessage.Exception;
            }
            return result;
        }

        public PrdDyingMRRMaster GenerateMrrSave(vmPrdDyingMRRMaster master, List<vmPrdDyingMRRDetail> details, UserCommonEntity commonEntity)
        {
            PrdDyingMRRMaster MRR = new PrdDyingMRRMaster();
            DateTime serverDate = DateTime.Now;
            String HostID = HostService.GetIP();

            ERP_Entities context = new ERP_Entities();
            serverDate = context.Database.SqlQuery<DateTime>("SELECT getdate()").AsEnumerable().First();
            Int64 MasterID = 0; long detialsID = 0;
            MasterID = context.Database.SqlQuery<Int64>(" Select ISNULL(max(DyingMRRID), 0) +1 from PrdDyingMRRMaster").AsEnumerable().First();
            detialsID = context.Database.SqlQuery<Int64>(" Select ISNULL(max(DyingMRRDetailID), 0) +1 from PrdDyingMRRDetail").AsEnumerable().First();
            string startTime = "7:00 AM";
            TimeSpan ts;
            TimeSpan.TryParse(startTime, out ts);
            try
            {
                MRR.DyingMRRID = MasterID;
                MRR.DyingMRRNo = MRR.DyingMRRID.ToString();
                MRR.DyingSetNo = master.DyingSetNo;
                MRR.MachineID = master.MachineID;
                MRR.ItemID = master.ItemID;
                MRR.KGPreMin = master.KGPreMin;
                MRR.Moiture = master.Moiture;
                MRR.Speed = master.Speed;
                MRR.TotalLength = master.TotalLength;

                MRR.ShiftID = master.ShiftID;
                MRR.EndTime = master.EndTime == null ? TimeSpan.Parse("0:00") : master.EndTime;
                MRR.StartTime = master.StartTime == null ? TimeSpan.Parse("0:00") : master.StartTime;
                MRR.Description = master.Description;
                MRR.Description = "";
                MRR.BuyerID = master.BuyerID;
                MRR.RefSetID = (int)master.RefSetID;
                MRR.RefSetDate = master.RefSetDate;
                MRR.Date = master.Date;
                MRR.CompanyID = commonEntity.loggedCompnyID ?? 0;
                MRR.CreateBy = commonEntity.loggedUserID;
                MRR.CreateOn = master.CreateOn;
                MRR.CreatePc = HostID;
                //MRR.UpdateBy = master.UpdateBy;
                //MRR.UpdateOn = master.UpdateOn;
                //MRR.UpdatePc = master.UpdatePc;
                MRR.IsDeleted = master.IsDeleted;
                //MRR.DeleteBy = master.DeleteBy;
                //MRR.DeleteOn = master.DeleteOn;
                //MRR.DeletePc = master.DeletePc;
                foreach (vmPrdDyingMRRDetail item in details)
                {
                    PrdDyingMRRDetail detail = new PrdDyingMRRDetail();
                    detail.DyingMRRDetailID = ++detialsID;
                    detail.DyingMRRID = MRR.DyingMRRID;
                    detail.DyingProcessID = item.DyingProcessID;
                    detail.Time = item.Time;
                    detail.OperationID = item.OperationID;
                    detail.Quantity = item.Quantity;
                    detail.UnitID = item.UnitID;
                    detail.CompanyID = commonEntity.loggedCompnyID ?? 0; ;
                    detail.CreateBy = commonEntity.loggedUserID;
                    detail.CreateOn = serverDate;
                    detail.CreatePc = HostID;
                    //detail.UpdateBy = item.UpdateBy;
                    //detail.UpdateOn = item.UpdateOn;
                    //detail.UpdatePc = item.UpdatePc;
                    detail.IsDeleted = item.IsDeleted;
                    //detail.DeleteBy = item.DeleteBy;
                    //detail.DeleteOn = item.DeleteOn;
                    //detail.DeletePc = item.DeletePc;
                    MRR.PrdDyingMRRDetails.Add(detail);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return MRR;
        }

        public int SaveMRR(vmPrdDyingMRRMaster master, List<vmPrdDyingMRRDetail> details, UserCommonEntity commonEntity)
        {
            int result = (int)ResponseMessage.Error;
            try
            {
                PrdDyingMRRMaster entity = new PrdDyingMRRMaster();
                PrdDyingMRRMaster_IEF = new PrdDyingMRRMaster_EF();
                if(master.EntityState=="Insert")
                {
                    entity = GenerateMrrSave(master, details, commonEntity);
                    PrdDyingMRRMaster_IEF.Insert(entity);
                    PrdDyingMRRMaster_IEF.Save();
                }
                else
                {
                    entity = GenerateMrrUpdate(master, details, commonEntity);
                    PrdDyingMRRMaster_IEF.Update(entity);

                    if(entity.PrdDyingMRRDetails.Count() > 0){
                        PrdDyingMRRDetail_IEF = new PrdDyingMRRDetail_EF();
                        PrdDyingMRRDetail_IEF.UpdateList(entity.PrdDyingMRRDetails);
                        PrdDyingMRRDetail_IEF.Save();
                    }
                    PrdDyingMRRMaster_IEF.Save();
                }
                result = (int)entity.DyingMRRID;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                result = (int)ResponseMessage.Exception;
            }
            return result;
        }



        public PrdDyingMRRMaster GenerateMrrUpdate(vmPrdDyingMRRMaster master, List<vmPrdDyingMRRDetail> details, UserCommonEntity commonEntity)
        {
            PrdDyingMRRMaster MRR = new PrdDyingMRRMaster();
            DateTime serverDate = DateTime.Now;
            String HostID = HostService.GetIP();
            
            ERP_Entities context = new ERP_Entities();
            PrdDyingMRRMaster masterDatafromdb = context.PrdDyingMRRMasters.Where(X => X.DyingMRRID == master.DyingMRRID).FirstOrDefault();
            serverDate = context.Database.SqlQuery<DateTime>("SELECT getdate()").AsEnumerable().First();
            Int64 MasterID = 0; long detialsID = 0;
            MasterID = context.Database.SqlQuery<Int64>(" Select ISNULL(max(DyingMRRID), 0) +1 from PrdDyingMRRMaster").AsEnumerable().First();
            detialsID = context.Database.SqlQuery<Int64>(" Select ISNULL(max(DyingMRRDetailID), 0) +1 from PrdDyingMRRDetail").AsEnumerable().First();

            PrdDyingMRRDetail_IEF = new PrdDyingMRRDetail_EF();
            List<PrdDyingMRRDetail> insertionList = new List<PrdDyingMRRDetail>();
            try
            {
                MRR.DyingMRRID = master.DyingMRRID;
                MRR.DyingMRRNo = MRR.DyingMRRID.ToString();
                MRR.DyingSetNo = master.DyingSetNo;
                MRR.MachineID = master.MachineID;
                MRR.ItemID = master.ItemID;
                MRR.KGPreMin = master.KGPreMin;
                MRR.Moiture = master.Moiture;
                MRR.Speed = master.Speed;
                MRR.TotalLength = master.TotalLength;

                MRR.ShiftID = master.ShiftID;
                MRR.EndTime = master.EndTime == null ? TimeSpan.Parse("0:00") : master.EndTime;
                MRR.StartTime = master.StartTime == null ? TimeSpan.Parse("0:00") : master.StartTime;
                MRR.Description = master.Description;
                MRR.Description = "";
                MRR.BuyerID = master.BuyerID;
                MRR.RefSetID = (int)master.RefSetID;
                MRR.RefSetDate = master.RefSetDate;
                MRR.Date = master.Date;
                MRR.CompanyID = commonEntity.loggedCompnyID ?? 0;
                if (master.EntityState == "Insert")
                {
                    MRR.CreateBy = commonEntity.loggedUserID;
                    MRR.CreateOn = serverDate;
                    MRR.CreatePc = HostID;
                }
                if(master.EntityState=="Update")
                {
                    MRR.CreateBy = masterDatafromdb.CreateBy;
                    MRR.CreateOn = masterDatafromdb.CreateOn;
                    MRR.CreatePc = masterDatafromdb.CreatePc;
                    MRR.UpdateBy = commonEntity.loggedUserID;
                    MRR.UpdateOn = serverDate;
                    MRR.UpdatePc = HostID;
                }

                MRR.IsDeleted = master.IsDeleted;
                if (MRR.IsDeleted)
                {
                    MRR.DeleteBy = commonEntity.loggedUserID; 
                    MRR.DeleteOn = serverDate;
                    MRR.DeletePc = HostID;
                }

                foreach (vmPrdDyingMRRDetail item in details)
                {
                    PrdDyingMRRDetail detail = new PrdDyingMRRDetail();
                    if (item.EntityState == "Insert")
                    {
                        detail.DyingMRRDetailID = ++detialsID;
                    }
                    else
                    {
                        detail.DyingMRRDetailID = item.DyingMRRDetailID;
                    }
                    
                    detail.DyingMRRID = MRR.DyingMRRID;
                    detail.DyingProcessID = item.DyingProcessID;
                    detail.Time = item.Time;
                    detail.OperationID = item.OperationID;
                    detail.Quantity = item.Quantity;
                    detail.UnitID = item.UnitID;
                    detail.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    if (item.EntityState == "Insert")
                    {
                        detail.CreateBy = commonEntity.loggedUserID;
                        detail.CreateOn = serverDate;
                        detail.CreatePc = HostID;
                    }
                    if (item.EntityState == "Update")
                    {
                        detail.CreateBy = item.CreateBy;
                        detail.CreateOn = item.CreateOn;
                        detail.CreatePc = item.CreatePc;
                        detail.UpdateBy = commonEntity.loggedUserID;
                        detail.UpdateOn = serverDate;
                        detail.UpdatePc = HostID;
                        
                    }

                    detail.IsDeleted = item.IsDeleted;
                    if(detail.IsDeleted)
                    {
                        detail.DeleteBy = commonEntity.loggedUserID;
                        detail.DeleteOn = serverDate;
                        detail.DeletePc = HostID;
                    }
                    if(item.EntityState == "Update")
                    {
                        MRR.PrdDyingMRRDetails.Add(detail);
                    }
                    else
                    {
                        insertionList.Add(detail);
                    }
                      
                }
                if(insertionList.Count>0)
                {
                    PrdDyingMRRDetail_IEF.InsertList(insertionList);
                    PrdDyingMRRDetail_IEF.Save();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return MRR;
        }
        #endregion  Save

        #region Delete
        public string DeleteChemicalProcessMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                PrdDyingMRRMaster_IEF = new PrdDyingMRRMaster_EF();
                PrdDyingMRRDetail_IEF = new PrdDyingMRRDetail_EF();

                var MasterItem = new PrdDyingMRRMaster();
                var DetailItem = new List<PrdDyingMRRDetail>();

                //For Update Master Detail
                var MasterAll = PrdDyingMRRMaster_IEF.GetAll().Where(x => x.DyingMRRID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = PrdDyingMRRDetail_IEF.GetAll().Where(x => x.DyingMRRID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.DyingMRRID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdDyingMRRDetail d in DetailAll.Where(d => d.DyingMRRID == objcmnParam.id))
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
                        PrdDyingMRRMaster_IEF.Update(MasterItem);
                        PrdDyingMRRMaster_IEF.Save();
                    }
                    if (DetailItem != null)
                    {
                        PrdDyingMRRDetail_IEF.UpdateList(DetailItem.ToList());
                        PrdDyingMRRDetail_IEF.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.DyingMRRNo;
                }
                catch (Exception e)
                {
                    result = "";
                    e.ToString();
                }
            }
            return result;
        }
        #endregion Delete

        #region Get

        public List<vmPrdDyingMRRMaster> GetAllProcess(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmPrdDyingMRRMaster> returnList = new List<vmPrdDyingMRRMaster>();
            recordsTotal = 0;
            try
            {
                using (ERP_Entities context = new ERP_Entities())
                {
                    returnList = (from process in context.PrdDyingMRRMasters
                                  join item in context.CmnItemMasters on process.ItemID equals item.ItemID into itemGroup
                                  from ig in itemGroup.DefaultIfEmpty()

                                  join mc in context.PrdWeavingMachinConfigs on process.MachineID equals mc.MachineConfigID into mcGroup
                                  from mg in mcGroup.DefaultIfEmpty()
                                  join shift in context.HRMShifts on process.ShiftID equals shift.ShiftID into shiftGroup
                                  from sg in shiftGroup.DefaultIfEmpty()
                                  where process.CompanyID == objcmnParam.loggedCompany && process.IsDeleted == false


                                  select new vmPrdDyingMRRMaster
                                  {
                                      DyingMRRID = process.DyingMRRID,
                                      DyingMRRNo = process.DyingMRRNo,
                                      DyingSetNo = process.DyingSetNo,
                                      MachineID = process.MachineID,
                                      MachineConfigNo = mg.MachineConfigNo ?? "",
                                      ItemID = process.ItemID??0,
                                      ArticleNo = ig.ArticleNo ?? "",
                                      Date = process.Date,
                                      KGPreMin = process.KGPreMin,
                                      Moiture = process.Moiture,
                                      Speed = process.Speed,
                                      TotalLength = process.TotalLength,
                                      ShiftID = process.ShiftID,
                                      ShiftName = sg.ShiftName ?? "",
                                      EndTime = process.EndTime,
                                      StartTime = process.StartTime,
                                      Description = process.Description,
                                      BuyerID = process.BuyerID,
                                      RefSetID = process.RefSetID,
                                      RefSetDate = process.RefSetDate,
                                      CompanyID = process.CompanyID,
                                      CreateBy = process.CreateBy,
                                      CreateOn = process.CreateOn,
                                      CreatePc = process.CreatePc,
                                      UpdateBy = process.UpdateBy,
                                      UpdateOn = process.UpdateOn,
                                      UpdatePc = process.UpdatePc,
                                      IsDeleted = process.IsDeleted,
                                      DeleteBy = process.DeleteBy,
                                      DeleteOn = process.DeleteOn,
                                      DeletePc = process.DeletePc,
                                      EntityState = "Update",
                                  }).ToList();
                    recordsTotal = returnList.Count();
                    returnList = returnList.OrderByDescending(x => x.DyingMRRID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnList;
        }
        
        public  vmPrdDyingMRRMaster GetProcessByID(int? companyID, int? loggedUser, int? DyingMRRID)
        {
            vmPrdDyingMRRMaster returnList = new vmPrdDyingMRRMaster();
            try
            {
                using (ERP_Entities context = new ERP_Entities())
                {
                    returnList = (from process in context.PrdDyingMRRMasters
                                  join item in context.CmnItemMasters on process.ItemID equals item.ItemID into itemGroup
                                  from ig in itemGroup.DefaultIfEmpty()

                                  join mc in context.PrdWeavingMachinConfigs on process.MachineID equals mc.MachineConfigID into mcGroup
                                  from mg in mcGroup.DefaultIfEmpty()
                                  join shift in context.HRMShifts on process.ShiftID equals shift.ShiftID into shiftGroup
                                  from sg in shiftGroup.DefaultIfEmpty()
                                  where process.CompanyID == companyID && process.IsDeleted == false
                                  && process.DyingMRRID == DyingMRRID

                                  select new vmPrdDyingMRRMaster
                                  {
                                      DyingMRRID = process.DyingMRRID,
                                      DyingMRRNo = process.DyingMRRNo,
                                      DyingSetNo = process.DyingSetNo,
                                      MachineID = process.MachineID,
                                      MachineConfigNo = mg.MachineConfigNo ?? "",
                                      ItemID = process.ItemID??0,
                                      ArticleNo = ig.ArticleNo ?? "",
                                      Date = process.Date,
                                      KGPreMin = process.KGPreMin,
                                      Moiture = process.Moiture,
                                      Speed = process.Speed,
                                      TotalLength = process.TotalLength,
                                      ShiftID = process.ShiftID,
                                      ShiftName = sg.ShiftName ?? "",
                                      EndTime = process.EndTime,
                                      StartTime = process.StartTime,
                                      Description = process.Description,
                                      BuyerID = process.BuyerID,
                                      RefSetID = process.RefSetID,
                                      RefSetDate = process.RefSetDate,
                                      CompanyID = process.CompanyID,
                                      CreateBy = process.CreateBy,
                                      CreateOn = process.CreateOn,
                                      CreatePc = process.CreatePc,
                                      UpdateBy = process.UpdateBy,
                                      UpdateOn = process.UpdateOn,
                                      UpdatePc = process.UpdatePc,
                                      IsDeleted = process.IsDeleted,
                                      DeleteBy = process.DeleteBy,
                                      DeleteOn = process.DeleteOn,
                                      DeletePc = process.DeletePc,
                                      EntityState="Update",
                                  }).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnList;
        }

        public List<vmPrdDyingMRRDetail> GetProcessDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DyingMRRID)
        {
            List<vmPrdDyingMRRDetail> returnList = new List<vmPrdDyingMRRDetail>();
            try
            {
                using (ERP_Entities context = new ERP_Entities())
                {
                    returnList = (from process in context.PrdDyingMRRDetails
                                  where process.CompanyID == companyID && process.IsDeleted == false
                                 && process.DyingMRRID == DyingMRRID
                                  select new vmPrdDyingMRRDetail
                                  {
                                      DyingMRRDetailID = process.DyingMRRDetailID,
                                      DyingMRRID = process.DyingMRRID,
                                      DyingProcessID = process.DyingProcessID,
                                      Time = process.Time,
                                      OperationID = process.OperationID,
                                      Quantity = process.Quantity,
                                      UnitID = process.UnitID,
                                      CompanyID = process.CompanyID,
                                      CreateBy = process.CreateBy,
                                      CreateOn = process.CreateOn,
                                      CreatePc = process.CreatePc,
                                      UpdateBy = process.UpdateBy,
                                      UpdateOn = process.UpdateOn,
                                      UpdatePc = process.UpdatePc,
                                      IsDeleted = process.IsDeleted,
                                      DeleteBy = process.DeleteBy,
                                      DeleteOn = process.DeleteOn,
                                      DeletePc = process.DeletePc,
                                      EntityState = "Update",
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnList;
        }
        #endregion Get 

    }

}

