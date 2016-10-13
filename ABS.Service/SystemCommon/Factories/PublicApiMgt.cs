using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;

namespace ABS.Service.SystemCommon.Factories
{
    public class PublicApiMgt : iPublicApiMgt
    {
        
        public IEnumerable<vmItem> GetItemMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItem> returnlist = null;
            IEnumerable<vmItem> itemList = null;
            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    itemList = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_CmnItemMaster.GetAll()
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

                                                    }).Where(m => m.ItemTypeID == 1 && m.IsDeleted == false
                                               && m.CompanyID == objcmnParam.selectedCompany
                                               && (m.ItemName.Contains( (objcmnParam.serachItemName == "100")? m.ItemName: objcmnParam.serachItemName.ToString()))
                                               )
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
                                       .ToList();
                    returnlist = itemList.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = itemList.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return returnlist;
        }
        public IEnumerable<vmItem> GetItemMasterDeveloped(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItem> returnlist = null;
            IEnumerable<vmItem> itemList = null;
            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    itemList = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_CmnItemMaster.GetAll()
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
                                    IsDeleted = item.IsDeleted,
                                    IsDevelopmentComplete = item.IsDevelopmentComplete

                                }).Where(m => m.ItemTypeID == 1 && m.IsDeleted == false
                           && m.CompanyID == objcmnParam.selectedCompany
                           && m.IsDevelopmentComplete == 0
                           && (m.ItemName.Contains((objcmnParam.serachItemName == "100") ? m.ItemName : objcmnParam.serachItemName.ToString()))
                                               )
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
                                            IsDeleted = x.IsDeleted,
                                            IsDevelopmentComplete = x.IsDevelopmentComplete
                                        })
                                       .ToList();
                    returnlist = itemList.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = itemList.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return returnlist;
        }


        public IEnumerable<vmItem> GetFinishedItemMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItem> returnlist = null;
            IEnumerable<vmItem> itemList = null;
            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    itemList = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_CmnItemMaster.GetAll()
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
                                    IsDeleted = item.IsDeleted,
                                    IsDevelopmentComplete = item.IsDevelopmentComplete

                                }).Where(m => m.ItemTypeID == 1 && m.IsDeleted == false
                           && m.CompanyID == objcmnParam.selectedCompany
                           && m.IsDevelopmentComplete == 1
                           && (m.ItemName.Contains((objcmnParam.serachItemName == "100") ? m.ItemName : objcmnParam.serachItemName.ToString()))
                                               )
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
                                            IsDeleted = x.IsDeleted,
                                            IsDevelopmentComplete = x.IsDevelopmentComplete
                                        })
                                       .ToList();
                    returnlist = itemList.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = itemList.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return returnlist;
        }
    }
}
