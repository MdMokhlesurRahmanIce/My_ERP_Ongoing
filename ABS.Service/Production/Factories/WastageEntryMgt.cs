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
    public class WastageEntryMgt //: iBallWarpingMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdWastage> GenericFactory_PrdWastage_EF = null;
        private iGenericFactory_EF<PrdWastageDetail> GenericFactory_PrdWastageDetail_EF = null;
        private iGenericFactory<vmWastageMasterDetail> GenericFactory_vmWastageMasterDetail_VM = null;

        public IEnumerable<vmWastageMasterDetail> GetWastageMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmWastageMasterDetail_VM = new vmWastageMasterDetail_VM();
            IEnumerable<vmWastageMasterDetail> WastageMaster = null;
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

                    spQuery = "[Get_WastageMRRMaster]";
                    WastageMaster = GenericFactory_vmWastageMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdWastages.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//WastageMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return WastageMaster;
        }

        public IEnumerable<vmWastageMasterDetail> GetWastageDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmWastageMasterDetail_VM = new vmWastageMasterDetail_VM();
            IEnumerable<vmWastageMasterDetail> WastageDetailByID = null;
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
                    ht.Add("WastageID", objcmnParam.id);

                    spQuery = "[Get_WastageMRRDetailByID]";
                    WastageDetailByID = GenericFactory_vmWastageMasterDetail_VM.ExecuteQuery(spQuery, ht);

                    recordsTotal = WastageDetailByID.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return WastageDetailByID;
        }



        public string SaveUpdateWastage(vmWastageMasterDetail itemMaster, List<vmWastageMasterDetail> MainDetail, vmCmnParameters objcmnParam)
        {
            //    private iGenericFactory_EF<PrdWastage> GenericFactory_PrdWastage_EF;
            //private iGenericFactory_EF<PrdWastageDetail> GenericFactory_PrdWastageDetail_EF;
            //private iGenericFactory<vmWastageMasterDetail> GenericFactory_vmWastageMasterDetail_VM = null;
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************
                string CustomNo = string.Empty; long MainMasterId = 0, StopMasterId = 0, BreakMasterId = 0,
                MainDetailId = 0, MainFirstDigit = 0, MainOtherDigits = 0;
                int SMainRowNum = 0, UMainRowNum = 0, LastRowNum = 0; string WastageNo = "";
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdWastage_EF = new PrdWastage_EF();
                GenericFactory_PrdWastageDetail_EF = new PrdWastageDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdWastage();
                var DetailItemMain = new List<PrdWastageDetail>();
                var UDetailItemMain = new List<PrdWastageDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmWastageMasterDetail item = new vmWastageMasterDetail();
                //***************************************End Create Model Instance to get Data***********************************


                SMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Save").Count());
                UMainRowNum = Convert.ToInt32(MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").Count());
                //**************************************************Start Main Operation************************************************
                if (SMainRowNum > 0 || UMainRowNum > 0)
                {
                    try
                    {
                        if (itemMaster.WastageID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MainMasterId = Convert.ToInt16(GenericFactory_PrdWastage_EF.getMaxID("PrdWastage"));
                            MainDetailId = Convert.ToInt64(GenericFactory_PrdWastageDetail_EF.getMaxID("PrdWastageDetail"));
                            MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                            MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************
                            CustomNo = GenericFactory_PrdWastage_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                WastageNo = MainMasterId.ToString();
                            }
                            else
                            {
                                WastageNo = CustomNo;
                            }

                            MasterItem = new PrdWastage
                            {
                                WastageID = MainMasterId,
                                WastageNo = WastageNo,
                                WastageDate = (DateTime)itemMaster.WastageDate,
                                DepartmentID = (int)itemMaster.DepartmentID,
                                Remarks = itemMaster.Remarks,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc =  HostService.GetIP(),
                                IsDeleted = false
                            };
                            for (int i = 0; i < SMainRowNum; i++)
                            {
                                item = MainDetail[i];

                                var Detailitem = new PrdWastageDetail
                                {
                                    WastageDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                    WastageID = MainMasterId,
                                    ItemID = item.ItemID,
                                    Qty = item.Qty,
                                    UnitID = item.UnitID,
                                    Remarks = item.Remarks,

                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc =  HostService.GetIP(),
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
                                var MasterAll = GenericFactory_PrdWastage_EF.GetAll().Where(x => x.WastageID == itemMaster.WastageID && x.CompanyID == objcmnParam.loggedCompany);
                                var DetailAll = GenericFactory_PrdWastageDetail_EF.GetAll().Where(x => x.WastageID == itemMaster.WastageID && x.CompanyID == objcmnParam.loggedCompany);
                                //*************************************End Get Data From Related Table to Update*********************************                            
                                WastageNo = itemMaster.WastageNo;
                                MasterItem = MasterAll.First(x => x.WastageID == itemMaster.WastageID);
                                MasterItem.WastageDate = (DateTime)itemMaster.WastageDate;
                                MasterItem.DepartmentID = (int)itemMaster.DepartmentID;
                                MasterItem.Remarks = itemMaster.Remarks;

                                MasterItem.CompanyID = objcmnParam.loggedCompany;
                                MasterItem.UpdateBy = objcmnParam.loggeduser;
                                MasterItem.UpdateOn = DateTime.Now;
                                MasterItem.UpdatePc =  HostService.GetIP();

                                for (int i = 0; i < UMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Update" || x.ModelState == "Delete").ToList()[i];

                                    foreach (PrdWastageDetail d in DetailAll.Where(d => d.WastageID == item.WastageID && d.WastageDetailID == item.WastageDetailID))
                                    {
                                        if (item.ModelState == "Delete")
                                        {
                                            d.CompanyID = objcmnParam.loggedCompany;
                                            d.DeleteBy = objcmnParam.loggeduser;
                                            d.DeleteOn = DateTime.Now;
                                            d.DeletePc =  HostService.GetIP();
                                            d.IsDeleted = true;
                                        }
                                        else
                                        {
                                            d.ItemID = item.ItemID;
                                            d.Qty = item.Qty;
                                            d.UnitID = item.UnitID;
                                            d.Remarks = item.Remarks;

                                            d.CompanyID = objcmnParam.loggedCompany;
                                            d.UpdateBy = objcmnParam.loggeduser;
                                            d.UpdateOn = DateTime.Now;
                                            d.UpdatePc =  HostService.GetIP();
                                        }
                                        UDetailItemMain.Add(d);
                                    }
                                }
                            }
                            if (SMainRowNum > 0)
                            {
                                for (int i = 0; i < SMainRowNum; i++)
                                {
                                    item = MainDetail.Where(x => x.ModelState == "Save").ToList()[i];

                                    MainDetailId = Convert.ToInt64(GenericFactory_PrdWastageDetail_EF.getMaxID("PrdWastageDetail"));
                                    MainFirstDigit = Convert.ToInt64(MainDetailId.ToString().Substring(0, 1));
                                    MainOtherDigits = Convert.ToInt64(MainDetailId.ToString().Substring(1, MainDetailId.ToString().Length - 1));
                                    //***********************************************End Generate Master & Detail ID*****************************************  

                                    var Detailitem = new PrdWastageDetail
                                    {
                                        WastageDetailID = Convert.ToInt64(MainFirstDigit + "" + MainOtherDigits),
                                        WastageID = (long)itemMaster.WastageID,
                                        ItemID = item.ItemID,
                                        Qty = item.Qty,
                                        UnitID = item.UnitID,
                                        Remarks = item.Remarks,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc =  HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItemMain.Add(Detailitem);
                                    MainOtherDigits++;
                                }
                            }
                        }
                        //***********************************Start Get Data From Related Table to Update*********************************

                        if (itemMaster.WastageID > 0)
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdWastage_EF.Update(MasterItem);
                                GenericFactory_PrdWastage_EF.Save();
                            }
                            if (UDetailItemMain != null && UDetailItemMain.Count != 0)
                            {
                                GenericFactory_PrdWastageDetail_EF.UpdateList(UDetailItemMain.ToList());
                                GenericFactory_PrdWastageDetail_EF.Save();
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_PrdWastageDetail_EF.InsertList(DetailItemMain.ToList());
                                GenericFactory_PrdWastageDetail_EF.Save();
                                GenericFactory_PrdWastageDetail_EF.updateMaxID("PrdWastageDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
                            }

                        }
                        else
                        {
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdWastage_EF.Insert(MasterItem);
                                GenericFactory_PrdWastage_EF.Save();
                                GenericFactory_PrdWastage_EF.updateMaxID("PrdWastage", Convert.ToInt64(MainMasterId));
                                GenericFactory_PrdWastage_EF.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (DetailItemMain != null && DetailItemMain.Count != 0)
                            {
                                GenericFactory_PrdWastageDetail_EF.InsertList(DetailItemMain.ToList());
                                GenericFactory_PrdWastageDetail_EF.Save();
                                GenericFactory_PrdWastageDetail_EF.updateMaxID("PrdWastageDetail", Convert.ToInt64(MainFirstDigit + "" + (MainOtherDigits - 1)));
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

        public string DeleteWastageMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_PrdWastage_EF = new PrdWastage_EF();
                GenericFactory_PrdWastageDetail_EF = new PrdWastageDetail_EF();
                var MasterItem = new PrdWastage();
                var DetailItem = new List<PrdWastageDetail>();
                try
                {
                    var MasterAll = GenericFactory_PrdWastage_EF.GetAll().Where(x => x.WastageID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                    var DetailAll = GenericFactory_PrdWastageDetail_EF.GetAll().Where(x => x.WastageID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                    //Delete PrdWastage
                    MasterItem = MasterAll.First(x => x.WastageID == objcmnParam.id);
                    MasterItem.IsDeleted = true;
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc =  HostService.GetIP();

                    //Delete PrdWastageDetail
                    foreach (PrdWastageDetail d in DetailAll.Where(d => d.WastageID == objcmnParam.id))
                    {
                        d.IsDeleted = true;
                        d.CompanyID = objcmnParam.loggedCompany;
                        d.DeleteBy = objcmnParam.loggeduser;
                        d.DeleteOn = DateTime.Now;
                        d.DeletePc =  HostService.GetIP();
                        DetailItem.Add(d);
                    }

                    if (MasterItem != null)
                    {
                        GenericFactory_PrdWastage_EF.Update(MasterItem);
                        GenericFactory_PrdWastage_EF.Save();
                    }
                    if (DetailItem != null && DetailItem.Count != 0)
                    {
                        GenericFactory_PrdWastageDetail_EF.UpdateList(DetailItem);
                        GenericFactory_PrdWastageDetail_EF.Save();
                    }

                    transaction.Complete();
                    result = "1";

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
