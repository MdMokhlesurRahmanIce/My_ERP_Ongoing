using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
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
    public class QuotationMgt : iQuotationMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmRequisitionDetails> GFactory_GF_RequisitionDetail = null;
        private iGenericFactory_EF<CmnUser> GenericFactory_EF_PIBuyer = null;
        private iGenericFactory<InvRequisitionMaster> GFactory_GF_Requisition = null;
        private iGenericFactory<vmQuotation> GFactory_GF_Quotation = null;
        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_PICompany = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
        private iGenericFactory_EF<SalPIMaster> GenericFactory_EF_SalPIMaster = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_SampleNo = null;
        private iGenericFactory_EF<SalPIDetail> GenericFactory_EF_SalPIDetail = null;
        private iGenericFactory_EF<CmnBank> GFactory_EF_CmnBank = null;
        private iGenericFactory_EF<CmnBankBranch> GFactory_EF_CmnBankBranch = null;
        private iGenericFactory_EF<CmnBankAdvising> GFactory_EF_CmnBankAdvising = null;
        // private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode = null;
        private iGenericFactory_EF<CmnItemColor> GFactory_EF_CmnItemColor = null;
        private iGenericFactory_EF<RndYarnCR> GenericFactory_EF_RndYarnCR = null;

        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactory_EF_CmnUserWiseCompany = null;
        private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode = null;

        private iGenericFactory<vmChallan> GenericFactory_GF_vmChallan = null;

        private iGenericFactory_EF<PurchaseQuotationMaster> GenericFactory_EF_QuotationMaster = null;
        private iGenericFactory_EF<PurchaseQuotationDetail> GenericFactory_EF_QuotationDetail = null;

      

        public IEnumerable<InvRequisitionMaster> GetSPR(vmCmnParameters objcmnParam)
        {
            GFactory_GF_Requisition = new InvRequisitionMaster_GF();
            IEnumerable<InvRequisitionMaster> lstSPR = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                spQuery = "Get_PurchaseQuotationSPRNo";
                lstSPR = GFactory_GF_Requisition.ExecuteQuery(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSPR;
        }

        public vmQuotation GetQuotationMasterById(int? QuotationId, int CompanyID)
        {          
          vmQuotation lstQuotation = null;
            GFactory_GF_Quotation = new vmQuotation_GF();
           
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("QuotationID", QuotationId);
                ht.Add("CompanyId", CompanyID);
                spQuery = "[dbo].[Get_PurchaseQuotationMaster]";
                lstQuotation = GFactory_GF_Quotation.ExecuteCommandSingle(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQuotation;;


            //try
            //{
            //    using (_ctxCmn = new ERP_Entities())
            //    {
            //        lstQuotation = (from qcm in _ctxCmn.PurchaseQuotationMasters.Where(s => s.IsDeleted == false && s.QuotationID == QuotationId && s.CompanyID == CompanyID)
            //                              join re in _ctxCmn.InvRequisitionMasters on qcm.RequisitionID equals re.RequisitionID
            //                              join org in _ctxCmn.CmnUsers on qcm.PartyID equals org.UserID
            //                              join cm in _ctxCmn.CmnComboes on qcm.QuotationTypeID equals cm.ComboID
            //                              join curr in _ctxCmn.AccCurrencyInfoes on qcm.CurrencyID equals curr.Id
            //                              select new vmQuotation
            //                              {
            //                                  RequisitionNo = re.RequisitionNo,
            //                                  RequisitionID = re.RequisitionID,
            //                                  QuotationID = qcm.QuotationID,
            //                                  QuotationNo = qcm.QuotationNo,
            //                                  QuotationDate = qcm.QuotationDate,
            //                                  SPRDate = re.RequisitionDate,
            //                                  DeliveryDate = qcm.DeliveryDate,
            //                                  UserID = org.UserID,
            //                                  UserFullName = org.UserFullName,
            //                                  ComboID = cm.ComboID,
            //                                  ComboName = cm.ComboName,
            //                                  CurrencyID = qcm.CurrencyID,
            //                                  Currency = curr.CurrencyName,
            //                                  Remarks = qcm.Remarks
            //                              });
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}

           
        }

        public IEnumerable<vmQuotation> GetQuotationDetailById(int? QuotationId, int CompanyID)
        {
            IEnumerable<vmQuotation> lstQuotation = null;
            GFactory_GF_Quotation = new vmQuotation_GF();
            string spQuery = "";
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("QuotationID", QuotationId);
                ht.Add("CompanyID", CompanyID);
                spQuery = "Get_PurchaseQuotationDetail";
                lstQuotation = GFactory_GF_Quotation.ExecuteQuery(spQuery, ht);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstQuotation;
        }


        //public QuotationMgt()
        //{
        //    GenericFactory_EF_PIBuyer = new CmnUser_EF();
        //    GenericFactory_EF_PICompany = new CmnCompany_EF();

        //    GenericFactory_EF_SalPIMaster = new SalPIMaster_EF();
        //    GenericFactory_EF_SampleNo = new CmnItemMaster_EF();
        //    GenericFactory_EF_SalPIDetail = new SalPIDetail_EF();
        //    GFactory_EF_CmnBank = new CmnBank_EF();
        //    GFactory_EF_CmnBankBranch = new CmnBankBranch_EF();
        //    GFactory_EF_CmnBankAdvising = new CmnBankAdvising_EF();
        //    GFactory_EF_CmnCustomCode = new SalCmnCustomCode_EF();
        //    GFactory_EF_CmnItemColor = new CmnItemColor_EF();
        //    GenericFactory_EF_RndYarnCR = new RndYarn_EF();
        //    GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
        //    GenericFactory_EF_CmnUserWiseCompany = new CmnUserWiseCompany_EF();



        //}



        public IEnumerable<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? UserTypeID, int? CompanyID)
        {
            iGenericFactory_EF<CmnUser> GenericFactory_EF_User = null;
            GenericFactory_EF_User = new CmnUser_EF();
            IEnumerable<CmnUser> objUsers = null;
            string spQuery = string.Empty;
            try
            {
                objUsers = GenericFactory_EF_User.GetAll().Select(m => new CmnUser { UserID = m.UserID, UserFullName = m.UserFullName, UserTypeID = m.UserTypeID, IsDeleted = m.IsDeleted, CompanyID = m.CompanyID }).Where(s => s.IsDeleted == false && s.UserTypeID == UserTypeID && s.CompanyID == CompanyID).OrderBy(m =>m.UserFullName).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUsers;
        }
        public IEnumerable<vmQuotation> GetQuotationList()
        {
            IEnumerable<vmQuotation> lstQuotationMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstQuotationMaster = _ctxCmn.PurchaseQuotationMasters.Where(m => m.IsDeleted == false).Select(m => new vmQuotation { QuotationID = m.QuotationID, QuotationNo = m.QuotationNo, CompanyID = m.CompanyID }).ToList();
                    lstQuotationMaster = lstQuotationMaster.OrderBy(m => m.QuotationID);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQuotationMaster;
        }
        public IEnumerable<vmQuotation> GetQuotationMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmQuotation> _vmQuotation = null;
            IEnumerable<vmQuotation> lstQuotationPaging = null;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstQuotationPaging = (from qcm in _ctxCmn.PurchaseQuotationMasters.Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany)
                                            join org in _ctxCmn.CmnUsers on qcm.PartyID equals org.UserID
                                            join curr in _ctxCmn.AccCurrencyInfoes on qcm.CurrencyID equals curr.Id
                                            select new vmQuotation
                                            {
                                               QuotationID = qcm.QuotationID,
                                               QuotationNo = qcm.QuotationNo,
                                               QuotationDate = qcm.QuotationDate,
                                               UserFullName = org.UserFullName,
                                               Currency = curr.CurrencyName

                                            }).ToList();
                    lstQuotationPaging = lstQuotationPaging.OrderByDescending(x => x.QuotationID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstQuotationPaging.Count();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstQuotationPaging;
        }


        public string GetDataBySuppplierID(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            IEnumerable<vmQuotation> _vmQuotation = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    _vmQuotation = (from qcm in _ctxCmn.PurchaseQuotationMasters.Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany && s.PartyID == objcmnParam.id && s.RequisitionID == objcmnParam.tTypeId)
                                         
                                          select new vmQuotation
                                          {                                            
                                              UserID = qcm.PartyID

                                          }).ToList();
                    if(_vmQuotation.Count()>0)
                    {
                        result = "Exist";
                    }
                }

               }
             
           
            catch (Exception)
            {
                result = "-1";
            }

            return result;
        }

        public string SaveQuotationMasterDetails(PurchaseQuotationMaster QuotationMaster, List<vmQuotation> QuotationDetails, int menuID)
        {
            //Declerations
            string result = string.Empty; string customCode = string.Empty; string CustomNo = string.Empty;
            int RequisitionID = Convert.ToInt32(QuotationMaster.QuotationID), SDetailRowNum = 0, UDetailRowNum = 0;
            long FirstDigit = 0, OtherDigits = 0, nextDetailId = 0; int NextId = 0;
            GenericFactory_EF_QuotationMaster = new PurchaseQuotationMaster_EF();
            GenericFactory_EF_QuotationDetail = new PurchaseQuotationDetail_EF();
            List<PurchaseQuotationDetail> objRqDetails = new List<PurchaseQuotationDetail>();
                
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //Transaction Occur here************************************************
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (QuotationMaster.QuotationID > 0)
                        {

                            //**************Master Model************************************************
                            var lstInvReqMaster = GenericFactory_EF_QuotationMaster.GetAll().FirstOrDefault(x => x.QuotationID == QuotationMaster.QuotationID);
                            lstInvReqMaster.UpdateBy = QuotationMaster.CreateBy;
                            lstInvReqMaster.UpdateOn = DateTime.Now;
                            lstInvReqMaster.UpdatePc = HostService.GetIP();
                            lstInvReqMaster.QuotationDate = QuotationMaster.QuotationDate;
                            lstInvReqMaster.DeliveryDate = QuotationMaster.DeliveryDate;
                            lstInvReqMaster.Remarks = QuotationMaster.Remarks;
                            lstInvReqMaster.QuotationTypeID = QuotationMaster.QuotationTypeID;
                            lstInvReqMaster.PartyID  = QuotationMaster.PartyID;
                            lstInvReqMaster.CurrencyID = QuotationMaster.CurrencyID;
                            lstInvReqMaster.RequisitionID = QuotationMaster.RequisitionID;
                           

                            //*************Details Model************************************************

                            foreach (vmQuotation ivrd in QuotationDetails)
                            {
                                PurchaseQuotationDetail objQuotationDetail = GenericFactory_EF_QuotationDetail.GetAll().FirstOrDefault(x => x.QuotationID == QuotationMaster.QuotationID && x.QuotationDetailID == ivrd.QuotationDetailID);
                                objQuotationDetail.QuotationID = QuotationMaster.QuotationID;
                                objQuotationDetail.ItemID = ivrd.ItemID;
                                objQuotationDetail.UnitID = (int)ivrd.UnitID;                            
                                objQuotationDetail.UnitPrice = ivrd.UnitPrice;
                                objQuotationDetail.Amount = ivrd.Amount;
                                objQuotationDetail.FOBValue = ivrd.FOBValue == null ? 0 : ivrd.FOBValue;
                                objQuotationDetail.FreightCharge = ivrd.FreightCharge == null ? 0 : ivrd.FreightCharge;
                                //objQuotationDetail.DischargeLocationID = ivrd.DischargeLocationID;
                                //objQuotationDetail.LoadingLocationID = ivrd.LoadingLocationID;
                               // objQuotationDetail.TransportTypeID = ivrd.TransportTypeID;
                                objQuotationDetail.UpdateBy = lstInvReqMaster.CreateBy;
                                objQuotationDetail.UpdateOn = DateTime.Now;
                                objQuotationDetail.UpdatePc = HostService.GetIP();
                                objRqDetails.Add(objQuotationDetail);
                            }
                            if (lstInvReqMaster != null)
                            {
                                GenericFactory_EF_QuotationMaster.Update(lstInvReqMaster);
                                GenericFactory_EF_QuotationMaster.Save();
                            }
                            // **************Details Transaction Update************************************************
                            if (objRqDetails.Count != 0)
                            {
                                GenericFactory_EF_QuotationDetail.UpdateList(objRqDetails);
                                GenericFactory_EF_QuotationDetail.Save();
                            }
                            transaction.Complete();
                            result = lstInvReqMaster.QuotationNo;
                        }
                        else
                        {
                            //Initialisation ************************************************
                            NextId = Convert.ToInt16(GenericFactory_EF_QuotationMaster.getMaxID("PurchaseQuotationMaster"));
                            nextDetailId = Convert.ToInt64(GenericFactory_EF_QuotationDetail.getMaxID("PurchaseQuotationDetail"));
                            FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));
                            CustomNo = GenericFactory_EF_QuotationMaster.getCustomCode(menuID, Convert.ToDateTime(QuotationMaster.QuotationDate), QuotationMaster.CompanyID, 1, 1);   // QuotationMaster.CompanyID

                            if ((customCode != "") || (customCode != null))
                                customCode = CustomNo;
                            else
                                customCode = NextId.ToString();
                            //**************Master Model************************************************
                            QuotationMaster.QuotationID = NextId;
                            QuotationMaster.CreateOn = DateTime.Now;
                            QuotationMaster.CreatePc = HostService.GetIP();
                            QuotationMaster.QuotationNo = customCode;

                            //*************Details Model************************************************
                            foreach (vmQuotation ivrd in QuotationDetails)
                            {
                                PurchaseQuotationDetail objDetail = new PurchaseQuotationDetail();
                                objDetail.QuotationDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                objDetail.QuotationID = NextId;
                                objDetail.ItemID = ivrd.ItemID;
                                objDetail.UnitID = (int)ivrd.UnitID;
                                objDetail.Qty = ivrd.Qty;
                                objDetail.UnitPrice = ivrd.UnitPrice;
                                objDetail.Amount = ivrd.Amount;
                                objDetail.FOBValue = ivrd.FOBValue;
                                //objDetail.TransportTypeID = ivrd.TransportTypeID;
                                objDetail.FreightCharge = ivrd.FreightCharge;
                                //objDetail.DischargeLocationID = ivrd.DischargeLocationID;
                                //objDetail.LoadingLocationID = ivrd.LoadingLocationID;
                                objDetail.CreateBy = QuotationMaster.CreateBy;
                                objDetail.CreateOn = DateTime.Now;
                                objDetail.IsDeleted = false;
                                objDetail.CreatePc = HostService.GetIP();
                                objRqDetails.Add(objDetail);
                                OtherDigits++;
                            }

                            //QuotationID
                            //**************Master Transaction Save************************************************                        
                            _ctxCmn.PurchaseQuotationMasters.Add(QuotationMaster);

                            //QuotationIDDetailID
                            //**************Details Transaction Save************************************************                           
                            _ctxCmn.PurchaseQuotationDetails.AddRange(objRqDetails);
                            _ctxCmn.SaveChanges();

                            //**************Reset Transaction************************************************
                            GenericFactory_EF_QuotationMaster.updateMaxID("PurchaseQuotationMaster", Convert.ToInt64(NextId));
                            GenericFactory_EF_QuotationMaster.updateCustomCode(menuID, DateTime.Now, 1, 1, 1);              // QuotationMaster.CompanyID
                            GenericFactory_EF_QuotationDetail.updateMaxID("PurchaseQuotationDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));

                            //**************Commit Transaction************************************************
                            transaction.Complete();
                            result = customCode;
                        }

                    }
                }
            }
            catch (Exception)
            {
                result = "-1";
            }

            return result;
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
                spQuery = "[dbo].[Get_PurchaseRequisitionDetail]";
                lstRequisiton = GFactory_GF_RequisitionDetail.ExecuteQuery(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstRequisiton;
        }
        public IEnumerable<InvRequisitionMaster> GetSPRNo(vmCmnParameters objcmnParam, out int recordsTotal)
        {

            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            IEnumerable<InvRequisitionMaster> objSPRNoWithoutPaging = null;

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objSPRNoWithoutPaging = (from spr in _ctxCmn.InvRequisitionMasters.Where(m => m.IsDeleted == false && m.CompanyID == objcmnParam.loggedCompany && m.RequisitionTypeID == 8) select spr).ToList().Select(m => new InvRequisitionMaster { RequisitionID = m.RequisitionID, RequisitionNo = m.RequisitionNo }).ToList();                    
                    objSPRNo = objSPRNoWithoutPaging.OrderByDescending(x => x.RequisitionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objSPRNoWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objSPRNo;
        }

        public IEnumerable<CmnCompany> GetCompany(vmCmnParameters objcmnParam)
        {
            
            IEnumerable<CmnCompany> objPICompany = null;
            IEnumerable<CmnUserWiseCompany> objCmnUserWCompany = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objCmnUserWCompany = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == objcmnParam.loggeduser && m.IsDeleted == false).ToList();
                    objPICompany = (from cmnUsrWsCom in objCmnUserWCompany
                                    join company in _ctxCmn.CmnCompanies on cmnUsrWsCom.CompanyID equals company.CompanyID
                                    select new { CompanyID = company.CompanyID, CompanyName = company.CompanyName }
                                    ).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objPICompany;
        }
        public IEnumerable<CmnOrganogram> GetDeptByCompanyID(vmCmnParameters objcmnParam, int companyID)
        { 
            IEnumerable<CmnOrganogram> lstOrganogram = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstOrganogram = (from org in _ctxCmn.CmnOrganograms.Where(m => m.IsDeleted == false && (m.CompanyID == companyID)) select org).ToList().Select(m => new CmnOrganogram { OrganogramID = m.OrganogramID, OrganogramName = m.OrganogramName }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstOrganogram;
        }
        public IEnumerable<vmChallan> GetItemDetailBySPRID(vmCmnParameters objcmnParam, Int64 SprID)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstItemDetailBySPRID = null;
           // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);

                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("RequisitionID", SprID);

                spQuery = "[Get_InvChallanItemBySPRID]";

                lstItemDetailBySPRID = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                // recordsTotal = lstMasterInfoByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailBySPRID;
        }

        public IEnumerable<vmChallan> GetChallanDetailByChallanID(vmCmnParameters objcmnParam, Int64 ChallanID, out int recordsTotal)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstItemDetailByChID = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);

                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("CHID", ChallanID);

                spQuery = "[Get_InvChallanItemByCHID]";

                lstItemDetailByChID = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                recordsTotal = lstItemDetailByChID.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByChID;
        }


        public IEnumerable<vmChallan> GetChallanMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstChallanMaster = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_InvChallanMaster]";

                lstChallanMaster = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanMaster;
        }


        public IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        { 
            IEnumerable<vmItemGroup> objSampleNo = null;
            IEnumerable<CmnItemMaster> cmnItemGroupID = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    cmnItemGroupID = (from grp in _ctxCmn.CmnItemMasters.Where(x => x.ItemTypeID == 1 && x.IsDeleted == false) select grp).ToList().GroupBy(x => x.ItemGroupID).Select(o => new CmnItemMaster { ItemGroupID = o.Key }).ToList();

                    objSampleNo = (from groupItm in _ctxCmn.CmnItemGroups.ToList()
                                   join groupId in cmnItemGroupID on groupItm.ItemGroupID equals groupId.ItemGroupID
                                   select new
                                   {
                                       ItemGroupID = groupId.ItemGroupID,
                                       ItemGroupName = groupItm.ItemGroupName
                                   }).Select(m => new vmItemGroup { ItemGroupID = m.ItemGroupID ?? 0, ItemGroupName = m.ItemGroupName }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objSampleNo;
        }

        public IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging)
        { 
            IEnumerable<CmnCombo> lstChallanTrnsTypes = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstChallanTrnsTypes = (from grp in _ctxCmn.CmnComboes.Where(x => x.ComboType == "ChallanTransType" && x.IsDeleted == false) select grp).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstChallanTrnsTypes;

        }
        public IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging)
        { 
            IEnumerable<AccCurrencyInfo> lstCurrency = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstCurrency = (from grp in _ctxCmn.AccCurrencyInfoes select grp).ToList().Select(m => new AccCurrencyInfo { Id = m.Id, CurrencyName = m.CurrencyName, ConversionRate = m.ConversionRate }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstCurrency;
        }

        public IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging)
        {
             
            List<vmCmnUser> lstParty = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    //**************************Paging Implemented******************************
                    using (_ctxCmn = new ERP_Entities())
                    {
                        lstParty = (from x in _ctxCmn.CmnUsers
                                    select new
                                    {
                                        UserID = x.UserID,
                                        UserFullName = x.UserFullName,
                                        UserTypeID = x.UserTypeID,
                                        IsDeleted = x.IsDeleted

                                    }).ToList().Select(m => new vmCmnUser
                                      {
                                          UserID = m.UserID,
                                          UserFullName = m.UserFullName,
                                          UserTypeID = m.UserTypeID,
                                          IsDeleted = m.IsDeleted
                                      })
                                   .Where(m => m.UserTypeID == 3 && m.IsDeleted == false && m.UserFullName != null)

                                   .OrderBy(p => p.UserID)
                                   .Skip(pageNumber)
                                   .Take(pageSize).ToList();
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstParty;
        }


        public IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal)
        {
             
            int itemGroupId = Convert.ToInt32(groupId);
            IEnumerable<vmChallan> objItemMaster = null;
            IEnumerable<vmChallan> objItemMasterWithoutPaging = null; 

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objItemMasterWithoutPaging = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_SampleNo.GetAll()

                                                  join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID  //GFactory_EF_CmnItemColor.GetAll()
                                                  join unit in _ctxCmn.CmnUOMs on item.UOMID equals unit.UOMID
                                                  //join warp in GenericFactory_EF_RndYarnCR.GetAll() on item.WarpYarnID equals warp.YarnID
                                                  //into warpYarn
                                                  //from yWarpYarn in warpYarn..DefaultIfEmpty()
                                                  //join weft in GenericFactory_EF_RndYarnCR.GetAll() on item.WeftYarnID equals weft.YarnID
                                                  //into weftYarn
                                                  //from xWeftYarn in weftYarn.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      ItemID = item.ItemID,
                                                      ItemName = item.ItemName,
                                                      UOMName = unit.UOMName,
                                                      UnitID = unit.UOMID,
                                                      ItemSizeID = item.ItemSizeID,
                                                      UniqueCode = item.UniqueCode,
                                                      ArticleNo = item.ArticleNo,
                                                      CuttableWidth = item.CuttableWidth,
                                                      WeightPerUnit = item.WeightPerUnit,
                                                      Description = item.Description,
                                                      CompanyID = item.CompanyID,
                                                      Weave = item.Weave,
                                                      ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
                                                      ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
                                                      Construction = item.Description,//"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + item.EPI.ToString() + "x" + item.PPI.ToString(),//item.Weave,
                                                      Width = 0.00M,
                                                      ItemTypeID = item.ItemTypeID,
                                                      ItemGroupID = item.ItemGroupID,
                                                      IsDeleted = item.IsDeleted,
                                                      Qty = 0.00m,
                                                      ExistQty = 0.00m,
                                                      PackingUnitID = 1,
                                                      PackingQty = 0.00m,
                                                      GrossWeight = 0.00m,
                                                      NetWeight = 0.00m,
                                                      WeightUnitID = 1

                                                  })
                                        .Select(x => new vmChallan
                                        {
                                            ItemID = x.ItemID,
                                            ItemName = x.ItemName,
                                            UOMName = x.UOMName,
                                            ItemSizeID = x.ItemSizeID,
                                            UniqueCode = x.UniqueCode,
                                            ArticleNo = x.ArticleNo,
                                            CuttableWidth = x.CuttableWidth,
                                            WeightPerUnit = x.WeightPerUnit,
                                            Description = x.Description,
                                            CompanyID = x.CompanyID,
                                            Weave = x.Weave,
                                            ItemColorID = x.ItemColorID,
                                            ColorName = x.ColorName,
                                            Construction = x.Construction,
                                            // Construction = GetConstruction(x.ItemID),
                                            Width = x.Width,
                                            ItemTypeID = x.ItemTypeID,
                                            ItemGroupID = x.ItemGroupID,
                                            IsDeleted = x.IsDeleted,
                                            Qty = x.Qty,
                                            ExistQty = x.ExistQty,
                                            PackingUnitID = x.PackingUnitID,
                                            PackingQty = x.PackingQty,
                                            GrossWeight = x.GrossWeight,
                                            NetWeight = x.NetWeight,
                                            WeightUnitID = x.WeightUnitID,
                                            UnitID = x.UnitID
                                        })
                                        .Where(m => m.ItemGroupID == itemGroupId && m.ItemTypeID == 1 && m.IsDeleted == false).ToList();

                    objItemMaster = objItemMasterWithoutPaging.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objItemMasterWithoutPaging.Count();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objItemMaster;
        }

        public string SaveUpdateChallanMasterNdetails(InvRChallanMaster chMaster, List<InvRChallanDetail> chDetails, int menuID) 
        {
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            _ctxCmn = new ERP_Entities();
            string result = "";
            if (chMaster.CHID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 chID = chMaster.CHID;
                        IEnumerable<InvRChallanMaster> lstInvRChallanMaster = (from qcm in _ctxCmn.InvRChallanMasters.Where(m => m.CHID == chID) select qcm).ToList();
                        InvRChallanMaster objInvRChallanMaster = new InvRChallanMaster();
                        foreach (InvRChallanMaster qcms in lstInvRChallanMaster)
                        {
                            qcms.UpdateBy = chMaster.CreateBy;
                            qcms.UpdateOn = DateTime.Now;
                            qcms.UpdatePc =  HostService.GetIP();
                            qcms.CHID = chMaster.CHID;
                            qcms.CurrencyID = chMaster.CurrencyID;
                            qcms.CompanyID = chMaster.CompanyID;
                            qcms.CHDate = chMaster.CHDate;
                            qcms.CHTypeID = chMaster.CHTypeID;
                            qcms.DepartmentID = chMaster.DepartmentID;
                            qcms.DischargePortID = chMaster.DischargePortID;
                            qcms.IsDeleted = false;
                            qcms.LoadingPortID = chMaster.LoadingPortID;
                            qcms.PartyID = chMaster.PartyID;
                            qcms.PIID = chMaster.PIID;
                            qcms.POID = chMaster.POID;
                            qcms.RefCHDate = chMaster.RefCHDate;
                            qcms.RefCHNo = chMaster.RefCHNo;
                            qcms.Remarks = chMaster.Remarks;
                            qcms.TransactionTypeID = chMaster.TransactionTypeID;

                            objInvRChallanMaster = qcms;
                        }
                        List<InvRChallanDetail> lstInvRChallanDetail = new List<InvRChallanDetail>();
                        foreach (InvRChallanDetail qcdt in chDetails)
                        {
                            InvRChallanDetail objInvRChallanDetail = (from qcdetl in _ctxCmn.InvRChallanDetails.Where(m => m.CHDetailID == qcdt.CHDetailID) select qcdetl).FirstOrDefault();
                            //start for exist passed n reject qty 
                            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                            //end for exist passed n reject qty 

                            objInvRChallanDetail.Amount = qcdt.Amount;
                            objInvRChallanDetail.BatchID = qcdt.BatchID;
                            objInvRChallanDetail.IsDeleted = false;
                            objInvRChallanDetail.GrossWeight = qcdt.GrossWeight;
                            objInvRChallanDetail.ItemID = qcdt.ItemID;
                            objInvRChallanDetail.LotID = qcdt.LotID;
                            objInvRChallanDetail.NetWeight = qcdt.NetWeight;
                            objInvRChallanDetail.PackingQty = qcdt.PackingQty;
                            objInvRChallanDetail.PackingUnitID = qcdt.PackingUnitID;
                            objInvRChallanDetail.Qty = qcdt.Qty;
                            objInvRChallanDetail.UnitID = qcdt.UnitID;
                            objInvRChallanDetail.UnitPrice = qcdt.UnitPrice;
                            objInvRChallanDetail.WeightUnitID = qcdt.WeightUnitID; 
                            objInvRChallanDetail.UpdateBy = chMaster.CreateBy;
                            objInvRChallanDetail.UpdateOn = DateTime.Now;
                            objInvRChallanDetail.UpdatePc =  HostService.GetIP();
                            lstInvRChallanDetail.Add(objInvRChallanDetail);

                           // InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                            // objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
                        }
                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = chMaster.CHNo.ToString();
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
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvRChallanMaster"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvRChallanDetail"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId.........//


                        //......... START for custom code........... //
                        string customCode = "";
                       
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, chMaster.CompanyID??1, 1, 1);

                        if (CustomNo != null)
                        {
                            customCode = CustomNo;
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }
                        //.........END for custom code............ //

                        string newChNo = customCode;
                        chMaster.CHID = NextId;
                        chMaster.CreateOn = DateTime.Now;
                        chMaster.CreatePc =  HostService.GetIP();
                        chMaster.CHNo = newChNo;
                        chMaster.IsDeleted = false;
                        // itemMaster.IsHDOCompleted = false;
                        //  itemMaster.IsLcCompleted = false;
                        List<InvRChallanDetail> lstchDetail = new List<InvRChallanDetail>();
                        foreach (InvRChallanDetail sdtl in chDetails)
                        {
                            InvRChallanDetail objchDetail = new InvRChallanDetail();
                            objchDetail.CHDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);//nextQCDetailId;
                            objchDetail.CHID = NextId;
                            objchDetail.ItemID = sdtl.ItemID;
                            objchDetail.BatchID = sdtl.BatchID;
                            objchDetail.GrossWeight = sdtl.GrossWeight;
                            objchDetail.LotID = sdtl.LotID;
                            objchDetail.NetWeight = sdtl.NetWeight;
                            objchDetail.PackingQty = sdtl.PackingQty;
                            objchDetail.PackingUnitID = sdtl.PackingUnitID;
                            objchDetail.UnitID = sdtl.UnitID;
                            objchDetail.UnitPrice = sdtl.UnitPrice;
                            objchDetail.WeightUnitID = sdtl.WeightUnitID; 
                            objchDetail.Qty = sdtl.Qty;
                            objchDetail.IsDeleted = false;
                            objchDetail.Amount = sdtl.Amount;
                            objchDetail.UnitID = sdtl.UnitID;
                            objchDetail.CreateBy = chMaster.CreateBy;
                            objchDetail.CreateOn = DateTime.Now;
                            objchDetail.CreatePc =  HostService.GetIP();
                            // objSalPIDetail.IsCICompleted = false;
                            lstchDetail.Add(objchDetail);
                            //nextQCDetailId++;
                            OtherDigits++;

                           // InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == sdtl.ItemID) select grrd).FirstOrDefault();
                           // objInvGrrDetail.QcRemainingQty = objInvGrrDetail.QcRemainingQty - (sdtl.PassQty + sdtl.RejectQty);

                        }

                        _ctxCmn.InvRChallanMasters.Add(chMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("InvRChallanMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, chMaster.CompanyID ?? 1, 1, 1);

                        _ctxCmn.InvRChallanDetails.AddRange(lstchDetail);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("InvRChallanDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        _ctxCmn.SaveChanges();

                        transaction.Complete();
                        result = newChNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                    }
                }

            }
            return result;
        } 
    }
}
