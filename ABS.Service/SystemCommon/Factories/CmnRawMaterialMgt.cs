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
    public class CmnRawMaterialMgt : iCmnRawMaterial
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_RawMaterial = null;
        private iGenericFactory_EF<CmnItemGroup> GenericFactory_EF_ItemGroup = null;
        private iGenericFactory<vmFinishGood> GenericFactoryFor_FabricDevelopment = null;
        private iGenericFactory_EF<CmnACCIntegration> GenericFactory_CmnACCIntegration = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactory_EF_CmnItemMaster = null;
        private iGenericFactory_EF<CmnACCIntegration> GenericFactoryEF_CmnACCIntegration = null;

        /// No CompanyID Provided
        public string SaveRowMaterial(CmnItemMaster model, int accDetailID, int ACTypeID)
        {
            GenericFactory_EF_RawMaterial = new CmnItemMaster_EF();
            GenericFactory_CmnACCIntegration = new CmnACCIntegration_EF();
            string result = "";
            try
            {
                GenericFactory_EF_CmnItemMaster = new CmnItemMaster_EF();
                //Int64 NextId = GenericFactory_EF_RawMaterial.getMaxVal_int64("ItemID", "CmnItemMaster");
                Int64 NextId = Convert.ToInt64(GenericFactory_EF_CmnItemMaster.getMaxID("CmnItemMaster"));
                model.ItemID = NextId;
                string ArticleNo = SetArticaleNo(model.ItemGroupID);
                model.ArticleNo = ArticleNo;
                string UniqueCode = model.ItemTypeID.ToString() + model.ItemGroupID.ToString() + model.ItemID.ToString();
                model.UniqueCode = UniqueCode;
                model.CreateOn = DateTime.Today;
                model.CreatePc =  HostService.GetIP();
                model.IsDeleted = false;
                GenericFactory_EF_RawMaterial.Insert(model);
                GenericFactory_EF_RawMaterial.Save();
                GenericFactory_EF_CmnItemMaster.updateMaxID("CmnItemMaster", NextId);
                result = ArticleNo + "," + UniqueCode;

                //For insert into CmnAccIntegration

                CmnACCIntegration modelAccIn = new CmnACCIntegration();
                //Int64 NextIdAccIn = GenericFactory_CmnACCIntegration.getMaxVal_int64("ACID", "CmnACCIntegration");

                GenericFactoryEF_CmnACCIntegration = new CmnACCIntegration_EF();
                Int64 NextIdAccIn = Convert.ToInt64(GenericFactoryEF_CmnACCIntegration.getMaxID("CmnACCIntegration"));
                modelAccIn.ACID = NextIdAccIn;
                modelAccIn.AcDetailID = accDetailID;
                modelAccIn.ACTypeID = ACTypeID;
                modelAccIn.CompanyID = model.CompanyID;
                modelAccIn.CreateBy = model.CreateBy;
                modelAccIn.CreateOn = DateTime.Today;
                modelAccIn.CreatePc = HostService.GetIP();
                modelAccIn.IsDeleted = false;
                modelAccIn.IsActive = true;
                modelAccIn.TransactionID = model.ItemID;

                GenericFactory_CmnACCIntegration.Insert(modelAccIn);
                GenericFactory_CmnACCIntegration.Save();
                GenericFactoryEF_CmnACCIntegration.updateMaxID("CmnACCIntegration", NextIdAccIn);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;

        }

        /// No CompanyID Provided
        private string SetArticaleNo(int? ItemGroup)
        {
            GenericFactory_EF_ItemGroup = new CmnItemGroup_EF();
            GenericFactory_EF_RawMaterial = new CmnItemMaster_EF();
            string articaleNo = string.Empty;

            try
            {
                CmnItemGroup itemGroup = GenericFactory_EF_ItemGroup.FindBy(x => x.ItemGroupID == ItemGroup).FirstOrDefault();
                // int count = GenericFactory_EF_RawMaterial.GetAll().Where(x => x.ItemGroupID == ItemGroup).Count();
                int count = GenericFactory_EF_RawMaterial.FindBy(x => x.ItemGroupID == ItemGroup).Count();
                Int64 itemcode = Convert.ToInt64(itemGroup.CustomCode) + count + 1;
                articaleNo = itemcode.ToString();
                //CmnItemGroup itemGroup = GenericFactory_EF_ItemGroup.FindBy(x => x.ItemGroupID == ItemGroup).FirstOrDefault();
                //List<CmnItemMaster> _ItemMasterobj = GenericFactory_EF_RawMaterial.GetAll().Where(x => x.ItemGroupID == ItemGroup).ToList();

                //string exitemGroup = itemGroup.CustomCode;
                //int rowNumber = (from item in _ItemMasterobj
                //                 where item.ArticleNo.Contains(exitemGroup)
                //                 select item.ArticleNo).Count();
                //if (rowNumber == 0)
                //{
                //    articaleNo = exitemGroup + "00000";
                //    articaleNo = (Convert.ToInt64(articaleNo) + 1).ToString();
                //}
                //else
                //{
                //    articaleNo = exitemGroup + "00000";
                //    Int64? Nnum = (Convert.ToInt64(articaleNo) + rowNumber) + 1;
                //    articaleNo = Nnum.ToString();
                //}  
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return articaleNo;
        }

        public List<vmFinishGood> GetAllRowMaterial(vmCmnParameters objcmnParam, out int recordsTotal)
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
                    ht.Add("ItemTypeID", objcmnParam.ItemType);

                    spQuery = "[SPGetItemDetail]";
                    _objvmItemGroup = GenericFactoryFor_FabricDevelopment.ExecuteQuery(spQuery, ht).ToList();

                    recordsTotal = _ctxCmn.CmnItemMasters.Where(x => x.ItemTypeID == objcmnParam.ItemType).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _objvmItemGroup;
        }

        public int DeleteRawMaterial(CmnItemMaster model)
        {
            GenericFactory_EF_RawMaterial = new CmnItemMaster_EF();
            int result = 0;

            try
            {
                CmnItemMaster _model = GenericFactory_EF_RawMaterial.GetAll().Where(x => x.ItemID == model.ItemID).FirstOrDefault();
                _model.DeleteOn = DateTime.Today;
                _model.DeleteBy = model.DeleteBy;
                _model.DeletePc =  HostService.GetIP();
                _model.IsDeleted = true;
                GenericFactory_EF_RawMaterial.Update(_model);
                GenericFactory_EF_RawMaterial.Save();

                result = 1;

                // For delete into CmnAccIntegration

                CmnACCIntegration _modelACCIn = GenericFactory_CmnACCIntegration.GetAll().Where(x => x.TransactionID == model.ItemID).FirstOrDefault();
                _modelACCIn.DeleteOn = DateTime.Today;
                _modelACCIn.DeleteBy = model.DeleteBy;
                _modelACCIn.DeletePc =  HostService.GetIP();
                _modelACCIn.IsDeleted = true;
                GenericFactory_CmnACCIntegration.Update(_modelACCIn);
                GenericFactory_CmnACCIntegration.Save();


            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
        }

        /// CompanyID Provided but Static
        public vmFinishGood GetRawMaterial(int id, int typeId,int companyId)
        {
            GenericFactoryFor_FabricDevelopment = new FabricDevelopment_EF();
            vmFinishGood _objFinishGood = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", companyId);
                    ht.Add("LoggedUser", 0);

                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("ItemTypeID", typeId);
                    ht.Add("ItemGroupID", 0);
                    ht.Add("ItemID", id);
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
        public int UpdateRawMaterial(CmnItemMaster model)
        {
            GenericFactory_EF_RawMaterial = new CmnItemMaster_EF();
            int result = 0;

            try
            {
                CmnItemMaster _model = GenericFactory_EF_RawMaterial.GetAll().Where(x => x.ItemID == model.ItemID).FirstOrDefault();

                //string ArticleNo = SetArticaleNo(model.ItemGroupID);
               // _model.ArticleNo = ArticleNo;
                _model.UpdateOn = DateTime.Today;
                _model.UpdateBy = model.UpdateBy;
                _model.UpdatePc =  HostService.GetIP();
               // _model.ItemTypeID = model.ItemTypeID;
               // _model.ItemGroupID = model.ItemGroupID;
                _model.ItemName = model.ItemName;
                _model.UOMID = model.UOMID;
                _model.ItemColorID = model.ItemColorID;
                _model.ItemSizeID = model.ItemSizeID;
                _model.ItemBrandID = model.ItemBrandID;
                _model.ItemModelID = model.ItemModelID;
                _model.Description = model.Description;
                _model.Note = model.Note;
                GenericFactory_EF_RawMaterial.Update(_model);
                GenericFactory_EF_RawMaterial.Save();

                result = 1;

                // For update into CmnAccIntegration

                CmnACCIntegration _modelACCIn = GenericFactory_CmnACCIntegration.GetAll().Where(x => x.TransactionID == model.ItemID).FirstOrDefault(); 
 
                _modelACCIn.UpdateOn = DateTime.Today;
                _modelACCIn.UpdateBy = model.UpdateBy;
                _modelACCIn.UpdatePc =  HostService.GetIP();
                
                GenericFactory_CmnACCIntegration.Update(_modelACCIn);
                GenericFactory_CmnACCIntegration.Save();

                result = 1;


            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return result;
        }
    }
}
