using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections;
using ABS.Utility;
using ABS.Service.MenuMgt;

namespace ABS.Service.Production.Factories
{
    public class SetSetupMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdSetMaster> GFactory_EF_PrdSetMaster = null;
        private iGenericFactory<vmSetMaster> GFactory_VM_PrdSetMaster = null;
        private iGenericFactory_EF<PrdSetSetup> GFactory_EF_PrdSetSetup = null;
        private iGenericFactory<vmSetSetupMasterDetail> GFactory_VM_PrdSetSetupMasterDetail = null;
        private iGenericFactory<vmSelectedItemDataSetSetup> GFactory_vmSelectedItemDataSetSetup_GF = null;

        public IEnumerable<vmPISetSetup> GetPI(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmPISetSetup> objPI = null;
            var cache_PI = MemoryCache.Default; //To Cache Data

            int currentrecordsTotal = 0;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    currentrecordsTotal = _ctxCmn.SalPIMasters.Where(x => x.IsActive == true && x.IsDeleted == false).Count();

                    if (cache_PI.Get("dataCache_PI") != null)
                    {
                        objPI = (IEnumerable<vmPISetSetup>)cache_PI.Get("dataCache_PI"); //Get Data From Cache

                        recordsTotal = objPI.Count();
                    }
                    if (cache_PI.Get("dataCache_PI") == null || currentrecordsTotal != recordsTotal)
                    {
                        var cachePolicty = new CacheItemPolicy();                       //To Cache Data
                        cachePolicty.AbsoluteExpiration = DateTime.Now.AddDays(1);  //To Cache Data

                        objPI = (from PM in _ctxCmn.SalPIMasters
                                 where PM.IsActive == true && PM.IsDeleted == false
                                 orderby PM.PIID descending
                                 select new
                                 {
                                     PIID = PM.PIID,
                                     PINO = PM.PINO
                                 }).ToList().Select(x => new vmPISetSetup
                                 {
                                     PIID = x.PIID,
                                     PINO = x.PINO
                                 }).ToList();

                        cache_PI.Add("dataCache_PI", objPI.ToList(), cachePolicty);        //To Cache Data

                        recordsTotal = objPI.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPI;
        }

        public IEnumerable<vmItemSetSetup> GetItem(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objItem = null;
            var cache_Item = MemoryCache.Default;

            int currentItemsTotal = 0;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    if (objcmnParam.id == 0)
                    {

                        if (cache_Item.Get("dataCache_Item") != null)
                        {
                            objItem = (IEnumerable<vmItemSetSetup>)cache_Item.Get("dataCache_Item"); //Get Data From Cache   

                            recordsTotal = objItem.Count();
                        }
                        if (cache_Item.Get("dataCache_Item") == null || currentItemsTotal != recordsTotal)
                        {
                            var cachePolictyItem = new CacheItemPolicy();                       //To Cache Data
                            cachePolictyItem.AbsoluteExpiration = DateTime.Now.AddDays(1);  //To Cache Data

                            objItem = (from IM in _ctxCmn.CmnItemMasters
                                       where IM.IsDeleted == false && IM.ItemTypeID == 1
                                       orderby IM.ItemID descending
                                       select new
                                       {
                                           ItemID = IM.ItemID,
                                           ArticleNo = IM.ArticleNo
                                       }).ToList().Select(x => new vmItemSetSetup
                                       {
                                           ItemID = x.ItemID,
                                           ArticleNo = x.ArticleNo
                                       }).ToList();

                            cache_Item.Add("dataCache_Item", objItem.ToList(), cachePolictyItem);        //To Cache Data
                        }
                    }
                    else
                    {
                        objItem = (from IM in _ctxCmn.CmnItemMasters
                                   join PD in _ctxCmn.SalPIDetails on IM.ItemID equals PD.ItemID
                                   join PM in _ctxCmn.SalPIMasters on PD.PIID equals PM.PIID
                                   join CU in _ctxCmn.CmnUsers on PM.BuyerID equals CU.UserID
                                   where PM.PIID == objcmnParam.id && IM.IsDeleted == false && IM.ItemTypeID == 1
                                   orderby IM.ItemID descending
                                   select new
                                   {
                                       PIID = PM.PIID,
                                       ItemID = IM.ItemID,
                                       BuyerID = PM.BuyerID,
                                       ArticleNo = IM.ArticleNo,
                                       BuyerName = CU.UserFullName
                                   }).ToList().Select(x => new vmItemSetSetup
                                   {
                                       PIID = x.PIID,
                                       ItemID = x.ItemID,
                                       BuyerID = x.BuyerID,
                                       ArticleNo = x.ArticleNo,
                                       BuyerName = x.BuyerName
                                   }).ToList();
                    }

                    recordsTotal = objItem.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItem;
        }

        public vmSelectedItemDataSetSetup GetSelectedItemData(vmCmnParameters objcmnParam)
        {
            GFactory_vmSelectedItemDataSetSetup_GF = new vmSelectedItemDataSetSetup_GF();
            vmSelectedItemDataSetSetup objItemDataById = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("PIID", objcmnParam.ItemType);
                    ht.Add("ItemID", objcmnParam.id);

                    spQuery = "[Get_PSSSelectedItemData]";
                    objItemDataById = GFactory_vmSelectedItemDataSetSetup_GF.ExecuteQuerySingle(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemDataById;
        }

        //public vmSelectedItemDataSetSetup GetSelectedItemData(vmCmnParameters objcmnParam)
        //{
        //    vmSelectedItemDataSetSetup objItemDataById = null;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            var BallNo = (from b in _ctxCmn.CmnItemMasters
        //                          join i in _ctxCmn.RndYarnCRDetails on b.WarpYarnID equals i.YarnID
        //                          where b.ItemID == objcmnParam.id
        //                          select i.Ratio
        //                             ).Sum();

        //            objItemDataById = (from IM in _ctxCmn.CmnItemMasters
        //                               join YM in _ctxCmn.RndYarnCRs on IM.WarpYarnID equals YM.YarnID
        //                               join CC in _ctxCmn.CmnItemColors on IM.ItemColorID equals CC.ItemColorID
        //                               join PD in _ctxCmn.SalPIDetails on IM.ItemID equals PD.ItemID
        //                               where IM.ItemID == objcmnParam.id && objcmnParam.ItemType == 0 ? true : PD.PIID == objcmnParam.ItemType
        //                               select new
        //                               {
        //                                   YarnID = IM.WarpYarnID,
        //                                   WeftYarnID = IM.WeftYarnID,
        //                                   Weave = IM.Weave,
        //                                   Length = PD.Quantity,//IM.Length,
        //                                   YarnCount = YM.YarnCount,
        //                                   YarnRatio = YM.YarnRatio,
        //                                   YarnRatioLot = YM.YarnRatioLot,
        //                                   TotalEnds = IM.TotalEnds,
        //                                   ColorID = CC.ItemColorID,
        //                                   ColorName = CC.ColorName,
        //                                   BallNo = BallNo
        //                               })
        //                                .Select(x => new vmSelectedItemDataSetSetup
        //                                {
        //                                    YarnID = (long)x.YarnID,
        //                                    WeftYarnID = (long)x.WeftYarnID,
        //                                    Weave = x.Weave,
        //                                    Length = (decimal)x.Length,
        //                                    YarnCount = x.YarnCount,
        //                                    YarnRatio = x.YarnRatio,
        //                                    YarnRatioLot = x.YarnRatioLot,
        //                                    TotalEnds = (decimal)x.TotalEnds,
        //                                    ColorID = x.ColorID,
        //                                    ColorName = x.ColorName,
        //                                    BallNo = (int)x.BallNo
        //                                }).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objItemDataById;
        //}

        public IEnumerable<vmBuyer> GetSupplier(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBuyer> objSupplier = null;
            var cacheSupplier = MemoryCache.Default; //To Cache Data

            int currentSupplier = 0;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    currentSupplier = _ctxCmn.CmnUsers.Where(x => x.UserTypeID == objcmnParam.ItemType && x.IsActive == true && x.IsDeleted == false).Count();

                    if (cacheSupplier.Get("dataCacheSupplier") != null)
                    {
                        objSupplier = (IEnumerable<vmBuyer>)cacheSupplier.Get("dataCacheSupplier"); //Get Data From Cache

                        recordsTotal = objSupplier.Count();
                    }
                    if (cacheSupplier.Get("dataCacheSupplier") == null || currentSupplier != recordsTotal)
                    {
                        var cachePolictySupplier = new CacheItemPolicy();                       //To Cache Data
                        cachePolictySupplier.AbsoluteExpiration = DateTime.Now.AddDays(1);  //To Cache Data

                        objSupplier = (from b in _ctxCmn.CmnUsers
                                       where b.UserTypeID == objcmnParam.ItemType && b.IsActive == true && b.IsDeleted == false
                                       orderby b.UserID descending
                                       select new
                                       {
                                           UserID = b.UserID,
                                           UserFullName = b.UserFullName
                                       }).ToList().Select(x => new vmBuyer
                                       {
                                           UserID = x.UserID,
                                           UserName = x.UserFullName
                                       }).ToList();

                        cacheSupplier.Add("dataCacheSupplier", objSupplier.ToList(), cachePolictySupplier);        //To Cache Data

                        recordsTotal = objSupplier.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSupplier;
        }

        public IEnumerable<vmSetDetail> GetRefSet(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSetDetail> objRefSet = null;
            //var cacheRefSet = MemoryCache.Default; //To Cache Data

            //int currentRefSet = 0;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //currentRefSet = _ctxCmn.PrdSetSetups.Where(x => x.IsDeleted == false).Count();

                    //if (cacheRefSet.Get("dataCacheRefSet") != null)
                    //{
                    //    objRefSet = (IEnumerable<vmSetDetail>)cacheRefSet.Get("dataCacheRefSet"); //Get Data From Cache  
                    //    recordsTotal = objRefSet.Count();
                    //}
                    //if (cacheRefSet.Get("dataCacheRefSet") == null || currentRefSet != recordsTotal)
                    //{
                        //var cachePolictyRefSet = new CacheItemPolicy();                       //To Cache Data
                        //cachePolictyRefSet.AbsoluteExpiration = DateTime.Now.AddDays(1);  //To Cache Data

                        objRefSet = (from b in _ctxCmn.PrdSetSetups
                                     where b.IsDeleted == false && b.ItemID==objcmnParam.id
                                     orderby b.YarnID descending
                                     select new
                                     {
                                         SetID = b.SetID,
                                         SetNo = b.SetNo
                                     }).ToList().Select(x => new vmSetDetail
                                     {
                                         SetID = x.SetID,
                                         SetNo = x.SetNo
                                     }).ToList();

                        //cacheRefSet.Add("dataCacheRefSet", objRefSet.ToList(), cachePolictyRefSet);        //To Cache Data                        
                    //}
                    //recordsTotal = objRefSet.Count();
                        recordsTotal = _ctxCmn.PrdSetSetups.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objRefSet;
        }

        public vmSetDetail GetSetWiseDate(vmCmnParameters objcmnParam)
        {
            vmSetDetail objSetWiseDate = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objSetWiseDate = (from S in _ctxCmn.PrdSetSetups
                                      where S.SetID == objcmnParam.id
                                      select new
                                      {
                                          RefSetDate = S.SetDate
                                      })
                                        .Select(x => new vmSetDetail
                                        {
                                            RefSetDate = x.RefSetDate
                                        }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSetWiseDate;
        }

        public IEnumerable<vmSetMaster> GetSetSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GFactory_VM_PrdSetMaster = new PrdSetMaster_VM();
            IEnumerable<vmSetMaster> objSetMaster = null;

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

                    spQuery = "[Get_SetMaster]";
                    objSetMaster = GFactory_VM_PrdSetMaster.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdSetMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//objSetMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSetMaster;
        }

        public vmSetSetupMasterDetail GetSetMasterByID(vmCmnParameters objcmnParam)
        {
            GFactory_VM_PrdSetSetupMasterDetail = new PrdSetSetupMasterDetail_VM();
            vmSetSetupMasterDetail objSingleSetMaster = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("SetMasterID", objcmnParam.id);

                    spQuery = "[Get_SetMasterByID]";
                    objSingleSetMaster = GFactory_VM_PrdSetSetupMasterDetail.ExecuteQuerySingle(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSingleSetMaster;
        }

        public IEnumerable<vmSetSetupMasterDetail> GetSetSetupDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GFactory_VM_PrdSetSetupMasterDetail = new PrdSetSetupMasterDetail_VM();
            IEnumerable<vmSetSetupMasterDetail> objSetDetail = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("SetMasterID", objcmnParam.id);

                    spQuery = "[Get_SetDetailByID]";
                    objSetDetail = GFactory_VM_PrdSetSetupMasterDetail.ExecuteQuery(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSetDetail;
        }

        public async Task<string> SaveUpdateSetSetupMasterDetail(vmSetSetupMasterDetail ModelMaster, List<vmSetSetupMasterDetail> ModelDetail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            long TransactionID = 0;
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdSetMaster = new PrdSetMaster_EF();
                GFactory_EF_PrdSetSetup = new PrdSetSetup_EF();
                string SetNo = string.Empty; int SetMasterId = 0, SetDetailId = 0;
                long SetFirstDigit = 0, SetOtherDigits = 0;

                var Masteritem = new PrdSetMaster();
                var SetDetail = new List<PrdSetSetup>();

                vmSetSetupMasterDetail item = new vmSetSetupMasterDetail();
                vmSetSetupMasterDetail items = new vmSetSetupMasterDetail();
                //-------------------END----------------------

                if (ModelDetail.Count() > 0)
                {
                    try
                    {
                        using (_ctxCmn = new ERP_Entities())
                        {
                            if (ModelMaster.SetMasterID == 0)
                            {
                                SetMasterId = Convert.ToInt16(GFactory_EF_PrdSetMaster.getMaxID("PrdSetMaster"));
                                SetDetailId = Convert.ToInt16(GFactory_EF_PrdSetSetup.getMaxID("PrdSetSetup"));
                                SetFirstDigit = Convert.ToInt64(SetDetailId.ToString().Substring(0, 1));
                                SetOtherDigits = Convert.ToInt64(SetDetailId.ToString().Substring(1, SetDetailId.ToString().Length - 1));

                                //CustomNo = GFactory_EF_PrdSetSetup.getCustomCode(MenuId, DateTime.Now, CompanyId, 1, 1);
                                //if (CustomNo == null || CustomNo == "")
                                //{
                                //    SetNo = SetDetailId.ToString();
                                //}
                                //else
                                //{
                                //    SetNo = CustomNo;
                                //}
                                TransactionID = SetMasterId;
                                Masteritem = new PrdSetMaster
                                {
                                    SetMasterID = SetMasterId,
                                    BuyerID = ModelMaster.BuyerID,
                                    Description = ModelMaster.Description,
                                    ItemID = (long)ModelMaster.ItemID,
                                    PIID = ModelMaster.PIID,
                                    PIItemlength = ModelMaster.PIItemlength,
                                    RefSetDate = ModelMaster.RefSetDate,
                                    RefSetID = ModelMaster.RefSetID,
                                    SetDate = ModelMaster.SetDate,
                                    SupplierID = ModelMaster.SupplierID,
                                    StatusID = ModelMaster.StatusID,
                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false
                                };

                                for (int i = 0; i < ModelDetail.Count(); i++)
                                {
                                    item = ModelDetail[i];

                                    var Detailitem = new PrdSetSetup
                                    {
                                        SetID = Convert.ToInt64(SetFirstDigit + "" + SetOtherDigits),
                                        YarnID = (long)item.YarnID,
                                        WarpYarnID = item.YarnID,
                                        WeftYarnID = item.WeftYarnID,
                                        Weave = item.Weave,
                                        BuyerID = item.BuyerID,
                                        ColorID = item.ColorID,
                                        Description = item.Description,
                                        EndsPerCreel = item.EndsPerCreel,
                                        LeaseReapet = item.LeaseRepeat,
                                        MachineSpeed = item.MachineSpeed,
                                        SetDate = item.SetDate,
                                        ItemID = (long)item.ItemID,
                                        PIID = item.PIID,
                                        SetLength = (long)item.SetLength,
                                        SetMasterID = SetMasterId,
                                        SetNo = item.SetNo,
                                        SupplierID = item.SupplierID,
                                        TotalEnds = item.TotalEnds,
                                        YarnCount = item.YarnCount,
                                        YarnRatio = item.YarnRatio,
                                        YarnRatioLot = item.YarnRatioLot,
                                        NoOfBall = item.NoOfBall,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    //***************************************END*******************************************
                                    SetDetail.Add(Detailitem);
                                    SetOtherDigits++;
                                }
                            }
                            else
                            {
                                var SetMasterAll = GFactory_EF_PrdSetMaster.GetAll().Where(x => x.SetMasterID == ModelMaster.SetMasterID);
                                var SetDetailAll = GFactory_EF_PrdSetSetup.GetAll().Where(x => x.SetMasterID == ModelMaster.SetMasterID);

                                Masteritem = SetMasterAll.FirstOrDefault(x => x.SetMasterID == ModelMaster.SetMasterID);
                                Masteritem.BuyerID = ModelMaster.BuyerID;
                                Masteritem.Description = ModelMaster.Description;
                                Masteritem.ItemID = (long)ModelMaster.ItemID;
                                Masteritem.PIID = ModelMaster.PIID;
                                Masteritem.PIItemlength = ModelMaster.PIItemlength;
                                Masteritem.RefSetDate = ModelMaster.RefSetDate;
                                Masteritem.RefSetID = ModelMaster.RefSetID;
                                Masteritem.SetDate = ModelMaster.SetDate;
                                Masteritem.SupplierID = ModelMaster.SupplierID;
                                Masteritem.StatusID = ModelMaster.StatusID;
                                Masteritem.CompanyID = objcmnParam.loggedCompany;
                                Masteritem.UpdateBy = objcmnParam.loggeduser;
                                Masteritem.UpdateOn = DateTime.Now;
                                Masteritem.UpdatePc = HostService.GetIP();
                                Masteritem.IsDeleted = false;

                                for (int i = 0; i < ModelDetail.Count(); i++)
                                {
                                    item = ModelDetail[i];

                                    foreach (PrdSetSetup d in SetDetailAll.Where(d => d.SetMasterID == ModelMaster.SetMasterID && d.SetID == item.SetID))
                                    {
                                        d.YarnID = (long)item.YarnID;
                                        d.WarpYarnID = item.YarnID;
                                        d.WeftYarnID = item.WeftYarnID;
                                        d.Weave = item.Weave;
                                        d.BuyerID = item.BuyerID;
                                        d.ColorID = item.ColorID;
                                        d.Description = item.Description;
                                        d.EndsPerCreel = item.EndsPerCreel;
                                        d.LeaseReapet = item.LeaseRepeat;
                                        d.MachineSpeed = item.MachineSpeed;
                                        d.SetDate = item.SetDate;
                                        d.ItemID = (long)item.ItemID;
                                        d.PIID = item.PIID;
                                        d.SetLength = (long)item.SetLength;
                                        d.SetNo = item.SetNo;
                                        d.SupplierID = item.SupplierID;
                                        d.TotalEnds = item.TotalEnds;
                                        d.YarnCount = item.YarnCount;
                                        d.YarnRatio = item.YarnRatio;
                                        d.YarnRatioLot = item.YarnRatioLot;
                                        d.NoOfBall = item.NoOfBall;

                                        d.CompanyID = objcmnParam.loggedCompany;
                                        d.UpdateBy = objcmnParam.loggeduser;
                                        d.UpdateOn = DateTime.Now;
                                        d.UpdatePc = HostService.GetIP();
                                        d.IsDeleted = false;

                                        SetDetail.Add(d);
                                        break;
                                    }

                                }
                            }

                            if (ModelMaster.SetMasterID > 0)
                            {
                                if (Masteritem != null)
                                {
                                    GFactory_EF_PrdSetMaster.Update(Masteritem);
                                    GFactory_EF_PrdSetMaster.Save();
                                }
                                if (SetDetail != null && SetDetail.Count != 0)
                                {
                                    GFactory_EF_PrdSetSetup.UpdateList(SetDetail.ToList());
                                    GFactory_EF_PrdSetSetup.Save();
                                }
                            }
                            else
                            {
                                if (Masteritem != null)
                                {
                                    GFactory_EF_PrdSetMaster.Insert(Masteritem);
                                    GFactory_EF_PrdSetMaster.Save();
                                    GFactory_EF_PrdSetMaster.updateMaxID("PrdSetMaster", Convert.ToInt64(SetMasterId));
                                }
                                if (SetDetail != null && SetDetail.Count != 0)
                                {
                                    GFactory_EF_PrdSetSetup.InsertList(SetDetail.ToList());
                                    GFactory_EF_PrdSetSetup.Save();
                                    GFactory_EF_PrdSetSetup.updateMaxID("PrdSetSetup", Convert.ToInt64(SetFirstDigit + "" + (SetOtherDigits - 1)));
                                }
                            }

                            transaction.Complete();
                            result = "1";

                        }

                      

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
                    throw new Exception();
                }
            }

            // Approval Code 
            #region WorkFlow Transaction Entry and email sending
            if (ModelMaster.SetMasterID == 0)
            {
                UserCommonEntity commonEntity = new UserCommonEntity();
                commonEntity.currentMenuID = objcmnParam.menuId;
                commonEntity.loggedCompnyID = objcmnParam.loggedCompany;
                commonEntity.loggedUserID = objcmnParam.loggeduser;
                int workflowID = 0;
                List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                foreach (vmCmnWorkFlowMaster itemWFM in listWorkFlow)
                {
                    int userTeamID = itemWFM.UserTeamID ?? 0;
                    if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, itemWFM.WorkFlowID) && userTeamID > 0)
                    {
                        itemWFM.WorkFlowTranCustomID = (Int16)TransactionID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(itemWFM, commonEntity);
                    }
                    if (userTeamID == 0)
                    {
                        itemWFM.WorkFlowTranCustomID = (Int16)TransactionID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(itemWFM, commonEntity);
                    }
                }

                int mail = 0;
                foreach (vmCmnWorkFlowMaster itemWFM in listWorkFlow)
                {
                    NotificationEntity notification = new NotificationEntity();
                    notification.WorkFlowID = itemWFM.WorkFlowID;
                    notification.TransactionID = (Int16)TransactionID;
                    List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(notification, "Created");
                    foreach (var mailModel in nModel)
                    {
                        mail = await new EmailService().NotificationMail(mailModel);
                    }
                }

            }
            #endregion WorkFlow Transaction Entry and email sending    
            return result;
        }

        public string DelUpdateSetMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdSetMaster = new PrdSetMaster_EF();
                GFactory_EF_PrdSetSetup = new PrdSetSetup_EF();

                var Masteritem = new PrdSetMaster();
                var SetDetail = new List<PrdSetSetup>();

                //For Update Master Detail
                var SetMasterAll = GFactory_EF_PrdSetMaster.GetAll().Where(x => x.SetMasterID == objcmnParam.id);
                var SetDetailAll = GFactory_EF_PrdSetSetup.GetAll().Where(x => x.SetMasterID == objcmnParam.id);
                //-------------------END----------------------

                try
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        Masteritem = SetMasterAll.First(x => x.SetMasterID == objcmnParam.id);
                        Masteritem.CompanyID = objcmnParam.loggedCompany;
                        Masteritem.DeleteBy = objcmnParam.loggeduser;
                        Masteritem.DeleteOn = DateTime.Now;
                        Masteritem.DeletePc = HostService.GetIP();
                        Masteritem.IsDeleted = true;

                        foreach (PrdSetSetup d in SetDetailAll.Where(d => d.SetMasterID == objcmnParam.id))
                        {
                            d.CompanyID = objcmnParam.loggedCompany;
                            d.DeleteBy = objcmnParam.loggeduser;
                            d.DeleteOn = DateTime.Now;
                            d.DeletePc = HostService.GetIP();
                            d.IsDeleted = true;

                            SetDetail.Add(d);
                        }

                    }

                    if (Masteritem != null)
                    {
                        GFactory_EF_PrdSetMaster.Update(Masteritem);
                        GFactory_EF_PrdSetMaster.Save();
                    }
                    if (SetDetail != null)
                    {
                        GFactory_EF_PrdSetSetup.UpdateList(SetDetail.ToList());
                        GFactory_EF_PrdSetSetup.Save();
                    }

                    transaction.Complete();
                    result = "1";

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
