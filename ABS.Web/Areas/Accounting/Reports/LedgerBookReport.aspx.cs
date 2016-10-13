using ABS.Models;
using ABS.Reports.Accounting;
using ABS.Web.Utility;
using System;
using System.Linq;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class LedgerBookReport : System.Web.UI.Page
    {
        private string voucherNo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                BindAccountCode();
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
                if (dpStartdate.Value != "" && dpEnddate.Value != "" && ddlAccountCode.Value != "")
                {


                    ReportViewer1.Report = new LedgerBook();

                    (ReportViewer1.Report as LedgerBook).pvparam = UniqueCode.GetDateFormat_MM_dd_yyy(dpStartdate.Value);
                    (ReportViewer1.Report as LedgerBook).pvparam1 = UniqueCode.GetDateFormat_MM_dd_yyy(dpEnddate.Value);
                    (ReportViewer1.Report as LedgerBook).pvparam2 = ddlAccountCode.Value;
                    (ReportViewer1.Report as LedgerBook).companyId = Convert.ToInt32(Session["CompanyID"].ToString());


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