using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Accounting;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Accounting.Interfaces;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Accounting.Factories
{
    public class PreCostingMgt : iPreCostingMgt
    {

        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_CmnUser = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
 
        public List<vmBOM> GetBomNArticleNo(vmCmnParameters objcmnParam)
        {
            List<vmBOM> lstBomArticle = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBomArticle = (from bm in _ctxCmn.PrdBOMMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                     //join ttm in _ctxCmn.CmnItemMasters on rm.FinishingProcessID equals fp.FinishingProcessID
                                     //join crncy in _ctxCmn.AccCurrencyInfoes on rm.CurrencyID equals crncy.Id

                                     select new
                                     {
                                         BOMID = bm.BOMID,
                                         BOMNO = bm.BOMNO,
                                         ItemID = bm.ItemID,
                                         ItemName = bm.CmnItemMaster.ItemName,
                                         ArticleNo = bm.CmnItemMaster.ArticleNo,
                                         BomArticleNo = "BOM NO :" + bm.BOMNO + " + ArticleNo :" + bm.CmnItemMaster.ArticleNo

                                     }).ToList().Select(x => new vmBOM
                                     {
                                         BOMID = x.BOMID,
                                         BOMNO = x.BOMNO,
                                         ItemID = x.ItemID,
                                         ItemName = x.ItemName,
                                         ArticleNo = x.ArticleNo,
                                         BomArticleNo = x.BomArticleNo

                                     }).OrderByDescending(k => k.BOMID).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBomArticle;

        }
     
        public IEnumerable<vmBOM> GetDyingByBomID(vmCmnParameters objcmnParam, Int64 bomID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstdyn = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstdyn = (from dyn in _ctxCmn.PrdBOMDyings.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList()

                              //join unit in _ctxCmn.CmnUOMs on item.UOMID equals unit.UOMID
                              //into unt
                              //from yUnt in unt

                              select new
                              {
                                  ItemID = dyn.DyingChemicalID,
                                  ItemName = dyn.CmnItemMaster.ItemName,
                                  BOMDyingID = dyn.BOMDyingID,
                                  Qty = dyn.Qty,
                                  DyingChemicalID = dyn.DyingChemicalID,
                                  UnitID = dyn.UnitID,
                                  UOMName = dyn.CmnUOM.UOMName,
                                  CompanyID = dyn.CompanyID,
                                  IsDeleted = dyn.IsDeleted,
                                  CostingDyingID = 0,
                                  CostingID = 0,
                                  LastPurchasePrice = 0.00m,
                                  Amount = 0.00m
                              }).Select(x => new vmBOM
                              {
                                  ItemID = x.ItemID,
                                  ItemName = x.ItemName,
                                  BOMDyingID = x.BOMDyingID,
                                  Qty = x.Qty,
                                  DyingChemicalID = x.DyingChemicalID,
                                  UnitID = x.UnitID,
                                  UOMName = x.UOMName,
                                  CompanyID = x.CompanyID,
                                  IsDeleted = x.IsDeleted,
                                  CostingDyingID = x.CostingDyingID,
                                  CostingID = x.CostingID,
                                  LastPurchasePrice = x.LastPurchasePrice,
                                  Amount = x.Amount
                              }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstdyn;

        }

        public IEnumerable<vmBOM> GetSizingByBomID(vmCmnParameters objcmnParam, Int64 bomID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstSiz = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstSiz = (from siz in _ctxCmn.PrdBOMSizes.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList()

                              //join unit in _ctxCmn.CmnUOMs on item.UOMID equals unit.UOMID
                              //into unt
                              //from yUnt in unt

                              select new
                              {
                                  ItemID = siz.SizeChemicalID,
                                  ItemName = siz.CmnItemMaster.ItemName,
                                  BOMSizeID = siz.BOMSizeID,
                                  Qty = siz.Qty,
                                  SizeChemicalID = siz.SizeChemicalID,
                                  UnitID = siz.UnitID,
                                  UOMName = siz.CmnUOM.UOMName,
                                  CompanyID = siz.CompanyID,
                                  IsDeleted = siz.IsDeleted,
                                  CostingSizeID = 0,
                                  CostingID = 0,
                                  LastPurchasePrice = 0.00m,
                                  Amount = 0.00m
                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ItemName = x.ItemName,
                                        BOMSizeID = x.BOMSizeID,
                                        Qty = x.Qty,
                                        SizeChemicalID = x.SizeChemicalID,
                                        UnitID = x.UnitID,
                                        UOMName = x.UOMName,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted,
                                        CostingSizeID = x.CostingSizeID,
                                        CostingID = x.CostingID,
                                        LastPurchasePrice = x.LastPurchasePrice,
                                        Amount = x.Amount
                                    }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstSiz;
        }
        
        public IEnumerable<vmBOM> GetItemDetailByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            _ctxCmn = new ERP_Entities();
            try
            {
                lstBOM = (from item in _ctxCmn.CmnItemMasters.Where(m => m.ItemID == itemID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList()

                          //join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID
                          //into clr
                          //from yClr in clr

                          join warp in _ctxCmn.RndYarnCRs on item.WarpYarnID equals warp.YarnID
                          into warpYarn
                          from yWarpYarn in warpYarn
                          join weft in _ctxCmn.RndYarnCRs on item.WeftYarnID equals weft.YarnID
                          into weftYarn
                          from xWeftYarn in weftYarn
                          select new
                          {
                              ItemID = item.ItemID,
                              ItemName = item.ItemName,
                              ArticleNo = item.ArticleNo,
                              CuttableWidth = item.CuttableWidth,
                              WeightPerUnit = item.WeightPerUnit,
                              ColorName = item.CmnItemColor.ColorName,//xColor.ColorName,//color.ColorName,
                              Construction = item.Note,//"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + item.EPI.ToString() + "x" + item.PPI.ToString(),//item.Weave,
                              WarpYarn = yWarpYarn.YarnCount,
                              WeftYarn = xWeftYarn.YarnCount,
                              FinishingWidth = item.FinishingWidth

                          })
                                .Select(x => new vmBOM
                                {
                                    ItemID = x.ItemID,
                                    ItemName = x.ItemName,
                                    ArticleNo = x.ArticleNo,
                                    CuttableWidth = x.CuttableWidth,
                                    WeightPerUnit = x.WeightPerUnit,
                                    ColorName = x.ColorName,
                                    Construction = x.Construction,
                                    WarpYarn = x.WarpYarn,
                                    WeftYarn = x.WeftYarn,
                                    FinishingWidth = x.FinishingWidth
                                }).ToList();


            }
            catch (Exception e)
            {
                e.ToString();
            }

            return lstBOM;
        }

        public IEnumerable<object> GetDetailListByPrCostID(vmCmnParameters objcmnParam, Int64 costingID, out int recordsTotal)
        {
            IEnumerable<object> lstCost = null;
            _ctxCmn = new ERP_Entities();
            recordsTotal = 0;
            //using (_ctxCmn = new ERP_Entities())
            //{
            try
            {



                lstCost = (from costM in _ctxCmn.PrdPreCostingMasters.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                           // join item in _ctxCmn.CmnItemMasters on costM.ItemID equals item.ItemID

                           //join color in _ctxCmn.CmnItemColors on costM.CmnItemMaster.ItemColorID equals color.ItemColorID
                           //into clr 
                           //from yClr in clr.DefaultIfEmpty()
                           join warp in _ctxCmn.RndYarnCRs on costM.CmnItemMaster.WarpYarnID equals warp.YarnID
                           into warpYarn
                           from yWarpYarn in warpYarn.DefaultIfEmpty()
                           join weft in _ctxCmn.RndYarnCRs on costM.CmnItemMaster.WeftYarnID equals weft.YarnID
                           into weftYarn
                           from xWeftYarn in weftYarn.DefaultIfEmpty()
                           select new
                           {
                               ItemID = costM.ItemID,
                               ItemName = costM.CmnItemMaster.ItemName,
                               ArticleNo = costM.CmnItemMaster.ArticleNo,
                               CuttableWidth = costM.CmnItemMaster.CuttableWidth,
                               WeightPerUnit = costM.CmnItemMaster.WeightPerUnit,
                               ColorName = costM.CmnItemMaster.CmnItemColor.ColorName,
                               Construction = costM.CmnItemMaster.Note,
                               WarpYarn = yWarpYarn.YarnCount,
                               WeftYarn = xWeftYarn.YarnCount,
                               FinishingWidth = costM.CmnItemMaster.FinishingWidth,
                               BOMID = costM.BOMID,
                               BOMNO = costM.PrdBOMMaster.BOMNO,
                               BOMDate = costM.PrdBOMMaster.BOMDate,
                               BomArticleNo = "BOM NO :" + costM.PrdBOMMaster.BOMNO + " + ArticleNo :" + costM.CmnItemMaster.ArticleNo,
                               CurrencyName = costM.AccCurrencyInfo.CurrencyName,
                               CurrencyID = costM.AccCurrencyInfo.Id,
                               CostingDate = costM.CostingDate,
                               CostingID = costM.CostingID,
                               CostingNo = costM.CostingNo,


                               ForDying = (from dyn in _ctxCmn.PrdPreCostingDyings.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                           select new
                                           {
                                               ItemID = dyn.DyingChemicalID,
                                               ItemName = dyn.CmnItemMaster.ItemName,
                                               Qty = dyn.Qty,
                                               DyingChemicalID = dyn.DyingChemicalID,
                                               UnitID = dyn.UnitID,
                                               UOMName = dyn.CmnUOM.UOMName,
                                               CompanyID = dyn.CompanyID,
                                               IsDeleted = dyn.IsDeleted,
                                               CostingDyingID = dyn.CostingDyingID,
                                               CostingID = dyn.CostingID,
                                               LastPurchasePrice = dyn.LastPurchasePrice,
                                               Amount = dyn.Amount
                                           }),

                               ForSizing = (from siz in _ctxCmn.PrdPreCostingSizes.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)


                                            select new
                                            {
                                                ItemID = siz.SizeChemicalID,
                                                ItemName = siz.CmnItemMaster.ItemName,
                                                Qty = siz.Qty,
                                                SizeChemicalID = siz.SizeChemicalID,
                                                UnitID = siz.UnitID,
                                                UOMName = siz.CmnUOM.UOMName,
                                                CompanyID = siz.CompanyID,
                                                IsDeleted = siz.IsDeleted,
                                                CostingSizeID = siz.CostingSizeID,
                                                CostingID = siz.CostingID,
                                                LastPurchasePrice = siz.LastPurchasePrice,
                                                Amount = siz.Amount
                                            }),

                               ForFinishing = (from ft in _ctxCmn.PrdPreCostingFinishings.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                               join fp in _ctxCmn.PrdFinishingProcesses on ft.FinishingProcessID equals fp.FinishingProcessID

                                               select new
                                               {
                                                   // ItemID = ft.,
                                                   ProcessFullName = fp.FinishingProcessFName,
                                                   ProcessCode = fp.FinishingProcessName,
                                                   FinishingProcessID = ft.FinishingProcessID,
                                                   UnitPrice = ft.UnitPrice,
                                                   CompanyID = ft.CompanyID,
                                                   IsDeleted = ft.IsDeleted,
                                                   CostingID = ft.CostingID,
                                                   CostingFinishID = ft.CostingFinishID
                                               }),

                               ForYarn = (from cm in _ctxCmn.PrdPreCostingYarns.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                          //join cd in _ctxCmn.RndConsumptionDetails on cm.ConsumptionID equals cd.ConsumptionID

                                          join ct in _ctxCmn.RndConsumptionTypes on cm.ConsumptionTypeID equals ct.ConsumptionTypeID

                                          //  join itm in _ctxCmn.CmnItemMasters on cd.ItemID equals itm.ItemID

                                          // join lot in _ctxCmn.CmnLots on cd.LotID equals lot.LotID

                                          //into lots
                                          //from ylot in lots.DefaultIfEmpty()

                                          select new
                                          {
                                              ItemID = cm.CmnItemMaster.ItemID,
                                              ConsumptionTypeID = cm.ConsumptionTypeID,
                                              YarnType = ct.ConsumptionTypeName,
                                              YarnID = cm.YarnID,
                                              Yarn = cm.CmnItemMaster.ItemName,
                                              LotID = cm.LotID,
                                              LotNo = cm.CmnLot.LotNo,
                                              Qty = cm.Qty,
                                              UnitID = cm.UnitID,
                                              UOMName = cm.CmnUOM.UOMName,
                                              CompanyID = cm.CompanyID,
                                              IsDeleted = cm.IsDeleted,
                                              LastPurchasePrice = cm.LastPurchasePrice,
                                              Amount = cm.Amount,
                                              CostingID = cm.CostingID,
                                              CostingYarnID = cm.CostingYarnID
                                          }),

                               ForDetail = (from dtl in _ctxCmn.PrdPreCostingDetails.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                            select new
                                            {

                                                SizeCost = dtl.SizeCost,
                                                OverHeadCost = dtl.OverHeadCost,
                                                FinishingCost = dtl.FinishingCost,
                                                DyingCost = dtl.DyingCost,
                                                CompanyID = dtl.CompanyID,
                                                IsDeleted = dtl.IsDeleted,
                                                CostingID = dtl.CostingID,
                                                CostingDetailID = dtl.CostingDetailID,
                                                UnitPrice = dtl.UnitPrice,
                                                YarnCost = dtl.YarnCost
                                            })
                           }).ToList();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            //   }
            return lstCost;


        }

        public IEnumerable<vmBOM> GetFinishingByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBOM = (from ft in _ctxCmn.PrdFinishingTypes.Where(m => m.ItemID == itemID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList()

                              join fp in _ctxCmn.PrdFinishingProcesses on ft.FinishingProcessID equals fp.FinishingProcessID

                              select new
                              {
                                  ItemID = ft.ItemID,
                                  ProcessFullName = fp.FinishingProcessFName,
                                  ProcessCode = fp.FinishingProcessName,
                                  FinishingProcessID = fp.FinishingProcessID,
                                  UnitPrice = 0.00m,
                                  CompanyID = ft.CompanyID,
                                  IsDeleted = ft.IsDeleted,
                                  CostingID = 0,
                                  CostingFinishID = 0
                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ProcessFullName = x.ProcessFullName,
                                        ProcessCode = x.ProcessCode,
                                        FinishingProcessID = x.FinishingProcessID,
                                        UnitPrice = x.UnitPrice,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted,
                                        CostingID = x.CostingID,
                                        CostingFinishID = x.CostingFinishID
                                    }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;
        }

        public IEnumerable<vmBOM> GetYarnByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBOM = (from cm in _ctxCmn.RndConsumptionMasters.Where(m => m.ItemID == itemID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                              join cd in _ctxCmn.RndConsumptionDetails on cm.ConsumptionID equals cd.ConsumptionID

                              join ct in _ctxCmn.RndConsumptionTypes on cd.ConsumptionTypeID equals ct.ConsumptionTypeID

                              join itm in _ctxCmn.CmnItemMasters on cd.ItemID equals itm.ItemID

                              join lot in _ctxCmn.CmnLots on cd.LotID equals lot.LotID

                              into lots
                              from ylot in lots.DefaultIfEmpty()

                              select new
                              {
                                  ItemID = cm.ItemID,
                                  ConsumptionTypeID = ct.ConsumptionTypeID,
                                  YarnType = ct.ConsumptionTypeName,
                                  YarnID = cd.YarnID,
                                  Yarn = itm.ItemName,
                                  LotID = cd.LotID,
                                  LotNo = ylot.LotNo,
                                  Qty = cd.WeigthPerUnit,
                                  UnitID = itm.CmnUOM.UOMID,
                                  UOMName = itm.CmnUOM.UOMName,
                                  CompanyID = cm.CompanyID,
                                  IsDeleted = cm.IsDeleted,
                                  LastPurchasePrice = 0.00m,
                                  Amount = 0.00m,
                                  CostingID = 0,
                                  CostingYarnID = 0
                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ConsumptionTypeID = x.ConsumptionTypeID,
                                        YarnType = x.YarnType,
                                        YarnID = x.YarnID,
                                        Yarn = x.Yarn,
                                        LotID = x.LotID,
                                        LotNo = x.LotNo,
                                        Qty = x.Qty,
                                        UnitID = x.UnitID,
                                        UOMName = x.UOMName,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted,
                                        LastPurchasePrice = x.LastPurchasePrice,
                                        Amount = x.Amount,
                                        CostingID = x.CostingID,
                                        CostingYarnID = x.CostingYarnID
                                    }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;
        }

        public IEnumerable<vmBOM> GetCostMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstCost = null;
            IEnumerable<vmBOM> lstCostWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstCostWithoutPaging = (from bm in _ctxCmn.PrdPreCostingMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                            // join item in _ctxCmn.CmnItemMasters on bm.ItemID equals item.ItemID

                                            select new
                                            {
                                                CostingID = bm.CostingID,
                                                CostingNo = bm.CostingNo,
                                                CostingDate = bm.CostingDate,
                                                CurrencyName = bm.AccCurrencyInfo.CurrencyName,
                                                CurrencyID = bm.CurrencyID,
                                                BOMID = bm.BOMID,
                                                BOMNO = bm.PrdBOMMaster.BOMNO,
                                                ItemID = bm.ItemID,
                                                CompanyID = bm.CompanyID,
                                                IsDeleted = bm.IsDeleted,
                                                ItemName = bm.CmnItemMaster.ItemName

                                            }).ToList().Select(x => new vmBOM
                                            {
                                                CostingID = x.CostingID,
                                                CostingNo = x.CostingNo,
                                                CostingDate = x.CostingDate,
                                                CurrencyName = x.CurrencyName,
                                                CurrencyID = x.CurrencyID,
                                                BOMID = x.BOMID,
                                                BOMNO = x.BOMNO,
                                                ItemID = x.ItemID,
                                                CompanyID = x.CompanyID,
                                                IsDeleted = x.IsDeleted,
                                                ItemName = x.ItemName
                                            }).ToList();

                    lstCost = lstCostWithoutPaging.OrderByDescending(x => x.CostingID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstCostWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstCost;
        }

        public string SaveNUpdatePreCosting(PrdPreCostingMaster objMaster, List<PrdPreCostingDying> lstDying, List<PrdPreCostingSize> lstSize, List<PrdPreCostingFinishing> lstFinish, List<PrdPreCostingYarn> lstYarn, List<PrdPreCostingDetail> lstDetail, int menuID)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (objMaster.CostingID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 costingID = objMaster.CostingID;
                        IEnumerable<PrdPreCostingMaster> lstPrdCostMaster = (from qcm in _ctxCmn.PrdPreCostingMasters.Where(m => m.CostingID == costingID && m.CompanyID == objMaster.CompanyID && m.IsDeleted == false) select qcm).ToList();
                        PrdPreCostingMaster objCostMaster = new PrdPreCostingMaster();
                        foreach (PrdPreCostingMaster bms in lstPrdCostMaster)
                        {
                            bms.UpdateBy = objMaster.CreateBy;
                            bms.UpdateOn = DateTime.Now;
                            bms.UpdatePc = HostService.GetIP();
                            bms.CostingDate = objMaster.CostingDate;
                            bms.CurrencyID = objMaster.CurrencyID;

                            objCostMaster = bms;
                        }

                        //  for dyng 

                        List<PrdPreCostingDying> lstCostDying = new List<PrdPreCostingDying>();

                        foreach (PrdPreCostingDying qcdt in lstDying)
                        {
                            PrdPreCostingDying objDyingDetail = (from qcdetl in _ctxCmn.PrdPreCostingDyings.Where(m => m.CostingDyingID == qcdt.CostingDyingID) select qcdetl).FirstOrDefault();

                            objDyingDetail.UpdateBy = objMaster.CreateBy;
                            objDyingDetail.UpdateOn = DateTime.Now;
                            objDyingDetail.UpdatePc = HostService.GetIP();
                            objDyingDetail.Amount = qcdt.Amount;
                            objDyingDetail.LastPurchasePrice = qcdt.LastPurchasePrice;

                            lstCostDying.Add(objDyingDetail);

                        }

                        //  for size 

                        List<PrdPreCostingSize> lstCostSize = new List<PrdPreCostingSize>();

                        foreach (PrdPreCostingSize qsiz in lstSize)
                        {
                            PrdPreCostingSize objSizeDetail = (from qszdetl in _ctxCmn.PrdPreCostingSizes.Where(m => m.CostingSizeID == qsiz.CostingSizeID) select qszdetl).FirstOrDefault();

                            objSizeDetail.UpdateBy = objMaster.CreateBy;
                            objSizeDetail.UpdateOn = DateTime.Now;
                            objSizeDetail.UpdatePc = HostService.GetIP();
                            objSizeDetail.Amount = qsiz.Amount;
                            objSizeDetail.LastPurchasePrice = qsiz.LastPurchasePrice;
                            lstCostSize.Add(objSizeDetail);
                        }

                        //  for finsh 

                        List<PrdPreCostingFinishing> lstCostFinishing = new List<PrdPreCostingFinishing>();

                        foreach (PrdPreCostingFinishing fns in lstFinish)
                        {
                            PrdPreCostingFinishing objFinishingDetail = (from qszdetl in _ctxCmn.PrdPreCostingFinishings.Where(m => m.CostingFinishID == fns.CostingFinishID) select qszdetl).FirstOrDefault();

                            objFinishingDetail.UpdateBy = objMaster.CreateBy;
                            objFinishingDetail.UpdateOn = DateTime.Now;
                            objFinishingDetail.UpdatePc = HostService.GetIP();
                            objFinishingDetail.UnitPrice = fns.UnitPrice;
                      

                            lstCostFinishing.Add(objFinishingDetail);
                        }

                        //  for yarn 

                        List<PrdPreCostingYarn> lstCostYarn = new List<PrdPreCostingYarn>();

                        foreach (PrdPreCostingYarn yrn in lstYarn)
                        {
                            PrdPreCostingYarn objYarnDetail = (from qszdetl in _ctxCmn.PrdPreCostingYarns.Where(m => m.CostingYarnID == yrn.CostingYarnID) select qszdetl).FirstOrDefault();

                            objYarnDetail.UpdateBy = objMaster.CreateBy;
                            objYarnDetail.UpdateOn = DateTime.Now;
                            objYarnDetail.UpdatePc = HostService.GetIP();
                            objYarnDetail.Amount = yrn.Amount;
                            objYarnDetail.LastPurchasePrice = yrn.LastPurchasePrice;

                            lstCostYarn.Add(objYarnDetail);
                        }

                        //  for detail/total 

                        List<PrdPreCostingDetail> lstCostDetail = new List<PrdPreCostingDetail>();

                        foreach (PrdPreCostingDetail csd in lstDetail)
                        {
                            PrdPreCostingDetail objcstDetail = (from qszdetl in _ctxCmn.PrdPreCostingDetails.Where(m => m.CostingDetailID == csd.CostingDetailID) select qszdetl).FirstOrDefault();

                            objcstDetail.UpdateBy = objMaster.CreateBy;
                            objcstDetail.UpdateOn = DateTime.Now;
                            objcstDetail.UpdatePc = HostService.GetIP();
                            objcstDetail.DyingCost = csd.DyingCost;
                            objcstDetail.FinishingCost = csd.FinishingCost;
                            objcstDetail.OverHeadCost = csd.OverHeadCost;
                            objcstDetail.SizeCost = csd.SizeCost;
                            objcstDetail.UnitPrice = csd.UnitPrice;
                            objcstDetail.YarnCost = csd.YarnCost;

                            lstCostDetail.Add(objcstDetail);
                        }

                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = objMaster.CostingNo.ToString();
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        //...........START  new maxId...............//
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingMaster"));

                        // for dying 

                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextDyingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingDying"));
                        FirstDigit = Convert.ToInt64(nextDyingID.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDyingID.ToString().Substring(1, nextDyingID.ToString().Length - 1));


                        // for Sizing 

                        long FirstDigitSizing = 0;
                        long OtherDigitsSizing = 0;
                        long nextSizingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingSize"));
                        FirstDigitSizing = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsSizing = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));


                        // for finish 

                        long FirstDigitFinishing = 0;
                        long OtherDigitsFinishing = 0;
                        long nextFinishingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingFinishing"));
                        FirstDigitFinishing = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsFinishing = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));


                        // for yarn 

                        long FirstDigitYarn = 0;
                        long OtherDigitsYarn = 0;
                        long nextYarnID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingYarn"));
                        FirstDigitYarn = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsYarn = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));


                        // for detail/total 

                        long FirstDigitDetail = 0;
                        long OtherDigitsDetail = 0;
                        long nextDetailID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdPreCostingDetail"));
                        FirstDigitDetail = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsDetail = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));



                        //..........END new  maxId....................//


                        //......... START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, objMaster.CostingDate ?? DateTime.Now, objMaster.CompanyID, 1, 1);

                        if (CustomNo != "")
                        {
                            customCode = CustomNo;
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }

                        //.........END for custom code............ //

                        string newCostingNo = customCode;
                        objMaster.CostingID = NextId;
                        objMaster.CreateOn = DateTime.Now;
                        objMaster.CreatePc = HostService.GetIP();
                        objMaster.CostingNo = newCostingNo;
                        objMaster.IsDeleted = false;


                        // For Dying

                        List<PrdPreCostingDying> lstDyingDetail = new List<PrdPreCostingDying>();
                        foreach (PrdPreCostingDying sdtl in lstDying)
                        {
                            PrdPreCostingDying objchDetail = new PrdPreCostingDying();
                            objchDetail.CostingDyingID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objchDetail.CostingID = NextId;
                            objchDetail.Qty = sdtl.Qty;
                            objchDetail.DyingChemicalID = sdtl.DyingChemicalID;
                            objchDetail.IsDeleted = false;
                            objchDetail.UnitID = sdtl.UnitID;
                            objchDetail.Amount = sdtl.Amount;
                            objchDetail.LastPurchasePrice = sdtl.LastPurchasePrice;

                            objchDetail.CreateBy = objMaster.CreateBy;
                            objchDetail.CompanyID = objMaster.CompanyID;
                            objchDetail.CreateOn = DateTime.Now;
                            objchDetail.CreatePc = HostService.GetIP();
                            lstDyingDetail.Add(objchDetail);
                            OtherDigits++;

                        }

                        // For Sizing

                        List<PrdPreCostingSize> lstSizeDetail = new List<PrdPreCostingSize>();
                        foreach (PrdPreCostingSize sze in lstSize)
                        {
                            PrdPreCostingSize objSiDetail = new PrdPreCostingSize();
                            objSiDetail.CostingSizeID = Convert.ToInt64(FirstDigitSizing + "" + OtherDigitsSizing);
                            objSiDetail.CostingID = NextId;
                            objSiDetail.Qty = sze.Qty;
                            objSiDetail.IsDeleted = false;
                            objSiDetail.UnitID = sze.UnitID;
                            objSiDetail.SizeChemicalID = sze.SizeChemicalID;
                            objSiDetail.Amount = sze.Amount;
                            objSiDetail.LastPurchasePrice = sze.LastPurchasePrice;

                            objSiDetail.CreateBy = objMaster.CreateBy;
                            objSiDetail.CompanyID = objMaster.CompanyID;
                            objSiDetail.CreateOn = DateTime.Now;
                            objSiDetail.CreatePc = HostService.GetIP();
                            lstSizeDetail.Add(objSiDetail);
                            OtherDigitsSizing++;
                        }

                        // For Finishing

                        List<PrdPreCostingFinishing> lstFinishDetail = new List<PrdPreCostingFinishing>();
                        foreach (PrdPreCostingFinishing fns in lstFinish)
                        {
                            PrdPreCostingFinishing objFnsDetail = new PrdPreCostingFinishing();
                            objFnsDetail.CostingFinishID = Convert.ToInt64(FirstDigitFinishing + "" + OtherDigitsFinishing);
                            objFnsDetail.CostingID = NextId;
                            objFnsDetail.FinishingProcessID = fns.FinishingProcessID;
                            objFnsDetail.IsDeleted = false;
                            objFnsDetail.UnitPrice = fns.UnitPrice;

                            objFnsDetail.CreateBy = objMaster.CreateBy;
                            objFnsDetail.CompanyID = objMaster.CompanyID;
                            objFnsDetail.CreateOn = DateTime.Now;
                            objFnsDetail.CreatePc = HostService.GetIP();
                            lstFinishDetail.Add(objFnsDetail);
                            OtherDigitsFinishing++;
                        }

                        // For Yarn

                        List<PrdPreCostingYarn> lstYarnDetail = new List<PrdPreCostingYarn>();
                        foreach (PrdPreCostingYarn yrn in lstYarn)
                        {
                            PrdPreCostingYarn objYrnDetail = new PrdPreCostingYarn();
                            objYrnDetail.CostingYarnID = Convert.ToInt64(FirstDigitYarn + "" + OtherDigitsYarn);
                            objYrnDetail.CostingID = NextId;
                            objYrnDetail.Qty = yrn.Qty;
                            objYrnDetail.IsDeleted = false;
                            objYrnDetail.UnitID = yrn.UnitID;
                            objYrnDetail.LotID = yrn.LotID;
                            objYrnDetail.Amount = yrn.Amount;
                            objYrnDetail.LastPurchasePrice = yrn.LastPurchasePrice;
                            objYrnDetail.ConsumptionTypeID = yrn.ConsumptionTypeID;
                            objYrnDetail.YarnID = yrn.YarnID;

                            objYrnDetail.CreateBy = objMaster.CreateBy;
                            objYrnDetail.CompanyID = objMaster.CompanyID;
                            objYrnDetail.CreateOn = DateTime.Now;
                            objYrnDetail.CreatePc = HostService.GetIP();
                            lstYarnDetail.Add(objYrnDetail);
                            OtherDigitsYarn++;
                        }


                        // For detail/total

                        List<PrdPreCostingDetail> lstDtl = new List<PrdPreCostingDetail>();
                        foreach (PrdPreCostingDetail dtl in lstDetail)
                        {
                            PrdPreCostingDetail objDetail = new PrdPreCostingDetail();
                            objDetail.CostingDetailID = Convert.ToInt64(FirstDigitDetail + "" + OtherDigitsDetail);
                            objDetail.CostingID = NextId;
                            objDetail.DyingCost = dtl.DyingCost;
                            objDetail.IsDeleted = false;
                            objDetail.FinishingCost = dtl.FinishingCost;
                            objDetail.OverHeadCost = dtl.OverHeadCost;
                            objDetail.SizeCost = dtl.SizeCost;
                            objDetail.UnitPrice = dtl.UnitPrice;
                            objDetail.YarnCost = dtl.YarnCost;

                            objDetail.CreateBy = objMaster.CreateBy;
                            objDetail.CompanyID = objMaster.CompanyID;
                            objDetail.CreateOn = DateTime.Now;
                            objDetail.CreatePc = HostService.GetIP();
                            lstDtl.Add(objDetail);
                            OtherDigitsDetail++;
                        }

                        _ctxCmn.PrdPreCostingMasters.Add(objMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, objMaster.CostingDate ?? DateTime.Now, objMaster.CompanyID, 1, 1);

                        _ctxCmn.PrdPreCostingDyings.AddRange(lstDyingDetail);
                        _ctxCmn.PrdPreCostingSizes.AddRange(lstSizeDetail);
                        _ctxCmn.PrdPreCostingFinishings.AddRange(lstFinishDetail);
                        _ctxCmn.PrdPreCostingYarns.AddRange(lstYarnDetail);
                        _ctxCmn.PrdPreCostingDetails.AddRange(lstDtl);


                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingDying", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingSize", Convert.ToInt64(FirstDigitSizing + "" + (OtherDigitsSizing - 1)));
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingFinishing", Convert.ToInt64(FirstDigitFinishing + "" + (OtherDigitsFinishing - 1)));
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingYarn", Convert.ToInt64(FirstDigitYarn + "" + (OtherDigitsYarn - 1)));
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdPreCostingDetail", Convert.ToInt64(FirstDigitDetail + "" + (OtherDigitsDetail - 1)));

                        _ctxCmn.SaveChanges();

                        transaction.Complete();

                        result = newCostingNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                    }
                }

            }
            return result;
        }

        public string DeletePreCosting(vmCmnParameters objcmnParam, Int64 costingID)
        {
            _ctxCmn = new ERP_Entities();
            string result = "";
            if (costingID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        // Int64 bomID = objPrdBOMMaster.BOMID;
                        IEnumerable<PrdPreCostingMaster> lstPrdPreCostingMaster = (from qcm in _ctxCmn.PrdPreCostingMasters.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select qcm).ToList();
                        PrdPreCostingMaster objPrdPreCostingMaster = new PrdPreCostingMaster();
                        foreach (PrdPreCostingMaster bms in lstPrdPreCostingMaster) 
                        {
                            bms.DeleteBy = objcmnParam.loggeduser;
                            bms.DeleteOn = DateTime.Now;
                            bms.DeletePc = HostService.GetIP();
                            bms.IsDeleted = true;
                            objPrdPreCostingMaster = bms;
                        }

                        // dyng
                        List<PrdPreCostingDying> lstPrdPreCostingDying = (from alD in _ctxCmn.PrdPreCostingDyings.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select alD).ToList();
                        List<PrdPreCostingDying> lstDying = new List<PrdPreCostingDying>();
                        foreach (PrdPreCostingDying qcdt in lstPrdPreCostingDying)
                        {
                            PrdPreCostingDying objDyingDetail = (from qcdetl in _ctxCmn.PrdPreCostingDyings.Where(m => m.CostingDyingID == qcdt.CostingDyingID) select qcdetl).FirstOrDefault();

                            objDyingDetail.DeleteBy = objcmnParam.loggeduser;
                            objDyingDetail.DeleteOn = DateTime.Now;
                            objDyingDetail.DeletePc = HostService.GetIP();
                            objDyingDetail.IsDeleted = true;
                            lstDying.Add(objDyingDetail);
                        }

                        // size

                        List<PrdPreCostingSize> lstPrdPreCostingSize = (from als in _ctxCmn.PrdPreCostingSizes.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select als).ToList();
                        List<PrdPreCostingSize> lstSize = new List<PrdPreCostingSize>();
                        foreach (PrdPreCostingSize qsiz in lstPrdPreCostingSize)
                        {
                            PrdPreCostingSize objSizeDetail = (from qszdetl in _ctxCmn.PrdPreCostingSizes.Where(m => m.CostingSizeID == qsiz.CostingSizeID) select qszdetl).FirstOrDefault();

                            objSizeDetail.DeleteBy = objcmnParam.loggeduser;
                            objSizeDetail.DeleteOn = DateTime.Now;
                            objSizeDetail.DeletePc = HostService.GetIP();
                            objSizeDetail.IsDeleted = true;
                            lstSize.Add(objSizeDetail);
                        }

                        // finish

                        List<PrdPreCostingFinishing> lstPrdPreCostingFinishing = (from fn in _ctxCmn.PrdPreCostingFinishings.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select fn).ToList();
                        List<PrdPreCostingFinishing> lstFinish = new List<PrdPreCostingFinishing>();
                        foreach (PrdPreCostingFinishing qfin in lstPrdPreCostingFinishing)
                        {
                            PrdPreCostingFinishing objFinishDetail = (from qfnsdetl in _ctxCmn.PrdPreCostingFinishings.Where(m => m.CostingFinishID == qfin.CostingFinishID) select qfnsdetl).FirstOrDefault();

                            objFinishDetail.DeleteBy = objcmnParam.loggeduser;
                            objFinishDetail.DeleteOn = DateTime.Now;
                            objFinishDetail.DeletePc = HostService.GetIP();
                            objFinishDetail.IsDeleted = true;
                            lstFinish.Add(objFinishDetail);
                        }

                        // yarn

                        List<PrdPreCostingYarn> lstPrdPreCostingYarn = (from als in _ctxCmn.PrdPreCostingYarns.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select als).ToList();
                        List<PrdPreCostingYarn> lstYarn = new List<PrdPreCostingYarn>();
                        foreach (PrdPreCostingYarn qyrn in lstPrdPreCostingYarn)
                        {
                            PrdPreCostingYarn objYarnDetail = (from qyrndetl in _ctxCmn.PrdPreCostingYarns.Where(m => m.CostingYarnID == qyrn.CostingYarnID) select qyrndetl).FirstOrDefault();

                            objYarnDetail.DeleteBy = objcmnParam.loggeduser;
                            objYarnDetail.DeleteOn = DateTime.Now;
                            objYarnDetail.DeletePc = HostService.GetIP();
                            objYarnDetail.IsDeleted = true;
                            lstYarn.Add(objYarnDetail);
                        }

                        // detail/total

                        List<PrdPreCostingDetail> lstPrdPreCostingDetail = (from dtl in _ctxCmn.PrdPreCostingDetails.Where(m => m.CostingID == costingID && m.CompanyID == objcmnParam.loggedCompany) select dtl).ToList();
                        List<PrdPreCostingDetail> lstDetail = new List<PrdPreCostingDetail>();
                        foreach (PrdPreCostingDetail qdet in lstPrdPreCostingDetail)
                        {
                            PrdPreCostingDetail objDetail = (from detl in _ctxCmn.PrdPreCostingDetails.Where(m => m.CostingDetailID == qdet.CostingDetailID) select detl).FirstOrDefault();

                            objDetail.DeleteBy = objcmnParam.loggeduser;
                            objDetail.DeleteOn = DateTime.Now;
                            objDetail.DeletePc = HostService.GetIP();
                            objDetail.IsDeleted = true;
                            lstDetail.Add(objDetail);
                        } 

                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = lstPrdPreCostingMaster.FirstOrDefault().CostingNo.ToString();
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }
            }
            return result;
        }
    }
}
