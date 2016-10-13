<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Production/Reports/RPTProduction.Master" AutoEventWireup="true" CodeBehind="RndFabricAnalysisDevelopment.aspx.cs" Inherits="ABS.Web.Areas.Production.Reports.RndFabricAnalysisDevelopment" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ProdRptScriptsContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProdRptBodyContentPlaceHolder" runat="server">
    <div class="container" style="margin-top: 10px;">
        <!--=== Page Header ===-->
        <div class="page-header">
        </div>
        <!-- /Page Header -->
        <!--=== Page Content ===-->
        <div class="row">
            <div class="col-md-12">
                <div class="widget box">
                    <div class="widget-header">
                        <h4><i class="icon-reorder"></i></h4>

                        <div class="toolbar no-padding">
                            <div class="col-md-12">
                            <div class="col-md-4">
                            <%--    class="select2-select-00"--%>
                                <asp:DropDownList ID="ddlArticle" runat="server" 
                                    Width="100px">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <div class="btn-group">
                                    <asp:Button ID="btnShowReport" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                                        CssClass="btn btn-success" OnClick="btnShowReport_Click" />
                                    <%--     <asp:Button ID="btnShowReport" Text="Show Report" runat="server" OnClick="btnShowReport_Click" />--%>
                                </div>
                            </div>
                            <div class="col-md-4"></div>
                        </div> </div>
                    </div>
                    <div class="widget-content" style="height: 480px">
                        <telerik:ReportViewer ID="ReportViewer1" runat="server" ViewMode="PrintPreview" Width="100%" Height="100%">

                        </telerik:ReportViewer>

                    </div>
                </div>
            </div>
        </div>
        <!-- /.col-md-12 -->


        <!-- /Page Content -->
    </div>
</asp:Content>
