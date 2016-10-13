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


namespace ABS.Web.Areas.Sales.Reports
{
    public partial class RPTHDOSummary : System.Web.UI.Page
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
            //if (ddlHDONo.SelectedValue != "0")
            //{
            //    ReportViewer1.Report = new rpt_HDOMasterDetail();
            //    (ReportViewer1.Report as rpt_HDOMasterDetail).pramHDONo = ddlHDONo.SelectedValue.ToString();// "PI-00000155-Revise-2";
            //    ReportViewer1.RefreshReport();
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

            //    ReportViewer1.Report = new rpt_HDOMasterDetail();

            //    //(ReportViewer1.Report as rpt_HDOMasterDetail).pramHDONo = ddlHDONo.SelectedValue.ToString();// "PI-00000155-Revise-2";
            //    ReportViewer1.RefreshReport();
            //}
            if (txtStartDate.Text != "" && txtEndDate.Text != "" && ddlCompany.SelectedValue != "")
            {
                ReportViewer1.Report = new rptHDOSummary();
                (ReportViewer1.Report as rptHDOSummary).StartDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptHDOSummary).EndDate = UniqueCode.GetDateFormat_MM_dd_yyy(txtEndDate.Text);
                (ReportViewer1.Report as rptHDOSummary).CreateBy = Convert.ToInt16(Session["UserID"]);
                (ReportViewer1.Report as rptHDOSummary).CompanyID = Convert.ToInt16(ddlCompany.SelectedValue);
                ReportViewer1.RefreshReport();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

            }
        }
        //protected void LoadHDO(int lUserID, int lCompanyID)
        //{
        //    IEnumerable<SalHDOMaster> lstSalHDOMaster = objHDOMgt.GetHDOMaster(lUserID, lCompanyID).ToList()

        //        .Select(m => new SalHDOMaster
        //        {
        //            HDOID = m.HDOID,
        //            HDONo = m.HDONo,
        //            CompanyID = m.CompanyID
        //        }).ToList();


        //    //whichCompanies = _ctxCmn.CmnUserWiseCompanies.Where(m => m.UserID == objcmnParam.loggeduser && m.IsDeleted == false).ToList().

        //    ddlHDONo.DataSource = lstSalHDOMaster;
        //    ddlHDONo.DataValueField = "HDOID";
        //    ddlHDONo.DataTextField = "HDONo";
        //    ddlHDONo.DataBind();

        //    ListItem item = new ListItem("--Select HDONO--", "0");
        //    ddlHDONo.Items.Insert(0, item);
        //}

        protected void LoadCompany(int lUserID, int companyID)
        {
            //IEnumerable<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(1, 1, 1).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = false }).Where(m => m.CompanyID == companyID).ToList();
            // IEnumerable<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(1, 1, 1).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = false }).ToList();

            List<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(lUserID).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = false }).ToList();
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
                //LoadHDO(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }
    }
}