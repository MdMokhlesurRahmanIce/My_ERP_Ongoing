using System;
using System.Web.UI;
using ABS.Reports.Accounting;
using ABS.Web.Utility;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class LedgerBookAll : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
               
            }
        }

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static SelectList BindAccountCode(int level)
        //{
        //    var lst = new SelectList("Value","Text");

        //    switch (level)
        //    {
        //        case 1:
        //            using (var db = new ERP_Entities())
        //            {

        //              lst=  new SelectList(from r in db.AC1.ToList()
        //                    orderby r.AC1Name ascending
        //                    select new {Text = r.AC1Name, Value = r.Id}, "Value", "Text");
        //            }
        //            break;
        //        //case 2:
        //        //    new SelectList(from r in db.AC2.ToList()
        //        //                               orderby r.AC2Name ascending
        //        //                               select new { Text = r.AC2Name, Value = r.Id }, "Value", "Text");

        //        //    break;
        //        //case 3:
        //        //    return Json(new SelectList(from r in db.AC3.ToList()
        //        //                               orderby r.AC3Name ascending
        //        //                               select new { Text = r.AC3Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

        //        //    break;
        //        //case 4:
        //        //    return Json(new SelectList(from r in db.AC4.ToList()
        //        //                               orderby r.AC4Name ascending
        //        //                               select new { Text = r.AC4Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

        //        //    break;
        //        default:
        //        {
        //            using (var db = new ERP_Entities())
        //            {


        //               lst= new SelectList(from r in db.ACDetails.ToList()
        //                    orderby r.ACName ascending
        //                    select new {Text = r.ACName, Value = r.Id}, "Value", "Text");
        //            }
        //            break;
        //        }
        //    }

        //    return lst;
        //}


        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                if (dpStartdate.Value != "" && dpEnddate.Value != "" && hdnAcCode.Value != "" && ddlLevel.Value != "")
                {


                    ReportViewer1.Report = new LedgerBookAllLevel();

                    (ReportViewer1.Report as LedgerBookAllLevel).pvparam = UniqueCode.GetDateFormat_MM_dd_yyy(dpStartdate.Value);
                    (ReportViewer1.Report as LedgerBookAllLevel).pvparam1 = UniqueCode.GetDateFormat_MM_dd_yyy(dpEnddate.Value);
                    (ReportViewer1.Report as LedgerBookAllLevel).pvparam2 = hdnAcCode.Value;
                    (ReportViewer1.Report as LedgerBookAllLevel).pvparam3 = ddlLevel.Value;
                    (ReportViewer1.Report as LedgerBookAllLevel).CompanyId = Convert.ToInt32(Session["CompanyID"].ToString());

                    


                    ReportViewer1.RefreshReport();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "BindAccountCode();", true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "WarningMessage();", true);

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

    }
}