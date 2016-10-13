using ABS.Models.ViewModel.SystemCommon;
using ABS.Reports.Production;
using ABS.Service.SystemCommon.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.Production.Reports
{
    public partial class RndFabricAnalysisDevelopment : System.Web.UI.Page
    {
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
                    GetArticle();
                    
                }
            }
        }

        private void GetArticle()
        {
            var cmnParameter = new vmCmnParameters();
            List<VmItemMater> articles = systemCommon.GetArticleNo(cmnParameter);
            ddlArticle.DataSource = articles;
            ddlArticle.DataValueField = "ItemID";
            ddlArticle.DataTextField = "ArticleNo";
            ddlArticle.DataBind();

            ListItem item = new ListItem("--Select Article--", "0");
            ddlArticle.Items.Insert(0, item);
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ReportViewer1.Report = new rptRndFabricAnalysisDevelopment();
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);
            int item = Convert.ToInt32(ddlArticle.SelectedValue);
            (ReportViewer1.Report as rptRndFabricAnalysisDevelopment).CompanyID = lCompanyID;
            (ReportViewer1.Report as rptRndFabricAnalysisDevelopment).item = item;
        }
    }
}