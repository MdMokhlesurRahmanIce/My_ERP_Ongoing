using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Sales.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility;
namespace ABS.Service.SystemCommon.Factories
{
    public class ApprovalSetupMgt : iApprovalSetupMgt
    {
        private iGenericFactory_EF<CmnWorkFlowMaster> GenericFactory_EF_CmnWorkFlowMaster = null;
        private iGenericFactory_EF<CmnWorkFlowDetail> GenericFactory_EF_CmnWorkFlowDetail = null;
        private iGenericFactory_EF<CmnCompany> GenericFactory_EF_CmnCompany = null;
        private iGenericFactory_EF<CmnMenu> GenericFactory_EF_CmnMenu = null;
        private iGenericFactory_EF<CmnOrganogram> GenericFactory_EF_CmnOrganogram = null;
        private iGenericFactory<vmTeam> team_FactoryObj = null;


        private ERP_Entities _ctxCmn = null;

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public string SaveUpdateApprovalSetup(CmnWorkFlowMaster workFlowMaster, List<CmnWorkFlowDetail> workFlowDetail)
        {
            GenericFactory_EF_CmnWorkFlowMaster = new CmnWorkFlowMaster_EF();
            GenericFactory_EF_CmnWorkFlowDetail = new CmnWorkFlowDetail_EF();
            string result = string.Empty;
            if (workFlowMaster.WorkFlowID > 0)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        //start for Update in CmnWorkFlowMaster
                        int workFlowID = workFlowMaster.WorkFlowID;
                        IEnumerable<CmnWorkFlowMaster> lstCmnWorkFlowMaster = GenericFactory_EF_CmnWorkFlowMaster.FindBy(m => m.WorkFlowID == workFlowID).ToList();
                        CmnWorkFlowMaster objCmnWorkFlowMaster = new CmnWorkFlowMaster();
                        foreach (CmnWorkFlowMaster wfm in lstCmnWorkFlowMaster)
                        {
                            wfm.MenuID = workFlowMaster.MenuID;
                            wfm.IsActive = workFlowMaster.IsActive;
                            wfm.BranchID = workFlowMaster.BranchID;
                            wfm.CompanyID = workFlowMaster.CompanyID;
                            wfm.UpdateOn = DateTime.Now;
                            wfm.UpdatePc =  HostService.GetIP();
                            objCmnWorkFlowMaster = wfm;
                        }

                        GenericFactory_EF_CmnWorkFlowMaster.Update(objCmnWorkFlowMaster);
                        GenericFactory_EF_CmnWorkFlowMaster.Save();
                        // end for Update in CmnWorkFlowMaster

                        List<CmnWorkFlowDetail> lstCmnWorkFlowDetail = new List<CmnWorkFlowDetail>();
                        foreach (CmnWorkFlowDetail wfd in workFlowDetail)
                        {
                            CmnWorkFlowDetail objCmnWorkFlowDetail = GenericFactory_EF_CmnWorkFlowDetail.FindBy(m => m.WorkFlowDetailID == wfd.WorkFlowDetailID).FirstOrDefault();
                            objCmnWorkFlowDetail.CompanyID = wfd.CompanyID;
                            objCmnWorkFlowDetail.EmployeeID = wfd.EmployeeID;
                            objCmnWorkFlowDetail.Sequence = wfd.Sequence;
                            objCmnWorkFlowDetail.StatusID = wfd.StatusID;
                            objCmnWorkFlowDetail.IsDeleted = false;
                            objCmnWorkFlowDetail.UpdateOn = wfd.UpdateOn;
                            objCmnWorkFlowDetail.UpdatePc =  HostService.GetIP();
                            lstCmnWorkFlowDetail.Add(objCmnWorkFlowDetail);
                        }
                        GenericFactory_EF_CmnWorkFlowDetail.UpdateList(lstCmnWorkFlowDetail);
                        GenericFactory_EF_CmnWorkFlowDetail.Save();
                        transaction.Complete();
                        result = workFlowMaster.WorkFlowID.ToString();
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    //...........START  new maxId........//

                    //int NextId = Convert.ToInt16(GenericFactory_EF_CmnWorkFlowMaster.getMaxID("CmnWorkFlowMaster"));
                    //int nextWorkFlowDetailID = Convert.ToInt16(GenericFactory_EF_CmnWorkFlowDetail.getMaxID("CmnWorkFlowDetail"));

                    //...........START  new maxId........//
                    long NextId = Convert.ToInt64(GenericFactory_EF_CmnWorkFlowMaster.getMaxID("CmnWorkFlowMaster"));

                    long FirstDigit = 0;
                    long OtherDigits = 0;
                    long nextDetailId = Convert.ToInt64(GenericFactory_EF_CmnWorkFlowDetail.getMaxID("CmnWorkFlowDetail"));
                    FirstDigit = Convert.ToInt64(nextDetailId.ToString().Substring(0, 1));
                    OtherDigits = Convert.ToInt64(nextDetailId.ToString().Substring(1, nextDetailId.ToString().Length - 1));

                    //..........END new maxId.........//

                    //..........END new maxId.........//
                    try
                    {
                        workFlowMaster.WorkFlowID = Convert.ToInt16(NextId);
                        workFlowMaster.CreateOn = DateTime.Now;
                        workFlowMaster.CreatePc =  HostService.GetIP();
                        List<CmnWorkFlowDetail> lstWorkFlowDetail = new List<CmnWorkFlowDetail>();
                        foreach (CmnWorkFlowDetail wfd in workFlowDetail)
                        {
                            CmnWorkFlowDetail objCmnWorkFlowDetail = new CmnWorkFlowDetail();
                            objCmnWorkFlowDetail.WorkFlowDetailID = Convert.ToInt16(FirstDigit + "" + OtherDigits);//nextWorkFlowDetailID;
                            objCmnWorkFlowDetail.WorkFlowID = Convert.ToInt16(NextId);//NextId;
                            objCmnWorkFlowDetail.CompanyID = workFlowMaster.CompanyID ?? 0;
                            objCmnWorkFlowDetail.DBID = wfd.DBID;
                            objCmnWorkFlowDetail.EmployeeID = wfd.EmployeeID;
                            objCmnWorkFlowDetail.Sequence = wfd.Sequence;
                            objCmnWorkFlowDetail.StatusID = wfd.StatusID;
                            objCmnWorkFlowDetail.IsDeleted = false;
                            objCmnWorkFlowDetail.CreateBy = wfd.CreateBy;
                            objCmnWorkFlowDetail.CreateOn = DateTime.Now;
                            objCmnWorkFlowDetail.CreatePc =  HostService.GetIP();
                            objCmnWorkFlowDetail.Transfer = false;
                            lstWorkFlowDetail.Add(objCmnWorkFlowDetail);
                            // nextWorkFlowDetailID++;
                            OtherDigits++;
                        }

                        GenericFactory_EF_CmnWorkFlowMaster.Insert(workFlowMaster);
                        GenericFactory_EF_CmnWorkFlowMaster.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_CmnWorkFlowMaster.updateMaxID("CmnWorkFlowMaster", Convert.ToInt64(NextId));
                        GenericFactory_EF_CmnWorkFlowDetail.InsertList(lstWorkFlowDetail);
                        GenericFactory_EF_CmnWorkFlowDetail.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_CmnWorkFlowDetail.updateMaxID("CmnWorkFlowDetail", Convert.ToInt16(FirstDigit + "" + (OtherDigits - 1)));
                        // GenericFactory_EF_SalPIDetail.updateMaxID("SalPIDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                        transaction.Complete();

                        result = NextId.ToString();
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        result = "";
                    }
                }

            }
            return result;
        }

        public IEnumerable<vmCmnMenuPermission> GetApprovalSetupRecords()
        {
            GenericFactory_EF_CmnCompany = new CmnCompany_EF();
            GenericFactory_EF_CmnMenu = new CmnMenu_EF();
            GenericFactory_EF_CmnOrganogram = new CmnOrganogram_EF();
            IEnumerable<vmCmnMenuPermission> lstCmnMenuPermission = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstCmnMenuPermission = (from masterWorkFlow in _ctxCmn.CmnWorkFlowMasters.Where(m => m.IsDeleted == false)
                                            // join detailWorkFlow in GenericFactory_EF_CmnWorkFlowDetail.GetAll() on masterWorkFlow.WorkFlowID equals detailWorkFlow.WorkFlowID
                                            // join user in GenericFactory_EF_CmnUser.GetAll() on detailWorkFlow.EmployeeID equals user.UserID
                                            join company in _ctxCmn.CmnCompanies on masterWorkFlow.CompanyID equals company.CompanyID
                                            join menu in _ctxCmn.CmnMenus on masterWorkFlow.MenuID equals menu.MenuID
                                            join organogram in _ctxCmn.CmnOrganograms on masterWorkFlow.BranchID equals organogram.OrganogramID
                                            // join status in GenericFactory_EF_CmnStatu.GetAll() on detailWorkFlow.StatusID equals status.StatusID
                                            select new
                                            {
                                                WorkFlowID = masterWorkFlow.WorkFlowID,
                                                IsActive = masterWorkFlow.IsActive,
                                                MenuID = masterWorkFlow.MenuID,
                                                OrganogramID = masterWorkFlow.BranchID,
                                                WorkFlowDetailID = 0,//detailWorkFlow.WorkFlowDetailID,
                                                StatusID = 0,// detailWorkFlow.StatusID,
                                                StatusName = "",// status.StatusName,
                                                UserID = 0,// detailWorkFlow.EmployeeID, 
                                                Sequence = 0,//detailWorkFlow.Sequence,
                                                CompanyID = masterWorkFlow.CompanyID,
                                                MenuName = menu.MenuName,
                                                CompanyName = company.CompanyName,
                                                OrganogramName = organogram.OrganogramName,
                                                UserFullName = "",//user.UserFirstName 
                                                IsDeleted = masterWorkFlow.IsDeleted
                                            }).ToList().Select(x => new vmCmnMenuPermission
                                            {
                                                WorkFlowID = x.WorkFlowID,
                                                IsActive = x.IsActive,
                                                MenuID = x.MenuID,
                                                OrganogramID = x.OrganogramID ?? 0,
                                                WorkFlowDetailID = x.WorkFlowDetailID,
                                                StatusID = x.StatusID,
                                                StatusName = x.StatusName,
                                                UserID = x.UserID,
                                                Sequence = x.Sequence,
                                                CompanyID = x.CompanyID ?? 0,
                                                MenuName = x.MenuName,
                                                CompanyName = x.CompanyName,
                                                OrganogramName = x.OrganogramName,
                                                UserFullName = x.UserFullName,
                                                IsDeleted = x.IsDeleted
                                            }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstCmnMenuPermission;
        }

        public IEnumerable<vmCmnMenuPermission> GetApprovalDetailsByWorkFlowID(int workFlowID)
        {

            IEnumerable<vmCmnMenuPermission> lstCmnMenuPermission = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstCmnMenuPermission = (from detailWorkFlow in _ctxCmn.CmnWorkFlowDetails.Where(m => m.IsDeleted == false && m.WorkFlowID == workFlowID)
                                            join masterWorkFlow in _ctxCmn.CmnWorkFlowMasters on detailWorkFlow.WorkFlowID equals masterWorkFlow.WorkFlowID
                                            join user in _ctxCmn.CmnUsers on detailWorkFlow.EmployeeID equals user.UserID
                                            join company in _ctxCmn.CmnCompanies on masterWorkFlow.CompanyID equals company.CompanyID
                                            join menu in _ctxCmn.CmnMenus on masterWorkFlow.MenuID equals menu.MenuID
                                            join organogram in _ctxCmn.CmnOrganograms on masterWorkFlow.BranchID equals organogram.OrganogramID
                                            join status in _ctxCmn.CmnStatus on detailWorkFlow.StatusID equals status.StatusID
                                            select new
                                            {
                                                WorkFlowID = masterWorkFlow.WorkFlowID,
                                                IsActive = masterWorkFlow.IsActive,
                                                MenuID = masterWorkFlow.MenuID,
                                                OrganogramID = masterWorkFlow.BranchID,
                                                WorkFlowDetailID = detailWorkFlow.WorkFlowDetailID,
                                                StatusID = detailWorkFlow.StatusID,
                                                StatusName = status.StatusName,
                                                UserID = detailWorkFlow.EmployeeID,
                                                Sequence = detailWorkFlow.Sequence,
                                                CompanyID = masterWorkFlow.CompanyID,
                                                MenuName = menu.MenuName,
                                                CompanyName = company.CompanyName,
                                                OrganogramName = organogram.OrganogramName,
                                                UserFullName = user.UserFullName,
                                                IsDeleted = detailWorkFlow.IsDeleted
                                            }).ToList().Select(x => new vmCmnMenuPermission
                                            {
                                                WorkFlowID = x.WorkFlowID,
                                                IsActive = x.IsActive,
                                                MenuID = x.MenuID,
                                                OrganogramID = x.OrganogramID ?? 0,
                                                WorkFlowDetailID = x.WorkFlowDetailID,
                                                StatusID = x.StatusID,
                                                StatusName = x.StatusName,
                                                UserID = x.UserID,
                                                Sequence = x.Sequence,
                                                CompanyID = x.CompanyID ?? 0,
                                                MenuName = x.MenuName,
                                                CompanyName = x.CompanyName,
                                                OrganogramName = x.OrganogramName,
                                                UserFullName = x.UserFullName,
                                                IsDeleted = x.IsDeleted
                                            }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstCmnMenuPermission;
        }
        public List<vmTeam> GetTeamsUserByTemID(int? TeamID)
        {
            List<vmTeam> Teams = null;
            string spQuery = string.Empty;
            try
            {
                using (team_FactoryObj = new Team_EF())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("TemID", TeamID);
                    spQuery = "[CmnTemsDetailsByTeamID]";
                    Teams = team_FactoryObj.ExecuteQuery(spQuery, ht).ToList();
                }
            }
            catch
            {


            }
            return Teams;

        }
    }
}
