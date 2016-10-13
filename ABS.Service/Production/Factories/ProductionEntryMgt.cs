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
    public class ProductionEntryMgt //: iBallWarpingMgt
    {
        private ERP_Entities _ctxCmn = null;


        private iGenericFactory<vmPrdFinishingMRRMasterShrinkage> GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF = null;
        private iGenericFactory_EF<PrdFinishingMRRMaster> GenericFactory_PrdFinishingMRRMaster_EF = null;
        private iGenericFactory_EF<PrdFinishingMRRShrinkage> GenericFactory_PrdFinishingMRRShrinkage_EF = null;

        public vmPrdFinishingMRRMasterShrinkage GetWeavingSetInformation(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF = new vmPrdFinishingMRRMasterShrinkage_VM();
            vmPrdFinishingMRRMasterShrinkage objSetInformation = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("WeavingMRRID", objcmnParam.id);
                //ht.Add("FinishingMRRID", objcmnParam.ItemType); //FinishingMRRID is applicable when retrieve sizemrrmaster data

                spQuery = "[Get_WeavingSetInformation]";
                objSetInformation = GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetInformation;
        }

        public IEnumerable<vmPrdFinishingMRRMasterShrinkage> GetFinishingMRRMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF = new vmPrdFinishingMRRMasterShrinkage_VM();
            IEnumerable<vmPrdFinishingMRRMasterShrinkage> FinishingMaster = null;
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

                    spQuery = "[Get_FinishingMRRMaster]";
                    FinishingMaster = GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdFinishingMRRMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//FinishingMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return FinishingMaster;
        }

        public IEnumerable<vmPrdFinishingMRRMasterShrinkage> GetShrinkageByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF = new vmPrdFinishingMRRMasterShrinkage_VM();
            IEnumerable<vmPrdFinishingMRRMasterShrinkage> ListShrinkage = null;
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
                    ht.Add("FinishingMRRID", objcmnParam.id);

                    spQuery = "[Get_ShrinkageByID]";
                    ListShrinkage = GenericFactory_vmPrdFinishingMRRMasterShrinkage_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = ListShrinkage.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListShrinkage;
        }

        public IEnumerable<PrdFinishingType> GetFinishingType(vmCmnParameters objcmnParam)
        {
            IEnumerable<PrdFinishingType> objAllFinishingType = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objAllFinishingType = (from WM in _ctxCmn.PrdWeavingMRRMasters
                                            join FT in _ctxCmn.PrdFinishingTypes on WM.ItemID equals FT.ItemID
                                            join FP in _ctxCmn.PrdFinishingProcesses on FT.FinishingProcessID equals FP.FinishingProcessID
                                           where FT.CompanyID == objcmnParam.loggedCompany && FT.IsDeleted == false
                                           //&& SD.ItemID == objcmnParam.id
                                           && objcmnParam.id == 0 ? true : WM.WeavingMRRID == objcmnParam.id
                                           orderby FP.FinishingProcessName
                                           select new
                                           {
                                               FInishTypeID = FT.FInishTypeID,
                                               FInishTypeName = FP.FinishingProcessName
                                           }).ToList().Select(x => new PrdFinishingType
                                           {
                                               FInishTypeID = x.FInishTypeID,
                                               FInishTypeName = x.FInishTypeName
                                           }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objAllFinishingType;
        }

        public string SaveUpdateFinishing(vmPrdFinishingMRRMasterShrinkage itemMaster, List<vmPrdFinishingMRRMasterShrinkage> Shrinkage, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty, FinishingMRRNo = string.Empty; long MainMasterId = 0, ShrinkageMasterID = 0;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdFinishingMRRMaster_EF = new PrdFinishingMRRMaster_EF();
                GenericFactory_PrdFinishingMRRShrinkage_EF = new PrdFinishingMRRShrinkage_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdFinishingMRRMaster();
                var ShrinkageItem = new PrdFinishingMRRShrinkage();

                //**************************************************Start Main Operation************************************************
                try
                {
                    if (itemMaster.FinishingMRRID == 0)
                    {
                        //***************************************************Start Save Operation************************************************
                        //**********************************************Start Generate Master & Detail ID****************************************
                        MainMasterId = Convert.ToInt16(GenericFactory_PrdFinishingMRRMaster_EF.getMaxID("PrdFinishingMRRMaster"));
                        ShrinkageMasterID = Convert.ToInt16(GenericFactory_PrdFinishingMRRShrinkage_EF.getMaxID("PrdFinishingMRRShrinkage"));
                        //***********************************************End Generate Master & Detail ID*****************************************
                        CustomNo = GenericFactory_PrdFinishingMRRMaster_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        if (CustomNo == null || CustomNo == "")
                        {
                            FinishingMRRNo = MainMasterId.ToString();
                        }
                        else
                        {
                            FinishingMRRNo = CustomNo;
                        }

                        MasterItem = new PrdFinishingMRRMaster
                        {
                            FinishingMRRID = MainMasterId,
                            FinishingMRRNo = FinishingMRRNo,
                            FinishingMRRTypeID = objcmnParam.tTypeId,
                            FinishingMRRDate = (DateTime)itemMaster.FinishingMRRDate,
                            ItemID = itemMaster.ItemID,
                            SetID = itemMaster.SetID,
                            SizeMRRID = itemMaster.SizeMRRID,
                            WeavingMRRID = itemMaster.WeavingMRRID,
                            BuyerID = (int)itemMaster.BuyerID,
                            PIID = itemMaster.PIID,
                            FinishingTypeID = (int)itemMaster.FinishingTypeID,
                            MachineID = itemMaster.MachineID,
                            ShiftID = itemMaster.ShiftID,
                            StartTime = TimeSpan.Parse(itemMaster.StartTime == null ? "0:00" : itemMaster.StartTime),
                            EndTime = TimeSpan.Parse(itemMaster.EndTime == null ? "0:00" : itemMaster.EndTime),
                            Length = itemMaster.Length,
                            UnitID = itemMaster.UnitID,
                            OperatorID = itemMaster.OperatorID,
                            ShiftEngineerID = itemMaster.ShiftEngineerID,
                            Remarks = itemMaster.Remarks,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP(),
                            IsDeleted = false
                        };

                        ShrinkageItem = new PrdFinishingMRRShrinkage
                        {
                            FinishingMRRShrinkageID = ShrinkageMasterID,
                            FinishingMRRID = MainMasterId,
                            ReqWeight = Shrinkage[0].ReqWeight,
                            FiniWeight = Shrinkage[0].FiniWeight,
                            AWWeight = Shrinkage[0].AWWeight,
                            ItemID = Shrinkage[0].ItemID,
                            SetID = Shrinkage[0].SetID,
                            SizeMRRID = Shrinkage[0].SizeMRRID,
                            WeavingMRRID = Shrinkage[0].WeavingMRRID,
                            GreigeEPIPPI = Shrinkage[0].GreigeEPIPPI,
                            FiniEPI = Shrinkage[0].FiniEPI,
                            FiniPPI = Shrinkage[0].FiniPPI,
                            AWEPIPPI = Shrinkage[0].AWEPIPPI,
                            CuttableWidth = Shrinkage[0].CuttableWidth,
                            FiniWidth = Shrinkage[0].FiniWidth,
                            AWWidth = Shrinkage[0].AWWidth,
                            WReqd = Shrinkage[0].WReqd,
                            LShrinkage = Shrinkage[0].LShrinkage,
                            WShrinkage = Shrinkage[0].WShrinkage,
                            FiniSkew = Shrinkage[0].FiniSkew,
                            IsFSPercent = Shrinkage[0].IsFSPercent,
                            AWSkew = Shrinkage[0].AWSkew,
                            IsAWPercent = Shrinkage[0].IsAWPercent,
                            MovSkew = Shrinkage[0].MovSkew,
                            IsMovPercent = Shrinkage[0].IsMovPercent,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP(),
                            IsDeleted = false
                        };

                    }
                    else
                    {

                        var MasterAll = GenericFactory_PrdFinishingMRRMaster_EF.GetAll().Where(x => x.FinishingMRRID == itemMaster.FinishingMRRID && x.CompanyID == objcmnParam.loggedCompany);
                        var ShrinkageAll = GenericFactory_PrdFinishingMRRShrinkage_EF.GetAll().Where(x => x.FinishingMRRID == itemMaster.FinishingMRRID && x.CompanyID == objcmnParam.loggedCompany);
                        //*************************************End Get Data From Related Table to Update*********************************                            
                        FinishingMRRNo = itemMaster.FinishingMRRNo;
                        //Update PrdFinishingMRRMaster
                        MasterItem = MasterAll.First(x => x.FinishingMRRID == itemMaster.FinishingMRRID);
                        MasterItem.FinishingMRRDate = (DateTime)itemMaster.FinishingMRRDate;
                        MasterItem.ItemID = itemMaster.ItemID;
                        MasterItem.SetID = itemMaster.SetID;
                        MasterItem.SizeMRRID = itemMaster.SizeMRRID;
                        MasterItem.WeavingMRRID = itemMaster.WeavingMRRID;
                        MasterItem.BuyerID = (int)itemMaster.BuyerID;
                        MasterItem.PIID = itemMaster.PIID;
                        MasterItem.FinishingTypeID = (int)itemMaster.FinishingTypeID;
                        MasterItem.MachineID = itemMaster.MachineID;
                        MasterItem.ShiftID = itemMaster.ShiftID;
                        MasterItem.StartTime = TimeSpan.Parse(itemMaster.StartTime == null ? "0:00" : itemMaster.StartTime);
                        MasterItem.EndTime = TimeSpan.Parse(itemMaster.EndTime == null ? "0:00" : itemMaster.EndTime);
                        MasterItem.Length = itemMaster.Length;
                        MasterItem.UnitID = itemMaster.UnitID;
                        MasterItem.OperatorID = itemMaster.OperatorID;
                        MasterItem.ShiftEngineerID = itemMaster.ShiftEngineerID;
                        MasterItem.Remarks = itemMaster.Remarks;

                        MasterItem.CompanyID = objcmnParam.loggedCompany;
                        MasterItem.UpdateBy = objcmnParam.loggeduser;
                        MasterItem.UpdateOn = DateTime.Now;
                        MasterItem.UpdatePc = HostService.GetIP();

                        //Update PrdFinishingMRRShrinkage
                        ShrinkageItem = ShrinkageAll.First(x => x.FinishingMRRID == itemMaster.FinishingMRRID && x.FinishingMRRShrinkageID == Shrinkage[0].FinishingMRRShrinkageID);
                        ShrinkageItem.ReqWeight = Shrinkage[0].ReqWeight;
                        ShrinkageItem.FiniWeight = Shrinkage[0].FiniWeight;
                        ShrinkageItem.AWWeight = Shrinkage[0].AWWeight;
                        ShrinkageItem.ItemID = Shrinkage[0].ItemID;
                        ShrinkageItem.SetID = Shrinkage[0].SetID;
                        ShrinkageItem.SizeMRRID = Shrinkage[0].SizeMRRID;
                        ShrinkageItem.WeavingMRRID = Shrinkage[0].WeavingMRRID;
                        ShrinkageItem.GreigeEPIPPI = Shrinkage[0].GreigeEPIPPI;
                        ShrinkageItem.FiniEPI = Shrinkage[0].FiniEPI;
                        ShrinkageItem.FiniPPI = Shrinkage[0].FiniPPI;
                        ShrinkageItem.AWEPIPPI = Shrinkage[0].AWEPIPPI;
                        ShrinkageItem.CuttableWidth = Shrinkage[0].CuttableWidth;
                        ShrinkageItem.FiniWidth = Shrinkage[0].FiniWidth;
                        ShrinkageItem.AWWidth = Shrinkage[0].AWWidth;
                        ShrinkageItem.WReqd = Shrinkage[0].WReqd;
                        ShrinkageItem.LShrinkage = Shrinkage[0].LShrinkage;
                        ShrinkageItem.WShrinkage = Shrinkage[0].WShrinkage;
                        ShrinkageItem.FiniSkew = Shrinkage[0].FiniSkew;
                        ShrinkageItem.IsFSPercent = Shrinkage[0].IsFSPercent;
                        ShrinkageItem.AWSkew = Shrinkage[0].AWSkew;
                        ShrinkageItem.IsAWPercent = Shrinkage[0].IsAWPercent;
                        ShrinkageItem.MovSkew = Shrinkage[0].MovSkew;
                        ShrinkageItem.IsMovPercent = Shrinkage[0].IsMovPercent;

                        ShrinkageItem.CompanyID = objcmnParam.loggedCompany;
                        ShrinkageItem.UpdateBy = objcmnParam.loggeduser;
                        ShrinkageItem.UpdateOn = DateTime.Now;
                        ShrinkageItem.UpdatePc = HostService.GetIP();
                    }

                    if (itemMaster.FinishingMRRID > 0)
                    {
                        if (MasterItem != null)
                        {
                            GenericFactory_PrdFinishingMRRMaster_EF.Update(MasterItem);
                            GenericFactory_PrdFinishingMRRMaster_EF.Save();
                        }
                        if (ShrinkageItem != null)
                        {
                            GenericFactory_PrdFinishingMRRShrinkage_EF.Update(ShrinkageItem);
                            GenericFactory_PrdFinishingMRRShrinkage_EF.Save();
                        }
                    }
                    else
                    {
                        if (MasterItem != null)
                        {
                            GenericFactory_PrdFinishingMRRMaster_EF.Insert(MasterItem);
                            GenericFactory_PrdFinishingMRRMaster_EF.Save();
                            GenericFactory_PrdFinishingMRRMaster_EF.updateMaxID("PrdFinishingMRRMaster", Convert.ToInt64(MainMasterId));
                            GenericFactory_PrdFinishingMRRMaster_EF.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        }
                        if (ShrinkageItem != null)
                        {
                            GenericFactory_PrdFinishingMRRShrinkage_EF.Insert(ShrinkageItem);
                            GenericFactory_PrdFinishingMRRShrinkage_EF.Save();
                            GenericFactory_PrdFinishingMRRShrinkage_EF.updateMaxID("PrdFinishingMRRShrinkage", Convert.ToInt64(ShrinkageMasterID));
                        }
                    }

                    transaction.Complete();
                    result = FinishingMRRNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
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
            return result;
            //**************************************************End Main Operation************************************************            
        }

        public string DeleteFinishingMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_PrdFinishingMRRMaster_EF = new PrdFinishingMRRMaster_EF();
                GenericFactory_PrdFinishingMRRShrinkage_EF = new PrdFinishingMRRShrinkage_EF();
                var MasterItem = new PrdFinishingMRRMaster();
                var ShrinkageItem = new PrdFinishingMRRShrinkage();
                try
                {

                    var MasterAll = GenericFactory_PrdFinishingMRRMaster_EF.GetAll().Where(x => x.FinishingMRRID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                    var ShrinkageAll = GenericFactory_PrdFinishingMRRShrinkage_EF.GetAll().Where(x => x.FinishingMRRID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                    //Delete PrdFinishingMRRMaster
                    MasterItem = MasterAll.First(x => x.FinishingMRRID == objcmnParam.id);
                    MasterItem.IsDeleted = true;
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();

                    //Delete PrdFinishingMRRMaster
                    ShrinkageItem = ShrinkageAll.First(x => x.FinishingMRRID == objcmnParam.id);
                    ShrinkageItem.IsDeleted = true;
                    ShrinkageItem.CompanyID = objcmnParam.loggedCompany;
                    ShrinkageItem.DeleteBy = objcmnParam.loggeduser;
                    ShrinkageItem.DeleteOn = DateTime.Now;
                    ShrinkageItem.DeletePc = HostService.GetIP();

                    if (MasterItem != null)
                    {
                        GenericFactory_PrdFinishingMRRMaster_EF.Update(MasterItem);
                        GenericFactory_PrdFinishingMRRMaster_EF.Save();
                    }
                    if (ShrinkageItem != null)
                    {
                        GenericFactory_PrdFinishingMRRShrinkage_EF.Update(ShrinkageItem);
                        GenericFactory_PrdFinishingMRRShrinkage_EF.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.FinishingMRRNo;

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
