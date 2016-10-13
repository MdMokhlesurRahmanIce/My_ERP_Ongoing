using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Commercial.Interfaces;
using ABS.Service.Sales.Interfaces;
using ABS.Service.AllServiceClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Models.ViewModel.Commercial;
using ABS.Utility;

namespace ABS.Service.Commercial.Factories
{
    public class DCMgt : iDCMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<SalFDOMaster> GenericFactory_EF_FDOMaster = null;
        private iGenericFactory_EF<SalFDODetail> GenericFactory_EF_FDODetail = null;
        private iGenericFactory_EF<SalDCMaster> GenericFactory_EF_DCMaster = null;
        private iGenericFactory_EF<SalDCDetail> GenericFactory_EF_DCDetail = null;
        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_Company = null;
        //private iGenericFactory_EF<CmnBank> GenericFactory_EF_Bank = null;
        private iGenericFactory_EF<InvStockMaster> GFactory_EF_InvStockMaster = null;
        private iGenericFactory_EF<InvStockDetail> GFactory_EF_InvStockDetail = null;
        private iGenericFactory_EF<InvStockTransit> GFactory_EF_InvStockTransit = null;

        public IEnumerable<SalFDOMaster> GetAllFDONo(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<SalFDOMaster> objFDONo = null;
            recordsTotal = 0;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    List<SalFDOMaster> FDOMaster = new List<SalFDOMaster>();
                    FDOMaster = (from hd in _ctxCmn.SalFDOMasters select hd).Where(m => m.IsDCCompleted == false
                                && m.CompanyID == objcmnParam.selectedCompany).OrderByDescending(m => m.FDOMasterID).ToList();
                    objFDONo = FDOMaster
                           .Select(m => new SalFDOMaster
                           {
                               FDOMasterID = m.FDOMasterID,
                               FDONo = m.FDONo,
                               CompanyID = m.CompanyID,
                               IsDCCompleted = m.IsDCCompleted
                           }).ToList();
                    recordsTotal = objFDONo.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objFDONo;
        }

        public string SaveUpdateDC(SalDCMaster DCInfo, List<SalDCDetail> DCDetailList, vmCmnParameters objcmnParam)
        {
            GenericFactory_EF_DCMaster = new SalDCMaster_EF();
            GenericFactory_EF_DCDetail = new SalDCDetail_EF();
            GFactory_EF_InvStockMaster = new InvStockMaster_EF();
            GFactory_EF_InvStockDetail = new InvStockDetail_EF();
            GenericFactory_EF_FDOMaster = new SalFDOMaster_EF();
            GenericFactory_EF_FDODetail = new SalFDODetail_EF();
            GFactory_EF_InvStockTransit = new InvStockTransit_EF();

            string result = "";

            long NextId = Convert.ToInt64(GenericFactory_EF_DCMaster.getMaxID("SalDCMaster"));

            long FirstDigit = 0;
            long OtherDigits = 0;
            long nextDetailId = Convert.ToInt64(GenericFactory_EF_DCDetail.getMaxID("SalDCDetail"));
            FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
            OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));


            string customCode = "";

            string CustomNo = customCode = GenericFactory_EF_DCMaster.getCustomCode(objcmnParam.menuId, DCInfo.DCDate, 
                                            DCInfo.CompanyID, objcmnParam.loggeduser, 1); // 1 for DB ID
            if (CustomNo != null)
            {
                customCode = CustomNo;
            }
            else
            {
                customCode = NextId.ToString();
            }

            try
            {
                DCInfo.DCID = NextId;
                DCInfo.DCNo = customCode;
                DCInfo.CreateOn = DateTime.Now;
                DCInfo.IsActive = true;

                List<SalDCDetail> lstSalDCDetail = new List<SalDCDetail>();
                foreach (SalDCDetail sdtl in DCDetailList)
                {
                    SalDCDetail objSalDCDetail = new SalDCDetail();
                    //objSalDCDetail.DCDetailID = nextDetailId;
                    objSalDCDetail.DCDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                    objSalDCDetail.DCID = NextId;
                    objSalDCDetail.FDOID = sdtl.FDOID;
                    objSalDCDetail.TruckNo = sdtl.TruckNo;
                    objSalDCDetail.QuantityYds = sdtl.QuantityYds;
                    objSalDCDetail.Roll = sdtl.Roll;
                    objSalDCDetail.CompanyID = DCInfo.CompanyID;
                    objSalDCDetail.CreateBy = DCInfo.CreateBy;
                    objSalDCDetail.CreateOn = DateTime.Now;
                    objSalDCDetail.CreatePc =  HostService.GetIP();
                    objSalDCDetail.IsDeleted = false;
                    objSalDCDetail.StatusBy = 1;
                    objSalDCDetail.StatusID = 1;
                    lstSalDCDetail.Add(objSalDCDetail);

                    OtherDigits++;
                }

                using (TransactionScope transaction = new TransactionScope())
                {
                    /////////////////////// Start FDOMaster and FDO Detail Table Update /////////////////////////////

                    foreach (SalDCDetail sdcd in DCDetailList)
                    {
                        SalFDOMaster objSalFDOMaster = GenericFactory_EF_FDOMaster.FindBy(m => m.FDOMasterID == sdcd.FDOID).FirstOrDefault();
                        objSalFDOMaster.IsDCCompleted = true;
                        GenericFactory_EF_FDOMaster.Update(objSalFDOMaster);
                        GenericFactory_EF_FDOMaster.Save();

                        List<SalFDODetail> objSalFDODetail = GenericFactory_EF_FDODetail.FindBy(m => m.FDOMasterID == sdcd.FDOID).ToList();
                        foreach (SalFDODetail sfdod in objSalFDODetail)
                        {
                            sfdod.IsDCCompleted = true;
                            GenericFactory_EF_FDODetail.Update(sfdod);
                            GenericFactory_EF_FDODetail.Save();
                        }
                    }

                    /////////////////////// End FDOMaster and FDO Detail Table Update /////////////////////////////



                    /////////////////////// Start Stock Table Update /////////////////////////////
                    //#region Stock Hit
                    //IEnumerable<SalFDODetail> objFDODetail = null;
                    ////string spQuery = string.Empty;
                    //try
                    //{
                    //    objFDODetail = GenericFactory_EF_FDODetail.GetAll().Select(m => new
                    //    SalFDODetail
                    //    {                            
                    //        ItemID = m.ItemID,
                    //        BatchID = m.BatchID,
                    //        LotID = m.LotID,
                    //        GradeID = m.GradeID,
                    //        QuantitYds = m.QuantitYds,
                    //        FDOMasterID = m.FDOMasterID,
                    //        CompanyID = m.CompanyID                            
                    //    }).
                    //    Where(m => m.FDOMasterID == DCInfo.FDOID).ToList();
                    //}
                    //catch (Exception e)
                    //{
                    //    e.ToString();
                    //}

                    //List<InvStockMaster> objInvStockMaster = GFactory_EF_InvStockMaster.FindBy(m => m.DepartmentID == objSalFDOMaster.DepartmentID).ToList();

                    //foreach (SalFDODetail sfdod in objFDODetail)
                    //{
                    //    InvStockMaster objStockMaster = new InvStockMaster();

                    //    var ItemBatchFilter = objInvStockMaster.Where(x => x.ItemID == sfdod.ItemID
                    //                           && sfdod.BatchID == null ? true : x.BatchID == sfdod.BatchID).ToArray();

                    //    var TransitBatch2 = ItemBatchFilter.Where(x => sfdod.LotID == null ? true : x.LotID == sfdod.LotID).ToArray();

                    //    objStockMaster = ItemBatchFilter.Where(x => sfdod.GradeID == null ? true : x.GradeID == sfdod.GradeID).FirstOrDefault();                                                                                                      

                    //    objStockMaster.IssueQty = objStockMaster.IssueQty + sfdod.QuantitYds;
                    //    GFactory_EF_InvStockMaster.Update(objStockMaster);
                    //    GFactory_EF_InvStockMaster.Save();

                    //     ///////////////////////////////// detail insert /////////////////////////////////////

                    //    InvStockDetail objInvStockDetail = new InvStockDetail();
                    //    objInvStockDetail.StockID = objStockMaster.StockID;
                    //    objInvStockDetail.DepartmentID = objSalFDOMaster.DepartmentID;
                    //    objInvStockDetail.TransactionID = objSalFDOMaster.FDOMasterID;
                    //    objInvStockDetail.TransactionTypeID = objcmnParam.tTypeId??0;
                    //    objInvStockDetail.StockDate = DateTime.Now;
                    //    objInvStockDetail.ItemID = sfdod.ItemID;

                    //    objInvStockDetail.ItemTypeID = 1;
                    //    objInvStockDetail.LotID = sfdod.LotID == null ? 0 : (int)sfdod.LotID;
                    //    objInvStockDetail.BatchID = sfdod.BatchID == null ? 0 : (int)sfdod.BatchID;
                    //    objInvStockDetail.GradeID = sfdod.GradeID == null ? 0 : (int)sfdod.GradeID;
                    //    objInvStockDetail.LotNo = "";
                    //    objInvStockDetail.SupplierID = 0;
                    //    objInvStockDetail.ReceiveQty = 0;
                    //    objInvStockDetail.ReceiveValue = 0;
                    //    objInvStockDetail.ReceiveRate = 0;
                    //    objInvStockDetail.IssueQty = sfdod.QuantitYds;
                    //    objInvStockDetail.UOMID = 0;
                    //    objInvStockDetail.IssueRate = 0;
                    //    objInvStockDetail.IssueValue = 0;
                    //    objInvStockDetail.IsActive = true;
                    //    objInvStockDetail.IssueRate = 0;
                    //    objInvStockDetail.CompanyID = sfdod.CompanyID;
                    //    objInvStockDetail.CreateBy = objSalFDOMaster.CreateBy;
                    //    objInvStockDetail.CreateOn = DateTime.Now;
                    //    objInvStockDetail.CreatePc =  HostService.GetIP();

                    //    GFactory_EF_InvStockDetail.Insert(objInvStockDetail);
                    //    GFactory_EF_InvStockDetail.Save();

                    //}

                    //#endregion Stock Hit
                    /////////////////////// End Stock Table Update //////////////////////////////

                    /////////////////////// Stock Transit Table Data Delete /////////////////////
                    //#region Stock Transit Hit

                    //foreach (SalFDODetail sfdod in objFDODetail)
                    //{
                    //    List<InvStockTransit> objInvStockTransit =
                    //    GFactory_EF_InvStockTransit.FindBy(m => m.DepartmentID == objSalFDOMaster.DepartmentID
                    //                                       //&& m.LotID == sfdod.LotID && m.BatchID == sfdod.BatchID && m.GradeID == sfdod.GradeID
                    //                                       && m.ItemID == sfdod.ItemID && m.CompanyID == sfdod.CompanyID
                    //                                       && m.TransactionTypeID == objSalFDOMaster.FDOTypeID
                    //                                       && m.TransactionID == sfdod.FDOMasterID).ToList();

                    //    GFactory_EF_InvStockTransit.DeleteList(objInvStockTransit);
                    //    GFactory_EF_InvStockTransit.Save();
                    //}

                    //#endregion End Stock Transit Hit
                    /////////////////////// End Stock Transit Table Data Delete /////////////////////


                    GenericFactory_EF_DCMaster.Insert(DCInfo);
                    GenericFactory_EF_DCMaster.Save();

                    GenericFactory_EF_DCMaster.updateMaxID("SalDCMaster", Convert.ToInt64(NextId));

                    GenericFactory_EF_DCMaster.updateCustomCode(objcmnParam.menuId, DateTime.Now, DCInfo.CompanyID, 1, 1);
                    
                    GenericFactory_EF_DCDetail.InsertList(lstSalDCDetail);
                    GenericFactory_EF_DCDetail.Save();
                    GenericFactory_EF_DCDetail.updateMaxID("SalDCDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                    transaction.Complete();
                }
                result = customCode;
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            finally
            {

            }
            return result;
        }
        //This method is using for report.
        public IEnumerable<SalDCMaster> GetDCMaster(int userID, int companyID)
        {
            GenericFactory_EF_DCMaster = new SalDCMaster_EF();
            IEnumerable<SalDCMaster> objDCMaster = null;
            //string spQuery = string.Empty;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objDCMaster = _ctxCmn.SalDCMasters.Where(m => m.CompanyID == companyID).OrderByDescending(m => m.DCID).ToList()
                        .Select(m => new SalDCMaster
                        {
                            DCID = m.DCID,
                            DCNo = m.DCNo,
                            DCDate = m.DCDate,
                            CompanyID = m.CompanyID
                        })
                    .ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objDCMaster;
        }

        public IEnumerable<vmSalDCMasterDetail> GetDCMaster(vmCmnParameters cmnParam, out int recordsTotal)
        {
            GenericFactory_EF_DCMaster = new SalDCMaster_EF();
            IEnumerable<vmSalDCMasterDetail> objDCMaster = null;
            IEnumerable<vmSalDCMasterDetail> objDCMasterWithoutPage = null;
            List<CmnUserWiseCompany> whichCompanies = null;
            recordsTotal = 0;
            //string spQuery = string.Empty;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                whichCompanies = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == cmnParam.loggeduser && m.IsDeleted == false).ToList().
                   Select(m => new CmnUserWiseCompany
                   {
                       CompanyID = m.CompanyID,
                      // CreatePc = m.CreatePc
                   }).ToList();

                List<SalDCMaster> SalDCMaster = new List<SalDCMaster>();
                foreach (CmnUserWiseCompany u in whichCompanies)
                {
                    List<SalDCMaster> AddToList = new List<SalDCMaster>();
                    AddToList = _ctxCmn.SalDCMasters.Where(m => m.CompanyID == u.CompanyID && m.IsDeleted == false && m.IsActive == true).ToList();


                    if (AddToList != null && AddToList.Count > 0)
                    {
                        SalDCMaster.AddRange(AddToList);
                    }
                }

                List<SalDCDetail> SalDCDetail = new List<SalDCDetail>();
                foreach (CmnUserWiseCompany u in whichCompanies)
                {
                    List<SalDCDetail> AddDetailToList = new List<SalDCDetail>();
                    AddDetailToList = _ctxCmn.SalDCDetails.Where(m => m.CompanyID == u.CompanyID && m.IsDeleted == false).ToList();


                    if (AddDetailToList != null && AddDetailToList.Count > 0)
                    {
                        SalDCDetail.AddRange(AddDetailToList);
                    }
                }


                try
                {
                    //objDCMasterWithoutPage = SalDCMaster.Select(m => new SalDCMaster
                    //{
                    //    DCID = m.DCID,
                    //    DCNo = m.DCNo,
                    //    DCDate = m.DCDate,
                    //    CompanyID = m.CompanyID
                    //}).ToList();



                    objDCMasterWithoutPage = (from dcm in SalDCMaster
                                       join dcd in _ctxCmn.SalDCDetails on dcm.DCID equals dcd.DCID
                                       select new
                                       {
                                           DCID = dcm.DCID,
                                           DCNo = dcm.DCNo,
                                           DCDate = dcm.DCDate,
                                           QuantityYds = dcd.QuantityYds == null ? 0 : dcd.QuantityYds
                                       }).GroupBy(s => new { s.DCID }).Select(x => new vmSalDCMasterDetail
                                       {
                                           DCID = x.FirstOrDefault().DCID,
                                           DCNo = x.FirstOrDefault().DCNo,
                                           DCDate = x.FirstOrDefault().DCDate,
                                           QuantityYds = x.Sum(p => p.QuantityYds)
                                       }).ToList();



                    objDCMaster = objDCMasterWithoutPage.OrderByDescending(x => x.DCID).Skip(cmnParam.pageNumber).
                                  Take(cmnParam.pageSize).ToList();
                    recordsTotal = objDCMasterWithoutPage.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objDCMaster;
        }

        public IEnumerable<CmnCompany> GetDCCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_Company = new CmnCompany_EF();
            IEnumerable<CmnCompany> objCompany = null;
            try
            {
                objCompany = GenericFactory_EF_Company.GetAll().Select(m => new CmnCompany
                {
                    CompanyID = m.CompanyID,
                    CompanyName = m.CompanyName
                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCompany;
        }

        public IEnumerable<vmSalFDODetail> GetFDOQty(int id)
        {
            GenericFactory_EF_FDOMaster = new SalFDOMaster_EF();
            GenericFactory_EF_FDODetail = new SalFDODetail_EF();
            IEnumerable<vmSalFDODetail> objFD = null;
            try
            {
                var SalFDOMaster = GenericFactory_EF_FDOMaster.GetAll();
                var SalFDODetail = GenericFactory_EF_FDODetail.GetAll();

                objFD = (from FM in SalFDOMaster
                         join FD in SalFDODetail on FM.FDOMasterID equals FD.FDOMasterID
                         where FM.FDOMasterID == id
                         orderby FM.FDOMasterID descending
                         select new
                         {
                             FDOMasterID = FM.FDOMasterID,
                             QuantitYds = FD.QuantitYds,
                             TruckNo = FM.TruckNo,
                             RollNo = FD.RollNo

                         }).GroupBy(s => new { s.FDOMasterID, s.TruckNo})
                          .Select(j => new vmSalFDODetail
                          {
                              FDOMasterID = (int)j.Key.FDOMasterID,
                              TruckNo = j.Key.TruckNo,
                              RollNo = j.Sum(s => s.RollNo),
                              QuantitYds = j.Sum(s => s.QuantitYds)
                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFD;
        }
        public IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging)
        {
            //   GenericFactory_EF_Bank = new CmnBank_EF();
            List<CmnBank> AllDCBank = null;
            IEnumerable<CmnBank> objBank = null;

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                AllDCBank = _ctxCmn.CmnBanks.Where(m => m.IsDCBank == true).ToList();
            }


            try
            {
                objBank = AllDCBank.Select(m => new CmnBank { BankID = m.BankID, BankName = m.BankName }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objBank;
        }

        public IEnumerable<vmSalDCMasterDetail> GetDCDailyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmSalDCMasterDetail> objDCInfoDaily = null;

            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    var SalDCMasters = _ctxCmn.SalDCMasters.Where(m => m.IsActive == true && m.IsDeleted == false
                                        && m.DCDate == DateTime.Today).ToList();

                    objDCInfoDaily = (from dcm in SalDCMasters.Where(m => m.IsActive == true && m.IsDeleted == false 
                                      && m.DCDate == DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy hh:mm:ss tt"))) //Convert.ToDateTime("2016-07-22 00:00:00.000") )

                                      join dcd in _ctxCmn.SalDCDetails on dcm.DCID equals dcd.DCID
                                      select new
                                      {
                                          DCDate = dcm.DCDate,
                                          QuantityYds = dcd.QuantityYds == null ? 0 : dcd.QuantityYds
                                      }).GroupBy(x => x.DCDate.Day).Select(x => new vmSalDCMasterDetail
                                      {
                                          NoOfDC = SalDCMasters.Count(),
                                          QuantityYds = x.Sum(p => p.QuantityYds)

                                      }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objDCInfoDaily;
        }
        public IEnumerable<vmSalDCMasterDetail> GetDCMonthlyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmSalDCMasterDetail> objDCInfoMontly = null;

            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    var SalDCMasters = _ctxCmn.SalDCMasters.Where(m => m.IsActive == true && m.IsDeleted == false 
                                        && m.DCDate.Month == DateTime.Now.Month && m.DCDate.Year == DateTime.Now.Year).ToList();

                    objDCInfoMontly = (from dcm in SalDCMasters
                                       join dcd in _ctxCmn.SalDCDetails on dcm.DCID equals dcd.DCID
                                        select new
                                        {
                                            DCID = dcm.DCID,
                                            DCDate = dcm.DCDate,
                                            QuantityYds = dcd.QuantityYds == null ? 0 : dcd.QuantityYds
                                        }).GroupBy(s => new { s.DCDate.Month }).Select(x => new vmSalDCMasterDetail
                                        {
                                            //NoOfDC = x.Count(),
                                            NoOfDC = SalDCMasters.Count(),
                                            QuantityYds = x.Sum(p => p.QuantityYds),
                                        }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objDCInfoMontly;
        }
    }
}
