<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Production/Reports/RPTProduction.Master" AutoEventWireup="true" CodeBehind="WeavingProductionSummery.aspx.cs" Inherits="ABS.Web.Areas.Production.Reports.WeavingProductionSummery" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ProdRptScriptsContentPlaceHolder" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            //$("#ddlCompany").select2({
            //    placeholder: "Select a Company",
            //    allowClear: true
            //});

            $("#<%=txtStartDate.ClientID%>").datepicker({
                format: "mm/dd/yyyy",
                todayHighlight: true,
                autoclose: true
            });
           <%-- $("#<%=txtStartDate.ClientID%>").datepicker('setDate', new Date());--%>


            $("#<%=txtEndDate.ClientID%>").datepicker({
                format: "mm/dd/yyyy",
                todayHighlight: true,
                autoclose: true
            });
            <%--$("#<%=txtEndDate.ClientID%>").datepicker('setDate', new Date());--%>
        });

        function WarningMessage() {
            UYResult("Please provide specific value", "warning");
        }

    </script>
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
                            <table>
                                <tr>
                                        <td>
                                        <div class='input-group date'>
                                            Date From
                                            <asp:TextBox type="text" ID="txtStartDate" runat="server" ClientIDMode="Static" onblur="ValidateDate(this)" Enabled="True" Height="25px" Width="120px"></asp:TextBox>
                                        </div>
                                    </td> 
                                     <td>
                                        <div class='input-group date'>
                                            Date To
                                            <asp:TextBox type="text" ID="txtEndDate" runat="server" ClientIDMode="Static" onblur="ValidateDate(this)" Enabled="True" CssClass="textbox2" Height="25px" Width="120px"></asp:TextBox>
                                        </div>
                                    </td> 
                    <%--        <td>Month
                            <asp:DropDownList ID="ddlMonth" runat="server" class="select2-select-00"
                                Width="100px">
                            </asp:DropDownList>
                                    </td>--%>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <div class="btn-group">
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                                                CssClass="btn btn-success" OnClick="btnShowReport_Click" />
                                            <%--     <asp:Button ID="btnShowReport" Text="Show Report" runat="server" OnClick="btnShowReport_Click" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
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
