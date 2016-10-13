using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System.Collections;
using System.Transactions;
using ABS.Utility;



namespace ABS.Service.Inventory.Factories
{
    public class CostMgt : iCostMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo;
        private iGenericFactory<vmCost> GenericFactory_GF_vmCostAccessment;
        private iGenericFactory<PurchaseTaxCategory> GenericFactory_GF_PurchaseTaxCategory;
        private iGenericFactory<PurchaseTax> GenericFactory_GF_PurchaseTax;
        private iGenericFactory<PurchasePOMaster> GenericFactory_GF_PurchasePOMaster;
        private iGenericFactory<PurchaseConsumerChargeType> GenericFactory_GF_PurchaseConsumerChargeType;

        

        public IEnumerable<PurchaseTaxCategory> GetTaxCategory(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_PurchaseTaxCategory = new PurchaseTaxCategory_GF();

           

            string spQuery = "";
            IEnumerable<PurchaseTaxCategory> lstTaxCategory = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_PurchaseTaxCategory]";
                lstTaxCategory = GenericFactory_GF_PurchaseTaxCategory.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();   
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstTaxCategory;
        }


        public IEnumerable<PurchaseTax> GetTaxTypeByCategoryId(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_PurchaseTax = new PurchaseTax_GF();
            string spQuery = "";
            IEnumerable<PurchaseTax> lstTax = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("tTypeId", objcmnParam.tTypeId);
               
                spQuery = "[Get_PurchaseTaxByCategoryId]";
                lstTax = GenericFactory_GF_PurchaseTax.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();   
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstTax;
        }


        public IEnumerable<PurchasePOMaster> GetPurchaseOrderList(vmCmnParameters objcmnParam)
        {

            GenericFactory_GF_PurchasePOMaster = new PurchasePOMaster_GF();
            string spQuery = "";
            IEnumerable<PurchasePOMaster> lstPO = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("tTypeId", objcmnParam.tTypeId);

                spQuery = "[Get_POList]";
                lstPO = GenericFactory_GF_PurchasePOMaster.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();   
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstPO;
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


        public IEnumerable<CmnAddressCountry> GetLocation(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<CmnAddressCountry> objCountry = null;
            IEnumerable<CmnAddressCountry> objCountryWithoutPaging = null;

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objCountryWithoutPaging = (from spr in _ctxCmn.CmnAddressCountries.Where(m => m.IsDeleted == false) select spr).ToList().Select(m => new CmnAddressCountry { CountryID = m.CountryID, CountryName = m.CountryName }).ToList();
                    objCountry = objCountryWithoutPaging.OrderBy(x => x.CountryID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objCountryWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objCountry;
        }


        public IEnumerable<vmCost> GetCostInfoByPOID(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostInfo = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                //ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("tTypeId", objcmnParam.tTypeId);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                
                spQuery = "[Get_CostInfoByPOID]";
                lstCostInfo = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostInfo;
        }



        public IEnumerable<vmCost> GetCostAccessmentMaster(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostMaster = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_CostAccessmentMaster]";
                lstCostMaster = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostMaster;
        }


        public IEnumerable<vmCost> GetCostAccessmentDetailByCostAccessmentId(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostDetail = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("AccessmentCostID", objcmnParam.tTypeId);
                
                spQuery = "[Get_CostAccessmentDetail]";
                lstCostDetail = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostDetail;
        }


        
        public string SaveUpdateAccessmentCostMasterNdetails(PurchaseCostAccessmentMaster acCostMaster, List<PurchaseCostAccessmentDetail> acCostDetails, int menuID)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (acCostMaster.AccessmentCostID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 AccessmentCostID = acCostMaster.AccessmentCostID;
                        IEnumerable<PurchaseCostAccessmentMaster> lstInvRChallanMaster = (from qcm in _ctxCmn.PurchaseCostAccessmentMasters.Where(m => m.AccessmentCostID == AccessmentCostID) select qcm).ToList();

                        PurchaseCostAccessmentMaster objInvRChallanMaster = new PurchaseCostAccessmentMaster();
                        foreach (PurchaseCostAccessmentMaster qcms in lstInvRChallanMaster)
                        {
                            qcms.UpdateBy = acCostMaster.CreateBy;
                            qcms.UpdateOn = DateTime.Now;
                            qcms.UpdatePc =  HostService.GetIP();
                            qcms.AccessmentCostID = acCostMaster.AccessmentCostID;
                            qcms.CurrencyID = acCostMaster.CurrencyID;
                            qcms.CompanyID = acCostMaster.CompanyID;
                            qcms.AccessmentDate = acCostMaster.AccessmentDate;
                            qcms.DepartmentID = acCostMaster.DepartmentID;
                            qcms.AccessmentDescription = acCostMaster.AccessmentDescription;
                            qcms.IsDeleted = false;
                            qcms.AccessmentNo = acCostMaster.AccessmentNo;
                            qcms.AccessmentRefNo = acCostMaster.AccessmentRefNo;
                            qcms.AccessmentRefDate = acCostMaster.AccessmentRefDate;
                            qcms.BondAmount = acCostMaster.BondAmount;
                            
                            qcms.BondDue = acCostMaster.BondDue;
                            qcms.CustomOfficeID = acCostMaster.CustomOfficeID;
                            
                            qcms.CustomRefNo = acCostMaster.CustomRefNo;
                            qcms.DeclarantRefNo = acCostMaster.DeclarantRefNo;

                            qcms.SROAmount = acCostMaster.SROAmount;
                            qcms.SRODue = acCostMaster.SRODue;

                            qcms.POID = acCostMaster.POID;
                            qcms.RequisitionID = acCostMaster.RequisitionID;
                            qcms.DocUrl = acCostMaster.CustomRefNo;
                           
                            //qcms.SROAmount = acCostMaster.SROAmount;
                            //qcms.SRODue = acCostMaster.SRODue;
                            //qcms.POID = acCostMaster.POID;
                            //qcms.RequisitionID = acCostMaster.RequisitionID;
                            
                            objInvRChallanMaster = qcms;
                        }
                        List<PurchaseCostAccessmentDetail> lstInvRChallanDetail = new List<PurchaseCostAccessmentDetail>();
                        foreach (PurchaseCostAccessmentDetail qcdt in acCostDetails)
                        {
                            PurchaseCostAccessmentDetail objInvRChallanDetail = (from qcdetl in _ctxCmn.PurchaseCostAccessmentDetails.Where(m => m.AccessmentCostDetailID == qcdt.AccessmentCostDetailID) select qcdetl).FirstOrDefault();
                            
                            //start for exist passed n reject qty 
                            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                            //end for exist passed n reject qty 

                            objInvRChallanDetail.AccessmentCostDetailID = qcdt.AccessmentCostDetailID;
                            objInvRChallanDetail.AccessmentCostID = qcdt.AccessmentCostID;
                            //objInvRChallanDetail.CompanyID = qcdt.CompanyID;
                            objInvRChallanDetail.TaxID = qcdt.TaxID;
                            objInvRChallanDetail.TaxValue = qcdt.TaxValue;
                            objInvRChallanDetail.IsDeleted = false;
                            
                            objInvRChallanDetail.UpdateBy = acCostMaster.CreateBy;
                            objInvRChallanDetail.UpdateOn = DateTime.Now;
                            objInvRChallanDetail.UpdatePc =  HostService.GetIP();
                            lstInvRChallanDetail.Add(objInvRChallanDetail);

                            //InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                            //objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
                        }
                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = acCostMaster.AccessmentNo.ToString();
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
                        //...........START  new maxId...............//
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostAccessmentMaster"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostAccessmentDetail"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId....................//


                        //.........START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);

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
                        acCostMaster.AccessmentCostID = NextId;
                        acCostMaster.CreateOn = DateTime.Now;
                        acCostMaster.CreatePc =  HostService.GetIP();
                        acCostMaster.AccessmentNo = newChNo;
                        acCostMaster.IsDeleted = false;
                        

                        // itemMaster.IsHDOCompleted = false;
                        // itemMaster.IsLcCompleted = false;
                        

                        List<PurchaseCostAccessmentDetail> lstchDetail = new List<PurchaseCostAccessmentDetail>();
                        foreach (PurchaseCostAccessmentDetail sdtl in acCostDetails)
                        {
                            PurchaseCostAccessmentDetail objAccessDetail = new PurchaseCostAccessmentDetail();
                            objAccessDetail.AccessmentCostDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);    //nextQCDetailId;
                            objAccessDetail.AccessmentCostID = NextId;
                            objAccessDetail.TaxID = sdtl.TaxID;
                            objAccessDetail.TaxValue = sdtl.TaxValue;

                            //objAccessDetail. = sdtl.IsPercent;
                            
                            objAccessDetail.IsDeleted = false;
                            objAccessDetail.CreateBy = acCostMaster.CreateBy;
                            objAccessDetail.CreateOn = DateTime.Now;
                            objAccessDetail.CreatePc =  HostService.GetIP();
                            // objSalPIDetail.IsCICompleted = false;
                            lstchDetail.Add(objAccessDetail);
                            //nextQCDetailId++;
                            OtherDigits++;

                            // InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == sdtl.ItemID) select grrd).FirstOrDefault();
                            // objInvGrrDetail.QcRemainingQty = objInvGrrDetail.QcRemainingQty - (sdtl.PassQty + sdtl.RejectQty);

                        }

                        _ctxCmn.PurchaseCostAccessmentMasters.Add(acCostMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostAccessmentMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);
                        _ctxCmn.PurchaseCostAccessmentDetails.AddRange(lstchDetail);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostAccessmentDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
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
        

        // ----------------------------------------- Cost Clearing ----------------------------------------------

        public IEnumerable<PurchaseConsumerChargeType> GetConsumerChargeType(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_PurchaseConsumerChargeType = new PurchaseConsumerChargeType_GF();
            string spQuery = "";
            IEnumerable<PurchaseConsumerChargeType> lstTax = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("tTypeId", objcmnParam.tTypeId);

                spQuery = "[Get_PurchaseConsumerChargeType]";
                lstTax = GenericFactory_GF_PurchaseConsumerChargeType.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();   
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstTax;
        }


        public string SaveUpdateClearingCostMasterNdetails(PurchaseCostClearingMaster acCostMaster, List<PurchaseCostClearingDetail> acCostDetails, int menuID)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (acCostMaster.ClearingCostID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 ClearingCostID = acCostMaster.ClearingCostID;
                        IEnumerable<PurchaseCostClearingMaster> lstInvRChallanMaster = (from qcm in _ctxCmn.PurchaseCostClearingMasters.Where(m => m.ClearingCostID == ClearingCostID) select qcm).ToList();

                        PurchaseCostClearingMaster objInvRChallanMaster = new PurchaseCostClearingMaster();
                        foreach (PurchaseCostClearingMaster qcms in lstInvRChallanMaster)
                        {
                            qcms.UpdateBy = acCostMaster.CreateBy;
                            qcms.UpdateOn = DateTime.Now;
                            qcms.UpdatePc =  HostService.GetIP();
                            qcms.ClearingCostID = acCostMaster.ClearingCostID;
                            qcms.CurrencyID = acCostMaster.CurrencyID;

                            qcms.ClearingDate = acCostMaster.ClearingDate;

                            qcms.ClearingDescription = acCostMaster.ClearingDescription;
                            qcms.IsDeleted = false;
                       
                            qcms.POID = acCostMaster.POID;
                            qcms.RequisitionID = acCostMaster.RequisitionID;
                            qcms.DocUrl = acCostMaster.DocUrl;

                            qcms.RequisitionID = acCostMaster.RequisitionID;

                            objInvRChallanMaster = qcms;
                        }
                        List<PurchaseCostClearingDetail> lstInvRChallanDetail = new List<PurchaseCostClearingDetail>();
                        foreach (PurchaseCostClearingDetail qcdt in acCostDetails)
                        {
                            PurchaseCostClearingDetail objInvRChallanDetail = (from qcdetl in _ctxCmn.PurchaseCostClearingDetails.Where(m => m.ClearingCostDetailID == qcdt.ClearingCostDetailID) select qcdetl).FirstOrDefault();


                            objInvRChallanDetail.ClearingCostDetailID = qcdt.ClearingCostDetailID;
                            objInvRChallanDetail.ClearingCostID = qcdt.ClearingCostID;

                            objInvRChallanDetail.ConsumerChargeTypeID = qcdt.ConsumerChargeTypeID;
                            objInvRChallanDetail.Amount = qcdt.Amount;
                            objInvRChallanDetail.IsDeleted = false;

                            objInvRChallanDetail.UpdateBy = acCostMaster.CreateBy;
                            objInvRChallanDetail.UpdateOn = DateTime.Now;
                            objInvRChallanDetail.UpdatePc =  HostService.GetIP();
                            lstInvRChallanDetail.Add(objInvRChallanDetail);

                        }
                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = acCostMaster.ClearingNo.ToString();
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
                        //...........START  new maxId...............//
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostClearingMaster"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostClearingDetail"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId....................//


                        //.........START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);

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
                        acCostMaster.ClearingCostID = NextId;
                        acCostMaster.CreateOn = DateTime.Now;
                        acCostMaster.CreatePc =  HostService.GetIP();
                        acCostMaster.ClearingNo = newChNo;
                        acCostMaster.IsDeleted = false;
                        
                        List<PurchaseCostClearingDetail> lstchDetail = new List<PurchaseCostClearingDetail>();
                        foreach (PurchaseCostClearingDetail sdtl in acCostDetails)
                        {
                            PurchaseCostClearingDetail objAccessDetail = new PurchaseCostClearingDetail();
                            objAccessDetail.ClearingCostDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);    //nextQCDetailId;
                            objAccessDetail.ClearingCostID = NextId;
                            objAccessDetail.ConsumerChargeTypeID = sdtl.ConsumerChargeTypeID;
                            objAccessDetail.Amount = sdtl.Amount;

                            objAccessDetail.IsDeleted = false;
                            objAccessDetail.CreateBy = acCostMaster.CreateBy;
                            objAccessDetail.CreateOn = DateTime.Now;
                            objAccessDetail.CreatePc =  HostService.GetIP();

                            lstchDetail.Add(objAccessDetail);

                            OtherDigits++;
                        }

                        _ctxCmn.PurchaseCostClearingMasters.Add(acCostMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostClearingMasters", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);
                        _ctxCmn.PurchaseCostClearingDetails.AddRange(lstchDetail);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostClearingDetails", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
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


        public IEnumerable<vmCost> GetCostClearingMaster(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostMaster = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_PurchaseCostClearingMaster]";
                lstCostMaster = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostMaster;
        }

        
        public IEnumerable<vmCost> GetCostClearingDetailByCostClearingId(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostDetail = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("ClearingCostID", objcmnParam.tTypeId);

                spQuery = "[Get_PurchaseCostClearingDetail]";
                lstCostDetail = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostDetail;
        }


        // ----------------------------------------- Cost Transport ----------------------------------------------

        public IEnumerable<PurchaseConsumerChargeType> GetVehicles(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_PurchaseConsumerChargeType = new PurchaseConsumerChargeType_GF();
            string spQuery = "";
            IEnumerable<PurchaseConsumerChargeType> lstTax = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("tTypeId", objcmnParam.tTypeId);

                spQuery = "[Get_PurchaseConsumerChargeType]";
                lstTax = GenericFactory_GF_PurchaseConsumerChargeType.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();   
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstTax;
        }


        public string SaveUpdateTransportCostMasterNdetails(PurchaseCostTransportMaster acCostMaster, List<PurchaseCostTransportDetail> acCostDetails, int menuID)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (acCostMaster.TransportCostID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 TransportCostID = acCostMaster.TransportCostID;
                        IEnumerable<PurchaseCostTransportMaster> lstInvRChallanMaster = (from qcm in _ctxCmn.PurchaseCostTransportMasters.Where(m => m.TransportCostID == TransportCostID) select qcm).ToList();

                        PurchaseCostTransportMaster objInvRChallanMaster = new PurchaseCostTransportMaster();
                        foreach (PurchaseCostTransportMaster qcms in lstInvRChallanMaster)
                        {
                            qcms.UpdateBy = acCostMaster.CreateBy;
                            qcms.UpdateOn = DateTime.Now;
                            qcms.UpdatePc =  HostService.GetIP();
                            qcms.TransportCostID = acCostMaster.TransportCostID;
                            qcms.CurrencyID = acCostMaster.CurrencyID;

                            qcms.TransportDate = acCostMaster.TransportDate;
                            qcms.TransportDescription = acCostMaster.TransportDescription;
                            qcms.TransportNo = acCostMaster.TransportNo;
                            qcms.LoadingLocationID = acCostMaster.LoadingLocationID;
                            qcms.DischargeLocationID = acCostMaster.DischargeLocationID;
                            qcms.IsDeleted = false;

                            qcms.POID = acCostMaster.POID;
                            qcms.RequisitionID = acCostMaster.RequisitionID;
                            qcms.DocUrl = acCostMaster.DocUrl;

                            objInvRChallanMaster = qcms;
                        }

                        List<PurchaseCostTransportDetail> lstInvRChallanDetail = new List<PurchaseCostTransportDetail>();
                        foreach (PurchaseCostTransportDetail qcdt in acCostDetails)
                        {
                            PurchaseCostTransportDetail objInvRChallanDetail = (from qcdetl in _ctxCmn.PurchaseCostTransportDetails.Where(m => m.TransportCostID == qcdt.TransportCostID) select qcdetl).FirstOrDefault();
                            
                            objInvRChallanDetail.TransportCostDetailID = qcdt.TransportCostDetailID;

                            objInvRChallanDetail.TransportCostID = qcdt.TransportCostID;
                            objInvRChallanDetail.VehicleID = qcdt.VehicleID;
                            objInvRChallanDetail.NoOfVehicle = qcdt.NoOfVehicle;
                            
                            objInvRChallanDetail.PackingUnitID = qcdt.PackingUnitID;
                            objInvRChallanDetail.GoodsQtyPerVehicle = qcdt.GoodsQtyPerVehicle;
                            objInvRChallanDetail.FarePerVehicle = qcdt.FarePerVehicle;
                     
                            objInvRChallanDetail.IsDeleted = false;
                            objInvRChallanDetail.UpdateBy = acCostMaster.CreateBy;
                            objInvRChallanDetail.UpdateOn = DateTime.Now;
                            objInvRChallanDetail.UpdatePc =  HostService.GetIP();
                            lstInvRChallanDetail.Add(objInvRChallanDetail);

                        }
                        _ctxCmn.SaveChanges();
                        transaction.Complete();
                        result = acCostMaster.TransportNo.ToString();
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
                        //...........START  new maxId...............//
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostTransportDetails"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("PurchaseCostTransportDetails"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId....................//


                        //.........START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);

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
                        acCostMaster.TransportCostID = NextId;
                        acCostMaster.CreateOn = DateTime.Now;
                        acCostMaster.CreatePc =  HostService.GetIP();
                        acCostMaster.TransportNo = newChNo;
                        acCostMaster.IsDeleted = false;

                        List<PurchaseCostTransportDetail> lstchDetail = new List<PurchaseCostTransportDetail>();
                        foreach (PurchaseCostTransportDetail sdtl in acCostDetails)
                        {
                            PurchaseCostTransportDetail objAccessDetail = new PurchaseCostTransportDetail();
                            objAccessDetail.TransportCostDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);    //nextQCDetailId;
                            objAccessDetail.TransportCostID = NextId;
                            objAccessDetail.VehicleID = sdtl.VehicleID;
                            objAccessDetail.NoOfVehicle = sdtl.NoOfVehicle;

                            objAccessDetail.PackingUnitID = sdtl.PackingUnitID;
                            objAccessDetail.GoodsQtyPerVehicle = sdtl.GoodsQtyPerVehicle;
                            objAccessDetail.FarePerVehicle = sdtl.FarePerVehicle;
                            
                            objAccessDetail.IsDeleted = false;
                            objAccessDetail.CreateBy = acCostMaster.CreateBy;
                            objAccessDetail.CreateOn = DateTime.Now;
                            objAccessDetail.CreatePc =  HostService.GetIP();

                            lstchDetail.Add(objAccessDetail);

                            OtherDigits++;
                        }

                        _ctxCmn.PurchaseCostTransportMasters.Add(acCostMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostTransportMasters", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, acCostMaster.CompanyID ?? 1, 1, 1);
                        _ctxCmn.PurchaseCostTransportDetails.AddRange(lstchDetail);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("PurchaseCostTransportDetails", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
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


        public IEnumerable<vmCost> GetCostTransportMaster(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostMaster = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                spQuery = "[Get_PurchaseCostClearingMaster]";
                lstCostMaster = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //  recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostMaster;
        }


        public IEnumerable<vmCost> GetCostTransportDetailByCostTransportId(vmCmnParameters objcmnParam)
        {
            GenericFactory_GF_vmCostAccessment = new vmCost_GF();
            string spQuery = "";
            IEnumerable<vmCost> lstCostDetail = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("ClearingCostID", objcmnParam.tTypeId);

                spQuery = "[Get_PurchaseCostClearingDetail]";
                lstCostDetail = GenericFactory_GF_vmCostAccessment.ExecuteQuery(spQuery, ht);

                //recordsTotal = lstChallanMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCostDetail;
        }

















    }
}
