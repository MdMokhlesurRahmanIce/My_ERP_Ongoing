using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Reports.Commercial;
using ABS.Service.Commercial.Factories;
using ABS.Service.Sales.Factories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace ABS.Web.Areas.Commercial.Reports
{
    public partial class RptFactoryDeliveryOrder : System.Web.UI.Page
    {
        FactorySalesDeliveryOrderMgt objFDOMgt = new FactorySalesDeliveryOrderMgt();
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
                LoadFDONo(lUserID, lCompanyID);
            }
        }
        protected void LoadFDONo(int lUserID, int lCompanyID)
        {
            IEnumerable<SalFDOMaster> lstFDONo = objFDOMgt.GetFDONo(lUserID, Convert.ToInt16(ddlCompany.SelectedValue));
            ddlFDONo.DataSource = lstFDONo;
            ddlFDONo.DataValueField = "FDOMasterID";
            ddlFDONo.DataTextField = "FDONo";
            ddlFDONo.DataBind();

            ListItem item = new ListItem("--Select FDO No--", "0");
            ddlFDONo.Items.Insert(0, item);
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
                LoadFDONo(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlFDONo.SelectedValue != "0")
            {
                ReportViewer1.Report = new rptFactoryDeliveryOrder();
                (ReportViewer1.Report as rptFactoryDeliveryOrder).pramFDOID = Convert.ToInt16(ddlFDONo.SelectedItem.Value);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

                ReportViewer1.Report = new rptFactoryDeliveryOrder();

                //(ReportViewer1.Report as rpt_HDOMasterDetail).pramHDONo = ddlHDONo.SelectedValue.ToString();// "PI-00000155-Revise-2";
                ReportViewer1.RefreshReport();
            }
        }
    }
}
