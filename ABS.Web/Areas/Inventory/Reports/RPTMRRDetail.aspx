<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Inventory/Reports/RPTInventory.Master" AutoEventWireup="true" CodeBehind="RPTMRRDetail.aspx.cs" Inherits="ABS.Web.Areas.Inventory.Reports.RPTMRRDetail" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            //$("#ddlCompany").select2({
            //    placeholder: "Select a Company",
            //    allowClear: true
            //});

            $("#<%=txtStartDate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            });
           <%-- $("#<%=txtStartDate.ClientID%>").datepicker('setDate', new Date());--%>

            $("#<%=txtEndDate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
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


<asp:Content ID="Content2" ContentPlaceHolderID="cphInventoryMaster" runat="server">

    <div class="box">
        <div class="box-header with-border">

            <h3 class="box-title">MRR Detail Report</h3>
        <%--    <div class="box-tools pull-right">
                <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="Collapse"><i class="fa fa-minus"></i></button>
                <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>

            </div>--%>
        </div>
        <div class="box-body">


            <table>
                <tr>
                    <td>Company:
                    </td>
                    <td style="width: 5px"></td>
                    <td>

                       <%-- <asp:DropDownList runat="server" ID="ddlCompanyName" CssClass="form-control input-sm" ClientIDMode="Static" Width="200px">
                        </asp:DropDownList>--%>

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
