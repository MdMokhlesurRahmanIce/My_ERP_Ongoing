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
using ABS.Utility;


namespace ABS.Service.Inventory.Factories
{
    public class IssueMgt:iIssueMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactory_EF_User;
        private iGenericFactory_EF<InvRequisitionMaster> GenericFactory_EF_RequisitionMaster;
        private iGenericFactory_EF<InvIssueMaster> GenericFactory_EF_IssueMaster;
        private iGenericFactory_EF<InvRequisitionDetail> GenericFactory_EF_RequisitionDetail;
        private iGenericFactory_EF<InvIssueDetail> GenericFactory_EF_IssueDetail;
        private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode;
        private iGenericFactory_EF<InvStockMaster> GenericFactory_EF_StockMaster;
        private iGenericFactory<vmRequisition> GenericFactory_GF_RequisitionMaster;
        private iGenericFactory<InvRequisitionMaster> GenericFactory_GF_Requisition;
        private iGenericFactory<vmIssueMaster> GenericFactory_GF_Issue;
        private iGenericFactory<InvIssueDetail> GenericFactory_GF_IssueDetail;
        private iGenericFactory<InvIssueMaster> GenericFactory_GF_IssueMaster;

       
        /// No CompanyID Provided
        /// 

        public IEnumerable<vmRequisition> GetIssueList()
        {
            IEnumerable<vmRequisition> lstIssueMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstIssueMaster = _ctxCmn.InvIssueMasters.Where(m => m.IsDeleted == false).Select(m => new vmRequisition { IssueID = m.IssueID, IssueNo = m.IssueNo, CompanyID = m.CompanyID, CreateBy = m.CreateBy }).ToList();
                    lstIssueMaster = lstIssueMaster.OrderBy(m => m.IssueID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstIssueMaster;
        }
        public IEnumerable<InvRequisitionMaster> GetRequisitionNo(int? pageNumber, int? pageSize, int? IsPaging, int CompanyID, int RequisitionTypeID)
        {
            GenericFactory_GF_Requisition = new InvRequisitionMaster_GF();
            IEnumerable<InvRequisitionMaster> objRequisitionList = null;
            string spQuery = string.Empty;
            try
            {   
                    //objRequisitionList = GenericFactory_EF_RequisitionMaster.GetAll().Select(m => new InvRequisitionMaster { RequisitionID = m.RequisitionID, RequisitionNo = m.RequisitionNo, IsDeleted = m.IsDeleted }).Where(s => s.IsDeleted == false).ToList();

                Hashtable ht = new Hashtable();
                ht.Add("CompanyId", CompanyID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                ht.Add("RequisitionTypeID", RequisitionTypeID);
                spQuery = "[dbo].[Get_InvRequisitionList]";
                objRequisitionList = GenericFactory_GF_Requisition.ExecuteQuery(spQuery, ht);               
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objRequisitionList;
        }
        public IEnumerable<vmRequisition> GetRequisitionItemList(int? pageNumber, int? pageSize, int? IsPaging, int? RequisitionID,int? MrrID)
        {
            GenericFactory_GF_RequisitionMaster = new vmRequisition_GF();
            IEnumerable<vmRequisition> objRequisitionItemList = null;
            string spQuery = string.Empty;
            try
            {
               
                Hashtable ht = new Hashtable();
                ht.Add("RequisitionId", RequisitionID);
                ht.Add("MrrID", MrrID);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[dbo].[Get_InvRequisitionItemList]";
                objRequisitionItemList = GenericFactory_GF_RequisitionMaster.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objRequisitionItemList;
        }
        public IEnumerable<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_User = new CmnUser_EF();
            IEnumerable<CmnUser> objUsers = null;
            string spQuery = string.Empty;
            try
            {
                
                objUsers = GenericFactory_EF_User.GetAll().Select(m => new CmnUser { UserID = m.UserID, UserFullName = m.UserFullName, UserTypeID = m.UserTypeID, IsDeleted = m.IsDeleted }).Where(s => s.IsDeleted == false && s.UserTypeID == 1).ToList();             
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUsers;
        }

        public IEnumerable<CmnCompany> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCompany> objCompany = null;
            try
            {

                //objCompany = GenericFactoryFor_CmnLot_EF.GetAll().Select(m => new CmnLot { LotID = m.LotID, LotNo = m.LotNo, IsDeleted = m.IsDeleted }).Where(m => m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCompany;
        }

        public List<InvRequisitionMaster> GetRequisitionMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            _ctxCmn = new ERP_Entities();
            List<InvRequisitionMaster> _vmRequisition = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    _vmRequisition = _ctxCmn.InvRequisitionMasters.Where(x => x.IsDeleted == false).OrderBy(s => s.RequisitionID)
                                     .Skip(objcmnParam.pageNumber)
                                     .Take(objcmnParam.pageSize)
                                     .ToList();
                    recordsTotal = _vmRequisition.Count();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return _vmRequisition;
        }

        public List<InvMrrMaster> GetMRRList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            _ctxCmn = new ERP_Entities();
            List<InvMrrMaster> lstInvMrrMaster = null; 
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstInvMrrMaster = _ctxCmn.InvMrrMasters.Where(x => x.IsDeleted == false).OrderBy(s => s.MrrID)
                                     .ToList().Select(m => new InvMrrMaster { MrrID=m.MrrID, MrrNo = m.MrrNo}).ToList(); 
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstInvMrrMaster;
        }

        public List<InvRChallanMaster> GetChallanList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            _ctxCmn = new ERP_Entities();
            List<InvRChallanMaster> lstInvChallanMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstInvChallanMaster = _ctxCmn.InvRChallanMasters.Where(x => x.IsDeleted == false).OrderBy(s => s.CHID)
                                     .ToList().Select(m => new InvRChallanMaster { CHID = m.CHID, CHNo = m.CHNo }).ToList();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstInvChallanMaster;
        }

        public List<InvGrrMaster> GetGRRList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            _ctxCmn = new ERP_Entities();
            List<InvGrrMaster> lstInvGRRMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstInvGRRMaster = _ctxCmn.InvGrrMasters.Where(x => x.IsDeleted == false).OrderBy(s => s.CHID)
                                     .ToList().Select(m => new InvGrrMaster { GrrID = m.GrrID, GrrNo = m.GrrNo }).ToList();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstInvGRRMaster;
        }

        public string SaveIssueMasterDetails(InvIssueMaster IssueMaster, List<InvIssueDetail> IssueDetails, int menuID)
        {
            GenericFactory_GF_IssueDetail = new InvIssueDetail_GF();
            GenericFactory_GF_IssueMaster = new InvIssueMaster_GF();
            GenericFactory_EF_IssueMaster = new InvIssueMaster_EF();
            GenericFactory_EF_IssueDetail = new InvIssueDetail_EF();
            _ctxCmn = new ERP_Entities();
            List<InvStockMaster> lstStockMaster = new List<InvStockMaster>();
            List<InvIssueDetail> lstIssueDetail = new List<InvIssueDetail>();
            string result = "";
            if (IssueMaster.IssueID > 0)
            {              
                 int Result = 0; 
                    try
                    {
                        var lstInvIssueMaster = GenericFactory_EF_IssueMaster.GetAll().FirstOrDefault(x => x.IssueID == IssueMaster.IssueID);
                        lstInvIssueMaster.UpdateBy = IssueMaster.CreateBy;
                        lstInvIssueMaster.UpdateOn = DateTime.Now;
                        lstInvIssueMaster.UpdatePc = HostService.GetIP();
                        lstInvIssueMaster.IssueDate = IssueMaster.IssueDate;
                        lstInvIssueMaster.DepartmentID = IssueMaster.DepartmentID;
                        lstInvIssueMaster.Comments = IssueMaster.Comments;
                        result = lstInvIssueMaster.RequisitionNo;

                        foreach (InvIssueDetail ivrd in IssueDetails)
                        {
                            InvIssueDetail objIssueDetail = GenericFactory_EF_IssueDetail.GetAll().FirstOrDefault(x => x.IssueID == IssueMaster.IssueID && x.IssueDetailID == ivrd.IssueDetailID);
                            objIssueDetail.IssueID = IssueMaster.IssueID;
                            objIssueDetail.ItemID = ivrd.ItemID;
                            objIssueDetail.UnitID = (int)ivrd.UnitID;
                            objIssueDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                            objIssueDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                            objIssueDetail.IssueQty = ivrd.IssueQty;
                            objIssueDetail.UnitPrice = ivrd.UnitPrice == null ? 0 : ivrd.UnitPrice;
                            objIssueDetail.Amount = ivrd.IssueQty * ivrd.UnitPrice == null ? 0 : ivrd.UnitPrice;
                            objIssueDetail.UpdateBy = IssueMaster.CreateBy;
                            objIssueDetail.UpdateOn = DateTime.Now;
                            objIssueDetail.UpdatePc = HostService.GetIP();
                            lstIssueDetail.Add(objIssueDetail);
                        }
                        if (lstInvIssueMaster != null)
                        {
                            GenericFactory_EF_IssueMaster.Update(lstInvIssueMaster);
                            GenericFactory_EF_IssueMaster.Save();
                        }                        
                        // **************Details Transaction Update************************************************
                        if (lstIssueDetail.Count != 0)
                        {
                            GenericFactory_EF_IssueDetail.UpdateList(lstIssueDetail);
                            GenericFactory_EF_IssueDetail.Save();
                        }
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        Result = 0;
                    }
                  return  result;
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        //...........START  new maxId........//
                        long NextId = Convert.ToInt16(GenericFactory_EF_IssueMaster.getMaxID("InvIssueMaster"));
                       
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextDetailId = Convert.ToInt64(GenericFactory_EF_IssueDetail.getMaxID("InvIssueDetail"));
                        FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));
                       
                        string customCode = "";
                        string CustomNo = customCode = GenericFactory_EF_IssueMaster.getCustomCode(menuID, Convert.ToDateTime(IssueMaster.IssueDate), IssueMaster.CompanyID, 1, 1);
                        if (customCode != "")
                        {
                            customCode = CustomNo; 
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }
                        //.........END for custom code............ //

                        IssueMaster.IssueID = NextId;
                        IssueMaster.CreateOn = DateTime.Now;
                        IssueMaster.CreatePc = HostService.GetIP();
                        IssueMaster.IssueNo = customCode;   
                 
                     
                    
                        foreach (InvIssueDetail ivrd in IssueDetails)
                        {
                            
                            InvIssueDetail objIssueDetail = new InvIssueDetail();
                            objIssueDetail.IssueDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objIssueDetail.IssueID = NextId;
                            objIssueDetail.ItemID = ivrd.ItemID;
                            objIssueDetail.UnitID = ivrd.UnitID;
                            objIssueDetail.LotID = ivrd.LotID == 0 ? null : ivrd.LotID;
                            objIssueDetail.BatchID = ivrd.BatchID == 0 ? null : ivrd.BatchID;
                            objIssueDetail.IssueQty = ivrd.IssueQty;
                            objIssueDetail.UnitPrice = ivrd.UnitPrice;
                            objIssueDetail.Amount = ivrd.Amount;
                            objIssueDetail.CreateBy = IssueMaster.CreateBy;
                            objIssueDetail.CreateOn = DateTime.Now;
                            objIssueDetail.IsDeleted = false;
                            objIssueDetail.CreatePc = HostService.GetIP();
                            lstIssueDetail.Add(objIssueDetail);                          
                            OtherDigits++;
                            }

                        _ctxCmn.InvIssueMasters.Add(IssueMaster);
                      //GenericFactory_EF_IssueMaster.Insert(IssueMaster);
                      //GenericFactory_EF_IssueMaster.Save();
                        //............Update MaxID.................//
                      GenericFactory_EF_IssueMaster.updateMaxID("InvIssueMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                      GenericFactory_EF_IssueMaster.updateCustomCode(menuID, DateTime.Now, IssueMaster.CompanyID, 1, 1);

                       // GenericFactory_EF_IssueDetail.InsertList(lstRequisitionDetail);
                      _ctxCmn.InvIssueDetails.AddRange(lstIssueDetail);
                       //GenericFactory_EF_IssueDetail.Save();
                      _ctxCmn.SaveChanges();
                        //............Update MaxID.................//
                        GenericFactory_EF_IssueDetail.updateMaxID("InvIssueDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits-1)));
                        transaction.Complete();
                        result = customCode;
                    }
                    catch (Exception e)
                    { 
                        result = "";
                    }
                }

            }
            return result;
        }


        public List<vmIssueMaster> GetIssueMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            _ctxCmn = new ERP_Entities();
            List<vmIssueMaster> lstIssue = null;
            List<vmIssueMaster> lstIssueWOPaging = null;
            recordsTotal = 0;
            try
            {
                lstIssueWOPaging = (from qcm in _ctxCmn.InvIssueMasters.Where(s => s.IsDeleted == false && s.IssueTypeID == objcmnParam.tTypeId)
                                   // join com in _ctxCmn.CmnCompanies on qcm.ToCompanyID equals com.CompanyID
                                    join org in _ctxCmn.CmnOrganograms on qcm.ToDepartmentID equals org.OrganogramID
                                    join usr in _ctxCmn.CmnUsers on qcm.IssueBy equals usr.UserID
                                    select new vmIssueMaster
                                    {
                                        IssueID  = qcm.IssueID,
                                        IssueNo = qcm.IssueNo,
                                        IssueDate = qcm.IssueDate,                                        
                                        RequisitionNo = qcm.RequisitionNo,
                                        IssueBy = qcm.IssueBy,
                                        IssueByName = usr.UserFullName,
                                        ToDepartmentID = qcm.ToDepartmentID,
                                        ToDepartment = org.OrganogramName,
                                        //ToCompanyID = qcm.ToCompanyID,
                                       // ToCompany = com.CompanyName,
                                        Comments = qcm.Comments,
                                        
                                    }).ToList();
                                
                lstIssue = lstIssueWOPaging.OrderByDescending(x => x.IssueID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                recordsTotal = lstIssueWOPaging.Count();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstIssue;
        }

        public vmIssueMaster GetIssueMasterByIssueId(int? IssueId, int CompanyID)
        {
            GenericFactory_GF_Issue = new vmIssueMaster_GF();
            vmIssueMaster lstIssue = null;
             string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("IssueId",IssueId);
                ht.Add("CompanyID", CompanyID);                
                spQuery = "[dbo].[Get_InvIssueInfoByIssueId]";
                lstIssue = GenericFactory_GF_Issue.ExecuteCommandSingle(spQuery,ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstIssue;
        }

        public IEnumerable<vmRequisition> GetIssueDetailByIssueId(int? IssueId, int CompanyID)
        {
            GenericFactory_GF_RequisitionMaster = new vmRequisition_GF();
            IEnumerable<vmRequisition> IssueInfo = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("IssueId", IssueId);
                ht.Add("CompanyID", CompanyID);
                spQuery = "[dbo].[Get_InvIssueDetailByIssueId]";
                IssueInfo = GenericFactory_GF_RequisitionMaster.ExecuteQuery(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            };
            return IssueInfo;
        }
    }
}
