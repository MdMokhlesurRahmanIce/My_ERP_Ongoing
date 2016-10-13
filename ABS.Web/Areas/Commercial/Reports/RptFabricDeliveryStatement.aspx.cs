using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Reports.Sales;
using ABS.Service.Sales.Factories;
using ABS.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;


namespace ABS.Web.Areas.Commercial.Reports
{
    public partial class RptFabricDeliveryStatement : System.Web.UI.Page
    {
        HeadOfficeSalesDeliveryOrderMgt objHDOMgt = new HeadOfficeSalesDeliveryOrderMgt();
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
                    ListItem item = new ListItem("--Select HDONO--", "0");
                    //ddlHDONo.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }
                //LoadHDO(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "" && ddlCompany.SelectedValue != "")
            {
                ReportViewer1.Report = new rptFabricDeliveryStatement();
                (ReportViewer1.Report as rptFabricDeliveryStatement).StartDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptFabricDeliveryStatement).EndDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtEndDate.Text);
                (ReportViewer1.Report as rptFabricDeliveryStatement).CompanyID = Convert.ToInt16(ddlCompany.SelectedValue);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

            }
        }        

        protected void LoadCompany(int lUserID, int companyID)
        {
            List<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(lUserID).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = false }).ToList();
            ddlCompany.DataSource = lstCmnCompany;
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataBind();
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);
            ddlCompany.SelectedValue = lCompanyID.ToString();           
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int companyID = Convert.ToInt16(ddlCompany.SelectedValue);
            if (companyID > 0)
            {
                //LoadHDO(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }
    }
}