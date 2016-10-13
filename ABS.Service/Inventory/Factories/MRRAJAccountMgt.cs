using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.MenuMgt;
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
    public class MRRAJAccountMgt : iMRRAJAccountMgt
    {

        private ERP_Entities _ctxCmn = null; 
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
        private iGenericFactory<vmGrr> GenericFactory_GF_vmGrr = null;
        private iGenericFactory<vmQC> GenericFactory_GF_vmQC = null;

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

        public IEnumerable<InvMrrQcMaster> GetQCList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<InvMrrQcMaster> lstQCByGrrNo = null;
            IEnumerable<InvMrrQcMaster> lstQCByGrrNoWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstQCByGrrNoWithoutPaging = (from rm in _ctxCmn.InvMrrQcMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.IsMrrCompleted==false )

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

        public IEnumerable<vmGrr> GetMrrMasterListAccount(vmCmnParameters objcmnParam, out int recordsTotal)
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
  
                spQuery = "[Get_InvMrrMasterInfoAccount]";

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

                spQuery = "[Get_InvMrrDetailByMrrIDAccount]";

                lstMrrDetailsByMrrID = GenericFactory_GF_vmQC.ExecuteQuery(spQuery, ht);

                recordsTotal = lstMrrDetailsByMrrID.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMrrDetailsByMrrID;
        }

        public async Task<string> SaveUpdateMrrMasterNdetails(InvMrrMaster mrrMaster, List<InvMrrDetail> mrrDetails, int menuID, UserCommonEntity commonEntity)
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
                            bool chkFrstUpdateFAprvlPerm =false;

                            // start for firstNotifier user
                             bool chkFirstNtfier = false;

                             List<vmCmnWorkFlowMaster> listWorkFlowChk = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                             foreach (vmCmnWorkFlowMaster item in listWorkFlowChk)
                             {
                                 chkFirstNtfier = new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID);
                             }
                             // end for firstNotifier user


                            IEnumerable<InvMrrMaster> lstInvMrrMaster = (from qcm in _ctxCmn.InvMrrMasters.Where(m => m.MrrID == mrrID && m.IsDeleted == false && m.CompanyID==mrrMaster.CompanyID) select qcm).ToList();

                            chkFrstUpdateFAprvlPerm = lstInvMrrMaster.FirstOrDefault().UpdateBy == null ? false : true;

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

                                if (chkFirstNtfier == false && mrrMaster.IsApproved == true)
                                {
                                    qcms.IsApproved = true;
                                }

                                else 
                                {
                                    qcms.IsApproved = false;
                                }

                               // qcms.IsApproved = chkFirstNtfier==true?false:true;

                                qcms.IsStoreCompleted = mrrMaster.IsStoreCompleted;

                                objInvMrrMaster = qcms;

                                //if (mrrMaster.IsStoreCompleted == true)
                                //{
                                //    //  for one save then grrcomplete
                                //    InvMrrQcMaster objInvMrrQcMaster = (from spr in _ctxCmn.InvMrrQcMasters.Where(m => m.MrrQcID == mrrMaster.MrrQcID && m.CompanyID == mrrMaster.CompanyID) select spr).FirstOrDefault();
                                //    objInvMrrQcMaster.IsMrrCompleted = true;
                                //}
                            }

                            List<AccVoucherDetail> lstVchrD = new List<AccVoucherDetail>();
                            decimal totalAmount = 0.00m;
                            result = "1";
                            var itemMaxHeight = _ctxCmn.AccVoucherMasters.Max(y => y.Id);
                            var itemsMax = _ctxCmn.AccVoucherMasters.Where(x => x.Id == itemMaxHeight);
                            int VMasterId = itemsMax.FirstOrDefault().Id + 1;
                            string voucherNo = VoucherCodeGenerate("JV", DateTime.Today, mrrMaster.CompanyID);

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

                                //  add  items  for jurnal
                                if (mrrMaster.IsApproved == true && chkFirstNtfier==false)
                                {
                                   
                                    AccVoucherDetail vchrD = new AccVoucherDetail(); 
                                    CmnACCIntegration cmnaCCIntegrationItem = new CmnACCIntegration();
                                    cmnaCCIntegrationItem = (from acdtlid in _ctxCmn.CmnACCIntegrations.Where(m => m.TransactionID == objInvMrrDetail.ItemID && m.CompanyID == mrrMaster.CompanyID) select acdtlid).FirstOrDefault();
                                   
                                    if (cmnaCCIntegrationItem.AcDetailID!= null && cmnaCCIntegrationItem.AcDetailID != 0)
                                    {
                                        vchrD.AC5Id = (int)cmnaCCIntegrationItem.AcDetailID;
                                        vchrD.DebitAmount= (decimal)objInvMrrDetail.Amount;
                                        vchrD.CostCenterId = 1;
                                        vchrD.TransactionType = "Debit";


                                        vchrD.VMasterId = VMasterId;
                                        vchrD.VoucherNo = voucherNo;
                                        vchrD.IsActive = true;


                                        lstVchrD.Add(vchrD);

                                        totalAmount = totalAmount + (decimal)objInvMrrDetail.Amount;
                                       
                                    }
                                    else if (cmnaCCIntegrationItem.AcDetailID == null || cmnaCCIntegrationItem.AcDetailID == 0)
                                        {
                                            result = "-1";
                                          //  transaction.Dispose();
                                        }
                                }
                            }

                            //********* start  for journal creation 

                            if (mrrMaster.IsApproved == true && chkFirstNtfier == false)  
                            {
                                AccVoucherMaster vchrM  =  new AccVoucherMaster();

                                string supplierName = (from sprNwame in _ctxCmn.CmnUsers
                                                           .Where(m => m.UserID == mrrMaster.SupplierID)
                                                           .ToList() select (new CmnUser { UserFullName = sprNwame.UserFullName })).FirstOrDefault().UserFullName;
                                
                               
                                vchrM.Id = VMasterId;
                                vchrM.CompanyId = mrrMaster.CompanyID;
                                vchrM.CreatedBy = (int)mrrMaster.CreateBy;
                                vchrM.CreatedDate = DateTime.Now;
                                vchrM.CurrencyId = mrrMaster.CurrencyID;
                                vchrM.IPAddress = HostService.GetIP();
                                vchrM.IsActive = true;
                                vchrM.Narration = "Bought From " + supplierName + " and Amount is " + totalAmount;
                                vchrM.PaymentTo = supplierName;
                                vchrM.ReferenceNo = mrrMaster.MrrNo;
                                vchrM.SerialNo = GetSerialNumber(DateTime.Today);
                                vchrM.VoucherDate = DateTime.Today;
                                vchrM.VoucherNo = voucherNo;
                                vchrM.ReferenceNo = lstInvMrrMaster.FirstOrDefault().MrrNo;
                                vchrM.VoucherTypeId = 5;// general voucher; 


                                 
                                //  add  supplier  for jurnal
                                AccVoucherDetail vchrDSupllier = new AccVoucherDetail();
                                //vchrDSupllier.AC5Id = (int)(from acdtlid in _ctxCmn.CmnACCIntegrations
                                //                                .Where(m => m.TransactionID == mrrMaster.SupplierID)
                                //                                .ToList()
                                //                            select acdtlid).FirstOrDefault().AcDetailID;

                                //CmnACCIntegration cmnaCCIntegrationSupplier = new CmnACCIntegration();
                                int AcDetailID = (int)(_ctxCmn.CmnACCIntegrations.Where(m => m.TransactionID == mrrMaster.SupplierID && m.CompanyID == mrrMaster.CompanyID).FirstOrDefault().AcDetailID);
                                    //(from acdtlid in _ctxCmn.CmnACCIntegrations.Where(m => m.TransactionID == mrrMaster.SupplierID && m.CompanyID == mrrMaster.CompanyID) select acdtlid).FirstOrDefault();
                                 
                                if (AcDetailID!= null && AcDetailID != 0)
                                {
                                    vchrDSupllier.AC5Id = (int)AcDetailID;
                                    vchrDSupllier.CreditAmount = totalAmount;
                                    vchrDSupllier.CostCenterId = 1;
                                    vchrDSupllier.TransactionType = "Credit";
                                    vchrDSupllier.VMasterId = VMasterId;
                                    vchrDSupllier.VoucherNo = voucherNo;
                                    vchrDSupllier.IsActive = true;

                                    lstVchrD.Add(vchrDSupllier);

                                    //  start for cmn AcDetailID of multiple item with CreditAmount sum 

                                    List<AccVoucherDetail> lstVchrDCmnItm = new List<AccVoucherDetail>(); 

                                    lstVchrDCmnItm = (from gdq in lstVchrD
                                                      group gdq by new { gdq.AC5Id, gdq.VMasterId, gdq.CostCenterId, gdq.TransactionType, gdq.VoucherNo, gdq.IsActive, gdq.DebitAmount } into grdqt
                                                      select new AccVoucherDetail
                                                      {
                                                          AC5Id = (int)grdqt.Key.AC5Id, 
                                                          CreditAmount = (decimal)grdqt.Sum(g => g.CreditAmount),
                                                          DebitAmount = (decimal)grdqt.Sum(g=>g.DebitAmount),
                                                          CostCenterId = grdqt.Key.CostCenterId,
                                                          TransactionType = grdqt.Key.TransactionType,
                                                          VMasterId = (int)grdqt.Key.VMasterId,
                                                          VoucherNo=grdqt.Key.VoucherNo,
                                                          IsActive = grdqt.Key.IsActive 
                                                      }).ToList();


                                    //  end for cmn AcDetailID of multiple item with CreditAmount sum 

                                    _ctxCmn.AccVoucherMasters.Add(vchrM);
                                    _ctxCmn.AccVoucherDetails.AddRange(lstVchrDCmnItm);
                                   //  _ctxCmn.AccVoucherDetails.AddRange(lstVchrD);
                                     
                                }
                                else if (AcDetailID == null ||  AcDetailID == 0)
                                {

                                    result = "-2";
                                   // transaction.Dispose();
                                } 
 
                            }

                            //********* end  for journal creation

                             

                            #region WorkFlow Transaction Entry Team

                            if (chkFrstUpdateFAprvlPerm == false)
                            {
                                int workflowID = 0;
                                List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                                foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                                {
                                    int userTeamID = item.UserTeamID ?? 0;
                                    if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID) && userTeamID > 0)
                                    {
                                        if (result != "-2" || result != "-1")
                                        {
                                            item.WorkFlowTranCustomID = (int)mrrMaster.MrrID;
                                            workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                                        }
                                        //Mail Service 

                                    }
                                    if (userTeamID == 0)
                                    {
                                        if (result != "-2" || result != "-1")
                                        {
                                            item.WorkFlowTranCustomID = (int)mrrMaster.MrrID;
                                            workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                                        }
                                    }
                                }

                               //  start for mail

                                //int mail = 0;
                                //foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                                //{
                                //    NotificationEntity notification = new NotificationEntity();
                                //    notification.WorkFlowID = item.WorkFlowID;
                                //    notification.TransactionID = Convert.ToInt16(mrrMaster.MrrID);
                                //    List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(notification, "Accounts Approved.");
                                //    foreach (var mailModel in nModel)
                                //    {
                                //        mail = await new EmailService().NotificationMail(mailModel);
                                //    }
                                //}
                                 
                            }

                            #endregion Workflow Transaction Enltry Team
                            if (result != "-2" && result != "-1")
                            {
                                _ctxCmn.SaveChanges();
                                transaction.Complete();
                                result = mrrMaster.MrrNo.ToString(); 
                            } 
                            else if (result == "-2" || result == "-1")
                            { 
                                transaction.Dispose();
                               
                            }
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

     

        public string VoucherCodeGenerate(string voucherType, DateTime voucherDate, int companyId)
        {
            string pvCode = null;
            string serialNo = GetAutoNumber(voucherType, voucherDate);
            using (_ctxCmn = new ERP_Entities())
            {
               
                var singleCompany = (from r in _ctxCmn.CmnCompanies
                                     where r.CompanyID == companyId
                                     select r.CompanyShortName).ToList().SingleOrDefault();
                string company = singleCompany;
                
                try
                {

                    var dt = voucherDate; 
                    string day = dt.ToString("dd");
                    string yy = dt.ToString("yy");
                    string month = dt.ToString("MM"); 
                    string year = day + month + yy; 
                    if (serialNo != null && serialNo.Length == 1)
                    {
                        pvCode = company.Trim() + "-" + voucherType + "-" + year + "-000" + serialNo;
                    }

                    if (serialNo != null && serialNo.Length == 2)
                    {
                        pvCode = company.Trim() + "-" + voucherType + "-" + year + "-00" + serialNo;
                    }

                    if (serialNo != null && serialNo.Length == 3)
                    {
                        pvCode = company.Trim() + "-" + voucherType + "-" + year + "-0" + serialNo;
                    }
                    if (serialNo != null && serialNo.Length == 4)
                    {
                        pvCode = company.Trim() + "-" + voucherType + "-" + year + "-" + serialNo;
                    }

                }
                catch (Exception)
                {
                }
            }

            return pvCode;
        }

        private string GetAutoNumber(string voucherType, DateTime voucherDate)
        {
            string id = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    if (voucherType == "CPV")
                    {
                        var data =
                            _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 1 && r.VoucherDate == voucherDate).ToList();
                        id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                    }
                    else if (voucherType == "BPV")
                    {
                        var data =
                            _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 2 && r.VoucherDate == voucherDate).ToList();
                        id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();
                    }
                    else if (voucherType == "RV")
                    {
                        var data =
                            _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 3 && r.VoucherDate == voucherDate).ToList();
                        id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();
                    }

                    else if (voucherType == "CV")
                    {
                        var data =
                            _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 4 && r.VoucherDate == voucherDate).ToList();
                        id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();
                    }
                    else if (voucherType == "JV")
                    {
                        var data =
                            _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 5 && r.VoucherDate == voucherDate).ToList();
                        id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();
                    }

                }
                catch (Exception)
                {
                    id = "1";
                }
            }
            return id;
        }

        private int GetSerialNumber(DateTime voucherDate)
        {
            int id;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                { 
                    var data = _ctxCmn.AccVoucherMasters.Where(r => r.VoucherTypeId == 5 && r.VoucherDate == voucherDate).ToList();
                    id = Convert.ToInt32(data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString());
                }
                catch (Exception)
                {
                    id = 1;
                }
            }
            return id;
        }

        public async Task<dynamic> ApproveNotification(NotificationEntity model)
        {
            int returnValue = 0;
            try
            {
                List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(model, "DO Created.");
                foreach (var item in nModel)
                {
                    returnValue = await new EmailService().NotificationMail(item);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return returnValue;
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
