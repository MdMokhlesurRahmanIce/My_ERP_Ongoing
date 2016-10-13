using ABS.Models;
using ABS.Reports.Accounting;
using System;
using System.Linq;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class BankPayment : System.Web.UI.Page
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


        private void BindPV(int companyId)
        {
            try
            {
                using (var db = new ERP_Entities())
                {
                    var source = db.AccVoucherMasters.Where(r => r.VoucherTypeId == 2 && r.CompanyId == companyId).ToList();
                    cbPVNO.DataSource = source;
                    cbPVNO.DataValueField = "VoucherNo";
                    cbPVNO.DataTextField = "VoucherNo";
                    cbPVNO.DataBind();
                    cbPVNO.Items.Insert(0, "");
                    cbPVNO.Items[0].Value = "";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            //var companyId = Session["CompanyId"].ToString();


        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                if (cbPVNO.SelectedValue != "")
                {
                    ReportViewer1.Report = new BankPaymentVoucherNew();
                    (ReportViewer1.Report as BankPaymentVoucherNew).pvparam = cbPVNO.SelectedValue;
                    (ReportViewer1.Report as BankPaymentVoucherNew).pvparam1 = ddlCompanyName.SelectedValue;

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


        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyName.SelectedValue != "")
            {
                var companyId = Convert.ToInt32(ddlCompanyName.SelectedValue.ToString());

                BindPV(companyId);

            }
            ReportViewer1.Report = new BankPaymentVoucherNew();
            ReportViewer1.RefreshReport();
        }
    }
}