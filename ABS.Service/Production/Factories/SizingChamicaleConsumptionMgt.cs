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

namespace ABS.Service.Production.Factories
{
    public class SizingChamicaleConsumptionMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<PrdSizingChemicalConsumption> GFactory_EF_PrdSizingChemicalConsumption = null;
        private iGenericFactory_EF<PrdSizingChemicalconsumptionDetail> GFactory_EF_PrdSizingChemicalconsumptionDetail = null;
        private iGenericFactory<vmChemicalSetupMasterDetail> GFactory_vmChemicalSetupMasterDetail_GF = null;
        private iGenericFactory<vmBallInfo> GenericFactory_vmBallInfo_GF = null;

        public IEnumerable<vmChemicalSetupMasterDetail> GetSizingChemicalConsumptionMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objChemicalMaster = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objChemicalMaster = (from MP in _ctxCmn.PrdSizingChemicalConsumptions
                                         join CM in _ctxCmn.CmnItemMasters on MP.ItemID equals CM.ItemID
                                         join SD in _ctxCmn.PrdSetSetups on MP.SetID equals SD.SetID
                                         join UM in _ctxCmn.CmnOrganograms on MP.DepartmentID equals UM.OrganogramID
                                         where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                         select new
                                         {
                                             ChemicalConsumptionID = MP.ChemicalConsumptionID,
                                             ItemID = MP.ItemID,
                                             ArticleNo = CM.ArticleNo,
                                             SetID = MP.SetID,
                                             SetNo = SD.SetNo,
                                             DepartmentID = MP.DepartmentID,
                                             OrganogramName = UM.OrganogramName,
                                             ConsumptionDate = MP.ConsumptionDate,
                                             Remarks = MP.Remarks
                                         }).ToList().Select(x => new vmChemicalSetupMasterDetail
                                         {
                                             ChemicalConsumptionID = x.ChemicalConsumptionID,
                                             ItemID = x.ItemID,
                                             ArticleNo = x.ArticleNo,
                                             SetID = x.SetID,
                                             SetNo = x.SetNo,
                                             DepartmentID = x.DepartmentID,
                                             OrganogramName = x.OrganogramName,
                                             ConsumptionDate = x.ConsumptionDate,
                                             Remarks = x.Remarks
                                         }).ToList();

                    recordsTotal = objChemicalMaster.Count();
                    objChemicalMaster = objChemicalMaster.OrderByDescending(x => x.ChemicalConsumptionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objChemicalMaster;
        }

        public IEnumerable<vmBuyer> GetSupplier(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBuyer> objSupplier = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objSupplier = (from b in _ctxCmn.CmnUsers
                                   join ISM in _ctxCmn.InvStockMasters on b.UserID equals ISM.SupplierID
                                   where b.UserTypeID == objcmnParam.ItemType && b.IsActive == true && b.IsDeleted == false && ISM.CurrentStock > 0
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
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSupplier;
        }

        //public IEnumerable<vmChemicalSetupMasterDetail> GetSizingChemicalConsumptionDetailByID(vmCmnParameters objcmnParam)
        //{
        //    GFactory_vmChemicalSetupMasterDetail_GF = new vmChemicalSetupMasterDetail_GF();
        //    IEnumerable<vmChemicalSetupMasterDetail> objChemicalDetailByID = null;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add("CompanyID", objcmnParam.loggedCompany);
        //            ht.Add("LoggedUser", objcmnParam.loggeduser);
        //            ht.Add("PageNo", objcmnParam.pageNumber);
        //            ht.Add("RowCountPerPage", objcmnParam.pageSize);
        //            ht.Add("IsPaging", objcmnParam.IsPaging);
        //            ht.Add("SetID", objcmnParam.ItemType);
        //            ht.Add("ChemicalConsumptionID", objcmnParam.id);

        //            spQuery = "[Get_SizingChemicalConsumptionDetailByID]";
        //            objChemicalDetailByID = GFactory_vmChemicalSetupMasterDetail_GF.ExecuteQuery(spQuery, ht);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objChemicalDetailByID;
        //}

        public vmBallInfo GetCurrentStock(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmBallInfo_GF = new vmBallInfo_GF();
            vmBallInfo CurrentStockByFilter = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("ItemID", objcmnParam.id);
                    ht.Add("SupplierID", objcmnParam.ItemType);
                    ht.Add("BatchID", objcmnParam.ItemGroup);
                    ht.Add("DepartmentID", objcmnParam.DepartmentID);
                    ht.Add("UnitID", objcmnParam.UserType);

                    spQuery = "[Get_SizingChemConsumpCurrentStock]";
                    CurrentStockByFilter = GenericFactory_vmBallInfo_GF.ExecuteQuerySingle(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return CurrentStockByFilter;
        }

        public IEnumerable<vmChemicalSetupMasterDetail> GetSizingChemicalConsumptionDetailByID(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objDetail = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    if (objcmnParam.id == 0)
                    {
                        objDetail = (from PSS in _ctxCmn.PrdSetSetups
                                     join PSCS in _ctxCmn.PrdSizingChemicalSetups on PSS.SetID equals PSCS.SetID
                                     join PCSD in _ctxCmn.PrdSizingChemicalSetupDetails on PSCS.ChemicalSetupID equals PCSD.ChemicalSetupID
                                     join CM in _ctxCmn.CmnItemMasters on PCSD.ChemicalID equals CM.ItemID
                                     join CUOM in _ctxCmn.CmnUOMs on PCSD.UnitID equals CUOM.UOMID
                                     where PSS.SetID == objcmnParam.ItemType
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
                        objDetail = (from PSS in _ctxCmn.PrdSizingChemicalconsumptionDetails
                                     join PCSD in _ctxCmn.PrdSizingChemicalSetupDetails on PSS.ChemicalID equals PCSD.ChemicalID
                                     join CM in _ctxCmn.CmnItemMasters on PSS.ChemicalID equals CM.ItemID
                                     join CUOM in _ctxCmn.CmnUOMs on PSS.UnitID equals CUOM.UOMID
                                     where PSS.ChemicalConsumptionID == objcmnParam.id
                                     orderby PSS.ChemicalConsumptionDetailID
                                     select new
                                     {
                                         ChemicalConsumptionDetailID = PSS.ChemicalConsumptionDetailID,
                                         ChemicalConsumptionID = PSS.ChemicalConsumptionID,
                                         ChemicalID = PSS.ChemicalID,
                                         ItemName = CM.ItemName,
                                         MinQty = PCSD.MinQty,
                                         MaxQty = PCSD.MaxQty,
                                         UnitID = PSS.UnitID,
                                         UOMName = CUOM.UOMName,
                                         BatchID = PSS.BatchID,
                                         SupplierID = PSS.SupplierID,
                                         Qty = PSS.Qty,
                                         UnitPrice = PSS.UnitPrice,
                                         Amount = PSS.Amount,
                                         CurrentStock = _ctxCmn.InvStockMasters.Where(x => x.ItemID == PSS.ChemicalID && x.BatchID == PSS.BatchID && x.DepartmentID == objcmnParam.DepartmentID && x.CompanyID == objcmnParam.loggedCompany && x.UOMID == PSS.UnitID && (PSS.SupplierID == null || PSS.SupplierID == 0 ? true : x.SupplierID == PSS.SupplierID)).Select(x => x.CurrentStock).FirstOrDefault(),
                                         Batch = (from CB in _ctxCmn.CmnBatches
                                                  join IM in _ctxCmn.InvStockMasters on CB.BatchID equals IM.BatchID
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
                                         ChemicalConsumptionDetailID = x.ChemicalConsumptionDetailID,
                                         ChemicalConsumptionID = x.ChemicalConsumptionID,
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

        public string SaveUpdateSizingChemicalMasterDetail(vmChemicalSetupMasterDetail Master, List<vmChemicalSetupMasterDetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GFactory_EF_PrdSizingChemicalConsumption = new PrdSizingChemicalConsumption_EF();
                GFactory_EF_PrdSizingChemicalconsumptionDetail = new PrdSizingChemicalconsumptionDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdSizingChemicalConsumption();
                var DetailItem = new List<PrdSizingChemicalconsumptionDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmChemicalSetupMasterDetail item = new vmChemicalSetupMasterDetail();
                //***************************************End Create Model Instance to get Data***********************************
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.ChemicalConsumptionID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GFactory_EF_PrdSizingChemicalConsumption.getMaxID("PrdSizingChemicalConsumption"));
                            DetailId = Convert.ToInt64(GFactory_EF_PrdSizingChemicalconsumptionDetail.getMaxID("PrdSizingChemicalconsumptionDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            MasterItem = new PrdSizingChemicalConsumption
                            {
                                ChemicalConsumptionID = MasterId,
                                DepartmentID = (int)objcmnParam.DepartmentID,
                                ItemID = (long)Master.ItemID,
                                SetID = Master.SetID,
                                UserID = objcmnParam.loggeduser,
                                ConsumptionDate=Master.ConsumptionDate,
                                Remarks=Master.Remarks,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdSizingChemicalconsumptionDetail
                                {
                                    ChemicalConsumptionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    ChemicalConsumptionID = MasterId,
                                    ChemicalID = (long)item.ChemicalID,
                                    Qty = item.Qty,
                                    UnitID = (int)item.UnitID,
                                    Amount = item.Amount,
                                    BatchID = item.BatchID,
                                    SupplierID = item.SupplierID,
                                    UnitPrice = item.UnitPrice,
                                    ConsumptionDate=Master.ConsumptionDate,

                                    TransactionTypeID = objcmnParam.tTypeId,
                                    DepartmentID = objcmnParam.DepartmentID,
                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false
                                };
                                DetailItem.Add(Detailitem);
                                OtherDigits++;
                            }
                            //***************************************************End Save Operation************************************************
                        }
                        else
                        {
                            //***********************************Start Get Data From Related Table to Update*********************************
                            var MasterAll = GFactory_EF_PrdSizingChemicalConsumption.GetAll().Where(x => x.ChemicalConsumptionID == Master.ChemicalConsumptionID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GFactory_EF_PrdSizingChemicalconsumptionDetail.GetAll().Where(x => x.ChemicalConsumptionID == Master.ChemicalConsumptionID && x.CompanyID == objcmnParam.loggedCompany).ToList();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.ChemicalConsumptionID == Master.ChemicalConsumptionID);
                            MasterItem.DepartmentID = (int)objcmnParam.DepartmentID;
                            MasterItem.ItemID = (long)Master.ItemID;
                            MasterItem.SetID = Master.SetID;
                            MasterItem.UserID = objcmnParam.loggeduser;
                            MasterItem.ConsumptionDate = Master.ConsumptionDate;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                foreach (PrdSizingChemicalconsumptionDetail d in DetailAll.Where(d => d.ChemicalConsumptionID == Master.ChemicalConsumptionID && d.ChemicalConsumptionDetailID == item.ChemicalConsumptionDetailID))
                                {
                                    d.ChemicalID = (long)item.ChemicalID;
                                    d.Qty = item.Qty;
                                    d.UnitID = (int)item.UnitID;
                                    d.Amount = item.Amount;
                                    d.BatchID = item.BatchID;
                                    d.SupplierID = item.SupplierID;
                                    d.UnitPrice = item.UnitPrice;
                                    d.ConsumptionDate = Master.ConsumptionDate;

                                    d.CompanyID = objcmnParam.loggedCompany;
                                    d.UpdateBy = objcmnParam.loggeduser;
                                    d.UpdateOn = DateTime.Now;
                                    d.UpdatePc = HostService.GetIP();
                                    d.IsDeleted = false;

                                    DetailItem.Add(d);
                                    break;
                                }
                            }
                            //***************************************************End Update Operation********************************************
                        }

                        if (Master.ChemicalConsumptionID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GFactory_EF_PrdSizingChemicalConsumption.Update(MasterItem);
                                GFactory_EF_PrdSizingChemicalConsumption.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GFactory_EF_PrdSizingChemicalconsumptionDetail.UpdateList(DetailItem.ToList());
                                GFactory_EF_PrdSizingChemicalconsumptionDetail.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GFactory_EF_PrdSizingChemicalConsumption.Insert(MasterItem);
                                GFactory_EF_PrdSizingChemicalConsumption.Save();
                                GFactory_EF_PrdSizingChemicalConsumption.updateMaxID("PrdSizingChemicalConsumption", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GFactory_EF_PrdSizingChemicalconsumptionDetail.InsertList(DetailItem.ToList());
                                GFactory_EF_PrdSizingChemicalconsumptionDetail.Save();
                                GFactory_EF_PrdSizingChemicalconsumptionDetail.updateMaxID("PrdSizingChemicalconsumptionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
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
                else
                {
                    result = "";
                }
            }
            return result;
            //**************************************************End Main Operation************************************************
        }

        public string DelUpdateSizingChemicalMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdSizingChemicalConsumption = new PrdSizingChemicalConsumption_EF();
                GFactory_EF_PrdSizingChemicalconsumptionDetail = new PrdSizingChemicalconsumptionDetail_EF();

                var MasterItem = new PrdSizingChemicalConsumption();
                var DetailItem = new List<PrdSizingChemicalconsumptionDetail>();

                //For Update Master Detail
                var MasterAll = GFactory_EF_PrdSizingChemicalConsumption.GetAll().Where(x => x.ChemicalConsumptionID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GFactory_EF_PrdSizingChemicalconsumptionDetail.GetAll().Where(x => x.ChemicalConsumptionID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.ChemicalConsumptionID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdSizingChemicalconsumptionDetail d in DetailAll.Where(d => d.ChemicalConsumptionID == objcmnParam.id))
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
                        GFactory_EF_PrdSizingChemicalConsumption.Update(MasterItem);
                        GFactory_EF_PrdSizingChemicalConsumption.Save();
                    }
                    if (DetailItem != null)
                    {
                        GFactory_EF_PrdSizingChemicalconsumptionDetail.UpdateList(DetailItem.ToList());
                        GFactory_EF_PrdSizingChemicalconsumptionDetail.Save();
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
