using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
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
using System.Transactions;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnFinishGoodMgt : iFinishGood
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<RndYarnCR> GenericFactory_EF_YarnCR = null;
        private iGenericFactory_EF<RndYarnCRDetail> GenericFactory_EF_YarnCRDetail = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_CmnItemMaster = null;
        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory_EF<PrdFinishingType> GenericFactory_EF_FinishingType = null;
        private iGenericFactory<vmFinishGood> GenericFactoryFor_FabricDevelopment = null;
        private iGenericFactory<vmConsumption> GenericFactoryFor_Consumption = null;
        private iGenericFactory_EF<RndConsumptionMaster> GenericFactory_EFRndConsumptionMaster = null;
        private iGenericFactory_EF<RndConsumptionDetail> GenericFactory_EFRndConsumptionDetail = null;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocument = null;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocumentUpDate = null;
        private iGenericFactory_EF<CmnACCIntegration> GenericFactoryEF_CmnACCIntegration = null;

        /// No CompanyID Provided
        public Int64 SaveYarn(List<vmYarn> Yarns)
        {
            Int64 yarnId = 0;

            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    Int64 YarnId = SaveRndYarnCRTable(Yarns);
                    yarnId = YarnId;
                    SaveRndYarnCRDetails(Yarns, YarnId);
                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return yarnId;
        }


        public string SaveFinishGood(CmnItemMaster _itemMaster, List<vmFinishProcess> _finishing)
        {
            try
            {
                GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
               //Int64 NextId = GenericFactory_EF_CmnItemMaster.getMaxVal_int64("ItemID", "CmnItemMaster");
                Int64 NextId=Convert.ToInt64(GenericFactory_EF_CmnItemMaster.getMaxID("CmnItemMaster"));
             //   NextId++;
                string ItemGroupName = GetItemGroupName(_itemMaster.ItemGroupID);
            //    int maxGroup = GetMaxByGroupWise(_itemMaster.ItemGroupID, _itemMaster.CompanyID);
                _itemMaster.ArticleNo = _itemMaster.ArticleNo; //ItemGroupName + maxGroup + FinishingProcess;
                _itemMaster.ItemID = NextId;
                _itemMaster.CompanyID = Convert.ToInt16(_itemMaster.CompanyID);
                _itemMaster.IsDeleted = false;
                if(_itemMaster.IsDevelopmentComplete>0)
                _itemMaster.IsDevelopmentComplete = 1;
                _itemMaster.CreateBy = Convert.ToInt16(_itemMaster.CreateBy);
                _itemMaster.CreateOn = DateTime.Today;
                _itemMaster.CreatePc = HostService.GetIP();
                GenericFactory_EF_CmnItemMaster.Insert(_itemMaster);
                GenericFactory_EF_CmnItemMaster.Save();
                GenericFactory_EF_CmnItemMaster.updateMaxID("CmnItemMaster", NextId);
                if (_finishing.Count > 0)
                {
                    SaveFinishingProcess(_finishing, _itemMaster);
                }

                //Account Intigration
                if(_itemMaster.AcDetailID!=0)
                {
                    GenericFactoryEF_CmnACCIntegration = new CmnACCIntegration_EF();
                    Int64 AccMasterID = Convert.ToInt64(GenericFactoryEF_CmnACCIntegration.getMaxID("CmnACCIntegration"));
                    CmnACCIntegration _acc = new CmnACCIntegration();
                    _acc.ACID = AccMasterID;
                    _acc.TransactionID = NextId;
                    _acc.AcDetailID = _itemMaster.AcDetailID;
                    _acc.IsActive = true;
                    _acc.IsDeleted = false;
                    _acc.ACTypeID = 1;
                    _acc.CreateBy = Convert.ToInt16(_itemMaster.CreateBy);
                    _acc.CreateOn = DateTime.Today;
                    _acc.CreatePc = HostService.GetIP();
                    _acc.CompanyID = Convert.ToInt16(_itemMaster.CompanyID);
                    GenericFactoryEF_CmnACCIntegration.Insert(_acc);
                    GenericFactoryEF_CmnACCIntegration.Save();
                    GenericFactoryEF_CmnACCIntegration.updateMaxID("CmnACCIntegration", AccMasterID);

                }
                //Account Intigration
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return _itemMaster.ArticleNo;
        }

        private void SaveFinishingProcess(List<vmFinishProcess> _finishing, CmnItemMaster _itemMaster)
        {
            Int64 FInishTypeID;
            GenericFactory_EF_FinishingType = new PrdFinishingType_EF();
            try
            {

                FInishTypeID =Convert.ToInt64( GenericFactory_EF_FinishingType.getMaxID("PrdFinishingType"));
                foreach (vmFinishProcess aitem in _finishing)
                {
                    PrdFinishingType _prdFinishingType = new PrdFinishingType();
                    _prdFinishingType.FInishTypeID = FInishTypeID;
                    _prdFinishingType.ItemID = _itemMaster.ItemID;
                    _prdFinishingType.FinishingProcessID = aitem.id;
                    _prdFinishingType.IsActive = false;
                    _prdFinishingType.CompanyID = _itemMaster.CompanyID;
                    _prdFinishingType.IsDeleted = false;
                    _prdFinishingType.CreateBy = Convert.ToInt16(_itemMaster.CreateBy);
                    _prdFinishingType.CreateOn = DateTime.Today;
                    _prdFinishingType.CreatePc = HostService.GetIP();
                    GenericFactory_EF_FinishingType.Insert(_prdFinishingType);
                    GenericFactory_EF_FinishingType.Save();
                    FInishTypeID++;
                }
                GenericFactory_EF_FinishingType.updateMaxID("PrdFinishingType", FInishTypeID);
            }
            catch (Exception ex)
            {
                var message = ex.Message.ToString();
            }
        }
        private int GetMaxByGroupWise(int? itemGroup, int companyId)
        {
            int maxId = 0;
            GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
            maxId = GenericFactory_EF_CmnItemMaster.GetAll().Where(x => x.ItemGroupID == itemGroup && x.CompanyID == companyId).ToList().Count();
            return maxId;

        }



        /// No CompanyID Provided
        private string GetFinishingProcessType(long? FInishTypeID)
        {
            GenericFactory_EF_FinishingType = new PrdFinishingType_EF();
            string finishingTypeName = GenericFactory_EF_FinishingType.GetAll().Where(x => x.IsDeleted == false && x.FInishTypeID == FInishTypeID).FirstOrDefault().FInishTypeName;
            return finishingTypeName;

        }

        /// No CompanyID Provided
        private string GetItemGroupName(int? ItemGroupId)
        {
            string ItemName = "";
            try
            {
                GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
                ItemName = GenericFactory_EF_ItemGroup.GetAll().Where(x => x.IsDeleted == false && x.ItemGroupID == ItemGroupId).FirstOrDefault().ItemGroupName;
                return ItemName;

            }
            catch (Exception)
            {


            }
            return ItemName;
        }

        private void SaveRndYarnCRDetails(List<vmYarn> Yarns, Int64 YarnId)
        {
            using (GenericFactory_EF_YarnCRDetail = new RndYarnDetail_EF())
            {
                foreach (vmYarn _aitem in Yarns)
                {
                    RndYarnCRDetail _objRndYarnDetails = new RndYarnCRDetail();
                    Int64 NextId = GenericFactory_EF_YarnCRDetail.getMaxVal_int64("YarnDetailID", "RndYarnCRDetails");
                    _objRndYarnDetails.YarnDetailID = NextId;
                    _objRndYarnDetails.YarnID = YarnId;
                    _objRndYarnDetails.ItemID = _aitem.Yarn;//1; //This value is Constant = 1 (Seed Data)
                    _objRndYarnDetails.LotID = _aitem.LotID;
                    _objRndYarnDetails.Ratio = Convert.ToInt16(_aitem.Ratio);
                    _objRndYarnDetails.CompanyID = Convert.ToInt16(_aitem.CompnayID);
                    _objRndYarnDetails.IsDeleted = false;
                    _objRndYarnDetails.CreateBy = Convert.ToInt16(Yarns[0].LoginID);
                    _objRndYarnDetails.CreateOn = DateTime.Today;
                    _objRndYarnDetails.CreatePc = HostService.GetIP();
                    GenericFactory_EF_YarnCRDetail.Insert(_objRndYarnDetails);
                    GenericFactory_EF_YarnCRDetail.Save();
                }
            }
        }

        /// No CompanyID Provided
        public vmYarn GetYarnBYId(int? yarnId, int? CompanyID)
        {
            vmYarn _yarnCr = null;
            GenericFactory_EF_YarnCR = new RndYarn_EF();
            try
            {
                List<RndYarnCR> _objYarnCr = GenericFactory_EF_YarnCR.FindBy(x => x.IsDeleted == false && x.YarnID == yarnId && x.CompanyID == CompanyID).ToList();
                _yarnCr = (from olt in _objYarnCr
                           orderby olt.YarnID descending
                           select new vmYarn
                           {
                               YarnName = olt.YarnCount,
                               YarnRatio = olt.YarnRatio,
                               YarnType = olt.YarnType,
                               Yarn = (int)olt.YarnID,
                               LotName = olt.YarnRatioLot

                           }).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _yarnCr;
        }

        private Int64 SaveRndYarnCRTable(List<vmYarn> Yarns)
        {
            GenericFactory_EF_YarnCR = new RndYarn_EF();
            string YarnCount = SetYarnCount(Yarns);
            string YarnRatio = SetYarnRatio(Yarns);
            Int64 NextId = GenericFactory_EF_YarnCR.getMaxVal_int64("YarnID", "RndYarnCR");
            RndYarnCR _objRndYarn = new RndYarnCR();
            _objRndYarn.YarnID = NextId;
            _objRndYarn.YarnCount = YarnCount;
            _objRndYarn.YarnRatio = YarnRatio;
            _objRndYarn.YarnType = Yarns[0].YarnType;
            _objRndYarn.YarnRatioLot = SetYarnRatioLot(Yarns);
            _objRndYarn.CompanyID = Convert.ToInt16(Yarns[0].CompnayID);
            _objRndYarn.IsDeleted = false;
            _objRndYarn.CreateBy = Convert.ToInt16(Yarns[0].LoginID);
            _objRndYarn.CreateOn = DateTime.Today;
            _objRndYarn.CreatePc = HostService.GetIP();
            GenericFactory_EF_YarnCR.Insert(_objRndYarn);
            GenericFactory_EF_YarnCR.Save();
            return NextId;
        }

        private string SetYarnRatioLot(List<vmYarn> Yarns)
        {
            string YarnRationLot = "";

            foreach (vmYarn _aitem in Yarns)
            {
                YarnRationLot += _aitem.LotName + "x";
            }
            return YarnRationLot.Remove(YarnRationLot.Length - 1);
        }

        private string SetYarnRatio(List<vmYarn> Yarns)
        {
            string YarnRation = "";

            foreach (vmYarn _aitem in Yarns)
            {
                YarnRation += _aitem.Ratio + ":";
            }
            return YarnRation.Remove(YarnRation.Length - 1);
        }

        private string SetYarnCount(List<vmYarn> Yarns)
        {
            string YarnCount = "";

            foreach (vmYarn _aitem in Yarns)
            {
                YarnCount += _aitem.YarnName + " + ";
            }
            return YarnCount.Remove(YarnCount.Length - 2);
        }

        public List<vmFinishGood> GetFinishGoods(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();
            List<vmFinishGood> _objvmItemGroup = null;
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
                    ht.Add("ItemTypeID", objcmnParam.ItemType);//  1 is Finish Goood 
                    ht.Add("IsDevelopment", 0);
                    spQuery = "[SPGetItemDetail]";
                    _objvmItemGroup = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).ToList();
                    recordsTotal = _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.ItemTypeID==1 && x.IsDeleted == false && x.IsDevelopmentComplete == 0).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objvmItemGroup;
        }

        public List<vmFinishGood> GetFabricDevelopmentList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();
            List<vmFinishGood> _objvmItemGroup = null;
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
                    ht.Add("ItemTypeID", objcmnParam.ItemType);//  1 is Finish Goood 
                    ht.Add("IsDevelopment", 1);
                    spQuery = "[SPGetItemDetail]";
                    _objvmItemGroup = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.ItemTypeID == 1 && x.IsDeleted == false && x.IsDevelopmentComplete == 1).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objvmItemGroup;
        }

        public int DeleteFinishGood(CmnItemMaster _itemMaster)
        {
            GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
            int result = 0;
            try
            {
                CmnItemMaster _ItemMasterobj = GenericFactory_EF_CmnItemMaster.GetAll().Where(x => x.IsDeleted == false && x.ItemID == _itemMaster.ItemID).FirstOrDefault();
                _ItemMasterobj.DeleteOn = DateTime.Today;
                _ItemMasterobj.DeleteBy = _itemMaster.DeleteBy;
                _itemMaster.DeletePc = HostService.GetIP();
                _ItemMasterobj.IsDeleted = true;
                GenericFactory_EF_CmnItemMaster.Update(_ItemMasterobj);
                GenericFactory_EF_CmnItemMaster.Save();
                result = 1;

            }
            catch (Exception)
            {

                result = 0;
            }
            return result;
        }

        /// Static ID Provided
        /// GetFinishGoodById
        public vmFinishGood GetFinishGoodsById(int id)
        {
            vmFinishGood _objFinishGood = null;
            string spQuery = string.Empty;
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", 1);
                    ht.Add("LoggedUser", 0);

                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("ItemTypeID", 1);//  1 is Finish Goood 
                    ht.Add("ItemGroupID", 0);
                    ht.Add("ItemID", id);
                    ht.Add("IsDevelopment", 0);
                    spQuery = "[SPGetItemDetail]";

                    _objFinishGood = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objFinishGood;
        }
        public vmFinishGood GetFabricDevelopmentDetailById(int id)
        {
            vmFinishGood _objFinishGood = null;
            string spQuery = string.Empty;
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", 1);
                    ht.Add("LoggedUser", 0);

                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("ItemTypeID", 1);//  1 is Finish Goood 
                    ht.Add("ItemGroupID", 0);
                    ht.Add("ItemID", id);
                    ht.Add("IsDevelopment", 1);
                    spQuery = "[SPGetItemDetail]";
                  
                    _objFinishGood = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objFinishGood;
        }
        public vmFinishGood GetFabricDevelopmentDetailsByID(int id)
        {
            vmFinishGood _objFinishGood = null;
            string spQuery = string.Empty;
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", 1);
                    ht.Add("LoggedUser", 0);

                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("ItemTypeID", 1);//  1 is Finish Goood 
                    ht.Add("ItemGroupID", 0);
                    ht.Add("ItemID", id);
                    ht.Add("IsDevelopment", 0);
                    spQuery = "[SPGetItemDetail]";

                    _objFinishGood = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objFinishGood;
        }
        /// No CompanyID Provided
        public List<vmConsumption> GetConsumptionInfoByItemID(int id)
        {
            List<vmConsumption> _objConsumptionList = null;
            string spQuery = string.Empty;

            try
            {
                using (GenericFactoryFor_Consumption = new FabricConsumption_EF())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ItemID", id);
                    spQuery = "[Get_ConsumptionCal]";
                    _objConsumptionList = GenericFactoryFor_Consumption.ExecuteQuery(spQuery, ht).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return ConsumptionCal(_objConsumptionList);
        }

        /// No CompanyID Provided
        private List<vmConsumption> ConsumptionCal(List<vmConsumption> _objConsumptionList)
        {
            try
            {
                foreach (vmConsumption aconsumption in _objConsumptionList)
                {
                    if (aconsumption.YarnType == "Warp")
                    {
                        aconsumption.WeightPerUnit = ((aconsumption.TotalEnds * aconsumption.Formula) / aconsumption.YarnCount);
                    }
                    else
                    {
                        int? totalBeam = _objConsumptionList.Where(x => x.YarnType == "Weft").Sum(x => x.BeamRatio);
                        aconsumption.WeightPerUnit = (((aconsumption.NoOfPick / totalBeam) * (aconsumption.BeamRatio * aconsumption.Formula)) / aconsumption.YarnCount);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return _objConsumptionList;
        }

        /// No CompanyID Provided
        public vmFinishGood GetFinishGoodById(int id)
        {
            GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
            try
            {
                List<CmnItemMaster> _ItemMasterobj = GenericFactory_EF_CmnItemMaster.GetAll().Where(x => x.IsDeleted == false && x.ItemTypeID == 1 && x.ItemID == id).ToList();
                vmFinishGood _vmItemMaster = (from olt in _ItemMasterobj
                                              select new vmFinishGood
                                              {
                                                  ItemID = olt.ItemID,
                                                  PDLRef = olt.ArticleNo,
                                                  FiniWtID = olt.FinishingWeightID,
                                                  FiniWt = olt.CmnItemFinishingWeight.FinishingWeigth,
                                                  WarpYarnCount = olt.RndYarnCR == null ? "" : olt.RndYarnCR.YarnCount,
                                                  // WarpYarnCountId = olt.WarpYarnID,
                                                  WarpRatio = olt.RndYarnCR == null ? "" : olt.RndYarnCR.YarnRatio,
                                                  WarpSLlot = "",
                                                  WeftYCount = olt.RndYarnCR1 == null ? "" : olt.RndYarnCR1.YarnCount,
                                                  WeftRatio = olt.RndYarnCR1 == null ? "" : olt.RndYarnCR1.YarnRatio,
                                                  WeftSLlot = "",
                                                  //WeftSuppliter = olt.CmnUser2 == null ? "" : olt.CmnUser2.UserFullName,
                                                  WeftSupplierId = olt.WeftSupplierID,
                                                  GerigeEPIxPPI = olt.EPI + "x" + olt.PPI,
                                                  GPPI = olt.GPPI,
                                                  Color = olt.CmnItemColor == null ? "" : olt.CmnItemColor.ColorName,
                                                  ColorID = olt.ItemColorID,
                                                  Weave = olt.Weave,
                                                  SetNo = olt.SetNo,
                                                  FlangeNo = olt.FlangeNo,
                                                  TotalEnds = olt.TotalEnds,
                                                  // Lengthyds = olt.Length,
                                                  //Buyer = olt.CmnUser == null ? "" : olt.CmnUser.UserFullName,
                                                  //Buyerref = olt.CmnUser1 == null ? "" : olt.CmnUser1.UserFullName,
                                                  BuyerrefId = olt.BuyerRefID,
                                                  Need = olt.Note,
                                                  Remark = olt.Description,
                                                  ItemGroup = olt.CmnItemGroup == null ? "" : olt.CmnItemGroup.ItemGroupName,
                                                  ItemGroupID = olt.ItemGroupID,
                                                  //FinishProcess = olt.PrdFinishingType == null ? "" : olt.PrdFinishingType.FInishTypeName,
                                                  //FinishProcessId = olt.FinishingTypeID,
                                                  CuttableWidth = olt.CuttableWidth,
                                                  HSCODE = olt.HSCODE,
                                                  //WeftYCountId = olt.WeftYarnID,
                                                  WeightPerUnit = olt.WeightPerUnit,
                                                  ItemName = olt.ItemName,
                                                  BuyerID = olt.BuyerID,
                                                  GEPI = olt.GEPI

                                              }).FirstOrDefault();
                return _vmItemMaster;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public int UpdateFinishGood(CmnItemMaster _itemMaster, List<vmFinishProcess> _finishing)
        {
            GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
            int result = 0;
            try
            {
                CmnItemMaster _ItemMasterobj = GenericFactory_EF_CmnItemMaster.FindBy(x => x.IsDeleted == false && x.ItemID == _itemMaster.ItemID).FirstOrDefault();

                _ItemMasterobj.UpdateOn = DateTime.Today;
                _ItemMasterobj.IsDevelopmentComplete = _itemMaster.IsDevelopmentComplete;
                _ItemMasterobj.UpdateBy = _itemMaster.CreateBy;
                _ItemMasterobj.UpdatePc = HostService.GetIP();
                _ItemMasterobj.ArticleNo = _itemMaster.ArticleNo;
                _ItemMasterobj.FinishingWeight = _itemMaster.FinishingWeight;
                _ItemMasterobj.ItemTypeID = _itemMaster.ItemTypeID;
                _ItemMasterobj.WeightPerUnit = _itemMaster.WeightPerUnit;
                _ItemMasterobj.FinishingWeightID = _itemMaster.FinishingWeightID;
                _ItemMasterobj.ItemGroupID = _itemMaster.ItemGroupID;
                _ItemMasterobj.ItemName = _itemMaster.ItemName;
                _ItemMasterobj.WarpYarnID = _itemMaster.WarpYarnID;
                _ItemMasterobj.BuyerID = _itemMaster.BuyerID;
                _ItemMasterobj.WeftYarnID = _itemMaster.WeftYarnID;
                _ItemMasterobj.WeftSupplierID = _itemMaster.WeftSupplierID;
                _ItemMasterobj.PPI = _itemMaster.PPI;
                _ItemMasterobj.ItemColorID = _itemMaster.ItemColorID;
                _ItemMasterobj.EPI = _itemMaster.EPI;
                _ItemMasterobj.GPPI = _itemMaster.GPPI;
                _ItemMasterobj.SetNo = _itemMaster.SetNo;
                _ItemMasterobj.FlangeNo = _itemMaster.FlangeNo;
                _ItemMasterobj.Weave = _itemMaster.Weave;
                _ItemMasterobj.CoatingID = _itemMaster.CoatingID;
                _ItemMasterobj.SPCoatingID = _itemMaster.SPCoatingID;
                _ItemMasterobj.OverDyedID = _itemMaster.OverDyedID;
                _ItemMasterobj.WeftColorID = _itemMaster.WeftColorID;


                //_ItemMasterobj.FinishingTypeID = _itemMaster.FinishingTypeID;
                _ItemMasterobj.Length = _itemMaster.Length;
                _ItemMasterobj.Note = _itemMaster.Note;
                _ItemMasterobj.TotalEnds = _itemMaster.TotalEnds;
                _ItemMasterobj.Description = _itemMaster.Description;
                _ItemMasterobj.BuyerRefID = _itemMaster.BuyerRefID;
                _ItemMasterobj.CuttableWidth = _itemMaster.CuttableWidth;
                _ItemMasterobj.HSCODE = _itemMaster.HSCODE;
                //----------Weaving Info-------------------------------
                _ItemMasterobj.WeavingMachineID = _itemMaster.WeavingMachineID;

                _ItemMasterobj.WeavingDate = _itemMaster.WeavingDate;
                _ItemMasterobj.GWidth = _itemMaster.GWidth;
                _ItemMasterobj.WeavingLength = _itemMaster.WeavingLength;
                _ItemMasterobj.GGSM = _itemMaster.GGSM;
                //--------Finishing Info----------------------------------
                _ItemMasterobj.MinLShrinkage = _itemMaster.MinLShrinkage;
                _ItemMasterobj.MaxLshrinkage = _itemMaster.MaxLshrinkage;
                _ItemMasterobj.MinWshrinkage = _itemMaster.MinWshrinkage;
                _ItemMasterobj.MaxWShrinkage = _itemMaster.MaxWShrinkage;
                _ItemMasterobj.Skew = _itemMaster.Skew;
                _ItemMasterobj.EPI = _itemMaster.EPI;
                _ItemMasterobj.PPI = _itemMaster.PPI;
                _ItemMasterobj.Cotton = _itemMaster.Cotton;
                _ItemMasterobj.Spandex = _itemMaster.Spandex;
                _ItemMasterobj.Polyester = _itemMaster.Polyester;
                _ItemMasterobj.Lycra = _itemMaster.Lycra;
                _ItemMasterobj.FinishingWidth = _itemMaster.FinishingWidth;

                _ItemMasterobj.T4100 = _itemMaster.T4100;
                _ItemMasterobj.Viscos = _itemMaster.Viscos;
                _ItemMasterobj.Modal = _itemMaster.Modal;
                _ItemMasterobj.C4100 = _itemMaster.C4100;
                _ItemMasterobj.Tencel = _itemMaster.Tencel;
                _ItemMasterobj.OtherComp = _itemMaster.OtherComp;
                //-----------------------washing Info--------------------------

                _ItemMasterobj.MinLShrinkageW = _itemMaster.MinLShrinkageW;
                _ItemMasterobj.MaxLshrinkageW = _itemMaster.MaxLshrinkageW;
                _ItemMasterobj.MaxWShrinkageW = _itemMaster.MaxWShrinkageW;
                _ItemMasterobj.MinWshrinkageW = _itemMaster.MinWshrinkageW;
                _ItemMasterobj.SkewW = _itemMaster.SkewW;
                _ItemMasterobj.WEPI = _itemMaster.WEPI;
                _ItemMasterobj.WPPI = _itemMaster.WPPI;
                _ItemMasterobj.WashingWidth = _itemMaster.WashingWidth;
                _ItemMasterobj.WashingWeigth = _itemMaster.WashingWeigth;

                GenericFactory_EF_CmnItemMaster.Update(_ItemMasterobj);
                GenericFactory_EF_CmnItemMaster.Save();
                if (_finishing.Count > 0)
                {
                    UpdateFinishingProcess(_finishing, _itemMaster);
                }

                //Account Intigration
                GenericFactoryEF_CmnACCIntegration = new CmnACCIntegration_EF();

                if (_itemMaster.AcDetailID == 0)
                {
                    CmnACCIntegration _acc = GenericFactoryEF_CmnACCIntegration.FindBy(x => x.TransactionID == _itemMaster.ItemID && x.IsDeleted==false).FirstOrDefault();
                    _acc.DeleteBy = Convert.ToInt16(_itemMaster.CreateBy);
                    _acc.DeleteOn = DateTime.Today;
                    _acc.DeletePc = HostService.GetIP();
                    _acc.IsDeleted = true;
                    GenericFactoryEF_CmnACCIntegration.Update(_acc);
                    GenericFactoryEF_CmnACCIntegration.Save();

                }

                if (_itemMaster.AcDetailID != 0)
                {
                    
                    Int64 AccMasterID = Convert.ToInt64(GenericFactoryEF_CmnACCIntegration.getMaxID("CmnACCIntegration"));
                    CmnACCIntegration _acc = GenericFactoryEF_CmnACCIntegration.FindBy(x => x.TransactionID == _itemMaster.ItemID && x.IsDeleted == false).FirstOrDefault();
                  
                    _acc.AcDetailID = _itemMaster.AcDetailID;
                    _acc.IsActive = true;
                    _acc.IsDeleted = false;
                    _acc.ACTypeID = 1;
                    _acc.UpdateBy = Convert.ToInt16(_itemMaster.CreateBy);
                    _acc.UpdateOn = DateTime.Today;
                    _acc.UpdatePc = HostService.GetIP();

                    GenericFactoryEF_CmnACCIntegration.Update(_acc);
                    GenericFactoryEF_CmnACCIntegration.Save();
                    GenericFactoryEF_CmnACCIntegration.updateMaxID("", AccMasterID);

                }
                //Account Intigration



                result = 1;

            }
            catch (Exception ex)
            {
                
                result = 0;
            }
            return result;
        }

        private void UpdateFinishingProcess(List<vmFinishProcess> _finishing, CmnItemMaster _itemMaster)
        {
            if (_finishing.Count > 0)
            {
                Int64 FInishTypeID;
                GenericFactory_EF_FinishingType = new PrdFinishingType_EF();
                List<PrdFinishingType> matchFinishingTypes = null;
                try
                {

                    foreach (vmFinishProcess aitem in _finishing)
                    {
                        PrdFinishingType _finishingType = GenericFactory_EF_FinishingType.FindBy(x => x.IsDeleted == false && x.ItemID == _itemMaster.ItemID && x.FinishingProcessID == aitem.id).FirstOrDefault();
                        if (_finishingType != null)
                        {
                            matchFinishingTypes.Add(_finishingType);

                        }
                        else
                        {
                            SaveFinishingProcess(_finishing, _itemMaster);
                        }

                    }
                    List<vmFinishProcess> _unMatchFinshingTypes = GetUnMatchFinishingType(matchFinishingTypes, _itemMaster);
                    int result = DeleteFinishingType(_unMatchFinshingTypes, _itemMaster);
                }
                catch
                {


                }
            }
        }

        private int DeleteFinishingType(List<vmFinishProcess> _unMatchFinshingTypes, CmnItemMaster _itemMaster)
        {
            int result = 0;
            GenericFactory_EF_FinishingType = new PrdFinishingType_EF();
            try
            {
                foreach (vmFinishProcess aitem in _unMatchFinshingTypes)
                {
                    PrdFinishingType _finishingType = _ctxCmn.PrdFinishingTypes.Where(x => x.ItemID == _itemMaster.ItemID && x.FinishingProcessID == aitem.id).FirstOrDefault();
                    _finishingType.DeleteBy = _itemMaster.CreateBy;
                    _finishingType.DeleteOn = DateTime.Now;
                    _finishingType.UpdatePc = HostService.GetIP();
                    GenericFactory_EF_FinishingType.Update(_finishingType);
                    GenericFactory_EF_FinishingType.Save();
                }

                result = 1;
            }
            catch
            {
                result = 0;

            }
            return result;
        }

        private List<vmFinishProcess> GetUnMatchFinishingType(List<PrdFinishingType> matchFinishingTypes, CmnItemMaster _itemMaster)
        {
            List<vmFinishProcess> _unMatchFinshingTypes = null;
            List<PrdFinishingType> finishingTypes = _ctxCmn.PrdFinishingTypes.Where(x => x.ItemID == _itemMaster.ItemID).ToList();
            try
            {
                foreach (PrdFinishingType aitem in finishingTypes)
                {
                    PrdFinishingType _prdFinishingType = matchFinishingTypes.Where(x => x.FinishingProcessID == aitem.FinishingProcessID).FirstOrDefault();
                    if (_prdFinishingType == null)
                    {
                        vmFinishProcess _finishProcess = new vmFinishProcess();
                        _finishProcess.id = aitem.FinishingProcessID ?? 0;
                        _unMatchFinshingTypes.Add(_finishProcess);
                    }

                }

            }
            catch
            {


            }
            return _unMatchFinshingTypes;
        }
        public int SaveConsumpiton(List<vmConsumption> _objvmConsumptions, ConsumptionMaster _objConsumptonMaster)
        {
            int result = 0;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EFRndConsumptionMaster = new RndConsumptionMaster_EF();
                GenericFactory_EFRndConsumptionDetail = new RndConsumptionDetail_EF();
                List<RndConsumptionDetail> listDetails = GenericFactory_EFRndConsumptionDetail.FindBy(x => x.ItemID == _objConsumptonMaster.ItemID).ToList();
                List<RndConsumptionMaster> listMaster = GenericFactory_EFRndConsumptionMaster.FindBy(x => x.ItemID == _objConsumptonMaster.ItemID).ToList();
                GenericFactory_EFRndConsumptionDetail.DeleteList(listDetails);
                GenericFactory_EFRndConsumptionDetail.Save();
                GenericFactory_EFRndConsumptionMaster.DeleteList(listMaster);
                GenericFactory_EFRndConsumptionMaster.Save();

                Int64 ConsumptionId = SaveConsumptionMaster(_objConsumptonMaster);
                SaveConsumptionDetails(_objvmConsumptions, ConsumptionId, _objConsumptonMaster.CompanyID, _objConsumptonMaster.CreateBy);
                transaction.Complete();
            }
            return result;

        }
        private long SaveConsumptionDetails(List<vmConsumption> _objvmConsumptions,Int64 masterID,int ComapnyID,int userID)
        {
            Int64 NextId = 0;
            try
            {
                GenericFactory_EFRndConsumptionDetail = new RndConsumptionDetail_EF();
                NextId=Convert.ToInt64(GenericFactory_EFRndConsumptionDetail.getMaxID("RndConsumptionDetail"));
                foreach (vmConsumption item in _objvmConsumptions)
                {
                    
                    RndConsumptionDetail entity = new RndConsumptionDetail();
                    entity.ConsumptionDetailID = NextId++;
                    entity.ConsumptionID = masterID;

                    if (item.YarnType == "Warp")
                    {
                        entity.ConsumptionTypeID = 1;
                    }
                    else
                    {
                        entity.ConsumptionTypeID = 2;
                    }
                     
                    entity.YarnID = item.YarnID;
                    entity.LotID = item.LotID;
                    entity.ItemID = item.ItemID;
                    entity.BeamRatio = item.BeamRatio;
                    entity.NoOFPic = item.NoOfPick;
                    entity.TotalEnds = item.TotalEnds;
                    entity.Count =Convert.ToInt32(item.YarnCount);
                    entity.WeigthPerUnit = item.FinishingWeigth;
                    entity.Remarks = "";

                    entity.CompanyID = ComapnyID;
                    entity.CreateBy = userID;
                    entity.CreateOn = DateTime.Today; 
                    entity.CreatePc = HostService.GetIP();
                    GenericFactory_EFRndConsumptionDetail.Insert(entity);
                    GenericFactory_EFRndConsumptionDetail.Save();
                }
                GenericFactory_EFRndConsumptionDetail.updateMaxID("RndConsumptionDetail", NextId);


            }
            catch (Exception ex)
            {


                throw new Exception(ex.Message.ToString());
            }
            return NextId;
        }
        private long SaveConsumptionMaster(ConsumptionMaster _objConsumptonMaster)
        {
            Int64 NextId = 0;
            GenericFactory_EFRndConsumptionMaster = new RndConsumptionMaster_EF();
            try
            {
                NextId =Convert.ToInt64( GenericFactory_EFRndConsumptionMaster.getMaxID("RndConsumptionMaster"));
                RndConsumptionMaster _objConsumptionMaster = new RndConsumptionMaster();
                _objConsumptionMaster.ConsumptionID = NextId;
                _objConsumptionMaster.Description = _objConsumptonMaster.Description;
                _objConsumptionMaster.Note = _objConsumptonMaster.Note;
                _objConsumptionMaster.ItemID = _objConsumptonMaster.ItemID;
                _objConsumptionMaster.CompanyID = _objConsumptonMaster.CompanyID;
                _objConsumptionMaster.IsDeleted = false;
                _objConsumptionMaster.CreateBy = _objConsumptonMaster.CreateBy;
                _objConsumptionMaster.CreateOn = DateTime.Today;
                _objConsumptionMaster.CreatePc = HostService.GetIP();
                GenericFactory_EFRndConsumptionMaster.Insert(_objConsumptionMaster);
                GenericFactory_EFRndConsumptionMaster.Save();
                GenericFactory_EFRndConsumptionMaster.updateMaxID("RndConsumptionMaster", NextId + 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return NextId;
        }
        public List<vmRndDoc> GetDoclistByItemID (int itemID,int transactionID)
        {
            try
            {
                ERP_Entities _contex = new ERP_Entities();
                return (from doc in _contex.CmnDocuments
                        where doc.TransactionTypeID == transactionID && doc.TransactionID == itemID && doc.IsDeleted == false
                        select new vmRndDoc
                        {
                            DocumentID = doc.DocumentID,
                            DocumentPahtID = doc.DocumentPahtID,
                            DocName = doc.DocName,
                            DocumentName = doc.DocumentName,
                            TransactionID = doc.TransactionID,
                            TransactionTypeID = doc.TransactionTypeID,
                            CompanyID = doc.CompanyID,
                            CreateBy = doc.CreateBy,
                            CreateOn = doc.CreateOn,
                            CreatePc = doc.CreatePc,
                            UpdateBy = doc.UpdateBy,
                            UpdateOn = doc.UpdateOn,
                            UpdatePc = doc.UpdatePc,
                            IsDeleted = doc.IsDeleted,
                            DeleteBy = doc.DeleteBy,
                            DeleteOn = doc.DeleteOn,
                            DeletePc = doc.DeletePc,
                        }
                        ).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int uploadFiles(List<vmRndDoc> _DocList)
        {
            int? transactionTypeID = _DocList.FirstOrDefault().TransactionTypeID;
           
            transactionTypeID = 30;
            ERP_Entities _contex = new ERP_Entities();
            CmnDocumentPath objDocumentPath = _contex.CmnDocumentPaths.Where(x => x.TransactionTypeID == transactionTypeID).FirstOrDefault();

            GenericFactory_CmnDocument = new CmnDocument_EF();
            int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
            List<CmnDocument> lstCmnDocument = new List<CmnDocument>();
            List<CmnDocument> UpdateCmnDocument = new List<CmnDocument>();
            bool isCreated = true;
            foreach (vmRndDoc item in _DocList)
            {
                CmnDocument objCmnDocument = new CmnDocument();
                CmnDocument objCmnDocumentTemp = new CmnDocument(); 
                if (item.DocumentID == 0)
                {
                    isCreated = true;
                    objCmnDocument.DocumentID = DocumentID;
                }
                else
                {
                    isCreated = false;
                    objCmnDocument.DocumentID = item.DocumentID;
                    objCmnDocumentTemp = _contex.CmnDocuments.Where(x => x.DocumentID == item.DocumentID).FirstOrDefault();
                }

                objCmnDocument.DocumentPahtID = objDocumentPath.DocumentPathID;
                objCmnDocument.DocName = item.DocName;
                objCmnDocument.DocumentName = item.DocumentName;
                objCmnDocument.TransactionID = item.TransactionID;
                objCmnDocument.TransactionTypeID = item.TransactionTypeID;
                objCmnDocument.CompanyID = item.CompanyID;
                objCmnDocument.IsDeleted = item.IsDeleted;

                if (isCreated)
                {
                    objCmnDocument.CreateBy = Convert.ToInt16(item.CreateBy);
                    objCmnDocument.CreateOn = DateTime.Now;
                    objCmnDocument.CreatePc = HostService.GetIP();
                }
                else if(item.IsDeleted)
                {

                    objCmnDocument.CreateBy = objCmnDocumentTemp.CreateBy;
                    objCmnDocument.CreateOn = objCmnDocumentTemp.CreateOn;
                    objCmnDocument.CreatePc = objCmnDocumentTemp.CreatePc;
                    objCmnDocument.UpdateBy = objCmnDocumentTemp.UpdateBy;
                    objCmnDocument.UpdateOn = objCmnDocumentTemp.UpdateOn;
                    objCmnDocument.UpdatePc = objCmnDocumentTemp.UpdatePc;

                    objCmnDocument.DeleteBy = Convert.ToInt16(item.CreateBy);
                    objCmnDocument.DeleteOn = DateTime.Now;
                    objCmnDocument.DeletePc = HostService.GetIP();

                }
                else
                {
                    objCmnDocument.CreateBy = objCmnDocumentTemp.CreateBy;
                    objCmnDocument.CreateOn = objCmnDocumentTemp.CreateOn;
                    objCmnDocument.CreatePc = objCmnDocumentTemp.CreatePc;
                    objCmnDocument.UpdateBy = Convert.ToInt16(item.CreateBy);
                    objCmnDocument.UpdateOn = DateTime.Now;
                    objCmnDocument.UpdatePc = HostService.GetIP();
                }
                DocumentID++;
                if (isCreated)
                    lstCmnDocument.Add(objCmnDocument);
                else UpdateCmnDocument.Add(objCmnDocument);
                
            }

            GenericFactory_CmnDocument.InsertList(lstCmnDocument);
            GenericFactory_CmnDocument.Save();
            GenericFactory_CmnDocumentUpDate = new CmnDocument_EF();
            GenericFactory_CmnDocumentUpDate.UpdateList(UpdateCmnDocument);
            GenericFactory_CmnDocumentUpDate.Save();
            GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));
            return DocumentID;
        }

        public List<vmItemGroup> GetAcDetailIDByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID, int? GroupID)
        {
            ERP_Entities _contex = new ERP_Entities();
            return (from grp in _contex.CmnItemGroups
                    where grp.ItemGroupID == GroupID && grp.CompanyID == CompanyID
                    && grp.IsDeleted == false
                    select new vmItemGroup
                    {
                        ItemGroupID = grp.ItemGroupID,
                        AcDetailID = grp.AcDetailID ?? 0

                    }).ToList();

        }
    }
}
