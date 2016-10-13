using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;
using System.Data.Entity;
namespace ABS.Service.SystemCommon.Factories
{
    public class CmnCustomCodeMgt : iCmnCustomCodeMgt,IDisposable
    {
        bool disposed = false;
        private iGenericFactory_EF<CmnCustomCode> GenericFactory_EF_customCode = null;
        private iGenericFactory_EF<CmnCustomCodeDetail> GenericFactory_EF_customDetails = null;

        #region GetAll 
        public List<vmCmnCustomCode> GetAllCustomCode(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmCmnCustomCode> list = new List<vmCmnCustomCode>();
            #region  Change it to Sp
            //List<CmnCustomCode> listEntities = GenericFactory_EF_customCode.GetAll().ToList();
            //foreach (var item in listEntities)
            //{
            //    vmCmnCustomCode ent = new vmCmnCustomCode();
            //    ent.RecordID = item.RecordID;
            //    ent.CompanyID = item.CompanyID;
            //    ent.CompanyName = new CompanyFactory_EF().GetAll().ToList().Find(x => x.CompanyID == item.CompanyID).CompanyName;
            //    ent.MenuID = item.MenuID;
            //    if (item.MenuID.HasValue)
            //        ent.MenuName = new Menu_GF_EF().GetAll().ToList().Find(x => x.MenuID == item.MenuID).MenuName;
            //    ent.OrganogramID = item.OrganogramID;
            //    if (item.OrganogramID.HasValue)
            //        ent.OrganogramName = new Organogram_GF_EF().GetAll().ToList().Find(x => x.OrganogramID == item.OrganogramID).OrganogramName;
            //    ent.Prefix = item.Prefix ?? "";
            //    ent.Suffix = item.Suffix ?? "";
            //    ent.CreatePc = item.CreatePc ?? "";
            //    ent.UpdatePc = item.UpdatePc ?? "";
            //    ent.IsDeleted = item.IsDeleted;
            //    list.Add(ent);
            //}
            #endregion 
            #region Convert It Into Sp
            ERP_Entities dbContext = new ERP_Entities();
            var recordList = (from master in dbContext.CmnCustomCodes
                              join details in dbContext.CmnCustomCodeDetails on master.RecordID equals details.CustomCodeID
                              join company in dbContext.CmnCompanies on master.CompanyID equals company.CompanyID into leftCompanyGroup
                              from lcg in leftCompanyGroup.DefaultIfEmpty()
                              join menu in dbContext.CmnMenus on master.MenuID equals menu.MenuID into leftMenuGroup
                              from lmg in leftMenuGroup.DefaultIfEmpty()
                              join organogram in dbContext.CmnOrganograms on master.OrganogramID equals organogram.OrganogramID into leftOrganogramGroup
                              from log in leftOrganogramGroup.DefaultIfEmpty()
                              where master.IsDeleted == false
                              select new vmCmnCustomCode
                              {
                                  RecordID = master.RecordID,
                                  CompanyID = master.CompanyID,
                                  CompanyName = lcg.CompanyName,
                                  MenuID = lmg.MenuID,
                                  MenuName = lmg.MenuName,
                                  OrganogramID = log.OrganogramID,
                                  OrganogramName = log.OrganogramName ?? string.Empty,
                                  Prefix = master.Prefix,
                                  Suffix = master.Suffix,
                                  IsCompany = master.IsCompany,
                                  IsOrganogramCode = master.IsOrganogramCode

                              }).Distinct().ToList();

            list = recordList.ToList();
            #endregion Convert It Into Sp
            return list;
        }
        #endregion GetAll
        #region GetAll Details By ID
        public List<CmnCustomCodeDetail> GetCustomCodeDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            GenericFactory_EF_customDetails = new CmnCustomCodeDetail_EF();
            return GenericFactory_EF_customDetails.FindBy(x => x.CustomCodeID == DetailsID && x.IsDeleted == false).ToList();
        }
        #endregion GetAll Details By ID

        #region Save
        private CmnCustomCodeDetail DetailsBinding(CmnCustomCodeDetail Item)
        {
            CmnCustomCodeDetail entity = new CmnCustomCodeDetail();
            entity.EntryStateStatus = Item.EntryStateStatus;
            entity.RecordDetailID = Item.RecordDetailID;
            entity.CustomCode = Item.CustomCode;
            entity.CustomCodeID = Item.CustomCodeID;
            entity.ParameterName = Item.ParameterName;
            entity.Length = Item.Length;
            entity.Seperator = Item.Seperator;
            entity.Sequence = Item.Sequence;
            entity.StatusID = Item.StatusID;
            entity.CompanyID = Item.CompanyID;
            entity.CreateBy = Item.CreateBy;
            entity.CreateOn = Item.CreateOn;
            entity.CreatePc = Item.CreatePc;
            entity.UpdateBy = Item.UpdateBy;
            entity.UpdateOn = Item.UpdateOn;
            entity.UpdatePc = Item.UpdatePc;
            entity.IsDeleted = Item.IsDeleted;
            entity.DeleteBy = Item.DeleteBy;
            entity.DeleteOn = Item.DeleteOn;
            entity.DeletePc = Item.DeletePc;
            return entity;
        }
       
        private CmnCustomCode ModelBinding(CmnCustomCode model, List<CmnCustomCodeDetail> details)
        {
            GenericFactory_EF_customCode = new CmnCustomCode_EF();
            try
            {
                int NextID = GenericFactory_EF_customCode.getMaxVal_int("RecordID", "CmnCustomCode");
                model.RecordID = model.RecordID == 0 ? NextID : model.RecordID;
                model.CustomCode = model.RecordID.ToString();
                model.CompanyID = model.CompanyID;
                model.IsCompany = model.IsCompany ?? false;
                model.IsOrganogramCode = model.IsOrganogramCode ?? false;
                model.IsDeleted = false;
                NextID = GenericFactory_EF_customCode.getMaxVal_int("RecordDetailID", "CmnCustomCodeDetail");
                foreach (CmnCustomCodeDetail Item in details)
                {
                    CmnCustomCodeDetail entity = new CmnCustomCodeDetail();
                    entity.RecordDetailID = Item.RecordDetailID;
                    if (Item.RecordDetailID == 0)
                    {
                        if (Item.IsDeleted) continue;
                        Item.RecordDetailID = NextID++;
                        entity.RecordDetailID = Item.RecordDetailID;
                        entity.EntryStateStatus = "Add";
                    }
                    else if (Item.IsDeleted == true)
                    {
                        entity.EntryStateStatus = "Delete";
                    }
                    else
                    {
                        entity.EntryStateStatus = "Modified";
                    }
                    entity.CustomCode = entity.RecordDetailID.ToString();
                    entity.CustomCodeID = model.RecordID;
                    entity.ParameterName = Item.ParameterName;
                    entity.Length = Item.Length;
                    entity.Seperator = Item.Seperator;
                    entity.Sequence = Item.Sequence;
                    entity.StatusID = Item.StatusID;
                    entity.CompanyID = model.CompanyID;
                    entity.CreateBy = Item.CreateBy;
                    entity.CreateOn = Item.CreateOn;
                    entity.CreatePc = Item.CreatePc;
                    entity.UpdateBy = Item.UpdateBy;
                    entity.UpdateOn = Item.UpdateOn;
                    entity.UpdatePc = Item.UpdatePc;
                    if (entity.EntryStateStatus != "Delete")
                        entity.IsDeleted = false;
                    else
                    {
                        entity.EntryStateStatus = "Modified";
                        entity.IsDeleted = true;
                    }
                    entity.DeleteBy = Item.DeleteBy;
                    entity.DeleteOn = Item.DeleteOn;
                    entity.DeletePc = Item.DeletePc;
                    model.CmnCustomCodeDetails.Add(entity);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }
        public int SavCustomCode(CmnCustomCode model, List<CmnCustomCodeDetail> details)
        {
            GenericFactory_EF_customCode = new CmnCustomCode_EF();
            GenericFactory_EF_customDetails = new CmnCustomCodeDetail_EF();
            int masterID = model.RecordID;
            CmnCustomCode modelBase = new CmnCustomCode();
            List<Int32> UpdatedIDs = new List<Int32>();
            List<CmnCustomCodeDetail> detailsListDeleted = new List<CmnCustomCodeDetail>();
            List<CmnCustomCodeDetail> detailsList = new List<CmnCustomCodeDetail>();
            List<CmnCustomCodeDetail> detailsInsertionList = new List<CmnCustomCodeDetail>();
            int result = 0;
            try
            {
                modelBase = ModelBinding(model, details);
                foreach (var item in modelBase.CmnCustomCodeDetails)
                {
                    if (item.EntryStateStatus == "Modified")
                    {
                        CmnCustomCodeDetail detailsEntity = new CmnCustomCodeDetail();
                        detailsEntity = DetailsBinding(modelBase.CmnCustomCodeDetails.Where(x => x.RecordDetailID == item.RecordDetailID).FirstOrDefault());
                        detailsList.Add(detailsEntity);
                        UpdatedIDs.Add(detailsEntity.RecordDetailID);
                    }
                    if (item.EntryStateStatus == "Add" && masterID > 0)
                    {
                        CmnCustomCodeDetail detailsEntity = new CmnCustomCodeDetail();
                        detailsEntity = DetailsBinding(modelBase.CmnCustomCodeDetails.Where(x => x.RecordDetailID == item.RecordDetailID).FirstOrDefault());
                        detailsInsertionList.Add(detailsEntity);
                    }
                }

                foreach (var item in UpdatedIDs)
                {
                    modelBase.CmnCustomCodeDetails.Remove(modelBase.CmnCustomCodeDetails.Where(x => x.RecordDetailID == item).FirstOrDefault());
                }
                if (masterID == 0)
                {
                    GenericFactory_EF_customCode.Insert(modelBase);
                    GenericFactory_EF_customCode.Save();
                }
                else
                {
                    GenericFactory_EF_customCode.Update(model);
                    GenericFactory_EF_customDetails.UpdateList(detailsList);
                    foreach (CmnCustomCodeDetail item in detailsList)
                    {
                        if(item.IsDeleted)
                        {
                            detailsListDeleted.Add(item);
                        }
                        
                    }
                    if (detailsListDeleted.Count > 0)
                    {
                        GenericFactory_EF_customDetails.DeleteList(detailsListDeleted);
                    }
                    GenericFactory_EF_customDetails.InsertList(detailsInsertionList);
                 
                    
                    GenericFactory_EF_customCode.Save();
                    GenericFactory_EF_customDetails.Save();
                    GenericFactory_EF_customDetails = new CmnCustomCodeDetail_EF();
                    
                }

                result = 1;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return result;
            }
            return result;
        }

        public int UpdateMasterStatus(int master)
        {
            GenericFactory_EF_customCode = new CmnCustomCode_EF();
            int result = 0;
            int masterID = master;
            //GenericFactory_EF_customCode = new CmnCustomCode_EF();
            GenericFactory_EF_customDetails = new CmnCustomCodeDetail_EF();
            try
            {
                //List<CmnCustomCode> customcode = new List<CmnCustomCode>();
                //customcode = GenericFactory_EF_customCode.FindBy(x => x.RecordID == master).ToList();
                //customcode.ForEach(x => x.IsDeleted = true);
                //GenericFactory_EF_customCode.UpdateList(customcode);
                //GenericFactory_EF_customCode.Save();
                List<CmnCustomCode> customcode = new List<CmnCustomCode>();
                List<CmnCustomCodeDetail> customcodeDetails = new List<CmnCustomCodeDetail>();
                customcode = GenericFactory_EF_customCode.FindBy(x => x.RecordID == master).ToList();
                customcodeDetails = GenericFactory_EF_customDetails.FindBy(x => x.CustomCodeID == master).ToList();
                GenericFactory_EF_customDetails.DeleteList(customcodeDetails);
                GenericFactory_EF_customCode.DeleteList(customcode);
                GenericFactory_EF_customDetails.Save();
                GenericFactory_EF_customCode.Save();


                result = 1;
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }
        #endregion Save
        #region Dispose
        ~CmnCustomCodeMgt()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
            //Dispose Method
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion Dispose

    }
}

