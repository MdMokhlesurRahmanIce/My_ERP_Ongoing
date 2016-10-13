using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Factories
{
    public partial class CmnItemGroup_EF : GenericFactory_EF<ERP_Entities, CmnItemGroup> { }
    public partial class CmnItemItemType_EF : GenericFactory_EF<ERP_Entities, CmnItemType> { }


    public class TransactionTypeMgt : iTransactionTypeMgt
    {
        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnItemType> GenericFactory_EF_ItemType = null;

        public TransactionTypeMgt()
        {
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            GenericFactory_EF_ItemType = new CmnItemItemType_EF();
        }

        public int SaveItemGroup(CmnItemGroup model)
        {
            int result = 0;
            try
            {
                int NextId = GenericFactory_EF_ItemGroup.getMaxVal_int("ItemGroupID", "CmnItemGroup");
                model.ItemGroupID = NextId;
                model.CustomCode = NextId.ToString();
                model.CreateOn = DateTime.Today;
                model.CreatePc = Environment.MachineName;
                GenericFactory_EF_ItemGroup.Insert(model);
                GenericFactory_EF_ItemGroup.Save();
                result = 1;

            }
            catch (Exception ex)
            {

                ex.ToString();
                result = 0;
            }
            return result;


        }

        public int UpdateItemGroup(CmnItemGroup model)
        {

            int result = 0;

            try
            {
                CmnItemGroup _itemGroup = GenericFactory_EF_ItemGroup.FindBy(x=>x.ItemGroupID==model.ItemGroupID).FirstOrDefault();
                _itemGroup.UpdateBy = model.CreateBy;
                _itemGroup.UpdateOn = DateTime.Today;
                _itemGroup.UpdatePc = Environment.MachineName;
                _itemGroup.ItemGroupName = model.ItemGroupName;
                _itemGroup.ItemTypeID = model.ItemTypeID;
                _itemGroup.ParentID = model.ParentID;
                _itemGroup.IsActive = model.IsActive;

                GenericFactory_EF_ItemGroup.Update(_itemGroup);
                GenericFactory_EF_ItemGroup.Save();

                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
        }

        public int DeleteItemGroup(CmnItemGroup _model)
        {
            int result = 0;

            try
            {
                CmnItemGroup model = GenericFactory_EF_ItemGroup.GetAll().Where(x => x.ItemGroupID == _model.ItemGroupID).FirstOrDefault();

                model.DeleteOn = DateTime.Today;
                model.DeleteBy = _model.DeleteBy;
                model.IsDeleted = true;
                GenericFactory_EF_ItemGroup.Update(model);
                GenericFactory_EF_ItemGroup.Save();

                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
            

        }

        public List<vmItemGroup> GetAllItemGroups(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            List<vmItemGroup> _objvmItemGroups = null;
            List<CmnItemGroup> _objCmnItemGroups = null;
            List<CmnItemType> _objCmnItemTypes = null;
            try
            {
                _objCmnItemGroups = GenericFactory_EF_ItemGroup.GetAll().Where(x => x.IsDeleted == false && x.CompanyID == CompanyId).ToList();
                _objCmnItemTypes = GenericFactory_EF_ItemType.GetAll().Where(x => x.IsDeleted == false && x.CompanyID == CompanyId).ToList();

                _objvmItemGroups = (from ig in _objCmnItemGroups
                                    join it in _objCmnItemTypes on ig.ItemTypeID equals it.ItemTypeID
                                    where ig.CompanyID == CompanyId
                                    select new vmItemGroup
                                    {
                                        ItemGroupID = ig.ItemGroupID,
                                        ItemGroupName = ig.ItemGroupName,
                                        Type = it.ItemTypeName,
                                        IsActive = ig.IsActive == true ? "Yes" : "NO",
                                        Parent = ig.ParentID == null ? null : (from re in _objCmnItemGroups
                                                                               where re.ItemGroupID == ig.ParentID
                                                                               select re.ItemGroupName).FirstOrDefault()

                                    }).ToList();

                return _objvmItemGroups;
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objvmItemGroups;
        }

        public vmItemGroup GetItemGroupByID(int id)
        {
            vmItemGroup _objvmItemGroups = null;
            List<CmnItemGroup> objAllItemGroups = null;
            List<CmnItemGroup> objItemGroup = null;
            List<CmnItemType> _objCmnItemTypes = null;
            try
            {
                objItemGroup = GenericFactory_EF_ItemGroup.FindBy(m => m.ItemGroupID == id).ToList();
                _objCmnItemTypes = GenericFactory_EF_ItemType.GetAll().Where(x => x.IsDeleted == false).ToList();
                objAllItemGroups = GenericFactory_EF_ItemGroup.GetAll().Where(x => x.IsDeleted == false).ToList();
                _objvmItemGroups = (from ig in objItemGroup
                                    join it in _objCmnItemTypes on ig.ItemTypeID equals it.ItemTypeID
                                    where ig.ItemGroupID == id
                                    select new vmItemGroup
                                    {
                                        ItemGroupID = ig.ItemGroupID,
                                        ItemGroupName = ig.ItemGroupName,
                                        Type = it.ItemTypeName,
                                        TypeId=ig.ItemTypeID,
                                        IsActive = ig.IsActive == true ? "Yes" : "NO",
                                        ParentId = ig.ParentID,
                                        Parent = ig.ParentID == null ? null : (from re in objAllItemGroups
                                                                               where re.ItemGroupID == ig.ParentID
                                                                               select re.ItemGroupName).FirstOrDefault()

                                    }).FirstOrDefault();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objvmItemGroups;
        }



        public List<CmnItemGroup> objItemGroup { get; set; }
    }
}
