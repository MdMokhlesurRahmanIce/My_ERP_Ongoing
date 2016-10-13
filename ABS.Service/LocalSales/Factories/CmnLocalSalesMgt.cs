using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.LocalSales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.LocalSales.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models.ViewModel.Production;
using System.Transactions;
using ABS.Utility;

namespace ABS.Service.LocalSales.Factories
{
    public class CmnLocalSalesMgt : iCmnLocalSalesMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_CmnUser = null;
        private iGenericFactory_EF<CmnItemGrade> GenericFactoryFor_CmnGrade = null;

        private iGenericFactory_EF<CmnItemGroup> GenericFactoryFor_ItemGroup = null;
        private iGenericFactory<vmArticleDetail> GenericFactoryFor_ArticleDetails = null;
        private iGenericFactory<vmLSCurrentStock> GenericFactory_vmLSCurrentStock = null;
        private iGenericFactory_EF<CmnItemType> GenericFactory_vmItemType = null;
        private iGenericFactory_EF<SalSalesInvoiceMaster> GFactory_EF_SalSalesInvoiceMaster = null;
        private iGenericFactory_EF<SalSalesInvoiceDetail> GFactory_EF_SalSalesInvoiceDetails = null;

        public List<vmBuyer> GetBuyers(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? UserTypeID)
        {
            GenericFactoryFor_CmnUser = new CmnUser_EF();
            List<vmBuyer> Buyers = null;

            try
            {
                List<CmnUser> AllUser = GenericFactoryFor_CmnUser.GetAll().Where(x => x.IsDeleted == false && x.UserTypeID == UserTypeID && x.CompanyID == CompanyID).ToList();
                Buyers = (from olt in AllUser
                          orderby olt.UserID descending
                          select new vmBuyer
                          {
                              UserID = olt.UserID,
                              UserName = olt.UserFullName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Buyers;

        }
        public List<vmItemGroup> GetItemGroup(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType)
        {

            GenericFactoryFor_ItemGroup = new CmnItemGroup_EF();
            List<vmItemGroup> itemgroups = null;
            try
            {
                itemgroups = (from olt in GenericFactoryFor_ItemGroup.FindBy(olt => olt.CompanyID == CompanyID && olt.ItemTypeID == ItemType && olt.IsDeleted == false).ToList()
                              //orderby olt.ItemTypeID ascending
                              select new vmItemGroup
                              {
                                  ItemGroupID = olt.ItemGroupID,
                                  ItemGroupName = olt.ItemGroupName

                              }).ToList();
            }
            catch (Exception)
            {
                itemgroups = null;

            }
            return itemgroups;
        }
        public vmArticleDetail GetArticleDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID)
        {

            GenericFactoryFor_ArticleDetails = new ArticleDeails_EF();
            vmArticleDetail _objArticleDetail = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ItemID", ItemID);
                    ht.Add("CompanyID", CompanyID);
                    spQuery = "[LSGetArticleDetails]";
                    _objArticleDetail = GenericFactoryFor_ArticleDetails.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objArticleDetail;

        }
        public List<vmGrade> GetGrades(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_CmnGrade = new CmnGrade_EF();
            List<vmGrade> _grades = null;
            _grades = (from olt in GenericFactoryFor_CmnGrade.FindBy(olt => olt.IsDeleted == false).ToList()
                       select new vmGrade
                          {
                              GradeName = olt.GradeName,
                              ItemGradeID = olt.ItemGradeID

                          }).ToList();

            return _grades;

        }
        public vmLSCurrentStock GetCurrentStock(vmSLCmnParameters objcmnParam)
        {
            GenericFactory_vmLSCurrentStock = new VmSLCurrentStock_VM();

            vmLSCurrentStock CurrentStock = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.CompanyID);
                    ht.Add("ItemID", objcmnParam.ItemID);
                    ht.Add("DepartmentID", objcmnParam.DepartmentID);
                    ht.Add("SupplierID", objcmnParam.SupplierID);
                    ht.Add("BatchID", objcmnParam.BatchID);
                    ht.Add("GradeID", objcmnParam.GradeID);
                    ht.Add("LotID", objcmnParam.LotID);

                    spQuery = "[Get_LSCurrentStock]";
                    CurrentStock = GenericFactory_vmLSCurrentStock.ExecuteQuerySingle(spQuery, ht);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return CurrentStock;
        }
        public List<vmItemType> GetItemTypeWithoutFinsihGood(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemType)
        {
            GenericFactory_vmItemType = new VmItemType_VM();
            List<vmItemType> itmeTypes = null;
            itmeTypes = (from olt in GenericFactory_vmItemType.FindBy(x => x.IsDeleted == false && x.ItemTypeID != ItemType)
                         select new vmItemType
                       {
                           ItemTypeID = olt.ItemTypeID,
                           ItemTypeName = olt.ItemTypeName

                       }).ToList();

            return itmeTypes;
        }
        public IEnumerable<vmItem> GetItemMasterByTypeID(vmCmnParameters objcmnParam, string TypeId, out int recordsTotal)
        {
            int itemTypeId = Convert.ToInt32(TypeId);
            IEnumerable<vmItem> objItemMaster = null;
            IEnumerable<vmItem> objItemMasterWithoutPaging = null;

            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objItemMasterWithoutPaging = (from item in _ctxCmn.CmnItemMasters //GenericFactory_EF_CmnItemMaster.GetAll()

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
                                        .Where(m => m.IsDeleted == false
                                               && m.CompanyID == objcmnParam.selectedCompany
                                              && itemTypeId == 0 ? true : m.ItemTypeID == itemTypeId
                                               ).ToList();

                    objItemMaster = objItemMasterWithoutPaging.OrderBy(x => x.ItemID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objItemMasterWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();

                }
            }
            return objItemMaster;
        }
        public vmSLDemageSale GetArticleDetailsforDemageSale(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? ItemID)
        {
            long lItemID = Convert.ToInt64(ItemID);
            vmSLDemageSale objDetail = null;
            using (_ctxCmn = new ERP_Entities())
            {
                objDetail = (from im in _ctxCmn.CmnItemMasters
                             join UOM in _ctxCmn.CmnUOMs on im.UOMID equals UOM.UOMID into ps
                             from _UOM in ps.DefaultIfEmpty()
                             where im.ItemID == ItemID
                             select new
                                       {
                                           ItemID = im.ItemID,
                                           StockCode = im.ArticleNo,
                                           ItemName = im.ItemName,
                                           Description = im.Description,
                                           CuttableWidth = im.CuttableWidth,
                                           Lot = (from CB in _ctxCmn.CmnLots
                                                  join SM in _ctxCmn.InvStockMasters on CB.LotID equals SM.LotID
                                                  where SM.ItemID == lItemID
                                                  select new vmLot { LotID = CB.LotID, LotNo = CB.LotNo }).ToList(),

                                           Supplier = (from CU in _ctxCmn.CmnUsers
                                                       join SM in _ctxCmn.InvStockMasters on CU.UserID equals SM.SupplierID
                                                       where SM.ItemID == ItemID
                                                       select new vmSLSupplier { SupplierID = SM.SupplierID, SupplierName = CU.UserFullName }).ToList(),

                                           Batch = (from CB in _ctxCmn.CmnBatches
                                                    join SM in _ctxCmn.InvStockMasters on CB.BatchID equals SM.BatchID
                                                    where SM.ItemID == ItemID
                                                    select new vmSLBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).ToList(),

                                           Unit = _UOM.UOMName,
                                           UnitID = im.UOMID,
                                           Status = "New"

                                       }).ToList().Select(x => new vmSLDemageSale
                                       {
                                           ItemID = x.ItemID,
                                           StoreCode = x.StockCode,
                                           ItemName = x.ItemName,
                                           Description = x.Description,
                                           Lots = x.Lot,
                                           Suppliers = x.Supplier,
                                           Batchs = x.Batch,
                                           Unit = x.Unit,
                                           UnitID = x.UnitID,
                                           Status = x.Status,
                                           CuttableWidth = x.CuttableWidth
                                       }).FirstOrDefault();


            }
            return objDetail;
        }
        public string SaveUpdateSalesInvoice(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails, List<vmLSalesInvoiceDetail> DeleteSalesInvoiceDetails)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (Master.SIID > 0)
                    {
                        UpdateSalesInvoice(Master, SalesInvoiceDetails, DeleteSalesInvoiceDetails);

                    }
                    else
                    {
                        SaveSalesInvoice(Master, SalesInvoiceDetails);
                    }
                    transaction.Complete();
                    result = "1";
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return result;
        }

        private void UpdateSalesInvoice(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails, List<vmLSalesInvoiceDetail> DeleteSalesInvoiceDetails)
        {
            int Masterresult = UpdateSalPPBillingMaster(Master);
            if (Masterresult > 0)
            {
                UpdateSalSalesInvoiceDetail(Master, SalesInvoiceDetails, DeleteSalesInvoiceDetails);
            }
        }

        private void UpdateSalSalesInvoiceDetail(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails, List<vmLSalesInvoiceDetail> DeleteSalesInvoiceDetails)
        {

            List<vmLSalesInvoiceDetail> InsertList = new List<vmLSalesInvoiceDetail>();
            List<vmLSalesInvoiceDetail> UpdatetList = new List<vmLSalesInvoiceDetail>();
            List<SalSalesInvoiceDetail> DelteList=new List<SalSalesInvoiceDetail>();
            try
            {
                if (DeleteSalesInvoiceDetails.Count > 0)
                {
                    vmCmnParameters _cmnParameters = new vmCmnParameters();
                    foreach (vmLSalesInvoiceDetail aitem in DeleteSalesInvoiceDetails)
                    {                       
                        SalSalesInvoiceDetail _salesInvocieDetails = new SalSalesInvoiceDetail();
                        _salesInvocieDetails.SIDetailID = aitem.SIDetailID;
                        DelteList.Add(_salesInvocieDetails);
                        _cmnParameters.loggeduser = Master.CreateBy ?? 0;
                        
                    }
                    DeleteSalSalesInvoiceDetail(DelteList, _cmnParameters);
                }
                if (SalesInvoiceDetails.Count > 0)
                {
                    InsertList = SalesInvoiceDetails.Where(x => x.Status == "New").ToList();
                    UpdatetList = SalesInvoiceDetails.Where(x => x.Status == "Update").ToList();
                    SaveSalesInvoiceDetails(Master, InsertList);
                    UpdateSalesInvoiceMaster(Master, UpdatetList);
                }
            }
            catch
            {


            }
        }

        private void UpdateSalesInvoiceMaster(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> UpdatetList)
        {
            GFactory_EF_SalSalesInvoiceDetails = new SalSalesInvoiceDetail_EF();
            foreach (vmLSalesInvoiceDetail aitem in UpdatetList)
            {
                SalSalesInvoiceDetail _aitem = GFactory_EF_SalSalesInvoiceDetails.FindBy(x => x.SIDetailID == aitem.SIDetailID).FirstOrDefault();
                _aitem.SIID = Master.SIID;
                _aitem.ItemID = aitem.ItemID;
                _aitem.BatchID = aitem.BatchID;
                _aitem.LotID = aitem.LotID;
                _aitem.GradeID = aitem.GradeID;
                _aitem.SupplierID = aitem.SupplierID;
                _aitem.UnitID = aitem.UnitID;
                _aitem.UnitPrice = aitem.UnitPrice;
                _aitem.Qty = aitem.Qty;
                _aitem.Amount = aitem.Amount;
                _aitem.Remarks = aitem.Remarks;
                _aitem.CompanyID = Master.CompanyID;
                _aitem.UpdateBy = Master.CreateBy;
                _aitem.UpdateOn = DateTime.Today;
                _aitem.UpdatePc = HostService.GetIP();
                GFactory_EF_SalSalesInvoiceDetails.Update(_aitem);
                GFactory_EF_SalSalesInvoiceDetails.Save();

            }


        }

        private int UpdateSalPPBillingMaster(SalSalesInvoiceMaster Master)
        {
            int result = 0;

            try
            {
                GFactory_EF_SalSalesInvoiceMaster = new SalSalesInvoiceMaster_EF();
                SalSalesInvoiceMaster _salesInvoiceMaster = GFactory_EF_SalSalesInvoiceMaster.FindBy(x => x.SIID == Master.SIID).FirstOrDefault();
                _salesInvoiceMaster.UpdateOn = DateTime.Today;
                _salesInvoiceMaster.UpdatePc = HostService.GetIP();
                _salesInvoiceMaster.UpdateBy = Master.UserID;
                GFactory_EF_SalSalesInvoiceMaster.Update(_salesInvoiceMaster);
                GFactory_EF_SalSalesInvoiceMaster.Save();
                result = 1;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        private void SaveSalesInvoice(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails)
        {
            long SIID = SaveSalesInvoiceMaster(Master);
            if (SIID > 0)
            {
                Master.SIID = SIID;
                SaveSalesInvoiceDetails(Master, SalesInvoiceDetails);
            }
        }

        private void SaveSalesInvoiceDetails(SalSalesInvoiceMaster Master, List<vmLSalesInvoiceDetail> SalesInvoiceDetails)
        {
            GFactory_EF_SalSalesInvoiceDetails = new SalSalesInvoiceDetail_EF();
            long SIDetailID = Convert.ToInt16(GFactory_EF_SalSalesInvoiceDetails.getMaxID("SalSalesInvoiceDetail"));
            foreach (vmLSalesInvoiceDetail aitem in SalesInvoiceDetails)
            {
                SalSalesInvoiceDetail _aitem = new SalSalesInvoiceDetail();
                _aitem.SIDetailID = SIDetailID;
                _aitem.SIID = Master.SIID;
                _aitem.ItemID = aitem.ItemID;
                _aitem.BatchID = aitem.BatchID;
                _aitem.LotID = aitem.LotID;
                _aitem.GradeID = aitem.GradeID;
                _aitem.SupplierID = aitem.SupplierID;
                _aitem.UnitID = aitem.UnitID;
                _aitem.UnitPrice = aitem.UnitPrice;
                _aitem.Qty = aitem.Qty;
                _aitem.Amount = aitem.Amount;
                _aitem.Remarks = aitem.Remarks;
                _aitem.CompanyID = Master.CompanyID;
                _aitem.CreateBy = Master.CreateBy;
                _aitem.CreateOn = DateTime.Today;
                _aitem.CreatePc = HostService.GetIP();
                _aitem.IsDeleted = false;
                GFactory_EF_SalSalesInvoiceDetails.Insert(_aitem);
                GFactory_EF_SalSalesInvoiceDetails.Save();
                SIDetailID++;
            }
            GFactory_EF_SalSalesInvoiceDetails.updateMaxID("SalSalesInvoiceDetail", Convert.ToInt64(SIDetailID));
        }

        private long SaveSalesInvoiceMaster(SalSalesInvoiceMaster Master)
        {
            long SIID = 0;
            GFactory_EF_SalSalesInvoiceMaster = new SalSalesInvoiceMaster_EF();
            SIID = Convert.ToInt16(GFactory_EF_SalSalesInvoiceMaster.getMaxID("SalSalesInvoiceMaster"));
            Master.SIID = SIID;
            Master.CreateOn = DateTime.Today;
            Master.CreatePc = HostService.GetIP();
            Master.IsDeleted = false;
            GFactory_EF_SalSalesInvoiceMaster.Insert(Master);
            GFactory_EF_SalSalesInvoiceMaster.Save();
            GFactory_EF_SalSalesInvoiceMaster.updateMaxID("SalSalesInvoiceMaster", Convert.ToInt64(SIID));
            return SIID;
        }

        public List<vmSLDemageSale> LoadSalesListForUpdate(vmCmnParameters objcmnParam)
        {

            List<vmSLDemageSale> objDetails = null;
            using (_ctxCmn = new ERP_Entities())
            {
                objDetails = (from im in _ctxCmn.SalSalesInvoiceMasters
                              join salDe in _ctxCmn.SalSalesInvoiceDetails on im.SIID equals salDe.SIID
                              join itMaster in _ctxCmn.CmnItemMasters on salDe.ItemID equals itMaster.ItemID
                              join UOM in _ctxCmn.CmnUOMs on salDe.UnitID equals UOM.UOMID into ps
                              from _UOM in ps.DefaultIfEmpty()
                              where im.SIID == objcmnParam.id && salDe.IsDeleted==false
                              select new
                              {
                                  SIDetailID=salDe.SIDetailID,
                                  ItemID = salDe.ItemID,
                                  StockCode = itMaster.ArticleNo,
                                  ItemName = itMaster.ItemName,
                                  Description = itMaster.Description,
                                  Lot = (from CB in _ctxCmn.CmnLots
                                         join SM in _ctxCmn.InvStockMasters on CB.LotID equals SM.LotID
                                         where SM.ItemID == salDe.ItemID
                                         select new vmLot { LotID = CB.LotID, LotNo = CB.LotNo }).ToList(),

                                  Supplier = (from CU in _ctxCmn.CmnUsers
                                              join SM in _ctxCmn.InvStockMasters on CU.UserID equals SM.SupplierID
                                              where SM.ItemID == salDe.ItemID
                                              select new vmSLSupplier { SupplierID = SM.SupplierID, SupplierName = CU.UserFullName }).ToList(),

                                  Batch = (from CB in _ctxCmn.CmnBatches
                                           join SM in _ctxCmn.InvStockMasters on CB.BatchID equals SM.BatchID
                                           where SM.ItemID == salDe.ItemID
                                           select new vmSLBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).ToList(),

                                  Unit = _UOM.UOMName,
                                  UnitID = salDe.UnitID,
                                  Status = "Update",
                                  LotID = salDe.LotID,
                                  SupplierID = salDe.SupplierID,
                                  BatchID = salDe.BatchID,
                                  GradeID = salDe.GradeID,
                                  //CurrentStock=salDe.cur
                                  UnitPrice = salDe.UnitPrice,
                                  Qty = salDe.Qty,
                                  Amount = salDe.Amount,
                                  CuttableWidth = itMaster.CuttableWidth,
                              }).ToList().Select(x => new vmSLDemageSale
                             {
                                 ItemID = x.ItemID,
                                 StoreCode = x.StockCode,
                                 ItemName = x.ItemName,
                                 Description = x.Description,
                                 Lots = x.Lot,
                                 Suppliers = x.Supplier,
                                 Batchs = x.Batch,
                                 Unit = x.Unit,
                                 UnitID = x.UnitID,
                                 Status = x.Status,
                                 LotID = x.LotID,
                                 SupplierID = x.SupplierID,
                                 BatchID = x.BatchID,
                                 GradeID = x.GradeID,
                                 UnitPrice = x.UnitPrice,
                                 Qty = x.Qty,
                                 Amount = x.Amount,
                                 CuttableWidth = x.CuttableWidth,
                                 SIDetailID=x.SIDetailID

                             }).ToList();


            }
            return objDetails;

        }

        public List<vmLSalesInvoiceMaster> GetSalesInvoiceDetails(vmCmnParameters objcmnParam, out int recordsTotal)
        {

            List<vmLSalesInvoiceMaster> objSalesInvoiceMaster = null;
            List<vmLSalesInvoiceMaster> objSalesInvoiceMasterWithoutPaging = null;

            recordsTotal = 0;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    objSalesInvoiceMasterWithoutPaging = (from item in _ctxCmn.SalSalesInvoiceMasters
                                                          join buyers in _ctxCmn.CmnUsers on item.BuyerID equals buyers.UserID into ps
                                                          from buyers in ps.DefaultIfEmpty()

                                                          join SalPerson in _ctxCmn.CmnUsers on item.UserID equals SalPerson.UserID into salperson
                                                          from SalPerson in salperson.DefaultIfEmpty()

                                                          where item.CompanyID == objcmnParam.loggedCompany && item.SITypeID == objcmnParam.SITypeID
                                                          && item.IsDeleted == false
                                                          select new
                                                          {
                                                              SINo = item.SINo,
                                                              SIID = item.SIID,
                                                              BuyerName = buyers.UserFullName,
                                                              BuyerID = buyers.UserID,
                                                              DODate = item.DODate,
                                                              SIDate = item.SIDate,
                                                              UserID = item.UserID,
                                                              SalPerson = SalPerson.UserFullName,
                                                              Remarks = item.Remarks
                                                          }).ToList()
                                        .Select(x => new vmLSalesInvoiceMaster
                                        {
                                            SINo = x.SINo,
                                            SIID = x.SIID,
                                            BuyerName = x.BuyerName,
                                            BuyerID = x.BuyerID,
                                            DODate = x.DODate,
                                            SIDate = x.SIDate,
                                            SalesPerson = x.SalPerson,
                                            Remarks = x.Remarks,
                                            UserID = x.UserID

                                        }).ToList();

                    objSalesInvoiceMaster = objSalesInvoiceMasterWithoutPaging.OrderBy(x => x.SIID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objSalesInvoiceMasterWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();

                }
            }
            return objSalesInvoiceMaster;
        }
        public string DeleteSalesInvoice(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {

                int res = DeleteSalSalesInvoiceMaster(objcmnParam);
                if (res == 1)
                {
                    DeleteSalSalesInvoiceDetail(objcmnParam);
                }
                transaction.Complete();
                result = "1";
            }
            return result;
        }

        private void DeleteSalSalesInvoiceDetail(vmCmnParameters objcmnParam)
        {           
            List<SalSalesInvoiceDetail> _SalesInvoiceDetails = GFactory_EF_SalSalesInvoiceDetails.FindBy(x => x.SIID == objcmnParam.id).ToList();
            DeleteSalSalesInvoiceDetail(_SalesInvoiceDetails, objcmnParam);
           
        }

        private void DeleteSalSalesInvoiceDetail(List<SalSalesInvoiceDetail> _SalesInvoiceDetails, vmCmnParameters objcmnParam)
        {
            GFactory_EF_SalSalesInvoiceDetails = new SalSalesInvoiceDetail_EF();
            List<SalSalesInvoiceDetail> UpdateList = new List<SalSalesInvoiceDetail>();
            foreach (SalSalesInvoiceDetail aitem in _SalesInvoiceDetails)
            {
                SalSalesInvoiceDetail _salesInvoiceDetail = GFactory_EF_SalSalesInvoiceDetails.FindBy(x => x.SIDetailID == aitem.SIDetailID).FirstOrDefault();
                _salesInvoiceDetail.DeleteOn = DateTime.Today;
                _salesInvoiceDetail.DeleteBy = objcmnParam.loggeduser;
                _salesInvoiceDetail.DeletePc = HostService.GetIP();
                _salesInvoiceDetail.IsDeleted = true;
                UpdateList.Add(_salesInvoiceDetail);
            }
            GFactory_EF_SalSalesInvoiceDetails.UpdateList(UpdateList);
            GFactory_EF_SalSalesInvoiceDetails.Save();
        }

        private int DeleteSalSalesInvoiceMaster(vmCmnParameters objcmnParam)
        {
            GFactory_EF_SalSalesInvoiceMaster = new SalSalesInvoiceMaster_EF();
            SalSalesInvoiceMaster _SalesInvoiceMaster = GFactory_EF_SalSalesInvoiceMaster.FindBy(x => x.SIID == objcmnParam.id).FirstOrDefault();
            _SalesInvoiceMaster.DeleteOn = DateTime.Today;
            _SalesInvoiceMaster.DeleteBy = objcmnParam.loggeduser;
            _SalesInvoiceMaster.DeletePc = HostService.GetIP();
            _SalesInvoiceMaster.IsDeleted = true;
            GFactory_EF_SalSalesInvoiceMaster.Update(_SalesInvoiceMaster);
            GFactory_EF_SalSalesInvoiceMaster.Save();
            return 1;
        }

    }
}

