using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.MenuMgt
{
    public class WorkFLowMgt
    {
        
        public int GetWorkFlowBeginner(UserCommonEntity ent)
        {
            int beginnerID = 0;
            try
            {
                using (ERP_Entities dbContext = new ERP_Entities())
                {

                    beginnerID = (from WFM in dbContext.CmnWorkFlowMasters
                                  join WFD in dbContext.CmnWorkFlowDetails on WFM.WorkFlowID equals WFD.WorkFlowID
                                  where WFM.MenuID == ent.currentMenuID && WFM.CompanyID == ent.loggedCompnyID && WFD.Sequence == 1 && WFM.IsActive == true && WFD.IsDeleted == false
                                  select WFD.EmployeeID).FirstOrDefault();


                }
            }
            catch (Exception ex)
            {

            }
            return beginnerID;
        }

        //IN CASE OF SAVE
        //REQUIRD MENUID, COMPANYID, TARGET USERID AS TARGET USERID, USER AS CURRENT USERID,TRANSACTION ID AS MASTERID(INSERTED) ,STATUSID 1
        //IS APPROVED 0, IS DELETE 0, IS UPDATE 0 , @APPROVALCUSTOMCODE = ''
        public Hashtable SetProcedureParam(UserCommonEntity ent, int targetUserID, int masterID, int IsApprove, string Comment, int IsUpdate, int IsDelete, int statusID, string approvalCustomCode, int IsDeclained,string Message)
        {
            Hashtable ht = new Hashtable();
            ht.Add("MenuID", ent.currentMenuID); 
            ht.Add("UserID", ent.loggedUserID);
            ht.Add("TargetUserID", targetUserID);
            ht.Add("CompanyID", ent.loggedCompnyID);
            ht.Add("CreatePc", HostService.GetIP());
            ht.Add("TransactionID", masterID);
            ht.Add("TransactionDate", DateTime.Now);
            ht.Add("IsApprove", IsApprove);
            ht.Add("Comment", Comment);
            ht.Add("IsUpdate", IsUpdate);
            ht.Add("IsDelete", IsDelete);
            ht.Add("STATUSID", statusID);
            ht.Add("APPROVALCUSTOMCODE", approvalCustomCode);
            ht.Add("IsDeclained", IsDeclained);
            ht.Add("MessageName", Message);
            return ht;
        }

        public vmNotificationMail GetNotificationMailObject(NotificationEntity model,string message)
        {
            vmNotificationMail obj = new vmNotificationMail();
            try
            {
                using (var db = new ABS.Models.ERP_Entities())
                {
                   // if (model.NextWFUserID==null)
                    obj.nextUser = db.CmnUsers.Where(x => x.UserID == model.NextWFUserID).FirstOrDefault().UserFullName;
                    obj.companyName= db.CmnCompanies.Where(x => x.CompanyID == model.LoggedCompanyID).FirstOrDefault().CompanyName;
                    obj.menuName = db.CmnMenus.Where(x => x.MenuID == model.MenuID).FirstOrDefault().MenuName;
                    obj.customCode = model.TransactionID.ToString();
                    obj.customCode = db.CmnWorkFlowTransactions.Where(x => x.UserID == model.NextWFUserID && x.IsDeleted == false && x.StatusID == 1).FirstOrDefault().CustomCode;
                    obj.message = message;
                    obj.currentUser= db.CmnUsers.Where(x => x.UserID == model.CreatorID).FirstOrDefault().UserFullName;
                    obj.comments = model.Comments;
                    obj.nextUserEmailAddress= db.CmnUserAuthentications.Where(x => x.UserID == model.NextWFUserID).FirstOrDefault().LoginEmail;
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }

        public List<vmNotificationMail> GetNotificationMailObjectList(NotificationEntity model, string message)
        {
            List<vmNotificationMail> objList = new List<vmNotificationMail>();
            try
            {
                using (var db = new ABS.Models.ERP_Entities())
                {
                    List<CmnWorkFlowTransactionTran> list = new List<CmnWorkFlowTransactionTran>();
                    list = db.CmnWorkFlowTransactionTrans.Where(x => x.TransactionID == model.TransactionID && x.IsActve == true && x.WFMID == model.WorkFlowID && x.IsDeleted == false).ToList();
                    foreach (CmnWorkFlowTransactionTran item in list)
                    {
                        vmNotificationMail obj = new vmNotificationMail();
                        obj.nextUser = db.CmnUsers.Where(x => x.UserID == item.TUserID).FirstOrDefault().UserFullName;
                        obj.companyName = db.CmnCompanies.Where(x => x.CompanyID == item.CompanyID).FirstOrDefault().CompanyName;
                        obj.menuName = db.CmnMenus.Where(x => x.MenuID == item.MenuID).FirstOrDefault().MenuName;
                        obj.customCode = item.CustomCode.ToString();

                        if (string.IsNullOrEmpty(message))
                        {
                            obj.message = db.CmnWorkFlowTransactionTrans.Where(x => x.TransactionID == item.TransactionID 
                                && x.WFMID == item.WFMID && x.IsDeleted == false && x.Ccomment == "1").FirstOrDefault().Notification;
                        }
                        else
                        {
                            obj.message = message;
                        }

                        obj.currentUser = db.CmnUsers.Where(x => x.UserID == item.UserID).FirstOrDefault().UserFullName;
                        obj.comments = item.Notification;
                        obj.nextUserEmailAddress = db.CmnUserAuthentications.Where(x => x.UserID == item.TUserID).FirstOrDefault().LoginEmail;
                        obj.isApproved = true;
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objList;
        }

        public List<vmNotificationMail> GetNotificationMailObjectListDeclined(NotificationEntity model, string message)
        {
            List<vmNotificationMail> objList = new List<vmNotificationMail>();
            try
            {
                using (var db = new ABS.Models.ERP_Entities())
                {
                    List<CmnWorkFlowTransactionTran> list = new List<CmnWorkFlowTransactionTran>();
                    list = db.CmnWorkFlowTransactionTrans.Where(x => x.TransactionID == model.TransactionID && x.IsActve == true 
                        && x.WFMID == model.WorkFlowID && x.IsDeleted == false).ToList();
                    foreach (CmnWorkFlowTransactionTran item in list)
                    {
                        vmNotificationMail obj = new vmNotificationMail();
                        obj.nextUser = db.CmnUsers.Where(x => x.UserID == item.TUserID).FirstOrDefault().UserFullName;
                        obj.companyName = db.CmnCompanies.Where(x => x.CompanyID == item.CompanyID).FirstOrDefault().CompanyName;
                        obj.menuName = db.CmnMenus.Where(x => x.MenuID == item.MenuID).FirstOrDefault().MenuName;
                        obj.customCode = item.CustomCode.ToString();
                        obj.message = db.CmnWorkFlowTransactionTrans.Where(x => x.TransactionID == item.TransactionID && x.WFMID == item.WFMID
                            && x.IsDeleted == false && x.Ccomment == "1").FirstOrDefault().Notification;
                       
                        obj.currentUser = db.CmnUsers.Where(x => x.UserID == item.UserID).FirstOrDefault().UserFullName;
                        obj.comments = item.Notification;
                        obj.nextUserEmailAddress = db.CmnUserAuthentications.Where(x => x.UserID == item.TUserID).FirstOrDefault().LoginEmail;
                        obj.isApproved = false;
                        objList.Add(obj);
                    }

                }

            }
            catch (Exception ex)
            {
            }
            return objList;
        }
        //IN CASE OF Approve
        //REQUIRD MENUID,TRANSACTION,STATUSID 1(approval) ,Approval CustomCOde 
        //IS APPROVED 1, IS DELETE 0, IS UPDATE 0 , @APPROVALCUSTOMCODE = ''
        public Hashtable SetProcedureParamForApprove(NotificationEntity ent, int ComapnyID, int IsApprove, string Comment, int IsUpdate, int IsDelete, int statusID, string approvalCustomCode, int IsDeclained = 0,String Message="")
        {
            Hashtable ht = new Hashtable();
            ht.Add("MenuID", ent.MenuID);
            ht.Add("UserID", ent.CreatorID ?? 0);
            ht.Add("TargetUserID", ent.NextWFUserID ?? 0);
            ht.Add("CompanyID", ComapnyID);
            ht.Add("CreatePc", HostService.GetIP());
            ht.Add("TransactionID", ent.TransactionID);
            ht.Add("TransactionDate", DateTime.Now);
            ht.Add("IsApprove", IsApprove);
            ht.Add("Comment", Comment);
            ht.Add("IsUpdate", IsUpdate);
            ht.Add("IsDelete", IsDelete);
            ht.Add("STATUSID", statusID);
            ht.Add("APPROVALCUSTOMCODE", approvalCustomCode);
            ht.Add("IsDeclained", IsDeclained);
            ht.Add("MessageName", Message);
            return ht;
        }

        //IN CASE OF Approve
        //REQUIRD MENUID,TRANSACTION,STATUSID 1(approval) ,Approval CustomCOde 
        //IS APPROVED 1, IS DELETE 0, IS UPDATE 0 , @APPROVALCUSTOMCODE = ''
        public Hashtable SetProcedureParamForDeclained(NotificationEntity ent, int ComapnyID, int IsApprove, string Comment, int IsUpdate, int IsDelete, int statusID, string approvalCustomCode, int IsDeclained = 0,String Message="")
        {
            List<NotificationEntity> list = this.GetNotificationInfoByUser(ent.PrevWFUserID??0);
            int? declinedTo = 0;
            using (ERP_Entities dbContext = new ERP_Entities())
            {
                declinedTo = (from tb in dbContext.CmnWorkFlowDetails
                                   where tb.WorkFlowID == ent.WorkFlowID && tb.Sequence == ent.currentSequence - 2
                                   && tb.IsDeleted==false
                                   select tb.EmployeeID).FirstOrDefault();
            }
            
            Hashtable ht = new Hashtable();
            ht.Add("MenuID", ent.MenuID);
            ht.Add("UserID", declinedTo??0);
            ht.Add("TargetUserID", ent.PrevWFUserID);
            ht.Add("CompanyID", ComapnyID);
            ht.Add("CreatePc", HostService.GetIP());
            ht.Add("TransactionID", ent.TransactionID);
            ht.Add("TransactionDate", DateTime.Now);
            ht.Add("IsApprove", IsApprove);
            ht.Add("Comment", Comment);
            ht.Add("IsUpdate", IsUpdate);
            ht.Add("IsDelete", IsDelete);
            ht.Add("STATUSID", statusID);
            ht.Add("APPROVALCUSTOMCODE", approvalCustomCode);
            ht.Add("IsDeclained", IsDeclained);
            ht.Add("MessageName", Message);
            return ht;
        }
        public int ExecuteWorkFlowTransactionProcess(Hashtable ht)
        {
            string spQuery = "[SPInsertCmnWorkFlowTRAN]";
            ERP_Entities _dbctx = new ERP_Entities();
            int result = 0;
            try
            {
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }

                    IDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = dr.GetInt32(0);
                    }

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                e.StackTrace.ToString();
            }
            return result;
        }

        public List<NotificationEntity> GetNotificationInfoByUser(int userID)
        {
            List<NotificationEntity> returnList = new List<NotificationEntity>();
            try
            {
                using (ERP_Entities dbContext = new ERP_Entities())
                {
                    returnList = (from CWT in dbContext.CmnWorkFlowTransactions
                                  join menu in dbContext.CmnMenus on CWT.MenuID equals menu.MenuID
                                  join WFM in dbContext.CmnWorkFlowMasters on CWT.MenuID equals WFM.MenuID
                                  join WFD in dbContext.CmnWorkFlowDetails on WFM.WorkFlowID equals WFD.WorkFlowID
                                  where WFD.EmployeeID == CWT.UserID
                                  join ST in dbContext.CmnStatus on WFD.StatusID equals ST.StatusID

                                  join NDetails in dbContext.CmnWorkFlowDetails on
                                  new { WID = WFD.WorkFlowID, S = WFD.Sequence + 1 } equals new { WID = NDetails.WorkFlowID, S = NDetails.Sequence }
                                  into NDetailsGroup
                                  from ndg in NDetailsGroup.DefaultIfEmpty()

                                  join PreviousDetails in dbContext.CmnWorkFlowDetails on
                                  new { WID = WFD.WorkFlowID, S = WFD.Sequence - 1 } equals new { WID = PreviousDetails.WorkFlowID, S = PreviousDetails.Sequence }
                                  into PreviousDetailsGroup
                                  from pdg in PreviousDetailsGroup.DefaultIfEmpty()
                                  
                                  //User Name 
                                  join  user in dbContext.CmnUsers on WFD.EmployeeID equals user.UserID into userGroupWF
                                  from ugf in userGroupWF.DefaultIfEmpty()

                                  join userN in dbContext.CmnUsers on ndg.EmployeeID equals userN.UserID into userGroupNext
                                  from ugn in userGroupNext.DefaultIfEmpty()

                                  join userP in dbContext.CmnUsers on pdg.EmployeeID equals userP.UserID into userGroupPrev
                                  from ugp in userGroupPrev.DefaultIfEmpty()

                                  join userCreator in dbContext.CmnUsers on CWT.CreateBy equals userCreator.UserID into userCreatorGroup
                                  from ucg in userCreatorGroup.DefaultIfEmpty()

                                  where CWT.UserID == userID && CWT.IsDeleted == false && CWT.StatusID == 1 && WFM.IsActive == true && WFD.IsDeleted == false
                                  select new NotificationEntity
                                  {
                                      MenuName = menu.MenuName ,
                                      MenuPath = menu.MenuPath,
                                      CustomCode = CWT.CustomCode,
                                      RecordID = (int)CWT.RecordID ,
                                      TransactionID = (int)CWT.TransactionID,
                                      MenuID = CWT.MenuID,
                                      WorkFlowID = WFD.WorkFlowID,
                                      currentSequence = WFD.Sequence,
                                      Comments = CWT.ForwardComment ?? CWT.BackwardComment,
                                      CreatorName = ucg.UserFirstName ?? "N/A",
                                      MessageName= CWT.MessageName,
                                      MessageDate = CWT.MessageDate??DateTime.Now
                                  }).ToList();

                    #region  Message Time Calculation 
            
                    #endregion MessageTime Calculation
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return returnList;
        }

        #region WorkFlow Transaction Team Begin
        public List<vmCmnWorkFlowMaster> GetWorkFlowMasterListByMenu(UserCommonEntity ent)
        {
            List<vmCmnWorkFlowMaster> list = new List<vmCmnWorkFlowMaster>();
            try
            {
                using (ERP_Entities dbContext = new ERP_Entities())
                {
                    list = (from WFM in dbContext.CmnWorkFlowMasters
                            where WFM.MenuID == ent.currentMenuID && WFM.CompanyID == ent.loggedCompnyID  && WFM.IsActive == true && WFM.IsDeleted == false
                            select new vmCmnWorkFlowMaster
                            {
                                WorkFlowID = WFM.WorkFlowID,
                                MenuID = WFM.MenuID,
                                BranchID = WFM.BranchID,
                                UserTeamID = WFM.UserTeamID,
                                IsActive = WFM.IsActive,
                                CompanyID=WFM.CompanyID,
                                DBID = WFM.DBID,
                                CreateBy = WFM.CreateBy,
                                CreateOn = WFM.CreateOn,
                                CreatePc = WFM.CreatePc,
                                UpdateBy = WFM.UpdateBy,
                                UpdateOn = WFM.UpdateOn,
                                UpdatePc = WFM.UpdatePc,
                                IsDeleted = WFM.IsDeleted,
                                DeleteBy = WFM.DeleteBy,
                                DeleteOn = WFM.DeleteOn,
                                DeletePc = WFM.DeletePc,
                                Transfer = WFM.Transfer
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PleaseCheckMethod:GetWorkFlowMasterListByMenu" + ex.Message.ToString());
            }
            return list;
        }
        public int ExecuteTransactionProcess(vmCmnWorkFlowMaster master, UserCommonEntity ent)
        {
            int result = 0;
            try
            {
                int process = (int)WorkFlowTranProcess.insert;
                Hashtable ht = new Hashtable();
                ht.Add("InTEAMID", master.UserTeamID??0);
                ht.Add("InMENUID", ent.currentMenuID);
                ht.Add("InWFMID", master.WorkFlowID);
                ht.Add("Inprocess", process);
                ht.Add("InTransactionID", master.WorkFlowTranCustomID);
                ht.Add("InloggedUserID", ent.loggedUserID);
                ht.Add("InloggedCompanyID", ent.loggedCompnyID);
                ht.Add("InSequence", 1);
                string spQuery = "[SPInsCmnWorkFlowTransaction]";
                ERP_Entities _dbctx = new ERP_Entities();
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = dr.GetInt32(0);
                    }
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                result = 0;
                ex.Message.ToString();
            }
            return result;
        }
        public bool checkUserValidation(int userID,int workflowID)
        {
            bool isUserValid = false;
            using (ERP_Entities _dbctx = new ERP_Entities())
            {
                isUserValid = _dbctx.CmnWorkFlowDetails.Where(x => x.WorkFlowID == workflowID && x.IsDeleted == false && x.Sequence == 1).ToList().Exists(x => x.EmployeeID == userID);
            }
          return isUserValid;
        }

        public int ApproveProcess(NotificationEntity master)
        {
            bool? isAlreadyApproved = false;
            int result = 0;
            try
            {
                int process = (int)WorkFlowTranProcess.Appoved;
                Hashtable ht = new Hashtable();
                ht.Add("InTEAMID", master.TeamID ?? 0);
                ht.Add("InMENUID", master.MenuID);
                ht.Add("InWFMID", master.WorkFlowID);
                ht.Add("Inprocess", process);
                ht.Add("InTransactionID", master.TransactionID);
                ht.Add("InloggedUserID", master.LoggedUserID);
                ht.Add("InloggedCompanyID", master.LoggedCompanyID);
                ht.Add("InSequence", 1);
                ht.Add("InRecordID", master.RecordID);
                ht.Add("ForwardMessage", master.Comments);
                string spQuery = "[SPInsCmnWorkFlowTransaction]";
                ERP_Entities _dbctx = new ERP_Entities();
                using (_dbctx.Database.Connection)
                {
                    
                    _dbctx.Database.Connection.Open();
                    isAlreadyApproved = _dbctx.CmnWorkFlowTransactionTrans.Where(x => x.RecordID == master.RecordID).FirstOrDefault().IsActve;
                    result = 0;
                    if ((isAlreadyApproved??false))
                    {
                        DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                        cmd.CommandText = spQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (object obj in ht.Keys)
                        {
                            string str = Convert.ToString(obj);
                            SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                            cmd.Parameters.Add(parameter);
                        }
                        IDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            result = dr.GetInt32(0);
                        }
                        cmd.Parameters.Clear();
                    }
                
                }
            }
            catch (Exception ex)
            {
                result = 0;
                ex.Message.ToString();
            }
            return result;
        }

        public int DeclinedProcess(NotificationEntity master)
        {
            int result = 0;
            try
            {
                int process = (int)WorkFlowTranProcess.Declined;
                Hashtable ht = new Hashtable();
                ht.Add("InTEAMID", master.TeamID ?? 0);
                ht.Add("InMENUID", master.MenuID);
                ht.Add("InWFMID", master.WorkFlowID);
                ht.Add("Inprocess", process);
                ht.Add("InTransactionID", master.TransactionID);
                ht.Add("InloggedUserID", master.LoggedUserID);
                ht.Add("InloggedCompanyID", master.LoggedCompanyID);
                ht.Add("InSequence", 1);
                ht.Add("InRecordID", master.RecordID);
                ht.Add("ForwardMessage", master.Comments);
                string spQuery = "[SPInsCmnWorkFlowTransaction]";
                ERP_Entities _dbctx = new ERP_Entities();
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = dr.GetInt32(0);
                    }
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                result = 0;
                ex.Message.ToString();
            }
            return result;
        }

        public List<NotificationEntity> GetNotificationInfoesByUser(int userID)
        {
            List<NotificationEntity> returnList = new List<NotificationEntity>();
            List<NotificationEntity> list = new List<NotificationEntity>();
            try
            {
                using (ERP_Entities dbContext = new ERP_Entities())
                {
                    returnList = (from CWT in dbContext.CmnWorkFlowTransactionTrans
                                  join menu in dbContext.CmnMenus on CWT.MenuID equals menu.MenuID
                                  join usr in dbContext.CmnUsers on CWT.UserID equals usr.UserID into  cUsergrp
                                  from ug in cUsergrp.DefaultIfEmpty()
                                  join CWTF in dbContext.CmnWorkFlowTransactionTrans 
                                 // on new { WID = CWT.WFMID, S = CWT.EntrySequence  } equals new { WID = CWTF.WFMID, S = CWTF.EntrySequence - 1 }
                                  on new { A = CWT.RecordID - 1, B = CWT.TransactionID } equals new { A = CWTF.RecordID, B = CWTF.TransactionID }
                                  into commGroup
                                  from ng in commGroup.DefaultIfEmpty()
                                  where CWT.TUserID == userID && CWT.IsDeleted == false && CWT.IsActve == true  
                                  select new NotificationEntity
                                  {
                                      MenuName = menu.MenuShortName == null ? menu.MenuName : menu.MenuShortName,
                                      MenuPath = menu.MenuPath,
                                      TargetUserID=CWT.TUserID??0,
                                      TeamID = CWT.TeamID,
                                      IsTeam = CWT.IsTeam??false,
                                      CustomCode = CWT.CustomCode,
                                      RecordID = (int)CWT.RecordID,
                                      TransactionID = (int)CWT.TransactionID,
                                      MenuID = (int)CWT.MenuID,
                                      WorkFlowID = (int)CWT.WFMID,
                                      WFDStatusID = CWT.WFDStatusID,
                                      currentSequence = CWT.Sequence,
                                  //    Comments = ng.Notification ?? "Created",
                                      Comments = (from tb in dbContext.CmnWorkFlowTransactionTrans where tb.TransactionID == CWT.TransactionID && tb.Ccomment=="1" && tb.IsDeleted==false select tb.Notification ).FirstOrDefault(),
                                      CreatorName = ug.UserFirstName ?? "N/A",
                                      MessageName = CWT.Message,
                                      MessageDate = CWT.TransDate ?? DateTime.Now
                                  }).ToList();

                    #region  Message Time Calculation 

                    #endregion MessageTime Calculation
                }
                #region IsDeleted Check
                foreach (NotificationEntity item in returnList)
                {
                    bool isValid = false;
                    Hashtable ht = new Hashtable();
                    ht.Add("InTransactionID", item.TransactionID.ToString());
                    ht.Add("InMENUID", item.MenuID);
                   
                    string spQuery = "[SPGetValidCmnWorkFlow]";
                    ERP_Entities _dbctx = new ERP_Entities();
                    using (_dbctx.Database.Connection)
                    {
                        _dbctx.Database.Connection.Open();
                        DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                        cmd.CommandText = spQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (object obj in ht.Keys)
                        {
                            string str = Convert.ToString(obj);
                            SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                            cmd.Parameters.Add(parameter);
                        }
                        IDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            var isValid2 = dr.GetBoolean(0);
                            isValid = dr.GetBoolean(0);
                        }
                        cmd.Parameters.Clear();
                    }
                    if(!isValid)
                    {
                        list.Add(item);
                    }
                }
                #endregion IsDeleted Check
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return list;
        }




        #endregion WorkFlow Transaction Team End
    }
}
