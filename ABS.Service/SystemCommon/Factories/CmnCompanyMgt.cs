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
    public class CmnCompanyMgt : iCmnCompanyMgt
    {
        private iGenericFactory<CmnCompany> GenericFactoryFor_Company = null;
        private iGenericFactory_EF<CmnCompany> GenericFactoryEF_Company = null;
        private iGenericFactory_EF<CmnCustomCode> GFactory_EF_CmnCustomCode = null;

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<CmnCompany> GetCompanies(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Company = new CmnCompany_GF();
            IEnumerable<CmnCompany> objCompanies = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_CmnCompany]";
                objCompanies = GenericFactoryFor_Company.ExecuteQuery(spQuery, ht);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objCompanies;
        }


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public CmnCompany GetCompanyByID(int? id)
        {
            GenericFactoryFor_Company = new CmnCompany_GF();
            CmnCompany objCompany = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", id);

                spQuery = "[Get_CmnCompanySingle]";
                objCompany = GenericFactoryFor_Company.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return objCompany;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveCompanyParam(CmnCompany model, UserCommonEntity commonEntity)
        {
            GenericFactoryFor_Company = new CmnCompany_GF();
            GenericFactoryEF_Company = new CmnCompany_EF();
            GFactory_EF_CmnCustomCode = new SalCmnCustomCode_EF();
            int result = 0;

            try
            {
                #region CustomCode And Primary Key  Process
                int NextId = Convert.ToInt16(GenericFactoryEF_Company.getMaxID("CmnCompany"));
                model.CompanyID = NextId;
                //......... START for custom code........... //
                //string customCode = "";
                CmnCustomCode objCustomCode = GFactory_EF_CmnCustomCode.FindBy(m => m.MenuID == model.MenuID && m.IsDeleted == false).FirstOrDefault();
                //int customCodeID = Convert.ToInt16(objCustomCode.RecordID);
                //if (customCodeID > 0)
                //{
                //  //  customCode = GenericFactory_EF_SalPIMaster.getCustomCode(customCodeID, DateTime.Now, itemMaster.CompanyID, 1, 1);
                //    customCode = GFactory_EF_CmnCustomCode.getCustomCode(customCodeID, DateTime.Now, 0, 1, 1);
                //}
                //else
                //{
                //    customCode = NextId.ToString();
                //}
                //.........END for custom code............ //

                #endregion CustomCode And Primary Key  Process
                #region Basic Save
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("CompanyShortName", model.CompanyShortName);
                ht.Add("CompanyName", model.CompanyName);
                ht.Add("LoggedCompanyID", commonEntity.loggedCompnyID);
                ht.Add("LoggedUser", commonEntity.loggedUserID);

                //  ht.Add("StatusID", model.StatusID);
                string spQuery = "[Set_CmnCompany]";
                result = GenericFactoryFor_Company.ExecuteCommand(spQuery, ht);
                #endregion Basic Save

                #region WorkFlow Transaction Entry
                ////  CHECK WorkFLow Setup
                ////IF targetUserID=0 Then Work flow is not configured
                //int targetUserID = new WorkFLowMgt().GetWorkFlowBeginner(commonEntity);
                //if (targetUserID > 0)
                //{
                //    //IN CASE OF SAVE
                //    //REQUIRD MENUID, COMPANYID, TARGET USERID AS TARGET USERID, USER AS CURRENT USERID,TRANSACTION ID AS MASTERID(INSERTED) ,STATUSID 1
                //    //IS APPROVED 0, IS DELETE 0, IS UPDATE 0 , @APPROVALCUSTOMCODE = '',Is Declained=0
                //    var comment = "";
                //    var APPROVALCUSTOMCODE = string.Empty;
                //    //    int WorkFlowStatus = new WorkFLowMgt().ExecuteWorkFlowTransactionProcess(new WorkFLowMgt().SetProcedureParam(commonEntity, targetUserID, NextId, 0, comment, 0, 0, 1, "", 0));
                //    String MessageName = System.Enum.GetName(typeof(workFlowTranEnum_MessageName), (int)workFlowTranEnum_MessageName.Created);
                //    int WorkFlowStatus = new WorkFLowMgt().ExecuteWorkFlowTransactionProcess(new WorkFLowMgt().SetProcedureParam(commonEntity, targetUserID, NextId, (int)workFlowTranEnum_IsApproved.False, comment, (int)workFlowTranEnum_IsUpdate.False, (int)workFlowTranEnum_IsDelete.False, (int)workFlowTranEnum_Status.Active, APPROVALCUSTOMCODE, (int)workFlowTranEnum_IsDeclained.False, MessageName));

                //}
                #endregion WorkFlow Transaction Entry
                #region CustomCode And Primary Key  Process Update Process
                //Update Primary Key and Custom Code 
                GenericFactoryEF_Company.updateMaxID("CmnCompany", Convert.ToInt64(NextId));
                //  GFactory_EF_CmnCustomCode.updateCustomCode(customCodeID, DateTime.Now, 0, 1, 1);
                #endregion  CustomCode And Primary Key  Process Update Process

                #region WorkFlow Transaction Entry Team 
                int workflowID = 0;
                List<vmCmnWorkFlowMaster> listWorkFlow = new WorkFLowMgt().GetWorkFlowMasterListByMenu(commonEntity);
                foreach (vmCmnWorkFlowMaster item in listWorkFlow)
                {
                    int userTeamID = item.UserTeamID ?? 0;
                    if (new WorkFLowMgt().checkUserValidation(commonEntity.loggedUserID ?? 0, item.WorkFlowID) && userTeamID > 0)
                    {
                        item.WorkFlowTranCustomID = model.CompanyID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                        //Mail Service 

                    }
                    if (userTeamID == 0)
                    {
                        item.WorkFlowTranCustomID = model.CompanyID;
                        workflowID = new WorkFLowMgt().ExecuteTransactionProcess(item, commonEntity);
                    }
                }
                #endregion Workflow Transaction Enltry Team

            }
            catch (Exception)
            {
                throw new Exception();
            }

            return result;
        }


        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveCompany(CmnCompany model)
        {
            GenericFactoryEF_Company = new CmnCompany_EF();
            GFactory_EF_CmnCustomCode = new SalCmnCustomCode_EF();

            int result = 0;
            try
            {
                #region CustomCode And Primary Key  Process
                int NextId = Convert.ToInt16(GenericFactoryEF_Company.getMaxID("CmnCompany"));
                model.CompanyID = NextId;
                //......... START for custom code........... //
                //string customCode = "";
                CmnCustomCode objCustomCode = GFactory_EF_CmnCustomCode.FindBy(m => m.MenuID == model.MenuID && m.IsDeleted == false).FirstOrDefault();
                //int customCodeID = Convert.ToInt16(objCustomCode.RecordID);
                //if (customCodeID > 0)
                //{
                //  //  customCode = GenericFactory_EF_SalPIMaster.getCustomCode(customCodeID, DateTime.Now, itemMaster.CompanyID, 1, 1);
                //    customCode = GFactory_EF_CmnCustomCode.getCustomCode(customCodeID, DateTime.Now, 0, 1, 1);
                //}
                //else
                //{
                //    customCode = NextId.ToString();
                //}
                //.........END for custom code............ //

                #endregion CustomCode And Primary Key  Process
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("CompanyShortName", model.CompanyShortName);
                ht.Add("CompanyName", model.CompanyName);
                //ht.Add("StatusID", model.StatusID);
                string spQuery = "[Set_CmnCompany]";
                result = GenericFactoryFor_Company.ExecuteCommand(spQuery, ht);

                //Update Primary Key and Custom Code 
                GenericFactoryEF_Company.updateMaxID("CmnCompany", Convert.ToInt64(NextId));
                //GFactory_EF_CmnCustomCode.updateCustomCode(customCodeID, DateTime.Now, 0, 1, 1);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateCompany(CmnCompany model, UserCommonEntity commonEntity)
        {
            GenericFactoryFor_Company = new CmnCompany_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", model.CompanyID);
                ht.Add("CustomCode", model.CustomCode);
                ht.Add("CompanyShortName", model.CompanyShortName);
                ht.Add("CompanyName", model.CompanyName);
                ht.Add("LoggedCompanyID", commonEntity.loggedCompnyID);
                ht.Add("LoggedUser", commonEntity.loggedUserID);
                string spQuery = "[Put_CmnCompany]";
                result = GenericFactoryFor_Company.ExecuteCommand(spQuery, ht);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteCompany(int? CompanyID, UserCommonEntity commonEntity)
        {
            GenericFactoryFor_Company = new CmnCompany_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("CompanyID", CompanyID);
                ht.Add("LoggedCompanyID", commonEntity.loggedCompnyID);
                ht.Add("LoggedUser", commonEntity.loggedUserID);
                string spQuery = "[Delete_CmnCompany]";
                result = GenericFactoryFor_Company.ExecuteCommand(spQuery, ht);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }

            return result;
        }



     
    }
}
