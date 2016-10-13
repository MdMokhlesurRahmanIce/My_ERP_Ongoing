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
    public class SizingChemicalSetupMgt
    {
        private ERP_Entities _ctxCmn = null;

        //private iGenericFactory_EF<PrdDyingMachineSetup> GFactory_EF_PrdDyingMachineSetup = null;
        //private iGenericFactory_EF<PrdDyingMachineSetupDetail> GFactory_EF_PrdDyingMachineSetupDetail = null;
        private iGenericFactory_EF<PrdSizingChemicalSetup> GFactory_EF_PrdSizingChemicalSetup = null;
        private iGenericFactory_EF<PrdSizingChemicalSetupDetail> GFactory_EF_PrdSizingChemicalSetupDetail = null;

        public IEnumerable<vmChemicalSetupMasterDetail> GetSizingChemicalSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objChemicalMaster = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objChemicalMaster = (from MP in _ctxCmn.PrdSizingChemicalSetups
                                         join CM in _ctxCmn.CmnItemMasters on MP.ItemID equals CM.ItemID
                                         join SD in _ctxCmn.PrdSetSetups on MP.SetID equals SD.SetID
                                         join UM in _ctxCmn.CmnUOMs on MP.UnitID equals UM.UOMID
                                         where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                         select new
                                         {
                                             ChemicalSetupID = MP.ChemicalSetupID,
                                             ItemID = MP.ItemID,
                                             ArticleNo = CM.ArticleNo,
                                             SetID = MP.SetID,
                                             SetNo = SD.SetNo,
                                             DepartmentID = MP.DepartmentID,
                                             Qty = MP.Qty,
                                             UnitID = MP.UnitID,
                                             UOMName = UM.UOMName
                                         }).ToList().Select(x => new vmChemicalSetupMasterDetail
                                         {
                                             ChemicalSetupID = x.ChemicalSetupID,
                                             ItemID = x.ItemID,
                                             ArticleNo = x.ArticleNo,
                                             SetID = (long)x.SetID,
                                             SetNo = x.SetNo,
                                             DepartmentID = x.DepartmentID,
                                             Qty = x.Qty,
                                             UnitID = x.UnitID,
                                             UOMName = x.UOMName
                                         }).ToList();

                    recordsTotal = objChemicalMaster.Count();
                    objChemicalMaster = objChemicalMaster.OrderByDescending(x => x.ChemicalSetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objChemicalMaster;
        }

        public vmChemicalSetupMasterDetail GetSizingChemicalSetupMasterByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            vmChemicalSetupMasterDetail objChemicalMasterByID = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objChemicalMasterByID = (from MP in _ctxCmn.PrdSizingChemicalSetups
                                             join CM in _ctxCmn.CmnItemMasters on MP.ItemID equals CM.ItemID
                                             join SD in _ctxCmn.PrdSetSetups on MP.SetID equals SD.SetID
                                             join UM in _ctxCmn.CmnUOMs on MP.UnitID equals UM.UOMID
                                             where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                             && MP.ChemicalSetupID == objcmnParam.id
                                             //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                             orderby MP.ChemicalSetupID descending
                                             select new
                                             {
                                                 ChemicalSetupID = MP.ChemicalSetupID,
                                                 ItemID = MP.ItemID,
                                                 ArticleNo = CM.ArticleNo,
                                                 SetID = MP.SetID,
                                                 SetNo = SD.SetNo,
                                                 DepartmentID = MP.DepartmentID,
                                                 Qty = MP.Qty,
                                                 UnitID = MP.UnitID,
                                                 UOMName = UM.UOMName
                                             }).Select(x => new vmChemicalSetupMasterDetail
                                             {
                                                 ChemicalSetupID = x.ChemicalSetupID,
                                                 ItemID = x.ItemID,
                                                 ArticleNo = x.ArticleNo,
                                                 SetID = (long)x.SetID,
                                                 SetNo = x.SetNo,
                                                 DepartmentID = x.DepartmentID,
                                                 Qty = x.Qty,
                                                 UnitID = x.UnitID,
                                                 UOMName = x.UOMName
                                             }).FirstOrDefault();

                    //recordsTotal = objSetWiseMasterByID.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objChemicalMasterByID;
        }

        public IEnumerable<vmChemicalSetupMasterDetail> GetSizingChemicalSetupDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objChemicalDetailByID = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objChemicalDetailByID = (from MP in _ctxCmn.PrdSizingChemicalSetupDetails
                                             join CM in _ctxCmn.CmnItemMasters on MP.ChemicalID equals CM.ItemID
                                             join UM in _ctxCmn.CmnUOMs on MP.UnitID equals UM.UOMID
                                             where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                             && MP.ChemicalSetupID == objcmnParam.id
                                             //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                             orderby MP.ChemicalSetupID
                                             select new vmChemicalSetupMasterDetail
                                             {
                                                 ChemicalSetupDetailID = MP.ChemicalSetupDetailID,
                                                 ChemicalSetupID = MP.ChemicalSetupID,
                                                 ChemicalID = MP.ChemicalID,
                                                 ItemName = CM.ItemName,
                                                 MinQty = MP.MinQty,
                                                 MaxQty = (decimal)MP.MaxQty,
                                                 UnitID = MP.UnitID,
                                                 UOMName = UM.UOMName
                                             }).ToList();

                    recordsTotal = objChemicalDetailByID.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objChemicalDetailByID;
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
                GFactory_EF_PrdSizingChemicalSetup = new PrdSizingChemicalSetup_EF();
                GFactory_EF_PrdSizingChemicalSetupDetail = new PrdSizingChemicalSetupDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdSizingChemicalSetup();
                var DetailItem = new List<PrdSizingChemicalSetupDetail>();
                var DetailItems = new List<PrdSizingChemicalSetupDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmChemicalSetupMasterDetail item = new vmChemicalSetupMasterDetail();
                vmChemicalSetupMasterDetail items = new vmChemicalSetupMasterDetail();
                PrdSizingChemicalSetupDetail itemdel = new PrdSizingChemicalSetupDetail();
                //***************************************End Create Model Instance to get Data***********************************

                var SDetail = Detail.Where(x => x.ChemicalSetupDetailID == 0).ToList();
                var UDetail = Detail.Where(x => x.ChemicalSetupDetailID != 0).ToList();
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.ChemicalSetupID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GFactory_EF_PrdSizingChemicalSetup.getMaxID("PrdSizingChemicalSetup"));
                            DetailId = Convert.ToInt64(GFactory_EF_PrdSizingChemicalSetupDetail.getMaxID("PrdSizingChemicalSetupDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            MasterItem = new PrdSizingChemicalSetup
                            {
                                ChemicalSetupID = MasterId,
                                DepartmentID = (int)objcmnParam.DepartmentID,
                                ItemID = (long)Master.ItemID,
                                Qty = (decimal)Master.Qty,
                                SetID = Master.SetID,
                                UnitID = (int)Master.UnitID,
                                UserID = objcmnParam.loggeduser,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdSizingChemicalSetupDetail
                                {
                                    ChemicalSetupDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    ChemicalSetupID = MasterId,
                                    ChemicalID = (long)item.ChemicalID,
                                    MaxQty = item.MaxQty,
                                    MinQty = (decimal)item.MinQty,
                                    UnitID = (int)item.UnitID,

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
                            var MasterAll = GFactory_EF_PrdSizingChemicalSetup.GetAll().Where(x => x.ChemicalSetupID == Master.ChemicalSetupID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GFactory_EF_PrdSizingChemicalSetupDetail.GetAll().Where(x => x.ChemicalSetupID == Master.ChemicalSetupID && x.CompanyID == objcmnParam.loggedCompany).ToList();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.ChemicalSetupID == Master.ChemicalSetupID);
                            MasterItem.DepartmentID = (int)objcmnParam.DepartmentID;
                            MasterItem.ItemID = (long)Master.ItemID;
                            MasterItem.Qty = (decimal)Master.Qty;
                            MasterItem.SetID = Master.SetID;
                            MasterItem.UnitID = (int)Master.UnitID;
                            MasterItem.UserID = objcmnParam.loggeduser;

                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < UDetail.Count; i++)
                            {
                                item = UDetail[i];
                                foreach (PrdSizingChemicalSetupDetail d in DetailAll.Where(d => d.ChemicalSetupID == Master.ChemicalSetupID && d.ChemicalSetupDetailID == item.ChemicalSetupDetailID))
                                {
                                    d.ChemicalID = (long)item.ChemicalID;
                                    d.MaxQty = item.MaxQty;
                                    d.MinQty = (decimal)item.MinQty;
                                    d.UnitID = (int)item.UnitID;

                                    d.CompanyID = objcmnParam.loggedCompany;
                                    d.UpdateBy = objcmnParam.loggeduser;
                                    d.UpdateOn = DateTime.Now;
                                    d.UpdatePc = HostService.GetIP();
                                    d.IsDeleted = false;

                                    DetailItem.Add(d);
                                    break;
                                }
                            }
                            if (SDetail != null && SDetail.Count != 0)
                            {
                                for (int i = 0; i < SDetail.Count; i++)
                                {
                                    item = SDetail[i];
                                    DetailId = Convert.ToInt64(GFactory_EF_PrdSizingChemicalSetupDetail.getMaxID("PrdSizingChemicalSetupDetail"));
                                    FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                                    OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                                    var Detailitems = new PrdSizingChemicalSetupDetail
                                    {
                                        ChemicalSetupDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                        ChemicalSetupID = (long)Master.ChemicalSetupID,
                                        ChemicalID = (long)item.ChemicalID,
                                        MaxQty = item.MaxQty,
                                        MinQty = (decimal)item.MinQty,
                                        UnitID = (int)item.UnitID,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItems.Add(Detailitems);
                                    GFactory_EF_PrdSizingChemicalSetupDetail.updateMaxID("PrdSizingChemicalSetupDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits)));
                                }
                            }

                            if (UDetail.Count < DetailAll.Count())
                            {
                                for (int i = 0; i < DetailAll.Count(); i++)
                                {
                                    itemdel = DetailAll[i];

                                    var delDetail = (from del in DetailItem.Where(x => x.ChemicalSetupDetailID == itemdel.ChemicalSetupDetailID) select del.ChemicalSetupDetailID).FirstOrDefault();
                                    if (delDetail != itemdel.ChemicalSetupDetailID)
                                    {
                                        var tem = DetailAll.FirstOrDefault(d => d.ChemicalSetupID == Master.ChemicalSetupID && d.ChemicalSetupDetailID == itemdel.ChemicalSetupDetailID);
                                        tem.CompanyID = objcmnParam.loggedCompany;
                                        tem.DeleteBy = objcmnParam.loggeduser;
                                        tem.DeleteOn = DateTime.Now;
                                        tem.DeletePc = HostService.GetIP();
                                        tem.IsDeleted = true;
                                        DetailItem.Add(tem);
                                    }
                                }
                            }
                            //***************************************************End Update Operation********************************************
                        }

                        if (Master.ChemicalSetupID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GFactory_EF_PrdSizingChemicalSetup.Update(MasterItem);
                                GFactory_EF_PrdSizingChemicalSetup.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GFactory_EF_PrdSizingChemicalSetupDetail.UpdateList(DetailItem.ToList());
                                GFactory_EF_PrdSizingChemicalSetupDetail.Save();
                            }
                            if (DetailItems != null && DetailItems.Count != 0)
                            {
                                GFactory_EF_PrdSizingChemicalSetupDetail.InsertList(DetailItems.ToList());
                                GFactory_EF_PrdSizingChemicalSetupDetail.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GFactory_EF_PrdSizingChemicalSetup.Insert(MasterItem);
                                GFactory_EF_PrdSizingChemicalSetup.Save();
                                GFactory_EF_PrdSizingChemicalSetup.updateMaxID("PrdSizingChemicalSetup", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GFactory_EF_PrdSizingChemicalSetupDetail.InsertList(DetailItem.ToList());
                                GFactory_EF_PrdSizingChemicalSetupDetail.Save();
                                GFactory_EF_PrdSizingChemicalSetupDetail.updateMaxID("PrdSizingChemicalSetupDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
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
                GFactory_EF_PrdSizingChemicalSetup = new PrdSizingChemicalSetup_EF();
                GFactory_EF_PrdSizingChemicalSetupDetail = new PrdSizingChemicalSetupDetail_EF();

                var MasterItem = new PrdSizingChemicalSetup();
                var DetailItem = new List<PrdSizingChemicalSetupDetail>();

                //For Update Master Detail
                var MasterAll = GFactory_EF_PrdSizingChemicalSetup.GetAll().Where(x => x.ChemicalSetupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GFactory_EF_PrdSizingChemicalSetupDetail.GetAll().Where(x => x.ChemicalSetupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.ChemicalSetupID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdSizingChemicalSetupDetail d in DetailAll.Where(d => d.ChemicalSetupID == objcmnParam.id))
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
                        GFactory_EF_PrdSizingChemicalSetup.Update(MasterItem);
                        GFactory_EF_PrdSizingChemicalSetup.Save();
                    }
                    if (DetailItem != null)
                    {
                        GFactory_EF_PrdSizingChemicalSetupDetail.UpdateList(DetailItem.ToList());
                        GFactory_EF_PrdSizingChemicalSetupDetail.Save();
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
