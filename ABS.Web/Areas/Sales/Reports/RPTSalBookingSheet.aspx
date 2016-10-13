<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sales/Reports/RPTSales.Master" AutoEventWireup="true"
    CodeBehind="RPTSalBookingSheet.aspx.cs" Inherits="ABS.Web.Areas.Sales.Reports.RPTSalBookingSheet" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtStartDate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            })

            $("#<%=txtEndDate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            });
        });
        function WarningMessage() {
            alert("Please provide specific value");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSalesMaster" runat="server">
    <div class="box">
        <div class="box-header with-border">
            <h3 class="box-title">Booking Sheet Detail Report</h3>
        </div>
        <div class="box-body">
            <table>
                <tr>
                    <td>Company:
                    </td>
                    <td style="width: 5px"></td>
                    <td>                      
                        <asp:DropDownList ID="ddlCompany" runat="server" class="select2-select-00"
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10px"></td>
                    <td>Start date:
                    </td>
                    <td style="width: 5px"></td>
                    <td>
                        <div class='input-group date'>
                            <asp:TextBox type="text" ID="txtStartDate" runat="server" ClientIDMode="Static" onblur="ValidateDate(this)" Enabled="True" CssClass="textbox2" Width="190px"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </td>
                    <td style="width: 10px"></td>
                    <td>End date:
                    </td>
                    <td style="width: 5px"></td>
                    <td>
                        <div class='input-group date'>
                            <asp:TextBox type="text" ID="txtEndDate" runat="server" ClientIDMode="Static" onblur="ValidateDate(this)" Enabled="True" CssClass="textbox2" Width="190px"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </td>
                    <td style="width: 10px"></td>

                    <td>
                        <asp:Button ID="btnShowReport" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                            CssClass="btn btn-success" OnClick="btnShowReport_Click" />
                    </td>
                </tr>
            </table>
            <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="480px" Width="100%" ViewMode="PrintPreview"></telerik:ReportViewer>
        </div>
    </div>
</asp:Content>
