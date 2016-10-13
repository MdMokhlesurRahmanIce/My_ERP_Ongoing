<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commercial/Reports/RPTCommercial.Master" AutoEventWireup="true"
    CodeBehind="RptCommercialDocuments.aspx.cs" Inherits="ABS.Web.Areas.Commercial.Reports.RptCommercialDocuments" %>


<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCommercialMaster" runat="server">
    <div class="container" style="margin-top: 10px;">
        <div class="page-header">
        </div>
        <div class="row">
            <div class="col-md-3">
                <div>
                    Company
                            <asp:DropDownList ID="ddlCompany" runat="server" class="select2-select-00" Width="250px"
                                OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                </div>
                <div style="padding-bottom:10px;"></div>
                <div>
                    DC NO
                    <asp:DropDownList ID="ddlDCNo" runat="server" Width="250px" class="select2-select-00">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblCommercialDocument" Text="Commercial Documents List" Font-Bold="true" Font-Size="Medium" runat="server"></asp:Label>
                <br />
                <asp:CheckBox ID="ckbCheckAll" Text="Select All" runat="server" OnCheckedChanged="ckbCheckAll_CheckedChanged"
                    AutoPostBack="true" /><br />
                <asp:CheckBox ID="ckbPakingList" Text="Packing List" runat="server" /><br />
                <asp:CheckBox ID="ckbDeliveryChallan" Text="Delivery Challan" runat="server" /><br />
                <asp:CheckBox ID="ckbCommercialInvoice" Text="Commercial Invoice" runat="server" /><br />
                <asp:CheckBox ID="ckbBillOfExchange" Text="Bill Of Exchange" runat="server" />


            </div>
            <div class="col-md-3">
                <br />
                <asp:CheckBox ID="ckbPreshipmentInspectionCertificate" Text="Inspection Certificate" runat="server" /><br />
                <asp:CheckBox ID="ckbSpunCertificate" Text="Spun Certificate" runat="server" /><br />
                <asp:CheckBox ID="ckbForwardingLetter" Text="Forwarding Letter" runat="server" /><br />
                <asp:CheckBox ID="ckbBeneficiaryCertificate" Text="Beneficiary's Certificate" runat="server" /><br />
                <asp:CheckBox ID="ckbConcernCertificate" Text="Concern Certificate" runat="server" />
                
            </div>
            <div class="col-md-3">
                <br />
                <asp:CheckBox ID="chkCertificateOfOrigin" Text="Certificate Of Origin" runat="server" /><br />                
                <asp:CheckBox ID="ckbBankForwarding" Text="Bank Forwarding" runat="server" /><br />
                <asp:CheckBox ID="ckbPakingListGWNW" Text="Packing List (GWNW)" runat="server" /><br />
                <div class="btn-group">
                    <asp:Button ID="btnShowReport" Text="Show Report" CssClass="btn btn-success" runat="server" OnClick="btnShowReport_Click" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="widget-content" style="height: 480px">
                    <telerik:ReportViewer ID="ReportViewer1" runat="server" ViewMode="PrintPreview" Width="100%" Height="100%"></telerik:ReportViewer>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
