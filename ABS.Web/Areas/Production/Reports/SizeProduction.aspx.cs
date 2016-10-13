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
    public partial class SizeProduction : System.Web.UI.Page
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
                    LoadShift();
                    LoadBuyers();
                    LoadPI();
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

        protected void LoadPI()
        {
            var cmnParameter = new vmCmnParameters();

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

                cmnParameter.loggeduser = lCompanyID;
                cmnParameter.loggedCompany = lUserID;

                List<vmPIMaster> itemList = systemCommon.GetPI(cmnParameter).ToList();

                ddlPI.DataSource = itemList;
                ddlPI.DataValueField = "PIID";
                ddlPI.DataTextField = "PINO";
                ddlPI.DataBind();

                ListItem item = new ListItem("--Select PI--", "0");
                ddlPI.Items.Insert(0, item);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

       
        protected void LoadBuyers()
        {
            var cmnParameter = new vmCmnParameters();

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

                cmnParameter.loggeduser = lCompanyID;
                cmnParameter.loggedCompany = lUserID;

                List<vmBuyer> itemList = systemCommon.GetBuyers(cmnParameter).ToList();

                ddlBuyer.DataSource = itemList;
                ddlBuyer.DataValueField = "UserID";
                ddlBuyer.DataTextField = "UserName";
                ddlBuyer.DataBind();

                ListItem item = new ListItem("--Select Buyer--", "0");
                ddlBuyer.Items.Insert(0, item);
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
                ReportViewer1.Report = new rptSizeProduction();
                DateTime dateFrom = Convert.ToDateTime(txtStartDate.Text);
                DateTime? dateTo = null;
                if (txtEndDate.Text != "")
                {
                    dateTo = Convert.ToDateTime(txtEndDate.Text);
                }

                int shift = Convert.ToInt32(ddlShift.SelectedValue);
                int buyer = Convert.ToInt32(ddlShift.SelectedValue);
                int pi = Convert.ToInt32(ddlShift.SelectedValue);
                int item = Convert.ToInt32(ddlItem.SelectedValue);
                int set = Convert.ToInt32(ddlSet.SelectedValue);
                int machine = Convert.ToInt32(ddlMachine.SelectedValue);
                int pOperator = Convert.ToInt32(ddlMachine.SelectedValue);
                int company = Convert.ToInt16(Session["CompanyID"]);


                (ReportViewer1.Report as rptSizeProduction).paramDateFrom = dateFrom; //UniqueCode.GetDateFormat_MM_dd_yyy(txtStartDate.Text);
                (ReportViewer1.Report as rptSizeProduction).paramDateTo = dateTo;
                (ReportViewer1.Report as rptSizeProduction).paramShift = shift;
                (ReportViewer1.Report as rptSizeProduction).paramBuyer = buyer;
                (ReportViewer1.Report as rptSizeProduction).paramPI = pi;
                (ReportViewer1.Report as rptSizeProduction).paramItem = item;
                (ReportViewer1.Report as rptSizeProduction).paramSet = set;
                (ReportViewer1.Report as rptSizeProduction).paramMachine = machine;
                (ReportViewer1.Report as rptSizeProduction).paramOperator = pOperator;
                (ReportViewer1.Report as rptSizeProduction).companyId = company;

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