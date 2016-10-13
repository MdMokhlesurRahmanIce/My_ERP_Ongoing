<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounting/Reports/RPTAccounting.Master" AutoEventWireup="true" CodeBehind="BalanceSheet.aspx.cs" Inherits="ABS.Web.Areas.Accounting.Reports.BalanceSheet" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script type="text/javascript">

        $(document).ready(function () {
            $("#ddlCompanyName").select2({
                placeholder: "Select a Company",
                allowClear: true
            });

            $("#<%=dpStartdate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            });
           <%-- $("#<%=txtStartDate.ClientID%>").datepicker('setDate', new Date());--%>

            $("#<%=dpEnddate.ClientID%>").datepicker({
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="margin-top: 10px;">
    
        <div class="row">
            <div class="col-md-12">
                <div class="widget box">
                    <div class="widget-header">
                        <h4><i class="icon-reorder"></i>Balance Sheet</h4>
                        <div class="btn-group pull-right">
                            <asp:Button ID="Button1" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                                    CssClass="btn btn-sm btn-success" OnClick="btnShow_Click" />
                        </div>
                    </div>
                    <div class="widget-content">
                        <div class="row">
                            <div class="col-sm-4">

                                <div class="form-group">
                                    <div class="col-sm-4 control-label">Company</div>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:DropDownList runat="server" ID="ddlCompanyName" CssClass="form-control input-sm" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                
                                

                                
                                
                            </div>
                             <div class="col-sm-4">
                                 <div class="form-group">
                                    <div class="col-sm-4 control-label">Start date</div>
                                    <div class="col-sm-8">
                                        <div class='input-group date'>
                                            <input type="text" id="dpStartdate" class="form-control input-sm" readonly="readonly" runat="server"/>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                 
                                 
                                 </div>

                              <div class="col-sm-4">
                                  <div class="form-group">
                                    <div class="col-sm-4 control-label">End date</div>
                                    <div class="col-sm-8">
                                        <div class='input-group date'>
                                            <input type="text" id="dpEnddate" class="form-control input-sm" readonly="readonly" runat="server"/>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                  <div class="form-group">
                                        <div class="col-sm-4 control-label"></div>
                                    <div class="col-sm-8">
                                  
                                        </div>
                                      </div>
                                 </div>

                            <!-- /.col-md-6 -->

                            

                        </div>
                        <!-- /.row -->
                  
                              <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="480px" Width="100%" CssClass="reportViewer" ViewMode="PrintPreview">
                        </telerik:ReportViewer>
                      

                      
                    </div>
                    <!-- /.widget-content -->
                </div>
                <!-- /.widget .box -->
            </div>
            <!-- /.col-md-12 -->
        </div>

    </div>
</asp:Content>

