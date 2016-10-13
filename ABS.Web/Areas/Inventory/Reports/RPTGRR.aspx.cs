using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Reports.Inventory;
using ABS.Service.Inventory.Factories;
using ABS.Service.Sales.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.Inventory.Reports
{
    public partial class RPTGRR : System.Web.UI.Page
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
                    ListItem item = new ListItem("--Select GRR NO--", "-1");
                    ddlGRRNO.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

                LoadGRR(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlGRRNO.SelectedValue != "0")
            {
                ReportViewer1.Report = new rptGRR();
                (ReportViewer1.Report as rptGRR).pramGRRNo = ddlGRRNO.SelectedValue.ToString();
                (ReportViewer1.Report as rptGRR).createdBy = Convert.ToInt16(Session["UserID"]);
                (ReportViewer1.Report as rptGRR).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

                //ReportViewer1.Report = new rpt_HDOMasterDetail();

                ////(ReportViewer1.Report as rpt_HDOMasterDetail).pramHDONo = ddlHDONo.SelectedValue.ToString();// "PI-00000155-Revise-2";
                //ReportViewer1.RefreshReport();
            }
        }
        protected void LoadGRR(int lUserID, int companyID) 
        {
            List<vmChallan> lstGRRMaster = objGRRMgt.GetGrrList()
                                             .Select(m => new vmChallan
                                             {
                                                 GrrID = m.GrrID,
                                                 GrrNo = m.GrrNo,
                                                 ManualGRRNoRpt = m.ManualGRRNoRpt,
                                                 CreateBy = m.CreateBy, 
                                                 CompanyID = m.CompanyID
                                             })
                                             .Where(m => m.CompanyID == companyID).ToList();

            ddlGRRNO.DataSource = lstGRRMaster;
            ddlGRRNO.DataValueField = "GrrNo";
            ddlGRRNO.DataTextField = "ManualGRRNoRpt";
            ddlGRRNO.DataBind();

            ListItem item = new ListItem("--Select GRR NO--", "-1");
            ddlGRRNO.Items.Insert(0, item);
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
                LoadGRR(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }       
    }
}