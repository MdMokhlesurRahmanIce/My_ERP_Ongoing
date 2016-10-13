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
using ABS.Service.AllServiceClasses;
using ABS.Utility;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnUserMgt : iCmnUserMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory<vmAuthenticatedUser> GenericFactoryFor_AuthUser = null;
        private iGenericFactory<vmUser> GenericFactoryFor_User = null;
        private iGenericFactory<vmUserGroup> GenericFactoryFor_UserGroup = null;
        private iGenericFactory<vmUserType> GenericFactoryFor_UserType = null;
        private iGenericFactory<vmRecoverUser> GenericFactoryFor_RecUser = null;
        private iGenericFactory_EF<CmnUserWiseCompany> GenericFactory_EF_CmnUserWiseCompany = null;
        private iGenericFactory_EF<CmnUserAuthentication> GenericFactory_EF_AuthenticatedUser = null;

        #region Login
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public vmAuthenticatedUser Get_CmnUserAuthentication(vmLoginUser model)
        {
            vmAuthenticatedUser objAuthUser = null;
            string spQuery = string.Empty;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_AuthUser = new vmAuthenticatedUser_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("UserLogin", model.UserLogin.Trim());
                        ht.Add("Password", model.Password.Trim());

                        spQuery = "[Get_CmnUserAuthentication]";
                        objAuthUser = GenericFactoryFor_AuthUser.ExecuteCommandSingle(spQuery, ht);
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objAuthUser;
        }
        #endregion

        #region Recovery
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public vmRecoverUser Get_CmnUserRecovery(vmRecoverUser model)
        {
            vmRecoverUser recoveredUser = null;
            string spQuery = string.Empty;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_RecUser = new vmRecoveryUser_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("RecoverEmail", model.RecoverEmail.Trim());

                        spQuery = "[Get_CmnUserRecovery]";
                        recoveredUser = GenericFactoryFor_RecUser.ExecuteCommandSingle(spQuery, ht);
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return recoveredUser;
        }
        #endregion

        #region Create
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveUserType(vmUserType model)
        {
            int result = 0;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_UserType = new vmUserType_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", model.CompanyID);
                        ht.Add("LoggedUser", model.LoggedUser);
                        ht.Add("UserTypeName", model.UserTypeName);
                        ht.Add("ParentID", model.ParentID);

                        string spQuery = "[Set_CmnUserType]";
                        result = GenericFactoryFor_UserType.ExecuteCommand(spQuery, ht);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveUserGroup(vmUserGroup model)
        {
            int result = 0;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_UserGroup = new vmUserGroup_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", model.CompanyID);
                        ht.Add("LoggedUser", model.LoggedUser);
                        ht.Add("GroupName", model.GroupName);
                        ht.Add("Sequence", model.Sequence);

                        string spQuery = "[Set_CmnUserGroup]";
                        result = GenericFactoryFor_UserGroup.ExecuteCommand(spQuery, ht);
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public vmUser SaveUser(vmUser model, List<vmCompany> companyList)
        {
            vmUser result = null;
            try
            {

                if (model != null && model.UserGroupID > 0)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", model.CompanyID);
                    ht.Add("LoggedUser", model.LoggedUser);

                    //Login Account
                    ht.Add("LoginID", model.LoginID ?? null);
                    ht.Add("Email", model.LoginEmail);
                    ht.Add("Phone", model.LoginPhone ?? null);

                    //User Type
                    ht.Add("UserTypeID", model.UserTypeID);
                    ht.Add("UserGroupID", model.UserGroupID);
                    ht.Add("UserTitleID", model.UserTitleID);

                    //User Info
                    ht.Add("UserFirstName", model.UserFirstName ?? null);
                    ht.Add("UserMiddleName", model.UserMiddleName ?? null);
                    ht.Add("UserLastName", model.UserLastName ?? null);
                    ht.Add("GenderID", model.GenderID);
                    ht.Add("ReligionID", model.ReligionID);

                    ht.Add("FathersName", model.FathersName ?? null);
                    ht.Add("MothersName", model.MothersName ?? null);
                    ht.Add("SpouseNane", model.SpouseNane ?? null);

                    //User Parmanent Address
                    ht.Add("ParAddress1", model.ParAddress1 ?? "");
                    ht.Add("ParAddress2", model.ParAddress2 ?? "");
                    ht.Add("ParCountryID", model.ParCountryID);
                    ht.Add("ParStateID", model.ParStateID);
                    ht.Add("ParCityID", model.ParCityID);

                    //User Present Address
                    ht.Add("PreAddress1", model.PreAddress1 ?? "");
                    ht.Add("PreAddress2", model.PreAddress2 ?? "");
                    ht.Add("PreCountryID", model.PreCountryID);
                    ht.Add("PreStateID", model.PreStateID);
                    ht.Add("PreCityID", model.PreCityID);

                    //User Identity
                    ht.Add("UniqueIdentity", model.UniqueIdentity);
                    ht.Add("BloodGroup", model.BloodGroup ?? "");

                    ht.Add("Height", model.Height);
                    ht.Add("DOB", model.DOB);
                    ht.Add("PassportNO", model.PassportNO);
                    ht.Add("NID", model.NID);

                    ht.Add("ImageUrl", model.ImageUrl ?? "");
                    ht.Add("FingerUrl", model.FingerUrl ?? "");
                    ht.Add("SignatureUrl", model.SignatUrl ?? "");

                    //Open User Login Account
                    ht.Add("IsOnlineAccount", model.IsOnlineAccount);

                    //User Job Contract
                    ht.Add("DesignationID", model.DesignationID);
                    ht.Add("DepartmentID", model.DepartmentID);
                    ht.Add("JobContractTypeID", model.JobContractTypeID);

                    //for  CmnACCIntegration 
                    ht.Add("AcDetailID", model.AcDetailID);

                    if (model.UserID > 0)
                    {
                        using (GenericFactoryFor_User = new vmUser_GF())
                        {
                            ht.Add("UserID", model.UserID);
                            string spQuery = "[Put_CmnUser]";
                            result = GenericFactoryFor_User.ExecuteCommandSingle(spQuery, ht);
                            if (result != null && companyList.Count > 0)
                            {
                                UpdateUserWiseCompany(model.UserID ?? 0, model.LoggedUser, companyList);
                            }
                        }
                    }
                    else
                    {
                        using (GenericFactoryFor_User = new vmUser_GF())
                        {
                            string spQuery = "[Set_CmnUser]";
                            result = GenericFactoryFor_User.ExecuteCommandSingle(spQuery, ht);
                        }
                        if (result != null && companyList.Count > 0)
                        {
                            SaveUserWiseCompany(result.UserID ?? 0, model.LoggedUser, companyList);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        //private List<vmCompany> GetCompanyByUserID(int? userID)
        //{
        //    List<vmCompany> _companylist = (from uc in _ctxCmn.CmnUserWiseCompanies
        //                                    join c in _ctxCmn.CmnCompanies on uc.CompanyID equals c.CompanyID
        //                                    where uc.UserID == userID && uc.IsDeleted == false
        //                                    select new vmCompany
        //                                    {
        //                                        id = c.CompanyID,
        //                                        label = c.CompanyName
        //                                    }).ToList();
        //    return _companylist;


        //}

        private void UpdateUserWiseCompany(int userID, int? loggedUser, List<vmCompany> companyList)
        {

            try
            {
                List<vmCompany> _newCompanyList = GetNewComapny(companyList, userID);
                List<vmCompany> _deleteCompanyList = GetDeleteComapny(companyList, userID);
                SaveUserWiseCompany(userID, loggedUser, _newCompanyList);
                ModifyUserWiseCompany(userID, loggedUser, _deleteCompanyList);
                UpdateIsDefult(companyList, loggedUser, userID);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void UpdateIsDefult(List<vmCompany> companyList, int? loggedUser, int userID)
        {
            foreach (vmCompany aitem in companyList)
            {
                CmnUserWiseCompany _userWiseCompanyObj = GenericFactory_EF_CmnUserWiseCompany.FindBy(x => x.IsDeleted == false && x.UserID == userID && x.CompanyID == aitem.id).FirstOrDefault();
                _userWiseCompanyObj.UpdateOn = DateTime.Today;
                _userWiseCompanyObj.UpdateBy = loggedUser;
                _userWiseCompanyObj.UpdatePc = HostService.GetIP();
                _userWiseCompanyObj.IsDefault = aitem.IsDefult;
                GenericFactory_EF_CmnUserWiseCompany.Update(_userWiseCompanyObj);
                GenericFactory_EF_CmnUserWiseCompany.Save();
            }
        }

        private void ModifyUserWiseCompany(int? userID, int? loggedUser, List<vmCompany> _deleteCompanyList)
        {
            try
            {
                foreach (vmCompany aitem in _deleteCompanyList)
                {
                    CmnUserWiseCompany _userWiseCompanyObj = GenericFactory_EF_CmnUserWiseCompany.FindBy(x => x.IsDeleted == false && x.UserID == userID && x.CompanyID == aitem.id).FirstOrDefault();
                    _userWiseCompanyObj.DeleteOn = DateTime.Today;
                    _userWiseCompanyObj.DeleteBy = loggedUser;
                    _userWiseCompanyObj.DeletePc = HostService.GetIP();
                    _userWiseCompanyObj.IsDeleted = true;
                    GenericFactory_EF_CmnUserWiseCompany.Update(_userWiseCompanyObj);
                    GenericFactory_EF_CmnUserWiseCompany.Save();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<vmCompany> GetDeleteComapny(List<vmCompany> companyList, int? userID)
        {
            _ctxCmn = new ERP_Entities();
            List<vmCompany> _DeleteList = new List<vmCompany>();
            List<CmnUserWiseCompany> cmUserWiseComapnylist = _ctxCmn.CmnUserWiseCompanies.Where(x => x.UserID == userID && x.IsDeleted == false).ToList();
            foreach (CmnUserWiseCompany aitem in cmUserWiseComapnylist)
            {
                vmCompany isExistCompany = companyList.Where(x => x.id == aitem.CompanyID).FirstOrDefault();
                if (isExistCompany == null)
                {
                    vmCompany _objvmCompany = new vmCompany();
                    _objvmCompany.id = aitem.CompanyID;
                    _DeleteList.Add(_objvmCompany);
                }
            }
            return _DeleteList;

        }

        private List<vmCompany> GetNewComapny(List<vmCompany> companyList, int? userID)
        {
            _ctxCmn = new ERP_Entities();
            List<vmCompany> _newCompanyList = new List<vmCompany>();

            foreach (vmCompany acompany in companyList)
            {
                CmnUserWiseCompany cmUserWiseComapny = _ctxCmn.CmnUserWiseCompanies.Where(x => x.UserID == userID && x.CompanyID == acompany.id && x.IsDeleted == false).FirstOrDefault();
                if (cmUserWiseComapny == null)
                {
                    _newCompanyList.Add(acompany);

                }

            }
            return _newCompanyList;

        }

        private void SaveUserWiseCompany(int userID, int? loggedUser, List<vmCompany> companyList)
        {

            GenericFactory_EF_CmnUserWiseCompany = new CmnUserWiseComapny_EF();
            try
            {
                Int64 userCompanyId = GenericFactory_EF_CmnUserWiseCompany.getMaxVal_int64("UserCompanyID", "CmnUserWiseCompany");
                foreach (vmCompany aitem in companyList)
                {

                    CmnUserWiseCompany _userwiseCompany = new CmnUserWiseCompany();
                    _userwiseCompany.UserCompanyID = userCompanyId;
                    _userwiseCompany.UserID = userID;
                    _userwiseCompany.CompanyID = Convert.ToInt16(aitem.id);
                    _userwiseCompany.CreateBy = loggedUser;
                    _userwiseCompany.CreateOn = DateTime.Now;
                    _userwiseCompany.IsDeleted = false;
                    _userwiseCompany.IsDefault = aitem.IsDefult;
                    _userwiseCompany.CreatePc = HostService.GetIP();
                    GenericFactory_EF_CmnUserWiseCompany.Insert(_userwiseCompany);
                    GenericFactory_EF_CmnUserWiseCompany.Save();
                    userCompanyId++;
                }
            }
            catch
            {


            }
        }
        #endregion

        #region Read
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmUserGroup> GetUserGroup(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmUserGroup> objUserGroup = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                if (objcmnParam != null)
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", objcmnParam.loggedCompany);
                        ht.Add("LoggedUser", objcmnParam.loggeduser);

                        ht.Add("PageNo", objcmnParam.pageNumber);
                        ht.Add("RowCountPerPage", objcmnParam.pageSize);
                        ht.Add("IsPaging", objcmnParam.IsPaging);

                        spQuery = "[Get_CmnUserGroup]";

                        using (GenericFactoryFor_UserGroup = new vmUserGroup_GF())
                        {
                            objUserGroup = GenericFactoryFor_UserGroup.ExecuteQuery(spQuery, ht);
                        }
                        recordsTotal = _ctxCmn.CmnUserGroups.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUserGroup;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmUserType> GetUserType(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmUserType> objUserType = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                if (objcmnParam != null)
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", objcmnParam.loggedCompany);
                        ht.Add("LoggedUser", objcmnParam.loggeduser);

                        ht.Add("PageNo", objcmnParam.pageNumber);
                        ht.Add("RowCountPerPage", objcmnParam.pageSize);
                        ht.Add("IsPaging", objcmnParam.IsPaging);

                        spQuery = "[Get_CmnUserType]";
                        using (GenericFactoryFor_UserType = new vmUserType_GF())
                        {
                            objUserType = GenericFactoryFor_UserType.ExecuteQuery(spQuery, ht);
                        }
                        recordsTotal = _ctxCmn.CmnUserTypes.Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUserType;
        }

        public IEnumerable<vmUser> GetUser(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmUser> objUser = null;
            string spQuery = string.Empty;
            recordsTotal = 0;
            try
            {
                if (objcmnParam != null)
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", objcmnParam.loggedCompany);
                        ht.Add("LoggedUser", objcmnParam.loggeduser);
                        ht.Add("UserType", objcmnParam.UserType);

                        ht.Add("PageNo", objcmnParam.pageNumber);
                        ht.Add("RowCountPerPage", objcmnParam.pageSize);
                        ht.Add("IsPaging", objcmnParam.IsPaging);

                        spQuery = "[Get_CmnUser]";
                        using (GenericFactoryFor_User = new vmUser_GF())
                        {
                            objUser = GenericFactoryFor_User.ExecuteQuery(spQuery, ht);
                        }

                        recordsTotal = _ctxCmn.CmnUsers.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.UserTypeID == objcmnParam.UserType).Count();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUser;
        }

        /// <summary>
        /// Single USer From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public vmUser GetUserByID(int? id, int? CompanyID, int? LoggedUser)
        {
            vmUser objUser = null;
            try
            {
                if ((id > 0) && (CompanyID > 0))
                {
                    using (GenericFactoryFor_User = new vmUser_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", CompanyID);
                        ht.Add("LoggedUser", LoggedUser);

                        ht.Add("UserID", id);

                        string spQuery = "[Get_CmnUserByID]";
                        objUser = GenericFactoryFor_User.ExecuteQuerySingle(spQuery, ht);
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUser;
        }
        #endregion

        #region Update
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateUserType(vmUserType model)
        {
            int result = 0;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_UserType = new vmUserType_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", model.CompanyID);
                        ht.Add("LoggedUser", model.LoggedUser);

                        ht.Add("UserTypeID", model.UserTypeID);
                        ht.Add("UserTypeName", model.UserTypeName);
                        ht.Add("ParentID", model.ParentID);

                        string spQuery = "[Put_CmnUserType]";
                        result = GenericFactoryFor_UserType.ExecuteCommand(spQuery, ht);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateUserGroup(vmUserGroup model)
        {
            int result = 0;
            try
            {
                if (model != null)
                {
                    using (GenericFactoryFor_UserGroup = new vmUserGroup_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", model.CompanyID);
                        ht.Add("LoggedUser", model.LoggedUser);

                        ht.Add("UserGroupID", model.UserGroupID);
                        ht.Add("GroupName", model.GroupName);
                        ht.Add("Sequence", model.Sequence);

                        string spQuery = "[Put_CmnUserGroup]";
                        result = GenericFactoryFor_UserGroup.ExecuteCommand(spQuery, ht);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteUser(int? id, int? CompanyID, int? LoggedUser)
        {
            int result = 0;
            try
            {
                if ((id > 0) && (CompanyID > 0))
                {
                    using (GenericFactoryFor_User = new vmUser_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", CompanyID);
                        ht.Add("LoggedUser", LoggedUser);

                        ht.Add("UserID", id);

                        string spQuery = "[Delete_CmnUser]";
                        result = GenericFactoryFor_User.ExecuteCommand(spQuery, ht);
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteUserGroup(int? id, int? CompanyID, int? LoggedUser)
        {
            int result = 0;
            try
            {
                if ((id > 0) && (CompanyID > 0))
                {
                    using (GenericFactoryFor_UserGroup = new vmUserGroup_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", CompanyID);
                        ht.Add("LoggedUser", LoggedUser);

                        ht.Add("UserGroupID", id);

                        string spQuery = "[Delete_CmnUserGroup]";
                        result = GenericFactoryFor_UserGroup.ExecuteCommand(spQuery, ht);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteUserType(int? id, int? CompanyID, int? LoggedUser)
        {
            int result = 0;
            try
            {
                if ((id > 0) && (CompanyID > 0))
                {
                    using (GenericFactoryFor_UserType = new vmUserType_GF())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("CompanyID", CompanyID);
                        ht.Add("LoggedUser", LoggedUser);

                        ht.Add("UserTypeID", id);

                        string spQuery = "[Delete_CmnUserType]";
                        result = GenericFactoryFor_UserType.ExecuteCommand(spQuery, ht);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
        #endregion

        public string getCurrentPassword(int companyID, int loggedUser)
        {
            GenericFactory_EF_AuthenticatedUser = new CmnUserUserAuthentication_EF();

            string currentPassword = GenericFactory_EF_AuthenticatedUser.FindBy(x => x.UserID == loggedUser).FirstOrDefault().ConfirmPassword;
            return currentPassword;
        }

       public int ChangePassword(Models.CmnUserAuthentication model)
        {
            int result = 0;
            GenericFactory_EF_AuthenticatedUser = new CmnUserUserAuthentication_EF();
            try
            {
                CmnUserAuthentication _objUserAuthentication = GenericFactory_EF_AuthenticatedUser.FindBy(x => x.UserID == model.UserID).FirstOrDefault();


                _objUserAuthentication.Password = model.Password;
                _objUserAuthentication.ConfirmPassword = model.Password;
                _objUserAuthentication.UpdateBy = model.UpdateBy;
                _objUserAuthentication.UpdatePc = HostService.GetIP();
                _objUserAuthentication.UpdateOn = DateTime.Now;
                GenericFactory_EF_AuthenticatedUser.Update(_objUserAuthentication);
                GenericFactory_EF_AuthenticatedUser.Save();
                result = 1;
            }
            catch (Exception)
            {
                
               
            }
            return result;
        }
    }
}
