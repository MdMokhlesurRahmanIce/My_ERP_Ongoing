using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Sales.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;
using System.Web.Http;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility;

namespace ABS.Service.Sales.Factories
{
    public class LCMgt : iLCMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_Company = null;
        private iGenericFactory_EF<CmnUser> GenericFactory_EF_Buyer = null;
        private iGenericFactory_EF<CmnBank> GenericFactory_EF_Bank = null;
        private iGenericFactory_EF<CmnBankBranch> GenericFactory_EF_BankBranch = null;
        private iGenericFactory<vmSalLCDetail> GenericFactory_SalLCDetailForSP = null;
        private iGenericFactory_EF<SalLCMaster> GenericFactory_SalLCMaster = null;
        private iGenericFactory_EF<SalLCDetail> GenericFactory_SalLCDetail = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CommonCombo = null;
        private iGenericFactory_EF<SalPIMaster> GenericFactory_EF_SalPIMaster = null;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocument = null;
        private iGenericFactory_EF<CmnDocumentPath> GenericFactory_CmnDocumentPath = null;
        private iGenericFactory_EF<SalBookingMaster> GenericFactory_EF_SalBookingMaster;


        public IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_Company = new CmnCompany_EF();
            IEnumerable<CmnCompany> objCompanies = null;
            try
            {
                objCompanies = GenericFactory_EF_Company.GetAll().Select(m => new CmnCompany
                {
                    CompanyID = m.CompanyID,
                    CompanyName = m.CompanyName
                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCompanies;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnUser> GetBuyer(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_Buyer = new CmnUser_EF();
            IEnumerable<CmnUser> objBuyer = null;
            try
            {
                var Buyer = GenericFactory_EF_Buyer.GetAll();

                objBuyer = (from b in Buyer
                            where b.UserTypeID == 2 && b.UserFullName != null && b.UserFullName != "" && b.IsDeleted == false
                            orderby b.UserFullName ascending
                            select new
                            {
                                UserID = b.UserID,
                                UserFullName = b.UserFullName
                            }).ToList().Select(x => new CmnUser
                            {
                                UserID = x.UserID,
                                UserFullName = x.UserFullName
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBuyer;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_Bank = new CmnBank_EF();
            IEnumerable<CmnBank> objBank = null;
            try
            {
                objBank = GenericFactory_EF_Bank.GetAll().Select(m => new CmnBank { BankID = m.BankID, BankName = m.BankName }).ToList(); ;
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objBank;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_CommonCombo = new CmnCombo_EF();
            IEnumerable<CmnCombo> objPISight = null;
            try
            {
                objPISight = GenericFactory_EF_CommonCombo.GetAll().Select(m => new CmnCombo { CustomCode = m.CustomCode, ComboName = m.ComboName, ComboType = m.ComboType }).Where(m => m.ComboType == "sight").ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPISight;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnBankBranch> GetBankBranchById(int id)
        {
            GenericFactory_EF_BankBranch = new CmnBankBranch_EF();
            IEnumerable<CmnBankBranch> objBankBranch = null;
            try
            {
                objBankBranch = GenericFactory_EF_BankBranch.GetAll().Select(m => new CmnBankBranch { BranchID = m.BranchID, BranchName = m.BranchName, BankID = m.BankID }).Where(m => m.BankID == id).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objBankBranch;
        }

        /// CompanyID Provided but Not in Use
        public IEnumerable<vmSalLCDetail> GetLCMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSalLCDetail> objLCMaster = null;
            List<CmnUserWiseCompany> whichCompanies = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    whichCompanies = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == objcmnParam.loggeduser && m.IsDeleted == false).ToList().
                  Select(m => new CmnUserWiseCompany
                  {
                      CompanyID = m.CompanyID,
                      //CreatePc = m.CreatePc
                  }).ToList();

                    List<SalLCMaster> SalLCMaster = new List<SalLCMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalLCMaster> AddToList = new List<SalLCMaster>();
                        AddToList = (from hd in _ctxCmn.SalLCMasters select hd).Where(m => m.CompanyID == u.CompanyID && m.IsActive == true && m.IsDeleted == false).ToList();
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            SalLCMaster.AddRange(AddToList);
                        }
                    }

                    objLCMaster = (from slcm in SalLCMaster
                                   join buyer in _ctxCmn.CmnUsers on slcm.BuyerID equals buyer.UserID
                                   join ccombo in _ctxCmn.CmnComboes on slcm.Sight equals ccombo.ComboID
                                   select new
                                   {
                                       LCID = slcm.LCID,
                                       LCNo = slcm.LCNo,
                                       LCReferenceNo = slcm.LCReferenceNo,
                                       LCOpenBank = slcm.LCOpenBank,
                                       LCMasterBank = slcm.LCMasterBank,
                                       LCDate = slcm.LCDate,
                                       BuyerID = slcm.BuyerID,
                                       LCAmount = slcm.LCAmount,
                                       ShipmentDate = slcm.ShipmentDate,
                                       AmendmentDate = slcm.AmendmentDate,
                                       IsActive = slcm.IsActive,
                                       BuyerName = buyer.UserFullName,
                                       SightName = ccombo.ComboName,
                                       IsHDOCompleted = slcm.IsHDOCompleted

                                   }).Select(m => new vmSalLCDetail
                                   {
                                       LCID = m.LCID,
                                       LCNo = m.LCNo,
                                       LCReferenceNo = m.LCReferenceNo,
                                       LCOpenBank = m.LCOpenBank ?? 0,
                                       LCMasterBank = m.LCMasterBank ?? 0,
                                       LCDate = m.LCDate,
                                       BuyerID = m.BuyerID,
                                       LCAmount = m.LCAmount ?? 0.00m,
                                       ShipmentDate = m.ShipmentDate,
                                       AmendmentDate = m.AmendmentDate,
                                       IsActive = m.IsActive ?? false,
                                       BuyerName = m.BuyerName,
                                       SightName = m.SightName,
                                       IsHDOCompleted = m.IsHDOCompleted ?? false

                                   }).Where(a => a.IsActive == true)
                                     .OrderByDescending(s => s.LCID)
                                     .Skip(objcmnParam.pageNumber)
                                     .Take(objcmnParam.pageSize)
                                     .ToList();

                    recordsTotal = _ctxCmn.SalLCMasters.Where(m => m.IsActive == true && m.IsDeleted == false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLCMaster;
        }

        /// No CompanyID Provided
        public IEnumerable<vmSalLCDetail> GetPendingPI(int buyerId, int companyId)
        {
            GenericFactory_SalLCDetailForSP = new vmSalLCDetail_GF();
            IEnumerable<vmSalLCDetail> objPIInfo = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", companyId);
                ht.Add("BuyerID", buyerId);
                ht.Add("LoggedUser", 1);

                spQuery = "[Get_PendingPI]";
                objPIInfo = GenericFactory_SalLCDetailForSP.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPIInfo;
        }

        public string SaveUpdateLC(SalLCMaster LCInfo, List<SalLCDetail> LCDetailList, ArrayList fileNames, vmCmnParameters objcmnParam)
        {
            GenericFactory_SalLCMaster = new SalLCMaster_EF();
            GenericFactory_SalLCDetail = new SalLCDetail_EF();
            GenericFactory_CmnDocument = new CmnDocument_EF();
            GenericFactory_EF_SalPIMaster = new SalPIMaster_EF();
            GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();

            string result = ""; string newLCRefNo = "";
            long FirstDigit = 0, OtherDigits = 0, nextDetailId = 0, NextId = 0;

            #region Amendment Block
            if (LCInfo.LCID > 0)
            {

                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        NextId = Convert.ToInt64(GenericFactory_SalLCMaster.getMaxID("SalLCMaster"));
                        nextDetailId = Convert.ToInt64(GenericFactory_SalLCDetail.getMaxID("SalLCDetail"));
                        FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, Convert.ToInt32(nextDetailId.ToString().Length) - 1));

                        //******------------------start create LC Amendment No No------------------******
                        string[] amendmendNo = LCInfo.LCReferenceNo.ToString().Split(new string[] { "-AMD-" }, StringSplitOptions.None);  //itemMaster.PINO.ToString().Contains("Revise").ToString();

                        int amendNo = 0;
                        if (amendmendNo.Length > 1)
                        {
                            amendNo = Convert.ToInt16(amendmendNo[amendmendNo.Length - 1]) + 1;
                            newLCRefNo = amendmendNo[0] + "-AMD-" + amendNo.ToString();
                        }
                        else if (amendmendNo.Length == 1)
                        {
                            newLCRefNo = LCInfo.LCReferenceNo.ToString() + "-AMD-1".ToString();
                            amendNo = 1;
                        }
                        //******------------------end create  LC Amendment No------------------******


                        //******------------------Update LC master and Detail------------------******
                        Int64 lLCID = LCInfo.LCID;
                        IEnumerable<SalLCMaster> objSalLCMaster = GenericFactory_SalLCMaster.FindBy(m => m.LCID == lLCID).ToList();
                        SalLCMaster lstSalLCMaster = new SalLCMaster();
                        foreach (SalLCMaster lc in objSalLCMaster)
                        {
                            lc.IsActive = false;
                            lstSalLCMaster = lc;
                        }

                        GenericFactory_SalLCMaster.Update(lstSalLCMaster);
                        GenericFactory_SalLCMaster.Save();


                        IEnumerable<SalLCDetail> objSalLCDetail = GenericFactory_SalLCDetail.FindBy(m => m.LCID == lLCID).ToList();
                        SalLCDetail lstSalLCDetail = new SalLCDetail();
                        foreach (SalLCDetail lcd in objSalLCDetail)
                        {
                            lcd.IsActive = false;
                            lstSalLCDetail = lcd;
                        }

                        GenericFactory_SalLCDetail.Update(lstSalLCDetail);
                        GenericFactory_SalLCDetail.Save();
                        //******------------------End Update LC master and Detail ------------------******

                        //******------------------Start PI Table Update ------------------******
                        foreach (SalLCDetail sdtl in LCDetailList)
                        {
                            SalPIMaster objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == sdtl.PIID).FirstOrDefault();
                            objSalPIMaster.IsLcCompleted = true;
                            GenericFactory_EF_SalPIMaster.Update(objSalPIMaster);
                            GenericFactory_EF_SalPIMaster.Save();
                        }

                        //******------------------ End PI Table Update ------------------******
                        LCInfo.LCID = NextId;
                        LCInfo.LCReferenceNo = newLCRefNo;
                        LCInfo.AmendmentDate = LCInfo.LCDate;
                        LCInfo.AmendmentNo = amendNo.ToString();
                        LCInfo.CreateOn = DateTime.Now;
                        LCInfo.IsMasterLC = false;
                        LCInfo.IsActive = true;
                        LCInfo.IsHDOCompleted = false;

                        List<SalLCDetail> lstLCDetail = new List<SalLCDetail>();
                        foreach (SalLCDetail sdtl in LCDetailList)
                        {
                            SalLCDetail objLCDetail = new SalLCDetail();
                            objLCDetail.LCDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objLCDetail.LCID = NextId;
                            objLCDetail.PIID = sdtl.PIID;
                            objLCDetail.PINo = sdtl.PINo;
                            objLCDetail.CustomCode = LCInfo.LCReferenceNo;
                            objLCDetail.LCReferenceNo = LCInfo.LCReferenceNo;
                            objLCDetail.AmendmentNo = LCInfo.AmendmentNo;
                            objLCDetail.AmendmentDate = LCInfo.AmendmentDate;
                            objLCDetail.CompanyID = LCInfo.CompanyID;
                            objLCDetail.IsHDOCompleted = false;
                            objLCDetail.CreateBy = LCInfo.CreateBy;
                            objLCDetail.CreateOn = DateTime.Now;
                            objLCDetail.CreatePc = HostService.GetIP();
                            objLCDetail.IsActive = true;
                            objLCDetail.IsDeleted = false;
                            objLCDetail.DBID = 1;
                            objLCDetail.StatusBy = 1;
                            objLCDetail.StatusID = 1;
                            lstLCDetail.Add(objLCDetail);

                            OtherDigits++;
                        }

                        GenericFactory_SalLCMaster.Insert(LCInfo);
                        GenericFactory_SalLCMaster.Save();
                        GenericFactory_SalLCMaster.updateMaxID("SalLCMaster", Convert.ToInt64(NextId));

                        GenericFactory_SalLCDetail.InsertList(lstLCDetail);
                        GenericFactory_SalLCDetail.Save();
                        GenericFactory_SalLCDetail.updateMaxID("SalLCDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));

                        //**********----------------------Start File Upload----------------------**********
                        int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                        List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                        for (int i = 1; i <= fileNames.Count; i++)
                        {
                            CmnDocument objCmnDocument = new CmnDocument();
                            objCmnDocument.DocumentID = DocumentID;
                            objCmnDocument.DocumentPahtID = 1;
                            //objCmnDocument.DocumentName = fileNames[i].ToString();
                            string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            objCmnDocument.DocumentName = newLCRefNo + "_Doc_" + i + extension;
                            objCmnDocument.TransactionID = LCInfo.LCID;
                            objCmnDocument.TransactionTypeID = 1;
                            objCmnDocument.CompanyID = LCInfo.CompanyID;
                            objCmnDocument.CreateBy = LCInfo.CreateBy;
                            objCmnDocument.CreateOn = DateTime.Now;
                            objCmnDocument.CreatePc = HostService.GetIP();
                            objCmnDocument.IsDeleted = false;

                            objCmnDocument.IsDeleted = false;
                            lstCmnDocument.Add(objCmnDocument);

                            DocumentID++;
                        }

                        GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                        GenericFactory_CmnDocument.Save();
                        GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));

                        //**********----------------------File upload completed----------------------**********
                        transaction.Complete();
                        result = newLCRefNo;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }
            }
            #endregion Amendment Block

            #region New LC Block
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    //////////////////////////////////////////Primary Key and Custom Code Strat//////////////////////////////////

                    ////start new maxId
                    //int NextId = Convert.ToInt16(GenericFactory_SalLCMaster.getMaxID("SalLCMaster"));
                    //int nextDetailId = Convert.ToInt16(GenericFactory_SalLCDetail.getMaxID("SalLCDetail"));
                    ////end new maxId

                    //...........START  new maxId........//
                    NextId = Convert.ToInt64(GenericFactory_SalLCMaster.getMaxID("SalLCMaster"));
                    nextDetailId = Convert.ToInt64(GenericFactory_SalLCDetail.getMaxID("SalLCDetail"));
                    FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                    OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                    //..........END new maxId.........//

                    //// start for custom code 
                    //string customCode = "";
                    //CmnCustomCode objCustomCode = GFactory_EF_CmnCustomCode.FindBy(m => m.MenuID == 25 && m.CompanyID == LCInfo.CompanyID).FirstOrDefault();
                    //int customCodeID = Convert.ToInt16(objCustomCode.RecordID);
                    //if (customCodeID > 0)
                    //{
                    //    customCode = GenericFactory_SalLCMaster.getCustomCode(customCodeID, DateTime.Now, LCInfo.CompanyID, 1, 1);
                    //}
                    //else
                    //{
                    //    customCode = NextId.ToString();
                    //}
                    ////  end for custom code


                    //......... START for custom code........... //
                    string customCode = "";

                    string CustomNo = customCode = GenericFactory_SalLCMaster.getCustomCode(objcmnParam.menuId, LCInfo.LCDate ?? DateTime.Now, LCInfo.CompanyID, 1, 1);// // 1 for user id and 1 for db id --- work later
                    if (CustomNo != null)
                    {
                        customCode = CustomNo;
                    }
                    else
                    {
                        customCode = NextId.ToString();
                    }
                    //.........END for custom code............ //





                    //////////////////////////////////////////Primary Key and Custom Code End//////////////////////////////////

                    try
                    {
                        LCInfo.LCID = NextId;
                        LCInfo.CreateOn = DateTime.Now;
                        if (LCInfo.MasterLCNO != "")
                        {
                            LCInfo.IsMasterLC = true;
                        }
                        else
                        {
                            LCInfo.IsMasterLC = false;
                        }
                        LCInfo.IsActive = true;
                        LCInfo.LCReferenceNo = customCode;
                        LCInfo.AmendmentNo = "0";
                        LCInfo.IsHDOCompleted = false;

                        string ImagePath = string.Empty;

                        //if (string.IsNullOrEmpty(ImagePath) && !string.IsNullOrEmpty(LCInfo.ImageNames))
                        //{ 
                        //    //ImagePath=utils
                        //}


                        List<SalLCDetail> lstSalLCDetail = new List<SalLCDetail>();
                        foreach (SalLCDetail sdtl in LCDetailList)
                        {
                            SalLCDetail objSalLCDetail = new SalLCDetail();
                            //objSalLCDetail.LCDetailID = nextDetailId;
                            objSalLCDetail.LCDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objSalLCDetail.LCID = NextId;
                            objSalLCDetail.PIID = sdtl.PIID;
                            objSalLCDetail.PINo = sdtl.PINo;
                            objSalLCDetail.CustomCode = LCInfo.LCReferenceNo;
                            objSalLCDetail.LCReferenceNo = LCInfo.LCReferenceNo;
                            objSalLCDetail.IsHDOCompleted = false;
                            objSalLCDetail.CompanyID = LCInfo.CompanyID;
                            objSalLCDetail.CreateBy = LCInfo.CreateBy;
                            objSalLCDetail.CreateOn = DateTime.Now;
                            objSalLCDetail.CreatePc = HostService.GetIP();
                            objSalLCDetail.IsActive = true;
                            objSalLCDetail.IsDeleted = false;
                            objSalLCDetail.DBID = 1;
                            objSalLCDetail.StatusBy = 1;
                            objSalLCDetail.StatusID = 1;
                            lstSalLCDetail.Add(objSalLCDetail);

                            OtherDigits++;
                        }


                        //foreach (SalLCDetail sdtl in LCDetailList)
                        //{
                        //    SalPIMaster objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == sdtl.PIID).FirstOrDefault();
                        //    objSalPIMaster.IsLcCompleted = true;
                        //    GenericFactory_EF_SalPIMaster.Update(objSalPIMaster);
                        //    GenericFactory_EF_SalPIMaster.Save();
                        //}


                        /////////////////////// Start PI Table Update /////////////////////////////


                        foreach (SalLCDetail sdtl in LCDetailList)
                        {
                            SalPIMaster objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == sdtl.PIID).FirstOrDefault();
                            objSalPIMaster.IsLcCompleted = true;
                            GenericFactory_EF_SalPIMaster.Update(objSalPIMaster);
                            GenericFactory_EF_SalPIMaster.Save();
                        }

                        /////////////////////// End PI Table Update /////////////////////////////

                        /////////////////////// Start Booking Table Update /////////////////////////////

                        foreach (SalLCDetail sdtl in LCDetailList)
                        {
                            SalPIMaster objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == sdtl.PIID).FirstOrDefault();
                            SalBookingMaster objBookingMaster = GenericFactory_EF_SalBookingMaster.FindBy(m => m.BookingID ==
                                                                objSalPIMaster.BookingID).FirstOrDefault();

                            objBookingMaster.IsLCCompleted = true;
                            GenericFactory_EF_SalBookingMaster.Update(objBookingMaster);
                            GenericFactory_EF_SalBookingMaster.Save();
                        }

                        /////////////////////// End Booking Table Update /////////////////////////////


                        GenericFactory_SalLCMaster.Insert(LCInfo);
                        GenericFactory_SalLCMaster.Save();
                        GenericFactory_SalLCMaster.updateMaxID("SalLCMaster", Convert.ToInt64(NextId));

                        GenericFactory_SalLCMaster.updateCustomCode(25, DateTime.Now, LCInfo.CompanyID, 1, 1);

                        GenericFactory_SalLCDetail.InsertList(lstSalLCDetail);
                        GenericFactory_SalLCDetail.Save();
                        //GenericFactory_SalLCDetail.updateMaxID("SalLCDetail", Convert.ToInt64(nextDetailId - 1));
                        GenericFactory_SalLCDetail.updateMaxID("SalLCDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));


                        ////////////////////////////////////////Start File Upload//////////////////////////////

                        int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                        List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                        for (int i = 1; i <= fileNames.Count; i++)
                        {
                            CmnDocument objCmnDocument = new CmnDocument();
                            objCmnDocument.DocumentID = DocumentID;
                            objCmnDocument.DocumentPahtID = 1;
                            //objCmnDocument.DocumentName = fileNames[i].ToString();
                            string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            objCmnDocument.DocumentName = customCode + "_Doc_" + i + extension;
                            objCmnDocument.TransactionID = LCInfo.LCID;
                            objCmnDocument.TransactionTypeID = 1;
                            objCmnDocument.CompanyID = LCInfo.CompanyID;
                            objCmnDocument.CreateBy = LCInfo.CreateBy;
                            objCmnDocument.CreateOn = DateTime.Now;
                            objCmnDocument.CreatePc = HostService.GetIP();
                            objCmnDocument.IsDeleted = false;

                            objCmnDocument.IsDeleted = false;
                            lstCmnDocument.Add(objCmnDocument);

                            DocumentID++;
                        }

                        GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                        GenericFactory_CmnDocument.Save();
                        GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));
                        ///////////////////////////////////////////////File upload completed/////////////////////////////////////////////

                        transaction.Complete();
                        result = customCode;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }

            }
            #endregion New LC Block
            return result;
        }
        public IEnumerable<vmSalLCDetail> GetLCDetailByID(vmCmnParameters cmnParam, out int recordsTotal)
        {
            GenericFactory_SalLCDetailForSP = new vmSalLCDetail_GF();
            IEnumerable<vmSalLCDetail> objLC = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
            try
            {
                //object objLC_ = null;
                Hashtable ht = new Hashtable();
                ht.Add("LCID", cmnParam.id);

                spQuery = "[Get_LCDetailById]";
                objLC = GenericFactory_SalLCDetailForSP.ExecuteQuery(spQuery, ht).ToList();
                recordsTotal = objLC.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLC;
        }
        public IEnumerable<vmSalLCDetail> GetLCMasterById(int id)
        {
            GenericFactory_SalLCDetailForSP = new vmSalLCDetail_GF();
            IEnumerable<vmSalLCDetail> objLC = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("LCID", id);

                spQuery = "[Get_LCMasterById]";
                objLC = GenericFactory_SalLCDetailForSP.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLC;
        }
        public CmnDocumentPath GetUploadPath()
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
                    .Where(m => m.TransactionTypeID == 1).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUploadPath;
        }
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int lcID)
        {
            GenericFactory_CmnDocument = new CmnDocument_EF();
            IEnumerable<vmCmnDocument> objFileInfo = null;
            string fullFilePath = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //  var transactionName;
                    var virtualPath = _ctxCmn.CmnDocumentPaths.Where(m => m.TransactionTypeID == 1 && m.IsDeleted == false).ToList().
                                     Select(m => new CmnDocumentPath
                                     {
                                         VirtualPath = m.VirtualPath
                                     }).FirstOrDefault();

                    var transactionName = _ctxCmn.CmnTransactionTypes.Where(m => m.TransactionTypeID == 1 && m.IsDeleted == false).ToList().
                                     Select(m => new CmnTransactionType
                                     {
                                         TransactionTypeName = m.TransactionTypeName
                                     }).FirstOrDefault();


                    objFileInfo = _ctxCmn.CmnDocuments.Where(m => m.TransactionID == lcID && m.TransactionTypeID == 1).ToList().
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
    }
}
