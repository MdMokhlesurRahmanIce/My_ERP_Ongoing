using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Reports.Inventory;
using ABS.Reports.Production;
using ABS.Service.Inventory.Factories;
using ABS.Service.Production.Factories;
using ABS.Service.Sales.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Service.SystemCommon.Factories;

namespace ABS.Web.Areas.Production.Reports
{
    public partial class WeavingLoomData : System.Web.UI.Page
    {
        //private ERP_Entities _ctxCmn = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            int lUserID = Convert.ToInt16(Session["UserID"]);
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

            if (!IsPostBack)
            {
                if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }
            }
        }        

        

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Report = new rptWeavingLoomData();
                DateTime dateFrom = Convert.ToDateTime(txtStartDate.Text);
                int company = Convert.ToInt16(Session["CompanyID"]);


                (ReportViewer1.Report as rptWeavingLoomData).paramDateFrom = dateFrom; //UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);    
                (ReportViewer1.Report as rptWeavingLoomData).companyId = company;

                //Telerik.Reporting.Report report = (Telerik.Reporting.Report)this.ReportViewer1.Report;

                //Telerik.Reporting.TextBox txt = report.Items.Find("dtFrom", true)[0] as Telerik.Reporting.TextBox;
                //Telerik.Reporting.TextBox txt1 = report.Items.Find("dtTo", true)[0] as Telerik.Reporting.TextBox;

                //txt.Value = dateFrom.ToString("dd-MM-yyyy");
                //string edate = dateTo != null ? dateTo.Value.ToString("dd-MM-yyyy") : "";
                //if (txtEndDate.Text != "")
                //{
                //    txt1.Value = "To   " + edate;
                //}
                //else
                //{
                //    txt1.Value = edate;
                //}

                ReportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}