using ABS.Reports.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace ABS.Web.Areas.Sales.Reports
{
    public partial class RPTSample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var x = new Telerik.Reporting.InstanceReportSource();

            x.Parameters.Add(new Telerik.Reporting.Parameter("pramPINo", "PI-00000155-Revise-2"));
            x.ReportDocument = new rptPI();

            ReportViewer1.ReportSource = x;
            ReportViewer1.RefreshReport();

            //ReportViewer1.Report = new rptPI();
            //(ReportViewer1.Report as rptPI).pramPINo = "PI-00000155-Revise-2";
            //ReportViewer1.RefreshReport();
        }
    }
}