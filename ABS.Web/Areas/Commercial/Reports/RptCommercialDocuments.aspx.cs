using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Reports.Commercial;
using ABS.Service.Commercial.Factories;
using ABS.Service.Sales.Factories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace ABS.Web.Areas.Commercial.Reports
{
    public partial class RptCommercialDocuments : System.Web.UI.Page
    {
        DCMgt objDCMgt = new DCMgt();
        PIMgt objPIMgt = new PIMgt();
        protected void Page_Load(object sender, EventArgs e)
        {            
            int lUserID = Convert.ToInt16(Session["UserID"]);
            int lCompanyID = Convert.ToInt16(Session["CompanyID"]);

            if (!IsPostBack)
            {              
                if (!IsPostBack && lUserID > 0 && lCompanyID > 0)
                {
                    LoadCompany(lUserID, lCompanyID);
                    //ListItem item = new ListItem("--Select DC No--", "0");
                    //ddlDCNo.Items.Insert(0, item);
                }
                else if (lUserID == 0 || lCompanyID == 0)
                {
                    Response.Redirect("~/Account/Login");
                }
                LoadDCNo(lUserID, lCompanyID);
            }
        }
        protected void LoadDCNo(int lUserID, int lCompanyID)
        {
            IEnumerable<SalDCMaster> lstSalDCMaster = objDCMgt.GetDCMaster(lUserID, Convert.ToInt16(ddlCompany.SelectedValue));
            ddlDCNo.DataSource = lstSalDCMaster;
            ddlDCNo.DataValueField = "DCID";
            ddlDCNo.DataTextField = "DCNo";
            ddlDCNo.DataBind();

            ListItem item = new ListItem("--Select DC No--", "0");
            ddlDCNo.Items.Insert(0, item);
        }
        protected void LoadCompany(int lUserID, int companyID)
        {
            List<CmnCompany> lstCmnCompany = objPIMgt.GetPICompany(lUserID).Select(m => new CmnCompany { CompanyID = m.CompanyID, CompanyName = m.CompanyName, IsDeleted = false }).ToList();
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
                LoadDCNo(Convert.ToInt16(Session["UserID"]), companyID);
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (ddlDCNo.SelectedValue != "0")
            {
                ArrayList objList = new ArrayList();

                ReportBook reportBook = new ReportBook();
                
                int countCon = 0;
                for (int i = 0; i < 1; i++)
                {

                    if (ckbSpunCertificate.Checked)
                    {
                        reportBook.Reports.Add(new rptSpunCertificate());
                        (reportBook.Reports[countCon] as rptSpunCertificate).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;

                        // objList.Add("ckbSpun");
                    }
                    if (ckbCommercialInvoice.Checked)
                    {
                        reportBook.Reports.Add(new rptCommercialInvoice());
                        (reportBook.Reports[countCon] as rptCommercialInvoice).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;

                        //objList.Add("ckbCommercialInvoice");
                    }

                    if (ckbPreshipmentInspectionCertificate.Checked)
                    {
                        reportBook.Reports.Add(new rptPreShipmentInspectionCertificate());
                        (reportBook.Reports[countCon] as rptPreShipmentInspectionCertificate).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("ckbPreshipmentInspectionCertificate");
                    }
                    if (ckbDeliveryChallan.Checked)
                    {
                        reportBook.Reports.Add(new rptDeliveryChallanTruckReceipt());
                        (reportBook.Reports[countCon] as rptDeliveryChallanTruckReceipt).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("ckbDeliveryChallan");

                    }
                    if (ckbBeneficiaryCertificate.Checked)
                    {
                        reportBook.Reports.Add(new rptBeneficiarysCertificate());
                        (reportBook.Reports[countCon] as rptBeneficiarysCertificate).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("ckbBeneficiaryCertificate");
                    }
                    if (ckbBillOfExchange.Checked)
                    {
                        reportBook.Reports.Add(new rptBillOfExchange1());
                        (reportBook.Reports[countCon] as rptBillOfExchange1).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                      
                        countCon++;
                        //continue;
                        //objList.Add("ckbBillOfExchange");
                    }
                    if (ckbBillOfExchange.Checked)
                    {                      
                        reportBook.Reports.Add(new rptBillOfExchange2());
                        (reportBook.Reports[countCon] as rptBillOfExchange2).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);

                        countCon++;
                    }
                    if (chkCertificateOfOrigin.Checked)
                    {
                        reportBook.Reports.Add(new rptCertificateOfOrigin());
                        (reportBook.Reports[countCon] as rptCertificateOfOrigin).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("chkCertificateOfOrigin");
                    }
                    if (ckbConcernCertificate.Checked)
                    {
                        reportBook.Reports.Add(new rptConcernCertificate());
                        (reportBook.Reports[countCon] as rptConcernCertificate).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("chkCertificateOfOrigin");
                    }
                    if (ckbBankForwarding.Checked)
                    {
                        reportBook.Reports.Add(new rptBankForwarding());
                        (reportBook.Reports[countCon] as rptBankForwarding).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        //continue;
                        //objList.Add("chkCertificateOfOrigin");
                    }
                    if (ckbPakingList.Checked)
                    {
                        reportBook.Reports.Add(new rptPackingList());
                        (reportBook.Reports[countCon] as rptPackingList).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        // continue;
                        // objList.Add("ckbPakingList");
                    }
                    if (ckbPakingListGWNW.Checked)
                    {
                        reportBook.Reports.Add(new rptPackingListGWNW());
                        (reportBook.Reports[countCon] as rptPackingListGWNW).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        // continue;
                        // objList.Add("ckbPakingList");
                    }
                    if (ckbForwardingLetter.Checked)
                    {
                        reportBook.Reports.Add(new rptForwardingLetter());
                        (reportBook.Reports[countCon] as rptForwardingLetter).pramDCID = Convert.ToInt16(ddlDCNo.SelectedValue);
                        countCon++;
                        // continue;
                        // objList.Add("ckbPakingList");
                    }
                }
                var reportSource = new InstanceReportSource();
                reportSource.ReportDocument = reportBook;
                ReportViewer1.ReportSource = reportSource;


                ReportViewer1.RefreshReport();
            }
        }

        protected void ckbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if(ckbCheckAll.Checked==true)
            {
                ckbDeliveryChallan.Checked = true;
                ckbConcernCertificate.Checked = true;
                ckbBankForwarding.Checked = true;
                ckbBeneficiaryCertificate.Checked = true;
                ckbBillOfExchange.Checked = true;
                ckbCommercialInvoice.Checked = true;
                ckbForwardingLetter.Checked = true;
                chkCertificateOfOrigin.Checked = true;
                ckbPakingList.Checked = true;
                ckbPreshipmentInspectionCertificate.Checked = true;
                ckbSpunCertificate.Checked = true;
                ckbPakingListGWNW.Checked = true;
            }
            else
            {
                ckbDeliveryChallan.Checked = false;
                ckbConcernCertificate.Checked = false;
                ckbBankForwarding.Checked = false;
                ckbBeneficiaryCertificate.Checked = false;
                ckbBillOfExchange.Checked = false;
                ckbCommercialInvoice.Checked = false;
                ckbForwardingLetter.Checked = false;
                chkCertificateOfOrigin.Checked = false;
                ckbPakingList.Checked = false;
                ckbPreshipmentInspectionCertificate.Checked = false;
                ckbSpunCertificate.Checked = false;
                ckbPakingListGWNW.Checked = false;
            }
        }
    }
}
