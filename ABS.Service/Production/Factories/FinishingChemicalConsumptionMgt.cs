using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Commercial.Interfaces;
using ABS.Service.Sales.Interfaces;
using ABS.Service.AllServiceClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Interfaces;
using ABS.Models.ViewModel.Production;
using ABS.Utility;

namespace ABS.Service.Production.Factories
{
    public class FinishingChemicalConsumptionMgt : iFinishingChemicalConsumptionMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<PrdFinishingConsumptionMaster> GenericFactory_EF_PrdFinishingConsumptionMaster = null;
        private iGenericFactory_EF<PrdFinishingConsumptionDetail> GenericFactory_EF_PrdFinishingConsumptionDetail = null;
        private iGenericFactory<vmChemicalSetupMasterDetail> GenericFactory_vmChemicalSetupMasterDetail_GF = null;

        //public IEnumerable<vmChemicalSetupMasterDetail> GetChecmicalByID(vmCmnParameters objcmnParam)
        //{
        //    IEnumerable<vmChemicalSetupMasterDetail> objChemicalSetup = null;
        //    using (ERP_Entities _ctxCmn = new ERP_Entities())
        //    {
        //        try
        //        {
        //            objChemicalSetup = (from master in _ctxCmn.PrdFinishingChemicalSetups
        //                                join detail in _ctxCmn.PrdFinishingChemicalSetupDetails on master.FinChemicalStupID equals detail.FinChemicalStupID// into leftColorGroup
        //                                join FT in _ctxCmn.PrdFinishingTypes on master.FinishingProcessID equals FT.FinishingProcessID
        //                                join item in _ctxCmn.CmnItemMasters on detail.ChemicalID equals item.ItemID// into leftColorGroup
        //                                join uom in _ctxCmn.CmnUOMs on detail.UnitID equals uom.UOMID
        //                                where (FT.FInishTypeID == objcmnParam.id && master.IsDeleted == false)
        //                                select new vmChemicalSetupMasterDetail
        //                                {
        //                                    FinChemicalStupID = master.FinChemicalStupID,
        //                                    ChemicalID = detail.ChemicalID,
        //                                    MaxQty = detail.MaxQty,
        //                                    MinQty = detail.MinQty,
        //                                    ItemName = item.ItemName,
        //                                    UnitID = uom.UOMID,
        //                                    UOMName = uom.UOMName
        //                                }).Distinct().ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //    }
        //    return objChemicalSetup;
        //}

        public string SaveUpdateChemConsumptionInfo(vmChemicalSetupMasterDetail ChemConsumptionInfo, List<vmChemicalSetupMasterDetail> DCDetailList, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdFinishingConsumptionMaster = new PrdFinishingConsumptionMaster_EF();
                GenericFactory_EF_PrdFinishingConsumptionDetail = new PrdFinishingConsumptionDetail_EF();
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0; string CustomNo = string.Empty, FinishingConsumptionNo = string.Empty;
                //PrdFinishingConsumptionMaster Master = new PrdFinishingConsumptionMaster();
                //List<PrdFinishingConsumptionDetail> Detail = new List<PrdFinishingConsumptionDetail>();                
                PrdFinishingConsumptionMaster Master = new PrdFinishingConsumptionMaster();
                List<PrdFinishingConsumptionDetail> Detail = new List<PrdFinishingConsumptionDetail>();
                vmChemicalSetupMasterDetail item = new vmChemicalSetupMasterDetail();
                try
                {
                    if (ChemConsumptionInfo.FinishingConsumptionID == 0)
                    {
                        MasterId = Convert.ToInt64(GenericFactory_EF_PrdFinishingConsumptionMaster.getMaxID("PrdFinishingConsumptionMaster"));
                        DetailId = Convert.ToInt64(GenericFactory_EF_PrdFinishingConsumptionDetail.getMaxID("PrdFinishingConsumptionDetail"));
                        FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                        CustomNo = GenericFactory_EF_PrdFinishingConsumptionMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, objcmnParam.loggeduser, 1); // 1 for DB ID
                        if (CustomNo != null && CustomNo != "")
                        {
                            FinishingConsumptionNo = CustomNo;
                        }
                        else
                        {
                            FinishingConsumptionNo = MasterId.ToString();
                        }

                        Master = new PrdFinishingConsumptionMaster()
                        {
                            FinishingConsumptionID = MasterId,
                            FinishingConsumptionNo = FinishingConsumptionNo,
                            WeavingMRRID = ChemConsumptionInfo.WeavingMRRID,
                            ItemID = ChemConsumptionInfo.ItemID,
                            FinishingTypeID = ChemConsumptionInfo.FinishingTypeID,
                            Volume = ChemConsumptionInfo.Volume,
                            ConsumptionDate = ChemConsumptionInfo.ConsumptionDate,
                            Remarks = ChemConsumptionInfo.Remarks,
                            UnitID = (int)ChemConsumptionInfo.UnitID,

                            IsDeleted = false,
                            IsReleased = false,
                            TransactionTypeID=objcmnParam.tTypeId,
                            CompanyID = objcmnParam.loggedCompany,
                            CreateOn = DateTime.Now,
                            CreateBy = objcmnParam.loggeduser,
                            CreatePc = HostService.GetIP(),
                        };

                        foreach (vmChemicalSetupMasterDetail sdtl in DCDetailList)
                        {
                            PrdFinishingConsumptionDetail objConsmuptionDetail = new PrdFinishingConsumptionDetail();
                            objConsmuptionDetail.FinishingConsumptionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objConsmuptionDetail.FinishingConsumptionID = MasterId;
                            objConsmuptionDetail.ChemicalID = sdtl.ChemicalID;
                            objConsmuptionDetail.UnitID = sdtl.UnitID;
                            objConsmuptionDetail.RequiredQty = sdtl.RequiredQty;
                            objConsmuptionDetail.AccQty = sdtl.AccQty;
                            objConsmuptionDetail.Amount = sdtl.Amount;
                            objConsmuptionDetail.BatchID = sdtl.BatchID;
                            objConsmuptionDetail.SupplierID = sdtl.SupplierID;
                            objConsmuptionDetail.ConsumptionDate = Master.ConsumptionDate;                            
                            objConsmuptionDetail.UnitPrice = sdtl.UnitPrice;

                            objConsmuptionDetail.IsDeleted = false;
                            objConsmuptionDetail.IsReleased = false;

                            objConsmuptionDetail.DepartmentID = objcmnParam.DepartmentID;
                            objConsmuptionDetail.TransactionTypeID = objcmnParam.tTypeId;
                            objConsmuptionDetail.CompanyID = objcmnParam.loggedCompany;
                            objConsmuptionDetail.CreateOn = DateTime.Now;
                            objConsmuptionDetail.CreateBy = objcmnParam.loggeduser;
                            objConsmuptionDetail.CreatePc = HostService.GetIP();
                            Detail.Add(objConsmuptionDetail);

                            OtherDigits++;
                        }
                    }
                    else
                    {
                        Master = GenericFactory_EF_PrdFinishingConsumptionMaster.FindBy(m => m.FinishingConsumptionID == ChemConsumptionInfo.FinishingConsumptionID).FirstOrDefault();
                        Master.WeavingMRRID = ChemConsumptionInfo.WeavingMRRID;
                        Master.ItemID = ChemConsumptionInfo.ItemID;
                        Master.FinishingTypeID = ChemConsumptionInfo.FinishingTypeID;
                        Master.Volume = ChemConsumptionInfo.Volume; ;
                        Master.ConsumptionDate = ChemConsumptionInfo.ConsumptionDate;
                        Master.Remarks = ChemConsumptionInfo.Remarks;
                        Master.UnitID = (int)ChemConsumptionInfo.UnitID;

                        Master.TransactionTypeID = objcmnParam.tTypeId;
                        Master.CompanyID = objcmnParam.loggedCompany;
                        Master.UpdateOn = DateTime.Now;
                        Master.UpdateBy = objcmnParam.loggeduser;
                        Master.UpdatePc = HostService.GetIP();

                        foreach (vmChemicalSetupMasterDetail lcd in DCDetailList)
                        {
                            PrdFinishingConsumptionDetail FiniConDe = GenericFactory_EF_PrdFinishingConsumptionDetail.FindBy(m => m.FinishingConsumptionDetailID == lcd.FinishingConsumptionDetailID).FirstOrDefault();
                            FiniConDe.ChemicalID = lcd.ChemicalID;
                            FiniConDe.UnitID = lcd.UnitID;
                            FiniConDe.RequiredQty = lcd.RequiredQty;
                            FiniConDe.AccQty = lcd.AccQty;
                            FiniConDe.Amount = lcd.Amount;
                            FiniConDe.BatchID = lcd.BatchID;
                            FiniConDe.SupplierID = lcd.SupplierID;
                            FiniConDe.ConsumptionDate = Master.ConsumptionDate;
                            FiniConDe.UnitPrice = lcd.UnitPrice;

                            FiniConDe.DepartmentID = objcmnParam.DepartmentID;
                            FiniConDe.TransactionTypeID = objcmnParam.tTypeId;
                            FiniConDe.CompanyID = objcmnParam.loggedCompany;
                            FiniConDe.UpdateOn = DateTime.Now;
                            FiniConDe.UpdateBy = objcmnParam.loggeduser;
                            FiniConDe.UpdatePc = HostService.GetIP();
                            Detail.Add(FiniConDe);
                        }
                    }

                    if (ChemConsumptionInfo.FinishingConsumptionID > 0)
                    {
                        GenericFactory_EF_PrdFinishingConsumptionMaster.Update(Master);
                        GenericFactory_EF_PrdFinishingConsumptionMaster.Save();

                        GenericFactory_EF_PrdFinishingConsumptionDetail.UpdateList(Detail);
                        GenericFactory_EF_PrdFinishingConsumptionDetail.Save();
                    }
                    else
                    {
                        GenericFactory_EF_PrdFinishingConsumptionMaster.Insert(Master);
                        GenericFactory_EF_PrdFinishingConsumptionMaster.Save();
                        GenericFactory_EF_PrdFinishingConsumptionMaster.updateMaxID("PrdFinishingConsumptionMaster", Convert.ToInt64(MasterId));
                        GenericFactory_EF_PrdFinishingConsumptionMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);

                        GenericFactory_EF_PrdFinishingConsumptionDetail.InsertList(Detail);
                        GenericFactory_EF_PrdFinishingConsumptionDetail.Save();
                        GenericFactory_EF_PrdFinishingConsumptionDetail.updateMaxID("PrdFinishingConsumptionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                    }
                    transaction.Complete();
                    result = Master.FinishingConsumptionNo;
                }
                catch (Exception e)
                {
                    result = "";
                    e.ToString();
                }                
            }
            return result;
        }

        //public IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionByID(vmCmnParameters objcmnParam)
        //{
        //    GenericFactory_vmChemicalSetupMasterDetail_GF = new vmChemicalSetupMasterDetail_VM();
        //    IEnumerable<vmChemicalSetupMasterDetail> FinishingConsumptionMaster = null;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add("CompanyID", objcmnParam.loggedCompany);
        //            ht.Add("LoggedUser", objcmnParam.loggeduser);
        //            ht.Add("FInishTypeID", objcmnParam.id);
        //            ht.Add("FinishingConsumptionID", objcmnParam.ItemType);

        //            spQuery = "[Get_FinishingConsumptionDetailByID]";
        //            FinishingConsumptionMaster = GenericFactory_vmChemicalSetupMasterDetail_GF.ExecuteQuery(spQuery, ht);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return FinishingConsumptionMaster;
        //}

        public IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionByID(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objDetail = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    if (objcmnParam.id == 0)
                    {
                        objDetail = (from PSS in _ctxCmn.PrdFinishingTypes
                                     join PSCS in _ctxCmn.PrdFinishingChemicalSetups on PSS.FinishingProcessID equals PSCS.FinishingProcessID
                                     join PCSD in _ctxCmn.PrdFinishingChemicalSetupDetails on PSCS.FinChemicalStupID equals PCSD.FinChemicalStupID
                                     join CM in _ctxCmn.CmnItemMasters on PCSD.ChemicalID equals CM.ItemID
                                     join CUOM in _ctxCmn.CmnUOMs on PCSD.UnitID equals CUOM.UOMID
                                     where PSS.FInishTypeID == objcmnParam.ItemType
                                     orderby PCSD.ChemicalID
                                     select new
                                     {
                                         ChemicalID = PCSD.ChemicalID,
                                         ItemName = CM.ItemName,
                                         MinQty = PCSD.MinQty,
                                         MaxQty = PCSD.MaxQty,
                                         UnitID = PCSD.UnitID,
                                         UOMName = CUOM.UOMName,
                                         Batch = (from CB in _ctxCmn.CmnBatches
                                                  join IM in _ctxCmn.InvStockMasters on CB.BatchID equals IM.BatchID
                                                  where CB.ItemID == PCSD.ChemicalID
                                                  //where B.ItemTypeID != null
                                                  select new vmCmnBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).ToList(),
                                         Supplier = (from CU in _ctxCmn.CmnUsers
                                                     join IM in _ctxCmn.InvStockMasters on CU.UserID equals IM.SupplierID
                                                     where IM.ItemID == PCSD.ChemicalID
                                                     //where B.ItemTypeID != null
                                                     select new vmBallInfo { SupplierID = IM.SupplierID, SupplierName = CU.UserFullName }).ToList()
                                     }).ToList().Select(x => new vmChemicalSetupMasterDetail
                                     {
                                         ChemicalID = x.ChemicalID,
                                         ItemName = x.ItemName,
                                         MinQty = x.MinQty,
                                         MaxQty = x.MaxQty,
                                         UnitID = x.UnitID,
                                         UOMName = x.UOMName,
                                         Batch = x.Batch,
                                         Supplier = x.Supplier
                                     }).GroupBy(i => i.ChemicalID).Select(i => i.First()).ToList();
                    }
                    else
                    {
                        objDetail = (from PSS in _ctxCmn.PrdFinishingConsumptionDetails
                                     join PCSD in _ctxCmn.PrdSizingChemicalSetupDetails on PSS.ChemicalID equals PCSD.ChemicalID
                                     join CM in _ctxCmn.CmnItemMasters on PSS.ChemicalID equals CM.ItemID
                                     join CUOM in _ctxCmn.CmnUOMs on PSS.UnitID equals CUOM.UOMID
                                     where PSS.FinishingConsumptionID == objcmnParam.id
                                     orderby PSS.FinishingConsumptionDetailID
                                     select new
                                     {
                                         FinishingConsumptionDetailID = PSS.FinishingConsumptionDetailID,
                                         FinishingConsumptionID = PSS.FinishingConsumptionID,
                                         ChemicalID = PSS.ChemicalID,
                                         ItemName = CM.ItemName,
                                         MinQty = PCSD.MinQty,
                                         MaxQty = PCSD.MaxQty,
                                         UnitID = PSS.UnitID,
                                         UOMName = CUOM.UOMName,
                                         BatchID = PSS.BatchID,
                                         SupplierID = PSS.SupplierID,
                                         Qty = PSS.AccQty,
                                         UnitPrice = PSS.UnitPrice,
                                         Amount = PSS.Amount,
                                         CurrentStock = _ctxCmn.InvStockMasters.Where(x => x.ItemID == PSS.ChemicalID && x.BatchID == PSS.BatchID && x.DepartmentID == objcmnParam.DepartmentID && x.CompanyID == objcmnParam.loggedCompany && x.UOMID == PSS.UnitID && (PSS.SupplierID == null || PSS.SupplierID == 0 ? true : x.SupplierID == PSS.SupplierID)).Select(x => x.CurrentStock).FirstOrDefault(),
                                         Batch = (from CB in _ctxCmn.CmnBatches
                                                  where CB.ItemID == PSS.ChemicalID
                                                  //where B.ItemTypeID != null
                                                  select new vmCmnBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).Distinct().ToList(),
                                         Supplier = (from CU in _ctxCmn.CmnUsers
                                                     join IM in _ctxCmn.InvStockMasters on CU.UserID equals IM.SupplierID
                                                     where IM.ItemID == PSS.ChemicalID
                                                     //where B.ItemTypeID != null
                                                     select new vmBallInfo { SupplierID = IM.SupplierID, SupplierName = CU.UserFullName }).Distinct().ToList()
                                     }).ToList().Select(x => new vmChemicalSetupMasterDetail
                                     {
                                         ChemicalConsumptionDetailID = x.FinishingConsumptionDetailID,
                                         ChemicalConsumptionID = x.FinishingConsumptionID,
                                         ChemicalID = x.ChemicalID,
                                         ItemName = x.ItemName,
                                         MinQty = x.MinQty,
                                         MaxQty = x.MaxQty,
                                         UnitID = x.UnitID,
                                         UOMName = x.UOMName,
                                         BatchID = x.BatchID,
                                         SupplierID = x.SupplierID,
                                         Qty = x.Qty,
                                         UnitPrice = x.UnitPrice,
                                         Amount = x.Amount,
                                         CurrentStock = x.CurrentStock,
                                         Batch = x.Batch,
                                         Supplier = x.Supplier
                                     }).GroupBy(i => i.ChemicalConsumptionDetailID).Select(i => i.First()).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDetail;
        }        

        //public IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionByID(vmCmnParameters cmnParam)
        //{
        //    IEnumerable<vmChemicalSetupMasterDetail> objWeavingMachineSetup = null;
        //    using (ERP_Entities _ctxCmn = new ERP_Entities())
        //    {
        //        try
        //        {
        //            var PrdFinishingConsumptionMaster = _ctxCmn.PrdFinishingConsumptionMasters.ToList();
        //            var PrdFinishingConsumptionDetail = _ctxCmn.PrdFinishingConsumptionDetails.ToList();
        //            var CmnItemMaster = _ctxCmn.CmnItemMasters.ToList();
        //            var PrdFinishingType = _ctxCmn.PrdFinishingTypes.ToList();
        //            var CmnUOM = _ctxCmn.CmnUOMs.ToList();

        //            objWeavingMachineSetup = (from master in PrdFinishingConsumptionMaster
        //                                      join detail in PrdFinishingConsumptionDetail on master.FinishingConsumptionID equals detail.FinishingConsumptionID
        //                                      join item in CmnItemMaster on detail.ChemicalID equals item.ItemID
        //                                      join uom in CmnUOM on detail.UnitID equals uom.UOMID
        //                                      join pft in PrdFinishingType on master.FinishingTypeID equals pft.FInishTypeID
        //                                      where master.FinishingConsumptionID == cmnParam.id && master.IsDeleted == false
        //                                      select new vmChemicalSetupMasterDetail
        //                                      {
        //                                          FinishingConsumptionID = master.FinishingConsumptionID,
        //                                          FinishingConsumptionDetailID = detail.FinishingConsumptionDetailID,
        //                                          FinishingTypeID = master.FinishingTypeID,
        //                                          FInishTypeName = pft.FInishTypeName,
        //                                          Volume = master.Volume,
        //                                          ConsumptionDate = master.ConsumptionDate,
        //                                          Remarks = master.Remarks,
        //                                          ChemicalID = detail.ChemicalID,
        //                                          ItemName = item.ItemName,
        //                                          RequiredQty = detail.RequiredQty,
        //                                          AccQty = detail.AccQty,
        //                                          UnitID = detail.UnitID,
        //                                          UOMName = uom.UOMName
        //                                      }).ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //    }
        //    return objWeavingMachineSetup;
        //}

        public IEnumerable<vmChemicalSetupMasterDetail> GetFiniChemConsumptionMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmChemicalSetupMasterDetail_GF = new vmChemicalSetupMasterDetail_VM();
            IEnumerable<vmChemicalSetupMasterDetail> FinishingConsumptionMaster = null;
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

                    spQuery = "[Get_FinishingConsumptionMaster]";
                    FinishingConsumptionMaster = GenericFactory_vmChemicalSetupMasterDetail_GF.ExecuteQuery(spQuery, ht);
                    recordsTotal = _ctxCmn.PrdFinishingConsumptionMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return FinishingConsumptionMaster;
        }

        public string DeleteFiniChemConsumptionMD(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdFinishingConsumptionMaster = new PrdFinishingConsumptionMaster_EF();
                GenericFactory_EF_PrdFinishingConsumptionDetail = new PrdFinishingConsumptionDetail_EF();
                string BWSNos = string.Empty;
                var FiniConM = new PrdFinishingConsumptionMaster();
                var FiniConD = new PrdFinishingConsumptionDetail();
                try
                {
                    FiniConM = GenericFactory_EF_PrdFinishingConsumptionMaster.GetAll().Where(x => x.FinishingConsumptionID == objcmnParam.id).FirstOrDefault();
                    FiniConM.IsDeleted = true;
                    FiniConM.CompanyID = objcmnParam.loggedCompany;
                    FiniConM.DeleteBy = objcmnParam.loggeduser;
                    FiniConM.DeleteOn = DateTime.Now;
                    FiniConM.DeletePc = HostService.GetIP();

                    FiniConD = GenericFactory_EF_PrdFinishingConsumptionDetail.GetAll().Where(x => x.FinishingConsumptionID == objcmnParam.id).FirstOrDefault();
                    FiniConD.IsDeleted = true;
                    FiniConD.CompanyID = objcmnParam.loggedCompany;
                    FiniConD.DeleteBy = objcmnParam.loggeduser;
                    FiniConD.DeleteOn = DateTime.Now;
                    FiniConD.DeletePc = HostService.GetIP();

                    GenericFactory_EF_PrdFinishingConsumptionMaster.Update(FiniConM);
                    GenericFactory_EF_PrdFinishingConsumptionMaster.Save();

                    transaction.Complete();
                    result = FiniConM.FinishingConsumptionNo;
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
