<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commercial/Reports/RPTCommercial.Master" AutoEventWireup="true"
    CodeBehind="RptFactoryDeliveryOrder.aspx.cs" Inherits="ABS.Web.Areas.Commercial.Reports.RptFactoryDeliveryOrder" %>


<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCommercialMaster" runat="server">    
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

                             Company   <asp:DropDownList ID="ddlCompany" runat="server" class="select2-select-00" Width="250px"
                                OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                           DC NO
                    <asp:DropDownList ID="ddlFDONo" runat="server" Width="250px" class="select2-select-00">
                    </asp:DropDownList>
                            <div class="btn-group">
                                <asp:Button ID="btnShowReport" Text="Show Report" CssClass="btn btn-success" runat="server" OnClick="btnShowReport_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="widget-content" style="height:480px">
                         <telerik:ReportViewer ID="ReportViewer1" runat="server" ViewMode="PrintPreview" Width="100%" Height="100%"></telerik:ReportViewer>
                    </div>
                </div>
            </div>
   </div>
        <!-- /.col-md-12 -->

        <!-- /Page Content -->
    </div>
</asp:Content>
