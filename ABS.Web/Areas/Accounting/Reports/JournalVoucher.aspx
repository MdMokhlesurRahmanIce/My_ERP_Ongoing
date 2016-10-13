<%@ Page Language="C#" MasterPageFile="~/Areas/Accounting/Reports/RPTAccounting.Master" AutoEventWireup="true" CodeBehind="JournalVoucher.aspx.cs" Inherits="ABS.Web.Areas.Accounting.Reports.JournalVoucher" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.ReportViewer.WebForms" Assembly="Telerik.ReportViewer.WebForms, Version=7.2.13.1016, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#ddlCompanyName").select2({
                placeholder: "Select a Company",
                allowClear: true
            });
            $("#cbPVNO").select2({
                placeholder: "Select a VoucherNo",
                allowClear: true
            });

        });


        function WarningMessage() {
            UYResult("Please Select a Journal VoucherNo", "warning");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="margin-top: 10px;">
        <!--=== Page Header ===-->
        
        <div class="row">
            <div class="col-md-12">
                <div class="widget box">
                    <div class="widget-header">
                        <h4><i class="icon-reorder"></i>Journal Voucher</h4>
                    </div>
                    <div class="widget-content">
                        <div class="row well-sm">
                            <div class="col-sm-10">

                                <div class="form-group">
                                    <div class="col-sm-3 control-label">Select Company</div>
                                    <div class="col-sm-7">
                                        <div class="input-group">
                                        <asp:DropDownList runat="server" ID="ddlCompanyName" CssClass="form-control input-sm" ClientIDMode="Static" Width="200px" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-3 control-label">Select Voucher No</div>
                                    <div class="col-sm-7">
                                        <div class="input-group">
                                        <asp:DropDownList runat="server" ID="cbPVNO" CssClass="form-control input-sm" ClientIDMode="Static" Width="200px">
                                        </asp:DropDownList>
                                            </div>

                                    </div>
                                </div>

                            </div>
                            <!-- /.col-md-6 -->

                            <div class="col-sm-2">
                              
                                    <asp:Button ID="btnShow" runat="server" Text="Show Report" ValidationGroup="valSaveDB"
                                        CssClass="btn btn-success" OnClick="btnShow_Click" />
                               </div>
                            
                        </div>
                        <!-- /.row -->


                        <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="480px" Width="100%" CssClass="reportViewer"  ViewMode="PrintPreview">
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
