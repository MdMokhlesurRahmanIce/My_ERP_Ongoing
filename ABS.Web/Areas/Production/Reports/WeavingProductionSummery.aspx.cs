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
    public partial class WeavingProductionSummery : System.Web.UI.Page
    {
        //private ERP_Entities _ctxCmn = null;
        ProductionDDLMgt _allProdDdl = new ProductionDDLMgt();
        SystemCommonDDL systemCommon = new SystemCommonDDL();
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
                else
                {
                    //LoadMonth();
                }
            }
        }

        //public enum MonthEnum
        //{
        //    January,
        //    February,
        //    March,
        //    April,
        //    May,
        //    June,
        //    July,
        //    August,
        //    September,
        //    October,
        //    November,
        //    December
        //}

        //protected void LoadMonth()
        //{
        //   try
        //    {
        //        ddlMonth.DataSource = Enum.GetNames(typeof(MonthEnum));
        //        ddlMonth.DataBind();

        //        ListItem item = new ListItem("--Select Month--", "0");
        //        ddlMonth.Items.Insert(0, item);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //}

        //void GetMonthBoundaries(int month, int year, out DateTime firstDayOfMonth, out DateTime lastDayOfMonth)
        //{
        //    // how to get the 1st day of the month? it's always day 1, so this is simple enough 
        //    DateTime first = new DateTime(year, month, 1);

        //    // how do we get the last day of the month when months have
        //    // varying numbers of days?
        //    // 

        //    // adding 1 month to "first" gives us the 1st day of the following month
        //    // then if we subtract one second from that date, we get a DateTime that
        //    // represents the last second of the last day of the desired month

        //    // example: if month = 2 and year = 2011, then
        //    // first = 2011-02-01 00:00:00
        //    // last = 2011-02-28 23:59:59 
        //    DateTime last = first.AddMonths(1).AddSeconds(-1);

        //    // if you're not concerned with time of day in your DateTime values
        //    // and are only comparing days, then you can use the following line
        //    // instead to get the last day:
        //    //
        //    // DateTime last = first.AddMonths(1).AddDays(-1);
        //    // example: if month = 2 and year = 2011, then
        //    // first = 2011-02-01 00:00:00
        //    // last = 2011-02-28 00:00:00 
        //    firstDayOfMonth = first;
        //    lastDayOfMonth = last;
        //}

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Report = new rptWeavingProductionSummery();
                DateTime dateFrom = Convert.ToDateTime(txtStartDate.Text);
                DateTime? dateTo = null;
                if (txtEndDate.Text != "")
                {
                    dateTo = Convert.ToDateTime(txtEndDate.Text);
                }

                int company = Convert.ToInt16(Session["CompanyID"]);
                int department = 0;
                if (company == 1)
                {
                    department = 11;
                }
                else if(company == 2)
                {
                    department = 60;
                }

                //var month = ddlMonth.SelectedIndex;

 
                (ReportViewer1.Report as rptWeavingProductionSummery).paramDateFrom = dateFrom; //UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptWeavingProductionSummery).paramDateTo = dateTo;
                (ReportViewer1.Report as rptWeavingProductionSummery).companyId = company;
                (ReportViewer1.Report as rptWeavingProductionSummery).departmentId = department;

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

        //protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        // //   LoadSet();
        //}
    }
}