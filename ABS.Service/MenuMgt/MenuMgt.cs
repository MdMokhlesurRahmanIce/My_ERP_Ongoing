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
    public class MenuMgt : iMenuMgt
    {
        private ERP_Entities dbContext = null;
        private iGenericFactory<vmNotification> GenericFactoryFor_vmNotification = null;

        public MenuMgt()
        {
            //dbContext = new ERP_Entities();
            GenericFactoryFor_vmNotification = new vmNotification_GF();
        }
        #region Get Side Menu

        //Without Permisison 
        //public dynamic GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    using (ERP_Entities dbContext = new ERP_Entities())
        //    {
        //        var db = (
        //            from tb in dbContext.CmnMenus
        //            where tb.ModuleID == ModuleID &&
        //            (tb.ParentID == 0 || tb.ParentID == null)
        //            select new
        //            {
        //                MenuID = tb.MenuID,
        //                ParentID = tb.ParentID ?? 0,
        //                MenuName = tb.MenuName,
        //                MenuPath = tb.MenuPath,
        //                ReportName = tb.ReportName,
        //                MenuIconCss = tb.MenuIconCss,
        //                ChildMenues =
        //              from master in dbContext.CmnMenus
        //              join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //              from lg in leftGroup.DefaultIfEmpty()
        //              where lg.MenuName != null && tb.MenuID == lg.ParentID
        //              select new
        //              {
        //                  lg.MenuID,
        //                  lg.ParentID,
        //                  lg.MenuName,
        //                  lg.MenuPath,
        //                  lg.MenuIconCss
        //              }
        //            });
        //        return db;
        //    }
        //}

        public List<vmBreadCrums> GetBreadCrums(int? MenuID)
        {
            List<vmBreadCrums> list = new List<vmBreadCrums>();

            try
            {
                using (ERP_Entities dbContext = new ERP_Entities())
                {

                    var obj = (
                            from menu in dbContext.CmnMenus
                            join mod in dbContext.CmnModules on menu.ModuleID equals mod.ModuleID into ModGrouo
                            from modG in ModGrouo.DefaultIfEmpty()
                            join pMenu in dbContext.CmnMenus on menu.ParentID equals pMenu.MenuID into parentGroup
                            from pm in parentGroup.DefaultIfEmpty()
                            where menu.MenuID == MenuID
                            select new vmCmnMenu
                            {
                                MenuID = menu.MenuID,
                                MenuName = menu.MenuName,
                                MenuIconCss = menu.MenuIconCss,
                                ParentID = menu.ParentID ?? 0,
                                ParentMenuIconCss = pm.MenuIconCss,
                                ParentMenuName = pm.MenuName,
                                ModuleID = menu.ModuleID,
                                ModuleName = modG.ModuleName,
                                MenuPath = menu.MenuPath,
                                ParentMenuPath = pm.MenuPath
                            }).FirstOrDefault();


                    list.Add(new vmBreadCrums { Name = obj.ModuleName.ToLower(), Icon = obj.ParentMenuIconCss, Path = obj.ParentMenuPath });
                    list.Add(new vmBreadCrums { Name = obj.ParentMenuName.ToLower(), Icon = "", Path = obj.ParentMenuPath });

                    String[] menuName;
                    var customMenuName = obj.MenuName.ToString();
                    if (obj.MenuName.Contains('('))
                    {
                        menuName = obj.MenuName.Split('(');
                        customMenuName = menuName[0].ToString().ToLower() + ("(" + menuName[1].ToString().ToUpper());
                    }


                    list.Add(new vmBreadCrums { Name = customMenuName, Icon = "", Path = obj.MenuPath });

                }
            }
            catch (Exception)
            {
                list = new List<vmBreadCrums>();
                list.Add(new vmBreadCrums { Name = "Na", Icon = "icon_Home", Path = "" });

            }
            return list;
        }
        public int CheckAuthorization(Int64 companyID,Int64 userID,string path)
        {

            return 0;
        }

        //public dynamic GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    object[] obj = null;
        //    try
        //    {
        //        using (ERP_Entities dbContext = new ERP_Entities())
        //        {
        //            obj = (
        //               from tb in dbContext.CmnMenus
        //               where tb.ModuleID == ModuleID &&
        //               (tb.ParentID == 0 || tb.ParentID == null) && tb.IsDeleted == false
        //               select new
        //               {
        //                   MenuID = tb.MenuID,
        //                   ParentID = tb.ParentID ?? 0,
        //                   MenuName = tb.MenuName.ToLower(),
        //                   MenuPath = tb.MenuPath,
        //                   ReportName = tb.ReportName ?? "c",
        //                   MenuIconCss = tb.MenuIconCss,
        //                   ChildMenues =
        //                 from master in dbContext.CmnMenus
        //                 join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //                 from lg in leftGroup.DefaultIfEmpty()
        //                 where lg.MenuName != null && tb.MenuID == lg.ParentID
        //                 join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
        //                 from ltg in leftTranGroup.DefaultIfEmpty()
        //                 join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
        //                 from blj in bgroup.DefaultIfEmpty()
        //                 where lg.IsDeleted == false
        //                 orderby lg.Sequence ascending
        //                 select new
        //                 {
        //                     lg.MenuID,
        //                     lg.ParentID,
        //                     MenuName = lg.MenuName,
        //                     lg.MenuPath,
        //                     lg.MenuIconCss,
        //                     TransactionTypeID = (int?)ltg.TransactionTypeID,
        //                     TransactionTypeName = ltg.TransactionTypeName,
        //                     DepartmentID = blj.DepartmentID
        //                 }


        //               }).ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //    return obj;
        //}

        #region Top Menu With Permission 
        public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            ERP_Entities dbContext = new ERP_Entities();
            var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;
            var topMenu =
                (from a in dbContext.CmnModules
                 join b in dbContext.CmnModulePermissions on a.ModuleID equals b.ModuleID
                 join c in dbContext.CmnMenus on a.ModuleID equals c.ModuleID
                 join d in dbContext.CmnMenuPermissions on c.MenuID equals d.MenuID
                 where b.CompanyID == companyID && d.UserID == loggedUser
                 && a.IsDeleted == false
                 && b.IsDeleted == false
                 && c.IsDeleted == false
                 && d.IsDeleted == false
                 && d.EnableView == true
                 select new vmCmnModule
                 {
                     ModuleID = a.ModuleID,
                     ModuleName = a.ModuleName,
                     ModulePath = a.ModulePath,
                     ImageURL = a.ImageURL
                 }).Distinct().OrderBy(x=>x.ModuleID).ToList();

            return topMenu;
        }

        public object GetMenuPermission(vmApplicationTokenModel model)
        {
            int loggedUser = model.loggedUserID, companyID = model.loggedCompanyID;
            String moduleName = string.Empty;
            string[] parts = model.MenuPath.Split(new char[] { '/' });
            if (parts.Length > 2)
            {
                moduleName = parts[1];
            }
            ERP_Entities dbContext = new ERP_Entities();
            var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;

            var dba = (
                from tb in dbContext.CmnMenus
                join permission in dbContext.CmnMenuPermissions on tb.MenuID equals permission.MenuID
             //   where tb.CompanyID == companyID &&
                where permission.CompanyID == companyID &&
                (tb.ParentID == 0 || tb.ParentID == null)
                && (permission.UserID == loggedUser)
                && tb.MenuPath.Contains(moduleName)
                select new
                {
                    MenuID = tb.MenuID,
                    ParentID = tb.ParentID ?? 0,
                    Sequencea = tb.Sequence,
                    MenuName = tb.MenuName.ToLower(),
                    MenuPath = tb.MenuPath,
                    ReportName = tb.ReportName ?? "c",
                    MenuIconCss = tb.MenuIconCss,
                    tb.ModuleID,
                    permission.EnableView,
                    permission.EnableInsert,
                    permission.EnableUpdate,
                    permission.EnableDelete,
                    ChildMenues = (
                  from master in dbContext.CmnMenus
                  join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
                  from lg in leftGroup.DefaultIfEmpty()
                  where lg.MenuName != null && tb.MenuID == lg.ParentID
                  join permissions in dbContext.CmnMenuPermissions on lg.MenuID equals permissions.MenuID
                  join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
                  from ltg in leftTranGroup.DefaultIfEmpty()
                  join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
                  from blj in bgroup.DefaultIfEmpty()
                  where lg.IsDeleted == false
                  where lg.MenuName != null && tb.MenuID == lg.ParentID && permissions.EnableView == true
                  && (permissions.UserID == loggedUser || permissions.UserGroupID == usergroupID)
                  && permissions.CompanyID == companyID
                     //&& lg.MenuPath.Contains(model.MenuPath)
                     && lg.MenuPath==model.MenuPath
                  select new
                  {
                      lg.MenuID,
                      lg.ParentID,
                      lg.MenuName,
                      lg.MenuPath,
                      lg.MenuIconCss,
                      lg.ModuleID,
                      lg.Sequence,
                      permissions.EnableView,
                      permissions.EnableInsert,
                      permissions.EnableUpdate,
                      permissions.EnableDelete,
                      TransactionTypeID = (int?)ltg.TransactionTypeID,
                      TransactionTypeName = ltg.TransactionTypeName,
                      DepartmentID = blj.DepartmentID
                  }).Distinct().OrderBy(x => x.Sequence)
                }).ToList().OrderBy(x => x.Sequencea); 

            return dba;
        }
        #endregion  Menu With Permission 
        #region Menu With Permission 
        public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            ERP_Entities dbContext = new ERP_Entities();
            var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;

            var db = (
                from tb in dbContext.CmnMenus
                join permission in dbContext.CmnMenuPermissions on tb.MenuID equals permission.MenuID
                where tb.ModuleID == ModuleID && permission.CompanyID == companyID &&
                (tb.ParentID == 0 || tb.ParentID == null)
                && (permission.UserID == loggedUser)
                select new
                {
                    MenuID = tb.MenuID,
                    Sequencea = tb.Sequence,
                    ParentID = tb.ParentID ?? 0,
                    MenuName = tb.MenuName.ToLower(),
                    MenuPath = tb.MenuPath,
                    ReportName = tb.ReportName ?? "c",
                    MenuIconCss = tb.MenuIconCss,
                    permission.EnableView,
                    permission.EnableInsert,
                    permission.EnableUpdate,
                    permission.EnableDelete,
                    ChildMenues = (
                  from master in dbContext.CmnMenus
                  join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
                  from lg in leftGroup.DefaultIfEmpty()
                  where lg.MenuName != null && tb.MenuID == lg.ParentID
                  join permissions in dbContext.CmnMenuPermissions on lg.MenuID equals permissions.MenuID
                  join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
                  from ltg in leftTranGroup.DefaultIfEmpty()
                  join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
                  from blj in bgroup.DefaultIfEmpty()
                  where lg.IsDeleted == false
                  where lg.MenuName != null && tb.MenuID == lg.ParentID && permissions.EnableView==true
                  && permissions.CompanyID == companyID
                  && (permissions.UserID == loggedUser || permissions.UserGroupID == usergroupID)
                  select new 
                  {
                      lg.MenuID,
                      lg.ParentID,
                      lg.MenuName,
                      lg.MenuPath,
                      lg.MenuIconCss,
                      lg.Sequence,
                      permissions.EnableView,
                      permissions.EnableInsert,
                      permissions.EnableUpdate,
                      permissions.EnableDelete, 
                      TransactionTypeID = (int?)ltg.TransactionTypeID,
                      TransactionTypeName = ltg.TransactionTypeName,
                      DepartmentID = blj.DepartmentID
                  }).Distinct().OrderBy(x => x.Sequence)
                }).ToList().OrderBy(x=>x.Sequencea);
          //  db = null;
            return db;
        }
        
        //public object GetMenuPermission(vmApplicationTokenModel model)
        //{
        //    int loggedUser= model.loggedUserID,companyID= model.loggedCompanyID;
        //    String moduleName = string.Empty;
        //    string[] parts = model.MenuPath.Split(new char[] { '/' });
        //    if(parts.Length>2)
        //    {
        //        moduleName = parts[1];
        //    }
        //    ERP_Entities dbContext = new ERP_Entities();
        //    var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;

        //    var db = (
        //        from tb in dbContext.CmnMenus
        //        join permission in dbContext.CmnMenuPermissions on tb.MenuID equals permission.MenuID
        //        where tb.CompanyID == companyID &&
        //        (tb.ParentID == 0 || tb.ParentID == null)
        //        && (permission.UserID == loggedUser)
        //        && tb.MenuPath.Contains(moduleName)
        //        select new
        //        {
        //            MenuID = tb.MenuID,
        //            ParentID = tb.ParentID ?? 0,
        //            MenuName = tb.MenuName.ToLower(),
        //            MenuPath = tb.MenuPath,
        //            ReportName = tb.ReportName ?? "c",
        //            MenuIconCss = tb.MenuIconCss,
        //            tb.ModuleID,
        //            permission.EnableView,
        //            permission.EnableInsert,
        //            permission.EnableUpdate,
        //            permission.EnableDelete,
        //            ChildMenues = (
        //          from master in dbContext.CmnMenus
        //          join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //          from lg in leftGroup.DefaultIfEmpty()
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID
        //          join permissions in dbContext.CmnMenuPermissions on lg.MenuID equals permissions.MenuID
        //          join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
        //          from ltg in leftTranGroup.DefaultIfEmpty()
        //          join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
        //          from blj in bgroup.DefaultIfEmpty()
        //          where lg.IsDeleted == false
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID && permissions.EnableView == true
        //          && (permissions.UserID == loggedUser || permissions.UserGroupID == usergroupID)
        //           && lg.MenuPath.Contains(model.MenuPath)
        //          select new
        //          {
        //              lg.MenuID,
        //              lg.ParentID,
        //              lg.MenuName,
        //              lg.MenuPath,
        //              lg.MenuIconCss,
        //              lg.ModuleID,
        //              lg.Sequence,
        //              permissions.EnableView,
        //              permissions.EnableInsert,
        //              permissions.EnableUpdate,
        //              permissions.EnableDelete,
        //              TransactionTypeID = (int?)ltg.TransactionTypeID,
        //              TransactionTypeName = ltg.TransactionTypeName,
        //              DepartmentID = blj.DepartmentID
        //          }).Distinct().OrderBy(x => x.Sequence)
        //        });

        //    return db;
        //}
        #endregion  Menu With Permission 

        #endregion  Get Side Menu

        #region notification
        //This Method Will Be Banned After Dynamic Setup Active
        public IEnumerable<vmNotification> GetNotificationInfo(int? companyID, int? loggedUser, int? ModuleID)
        {

            IEnumerable<vmNotification> listNotification = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                if (ModuleID == 1)
                {
                    ht.Add("pageNumber", 1);
                    ht.Add("pageSize", 10);
                    ht.Add("IsPaging", 0);
                    ht.Add("StatusID", 1);
                    spQuery = "[Get_vmNotification]";
                }
                else if (ModuleID == 2)
                {
                    ht.Add("pageNumber", 1);
                    ht.Add("pageSize", 10);
                    ht.Add("IsPaging", 0);
                    ht.Add("StatusID", 2);
                    spQuery = "[Get_vmNotification]";
                }
                else if (ModuleID == 3)
                {
                    ht.Add("pageNumber", 1);
                    ht.Add("pageSize", 10);
                    ht.Add("IsPaging", 0);
                    ht.Add("StatusID", 3);
                    spQuery = "[Get_vmNotification]";
                }
                else if (ModuleID == 4)
                {
                    ht.Add("pageNumber", 1);
                    ht.Add("pageSize", 10);
                    ht.Add("IsPaging", 0);
                    ht.Add("StatusID", 4);
                    spQuery = "[Get_vmNotification]";
                }
                else
                {
                    ht.Add("pageNumber", 1);
                    ht.Add("pageSize", 10);
                    ht.Add("IsPaging", 0);
                    ht.Add("StatusID", 1);
                    spQuery = "[Get_vmNotification]";

                }
                listNotification = GenericFactoryFor_vmNotification.ExecuteQuery(spQuery, ht);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }
            return listNotification;
        }
        #endregion Notifaction 

        #region notification
        public List<NotificationEntity> GetNotificationInfoes(int? companyID, int? loggedUser, int? userID)
        {
            List<NotificationEntity> list = new List<NotificationEntity>();
            try
            {
                //list = new WorkFLowMgt().GetNotificationInfoByUser((int)userID);
                list = new WorkFLowMgt().GetNotificationInfoesByUser((int)userID);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                ex.ToString();
            }
            return list;
        }
        #endregion Notifaction 


    }

}
