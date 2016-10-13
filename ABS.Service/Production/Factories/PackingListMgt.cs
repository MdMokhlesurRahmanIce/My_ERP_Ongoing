using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
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
    public class PackingListMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmFinishingPackingListMasterDetail> GenericFactory_vmFinishingPackingListMasterDetail_GF = null;
        private iGenericFactory_EF<PrdFinishingPackingList> GenericFactory_PrdFinishingPackingList_EF = null;
        private iGenericFactory_EF<PrdFinishingPackingListDetail> GenericFactory_PrdFinishingPackingListDetail_EF = null;

        public vmFinishingPackingListMasterDetail GetPIBasedData(vmCmnParameters objcmnParam)
        {
            vmFinishingPackingListMasterDetail PIData = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    PIData = (from PIM in _ctxCmn.SalPIMasters
                              join LCD in _ctxCmn.SalLCDetails on PIM.PIID equals LCD.PIID
                              join LCM in _ctxCmn.SalLCMasters on LCD.LCID equals LCM.LCID
                              join CU in _ctxCmn.CmnUsers on PIM.BuyerID equals CU.UserID
                              where PIM.PIID == objcmnParam.id
                              select new vmFinishingPackingListMasterDetail
                              {
                                  PIID = PIM.PIID,
                                  PINO = PIM.PINO,
                                  LCNo = LCM.LCNo,
                                  ExportLCNo = LCM.ExportLCNo,
                                  BuyerName = CU.UserFullName
                              }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return PIData;
        }

        //public vmFinishingPackingListMasterDetail GetPIBasedData(vmCmnParameters objcmnParam)
        //{
        //    vmFinishingPackingListMasterDetail PIData = null;
        //    try
        //    {
        //        GenericFactory_vmFinishingPackingListMasterDetail_GF = new vmFinishingPackingListDetail_GF();
        //        string spQuery = string.Empty;
        //        Hashtable ht = new Hashtable();
        //        ht.Add("CompanyID", objcmnParam.loggedCompany);
        //        ht.Add("LoggedUser", objcmnParam.loggeduser);
        //        ht.Add("PIID", objcmnParam.id);
        //        ht.Add("PackingID", objcmnParam.ItemType);

        //        spQuery = "[Get_FinishingPackingPIBasedData]";
        //        PIData = GenericFactory_vmFinishingPackingListMasterDetail_GF.ExecuteQuerySingle(spQuery, ht);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return PIData;
        //}

        public List<vmFinishingPackingListMasterDetail> GetPackingMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmFinishingPackingListMasterDetail_GF = new vmFinishingPackingListDetail_GF();
            List<vmFinishingPackingListMasterDetail> ListPMaster = null;
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

                    spQuery = "[Get_PackingMasterList]";
                    ListPMaster = GenericFactory_vmFinishingPackingListMasterDetail_GF.ExecuteQuery(spQuery, ht).ToList();
                    recordsTotal = _ctxCmn.PrdFinishingPackingLists.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListPMaster;
        }

        public List<vmFinishingPackingListMasterDetail> GetPackingListDetailByID(vmCmnParameters objcmnParam)
        {
            List<vmFinishingPackingListMasterDetail> PDetail = null;
            try
            {
                GenericFactory_vmFinishingPackingListMasterDetail_GF = new vmFinishingPackingListDetail_GF();
                string spQuery = string.Empty;
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("FinishingMRRID", objcmnParam.id);
                ht.Add("PackingID", objcmnParam.ItemType);

                spQuery = "[Get_PackingListDetailByID]";
                PDetail = GenericFactory_vmFinishingPackingListMasterDetail_GF.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return PDetail;
        }

        public string SaveUpdatePackingListMasterDetail(vmFinishingPackingListMasterDetail Master, List<vmFinishingPackingListMasterDetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0; string CustomNo = string.Empty, PackingNo = string.Empty;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_PrdFinishingPackingList_EF = new PrdFinishingPackingList_EF();
                GenericFactory_PrdFinishingPackingListDetail_EF = new PrdFinishingPackingListDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdFinishingPackingList();
                var DetailItem = new List<PrdFinishingPackingListDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmFinishingPackingListMasterDetail item = new vmFinishingPackingListMasterDetail();
                //***************************************End Create Model Instance to get Data***********************************

                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.PackingID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_PrdFinishingPackingList_EF.getMaxID("PrdFinishingPackingList"));
                            DetailId = Convert.ToInt64(GenericFactory_PrdFinishingPackingListDetail_EF.getMaxID("PrdFinishingPackingListDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            CustomNo = GenericFactory_PrdFinishingPackingList_EF.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                PackingNo = MasterId.ToString();
                            }
                            else
                            {
                                PackingNo = CustomNo;
                            }

                            MasterItem = new PrdFinishingPackingList
                            {
                                PackingID = MasterId, 
                                PackingNo=PackingNo, 
                                PackingDate=(DateTime)Master.PackingDate, 
                                IsDCComplete=false, 
                                IsFDOComplete=false,
                                PIID = (long)Master.PIID,
                                FinishingMRRID = (long)Master.FinishingMRRID, 
                                ItemID=(long)Master.ItemID, 
                                Remarks =Master.Remarks,
                                IsDeleted = false,

                                PackingTypeID=objcmnParam.tTypeId,
                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP()
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdFinishingPackingListDetail
                                {
                                    PackingDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    PackingID = MasterId,
                                    QADetailID = (long)item.QADetailID,
                                    Shade = item.Shade,
                                    Length = item.Length,
                                    GWeight = item.GWeight,
                                    NWeight = item.NWeight,
                                    Shipment = item.Shipment,
                                    FiniWidth = item.FiniWidth,
                                    WPercent = item.WidthSr,
                                    DefectPointID = item.DefectPointID,
                                    IsNotDeliverable = false,
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
                            var MasterAll = GenericFactory_PrdFinishingPackingList_EF.GetAll().Where(x => x.PackingID == Master.PackingID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GenericFactory_PrdFinishingPackingListDetail_EF.GetAll().Where(x => x.PackingID == Master.PackingID && x.CompanyID == objcmnParam.loggedCompany).ToList();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.PackingID == Master.PackingID);
                            MasterItem.PackingDate = (DateTime)Master.PackingDate;
                            MasterItem.ItemID = (long)Master.ItemID;
                            MasterItem.PIID = (long)Master.PIID;
                            MasterItem.FinishingMRRID = (long)Master.FinishingMRRID;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.PackingTypeID = objcmnParam.tTypeId;
                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                foreach (PrdFinishingPackingListDetail d in DetailAll.Where(d => d.PackingID == Master.PackingID && d.PackingDetailID == item.PackingDetailID))
                                {
                                    d.QADetailID = (long)item.QADetailID;
                                    d.Shade = item.Shade;
                                    d.Length = item.Length;
                                    d.GWeight = item.GWeight;
                                    d.NWeight = item.NWeight;
                                    d.Shipment = item.Shipment;
                                    d.FiniWidth = item.FiniWidth;
                                    d.WPercent = item.WidthSr;
                                    d.DefectPointID = item.DefectPointID;
                                    d.Remarks = item.Remarks;

                                    d.CompanyID = objcmnParam.loggedCompany;
                                    d.UpdateBy = objcmnParam.loggeduser;
                                    d.UpdateOn = DateTime.Now;
                                    d.UpdatePc = HostService.GetIP();

                                    DetailItem.Add(d);
                                    break;
                                }
                            }
                            //***************************************************End Update Operation********************************************
                        }

                        if (Master.PackingID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingPackingList_EF.Update(MasterItem);
                                GenericFactory_PrdFinishingPackingList_EF.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingPackingListDetail_EF.UpdateList(DetailItem.ToList());
                                GenericFactory_PrdFinishingPackingListDetail_EF.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_PrdFinishingPackingList_EF.Insert(MasterItem);
                                GenericFactory_PrdFinishingPackingList_EF.Save();
                                GenericFactory_PrdFinishingPackingList_EF.updateMaxID("PrdFinishingPackingList", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_PrdFinishingPackingListDetail_EF.InsertList(DetailItem.ToList());
                                GenericFactory_PrdFinishingPackingListDetail_EF.Save();
                                GenericFactory_PrdFinishingPackingListDetail_EF.updateMaxID("PrdFinishingPackingListDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
                        }

                        transaction.Complete();
                        result = MasterItem.PackingNo;
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

        public string DeleteUpdatePackingMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GenericFactory_PrdFinishingPackingList_EF = new PrdFinishingPackingList_EF();
                GenericFactory_PrdFinishingPackingListDetail_EF = new PrdFinishingPackingListDetail_EF();

                var MasterItem = new PrdFinishingPackingList();
                var DetailItem = new List<PrdFinishingPackingListDetail>();

                //For Update Master Detail
                var MasterAll = GenericFactory_PrdFinishingPackingList_EF.GetAll().Where(x => x.PackingID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_PrdFinishingPackingListDetail_EF.GetAll().Where(x => x.PackingID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.PackingID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdFinishingPackingListDetail d in DetailAll.Where(d => d.PackingID == objcmnParam.id))
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
                        GenericFactory_PrdFinishingPackingList_EF.Update(MasterItem);
                        GenericFactory_PrdFinishingPackingList_EF.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_PrdFinishingPackingListDetail_EF.UpdateList(DetailItem.ToList());
                        GenericFactory_PrdFinishingPackingListDetail_EF.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.PackingNo;
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
