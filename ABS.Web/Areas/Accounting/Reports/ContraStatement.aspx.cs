using ABS.Models;
using ABS.Reports.Accounting;
using ABS.Web.Utility;
using System;
using System.Linq;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class ContraStatement : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login");
            }
            if (!IsPostBack)
            {
                BindCompany();
                //BindCostCenter();
                BindAccountCode();
            }

        }

        private void BindCompany()
        {
            var userId = Convert.ToInt32(Session["UserId"].ToString());
            using (var db = new ERP_Entities())
            {
                var source = (from p in db.CmnCompanies
                              join a in db.CmnUserWiseCompanies
                               on p.CompanyID equals a.CompanyID
                              where a.UserID == userId && p.IsDeleted == false
                              select new
                              {
                                  Text = p.CompanyName,
                                  value = p.CompanyID
                              }).Distinct().ToList();



                ddlCompanyName.DataSource = source;
                ddlCompanyName.DataValueField = "value";
                ddlCompanyName.DataTextField = "Text";
                ddlCompanyName.DataBind();
                ddlCompanyName.Items.Insert(0, "");
                ddlCompanyName.Items[0].Value = "";
            }


        }


        private void BindAccountCode()
        {
            using (var db = new ERP_Entities())
            {
                ddlAccountCode.DataSource = db.AccACDetails.ToList();
                ddlAccountCode.DataValueField = "Id";
                ddlAccountCode.DataTextField = "ACName";
                ddlAccountCode.DataBind();

                ddlAccountCode.Items.Insert(0, "");
                ddlAccountCode.Items[0].Value = "";
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (dpStartdate.Value != "" && dpEnddate.Value != "" && ddlCompanyName.SelectedValue != "")
                {


                    ReportViewer1.Report = new ContraStatReport();
                    (ReportViewer1.Report as ContraStatReport).pvparam = UniqueCode.GetDateFormat_MM_dd_yyy(dpStartdate.Value);
                    (ReportViewer1.Report as ContraStatReport).pvparam1 = UniqueCode.GetDateFormat_MM_dd_yyy(dpEnddate.Value);

                    if (ddlAccountCode.Value == "" || ddlAccountCode.Value == String.Empty)
                    {
                        (ReportViewer1.Report as ContraStatReport).pvparam2 = null;
                    }
                    else
                    {
                        (ReportViewer1.Report as ContraStatReport).pvparam2 = ddlAccountCode.Value;
                    }

                    (ReportViewer1.Report as ContraStatReport).pvparam4 = ddlCompanyName.SelectedValue;

                    ReportViewer1.RefreshReport();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}