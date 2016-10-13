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
    public partial class BallStopage : System.Web.UI.Page 
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
                     
                    LoadMachine(); 
                    LoadShift(); 
                }
            }
        }        



        protected void LoadMachine()
        {
            var itemDetais = new vmItemDetais();

            try
            {
                int lUserID = Convert.ToInt16(Session["UserID"]);
                int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

                itemDetais.loggeduser = lCompanyID;
                itemDetais.loggedCompany = lUserID;
                itemDetais.ItemType = 4;
                itemDetais.ItemGroup = 50;

                List<vmItemDetais> itemList = _allProdDdl.GetMachines(itemDetais).ToList();

                ddlMachine.DataSource = itemList;
                ddlMachine.DataValueField = "ItemID";
                ddlMachine.DataTextField = "ItemName";
                ddlMachine.DataBind();

                ListItem item = new ListItem("--Select Machine--", "0");
                ddlMachine.Items.Insert(0, item);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

 
        protected void LoadShift()
        {
            var cmnParameter = new vmCmnParameters();

            try
            {
                int lUserID = Convert.ToInt16(Session["UserID"]);
                int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

                cmnParameter.loggeduser = lCompanyID;
                cmnParameter.loggedCompany = lUserID;
                cmnParameter.ItemType = 1;
                //itemDetais.ItemGroup = 50;

                List<vmItemDetais> itemList = _allProdDdl.GetShift(cmnParameter).ToList();

                ddlShift.DataSource = itemList;
                ddlShift.DataValueField = "ItemId";
                ddlShift.DataTextField = "ItemName";
                ddlShift.DataBind();

                ListItem item = new ListItem("--Select Shift--", "0");
                ddlShift.Items.Insert(0, item);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
 
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Report = new rptBallStopage();
                DateTime dateFrom = Convert.ToDateTime(txtStartDate.Text);
                DateTime? dateTo = null;
                if (txtEndDate.Text != "")
                {
                    dateTo = Convert.ToDateTime(txtEndDate.Text);
                }

                int shift = Convert.ToInt32(ddlShift.SelectedValue); 
                int machine = Convert.ToInt32(ddlMachine.SelectedValue);
                
                int company = Convert.ToInt16(Session["CompanyID"]);


                (ReportViewer1.Report as rptBallStopage).paramDateFrom = dateFrom; //UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptBallStopage).paramDateTo = dateTo;
                (ReportViewer1.Report as rptBallStopage).paramShift = shift;
                (ReportViewer1.Report as rptBallStopage).paramMachine = machine;
                (ReportViewer1.Report as rptBallStopage).companyId = company;

                ReportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

    }
}