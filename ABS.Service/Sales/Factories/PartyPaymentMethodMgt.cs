using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.AllServiceClasses;
using ABS.Service.Sales.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Sales.Factories
{
    public class PartyPaymentMethodMgt  //:iPPMMgt
    {
        private iGenericFactory_EF<CmnCombo> GenericFactory_CmnCombo_EF = null;
        private iGenericFactory_EF<SalLCMaster> GenericFactoryFor_LCMaster_EF = null;
        private iGenericFactory_EF<CmnBank> GenericFactoryFor_Bank_EF = null;
        private iGenericFactory<SalPPBillingMaster> GenericFactoryFor_SalPPBillingMaster_GF = null;
        private iGenericFactory_EF<SalPPBillingMaster> GenericFactoryFor_SalPPBillingMaster_EF = null;
        private iGenericFactory_EF<SalPPBAMaster> GenericFactoryFor_SalPPBAMaster_EF = null;
        private iGenericFactory_EF<SalPPBADetail> GenericFactoryFor_SalPPBADetail_EF = null;
        private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode = null;

        /// No CompanyID Provided
        public IEnumerable<SalLCMaster> GetLCByBuyerId(int? id, int? ComId)
        {
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();

            IEnumerable<SalLCMaster> objLC = null;
            try
            {
                var LCMaster = GenericFactoryFor_LCMaster_EF.FindBy(x => x.BuyerID == id);

                objLC = (from LC in LCMaster
                         where LC.BuyerID == id && LC.IsHDOCompleted == true && LC.IsActive == true && LC.IsDeleted == false //&& LC.CompanyID == ComId
                         orderby LC.LCID descending
                         select new
                         {
                             LCID = LC.LCID,
                             LCNo = LC.LCNo
                         }).ToList().Select(x => new SalLCMaster
                         {
                             LCID = x.LCID,
                             LCNo = x.LCNo
                         }).Distinct().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLC;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnCombo> GetDocTypeOrPaymentMode(int? TabId, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_CmnCombo_EF = new CmnCombo_EF();

            string type = string.Empty;
            IEnumerable<CmnCombo> objDocType = null;
            try
            {
                var Combo = GenericFactory_CmnCombo_EF.GetAll();
                if (TabId == 1)
                    type = "doctype";
                else
                    type = "paymentmode";


                objDocType = (from C in Combo
                              where C.ComboType == type
                              orderby C.ComboID descending
                              select new CmnCombo
                              {
                                  ComboID = C.ComboID,
                                  ComboName = C.ComboName
                              }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDocType;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnCombo> GetBankCharge(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_CmnCombo_EF = new CmnCombo_EF();
            IEnumerable<CmnCombo> objCharge = null;
            try
            {
                var ComboCharge = GenericFactory_CmnCombo_EF.GetAll();
                objCharge = (from C in ComboCharge
                             where C.ComboType == "bankcharge"
                             orderby C.ComboID descending
                             select new CmnCombo
                             {
                                 ComboID = C.ComboID,
                                 ComboName = C.ComboName
                             }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCharge;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnBank> GetBankByLCID(int? id)
        {
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();
            GenericFactoryFor_Bank_EF = new CmnBank_EF();

            IEnumerable<CmnBank> objBank = null;
            try
            {
                var Bank = GenericFactoryFor_Bank_EF.GetAll();
                var LCs = GenericFactoryFor_LCMaster_EF.GetAll();
                objBank = (from B in Bank
                           join L in LCs on B.BankID equals L.LCOpenBank
                           where L.LCID == id
                           orderby B.BankID descending
                           select new CmnBank
                           {
                               BankID = B.BankID,
                               BankName = B.BankName
                           }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBank;
        }

        /// No CompanyID Provided
        public IEnumerable<SalPPBillingMaster> GetBillNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_SalPPBillingMaster_EF = new SalPPBillingMaster_EF();
            IEnumerable<SalPPBillingMaster> objBill_Doc = null;
            try
            {
                var Bill = GenericFactoryFor_SalPPBillingMaster_EF.GetAll();
                objBill_Doc = (from B in Bill
                               orderby B.BillMasterId descending
                               select new SalPPBillingMaster
                               {
                                   BillMasterId = B.BillMasterId,
                                   BillNo = B.BillNo
                               }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBill_Doc;
        }

        /// No CompanyID Provided
        public IEnumerable<SalPPBillingMaster> GetBillByBuyerID(int? id, int? TabId)
        {
            GenericFactoryFor_SalPPBillingMaster_EF = new SalPPBillingMaster_EF();
            IEnumerable<SalPPBillingMaster> objBill = null;
            try
            {
                var Bill = GenericFactoryFor_SalPPBillingMaster_EF.GetAll();
                if (TabId == 2)
                {
                    objBill = (from B in Bill
                               where B.BuyerId == id && B.IsPurchase == false
                               orderby B.BillMasterId descending
                               select new SalPPBillingMaster
                               {
                                   BillMasterId = B.BillMasterId,
                                   BillNo = B.BillNo
                               }).ToList();
                }
                else if (TabId == 3)
                {
                    objBill = (from B in Bill
                               where B.BuyerId == id && B.IsOD == false
                               orderby B.BillMasterId descending
                               select new SalPPBillingMaster
                               {
                                   BillMasterId = B.BillMasterId,
                                   BillNo = B.BillNo
                               }).ToList();
                }
                else if (TabId == 4)
                {
                    objBill = (from B in Bill
                               where B.BuyerId == id && B.IsAdjust == false
                               orderby B.BillMasterId descending
                               select new SalPPBillingMaster
                               {
                                   BillMasterId = B.BillMasterId,
                                   BillNo = B.BillNo
                               }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBill;
        }

        /// No CompanyID Provided
        public SalPPBillingMaster GetDocValByBillID(int? id)
        {
            GenericFactoryFor_SalPPBillingMaster_EF = new SalPPBillingMaster_EF();
            SalPPBillingMaster objDocVal = null;
            try
            {
                var Bill = GenericFactoryFor_SalPPBillingMaster_EF.GetAll();
                objDocVal = (from B in Bill
                             where B.BillMasterId == id
                             orderby B.BillMasterId descending
                             select new SalPPBillingMaster
                             {
                                 DeliveryValue = B.DeliveryValue,
                                 LIB = B.LIB,
                                 LIBDiscount = B.LIBDiscount,
                                 ConvertionPercentage = B.ConvertionPercentage
                             }).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDocVal;
        }

        public string SaveUpdatePartyPayment(vmSalPPBillingMaster model)
        {
            GenericFactoryFor_SalPPBillingMaster_GF = new SalPPBillingMaster_GF();
            string result = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                string spQuery = string.Empty;

                #region ***********************Starts Documents Info Entry***************************
                ht.Add("LUserID", model.LUserID);
                ht.Add("LCompanyID", model.LCompanyID);
                ht.Add("LMenuID", model.LMenuID);
                ht.Add("LTransactionTypeID", model.LTransactionTypeID);
                ht.Add("TabID", model.TabID);

                ht.Add("CompanyID", model.CompanyID);
                ht.Add("BuyerID", model.BuyerID);
                ht.Add("LCID", model.LCID);
                ht.Add("DeliveryQty", model.DeliveryQty);
                ht.Add("DocumentsNo", model.DocumentsNo != null ? model.DocumentsNo : "");

                if ((model.ShipmentDate == Convert.ToDateTime("1/1/1900 12:00:00 AM")) || (model.ShipmentDate == null))
                {
                    ht.Add("ShipmentDate", "");
                }
                else
                {
                    ht.Add("ShipmentDate", model.ShipmentDate);
                }

                if (model.DocsSentDateParty == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.DocsSentDateParty == null)
                {
                    ht.Add("DocsSentDateParty", "");
                }
                else
                {
                    ht.Add("DocsSentDateParty", model.DocsSentDateParty);
                }

                if (model.SubmissionDatePartyBank == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.SubmissionDatePartyBank == null)
                {
                    ht.Add("SubmissionDatePartyBank", "");
                }
                else
                {
                    ht.Add("SubmissionDatePartyBank", model.SubmissionDatePartyBank);
                }

                if (model.DocumentsDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.DocumentsDate == null)
                {
                    ht.Add("DocumentsDate", "");
                }
                else
                {
                    ht.Add("DocumentsDate", model.DocumentsDate);
                }

                if (model.DocsRecieveDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.DocsRecieveDate == null)
                {
                    ht.Add("DocsRecieveDate", "");
                }
                else
                {
                    ht.Add("DocsRecieveDate", model.DocsRecieveDate);
                }

                if (model.PartyAcceptanceDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.PartyAcceptanceDate == null)
                {
                    ht.Add("PartyAcceptanceDate", "");
                }
                else
                {
                    ht.Add("PartyAcceptanceDate", model.PartyAcceptanceDate);
                }

                if (model.BankAcceptanceDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.BankAcceptanceDate == null)
                {
                    ht.Add("BankAcceptanceDate", "");
                }
                else
                {
                    ht.Add("BankAcceptanceDate", model.BankAcceptanceDate);
                }

                ht.Add("BankID", model.BankID);
                ht.Add("ComboID", model.ComboID);
                ht.Add("RefNo", model.RefNo != null ? model.RefNo : "");
                ht.Add("DocumentValue", model.DocumentValue);
                ht.Add("RefBillNo", model.RefBillNo != null ? model.RefBillNo : "");
                #endregion***********************End Documents Info Entry*****************************

                #region ***********************Starts Purchase Info Entry*****************************
                ht.Add("BillMasterId", model.BillMasterId);
                ht.Add("LIB", model.LIB);
                ht.Add("ConversionRate", model.ConversionRate);
                ht.Add("SugarPAD", model.SugarPAD);
                ht.Add("ReserveMargin", model.ReserveMargin);

                if (model.PurchaseDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.PurchaseDate == null)
                {
                    ht.Add("PurchaseDate", "");
                }
                else
                {
                    ht.Add("PurchaseDate", model.PurchaseDate);
                }

                ht.Add("Discount", model.Discount);
                ht.Add("Percentage", model.Percentage);
                ht.Add("LIBRateOfInterest", model.LIBRateOfInterest);
                #endregion***********************End Purchase Info Entry******************************

                #region ***********************Starts Over Due Info Entry*****************************
                if (model.PaymentIssueDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.PaymentIssueDate == null)
                {
                    ht.Add("PaymentIssueDate", "");
                }
                else
                {
                    ht.Add("PaymentIssueDate", model.PaymentIssueDate);
                }

                if (model.AdjustmentDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.AdjustmentDate == null)
                {
                    ht.Add("AdjustmentDate", "");
                }
                else
                {
                    ht.Add("AdjustmentDate", model.AdjustmentDate);
                }

                if (model.MaturityDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.MaturityDate == null)
                {
                    ht.Add("MaturityDate", "");
                }
                else
                {
                    ht.Add("MaturityDate", model.MaturityDate);
                }

                if (model.PaymentRecievedDate == Convert.ToDateTime("1/1/1900 12:00:00 AM") || model.PaymentRecievedDate == null)
                {
                    ht.Add("PaymentRecievedDate", "");
                }
                else
                {
                    ht.Add("PaymentRecievedDate", model.PaymentRecievedDate);
                }

                ht.Add("TotalODDaysParty", model.TotalODDaysParty);
                ht.Add("TotalODInterestParty", model.TotalODInterestParty);
                ht.Add("ODAdjustment", model.ODAdjustment);
                ht.Add("PaymentValue", model.PaymentValue);
                ht.Add("Shortfall", model.Shortfall);
                ht.Add("TotalODDaysBank", model.TotalODDaysBank);
                ht.Add("TotalODInterestBank", model.TotalODInterestBank);
                #endregion***********************End Over Due Info Entry******************************

                spQuery = "[Set_PartyPayment]";

                result = GenericFactoryFor_SalPPBillingMaster_GF.ExecuteCommandString(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }

        public string SaveUpdateAdjustment(List<vmSalPPBAMasterDetail> model)
        {
            GenericFactoryFor_SalPPBillingMaster_EF = new SalPPBillingMaster_EF();
            GenericFactoryFor_SalPPBAMaster_EF = new SalPPBAMaster_EF();
            GenericFactoryFor_SalPPBADetail_EF = new SalPPBADetail_EF();

            string result = string.Empty;
            int BAMasterID = 0, BADetailID = 0, FirstDigit = 0, OtherDigits=0, CompanyId = 0, UserID = 0, MenuId = 0, TransactionTypeID = 0;
            string CustomNo = string.Empty, AdjustmentNo = string.Empty;

            SalPPBAMaster Masteritem = null;
            vmSalPPBAMasterDetail item = new vmSalPPBAMasterDetail();
            vmSalPPBAMasterDetail items = new vmSalPPBAMasterDetail();

            int s = model.Count() - 1;
            items = model[s];
            CompanyId = (int)items.CompanyID;
            UserID = (int)items.LUserID;
            MenuId = (int)items.LMenuID;
            TransactionTypeID = (int)items.LTransactionTypeID;

            var SalPPBADetail = new List<SalPPBADetail>();
            var BillingMaster = GenericFactoryFor_SalPPBillingMaster_EF.GetAll(); //To Update Remaining Quantity in SalHDODetail
            var UpdateBillingMaster = new List<SalPPBillingMaster>();
            //-------------------END----------------------

            if (model.Count() > 1)
            {
                try
                {
                    BAMasterID = Convert.ToInt16(GenericFactoryFor_SalPPBAMaster_EF.getMaxID("SalPPBAMaster"));
                    BADetailID = Convert.ToInt16(GenericFactoryFor_SalPPBADetail_EF.getMaxID("SalPPBADetail"));
                    FirstDigit = Convert.ToInt16(BADetailID.ToString().Substring(0, 1));
                    OtherDigits = Convert.ToInt16(BADetailID.ToString().Substring(1, BADetailID.ToString().Length - 1));


                    CustomNo = GenericFactoryFor_SalPPBAMaster_EF.getCustomCode(MenuId, DateTime.Now, CompanyId, 1, 1);
                    if (CustomNo == null || CustomNo == "")
                    {
                        AdjustmentNo = BAMasterID.ToString();
                    }
                    else
                    {
                        AdjustmentNo = CustomNo;
                    }

                    for (int i = 0; i < model.Count(); i++)
                    {
                        item = model[i];

                        if (i == ((model.Count) - 1))
                        {
                            //************-----Form SalPPBAMaster---------************
                            Masteritem = new SalPPBAMaster
                            {
                                BAID = BAMasterID,
                                BillID = item.BillMasterId,
                                AdjustmentNo = AdjustmentNo,
                                BranchID = 1,
                                BuyerID = (int)item.BuyerID,
                                ERQ = item.ERQ,
                                LIBAdjustmentAmount = item.LIBAdjustmentAmount,
                                PAD = item.PAD,
                                Remarks = item.Remarks,
                                RestRealizedAmount = item.RestRealizedAmount,
                                RestRealizedAmountPercent = item.RestRealizedAmtPercentage,
                               
                                CompanyID = CompanyId,
                                CreateBy = UserID,
                                CreateOn = DateTime.Now,
                                CreatePc =  HostService.GetIP(),
                                IsActive = true,
                                IsDeleted = false,
                                StatusBy = (int)item.LUserID,
                                StatusID = 1
                            };

                            foreach (SalPPBillingMaster u in BillingMaster.Where(u => u.BillMasterId == item.BillMasterId))
                            {
                                u.IsAdjust = true;
                                UpdateBillingMaster.Add(u);
                                break;
                            }
                        }
                        else
                        {
                            //************-----Form SalPPBADetail---------************
                            var Detailitem = new SalPPBADetail
                            {
                                BADetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                BAID = BAMasterID,
                                BankChargeID = (int)item.BankChargeID,
                                BranchID = 1,
                                ChargeAmount = item.ChargeAmount,

                                CompanyID = CompanyId,
                                CreateBy = UserID,
                                CreateOn = DateTime.Now,
                                CreatePc =  HostService.GetIP(),
                                IsActive = true,
                                IsDeleted = false,
                                StatusBy = UserID,
                                StatusID = 1,
                                StatusOn = DateTime.Now,
                            };

                            SalPPBADetail.Add(Detailitem);
                            OtherDigits++;
                        }
                    }

                    // Commit Transaction with TransactionScope
                    using (var transaction = new TransactionScope())
                    {
                        if (Masteritem != null)
                        {
                            GenericFactoryFor_SalPPBAMaster_EF.Insert(Masteritem);
                            GenericFactoryFor_SalPPBAMaster_EF.Save();
                            GenericFactoryFor_SalPPBAMaster_EF.updateMaxID("SalPPBAMaster", Convert.ToInt64(BAMasterID));
                            GenericFactoryFor_SalPPBAMaster_EF.updateCustomCode(MenuId, DateTime.Now, CompanyId, 1, 1);
                        }
                        if (SalPPBADetail != null)
                        {
                            GenericFactoryFor_SalPPBADetail_EF.InsertList(SalPPBADetail.ToList());
                            GenericFactoryFor_SalPPBADetail_EF.Save();
                            GenericFactoryFor_SalPPBADetail_EF.updateMaxID("SalPPBADetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        }
                        if (BillingMaster != null)
                        {
                            GenericFactoryFor_SalPPBillingMaster_EF.UpdateList(BillingMaster);
                            GenericFactoryFor_SalPPBillingMaster_EF.Save();
                        }

                        transaction.Complete();
                        result = AdjustmentNo;
                    }
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
