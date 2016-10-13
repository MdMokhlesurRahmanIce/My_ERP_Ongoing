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

namespace ABS.Web.Areas.Inventory.Reports
{
    public partial class RPTSPRDetail : System.Web.UI.Page
    {
        PIMgt objPIMgt = new PIMgt();
        GRRMgt objGRRMgt = new GRRMgt();

        protected void Page_Load(object sender, EventArgs e)
        {
            int lUserID = Convert.ToInt16(Session["UserID"]);
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

            if (!IsPostBack)
            {
                if (!IsPostBack && lUserID > 0 && lCompanyID > 0)
                {
                    LoadCompany(lUserID, lCompanyID);
                    // LoadPI(lUserID, lCompanyID);
                    // ListItem item = new ListItem("--Select GRR NO--", "-1");
                    // ddlGRRNO.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

               // LoadGRR(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "" && ddlCompany.SelectedValue != "")
            {
                ReportViewer1.Report = new rptSPRDetail();
                (ReportViewer1.Report as rptSPRDetail).startDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptSPRDetail).endDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtEndDate.Text);
                (ReportViewer1.Report as rptSPRDetail).createdBy = Convert.ToInt16(Session["UserID"]);
                (ReportViewer1.Report as rptSPRDetail).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                (ReportViewer1.Report as rptSPRDetail).RequisitionTypeID  = 8;
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