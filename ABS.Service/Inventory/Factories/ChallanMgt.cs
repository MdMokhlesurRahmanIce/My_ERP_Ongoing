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
    public class ChallanMgt : iChallanMgt
    {
        private ERP_Entities _ctxCmn = null;  
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo;
      
        private iGenericFactory<vmChallan> GenericFactory_GF_vmChallan;
         
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

        public string SaveUpdateChallanMasterNdetails(InvRChallanMaster chMaster, List<InvRChallanDetail> chDetails, int menuID) 
        {
            _ctxCmn = new ERP_Entities();
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
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
                            qcms.TypeID = chMaster.TypeID;

                            objInvRChallanMaster = qcms;
                        }
                        List<InvRChallanDetail> lstInvRChallanDetail = new List<InvRChallanDetail>();
                        foreach (InvRChallanDetail qcdt in chDetails)
                        {
                            InvRChallanDetail objInvRChallanDetail = (from qcdetl in _ctxCmn.InvRChallanDetails.Where(m => m.CHDetailID == qcdt.CHDetailID) select qcdetl).FirstOrDefault();
                            //start for exist passed n reject qty 
                            // decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                            //end for exist passed n reject qty 

                            objInvRChallanDetail.AditionalQty = qcdt.AditionalQty;
                            objInvRChallanDetail.DisAmount = qcdt.DisAmount;
                            objInvRChallanDetail.IsPercent = qcdt.IsPercent;
                            objInvRChallanDetail.TotalAmount = qcdt.TotalAmount;
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

                            //InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                            //objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);
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
                        //...........START  new maxId...............//
                        long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvRChallanMaster"));
                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextChDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvRChallanDetail"));
                        FirstDigit = Convert.ToInt64(nextChDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextChDetailId.ToString().Substring(1, nextChDetailId.ToString().Length - 1));
                        //..........END new maxId....................//


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
                        // itemMaster.IsLcCompleted = false;


                        List<InvRChallanDetail> lstchDetail = new List<InvRChallanDetail>();
                        foreach (InvRChallanDetail sdtl in chDetails)
                        {
                            InvRChallanDetail objchDetail = new InvRChallanDetail();
                            objchDetail.CHDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);//nextQCDetailId;
                            objchDetail.CHID = NextId;
                            objchDetail.AditionalQty = sdtl.AditionalQty;
                            objchDetail.DisAmount = sdtl.DisAmount;
                            objchDetail.IsPercent = sdtl.IsPercent;
                            objchDetail.TotalAmount = sdtl.TotalAmount;

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
