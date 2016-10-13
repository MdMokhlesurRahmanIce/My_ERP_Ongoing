using ABS.Models;
using ABS.Models.ViewModel.TokenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.UserFactory
{
   public class UserAuthentications
    {
        public ERP_Entities _ctx = null;

        public loggedUserModel MemberAuthentication(loginUserModel model)
        {
            loggedUserModel UserAuth = null;

            if (model != null)
            {
                using (_ctx = new ERP_Entities())
                {
                    //UserAuth = (
                    //    from u in _ctx.UserAuthentications
                    //    join r in _ctx.UserRoles on u.RoleID equals r.RoleID
                    //    where u.LoginID == model.LoginId && u.Password == model.Password && u.StatusID == 1
                    //    select new
                    //    {
                    //        UserId = u.UserID,
                    //        UserName = u.UserName,
                    //        Role = r.RoleName,
                    //        Status = (int)u.StatusID
                    //    }).Select(x => new loggedUserModel
                    //    {
                    //        UserId = x.UserId,
                    //        UserName = x.UserName,
                    //        Role = x.Role,
                    //        Status = (int)x.Status

                    //    }).FirstOrDefault();

                    UserAuth = (
                        from u in _ctx.CmnUserAuthentications
                       
                        where u.LoginID == model.LoginId && u.Password == model.Password && u.StatusID == 1
                        select new
                        {
                            UserId = u.UserID,
                            UserName ="DE",
                            Role = "Role",
                            Status = (int)u.StatusID
                        }).Select(x => new loggedUserModel
                        {
                            UserId = x.UserId,
                            UserName = x.UserName,
                            Role = x.Role,
                            Status = (int)x.Status

                        }).FirstOrDefault();


                }
            }
            return UserAuth;
        }
    }
}
