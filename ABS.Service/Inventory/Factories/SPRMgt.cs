using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System.Collections;
using ABS.Models.ViewModel.Inventory;
using System.Transactions;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.MenuMgt;
using ABS.Utility;

namespace ABS.Service.Inventory.Factories
{
    public class SPRMgt : iSPRMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactory_EF_User = null;
        private iGenericFactory_EF<InvRequisitionMaster> GenericFactory_EF_RequisitionMaster = null;
        private iGenericFactory_EF<InvRequisitionDetail> GenericFactory_EF_RequisitionDetail = null;
        private iGenericFactory<CmnTransactionType> GFactory_GF_CmnTransactionType = null;
        private iGenericFactory<vmCmnOrganogram> GFactory_GF_Department = null;
        private iGenericFactory<CmnCompany> GFactory_GF_Company = null;
        private iGenericFactory<CmnItemGroup> GFactory_GF_ItemGroup = null;
        private iGenericFactory<CmnItemMaster> GFactory_GF_Item = null;
        private iGenericFactory<vmRequisition> GFactory_GF_ItemRate = null;
        private iGenericFactory<vmRequisitionDetails> GFactory_GF_RequisitionDetail = null;
        private iGenericFactory<InvRequisitionMaster> GFactory_GF_RequisitionMaster = null;
        private iGenericFactory<InvRequisitionDetail> GFactory_GF_RequisitionDetailUpdate = null;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocument = null;
        private iGenericFactory_EF<CmnDocumentPath> GenericFactory_CmnDocumentPath = null;
        private iGenericFactory_EF<InvStockTransit>  GenericFactory_EF_InvStockTransit = null;
        private iGenericFactory_EF<CmnBatch> GenericFactoryFor_Batch = null;


        public IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging, int companyID)
        {
            IEnumerable<CmnCompany> objCompany = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objCompany = _ctxCmn.CmnCompanies.Where(m => m.IsDeleted == false && m.CompanyID != companyID).ToList().Select(m => new CmnCompany
                    {
                        CompanyID = m.CompanyID,
                        CompanyName = m.CompanyName,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objCompany;
        }

        public IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemGrade> objLot = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objLot = _ctxCmn.CmnItemGrades.Where(m => m.IsDeleted == false).ToList().Select(m => new CmnItemGrade
                    {
                        ItemGradeID = m.ItemGradeID,
                        GradeName = m.GradeName,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objLot;
        }
        public IEnumerable<vmUsers> GetAllSupplier(int? pageNumber, int? pageSize, int? IsPaging, int ItemId, int LoginCompanyID)
        {
            IEnumerable<vmUsers> objSupplier = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objSupplier = (from qcm in _ctxCmn.InvStockMasters.Where(s => s.IsDeleted == false && s.CompanyID == LoginCompanyID && s.ItemID == ItemId)                                  
                                    join usr in _ctxCmn.CmnUsers on qcm.SupplierID equals usr.UserID
                                   select new vmUsers
                                    {
                                        UserID = qcm.SupplierID,
                                        UserFullName = usr.UserFullName,
                                        LotNos = (from L in _ctxCmn.CmnLots
                                                  where L.ItemID == ItemId
                                                  select new vmLot { LotID = L.LotID, LotNo = L.LotNo }).ToList(),
                                        BatchNos = (from B in _ctxCmn.CmnBatches
                                                    where B.ItemID == ItemId
                                                    select new vmBatch { BatchID = B.BatchID, BatchNo = B.BatchNo }).ToList(),
                                        Suppliers = (from qm in _ctxCmn.InvStockMasters.Where(s => s.IsDeleted == false && s.CompanyID == LoginCompanyID && s.ItemID == ItemId)
                                                     join ur in _ctxCmn.CmnUsers on qm.SupplierID equals ur.UserID
                                                     select new vmUser
                                                     {
                                                         UserID = (int)qm.SupplierID,
                                                         UserFullName = ur.UserFullName,
                                                     }).ToList(),
                                    }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objSupplier;
        }

        #region GetBatch
        public IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging, int ItemId, int LoginCompanyID)
        {
            GenericFactoryFor_Batch = new CmnBatch_EF();
            IEnumerable<CmnBatch> BatchList = null;

            try
            {
                BatchList = GenericFactoryFor_Batch.GetAll().Select(m => new CmnBatch { BatchID = m.BatchID, BatchNo = m.BatchNo, IsDeleted = m.IsDeleted,ItemID = m.ItemID,CompanyID = m.CompanyID }).Where(m => m.IsDeleted == false && m.ItemID == ItemId && m.CompanyID == LoginCompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return BatchList;
        }
        #endregion

        public IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging, int ItemID, int LoginCompanyID)
        {          
            IEnumerable<CmnLot> objLot = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objLot = _ctxCmn.CmnLots.Where(m => m.IsDeleted == false && m.ItemID == ItemID && m.CompanyID == LoginCompanyID).ToList().Select(m => new CmnLot
                    {
                        LotID = m.LotID,
                        LotNo = m.LotNo,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objLot;
        }
        public IEnumerable<InvRequisitionMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno)
        {
            IEnumerable<InvRequisitionMaster> lstMNo = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstMNo = (from rm in _ctxCmn.InvRequisitionMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.ManualRequisitionNo == Mno)
                              select new
                              {
                                  RequisitionID = rm.RequisitionID,
                                  ManualRequisitionNo = rm.ManualRequisitionNo

                              }).ToList().Select(x => new InvRequisitionMaster
                              {
                                  RequisitionID = x.RequisitionID,
                                  ManualRequisitionNo = x.ManualRequisitionNo
                              }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstMNo;
        }
        public IEnumerable<vmRequisition> GetSPRList()
        {
            IEnumerable<vmRequisition> lstSPRMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstSPRMaster = _ctxCmn.InvRequisitionMasters.Where(m => m.IsDeleted == false && m.RequisitionTypeID == 8).Select(m => new vmRequisition { RequisitionID = m.RequisitionID, RequisitionNo = m.RequisitionNo, CompanyID = m.CompanyID, CreateBy = m.CreateBy, RequisitionTypeID = m.RequisitionTypeID }).ToList();
                    lstSPRMaster = lstSPRMaster.OrderBy(m => m.RequisitionID);
                }
              
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSPRMaster;
        }


        public IEnumerable<vmRequisition> GetSRList()
        {
            IEnumerable<vmRequisition> lstSRMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstSRMaster = _ctxCmn.InvRequisitionMasters.Where(m => m.IsDeleted == false && m.RequisitionTypeID == 6).Select(m => new vmRequisition { RequisitionID = m.RequisitionID, RequisitionNo = m.RequisitionNo, CompanyID = m.CompanyID, CreateBy = m.CreateBy, RequisitionTypeID = m.RequisitionTypeID }).ToList();
                    lstSRMaster = lstSRMaster.OrderBy(m => m.RequisitionID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSRMaster;
        }
        public IEnumerable<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? UserTypeID, int? CompanyID)
        {
            GenericFactory_EF_User = new CmnUser_EF();
            IEnumerable<CmnUser> objUsers = null;
            string spQuery = string.Empty;
            try
            {
                objUsers = GenericFactory_EF_User.GetAll().Select(m => new CmnUser { UserID = m.UserID, UserFullName = m.UserFullName, UserTypeID = m.UserTypeID, IsDeleted = m.IsDeleted, CompanyID = m.CompanyID }).Where(s => s.IsDeleted == false && s.UserTypeID == UserTypeID && s.CompanyID == CompanyID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUsers;
        }

        public IEnumerable<CmnTransactionType> GetAllRequisitionType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GFactory_GF_CmnTransactionType = new CmnTransactionType_GF();
            IEnumerable<CmnTransactionType> objRequisitionType = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_InvRequisitionType]";

                objRequisitionType = GFactory_GF_CmnTransactionType.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objRequisitionType;
        }


        public IEnumerable<CmnCompany> GetAllCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {

            GFactory_GF_Company = new CmnCompany_GF();
            IEnumerable<CmnCompany> objCompanylist = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_Company]";

                objCompanylist = GFactory_GF_Company.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCompanylist;
        }

        public IEnumerable<CmnItemGroup> GetAllItemGroup(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GFactory_GF_ItemGroup = new CmnItemGroup_GF();
            IEnumerable<CmnItemGroup> objItemGrouplist = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_ItemGroup]";
                objItemGrouplist = GFactory_GF_ItemGroup.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString(); 
            }
            return objItemGrouplist;
        }

        public IEnumerable<vmCmnOrganogram> GetDepartmentByCompanyID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GFactory_GF_Department = new vmCmnOrganogram_GF();
            IEnumerable<vmCmnOrganogram> objDepartment = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", CompanyID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_DepartmentByCompanyID]";

                objDepartment = GFactory_GF_Department.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDepartment;
        }

        public IEnumerable<CmnItemMaster> GetItemListByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? GroupID)
        {
            GFactory_GF_Item = new CmnItemMaster_GF();
            IEnumerable<CmnItemMaster> objItem = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("groupID", GroupID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_ItemByGroupID]";
                objItem = GFactory_GF_Item.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItem;
        }

        public IEnumerable<vmRequisition> LaodItemRateNUnit(int? pageNumber, int? pageSize, int? IsPaging, int? itemId)
        {
            GFactory_GF_ItemRate = new vmRequisition_GF();
            IEnumerable<vmRequisition> objItem = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ItemID", itemId);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                spQuery = "[dbo].[Get_ItemUnitNCurrentRate]";
                objItem = GFactory_GF_ItemRate.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItem;
        }

        //public IEnumerable<CmnCompany> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCompany> objCompany = null;
        //    try
        //    {

        //        //objCompany = GenericFactoryFor_CmnLot_EF.GetAll().Select(m => new CmnLot { LotID = m.LotID, LotNo = m.LotNo, IsDeleted = m.IsDeleted }).Where(m => m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objCompany;
        //}

        public IEnumerable<vmRequisition> GetRequisitionMaster(vmCmnParameters objcmnParam, out int recordsTotal, int TransactionTypeId)
        {
            IEnumerable<vmRequisition> _vmRequisition = null;
            IEnumerable<vmRequisition> lstRequisitionPaging = null;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    _vmRequisition = (from qcm in _ctxCmn.InvRequisitionMasters.Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany && s.RequisitionTypeID == TransactionTypeId)
                                            join org in _ctxCmn.CmnTransactionTypes on qcm.RequisitionTypeID equals org.TransactionTypeID
                                            join usr in _ctxCmn.CmnUsers on qcm.RequisitionBy equals usr.UserID
                                            join corg in _ctxCmn.CmnOrganograms on qcm.ToDepartmentID equals corg.OrganogramID
                                            select new vmRequisition
                                            {
                                                RequisitionID = qcm.RequisitionID,
                                                RequisitionNo = qcm.RequisitionNo,
                                                RequisitionDate = qcm.RequisitionDate,
                                                RequisitionTypeName = org.TransactionTypeName,
                                                RequisitionByName = usr.UserFullName,
                                                Remarks = qcm.Remarks,
                                                Purpose = qcm.Purpose,
                                                OrganogramName = corg.OrganogramName
                                            }).ToList();
                    lstRequisitionPaging = _vmRequisition.OrderByDescending(x => x.RequisitionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = _vmRequisition.Count();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstRequisitionPaging;
        }

        public IEnumerable<vmRequisition> GetRequisitionMasterSPR(vmCmnParameters objcmnParam, out int recordsTotal, int TransactionTypeId)
        {
            IEnumerable<vmRequisition> _vmRequisition = null;
            IEnumerable<vmRequisition> lstRequisitionPaging = null;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    _vmRequisition = (from qcm in _ctxCmn.InvRequisitionMasters.Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany && s.RequisitionTypeID == TransactionTypeId)
                                            join org in _ctxCmn.CmnTransactionTypes on qcm.RequisitionTypeID equals org.TransactionTypeID
                                            join usr in _ctxCmn.CmnUsers on qcm.RequisitionBy equals usr.UserID

                                            select new vmRequisition
                                            {
                                                RequisitionID = qcm.RequisitionID,
                                                RequisitionNo = qcm.RequisitionNo,
                                                RequisitionDate = qcm.RequisitionDate,
                                                EstDate = qcm.EstDate,
                                                RequisitionTypeName = org.TransactionTypeName,
                                                RequisitionByName = usr.UserFullName,
                                                ManualRequisitionNo = qcm.ManualRequisitionNo,
                                                Remarks = qcm.Remarks,
                                                Purpose = qcm.Purpose

                                            }).ToList();
                    lstRequisitionPaging = _vmRequisition.OrderByDescending(x => x.RequisitionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = _vmRequisition.Count();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstRequisitionPaging;
        }

        public string SaveSRequisitionMasterDetails(InvRequisitionMaster RequisitionMaster, List<vmRequisitionDetails> RequisitionDetails, UserCommonEntity commonEntity)
        {
            //Declerations
            string result = string.Empty; string customCode = string.Empty; string CustomNo = string.Empty;
            int RequisitionID = Convert.ToInt32(RequisitionMaster.RequisitionID), SDetailRowNum = 0, UDetailRowNum = 0;
            long FirstDigit = 0, OtherDigits = 0, nextDetailId = 0; int NextId = 0;
            long FirstTransitDigit = 0, OtherTransitDigits = 0, nextDetailTransitId = 0;
            GenericFactory_EF_RequisitionMaster = new InvRequisitionMaster_EF();
            GenericFactory_EF_RequisitionDetail = new InvRequisitionDetail_EF();
            GenericFactory_EF_InvStockTransit = new InvStockTransit_EF();
            List<InvRequisitionDetail> objRqDetails = new List<InvRequisitionDetail>();
            List<InvRequisitionDetail> SobjDetails = new List<InvRequisitionDetail>();
            List<InvStockTransit> objTransitDetail = new List<InvStockTransit>();
            List<InvStockTransit> objTransitUpdateDetail = new List<InvStockTransit>();
            SDetailRowNum = Convert.ToInt32(RequisitionDetails.Where(s => s.ModelState == "Save").Count());
            UDetailRowNum = Convert.ToInt32(RequisitionDetails.Where(s => s.ModelState == "Update").Count());
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //Transaction Occur here************************************************
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (RequisitionID > 0)
                        {

                            //**************Master Model************************************************
                            var lstInvReqMaster = GenericFactory_EF_RequisitionMaster.GetAll().FirstOrDefault(x => x.RequisitionID == RequisitionID);
                            lstInvReqMaster.UpdateBy = RequisitionMaster.CreateBy;
                            lstInvReqMaster.ManualRequisitionNo = RequisitionMaster.ManualRequisitionNo;
                            lstInvReqMaster.UpdateOn = DateTime.Now;
                            lstInvReqMaster.UpdatePc = HostService.GetIP();
                            lstInvReqMaster.RequisitionBy = RequisitionMaster.RequisitionBy;
                            lstInvReqMaster.RequisitionDate = RequisitionMaster.RequisitionDate;
                            lstInvReqMaster.EstDate = RequisitionMaster.EstDate;
                            lstInvReqMaster.ToDepartmentID = RequisitionMaster.ToDepartmentID;
                            lstInvReqMaster.Remarks = RequisitionMaster.Remarks;
                            lstInvReqMaster.Purpose = RequisitionMaster.Purpose;
                            result = lstInvReqMaster.RequisitionNo;

                            //*************Details Model************************************************
                            if (UDetailRowNum > 0)
                            {
                                foreach (vmRequisitionDetails ivrd in RequisitionDetails.Where(x => x.ModelState == "Update"))
                                {
                                    InvRequisitionDetail objRequisitionDetail = GenericFactory_EF_RequisitionDetail.GetAll().FirstOrDefault(x => x.RequisitionID == RequisitionID && x.RequisitionDetailID == ivrd.RequisitionDetailID);
                                    InvStockTransit objStockTransitDetail = GenericFactory_EF_InvStockTransit.GetAll().FirstOrDefault(x => x.TransactionID == RequisitionID && x.StockTransitID == ivrd.StockTransitID);
                                    objRequisitionDetail.RequisitionID = RequisitionMaster.RequisitionID;
                                    objRequisitionDetail.ItemID = ivrd.ItemID;
                                    objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                    objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                    objRequisitionDetail.Qty = ivrd.Qty;
                                    objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                    objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate;
                                    objRequisitionDetail.UpdateBy = RequisitionMaster.CreateBy;
                                    objRequisitionDetail.UpdateOn = DateTime.Now;
                                    objRequisitionDetail.UpdatePc = HostService.GetIP();
                                    objRqDetails.Add(objRequisitionDetail);

                                    //********************* Update Stock Transit ****************************
                                    objStockTransitDetail.TransactionID = RequisitionMaster.RequisitionID;
                                    objStockTransitDetail.ItemID = ivrd.ItemID;                                 
                                    objStockTransitDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objStockTransitDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                    objStockTransitDetail.TransitQty = ivrd.Qty;
                                    objStockTransitDetail.UpdateBy = (int)RequisitionMaster.CreateBy;
                                    objStockTransitDetail.UpdateOn = DateTime.Now;
                                    objStockTransitDetail.UpdatePc = HostService.GetIP();
                                    objTransitUpdateDetail.Add(objStockTransitDetail);
                                    //********************* Update Stock Transit ****************************
                                }
                            }
                            if (SDetailRowNum > 0)
                            {
                                nextDetailId = Convert.ToInt64(GenericFactory_EF_RequisitionDetail.getMaxID("InvRequisitionDetail"));
                                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                                //*******************************Stock Transit*******************************
                                nextDetailTransitId = Convert.ToInt64(GenericFactory_EF_InvStockTransit.getMaxID("InvStockTransit"));
                                FirstTransitDigit = Convert.ToInt64(nextDetailTransitId.ToString().Substring(0, 1));
                                OtherTransitDigits = Convert.ToInt64(nextDetailTransitId.ToString().Substring(1, nextDetailTransitId.ToString().Length - 1));
                                //*******************************Stock Transit*******************************

                                foreach (vmRequisitionDetails ivrd in RequisitionDetails.Where(x => x.ModelState == "Save"))
                                {                        
                                    InvRequisitionDetail objRequisitionDetail = new InvRequisitionDetail();
                                    InvStockTransit objStockTransit = new InvStockTransit();
                                    objRequisitionDetail.RequisitionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                    objRequisitionDetail.RequisitionID = RequisitionMaster.RequisitionID;
                                    objRequisitionDetail.ItemID = ivrd.ItemID;
                                    objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                    objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                    objRequisitionDetail.Qty = ivrd.Qty;
                                    objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                    objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate;
                                    objRequisitionDetail.CreateBy = RequisitionMaster.CreateBy;
                                    objRequisitionDetail.CreateOn = DateTime.Now;
                                    objRequisitionDetail.IsDeleted = false;
                                    objRequisitionDetail.CreatePc = HostService.GetIP();

                                    //*******************************Stock Transit*******************************
                                    objStockTransit.StockTransitID = Convert.ToInt64(FirstTransitDigit + "" + OtherTransitDigits);
                                    objStockTransit.TransactionID = RequisitionMaster.RequisitionID;
                                    objStockTransit.TransactionTypeID = RequisitionMaster.RequisitionTypeID;
                                    objStockTransit.ItemID = ivrd.ItemID;
                                    objStockTransit.TransitQty = ivrd.Qty;
                                    objStockTransit.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objStockTransit.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;                                 
                                    objStockTransit.IsComplete = false;                                  
                                    objStockTransit.CompanyID = RequisitionMaster.CompanyID;
                                    objStockTransit.DepartmentID = RequisitionMaster.DepartmentID;
                                    objStockTransit.CreateBy = (int)RequisitionMaster.CreateBy;
                                    objStockTransit.CreateOn = DateTime.Now;
                                    objStockTransit.IsDeleted = false;
                                    objStockTransit.CreatePc = HostService.GetIP();
                                    objTransitDetail.Add(objStockTransit);
                                    OtherTransitDigits++;

                                    //*******************************Stock Transit*******************************
                                    SobjDetails.Add(objRequisitionDetail);
                                    OtherDigits++;
                                }
                            }

                            //RequisitionID
                            //**************Master Transaction Update************************************************
                            if (lstInvReqMaster != null)
                            {
                                GenericFactory_EF_RequisitionMaster.Update(lstInvReqMaster);                             
                                GenericFactory_EF_RequisitionMaster.Save();
                            }
                            //RequisitionDetailID
                            // **************Details Transaction Update************************************************
                            if (objRqDetails.Count != 0)
                            {
                                GenericFactory_EF_RequisitionDetail.UpdateList(objRqDetails);
                                GenericFactory_EF_RequisitionDetail.Save();
                                GenericFactory_EF_InvStockTransit.UpdateList(objTransitUpdateDetail);
                                GenericFactory_EF_InvStockTransit.Save();
                               
                            }
                            if (SobjDetails.Count != 0)
                            {
                                GenericFactory_EF_RequisitionDetail.InsertList(SobjDetails);
                                GenericFactory_EF_RequisitionDetail.Save();
                                GenericFactory_EF_InvStockTransit.InsertList(objTransitDetail);
                                GenericFactory_EF_InvStockTransit.Save();
                                GenericFactory_EF_RequisitionDetail.updateMaxID("InvRequisitionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                                GenericFactory_EF_InvStockTransit.updateMaxID("InvStockTransit", Convert.ToInt64(FirstTransitDigit + "" + (OtherTransitDigits - 1)));
                            }
                                                                       
                            transaction.Complete();
                            result = "Update";
                        }
                        else
                        {
                            //Initialisation ************************************************
                            NextId = Convert.ToInt16(GenericFactory_EF_RequisitionMaster.getMaxID("InvRequisitionMaster"));
                            nextDetailId = Convert.ToInt64(GenericFactory_EF_RequisitionDetail.getMaxID("InvRequisitionDetail"));
                            FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                            //*******************************Stock Transit*******************************
                            nextDetailTransitId = Convert.ToInt64(GenericFactory_EF_InvStockTransit.getMaxID("InvStockTransit"));
                            FirstTransitDigit = Convert.ToInt64(nextDetailTransitId.ToString().Substring(0, 1));
                            OtherTransitDigits = Convert.ToInt64(nextDetailTransitId.ToString().Substring(1, nextDetailTransitId.ToString().Length - 1));
                            //*******************************Stock Transit*******************************

                            CustomNo = GenericFactory_EF_RequisitionMaster.getCustomCode(Convert.ToInt16(commonEntity.currentMenuID), Convert.ToDateTime(RequisitionMaster.RequisitionDate), RequisitionMaster.CompanyID, 1, 1);

                            if ((customCode != "") || (customCode != null))
                                customCode = CustomNo;
                            else
                                customCode = NextId.ToString();
                            //**************Master Model************************************************
                            RequisitionMaster.RequisitionID = NextId;
                            RequisitionMaster.CreateOn = DateTime.Now;
                            RequisitionMaster.CreatePc = HostService.GetIP();
                            RequisitionMaster.RequisitionNo = customCode;

                            //*************Details Model************************************************
                            foreach (vmRequisitionDetails ivrd in RequisitionDetails)
                            {
                                InvRequisitionDetail objRequisitionDetail = new InvRequisitionDetail();
                                InvStockTransit objStockTransit = new InvStockTransit();
                                objRequisitionDetail.RequisitionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits); 
                                objRequisitionDetail.IsGrrComplete = false;
                                objRequisitionDetail.RequisitionID = NextId;
                                objRequisitionDetail.ItemID = ivrd.ItemID;
                                objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                objRequisitionDetail.Qty = ivrd.Qty;
                                objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate; //ivrd.Amount;
                                objRequisitionDetail.CreateBy = RequisitionMaster.CreateBy;
                                objRequisitionDetail.CreateOn = DateTime.Now;
                                objRequisitionDetail.IsDeleted = false;
                                objRequisitionDetail.CreatePc = HostService.GetIP();

                                //*******************************Stock Transit*******************************
                                objStockTransit.StockTransitID = Convert.ToInt64(FirstTransitDigit + "" + OtherTransitDigits);
                                objStockTransit.ItemID = ivrd.ItemID;
                                objStockTransit.TransitQty = ivrd.Qty;
                                objStockTransit.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                objStockTransit.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                objStockTransit.TransactionID = NextId;
                                objStockTransit.IsComplete = false;                           
                                objStockTransit.TransactionTypeID = RequisitionMaster.RequisitionTypeID;                              
                                objStockTransit.CompanyID = RequisitionMaster.CompanyID;
                                objStockTransit.DepartmentID = RequisitionMaster.DepartmentID;
                                objStockTransit.CreateBy = (int)RequisitionMaster.CreateBy;
                                objStockTransit.CreateOn = DateTime.Now;
                                objStockTransit.IsDeleted = false;
                                objStockTransit.CreatePc = HostService.GetIP();
                                objTransitDetail.Add(objStockTransit);
                                OtherTransitDigits++;

                                //*******************************Stock Transit*******************************
                                objRqDetails.Add(objRequisitionDetail);

                                OtherDigits++;
                               
                            }

                            //RequisitionID
                            //**************Master Transaction Save************************************************                          
                            _ctxCmn.InvRequisitionMasters.Add(RequisitionMaster);

                            //RequisitionDetailID
                            //**************Details Transaction Save************************************************       
                            _ctxCmn.InvRequisitionDetails.AddRange(objRqDetails);
                            _ctxCmn.InvStockTransits.AddRange(objTransitDetail);
                            _ctxCmn.SaveChanges();
                            //_ctxCmn.SaveChangesAsync();

                            //**************Reset Transaction************************************************
                            GenericFactory_EF_RequisitionMaster.updateMaxID("InvRequisitionMaster", Convert.ToInt64(NextId));
                            GenericFactory_EF_RequisitionMaster.updateCustomCode(Convert.ToInt16(commonEntity.currentMenuID), Convert.ToDateTime(RequisitionMaster.RequisitionDate), RequisitionMaster.CompanyID, 1, 1);
                            GenericFactory_EF_RequisitionDetail.updateMaxID("InvRequisitionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            GenericFactory_EF_InvStockTransit.updateMaxID("InvStockTransit", Convert.ToInt64(FirstTransitDigit + "" + (OtherTransitDigits - 1)));
                            transaction.Complete();

                            //**************Commit Transaction************************************************

                            result = customCode;
                        }

                    }
                }
                #region WorkFlow Transaction Entry Team
                //int workflowID = 0;               
                //List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                //foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                //{
                //    int userTeamID = item.UserTeamID ?? 0;
                //    if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID) && userTeamID > 0)
                //    {
                //        item.WorkFlowTranCustomID = (int)RequisitionMaster.RequisitionID;
                //        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);


                //    }
                //    if (userTeamID == 0)
                //    {
                //        item.WorkFlowTranCustomID = (int)RequisitionMaster.RequisitionID;
                //        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                //    }
                //}
                #endregion Workflow Transaction Enltry Team

                //Mail Service ///
                //int mail = 0;
                //foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                //{
                //    NotificationEntity notification = new NotificationEntity();
                //    notification.WorkFlowID = item.WorkFlowID;
                //    notification.TransactionID = (int)RequisitionMaster.RequisitionID;
                //    List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(notification, "Created.");
                //    foreach (var mailModel in nModel)
                //    {
                //        mail = await new EmailService().NotificationMail(mailModel);
                //    }
                //}


            }
            catch (Exception ex)
            {
                result = "-1";
            }

            return result;
        }
        public string SaveRequisitionMasterDetails(InvRequisitionMaster RequisitionMaster, List<vmRequisitionDetails> RequisitionDetails, UserCommonEntity commonEntity, ArrayList fileNames)
        {
            //Declerations
            string result = string.Empty; string customCode = string.Empty; string CustomNo = string.Empty;
            int RequisitionID = Convert.ToInt32(RequisitionMaster.RequisitionID), SDetailRowNum = 0, UDetailRowNum = 0;
            long FirstDigit = 0, OtherDigits = 0, nextDetailId = 0; int NextId = 0;
            GenericFactory_EF_RequisitionMaster = new InvRequisitionMaster_EF();
            GenericFactory_EF_RequisitionDetail = new InvRequisitionDetail_EF();
            List<InvRequisitionDetail> objRqDetails = new List<InvRequisitionDetail>();
            List<InvRequisitionDetail> SobjDetails = new List<InvRequisitionDetail>();
            SDetailRowNum = Convert.ToInt32(RequisitionDetails.Where(s => s.ModelState == "Save").Count());
            UDetailRowNum = Convert.ToInt32(RequisitionDetails.Where(s => s.ModelState == "Update").Count());
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //Transaction Occur here************************************************
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (RequisitionID > 0)
                        {

                            //**************Master Model************************************************
                            var lstInvReqMaster = GenericFactory_EF_RequisitionMaster.GetAll().FirstOrDefault(x => x.RequisitionID == RequisitionID);
                            lstInvReqMaster.UpdateBy = RequisitionMaster.CreateBy;
                            lstInvReqMaster.ManualRequisitionNo = RequisitionMaster.ManualRequisitionNo;
                            lstInvReqMaster.UpdateOn = DateTime.Now;
                            lstInvReqMaster.UpdatePc = HostService.GetIP();
                            lstInvReqMaster.RequisitionBy = RequisitionMaster.RequisitionBy;
                            lstInvReqMaster.RequisitionDate = RequisitionMaster.RequisitionDate;
                            lstInvReqMaster.EstDate = RequisitionMaster.EstDate;
                            lstInvReqMaster.DepartmentID = RequisitionMaster.DepartmentID;
                            lstInvReqMaster.Remarks = RequisitionMaster.Remarks;
                            lstInvReqMaster.Purpose = RequisitionMaster.Purpose;
                            result = lstInvReqMaster.RequisitionNo;

                            //*************Details Model************************************************
                            if (UDetailRowNum > 0)
                            {
                                foreach (vmRequisitionDetails ivrd in RequisitionDetails.Where(x => x.ModelState == "Update"))
                                {
                                    InvRequisitionDetail objRequisitionDetail = GenericFactory_EF_RequisitionDetail.GetAll().FirstOrDefault(x => x.RequisitionID == RequisitionID && x.RequisitionDetailID == ivrd.RequisitionDetailID);
                                    objRequisitionDetail.RequisitionID = RequisitionMaster.RequisitionID;
                                    objRequisitionDetail.ItemID = ivrd.ItemID;
                                    objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                    objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                    objRequisitionDetail.Qty = ivrd.Qty;
                                    objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                    objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate;
                                    objRequisitionDetail.UpdateBy = RequisitionMaster.CreateBy;
                                    objRequisitionDetail.UpdateOn = DateTime.Now;
                                    objRequisitionDetail.UpdatePc = HostService.GetIP();
                                    objRqDetails.Add(objRequisitionDetail);
                                }
                            }
                            if (SDetailRowNum > 0)
                            {
                                nextDetailId = Convert.ToInt64(GenericFactory_EF_RequisitionDetail.getMaxID("InvRequisitionDetail"));
                                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));
                                foreach (vmRequisitionDetails ivrd in RequisitionDetails.Where(x => x.ModelState == "Save"))
                                {
                                    InvRequisitionDetail objRequisitionDetail = new InvRequisitionDetail();
                                    objRequisitionDetail.RequisitionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                    objRequisitionDetail.RequisitionID = RequisitionMaster.RequisitionID;
                                    objRequisitionDetail.ItemID = ivrd.ItemID;
                                    objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                    objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                    objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                    objRequisitionDetail.Qty = ivrd.Qty;
                                    objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                    objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate;
                                    objRequisitionDetail.CreateBy = RequisitionMaster.CreateBy;
                                    objRequisitionDetail.CreateOn = DateTime.Now;
                                    objRequisitionDetail.IsDeleted = false;
                                    objRequisitionDetail.CreatePc = HostService.GetIP();
                                    SobjDetails.Add(objRequisitionDetail);
                                    OtherDigits++;
                                }
                            }

                            //RequisitionID
                            //**************Master Transaction Update************************************************
                            if (lstInvReqMaster != null)
                            {
                                GenericFactory_EF_RequisitionMaster.Update(lstInvReqMaster);
                                GenericFactory_EF_RequisitionMaster.Save();
                            }
                            //RequisitionDetailID
                            // **************Details Transaction Update************************************************
                            if (objRqDetails.Count != 0)
                            {
                                GenericFactory_EF_RequisitionDetail.UpdateList(objRqDetails);
                                GenericFactory_EF_RequisitionDetail.Save();
                            }
                            if (SobjDetails.Count != 0)
                            {
                                GenericFactory_EF_RequisitionDetail.InsertList(SobjDetails);
                                GenericFactory_EF_RequisitionDetail.Save();
                                GenericFactory_EF_RequisitionDetail.updateMaxID("InvRequisitionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }

                            //**********----------------------Start File Upload----------------------**********
                            if (fileNames.Count > 0)
                            {
                                GenericFactory_CmnDocument = new CmnDocument_EF();
                                int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                                List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                                for (int i = 1; i <= fileNames.Count; i++)
                                {
                                    CmnDocument objCmnDocument = new CmnDocument();
                                    objCmnDocument.DocumentID = DocumentID;
                                    objCmnDocument.DocumentPahtID = 8;
                                    //objCmnDocument.DocumentName = fileNames[i].ToString();
                                    string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                                    objCmnDocument.DocumentName = RequisitionMaster.RequisitionNo + "_Doc_" + i + extension;
                                    objCmnDocument.TransactionID = RequisitionMaster.RequisitionID;
                                    objCmnDocument.TransactionTypeID = RequisitionMaster.RequisitionTypeID;
                                    objCmnDocument.CompanyID = RequisitionMaster.CompanyID;
                                    objCmnDocument.CreateBy = Convert.ToInt16(RequisitionMaster.CreateBy);
                                    objCmnDocument.CreateOn = DateTime.Now;
                                    objCmnDocument.CreatePc = HostService.GetIP();
                                    objCmnDocument.IsDeleted = false;                                   
                                    lstCmnDocument.Add(objCmnDocument);
                                }

                                GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                                GenericFactory_CmnDocument.Save();
                                GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));
                            }
                            //**************Commit Transaction************************************************                                            
                            transaction.Complete();
                            result = "Update";
                        }
                        else
                        {
                            //Initialisation ************************************************
                            NextId = Convert.ToInt16(GenericFactory_EF_RequisitionMaster.getMaxID("InvRequisitionMaster"));
                            nextDetailId = Convert.ToInt64(GenericFactory_EF_RequisitionDetail.getMaxID("InvRequisitionDetail"));
                            FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));
                            CustomNo = GenericFactory_EF_RequisitionMaster.getCustomCode(Convert.ToInt16(commonEntity.currentMenuID), Convert.ToDateTime(RequisitionMaster.RequisitionDate), RequisitionMaster.CompanyID, 1, 1);

                            if ((customCode != "") || (customCode != null))
                                customCode = CustomNo;
                            else
                                customCode = NextId.ToString();
                            //**************Master Model************************************************
                            RequisitionMaster.RequisitionID = NextId;
                            RequisitionMaster.CreateOn = DateTime.Now;
                            RequisitionMaster.CreatePc = HostService.GetIP();
                            RequisitionMaster.RequisitionNo = customCode;

                            //*************Details Model************************************************
                            foreach (vmRequisitionDetails ivrd in RequisitionDetails)
                            {
                                InvRequisitionDetail objRequisitionDetail = new InvRequisitionDetail();
                                objRequisitionDetail.RequisitionDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                objRequisitionDetail.IsGrrComplete = false;
                                objRequisitionDetail.RequisitionID = NextId;
                                objRequisitionDetail.ItemID = ivrd.ItemID;
                                objRequisitionDetail.UnitID = (int)ivrd.UnitID;
                                objRequisitionDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                                objRequisitionDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                                objRequisitionDetail.Qty = ivrd.Qty;
                                objRequisitionDetail.UnitPrice = ivrd.CurrentRate;
                                objRequisitionDetail.Amount = ivrd.Qty * ivrd.CurrentRate; //ivrd.Amount;
                                objRequisitionDetail.CreateBy = RequisitionMaster.CreateBy;
                                objRequisitionDetail.CreateOn = DateTime.Now;
                                objRequisitionDetail.IsDeleted = false;
                                objRequisitionDetail.CreatePc = HostService.GetIP();
                                objRqDetails.Add(objRequisitionDetail);
                                OtherDigits++;
                            }

                            //RequisitionID
                            //**************Master Transaction Save************************************************                          
                            _ctxCmn.InvRequisitionMasters.Add(RequisitionMaster);

                            //RequisitionDetailID
                            //**************Details Transaction Save************************************************       
                            _ctxCmn.InvRequisitionDetails.AddRange(objRqDetails);
                            _ctxCmn.SaveChanges();
                            //_ctxCmn.SaveChangesAsync();

                            //**************Reset Transaction************************************************
                            GenericFactory_EF_RequisitionMaster.updateMaxID("InvRequisitionMaster", Convert.ToInt64(NextId));
                            GenericFactory_EF_RequisitionMaster.updateCustomCode(Convert.ToInt16(commonEntity.currentMenuID), Convert.ToDateTime(RequisitionMaster.RequisitionDate), RequisitionMaster.CompanyID, 1, 1);
                            GenericFactory_EF_RequisitionDetail.updateMaxID("InvRequisitionDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));

                            //**********----------------------Start File Upload----------------------**********
                            if (fileNames.Count > 0)
                            {
                                GenericFactory_CmnDocument = new CmnDocument_EF();
                                int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                                List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                                for (int i = 1; i <= fileNames.Count; i++)
                                {
                                    CmnDocument objCmnDocument = new CmnDocument();
                                    objCmnDocument.DocumentID = DocumentID;
                                    objCmnDocument.DocumentPahtID = 8;
                                    //objCmnDocument.DocumentName = fileNames[i].ToString();
                                    string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                                    objCmnDocument.DocumentName = RequisitionMaster.RequisitionNo + "_Doc_" + i + extension;
                                    objCmnDocument.TransactionID = RequisitionMaster.RequisitionID;
                                    objCmnDocument.TransactionTypeID = RequisitionMaster.RequisitionTypeID;
                                    objCmnDocument.CompanyID = RequisitionMaster.CompanyID;
                                    objCmnDocument.CreateBy = Convert.ToInt16(RequisitionMaster.CreateBy);
                                    objCmnDocument.CreateOn = DateTime.Now;
                                    objCmnDocument.CreatePc = HostService.GetIP();
                                    objCmnDocument.IsDeleted = false;                                  
                                    lstCmnDocument.Add(objCmnDocument);

                                    DocumentID++;
                                }

                                GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                                GenericFactory_CmnDocument.Save();
                                GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));
                            }
                            //**********----------------------File upload completed----------------------**********

                            transaction.Complete();
                          
                            //**************Commit Transaction************************************************
                           
                            result = customCode;
                        }

                    }
                }
                #region WorkFlow Transaction Entry Team
                int workflowID = 0;
                List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                {
                    int userTeamID = item.UserTeamID ?? 0;
                    if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID) && userTeamID > 0)
                    {
                        item.WorkFlowTranCustomID = (int)RequisitionMaster.RequisitionID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);


                    }
                    if (userTeamID == 0)
                    {
                        item.WorkFlowTranCustomID = (int)RequisitionMaster.RequisitionID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                    }
                }
                #endregion Workflow Transaction Enltry Team

               // Mail Service ///
                //int mail = 0;
                //foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                //{
                //    NotificationEntity notification = new NotificationEntity();
                //    notification.WorkFlowID = item.WorkFlowID;
                //    notification.TransactionID = (int)RequisitionMaster.RequisitionID;
                //    List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(notification, "Created.");
                //    foreach (var mailModel in nModel)
                //    {
                //        mail = await new EmailService().NotificationMail(mailModel);
                //    }
                //}

              
            }
            catch (Exception ex)
            {
                result = "-1";
            }

            return result;
        }

        public IEnumerable<vmCmnDocument> GetFileDetailsById(int SPRID, int TransTypeID)
        {
            GenericFactory_CmnDocument = new CmnDocument_EF();
            IEnumerable<vmCmnDocument> objFileInfo = null;
            string fullFilePath = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //  var transactionName;
                    var virtualPath = _ctxCmn.CmnDocumentPaths.Where(m => m.TransactionTypeID == TransTypeID && m.IsDeleted == false).ToList().
                                     Select(m => new CmnDocumentPath
                                     {
                                         VirtualPath = m.VirtualPath
                                     }).FirstOrDefault();

                    var transactionName = _ctxCmn.CmnTransactionTypes.Where(m => m.TransactionTypeID == TransTypeID && m.IsDeleted == false).ToList().
                                     Select(m => new CmnTransactionType
                                     {
                                         TransactionTypeName = m.TransactionTypeName
                                     }).FirstOrDefault();


                    objFileInfo = _ctxCmn.CmnDocuments.Where(m => m.TransactionID == SPRID).ToList().
                                Select(m => new vmCmnDocument
                                {
                                    DocumentID = m.DocumentID,
                                    DocumentName = m.DocumentName,
                                    TransactionID = m.TransactionID,
                                    FullDocumentPath = virtualPath.VirtualPath + transactionName.TransactionTypeName + "/" + m.DocumentName
                                }).ToList();


                    //objFileInfo = GenericFactory_CmnDocument.GetAll().Select(m => new CmnDocument
                    //{
                    //    DocumentID = m.DocumentID,
                    //    DocumentName = m.DocumentName,
                    //    TransactionID = m.TransactionID
                    //}).
                    //    Where(m => m.TransactionID == lcID).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }


            return objFileInfo;
        }

        public CmnDocumentPath GetUploadPath(int TransactionTypeID)
        {
            GenericFactory_CmnDocumentPath = new CmnDocumentPath_EF();
            CmnDocumentPath objUploadPath = null;
            try
            {
                objUploadPath = GenericFactory_CmnDocumentPath.GetAll().Select(m => new
                CmnDocumentPath
                {
                    TransactionTypeID = m.TransactionTypeID,
                    PhysicalPath = m.PhysicalPath
                })
                    .Where(m => m.TransactionTypeID == TransactionTypeID).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUploadPath;
        }
        public string SprUpdateDelete(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            GenericFactory_EF_RequisitionMaster = new InvRequisitionMaster_EF();
            GenericFactory_EF_RequisitionDetail = new InvRequisitionDetail_EF();
            List<InvRequisitionDetail> Details = new List<InvRequisitionDetail>();
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    var lstInvReqMaster = GenericFactory_EF_RequisitionMaster.GetAll().FirstOrDefault(x => x.RequisitionID == objcmnParam.id);
                    lstInvReqMaster.IsDeleted = true;
                    lstInvReqMaster.DeleteBy = objcmnParam.loggeduser;
                    lstInvReqMaster.DeleteOn = DateTime.Now;
                    lstInvReqMaster.DeletePc = HostService.GetIP();

                    var updel = GenericFactory_EF_RequisitionDetail.GetAll();

                    foreach (InvRequisitionDetail u in updel.Where(x => x.RequisitionID == objcmnParam.id))
                    {
                        u.IsDeleted = true;
                        u.DeleteBy = objcmnParam.loggeduser;
                        u.DeleteOn = DateTime.Now;
                        u.DeletePc = HostService.GetIP();
                        Details.Add(u);
                    }

                    //**************Master Transaction Save************************************************
                    GenericFactory_EF_RequisitionMaster.Update(lstInvReqMaster);
                    GenericFactory_EF_RequisitionMaster.Save();

                    //RequisitionDetailID
                    //**************Details Transaction Save************************************************
                    GenericFactory_EF_RequisitionDetail.UpdateList(Details);
                    GenericFactory_EF_RequisitionDetail.Save();
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }

            return result;
        }

        public IEnumerable<vmSPR> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode)
        {
            vmSPR_GF GFactory_GF_SPR = new vmSPR_GF();
            string spQuery = "";
            IEnumerable<vmSPR> lstItemDetailByItmCode = null;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                //ht.Add("DepartmentID", objcmnParam.DepartmentID);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("ArticleNo", ItemCode);
                ht.Add("ItemId", 0);
                spQuery = "[Get_SPRItemInfo]";

                lstItemDetailByItmCode = GFactory_GF_SPR.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByItmCode;
        }

        public IEnumerable<vmSPR> GetItmDetailByItemId(vmCmnParameters objcmnParam, string ItemId)
        {
            vmSPR_GF GFactory_GF_SPR = new vmSPR_GF();
            string spQuery = "";
            IEnumerable<vmSPR> lstItemDetailByItmId = null;
            try
            {
                Hashtable ht = new Hashtable();
                //ht.Add("CompanyID", objcmnParam.loggedCompany);
                //ht.Add("LoggedUser", objcmnParam.loggeduser);
                //ht.Add("DepartmentID", objcmnParam.DepartmentID);
                //ht.Add("PageNo", objcmnParam.pageNumber);
                //ht.Add("RowCountPerPage", objcmnParam.pageSize);
                //ht.Add("IsPaging", objcmnParam.IsPaging);
                //ht.Add("ArticleNo", "");
                //ht.Add("ItemId", ItemId);
              
                ht.Add("itemID", ItemId);
                ht.Add("BatchID", null);
                ht.Add("LotID", null);
                ht.Add("GradeID", null);
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("SupplierID", null);
                ht.Add("DepartmentID", objcmnParam.DepartmentID);
                ht.Add("TransactionTypeID", objcmnParam.tTypeId);
                spQuery = "[sp_getStockInfoByItem]";  //"[Get_SPRItemInfo]";
                lstItemDetailByItmId = GFactory_GF_SPR.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByItmId;
        }
        public IEnumerable<vmSPR> GetItmDetail(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            vmSPR_GF GFactory_GF_SPR = new vmSPR_GF();
            string spQuery = "";
            recordsTotal = 0;
            IEnumerable<vmSPR> lstItemDetail = null;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                //ht.Add("DepartmentId", objcmnParam.DepartmentID);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                spQuery = "[Get_InvItemDetails]";

                lstItemDetail = GFactory_GF_SPR.ExecuteQuery(spQuery, ht);
                using (_ctxCmn = new ERP_Entities())
                {
                    recordsTotal = _ctxCmn.CmnItemMasters.Where(x => x.IsDeleted == false && x.ItemTypeID == 4 && x.CompanyID == objcmnParam.loggedCompany).Count();
                }
              //  recordsTotal = lstItemDetail.Count();
               // lstItemDetail = lstItemDetail.OrderBy(x => x.ArticleNo).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
            
                //using (_ctxCmn = new ERP_Entities())
                //{
                //    lstItemDetail = (from im in _ctxCmn.CmnItemMasters.Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany)
                //                     join uom in _ctxCmn.CmnUOMs on im.UOMID equals uom.UOMID                                           
                //                            select new vmSPR
                //                            {
                //                                ItemID = im.ItemID,
                //                                ArticleNo = im.ArticleNo,
                //                                ItemName = im.ItemName,
                //                                UOMName = uom.UOMName
                //                            }).ToList();
                //    lstItemDetail = lstItemDetail.OrderBy(x => x.ItemName).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                //    recordsTotal = lstItemDetail.Count();
                //}
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetail;
        }
        public vmRequisition GetRequisitonMasterByRequisitionID(int? RequisitionId, int CompanyId)
        {
            GFactory_GF_ItemRate = new vmRequisition_GF();
            vmRequisition lstRequisiton = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("RequisitionId", RequisitionId);
                ht.Add("CompanyId", CompanyId);
                spQuery = "[dbo].[Get_InvRequisitionMaster]";
                lstRequisiton = GFactory_GF_ItemRate.ExecuteCommandSingle(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstRequisiton;
        }

        public IEnumerable<vmRequisitionDetails> GetRequisitonDetailByRequisitionID(int? RequisitionId, int CompanyId)
        {
            GFactory_GF_RequisitionDetail = new vmRequisitionDetails_GF();
            IEnumerable<vmRequisitionDetails> lstRequisiton = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("RequisitionId", RequisitionId);
                ht.Add("CompanyId", CompanyId);
                spQuery = "[dbo].[Get_InvRequisitionDetail]";
                lstRequisiton = GFactory_GF_RequisitionDetail.ExecuteQuery(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstRequisiton;
        }

    }
}
