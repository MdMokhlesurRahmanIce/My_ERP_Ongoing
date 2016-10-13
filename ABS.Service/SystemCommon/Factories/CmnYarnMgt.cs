using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnYarnMgt : iYarnMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_Yarn = null;
        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnItemCount> GenericFactory_EF_ItemCount = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_CmnItemMaster = null;
        private iGenericFactory_EF<CmnACCIntegration> GenericFactoryEF_CmnACCIntegration = null;

        /// No CompanyID Provided
        public string SaveYarn(CmnItemMaster model)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            string result = "";
            try
            {
                SaveItemCount(model);
                GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
                //Int64 NextId = GenericFactory_EF_RawMaterial.getMaxVal_int64("ItemID", "CmnItemMaster");
                Int64 NextId = Convert.ToInt64(GenericFactory_EF_CmnItemMaster.getMaxID("CmnItemMaster"));
                model.ItemID = NextId;
                model.ArticleNo = model.ItemName;
                string UniqueCode = model.ItemTypeID.ToString() + model.ItemGroupID.ToString() + model.ItemID.ToString();
                model.UniqueCode = UniqueCode;
                model.CreateOn = DateTime.Today;
                model.CreatePc =  HostService.GetIP();
                model.IsDeleted = false;
                GenericFactory_EF_Yarn.Insert(model);
                GenericFactory_EF_Yarn.Save();
                GenericFactory_EF_CmnItemMaster.updateMaxID("CmnItemMaster", NextId);
                result = model.ArticleNo + "," + UniqueCode;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;

        }

        private void SaveItemCount(CmnItemMaster model)
        {

            try
            {
                GenericFactory_EF_ItemCount = new CmnItemCount_EF();
                _ctxCmn = new ERP_Entities();
                int isExist = GenericFactory_EF_ItemCount.GetAll().Where(x => x.Count == model.Count).Count();
                if (isExist == 0)
                {
                    CmnItemCount _CmnItemCount = new CmnItemCount();
                    int NextId = GenericFactory_EF_Yarn.getMaxVal_int("CountID", "CmnItemCount");
                    _CmnItemCount.CountID = NextId;
                    _CmnItemCount.Count = model.Count ?? 0;
                    _CmnItemCount.Description = model.Count.ToString();
                    _CmnItemCount.CompanyID = model.CompanyID;
                    _CmnItemCount.CreateBy = model.CreateBy;
                    _CmnItemCount.CreateOn = DateTime.Now;
                    _CmnItemCount.CreatePc =  HostService.GetIP();
                    GenericFactory_EF_ItemCount.Insert(_CmnItemCount);
                    GenericFactory_EF_ItemCount.Save();
                }

            }
            catch
            {


            }


        }

        //private string SetArticaleNo(int? ItemGroup)
        //{
        //    GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
        //    string articaleNo = "";

        //    try
        //    {
        //        CmnItemGroup itemGroup = GenericFactory_EF_ItemGroup.FindBy(x => x.ItemGroupID == ItemGroup).FirstOrDefault();
        //        List<CmnItemMaster> _ItemMasterobj = GenericFactory_EF_Yarn.GetAll().Where(x => x.ItemGroupID == ItemGroup).ToList();

        //        string exitemGroup = itemGroup.CustomCode;
        //        int rowNumber = (from item in _ItemMasterobj
        //                         where item.ArticleNo.Contains(exitemGroup)
        //                         select item.ArticleNo).Count();

        //        if (rowNumber == 0)
        //        {
        //            articaleNo = exitemGroup + "00000";
        //            articaleNo = (Convert.ToInt64(articaleNo) + 1).ToString();

        //        }
        //        else
        //        {
        //            articaleNo = exitemGroup + "00000";
        //            Int64? Nnum = (Convert.ToInt64(articaleNo) + rowNumber) + 1;
        //            articaleNo = Nnum.ToString();

        //        }

        //        return articaleNo;


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        /// No CompanyID Provided
        public List<VmItemMater> GetAllYarn(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            try
            {
                // YarnId  is 3
                List<CmnItemMaster> _ItemMasterobj = GenericFactory_EF_Yarn.GetAll().Where(x => x.IsDeleted == false && x.ItemTypeID == 3 && x.CompanyID == CompanyID).ToList();

                List<VmItemMater> _vmItemMasters = (from olt in _ItemMasterobj
                                                    select new
                                                    {
                                                        ItemID = olt.ItemID,
                                                        ArticleNo = olt.ArticleNo,
                                                        ItemType = olt.CmnItemType.ItemTypeName,
                                                        ItemGroup = olt.CmnItemGroup == null ? "" : olt.CmnItemGroup.ItemGroupName,
                                                        ItemName = olt.ItemName,
                                                        Unit = olt.CmnUOM == null ? "" : olt.CmnUOM.UOMName,
                                                        Color = olt.CmnItemColor == null ? "" : olt.CmnItemColor.ColorName,
                                                        Size = olt.CmnItemSize == null ? "" : olt.CmnItemSize.SizeName,
                                                        Brand = olt.CmnItemBrand == null ? "" : olt.CmnItemBrand.BrandName,
                                                        Model = olt.CmnItemModel == null ? "" : olt.CmnItemModel.ModelName,
                                                        Count = olt.Count

                                                    }).ToList().Select(x => new VmItemMater
                                                    {
                                                        ItemID = x.ItemID,
                                                        ArticleNo = x.ArticleNo,
                                                        ItemType = x.ItemType,
                                                        ItemGroup = x.ItemGroup,
                                                        ItemName = x.ItemName,
                                                        Unit = x.Unit,
                                                        Color = x.Color,
                                                        Size = x.Size,
                                                        Brand = x.Brand,
                                                        Model = x.Model,
                                                        Count = x.Count

                                                    }).ToList();
                return _vmItemMasters;
            }
            catch (Exception)
            {
                throw;
            }

        }        
        public int DeleteYarn(CmnItemMaster model)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            int result = 0;

            try
            {
                CmnItemMaster _model = GenericFactory_EF_Yarn.GetAll().Where(x => x.ItemID == model.ItemID).FirstOrDefault();
                _model.DeleteOn = DateTime.Today;
                _model.DeleteBy = model.DeleteBy;
                _model.DeletePc =  HostService.GetIP();
                _model.IsDeleted = true;
                GenericFactory_EF_Yarn.Update(_model);
                GenericFactory_EF_Yarn.Save();

                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
        }

        public VmItemMater GetYarn(int id)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            try
            {
                List<CmnItemMaster> _ItemMasterobj = GenericFactory_EF_Yarn.GetAll().Where(x => x.IsDeleted == false && x.ItemID == id).ToList();

                VmItemMater _vmItemMasters = (from olt in _ItemMasterobj
                                              select new
                                              {
                                                  ItemID = olt.ItemID,
                                                  ArticleNo = olt.ArticleNo,
                                                  ItemType = olt.CmnItemType.ItemTypeName,
                                                  ItemGroup = olt.CmnItemGroup == null ? "" : olt.CmnItemGroup.ItemGroupName,
                                                  ItemName = olt.ItemName,
                                                  Unit = olt.CmnUOM == null ? "" : olt.CmnUOM.UOMName,
                                                  Color = olt.CmnItemColor == null ? "" : olt.CmnItemColor.ColorName,
                                                  Size = olt.CmnItemSize == null ? "" : olt.CmnItemSize.SizeName,
                                                  Brand = olt.CmnItemBrand == null ? "" : olt.CmnItemBrand.BrandName,
                                                  Model = olt.CmnItemModel == null ? "" : olt.CmnItemModel.ModelName,
                                                  Description = olt.Description,
                                                  Note = olt.Note,
                                                  UniqueCode = olt.UniqueCode,
                                                  UnitId = olt.UOMID,
                                                  ColorId = olt.ItemColorID,
                                                  SizeId = olt.ItemSizeID,
                                                  BrandId = olt.ItemBrandID,
                                                  ModelId = olt.ItemBrandID,
                                                  ItemGropID = olt.ItemGroupID,
                                                  Count = olt.Count

                                              }).ToList().Select(x => new VmItemMater
                                              {
                                                  ItemID = x.ItemID,
                                                  ArticleNo = x.ArticleNo,
                                                  ItemType = x.ItemType,
                                                  ItemGroup = x.ItemGroup,
                                                  ItemName = x.ItemName,
                                                  Unit = x.Unit,
                                                  Color = x.Color,
                                                  Size = x.Size,
                                                  Brand = x.Brand,
                                                  Model = x.Model,
                                                  Description = x.Description,
                                                  Note = x.Note,
                                                  UniqueCode = x.UniqueCode,
                                                  UnitId = x.UnitId,
                                                  ColorId = x.ColorId,
                                                  SizeId = x.SizeId,
                                                  BrandId = x.BrandId,
                                                  ModelId = x.BrandId,
                                                  ItemGropID = x.ItemGropID,
                                                  Count = x.Count
                                              }).FirstOrDefault();
                return _vmItemMasters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// No CompanyID Provided
        public int UpdateYarn(CmnItemMaster model)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            int result = 0;

            try
            {
                     SaveItemCount(model);
                    CmnItemMaster _model = GenericFactory_EF_Yarn.GetAll().Where(x => x.ItemID == model.ItemID).FirstOrDefault();
                    _model.UpdateOn = DateTime.Today;
                    _model.UpdateBy = model.UpdateBy;
                    _model.UpdatePc =  HostService.GetIP();
                    _model.ItemTypeID = model.ItemTypeID;
                    _model.ItemGroupID = model.ItemGroupID;
                    _model.ItemName = model.ItemName;
                    _model.UOMID = model.UOMID;
                    _model.ItemColorID = model.ItemColorID;
                    _model.ItemSizeID = model.ItemSizeID;
                    _model.ItemBrandID = model.ItemBrandID;
                    _model.ItemModelID = model.ItemModelID;
                    _model.Description = model.Description;
                    _model.Note = model.Note;
                    _model.Count = model.Count;
                    GenericFactory_EF_Yarn.Update(_model);
                    GenericFactory_EF_Yarn.Save();

                    result = 1;
                }
            
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
        }

        /// No CompanyID Provided
        public bool CheckItemCode(string ItemName)
        {
            GenericFactory_EF_Yarn = new CmnItemMaster_EF();
            bool status = false;
            try
            {
                int IsExist = GenericFactory_EF_Yarn.GetAll().Where(x => x.ItemName == ItemName).Count();
                if (IsExist > 0)
                {
                    status = true;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }
    }
}
