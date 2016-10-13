using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Purchase;
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
    public partial class RPTCS : System.Web.UI.Page
    {
        PIMgt objPIMgt = new PIMgt();
        ComparativeStatementMgt objCSMgt = new ComparativeStatementMgt(); 

        protected void Page_Load(object sender, EventArgs e)
        {
            int lUserID = Convert.ToInt16(Session["UserID"]);
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

            if (!IsPostBack)
            {
                if (!IsPostBack && lUserID > 0 && lCompanyID > 0)
                {
                    LoadCompany(lUserID, lCompanyID);                   
                    ListItem item = new ListItem("--Select CS No--", "-1");
                    ddlCSNo.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

                LoadCS(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlCSNo.SelectedValue != "0")
            {
                ReportViewer1.Report = new rptCS();
                (ReportViewer1.Report as rptCS).CSId = Convert.ToInt16(ddlCSNo.SelectedValue.ToString());
                (ReportViewer1.Report as rptCS).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);
               
            }
        }
        protected void LoadCS(int lUserID, int companyID) 
        {
            List<vmComparativeStatement> lstCSMaster = objCSMgt.GetCSList()
                                             .Select(m => new vmComparativeStatement
                                             {
                                                 CSID = m.CSID,
                                                 CSNo = m.CSNo,
                                                 CompanyID = m.CompanyID
                                             })
                                             .Where(m => m.CompanyID == companyID).ToList();

            ddlCSNo.DataSource = lstCSMaster;
            ddlCSNo.DataValueField = "CSID";
            ddlCSNo.DataTextField = "CSNo";
            ddlCSNo.DataBind();

            ListItem item = new ListItem("--Select CS No--", "-1");
            ddlCSNo.Items.Insert(0, item);
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
            //ListItem item = new ListItem("--Select Company--", "-1");
            //ddlCompany.Items.Insert(0, item);

        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int companyID = Convert.ToInt16(ddlCompany.SelectedValue);
            if (companyID > 0)
            {
                LoadCS(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }       
    }
}