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
    public class FebricInspectionMgt : iFebricInspection
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmFabricInspection> GenericFactory_PrdFabricInspection_vm;
        //private iGenericFactory_EF<PrdInternalIssueDetail> GenericFactory_EFInternalIssueDetails = null;
        private iGenericFactory_EF<PrdFinishingInspactionDetail> GenericFactory_EFFinishingInspactionDetails = null;
        private iGenericFactory_EF<PrdFinishingInspactionMaster> GenericFactory_EFFinishingInspactionMaster = null;
        private iGenericFactory<vmFabricInspectionMaster> GenericFactory_FabricINspectionMaster = null;

        public vmFabricInspection Get_FabricInspectionByStyle(vmCmnParameters objcmnParam)
        {
            vmFabricInspection _objFabricInspection = null;
            try
            {
                GenericFactory_PrdFabricInspection_vm = new PrdFabricInspection_VM();
                string spQuery = string.Empty;
                Hashtable ht = new Hashtable();
                ht.Add("FinishingMRRID", objcmnParam.id); // FinsihingMRRID is Style
                spQuery = "[Get_FabricInspectionByStyle]";
                _objFabricInspection = GenericFactory_PrdFabricInspection_vm.ExecuteQuery(spQuery, ht).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objFabricInspection;
        }

        //public int SaveUpdateFebricInspection(PrdFinishingInspactionMaster _objFinishingInspactionMaster, List<vmFinishingInspactionDetail> _objNewInspactionDetails, List<vmFinishingInspactionDetail> _objDeleteInsPectiondetails, vmCmnParameters objcmnParam)
        //{
        //    Int64 InspactionID = 0;
        //    Int64 InspactionDateilID = 0;
        //    int status = 0;
        //    try
        //    {
        //        if (_objFinishingInspactionMaster.InspactionID == 0)
        //        {
        //            InspactionID = SaveInspactionMaster(_objFinishingInspactionMaster, objcmnParam);
        //            if (InspactionID > 0)
        //            {
        //                InspactionDateilID = SaveInspactionDetail(_objNewInspactionDetails, _objFinishingInspactionMaster, InspactionID, objcmnParam);

        //                if (InspactionDateilID > 0)
        //                {
        //                    status = 1;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            status = UpdateFebricInspection(_objNewInspactionDetails, _objDeleteInsPectiondetails, _objFinishingInspactionMaster, objcmnParam);
        //        }

        //    }
        //    catch(Exception e)
        //    {
        //        status = 0;
        //        e.ToString();
        //    }
        //    return status;
        //}

        //private int UpdateFebricInspection(List<vmFinishingInspactionDetail> _objInspactionDetailsList, List<vmFinishingInspactionDetail> _objDeleteInsPectiondetails, PrdFinishingInspactionMaster _objFinishingInspactionMaster, vmCmnParameters objcmnParam)
        //{
        //    int status = 0;
        //    try
        //    {
        //        if (_objInspactionDetailsList.Count > 0)
        //        {
        //            SaveInspactionDetail(_objInspactionDetailsList, _objFinishingInspactionMaster, _objFinishingInspactionMaster.InspactionID, objcmnParam);
        //        }
        //        if (_objInspactionDetailsList.Count > 0)
        //        {
        //            UpdateInspactionDetail(_objInspactionDetailsList, _objFinishingInspactionMaster, objcmnParam);

        //        }
        //        if (_objDeleteInsPectiondetails.Count > 0)
        //        {

        //            DeleteInspactionDetail(_objDeleteInsPectiondetails, _objFinishingInspactionMaster, objcmnParam);
        //        }
        //        status = 1;
        //    }
        //    catch(Exception e)
        //    {
        //        status = 0;
        //        e.ToString();
        //    }
        //    return status;

        //}

        //private int DeleteInspactionDetail(List<vmFinishingInspactionDetail> _objDeleteInsPectiondetails, PrdFinishingInspactionMaster _objFinishingInspactionMaster, vmCmnParameters objcmnParam)
        //{
        //    int status = 0;
        //    try
        //    {
        //        GenericFactory_EFFinishingInspactionDetails = new PrdFinishingInspactionDetail_EF();
        //        foreach (vmFinishingInspactionDetail aitem in _objDeleteInsPectiondetails)
        //        {
        //            PrdFinishingInspactionDetail _FabricInspactionDetail = GenericFactory_EFFinishingInspactionDetails.GetAll().Where(x => x.InspactionDateilID == aitem.InspactionDateilID).FirstOrDefault();
        //            _FabricInspactionDetail.DeleteBy = objcmnParam.loggeduser;
        //            _FabricInspactionDetail.IsDeleted = true;
        //            _FabricInspactionDetail.DeleteOn = DateTime.Now;
        //            _FabricInspactionDetail.DeletePc =  HostService.GetIP();
        //            GenericFactory_EFFinishingInspactionDetails.Update(_FabricInspactionDetail);
        //            GenericFactory_EFFinishingInspactionDetails.Save();

        //        }
        //        status = 1;
        //    }
        //    catch(Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return status;
        //}

        //private int UpdateInspactionDetail(List<vmFinishingInspactionDetail> _objUpdateInspectionDetails, PrdFinishingInspactionMaster _objFinishingInspactionMaster, vmCmnParameters objcmnParam)
        //{
        //    int status = 0;
        //    try
        //    {
        //        GenericFactory_EFFinishingInspactionDetails = new PrdFinishingInspactionDetail_EF();
        //        foreach (vmFinishingInspactionDetail aitem in _objUpdateInspectionDetails.Where(x => x.ModelStatus == "Update"))
        //        {                    
        //            PrdFinishingInspactionDetail _FabricInspactionDetail = GenericFactory_EFFinishingInspactionDetails.GetAll().Where(x => x.InspactionDateilID == aitem.InspactionDateilID).FirstOrDefault();
        //            _FabricInspactionDetail.BeamNo = aitem.BeamNo;
        //            _FabricInspactionDetail.RollNo = aitem.RollNo;
        //            _FabricInspactionDetail.Length = aitem.GreigeLength;
        //            _FabricInspactionDetail.Piece = aitem.Piece;
        //            _FabricInspactionDetail.DefecetPointID = aitem.DefectPoint;
        //            _FabricInspactionDetail.GrossWeight = aitem.GrossWt;
        //            _FabricInspactionDetail.NetWeight = aitem.NetWt;
        //            _FabricInspactionDetail.Remarks = aitem.Remarks;

        //            _FabricInspactionDetail.UpdateBy = objcmnParam.loggeduser;
        //            _FabricInspactionDetail.UpdateOn = DateTime.Now;
        //            _FabricInspactionDetail.UpdatePc =  HostService.GetIP();
        //            GenericFactory_EFFinishingInspactionDetails.Update(_FabricInspactionDetail);
        //            GenericFactory_EFFinishingInspactionDetails.Save();
        //        }
        //        status = 1;
        //    }
        //    catch(Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return status;
        //}

        //private Int64 SaveInspactionDetail(List<vmFinishingInspactionDetail> _objNewInspactionDetails, PrdFinishingInspactionMaster _objFinishingInspactionMaster, Int64 InspactionID, vmCmnParameters objcmnParam)
        //{
        //    GenericFactory_EFFinishingInspactionDetails = new PrdFinishingInspactionDetail_EF();
        //    Int64 NextId = 0;
        //    NextId = GenericFactory_EFFinishingInspactionDetails.getMaxVal_int64("InspactionDateilID", "PrdFinishingInspactionDetail");
        //    foreach (vmFinishingInspactionDetail aitem in _objNewInspactionDetails.Where(x=>x.ModelStatus=="New"))
        //    {
        //        PrdFinishingInspactionDetail _objInspactionDetail = new PrdFinishingInspactionDetail();
        //        _objInspactionDetail.InspactionDateilID = NextId;
        //        _objInspactionDetail.InspactionID = InspactionID;
        //        _objInspactionDetail.BeamNo = aitem.BeamNo;
        //        _objInspactionDetail.RollNo = aitem.RollNo;
        //        _objInspactionDetail.Length = aitem.GreigeLength;
        //        _objInspactionDetail.Piece = aitem.Piece;
        //        _objInspactionDetail.DefecetPointID = aitem.DefectPoint;
        //        _objInspactionDetail.GrossWeight = aitem.GrossWt;
        //        _objInspactionDetail.NetWeight = aitem.NetWt;
        //        _objInspactionDetail.Remarks = aitem.Remarks;

        //        _objInspactionDetail.CompanyID = objcmnParam.loggedCompany;
        //        _objInspactionDetail.CreateBy = objcmnParam.loggeduser;
        //        _objInspactionDetail.CreateOn = DateTime.Now;
        //        _objInspactionDetail.CreatePc =  HostService.GetIP();
        //        _objInspactionDetail.IsDeleted = false;
        //        GenericFactory_EFFinishingInspactionDetails.Insert(_objInspactionDetail);
        //        GenericFactory_EFFinishingInspactionDetails.Save();
        //        NextId++;
        //    }
        //    return NextId;
        //}

        //private Int64 SaveInspactionMaster(PrdFinishingInspactionMaster _objFinishingInspactionMaster, vmCmnParameters objcmnParam)
        //{
        //    Int64 status = 0;
        //    Int64 NextId = 0;
        //    try
        //    {
        //        GenericFactory_EFFinishingInspactionMaster = new PrdFinishingInspactionMaster_EF();
        //        NextId = GenericFactory_EFFinishingInspactionMaster.getMaxVal_int64("InspactionID", "PrdFinishingInspactionMaster");
        //        _objFinishingInspactionMaster.InspactionID = NextId;
        //        _objFinishingInspactionMaster.InspactionNo = NextId.ToString();

        //        _objFinishingInspactionMaster.CompanyID = objcmnParam.loggedCompany;
        //        _objFinishingInspactionMaster.CreateBy = objcmnParam.loggeduser;
        //        _objFinishingInspactionMaster.CreateOn = DateTime.Now;
        //        _objFinishingInspactionMaster.CreatePc =  HostService.GetIP();
        //        _objFinishingInspactionMaster.IsDeleted = false;
        //        _objFinishingInspactionMaster.IsQAComplete = false;
        //        GenericFactory_EFFinishingInspactionMaster.Insert(_objFinishingInspactionMaster);
        //        GenericFactory_EFFinishingInspactionMaster.Save();

        //        status = NextId;
        //    }
        //    catch(Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return status;
        //}        

        public List<vmFabricInspectionMaster> FabricInspectionDetails(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_FabricINspectionMaster = new PrdFabricInspectionMaster_VM();
            List<vmFabricInspectionMaster> _objFabricInspectionMasters = null;
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

                    spQuery = "[Get_FabricInspectionDetails]";
                    _objFabricInspectionMasters = GenericFactory_FabricINspectionMaster.ExecuteQuery(spQuery, ht).ToList();
                    recordsTotal = _ctxCmn.PrdWeavingMachinConfigs.Where(x=> x.CompanyID==objcmnParam.loggedCompany && x.IsDeleted==false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objFabricInspectionMasters;
        }
        public vmFabricInspectionMaster GetFebricInspectionByInspectionID(vmCmnParameters objcmnParam)
        {
            GenericFactory_FabricINspectionMaster = new PrdFabricInspectionMaster_VM();
            vmFabricInspectionMaster _objFabricInspectionMasters = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("InspactionID", objcmnParam.id);
                spQuery = "[Get_FabricInspectionDetailsByID]";
                _objFabricInspectionMasters = GenericFactory_FabricINspectionMaster.ExecuteQuery(spQuery, ht).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objFabricInspectionMasters;
        }

        public List<vmFinishingInspactionDetail> GetFebricInspectionDetailsID(vmCmnParameters objcmnParam)
        {
            List<vmFinishingInspactionDetail> _finishingInspactionDetails = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    _finishingInspactionDetails = (from _fid in _ctxCmn.PrdFinishingInspactionDetails                                                   
                                                   where _fid.InspactionID == objcmnParam.id && _fid.IsDeleted == false
                                                   select new vmFinishingInspactionDetail
                                                   {
                                                       InspactionID = _fid.InspactionID,
                                                       InspactionDateilID = _fid.InspactionDateilID,
                                                       RollNo = _fid.RollNo,
                                                       GreigeLength = _fid.Length,
                                                       Piece = _fid.Piece,
                                                       DefectPoint = _fid.DefecetPointID,
                                                       GrossWt = _fid.GrossWeight,
                                                       NetWt = _fid.NetWeight,
                                                       Remarks = _fid.Remarks,
                                                       //ModelStatus = "Update",
                                                       BeamNo = _fid.BeamNo,
                                                       UnitID=_fid.UnitID
                                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _finishingInspactionDetails;
        }

        public string SaveUpdateFebricInspection(PrdFinishingInspactionMaster Master, List<vmFinishingInspactionDetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0; string CustomNo = string.Empty, InspactionNo = string.Empty;
                //***************************************End Initialize Variable*************************************************

                //**************************Start Initialize Generic Repository Based on table***********************************
                GenericFactory_EFFinishingInspactionMaster = new PrdFinishingInspactionMaster_EF();
                GenericFactory_EFFinishingInspactionDetails = new PrdFinishingInspactionDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new PrdFinishingInspactionMaster();
                var DetailItem = new List<PrdFinishingInspactionDetail>();
                var DetailItems = new List<PrdFinishingInspactionDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                vmFinishingInspactionDetail item = new vmFinishingInspactionDetail();
                vmFinishingInspactionDetail items = new vmFinishingInspactionDetail();
                PrdFinishingInspactionDetail itemdel = new PrdFinishingInspactionDetail();
                //***************************************End Create Model Instance to get Data***********************************

                var SDetail = Detail.Where(x => x.InspactionID == 0).ToList();
                var UDetail = Detail.Where(x => x.InspactionID != 0).ToList();
                //**************************************************Start Main Operation************************************************
                if (Detail.Count > 0)
                {
                    try
                    {
                        if (Master.InspactionID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate Master & Detail ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_EFFinishingInspactionMaster.getMaxID("PrdFinishingInspactionMaster"));
                            DetailId = Convert.ToInt64(GenericFactory_EFFinishingInspactionDetails.getMaxID("PrdFinishingInspactionDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate Master & Detail ID*****************************************

                            CustomNo = GenericFactory_EFFinishingInspactionMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                            if (CustomNo == null || CustomNo == "")
                            {
                                InspactionNo = MasterId.ToString();
                            }
                            else
                            {
                                InspactionNo = CustomNo;
                            }

                            MasterItem = new PrdFinishingInspactionMaster
                            {
                                InspactionID = MasterId,
                                InspactionNo = InspactionNo,
                                FinishingMRRID = Master.FinishingMRRID,
                                ItemID = Master.ItemID,
                                SetID = Master.SetID,
                                WeivingMRRID = Master.WeivingMRRID,
                                SizeMRRID = Master.SizeMRRID,
                                MachineConfigID = Master.MachineConfigID,
                                ShiftID = Master.ShiftID,
                                PlateID = Master.PlateID,
                                OperatorID = Master.OperatorID,
                                Date = Master.Date,
                                Remarks = Master.Remarks,
                                IsQAComplete = false,
                                IsDeleted = false,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP()
                            };

                            for (int i = 0; i < Detail.Count; i++)
                            {
                                item = Detail[i];
                                var Detailitem = new PrdFinishingInspactionDetail
                                {
                                    InspactionDateilID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    InspactionID = MasterId,
                                    BeamNo = item.BeamNo,
                                    RollNo = item.RollNo,
                                    Length = item.GreigeLength,
                                    UnitID = item.UnitID,
                                    Piece = item.Piece,
                                    DefecetPointID = item.DefectPoint,
                                    GrossWeight = item.GrossWt,
                                    NetWeight = item.NetWt,
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
                            var MasterAll = GenericFactory_EFFinishingInspactionMaster.GetAll().Where(x => x.InspactionID == Master.InspactionID && x.CompanyID == objcmnParam.loggedCompany);
                            var DetailAll = GenericFactory_EFFinishingInspactionDetails.GetAll().Where(x => x.InspactionID == Master.InspactionID && x.CompanyID == objcmnParam.loggedCompany).ToList();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.InspactionID == Master.InspactionID);
                            MasterItem.FinishingMRRID = Master.FinishingMRRID;
                            MasterItem.ItemID = Master.ItemID;
                            MasterItem.SetID = Master.SetID;
                            MasterItem.WeivingMRRID = Master.WeivingMRRID;
                            MasterItem.SizeMRRID = Master.SizeMRRID;
                            MasterItem.MachineConfigID = Master.MachineConfigID;
                            MasterItem.ShiftID = Master.ShiftID;
                            MasterItem.PlateID = Master.PlateID;
                            MasterItem.OperatorID = Master.OperatorID;
                            MasterItem.Date = Master.Date;
                            MasterItem.Remarks = Master.Remarks;

                            MasterItem.CompanyID = objcmnParam.loggedCompany;
                            MasterItem.UpdateBy = objcmnParam.loggeduser;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < UDetail.Count; i++)
                            {
                                item = UDetail[i];
                                foreach (PrdFinishingInspactionDetail d in DetailAll.Where(d => d.InspactionID == Master.InspactionID && d.InspactionDateilID == item.InspactionDateilID))
                                {
                                    d.BeamNo = item.BeamNo;
                                    d.RollNo = item.RollNo;
                                    d.Length = item.GreigeLength;
                                    d.UnitID = item.UnitID;
                                    d.Piece = item.Piece;
                                    d.DefecetPointID = item.DefectPoint;
                                    d.GrossWeight = item.GrossWt;
                                    d.NetWeight = item.NetWt;
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
                            if (SDetail != null && SDetail.Count != 0)
                            {
                                for (int i = 0; i < SDetail.Count; i++)
                                {
                                    item = SDetail[i];
                                    DetailId = Convert.ToInt64(GenericFactory_EFFinishingInspactionDetails.getMaxID("PrdFinishingInspactionDetail"));
                                    FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                                    OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                                    var Detailitems = new PrdFinishingInspactionDetail
                                    {
                                        InspactionDateilID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                        InspactionID = Master.InspactionID,
                                        BeamNo = item.BeamNo,
                                        RollNo = item.RollNo,
                                        Length = item.GreigeLength,
                                        UnitID = item.UnitID,
                                        Piece = item.Piece,
                                        DefecetPointID = item.DefectPoint,
                                        GrossWeight = item.GrossWt,
                                        NetWeight = item.NetWt,
                                        Remarks = item.Remarks,

                                        CompanyID = objcmnParam.loggedCompany,
                                        CreateBy = objcmnParam.loggeduser,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItems.Add(Detailitems);
                                    GenericFactory_EFFinishingInspactionDetails.updateMaxID("PrdFinishingInspactionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits)));
                                }
                            }

                            if (UDetail.Count < DetailAll.Count())
                            {
                                for (int i = 0; i < DetailAll.Count(); i++)
                                {
                                    itemdel = DetailAll[i];

                                    var delDetail = (from del in DetailItem.Where(x => x.InspactionDateilID == itemdel.InspactionDateilID) select del.InspactionDateilID).FirstOrDefault();
                                    if (delDetail != itemdel.InspactionDateilID)
                                    {
                                        var tem = DetailAll.FirstOrDefault(d => d.InspactionID == Master.InspactionID && d.InspactionDateilID == itemdel.InspactionDateilID);
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

                        if (Master.InspactionID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_EFFinishingInspactionMaster.Update(MasterItem);
                                GenericFactory_EFFinishingInspactionMaster.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_EFFinishingInspactionDetails.UpdateList(DetailItem.ToList());
                                GenericFactory_EFFinishingInspactionDetails.Save();
                            }
                            if (DetailItems != null && DetailItems.Count != 0)
                            {
                                GenericFactory_EFFinishingInspactionDetails.InsertList(DetailItems.ToList());
                                GenericFactory_EFFinishingInspactionDetails.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_EFFinishingInspactionMaster.Insert(MasterItem);
                                GenericFactory_EFFinishingInspactionMaster.Save();
                                GenericFactory_EFFinishingInspactionMaster.updateMaxID("PrdFinishingInspactionMaster", Convert.ToInt64(MasterId));
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_EFFinishingInspactionDetails.InsertList(DetailItem.ToList());
                                GenericFactory_EFFinishingInspactionDetails.Save();
                                GenericFactory_EFFinishingInspactionDetails.updateMaxID("PrdFinishingInspactionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
                        }

                        transaction.Complete();
                        result = MasterItem.InspactionNo;
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

        public string DeleteUpdateFabricInspectionMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GenericFactory_EFFinishingInspactionMaster = new PrdFinishingInspactionMaster_EF();
                GenericFactory_EFFinishingInspactionDetails = new PrdFinishingInspactionDetail_EF();

                var MasterItem = new PrdFinishingInspactionMaster();
                var DetailItem = new List<PrdFinishingInspactionDetail>();

                //For Update Master Detail
                var MasterAll = GenericFactory_EFFinishingInspactionMaster.GetAll().Where(x => x.InspactionID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_EFFinishingInspactionDetails.GetAll().Where(x => x.InspactionID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.InspactionID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdFinishingInspactionDetail d in DetailAll.Where(d => d.InspactionID == objcmnParam.id))
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
                        GenericFactory_EFFinishingInspactionMaster.Update(MasterItem);
                        GenericFactory_EFFinishingInspactionMaster.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_EFFinishingInspactionDetails.UpdateList(DetailItem.ToList());
                        GenericFactory_EFFinishingInspactionDetails.Save();
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

