using ABS.Models;
using ABS.Models.ViewModel.TokenModel;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.UserFactory
{
   public class UserAuthorizations
    {
        public ERP_Entities _ctx = null;
        public tokenModel objModel = null;

        public bool ValidateToken(string authorizedToken, string userAgent)
        {
            bool result = false;
            try
            {
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(authorizedToken));
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 5)
                {
                    objModel = new tokenModel()
                    {
                        clientToken = parts[0],
                        userid = parts[1],
                        methodtype = parts[2],
                        menuID = parts[3],
                        companyID = parts[4],
                        ip = HostService.GetIP()
                    };

                    //compare token
                    string serverToken = generateToken(objModel.userid, objModel.methodtype, objModel.ip, userAgent);
                    if (objModel.clientToken == serverToken)
                    {
                        result = ValidateAuthorization(objModel.userid, objModel.methodtype, objModel.menuID, objModel.companyID);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }

        public string generateToken(string userid, string methodtype, string ip, string userAgent)
        {
            string message = string.Join(":", new string[] { userid, ip, userAgent });
            string key = methodtype ?? "";

            var encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public bool ValidateAuthorization(string userid, string methodtype,string menuID,string companyID)
        {
            bool IsValid = false;
            int pageMenuID = 0;
            int currentComapnyID = 0;
            int currentUserID = 0;
            if (!string.IsNullOrEmpty(menuID)) pageMenuID = Convert.ToInt16(menuID);
            if (!string.IsNullOrEmpty(companyID)) currentComapnyID = Convert.ToInt16(companyID);
            if (!string.IsNullOrEmpty(userid)) currentUserID = Convert.ToInt16(userid);


            if (userid != null)
            {
                using (_ctx = new ERP_Entities())
                {
                    if (!_ctx.CmnUserAuthentications.Any(u => u.LoginID == userid && u.StatusID == 1))
                    {
                        switch (methodtype)
                        {
                            case "get":
                                IsValid = (from permission in _ctx.CmnMenuPermissions
                                           where permission.IsDeleted == false
                                           && permission.UserID== currentUserID
                                           && permission.MenuID == pageMenuID
                                           && permission.CompanyID== currentComapnyID
                                           && permission.EnableView==true
                                           select permission).Any() ;
                                break;
                            case "post":
                                IsValid = (from permission in _ctx.CmnMenuPermissions
                                           where permission.IsDeleted == false
                                             && permission.UserID == currentUserID
                                           && permission.MenuID == pageMenuID
                                           && permission.CompanyID == currentComapnyID
                                           && permission.EnableInsert == true
                                           select permission).Any();
                                break;
                            case "put":
                                IsValid = (from permission in _ctx.CmnMenuPermissions
                                           where permission.IsDeleted == false
                                           && permission.UserID == currentUserID
                                           && permission.MenuID == pageMenuID
                                           && permission.CompanyID == currentComapnyID
                                           && permission.EnableUpdate == true
                                           select permission).Any();
                                break;
                            case "delete":
                                IsValid = (from permission in _ctx.CmnMenuPermissions
                                           where permission.IsDeleted == false
                                           && permission.UserID == currentUserID
                                           && permission.MenuID == pageMenuID
                                           && permission.CompanyID == currentComapnyID
                                           && permission.EnableDelete == true
                                           select permission).Any();
                                break;
                            default:
                                IsValid = (from permission in _ctx.CmnMenuPermissions
                                           where permission.IsDeleted == false
                                           && permission.UserID == currentUserID
                                           && permission.MenuID == pageMenuID
                                           && permission.CompanyID == currentComapnyID
                                           && permission.EnableView == true
                                           select permission).Any();
                                break;
                        }
                    }
                }
            }
            return IsValid;
        }
    }
}
