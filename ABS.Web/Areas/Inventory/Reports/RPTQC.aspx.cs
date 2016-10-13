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
    public partial class RPTQC : System.Web.UI.Page 
    {
        PIMgt objPIMgt = new PIMgt();
        QCMgt objQCMgt = new QCMgt(); 

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
                    ListItem item = new ListItem("--Select QC NO--", "-1");
                    ddlQCNO.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

                LoadQC(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlQCNO.SelectedValue != "0")
            {
                ReportViewer1.Report = new rptQC();
                (ReportViewer1.Report as rptQC).pramQCNo = ddlQCNO.SelectedValue.ToString();
                (ReportViewer1.Report as rptQC).createdBy = Convert.ToInt16(Session["UserID"]);
                (ReportViewer1.Report as rptQC).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
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
        protected void LoadQC(int lUserID, int companyID) 
        {
            List<vmChallan> lstQCMaster = objQCMgt.GetQCList()
                                             .Select(m => new vmChallan
                                             {
                                                 MrrQcID = m.MrrQcID,
                                                 MrrQcNo = m.MrrQcNo,
                                                 ManualQCNoRpt = m.ManualQCNoRpt,
                                                 CreateBy = m.CreateBy,
                                                 CompanyID = m.CompanyID
                                             })
                                             .Where(m => m.CompanyID == companyID).ToList();

            ddlQCNO.DataSource = lstQCMaster.OrderByDescending(s=>s.MrrQcID);
            ddlQCNO.DataValueField = "MrrQcNo";
            ddlQCNO.DataTextField = "ManualQCNoRpt";
            ddlQCNO.DataBind();

            ListItem item = new ListItem("--Select QC NO--", "-1");
            ddlQCNO.Items.Insert(0, item);
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
                LoadQC(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }       
    }
}