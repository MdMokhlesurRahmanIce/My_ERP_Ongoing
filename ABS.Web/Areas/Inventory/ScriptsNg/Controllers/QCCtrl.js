/**
* PICtrl.js
*/
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('qCCtrl', ['$scope', 'qcService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, qcService, conversion, $filter, $localStorage, uiGridConstants) {

        $scope.gridOptionsMasterQC = [];

        $scope.ChkQCCertificateNoWhnSave = "";

        var objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        var baseUrl = '/Inventory/api/QC/';
        var isExisting = 0;
        //$scope.loaderMore = false;   
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        $scope.HQCNO = "";
        var date = new Date();
        $scope.QCDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.GRRDate = '';//('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        //$scope.PINoList = [];
        $scope.PONoList = [];
        $scope.SPRNOList = [];
        $scope.listSupplier = [];

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        $scope.LgUser = $('#hUserID').val();
        var varSPRPOLC_ID = "";
        var varChallanInvoiceReceiptType = "";

        var varSPRPOLCType = "";
        var varSPRPOLCNo = "";
        var varCIRType = "";
        var varCIRNo = "";

        $scope.PITypeID = 5;
      
        $scope.MrrQcNo = 0;
        $scope.MrrQcID = 0;

        $scope.btnQCSaveText = "Save";
        $scope.btnQCShowText = "Show List";
        $scope.btnQCReviseText = "Update";
        

        $scope.ListQCDetails = [];
        $scope.ListQCMaster = [];

        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnQCShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnQCShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.loadQCMasterRecords(0);
            }
        }

        function loadUserCommonEntity(num) {
            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            //  console.clear();
            //  debugger
            //Coming from SideNavCrl  
            $scope.permissionPageVisibility = true;
            $scope.generateSecurityParam = {};
            $scope.generateSecurityParam.MenuID = $scope.UserCommonEntity.currentMenuID;
            $scope.generateSecurityParam.CompanyID = $scope.UserCommonEntity.loggedCompnyID;

            $scope.HeaderToken = {};
            $scope.generateSecurityParam.methodtype = 'get';
            $scope.HeaderToken.get = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'put';
            $scope.HeaderToken.put = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'post';
            $scope.HeaderToken.post = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'delete';
            $scope.HeaderToken.delete = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };


            //////Coming from SideNavCrl
            ////$scope.UserCommonEntity = {};
            //$scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
            //$scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
            //$scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
            //$scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
            //$scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
            //$scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
            //$scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
            //$scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            //$scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            //console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);




        $scope.grdTQCNo = "";
        $scope.grdTQCDate = "";
        $scope.IsQCFromSpr = false;
        $scope.IsQCFromSprLoan = false;
        $scope.grdTSupplier = "";
        $scope.grdTSPRNo = ""
        $scope.grdTGRRNo = "";


        function chkTrnsTypeFHideShow() {
            if ($scope.UserCommonEntity.currentTransactionTypeID == 19) {  //qc    GRR  from  SPR

                $scope.IsQCFromSpr = true;
                $scope.IsQCFromSprLoan = false;

                $scope.grdTQCNo = "QC No";
                $scope.grdTQCDate = "QC Date";

                $scope.grdTSupplier = "Supplier";
                $scope.grdTSPRNo = "SPR No";
                $scope.grdTGRRNo = "GRR No";

                // for master load


                $scope.PageTitle = 'Quality Control (QC) Creation';
                $scope.ListTitle = 'QC Records';
                $scope.ListTitleQCMasters = 'QC Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleQCDeatails = 'QC Information (Details)';

            }
            else if ($scope.UserCommonEntity.currentTransactionTypeID == 37) { //qc       GRR  from Loan SPR

                $scope.IsQCFromSpr = false;
                $scope.IsQCFromSprLoan = true;

                $scope.grdTQCNo = "Loan QC No";
                $scope.grdTQCDate = "Loan QC Date";

                $scope.grdTSupplier = "From Company (Supplier)";
                $scope.grdTSPRNo = "Loan Order No";
                $scope.grdTGRRNo = "Loan GRR No";

                $scope.PageTitle = 'Loan Quality Control (QC) Creation';
                $scope.ListTitle = 'Loan QC Records';
                $scope.ListTitleQCMasters = 'Loan QC Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleQCDeatails = 'Loan QC Information (Details)';
            }

            //else if ($scope.UserCommonEntity.currentTransactionTypeID == 10) {  // for  loan disburse  or  loan return receive grr
            //    $scope.IsIssueReceive = false;
            //    $scope.CommonFIssueNReturnReceive = false;
            //    $scope.IsMrr = true;
            //    $scope.CommonFLoanReceiveNMrr = true;
            //    $scope.IsLoanReceive = false;
            //    $scope.IsReturnReceive = false;

            //    $scope.grdTMrNo = "MRR No";
            //    $scope.grdTMrDate = "MRR Date";
            //    $scope.grdTSprDate = "SPR Date";
            //    $scope.grdTSprNo = "SPR No";


            //    $scope.PageTitle = 'MRR Creation';
            //    $scope.ListTitle = 'MRR Records';
            //    $scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
            //    $scope.ListTitleSampleNo = 'Sample Info';
            //    $scope.ListTitleMrrDeatails = 'MRR Information (Details)';

            //}

        }
        chkTrnsTypeFHideShow();
















        //**********---- Get GRRNo Records ----*************** //
        function loadGRRNo(isPaging) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };

            var TransTypeID = 0;
            if ($scope.IsQCFromSpr == true) {
                TransTypeID = 23;
            }
            else if ($scope.IsQCFromSprLoan == true) {
                TransTypeID = 36;
            }

            var apiRoute = baseUrl + 'GetGRRNo/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(TransTypeID) + "]";

            var listGRRNo = qcService.GetList(apiRoute, cmnParam);

            listGRRNo.then(function (response) {
                $scope.listGRRNo = response.data.objGrrNo;
            },
                function (error) {
                    console.log("Error: " + error);
                });


            //var apiRoute = baseUrl + 'GetGRRNo/';
            //var listGRRNo = qcService.getModel(apiRoute, page, pageSize, isPaging);
            //listGRRNo.then(function (response) {
            //    $scope.listGRRNo = response.data;
            //    //console.log($scope.listBuyer);
            //},
            //function (error) {
            //    console.log("Error: " + error);
            //});
        }
        loadGRRNo(0);



        // **********---- Get All Suppliers Records ----*************** //

        function loadQCByRecords(isPaging) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = baseUrl + 'GetUser/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listuser = qcService.GetList(apiRoute, cmnParam);
            listuser.then(function (response) {
                $scope.listQCBy = response.data;
                angular.forEach($scope.listQCBy, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {

                        $scope.ngmQCByList = item.UserID;
                        $("#ddlQCBy").select2("data", { id: item.UserID, text: item.UserFullName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadQCByRecords(0);

        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateNo = function () {

            var getMNo = $scope.QCCertificateNo;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            if (getMNo.trim() != "") {
                var apiRoute = baseUrl + 'ChkDuplicateNo/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(getMNo) + "]";
                var getDuplicateNo = qcService.GetList(apiRoute, cmnParam);
                getDuplicateNo.then(function (response) {

                    $scope.ChkQCCertificateNoWhnSave = "";

                    if (response.data.length > 0) {
                        $scope.ChkQCCertificateNoWhnSave = response.data[0].QCCertificateNo
                        $scope.QCCertificateNo = "";
                        Command: toastr["warning"]("QC Certificate No Already Exists.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                    Command: toastr["warning"]("Please Enter QC Certificate No.");
            }

        }

        //######## End Check Duplicate No ################//

        //**********---- Get All Suppliers Records ----*************** //

        //function loadSuppliersRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetSuppliers/';
        //    var listBuyer = qcService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
        //    listBuyer.then(function (response) {
        //        $scope.listSupplier = response.data;
        //        //console.log($scope.listBuyer);
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadSuppliersRecords(0);


        //$scope.LoadSuppliers = function () {
        //    var apiRoute = baseUrl + 'GetSuppliers/';
        //    var _Supplier = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
        //    _Supplier.then(function (response) {
        //        $scope.Suppliers = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //$scope.LoadSuppliers();

        //**********---- QC Detail from GrrNo  Changes ----***************//

        $scope.getItemDetailByGrrNo = function () {


            $scope.MrrQcID = 0;

            $scope.btnQCSaveText = "Save";

            $scope.ListQCDetails = [];
            var GrrNo = $scope.lstGRRNoList; // $("#ddlGRRNo").select2('data').text; // $scope.lstGRRNoList;
             

            angular.forEach($scope.listGRRNo, function (item) {
                if (GrrNo == item.GrrID) {

                    $scope.listCompany = [];
                    $scope.lstCompanyList = '';
                    $("#ddlCompany").select2("data", { id: '', text: '' });
                    //  if (response.data.lstQC[0].CurrencyID != null) { 
                    $scope.listCompany.push({
                        CompanyID: item.FromCompanyID, CompanyName: item.FromCompanyName
                    });

                    $scope.lstCompanyList = item.FromCompanyID;
                    $("#ddlCompany").select2("data", { id: item.FromCompanyID, text: item.FromCompanyName });
                     
                    return false;
                }
            });



            $scope.IsHiddenDetail = false;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            if (GrrNo != "") {
                var apiRoute = baseUrl + 'GetItemDetailByGrrNo/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(GrrNo) + "]";

                var ListQCDetails = qcService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ListQCDetails.then(function (response) {
                    $scope.ListQCDetails = response.data.lstQC;


                    $scope.listSupplier = [];
                    $scope.lstSupplierList = '';
                    $("#ddlSupplier").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].UserID != null) {
                        $scope.listSupplier.push({
                            UserID: response.data.lstQC[0].UserID, UserFullName: response.data.lstQC[0].UserFullName
                        });
                        $scope.lstSupplierList = response.data.lstQC[0].UserID;
                        $("#ddlSupplier").select2("data", { id: response.data.lstQC[0].UserID, text: response.data.lstQC[0].UserFullName });
                    }
                    $scope.SPRNOList = [];
                    $scope.ngmSPRNOList = '';
                    $("#ddlSPRNo").select2("data", { id: '', text: '' });
                    if (response.data.lstQC[0].RequisitionID != null) {
                        $scope.SPRNOList.push({
                            RequisitionID: response.data.lstQC[0].RequisitionID, RequisitionNo: response.data.lstQC[0].RequisitionNo
                        });
                        $scope.ngmSPRNOList = response.data.lstQC[0].RequisitionID;
                        $("#ddlSPRNo").select2("data", { id: response.data.lstQC[0].RequisitionID, text: response.data.lstQC[0].RequisitionNo });
                    }
                    $scope.PONoList = [];
                    $scope.ngmPONoList = '';
                    $("#ddlPONo").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].POID != null) {
                        $scope.PONoList.push({
                            POID: response.data.lstQC[0].POID, PONo: response.data.lstQC[0].PONo
                        });
                        $scope.ngmPONoList = response.data.lstQC[0].POID;
                        $("#ddlPONo").select2("data", { id: response.data.lstQC[0].POID, text: response.data.lstQC[0].PONo });
                    }

                    //$scope.PINoList = [];
                    //$scope.ngmPINoList = '';
                    //$("#ddlPINo").select2("data", { id: '', text: '' });

                    //if (response.data.lstQC[0].PIID != null) {
                    //    $scope.PINoList.push({
                    //        PIID: response.data.lstQC[0].PIID, PINo: response.data.lstQC[0].PINo
                    //    });

                    //    $scope.ngmPINoList = response.data.lstQC[0].PIID;
                    //    $("#ddlPINo").select2("data", { id: response.data.lstQC[0].PIID, text: response.data.lstQC[0].PINo });
                    //}

                    $scope.LCNO = response.data.lstQC[0].LCNO;

                    $scope.RefChallanNo = response.data.lstQC[0].RefCHNo;



                    //var grrdate = response.data.lstQC[0].GrrDate;

                    //  console.log(response.data.lstQC[0].GRRDate);
                    $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);



                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        //**********----delete  Record from ListQCDetails----***************//

        $scope.deleteRow = function (index) {
            // $scope.ListQCDetails.splice($scope.ListQCDetails.indexOf($scope.data), 1);
            $scope.ListQCDetails.splice(index, 1);
            // $scope.showDtgrid = $scope.ListPIDetails.length;
        };

        //**********----Create Calculation----***************//
        $scope.calculation = function (dataModel) {
            $scope.ListQCDetails1 = [];
            angular.forEach($scope.ListQCDetails, function (item) {

                var QcValidQty = parseFloat(parseFloat(item.QcValidQty))
                var passNrejectQty = parseFloat(parseFloat(item.PassQty) + parseFloat(item.RejectQty)).toFixed(2);
                if (QcValidQty >= passNrejectQty) {
                    $scope.ListQCDetails1.push({
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, LotID: item.LotID,
                        BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, GrrQty: item.GrrQty, PassQty: item.PassQty, RejectQty: item.RejectQty, QcRemainingQty: item.QcRemainingQty, QcValidQty: item.QcValidQty, Remarks: item.Remarks,

                        AdditionalPassQty: item.AdditionalPassQty, AdditionalRejectQty: item.AdditionalRejectQty, QcRemainingAdditionalQty: item.QcRemainingAdditionalQty, QcValidAdditionalQty: item.QcValidAdditionalQty

                    });
                    $scope.ListQCDetails = $scope.ListQCDetails1;
                }
                else if (QcValidQty < passNrejectQty) {
                        Command: toastr["warning"]("GrrQty  Must Not Less than Sum of PassedQty and RejectedQty !!!!");

                    $scope.ListQCDetails1.push({
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, LotID: item.LotID,
                        BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, GrrQty: item.GrrQty, PassQty: 0.00, RejectQty: 0.00, QcRemainingQty: item.QcRemainingQty, QcValidQty: item.QcValidQty, Remarks: item.Remarks,

                        AdditionalPassQty: item.AdditionalPassQty, AdditionalRejectQty: item.AdditionalRejectQty, QcRemainingAdditionalQty: item.QcRemainingAdditionalQty, QcValidAdditionalQty: item.QcValidAdditionalQty

                    });
                    $scope.ListQCDetails = $scope.ListQCDetails1;
                }


            });
        }


        //**********----Create Calculation----***************//
        $scope.calculationAdditional = function (dataModel) {
            $scope.ListQCDetails1 = [];
            angular.forEach($scope.ListQCDetails, function (item) {

                var QcValidQtyAdditional = parseFloat(parseFloat(item.QcValidAdditionalQty))

                var passNrejectQtyAdditional = parseFloat(parseFloat(item.AdditionalPassQty) + parseFloat(item.AdditionalRejectQty)).toFixed(2);

                if (QcValidQtyAdditional >= passNrejectQtyAdditional) {
                    $scope.ListQCDetails1.push({
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, LotID: item.LotID,
                        BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, GrrQty: item.GrrQty, PassQty: item.PassQty, RejectQty: item.RejectQty, QcRemainingQty: item.QcRemainingQty, QcValidQty: item.QcValidQty, Remarks: item.Remarks,

                        AdditionalPassQty: item.AdditionalPassQty, AdditionalRejectQty: item.AdditionalRejectQty, QcRemainingAdditionalQty: item.QcRemainingAdditionalQty, QcValidAdditionalQty: item.QcValidAdditionalQty

                    });
                    $scope.ListQCDetails = $scope.ListQCDetails1;
                }
                else if (QcValidQtyAdditional < passNrejectQtyAdditional) {
                        Command: toastr["warning"]("Additional Grr Qty  Must Not Less than Sum of Additional Passed Qty and Additional  Rejected Qty !!!!");

                    $scope.ListQCDetails1.push({
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, LotID: item.LotID,
                        BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, GrrQty: item.GrrQty, PassQty: item.PassQty, RejectQty: item.RejectQty, QcRemainingQty: item.QcRemainingQty, QcValidQty: item.QcValidQty, Remarks: item.Remarks,

                        AdditionalPassQty: 0.00, AdditionalRejectQty: 0.00, QcRemainingAdditionalQty: item.QcRemainingAdditionalQty, QcValidAdditionalQty: item.QcValidAdditionalQty

                    });
                    $scope.ListQCDetails = $scope.ListQCDetails1;
                }


            });
        }


        //  Pagination QC List Master
        //  function for call  when clear or save  or page load not click n open popup

        // function callPaginationMthd() {

        //  Pagination QC List Master
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                this.pageNumber = 1;
                if (this.ddlpageSize == "All") {
                    this.ddlpageSize = $scope.pagination.totalItems;
                }
                else {
                    this.pageSize = this.ddlpageSize;
                }

                $scope.loadQCMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadQCMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadQCMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadQCMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadQCMasterRecords(1);
                }
            }
        };
        //  }

        //**********---- Get All QC Master Record ----*************** //
        $scope.loadQCMasterRecords = function(isPaging) {

            // callPaginationMthd();

            $scope.gridOptionsMasterQC.enableFiltering = true;
            $scope.gridOptionsMasterQC.enableGridMenu = true;

            

            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading


            $scope.loaderMoreQCMaster = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsMasterQC = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [
                    { name: "MrrQcID", displayName: "MrrQcID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPLID", displayName: "SPLID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CIRID", displayName: "CIRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPLTypeID", displayName: "SPLTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CIRTypeID", displayName: "CIRTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierID", displayName: "SupplierID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StatusID", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPLDate", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CIRDate", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "MrrQcNo", displayName: $scope.grdTQCNo, title: $scope.grdTQCNo, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "QCCertificateNo", displayName: "Manual QCNo", title: "Manual QCNo", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrQcDate", displayName: $scope.grdTQCDate, cellFilter: 'date:"dd-MM-yyyy"', width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrNo", displayName: $scope.grdTGRRNo, title: $scope.grdTGRRNo, width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                   // { name: "SPLTypeName", displayName: "SPL Type", title: "SPLTypeName", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                   //  { name: "SPLNoName", displayName: "SPL No", title: "SPL No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                   // { name: "CIRTypeName", displayName: "CIR Type", title: "CIRTypeName", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                   // { name: "CIRNoName", displayName: "CIR No", title: "CIRTypeName", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierName", displayName:  $scope.grdTSupplier, title:  $scope.grdTSupplier, width: '35%', headerCellClass: $scope.highlightFilteredHeader },


                



                    {
                        name: 'Action',
                        displayName: "Action",

                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,

                        width: '10%',
                        pinnedRight: true,
                        headerCellClass: $scope.highlightFilteredHeader,

                        //cellTemplate: '<span class="label label-success label-mini">' +
                        //           '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                        //                 '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                        //             '</a>' +
                        //         '</span>' +
                        //         '<span class="label label-success label-mini">' +
                        //           '<a href="" title="Edit" ng-click="grid.appScope.loadQCMasterDetailsByQC(row.entity)">' +
                        //             '<i class="icon-edit"></i> Edit' +
                        //           '</a>' +
                        //           '</span>'

                        cellTemplate: '<span class="label label-success label-mini">' +
                                        '<a href="" title="Edit" ng-click="grid.appScope.loadQCMasterDetailsByQC(row.entity)">' +
                                        '<i class="icon-edit"></i> Edit' +
                                         '</a>' +
                                        '</span>'
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                },
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'ItemSample.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
                exporterPdfFooter: function (currentPage, pageCount) {
                    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
                },
                exporterPdfCustomFormatter: function (docDefinition) {
                    docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
                    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                    return docDefinition;
                },
                exporterPdfOrientation: 'portrait',
                exporterPdfPageSize: 'LETTER',
                exporterPdfMaxGridWidth: 500,
                exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            };

            var apiRoute = baseUrl + 'GetQCMasterList/'; // by viewModel

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listQCMaster = qcService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listQCMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMasterQC.data = response.data.objVmQC;
                $scope.loaderMoreQCMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.loadQCMasterRecords(0);

        //**********----Load QC MasterForm and QCDetails List By select QC Master List ----***************//
        $scope.loadQCMasterDetailsByQC = function (dataModel) {
            //debugger;

            $("#ddlGRRNo").prop("disabled", true);

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnQCShowText = "Show List";
            $scope.IsHidden = true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            //

            $scope.btnQCSaveText = "Update";
            // $scope.listQCMaster = [];
            var mrrQCID = dataModel.MrrQcID;


            $scope.MrrQcID = dataModel.MrrQcID;
            $scope.HQCNO = dataModel.MrrQcNo;
            $scope.MrrQcNo = dataModel.MrrQcNo;
            $scope.QCDate = conversion.getDateToString(dataModel.MrrQcDate);

            // for ddlGRRNo
            //$scope.lstGRRNoList = dataModel.GrrID;
            //$("#ddlGRRNo").select2("data", { id: dataModel.GrrID, text: dataModel.GrrNo });


            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });
            if (dataModel.FromCompanyID != null) {
                $scope.listCompany.push({
                    CompanyID: dataModel.FromCompanyID, CompanyName: dataModel.FromCompanyName
                });

                $scope.lstCompanyList = dataModel.FromCompanyID;
                $("#ddlCompany").select2("data", { id: dataModel.FromCompanyID, text: dataModel.FromCompanyName });
            }



            $scope.listGRRNo = [];
            $scope.lstGRRNoList = '';
            $("#ddlGRRNo").select2("data", { id: '', text: '' });

            if (dataModel.GrrID != null) {
                $scope.listGRRNo.push({
                    GrrID: dataModel.GrrID, GrrNo: dataModel.GrrNo
                });
                $scope.lstGRRNoList = dataModel.GrrID;
                $("#ddlGRRNo").select2("data", { id: dataModel.GrrID, text: dataModel.GrrNo });
            }


            $scope.listSupplier = [];
            $scope.lstSupplierList = '';
            $("#ddlSupplier").select2("data", { id: '', text: '' });

            if (dataModel.SupplierID != null) {
                $scope.listSupplier.push({
                    UserID: dataModel.SupplierID, UserFullName: dataModel.SupplierName
                });
                $scope.lstSupplierList = dataModel.SupplierID;
                $("#ddlSupplier").select2("data", { id: dataModel.SupplierID, text: dataModel.SupplierName });
            }


            $scope.SPRNOList = [];
            $scope.ngmSPRNOList = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '' });
            if (dataModel.RequisitionID != null) {
                $scope.SPRNOList.push({
                    RequisitionID: dataModel.RequisitionID, RequisitionNo: dataModel.RequisitionNo
                });
                $scope.ngmSPRNOList = dataModel.RequisitionID;
                $("#ddlSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });
            }
            $scope.PONoList = [];
            $scope.ngmPONoList = '';
            $("#ddlPONo").select2("data", { id: '', text: '' });

            if (dataModel.POID != null) {
                $scope.PONoList.push({
                    POID: dataModel.POID, PONo: dataModel.PONo
                });

                $scope.ngmPONoList = dataModel.POID;
                $("#ddlPONo").select2("data", { id: dataModel.POID, text: dataModel.PONo });
            }

            if (dataModel.UserID != null) {
                $scope.listQCBy.push({
                    UserID: dataModel.UserID, UserFullName: dataModel.UserFullName
                });

                $scope.ngmQCByList = dataModel.UserID;
                $("#ddlQCBy").select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });
            }

            $scope.LCNO = dataModel.LCNO;
            $scope.RefChallanNo = dataModel.RefCHNo;
            $scope.GRRDate = conversion.getDateToString(dataModel.GrrDate);

            $scope.Remarks = dataModel.Remarks;
            $scope.QCCertificateNo = dataModel.QCCertificateNo;
            $scope.Description = dataModel.Description;


            $scope.bool = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = baseUrl + 'GetQCDetailsListByQCMasterID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(mrrQCID) + "]";

            var ListQCDetails = qcService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            ListQCDetails.then(function (response) {
                $scope.ListQCDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //**********----Save and Update QCMaster and QCDetail  Records----***************//
        $scope.save = function () {

            // var getMNo = $scope.QCCertificateNo;

            if ($scope.ChkQCCertificateNoWhnSave == "") {

                $("#save").prop("disabled", true);

                var NewStringToDate = conversion.getStringToDate($scope.QCDate);

                var HedarTokenPostPut = $scope.MrrQcID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

                var qcMaster = {
                    MrrQcID: $scope.MrrQcID,
                    MrrQcNo: $scope.MrrQcNo,
                    MrrQcDate: NewStringToDate,
                    RequisitionID: $scope.ngmSPRNOList,
                    QCCertificateNo: $scope.QCCertificateNo,
                    DocURL: '',
                    Description: $scope.Description,

                    // SPLID: $scope.lstSPR_PO_LC_No,
                    // CIRID: $scope.lstChallan_Invoice_Receipt_NoList,
                    // SPLTypeID: $scope.lstSPR_PO_LCList,
                    // CIRTypeID: $scope.lstChallan_Invoice_ReceiptList,
                    GrrID: $scope.lstGRRNoList,
                    GrrNo: $("#ddlGRRNo").select2('data').text,
                    // PIID: $scope.ngmPINoList,
                    POID: $scope.ngmPONoList,
                    PONo: $("#ddlPONo").select2('data').text,
                    Remarks: $scope.Remarks,
                    SupplierID: $scope.lstSupplierList,

                    TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,

                    //  EmployeeID: LoggedUserID,

                    StatusID: 1,
                    StatusBy: LoggedUserID,
                    StatusDate: NewStringToDate,
                    CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                    DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                    CreateBy: $scope.ngmQCByList
                };


                //// ######################################    start for file

                //var fileList = [];
                //angular.forEach($scope.files, function (item) {
                //    this.push(item.name);
                //}, fileList);

                //if (fileList.length == 0) {
                //    $("#save").prop("disabled", false);
                //    Command: toastr["warning"]("Please attach QC document.");
                //    return;
                //}

                //// ##########################################    end for file


                var menuID = $scope.UserCommonEntity.currentMenuID;
                var qcDetail = $scope.ListQCDetails;
                var chkPassQty = 1;
                //  var chkBuyerStyle = 1;
                angular.forEach($scope.ListQCDetails, function (item) {

                    if (item.PassQty <= 0) {
                        chkPassQty = 0;
                    }
                });

                if ($scope.ListQCDetails.length > 0) {

                    if (chkPassQty == 1) {
                        var apiRoute = baseUrl + 'SaveUpdateQCMasterNdetails/';

                        // var cmnParam = "[" + JSON.stringify(qcMaster) + "," + JSON.stringify(qcDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(fileList) + "]";

                        var cmnParam = "[" + JSON.stringify(qcMaster) + "," + JSON.stringify(qcDetail) + "," + JSON.stringify(menuID) + "]";

                        var QCMasterNdetailsCreateUpdate = qcService.GetList(apiRoute, cmnParam, HedarTokenPostPut);

                        QCMasterNdetailsCreateUpdate.then(function (response) {
                            var result = 0;
                            if (response.data != "") {
                                $scope.HQCNO = response.data;


                                ///////###################################     start file upload/////////////

                                //var data = new FormData();

                                //for (var i in $scope.files) {
                                //    data.append("uploadedFile", $scope.files[i]);
                                //}
                                //data.append("uploadedFile", response.data);
                                //// ADD LISTENERS.
                                //var objXhr = new XMLHttpRequest();
                                ////objXhr.addEventListener("progress", updateProgress, false);
                                ////objXhr.addEventListener("load", transferComplete, false);


                                //var apiRoute = baseUrl + 'UploadFiles/';
                                //objXhr.open("POST", apiRoute);
                                //objXhr.send(data);
                                ////  debugger;
                                //document.getElementById('file').value = '';
                                //$scope.files = [];

                                ///////////// ###################################   end file upload /////////////////


                                // alert('Saved Successfully.');
                                Command: toastr["success"]("Save  Successfully!!!!");
                                $scope.clear();
                                // result = 1;
                            }
                            else if (response.data == "") {
                                    Command: toastr["warning"]("Save Not Successfull!!!!");
                                $("#save").prop("disabled", false);
                            }
                            // 
                            // ShowCustomToastrMessageResult(result);
                        },
                    function (error) {
                        // console.log("Error: " + error);
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                    });
                    }
                    else if (chkPassQty == 0) {
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Passed Quantity Must Not Zero Or Empty !!!!");

                    }
                }
                else if ($scope.ListQCDetails.length <= 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("QC Detail Must Not Empty!!!!");
                }
            }

            else if ($scope.ChkQCCertificateNoWhnSave != "") {

                    Command: toastr["warning"]("QC Certificate No Already Exists.");

                // Command: toastr["warning"]("Please Enter QC Certificate No.");
            }

        };



        $scope.files = [];
        $scope.getFileDetails = function (e) {
            $scope.$apply(function () {

                /// file validation //////////////
                $scope.file = e.files[0];

                if ($scope.file.size > 200000000) {
                    // alert('file size should not be greater than 200 MB');
                    Command: toastr["warning"]("file size should not be greater than 200 MB!!!!");
                    return;
                }

                var allowed = ["jpeg", "png", "gif", "jpg", "pdf"];
                var found = false;
                //var img;
                //img = new Image();
                allowed.forEach(function (extension) {
                    if ($scope.file.type.match('image/' + extension)) {
                        found = true;
                    }
                    if ($scope.file.type.match('application/' + 'pdf')) {
                        found = true;
                    }
                    //if ($scope.file.type.match('application/' + 'msword')) {
                    //    found = true;
                    //}
                });
                if (!found) {
                    //alert('file type should be .jpeg, .png, .jpg, .gif, .pdf, .doc');
                    Command: toastr["warning"]("file type should be .jpeg, .png, .jpg, .gif, .pdf!!!!");
                    return;
                }

                //// file validation /////////////


                // STORE THE FILE OBJECT IN AN ARRAY.
                for (var i = 0; i < e.files.length; i++) {
                    $scope.files.push(e.files[i])
                }
            });
        };

        $scope.getFileInfo = function (dataModel) {
            $scope.ListFileDetails = [];
            var id = dataModel.MrrQcID;
            // var apiRoute = baseUrl + 'GetFileDetailsById/' + id;
            var TransTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id + '/' + TransTypeID;

            var ListFileDetails = qcService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }





        //**********----Reset Record----***************//
        $scope.clear = function () {

            $("#ddlGRRNo").prop("disabled", false);


            $scope.showDtgrid = 0;//$scope.ListPIDetails.length;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnQCShowText = "Show List";
            $scope.btnQCSaveText = "Save";
            $scope.listQCMaster = [];
            $scope.ListQCDetails = [];
            $scope.bool = true;
            $scope.QCID = '0';

            $scope.MrrQcNo = 0;
            $scope.MrrQcID = 0;


            //$scope.lstSPR_PO_LC_No = "";
            //$scope.lstChallan_Invoice_ReceiptList = "";
            //$scope.Challan_Invoice_Receipt_Date = "";
            $scope.lstGRRNoList = "";
            //$scope.lstSPR_PO_LCList = "";
            //$scope.SPR_PO_LC_Date = "";
            //$scope.lstChallan_Invoice_Receipt_NoList = "";
            $scope.lstSupplierList = "";


            //// for ddlSPR_PO_LC  type
            //$scope.lstSPR_PO_LCList = "";
            //$("#ddlSPR_PO_LC").select2("data", { id: "", text: "--Slect SPR/PO/LC No--" });

            //// for ddlSPR_PO_LC_No 
            //$scope.lstSPR_PO_LC_No = "";
            //$("#ddlSPR_PO_LC_No").select2("data", { id: "", text: "--Slect SPR/PO/LC No--" });

            //// for ddlChallan_Invoice_Receipt type
            //$scope.lstChallan_Invoice_ReceiptList = "";
            //$("#ddlChallan_Invoice_Receipt").select2("data", { id: "", text: "--Slect Challan/Invoice/Receipt --" });

            //// for ddlChallan_Invoice_Receipt_No   
            //$scope.lstChallan_Invoice_Receipt_NoList = "";
            //$("#ddlChallan_Invoice_Receipt_No").select2("data", { id: "", text: "--Slect Challan/Invoice/Receipt No--" });

            chkTrnsTypeFHideShow();

            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });
          

            $scope.SPRNOList = [];
            $scope.ngmSPRNOList = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '' });

            $scope.PONoList = [];
            $scope.ngmPONoList = '';
            $("#ddlPONo").select2("data", { id: '', text: '' });


            //$scope.PINoList = [];
            //$scope.ngmPINoList = '';
            //$("#ddlPINo").select2("data", { id: '', text: '' });


            $scope.LCNO = '';

            $scope.RefChallanNo = '';

            $scope.GRRDate = '';
            $scope.Remarks = '';
            $scope.Description = '';
            $scope.QCCertificateNo = '';

            // for ddlSupplier  
            $scope.lstSupplierList = "";
            $("#ddlSupplier").select2("data", { id: "", text: "" });

            // for ddlGRRNo
            $scope.lstGRRNoList = ""
            $("#ddlGRRNo").select2("data", { id: "", text: "--Select GRR No--" });

            // loadChallanInvoiceReceiptTypes(0);
            loadGRRNo(0);
            //loadSuppliersRecords(0);
            //loadSPRPOLCtypes(0); 
            $scope.loadQCMasterRecords(1);

            var date = new Date();
            $scope.QCDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        };

    }]);



