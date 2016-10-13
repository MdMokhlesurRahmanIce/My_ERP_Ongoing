using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;

using ABS.Models.ViewModel.Inventory;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using ABS.Models.ViewModel.SystemCommon;
using System.Collections;
using System.Data.Common;
using ABS.Models.ViewModel.Sales;
using ABS.Utility;


namespace ABS.Service.Inventory.Factories
{

    public class OpeningStockMgt : iOpeningStockMgt  
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo;
        private iGenericFactory<vmChallan> GenericFactory_GF_vmChallan;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocument = null;
        private iGenericFactory_EF<CmnDocumentPath> GenericFactory_CmnDocumentPath = null;

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

        public IEnumerable<CmnUOM> GetPackingUnit(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<CmnUOM> lstPackingUnit = null;
            IEnumerable<CmnUOM> lstPackingUnitWithoutPaging = null;

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstPackingUnitWithoutPaging = (from pu in _ctxCmn.CmnUOMs.Where(m => m.IsDeleted == false && m.UOMGroupID == 5) select pu).ToList().Select(m => new CmnUOM { UOMID = m.UOMID, UOMName = m.UOMName }).ToList();
                    lstPackingUnit = lstPackingUnitWithoutPaging.OrderBy(x => x.UOMID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstPackingUnitWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstPackingUnit;
        }

        public IEnumerable<CmnUOM> GetWeightUnit(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<CmnUOM> lstWeightUnit = null;
            IEnumerable<CmnUOM> lstWeightUnitWithoutPaging = null;

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstWeightUnitWithoutPaging = (from wu in _ctxCmn.CmnUOMs.Where(m => m.IsDeleted == false && m.UOMGroupID == 4) select wu).ToList().Select(m => new CmnUOM { UOMID = m.UOMID, UOMName = m.UOMName }).ToList();
                    lstWeightUnit = lstWeightUnitWithoutPaging.OrderBy(x => x.UOMID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstWeightUnitWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstWeightUnit;
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

                    objSPRNoWithoutPaging = (from spr in _ctxCmn.InvRequisitionMasters.Where(m => m.IsDeleted == false) select spr).ToList().Select(m => new InvRequisitionMaster { RequisitionID = m.RequisitionID, RequisitionNo = m.RequisitionNo }).ToList();
                    objSPRNo = objSPRNoWithoutPaging.OrderBy(x => x.RequisitionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objSPRNoWithoutPaging.Count();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objSPRNo;
        }

        public IEnumerable<PurchasePOMaster> GetPONo(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<PurchasePOMaster> objPONo = null;
            IEnumerable<PurchasePOMaster> objPONoWithoutPaging = null; 

            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objPONoWithoutPaging = (from spr in _ctxCmn.PurchasePOMasters.Where(m => m.IsDeleted == false && m.CompanyID == objcmnParam.loggedCompany) select spr).ToList().Select(m => new PurchasePOMaster { POID = m.POID, PONo = m.PONo }).ToList();
                    objPONo = objPONoWithoutPaging.OrderByDescending(x => x.POID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objPONoWithoutPaging.Count();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPONo;
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

        public IEnumerable<vmChallan> GetItemDetailByPOID(vmCmnParameters objcmnParam, Int64 POID) 
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstItemDetailByPOID = null; 
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);

                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("POID", POID);

                spQuery = "[Get_InvDetailByPOID]";

                lstItemDetailByPOID = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                // recordsTotal = lstMasterInfoByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByPOID;
        }


        public IEnumerable<vmChallan> GetItmDetailByItmCode(vmCmnParameters objcmnParam, string ItemCode)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstItemDetailByItmCode = null;
            // recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);

                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("ArticleNo", ItemCode);

                spQuery = "[Get_InvChallanItemByItemCode]";

                lstItemDetailByItmCode = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                // recordsTotal = lstMasterInfoByGrrNo.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByItmCode;
        }


        public IEnumerable<vmChallan> GetGrrDetailByGrrID(vmCmnParameters objcmnParam, Int64 GrrID, out int recordsTotal)
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
                ht.Add("GrrID", GrrID);

                spQuery = "[Get_InvGrrItemByGrrID]";

                lstItemDetailByChID = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                recordsTotal = lstItemDetailByChID.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByChID;
        }


        public IEnumerable<vmChallan> GetMrrMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            string spQuery = "";
            IEnumerable<vmChallan> lstGrrMaster = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("IsSPR", objcmnParam.tTypeId);

                spQuery = "[Get_InvGrrMaster]";

                lstGrrMaster = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                recordsTotal = lstGrrMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstGrrMaster;
        }

        public IEnumerable<vmChallan> GetGrrList()
        { 
            IEnumerable<vmChallan> lstGrrMaster = null; 
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstGrrMaster = _ctxCmn.InvGrrMasters.Where(m => m.IsDeleted == false).Select(m=> new vmChallan {GrrID=m.GrrID, GrrNo=m.GrrNo, ManualGRRNoRpt = m.ManualGrrNo+"||"+m.GrrNo,  CompanyID=m.CompanyID, CreateBy=m.CreateBy }).ToList();
                }
              
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstGrrMaster;
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

            return lstParty;
        }


        public IEnumerable<vmChallan> GetItemMasterById(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_GF_vmChallan = new vmChallan_GF();
            //  int itemGroupId = Convert.ToInt32(groupId);

            string spQuery = "";
            IEnumerable<vmChallan> objItemMaster = null;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("LoggedUser", objcmnParam.loggeduser);
                ht.Add("PageNo", objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                // ht.Add("ItemGroupID", itemGroupId);

                spQuery = "[Get_Item]";

                objItemMaster = GenericFactory_GF_vmChallan.ExecuteQuery(spQuery, ht);

                recordsTotal = objItemMaster.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemMaster;

        }

        public string SaveUpdateChallanMasterNdetails(InvGrrMaster chMaster, List<InvGrrDetail> chDetails, int menuID, ArrayList fileNames)
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            if (chMaster.GrrID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Int64 grrID = chMaster.GrrID;
                        IEnumerable<InvGrrMaster> lstInvGrrMaster = (from qcm in _ctxCmn.InvGrrMasters.Where(m => m.GrrID == grrID && m.CompanyID == chMaster.CompanyID) select qcm).ToList();
                        InvGrrMaster objInvGrrMaster = new InvGrrMaster();
                        foreach (InvGrrMaster qcms in lstInvGrrMaster)
                        {
                            qcms.UpdateBy = chMaster.CreateBy;
                            qcms.UpdateOn = DateTime.Now;
                            qcms.UpdatePc =  HostService.GetIP();
                            qcms.CHID = chMaster.CHID;
                            qcms.CurrencyID = chMaster.CurrencyID;
                            //  qcms.CompanyID = chMaster.CompanyID;
                            qcms.GrrDate = chMaster.GrrDate;
                            // qcms.CHTypeID = chMaster.CHTypeID;
                            qcms.DepartmentID = chMaster.DepartmentID;
                            qcms.TransactionTypeID = chMaster.TransactionTypeID;
                            //  qcms.DischargePortID = chMaster.DischargePortID;
                            qcms.IsDeleted = false;
                            // qcms.LoadingPortID = chMaster.LoadingPortID;
                            qcms.SupplierID = chMaster.SupplierID;
                            qcms.PIID = chMaster.PIID;
                            qcms.POID = chMaster.POID;
                            qcms.RefCHDate = chMaster.RefCHDate;
                            qcms.RefCHNo = chMaster.RefCHNo;
                            qcms.Remarks = chMaster.Remarks;
                            qcms.Description = chMaster.Description;
                            //qcms.TransactionTypeID = chMaster.TransactionTypeID;
                            // qcms.TypeID = chMaster.TypeID;

                            objInvGrrMaster = qcms;
                        }
                        List<InvGrrDetail> lstInvGrrDetail = new List<InvGrrDetail>();
                        foreach (InvGrrDetail qcdt in chDetails)
                        {
                            InvGrrDetail objInvGrrDetail = (from qcdetl in _ctxCmn.InvGrrDetails.Where(m => m.GrrDetailID == qcdt.GrrDetailID) select qcdetl).FirstOrDefault();
                            //start for exist passed n reject qty 
                            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                            //end for exist passed n reject qty 

                            objInvGrrDetail.AdditionalQty = qcdt.AdditionalQty;
                            objInvGrrDetail.DisAmount = qcdt.DisAmount;
                            objInvGrrDetail.IsPercent = qcdt.IsPercent;
                            objInvGrrDetail.TotalAmount = qcdt.TotalAmount;
                            objInvGrrDetail.Amount = qcdt.Amount;
                            objInvGrrDetail.BatchID = qcdt.BatchID;
                            objInvGrrDetail.IsDeleted = false;
                            objInvGrrDetail.GrossWeight = qcdt.GrossWeight;
                            objInvGrrDetail.ItemID = qcdt.ItemID;
                            objInvGrrDetail.LotID = qcdt.LotID;
                            objInvGrrDetail.NetWeight = qcdt.NetWeight;
                            objInvGrrDetail.PackingQty = qcdt.PackingQty;
                            objInvGrrDetail.PackingUnitID = qcdt.PackingUnitID;
                            objInvGrrDetail.Qty = qcdt.Qty;

                            objInvGrrDetail.QcRemainingQty = qcdt.Qty;
                            objInvGrrDetail.IsQcCompleted = false;

                            objInvGrrDetail.UnitID = qcdt.UnitID;
                            objInvGrrDetail.UnitPrice = qcdt.UnitPrice;
                            objInvGrrDetail.WeightUnitID = qcdt.WeightUnitID;
                            objInvGrrDetail.UpdateBy = chMaster.CreateBy;
                            objInvGrrDetail.UpdateOn = DateTime.Now;
                            objInvGrrDetail.UpdatePc =  HostService.GetIP();
                            lstInvGrrDetail.Add(objInvGrrDetail);

                            //InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                            //objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
                        }
                        _ctxCmn.SaveChanges();

                        //**********----------------------Start File Upload----------------------**********
                        GenericFactory_CmnDocument = new CmnDocument_EF();
                        int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                        List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                        for (int i = 1; i <= fileNames.Count; i++)
                        {
                            CmnDocument objCmnDocument = new CmnDocument();
                            objCmnDocument.DocumentID = DocumentID;
                            objCmnDocument.DocumentPahtID = 2;
                            //objCmnDocument.DocumentName = fileNames[i].ToString();
                            string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            objCmnDocument.DocumentName = chMaster.GrrNo + "_Doc_" + i + extension;
                            objCmnDocument.TransactionID = chMaster.GrrID;
                            objCmnDocument.TransactionTypeID = chMaster.TransactionTypeID;
                            objCmnDocument.CompanyID = chMaster.CompanyID;
                            objCmnDocument.CreateBy = Convert.ToInt16(chMaster.CreateBy);
                            objCmnDocument.CreateOn = DateTime.Now;
                            objCmnDocument.CreatePc =  HostService.GetIP();
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
                        result = chMaster.GrrNo.ToString();
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
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvGrrMaster"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvGrrDetail"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId....................//


                        //......... START for custom code........... //

                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, chMaster.GrrDate, chMaster.CompanyID, 1, 1);

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
                        chMaster.GrrID = NextId;
                        chMaster.CreateOn = DateTime.Now;
                        chMaster.CreatePc =  HostService.GetIP();
                        chMaster.GrrNo = newChNo;
                        chMaster.IsDeleted = false;


                        List<InvGrrDetail> lstchDetail = new List<InvGrrDetail>();
                        foreach (InvGrrDetail sdtl in chDetails)
                        {
                            InvGrrDetail objchDetail = new InvGrrDetail();
                            objchDetail.GrrDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);//nextQCDetailId;
                            objchDetail.GrrID = NextId;
                            objchDetail.AdditionalQty = sdtl.AdditionalQty;
                            objchDetail.DisAmount = sdtl.DisAmount;
                            objchDetail.IsPercent = sdtl.IsPercent;
                            objchDetail.TotalAmount = sdtl.TotalAmount;

                            objchDetail.QcRemainingQty = sdtl.Qty;
                            objchDetail.IsQcCompleted = false;


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
                            lstchDetail.Add(objchDetail);
                            OtherDigits++;

                            // InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == sdtl.ItemID) select grrd).FirstOrDefault();
                            // objInvGrrDetail.QcRemainingQty = objInvGrrDetail.QcRemainingQty - (sdtl.PassQty + sdtl.RejectQty);

                        }

                        _ctxCmn.InvGrrMasters.Add(chMaster);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("InvGrrMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, chMaster.GrrDate, chMaster.CompanyID, 1, 1);
                        _ctxCmn.InvGrrDetails.AddRange(lstchDetail);

                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("InvGrrDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        _ctxCmn.SaveChanges();

                        //**********----------------------Start File Upload----------------------**********
                        GenericFactory_CmnDocument = new CmnDocument_EF();
                        int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                        List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                        for (int i = 1; i <= fileNames.Count; i++)
                        {
                            CmnDocument objCmnDocument = new CmnDocument();
                            objCmnDocument.DocumentID = DocumentID;
                            objCmnDocument.DocumentPahtID = 2;
                            //objCmnDocument.DocumentName = fileNames[i].ToString();
                            string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            objCmnDocument.DocumentName = chMaster.GrrNo + "_Doc_" + i + extension;
                            objCmnDocument.TransactionID = chMaster.GrrID;
                            objCmnDocument.TransactionTypeID = chMaster.TransactionTypeID;
                            objCmnDocument.CompanyID = chMaster.CompanyID;
                            objCmnDocument.CreateBy = Convert.ToInt16(chMaster.CreateBy);
                            objCmnDocument.CreateOn = DateTime.Now;
                            objCmnDocument.CreatePc =  HostService.GetIP();
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
                            IEnumerable<InvMrrMaster> lstInvMrrMaster = (from qcm in _ctxCmn.InvMrrMasters.Where(m => m.MrrID == mrrID && m.IsDeleted == false) select qcm).ToList();
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

                                objInvMrrMaster = qcms;
                            }
                            List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                            foreach (InvMrrDetail qcdt in mrrDetails)
                            {
                                InvMrrDetail objInvMrrDetail = (from qcdetl in _ctxCmn.InvMrrDetails.Where(m => m.MrrDetailID == qcdt.MrrDetailID && m.IsDeleted == false) select qcdetl).FirstOrDefault();

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
                            List<InvMrrDetail> lstInvMrrDetail = new List<InvMrrDetail>();
                            foreach (InvMrrDetail sdtl in mrrDetails)
                            {
                                //, , , , , , , , , , IsQcCompleted, , , 
                                InvMrrDetail objInvMrrDetail = new InvMrrDetail();
                                objInvMrrDetail.MrrDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                                objInvMrrDetail.MrrID = NextId;
                                objInvMrrDetail.ItemID = sdtl.ItemID;
                                objInvMrrDetail.Amount = sdtl.Amount;
                                objInvMrrDetail.UnitPrice = sdtl.UnitPrice;
                                objInvMrrDetail.BatchID = sdtl.BatchID;
                                objInvMrrDetail.IsDeleted = false;
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
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int grrID, int TransTypeID)
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


                    objFileInfo = _ctxCmn.CmnDocuments.Where(m => m.TransactionID == grrID).ToList().
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











