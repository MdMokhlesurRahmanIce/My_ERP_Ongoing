using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using ABS.Service.MenuMgt;
using ABS.Models.ViewModel.SystemCommon;

namespace ABS.Service.SystemCommon.Factories
{
    public class TeamSetupMgt : iTeamSetupMgt
    {
        private iGenericFactory<CmnUserTeam> GenericFactoryFor_Company = null;
        private iGenericFactory_EF<CmnUserTeam> GenericFactoryEF_CmnUserTeam = null;
        private iGenericFactory_EF<CmnuserTeamDetail> GFactory_EF_CmnuserTeamDetail = null;

        private List<CmnuserTeamDetail> UpdateDetailsBinding(List<vCmnuserTeamDetail> details, UserCommonEntity commonEntity)
        {
           
            List<CmnuserTeamDetail> list = new List<CmnuserTeamDetail>(); 

            foreach (vCmnuserTeamDetail Item in details)
            {
                CmnuserTeamDetail entity = new CmnuserTeamDetail();
                entity.TeamDetailID = Item.TeamDetailID;
                entity.TeamID = Item.TeamID;
                entity.UserID = Item.UserID;
                entity.Sequence = Item.Sequence;
                entity.CompanyID = Item.CompanyID;
                entity.IsDeleted = Item.IsDeleted;
                if (Item.IsDeleted==false)
                {
                    entity.UpdateBy = commonEntity.loggedUserID;
                    entity.UpdateOn = DateTime.Now;
                    entity.UpdatePc =  HostService.GetIP();
                }
                else
                {
                    entity.DeleteBy = commonEntity.loggedUserID;
                    entity.DeleteOn = DateTime.Now;
                    entity.DeletePc =  HostService.GetIP();
                }
               
               
                list.Add(entity);
            }
            return list;
        }
        private List<CmnuserTeamDetail> InsertionList(vCmnUserTeam model, List<vCmnuserTeamDetail> details, UserCommonEntity commonEntity)
        {
            List<CmnuserTeamDetail> list = new List<CmnuserTeamDetail>();
            GenericFactoryEF_CmnUserTeam = new CmnUserTeam_EF();
            Int64 NextID = GenericFactoryEF_CmnUserTeam.getMaxVal_int64("TeamDetailID", "CmnuserTeamDetail");
            foreach (vCmnuserTeamDetail Item in details)
            {
                CmnuserTeamDetail entity = new CmnuserTeamDetail();

                entity.TeamDetailID = NextID++;
                
                entity.TeamID = model.TeamID;
                entity.UserID = Item.UserID;
                entity.Sequence = Item.Sequence;
                entity.CompanyID = Item.CompanyID;
                entity.CreateBy = Item.CreateBy; 
                entity.CreateOn = DateTime.Now; 
                entity.CreatePc =  HostService.GetIP();
                entity.UpdateBy = Item.UpdateBy;
                entity.UpdateOn = Item.UpdateOn;
                entity.UpdatePc = Item.UpdatePc;
                entity.IsDeleted = Item.IsDeleted;
                entity.DeleteBy = Item.DeleteBy;
                entity.DeleteOn = Item.DeleteOn;
                entity.DeletePc = Item.DeletePc;
                list.Add(entity);

            } 
            return list;
        }
        private CmnUserTeam InsertionModelBinding(vCmnUserTeam model, List<vCmnuserTeamDetail> details, UserCommonEntity commonEntity)
        {
            GenericFactoryEF_CmnUserTeam = new CmnUserTeam_EF();
            try
            {
                Int64 NextID = GenericFactoryEF_CmnUserTeam.getMaxVal_int64("TeamID", "CmnUserTeam");
                CmnUserTeam returnModel = new CmnUserTeam();

                returnModel.TeamID = model.TeamID == 0 ? NextID : model.TeamID;
                returnModel.TeamName = model.TeamName;
                returnModel.DepartmentID = model.DepartmentID;
                returnModel.CompanyID = model.CompanyID;
                if(model.EntityMode=="Inserted")
                {
                    returnModel.CreateBy = commonEntity.loggedUserID;
                    returnModel.CreateOn = DateTime.Now;
                    returnModel.CreatePc =  HostService.GetIP();
                }
                else
                {
                    returnModel.UpdateBy = commonEntity.loggedUserID;
                    returnModel.UpdateOn = DateTime.Now;
                    returnModel.UpdatePc =  HostService.GetIP();
                }
                returnModel.IsDeleted = model.IsDeleted;
                if(returnModel.IsDeleted==true)
                {
                    returnModel.DeleteBy = commonEntity.loggedUserID;
                    returnModel.DeleteOn = DateTime.Now;
                    returnModel.DeletePc =  HostService.GetIP();
                }
               

                NextID = GenericFactoryEF_CmnUserTeam.getMaxVal_int64("TeamDetailID", "CmnuserTeamDetail");
                foreach (vCmnuserTeamDetail Item in details)
                {
                    CmnuserTeamDetail entity = new CmnuserTeamDetail();

                    entity.TeamDetailID = NextID++;
                    if (Item.EntityMode != "Inserted") entity.TeamDetailID = Item.TeamDetailID;
                    entity.TeamID = returnModel.TeamID;
                    entity.UserID = Item.UserID;
                    entity.Sequence = Item.Sequence;
                    entity.CompanyID = Item.CompanyID;
                    entity.CreateBy = Item.CreateBy;
                    entity.CreateOn = DateTime.Now;
                    entity.CreatePc =  HostService.GetIP();
                    entity.UpdateBy = Item.UpdateBy;
                    entity.UpdateOn = Item.UpdateOn;
                    entity.UpdatePc = Item.UpdatePc;
                    entity.IsDeleted = Item.IsDeleted;
                    entity.DeleteBy = Item.DeleteBy;
                    entity.DeleteOn = Item.DeleteOn;
                    entity.DeletePc = Item.DeletePc;
                    returnModel.CmnuserTeamDetails.Add(entity);
                }

                return returnModel;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public int SaveTeam(vCmnUserTeam master, List<vCmnuserTeamDetail> Details, UserCommonEntity commonEntity)
        {
            int returnValue = 0;
            try
            {
                GenericFactoryEF_CmnUserTeam = new CmnUserTeam_EF();
                if (master.EntityMode=="Inserted")
                {
                    CmnUserTeam returnModel = this.InsertionModelBinding(master, Details, commonEntity);
                    GenericFactoryEF_CmnUserTeam.Insert(returnModel);
                    GenericFactoryEF_CmnUserTeam.Save();
                    returnValue = 1;
                }

               else if (master.EntityMode == "Updated" && !Details.Any(x => x.EntityMode == "Inserted"))
                {
                    CmnUserTeam returnModel = this.InsertionModelBinding(master, Details, commonEntity);
                    GenericFactoryEF_CmnUserTeam.Update(returnModel);
                    GFactory_EF_CmnuserTeamDetail = new CmnuserTeamDetail_EF();
                    GFactory_EF_CmnuserTeamDetail.UpdateList(this.UpdateDetailsBinding(Details, commonEntity));
                    GenericFactoryEF_CmnUserTeam.Save();
                    GFactory_EF_CmnuserTeamDetail.Save();
                    returnValue = 2;
                }
                else 
                {
                    CmnUserTeam returnModel = this.InsertionModelBinding(master, Details, commonEntity);
                    GenericFactoryEF_CmnUserTeam.Update(returnModel);
                    GFactory_EF_CmnuserTeamDetail = new CmnuserTeamDetail_EF();
                    GFactory_EF_CmnuserTeamDetail.UpdateList(this.UpdateDetailsBinding(Details.Where(x=>x.EntityMode=="Updated").ToList(), commonEntity));
                    GFactory_EF_CmnuserTeamDetail.InsertList(this.InsertionList(master,Details.Where(x => x.EntityMode == "Inserted").ToList(), commonEntity));
                    GenericFactoryEF_CmnUserTeam.Save();
                    GFactory_EF_CmnuserTeamDetail.Save();
                    returnValue = 2;
                }

                
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return returnValue;
        }

        #region GetAll
        public List<vCmnUserTeam> GetTeam(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vCmnUserTeam> list = new List<vCmnUserTeam>();
            try
            {
                ERP_Entities dbContext = new ERP_Entities();
                var recordList = (from master in dbContext.CmnUserTeams
                                  join details in dbContext.CmnOrganograms on master.DepartmentID equals details.OrganogramID into leftOrganoGroup
                                  from log in leftOrganoGroup.DefaultIfEmpty()
                                  where master.IsDeleted == false && master.CompanyID== companyID
                                  select new vCmnUserTeam
                                  {
                                      TeamID = master.TeamID
                                      ,
                                      TeamName = master.TeamName
                                      ,
                                      DepartmentID = master.DepartmentID

                                      ,
                                      DepartmentName = log.OrganogramName
                                     ,
                                      IsDeleted = master.IsDeleted
                                       ,
                                      CompanyID = master.CompanyID
                                  ,
                                      CreateBy = master.CreateBy
                                  ,
                                      CreateOn = master.CreateOn
                                  ,
                                      CreatePc = master.CreatePc
                                  ,
                                      UpdateBy = master.UpdateBy
                                  ,
                                      UpdateOn = master.UpdateOn
                                  ,
                                      UpdatePc = master.UpdatePc
                                  ,
                                      DeleteBy = master.DeleteBy
                                  ,
                                      DeleteOn = master.DeleteOn
                                    ,
                                      DeletePc = master.DeletePc
                                      ,
                                      EntityMode = "UnChanged"


                                  }).Distinct().ToList();

                list = recordList.ToList();
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }

            return list;
        }
        #endregion GetAll

        #region GetAll Details By ID
        public List<vCmnuserTeamDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            //var detail1s= GenericFactory_EF_PrdDyingChemicalSetupDetail.FindBy(x => x.ChemicalSetupID == DetailsID && x.IsDeleted == false).ToList();
            List<vCmnuserTeamDetail> recordList = new List<vCmnuserTeamDetail>();
            using (ERP_Entities dbContext = new ERP_Entities())
            {
                recordList = (from details in dbContext.CmnuserTeamDetails
                              join user in dbContext.CmnUsers on details.UserID equals user.UserID into leftuserGroup
                              from lug in leftuserGroup.DefaultIfEmpty()
                              where details.IsDeleted == false && details.CompanyID == companyID && details.TeamID== DetailsID
                              select new vCmnuserTeamDetail
                              {
                                  TeamDetailID = details.TeamDetailID,
                                  TeamID = details.TeamID
                                  ,
                                  UserID = lug.UserID
                                  ,
                                  UserName=lug.UserFullName
                                  ,
                                  Sequence = details.Sequence
                                  ,
                                  CompanyID = details.CompanyID
                                  ,
                                  CreateBy = details.CreateBy
                                  ,
                                  CreateOn = details.CreateOn
                                  ,
                                  CreatePc = details.CreatePc
                                  ,
                                  UpdateBy = details.UpdateBy
                                  ,
                                  UpdateOn = details.UpdateOn
                                  ,
                                  UpdatePc = details.UpdatePc
                                  ,
                                  DeleteBy = details.DeleteBy
                                  ,
                                  DeleteOn = details.DeleteOn
                                    ,
                                  DeletePc = details.DeletePc
                                  ,
                                  EntityMode="Updated"

                              }).Distinct().ToList();
            }
            return recordList;
        }
        #endregion GetAll Details By ID

        public int DeleteTeam(vCmnUserTeam master, UserCommonEntity commonEntity)
        {
            int returnValue = 0;
            try
            {
                GenericFactoryEF_CmnUserTeam = new CmnUserTeam_EF();
                CmnUserTeam returnModel = GenericFactoryEF_CmnUserTeam.FindBy(x => x.TeamID == master.TeamID).FirstOrDefault();
                returnModel.IsDeleted = true;
                returnModel.DeleteBy = commonEntity.loggedUserID;
                returnModel.DeleteOn = DateTime.Now;
                returnModel.DeletePc =  HostService.GetIP();
                GenericFactoryEF_CmnUserTeam.Update(returnModel);
                GenericFactoryEF_CmnUserTeam.Save();
                returnValue = 3;
            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }

    }
}
