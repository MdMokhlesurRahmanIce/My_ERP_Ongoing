using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.AllServiceClasses;
using ABS.Service.Sales.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Utility;
using ABS.Models.ViewModel.SystemCommon;
using System.Runtime.Caching;
using ABS.Service.MenuMgt;
using System.Transactions;


namespace ABS.Service.Sales.Factories
{
    public class HeadOfficeSalesDeliveryOrderMgt //: iDeliveryOrderMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company_EF = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_Buyer_EF = null;
        private iGenericFactory_EF<SalHDOMaster> GenericFactoryFor_HDOMaster_EF = null;
        private iGenericFactory_EF<SalHDODetail> GenericFactoryFor_HDODetails_EF = null;
        private iGenericFactory_EF<SalLCMaster> GenericFactoryFor_LCMaster_EF = null;
        private iGenericFactory_EF<SalLCDetail> GenericFactoryFor_LCDetails_EF = null;
        private iGenericFactory_EF<SalPIMaster> GenericFactoryFor_PIMaster_EF = null;
        private iGenericFactory_EF<SalPIDetail> GenericFactoryFor_PIDetails_EF = null;
        private iGenericFactory<SalHDOMaster> GenericFactoryFor_DoMaster = null;
        private iGenericFactory_EF<CmnCombo> GenericFactoryFor_CmnCombo_EF = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactoryFor_CmnItemMaster_EF = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactoryFor_CmnUserWiseCompany_EF = null;
        private iGenericFactory_EF<CmnWorkFlowTransaction> GenericFactoryFor_CmnWorkFlowTransaction_EF = null;
       // private iGenericFactory_EF<CmnEmailTracking> GenericFactoryFor_CmnEmailTracking = null;

        /// <summary>
        /// Get Data Using Entity
        /// <para>Use it when to retive data through Entity Method</para>
        /// </summary>        
        public IEnumerable<CmnCompany> GetCompany(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_Company_EF = new CmnCompany_EF();
            GenericFactoryFor_CmnUserWiseCompany_EF = new CmnUserWiseCompany_EF();

            IEnumerable<CmnCompany> objCompanies = null;
            try
            {
                var company = GenericFactoryFor_Company_EF.GetAll().ToList();
                var UserWiseCompany = GenericFactoryFor_CmnUserWiseCompany_EF.GetAll();
                objCompanies = (from c in company
                                join UP in UserWiseCompany on c.CompanyID equals UP.CompanyID
                                where UP.UserID == objcmnParam.loggeduser
                                orderby c.CompanyID descending
                                select new
                                {
                                    CompanyId = c.CompanyID,
                                    CompanyName = c.CompanyName
                                }).ToList().Select(x => new CmnCompany
                                {
                                    CompanyID = x.CompanyId,
                                    CompanyName = x.CompanyName
                                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCompanies;
        }

        /// <summary>
        /// Get Data Using Entity
        /// <para>Use it when to retive data through Entity Method</para>
        /// </summary>   
        public IEnumerable<vmSalHDOMaster> GetHDOMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSalHDOMaster> objDOMaster = null;
            List<CmnUserWiseCompany> whichCompanies = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {


                    ////////////// Getting DO status ///////////////////////
                    //var AppStaus = (from wM in _ctxCmn.CmnWorkFlowMasters
                    //                join WD in _ctxCmn.CmnWorkFlowDetails on wM.WorkFlowID equals WD.WorkFlowID
                    //                join CS in _ctxCmn.CmnStatus on WD.Sequence == 1 ? 1 : WD.Sequence - 1 equals CS.StatusID

                    //                join wfT in _ctxCmn.CmnWorkFlowTransactions on WD.EmployeeID equals wfT.UserID
                    //                where wM.MenuID == objcmnParam.menuId && wM.CompanyID == objcmnParam.loggedCompany
                    //                && wfT.MenuID == objcmnParam.menuId && wfT.CompanyID == objcmnParam.loggedCompany
                    //                && wfT.StatusID == 1
                    //                select new vmAppStatus
                    //                {
                    //                    StatusName = CS.StatusName,
                    //                    TransactionID = (int)wfT.TransactionID
                    //                }).ToList();


                    ////////////// End Getting DO status ///////////////////////


                    whichCompanies = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == objcmnParam.loggeduser && m.IsDeleted == false).ToList().
                       Select(m => new CmnUserWiseCompany
                       {
                           CompanyID = m.CompanyID,
                           //CreatePc = m.CreatePc
                       }).ToList();

                    List<SalHDOMaster> SalHDOMaster = new List<SalHDOMaster>();
                    List<SalHDODetail> SalHDODetail = new List<SalHDODetail>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalHDOMaster> AddToList = new List<SalHDOMaster>();
                        List<SalHDODetail> AddToListD = new List<SalHDODetail>();
                        AddToList = (from hd in _ctxCmn.SalHDOMasters select hd).Where(m => m.CompanyID == u.CompanyID
                                    && m.IsDeleted == false && m.IsActive == true).ToList();
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            SalHDOMaster.AddRange(AddToList);
                        }
                        AddToListD = (from hd in _ctxCmn.SalHDODetails select hd).Where(m => m.CompanyID == u.CompanyID
                                    && m.IsDeleted == false && m.IsActive == true).ToList();
                        if (AddToListD != null && AddToListD.Count > 0)
                        {
                            SalHDODetail.AddRange(AddToListD);
                        }
                    }

                    objDOMaster = (from m in SalHDOMaster
                                   join d in SalHDODetail on m.HDOID equals d.HDOID
                                   join buyer in _ctxCmn.CmnUsers on m.BuyerID equals buyer.UserID
                                   join c in _ctxCmn.CmnStatus on m.StatusID equals c.StatusID
                                   join decUser in _ctxCmn.CmnUsers on m.DeclinedBy equals decUser.UserID into alldata
                                   from decUser in alldata.DefaultIfEmpty()
                                   select new
                                   {
                                       BuyerID = m.BuyerID,
                                       BuyerName = buyer.UserFullName,
                                       DecUserName = decUser == null ? "" : decUser.UserFullName,
                                       LCID = m.LCID,
                                       HDOID = m.HDOID,
                                       HDONo = m.HDONo,
                                       HDODate = m.HDODate,
                                       AHDONo = m.AHDONo,
                                       AHDOQTY = m.AHDOQTY,
                                       B2BLCNo = m.B2BLCNo,
                                       B2BLCDate = m.B2BLCDate,
                                       Remarks = m.Remarks,
                                       StatusName = c.StatusName,
                                       IsAllApproved = m.IsAllApproved
                                   }).Distinct().ToList().Select(x => new vmSalHDOMaster
                                   {
                                       UserId = x.BuyerID,
                                       BuyerName = x.BuyerName,
                                       DecUserName = x.DecUserName,
                                       LCID = x.LCID,
                                       HDOID = x.HDOID,
                                       HDONo = x.HDONo,
                                       HDODate = x.HDODate,
                                       AdoNo = x.AHDONo,
                                       AdoQty = x.AHDOQTY,
                                       B2BLCNo = x.B2BLCNo,
                                       B2BLCDate = x.B2BLCDate,
                                       Remarks = x.Remarks,
                                       Status = x.StatusName,
                                       IsAllApproved = (bool)x.IsAllApproved
                                   }).ToList();

                    recordsTotal = objDOMaster.Count();
                    objDOMaster = objDOMaster.OrderByDescending(x => x.HDOID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize)
                                 .Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDOMaster;
        }

        //For Reporting-----
        public IEnumerable<SalHDOMaster> GetHDOMaster(int loggedUserId, int companyId)
        {
            GenericFactoryFor_DoMaster = new SalHDOMaster_GF();
            IEnumerable<SalHDOMaster> objDOMaster = null;

            try
            {

                Hashtable ht = new Hashtable();
                string spQuery = string.Empty;
                ht.Add("LUserID", loggedUserId);
                ht.Add("LCompanyID", companyId);
                spQuery = "[Get_SalHDONo]";
                objDOMaster = GenericFactoryFor_DoMaster.ExecuteQuery(spQuery, ht).ToList();


                //using (_ctxCmn = new ERP_Entities())
                //{
                //    var HDOMaster = GenericFactoryFor_HDOMaster_EF.GetAll();

                //    objDOMaster = (from m in HDOMaster
                //                   where m.IsDeleted == false && m.IsActive == true && m.IsAllApproved == true && m.CompanyID == companyId
                //                   orderby m.HDOID descending
                //                   select new
                //                   {
                //                       //BuyerID = m.BuyerID,
                //                       HDOID = m.HDOID,
                //                       HDONo = m.HDONo,
                //                       HDODate = m.HDODate,
                //                       AHDONo = m.AHDONo,
                //                       AHDOQTY = m.AHDOQTY,
                //                       B2BLCNo = m.B2BLCNo,
                //                       B2BLCDate = m.B2BLCDate,
                //                       Remarks = m.Remarks
                //                   }).ToList().Select(x => new SalHDOMaster
                //                   {
                //                       //BuyerID=x.BuyerID,
                //                       HDOID = x.HDOID,
                //                       HDONo = x.HDONo,
                //                       HDODate = x.HDODate,
                //                       AHDONo = x.AHDONo,
                //                       AHDOQTY = x.AHDOQTY,
                //                       B2BLCNo = x.B2BLCNo,
                //                       B2BLCDate = x.B2BLCDate,
                //                       Remarks = x.Remarks
                //                   }).ToList();
                //}
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDOMaster;
        }

        public IEnumerable<CmnItemMaster> GetProducts(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactoryFor_CmnItemMaster_EF = new CmnItemMaster_EF();
            IEnumerable<CmnItemMaster> objProduct = null;

            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objProduct = (from m in _ctxCmn.CmnItemMasters
                                  where m.IsDeleted == false
                                  orderby m.ItemName
                                  select new
                                  {
                                      ItemID = m.ItemID,
                                      ItemName = m.ItemName,
                                      Description = m.Description,
                                      CuttableWidth = m.CuttableWidth
                                  }).ToList().Select(x => new CmnItemMaster
                                  {
                                      ItemID = x.ItemID,
                                      ItemName = x.ItemName,
                                      Description = x.Description,
                                      CuttableWidth = x.CuttableWidth
                                  }).ToList();
                    recordsTotal = objProduct.Count();
                    objProduct = objProduct.OrderBy(x => x.ItemName).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProduct;
        }

        public IEnumerable<SalLCMaster> GetLC(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();
            IEnumerable<SalLCMaster> objLC = null;
            try
            {
                var LCMaster = GenericFactoryFor_LCMaster_EF.GetAll();

                objLC = (from LC in LCMaster
                         where LC.IsHDOCompleted == false && LC.IsDeleted == false && LC.IsActive == true
                         orderby LC.LCID descending
                         select new
                         {
                             LCID = LC.LCID,
                             LCNo = LC.LCNo
                         }).ToList().Select(x => new SalLCMaster
                         {
                             LCID = x.LCID,
                             LCNo = x.LCNo
                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLC;
        }

        public vmSalHDOMaster GetDOMasterById(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_Company_EF = new CmnCompany_EF();
            GenericFactoryFor_Buyer_EF = new CmnUser_EF();
            GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();
            GenericFactoryFor_LCDetails_EF = new SalLCDetail_EF();

            vmSalHDOMaster objDOMasterById = null;
            try
            {
                var HDOMaster = GenericFactoryFor_HDOMaster_EF.FindBy(h => h.HDOID == objcmnParam.id);
                var Buyer = GenericFactoryFor_Buyer_EF.GetAll();
                var Company = GenericFactoryFor_Company_EF.GetAll();
                var LCMaster = GenericFactoryFor_LCMaster_EF.GetAll();
                var LCDetail = GenericFactoryFor_LCDetails_EF.GetAll();

                objDOMasterById = (from m in HDOMaster
                                   join c in Company on m.CompanyID equals c.CompanyID
                                   join b in Buyer on m.BuyerID equals b.UserID
                                   join d in LCDetail on m.LCID equals d.LCID
                                   join LM in LCMaster on d.LCID equals LM.LCID
                                   where m.HDOID == objcmnParam.id
                                   orderby m.HDOID descending
                                   select new
                                   {
                                       CompanyID = m.CompanyID,
                                       CompanyName = c.CompanyName,
                                       HDOID = m.HDOID,
                                       HDONo = m.HDONo,
                                       HDODate = m.HDODate,
                                       UserId = b.UserID,
                                       UserName = b.UserFullName,
                                       LCID = m.LCID,
                                       LCNo = LM.LCNo,
                                       AdoNo = m.AHDONo,
                                       AdoQty = m.AHDOQTY,
                                       B2BLCNo = m.B2BLCNo,
                                       UpNo = m.UPNo,
                                       Beneficiary = m.Beneficiary,
                                       Remarks = m.Remarks,
                                       B2BLCDate = m.B2BLCDate
                                   })
                                    .Select(x => new vmSalHDOMaster
                                    {
                                        CompanyID = x.CompanyID,
                                        CompanyName = x.CompanyName,
                                        HDOID = x.HDOID,
                                        HDONo = x.HDONo,
                                        HDODate = x.HDODate,
                                        UserId = x.UserId,
                                        UserName = x.UserName,
                                        LCID = x.LCID,
                                        LCNo = x.LCNo,
                                        AdoNo = x.AdoNo,
                                        AdoQty = x.AdoQty,
                                        B2BLCNo = x.B2BLCNo,
                                        UpNo = x.UpNo,
                                        Beneficiary = x.Beneficiary,
                                        Remarks = x.Remarks,
                                        B2BLCDate = x.B2BLCDate
                                    }).Distinct().FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDOMasterById;
        }

        /// <summary>
        /// Get Data Using Entity
        /// <para>Use it when to retive data through Entity Method</para>
        /// </summary>   
        public IEnumerable<vmBuyerHDO> GetBuyer(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBuyerHDO> objBuyer = null;
            var cache = MemoryCache.Default; //To Cache Data

            int currentrecordsTotal = 0;
            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    currentrecordsTotal = _ctxCmn.CmnUsers.Where(x => x.UserTypeID == 2 && x.IsActive == true && x.IsDeleted == false).Count();

                    if (cache.Get("dataCache") != null)
                    {
                        objBuyer = (IEnumerable<vmBuyerHDO>)cache.Get("dataCache"); //Get Data From Cache
                        objBuyer = objBuyer.OrderBy(x => x.UserFullName);

                        recordsTotal = objBuyer.Count();
                    }
                    if (cache.Get("dataCache") == null || currentrecordsTotal != recordsTotal)
                    {
                        var cachePolicty = new CacheItemPolicy();                       //To Cache Data
                        cachePolicty.AbsoluteExpiration = DateTime.Now.AddDays(1);  //To Cache Data

                        objBuyer = (from b in _ctxCmn.CmnUsers
                                    where b.UserTypeID == 2 && b.IsActive == true && b.IsDeleted == false && b.UserFullName != null && b.UserFullName != ""
                                    orderby b.UserFullName
                                    select new
                                    {
                                        UserID = b.UserID,
                                        UserFullName = b.UserFullName
                                    }).ToList().Select(x => new vmBuyerHDO
                                    {
                                        UserID = x.UserID,
                                        UserFullName = x.UserFullName
                                    }).ToList();

                        cache.Add("dataCache", objBuyer.ToList(), cachePolicty);        //To Cache Data

                        recordsTotal = objBuyer.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBuyer;
        }

        /// <summary>
        /// Get Data Using Entity
        /// <para>Use it when to retive data through Entity Method</para>
        /// </summary>   
        public IEnumerable<SalLCMaster> GetLCByBuyerId(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();
            IEnumerable<SalLCMaster> objLCByBuyerID = null;
            try
            {
                var LCMaster = GenericFactoryFor_LCMaster_EF.FindBy(l => l.BuyerID == objcmnParam.id);

                objLCByBuyerID = (from LC in LCMaster
                                  where LC.BuyerID == objcmnParam.id && LC.CompanyID == objcmnParam.loggedCompany && LC.IsActive == true
                                  && LC.IsDeleted == false && LC.IsHDOCompleted == objcmnParam.IsTrue
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
            return objLCByBuyerID;
        }

        /// <summary>
        /// Get Data Using Entity
        /// <para>Use it when to retive data through Entity Method</para>
        /// </summary>   
        public IEnumerable<vmSalLCDetail> GetLCMasterById(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            bool IsTrues = true;
            recordsTotal = 0;
            IEnumerable<vmSalLCDetail> objLCMasterByID = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    var IsLCActive = (from Is in _ctxCmn.SalLCMasters where Is.LCID == objcmnParam.id select Is.IsActive).FirstOrDefault();
                    IsTrues = objcmnParam.IsTrue == true && IsLCActive == false ? false : true;

                    objLCMasterByID = (from LC in _ctxCmn.SalLCMasters
                                       join CCombo in _ctxCmn.CmnComboes on LC.Sight equals CCombo.ComboID
                                       where LC.LCID == objcmnParam.id && LC.IsHDOCompleted == objcmnParam.IsTrue && LC.CompanyID == objcmnParam.selectedCompany
                                       && LC.IsActive == IsTrues
                                       orderby LC.LCID descending
                                       select new
                                       {
                                           LCID = LC.LCID,
                                           LCNo = LC.LCNo,
                                           LCDate = LC.LCDate,
                                           LCAmount = LC.LCAmount,
                                           ShipmentDate = LC.ShipmentDate,
                                           ODInterest = LC.ODInterest,
                                           SightName = CCombo.ComboName
                                       }).ToList().Select(x => new vmSalLCDetail
                                       {
                                           LCID = x.LCID,
                                           LCNo = x.LCNo,
                                           LCDate = x.LCDate,
                                           LCAmount = x.LCAmount ?? 0,
                                           ShipmentDate = x.ShipmentDate,
                                           ODInterest = x.ODInterest ?? 0,
                                           SightName = x.SightName
                                       }).ToList();
                    recordsTotal = objLCMasterByID.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objLCMasterByID;
        }

        public IEnumerable<vmSalLCDetail> GetLCDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactoryFor_PIMaster_EF = new SalPIMaster_EF();
            GenericFactoryFor_PIDetails_EF = new SalPIDetail_EF();
            GenericFactoryFor_LCMaster_EF = new SalLCMaster_EF();
            GenericFactoryFor_LCDetails_EF = new SalLCDetail_EF();
            GenericFactoryFor_CmnCombo_EF = new CmnCombo_EF();

            IEnumerable<vmSalLCDetail> objLCDetailByID = null;
            recordsTotal = 0;
            try
            {
                var PIMaster = GenericFactoryFor_PIMaster_EF.GetAll();
                var PIDetail = GenericFactoryFor_PIDetails_EF.GetAll();
                var CmnCombos = GenericFactoryFor_CmnCombo_EF.GetAll();
                var LCMasterByID = GenericFactoryFor_LCMaster_EF.FindBy(lD => lD.LCID == objcmnParam.id);
                var LCDetailByID = GenericFactoryFor_LCDetails_EF.FindBy(lD => lD.LCID == objcmnParam.id);

                objLCDetailByID = (from LC in LCDetailByID
                                   join LCM in LCMasterByID on LC.LCID equals LCM.LCID
                                   join PIM in PIMaster on LC.PIID equals PIM.PIID
                                   join PID in PIDetail on LC.PIID equals PID.PIID
                                   join CC in CmnCombos on PIM.SightID equals CC.ComboID
                                   join CCS in CmnCombos on PIM.ShipmentID equals CCS.ComboID
                                   where LC.LCID == objcmnParam.id && PIM.IsHDOCompleted == objcmnParam.IsTrue && LC.IsActive == true
                                   && LC.IsDeleted == false && PIM.IsActive == true //&& LCM.IsHDOCompleted==false  //&& PIM.IsLcCompleted==true
                                   orderby LC.LCDetailID descending
                                   select new
                                   {
                                       LCDetailID = LC.LCDetailID,
                                       PIID = LC.PIID,
                                       PINo = PIM.PINO,
                                       PIDate = PIM.PIDate,
                                       SightName = CC.ComboName,
                                       ShipmentName = CCS.ComboName,
                                       TtlAmount = PID.Amount
                                   }).GroupBy(s => new { s.LCDetailID, s.PIID, s.PINo, s.PIDate, s.SightName, s.ShipmentName })
                          .Select(j => new vmSalLCDetail
                          {
                              LCDetailID = j.Key.LCDetailID,
                              PIID = (int)j.Key.PIID,
                              PINo = j.Key.PINo,
                              PIDate = j.Key.PIDate,
                              SightName = j.Key.SightName,
                              ShipmentName = j.Key.ShipmentName,
                              TtlAmount = j.Sum(s => s.TtlAmount),
                          }).ToList();

                recordsTotal = objLCDetailByID.Count();
                objLCDetailByID = objLCDetailByID.OrderByDescending(x => x.LCDetailID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLCDetailByID;
        }

        public IEnumerable<vmPIDetail> GetPIDetailsById(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmPIDetail> objPI = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    var PIDetailByID = _ctxCmn.SalPIDetails.Where(m => m.CompanyID == objcmnParam.selectedCompany && m.IsDeleted == false && m.IsActive == true).ToList();
                    var PIMaster = _ctxCmn.SalPIMasters.Where(m => m.CompanyID == objcmnParam.selectedCompany && m.IsDeleted == false && m.IsActive == true).ToList();
                    var ItemMaster = _ctxCmn.CmnItemMasters.Where(m => m.IsDeleted == false).ToList();
                    objPI = (from PD in PIDetailByID
                             join PM in PIMaster on PD.PIID equals PM.PIID
                             join CM in ItemMaster on PD.ItemID equals CM.ItemID
                             where objcmnParam.id == 0 ? true : PM.PIID == objcmnParam.id
                             // where  PM.PIID == objcmnParam.id
                             orderby PD.PIDetailID descending
                             select new
                             {
                                 PIDetailID = PD.PIDetailID,
                                 PIID = PD.PIID,
                                 ItemID = PD.ItemID,
                                 ItemName = CM.ItemName,
                                 PINO = PM.PINO,
                                 BuyerStyle = PD.BuyerStyle,
                                 Construction = CM.Note,
                                 Description = CM.Description,
                                 CuttableWidth = PD.CuttableWidth,
                                 Quantity = PD.Quantity,
                                 UnitPrice = PD.UnitPrice,
                                 Amount = PD.Amount
                             }).ToList().Select(x => new vmPIDetail
                             {
                                 PIDetailID = x.PIDetailID,
                                 PIID = x.PIID,
                                 ItemID = x.ItemID,
                                 ItemName = x.ItemName,
                                 PINO = x.PINO,
                                 BuyerStyle = x.BuyerStyle,
                                 Construction = x.Construction,
                                 Description = x.Description,
                                 CuttableWidth = x.CuttableWidth,
                                 Quantity = x.Quantity,
                                 UnitPrice = x.UnitPrice,
                                 Amount = x.Amount
                             }).ToList();

                    recordsTotal = objPI.Count();
                    objPI = objPI.OrderByDescending(x => x.PIDetailID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPI;
        }

        public IEnumerable<vmPIDetail> GetProductRevised(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmPIDetail> ObjRevisedHD = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    ObjRevisedHD = (from HD in _ctxCmn.SalHDODetails
                                    join CM in _ctxCmn.CmnItemMasters on HD.ProductID equals CM.ItemID
                                    where objcmnParam.id == 0 ? true : HD.HDOID == objcmnParam.id
                                    // where  PM.PIID == objcmnParam.id
                                    orderby HD.HDODetailID descending
                                    select new
                                    {
                                        HDODetailID = HD.HDODetailID,
                                        PIID = HD.PIID == null ? 0 : HD.PIID,
                                        ItemID = HD.ProductID,
                                        ItemName = CM.ItemName,
                                        Description = CM.Description,
                                        CuttableWidth = CM.CuttableWidth,
                                        Quantity = HD.Quantity,
                                        UnitPrice = HD.Price,
                                        Amount = HD.Amount
                                    }).ToList().Select(x => new vmPIDetail
                                    {
                                        HDODetailID = x.HDODetailID,
                                        PIID = (long)x.PIID,
                                        ItemID = x.ItemID,
                                        ItemName = x.ItemName,
                                        Description = x.Description,
                                        CuttableWidth = (x.CuttableWidth).ToString(),
                                        Quantity = x.Quantity,
                                        UnitPrice = x.UnitPrice,
                                        Amount = x.Amount
                                    }).ToList();

                    recordsTotal = ObjRevisedHD.Count();
                    ObjRevisedHD = ObjRevisedHD.OrderBy(x => x.HDODetailID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ObjRevisedHD;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public async Task<dynamic> SaveUpdateHeadOfficeSalesDeliveryOrder(vmSalHDOMaster model, UserCommonEntity commonEntity)
        {
            GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
            GenericFactoryFor_HDODetails_EF = new SalHDODetail_EF();
            GenericFactoryFor_DoMaster = new SalHDOMaster_GF();
            string result = string.Empty;
            string resultDONo = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                string spQuery = string.Empty;
                int NextId = Convert.ToInt16(GenericFactoryFor_HDOMaster_EF.getMaxID("SalHDOMaster"));
                if (model.HDOID > 0)
                {
                    if (model.IsDeleted == false)
                    {
                        model.AdoNo = "ADO-0001";
                        model.AdoQty = 0;
                        ht.Add("LUserID", commonEntity.loggedUserID);
                        ht.Add("LCompanyID", commonEntity.loggedCompnyID);
                        ht.Add("LMenuID", commonEntity.currentMenuID);
                        ht.Add("LTransactionTypeID", commonEntity.currentTransactionTypeID);
                        ht.Add("HDOID", model.HDOID);
                        ht.Add("CompanyID", model.CompanyID);
                        ht.Add("BuyerID", model.UserId);
                        ht.Add("HDONo", model.HDONo);
                        ht.Add("HDODate", model.HDODate);
                        ht.Add("LCID", model.LCID);
                        ht.Add("AHDONo", model.AdoNo);
                        ht.Add("AHDOQTY", model.AdoQty);
                        ht.Add("B2BLCNo", model.B2BLCNo);
                        if (model.B2BLCDate == Convert.ToDateTime("1/1/1900 12:00:00 AM"))
                        {
                            ht.Add("B2BLCDate", "");
                        }
                        else
                        {
                            ht.Add("B2BLCDate", model.B2BLCDate);
                        }
                        ht.Add("UpNo", model.UpNo);
                        ht.Add("Beneficiary", model.Beneficiary);
                        ht.Add("Remarks", model.Remarks);

                        spQuery = "[Put_HeadOfficeDeliveryOrder]";
                    }
                    else
                    {
                        ht.Add("LUserID", model.LUserID);
                        ht.Add("LCompanyID", model.LCompanyID);

                        ht.Add("HDOID", model.HDOID);
                        ht.Add("IsDeleted", model.IsDeleted);

                        spQuery = "[Delete_SalHDO]";
                    }
                }
                else
                {
                    model.AdoNo = "ADO-0001";
                    model.AdoQty = 0;
                    ht.Add("LUserID", commonEntity.loggedUserID);
                    ht.Add("LCompanyID", commonEntity.loggedCompnyID);
                    ht.Add("LMenuID", commonEntity.currentMenuID);
                    ht.Add("LTransactionTypeID", commonEntity.currentTransactionTypeID);

                    ht.Add("HDOID", model.HDOID);
                    ht.Add("CompanyID", model.CompanyID);
                    ht.Add("BuyerID", model.UserId);
                    ht.Add("HDONo", model.HDONo);
                    ht.Add("HDODate", model.HDODate);
                    ht.Add("LCID", model.LCID);
                    ht.Add("AHDONo", model.AdoNo);
                    ht.Add("AHDOQTY", model.AdoQty);
                    ht.Add("B2BLCNo", model.B2BLCNo);
                    if (model.B2BLCDate == Convert.ToDateTime("1/1/1900 12:00:00 AM"))
                    {
                        ht.Add("B2BLCDate", "");
                    }
                    else
                    {
                        ht.Add("B2BLCDate", model.B2BLCDate);
                    }
                    ht.Add("UpNo", model.UpNo);
                    ht.Add("Beneficiary", model.Beneficiary);
                    ht.Add("Remarks", model.Remarks);

                    spQuery = "[Set_SalHDO]";
                }
                result = GenericFactoryFor_DoMaster.ExecuteCommandString(spQuery, ht);

                //resultDONo = result;
                #region WorkFlow Transaction Entry and email sending
                if (model.HDOID == 0)
                {
                    // string[] words = result.Split(',');
                    // resultDONo = words[1];

                    SalHDOMaster HDOMaster = GenericFactoryFor_HDOMaster_EF.FindBy(h => h.HDONo == result).FirstOrDefault();
                   // GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();

                    int workflowID = 0;
                    List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                    foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                    {
                        int userTeamID = item.UserTeamID ?? 0;
                        if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID) && userTeamID > 0)
                        {
                            item.WorkFlowTranCustomID = (Int16)HDOMaster.HDOID;
                            workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                        }
                        if (userTeamID == 0)
                        {
                            item.WorkFlowTranCustomID = Convert.ToInt16(model.CompanyID);
                            workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                        }
                    }

                    int mail = 0;
                    foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                    {
                        NotificationEntity notification = new NotificationEntity();
                        notification.WorkFlowID = item.WorkFlowID;
                        notification.TransactionID = (Int16)HDOMaster.HDOID;
                        List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(notification, "Created");
                        foreach (var mailModel in nModel)
                        {
                            mail = await new EmailService().NotificationMail(mailModel);

                            // for saving email sending record//////////////////////// 

                            //CmnEmailTracking emailtrack = new CmnEmailTracking();


                            //emailtrack.MenuID = commonEntity.currentMenuID ?? 0;
                            //emailtrack.SenderEmployeeID = commonEntity.loggedUserID ?? 0;
                            //emailtrack.SenderEmailAddress = "";

                            //emailtrack.ReceiverEmployeeID = 11;
                            //emailtrack.ReceiverEmailAddress = mailModel.nextUserEmailAddress;

                            //emailtrack.TransactionNo = words[1];
                            //emailtrack.TransactionID = Convert.ToInt16(words[0]);
                            //emailtrack.Message = mailModel.message;
                            //emailtrack.MessageDate = DateTime.Now;
                            //emailtrack.CompanyID = (int)model.CompanyID;
                            //emailtrack.IsActive = true;
                            //emailtrack.IsApprove = mailModel.isApproved;
                            //emailtrack.IsDeleted = false;


                            //GenericFactoryFor_CmnEmailTracking = new CmnEmailTracking_EF();
                            //GenericFactoryFor_CmnEmailTracking.Insert(emailtrack);
                            //GenericFactoryFor_CmnEmailTracking.Save();
                        }
                    }

                }
                #endregion WorkFlow Transaction Entry and email sending    
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }
        //public async Task<int> ApproveNotification(NotificationEntity model)
        //{
        //    int returnValue = 0;
        //    try
        //    {
        //        List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(model, "DO Created.");
        //        foreach (var item in nModel)
        //        {
        //            returnValue = await new EmailService().NotificationMail(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return returnValue;
        //}
        #region AppDate
        public string ApproveModel(vmCmnParameters model)
        {
            GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
            string result = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    SalHDOMaster modleENtity = GenericFactoryFor_HDOMaster_EF.FindBy(h => h.HDOID == model.id).FirstOrDefault();

                    var UserStatus = (from WF in _ctxCmn.CmnWorkFlowMasters
                                      join WD in _ctxCmn.CmnWorkFlowDetails on WF.WorkFlowID equals WD.WorkFlowID
                                      where WF.MenuID == model.menuId && WF.CompanyID == model.loggedCompany
                                      && WD.EmployeeID == model.loggeduser && WD.CompanyID == model.loggedCompany
                                      select WD.StatusID).FirstOrDefault();

                    var UserSequence = (from WF in _ctxCmn.CmnWorkFlowMasters
                                        join WD in _ctxCmn.CmnWorkFlowDetails on WF.WorkFlowID equals WD.WorkFlowID
                                        where WF.MenuID == model.menuId && WF.CompanyID == model.loggedCompany
                                        && WD.EmployeeID == model.loggeduser && WD.CompanyID == model.loggedCompany
                                        select WD.Sequence).FirstOrDefault();

                    var SequenceMax = (from WFM in _ctxCmn.CmnWorkFlowMasters
                                       join WFD in _ctxCmn.CmnWorkFlowDetails on WFM.WorkFlowID equals WFD.WorkFlowID
                                       where WFM.MenuID == model.menuId && WFM.CompanyID == model.loggedCompany
                                       && WFD.CompanyID == model.loggedCompany
                                       select WFD.Sequence).Max(); // Need to do rework here. you need to find status max by using sequence max

                    var StatusMax = (from WFM in _ctxCmn.CmnWorkFlowMasters
                                     join WFD in _ctxCmn.CmnWorkFlowDetails on WFM.WorkFlowID equals WFD.WorkFlowID
                                     where WFM.MenuID == model.menuId && WFM.CompanyID == model.loggedCompany
                                     && WFD.CompanyID == model.loggedCompany && WFD.Sequence == SequenceMax
                                     select WFD.StatusID).FirstOrDefault(); // Need to do rework here. you need to find status max by using sequence max

                    if (model.ItemType == 0) // approval part
                    {

                        modleENtity.StatusID = (int)UserStatus;
                        modleENtity.StatusDate = DateTime.Now;
                        modleENtity.ApprovedBy = model.loggeduser;
                        modleENtity.DeclinedBy = 0;
                        if (StatusMax == UserStatus)
                        {
                            modleENtity.IsAllApproved = true;
                        }
                        else
                        {
                            modleENtity.IsAllApproved = false;
                        }
                    }    //// decline part
                    else
                    {
                        int newSequence = UserSequence - 1;

                        var StatusAsPerSequence = (from WFM in _ctxCmn.CmnWorkFlowMasters
                                                   join WFD in _ctxCmn.CmnWorkFlowDetails on WFM.WorkFlowID equals WFD.WorkFlowID
                                                   where WFM.MenuID == model.menuId && WFM.CompanyID == model.loggedCompany
                                                   && WFD.CompanyID == model.loggedCompany && WFD.Sequence == newSequence
                                                   select WFD.StatusID).FirstOrDefault(); // Need to do rework here. you need to find status max by using sequence max

                        modleENtity.StatusID = StatusAsPerSequence;
                        modleENtity.DeclinedBy = model.loggeduser;

                        //if (modleENtity.StatusID > 1 && modleENtity.StatusID < 5 )
                        //{
                        //    modleENtity.StatusID = modleENtity.StatusID - 1; // need to check. it can be done by minus user status
                        //}
                        //else if (modleENtity.StatusID == 1 && UserStatus == 1)
                        //{
                        //    modleENtity.IsDeleted = true;
                        //    modleENtity.StatusID = 0;
                        //    modleENtity.IsActive = false;
                        //}
                        //else if (modleENtity.StatusID == 1 && UserStatus == 2)
                        //{
                        //    modleENtity.StatusID = 1;
                        //}
                    }
                    GenericFactoryFor_HDOMaster_EF.Update(modleENtity);
                    GenericFactoryFor_HDOMaster_EF.Save();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }
        #endregion

        public string RevisedHeadOfficeSalesDeliveryOrder(vmSalHDOMaster model, List<vmPIDetail> ListModel, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                string CustomNo = string.Empty, HDONo = string.Empty; int MasterId = 0; long DetailId = 0, FirstDigit = 0, OtherDigits = 0; int DetailRowNum = 0, RevisionNo = 0;
                GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
                GenericFactoryFor_HDODetails_EF = new SalHDODetail_EF();
                GenericFactoryFor_CmnWorkFlowTransaction_EF = new CmnWorkFlowTransaction_EF();

                var MasterItem = new SalHDOMaster();
                var DetailItem = new List<SalHDODetail>();
                var UMasterItem = new SalHDOMaster();
                var UDetailItem = new List<SalHDODetail>();
                var UWFTList = new List<CmnWorkFlowTransaction>();
                vmPIDetail itemDetail = new vmPIDetail();

                try
                {
                    if (model.HDOID > 0)
                    {
                        var HDOMaster = GenericFactoryFor_HDOMaster_EF.GetAll();
                        var HDODetail = GenericFactoryFor_HDODetails_EF.GetAll();
                        //********************************Start Set Revision No and Get Revised HDO No******************************************************
                        var PreviousID = (from Prev in HDOMaster.Where(x => x.HDOID == model.HDOID) select Prev.OriginalDOMasterID).FirstOrDefault();
                        RevisionNo = Convert.ToInt16(HDOMaster.Where(x => x.OriginalDOMasterID == PreviousID).Count());

                        string[] splitString = model.HDONo.Split('[');
                        HDONo = splitString[0].Trim();
                        HDONo = (HDONo + " [Revised-" + RevisionNo + "]").ToString();
                        //**********************************Endt Set Revision No and Get Revised HDO No******************************************************
                        //************************************************Start Create New Revised HDO******************************************************
                        DetailRowNum = Convert.ToInt32(ListModel.Count());

                        MasterId = Convert.ToInt16(GenericFactoryFor_HDOMaster_EF.getMaxID("SalHDOMaster"));
                        DetailId = Convert.ToInt64(GenericFactoryFor_HDODetails_EF.getMaxID("SalHDODetail"));
                        FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                        MasterItem = new SalHDOMaster
                        {
                            HDOID = MasterId,
                            HDONo = HDONo,
                            HDODate = DateTime.Today,
                            LCID = (int)model.LCID,
                            BuyerID = (int)model.UserId,
                            Remarks = model.Remarks,
                            IsActive = true,
                            AHDONo = model.AdoNo,
                            AHDOQTY = model.AdoQty,
                            OriginalDOMasterID = PreviousID,
                            RevisionNo = RevisionNo,
                            B2BLCNo = model.B2BLCNo,
                            B2BLCDate = model.B2BLCDate,
                            UPNo = model.UpNo,
                            Beneficiary = model.Beneficiary,
                            TransactionTypeID = objcmnParam.tTypeId,

                            StatusID = 1,
                            IsAllApproved = false,
                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP(),
                            IsDeleted = false
                        };

                        for (int i = 0; i < DetailRowNum; i++)
                        {
                            itemDetail = ListModel[i];

                            var Details = new SalHDODetail
                            {
                                HDODetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                HDOID = MasterId,
                                PIID = itemDetail.PIID,
                                ProductID = (long)itemDetail.ItemID,
                                Price = itemDetail.UnitPrice,
                                Quantity = itemDetail.Quantity,
                                RemainingQty = itemDetail.Quantity,
                                IsFDOCompleted = false,
                                Amount = itemDetail.Amount,
                                IsOriginal = true,
                                IsActive = true,
                                IsDC = false,
                                OriginalDOMasterID = PreviousID,
                                StatusID = 1,
                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc = HostService.GetIP(),
                                IsDeleted = false
                            };
                            DetailItem.Add(Details);
                            OtherDigits++;
                        }
                        //**************************************************End Create New Revised HDO******************************************************
                        //**************************************************Start Update Related table******************************************************
                        UMasterItem = HDOMaster.Where(x => x.HDOID == model.HDOID).FirstOrDefault();
                        UMasterItem.IsActive = false;

                        foreach (SalHDODetail m in HDODetail.Where(x => x.HDOID == model.HDOID))
                        {
                            m.IsActive = false;
                            UDetailItem.Add(m);
                        }

                        var UpWFT = GenericFactoryFor_CmnWorkFlowTransaction_EF.GetAll().Where(x => x.TransactionID == model.HDOID);
                        foreach (CmnWorkFlowTransaction t in UpWFT.Where(x => x.TransactionID == model.HDOID && x.MenuID == objcmnParam.menuId && x.CompanyID == objcmnParam.loggedCompany))
                        {
                            t.IsDeleted = true;
                            t.DeleteBy = objcmnParam.loggeduser;
                            t.DeleteOn = DateTime.Now;
                            t.DeletePc = HostService.GetIP();
                            UWFTList.Add(t);
                        }
                        //****************************************************End Update Related table******************************************************
                        if (UMasterItem != null)
                        {
                            GenericFactoryFor_HDOMaster_EF.Update(UMasterItem);
                            GenericFactoryFor_HDOMaster_EF.Save();
                        }
                        if (MasterItem != null)
                        {
                            GenericFactoryFor_HDOMaster_EF.Insert(MasterItem);
                            GenericFactoryFor_HDOMaster_EF.Save();
                            GenericFactoryFor_HDOMaster_EF.updateMaxID("SalHDOMaster", Convert.ToInt64(MasterId));
                            GenericFactoryFor_HDOMaster_EF.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        }
                        if (UDetailItem != null && UDetailItem.Count != 0)
                        {
                            GenericFactoryFor_HDODetails_EF.UpdateList(UDetailItem.ToList());
                            GenericFactoryFor_HDODetails_EF.Save();
                        }
                        if (DetailItem != null && DetailItem.Count != 0)
                        {
                            GenericFactoryFor_HDODetails_EF.InsertList(DetailItem.ToList());
                            GenericFactoryFor_HDODetails_EF.Save();
                            GenericFactoryFor_HDODetails_EF.updateMaxID("SalHDODetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        }
                        if (UWFTList != null && UWFTList.Count != 0)
                        {
                            GenericFactoryFor_CmnWorkFlowTransaction_EF.UpdateList(UWFTList.ToList());
                            GenericFactoryFor_CmnWorkFlowTransaction_EF.Save();
                        }

                        #region WorkFlow Transaction Entry
                        //  CHECK WorkFLow Setup
                        //IF targetUserID=0 Then Work flow is not configured

                        UserCommonEntity userCommonEntity = new UserCommonEntity();
                        userCommonEntity.loggedUserID = objcmnParam.loggedCompany;
                        userCommonEntity.loggedCompnyID = objcmnParam.loggedCompany;
                        userCommonEntity.currentMenuID = objcmnParam.menuId;


                        int targetUserID = new WorkFLowMgt().GetWorkFlowBeginner(userCommonEntity);
                        if (targetUserID > 0)
                        {
                            //IN CASE OF SAVE
                            //REQUIRD MENUID, COMPANYID, TARGET USERID AS TARGET USERID, USER AS CURRENT USERID,TRANSACTION ID AS MASTERID(INSERTED) ,STATUSID 1
                            //IS APPROVED 0, IS DELETE 0, IS UPDATE 0 , @APPROVALCUSTOMCODE = '',Is Declained=0
                            var comment = string.Empty;
                            var APPROVALCUSTOMCODE = string.Empty;
                            String MessageName = System.Enum.GetName(typeof(workFlowTranEnum_MessageName), (int)workFlowTranEnum_MessageName.Created);
                            int WorkFlowStatus = new WorkFLowMgt().ExecuteWorkFlowTransactionProcess(new WorkFLowMgt().SetProcedureParam(userCommonEntity,
                                targetUserID, MasterId, (int)workFlowTranEnum_IsApproved.False, comment, (int)workFlowTranEnum_IsUpdate.False,
                                (int)workFlowTranEnum_IsDelete.False, (int)workFlowTranEnum_Status.Active, APPROVALCUSTOMCODE,
                                (int)workFlowTranEnum_IsDeclained.False, MessageName));

                        }
                        #endregion WorkFlow Transaction Entry

                        transaction.Complete();
                        result = HDONo;
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                    if (result == "")
                    {
                        result = "";
                    }
                }
                #region To check Validation Error
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
                #endregion
            }

            return result;
        }
    }
}