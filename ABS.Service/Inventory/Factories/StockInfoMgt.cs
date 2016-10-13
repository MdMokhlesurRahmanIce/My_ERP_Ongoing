using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;

using ABS.Models.ViewModel.Sales;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System.Collections;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility;

namespace ABS.Service.Inventory.Factories
{
      
    public class StockInfoMgt : iStockInfoMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmStockMaster> GenericFactory_EF_Stock = null;
        public IEnumerable<vmStockMaster> GetAllStockItems(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_EF_Stock = new vmStockMaster_GF();
            IEnumerable<vmStockMaster> objStock = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("itemTypeID", objcmnParam.ItemType);
                ht.Add("ItemGroupID", objcmnParam.ItemGroup);
                ht.Add("itemID", null);
                ht.Add("BatchID", null);
                ht.Add("LotID", null);
                ht.Add("GradeID", null);
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                ht.Add("DepartmentID", objcmnParam.DepartmentID);
                ht.Add("PageNo",objcmnParam.pageNumber);
                ht.Add("RowCountPerPage", objcmnParam.pageNumber);
                ht.Add("IsPaging", objcmnParam.IsPaging);

                // spQuery = "[Get_AllStockItems]";
                spQuery = "[sp_getStockInfo]";
                objStock = GenericFactory_EF_Stock.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objStock;
        }

        public IEnumerable<vmItemTypes> GetItemTypeList()
        {
            IEnumerable<vmItemTypes> lstItemType = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstItemType = _ctxCmn.CmnItemTypes.Where(m => m.IsDeleted == false).Select(m => new vmItemTypes { ItemTypeID = m.ItemTypeID, ItemTypeName = m.ItemTypeName, CompanyID = m.CompanyID, CreateBy = m.CreateBy }).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemType;
        }

        public IEnumerable<vmItmGroup> GetItemGroupList()
        {
            IEnumerable<vmItmGroup> lstItemGroup = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstItemGroup = _ctxCmn.CmnItemGroups.Where(m => m.IsDeleted == false && m.ParentID == null).Select(m => new vmItmGroup { ItemGroupID = m.ItemGroupID, ItemGroupName = m.ItemGroupName, CompanyID = m.CompanyID, CreateBy = m.CreateBy }).ToList().OrderBy(m => m.ItemGroupName);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemGroup;
        }
    }
}
