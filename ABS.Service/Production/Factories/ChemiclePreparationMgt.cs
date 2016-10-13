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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class ChemiclePreparationMgt //: iChemiclePreparationMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmFinishingChemicalPreparation> GenericFactory_vmFinishingChemicalPreparation_GF = null;
        private iGenericFactory_EF<PrdFinishingChemicalSetup> GenericFactory_PrdFinishingChemicalSetup_EF = null;
        private iGenericFactory_EF<PrdFinishingChemicalSetupDetail> GenericFactory_PrdFinishingChemicalSetupDetail_EF = null;
        #region Read
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through ef</para>
        /// </summary>
        public IEnumerable<vmFinishingChemicalPreparation> GetFinChemicalPreparationMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmFinishingChemicalPreparation_GF = new vmFinishingChemicalPreparation_GF();
            IEnumerable<vmFinishingChemicalPreparation> FinishingChemMaster = null;
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

                    spQuery = "[Get_FinChemicalPreparationMaster]";
                    FinishingChemMaster = GenericFactory_vmFinishingChemicalPreparation_GF.ExecuteQuery(spQuery, ht);

                    recordsTotal = _ctxCmn.PrdFinishingMRRMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();//FinishingMaster.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return FinishingChemMaster;
        }

        public IEnumerable<vmFinishingChemicalPreparation> GetFiniChemicalPrepDetailByID(vmCmnParameters objcmnParam)
        {
            GenericFactory_vmFinishingChemicalPreparation_GF = new vmFinishingChemicalPreparation_GF();
            IEnumerable<vmFinishingChemicalPreparation> objFinChemicalDetailByID = null;            
            string spQuery = string.Empty;
            try
            {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("PageNo", objcmnParam.pageNumber);
                    ht.Add("RowCountPerPage", objcmnParam.pageSize);
                    ht.Add("IsPaging", objcmnParam.IsPaging);
                    ht.Add("FinChemicalStupID", objcmnParam.id);

                    spQuery = "[Get_FinChemicalPreparationDetailByID]";
                    objFinChemicalDetailByID = GenericFactory_vmFinishingChemicalPreparation_GF.ExecuteQuery(spQuery, ht);  
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFinChemicalDetailByID;           
        }

        public string SaveUpdateFiniChemicalMasterDetail(vmFinishingChemicalPreparation Master, List<vmFinishingChemicalPreparation> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0; string CustomNo=string.Empty, ChemicalNo;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdFinishingChemicalSetup_EF = new PrdFinishingChemicalSetup_EF();
                GenericFactory_PrdFinishingChemicalSetupDetail_EF = new PrdFinishingChemicalSetupDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdFinishingChemicalSetup();
                var DetailItem = new List<PrdFinishingChemicalSetupDetail>();
                var DetailItems = new List<PrdFinishingChemicalSetupDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmFinishingChemicalPreparation item = new vmFinishingChemicalPreparation();
                vmFinishingChemicalPreparation items = new vmFinishingChemicalPreparation();
                PrdFinishingChemicalSetupDetail itemdel = new PrdFinishingChemicalSetupDetail();
                //***************************************End Create Model Instance to get Data***********************************

                var SDetail = Detail.Where(x => x.FinChemicalStupDetailID == 0).ToList();
                var UDetail = Detail.Where(x => x.FinChemicalStupDetailID != 0).ToList();
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.FinChemicalStupID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_PrdFinishingChemicalSetup_EF.getMaxID("PrdFinishingChemicalSetup"));
                            DetailId = Convert.ToInt64(GenericFactory_PrdFinishingChemicalSetupDetail_EF.getMaxID("PrdFinishingChemicalSetupDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            CustomNo = GenericFactory_PrdFinishingChemicalSetup_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                ChemicalNo = MasterId.ToString();
                            }
                            else
                            {
                                ChemicalNo = CustomNo;
                            }

                            MasterItem = new PrdFinishingChemicalSetup
                            {
                                FinChemicalStupID = (int)MasterId,
                                FinishingProcessID = Master.FinishingProcessID,
                                FinChemicalStupNo = (int)MasterId,
                                Date = Master.PreparationDate,
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
                                var Detailitem = new PrdFinishingChemicalSetupDetail
                                {
                                    FinChemicalStupDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                    FinChemicalStupID = (int)MasterId,
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
                            var MasterAll = GenericFactory_PrdFinishingChemicalSetup_EF.GetAll().Where(x => x.FinChemicalStupID == Master.FinChemicalStupID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GenericFactory_PrdFinishingChemicalSetupDetail_EF.GetAll().Where(x => x.FinChemicalStupID == Master.FinChemicalStupID && x.CompanyID == objcmnParam.loggedCompany).ToArray();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.FinChemicalStupID == Master.FinChemicalStupID);
                            MasterItem.FinishingProcessID = (int)Master.FinishingProcessID;
                            //MasterItem.FinChemicalStupNo = int.Parse(Master.FinChemicalStupNo);
                            MasterItem.Date = (DateTime)Master.PreparationDate;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < UDetail.Count; i++)
                            {
                                item = UDetail[i];
                                foreach (PrdFinishingChemicalSetupDetail d in DetailAll.Where(d => d.FinChemicalStupID == Master.FinChemicalStupID && d.FinChemicalStupDetailID == item.FinChemicalStupDetailID))
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
                                    DetailId = Convert.ToInt64(GenericFactory_PrdFinishingChemicalSetupDetail_EF.getMaxID("PrdFinishingChemicalSetupDetail"));
                                    FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                                    OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                                    var Detailitems = new PrdFinishingChemicalSetupDetail
                                    {
                                        FinChemicalStupDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                        FinChemicalStupID = (int)Master.FinChemicalStupID,
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
                                    GenericFactory_PrdFinishingChemicalSetupDetail_EF.updateMaxID("PrdFinishingChemicalSetupDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits)));
                                }
                            }

                            if (UDetail.Count < DetailAll.Count())
                            {
                                for (int i = 0; i < DetailAll.Count(); i++)
                                {
                                    itemdel = DetailAll[i];

                                    var delDetail = (from del in DetailItem.Where(x => x.FinChemicalStupDetailID == itemdel.FinChemicalStupDetailID) select del.FinChemicalStupDetailID).FirstOrDefault();
                                    if (delDetail != itemdel.FinChemicalStupDetailID)
                                    {
                                        var tem = DetailAll.FirstOrDefault(d => d.FinChemicalStupID == Master.FinChemicalStupID && d.FinChemicalStupDetailID == itemdel.FinChemicalStupDetailID);
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

                        if (Master.FinChemicalStupID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingChemicalSetup_EF.Update(MasterItem);
                                GenericFactory_PrdFinishingChemicalSetup_EF.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.UpdateList(DetailItem.ToList());
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.Save();
                            }
                            if (DetailItems != null && DetailItems.Count != 0)
                            {
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.InsertList(DetailItems.ToList());
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingChemicalSetup_EF.Insert(MasterItem);
                                GenericFactory_PrdFinishingChemicalSetup_EF.Save();
                                GenericFactory_PrdFinishingChemicalSetup_EF.updateMaxID("PrdFinishingChemicalSetup", Convert.ToInt64(MasterId));
                                GenericFactory_PrdFinishingChemicalSetup_EF.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.InsertList(DetailItem.ToList());
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.Save();
                                GenericFactory_PrdFinishingChemicalSetupDetail_EF.updateMaxID("PrdFinishingChemicalSetupDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));                                
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

        public string DelUpdateFiniChemicalMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                var MasterItem = new PrdFinishingChemicalSetup();
                var DetailItem = new List<PrdFinishingChemicalSetupDetail>();

                //For Update Master Detail
                var MasterAll = GenericFactory_PrdFinishingChemicalSetup_EF.GetAll().Where(x => x.FinChemicalStupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_PrdFinishingChemicalSetupDetail_EF.GetAll().Where(x => x.FinChemicalStupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.FinChemicalStupID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdFinishingChemicalSetupDetail d in DetailAll.Where(d => d.FinChemicalStupID == objcmnParam.id))
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
                        GenericFactory_PrdFinishingChemicalSetup_EF.Update(MasterItem);
                        GenericFactory_PrdFinishingChemicalSetup_EF.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_PrdFinishingChemicalSetupDetail_EF.UpdateList(DetailItem.ToList());
                        GenericFactory_PrdFinishingChemicalSetupDetail_EF.Save();
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

        public object[] GetChemicalPreparation(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            object[] objChemicalPreparation = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                if (objcmnParam != null)
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        objChemicalPreparation = (
                                    from fs in _ctxCmn.PrdFinishingChemicalSetups
                                    where fs.CompanyID == objcmnParam.loggedCompany
                                    select new
                                    {
                                        FinChemicalStupID = fs.FinChemicalStupID,
                                        FinChemicalStupNo = fs.FinChemicalStupNo,
                                        FinishingProcessID = fs.FinishingProcessID,
                                        Date = fs.Date.ToString(),
                                        Remarks = fs.Remarks,
                                        objChemicalPreparationDT = from dt in _ctxCmn.PrdFinishingChemicalSetupDetails
                                                                 where dt.FinChemicalStupID == fs.FinChemicalStupID && dt.CompanyID == objcmnParam.loggedCompany
                                                                 select new
                                                                 {
                                                                     FinChemicalStupDetailID = dt.FinChemicalStupDetailID,
                                                                     ChemicalID = dt.ChemicalID,
                                                                     MaxQty = dt.MaxQty,
                                                                     MinQty = dt.MinQty,
                                                                     UnitID = dt.UnitID
                                                                 }
                                    }).ToArray();

                        recordsTotal = _ctxCmn.PrdFinishingChemicalSetups.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objChemicalPreparation;
        }
        #endregion
    }
}
