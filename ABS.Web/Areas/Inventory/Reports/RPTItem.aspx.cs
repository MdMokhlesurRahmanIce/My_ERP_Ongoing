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
    public partial class RPTItem : System.Web.UI.Page
    {
        PIMgt objPIMgt = new PIMgt();
        StockInfoMgt objMgt = new StockInfoMgt(); 

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
                    ListItem item = new ListItem("--ALL--", "ALL");
                    ddlItemType.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }

                LoadGroup(lUserID, lCompanyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            //if (ddlItemType.SelectedValue != "0")
            //{
                 ReportViewer1.Report = new rptItem();
                (ReportViewer1.Report as rptItem).pramItemTypeId = ddlItemType.SelectedValue.ToString();               
                (ReportViewer1.Report as rptItem).companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                 ReportViewer1.RefreshReport();
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

            //    //ReportViewer1.Report = new rpt_HDOMasterDetail();

            //    ////(ReportViewer1.Report as rpt_HDOMasterDetail).pramHDONo = ddlHDONo.SelectedValue.ToString();// "PI-00000155-Revise-2";
            //    //ReportViewer1.RefreshReport();
            //}
        }
        //protected void LoadItemType(int lUserID, int companyID) 
        //{
        //    List<vmItemTypes> lstItemMaster = objMgt.GetItemTypeList()
        //                                     .Select(m => new vmItemTypes
        //                                     {
        //                                         ItemTypeID = m.ItemTypeID,
        //                                         ItemTypeName = m.ItemTypeName,
        //                                         CompanyID = m.CompanyID,
        //                                         CreateBy = m.CreateBy,                                               
        //                                     })
        //                                     .Where(m => m.CompanyID == companyID).ToList();

        //    ddlItemType.DataSource = lstItemMaster;
        //    ddlItemType.DataValueField = "ItemTypeID";
        //    ddlItemType.DataTextField = "ItemTypeName";
        //    ddlItemType.DataBind();

        //    ListItem item = new ListItem("--ALL--", "ALL");
        //    ddlItemType.Items.Insert(0, item);
        //}


        protected void LoadGroup(int lUserID, int companyID)
        {
            List<vmItmGroup> lstItemMaster = objMgt.GetItemGroupList()
                                             .Select(m => new vmItmGroup
                                             {
                                                 ItemGroupID = m.ItemGroupID,
                                                 ItemGroupName = m.ItemGroupName,
                                                 CompanyID = m.CompanyID,
                                                 CreateBy = m.CreateBy,
                                             })
                                             .Where(m => m.CompanyID == companyID).ToList();

            ddlItemType.DataSource = lstItemMaster;
            ddlItemType.DataValueField = "ItemGroupID";
            ddlItemType.DataTextField = "ItemGroupName";
            ddlItemType.DataBind();

            ListItem item = new ListItem("--ALL--", "ALL");
            ddlItemType.Items.Insert(0, item);
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
                LoadGroup(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }       
    }
}