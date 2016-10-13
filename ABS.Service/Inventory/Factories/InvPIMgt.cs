using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Inventory.Factories
{
    public class InvPIMgt : iInvPIMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnUser> GenericFactory_EF_PIBuyer;
        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_PICompany;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_PICombo;
        private iGenericFactory_EF<SalPIMaster> GenericFactory_EF_SalPIMaster;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_SampleNo;
        private iGenericFactory_EF<SalPIDetail> GenericFactory_EF_SalPIDetail;
        private iGenericFactory_EF<CmnBank> GFactory_EF_CmnBank;
        private iGenericFactory_EF<CmnBankBranch> GFactory_EF_CmnBankBranch;
        private iGenericFactory_EF<CmnBankAdvising> GFactory_EF_CmnBankAdvising;
        // private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode;
        private iGenericFactory_EF<CmnItemColor> GFactory_EF_CmnItemColor;
        private iGenericFactory_EF<RndYarnCR> GenericFactory_EF_RndYarnCR;

        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactory_EF_CmnUserWiseCompany;
        private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode;


        
        //public InvPIMgt()
        //{
        //    GenericFactory_EF_PIBuyer = new CmnUser_EF();
        //    GenericFactory_EF_PICompany = new CmnCompany_EF();
        //    GenericFactory_EF_PICombo = new CmnCombo_EF();
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

        public IEnumerable<InvRequisitionMaster> LoadSPRNO(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            _ctxCmn = new ERP_Entities();
            IEnumerable<InvRequisitionMaster> objSPRNO = null; 
            IEnumerable<InvRequisitionMaster> objSPRNOWithOutPaging = null; 
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objSPRNOWithOutPaging = (from master in _ctxCmn.InvRequisitionMasters.Where(m => m.IsDeleted == false) 
                                             select master).ToList().Select(m=>  new InvRequisitionMaster {RequisitionID=m.RequisitionID, RequisitionNo=m.RequisitionNo}).ToList(); 
                    objSPRNO = objSPRNOWithOutPaging.OrderByDescending(x => x.RequisitionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

                    recordsTotal = objSPRNOWithOutPaging.Count(); 
                } 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSPRNO;
        }
        //public IEnumerable<CmnBank> GetBankAdvisingList()
        //{
              //_ctxCmn = new ERP_Entities();
        //    IEnumerable<CmnBank> objCmnBank = null;
        //    try
        //    {
        //        objCmnBank = (from bnkAdvising in _ctxCmn.CmnBankAdvisings.Where(badvs => badvs.IsDeleted == false)
        //                      join bnk in _ctxCmn.CmnBanks on bnkAdvising.BankID equals bnk.BankID
        //                      select new
        //                      {
        //                          BankID = bnk.BankID,
        //                          BankName = bnk.BankName,
        //                          BankShortName = bnk.BankShortName,
        //                          CompanyID = bnk.CompanyID,
        //                          CustomCode = bnk.CustomCode,
        //                          IsDefaultBankAdvising = bnkAdvising.IsDefault

        //                      }).ToList().Select(x => new CmnBank  
        //                      {
        //                          BankID = x.BankID,
        //                          BankName = x.BankName,
        //                          BankShortName = x.BankShortName,
        //                          CompanyID = x.CompanyID,
        //                          CustomCode = x.CustomCode,
        //                          IsDefaultBankAdvising = x.IsDefaultBankAdvising

        //                      }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objCmnBank;
        //}



        //public IEnumerable<CmnBankBranch> GetBranchListByBankID(int Id)
        //{
        //    IEnumerable<CmnBankBranch> objCmnBankBranch = null;
        //    try
        //    {
        //        objCmnBankBranch = GFactory_EF_CmnBankBranch.GetAll().Select(m => new vmPIMaster { BankID = m.BankID, BranchID = m.BranchID, BranchName = m.BranchName, IsDeleted = m.IsDeleted, IsDefaultBankBranch = m.IsDefault }).Where(m => m.BankID == Id && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objCmnBankBranch;
        //}



        //public List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging)
        //{
              //_ctxCmn = new ERP_Entities();
        //    List<vmCmnUser> objPIBuyer = null;
        //    try
        //    {
        //        //**************************Paging Implemented******************************
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            objPIBuyer = (from x in _ctxCmn.CmnUsers
        //                          select new
        //                          {
        //                              UserID = x.UserID,
        //                              UserFullName = x.UserFullName,
        //                              UserTypeID = x.UserTypeID,
        //                              IsDeleted = x.IsDeleted

        //                          }).ToList().Select(m => new vmCmnUser
        //                          {
        //                              UserID = m.UserID,
        //                              UserFullName = m.UserFullName,
        //                              UserTypeID = m.UserTypeID,
        //                              IsDeleted = m.IsDeleted
        //                          })
        //                       .Where(m => m.UserTypeID == 2 && m.IsDeleted == false && m.UserFullName != null)

        //                       .OrderBy(p => p.UserID)
        //                       .Skip(pageNumber)
        //                       .Take(pageSize).ToList();
        //        }

        //        //objPIBuyer = GenericFactory_EF_PIBuyer.GetAll().Select(m => new CmnUser
        //        //{
        //        //    UserID = m.UserID,
        //        //    UserFullName = m.UserFullName,
        //        //    UserTypeID = m.UserTypeID,
        //        //    IsDeleted = m.IsDeleted
        //        //}).Where(m => m.UserTypeID == 2 && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIBuyer;
        //}

        //public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<CmnUser> objPISalesPerson = null;
        //    try
        //    {
        //        objPISalesPerson = GenericFactory_EF_PIBuyer.GetAll().Select(m => new CmnUser { UserID = m.UserID, UserFullName = m.UserFullName, UserTypeID = m.UserTypeID, IsDeleted = m.IsDeleted }).Where(m => m.UserTypeID == 1 && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPISalesPerson;
        //}

        //public IEnumerable<CmnCompany> GetPICompany(int userID)
        //{
        //    IEnumerable<CmnCompany> objPICompany = null;
        //    IEnumerable<CmnUserWiseCompany> objCmnUserWCompany = null;
        //    try
        //    {
        //        objCmnUserWCompany = GenericFactory_EF_CmnUserWiseCompany.GetAll().Where(m => m.UserID == userID && m.IsDeleted == false).ToList();
        //        objPICompany = (from cmnUsrWsCom in objCmnUserWCompany
        //                        join company in GenericFactory_EF_PICompany.GetAll() on cmnUsrWsCom.CompanyID equals company.CompanyID
        //                        select new { CompanyID = company.CompanyID, CompanyName = company.CompanyName }
        //                        ).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPICompany;
        //}

        //public IEnumerable<SalPIDetail> GetPIDetailsByActivePI(Int64 activePI)
        //{
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


        ////public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI)
        ////{
        ////    IEnumerable<vmPIDetail> objPIDetails = null;
        ////    try
        ////    {
        ////        objPIDetails = (from details in GenericFactory_EF_SalPIDetail.GetAll().Where(m => m.PIID == activePI && m.IsDeleted == false)
        ////                        join itemMaster in GenericFactory_EF_SampleNo.GetAll() on details.ItemID equals itemMaster.ItemID
        ////                        //
        ////                        join warp in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WarpYarnID equals warp.YarnID into warpYarn
        ////                        from yWarpYarn in warpYarn
        ////                        join weft in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WeftYarnID equals weft.YarnID into weftYarn
        ////                        from xWeftYarn in weftYarn
        ////                            //
        ////                        select new
        ////                        {
        ////                            PIDetailID = details.PIDetailID,
        ////                            PIID = details.PIID,
        ////                            IsActive = details.IsActive,
        ////                            BuyerStyle = details.BuyerStyle,
        ////                            CuttableWidth = details.CuttableWidth,
        ////                            Quantity = details.Quantity,
        ////                            UnitPrice = details.UnitPrice,
        ////                            Amount = details.Amount,
        ////                            ItemID = details.ItemID,
        ////                            Description = details.Description,
        ////                            ItemName = itemMaster.ItemName,
        ////                            Construction = "(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + itemMaster.EPI.ToString() + "x" + itemMaster.PPI.ToString(),//itemMaster.Weave,

        ////                            ExRate = 0,
        ////                            CompanyID = details.CompanyID,
        ////                            IsDeleted = details.IsDeleted

        ////                        }).ToList().Select(x => new vmPIDetail
        ////                        {
        ////                            PIDetailID = x.PIDetailID,
        ////                            PIID = x.PIID,
        ////                            IsActive = x.IsActive,
        ////                            BuyerStyle = x.BuyerStyle,
        ////                            CuttableWidth = x.CuttableWidth,
        ////                            Quantity = x.Quantity,
        ////                            UnitPrice = x.UnitPrice,
        ////                            Amount = x.Amount,
        ////                            ItemID = x.ItemID,
        ////                            Description = x.Description,
        ////                            ItemName = x.ItemName,
        ////                            Construction = x.Construction,
        ////                            ExRate = x.ExRate,
        ////                            CompanyID = x.CompanyID,
        ////                            IsDeleted = x.IsDeleted
        ////                        }).ToList();


        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }

        ////    return objPIDetails;
        ////}

        //public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(Int64 activePI)
        //{
        //    IEnumerable<vmPIDetail> objPIDetails = null;
        //    try
        //    {
        //        objPIDetails = (from details in GenericFactory_EF_SalPIDetail.GetAll().Where(m => m.PIID == activePI && m.IsDeleted == false)


        //                        join itemMaster in GenericFactory_EF_SampleNo.GetAll() on details.ItemID equals itemMaster.ItemID
        //                        ////
        //                        //join warp in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WarpYarnID equals warp.YarnID into warpYarn
        //                        //from yWarpYarn in warpYarn
        //                        //join weft in GenericFactory_EF_RndYarnCR.GetAll() on itemMaster.WeftYarnID equals weft.YarnID into weftYarn
        //                        //from xWeftYarn in weftYarn
        //                        ////
        //                        select new
        //                        {
        //                            PIDetailID = details.PIDetailID,
        //                            PIID = details.PIID,
        //                            IsActive = details.IsActive,
        //                            BuyerStyle = details.BuyerStyle,
        //                            CuttableWidth = details.CuttableWidth,
        //                            Quantity = details.Quantity == null ? 0.00m : details.Quantity,
        //                            UnitPrice = details.UnitPrice == null ? 0.00m : details.Quantity,
        //                            Amount = details.Amount == null ? 0.00m : details.Quantity,
        //                            ItemID = details.ItemID,
        //                            Description = details.Description,
        //                            ItemName = itemMaster.ItemName,
        //                            Construction = itemMaster.Description, //"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + itemMaster.EPI.ToString() + "x" + itemMaster.PPI.ToString(),//itemMaster.Weave,

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
        //                            // Construction = GetConstruction(x.ItemID),
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
        //public IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<vmItemGroup> objPISampleNo = null;
        //    IEnumerable<CmnItemMaster> cmnItemGroupID = null;
        //    try
        //    {
        //        //objPISampleNo = GenericFactory_EF_SampleNo.GetAll().Where(x=>x.ItemTypeID==1).GroupBy(x => x.UniqueCode).Select(o => new CmnItemMaster { UniqueCode = o.Key }).ToList();
        //        cmnItemGroupID = GenericFactory_EF_SampleNo.GetAll().Where(x => x.ItemTypeID == 1 && x.IsDeleted == false).GroupBy(x => x.ItemGroupID).Select(o => new CmnItemMaster { ItemGroupID = o.Key }).ToList();
        //        //.Where(x => x.ItemTypeID == 1).GroupBy(x => x.ItemGroupID).Select(o => new vmItemGroup { ItemGroupID = o.Key }).ToList();
        //        objPISampleNo = (from groupItm in GenericFactory_EF_ItemGroup.GetAll()
        //                         join groupId in cmnItemGroupID on groupItm.ItemGroupID equals groupId.ItemGroupID
        //                         select new
        //                         {
        //                             ItemGroupID = groupId.ItemGroupID,
        //                             ItemGroupName = groupItm.ItemGroupName
        //                         }).Select(m => new vmItemGroup { ItemGroupID = m.ItemGroupID ?? 0, ItemGroupName = m.ItemGroupName }).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPISampleNo;
        //}

        ////public IEnumerable<vmItem> GetItemMasterById(string uniqueCode)
        ////{
        ////    int itemGroupId = Convert.ToInt32(uniqueCode);
        ////    IEnumerable<vmItem> objPIItemMaster = null;
        ////    try
        ////    {
        ////        objPIItemMaster = (from item in GenericFactory_EF_SampleNo.GetAll() 

        ////                           join color in GFactory_EF_CmnItemColor.GetAll() on item.ItemColorID equals color.ItemColorID

        ////                           //join warp in GenericFactory_EF_RndYarnCR.GetAll() on item.WarpYarnID equals warp.YarnID
        ////                           //into warpYarn
        ////                           //from yWarpYarn in warpYarn
        ////                           //join weft in GenericFactory_EF_RndYarnCR.GetAll() on item.WeftYarnID equals weft.YarnID
        ////                           //into weftYarn
        ////                           //from xWeftYarn in weftYarn
        ////                           select new
        ////                           {
        ////                               ItemID = item.ItemID,
        ////                               ItemName = item.ItemName,
        ////                               ItemSizeID = item.ItemSizeID,
        ////                               UniqueCode = item.UniqueCode,
        ////                               ArticleNo = item.ArticleNo,
        ////                               CuttableWidth = item.CuttableWidth,
        ////                               WeightPerUnit = item.WeightPerUnit,
        ////                               Description = item.Description,
        ////                               CompanyID = item.CompanyID,
        ////                               Weave = item.Weave,
        ////                               ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
        ////                               ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
        ////                               Construction = item.Description,//"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + item.EPI.ToString() + "x" + item.PPI.ToString(),//item.Weave,
        ////                               Width = 0.00M,
        ////                               ItemTypeID = item.ItemTypeID,
        ////                               ItemGroupID = item.ItemGroupID,
        ////                               IsDeleted = item.IsDeleted

        ////                           })
        ////                            .Select(x => new vmItem
        ////                            {
        ////                                ItemID = x.ItemID,
        ////                                ItemName = x.ItemName,
        ////                                ItemSizeID = x.ItemSizeID,
        ////                                UniqueCode = x.UniqueCode,
        ////                                ArticleNo = x.ArticleNo,
        ////                                CuttableWidth = x.CuttableWidth,
        ////                                WeightPerUnit = x.WeightPerUnit,
        ////                                Description = x.Description,
        ////                                CompanyID = x.CompanyID,
        ////                                Weave = x.Weave,
        ////                                ItemColorID = x.ItemColorID,
        ////                                ColorName = x.ColorName,
        ////                                Construction = x.Construction,
        ////                                Width = x.Width,
        ////                                ItemTypeID = x.ItemTypeID,
        ////                                ItemGroupID = x.ItemGroupID,
        ////                                IsDeleted = x.IsDeleted
        ////                            }).Where(m => m.ItemGroupID == itemGroupId && m.ItemTypeID == 1 && m.IsDeleted == false).ToList();

        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return objPIItemMaster;
        ////}

        //public IEnumerable<vmItem> GetItemMasterById(vmCmnParameters objcmnParam, string groupId, out int recordsTotal)
        //{
              //_ctxCmn = new ERP_Entities();
        //    int itemGroupId = Convert.ToInt32(groupId);
        //    IEnumerable<vmItem> objPIItemMaster = null;
        //    IEnumerable<vmItem> objPIItemMasterWithoutPaging = null;

        //    recordsTotal = 0;
        //    try
        //    {
        //        objPIItemMasterWithoutPaging = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_SampleNo.GetAll()

        //                                        join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID  //GFactory_EF_CmnItemColor.GetAll()

        //                                        //join warp in GenericFactory_EF_RndYarnCR.GetAll() on item.WarpYarnID equals warp.YarnID
        //                                        //into warpYarn
        //                                        //from yWarpYarn in warpYarn..DefaultIfEmpty()
        //                                        //join weft in GenericFactory_EF_RndYarnCR.GetAll() on item.WeftYarnID equals weft.YarnID
        //                                        //into weftYarn
        //                                        //from xWeftYarn in weftYarn.DefaultIfEmpty()
        //                                        select new
        //                                        {
        //                                            ItemID = item.ItemID,
        //                                            ItemName = item.ItemName,
        //                                            ItemSizeID = item.ItemSizeID,
        //                                            UniqueCode = item.UniqueCode,
        //                                            ArticleNo = item.ArticleNo,
        //                                            CuttableWidth = item.CuttableWidth,
        //                                            WeightPerUnit = item.WeightPerUnit,
        //                                            Description = item.Description,
        //                                            CompanyID = item.CompanyID,
        //                                            Weave = item.Weave,
        //                                            ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
        //                                            ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
        //                                            Construction = item.Description,//"(" + yWarpYarn.YarnCount + ")X(" + xWeftYarn.YarnCount + ")/" + item.EPI.ToString() + "x" + item.PPI.ToString(),//item.Weave,
        //                                            Width = 0.00M,
        //                                            ItemTypeID = item.ItemTypeID,
        //                                            ItemGroupID = item.ItemGroupID,
        //                                            IsDeleted = item.IsDeleted

        //                                        })
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
        //                                // Construction = GetConstruction(x.ItemID),
        //                                Width = x.Width,
        //                                ItemTypeID = x.ItemTypeID,
        //                                ItemGroupID = x.ItemGroupID,
        //                                IsDeleted = x.IsDeleted
        //                            })
        //                            .Where(m => m.ItemGroupID == itemGroupId && m.ItemTypeID == 1 && m.IsDeleted == false).ToList();

        //        objPIItemMaster = objPIItemMasterWithoutPaging.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
        //        recordsTotal = objPIItemMasterWithoutPaging.Count();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPIItemMaster;
        //}

        //private string GetConstruction(long itemID)
        //{
               //_ctxCmn = new ERP_Entities();
        //    // throw new NotImplementedException();
        //    int ItemId = Convert.ToInt16(itemID);
        //    //string construction = "";
        //    //try
        //    //{
        //    //    string sql = string.Format("select [dbo].[CmnItemConstruction]({0})", ItemId);
        //    //    construction = _ctxCmn.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    //    return _ctxCmn.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    //}
        //    //catch(Exception e)
        //    //{
        //    //    e.ToString();
        //    //}
        //    //return construction;
        //    return GFactory_EF_CmnItemColor.GetConstruction(ItemId);
        //}

        //public IEnumerable<vmPIMaster> GetBankAdvisingListByCompanyID(int Id)
        //{
        //    IEnumerable<vmPIMaster> objCmnBank = null;
        //    try
        //    {
        //        objCmnBank = (from bnkAdvising in GFactory_EF_CmnBankAdvising.GetAll().Where(badvs => badvs.CompanyID == Id && badvs.IsDeleted == false)
        //                      join bnk in GFactory_EF_CmnBank.GetAll() on bnkAdvising.BankID equals bnk.BankID
        //                      select new
        //                      {
        //                          BankID = bnk.BankID,
        //                          BankName = bnk.BankName,
        //                          BankShortName = bnk.BankShortName,
        //                          CompanyID = bnk.CompanyID,
        //                          CustomCode = bnk.CustomCode,
        //                          IsDefaultBankAdvising = bnkAdvising.IsDefault

        //                      }).ToList().Select(x => new vmPIMaster
        //                      {
        //                          BankID = x.BankID,
        //                          BankName = x.BankName,
        //                          BankShortName = x.BankShortName,
        //                          CompanyID = x.CompanyID,
        //                          CustomCode = x.CustomCode,
        //                          IsDefaultBankAdvising = x.IsDefaultBankAdvising

        //                      }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objCmnBank;
        //}



        //public IEnumerable<vmPIMaster> GetBranchListByBankID(int Id)
        //{
        //    IEnumerable<vmPIMaster> objCmnBankBranch = null;
        //    try
        //    {
        //        objCmnBankBranch = GFactory_EF_CmnBankBranch.GetAll().Select(m => new vmPIMaster { BankID = m.BankID, BranchID = m.BranchID, BranchName = m.BranchName, IsDeleted = m.IsDeleted, IsDefaultBankBranch = m.IsDefault }).Where(m => m.BankID == Id && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objCmnBankBranch;
        //}

        //public IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objPIShipment = null;
        //    try
        //    {
        //        objPIShipment = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "shipment" && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIShipment;
        //}

        //public IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objPIValidity = null;
        //    try
        //    {
        //        objPIValidity = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "validity" && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIValidity;
        //}


        //public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objPISight = null;
        //    try
        //    {
        //        objPISight = GenericFactory_EF_PICombo.GetAll().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).Where(m => m.ComboType == "sight" && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPISight;
        //}


        //public IEnumerable<vmPIMaster> GetPIMasterByPIActive(vmCmnParameters objcmnParam, out int recordsTotal)
        //{
        //_ctxCmn = new ERP_Entities();
        //    IEnumerable<vmPIMaster> objvmPIMaster = null;
        //    IEnumerable<vmPIMaster> objvmPIMasterWithOutPaging = null;
        //    recordsTotal = 0;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {

        //            objvmPIMasterWithOutPaging = (from master in _ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false)
        //                                          join comboSihgt in _ctxCmn.CmnComboes on master.SightID equals comboSihgt.ComboID
        //                                          join comboShipment in _ctxCmn.CmnComboes on master.ShipmentID equals comboShipment.ComboID
        //                                          join comboValidity in _ctxCmn.CmnComboes on master.ValidityID equals comboValidity.ComboID
        //                                          join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
        //                                          join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
        //                                          join salesPerson in _ctxCmn.CmnUsers on master.EmployeeID equals salesPerson.UserID
        //                                          join bnk in _ctxCmn.CmnBanks on master.AdvisingBankID equals bnk.BankID
        //                                          join bnkBrnch in _ctxCmn.CmnBankBranches on master.BranchID equals bnkBrnch.BranchID
        //                                          select new
        //                                          {
        //                                              PIID = master.PIID,
        //                                              IsActive = master.IsActive,
        //                                              PINO = master.PINO,
        //                                              PIDate = master.PIDate,
        //                                              PITypeID = master.PITypeID,
        //                                              SightID = master.SightID,
        //                                              ShipmentID = master.ShipmentID,
        //                                              ValidityID = master.ValidityID,
        //                                              BuyerID = master.BuyerID,
        //                                              EmployeeID = master.EmployeeID,
        //                                              CompanyID = master.CompanyID,
        //                                              NegoDay = master.NegoDay,
        //                                              Remarks = master.Remarks,
        //                                              Discount = master.Discount,
        //                                              ODInterest = master.ODInterest,
        //                                              CreateBy = master.CreateBy,
        //                                              ComboNameShipment = comboShipment.ComboName,
        //                                              ComboNameSight = comboSihgt.ComboName,
        //                                              ComboNameValidity = comboValidity.ComboName,
        //                                              CompanyName = company.CompanyName,
        //                                              BuyerFirstName = buyer.UserFullName,
        //                                              SalesPersonFirstName = salesPerson.UserFullName,
        //                                              BankID = master.AdvisingBankID,
        //                                              BranchID = master.BranchID,
        //                                              BranchName = bnkBrnch.BranchName,
        //                                              BankName = bnk.BankName,
        //                                              BankShortName = bnk.BankShortName,
        //                                              CompanyIDBankAdvise = master.CompanyID

        //                                          }).ToList().Select(x => new vmPIMaster
        //                                          {
        //                                              PIID = x.PIID,
        //                                              IsActive = x.IsActive,
        //                                              PINO = x.PINO,
        //                                              PIDate = x.PIDate,
        //                                              PITypeID = x.PITypeID,
        //                                              SightID = x.SightID,
        //                                              ShipmentID = x.ShipmentID,
        //                                              ValidityID = x.ValidityID,
        //                                              ODInterest = x.ODInterest,
        //                                              BuyerID = x.BuyerID,
        //                                              EmployeeID = x.EmployeeID,
        //                                              CompanyID = x.CompanyID,
        //                                              CreateBy = x.CreateBy,
        //                                              NegoDay = x.NegoDay,
        //                                              Remarks = x.Remarks,
        //                                              Discount = x.Discount,
        //                                              ComboNameShipment = x.ComboNameShipment,
        //                                              ComboNameSight = x.ComboNameSight,
        //                                              ComboNameValidity = x.ComboNameValidity,
        //                                              CompanyName = x.CompanyName,
        //                                              BuyerFirstName = x.BuyerFirstName,
        //                                              SalesPersonFirstName = x.SalesPersonFirstName,
        //                                              BankID = x.BankID,
        //                                              BranchID = x.BranchID,
        //                                              BranchName = x.BranchName,
        //                                              BankName = x.BankName,
        //                                              BankShortName = x.BankShortName,
        //                                              CompanyIDBankAdvise = x.CompanyID
        //                                          }).ToList();

        //            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.PIID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

        //            recordsTotal = objvmPIMasterWithOutPaging.Count();

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objvmPIMaster;
        //}

        //public IEnumerable<vmPIMaster> GetPIMasterByPIActive()
        //{
        //_ctxCmn = new ERP_Entities();
        //    IEnumerable<vmPIMaster> objvmPIMaster = null;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {

        //            objvmPIMaster = (from master in _ctxCmn.SalPIMasters.Where(m => m.IsActive == true && m.IsDeleted == false)
        //                             join comboSihgt in _ctxCmn.CmnComboes on master.SightID equals comboSihgt.ComboID
        //                             join comboShipment in _ctxCmn.CmnComboes on master.ShipmentID equals comboShipment.ComboID
        //                             join comboValidity in _ctxCmn.CmnComboes on master.ValidityID equals comboValidity.ComboID
        //                             join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
        //                             join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
        //                             join salesPerson in _ctxCmn.CmnUsers on master.EmployeeID equals salesPerson.UserID
        //                             join bnk in _ctxCmn.CmnBanks on master.AdvisingBankID equals bnk.BankID
        //                             join bnkBrnch in _ctxCmn.CmnBankBranches on master.BranchID equals bnkBrnch.BranchID
        //                             select new
        //                             {
        //                                 PIID = master.PIID,
        //                                 IsActive = master.IsActive,
        //                                 PINO = master.PINO,
        //                                 PIDate = master.PIDate,
        //                                 PITypeID = master.PITypeID,
        //                                 SightID = master.SightID,
        //                                 ShipmentID = master.ShipmentID,
        //                                 ValidityID = master.ValidityID,
        //                                 BuyerID = master.BuyerID,
        //                                 EmployeeID = master.EmployeeID,
        //                                 CompanyID = master.CompanyID,
        //                                 NegoDay = master.NegoDay,
        //                                 Remarks = master.Remarks,
        //                                 Discount = master.Discount,
        //                                 ODInterest = master.ODInterest,
        //                                 CreateBy = master.CreateBy,
        //                                 ComboNameShipment = comboShipment.ComboName,
        //                                 ComboNameSight = comboSihgt.ComboName,
        //                                 ComboNameValidity = comboValidity.ComboName,
        //                                 CompanyName = company.CompanyName,
        //                                 BuyerFirstName = buyer.UserFullName,
        //                                 SalesPersonFirstName = salesPerson.UserFullName,
        //                                 BankID = master.AdvisingBankID,
        //                                 BranchID = master.BranchID,
        //                                 BranchName = bnkBrnch.BranchName,
        //                                 BankName = bnk.BankName,
        //                                 BankShortName = bnk.BankShortName,
        //                                 CompanyIDBankAdvise = master.CompanyID

        //                             }).ToList().Select(x => new vmPIMaster
        //                             {
        //                                 PIID = x.PIID,
        //                                 IsActive = x.IsActive,
        //                                 PINO = x.PINO,
        //                                 PIDate = x.PIDate,
        //                                 PITypeID = x.PITypeID,
        //                                 SightID = x.SightID,
        //                                 ShipmentID = x.ShipmentID,
        //                                 ValidityID = x.ValidityID,
        //                                 ODInterest = x.ODInterest,
        //                                 BuyerID = x.BuyerID,
        //                                 EmployeeID = x.EmployeeID,
        //                                 CompanyID = x.CompanyID,
        //                                 CreateBy = x.CreateBy,
        //                                 NegoDay = x.NegoDay,
        //                                 Remarks = x.Remarks,
        //                                 Discount = x.Discount,
        //                                 ComboNameShipment = x.ComboNameShipment,
        //                                 ComboNameSight = x.ComboNameSight,
        //                                 ComboNameValidity = x.ComboNameValidity,
        //                                 CompanyName = x.CompanyName,
        //                                 BuyerFirstName = x.BuyerFirstName,
        //                                 SalesPersonFirstName = x.SalesPersonFirstName,
        //                                 BankID = x.BankID,
        //                                 BranchID = x.BranchID,
        //                                 BranchName = x.BranchName,
        //                                 BankName = x.BankName,
        //                                 BankShortName = x.BankShortName,
        //                                 CompanyIDBankAdvise = x.CompanyID
        //                             }).Where(m => m.IsDeleted == false)
        //                               .OrderBy(x => x.PIID)
        //                               .ToList();

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objvmPIMaster;
        //}

        ///// <summary>
        ///// Save Data To Database
        ///// <para>Use it when save data through ORM</para>
        ///// </summary>
        //public string SaveUpdatePIItemMasterNdetails(SalPIMaster itemMaster, List<SalPIDetail> itemDetails, int menuID)
        //{
        //    string result = "";
        //    if (itemMaster.PIID > 0)
        //    {
        //        //  ---- start create PI revise No -----// 
        //        string[] piNo = itemMaster.PINO.ToString().Split(new string[] { "-Revise-" }, StringSplitOptions.None);
        //        int reviseNo = 0;
        //        string newPiReviseNo = "";
        //        if (piNo.Length > 1)
        //        {
        //            reviseNo = Convert.ToInt16(piNo[piNo.Length - 1]) + 1;
        //            newPiReviseNo = piNo[0] + "-Revise-" + reviseNo.ToString();
        //        }
        //        else if (piNo.Length == 1)
        //        {
        //            newPiReviseNo = itemMaster.PINO.ToString() + "-Revise-1".ToString();
        //        }

        //        //  ---- end create PI revise No -----// 

        //        using (TransactionScope transaction = new TransactionScope())
        //        {
        //            try
        //            {
        //                //start new maxId
        //                long NextId = Convert.ToInt64(GenericFactory_EF_SalPIMaster.getMaxID("SalPIMaster"));

        //                long FirstDigit = 0;
        //                long OtherDigits = 0;

        //                long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalPIDetail.getMaxID("SalPIDetail"));
        //                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
        //                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));


        //                //end new maxId

        //                // start for Update in SalPIMaster

        //                Int64 lPIID = itemMaster.PIID;
        //                IEnumerable<SalPIMaster> objSalPIMaster = GenericFactory_EF_SalPIMaster.FindBy(m => m.PIID == lPIID && m.IsDeleted == false).ToList();
        //                SalPIMaster lstSalPIMaster = new SalPIMaster();
        //                foreach (SalPIMaster spl in objSalPIMaster)
        //                {
        //                    spl.IsActive = false;
        //                    lstSalPIMaster = spl;
        //                }

        //                GenericFactory_EF_SalPIMaster.Update(lstSalPIMaster);
        //                GenericFactory_EF_SalPIMaster.Save();

        //                // end for Update in SalPIMaster

        //                itemMaster.PIID = NextId;
        //                itemMaster.PINO = newPiReviseNo;
        //                itemMaster.CreateOn = DateTime.Now;
        //                itemMaster.CreatePc =  HostService.GetIP();
        //                itemMaster.IsHDOCompleted = false;
        //                itemMaster.IsLcCompleted = false;

        //                List<SalPIDetail> lstSalPIDetail = new List<SalPIDetail>();
        //                foreach (SalPIDetail sdtl in itemDetails)
        //                {
        //                    SalPIDetail objSalPIDetail = new SalPIDetail();
        //                    objSalPIDetail.PIDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
        //                    objSalPIDetail.PIID = NextId;
        //                    objSalPIDetail.ItemID = sdtl.ItemID;
        //                    objSalPIDetail.CompanyID = sdtl.CompanyID;
        //                    objSalPIDetail.UnitPrice = sdtl.UnitPrice;
        //                    objSalPIDetail.CuttableWidth = sdtl.CuttableWidth;
        //                    objSalPIDetail.BuyerStyle = sdtl.BuyerStyle;
        //                    objSalPIDetail.Quantity = sdtl.Quantity;
        //                    objSalPIDetail.Amount = sdtl.Amount;
        //                    objSalPIDetail.Description = sdtl.Description;
        //                    objSalPIDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
        //                    objSalPIDetail.CreateOn = DateTime.Now;
        //                    objSalPIDetail.IsActive = sdtl.IsActive;
        //                    objSalPIDetail.CreatePc =  HostService.GetIP();
        //                    // objSalPIDetail.IsCICompleted = false;
        //                    lstSalPIDetail.Add(objSalPIDetail);
        //                    OtherDigits++;
        //                }
        //                GenericFactory_EF_SalPIMaster.Insert(itemMaster);
        //                GenericFactory_EF_SalPIMaster.Save();
        //                //............Update MaxID.................//
        //                GenericFactory_EF_SalPIMaster.updateMaxID("SalPIMaster", Convert.ToInt64(NextId));
        //                GenericFactory_EF_SalPIDetail.InsertList(lstSalPIDetail);
        //                GenericFactory_EF_SalPIDetail.Save();
        //                //............Update MaxID.................//
        //                GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
        //                transaction.Complete();
        //                result = newPiReviseNo;
        //            }
        //            catch (Exception e)
        //            {
        //                e.ToString();
        //                result = "";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        using (TransactionScope transaction = new TransactionScope())
        //        {
        //            try
        //            {
        //                //...........START  new maxId........//
        //                long NextId = Convert.ToInt64(GenericFactory_EF_SalPIMaster.getMaxID("SalPIMaster"));

        //                long FirstDigit = 0;
        //                long OtherDigits = 0;
        //                long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalPIDetail.getMaxID("SalPIDetail"));
        //                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
        //                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

        //                //..........END new maxId.........//


        //                //......... START for custom code........... //
        //                string customCode = "";

        //                string CustomNo = customCode = GenericFactory_EF_SalPIMaster.getCustomCode(menuID, DateTime.Now, itemMaster.CompanyID, 1, 1);
        //                if (CustomNo != null)
        //                {
        //                    customCode = CustomNo;
        //                }
        //                else
        //                {
        //                    customCode = NextId.ToString();
        //                }
        //                //.........END for custom code............ //

        //                string newPiNo = customCode;
        //                itemMaster.PIID = NextId;
        //                itemMaster.CreateOn = DateTime.Now;
        //                itemMaster.CreatePc =  HostService.GetIP();
        //                itemMaster.PINO = newPiNo;
        //                itemMaster.IsHDOCompleted = false;
        //                itemMaster.IsLcCompleted = false;
        //                List<SalPIDetail> lstSalPIDetail = new List<SalPIDetail>();
        //                foreach (SalPIDetail sdtl in itemDetails)
        //                {
        //                    SalPIDetail objSalPIDetail = new SalPIDetail();
        //                    //objSalPIDetail.PIDetailID = nextDetailId;
        //                    objSalPIDetail.PIDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
        //                    objSalPIDetail.PIID = NextId;
        //                    objSalPIDetail.ItemID = sdtl.ItemID;
        //                    objSalPIDetail.CompanyID = sdtl.CompanyID;
        //                    objSalPIDetail.UnitPrice = sdtl.UnitPrice;
        //                    objSalPIDetail.CuttableWidth = sdtl.CuttableWidth;
        //                    objSalPIDetail.BuyerStyle = sdtl.BuyerStyle;
        //                    objSalPIDetail.Quantity = sdtl.Quantity;
        //                    objSalPIDetail.Amount = sdtl.Amount;
        //                    objSalPIDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
        //                    objSalPIDetail.CreateOn = DateTime.Now;
        //                    objSalPIDetail.IsActive = sdtl.IsActive;
        //                    objSalPIDetail.CreatePc =  HostService.GetIP();
        //                    // objSalPIDetail.IsCICompleted = false;
        //                    lstSalPIDetail.Add(objSalPIDetail);
        //                    //nextDetailId++;
        //                    OtherDigits++;
        //                }

        //                GenericFactory_EF_SalPIMaster.Insert(itemMaster);
        //                GenericFactory_EF_SalPIMaster.Save();
        //                //............Update MaxID.................//
        //                GenericFactory_EF_SalPIMaster.updateMaxID("SalPIMaster", Convert.ToInt64(NextId));
        //                //............Update CustomCode.............//
        //                GenericFactory_EF_SalPIMaster.updateCustomCode(menuID, DateTime.Now, itemMaster.CompanyID, 1, 1);

        //                // List<SalPIDetail> lstSalPIDetail222 = new List<SalPIDetail>();
        //                GenericFactory_EF_SalPIDetail.InsertList(lstSalPIDetail);
        //                GenericFactory_EF_SalPIDetail.Save();
        //                //............Update MaxID.................//
        //                // GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(nextDetailId - 1));

        //                GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
        //                transaction.Complete();
        //                result = newPiNo;
        //            }
        //            catch (Exception e)
        //            {
        //                result = "";
        //            }
        //        }

        //    }
        //    return result;
        //}

        //public int DeleteSalPIMasterNSalPIDetail(int Id)
        //{
        //    int result = 0;
        //    try
        //    {
        //        GenericFactory_EF_SalPIMaster.Delete(m => m.PIID == Id);
        //        GenericFactory_EF_SalPIMaster.Save();
        //        result = -103;
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = 0;
        //    }

        //    return result;
        //}
    }
}
