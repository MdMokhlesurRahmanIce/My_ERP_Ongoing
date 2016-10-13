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
    public class QAMgt : iQAMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmFinishingInspactionDetail> GenericFactory_FabricINspectionMaster_vm = null;
        private iGenericFactory_EF<PrdFinishingQAMaster> GenericFactory_PrdFinishingQAMaster_EF = null;
        private iGenericFactory_EF<PrdFinishingQADetail> GenericFactory_PrdFinishingQADetail_EF = null;

        public List<vmFinishingInspactionDetail> GetInspactionDetailsByIDAndDates(vmCmnParameters objcmnParam)
        {
            List<vmFinishingInspactionDetail> _objInspection = null;
            try
            {
                GenericFactory_FabricINspectionMaster_vm = new vmFinishingInspactionDetail_VM();
                string spQuery = string.Empty;
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("ItemID", objcmnParam.id);
                ht.Add("QAID", objcmnParam.ItemType);
                ht.Add("FromDate", objcmnParam.FromDate);
                ht.Add("ToDate", objcmnParam.ToDate);
                spQuery = "[Get_InspactionDetailsByIDAndDates]";
                _objInspection = GenericFactory_FabricINspectionMaster_vm.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objInspection;
        }

        public List<vmFinishingInspactionDetail> GetQAMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmFinishingInspactionDetail> _objQAMasterList = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    GenericFactory_FabricINspectionMaster_vm = new vmFinishingInspactionDetail_VM();
                    string spQuery = string.Empty;
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("PageNo", objcmnParam.pageNumber);
                    ht.Add("RowCountPerPage", objcmnParam.pageSize);
                    ht.Add("IsPaging", objcmnParam.IsPaging);

                    spQuery = "[Get_FinishingQAMaster]";
                    _objQAMasterList = GenericFactory_FabricINspectionMaster_vm.ExecuteQuery(spQuery, ht).ToList();
                    recordsTotal = _ctxCmn.PrdFinishingQAMasters.Where(x=> x.CompanyID==objcmnParam.loggedCompany && x.IsDeleted==false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objQAMasterList;
        }

        public string SaveUpdateQAMasterDetail(vmFinishingInspactionDetail Master, List<vmFinishingInspactionDetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0; string CustomNo = string.Empty, QANo = string.Empty;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdFinishingQAMaster_EF = new PrdFinishingQAMaster_EF();
                GenericFactory_PrdFinishingQADetail_EF = new PrdFinishingQADetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdFinishingQAMaster();
                var DetailItem = new List<PrdFinishingQADetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmFinishingInspactionDetail item = new vmFinishingInspactionDetail();
                //***************************************End Create Model Instance to get Data***********************************
               
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.QAID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_PrdFinishingQAMaster_EF.getMaxID("PrdFinishingQAMaster"));
                            DetailId = Convert.ToInt64(GenericFactory_PrdFinishingQADetail_EF.getMaxID("PrdFinishingQADetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            CustomNo = GenericFactory_PrdFinishingQAMaster_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                QANo = MasterId.ToString();
                            }
                            else
                            {
                                QANo = CustomNo;
                            }

                            MasterItem = new PrdFinishingQAMaster
                            {
                                QAID = MasterId,
                                QANo = QANo,
                                ItemID = (long)Master.ItemID,                                
                                QADate = (DateTime)Master.QADate,
                                FromDate = Master.FromDate,
                                ToDate = Master.ToDate,
                                Remarks = Master.Remarks,
                                QATypeID = objcmnParam.tTypeId,
                                IsDeleted = false,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP()
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdFinishingQADetail
                                {
                                    QADetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    QAID = MasterId,
                                    InspactionDateilID = (long)item.InspactionDateilID,
                                    InspactionID = (long)item.InspactionID,
                                    LPercent = item.LPercent,
                                    WPercent = item.WPercent,
                                    GradeID = item.ItemGradeID,
                                    IsNotDeliverable = item.IsNotDeliverable,
                                    Remarks = item.Remarks,

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
                            var MasterAll = GenericFactory_PrdFinishingQAMaster_EF.GetAll().Where(x => x.QAID == Master.QAID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GenericFactory_PrdFinishingQADetail_EF.GetAll().Where(x => x.QAID == Master.QAID && x.CompanyID == objcmnParam.loggedCompany).ToList();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.QAID == Master.QAID);
                            MasterItem.QADate = (DateTime)Master.QADate;
                            MasterItem.ItemID = (long)Master.ItemID;
                            MasterItem.FromDate = Master.FromDate;
                            MasterItem.ToDate = Master.ToDate;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.QATypeID = objcmnParam.tTypeId;
                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                foreach (PrdFinishingQADetail d in DetailAll.Where(d => d.QAID == Master.QAID && d.QADetailID == item.QADetailID))
                                {
                                    d.InspactionDateilID = (long)item.InspactionDateilID;
                                    d.InspactionID = (long)item.InspactionID;
                                    d.LPercent = item.LPercent;
                                    d.WPercent = item.WPercent;
                                    d.GradeID = item.ItemGradeID;
                                    d.IsNotDeliverable = item.IsNotDeliverable;
                                    d.Remarks = item.Remarks;

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

                        if (Master.QAID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingQAMaster_EF.Update(MasterItem);
                                GenericFactory_PrdFinishingQAMaster_EF.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingQADetail_EF.UpdateList(DetailItem.ToList());
                                GenericFactory_PrdFinishingQADetail_EF.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingQAMaster_EF.Insert(MasterItem);
                                GenericFactory_PrdFinishingQAMaster_EF.Save();
                                GenericFactory_PrdFinishingQAMaster_EF.updateMaxID("PrdFinishingQAMaster", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingQADetail_EF.InsertList(DetailItem.ToList());
                                GenericFactory_PrdFinishingQADetail_EF.Save();
                                GenericFactory_PrdFinishingQADetail_EF.updateMaxID("PrdFinishingQADetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
                        }

                        transaction.Complete();
                        result = MasterItem.QANo;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
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

        public string DeleteUpdateQAMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GenericFactory_PrdFinishingQAMaster_EF = new PrdFinishingQAMaster_EF();
                GenericFactory_PrdFinishingQADetail_EF = new PrdFinishingQADetail_EF();

                var MasterItem = new PrdFinishingQAMaster();
                var DetailItem = new List<PrdFinishingQADetail>();

                //For Update Master Detail
                var MasterAll = GenericFactory_PrdFinishingQAMaster_EF.GetAll().Where(x => x.QAID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_PrdFinishingQADetail_EF.GetAll().Where(x => x.QAID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.QAID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdFinishingQADetail d in DetailAll.Where(d => d.QAID == objcmnParam.id))
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
                        GenericFactory_PrdFinishingQAMaster_EF.Update(MasterItem);
                        GenericFactory_PrdFinishingQAMaster_EF.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_PrdFinishingQADetail_EF.UpdateList(DetailItem.ToList());
                        GenericFactory_PrdFinishingQADetail_EF.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.QANo;
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