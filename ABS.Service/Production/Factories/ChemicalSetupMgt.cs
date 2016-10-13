using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
   public class ChemicalSetupMgt: iChemicalSetupMgt
    {
        /// No CompanyID Provided but not in used
        private iGenericFactory_EF<PrdDyingChemicalSetup> GFactory_EF_PrdDyingChemicalSetup = null;
        private iGenericFactory_EF<PrdDyingChemicalSetupDetail> GenericFactory_EF_PrdDyingChemicalSetupDetail = null;
       
        #region GetAll 
        public List<vmChemicalSetup> GetAllList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmChemicalSetup> list = new List<vmChemicalSetup>();
            #region Convert It Into Sp
            ERP_Entities dbContext = new ERP_Entities();
            var recordList = (from master in dbContext.PrdDyingChemicalSetups
                              join details in dbContext.PrdDyingChemicalSetupDetails on master.ChemicalSetupID equals details.ChemicalSetupID
                              join color in dbContext.CmnItemColors on master.ColorID equals color.ItemColorID into leftColorGroup
                              from lcg in leftColorGroup.DefaultIfEmpty()
                            
                              where master.IsDeleted == false
                              select new vmChemicalSetup
                              {
                                  ChemicalSetupID = master.ChemicalSetupID
                                  ,
                                  ColorID = lcg.ItemColorID
                                 

                              }).Distinct().ToList();

            list = recordList.ToList();
            #endregion Convert It Into Sp
            return list;
        }
        #endregion GetAll
        #region Save 
        private PrdDyingChemicalSetupDetail DetailsBinding(PrdDyingChemicalSetupDetail Item)
        {
            PrdDyingChemicalSetupDetail entity = new PrdDyingChemicalSetupDetail();
            entity.EntryStateStatus = Item.EntryStateStatus;
            entity.ChemicalSetupDetailID = Item.ChemicalSetupDetailID;
            entity.ChemicalSetupID = Item.ChemicalSetupID;
            entity.ItemID = Item.ItemID;
            entity.Qty = Item.Qty;
            entity.UnitID = Item.UnitID;
            entity.IsDeleted = Item.IsDeleted;
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

        private PrdDyingChemicalSetup ModelBinding(PrdDyingChemicalSetup model, List<PrdDyingChemicalSetupDetail> details)
        {
            GFactory_EF_PrdDyingChemicalSetup = new PrdDyingChemicalSetup_EF();
            try
            {
                Int64 NextID = GFactory_EF_PrdDyingChemicalSetup.getMaxVal_int64("ChemicalSetupID", "PrdDyingChemicalSetup");
                model.ChemicalSetupID = model.ChemicalSetupID == 0 ? NextID : model.ChemicalSetupID;
                model.ColorID = model.ColorID;
                model.Qty = model.Qty;
                model.DepartmentID = model.DepartmentID;
                model.UserID = model.UserID;
                model.UnitID = model.UnitID;
                model.IsDeleted = false;
                NextID = GFactory_EF_PrdDyingChemicalSetup.getMaxVal_int64("ChemicalSetupDetailID", "PrdDyingChemicalSetupDetail");
                foreach (PrdDyingChemicalSetupDetail Item in details)
                {
                    PrdDyingChemicalSetupDetail entity = new PrdDyingChemicalSetupDetail();
                    entity.ChemicalSetupDetailID = Item.ChemicalSetupDetailID;
                    if (Item.ChemicalSetupDetailID == 0)
                    {
                        if (Item.IsDeleted) continue;
                        Item.ChemicalSetupDetailID = NextID++;
                        entity.ChemicalSetupDetailID = Item.ChemicalSetupDetailID;
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
                    entity.ItemID = Item.ItemID;
                    entity.Qty = Item.Qty;
                    entity.ChemicalSetupID = model.ChemicalSetupID;
                    entity.UnitID = Item.UnitID;
                    entity.IsDeleted = Item.IsDeleted;
                    entity.CompanyID = Item.CompanyID;
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
                    
                    model.PrdDyingChemicalSetupDetails.Add(entity);
                }

                return model;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }
        public int SaveChemicalPrepartion(PrdDyingChemicalSetup model, List<PrdDyingChemicalSetupDetail> details, UserCommonEntity commonEntity)
        {

            GFactory_EF_PrdDyingChemicalSetup = new PrdDyingChemicalSetup_EF();
            GenericFactory_EF_PrdDyingChemicalSetupDetail = new PrdDyingChemicalSetupDetail_EF();
            int masterID =(int)model.ChemicalSetupID;
            PrdDyingChemicalSetup modelBase = new PrdDyingChemicalSetup();
            List<Int32> UpdatedIDs = new List<Int32>();
            List<PrdDyingChemicalSetupDetail> detailsList = new List<PrdDyingChemicalSetupDetail>();
            List<PrdDyingChemicalSetupDetail> detailsInsertionList = new List<PrdDyingChemicalSetupDetail>();
            int result = 0;
            try
            {
                modelBase = ModelBinding(model, details);
                foreach (var item in modelBase.PrdDyingChemicalSetupDetails)
                {
                    if (item.EntryStateStatus == "Modified")
                    {
                        PrdDyingChemicalSetupDetail detailsEntity = new PrdDyingChemicalSetupDetail();
                        detailsEntity = DetailsBinding(modelBase.PrdDyingChemicalSetupDetails.Where(x => x.ChemicalSetupDetailID == item.ChemicalSetupDetailID).FirstOrDefault());
                        detailsList.Add(detailsEntity);
                        UpdatedIDs.Add((int)detailsEntity.ChemicalSetupDetailID);
                    }
                    if (item.EntryStateStatus == "Add" && masterID > 0)
                    {
                        PrdDyingChemicalSetupDetail detailsEntity = new PrdDyingChemicalSetupDetail();
                        detailsEntity = DetailsBinding(modelBase.PrdDyingChemicalSetupDetails.Where(x => x.ChemicalSetupDetailID == item.ChemicalSetupDetailID).FirstOrDefault());
                        detailsInsertionList.Add(detailsEntity);
                    }
                }

                foreach (var item in UpdatedIDs)
                {
                    modelBase.PrdDyingChemicalSetupDetails.Remove(modelBase.PrdDyingChemicalSetupDetails.Where(x => x.ChemicalSetupDetailID == item).FirstOrDefault());
                }
                if (masterID == 0)
                {
                    GFactory_EF_PrdDyingChemicalSetup.Insert(modelBase);
                    GFactory_EF_PrdDyingChemicalSetup.Save();
                }
                else
                {
                    GFactory_EF_PrdDyingChemicalSetup.Update(model);
                    GenericFactory_EF_PrdDyingChemicalSetupDetail.UpdateList(detailsList);
                    GenericFactory_EF_PrdDyingChemicalSetupDetail.InsertList(detailsInsertionList);
                    GFactory_EF_PrdDyingChemicalSetup.Save();
                    GenericFactory_EF_PrdDyingChemicalSetupDetail.Save();
                }

                result = 1;
            }
            catch (Exception ex)
            {
                ex.Message.ToString(); 
            }
            return result;
        }

        public int UpdateMasterStatus(int master)
        {
            GFactory_EF_PrdDyingChemicalSetup = new PrdDyingChemicalSetup_EF();
            int result = 0;
            int masterID = master;
            try
            {
                List<PrdDyingChemicalSetup> customcode = new List<PrdDyingChemicalSetup>();
                customcode = GFactory_EF_PrdDyingChemicalSetup.FindBy(x => x.ChemicalSetupID == master).ToList();
                customcode.ForEach(x => x.IsDeleted = true);
                GFactory_EF_PrdDyingChemicalSetup.UpdateList(customcode);
                GFactory_EF_PrdDyingChemicalSetup.Save();
                result = 1;
            }
            catch (Exception)
            {
                result = -1;
                
            }
            return result;
        }
        #endregion Save

        #region GetAll
        public List<vmChemicalSetup> GetChemicalSetupList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmChemicalSetup> list = new List<vmChemicalSetup>();
            recordsTotal = 0;
            try
            {
                #region Convert It Into Sp
                ERP_Entities dbContext = new ERP_Entities();
                list = (from master in dbContext.PrdDyingChemicalSetups
                                  join details in dbContext.PrdDyingChemicalSetupDetails on master.ChemicalSetupID equals details.ChemicalSetupID
                                  join color in dbContext.CmnItemColors on master.ColorID equals color.ItemColorID into leftColorGroup
                                  from lcg in leftColorGroup.DefaultIfEmpty()
                                  join unit in dbContext.CmnUOMs on master.UnitID equals unit.UOMID into leftUnitGroup
                                  from lug in leftUnitGroup.DefaultIfEmpty()
                                  where master.IsDeleted == false
                                  select new vmChemicalSetup
                                  {
                                      ChemicalSetupID = master.ChemicalSetupID,
                                      ColorID = lcg.ItemColorID,
                                      Qty=master.Qty,
                                      ColorName=lcg.ColorName,
                                      UnitID = lug.UOMID,
                                      UnitName=lug.UOMName,
                                      IsDeleted = master.IsDeleted
                                  }).Distinct().ToList();

                recordsTotal = list.Count();
                list = list.OrderByDescending(x => x.ChemicalSetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                #endregion Convert It Into Sp
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }
           
            return list;
        }
        #endregion GetAll
       
        #region GetAll Details By ID
        public List<vmChemicalSetupDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            //var detail1s= GenericFactory_EF_PrdDyingChemicalSetupDetail.FindBy(x => x.ChemicalSetupID == DetailsID && x.IsDeleted == false).ToList();
            List<vmChemicalSetupDetail> recordList = new List<vmChemicalSetupDetail>();
            using (ERP_Entities dbContext = new ERP_Entities())
            {
                recordList = (from details in dbContext.PrdDyingChemicalSetupDetails
                              join item in dbContext.CmnItemMasters on details.ItemID equals item.ItemID into leftitemGroup
                              from lIg in leftitemGroup.DefaultIfEmpty()
                              join unit in dbContext.CmnUOMs on details.UnitID equals unit.UOMID into leftUnitGroup
                              from lug in leftUnitGroup.DefaultIfEmpty()
                              where details.IsDeleted == false && details.ChemicalSetupID == DetailsID
                              select new vmChemicalSetupDetail
                              {
                                  ChemicalSetupDetailID = details.ChemicalSetupDetailID,
                                  ChemicalSetupID = details.ChemicalSetupID
                                  ,
                                  ItemID = lIg.ItemID
                                  ,
                                  ArticleNo =   lIg.ItemName // lIg.ArticleNo
                                  ,Qty=details.Qty
                                  ,
                                  UnitID = lug.UOMID
                                  ,
                                  UnitName = lug.UOMName
                                  ,
                                  IsDeleted = details.IsDeleted


                              }).Distinct().ToList();
            }
            return recordList;
        }
        #endregion GetAll Details By ID
        #region Delete
        public string DeleteChemicalPreparationMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdDyingChemicalSetup = new PrdDyingChemicalSetup_EF();
                GenericFactory_EF_PrdDyingChemicalSetupDetail = new PrdDyingChemicalSetupDetail_EF();

                var MasterItem = new PrdDyingChemicalSetup();
                var DetailItem = new List<PrdDyingChemicalSetupDetail>();

                //For Update Master Detail
                var MasterAll = GFactory_EF_PrdDyingChemicalSetup.GetAll().Where(x => x.ChemicalSetupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                var DetailAll = GenericFactory_EF_PrdDyingChemicalSetupDetail.GetAll().Where(x => x.ChemicalSetupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
                //-------------------END----------------------

                try
                {
                    MasterItem = MasterAll.First(x => x.ChemicalSetupID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    foreach (PrdDyingChemicalSetupDetail d in DetailAll.Where(d => d.ChemicalSetupID == objcmnParam.id))
                    {
                        d.CompanyID = objcmnParam.loggedCompany;
                        d.DeleteBy = objcmnParam.loggeduser;
                        d.DeleteOn = DateTime.Now;
                        d.DeletePc = HostService.GetIP();
                        d.IsDeleted = true;

                        DetailItem.Add(d);
                    }

                    if (MasterItem != null)
                    {
                        GFactory_EF_PrdDyingChemicalSetup.Update(MasterItem);
                        GFactory_EF_PrdDyingChemicalSetup.Save();
                    }
                    if (DetailItem != null)
                    {
                        GenericFactory_EF_PrdDyingChemicalSetupDetail.UpdateList(DetailItem.ToList());
                        GenericFactory_EF_PrdDyingChemicalSetupDetail.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.CmnItemColor.ColorName;
                }
                catch (Exception e)
                {
                    result = "";
                    e.ToString();
                }
            }
            return result;
        }
        #endregion Delete
    }
}
