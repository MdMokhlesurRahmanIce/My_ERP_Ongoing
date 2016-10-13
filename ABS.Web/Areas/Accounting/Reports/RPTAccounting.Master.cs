using System;
using System.Web;
using System.Web.Security;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class RPTAccounting : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    hUserID.Text = HttpContext.Current.Session["UserID"].ToString();
                    hCompanyID.Text = HttpContext.Current.Session["CompanyID"].ToString();
                    UserName.Text = HttpContext.Current.Session["UserFirstName"].ToString();
                }
                else
                {
                    Response.Redirect("~/Account/Login");
                }
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Account/Login");
        }
    }
}