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
    public class BillOfMaterialMgt : iBillOfMaterialMgt
    {

        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_CmnUser = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;

  
        public IEnumerable<vmBOM> GetItemDetailFDying(vmCmnParameters objcmnParam, Int32 itemTypeID, Int32 itemGroupID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBOM = (from item in _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == itemTypeID && m.ItemGroupID == itemGroupID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.UOMID!=null)

                              //join unit in _ctxCmn.CmnUOMs on item.UOMID equals unit.UOMID
                              //into unt
                              //from yUnt in unt

                              select new
                              {
                                  ItemID = item.ItemID,
                                  ItemName = item.ItemName,
                                  BOMDyingID = 0,
                                  Qty = 0.00m,
                                  DyingChemicalID = item.ItemID,
                                  UnitID = item.CmnUOM.UOMID,
                                  //ArticleNo = item.ArticleNo,
                                  //CuttableWidth = item.CuttableWidth,
                                  //WeightPerUnit = item.WeightPerUnit,
                                  UOMName = item.CmnUOM.UOMName,
                                  CompanyID = item.CompanyID,
                                  IsDeleted = item.IsDeleted

                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ItemName = x.ItemName,
                                        //ArticleNo = x.ArticleNo,
                                        //CuttableWidth = x.CuttableWidth,
                                        //WeightPerUnit = x.WeightPerUnit,
                                        UOMName = x.UOMName,
                                        BOMDyingID = x.BOMDyingID,
                                        Qty = x.Qty,
                                        DyingChemicalID = x.DyingChemicalID,
                                        UnitID = x.UnitID,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted
                                    }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;

        }
        public IEnumerable<vmBOM> GetItemDetailFSizing(vmCmnParameters objcmnParam, Int32 itemTypeID, Int32 itemGroupID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBOM = (from item in _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == itemTypeID && m.ItemGroupID == itemGroupID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.UOMID != null).ToList()

                              //join unit in _ctxCmn.CmnUOMs on item.UOMID equals unit.UOMID
                              //into unt
                              //from yUnt in unt

                              select new
                              {
                                  ItemID = item.ItemID,
                                  ItemName = item.ItemName,
                                  //ArticleNo = item.ArticleNo,
                                  //CuttableWidth = item.CuttableWidth,
                                  //WeightPerUnit = item.WeightPerUnit,
                                  UOMName = item.CmnUOM.UOMName,
                                  BOMSizeID = 0,
                                  SizeChemicalID = item.ItemID,
                                  Qty = 0,
                                  UnitID = item.CmnUOM.UOMID,
                                  CompanyID = item.CompanyID,
                                  IsDeleted = item.IsDeleted


                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ItemName = x.ItemName,
                                        //ArticleNo = x.ArticleNo,
                                        //CuttableWidth = x.CuttableWidth,
                                        //WeightPerUnit = x.WeightPerUnit,
                                        UOMName = x.UOMName,
                                        BOMSizeID = x.BOMSizeID,
                                        SizeChemicalID = x.SizeChemicalID,
                                        Qty = x.Qty,
                                        UnitID = x.UnitID,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted

                                    }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;
        }

        public IEnumerable<vmBOM> GetItemDetailByItemID(vmCmnParameters objcmnParam, Int64 itemID, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
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
                                  ColorName = item.CmnItemColor.ColorName, //yClr.ColorName,//xColor.ColorName,//color.ColorName,
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
            }
            return lstBOM;
        }

        public IEnumerable<object> GetDetailListByBOMID(vmCmnParameters objcmnParam, vmCmnParameters objcmnParam1, Int64 bomID, out int recordsTotal)
        {
            IEnumerable<object> lstBOM = null;
            _ctxCmn = new ERP_Entities();
            recordsTotal = 0;
            //using (_ctxCmn = new ERP_Entities())
            //{
            try
            {
                lstBOM = (from bom in _ctxCmn.PrdBOMMasters.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                         // join item in _ctxCmn.CmnItemMasters on bom.ItemID equals item.ItemID

                          //join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID
                          //into clr
                          //from yClr in clr.DefaultIfEmpty()

                          join warp in _ctxCmn.RndYarnCRs on  bom.CmnItemMaster.WarpYarnID equals warp.YarnID
                          into warpYarn
                          from yWarpYarn in warpYarn.DefaultIfEmpty()
                          join weft in _ctxCmn.RndYarnCRs on bom.CmnItemMaster.WeftYarnID equals weft.YarnID
                          into weftYarn
                          from xWeftYarn in weftYarn.DefaultIfEmpty()
                          select new
                          {
                              ItemID = bom.CmnItemMaster.ItemID,
                              ItemName = bom.CmnItemMaster.ItemName,
                              ArticleNo = bom.CmnItemMaster.ArticleNo,
                              CuttableWidth = bom.CmnItemMaster.CuttableWidth,
                              WeightPerUnit = bom.CmnItemMaster.WeightPerUnit,
                              ColorName = bom.CmnItemMaster.CmnItemColor.ColorName, //yClr.ColorName,
                              Construction = bom.CmnItemMaster.Note,
                              WarpYarn = yWarpYarn.YarnCount,
                              WeftYarn = xWeftYarn.YarnCount,
                              FinishingWidth = bom.CmnItemMaster.FinishingWidth,
                              BOMID = bom.BOMID,
                              BOMNO = bom.BOMNO,
                              BOMDate = bom.BOMDate,
                              Description = bom.Description,


                              ForDying = (from dCmnItm in _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == objcmnParam.ItemType && m.ItemGroupID == objcmnParam.ItemGroup && m.IsDeleted == false && m.CompanyID == objcmnParam.loggedCompany && m.UOMID != null)


                                            join dt in _ctxCmn.PrdBOMDyings.Where(m=>m.BOMID==bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false) on dCmnItm.ItemID equals dt.DyingChemicalID //dying

                                            into dts from ydt in dts.DefaultIfEmpty()

                                           //join itemDyng in _ctxCmn.CmnItemMasters on dt.DyingChemicalID equals itemDyng.ItemID
                                           //join unitDyng in _ctxCmn.CmnUOMs on dt.UnitID equals unitDyng.UOMID
                                           //into unitDyng
                                           //from yUnitDyng in unitDyng.DefaultIfEmpty()
                                           select new
                                           {
                                               ItemID = dCmnItm.ItemID,
                                               ItemName = dCmnItm.ItemName,
                                               BOMDyingID = ydt.BOMDyingID == null ? 0 : ydt.BOMDyingID,
                                               Qty = ydt.Qty == null ? 0.00m : ydt.Qty,
                                               DyingChemicalID = dCmnItm.ItemID,
                                               UnitID = dCmnItm.UOMID,
                                               UOMName = dCmnItm.CmnUOM.UOMName,
                                               CompanyID = dCmnItm.CompanyID,
                                               IsDeleted = dCmnItm.IsDeleted,
                                               BOMID=ydt.BOMID
                                            }),

                   
                              ForSizing = (from dCmnItmSiz in _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == objcmnParam1.ItemType && m.ItemGroupID == objcmnParam1.ItemGroup && m.IsDeleted == false && m.CompanyID == objcmnParam.loggedCompany && m.UOMID != null)

                                           join dtsiz in _ctxCmn.PrdBOMSizes.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false) on dCmnItmSiz.ItemID equals dtsiz.SizeChemicalID //Sizing 
                                           into dtsizes
                                           from ydtsiz in dtsizes.DefaultIfEmpty()

                                           //  where ydtsiz.BOMID == bomID && ydtsiz.CompanyID == objcmnParam.loggedCompany && ydtsiz.IsDeleted == false

                                           //join itemSize in _ctxCmn.CmnItemMasters on dtsiz.SizeChemicalID equals itemSize.ItemID

                                           //join unitSize in _ctxCmn.CmnUOMs on dtsiz.UnitID equals unitSize.UOMID
                                           //into unitSize
                                           //from yUnitSize in unitSize

                                           select new
                                           {
                                               ItemID = dCmnItmSiz.ItemID,
                                               ItemName = dCmnItmSiz.ItemName,
                                               Qty = ydtsiz.Qty == null ? 0.00m : ydtsiz.Qty,
                                               SizeChemicalID = dCmnItmSiz.ItemID,
                                               UnitID = dCmnItmSiz.UOMID == null ? 0 : dCmnItmSiz.UOMID,
                                               UOMName = dCmnItmSiz.CmnUOM.UOMName == null ? "" : dCmnItmSiz.CmnUOM.UOMName,
                                               CompanyID = dCmnItmSiz.CompanyID,
                                               IsDeleted = dCmnItmSiz.IsDeleted,
                                               BOMSizeID = ydtsiz.BOMSizeID == null ? 0 : ydtsiz.BOMSizeID,
                                               BOMID = ydtsiz.BOMID
                                           })
                              
                              
                              //GetSizingData(objcmnParam1, bomID),
                              

                          }).ToList();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            //   }
            return lstBOM;

        }

        private List<vmBOM> GetSizingData(vmCmnParameters objcmnParam1, long bomID)
        {
            List<vmBOM> lstAllSizing = new List<vmBOM>();
            _ctxCmn = new ERP_Entities();
            try
            {
                IEnumerable<vmBOM> lstItem = (from itm in _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID==objcmnParam1.ItemType && m.ItemGroupID==objcmnParam1.ItemGroup && m.UOMID != null && m.CompanyID == objcmnParam1.loggedCompany && m.IsDeleted == false)
                                              select new
                                              {
                                                  ItemID = itm.ItemID,
                                                  ItemName = itm.ItemName,
                                                  UnitID = itm.UOMID,
                                                  UOMName = itm.CmnUOM.UOMName,
                                                  CompanyID = itm.CompanyID,
                                                  IsDeleted = itm.IsDeleted,
                                                  BOMSizeID = 0, 
                                                  Qty = 0.00m,
                                                  SizeChemicalID = itm.ItemID 
                                              })
                                              .Select(m => new vmBOM
                                              {

                                                  ItemID = m.ItemID,
                                                  ItemName = m.ItemName,
                                                  UnitID = m.UnitID,
                                                  UOMName = m.UOMName,
                                                  CompanyID = m.CompanyID,
                                                  IsDeleted = m.IsDeleted,
                                                  BOMSizeID = m.BOMSizeID,
                                                  Qty = m.Qty,
                                                  SizeChemicalID = m.SizeChemicalID

                                              }).ToList();



                IEnumerable<vmBOM> lstSz = (from dn in _ctxCmn.PrdBOMSizes.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam1.loggedCompany && m.IsDeleted == false)
                                             select new
                                             {
                                                 ItemID = 0,
                                                 ItemName = "",
                                                 UnitID = 0,
                                                 UOMName = "",
                                                 CompanyID = dn.CompanyID,
                                                 IsDeleted = dn.IsDeleted,
                                                 BOMSizeID = dn.BOMSizeID,
                                                 Qty = dn.Qty,
                                                 SizeChemicalID = dn.SizeChemicalID
                                             })
                                              .Select(m => new vmBOM
                                              {

                                                  ItemID = m.ItemID,
                                                  ItemName = m.ItemName,
                                                  UnitID = m.UnitID,
                                                  UOMName = m.UOMName,
                                                  CompanyID = m.CompanyID,
                                                  IsDeleted = m.IsDeleted,
                                                  BOMSizeID = m.BOMSizeID,
                                                  Qty = m.Qty,
                                                  SizeChemicalID = m.SizeChemicalID

                                              }).ToList();




                foreach (vmBOM bm in lstItem)
                {
                    vmBOM newList = (from dt in lstSz.Where(m => m.SizeChemicalID == bm.ItemID) select dt).FirstOrDefault();
                    if (newList != null)
                    {
                        bm.Qty = newList.Qty;
                        bm.BOMDyingID = newList.BOMDyingID;
                    }
                    lstAllSizing.Add(bm);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }


            return lstAllSizing;
        }

        private List<vmBOM> GetDyingData(vmCmnParameters objcmnParam, long bomID)
        {
            List<vmBOM> lstAllDying = new List<vmBOM>();
            _ctxCmn = new ERP_Entities();
            try
            {
                IEnumerable<vmBOM> lstItem = (from itm in _ctxCmn.CmnItemMasters.Where(m => m.UOMID != null && m.ItemTypeID==objcmnParam.ItemType && m.ItemGroupID==objcmnParam.ItemGroup && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)
                                              select new { ItemID = itm.ItemID, 
                                                           ItemName = itm.ItemName, 
                                                           UnitID = itm.UOMID,
                                                           UOMName = itm.CmnUOM.UOMName, 
                                                           CompanyID = itm.CompanyID, 
                                                           IsDeleted = itm.IsDeleted,
                                                           BOMDyingID = 0,
                                                           Qty = 0.00m,
                                                           DyingChemicalID = itm.ItemID
                                              })
                                              .Select(m => new vmBOM
                                                {

                                                    ItemID = m.ItemID,
                                                    ItemName = m.ItemName,
                                                    UnitID = m.UnitID,
                                                    UOMName = m.UOMName,
                                                    CompanyID = m.CompanyID,
                                                    IsDeleted = m.IsDeleted,
                                                    BOMDyingID = m.BOMDyingID,
                                                    Qty = m.Qty,
                                                    DyingChemicalID = m.DyingChemicalID

                                                }).ToList();



                IEnumerable<vmBOM> lstDng = (from dn in _ctxCmn.PrdBOMDyings.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)
                                              select new
                                              {
                                                  ItemID = 0,
                                                  ItemName = "",
                                                  UnitID = 0,
                                                  UOMName = "",
                                                  CompanyID = dn.CompanyID,
                                                  IsDeleted = dn.IsDeleted,
                                                  BOMDyingID = dn.BOMDyingID,
                                                  Qty = dn.Qty,
                                                  DyingChemicalID = dn.DyingChemicalID
                                              })
                                              .Select(m => new vmBOM
                                              {

                                                  ItemID = m.ItemID,
                                                  ItemName = m.ItemName,
                                                  UnitID = m.UnitID,
                                                  UOMName = m.UOMName,
                                                  CompanyID = m.CompanyID,
                                                  IsDeleted = m.IsDeleted,
                                                  BOMDyingID = m.BOMDyingID,
                                                  Qty = m.Qty,
                                                  DyingChemicalID = m.DyingChemicalID

                                              }).ToList();


             

                foreach (vmBOM bm in lstItem )
                {
                    vmBOM newList = new vmBOM();
                    newList = (from dt in lstDng.Where(m => m.DyingChemicalID == bm.ItemID) select dt).FirstOrDefault();
                    if (newList!=null)
                    {
                       bm.Qty = newList.Qty;
                       bm.BOMDyingID = newList.BOMDyingID;
                    } 
                    lstAllDying.Add(bm); 
                } 
            }
            catch (Exception e)
            {
                e.ToString();
            }


            return lstAllDying;
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
                                  ProcessCode = fp.FinishingProcessName

                              })
                                    .Select(x => new vmBOM
                                    {
                                        ItemID = x.ItemID,
                                        ProcessFullName = x.ProcessFullName,
                                        ProcessCode = x.ProcessCode
                                    }).ToList();


                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;
        }

        public IEnumerable<vmBOM> GetBOMMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBOM> lstBOM = null;
            IEnumerable<vmBOM> lstBOMWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstBOMWithoutPaging = (from bm in _ctxCmn.PrdBOMMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList()

                                         //  join item in _ctxCmn.CmnItemMasters on bm.ItemID equals item.ItemID

                                           select new
                                           {
                                               BOMID = bm.BOMID,
                                               BOMNO = bm.BOMNO,
                                               ItemID = bm.ItemID,
                                               BOMDate = bm.BOMDate,
                                               CompanyID = bm.CompanyID,
                                               Description = bm.Description,
                                               ItemName = bm.CmnItemMaster.ItemName

                                           }).ToList().Select(x => new vmBOM
                                           {
                                               BOMID = x.BOMID,
                                               BOMNO = x.BOMNO,
                                               ItemID = x.ItemID,
                                               BOMDate = x.BOMDate,
                                               CompanyID = x.CompanyID,
                                               Description = x.Description,
                                               ItemName = x.ItemName
                                           }).ToList();

                    lstBOM = lstBOMWithoutPaging.OrderByDescending(x => x.BOMID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstBOMWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstBOM;
        }

        public string SaveNUpdateBOM(PrdBOMMaster objPrdBOMMaster, List<PrdBOMDying> lstPrdBOMDying, List<PrdBOMSize> lstPrdBOMSize, int menuID)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (objPrdBOMMaster.BOMID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 bomID = objPrdBOMMaster.BOMID;
                        IEnumerable<PrdBOMMaster> lstPrdBOMMaster = (from qcm in _ctxCmn.PrdBOMMasters.Where(m => m.BOMID == bomID && m.CompanyID == objPrdBOMMaster.CompanyID) select qcm).ToList();
                        PrdBOMMaster objBOMMaster = new PrdBOMMaster();
                        foreach (PrdBOMMaster bms in lstPrdBOMMaster)
                        {
                            bms.UpdateBy = objPrdBOMMaster.CreateBy;
                            bms.UpdateOn = DateTime.Now;
                            bms.UpdatePc = HostService.GetIP();
                            bms.BOMDate = objPrdBOMMaster.BOMDate;
                            bms.Description = objPrdBOMMaster.Description;
                            objBOMMaster = bms;
                        }

                        List<PrdBOMDying> lstBOMDying = new List<PrdBOMDying>();

                        List<PrdBOMDying> lstBOMDyingNewAdd = new List<PrdBOMDying>();
                        // for dying 

                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextDyingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdBOMDying"));
                        FirstDigit = Convert.ToInt64(nextDyingID.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDyingID.ToString().Substring(1, nextDyingID.ToString().Length - 1));



                        foreach (PrdBOMDying qcdt in lstPrdBOMDying)
                        {
                            if (qcdt.BOMDyingID > 0)
                            {
                                PrdBOMDying objDyingDetail = (from qcdetl in _ctxCmn.PrdBOMDyings.Where(m => m.BOMDyingID == qcdt.BOMDyingID) select qcdetl).FirstOrDefault();

                                objDyingDetail.UpdateBy = objPrdBOMMaster.CreateBy;
                                objDyingDetail.UpdateOn = DateTime.Now;
                                objDyingDetail.UpdatePc = HostService.GetIP();
                                objDyingDetail.Qty = qcdt.Qty;
                                lstBOMDying.Add(objDyingDetail);

                            }

                            //end  for  new item added when update  dying 

                            else if (qcdt.BOMDyingID == 0)
                            {
                                PrdBOMDying objDyingDetailNewAdded = new PrdBOMDying();

                                objDyingDetailNewAdded.BOMDyingID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                objDyingDetailNewAdded.BOMID = bomID;// qcdt.BOMID;
                                objDyingDetailNewAdded.Qty = qcdt.Qty;
                                objDyingDetailNewAdded.DyingChemicalID = qcdt.DyingChemicalID;
                                objDyingDetailNewAdded.IsDeleted = false;
                                objDyingDetailNewAdded.UnitID = qcdt.UnitID;
                                objDyingDetailNewAdded.CreateBy = objPrdBOMMaster.CreateBy;
                                objDyingDetailNewAdded.CompanyID = objPrdBOMMaster.CompanyID;
                                objDyingDetailNewAdded.CreateOn = DateTime.Now;
                                objDyingDetailNewAdded.CreatePc = HostService.GetIP();
                                lstBOMDyingNewAdd.Add(objDyingDetailNewAdded);
                                OtherDigits++;
                            }
                            //star  for  new item added when update  dying

                        }


                        List<PrdBOMSize> lstBOMSize = new List<PrdBOMSize>();
                        List<PrdBOMSize> lstBOMSizeNewAdd = new List<PrdBOMSize>();

                        // for Sizing 


                        long FirstDigitSizing = 0;
                        long OtherDigitsSizing = 0;
                        long nextSizingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdBOMSize"));
                        FirstDigitSizing = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsSizing = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));


                        foreach (PrdBOMSize qsiz in lstPrdBOMSize)
                        {
                            if (qsiz.BOMSizeID > 0)
                            {
                                PrdBOMSize objSizeDetail = (from qszdetl in _ctxCmn.PrdBOMSizes.Where(m => m.BOMSizeID == qsiz.BOMSizeID) select qszdetl).FirstOrDefault();

                                objSizeDetail.UpdateBy = objPrdBOMMaster.CreateBy;
                                objSizeDetail.UpdateOn = DateTime.Now;
                                objSizeDetail.UpdatePc = HostService.GetIP();
                                objSizeDetail.Qty = qsiz.Qty;
                                lstBOMSize.Add(objSizeDetail);
                            }

                            //end  for  new item added when update  siz 

                            else if (qsiz.BOMSizeID == 0)
                            {
                                PrdBOMSize objSiDetailNewAdded = new PrdBOMSize();
                                objSiDetailNewAdded.BOMSizeID = Convert.ToInt64(FirstDigitSizing + "" + OtherDigitsSizing);
                                objSiDetailNewAdded.BOMID = bomID; //qsiz.BOMID;
                                objSiDetailNewAdded.Qty = qsiz.Qty;
                                objSiDetailNewAdded.IsDeleted = false;
                                objSiDetailNewAdded.UnitID = qsiz.UnitID;
                                objSiDetailNewAdded.SizeChemicalID = qsiz.SizeChemicalID;
                                objSiDetailNewAdded.CreateBy = objPrdBOMMaster.CreateBy;
                                objSiDetailNewAdded.CompanyID = objPrdBOMMaster.CompanyID;
                                objSiDetailNewAdded.CreateOn = DateTime.Now;
                                objSiDetailNewAdded.CreatePc = HostService.GetIP();
                                lstBOMSizeNewAdd.Add(objSiDetailNewAdded);
                                OtherDigitsSizing++;
                            }
                            //star  for  new item added when update  siz 
                        }

                        //  for new add  when update 

                        if (lstBOMDyingNewAdd.Count > 0)
                        {
                            _ctxCmn.PrdBOMDyings.AddRange(lstBOMDyingNewAdd);
                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("PrdBOMDying", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            _ctxCmn.SaveChanges();
                        }
                        if (lstBOMSizeNewAdd.Count > 0)
                        {
                            _ctxCmn.PrdBOMSizes.AddRange(lstBOMSizeNewAdd);
                            //............Update MaxID.................// 
                            GenericFactory_EF_CmnCombo.updateMaxID("PrdBOMSize", Convert.ToInt64(FirstDigitSizing + "" + (OtherDigitsSizing - 1)));
                            _ctxCmn.SaveChanges();
                        }

                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = objPrdBOMMaster.BOMNO.ToString();
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
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdBOMMaster"));

                        // for dying 

                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextDyingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdBOMDying"));
                        FirstDigit = Convert.ToInt64(nextDyingID.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDyingID.ToString().Substring(1, nextDyingID.ToString().Length - 1));


                        // for Sizing 

                        long FirstDigitSizing = 0;
                        long OtherDigitsSizing = 0;
                        long nextSizingID = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PrdBOMSize"));
                        FirstDigitSizing = Convert.ToInt64(nextSizingID.ToString().Substring(0, 1));
                        OtherDigitsSizing = Convert.ToInt64(nextSizingID.ToString().Substring(1, nextSizingID.ToString().Length - 1));

                        //..........END new  maxId....................//


                        //......... START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, objPrdBOMMaster.BOMDate ?? DateTime.Now, objPrdBOMMaster.CompanyID, 1, 1);

                        if (CustomNo != "")
                        {
                            customCode = CustomNo;
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }

                        //.........END for custom code............ //

                        string newBomNo = customCode;
                        objPrdBOMMaster.BOMID = NextId;
                        objPrdBOMMaster.CreateOn = DateTime.Now;
                        objPrdBOMMaster.CreatePc = HostService.GetIP();
                        objPrdBOMMaster.BOMNO = newBomNo;
                        objPrdBOMMaster.IsDeleted = false;

                        // For Dying

                        List<PrdBOMDying> lstDyingDetail = new List<PrdBOMDying>();
                        foreach (PrdBOMDying sdtl in lstPrdBOMDying)
                        {
                            PrdBOMDying objchDetail = new PrdBOMDying();
                            objchDetail.BOMDyingID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objchDetail.BOMID = NextId;
                            objchDetail.Qty = sdtl.Qty;
                            objchDetail.DyingChemicalID = sdtl.DyingChemicalID;
                            objchDetail.IsDeleted = false;
                            objchDetail.UnitID = sdtl.UnitID;
                            objchDetail.CreateBy = objPrdBOMMaster.CreateBy;
                            objchDetail.CompanyID = objPrdBOMMaster.CompanyID;
                            objchDetail.CreateOn = DateTime.Now;
                            objchDetail.CreatePc = HostService.GetIP();
                            lstDyingDetail.Add(objchDetail);
                            OtherDigits++;

                        }

                        // For Sizing

                        List<PrdBOMSize> lstSizeDetail = new List<PrdBOMSize>();
                        foreach (PrdBOMSize sze in lstPrdBOMSize)
                        {
                            PrdBOMSize objSiDetail = new PrdBOMSize();
                            objSiDetail.BOMSizeID = Convert.ToInt64(FirstDigitSizing + "" + OtherDigitsSizing);
                            objSiDetail.BOMID = NextId;
                            objSiDetail.Qty = sze.Qty;
                            objSiDetail.IsDeleted = false;
                            objSiDetail.UnitID = sze.UnitID;
                            objSiDetail.SizeChemicalID = sze.SizeChemicalID;
                            objSiDetail.CreateBy = objPrdBOMMaster.CreateBy;
                            objSiDetail.CompanyID = objPrdBOMMaster.CompanyID;
                            objSiDetail.CreateOn = DateTime.Now;
                            objSiDetail.CreatePc = HostService.GetIP();
                            lstSizeDetail.Add(objSiDetail);
                            OtherDigitsSizing++;

                        }


                        _ctxCmn.PrdBOMMasters.Add(objPrdBOMMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdBOMMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, objPrdBOMMaster.BOMDate ?? DateTime.Now, objPrdBOMMaster.CompanyID, 1, 1);
                        _ctxCmn.PrdBOMDyings.AddRange(lstDyingDetail);
                        _ctxCmn.PrdBOMSizes.AddRange(lstSizeDetail);
                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdBOMDying", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        GenericFactory_EF_CmnCombo.updateMaxID("PrdBOMSize", Convert.ToInt64(FirstDigitSizing + "" + (OtherDigitsSizing - 1)));
                        _ctxCmn.SaveChanges();

                        transaction.Complete();
                        result = newBomNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                    }
                }

            }
            return result;
        }

        public string DeleteBOM(vmCmnParameters objcmnParam, Int64 bomID)
        {
            _ctxCmn = new ERP_Entities();
            string result = "";
            if (bomID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        // Int64 bomID = objPrdBOMMaster.BOMID;
                        IEnumerable<PrdBOMMaster> lstPrdBOMMaster = (from qcm in _ctxCmn.PrdBOMMasters.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany) select qcm).ToList();
                        PrdBOMMaster objBOMMaster = new PrdBOMMaster();
                        foreach (PrdBOMMaster bms in lstPrdBOMMaster)
                        {
                            bms.DeleteBy = objcmnParam.loggeduser;
                            bms.DeleteOn = DateTime.Now;
                            bms.DeletePc = HostService.GetIP();
                            bms.IsDeleted = true;
                            objBOMMaster = bms;
                        }

                        List<PrdBOMDying> lstPrdBOMDying = (from alD in _ctxCmn.PrdBOMDyings.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany) select alD).ToList();
                        List<PrdBOMDying> lstBOMDying = new List<PrdBOMDying>();
                        foreach (PrdBOMDying qcdt in lstPrdBOMDying)
                        {
                            PrdBOMDying objDyingDetail = (from qcdetl in _ctxCmn.PrdBOMDyings.Where(m => m.BOMDyingID == qcdt.BOMDyingID) select qcdetl).FirstOrDefault();

                            objDyingDetail.DeleteBy = objcmnParam.loggeduser;
                            objDyingDetail.DeleteOn = DateTime.Now;
                            objDyingDetail.DeletePc = HostService.GetIP();
                            objDyingDetail.IsDeleted = true;
                            lstBOMDying.Add(objDyingDetail);
                        }

                        List<PrdBOMSize> lstPrdBOMSize = (from als in _ctxCmn.PrdBOMSizes.Where(m => m.BOMID == bomID && m.CompanyID == objcmnParam.loggedCompany) select als).ToList();
                        List<PrdBOMSize> lstBOMSize = new List<PrdBOMSize>();
                        foreach (PrdBOMSize qsiz in lstPrdBOMSize)
                        {
                            PrdBOMSize objSizeDetail = (from qszdetl in _ctxCmn.PrdBOMSizes.Where(m => m.BOMSizeID == qsiz.BOMSizeID) select qszdetl).FirstOrDefault();

                            objSizeDetail.DeleteBy = objcmnParam.loggeduser;
                            objSizeDetail.DeleteOn = DateTime.Now;
                            objSizeDetail.DeletePc = HostService.GetIP();
                            objSizeDetail.IsDeleted = true;
                            lstBOMSize.Add(objSizeDetail);
                        }

                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = lstPrdBOMMaster.FirstOrDefault().BOMNO.ToString();
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
