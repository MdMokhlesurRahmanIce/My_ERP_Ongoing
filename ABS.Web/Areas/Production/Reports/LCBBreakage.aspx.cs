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
    public partial class LCBBreakage : System.Web.UI.Page 
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
                    LoadItem();
                    LoadSetFirst();
                    LoadMachine();
                    LoadOperators();
                     
                }
            }
        }        

        protected void LoadItem()
        {
            var itemDetais = new vmItemDetais();

            try
            {
                int lUserID = Convert.ToInt16(Session["UserID"]);
                int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

                //using (_ctxCmn = new ERP_Entities())
                //{
                //    itemDetais = (from IM in _ctxCmn.CmnItemMasters
                //                  where IM.ItemTypeID == 1
                //                  select new
                //                  {
                //                      ItemId = IM.ItemID,
                //                      ItemName = IM.ItemName
                //                  })
                //                         .Select(x => new vmItemDetais
                //                         {
                //                             ItemId = (int)x.ItemId,
                //                             ItemName = x.ItemName,
                //                             loggeduser = lUserID,
                //                             loggedCompany = lCompanyID
                //                         }).FirstOrDefault();
                //}

                itemDetais.loggeduser = lCompanyID;
                itemDetais.loggedCompany = lUserID;

                List<vmItemDetais> itemList = _allProdDdl.GetItems(itemDetais).ToList();

                ddlItem.DataSource = itemList;
                ddlItem.DataValueField = "ItemID";
                ddlItem.DataTextField = "ItemName";
                ddlItem.DataBind();

                ListItem item = new ListItem("--Select Item--", "0");
                ddlItem.Items.Insert(0, item);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        protected void LoadSetFirst()
        {
            ListItem item = new ListItem("--Select Set--", "0");
            ddlSet.Items.Insert(0, item);
        }

        protected void LoadSet()
        {
            var itemDetais = new vmItemDetais();

            try
            {
                int lUserID = Convert.ToInt16(Session["UserID"]);
                int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

                itemDetais.loggeduser = lCompanyID;
                itemDetais.loggedCompany = lUserID;
                itemDetais.ItemId = Convert.ToInt32(ddlItem.SelectedValue);

                List<vmItemDetais> itemList = _allProdDdl.GetSets(itemDetais).ToList();

                ddlSet.DataSource = itemList;
                ddlSet.DataValueField = "ItemID";
                ddlSet.DataTextField = "ItemName";
                ddlSet.DataBind();

                ListItem item = new ListItem("--Select Set--", "0");
                ddlSet.Items.Insert(0, item);
            }
            catch (Exception e)
            {
                e.ToString();
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

        protected void LoadOperators()
        {
            var itemDetais = new vmCmnParameters();

            try
            {
                int lUserID = Convert.ToInt16(Session["UserID"]);
                int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

                itemDetais.loggeduser = lCompanyID;
                itemDetais.loggedCompany = lUserID;
                itemDetais.ItemType = 1;
                //itemDetais.ItemGroup = 50;

                List<vmOperator> itemList = _allProdDdl.GetOperators(itemDetais).ToList();

                ddlOperator.DataSource = itemList;
                ddlOperator.DataValueField = "UserID";
                ddlOperator.DataTextField = "UserFullName";
                ddlOperator.DataBind();

                ListItem item = new ListItem("--Select Operator--", "0");
                ddlOperator.Items.Insert(0, item);
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
                ReportViewer1.Report = new rptLCBBreakage();
                DateTime dateFrom = Convert.ToDateTime(txtStartDate.Text);
                DateTime? dateTo = null;
                if (txtEndDate.Text != "")
                {
                    dateTo = Convert.ToDateTime(txtEndDate.Text);
                }
                 
                int item = Convert.ToInt32(ddlItem.SelectedValue);
                int set = Convert.ToInt32(ddlSet.SelectedValue);
                int machine = Convert.ToInt32(ddlMachine.SelectedValue);
                int pOperator = Convert.ToInt32(ddlOperator.SelectedValue);
                int company = Convert.ToInt16(Session["CompanyID"]);


                (ReportViewer1.Report as rptLCBBreakage).paramDateFrom = dateFrom; //UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptLCBBreakage).paramDateTo = dateTo;
                (ReportViewer1.Report as rptLCBBreakage).paramItem = item;
                (ReportViewer1.Report as rptLCBBreakage).paramSet = set;
                (ReportViewer1.Report as rptLCBBreakage).paramMachine = machine;
                (ReportViewer1.Report as rptLCBBreakage).paramOperator = pOperator;
                (ReportViewer1.Report as rptLCBBreakage).companyId = company;

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

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSet();
        }
    }
}