using System;
using System.Web.UI;
using ABS.Reports.Accounting;
using ABS.Web.Utility;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class TrailBalanceAllLevel : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
               
            }
        }

    


        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                if (dpStartdate.Value != "" && dpEnddate.Value != "" && (hdnAcCode.Value != "" || chkAll.Checked) && ddlLevel.Value != "")
                {

                    var ledgerValue = hdnAcCode.Value;

                    if (chkAll.Checked)
                    {
                        ledgerValue = "All";
                       
                    }
                    ReportViewer1.Report = new TrialBalanceAllLevel();
                    (ReportViewer1.Report as TrialBalanceAllLevel).pvparam = Convert.ToInt32(Session["CompanyId"].ToString());
                    (ReportViewer1.Report as TrialBalanceAllLevel).pvparam1 = UniqueCode.GetDateFormat_MM_dd_yyy(dpStartdate.Value);
                    (ReportViewer1.Report as TrialBalanceAllLevel).pvparam2 = UniqueCode.GetDateFormat_MM_dd_yyy(dpEnddate.Value);
                    (ReportViewer1.Report as TrialBalanceAllLevel).pvparam3 = ledgerValue;
                    (ReportViewer1.Report as TrialBalanceAllLevel).pvparam4 = ddlLevel.Value;



                    ReportViewer1.RefreshReport();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "BindAccountCode();", true);

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