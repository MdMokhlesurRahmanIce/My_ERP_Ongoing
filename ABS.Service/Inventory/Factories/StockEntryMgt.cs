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

    public class StockEntryMgt : iStockEntryMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory<InvStockMaster> GenericFactory_EF_StockItems = null;
        //private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_GrrItems = null;
        //private iGenericFactory_EF<CmnLot> GenericFactoryFor_CmnLot_EF = null;
        private iGenericFactory_EF<InvStockMaster> GenericFactory_EF_StockMaster = null;
        //private iGenericFactory<vmStockMaster> GenericFactory_EF_Stock = null;

        /// No CompanyID Provided
        public IEnumerable<CmnItemMaster> GetFinishItemDescription(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemMaster> objItems = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {                   
                    objItems = _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == 1).ToList().
                             Select(m => new CmnItemMaster
                             {
                                 ItemID = m.ItemID,
                                 ItemName = m.ArticleNo + " - " + m.Description
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItems;
        }
        public IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            // GenericFactoryFor_CmnLot_EF = new CmnLot_EF();
            IEnumerable<CmnLot> objLot = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objLot = _ctxCmn.CmnLots.Where(m => m.IsDeleted == false).ToList().Select(m => new CmnLot
                    {
                        LotID = m.LotID,
                        LotNo = m.LotNo,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objLot;
        }

        public IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging)
        {
           // GenericFactoryFor_CmnLot_EF = new CmnLot_EF();
            IEnumerable<CmnItemGrade> objLot = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {

                    objLot = _ctxCmn.CmnItemGrades.Where(m=> m.IsDeleted==false).ToList().Select(m => new CmnItemGrade
                    {
                        ItemGradeID = m.ItemGradeID,
                        GradeName = m.GradeName,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objLot;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public int SaveStockEntry(InvStockMaster model)
        {
            GenericFactory_EF_StockItems = new InvStockMaster_GF();
            int result = 0;
            string spQuery = "";
            Hashtable ht = new Hashtable();
            try
            {
                model.CreateOn = DateTime.Today;
                model.CreatePc =  HostService.GetIP();
                model.IsDeleted = false;

                ht.Add("ItemID", model.ItemID);
                ht.Add("GradeID", model.GradeID);
                ht.Add("Qty", model.ReceiveQty);
                ht.Add("CompanyID", model.CompanyID);

                spQuery = "[Put_CmnInvStockMaster]";
                result = GenericFactory_EF_StockItems.ExecuteCommand(spQuery, ht);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;

        }
        public IEnumerable<vmStockMaster> GetItemList(vmCmnParameters cmnParam, out int recordsTotal)
        {
            GenericFactory_EF_StockMaster = new InvStockMaster_EF();
            List<CmnItemMaster> itemMaster = null;

            List<vmStockMaster> stockMaster = null;

            List<vmStockMaster> objStockMaster = null;
            List<vmStockMaster> objStockMasterWithoutPaging = null;


            recordsTotal = 0;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    itemMaster = _ctxCmn.CmnItemMasters.Where(m => m.ItemTypeID == 1 
                    && m.CompanyID == cmnParam.selectedCompany 
                    && m.IsDeleted == false).ToList();

                    //m.CompanyID == cmnParam.loggedCompany &&
                    stockMaster = _ctxCmn.InvStockMasters.Where(m => m.ItemTypeID == 1 && m.CompanyID == cmnParam.selectedCompany
                                    && m.IsDeleted == false).ToList().Select(m => new vmStockMaster
                                    {
                                        ItemID = m.ItemID,
                                        GradeID = m.GradeID ?? 0,
                                        CurrentStock = m.CurrentStock ?? 0
                                    }).ToList();

                    //m.CompanyID == cmnParam.loggedCompany && 

                    objStockMasterWithoutPaging = (from sm in stockMaster
                                   join ig in _ctxCmn.CmnItemGrades on sm.GradeID equals ig.ItemGradeID
                                   join im in itemMaster on sm.ItemID equals im.ItemID
                                   select new
                                   {
                                       ItemID = sm.ItemID,
                                       ItemName = im.ItemName,
                                       ArticleNo = im.ArticleNo,
                                       Description = im.Description,
                                       GradeID = sm.GradeID,
                                       GradeName = ig.GradeName,
                                       CurrentStock = sm.CurrentStock

                                   }).Select(m => new vmStockMaster
                                   {
                                       ItemID = m.ItemID,
                                       GradeID = m.GradeID,
                                       GradeName = m.GradeName,
                                       ItemName = m.ArticleNo + " - " + m.Description,
                                       CurrentStock = m.CurrentStock
                                   }).ToList();


                    objStockMaster = objStockMasterWithoutPaging.OrderByDescending(s => s.ItemID)
                                     .Skip(cmnParam.pageNumber)
                                     .Take(cmnParam.pageSize).ToList();

                    recordsTotal = objStockMasterWithoutPaging.Count();
                }
            }

            catch (Exception e)
            {
                e.ToString();
            }
            return objStockMaster;
        }
    }
}
