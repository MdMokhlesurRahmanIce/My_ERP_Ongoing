using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Reports.Inventory;
using ABS.Service.Inventory.Factories;
using ABS.Service.Sales.Factories;
using ABS.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.Purchase.Reports
{
    public partial class RPTCSDetail : System.Web.UI.Page
    {
        PIMgt objPIMgt = new PIMgt();
        protected void Page_Load(object sender, EventArgs e)
        {
            int lUserID = Convert.ToInt16(Session["UserID"]);
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

            if (!IsPostBack)
            {
                if (!IsPostBack && lUserID > 0 && lCompanyID > 0)
                {
                    LoadCompany(lUserID, lCompanyID);                   
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "" && ddlCompany.SelectedValue != "")
            {
                ReportViewer1.Report = new rptCSDetail();
                (ReportViewer1.Report as rptCSDetail).startDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptCSDetail).endDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtEndDate.Text);
                (ReportViewer1.Report as rptCSDetail).CSId = 0;
                (ReportViewer1.Report as rptCSDetail).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);
                 
            }
        } 

        protected void LoadCompany(int lUserID, int companyID)
        {
            List<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(lUserID).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = m.IsDeleted }).ToList();
            ddlCompany.DataSource = lstCmnCompany;
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataTextField = "CompanyName";

            ddlCompany.DataBind();

            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);
            ddlCompany.SelectedValue = lCompanyID.ToString();  
        } 
    }
}