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
    public partial class RPTSR : System.Web.UI.Page
    {
        PIMgt objPIMgt = new PIMgt();
        SPRMgt objGRRMgt = new SPRMgt(); 

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
                    ddlSRNO.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

                LoadSPR(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlSRNO.SelectedValue != "0")
            {
                ReportViewer1.Report = new rptSR();
                (ReportViewer1.Report as rptSR).pramSRNo = ddlSRNO.SelectedValue.ToString();
                (ReportViewer1.Report as rptSR).createdBy = Convert.ToInt16(Session["UserID"]);
                (ReportViewer1.Report as rptSR).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
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
        protected void LoadSPR(int lUserID, int companyID) 
        {
            List<vmRequisition> lstSPRMaster = objGRRMgt.GetSRList()
                                             .Select(m => new vmRequisition
                                             {
                                                 RequisitionID = m.RequisitionID,
                                                 RequisitionNo = m.RequisitionNo,                                                 
                                                 CreateBy = m.CreateBy, 
                                                 CompanyID = m.CompanyID
                                             })
                                             .Where(m => m.CompanyID == companyID).ToList();

            ddlSRNO.DataSource = lstSPRMaster.OrderByDescending(s=>s.RequisitionID);
            ddlSRNO.DataValueField = "RequisitionNo";
            ddlSRNO.DataTextField = "RequisitionNo";
            ddlSRNO.DataBind();

            ListItem item = new ListItem("--Select SR NO--", "-1");
            ddlSRNO.Items.Insert(0, item);
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
                LoadSPR(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }       
    }
}