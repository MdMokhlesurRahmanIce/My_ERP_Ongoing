using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using ABS.Models;
using PBSConnLib;

namespace ABS.Web.Areas.Accounting.Reports
{
    public partial class ChartOfAccountReport : System.Web.UI.Page
    {
        private readonly ERP_Entities _db = new ERP_Entities();

        private PBSDBUtility dbConn = new PBSDBUtility();      
     

        TreeNode tn;

        DataTable DTabLevel1 = new DataTable();
        DataTable DTabLevel2 = new DataTable();
        DataTable DTabLevel3 = new DataTable();
        DataTable DTabLevel4 = new DataTable();
        DataTable DTabLevel5 = new DataTable();


  
    

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/Account/Login");
        }

        treemenupulate();
    }




    public void treemenupulate()
    {

        TreeNode root = null;
        try
        {
            root = new TreeNode("Chart Of Accounts");
            tv.Nodes.Add(root);




            TreeNode parentNode = null;
            DTabLevel1 = dbConn.GetDataBySQLString("Select Id,AC1Name Name From AccAC1");
            foreach (DataRow dr in DTabLevel1.Rows)
            {
                parentNode = new TreeNode(dr[1].ToString());
                string mdl_name = dr[1].ToString().Trim();

                root.ChildNodes.Add(parentNode);

                int level1Id = int.Parse(dr[0].ToString());
                TreeNode childnodeL2 = null;
                DTabLevel2 = dbConn.GetDataBySQLString("Select Id,AC2Name Name From AccAC2 Where AC1Id=" + level1Id + "");
                foreach (DataRow drc in DTabLevel2.Rows)
                {
                    childnodeL2 = new TreeNode(drc[1].ToString());
                    string men_name = drc[1].ToString().Trim();
                    parentNode.ChildNodes.Add(childnodeL2);
                    parentNode.Collapse();
                    int level2Id = int.Parse(drc[0].ToString());
                    TreeNode childnodeL3 = null;
                    DTabLevel3 = dbConn.GetDataBySQLString("Select Id,AC3Name Name From AccAC3 Where AC2Id=" + level2Id + "");
                    foreach (DataRow drs in DTabLevel3.Rows)
                    {
                        childnodeL3 = new TreeNode(drs[1].ToString());
                        childnodeL2.ChildNodes.Add(childnodeL3);
                        childnodeL2.Collapse();
                        int level3Id = int.Parse(drs[0].ToString());
                        TreeNode childnodeL4 = null;
                        DTabLevel4 = dbConn.GetDataBySQLString("Select Id,AC4Name Name From AccAC4 Where AC3Id=" + level3Id + "");


                        foreach (DataRow drs4 in DTabLevel4.Rows)
                        {
                            childnodeL4 = new TreeNode(drs4[1].ToString());
                            childnodeL3.ChildNodes.Add(childnodeL4);
                            childnodeL3.Collapse();
                            int level4Id = int.Parse(drs4[0].ToString());
                            TreeNode childnodeL5 = null;
                            DTabLevel5 = dbConn.GetDataBySQLString("Select Id,ACName Name From AccACDetail Where AC4Id=" + level4Id + "");

                            foreach (DataRow drs5 in DTabLevel4.Rows)
                            {
                                childnodeL5 = new TreeNode(drs5[1].ToString());
                                childnodeL4.ChildNodes.Add(childnodeL5);
                                childnodeL4.Collapse();


                            }





                        }



                    }

                }

            }

        }
        catch (Exception ee)
        {
            throw ee;
        }


    }

    protected void tv_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        string name = tv.SelectedValue;
        string name1 = tv.SelectedNode.ChildNodes.ToString();
        tn = new TreeNode();
        tn = tv.FindNode(name1);


        tn.Expand();

    }


    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {

        try
        {





            DataTable dt = dbConn.GetDataByProc("AC_rptChartOfAccountexcel");

            //calling create Excel File Method and ing dataTable   
            ExporttoExcel(dt);
        }
        catch (Exception)
        {

            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "MessageBox('Please Enter Data Correctly','Information');", true);
        }



    }


    private void ExporttoExcel(DataTable dtExcel)
    {



        string filename = "ChartOfAccount"+DateTime.Today.ToShortTimeString();

        

        filename = filename + ".xls";





        try
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename="+filename+"");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

            HttpContext.Current.Response.Write("<font style='font-size:11.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR>");

            HttpContext.Current.Response.Write("<Table border='0' bgColor='#ffffff' " +
                          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                          "style='font-size:11.0pt; font-family:Calibri; background:white;'>");


            #region Report Header
            //HttpContext.Current.Response.Write("<TR valign='top'>");
            //HttpContext.Current.Response.Write("<B><U><TD align='center' colspan='9' style='font-size:14.0pt;text-weight:bold;text-decoration:underline;'>TO WHOMSOEVER IT MAY CONCERN</TD>");
            //HttpContext.Current.Response.Write("</U></B></TR>");

            //HttpContext.Current.Response.Write("<TR valign='top'><TD align='left' colspan='9'> Employee Personal Details </TD></TR>");
            //HttpContext.Current.Response.Write("<TR valign='top'><TD align='left' colspan='9'> </TD></TR>");

            //HttpContext.Current.Response.Write("<TR valign='top'><TD align='left' colspan='9' rowspan='3' style='whitespace:normal;'>");
            //HttpContext.Current.Response.Write("</TD></TR>");

            #endregion

            #region Header Row

            HttpContext.Current.Response.Write("<TR valign='top'><td colspan='10'");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#FFFFFF' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'>");
            HttpContext.Current.Response.Write("<TR  valign='top' style='background:#D8D8D8;'>");
            HttpContext.Current.Response.Write("<TD align='left' style='width:20%;'>Level-1</TD>");
            HttpContext.Current.Response.Write("<TD align='center' style='width:10%;'>Level-2</TD>");
            HttpContext.Current.Response.Write("<TD align='center' style='width:10%;'>Level-3</TD>");
            HttpContext.Current.Response.Write("<TD align='center' style='width:10%;'>Level 4</TD>");

            HttpContext.Current.Response.Write("<TD align='center' style='width:10%;'>Ledger Name</TD>");

            HttpContext.Current.Response.Write("</TR>");
            #endregion

            #region Detail Row
            for (int iRow = 0; iRow < dtExcel.Rows.Count; iRow++)
            {
                HttpContext.Current.Response.Write("<TR valign='top'>");
                HttpContext.Current.Response.Write("<TD align='left'>" + dtExcel.Rows[iRow]["Level_1"].ToString() + "</TD>");
                HttpContext.Current.Response.Write("<TD align='left'>" + dtExcel.Rows[iRow]["Level_2"].ToString() + "</TD>");
                HttpContext.Current.Response.Write("<TD align='left'>" + dtExcel.Rows[iRow]["Level_3"].ToString() + "</TD>");
                HttpContext.Current.Response.Write("<TD align='left'>" + dtExcel.Rows[iRow]["Level_4"].ToString() + "</TD>");
                HttpContext.Current.Response.Write("<TD align='left'>" + dtExcel.Rows[iRow]["LedgerName"].ToString() + "</TD>");
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            #endregion

            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            throw (ex);
        }



    }


    }
}