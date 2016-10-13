<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounting/Reports/RPTAccounting.Master" AutoEventWireup="true"
    CodeBehind="LedgerBookAll.aspx.cs" Inherits="ABS.Web.Areas.Accounting.Reports.LedgerBookAll" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        table tr td {
            margin: 10px; /* Add something like this */
        }
    </style>

    <script type="text/javascript">


        function SelectedAccountCode() {

        }
        function pageLoad() {


            $("#<%=dpStartdate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            });


            $("#<%=dpEnddate.ClientID%>").datepicker({
                format: "dd/mm/yyyy",
                todayHighlight: true,
                autoclose: true
            });




            $("#<%=ddlLevel.ClientID%>").select2({
                placeholder: "Select Level",
                allowClear: true
            });

            //$("#reveal").hide();



            $("#<%=ddlAccountCode.ClientID%>").on("change", function () {
                var ddlvalue = $("#<%=ddlAccountCode.ClientID%>").val();
                $("#<%=hdnAcCode.ClientID%>").val(ddlvalue);
            });
        }



        function WarningMessage() {
            UYResult("Please provide specific value", "warning");
        }

        function BindAccountCode() {

            $("#<%=ddlAccountCode.ClientID%>").empty();
        $("#<%=hdnAcCode.ClientID%>").empty()


        var level = parseInt($("#<%=ddlLevel.ClientID%>").val());

        if ($("#<%=ddlLevel.ClientID%>").val() != "" && $("#<%=ddlLevel.ClientID%>").val() != NaN && $("#<%=ddlLevel.ClientID%>").val() != null) {


            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/Accounting/ACDetails/BindAccountCode")%>',
                    data: "{'id': '" + level + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //$("#reveal").show();
                        $("#<%=ddlAccountCode.ClientID%>").append($("<option></option>"));

                             $.each(data, function (key, value) {
                                 $("#<%=ddlAccountCode.ClientID%>").append($("<option></option>").val(value.Value).html(value.Text));

                            });
                            $("#<%=ddlAccountCode.ClientID%>").select2({
                                 placeholder: "Select",
                                 allowClear: true
                             });

                             var hdnValue = $("#<%=hdnAcCode.ClientID%>").val();
                             if (hdnValue != null && hdnValue != "") {
                                 $("#<%=ddlAccountCode.ClientID%>").select2("val", hdnValue);
                            }

                         },
                    failure: function (response) {
                        alert("Error in calling Ajax:" + response);
                    }
                });
                }


            }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />

    <div class="container" style="margin-top: 10px;">

        <div class="row">
            <div class="col-md-12">
                <div class="widget box">
                    <div class="widget-header">
                        <h4><i class="icon-reorder"></i>Ledger Book (All Level)</h4>
                        <div class="btn-group pull-right">
                            <asp:Button ID="Button1" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                                CssClass="btn btn-sm btn-success" OnClick="btnShow_Click" />
                        </div>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hdnAcCode" ClientIDMode="Static" />
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="col-sm-3 control-label">
                                                Start date
                                            </div>
                                            <div class="col-sm-7">
                                                <div class='input-group date'>
                                                    <input type="text" id="dpStartdate" class="form-control input-sm" readonly="readonly" runat="server" />
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-sm-3 control-label">
                                                Level
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="input-group">
                                                    <%--     <select id="ddlOutlet" class="form-control input-sm" runat="server"></select>
                        
                                <span class="input-group-addon"><asp:CheckBox ID="chkAll" runat="server" Text=" All" ClientIDMode="Static" /></span>--%>

                                                    <select id="ddlLevel" class="form-control input-sm" style="width: 265px" runat="server" onchange="BindAccountCode()">
                                                        <option></option>
                                                        <option value="1">Level-1</option>
                                                        <option value="2">Level-2</option>
                                                        <option value="3">Level-3</option>
                                                        <option value="4">Level-4</option>
                                                        <option value="5">Level-5</option>

                                                    </select>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="col-sm-3 control-label">
                                                End Date
                                            </div>
                                            <div class="col-sm-7">
                                                <div class='input-group date'>
                                                    <input type="text" id="dpEnddate" class="form-control input-sm" readonly="readonly" runat="server" />

                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <div class="col-sm-3 control-label">Account Ledger</div>
                                            <div class="col-sm-7">
                                                <div class="input-group">
                                                    <%--<select id="ddlAccountCode" class="form-control input-sm" style="width: 265px" runat="server"></select>--%>

                                                    <%--                                <span class="input-group-addon">
                                    <asp:CheckBox ID="chkUser" runat="server" Text=" All" ClientIDMode="Static" /></span>--%>
                                                    <select id="ddlAccountCode" class="form-control input-sm" style="width: 265px" runat="server">
                                                    </select>


                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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

