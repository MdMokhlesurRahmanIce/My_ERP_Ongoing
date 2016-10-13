using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Factories
{
    public partial class CmnItemGroup_EF : GenericFactory_EF<ERP_Entities, CmnItemGroup> { }
    public partial class CmnItemItemType_EF : GenericFactory_EF<ERP_Entities, CmnItemType> { }

    public class CmnItemGroupMgt : iCmnItemGroupMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<CmnItemType> GenericFactory_EF_ItemType = null;
        private iGenericFactory<vmItemGroup> GenericFactoryFor_VmItemGroup = null;

        /// No CompanyID Provided
        public int SaveItemGroup(CmnItemGroup model)
        {
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            int result = 0;
            try
            {
                int NextId = GenericFactory_EF_ItemGroup.getMaxVal_int("ItemGroupID", "CmnItemGroup");
                model.ItemGroupID = NextId;
                // model.CustomCode = NextId.ToString();
                model.CreateOn = DateTime.Today;
                model.CreatePc =  HostService.GetIP();
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
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            int result = 0;

            try
            {
                CmnItemGroup _itemGroup = GenericFactory_EF_ItemGroup.FindBy(x => x.ItemGroupID == model.ItemGroupID).FirstOrDefault();
                _itemGroup.UpdateBy = model.CreateBy;
                _itemGroup.UpdateOn = DateTime.Today;
                _itemGroup.UpdatePc =  HostService.GetIP();
                _itemGroup.ItemGroupName = model.ItemGroupName;
                _itemGroup.ItemTypeID = model.ItemTypeID;
                _itemGroup.ParentID = model.ParentID;
                _itemGroup.CustomCode = model.CustomCode;
                _itemGroup.AcDetailID = model.AcDetailID;
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
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            int result = 0;
            try
            {
                CmnItemGroup model = GenericFactory_EF_ItemGroup.GetAll().Where(x => x.ItemGroupID == _model.ItemGroupID).FirstOrDefault();
                model.DeleteOn = DateTime.Today;
                model.DeleteBy = _model.DeleteBy;
                model.DeletePc =  HostService.GetIP();
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

        public List<vmItemGroup> GetAllItemGroups(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactoryFor_VmItemGroup = new vmItemGroup_GF();
            List<vmItemGroup> _objvmItemGroups = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);

                    ht.Add("PageNo", objcmnParam.pageNumber);
                    ht.Add("RowCountPerPage", objcmnParam.pageSize);
                    ht.Add("IsPaging", objcmnParam.IsPaging);

                    spQuery = "[SPGetItemGroupDetail]";
                    _objvmItemGroups = GenericFactoryFor_VmItemGroup.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.CmnItemGroups.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objvmItemGroups;


        }
        public vmItemGroup GetItemGroupByID(int? GID, int? CompanyID)
        {
            GenericFactoryFor_VmItemGroup = new vmItemGroup_GF();
            vmItemGroup _objvmItemGroup = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", CompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("ItemTypeID", 0);
                    ht.Add("ItemGroupID", GID);
                    spQuery = "[SPGetItemGroupDetail]";
                    _objvmItemGroup = GenericFactoryFor_VmItemGroup.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objvmItemGroup;
        }

        public int GetMaxByParentID(int? ParentID, int? CompanyID)
        {
            int maxValue = 0;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    int _ParentID = Convert.ToInt16(_ctx.CmnItemGroups.Where(x => x.ItemGroupID == ParentID).Select(x => x.ParentID).FirstOrDefault());

                    int CustomeCode = Convert.ToInt16(_ctx.CmnItemGroups.Where(x => x.ItemGroupID == ParentID).Select(x => x.CustomCode).FirstOrDefault());

                    List<CmnItemGroup> itemgroups = _ctx.CmnItemGroups.Where(x => x.ParentID == _ParentID && x.CompanyID == CompanyID).ToList();

                    maxValue = CustomeCode + itemgroups.Count();

                    maxValue = maxValue + 1;
                }
                catch
                {
                    maxValue = 0;

                }

            }
            return maxValue;

        }
        public int CheckItemGroupCode(int? GroupCode)
        {
            int isexist = 0;
            using (ERP_Entities _ctx = new ERP_Entities())
            {
                try
                {
                    string customeCode = GroupCode.ToString();
                    CmnItemGroup _itemGroup = _ctx.CmnItemGroups.Where(x => x.CustomCode == customeCode).FirstOrDefault();

                    if (_itemGroup != null)
                    {
                        isexist = 1;
                    }

                }
                catch
                {
                    isexist = 0;

                }
                return isexist;

            }
        }
    }
}

