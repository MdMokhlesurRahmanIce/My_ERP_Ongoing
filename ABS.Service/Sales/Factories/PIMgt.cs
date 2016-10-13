using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Sales.Interfaces;
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
    public class PIMgt : iPIMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_PICompany = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_PICombo = null;
        private iGenericFactory_EF<SalIncoterm> GenericFactory_EF_SalIncoterm = null;
        private iGenericFactory_EF<SalBookingMaster> GenericFactory_EF_SalBookingMaster = null;
        private iGenericFactory_EF<SalPIMaster> GenericFactory_EF_SalPIMaster;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_SampleNo = null;
        private iGenericFactory_EF<SalPIDetail> GenericFactory_EF_SalPIDetail = null;
        private iGenericFactory_EF<CmnBank> GFactory_EF_CmnBank = null;
        private iGenericFactory_EF<CmnBankBranch> GFactory_EF_CmnBankBranch = null;
        private iGenericFactory_EF<CmnBankAdvising> GFactory_EF_CmnBankAdvising = null;

        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactory_EF_CmnUserWiseCompany = null;
        private iGenericFactory<vmPIDetail> GenericFactory_SalPIDetailForSP = null;

        public List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging)
        {
            List<vmCmnUser> objPIBuyer = null;
            try
            {
                //**************************Paging Implemented******************************
                using (_ctxCmn = new ERP_Entities())
                {
                    objPIBuyer = (from x in _ctxCmn.CmnUsers
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
                               .Where(m => m.UserTypeID == 2 && m.IsDeleted == false && m.UserFullName != null && m.UserFullName != "")
                               .OrderBy(p => p.UserFullName).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIBuyer;
        }

        /// No CompanyID Provided
        public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        {
            // GenericFactory_EF_PIBuyer = new CmnUser_EF();
            List<CmnUser> objPISalesPerson = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objPISalesPerson = _ctxCmn.CmnUsers.Where(m => m.UserTypeID == 1 && m.IsDeleted == false).ToList()
                                        .Select(m => new CmnUser { UserID = m.UserID, UserFullName = m.UserFullName, UserTypeID = m.UserTypeID, IsDeleted = m.IsDeleted }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objPISalesPerson;
        }

        /// No CompanyID Provided
        public IEnumerable<CmnCompany> GetPICompany(int userID)
        {
            GenericFactory_EF_PICompany = new CmnCompany_EF();
            GenericFactory_EF_CmnUserWiseCompany = new CmnUserWiseCompany_EF();

            IEnumerable<CmnCompany> objPICompany = null;
            IEnumerable<CmnUserWiseCompany> objCmnUserWCompany = null;
            try
            {
                objCmnUserWCompany = GenericFactory_EF_CmnUserWiseCompany.GetAll().Where(m => m.UserID == userID && m.IsDeleted == false).ToList();
                objPICompany = (from cmnUsrWsCom in objCmnUserWCompany
                                join company in GenericFactory_EF_PICompany.GetAll() on cmnUsrWsCom.CompanyID equals company.CompanyID
                                select new { CompanyID = company.CompanyID, CompanyName = company.CompanyName }
                                ).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPICompany;
        }

        /// No CompanyID Provided
        //public IEnumerable<SalPIDetail> GetPIDetailsByActivePI(Int64 activePI)
        //{
        //    GenericFactory_EF_SalPIDetail = new SalPIDetail_EF();
        //    IEnumerable<SalPIDetail> objPIDetails = null;
        //    try
        //    {
        //        objPIDetails = GenericFactory_EF_SalPIDetail.GetAll().Select(m => new SalPIDetail
        //        {
        //            PIID = m.PIID,
        //            IsActive = m.IsActive,
        //            PIDetailID = m.PIDetailID,
        //            ItemID = m.ItemID,
        //            Description = m.Description,
        //            CompanyID = m.CompanyID,
        //            CuttableWidth = m.CuttableWidth,
        //            BuyerStyle = m.BuyerStyle,
        //            Quantity = m.Quantity,
        //            UnitPrice = m.UnitPrice,
        //            Amount = m.Amount,
        //            CreateBy = m.CreateBy,
        //            IsDeleted = m.IsDeleted

        //        }).Where(m => m.PIID == activePI && m.IsDeleted == false).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPIDetails;
        //}

        /// No CompanyID Provided
        public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI)
        {

            GenericFactory_SalPIDetailForSP = new vmPIDetail_GF();

            IEnumerable<vmPIDetail> objPIDetails = null;

            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                // ht.Add("CompanyID", companyId);
                ht.Add("PIID", activePI);
                //ht.Add("LoggedUser", 1);

                spQuery = "[GetPIDetailByActivePI]";
                objPIDetails = GenericFactory_SalPIDetailForSP.ExecuteQuery(spQuery, ht).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPIDetails;

            //GenericFactory_EF_SalPIDetail = new SalPIDetail_EF();
            //GenericFactory_EF_SampleNo = new CmnItemMaster_EF();

            //IEnumerable<vmPIDetail> objPIDetails = null;
            //try
            //{
            //    objPIDetails = (from details in GenericFactory_EF_SalPIDetail.GetAll().Where(m => m.PIID == activePI && m.IsDeleted == false)


            //                    join itemMaster in GenericFactory_EF_SampleNo.GetAll() on details.ItemID equals itemMaster.ItemID
            //                    ////
            //                    //join warp in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WarpYarnID equals warp.YarnID into warpYarn
            //                    //from yWarpYarn in warpYarn
            //                    //join weft in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WeftYarnID equals weft.YarnID into weftYarn
            //                    //from xWeftYarn in weftYarn
            //                    ////
            //                    select new
            //                    {
            //                        PIDetailID = details.PIDetailID,
            //                        PIID = details.PIID,
            //                        IsActive = details.IsActive,
            //                        BuyerStyle = details.BuyerStyle,
            //                        CuttableWidth = details.CuttableWidth,
            //                        ItemConstructionTypeID = details.ItemConstructionTypeID,
            //                        Quantity = details.Quantity == null ? 0.00m : details.Quantity,
            //                        UnitPrice = details.UnitPrice == null ? 0.00m : details.UnitPrice,
            //                        Amount = details.Amount == null ? 0.00m : details.Amount,
            //                        ItemID = details.ItemID,
            //                        //Description = details.Description,
            //                        ItemName = itemMaster.ItemName,
            //                        ArticleNo = itemMaster.ArticleNo,
            //                        Description = itemMaster.Description, //"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + itemMaster.EPI.ToString() + "x" + itemMaster.PPI.ToString(),//itemMaster.Weave,
            //                        Construction = itemMaster.Note,
            //                        ExRate = 0,
            //                        CompanyID = details.CompanyID,
            //                        IsDeleted = details.IsDeleted

            //                    }).ToList().Select(x => new vmPIDetail
            //                    {
            //                        PIDetailID = x.PIDetailID,
            //                        PIID = x.PIID,
            //                        IsActive = x.IsActive,
            //                        BuyerStyle = x.BuyerStyle,
            //                        CuttableWidth = x.CuttableWidth,
            //                        ItemConstructionTypeID = x.ItemConstructionTypeID,
            //                        Quantity = x.Quantity,
            //                        UnitPrice = x.UnitPrice,
            //                        Amount = x.Amount,
            //                        ItemID = x.ItemID,
            //                        //Description = x.Description,
            //                        ItemName = x.ItemName,
            //                        ArticleNo = x.ArticleNo,
            //                        Description = x.Description,
            //                        //Construction = GetConstruction(x.ItemID),
            //                        Construction = x.Construction,
            //                        ExRate = x.ExRate,
            //                        CompanyID = x.CompanyID,
            //                        IsDeleted = x.IsDeleted
            //                    }).ToList();
            //}
            //catch (Exception e)
            //{
            //    e.ToString();
            //}

            //return objPIDetails;
        }

        /// No CompanyID Provided
        public IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging) // Getting all item groups
        {
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            GenericFactory_EF_SampleNo = new CmnItemMaster_EF();

            IEnumerable<vmItemGroup> objPISampleNo = null;
            IEnumerable<CmnItemMaster> cmnItemGroupID = null;
            try
            {
                //objPISampleNo = GenericFactory_EF_SampleNo.GetAll().Where(x=>x.ItemTypeID==1).GroupBy(x => x.UniqueCode).Select(o => new CmnItemMaster { UniqueCode = o.Key }).ToList();
                cmnItemGroupID = GenericFactory_EF_SampleNo.GetAll().Where(x => x.ItemTypeID == 1 && x.IsDeleted == false).GroupBy(x => x.ItemGroupID).
                    Select(o => new CmnItemMaster { ItemGroupID = o.Key }).ToList();
                //.Where(x => x.ItemTypeID == 1).GroupBy(x => x.ItemGroupID).Select(o => new vmItemGroup { ItemGroupID = o.Key }).ToList();
                objPISampleNo = (from groupItm in GenericFactory_EF_ItemGroup.GetAll()
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
            return objPISampleNo;
        }
        /// CompanyID Provided but Not in Use // // Getting Item List for modal by group id
        public IEnumerable<vmItem> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal)
        {
            int itemGroupId = Convert.ToInt32(groupId);
            IEnumerable<vmItem> objPIItemMaster = null;
            IEnumerable<vmItem> objPIItemMasterWithoutPaging = null;

            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objPIItemMasterWithoutPaging = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_SampleNo.GetAll()

                                                    join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID  //GFactory_EF_CmnItemColor.GetAll()                                                 
                                                    select new
                                                    {
                                                        ItemID = item.ItemID,
                                                        ItemName = item.ItemName,
                                                        ItemSizeID = item.ItemSizeID,
                                                        UniqueCode = item.UniqueCode,
                                                        ArticleNo = item.ArticleNo,
                                                        CuttableWidth = item.CuttableWidth,
                                                        WeightPerUnit = item.WeightPerUnit,
                                                        CompanyID = item.CompanyID,
                                                        Weave = item.Weave,
                                                        ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
                                                        ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
                                                        Construction = item.Note,
                                                        Description = item.Description,
                                                        Width = 0.00M,
                                                        ItemTypeID = item.ItemTypeID,
                                                        ItemGroupID = item.ItemGroupID,
                                                        IsDeleted = item.IsDeleted

                                                    }).ToList()
                                        .Select(x => new vmItem
                                        {
                                            ItemID = x.ItemID,
                                            ItemName = x.ItemName,
                                            ItemSizeID = x.ItemSizeID,
                                            UniqueCode = x.UniqueCode,
                                            ArticleNo = x.ArticleNo,
                                            CuttableWidth = x.CuttableWidth,
                                            WeightPerUnit = x.WeightPerUnit,
                                            CompanyID = x.CompanyID,
                                            Weave = x.Weave,
                                            ItemColorID = x.ItemColorID,
                                            ColorName = x.ColorName,
                                            Construction = x.Construction,
                                            Description = x.Description,
                                            Width = x.Width,
                                            ItemTypeID = x.ItemTypeID,
                                            ItemGroupID = x.ItemGroupID,
                                            IsDeleted = x.IsDeleted
                                        })
                                        .Where(m => m.ItemGroupID == itemGroupId && m.ItemTypeID == 1 && m.IsDeleted == false).ToList();

                    objPIItemMaster = objPIItemMasterWithoutPaging.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objPIItemMasterWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIItemMaster;
        }
        public IEnumerable<vmItem> GetBookingDetailByID(vmCmnParameters objcmnParam, string groupId)
        {
            int itemGroupId = Convert.ToInt32(groupId);
            IEnumerable<vmItem> objPIItemMaster = null;

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objPIItemMaster = (from sbm in _ctxCmn.SalBookingMasters
                                       join sbd in _ctxCmn.SalBookingDetails on sbm.BookingID equals sbd.BookingID
                                       join item in _ctxCmn.CmnItemMasters on sbd.ItemID equals item.ItemID
                                       select new
                                       {
                                           BookingID = sbm.BookingID,
                                           BookingDetailID = sbd.BookingDetailID,
                                           ItemID = item.ItemID,
                                           ItemName = item.ItemName,
                                           UniqueCode = item.UniqueCode,
                                           ArticleNo = item.ArticleNo,
                                           CuttableWidth = item.CuttableWidth,
                                           WeightPerUnit = item.WeightPerUnit,
                                           CompanyID = item.CompanyID,
                                           Construction = item.Note,
                                           Description = item.Description,
                                           Quantity = sbd.Quantity,
                                           IsDeleted = sbd.IsDeleted // from salbooking detail
                                       }).ToList()
                                        .Select(x => new vmItem
                                        {
                                            BookingID = x.BookingID,
                                            BookingDetailID = x.BookingDetailID,
                                            ItemID = x.ItemID,
                                            ItemName = x.ItemName,
                                            UniqueCode = x.UniqueCode,
                                            ArticleNo = x.ArticleNo,
                                            CuttableWidth = x.CuttableWidth,
                                            WeightPerUnit = x.WeightPerUnit,
                                            CompanyID = x.CompanyID,
                                            Construction = x.Construction,
                                            Description = x.Description,
                                            Quantity = x.Quantity,
                                            Amount = 0.00M,
                                            ExRate = 0.00M,
                                            IsDeleted = x.IsDeleted
                                        })
                                        .Where(m => m.BookingID == itemGroupId && m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIItemMaster;
        }
        public IEnumerable<vmPIMaster> GetBankAdvisingListByCompanyID(int Id)
        {
            GFactory_EF_CmnBank = new CmnBank_EF();
            GFactory_EF_CmnBankAdvising = new CmnBankAdvising_EF();

            IEnumerable<vmPIMaster> objCmnBank = null;
            try
            {
                objCmnBank = (from bnkAdvising in GFactory_EF_CmnBankAdvising.GetAll().Where(badvs => badvs.CompanyID == Id && badvs.IsDeleted == false)
                              join bnk in GFactory_EF_CmnBank.GetAll() on bnkAdvising.BankID equals bnk.BankID
                              select new
                              {
                                  BankID = bnk.BankID,
                                  BankName = bnk.BankName,
                                  BankShortName = bnk.BankShortName,
                                  CompanyID = bnk.CompanyID,
                                  CustomCode = bnk.CustomCode,
                                  IsDefaultBankAdvising = bnkAdvising.IsDefault

                              }).ToList().Select(x => new vmPIMaster
                              {
                                  BankID = x.BankID,
                                  BankName = x.BankName,
                                  BankShortName = x.BankShortName,
                                  CompanyID = x.CompanyID,
                                  CustomCode = x.CustomCode,
                                  IsDefaultBankAdvising = x.IsDefaultBankAdvising

                              }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCmnBank;
        }
        public IEnumerable<vmPIMaster> GetBranchListByBankID(int Id)
        {
            GFactory_EF_CmnBankBranch = new CmnBankBranch_EF();
            IEnumerable<vmPIMaster> objCmnBankBranch = null;
            try
            {
                objCmnBankBranch = GFactory_EF_CmnBankBranch.GetAll().Select(m => new vmPIMaster { BankID = m.BankID, BranchID = m.BranchID, BranchName = m.BranchName, IsDeleted = m.IsDeleted, IsDefaultBankBranch = m.IsDefault }).Where(m => m.BankID == Id && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCmnBankBranch;
        }
        /// No CompanyID Provided
        public IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objPIShipment = null;
            try
            {
                objPIShipment = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "shipment" && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIShipment;
        }
        public IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objPIValidity = null;
            try
            {
                objPIValidity = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "validity" && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIValidity;
        }
        public IEnumerable<CmnCombo> GetPIStatus(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objPIStatus = null;
            try
            {
                objPIStatus = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo
                {
                    ComboID = m.ComboID,
                    ComboName = m.ComboName,
                    ComboType = m.ComboType,
                    IsDefault = m.IsDefault,
                    IsDeleted = m.IsDeleted
                }).Where(m => m.ComboType == "PIStatus" && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIStatus;
        }
        public IEnumerable<SalIncoterm> GetIncoterm(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_SalIncoterm = new SalIncoterm_EF();

            IEnumerable<SalIncoterm> objIncoterm = null;
            try
            {
                objIncoterm = GenericFactory_EF_SalIncoterm.GetAll().Select(m => new SalIncoterm
                {
                    IncoTermID = m.IncoTermID,
                    IncotermName = m.IncotermName,
                    IsDeleted = m.IsDeleted,
                    IsDefault = m.IsDefault
                }).Where(m => m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objIncoterm;
        }
        public IEnumerable<SalBookingMaster> GetBookingList(int buyerid, int companyID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();

            IEnumerable<SalBookingMaster> objBookingList = null;
            try
            {
                objBookingList = GenericFactory_EF_SalBookingMaster.GetAll().Select(m => new SalBookingMaster
                {
                    CompanyID = m.CompanyID,
                    BuyerID = m.BuyerID,
                    BookingID = m.BookingID,
                    BookingNo = m.BookingNo,
                    IsPICompleted = m.IsPICompleted,
                    IsDeleted = m.IsDeleted
                }).Where
                (
                    m => m.BuyerID == buyerid &&
                    m.CompanyID == companyID &&
                     m.IsPICompleted == false
                     && m.IsDeleted == false
                ).ToList().OrderByDescending(m => m.BookingID);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objBookingList;
        }
        public IEnumerable<CmnCombo> GetAcceptableQuantity(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objItemAcceptancePerc = null;
            try
            {
                objItemAcceptancePerc = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo
                {
                    ComboID = m.ComboID,
                    ComboName = m.ComboName,
                    ComboType = m.ComboType,
                    IsDefault = m.IsDefault,
                    IsDeleted = m.IsDeleted
                }).Where(m => m.ComboType == "ItemAcceptableQuantityAmountPercentage"
                        && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objItemAcceptancePerc;
        }
        public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objPISight = null;
            try
            {
                objPISight = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "sight" && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPISight;
        }
        public IEnumerable<CmnCombo> GetSalesItemConstructionType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_PICombo = new CmnCombo_EF();

            IEnumerable<CmnCombo> objConstructionType = null;
            try
            {
                objConstructionType = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo
                {
                    ComboID = m.ComboID,
                    ComboName = m.ComboName,
                    ComboType = m.ComboType,
                    IsDefault = m.IsDefault,
                    IsDeleted = m.IsDeleted
                }).Where(m => m.ComboType == "SalesItemConstructionType"
                        && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objConstructionType;
        }
        /// CompanyID Provided but Not in Use
        public IEnumerable<vmPIMaster> GetPIMasterByPIActive(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmPIMaster> objvmPIMaster = null;
            IEnumerable<vmPIMaster> objvmPIMasterWithOutPaging = null;
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

                    List<SalPIMaster> SalPIMaster = new List<SalPIMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalPIMaster> AddToList = new List<SalPIMaster>();
                        AddToList = (from hd in _ctxCmn.SalPIMasters select hd).Where(m => m.CompanyID == u.CompanyID && m.IsActive == true && m.IsDeleted == false).ToList();
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            SalPIMaster.AddRange(AddToList);
                        }
                    }

                    objvmPIMasterWithOutPaging = (from master in SalPIMaster //_ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false)
                                                  join comboSihgt in _ctxCmn.CmnComboes on master.SightID equals comboSihgt.ComboID
                                                  join comboShipment in _ctxCmn.CmnComboes on master.ShipmentID equals comboShipment.ComboID
                                                  join comboValidity in _ctxCmn.CmnComboes on master.ValidityID equals comboValidity.ComboID
                                                  join comboPIStatus in _ctxCmn.CmnComboes on master.StatusID equals comboPIStatus.ComboID
                                                  join comboAcceptance in _ctxCmn.CmnComboes on (int)master.AcceptanceID equals comboAcceptance.ComboID
                                                  join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
                                                  join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
                                                  join salesPerson in _ctxCmn.CmnUsers on master.EmployeeID equals salesPerson.UserID
                                                  join bnk in _ctxCmn.CmnBanks on master.AdvisingBankID equals bnk.BankID
                                                  join bnkBrnch in _ctxCmn.CmnBankBranches on master.BranchID equals bnkBrnch.BranchID
                                                  join inco in _ctxCmn.SalIncoterms on (int)master.IncotermID equals inco.IncoTermID
                                                  join sbm in _ctxCmn.SalBookingMasters on master.BookingID equals sbm.BookingID
                                                  select new
                                                  {
                                                      PIID = master.PIID,
                                                      IsActive = master.IsActive,
                                                      IsLcCompleted = master.IsLcCompleted,
                                                      IsHDOCompleted = master.IsHDOCompleted,
                                                      LCStatus = master.IsLcCompleted == true ? "Completed" : "Pending",
                                                      HDOStatus = master.IsHDOCompleted == true ? "Completed" : "Pending",
                                                      PINO = master.PINO,
                                                      PIDate = master.PIDate,
                                                      TransactionTypeID = master.TransactionTypeID,
                                                      SightID = master.SightID,
                                                      ShipmentID = master.ShipmentID,
                                                      ValidityID = master.ValidityID,
                                                      StatusID = master.StatusID,
                                                      AcceptanceID = master.AcceptanceID,
                                                      BuyerID = master.BuyerID,
                                                      EmployeeID = master.EmployeeID,
                                                      CompanyID = master.CompanyID,
                                                      NegoDay = master.NegoDay,
                                                      Remarks = master.Remarks,
                                                      Discount = master.Discount,
                                                      ODInterest = master.ODInterest,
                                                      CreateBy = master.CreateBy,
                                                      ComboNameShipment = comboShipment.ComboName,
                                                      ComboNameSight = comboSihgt.ComboName,
                                                      ComboNameValidity = comboValidity.ComboName,
                                                      comboNamePIStatus = comboPIStatus.ComboName,
                                                      ComboNameAcceptance = comboAcceptance.ComboName,
                                                      CompanyName = company.CompanyName,
                                                      BuyerFirstName = buyer.UserFullName,
                                                      SalesPersonFirstName = salesPerson.UserFullName,
                                                      BankID = master.AdvisingBankID,
                                                      BranchID = master.BranchID,
                                                      BranchName = bnkBrnch.BranchName,
                                                      BankName = bnk.BankName,
                                                      BankShortName = bnk.BankShortName,
                                                      CompanyIDBankAdvise = master.CompanyID,
                                                      IncotermID = master.IncotermID == null ? 0 : master.IncotermID,
                                                      IncotermName = inco.IncotermName == null ? "" : inco.IncotermName,
                                                      IncotermDescription = master.IncotermDescription == null ? "" : master.IncotermDescription,
                                                      BookingNo = sbm.BookingNo,
                                                      BookingID = master.BookingID

                                                  }).ToList().Select(x => new vmPIMaster
                                                  {
                                                      PIID = x.PIID,
                                                      IsActive = x.IsActive,
                                                      IsLcCompleted = x.IsLcCompleted,
                                                      IsDOCompleted = x.IsHDOCompleted,
                                                      LCStatus = x.LCStatus,
                                                      HDOStatus = x.HDOStatus,
                                                      PINO = x.PINO,
                                                      PIDate = x.PIDate,
                                                      TransactionTypeID = x.TransactionTypeID,
                                                      SightID = x.SightID,
                                                      ShipmentID = x.ShipmentID,
                                                      ValidityID = x.ValidityID,
                                                      StatusID = x.StatusID,
                                                      AcceptanceID = x.AcceptanceID,
                                                      ODInterest = x.ODInterest,
                                                      BookingID = x.BookingID,
                                                      BuyerID = x.BuyerID,
                                                      EmployeeID = x.EmployeeID,
                                                      CompanyID = x.CompanyID,
                                                      CreateBy = x.CreateBy,
                                                      NegoDay = x.NegoDay,
                                                      Remarks = x.Remarks,
                                                      Discount = x.Discount,
                                                      ComboNameShipment = x.ComboNameShipment,
                                                      ComboNameSight = x.ComboNameSight,
                                                      ComboNameValidity = x.ComboNameValidity,
                                                      comboNamePIStatus = x.comboNamePIStatus,
                                                      ComboNameAcceptance = x.ComboNameAcceptance,
                                                      CompanyName = x.CompanyName,
                                                      BuyerFirstName = x.BuyerFirstName,
                                                      SalesPersonFirstName = x.SalesPersonFirstName,
                                                      BankID = x.BankID,
                                                      BranchID = x.BranchID,
                                                      BranchName = x.BranchName,
                                                      BankName = x.BankName,
                                                      BankShortName = x.BankShortName,
                                                      CompanyIDBankAdvise = x.CompanyID,
                                                      IncotermID = x.IncotermID ?? 0,
                                                      IncotermName = x.IncotermName ?? "",
                                                      IncotermDescription = x.IncotermDescription ?? "",
                                                      BookingNo = x.BookingNo
                                                  }).ToList();

                    objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.PIID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

                    recordsTotal = objvmPIMasterWithOutPaging.Count();

                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objvmPIMaster;
        }
        /// No CompanyID Provided //// Using for report purpose
        public IEnumerable<vmPIMaster> GetPIMasterByPIActive()
        {
            IEnumerable<vmPIMaster> objvmPIMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objvmPIMaster = (from master in _ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false)
                                     join comboSihgt in _ctxCmn.CmnComboes on master.SightID equals comboSihgt.ComboID
                                     join comboShipment in _ctxCmn.CmnComboes on master.ShipmentID equals comboShipment.ComboID
                                     join comboValidity in _ctxCmn.CmnComboes on master.ValidityID equals comboValidity.ComboID
                                     join comboPIStatus in _ctxCmn.CmnComboes on master.StatusID equals comboPIStatus.ComboID
                                     join comboAcceptance in _ctxCmn.CmnComboes on (int)master.AcceptanceID equals comboAcceptance.ComboID
                                     join inco in _ctxCmn.SalIncoterms on (int)master.IncotermID equals inco.IncoTermID
                                     join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
                                     join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
                                     join salesPerson in _ctxCmn.CmnUsers on master.EmployeeID equals salesPerson.UserID
                                     join bnk in _ctxCmn.CmnBanks on master.AdvisingBankID equals bnk.BankID
                                     join bnkBrnch in _ctxCmn.CmnBankBranches on master.BranchID equals bnkBrnch.BranchID
                                     select new
                                     {
                                         PIID = master.PIID,
                                         IsActive = master.IsActive,
                                         PINO = master.PINO,
                                         PIDate = master.PIDate,
                                         TransactionTypeID = master.TransactionTypeID,
                                         SightID = master.SightID,
                                         ShipmentID = master.ShipmentID,
                                         ValidityID = master.ValidityID,
                                         StatusID = master.StatusID,
                                         IncotermID = master.IncotermID ?? 0,
                                         IncotermDescription = master.IncotermDescription,
                                         IncotermName = inco.IncotermName,
                                         AcceptanceID = master.AcceptanceID,
                                         BuyerID = master.BuyerID,
                                         EmployeeID = master.EmployeeID,
                                         CompanyID = master.CompanyID,
                                         NegoDay = master.NegoDay,
                                         Remarks = master.Remarks,
                                         Discount = master.Discount,

                                         ODInterest = master.ODInterest,
                                         CreateBy = master.CreateBy,
                                         ComboNameShipment = comboShipment.ComboName,
                                         ComboNameSight = comboSihgt.ComboName,
                                         ComboNameValidity = comboValidity.ComboName,
                                         comboPIStatus = comboPIStatus.ComboName,
                                         ComboNameAcceptance = comboAcceptance.ComboName,
                                         CompanyName = company.CompanyName,
                                         BuyerFirstName = buyer.UserFullName,
                                         SalesPersonFirstName = salesPerson.UserFullName,
                                         BankID = master.AdvisingBankID,
                                         BranchID = master.BranchID,
                                         BranchName = bnkBrnch.BranchName,
                                         BankName = bnk.BankName,
                                         BankShortName = bnk.BankShortName,
                                         CompanyIDBankAdvise = master.CompanyID

                                     }).ToList().Select(x => new vmPIMaster
                                     {
                                         PIID = x.PIID,
                                         IsActive = x.IsActive,
                                         PINO = x.PINO,
                                         PIDate = x.PIDate,
                                         TransactionTypeID = x.TransactionTypeID,
                                         SightID = x.SightID,
                                         ShipmentID = x.ShipmentID,
                                         ValidityID = x.ValidityID,
                                         AcceptanceID = x.AcceptanceID,

                                         ODInterest = x.ODInterest,
                                         BuyerID = x.BuyerID,
                                         EmployeeID = x.EmployeeID,
                                         CompanyID = x.CompanyID,
                                         CreateBy = x.CreateBy,
                                         NegoDay = x.NegoDay,
                                         Remarks = x.Remarks,
                                         Discount = x.Discount,
                                         ComboNameShipment = x.ComboNameShipment,
                                         ComboNameSight = x.ComboNameSight,
                                         ComboNameValidity = x.ComboNameValidity,
                                         comboNamePIStatus = x.comboPIStatus,
                                         ComboNameAcceptance = x.ComboNameAcceptance,
                                         CompanyName = x.CompanyName,
                                         BuyerFirstName = x.BuyerFirstName,
                                         SalesPersonFirstName = x.SalesPersonFirstName,
                                         IncotermID = x.IncotermID,
                                         IncotermName = x.IncotermName,
                                         IncotermDescription = x.IncotermDescription,
                                         BankID = x.BankID,
                                         BranchID = x.BranchID,
                                         BranchName = x.BranchName,
                                         BankName = x.BankName,
                                         BankShortName = x.BankShortName,
                                         CompanyIDBankAdvise = x.CompanyID
                                     }).Where(m => m.IsDeleted == false)
                                       .OrderBy(x => x.PIID)
                                       .ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objvmPIMaster;
        }
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public string SaveUpdatePIItemMasterNdetails(SalPIMaster itemMaster, List<SalPIDetail> itemDetails, int menuID)
        {
            GenericFactory_EF_SalPIMaster = new SalPIMaster_EF();
            GenericFactory_EF_SalPIDetail = new SalPIDetail_EF();
            GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();
            string result = "";
            if (itemMaster.PIID > 0)
            {
                //  ---- start create PI revise No -----// 
                string[] piNo = itemMaster.PINO.ToString().Split(new string[] { "-Revise-" }, StringSplitOptions.None);
                int reviseNo = 0;
                string newPiReviseNo = "";
                if (piNo.Length > 1)
                {
                    reviseNo = Convert.ToInt16(piNo[piNo.Length - 1]) + 1;
                    newPiReviseNo = piNo[0] + "-Revise-" + reviseNo.ToString();
                }
                else if (piNo.Length == 1)
                {
                    newPiReviseNo = itemMaster.PINO.ToString() + "-Revise-1".ToString();
                }

                //  ---- end create PI revise No -----// 

                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        //start new maxId
                        long NextId = Convert.ToInt64(GenericFactory_EF_SalPIMaster.getMaxID("SalPIMaster"));

                        long FirstDigit = 0;
                        long OtherDigits = 0;

                        long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalPIDetail.getMaxID("SalPIDetail"));
                        FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));


                        //end new maxId

                        // start for Update in SalPIMaster

                        Int64 lPIID = itemMaster.PIID;
                        IEnumerable<SalPIMaster> objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == lPIID && m.IsDeleted == false).ToList();
                        SalPIMaster lstSalPIMaster = new SalPIMaster();
                        foreach (SalPIMaster spl in objSalPIMaster)
                        {
                            spl.IsActive = false;
                            lstSalPIMaster = spl;
                        }

                        GenericFactory_EF_SalPIMaster.Update(lstSalPIMaster);
                        GenericFactory_EF_SalPIMaster.Save();

                        // end for Update in SalPIMaster

                        itemMaster.PIID = NextId;
                        itemMaster.PINO = newPiReviseNo;
                        itemMaster.CreateOn = DateTime.Now;
                        itemMaster.CreatePc = HostService.GetIP();
                        itemMaster.IsHDOCompleted = false;
                        itemMaster.IsLcCompleted = false;
                        itemMaster.BookingID = itemMaster.BookingID;

                        List<SalPIDetail> lstSalPIDetail = new List<SalPIDetail>();
                        foreach (SalPIDetail sdtl in itemDetails)
                        {
                            SalPIDetail objSalPIDetail = new SalPIDetail();
                            objSalPIDetail.PIDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objSalPIDetail.PIID = NextId;
                            objSalPIDetail.ItemID = sdtl.ItemID;
                            objSalPIDetail.CompanyID = itemMaster.CompanyID;
                            objSalPIDetail.UnitPrice = sdtl.UnitPrice;
                            objSalPIDetail.CuttableWidth = sdtl.CuttableWidth;
                            objSalPIDetail.BuyerStyle = sdtl.BuyerStyle;
                            objSalPIDetail.ItemConstructionTypeID = sdtl.ItemConstructionTypeID;
                            objSalPIDetail.Quantity = sdtl.Quantity;
                            objSalPIDetail.Amount = sdtl.Amount;
                            objSalPIDetail.Description = sdtl.Description;
                            objSalPIDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
                            objSalPIDetail.CreateOn = DateTime.Now;
                            objSalPIDetail.IsActive = true;
                            objSalPIDetail.CreatePc = HostService.GetIP();
                            // objSalPIDetail.IsCICompleted = false;
                            lstSalPIDetail.Add(objSalPIDetail);
                            OtherDigits++;
                        }
                        GenericFactory_EF_SalPIMaster.Insert(itemMaster);
                        GenericFactory_EF_SalPIMaster.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_SalPIMaster.updateMaxID("SalPIMaster", Convert.ToInt64(NextId));
                        GenericFactory_EF_SalPIDetail.InsertList(lstSalPIDetail);
                        GenericFactory_EF_SalPIDetail.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        transaction.Complete();
                        result = newPiReviseNo;
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
                        long NextId = Convert.ToInt64(GenericFactory_EF_SalPIMaster.getMaxID("SalPIMaster"));

                        long FirstDigit = 0;
                        long OtherDigits = 0;
                        long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalPIDetail.getMaxID("SalPIDetail"));
                        FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                        OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                        //..........END new maxId.........//


                        //......... START for custom code........... //
                        string customCode = "";

                        string CustomNo = customCode = GenericFactory_EF_SalPIMaster.getCustomCode(menuID, itemMaster.PIDate, itemMaster.CompanyID, 1, 1); // 1 for user id and 1 for db id
                        if (CustomNo != null)
                        {
                            customCode = CustomNo;
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }
                        //.........END for custom code............ //

                        string newPiNo = customCode;
                        itemMaster.PIID = NextId;
                        itemMaster.CreateOn = DateTime.Now;
                        itemMaster.CreatePc = HostService.GetIP();
                        itemMaster.PINO = newPiNo;
                        itemMaster.IsHDOCompleted = false;
                        itemMaster.IsLcCompleted = false;
                        itemMaster.BookingID = itemMaster.BookingID;
                        List<SalPIDetail> lstSalPIDetail = new List<SalPIDetail>();
                        foreach (SalPIDetail sdtl in itemDetails)
                        {
                            SalPIDetail objSalPIDetail = new SalPIDetail();
                            //objSalPIDetail.PIDetailID = nextDetailId;
                            objSalPIDetail.PIDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
                            objSalPIDetail.PIID = NextId;
                            objSalPIDetail.ItemID = sdtl.ItemID;
                            objSalPIDetail.CompanyID = itemMaster.CompanyID;
                            objSalPIDetail.UnitPrice = sdtl.UnitPrice;
                            objSalPIDetail.CuttableWidth = sdtl.CuttableWidth;
                            objSalPIDetail.BuyerStyle = sdtl.BuyerStyle;
                            objSalPIDetail.ItemConstructionTypeID = sdtl.ItemConstructionTypeID;
                            objSalPIDetail.Quantity = sdtl.Quantity;
                            objSalPIDetail.Amount = sdtl.Amount;
                            objSalPIDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
                            objSalPIDetail.CreateOn = DateTime.Now;
                            objSalPIDetail.IsActive = true;
                            objSalPIDetail.CreatePc = HostService.GetIP();
                            // objSalPIDetail.IsCICompleted = false;
                            lstSalPIDetail.Add(objSalPIDetail);
                            //nextDetailId++;
                            OtherDigits++;
                        }

                        GenericFactory_EF_SalPIMaster.Insert(itemMaster);
                        GenericFactory_EF_SalPIMaster.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_SalPIMaster.updateMaxID("SalPIMaster", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_SalPIMaster.updateCustomCode(menuID, DateTime.Now, itemMaster.CompanyID, 1, 1);

                        // List<SalPIDetail> lstSalPIDetail222 = new List<SalPIDetail>();
                        GenericFactory_EF_SalPIDetail.InsertList(lstSalPIDetail);
                        GenericFactory_EF_SalPIDetail.Save();
                        //............Update MaxID.................//
                        // GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(nextDetailId - 1));

                        GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));

                        ////// Updating Booking Table /////////////////////
                        SalBookingMaster objBookingMaster = GenericFactory_EF_SalBookingMaster.FindBy(m => m.BookingID == itemMaster.BookingID).FirstOrDefault();
                        objBookingMaster.IsPICompleted = true;
                        GenericFactory_EF_SalBookingMaster.Update(objBookingMaster);
                        GenericFactory_EF_SalBookingMaster.Save();
                        ////////////// End That /////////////////////////////


                        transaction.Complete();
                        result = newPiNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                    }
                }

            }
            return result;
        }
        public int DeleteSalPIMasterNSalPIDetail(int Id)
        {
            GenericFactory_EF_SalPIMaster = new SalPIMaster_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_SalPIMaster.Delete(m => m.PIID == Id);
                GenericFactory_EF_SalPIMaster.Save();
                result = -103;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return result;
        }
        public IEnumerable<vmPIDetail> GetPIDailyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmPIDetail> objPIInfoDaily = null;

            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    var SalPIMasters = _ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false && m.PIDate == DateTime.Today).ToList();

                    objPIInfoDaily = (from pim in SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false && m.PIDate == DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy hh:mm:ss tt"))) //Convert.ToDateTime("2016-07-22 00:00:00.000") )
                                      join pid in _ctxCmn.SalPIDetails on pim.PIID equals pid.PIID
                                      select new
                                      {
                                          PIID = pim.PIID,
                                          PIDate = pim.PIDate,
                                          Amount = pid.Amount == null ? 0 : pid.Amount
                                      }).ToList().GroupBy(x => x.PIDate).Select(x => new vmPIDetail
                                      {
                                          //PIID = x.First().PIID,
                                          NoOfPI = SalPIMasters.Count(),
                                          Amount = x.Sum(p => p.Amount)

                                      }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIInfoDaily;
        }
        public IEnumerable<vmPIDetail> GetPIMonthlyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmPIDetail> objPIInfoMontyly = null;

            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    var SalPIMasters = _ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false
                                        && m.PIDate.Month == DateTime.Now.Month).ToList();

                    objPIInfoMontyly = (from pim in SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false
                                        && m.PIDate.Month == DateTime.Now.Month)
                                        join pid in _ctxCmn.SalPIDetails on pim.PIID equals pid.PIID
                                        select new
                                        {
                                            PIID = pim.PIID,
                                            PIDate = pim.PIDate,
                                            Amount = pid.Amount == null ? 0 : pid.Amount
                                        }).GroupBy(x => x.PIDate.Month).Select(x => new vmPIDetail
                                        {
                                            NoOfPI = SalPIMasters.Count(),
                                            Amount = x.Sum(p => p.Amount)

                                        }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIInfoMontyly;
        }
        //public IEnumerable<vmItem> GetItemMasterById(string uniqueCode)
        //{
        //    int itemGroupId = Convert.ToInt32(uniqueCode);
        //    IEnumerable<vmItem> objPIItemMaster = null;
        //    try
        //    {
        //        objPIItemMaster = (from item in GenericFactory_EF_SampleNo.GetAll() 

        //                           join color in GFactory_EF_CmnItemColor.GetAll() on item.ItemColorID equals color.ItemColorID

        //                           //join warp in GenericFactory_EF_RndYarnCR.GetAll() on item.WarpYarnID equals warp.YarnID
        //                           //into warpYarn
        //                           //from yWarpYarn in warpYarn
        //                           //join weft in GenericFactory_EF_RndYarnCR.GetAll() on item.WeftYarnID equals weft.YarnID
        //                           //into weftYarn
        //                           //from xWeftYarn in weftYarn
        //                           select new
        //                           {
        //                               ItemID = item.ItemID,
        //                               ItemName = item.ItemName,
        //                               ItemSizeID = item.ItemSizeID,
        //                               UniqueCode = item.UniqueCode,
        //                               ArticleNo = item.ArticleNo,
        //                               CuttableWidth = item.CuttableWidth,
        //                               WeightPerUnit = item.WeightPerUnit,
        //                               Description = item.Description,
        //                               CompanyID = item.CompanyID,
        //                               Weave = item.Weave,
        //                               ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
        //                               ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
        //                               Construction = item.Description,//"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + item.EPI.ToString() + "x" + item.PPI.ToString(),//item.Weave,
        //                               Width = 0.00M,
        //                               ItemTypeID = item.ItemTypeID,
        //                               ItemGroupID = item.ItemGroupID,
        //                               IsDeleted = item.IsDeleted

        //                           })
        //                            .Select(x => new vmItem
        //                            {
        //                                ItemID = x.ItemID,
        //                                ItemName = x.ItemName,
        //                                ItemSizeID = x.ItemSizeID,
        //                                UniqueCode = x.UniqueCode,
        //                                ArticleNo = x.ArticleNo,
        //                                CuttableWidth = x.CuttableWidth,
        //                                WeightPerUnit = x.WeightPerUnit,
        //                                Description = x.Description,
        //                                CompanyID = x.CompanyID,
        //                                Weave = x.Weave,
        //                                ItemColorID = x.ItemColorID,
        //                                ColorName = x.ColorName,
        //                                Construction = x.Construction,
        //                                Width = x.Width,
        //                                ItemTypeID = x.ItemTypeID,
        //                                ItemGroupID = x.ItemGroupID,
        //                                IsDeleted = x.IsDeleted
        //                            }).Where(m => m.ItemGroupID == itemGroupId && m.ItemTypeID == 1 && m.IsDeleted == false).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPIItemMaster;
        //}

        //public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI)
        //{
        //    IEnumerable<vmPIDetail> objPIDetails = null;
        //    try
        //    {
        //        objPIDetails = (from details in GenericFactory_EF_SalPIDetail.GetAll().Where(m => m.PIID == activePI && m.IsDeleted == false)
        //                        join itemMaster in GenericFactory_EF_SampleNo.GetAll() on details.ItemID equals itemMaster.ItemID
        //                        //
        //                        join warp in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WarpYarnID equals warp.YarnID into warpYarn
        //                        from yWarpYarn in warpYarn
        //                        join weft in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WeftYarnID equals weft.YarnID into weftYarn
        //                        from xWeftYarn in weftYarn
        //                            //
        //                        select new
        //                        {
        //                            PIDetailID = details.PIDetailID,
        //                            PIID = details.PIID,
        //                            IsActive = details.IsActive,
        //                            BuyerStyle = details.BuyerStyle,
        //                            CuttableWidth = details.CuttableWidth,
        //                            Quantity = details.Quantity,
        //                            UnitPrice = details.UnitPrice,
        //                            Amount = details.Amount,
        //                            ItemID = details.ItemID,
        //                            Description = details.Description,
        //                            ItemName = itemMaster.ItemName,
        //                            Construction = "(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + itemMaster.EPI.ToString() + "x" + itemMaster.PPI.ToString(),//itemMaster.Weave,

        //                            ExRate = 0,
        //                            CompanyID = details.CompanyID,
        //                            IsDeleted = details.IsDeleted

        //                        }).ToList().Select(x => new vmPIDetail
        //                        {
        //                            PIDetailID = x.PIDetailID,
        //                            PIID = x.PIID,
        //                            IsActive = x.IsActive,
        //                            BuyerStyle = x.BuyerStyle,
        //                            CuttableWidth = x.CuttableWidth,
        //                            Quantity = x.Quantity,
        //                            UnitPrice = x.UnitPrice,
        //                            Amount = x.Amount,
        //                            ItemID = x.ItemID,
        //                            Description = x.Description,
        //                            ItemName = x.ItemName,
        //                            Construction = x.Construction,
        //                            ExRate = x.ExRate,
        //                            CompanyID = x.CompanyID,
        //                            IsDeleted = x.IsDeleted
        //                        }).ToList();


        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIDetails;
        //}

        /// No CompanyID Provided
        //private string GetConstruction(long ItemID)
        //{
        //    GFactory_EF_CmnItemColor = new CmnItemColor_EF();
        //    //// throw new NotImplementedException();
        //    //int ItemId = Convert.ToInt16(itemID);
        //    //string construction = "";
        //    //try
        //    //{
        //    //    string sql = string.Format("select [dbo].[CmnItemConstruction]({0})", ItemId);
        //    //    construction = _ctxCmn.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    //    return _ctxCmn.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    e.ToString();
        //    //}
        //    //return construction;
        //    return GFactory_EF_CmnItemColor.GetConstruction(ItemID);
        //}
    }
}
