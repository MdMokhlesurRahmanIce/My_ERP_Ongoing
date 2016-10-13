using ABS.Models;
using ABS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;

namespace ABS.Web.Utility
{
    public static class UniqueCode
    {



        public static string GetAutoNumber(string txt)
        {
            string id = null;

            try
            {
                switch (txt.ToLower())
                {
                    case "ac1":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccAC1.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break;
                    case "ac2":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccAC2.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break;

                    case "ac3":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccAC3.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break;
                    case "AC4":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccAC4.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break;
                    case "ac5":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccACDetails.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break; 
                    case "voucher":
                        using (var db = new ERP_Entities())
                        {
                            var data = db.AccVoucherMasters.ToList();
                            id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();

                        }
                        break;
                    default:
                        id = "1";
                        break;


                }

            }

            catch (Exception)
            {
                id = "1";
            }
            return id;
        }


        public static string GetAccountLedgerCode(string level, string autoNumber)
        {


            string levelCode = null;
            try
            {

                if (autoNumber != null && autoNumber.Length == 1)
                {
                    levelCode = level + "-0000" + autoNumber;
                }

                if (autoNumber != null && autoNumber.Length == 2)
                {
                    levelCode = level + "-000" + autoNumber;


                }


                if (autoNumber != null && autoNumber.Length == 3)
                {
                    levelCode = level + "-00" + autoNumber;


                }


                if (autoNumber != null && autoNumber.Length == 4)
                {
                    levelCode = level + "-0" + autoNumber;

                }
                if (autoNumber != null && autoNumber.Length == 5)
                {
                    levelCode = level + "-" + autoNumber;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return levelCode;
        }
        public static string GetSupplierCode(string serial)
        {


            string pvCode = null;
            try
            {
                string pvSerial = serial;




                if (pvSerial != null && pvSerial.Length == 1)
                {
                    pvCode = "000" + pvSerial;
                }

                if (pvSerial != null && pvSerial.Length == 2)
                {
                    pvCode = "00" + pvSerial;


                }


                if (pvSerial != null && pvSerial.Length == 3)
                {
                    pvCode = "0" + pvSerial;


                }


                if (pvSerial != null && pvSerial.Length == 4)
                {
                    pvCode = "" + pvSerial;

                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pvCode;
        }
        /*public static string VoucherCodeGenerate(string company, DateTime voucherDate, string voucherCode, string serialNo)
        {


            string pvCode = null;
            try
            {
                
                var dt = voucherDate;


                string day = dt.ToString("dd");
                string yy = dt.ToString("yy");
                string month = dt.ToString("MM");


                string year = day + month + yy;

              

              

                if (serialNo != null && serialNo.Length == 1)
                {
                    pvCode = company.Trim() + year + "" + voucherCode + "" + "000" + serialNo;
                }

                if (serialNo != null && serialNo.Length == 2)
                {
                    pvCode = company.Trim() + year + "" + voucherCode + "" + "00" + serialNo;
                }

                if (serialNo != null && serialNo.Length == 3)
                {
                    pvCode = company.Trim() + year + "" + voucherCode + "0" + serialNo;
                }
                if (serialNo != null && serialNo.Length == 4)
                {
                    pvCode = company.Trim() + year + "" + voucherCode + "" + serialNo;
                }

            }
            catch (Exception ex)
            {
                //Alert.Show(ex.Message);
            }

            return pvCode;
        }*/

        //public static int GetMenuId(string controllerName, string actionName)
        //{
        //    var menuId = 0;

        //    try
        //    {
        //        using (var db = new ERP_Entities())
        //        {

        //            if (actionName == "Index" || actionName == "Edit")
        //            {
        //                menuId = db.MenuFunctions.First(r => r.ControllerName == controllerName).Id;

        //            }
        //            else
        //            {
        //                menuId = db.MenuFunctions.First(r => r.ControllerName == controllerName && r.ActionName == actionName).Id;

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        using (var db = new ERP_Entities())
        //        {
        //            menuId = db.MenuFunctions.First(r => r.ControllerName == controllerName).Id;

        //        }


        //    }
        //    return menuId;

        //}

        public static string GetDateFormat_dd_mm_yyyy(string date)
        {

            string formatedDate = "";
            try
            {
                DateTime newDate = DateTime.Parse(date);

                formatedDate = newDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                formatedDate = "";
            }
            return formatedDate;
        }



        public static string GetDateFormat_dd_mm_yyyy(DateTime date)
        {

            string formatedDate = "";
            try
            {

                formatedDate = date.ToString("dd/MM/yyyy");
            }
            catch
            {
                formatedDate = "";
            }
            return formatedDate;
        }
        public static string GetDateFormat_dd_mm_yyyy(DateTime? chequeDate)
        {
            return (chequeDate != null ? chequeDate.Value.ToString("dd/MM/yyyy") : "n/a");
        }


        public static DateTime GetDateFormat_MM_dd_yyy(string dmyFormat)
        {
            DateTime formatedDate;
            try
            {
                formatedDate = Convert.ToDateTime(DateTime.ParseExact(dmyFormat, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            }
            catch (Exception exception)
            {

                formatedDate = Convert.ToDateTime(DateTime.ParseExact(dmyFormat, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));

            }

            return formatedDate;
        }

        //public static UserPrivilege UserPrivilegeIndex(int menuId, int userId)
        //{
        //    var userPrevilege = new UserPrivilege();
        //    using (var db = new ERP_Entities())
        //    {
        //     userPrevilege= db.UserPrivileges.FirstOrDefault(r => r.UserId == userId  &&
        //                                                          r.MenuFunctionId == menuId);
        //    }
        //    return userPrevilege;

        //}


        //public static bool UserPrivilegeAdd(int menuId, int userId)
        //{
        //    var userPrivilege = false;
        //    using (var db = new ERP_Entities())
        //    {
        //      userPrivilege=  db.UserPrivileges.FirstOrDefault(r => r.UserId == userId  && r.MenuFunctionId == menuId).IsAdd;
        //    }
        //    return userPrivilege;

        //}

        //public static bool UserPrivilegeEdit(int menuId, int userId)
        //{
        //    var userEditPrivilige = false;
        //    using (var db = new ERP_Entities())
        //    {

        //        userEditPrivilige = db.UserPrivileges.FirstOrDefault(r => r.UserId == userId && r.MenuFunctionId == menuId).IsEdit;

        //    }
        //    return userEditPrivilige;
        //}


        public static List<ViewModelVoucherDetail> GetVoucherDetailsByMasterId(int vmId)
        {
            try
            {
                List<ViewModelVoucherDetail> voucherDetails;
                using (var db = new ERP_Entities())
                {
                    voucherDetails = (from r in db.AccVoucherDetails
                                      where r.VMasterId == vmId
                                      select new ViewModelVoucherDetail()
                                      {
                                          Id = r.Id,
                                          VMasterId = r.VMasterId,
                                          IsActive = r.IsActive,
                                          VoucherNo = r.VoucherNo,
                                          AC5Id = r.AC5Id,
                                          CostCenterId = r.CostCenterId,
                                          DebitAmount = r.DebitAmount,
                                          CreditAmount = r.CreditAmount,
                                          TransactionType = r.TransactionType
                                      }).ToList();

                }

                return voucherDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static decimal sumDebit(int vmId)
        {
            decimal y;
            using (var db = new ERP_Entities())
            {
                y = db.AccVoucherDetails.Where(r => r.VMasterId == vmId).Select(r => r.DebitAmount).Sum();


            }
            return y;
        }
        public static decimal sumCredit(int vmId)
        {
            decimal x;
            using (var db = new ERP_Entities())
            {
                x = db.AccVoucherDetails.Where(r => r.VMasterId == vmId).Select(r => r.CreditAmount).Sum();


            }
            return x;
        }

        public static CmnMenuPermission MenuPermission(int menuId, int userId)
        {
            CmnMenuPermission userPrivilege = null;
            using (var db = new ERP_Entities())
            {
                userPrivilege = db.CmnMenuPermissions.FirstOrDefault(r => r.UserID == userId && r.MenuID == menuId);
            }
            return userPrivilege;

        }



        public static List<Autocomplete> GetLedgerList(string query, string type)
        {
            List<Autocomplete> results = new List<Autocomplete>();
            try
            {


                switch (type.ToLower())
                {
                    case "ac2":
                        using (var _db = new ERP_Entities())
                        {
                            results = (from p in _db.AccAC2
                                       where p.AC2Name.Contains(query)
                                       orderby p.AC2Name
                                       select p).Take(10).Select(r => new Autocomplete()
                                       {
                                           Id = r.Id,
                                           Name = r.AC2Name
                                       }).ToList();
                        }
                        break;
                    case "ac3":
                        using (var _db = new ERP_Entities())
                        {
                            results = (from p in _db.AccAC3
                                       where p.AC3Name.Contains(query)
                                       orderby p.AC3Name
                                       select p).Take(10).Select(r => new Autocomplete()
                                       {
                                           Id = r.Id,
                                           Name = r.AC3Name
                                       }).ToList();
                        }
                        break;
                    case "ac4":
                        using (var _db = new ERP_Entities())
                        {
                            results = (from p in _db.AccAC4
                                       where p.AC4Name.Contains(query)
                                       orderby p.AC4Name
                                       select p).Take(10).Select(r => new Autocomplete()
                                       {
                                           Id = r.Id,
                                           Name = r.AC4Name
                                       }).ToList();
                        }
                        break;
                    case "ac5":
                        using (var _db = new ERP_Entities())
                        {


                            results = (from p in _db.AccACDetails
                                       where p.ACName.Contains(query)
                                       orderby p.ACName
                                       select p).Take(10).Select(r => new Autocomplete()
                                          {
                                              Id = r.Id,
                                              Name = r.ACName
                                          }).ToList();
                        }
                        break;
                    default:
                        throw new Exception();
                        break;
                }

                //foreach (var r in results)
                //{
                //    // create objects
                //    var person = new Autocomplete
                //    {
                //        Name = string.Format("{0}", r.ACName),
                //        Id = r.Id
                //    };


                //    people.Add(person);
                //}




            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return results;
        }

        #region voucherCode



        public static string GetVoucherSerialByMonth(EmVoucherType type, DateTime voucherDate)
        {
            string id = null;
            try
            {
                switch (type)
                {
                    case EmVoucherType.CashVoucherHeadOffice:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 1 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break; 
                    case EmVoucherType.BankVoucher:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 2 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break; 
                    case EmVoucherType.ReceiptVoucher:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 3 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break;
                    case EmVoucherType.ContraVoucher:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 4 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break;  
                    case EmVoucherType.JournalVoucher:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 5 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break;   
                    case EmVoucherType.CashVoucherFactory:
                        using (var db = new ERP_Entities())
                        {
                            var data =
                                db.AccVoucherMasters.Where(
                                    r =>
                                        r.VoucherTypeId == 6 && r.VoucherDate.Year == voucherDate.Year &&
                                        r.VoucherDate.Month == voucherDate.Month).ToList();
                            id = data.Max(r => Convert.ToInt32(r.SerialNo) + 1).ToString();

                        }
                        break;
                    default:
                        id = "1";
                        break;
                }
            }

            catch (Exception)
            {
                id = "1";
            }
            return id;
        }

        public static string VoucherCodeGenerate(EmVoucherType type, string voucherCode, DateTime voucherDate, int companyId)
        {
            string serialNo = GetVoucherSerialByMonth(type, voucherDate);
            string company = null;
            using (var _db = new ERP_Entities())
            {
                company=(from r in _db.CmnCompanies
                                 where r.CompanyID == companyId
                                 select r.CompanyShortName).SingleOrDefault();
            }
                
            
            string pvCode = null;
            try
            {
                var dt = voucherDate;
                string yy = dt.ToString("yy");
                string month = dt.ToString("MM");
                string date =  month+"-" + yy;

                if (serialNo != null && serialNo.Length == 1)
                {
                    pvCode = company.Trim() + "-" + voucherCode + "-" + date + "-000" + serialNo;
                }

                if (serialNo != null && serialNo.Length == 2)
                {
                    pvCode = company.Trim() + "-" + voucherCode + "-" + date + "-00" + serialNo;
                }

                if (serialNo != null && serialNo.Length == 3)
                {
                    pvCode = company.Trim() + "-" + voucherCode + "-" + date + "-0" + serialNo;
                }
                if (serialNo != null && serialNo.Length == 4)
                {
                    pvCode = company.Trim() + "-" + voucherCode + "-" + date + "-" + serialNo;
                }

            }
            catch (Exception)
            {
            }

            return pvCode;
        }
        #endregion

    }
}