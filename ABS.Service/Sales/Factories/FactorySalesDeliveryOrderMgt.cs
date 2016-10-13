using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility;
namespace ABS.Service.Sales.Factories
{
    public class FactorySalesDeliveryOrderMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company_EF = null;
        private iGenericFactory_EF<SalHDOMaster> GenericFactoryFor_HDOMaster_EF = null;
        private iGenericFactory_EF<SalHDODetail> GenericFactoryFor_HDODetails_EF = null;
        private iGenericFactory_EF<SalFDODetail> GenericFactoryFor_FDODetails = null;
        private iGenericFactory_EF<SalFDOMaster> GenericFactoryFor_FDOMaster = null;
        private iGenericFactory_EF<InvStockMaster> GFactory_EF_InvStockMaster = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactoryFor_CmnUserWiseCompany_EF = null;
        private iGenericFactory_EF<CmnOrganogram> GenericFactoryFor_CmnOrganogram_EF = null;
        private iGenericFactory_EF<CmnLot> GenericFactoryFor_CmnLot_EF = null;
        private iGenericFactory_EF<InvStockTransit> GFactory_EF_InvStockTransit = null;

        public IEnumerable<CmnCompany> GetCompany(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_Company_EF = new CmnCompany_EF();
            GenericFactoryFor_CmnUserWiseCompany_EF = new CmnUserWiseCompany_EF();
            IEnumerable<CmnCompany> objCompanies = null;
            try
            {
                var company = GenericFactoryFor_Company_EF.GetAll();
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

        public IEnumerable<CmnOrganogram> GetDepartment(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_CmnOrganogram_EF = new CmnOrganogram_EF();
            IEnumerable<CmnOrganogram> objDept = null;
            try
            {
                var cmnOrgan = GenericFactoryFor_CmnOrganogram_EF.GetAll();
                objDept = (from organ in cmnOrgan
                           where organ.CompanyID == objcmnParam.selectedCompany && organ.IsDepartment == true
                           && organ.IsDeleted == false
                           orderby organ.OrganogramName
                           select new
                           {
                               OrganogramID = organ.OrganogramID,
                               OrganogramName = organ.OrganogramName
                           }).ToList().Select(x => new CmnOrganogram
                           {
                               OrganogramID = x.OrganogramID,
                               OrganogramName = x.OrganogramName
                           }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDept;
        }

        public IEnumerable<CmnLot> GetLotByItemId(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_CmnLot_EF = new CmnLot_EF();
            IEnumerable<CmnLot> objLot = null;
            try
            {
                var Lot = GenericFactoryFor_CmnLot_EF.GetAll();
                objLot = (from L in Lot
                          where L.ItemID == objcmnParam.id && L.IsDeleted == false
                          orderby L.LotID
                          select new
                          {
                              LotID = L.LotID,
                              LotNo = L.LotNo
                          }).ToList().Select(x => new CmnLot
                          {
                              LotID = x.LotID,
                              LotNo = x.LotNo
                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objLot;
        }

        public IEnumerable<vmSalHDOMaster> GetDOMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<CmnUserWiseCompany> whichCompanies = null;
            IEnumerable<vmSalHDOMaster> objDOMaster = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    whichCompanies = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == objcmnParam.loggeduser && m.IsDeleted == false).ToList().
                       Select(m => new CmnUserWiseCompany
                       {
                           CompanyID = m.CompanyID,
                           // CreatePc = m.CreatePc
                       }).ToList();

                    // Get HDO master information
                    List<SalHDOMaster> HDOMaster = new List<SalHDOMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalHDOMaster> AddToList = new List<SalHDOMaster>();
                        AddToList = (from hd in _ctxCmn.SalHDOMasters select hd).Where(m => m.CompanyID == u.CompanyID && m.IsDeleted == false && m.IsActive == true && m.IsAllApproved == true).ToList();
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            HDOMaster.AddRange(AddToList);
                        }
                    }

                    // Get HDO Detail Information
                    List<SalHDODetail> HDODetail = new List<SalHDODetail>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalHDODetail> shdod = new List<SalHDODetail>();
                        shdod = (from hd in _ctxCmn.SalHDODetails select hd).Where(d => d.IsFDOCompleted == false && d.CompanyID == u.CompanyID && d.IsDeleted == false && d.IsActive == true).ToList();
                        if (shdod != null)
                        {
                            HDODetail.AddRange(shdod);
                        }
                    }

                    // Get LC Master Information
                    List<SalLCMaster> LCMaster = new List<SalLCMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalLCMaster> AddToList = new List<SalLCMaster>();
                        AddToList = (from hd in _ctxCmn.SalLCMasters select hd).Where(L => L.CompanyID == u.CompanyID && L.IsDeleted == false).ToList();// && L.IsActive == true
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            LCMaster.AddRange(AddToList);
                        }
                    }

                    objDOMaster = (from m in HDOMaster
                                   join D in HDODetail
                                   on m.HDOID equals D.HDOID
                                   join L in LCMaster
                                   on m.LCID equals (int)L.LCID
                                   join U in _ctxCmn.CmnUsers on m.BuyerID equals U.UserID
                                   select new
                                   {
                                       HDOID = m.HDOID,
                                       HDONo = m.HDONo,
                                       HDODate = m.HDODate,
                                       LCNo = L.LCNo,
                                       UserName = U.UserFullName
                                   }).Distinct().ToList().Select(x => new vmSalHDOMaster
                                   {
                                       HDOID = x.HDOID,
                                       HDONo = x.HDONo,
                                       HDODate = x.HDODate,
                                       LCNo = x.LCNo,
                                       UserName = x.UserName
                                   }).ToList();

                    recordsTotal = objDOMaster.Count();
                    objDOMaster = objDOMaster.OrderBy(x => x.HDOID).Skip(0).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDOMaster;
        }

        public vmSalHDOMaster GetDOMasterById(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
            GenericFactoryFor_HDODetails_EF = new SalHDODetail_EF();

            vmSalHDOMaster objDOMasterById = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    var HDONoWiseID = GenericFactoryFor_HDOMaster_EF.FindBy(HD => objcmnParam.ParamName == "" ? HD.HDOID == objcmnParam.id : HD.HDONo == objcmnParam.ParamName && HD.CompanyID == objcmnParam.loggedCompany && HD.IsDeleted == false && HD.IsActive == true && HD.IsAllApproved == true).Select(x => x.HDOID).FirstOrDefault();
                    var DODetailCount = GenericFactoryFor_HDODetails_EF.FindBy(HD => HD.HDOID == HDONoWiseID 
                    && HD.CompanyID == objcmnParam.selectedCompany && HD.IsFDOCompleted == false && HD.IsDeleted == false 
                    && HD.IsActive == true).ToList().Count;

                    objDOMasterById = (from m in _ctxCmn.SalHDOMasters
                                       join b in _ctxCmn.CmnUsers on m.BuyerID equals b.UserID
                                       //join l in _ctxCmn.SalLCMasters.Where(L => L.CompanyID == objcmnParam.loggedCompany && L.IsDeleted == false && L.IsActive == true) 
                                       //on m.LCID equals (int)l.LCID
                                       where m.HDOID == HDONoWiseID
                                       orderby m.HDOID descending
                                       select new
                                       {
                                           HDOID = m.HDOID,
                                           HDONo = m.HDONo,
                                           DeliveredTo = b.ParAddress1,
                                           UserId = b.UserID,
                                           UserName = b.UserFullName,
                                           CompanyID = m.CompanyID,
                                           DODetailCount = DODetailCount
                                       })
                                        .Select(x => new vmSalHDOMaster
                                        {
                                            HDOID = x.HDOID,
                                            HDONo = x.HDONo,
                                            DeliveredTo = x.DeliveredTo,
                                            UserId = x.UserId,
                                            UserName = x.UserName,
                                            CompanyID = x.CompanyID,
                                            DODetailCount = x.DODetailCount
                                        }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDOMasterById;
        }

        public IEnumerable<vmHDODetail> GetDODetailByID(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
            GenericFactoryFor_HDODetails_EF = new SalHDODetail_EF();

            IEnumerable<vmHDODetail> objDODetail = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    var HDONoWiseID = GenericFactoryFor_HDOMaster_EF.FindBy(HD => objcmnParam.ParamName == "" ? HD.HDOID == objcmnParam.id : HD.HDONo == objcmnParam.ParamName).Select(x => x.HDOID).FirstOrDefault();
                    var IsPIID = GenericFactoryFor_HDODetails_EF.FindBy(HD => HD.HDOID == HDONoWiseID).ToList().Select(x => x.PIID).FirstOrDefault();

                    if (IsPIID > 0)
                    {
                        objDODetail = (from HD in _ctxCmn.SalHDODetails
                                       join CIM in _ctxCmn.CmnItemMasters on HD.ProductID equals CIM.ItemID
                                       join PM in _ctxCmn.SalPIMasters on HD.PIID equals PM.PIID
                                       where HD.HDOID == HDONoWiseID && HD.IsFDOCompleted == false
                                       orderby HD.HDODetailID descending
                                       select new vmHDODetail
                                       {
                                           HDODetailID = HD.HDODetailID,
                                           PIID = (int)HD.PIID,
                                           ItemID = (int)HD.ProductID,
                                           PINO = PM.PINO,
                                           ItemName = CIM.ItemName,
                                           UnitPrice = (decimal)HD.Price,
                                           Quantity = (decimal)HD.Quantity,
                                           RemainingQty = (decimal)HD.RemainingQty,
                                           HoldQty = (decimal)HD.RemainingQty,
                                           Amount = (decimal)HD.Amount,
                                           LotNos = (from L in _ctxCmn.CmnLots
                                                     where L.LotNo != null && L.ItemID == HD.ProductID
                                                     select new vmLot { LotID = L.LotID, LotNo = L.LotNo }).ToList(),
                                           BatchNos = (from B in _ctxCmn.CmnBatches
                                                       where B.BatchNo != null && B.ItemID == HD.ProductID
                                                       select new vmBatch { BatchID = B.BatchID, BatchNo = B.BatchNo }).ToList(),
                                           GradeList = (from B in _ctxCmn.CmnItemGrades
                                                        where B.GradeName != null
                                                        select new vmGrade { ItemGradeID = B.ItemGradeID, GradeName = B.GradeName }).ToList()

                                       }).ToList();

                    }
                    else
                    {
                        objDODetail = (from HD in _ctxCmn.SalHDODetails
                                       join CIM in _ctxCmn.CmnItemMasters on HD.ProductID equals CIM.ItemID
                                       where HD.HDOID == HDONoWiseID && HD.IsFDOCompleted == false //&& PM.IsActive == true                               
                                       orderby HD.HDODetailID descending
                                       select new vmHDODetail
                                       {
                                           HDODetailID = HD.HDODetailID,
                                           PIID = 0,
                                           ItemID = (int)HD.ProductID,
                                           PINO = "Not Available",
                                           ItemName = CIM.ItemName,
                                           UnitPrice = (decimal)HD.Price,
                                           Quantity = (decimal)HD.Quantity,
                                           RemainingQty = (decimal)HD.RemainingQty,
                                           HoldQty = (decimal)HD.RemainingQty,
                                           Amount = (decimal)HD.Amount,
                                           LotNos = (from L in _ctxCmn.CmnLots
                                                     where L.LotNo != null && L.ItemID == HD.ProductID
                                                     select new vmLot { LotID = L.LotID, LotNo = L.LotNo }).ToList(),
                                           BatchNos = (from B in _ctxCmn.CmnBatches
                                                       where B.BatchNo != null && B.ItemID == HD.ProductID
                                                       select new vmBatch { BatchID = B.BatchID, BatchNo = B.BatchNo }).ToList(),
                                           GradeList = (from B in _ctxCmn.CmnItemGrades
                                                        where B.GradeName != null
                                                        select new vmGrade { ItemGradeID = B.ItemGradeID, GradeName = B.GradeName }).ToList()

                                       }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDODetail;
        }

        public IEnumerable<SalFDOMaster> GetFDOMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<SalFDOMaster> objFDOMaster = null;
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
                          // CreatePc = m.CreatePc
                      }).ToList();

                    // Get FDO master information
                    List<SalFDOMaster> FDOMaster = new List<SalFDOMaster>();
                    List<SalFDOMaster> sfdom = new List<SalFDOMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        //SalFDOMaster sfdom = new SalFDOMaster();
                        sfdom = (from hd in _ctxCmn.SalFDOMasters select hd).Where(m => m.CompanyID == u.CompanyID && m.IsDeleted == false && m.IsActive == true).ToList();
                        if (sfdom != null)
                        {
                            FDOMaster.AddRange(sfdom);
                        }
                    }

                    objFDOMaster = (from m in FDOMaster
                                    select new SalFDOMaster
                                    {
                                        FDOMasterID = m.FDOMasterID,
                                        FDONo = m.FDONo,
                                        FDODate = m.FDODate,
                                        BillNo = m.BillNo,
                                        BillDate = m.BillDate,
                                        DeliveryTo = m.DeliveryTo
                                    }).ToList();


                    recordsTotal = objFDOMaster.Count();
                    objFDOMaster = objFDOMaster.OrderByDescending(x => x.FDOMasterID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objFDOMaster;
        }

        public IEnumerable<SalFDODetail> GetFDODetail(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<SalFDODetail> objFDODetail = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objFDODetail = (from m in _ctxCmn.SalFDODetails.Where(m => m.IsDeleted == false && m.FDOMasterID == objcmnParam.id).ToList()//&& m.IsDCCompleted == false).ToList()
                                    select new SalFDODetail
                                    {
                                        FDODetailsID = m.FDODetailsID,
                                        RollNo = m.RollNo,
                                        QuantitYds = m.QuantitYds,
                                        QuantityKg = m.QuantityKg,
                                        Rate = m.Rate,
                                        Amount = m.Amount,
                                        NetQtyKg = m.NetQtyKg,
                                        GrossQtyKg = m.GrossQtyKg

                                    }).ToList();

                    recordsTotal = objFDODetail.Count();
                   // objFDODetail = objFDODetail.Skip(objcmnParam.pageNumber)
                                  // .Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objFDODetail;
        }

        public string SaveUpdateFactorySalesDeliveryOrderMasterDetail(vmSalFDODetail Master, List<vmSalFDODetail> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GenericFactoryFor_HDOMaster_EF = new SalHDOMaster_EF();
                GenericFactoryFor_HDODetails_EF = new SalHDODetail_EF();
                GenericFactoryFor_FDOMaster = new SalFDOMaster_EF();
                GenericFactoryFor_FDODetails = new SalFDODetail_EF();
                GFactory_EF_InvStockTransit = new InvStocTransit_Sale_EF();
                GFactory_EF_InvStockMaster = new InvStockMaster_Sale_EF();

                string CustomNo = string.Empty, FDONo = ""; decimal CurrentStock = 0, TransitQty = 0;
                long FdoMasterId = 0, FdoDetailId = 0, FirstDigit = 0, OtherDigits = 0, NextId = 0;

                var Masteritem = new SalFDOMaster();
                var FDODetail = new List<SalFDODetail>();
                var UpdateHDODetail = new List<SalHDODetail>();
                var InsertStockTransit = new List<InvStockTransit>();
                var item = new vmSalFDODetail();
                var items = new vmSalFDODetail();

                //To Update Remaining Quantity in SalHDODetail
                var HdoDetail = GenericFactoryFor_HDODetails_EF.GetAll();
                var StockMaster = GFactory_EF_InvStockMaster.GetAll().Where(x => x.DepartmentID == objcmnParam.SelectedDepartmentID).ToList();
                var StockTransit = GFactory_EF_InvStockTransit.GetAll().Where(x => x.DepartmentID == objcmnParam.SelectedDepartmentID).ToList();
                //-------------------END----------------------

                if (Detail.Count() > 0)
                {
                    try
                    {
                        NextId = GFactory_EF_InvStockTransit.getMaxVal_int64("StockTransitID", "InvStockTransit");
                        //start new maxId
                        FdoMasterId = Convert.ToInt64(GenericFactoryFor_FDOMaster.getMaxID("SalFDOMaster"));
                        FdoDetailId = Convert.ToInt64(GenericFactoryFor_FDODetails.getMaxID("SalFDODetail"));
                        FirstDigit = Convert.ToInt64(FdoDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(FdoDetailId.ToString().Substring(1, FdoDetailId.ToString().Length - 1));
                        //end new maxId
                        CustomNo = GenericFactoryFor_FDOMaster.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1); // // 1 for user id and 1 for db id --- work later
                        if (CustomNo != null)
                        {
                            FDONo = CustomNo;
                        }
                        else
                        {
                            FDONo = FdoMasterId.ToString();
                        }

                        Masteritem = new SalFDOMaster
                        {
                            FDOMasterID = FdoMasterId,
                            TransactionTypeID = objcmnParam.tTypeId,
                            FDONo = FDONo,
                            FDODate = DateTime.Today,
                            HDOID = (Master.MHDOID).ToString(),
                            PartyID = (int)Master.MPartyID,
                            DeliveryTo = Master.MDeliveryTo,
                            BillNo = Master.MBillNo,
                            BillDate = Master.MBillDate,
                            DriverName = Master.DriverName,
                            DriverPhoneNo = Master.DriverPhoneNo,
                            BuyerContactName = Master.BuyerContactName,
                            BuyerContactPhoneNo = Master.BuyerContactPhoneNo,
                            TruckNo = Master.TruckNo,
                            CompanyID = objcmnParam.loggedCompany,
                            DepartmentID = objcmnParam.SelectedDepartmentID,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP(),
                            IsActive = true,
                            IsDeleted = false,
                            StatusBy = objcmnParam.loggeduser,
                            StatusID = 1,
                            IsDCCompleted = false
                        };

                        for (int i = 0; i < Detail.Count(); i++)
                        {
                            item = Detail[i];

                            CurrentStock = 0;
                            TransitQty = 0;
                            //*************************To Check Stock Quantity in InvStockMaster************************

                            var StockMasterTemp1 = StockMaster.Where(s => item.BatchId == null ? true : s.BatchID == item.BatchId).ToList();

                            var StockMasterTemp2 = (from x in StockMasterTemp1.Where(x => item.LotId == null ? true : x.LotID == item.LotId)
                                                    where item.ItemGradeID == null ? true : x.GradeID == item.ItemGradeID
                                                    select new InvStockMaster
                                                    {
                                                        DepartmentID = x.DepartmentID,
                                                        LotID = x.LotID,
                                                        BatchID = x.BatchID,
                                                        GradeID = x.GradeID,
                                                        ItemID = x.ItemID,
                                                        CurrentStock = x.CurrentStock
                                                    }).ToList();
                            foreach (InvStockMaster j in StockMasterTemp2.Where(s => s.ItemID == item.ItemID))
                            {
                                CurrentStock = CurrentStock + (decimal)j.CurrentStock;
                                //break;
                            }

                            ///// checking transit table from database data//////////////
                            var TransitReal1 = StockTransit.Where(x => x.ItemID == item.ItemID).ToList();

                            var TransitReal2 = TransitReal1.Where(x => item.BatchId == null ? true : x.BatchID == item.BatchId).ToList();


                            var TransitReal = (from t in TransitReal2.Where(x => item.LotId == null ? true : x.LotID == item.LotId)
                                               where item.ItemGradeID == null ? true : t.GradeID == item.ItemGradeID
                                               select t.TransitQty).Sum();

                            /////////// checking data from FDO Detail///////////////////
                            var TransitTemp1 = InsertStockTransit.Where(x => x.ItemID == item.ItemID).ToList();

                            var TransitTemp2 = TransitTemp1.Where(x => item.BatchId == null ? true : x.BatchID == item.BatchId).ToList();
                            
                            var TransitTemp = (from t in TransitTemp2.Where(x => item.LotId == null ? true : x.LotID == item.LotId)
                                               where item.ItemGradeID == null ? true : t.GradeID == item.ItemGradeID
                                               select t.TransitQty).Sum();

                            TransitQty = (decimal)(TransitReal + TransitTemp);
                            //***************************************END*******************************************

                            //**********************************Detail Entry***************************************
                            if ((CurrentStock - TransitQty) >= item.QuantityYds && item.QuantityYds > 0)
                            {
                                var Detailitem = new SalFDODetail
                                {
                                    FDODetailsID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    FDOMasterID = FdoMasterId,
                                    ItemID = (long)item.ItemID,
                                    NetQtyKg = (decimal)item.NetQuantityKg,
                                    PIID = (int)item.PIID,
                                    QuantitYds = (decimal)item.QuantityYds,
                                    QuantityKg = (decimal)item.Quantity,
                                    GrossQtyKg = (decimal)item.GrossQuantityKg,
                                    Amount = (decimal)item.Amount,
                                    Rate = (decimal)item.UnitPrice,
                                    RollNo = (decimal)item.Roll,
                                    LotID = item.LotId,
                                    BatchID = item.BatchId,
                                    GradeID = item.ItemGradeID,

                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false,
                                    IsDCCompleted = false
                                };
                                //***************************************END*******************************************
                                var InsertTran = new InvStockTransit
                                {
                                    StockTransitID = NextId,
                                    TransactionID = FdoMasterId,
                                    TransactionTypeID = objcmnParam.tTypeId,
                                    TransitQty = item.QuantityYds,
                                    ItemID = item.ItemID,
                                    DepartmentID = objcmnParam.SelectedDepartmentID,
                                    LotID = item.LotId ?? 0,
                                    BatchID = item.BatchId ?? 0,
                                    GradeID = Convert.ToInt16(item.ItemGradeID ?? 0),
                                    IsDeleted = false,
                                    IsComplete = false,

                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP()
                                };
                                InsertStockTransit.Add(InsertTran);
                                NextId = NextId + 1;
                                //******************To Update Remaining Quantity in SalHDODetail******************
                                foreach (SalHDODetail u in HdoDetail.Where(u => u.HDODetailID == item.HDODetailID))
                                {
                                    if (u.RemainingQty > 0)
                                    {
                                        if (item.RemainingQty > 0)
                                        {
                                            u.RemainingQty = item.RemainingQty;
                                        }
                                        else
                                        {
                                            u.RemainingQty = item.RemainingQty;
                                            u.IsFDOCompleted = true;
                                        }
                                        UpdateHDODetail.Add(u);
                                        break;
                                    }
                                }
                                //************************************END************************************
                                FDODetail.Add(Detailitem);
                                OtherDigits++;
                            }
                            else
                            {
                                result = "1";
                                throw new Exception(); // if any product don't support by stock, go back.
                            }
                        }

                        if (Masteritem != null)
                        {
                            GenericFactoryFor_FDOMaster.Insert(Masteritem);
                            GenericFactoryFor_FDOMaster.Save();
                            GenericFactoryFor_FDOMaster.updateMaxID("SalFDOMaster", Convert.ToInt64(FdoMasterId));
                            GenericFactoryFor_FDOMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        }
                        if (FDODetail != null && FDODetail.Count != 0)
                        {
                            GenericFactoryFor_FDODetails.InsertList(FDODetail.ToList());
                            GenericFactoryFor_FDODetails.Save();
                            GenericFactoryFor_FDODetails.updateMaxID("SalFDODetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        }
                        if (UpdateHDODetail != null && UpdateHDODetail.Count != 0)
                        {
                            GenericFactoryFor_HDODetails_EF.UpdateList(UpdateHDODetail.ToList());
                            GenericFactoryFor_HDODetails_EF.Save();
                        }
                        if (InsertStockTransit != null || InsertStockTransit.Count() != 0)
                        {
                            GFactory_EF_InvStockTransit.InsertList(InsertStockTransit.ToList());
                            GFactory_EF_InvStockTransit.Save();
                        }

                        transaction.Complete();
                        result = FDONo;

                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
            }

            return result;
        }

        public IEnumerable<SalFDOMaster> GetFDONo(int userID, int companyID)
        {
            IEnumerable<SalFDOMaster> objFDONo = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    List<SalFDOMaster> FDOMaster = new List<SalFDOMaster>();
                    FDOMaster = (from hd in _ctxCmn.SalFDOMasters select hd).Where(m => m.IsDCCompleted == false
                                && m.CompanyID == companyID).OrderByDescending(m => m.FDOMasterID).ToList();
                    objFDONo = FDOMaster
                           .Select(m => new SalFDOMaster
                           {
                               FDOMasterID = m.FDOMasterID,
                               FDONo = m.FDONo,
                               CompanyID = m.CompanyID,
                               IsDCCompleted = m.IsDCCompleted
                           }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objFDONo;
        }
    }
}
