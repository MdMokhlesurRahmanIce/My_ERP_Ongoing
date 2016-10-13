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
    public class BookingMgt : iBookingMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_PICompany = null;
        private iGenericFactory_EF<SalBookingMaster> GenericFactory_EF_SalBookingMaster;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_CmnItemMaster = null;
        private iGenericFactory_EF<SalBookingDetail> GenericFactory_EF_SalBookingDetail = null;

        //private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactory_EF_CmnUserWiseCompany = null;

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
        public List<CmnUser> GetBuyerReference(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnUser> objPIBuyerReference = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objPIBuyerReference = _ctxCmn.CmnUsers.Where(m => m.UserTypeID == 4 && m.IsActive == true && m.IsDeleted == false).ToList()
                                        .Select(m => new CmnUser
                                        {
                                            UserID = m.UserID,
                                            UserFullName = m.UserFullName,
                                            UserTypeID = m.UserTypeID,
                                            IsDeleted = m.IsDeleted
                                        }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIBuyerReference;
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
        //public IEnumerable<SalBookingDetail> GetPIDetailsByActivePI(Int64 activePI)
        //{
        //    GenericFactory_EF_SalBookingDetail = new SalBookingDetail_EF();
        //    IEnumerable<SalBookingDetail> objPIDetails = null;
        //    try
        //    {
        //        objPIDetails = GenericFactory_EF_SalBookingDetail.GetAll().Select(m => new SalBookingDetail
        //        {
        //            BookingID = m.BookingID,
        //            ItemID = m.ItemID,
        //            CompanyID = m.CompanyID,
        //            Quantity = m.Quantity,
        //            CreateBy = m.CreateBy,
        //            IsDeleted = m.IsDeleted

        //        }).Where(m => m.BookingID == activePI && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPIDetails;
        //}

        /// No CompanyID Provided
        public IEnumerable<vmBookingDetail> GetBookingDetail(Int64 activePI)
        {
            GenericFactory_EF_SalBookingDetail = new SalBookingDetail_EF();
            GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();

            IEnumerable<vmBookingDetail> objPIDetails = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objPIDetails = (from details in _ctxCmn.SalBookingDetails.Where(m => m.BookingID == activePI && m.IsDeleted == false)

                                        //join itemMaster in GenericFactory_EF_CmnItemMaster.GetAll() on details.ItemID equals itemMaster.ItemID
                                    select new
                                    {
                                        BookingDetailID = details.BookingDetailID,
                                        BookingID = details.BookingID,
                                        CuttableWidth = details.CmnItemMaster.CuttableWidth,
                                        Quantity = details.Quantity == null ? 0.00m : details.Quantity,
                                        ArticleNo = details.CmnItemMaster.ArticleNo,
                                        ItemID = details.ItemID,
                                        //ItemName = itemMaster.ItemName,
                                        DeliveryStartDate = details.DeliveryStartDate,
                                        DeliveryFinishDate = details.DeliveryFinishDate,
                                        Description = details.CmnItemMaster.Description,
                                        CompanyID = details.CompanyID,
                                        IsDeleted = details.IsDeleted

                                    }).ToList().Select(x => new vmBookingDetail
                                    {
                                        BookingDetailID = x.BookingDetailID,
                                        BookingID = x.BookingID,
                                        CuttableWidth = x.CuttableWidth,
                                        Quantity = x.Quantity,
                                        ArticleNo = x.ArticleNo,
                                        ItemID = x.ItemID,
                                        //ItemName = x.ItemName,
                                        DeliveryStartDate = x.DeliveryStartDate,
                                        DeliveryFinishDate = x.DeliveryFinishDate,
                                        Description = x.Description,
                                        CompanyID = x.CompanyID,
                                        IsDeleted = x.IsDeleted
                                    }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return objPIDetails;
        }

        /// No CompanyID Provided
        public IEnumerable<vmItemGroup> GetPISampleNo(int companyID, int? pageNumber, int? pageSize, int? IsPaging) // Getting all item groups
        {
            //GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            //GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();

            IEnumerable<vmItemGroup> objPISampleNo = null;

            //IEnumerable<CmnItemMaster> cmnGroupIDOfItemMaster = null;
            IEnumerable<CmnItemGroup> ItemGroup = null;

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    //cmnGroupIDOfItemMaster = _ctxCmn.CmnItemMasters.Where(x => x.ItemTypeID == 1 && x.IsDeleted == false).ToList()
                    //    .GroupBy(x => x.ItemGroupID).Select(o => new CmnItemMaster { ItemGroupID = o.Key }).ToList();

                    ItemGroup = _ctxCmn.CmnItemGroups.Where(x => x.ItemTypeID == 1 && x.IsDeleted == false
                                                            && x.CompanyID == companyID).ToList()
                                 .Select(m => new CmnItemGroup { ItemGroupID = m.ItemGroupID, ItemGroupName = m.ItemGroupName }).ToList();

                    objPISampleNo = (from groupItm in ItemGroup
                                         // join itemMasterGroup in cmnGroupIDOfItemMaster on groupItm.ItemGroupID equals itemMasterGroup.ItemGroupID
                                     select new
                                     {
                                         ItemGroupID = groupItm.ItemGroupID,
                                         ItemGroupName = groupItm.ItemGroupName
                                     }).Select(m => new vmItemGroup
                                     {
                                         ItemGroupID = m.ItemGroupID,
                                         ItemGroupName = m.ItemGroupName
                                     }).ToList().OrderBy(m => m.ItemGroupName);

                }
                catch (Exception e)
                {
                    e.ToString();
                }
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
                    objPIItemMasterWithoutPaging = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_CmnItemMaster.GetAll()

                                                    join color in _ctxCmn.CmnItemColors on item.ItemColorID equals color.ItemColorID  //GFactory_EF_CmnItemColor.GetAll()                                                 
                                                    select new
                                                    {
                                                        ItemID = item.ItemID,
                                                        ItemName = item.ItemName,
                                                        ItemSizeID = item.ItemSizeID,
                                                        UniqueCode = item.UniqueCode,
                                                        ArticleNo = item.ArticleNo,
                                                        CuttableWidth = item.CuttableWidth,
                                                        FinishingWeight = item.FinishingWeight,
                                                        CompanyID = item.CompanyID,
                                                        Weave = item.Weave,
                                                        ItemColorID = color.ItemColorID,//xColor.ItemColorID,//color.ItemColorID,
                                                        ColorName = color.ColorName,//xColor.ColorName,//color.ColorName,
                                                        Construction = item.Note,
                                                        Description = item.Description,
                                                        FinishingWidth = item.FinishingWidth,
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
                                            FinishingWeight = x.FinishingWeight,
                                            CompanyID = x.CompanyID,
                                            Weave = x.Weave,
                                            ItemColorID = x.ItemColorID,
                                            ColorName = x.ColorName,
                                            Construction = x.Construction,
                                            Description = x.Description,
                                            FinishingWidth = x.FinishingWidth,
                                            ItemTypeID = x.ItemTypeID,
                                            ItemGroupID = x.ItemGroupID,
                                            IsDeleted = x.IsDeleted
                                        })
                                        .Where(m => m.ItemTypeID == 1 && m.IsDeleted == false
                                               && m.CompanyID == objcmnParam.selectedCompany
                                               && itemGroupId == 0 ? true : m.ItemGroupID == itemGroupId).ToList();

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
        /// No CompanyID Provided
        /// CompanyID Provided but Not in Use
        public IEnumerable<vmBookingMaster> GetBookingMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBookingMaster> objvmPIMaster = null;
            IEnumerable<vmBookingMaster> objvmPIMasterWithOutPaging = null;
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

                    List<SalBookingMaster> SalBookingMaster = new List<SalBookingMaster>();
                    foreach (CmnUserWiseCompany u in whichCompanies)
                    {
                        List<SalBookingMaster> AddToList = new List<SalBookingMaster>();
                        AddToList = (from hd in _ctxCmn.SalBookingMasters select hd).Where(m => m.CompanyID == u.CompanyID && m.IsDeleted == false).ToList();
                        if (AddToList != null && AddToList.Count > 0)
                        {
                            SalBookingMaster.AddRange(AddToList);
                        }
                    }

                    objvmPIMasterWithOutPaging = (from master in SalBookingMaster
                                                  join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
                                                  join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
                                                  join BuyerReference in _ctxCmn.CmnUsers on master.BuyerRefID equals BuyerReference.UserID
                                                  select new
                                                  {
                                                      BookingID = master.BookingID,
                                                      BookingNo = master.BookingNo,
                                                      BookingDate = master.BookingDate,
                                                      TransactionTypeID = master.TransactionTypeID,
                                                      BuyerID = master.BuyerID,
                                                      EmployeeID = master.EmployeeID,
                                                      BuyerRefID = master.BuyerRefID,
                                                      Description = master.Description,
                                                      CompanyID = master.CompanyID,
                                                      CreateBy = master.CreateBy,
                                                      CompanyName = company.CompanyName,
                                                      BuyerFullName = buyer.UserFullName,
                                                      BuyerReferenceFullName = BuyerReference.UserFullName,
                                                      CompanyIDBankAdvise = master.CompanyID,
                                                      IsPICompleted = master.IsPICompleted,
                                                      IsLCCompleted = master.IsLCCompleted,
                                                      IsHDOCompleted = master.IsHDOCompleted
                                                  }).ToList().Select(x => new vmBookingMaster
                                                  {
                                                      BookingID = x.BookingID,
                                                      BookingNo = x.BookingNo,
                                                      BookingDate = x.BookingDate,
                                                      TransactionTypeID = x.TransactionTypeID,
                                                      BuyerID = x.BuyerID,
                                                      EmployeeID = x.EmployeeID,
                                                      BuyerRefID = x.BuyerRefID,
                                                      Description = x.Description,
                                                      CompanyID = x.CompanyID,
                                                      CreateBy = x.CreateBy,
                                                      CompanyName = x.CompanyName,
                                                      BuyerFullName = x.BuyerFullName,
                                                      BuyerReferenceFullName = x.BuyerReferenceFullName,
                                                      IsPICompleted = x.IsPICompleted,
                                                      IsLCCompleted = x.IsLCCompleted,
                                                      IsHDOCompleted = x.IsHDOCompleted
                                                  }).ToList();

                    objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.BookingID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

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
        //public IEnumerable<vmBookingMaster> GetPIMasterByPIActive()
        //{
        //    IEnumerable<vmBookingMaster> objvmPIMaster = null;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            objvmPIMaster = (from master in _ctxCmn.SalBookingMasters.Where(m => m.IsDeleted == false)
        //                             join company in _ctxCmn.CmnCompanies on master.CompanyID equals company.CompanyID
        //                             join buyer in _ctxCmn.CmnUsers on master.BuyerID equals buyer.UserID
        //                             join BuyerReference in _ctxCmn.CmnUsers on master.EmployeeID equals BuyerReference.UserID
        //                             select new
        //                             {
        //                                 BookingID = master.BookingID,
        //                                 BookingNo = master.BookingNo,
        //                                 BookingDate = master.BookingDate,
        //                                 TransactionTypeID = master.TransactionTypeID,
        //                                 BuyerID = master.BuyerID,
        //                                 EmployeeID = master.EmployeeID,
        //                                 CompanyID = master.CompanyID,
        //                                 CreateBy = master.CreateBy,
        //                                 CompanyName = company.CompanyName,
        //                                 BuyerFirstName = buyer.UserFullName,
        //                                 BuyerReferenceFullName = BuyerReference.UserFullName,
        //                                 CompanyIDBankAdvise = master.CompanyID

        //                             }).ToList().Select(x => new vmBookingMaster
        //                             {
        //                                 BookingID = x.BookingID,
        //                                 BookingNo = x.BookingNo,
        //                                 BookingDate = x.BookingDate,
        //                                 TransactionTypeID = x.TransactionTypeID,
        //                                 BuyerID = x.BuyerID,
        //                                 EmployeeID = x.EmployeeID,
        //                                 CompanyID = x.CompanyID,
        //                                 CreateBy = x.CreateBy,
        //                                 CompanyName = x.CompanyName,
        //                                 BuyerFullName = x.BuyerFirstName,
        //                                 BuyerReferenceFullName = x.BuyerReferenceFullName
        //                             }).Where(m => m.IsDeleted == false)
        //                               .OrderBy(x => x.BookingID)
        //                               .ToList();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objvmPIMaster;
        //}
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        //public string SaveUpdateBookingItemMasterNdetails(SalBookingMaster itemMaster, List<SalBookingDetail> itemDetails, int menuID)
        //{
        //    GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();
        //    GenericFactory_EF_SalBookingDetail = new SalBookingDetail_EF();
        //    string result = "";
        //    if (itemMaster.BookingID > 0)
        //    {
        //        using (TransactionScope transaction = new TransactionScope())
        //        {
        //            try
        //            {
        //                long NextId = Convert.ToInt64(GenericFactory_EF_SalBookingMaster.getMaxID("SalBookingMaster"));

        //                long FirstDigit = 0;
        //                long OtherDigits = 0;

        //                long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalBookingDetail.getMaxID("SalBookingDetail"));
        //                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
        //                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));


        //                itemMaster.BookingID = NextId;
        //                itemMaster.BookingNo = itemMaster.BookingNo;
        //                itemMaster.CreateOn = DateTime.Now;
        //                itemMaster.CreatePc =  HostService.GetIP();

        //                List<SalBookingDetail> lstSalBookingDetail = new List<SalBookingDetail>();
        //                foreach (SalBookingDetail sdtl in itemDetails)
        //                {
        //                    SalBookingDetail objSalBookingDetail = new SalBookingDetail();
        //                    objSalBookingDetail.BookingDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
        //                    objSalBookingDetail.BookingID = NextId;
        //                    objSalBookingDetail.ItemID = sdtl.ItemID;
        //                    objSalBookingDetail.CompanyID = itemMaster.CompanyID;
        //                    objSalBookingDetail.Quantity = sdtl.Quantity;
        //                    objSalBookingDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
        //                    objSalBookingDetail.CreateOn = DateTime.Now;
        //                    objSalBookingDetail.CreatePc =  HostService.GetIP();
        //                    lstSalBookingDetail.Add(objSalBookingDetail);
        //                    OtherDigits++;
        //                }
        //                GenericFactory_EF_SalBookingMaster.Insert(itemMaster);
        //                GenericFactory_EF_SalBookingMaster.Save();

        //                GenericFactory_EF_SalBookingMaster.updateMaxID("SalBookingMaster", Convert.ToInt64(NextId));
        //                GenericFactory_EF_SalBookingDetail.InsertList(lstSalBookingDetail);
        //                GenericFactory_EF_SalBookingDetail.Save();

        //                GenericFactory_EF_SalBookingDetail.updateMaxID("SalBookingDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
        //                transaction.Complete();
        //                result = itemMaster.BookingNo;
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
        //                long NextId = Convert.ToInt64(GenericFactory_EF_SalBookingMaster.getMaxID("SalBookingMaster"));

        //                long FirstDigit = 0;
        //                long OtherDigits = 0;
        //                long nextDetailId = Convert.ToInt64(GenericFactory_EF_SalBookingDetail.getMaxID("SalBookingDetail"));
        //                FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
        //                OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

        //                string customCode = "";

        //                string CustomNo = customCode = GenericFactory_EF_SalBookingMaster.getCustomCode(menuID, itemMaster.BookingDate, itemMaster.CompanyID, 1, 1); // 1 for user id and 1 for db id
        //                if (CustomNo != null && CustomNo != "")
        //                {
        //                    customCode = CustomNo;
        //                }
        //                else
        //                {
        //                    customCode = NextId.ToString();
        //                }

        //                string bookingNo = customCode;
        //                itemMaster.BookingID = NextId;
        //                itemMaster.CreateOn = DateTime.Now;
        //                itemMaster.CreatePc =  HostService.GetIP();
        //                itemMaster.BookingNo = bookingNo;
        //                itemMaster.EmployeeID = itemMaster.CreateBy??0;
        //                itemMaster.BuyerRefID = itemMaster.BuyerRefID;
        //                List<SalBookingDetail> lstSalBookingDetail = new List<SalBookingDetail>();
        //                foreach (SalBookingDetail sdtl in itemDetails)
        //                {
        //                    SalBookingDetail objSalBookingDetail = new SalBookingDetail();
        //                    objSalBookingDetail.BookingDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);
        //                    objSalBookingDetail.BookingID = NextId;
        //                    objSalBookingDetail.ItemID = sdtl.ItemID;
        //                    objSalBookingDetail.CompanyID = itemMaster.CompanyID;
        //                    objSalBookingDetail.Quantity = sdtl.Quantity;
        //                    objSalBookingDetail.DeliveryDate = sdtl.DeliveryDate;
        //                    objSalBookingDetail.CreateBy = itemMaster.CreateBy;//sdtl.CreateBy;
        //                    objSalBookingDetail.CreateOn = DateTime.Now;
        //                    objSalBookingDetail.CreatePc =  HostService.GetIP();
        //                    // objSalBookingDetail.IsCICompleted = false;
        //                    lstSalBookingDetail.Add(objSalBookingDetail);
        //                    //nextDetailId++;
        //                    OtherDigits++;
        //                }

        //                GenericFactory_EF_SalBookingMaster.Insert(itemMaster);
        //                GenericFactory_EF_SalBookingMaster.Save();

        //                GenericFactory_EF_SalBookingMaster.updateMaxID("SalBookingMaster", Convert.ToInt64(NextId));

        //                GenericFactory_EF_SalBookingMaster.updateCustomCode(menuID, DateTime.Now, itemMaster.CompanyID, 1, 1);
        //                GenericFactory_EF_SalBookingDetail.InsertList(lstSalBookingDetail);
        //                GenericFactory_EF_SalBookingDetail.Save();

        //                GenericFactory_EF_SalBookingDetail.updateMaxID("SalBookingDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
        //                transaction.Complete();
        //                result = bookingNo;
        //            }
        //            catch (Exception e)
        //            {
        //                result = "";
        //            }
        //        }
        //    }
        //    return result;
        //}
        //public IEnumerable<SalBookingDetail> GetPIDetailsByActivePI(Int64 activePI)
        //{
        //    GenericFactory_EF_SalBookingDetail = new SalBookingDetail_EF();
        //    IEnumerable<SalBookingDetail> objPIDetails = null;
        //    try
        //    {
        //        objPIDetails = GenericFactory_EF_SalBookingDetail.GetAll().Select(m => new SalBookingDetail
        //        {
        //            BookingID = m.BookingID,
        //            BookingDetailID = m.BookingDetailID,
        //            ItemID = m.ItemID,
        //            CompanyID = m.CompanyID,
        //            Quantity = m.Quantity,
        //            CreateBy = m.CreateBy,
        //            IsDeleted = m.IsDeleted

        //        }).Where(m => m.BookingID == activePI && m.IsDeleted == false).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objPIDetails;
        //}
        public int DeleteMasterDetail(int Id)
        {
            GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_SalBookingMaster.Delete(m => m.BookingID == Id);
                GenericFactory_EF_SalBookingMaster.Save();
                result = -103;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return result;
        }

        public string SaveUpdateBookingItemMasterNdetails(SalBookingMaster itemMaster, List<SalBookingDetail> itemDetails, int menuID)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                //*********************************************Start Initialize Variable*****************************************             
                long MasterId = 0, DetailId = 0, FirstDigit = 0, OtherDigits = 0;
                //***************************************End Initialize Variable*************************************************
                GenericFactory_EF_SalBookingMaster = new SalBookingMaster_EF();
                GenericFactory_EF_SalBookingDetail = new SalBookingDetail_EF();
                //****************************End Initialize Generic Repository Based on table***********************************

                //**********************************Start Create Related Table Instance to Save**********************************
                var MasterItem = new SalBookingMaster();
                var DetailItem = new List<SalBookingDetail>();
                var DetailItems = new List<SalBookingDetail>();
                //************************************End Create Related Table Instance to Save**********************************

                //*************************************Start Create Model Instance to get Data***********************************
                SalBookingDetail item = new SalBookingDetail();
                SalBookingMaster items = new SalBookingMaster();
                SalBookingDetail itemdel = new SalBookingDetail();
                //***************************************End Create Model Instance to get Data***********************************

                var SDetail = itemDetails.Where(x => x.BookingDetailID == 0).ToList();
                var UDetail = itemDetails.Where(x => x.BookingDetailID != 0).ToList();
                //**************************************************Start Main Operation************************************************
                if (itemDetails.Count > 0)
                {
                    try
                    {
                        if (itemMaster.BookingID == 0)
                        {
                            //***************************************************Start Save Operation************************************************
                            //**********************************************Start Generate itemMaster & itemDetails ID****************************************
                            MasterId = Convert.ToInt16(GenericFactory_EF_SalBookingMaster.getMaxID("SalBookingMaster"));
                            DetailId = Convert.ToInt64(GenericFactory_EF_SalBookingDetail.getMaxID("SalBookingDetail"));
                            FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));
                            //***********************************************End Generate itemMaster & itemDetails ID*****************************************

                            string customCode = "";

                            string CustomNo = customCode = GenericFactory_EF_SalBookingMaster.getCustomCode(menuID, itemMaster.BookingDate, itemMaster.CompanyID, 1, 1); // 1 for user id and 1 for db id
                            if (CustomNo != null && CustomNo != "")
                            {
                                customCode = CustomNo;
                            }
                            else
                            {
                                customCode = MasterId.ToString();
                            }

                            string bookingNo = customCode;


                            MasterItem = new SalBookingMaster
                            {
                                BookingID = (int)MasterId,
                                BookingNo = customCode,
                                TransactionTypeID = itemMaster.TransactionTypeID,
                                BuyerID = itemMaster.BuyerID,
                                BuyerRefID = itemMaster.BuyerRefID,
                                Description = itemMaster.Description,
                                BookingDate = itemMaster.BookingDate,
                                IsPICompleted = false,
                                IsLCCompleted = false,
                                IsHDOCompleted = false,
                                CompanyID = itemMaster.CompanyID,
                                CreateOn = DateTime.Now,
                                CreateBy = itemMaster.CreateBy,
                                CreatePc = HostService.GetIP(),
                                EmployeeID = itemMaster.CreateBy ?? 0

                            };

                            for (int i = 0; i < itemDetails.Count; i++)
                            {
                                item = itemDetails[i];
                                var Detailitem = new SalBookingDetail
                                {
                                    BookingDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                    BookingID = (int)MasterId,
                                    ItemID = item.ItemID,
                                    CompanyID = itemMaster.CompanyID,
                                    Quantity = item.Quantity,
                                    DeliveryStartDate = item.DeliveryStartDate,
                                    DeliveryFinishDate = item.DeliveryFinishDate,
                                    IsDelevered = false,
                                    CreateBy = itemMaster.CreateBy,
                                    CreateOn = DateTime.Now,
                                    CreatePc = HostService.GetIP(),
                                    IsDeleted = false
                                };
                                DetailItem.Add(Detailitem);
                                OtherDigits++;
                            }
                            //***************************************************End Save Operation************************************************
                        }
                        else
                        {
                            //***********************************Start Get Data From Related Table to Update*********************************
                            var MasterAll = GenericFactory_EF_SalBookingMaster.GetAll().Where(x => x.BookingID == itemMaster.BookingID && x.CompanyID == itemMaster.CompanyID);
                            var DetailAll = GenericFactory_EF_SalBookingDetail.GetAll().Where(x => x.BookingID == itemMaster.BookingID && x.CompanyID == itemMaster.CompanyID).ToArray();
                            //*************************************End Get Data From Related Table to Update*********************************

                            //***************************************************Start Update Operation********************************************
                            MasterItem = MasterAll.First(x => x.BookingID == itemMaster.BookingID);
                            MasterItem.BookingNo = itemMaster.BookingNo;
                            MasterItem.BookingDate = itemMaster.BookingDate;
                            MasterItem.BuyerID = itemMaster.BuyerID;
                            MasterItem.BuyerRefID = itemMaster.BuyerRefID;
                            MasterItem.Description = itemMaster.Description;
                            MasterItem.CompanyID = itemMaster.CompanyID;
                            MasterItem.UpdateBy = itemMaster.CreateBy;
                            MasterItem.UpdateOn = DateTime.Now;
                            MasterItem.UpdatePc = HostService.GetIP();
                            MasterItem.IsDeleted = false;

                            for (int i = 0; i < UDetail.Count; i++)
                            {
                                item = UDetail[i];
                                foreach (SalBookingDetail d in DetailAll.Where(d => d.BookingID == itemMaster.BookingID && d.BookingDetailID == item.BookingDetailID))
                                {
                                    d.ItemID = item.ItemID;
                                    d.Quantity = item.Quantity;
                                    d.DeliveryStartDate = item.DeliveryStartDate;
                                    d.DeliveryFinishDate = item.DeliveryFinishDate;
                                    d.CompanyID = itemMaster.CompanyID;
                                    d.UpdateBy = itemMaster.CreateBy;
                                    d.UpdateOn = DateTime.Now;
                                    d.UpdatePc = HostService.GetIP();
                                    d.IsDeleted = false;

                                    DetailItem.Add(d);
                                    break;
                                }
                            }
                            if (SDetail != null && SDetail.Count != 0)
                            {
                                for (int i = 0; i < SDetail.Count; i++)
                                {
                                    item = SDetail[i];
                                    DetailId = Convert.ToInt64(GenericFactory_EF_SalBookingDetail.getMaxID("SalBookingDetail"));
                                    FirstDigit = Convert.ToInt64(DetailId.ToString().Substring(0, 1));
                                    OtherDigits = Convert.ToInt64(DetailId.ToString().Substring(1, DetailId.ToString().Length - 1));

                                    var Detailitems = new SalBookingDetail
                                    {
                                        BookingDetailID = Convert.ToInt32(FirstDigit + "" + OtherDigits),
                                        BookingID = (long)itemMaster.BookingID,
                                        ItemID = item.ItemID,
                                        Quantity = item.Quantity,
                                        DeliveryStartDate = item.DeliveryStartDate,
                                        DeliveryFinishDate = item.DeliveryFinishDate,
                                        CompanyID = itemMaster.CompanyID,
                                        CreateBy = itemMaster.CreateBy,
                                        CreateOn = DateTime.Now,
                                        CreatePc = HostService.GetIP(),
                                        IsDeleted = false
                                    };
                                    DetailItems.Add(Detailitems);
                                    GenericFactory_EF_SalBookingDetail.updateMaxID("SalBookingDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits)));
                                }
                            }

                            if (UDetail.Count < DetailAll.Count())
                            {
                                for (int i = 0; i < DetailAll.Count(); i++)
                                {
                                    itemdel = DetailAll[i];

                                    var delDetail = (from del in DetailItem.Where(x => x.BookingDetailID == itemdel.BookingDetailID) select del.BookingDetailID).FirstOrDefault();
                                    if (delDetail != itemdel.BookingDetailID)
                                    {
                                        var tem = DetailAll.FirstOrDefault(d => d.BookingID == itemMaster.BookingID && d.BookingDetailID == itemdel.BookingDetailID);
                                        tem.CompanyID = itemMaster.CompanyID;
                                        tem.DeleteBy = itemMaster.CreateBy;
                                        tem.DeleteOn = DateTime.Now;
                                        tem.DeletePc = HostService.GetIP();
                                        tem.IsDeleted = true;
                                        DetailItem.Add(tem);
                                    }
                                }
                            }
                            //***************************************************End Update Operation********************************************
                        }

                        if (itemMaster.BookingID > 0)
                        {
                            //***************************************************Start Update************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_SalBookingMaster.Update(MasterItem);
                                GenericFactory_EF_SalBookingMaster.Save();
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_EF_SalBookingDetail.UpdateList(DetailItem.ToList());
                                GenericFactory_EF_SalBookingDetail.Save();
                            }
                            if (DetailItems != null && DetailItems.Count != 0)
                            {
                                GenericFactory_EF_SalBookingDetail.InsertList(DetailItems.ToList());
                                GenericFactory_EF_SalBookingDetail.Save();
                            }
                            //***************************************************End Update************************************************
                        }
                        else
                        {
                            //***************************************************Start Save************************************************
                            if (MasterItem != null)
                            {
                                GenericFactory_EF_SalBookingMaster.Insert(MasterItem);
                                GenericFactory_EF_SalBookingMaster.Save();
                                GenericFactory_EF_SalBookingMaster.updateMaxID("SalBookingMaster", Convert.ToInt64(MasterId));

                                GenericFactory_EF_SalBookingMaster.updateCustomCode(menuID, itemMaster.BookingDate, itemMaster.CompanyID, 1, 1);
                            }
                            if (DetailItem != null && DetailItem.Count != 0)
                            {
                                GenericFactory_EF_SalBookingDetail.InsertList(DetailItem.ToList());
                                GenericFactory_EF_SalBookingDetail.Save();
                                GenericFactory_EF_SalBookingDetail.updateMaxID("SalBookingDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                            //******************************************************End Save************************************************
                        }

                        transaction.Complete();
                        result = MasterItem.BookingNo;
                    }
                    catch (Exception e)
                    {
                        result = "";
                        e.ToString();
                    }
                }
                else
                {
                    result = "";
                }
            }
            return result;
            //**************************************************End Main Operation************************************************
        }
    }
}
