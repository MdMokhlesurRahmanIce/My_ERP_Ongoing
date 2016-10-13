using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Inventory.Factories
{
    public class MRRMgt : iMRRMgt
    {

        private ERP_Entities _ctxCmn = null; 
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
        private iGenericFactory<vmGrr> GenericFactory_GF_vmGrr = null;
        private iGenericFactory<vmQC> GenericFactory_GF_vmQC = null;
        private iGenericFactory<vmRequisition> GenericFactory_GF_RequisitionMaster = null;
        public IEnumerable<vmRequisition> GetRequisitionItemList(int? RequisitionID)
        {
            GenericFactory_GF_RequisitionMaster = new vmRequisition_GF();
            IEnumerable<vmRequisition> objRequisitionItemList = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("RequisitionId", RequisitionID);
                spQuery = "[dbo].[Get_InvSPRItemList]";
                objRequisitionItemList = GenericFactory_GF_RequisitionMaster.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objRequisitionItemList;
        }
        public List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<InvGrrMaster> lstInvGrrMaster = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstInvGrrMaster = (from grr in _ctxCmn.InvGrrMasters
                                       select new { GrrID = grr.GrrID, GrrNo = grr.GrrNo, IsDeleted = grr.IsDeleted }).ToList()
                                       .Select(m => new InvGrrMaster { GrrID = m.GrrID, GrrNo = m.GrrNo, IsDeleted = m.IsDeleted })
                                       .Where(m => m.IsDeleted == false && m.IsMrrCompleted == false).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstInvGrrMaster;
        }

        public List<vmGrr> GetSuppliers(vmCmnParameters objcmnParam)
        {
            List<vmGrr> lstSuppliers = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstSuppliers = (from spl in _ctxCmn.CmnUsers.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.UserTypeID == 3)
                                    select spl).Select(m => new vmGrr { SupplierID = m.UserID, SupplierName = m.UserFullName }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstSuppliers;
        }

        public IEnumerable<vmChallan> GetMrrList()
        {
            IEnumerable<vmChallan> lstMrrMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstMrrMaster = _ctxCmn.InvMrrMasters.Where(m => m.IsDeleted == false).Select(m => new vmChallan { MrrID = m.MrrID, MrrNo = m.MrrNo, ManualMRRNoRpt = m.ManualMRRNo + "||" + m.MrrNo, CompanyID = m.CompanyID, CreateBy = m.CreateBy }).ToList();
                } 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMrrMaster;
        }

        public List<InvIssueMaster> GetIssueNo(vmCmnParameters objcmnParam)
        {
            List<InvIssueMaster> lstIssue = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstIssue = (from spl in _ctxCmn.InvIssueMasters.Where(m =>m.IssueTypeID==objcmnParam.tTypeId && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)
                                select spl).ToList().Select(m => new InvIssueMaster { IssueID = m.IssueID, IssueNo = m.IssueNo }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstIssue;
        }

        public List<vmQC> GetSprLoanList(vmCmnParameters objcmnParam, Int32 loanTransTypeSpr)
        {
            List<vmQC> lstLoanSpr = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstLoanSpr = (from spl in _ctxCmn.InvRequisitionMasters.Where(m => m.RequisitionTypeID == loanTransTypeSpr && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)
                                  select spl).Select(m => new vmQC { RequisitionID = m.RequisitionID, SprNo = m.RequisitionNo, CompanyID=m.CompanyID, CompanyName=m.CmnCompany.CompanyName, SPRDate = m.RequisitionDate }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstLoanSpr;
        }

        public List<CmnCombo> GetMrrType(string mrrType, vmCmnParameters objcmnParam)
        {
            List<CmnCombo> lstMrrType = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstMrrType = (from mrrt in _ctxCmn.CmnComboes.Where(m => m.ComboType == mrrType && m.IsDeleted == false)
                                  select mrrt).ToList().Select(m => new CmnCombo
                                  {
                                      ComboID = m.ComboID,
                                      ComboName = m.ComboName,
                                      ComboType = m.ComboType,
                                      IsDefault = m.IsDefault,
                                      IsDeleted = m.IsDeleted
                                  }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstMrrType;
        }

        public IEnumerable<vmGrr> GetMasterInfoByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal)
        {
            GenericFactory_GF_vmGrr = new vmGrr_GF();
            string spQuery = "";
            IEnumerable<vmGrr> lstMasterInfoByGrrNo = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);

                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("GrrID", grrID);

                spQuery = "[Get_InvGrrInfoByGrrID]";

                lstMasterInfoByGrrNo = GenericFactory_GF_vmGrr.ExecuteQuery(spQuery, ht);

                recordsTotal = lstMasterInfoByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMasterInfoByGrrNo;
        }

        public IEnumerable<vmQC> GetDetailInfoByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstDetailInfoByGrrNo = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("GrrID", grrID);

                spQuery = "[Get_InvGrrItemByGrrPreType]";

                lstDetailInfoByGrrNo = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);

                recordsTotal = lstDetailInfoByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstDetailInfoByGrrNo;
        }

        public IEnumerable<vmQC> GetQCListByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstQCByGrrNo = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("GrrID", grrID);

                spQuery = "[Get_InvQCByGrrPostType]";

                lstQCByGrrNo = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);

                recordsTotal = lstQCByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQCByGrrNo;
        }

        public IEnumerable<InvMrrQcMaster> GetQCList(vmCmnParameters objcmnParam, Int32 TransType, out int recordsTotal)
        {
            IEnumerable<InvMrrQcMaster> lstQCByGrrNo = null;
            IEnumerable<InvMrrQcMaster> lstQCByGrrNoWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstQCByGrrNoWithoutPaging = (from rm in _ctxCmn.InvMrrQcMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.IsMrrCompleted==false && m.TransactionTypeID==TransType)

                                                 select new
                                                 {
                                                     MrrQcID = rm.MrrQcID,
                                                     MrrQcNo = rm.MrrQcNo

                                                 }).ToList().Select(x => new InvMrrQcMaster
                                                 {
                                                     MrrQcID = x.MrrQcID,
                                                     MrrQcNo = x.MrrQcNo
                                                 }).ToList();

                    lstQCByGrrNo = lstQCByGrrNoWithoutPaging.OrderByDescending(x => x.MrrQcID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstQCByGrrNoWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstQCByGrrNo;
        }

        public IEnumerable<vmGrr> GetMrrMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_GF_vmGrr = new vmGrr_GF();
            string spQuery = "";
            IEnumerable<vmGrr> lstMrrMaster = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("TransactionTypeID", objcmnParam.tTypeId);

                spQuery = "[Get_InvMrrMasterInfo]";

                lstMrrMaster = GenericFactory_GF_vmGrr.ExecuteQuery(spQuery, ht);

                recordsTotal = (int)lstMrrMaster.FirstOrDefault().RecordTotal;
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMrrMaster;
        }

        public IEnumerable<InvMrrMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno)
        {
            IEnumerable<InvMrrMaster> lstMNo = null; 
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstMNo = (from rm in _ctxCmn.InvMrrMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.ManualMRRNo == Mno) select new
                                                 {
                                                     MrrID = rm.MrrID,
                                                     ManualMRRNo = rm.ManualMRRNo

                                                 }).ToList().Select(x => new InvMrrMaster
                                                 {
                                                     MrrID = x.MrrID,
                                                     ManualMRRNo = x.ManualMRRNo
                                                 }).ToList(); 
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstMNo;
        }
        public IEnumerable<vmQC> GetDetailInfoByQCID(vmCmnParameters objcmnParam, Int64 mrrQCID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstQCByGrrNo = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("MrrQcID", mrrQCID); 
              //  spQuery = "[Get_InvGrrItemByQCID]";

                spQuery = "[Get_InvPOItemDetailByQCID]"; 
                lstQCByGrrNo = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht); 
                recordsTotal = lstQCByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQCByGrrNo;
        }

        public IEnumerable<vmQC> GetDetailInfoByLoanSprID(vmCmnParameters objcmnParam, Int64 LoanSprID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstQCByGrrNo = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.selectedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("LoanSprID", LoanSprID); 

                // spQuery = "[Get_InvPOItemDetailByQCID]";

                spQuery = "[Get_InvItemsDetailByLoanSprID]";

                lstQCByGrrNo = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);
                recordsTotal = lstQCByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQCByGrrNo;
        }

        public IEnumerable<vmQC> GetDetailInfoByIssueID(vmCmnParameters objcmnParam, Int64 IssueID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstByIssueID = null; 
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("IssueID", IssueID);

                spQuery = "[Get_InvDetailByIssueID]";

                lstByIssueID = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);

                recordsTotal = lstByIssueID.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstByIssueID;
        }

        public IEnumerable<vmQC> GetMrrDetailsListByMrrID(vmCmnParameters objcmnParam, Int64 mrrID, out int recordsTotal)
        {
            GenericFactory_GF_vmQC = new vmQC_GF();
            string spQuery = "";
            IEnumerable<vmQC> lstMrrDetailsByMrrID = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("MrrID", mrrID);

                spQuery = "[Get_InvMrrMasterDetailByMrrID]";

                lstMrrDetailsByMrrID = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);

                recordsTotal = lstMrrDetailsByMrrID.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMrrDetailsByMrrID;
        }

        public string SaveUpdateMrrMasterNdetails(InvMrrMaster mrrMaster, List<InvMrrDetail> mrrDetails, int menuID)
        {
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                if (mrrMaster.MrrID > 0)
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            Int64 mrrID = mrrMaster.MrrID;
                            IEnumerable<InvMrrMaster> lstInvMrrMaster = (from qcm in _ctxCmn.InvMrrMasters.Where(m => m.MrrID == mrrID && m.IsDeleted == false && m.CompanyID==mrrMaster.CompanyID) select qcm).ToList();
                            InvMrrMaster objInvMrrMaster = new InvMrrMaster();
                            foreach (InvMrrMaster qcms in lstInvMrrMaster)
                            {
                                qcms.UpdateBy = mrrMaster.CreateBy;
                                qcms.UpdateOn = DateTime.Now;
                                qcms.UpdatePc =  HostService.GetIP();
                                qcms.ChallanNo = mrrMaster.ChallanNo;
                                qcms.CHID = mrrMaster.CHID;
                                qcms.CompanyID = mrrMaster.CompanyID;
                                 
                                // qcms.MRRCertificateNo = mrrMaster.MRRCertificateNo;
                                qcms.ManualMRRNo = mrrMaster.ManualMRRNo;
                                qcms.GrrID = mrrMaster.GrrID;
                                qcms.MrrTypeID = mrrMaster.MrrTypeID;
                                qcms.CurrencyID = mrrMaster.CurrencyID;
                                qcms.MrrDate = mrrMaster.MrrDate;
                                qcms.DepartmentID = mrrMaster.DepartmentID;
                                qcms.Description = mrrMaster.Description;
                                qcms.IsDeleted = false;
                                qcms.SupplierID = mrrMaster.SupplierID;
                                qcms.MrrNo = mrrMaster.MrrNo;
                                qcms.MrrQcID = mrrMaster.MrrQcID;
                                qcms.MrrTypeID = mrrMaster.MrrTypeID;
                                qcms.PIID = mrrMaster.PIID;
                                qcms.POID = mrrMaster.POID;
                                qcms.PONo = mrrMaster.PONo;
                                qcms.Remarks = mrrMaster.Remarks;
                                qcms.ReqNo = mrrMaster.ReqNo;
                                qcms.RequisitionID = mrrMaster.RequisitionID;
                                qcms.FromDepartmentID = mrrMaster.FromDepartmentID;
                                qcms.IssueID = mrrMaster.IssueID;
                                qcms.UserID = mrrMaster.UserID;
                                //qcms.StatusBy = mrrMaster.StatusBy;
                                //qcms.StatusDate = mrrMaster.StatusDate;
                                //qcms.StatusID = mrrMaster.StatusID;
                                qcms.IsAccountsCompleted = mrrMaster.IsAccountsCompleted;
                                qcms.IsApproved = mrrMaster.IsApproved;
                                qcms.IsStoreCompleted = mrrMaster.IsStoreCompleted;

                                objInvMrrMaster = qcms;

                                if (mrrMaster.IsStoreCompleted == true)
                                {
                                    //  for one save then grrcomplete
                                    InvMrrQcMaster objInvMrrQcMaster = (from spr in _ctxCmn.InvMrrQcMasters.Where(m => m.MrrQcID == mrrMaster.MrrQcID && m.CompanyID == mrrMaster.CompanyID) select spr).FirstOrDefault();
                                    objInvMrrQcMaster.IsMrrCompleted = true;
                                }
                            }
                            List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                            foreach (InvMrrDetail qcdt in mrrDetails)
                            {
                                InvMrrDetail objInvMrrDetail = (from qcdetl in _ctxCmn.InvMrrDetails.Where(m => m.MrrDetailID == qcdt.MrrDetailID && m.IsDeleted == false) select qcdetl).FirstOrDefault();

                                objInvMrrDetail.AdditionalQty = qcdt.AdditionalQty;
                                objInvMrrDetail.IsQcCompleted = false;
                                objInvMrrDetail.Amount = qcdt.Amount;
                                objInvMrrDetail.BatchID = qcdt.BatchID;
                                objInvMrrDetail.IsDeleted = false;
                                objInvMrrDetail.GradeID = qcdt.GradeID;
                                objInvMrrDetail.UnitID = qcdt.UnitID;
                                objInvMrrDetail.UpdateBy = mrrMaster.CreateBy;
                                objInvMrrDetail.UpdateOn = DateTime.Now;
                                objInvMrrDetail.UpdatePc =  HostService.GetIP();
                                objInvMrrDetail.ItemID = qcdt.ItemID;
                                objInvMrrDetail.LotID = qcdt.LotID;
                                objInvMrrDetail.Qty = qcdt.Qty;
                                objInvMrrDetail.UnitPrice = qcdt.UnitPrice;
                                lstInvMrrDetail.Add(objInvMrrDetail);

                            }
                            _ctxCmn.SaveChanges();
                            transaction.Complete();
                            result = mrrMaster.MrrNo.ToString();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            result = "";
                        }
                    }
                }
                else
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            //...........START  new maxId........//
                            long NextId = Convert.ToInt64(GenericFactory_EF_CmnCombo.getMaxID("InvMrrMaster"));

                            long FirstDigit = 0;
                            long OtherDigits = 0;
                            long nextDetailId = Convert.ToInt64(GenericFactory_EF_CmnCombo.getMaxID("InvMrrDetail"));
                            FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                            //..........END new maxId.........//


                            //......... START for custom code........... //
                            string customCode = "";

                            string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, mrrMaster.CompanyID, 1, 1);
                            if (CustomNo != null)
                            {
                                customCode = CustomNo;
                            }
                            else
                            {
                                customCode = NextId.ToString();
                            }
                            //.........END for custom code............ //

                            string newMrrNo = customCode;
                            mrrMaster.MrrID = NextId;
                            mrrMaster.CreateOn = DateTime.Now;
                            mrrMaster.CreatePc =  HostService.GetIP();
                            mrrMaster.MrrNo = newMrrNo;
                            mrrMaster.IsDeleted = false;


                            if (mrrMaster.IsStoreCompleted == true)
                            {
                                //  for one save then grrcomplete
                                InvMrrQcMaster objInvMrrQcMaster = (from spr in _ctxCmn.InvMrrQcMasters.Where(m => m.MrrQcID == mrrMaster.MrrQcID && m.CompanyID == mrrMaster.CompanyID) select spr).FirstOrDefault();
                                objInvMrrQcMaster.IsMrrCompleted = true;
                            }


                            List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                            foreach (InvMrrDetail sdtl in mrrDetails)
                            {
                                
                                InvMrrDetail objInvMrrDetail = new InvMrrDetail();
                                objInvMrrDetail.MrrDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                objInvMrrDetail.MrrID = NextId;
                                objInvMrrDetail.ItemID = sdtl.ItemID;
                                objInvMrrDetail.Amount = sdtl.Amount;
                                objInvMrrDetail.UnitPrice = sdtl.UnitPrice;
                                objInvMrrDetail.BatchID = sdtl.BatchID;
                                objInvMrrDetail.IsDeleted = false;
                                objInvMrrDetail.AdditionalQty = sdtl.AdditionalQty; 
                                // objInvMrrDetail.IsQcCompleted = mrrMaster.MrrQcID > 0 ? true : false;
                                objInvMrrDetail.LotID = sdtl.LotID;
                                objInvMrrDetail.Qty = sdtl.Qty;
                                objInvMrrDetail.UnitID = sdtl.UnitID;
                                objInvMrrDetail.CreateBy = mrrMaster.CreateBy;//sdtl.CreateBy;
                                objInvMrrDetail.CreateOn = DateTime.Now;
                                objInvMrrDetail.CreatePc =  HostService.GetIP();
                                lstInvMrrDetail.Add(objInvMrrDetail);
                                OtherDigits++;
                            }

                            _ctxCmn.InvMrrMasters.Add(mrrMaster);
                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("InvMrrMaster", Convert.ToInt64(NextId));
                            //............Update CustomCode.............//
                            GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, mrrMaster.CompanyID, 1, 1);
                            _ctxCmn.InvMrrDetails.AddRange(lstInvMrrDetail);

                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("InvMrrDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            _ctxCmn.SaveChanges();

                            transaction.Complete();
                            result = newMrrNo;
                        }
                        catch (Exception e)
                        {
                            result = "";
                        }
                    }
                }
            }
            return result;
        }

        public string SaveLot(CmnLot objCmnLot)
        {
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                if (objCmnLot.LotID > 0)
                {
                    //using (TransactionScope transaction = new TransactionScope())
                    //{
                    //    try
                    //    {
                    //        Int64 mrrID = mrrMaster.MrrID;
                    //        IEnumerable<InvMrrMaster> lstInvMrrMaster = (from qcm in _ctxCmn.InvMrrMasters.Where(m => m.MrrID == mrrID) select qcm).ToList();
                    //        InvMrrMaster objInvMrrMaster = new InvMrrMaster();
                    //        foreach (InvMrrMaster qcms in lstInvMrrMaster)
                    //        {
                    //            qcms.UpdateBy = mrrMaster.CreateBy;
                    //            qcms.UpdateOn = DateTime.Now;
                    //            qcms.UpdatePc =  HostService.GetIP();
                    //            qcms.ChallanNo = mrrMaster.ChallanNo;
                    //            qcms.CHID = mrrMaster.CHID;
                    //            qcms.CompanyID = mrrMaster.CompanyID;
                    //            qcms.GrrID = mrrMaster.GrrID;
                    //            qcms.CurrencyID = mrrMaster.CurrencyID;
                    //            qcms.MrrDate = mrrMaster.MrrDate;
                    //            qcms.DepartmentID = mrrMaster.DepartmentID;
                    //            qcms.Description = mrrMaster.Description;
                    //            qcms.IsDeleted = false;
                    //            qcms.MrrNo = mrrMaster.MrrNo;
                    //            qcms.MrrQcID = mrrMaster.MrrQcID;
                    //            qcms.MrrTypeID = mrrMaster.MrrTypeID;
                    //            qcms.PIID = mrrMaster.PIID;
                    //            qcms.POID = mrrMaster.POID;
                    //            qcms.PONo = mrrMaster.PONo;
                    //            qcms.Remarks = mrrMaster.Remarks;
                    //            qcms.ReqNo = mrrMaster.ReqNo;
                    //            qcms.RequisitionID = mrrMaster.RequisitionID;
                    //            //qcms.StatusBy = mrrMaster.StatusBy;
                    //            //qcms.StatusDate = mrrMaster.StatusDate;
                    //            //qcms.StatusID = mrrMaster.StatusID;

                    //            objInvMrrMaster = qcms;
                    //        }
                    //        List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                    //        foreach (InvMrrDetail qcdt in mrrDetails)
                    //        {
                    //            InvMrrDetail objInvMrrDetail = (from qcdetl in _ctxCmn.InvMrrDetails.Where(m => m.MrrDetailID == qcdt.MrrDetailID) select qcdetl).FirstOrDefault();
                    //            //start for exist passed n reject qty 
                    //            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                    //            //end for exist passed n reject qty 

                    //            objInvMrrDetail.Amount = qcdt.Amount;
                    //            objInvMrrDetail.BatchID = qcdt.BatchID;
                    //            objInvMrrDetail.IsDeleted = false;
                    //            objInvMrrDetail.GradeID = qcdt.GradeID;
                    //            objInvMrrDetail.UnitID = qcdt.UnitID;
                    //            objInvMrrDetail.UpdateBy = mrrMaster.CreateBy;
                    //            objInvMrrDetail.UpdateOn = DateTime.Now;
                    //            objInvMrrDetail.UpdatePc =  HostService.GetIP();
                    //            objInvMrrDetail.ItemID = qcdt.ItemID;
                    //            objInvMrrDetail.LotID = qcdt.LotID;
                    //            objInvMrrDetail.Qty = qcdt.Qty;
                    //            objInvMrrDetail.UnitPrice = qcdt.UnitPrice;
                    //            lstInvMrrDetail.Add(objInvMrrDetail);

                    //            //InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                    //            //objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
                    //        }
                    //        _ctxCmn.SaveChanges();
                    //        transaction.Complete();
                    //        result = mrrMaster.MrrNo.ToString();
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        e.ToString();
                    //        result = "";
                    //    }
                    //}
                }
                else
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            //...........START  new maxId........//
                            long NextId = Convert.ToInt64(GenericFactory_EF_CmnCombo.getMaxID("CmnLot"));

                            objCmnLot.LotID = NextId;
                            objCmnLot.CreateOn = DateTime.Now;
                            objCmnLot.CreatePc =  HostService.GetIP();
                            objCmnLot.IsDeleted = false;
                            _ctxCmn.CmnLots.Add(objCmnLot);
                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("CmnLot", Convert.ToInt64(NextId));
                            _ctxCmn.SaveChanges();
                            transaction.Complete();
                            result = NextId.ToString();
                        }
                        catch (Exception e)
                        {
                            result = "";
                        }
                    }
                }
            }
            return result;
        }

        public string SaveBatch(CmnBatch objCmnBatch)
        {
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                if (objCmnBatch.BatchID > 0)
                {
                    //using (TransactionScope transaction = new TransactionScope())
                    //{
                    //    try
                    //    {
                    //        Int64 mrrID = mrrMaster.MrrID;
                    //        IEnumerable<InvMrrMaster> lstInvMrrMaster = (from qcm in _ctxCmn.InvMrrMasters.Where(m => m.MrrID == mrrID) select qcm).ToList();
                    //        InvMrrMaster objInvMrrMaster = new InvMrrMaster();
                    //        foreach (InvMrrMaster qcms in lstInvMrrMaster)
                    //        {
                    //            qcms.UpdateBy = mrrMaster.CreateBy;
                    //            qcms.UpdateOn = DateTime.Now;
                    //            qcms.UpdatePc =  HostService.GetIP();
                    //            qcms.ChallanNo = mrrMaster.ChallanNo;
                    //            qcms.CHID = mrrMaster.CHID;
                    //            qcms.CompanyID = mrrMaster.CompanyID;
                    //            qcms.GrrID = mrrMaster.GrrID;
                    //            qcms.CurrencyID = mrrMaster.CurrencyID;
                    //            qcms.MrrDate = mrrMaster.MrrDate;
                    //            qcms.DepartmentID = mrrMaster.DepartmentID;
                    //            qcms.Description = mrrMaster.Description;
                    //            qcms.IsDeleted = false;
                    //            qcms.MrrNo = mrrMaster.MrrNo;
                    //            qcms.MrrQcID = mrrMaster.MrrQcID;
                    //            qcms.MrrTypeID = mrrMaster.MrrTypeID;
                    //            qcms.PIID = mrrMaster.PIID;
                    //            qcms.POID = mrrMaster.POID;
                    //            qcms.PONo = mrrMaster.PONo;
                    //            qcms.Remarks = mrrMaster.Remarks;
                    //            qcms.ReqNo = mrrMaster.ReqNo;
                    //            qcms.RequisitionID = mrrMaster.RequisitionID;
                    //            //qcms.StatusBy = mrrMaster.StatusBy;
                    //            //qcms.StatusDate = mrrMaster.StatusDate;
                    //            //qcms.StatusID = mrrMaster.StatusID;

                    //            objInvMrrMaster = qcms;
                    //        }
                    //        List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                    //        foreach (InvMrrDetail qcdt in mrrDetails)
                    //        {
                    //            InvMrrDetail objInvMrrDetail = (from qcdetl in _ctxCmn.InvMrrDetails.Where(m => m.MrrDetailID == qcdt.MrrDetailID) select qcdetl).FirstOrDefault();
                    //            //start for exist passed n reject qty 
                    //            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                    //            //end for exist passed n reject qty 

                    //            objInvMrrDetail.Amount = qcdt.Amount;
                    //            objInvMrrDetail.BatchID = qcdt.BatchID;
                    //            objInvMrrDetail.IsDeleted = false;
                    //            objInvMrrDetail.GradeID = qcdt.GradeID;
                    //            objInvMrrDetail.UnitID = qcdt.UnitID;
                    //            objInvMrrDetail.UpdateBy = mrrMaster.CreateBy;
                    //            objInvMrrDetail.UpdateOn = DateTime.Now;
                    //            objInvMrrDetail.UpdatePc =  HostService.GetIP();
                    //            objInvMrrDetail.ItemID = qcdt.ItemID;
                    //            objInvMrrDetail.LotID = qcdt.LotID;
                    //            objInvMrrDetail.Qty = qcdt.Qty;
                    //            objInvMrrDetail.UnitPrice = qcdt.UnitPrice;
                    //            lstInvMrrDetail.Add(objInvMrrDetail);

                    //            //InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                    //            //objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
                    //        }
                    //        _ctxCmn.SaveChanges();
                    //        transaction.Complete();
                    //        result = mrrMaster.MrrNo.ToString();
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        e.ToString();
                    //        result = "";
                    //    }
                    //}
                }
                else
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            //...........START  new maxId........//
                            long NextId = Convert.ToInt64(GenericFactory_EF_CmnCombo.getMaxID("CmnBatch"));

                            objCmnBatch.BatchID = NextId;
                            objCmnBatch.CreateOn = DateTime.Now;
                            objCmnBatch.CreatePc =  HostService.GetIP();
                            objCmnBatch.BatchDate = DateTime.Now;
                            objCmnBatch.IsDeleted = false;
                            _ctxCmn.CmnBatches.Add(objCmnBatch);
                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("CmnBatch", Convert.ToInt64(NextId));
                            _ctxCmn.SaveChanges();

                            transaction.Complete();
                            result = NextId.ToString();
                        }
                        catch (Exception e)
                        {
                            result = "";
                        }
                    }
                }
            }
            return result;
        }

        public object[] GetWherehouseList(vmCmnParameters objcmnParam, out int recordsTotal) 
        {
            object[] obj = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    obj = (
                           from og in _ctxCmn.CmnOrganograms // Factory
                           where og.ParentID == null
                           select new
                           {
                               OrganogramID = og.OrganogramID,
                               ParentID = og.ParentID,
                               IsBranch = og.IsBranch,
                               IsDepartment = og.IsDepartment,
                               OrganogramName = og.OrganogramName,
                               ListDpt = from dt in _ctxCmn.CmnOrganograms //Store
                                         where dt.ParentID == og.OrganogramID
                                         select new
                                         {
                                             OrganogramID = dt.OrganogramID,
                                             ParentID = dt.ParentID,
                                             IsBranch = dt.IsBranch,
                                             IsDepartment = dt.IsDepartment,
                                             OrganogramName = dt.OrganogramName,
                                             ListBrn = from br in _ctxCmn.CmnOrganograms //Corner
                                                       where br.ParentID == dt.OrganogramID
                                                       select new
                                                       {
                                                           OrganogramID = br.OrganogramID,
                                                           ParentID = br.ParentID,
                                                           IsBranch = br.IsBranch,
                                                           IsDepartment = br.IsDepartment,
                                                           OrganogramName = br.OrganogramName,
                                                           ListShelf = from sh in _ctxCmn.CmnOrganograms //Shelf
                                                                       where sh.ParentID == br.OrganogramID
                                                                       select new
                                                                       {
                                                                           OrganogramID = sh.OrganogramID,
                                                                           ParentID = sh.ParentID,
                                                                           IsBranch = sh.IsBranch,
                                                                           IsDepartment = sh.IsDepartment,
                                                                           OrganogramName = sh.OrganogramName,
                                                                           ListRack = from rk in _ctxCmn.CmnOrganograms //Rack
                                                                                      where rk.ParentID == sh.OrganogramID
                                                                                      select new
                                                                                      {
                                                                                          OrganogramID = rk.OrganogramID,
                                                                                          ParentID = rk.ParentID,
                                                                                          IsBranch = rk.IsBranch,
                                                                                          IsDepartment = rk.IsDepartment,
                                                                                          OrganogramName = rk.OrganogramName
                                                                                      }
                                                                       }
                                                       }
                                         }
                           }).ToArray();
                }
                catch(Exception e)
                {
                
                }
            }

            return obj; 
        }
    }
}
