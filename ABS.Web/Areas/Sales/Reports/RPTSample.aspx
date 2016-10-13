<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sales/Reports/RPTSales.Master" AutoEventWireup="true"
    CodeBehind="RPTSample.aspx.cs" Inherits="ABS.Web.Areas.Sales.Reports.RPTSample" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSalesMaster" runat="server">
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    <telerik:ReportViewer ID="ReportViewer1" runat="server" ViewMode="PrintPreview" Width="100%"></telerik:ReportViewer>
</asp:Content>
