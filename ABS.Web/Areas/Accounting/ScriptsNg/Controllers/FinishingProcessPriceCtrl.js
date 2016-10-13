/**
* MRRCtrl.js
*/


app.controller('finishingProcessPriceCtrl', ['$scope', 'finishingProcessPriceService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, finishingProcessPriceService, conversion, $filter, $localStorage, uiGridConstants) {

        $scope.gridOptionsFppMaster = [];

        $scope.gridOptionslistItemMaster = [];

        $scope.gridFinishingPriceChange = [];

        $scope.ListFppDetails = [];

        var objcmnParam = {};
        $scope.IsActiveChkBox = true;

        function loadUserCommonEntity(num) {

            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);

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


            ////Coming from SideNavCrl
            //$scope.UserCommonEntity = {};
            $scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
            $scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
            $scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
            $scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
            $scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
            $scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
            $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID; // for transactiontyeid not duplicate 
            $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);


        $scope.PageTitle = 'Finishing Process Price  Creation';
        $scope.ListTitle = 'Finishing Process Price Records';
        $scope.ListTitleMasters = 'Finishing Process Price  Information (Masters)';
        $scope.ListTitleDeatails = 'Finishing Process Price Information (Details)';


        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;



        var baseUrl = '/Accounting/api/FinishingProcessPrice/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.PriceDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.ProcessPriceID = "0";


        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.btnReviseText = "Update";

        $scope.ListProcessPriceDetails = [];
        $scope.ListProcessPriceMaster = [];

        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;


        //$scope.ShowHide = function () {
        //    $scope.IsHidden = $scope.IsHidden == true ? false : true;
        //    $scope.IsHiddenDetail = true;
        //    if ($scope.IsHidden == true) {
        //        $scope.btnShowText = "Show List";
        //        $scope.IsShow = true;

        //        $scope.IsCreateIcon = false;
        //        $scope.IsListIcon = true;
        //    }
        //    else {
        //        $scope.btnShowText = "Create";
        //        $scope.IsShow = false;
        //        $scope.IsHidden = false;

        //        $scope.IsCreateIcon = true;
        //        $scope.IsListIcon = false;
        //        $scope.loadFppRecords(0);
        //    }
        //}


        function loadFinishProcessRecords(isPaging) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = baseUrl + 'GetFinishingProcess/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var FinishProcess = finishingProcessPriceService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            FinishProcess.then(function (response) {
                $scope.listFinishProcess = response.data.lstFinishingProcess;

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        loadFinishProcessRecords(0);



        ////**********----Pagination gridFinishingPriceChange  ----***************
        //$scope.paginationItemMaster = {
        //    paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
        //    ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

        //    getTotalPagesItemMaster: function () {
        //        return Math.ceil(this.totalItems / this.pageSize);
        //    },
        //    pageSizeChange: function () {
        //        if (this.ddlpageSize == "All")
        //            this.pageSize = $scope.paginationItemMaster.totalItems;
        //        else
        //            this.pageSize = this.ddlpageSize

        //        this.pageNumber = 1
        //        $scope.loadFinishingPriceChangeRecords(1);
        //    },
        //    firstPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber = 1
        //            $scope.loadFinishingPriceChangeRecords(1);
        //        }
        //    },
        //    nextPage: function () {
        //        if (this.pageNumber < this.getTotalPagesItemMaster()) {
        //            this.pageNumber++;
        //            $scope.loadFinishingPriceChangeRecords(1);
        //        }
        //    },
        //    previousPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber--;
        //            $scope.loadFinishingPriceChangeRecords(1);
        //        }
        //    },
        //    lastPage: function () {
        //        if (this.pageNumber >= 1) {
        //            this.pageNumber = this.getTotalPagesItemMaster();
        //            $scope.loadFinishingPriceChangeRecords(1);
        //        }
        //    }
        //};

        //**********----Get All Item Record by  select Sample No----***************//
        $scope.getItemDetailByFinishProcess = function (isPaging) {

            $scope.IsHiddenDetail = true;

            var finishProcessID = $scope.ngmFinishProcess;
            if (finishProcessID > 0) {

                var apiRoute = baseUrl + 'GetFinPricChngeGrdByFProcessID/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(finishProcessID) + "]";
                var listItem = finishingProcessPriceService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                listItem.then(function (response) {

                    $scope.IsHiddenDetail = false;
                    $scope.ListFppDetails = response.data.lstFinishingPriceProcess;

                },
                function (error) {
                    $scope.IsHiddenDetail = true;
                    console.log("Error: " + error);
                });

            }
            else {
                $scope.IsHiddenDetail = true;
                Command: toastr["warning"]("Select Finish Process !!!!");
            }
        };

        //$scope.loadFinishingPriceChangeRecords = function (isPaging)
        //{


        //    var finishProcessID = $scope.ngmFinishProcess;
        //    if (finishProcessID > 0) {

        //        $scope.gridFinishingPriceChange.enableFiltering = true;
        //        $scope.gridFinishingPriceChange.enableGridMenu = true;

        //        // For Loading
        //        if (isPaging == 0)
        //            $scope.paginationItemMaster.pageNumber = 1;
        //        // For Loading
        //        $scope.loaderMoreItemMaster = true;
        //        $scope.lblMessageItemMaster = 'loading please wait....!';
        //        $scope.result = "color-red";

        //        //Ui Grid
        //        objcmnParam = {
        //            pageNumber: (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize),
        //            pageSize: $scope.paginationItemMaster.pageSize,
        //            IsPaging: isPaging,
        //            loggeduser: $scope.UserCommonEntity.loggedUserID,
        //            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //            menuId: $scope.UserCommonEntity.currentModuleID,
        //            tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //        };


        //        $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
        //            if (col.filters[0].term) {
        //                return 'header-filtered';
        //            } else {
        //                return '';
        //            }
        //        };

        //        // $scope.IsItemTypeFinishedGoods
        //        // $scope.IsItemTypeRMOthers
        //        $scope.gridFinishingPriceChange = {
        //            columnDefs: [

        //                { name: "FinishingProcessName", displayName: "Finishing Process", title: "Finishing Process", width: '15%', headerCellClass: $scope.highlightFilteredHeader },

        //                { name: "ProcessPriceNo", displayName: "Process Price No", title: "Process Price No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

        //                { name: "Price", displayName: "Price", title: "Price", cellFilter: 'number: 2', width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "PriceDate", displayName: "Price Date", title: "Price Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //                { name: "CurrencyName", displayName: "Currency", title: "Currency", width: '15%', headerCellClass: $scope.highlightFilteredHeader },

        //                { name: "Remarks", displayName: "Remarks", title: "Remarks", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

        //                {
        //                    name: "IsActive", displayName: "Is Active", title: "Is Active", width: '5%', headerCellClass: $scope.highlightFilteredHeader,

        //                    cellTemplate: '<input type="checkbox" ng-model="row.entity.IsActive" ng-readonly="true" ng-disable="true" >'
        //                }

        //            ],
        //            onRegisterApi: function (gridApi) {
        //                $scope.gridApi = gridApi;
        //            },
        //            enableFiltering: true,
        //            enableGridMenu: true,
        //            enableSelectAll: true,
        //            exporterCsvFilename: 'ItemSample.csv',
        //            exporterPdfDefaultStyle: { fontSize: 9 },
        //            exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        //            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        //            exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
        //            exporterPdfFooter: function (currentPage, pageCount) {
        //                return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //            },
        //            exporterPdfCustomFormatter: function (docDefinition) {
        //                docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
        //                docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //                return docDefinition;
        //            },
        //            exporterPdfOrientation: 'portrait',
        //            exporterPdfPageSize: 'LETTER',
        //            exporterPdfMaxGridWidth: 500,
        //            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //        };

        //        // $scope.listItemMaster = [];
        //        //var groupID = $scope.lstSampleNoList;
        //        //if (groupID > 0) {
        //        var apiRoute = baseUrl + 'GetFinPricChngeGrdByFProcessID/';
        //        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(finishProcessID) + "]";
        //        var listItem = finishingProcessPriceService.GetList(apiRoute, cmnParam);
        //        listItem.then(function (response) {
        //            //$scope.listItemMaster = response.data;
        //            $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
        //            $scope.gridFinishingPriceChange.data = response.data.lstFinishingPriceProcess;
        //            $scope.loaderMoreItemMaster = false;
        //        },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });

        //    }
        //    else {
        //        Command: toastr["warning"]("Select Finish Process !!!!");
        //    }
        //}


        // $scope.loadFinishingPriceChangeRecords(1);




        //function loadRecords_OrganogramDept(isPaging) {
        //    var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
        //    var listOrganogramsDept = finishingProcessPriceService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
        //    listOrganogramsDept.then(function (response) {
        //        $scope.listOrganogramsDepartment = response.data;

        //        angular.forEach($scope.listOrganogramsDepartment, function (item) {
        //            if (item.OrganogramID == $scope.UserCommonEntity.loggedUserDepartmentID) {
        //                $scope.Dept = item.OrganogramID;
        //                $('#ddlDept').select2("data", { id: item.OrganogramID, text: item.OrganogramName });

        //                return false;
        //            }
        //        });

        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadRecords_OrganogramDept(0);

        //function loadIssueRecords(isPaging) {

        //    if ($scope.TrnsTypeFLoadDDLIssue > 0) {
        //        objcmnParam = {
        //            pageNumber: page,
        //            pageSize: pageSize,
        //            IsPaging: isPaging,
        //            loggeduser: $scope.UserCommonEntity.loggedUserID,
        //            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //            menuId: $scope.UserCommonEntity.currentMenuID,
        //            tTypeId: $scope.TrnsTypeFLoadDDLIssue
        //        };

        //        var apiRoute = baseUrl + 'GetIssueNo/';
        //        var Issues = finishingProcessPriceService.GetIssueNo(apiRoute, objcmnParam);
        //        Issues.then(function (response) {
        //            $scope.listIssueNo = response.data.lstIssue;


        //            //console.log($scope.listBuyer);
        //        },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //    }
        //}
        //loadIssueRecords(0);





        //**********---- QC Detail from QCNo  Changes ----***************//

        //$scope.getItemDetailByQCNo = function () {

        //    $scope.MrrID = "0";
        //    $scope.btnMrrSaveText = "Save";

        //    $scope.ListMrrDetails = [];
        //    var qcNo = $scope.lstQCNoList;
        //    $scope.IsMrr = true;
        //    $scope.IsIssueReceive = false;
        //    $scope.IsReturnReceive = false;
        //    $scope.CommonFIssueNReturnReceive = false;

        //    $scope.IsHiddenDetail = false;

        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };


        //    if (qcNo != "") {
        //        var apiRoute = baseUrl + 'GetDetailInfoByQCID/';

        //        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(qcNo) + "]";

        //        var QCDetails = finishingProcessPriceService.GetList(apiRoute, cmnParam);;
        //        QCDetails.then(function (response) {
        //            $scope.ListMrrDetails = response.data.lstQC;
        //            $scope.QCCertificateNo = response.data.lstQC[0].QCCertificateNo;


        //            $scope.listSupplier = [];
        //            $scope.lstSupplierList = '';
        //            $("#ddlSupplier").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].SupplierID != null) {
        //                $scope.listSupplier.push({
        //                    SupplierID: response.data.lstQC[0].SupplierID, SupplierName: response.data.lstQC[0].SupplierName
        //                });

        //                $scope.lstSupplierList = response.data.lstQC[0].SupplierID;
        //                $("#ddlSupplier").select2("data", { id: response.data.lstQC[0].SupplierID, text: response.data.lstQC[0].SupplierName });
        //            }

        //            $scope.listSprNo = [];
        //            $scope.SprNo = '';
        //            $("#txtSPRNo").select2("data", { id: '', text: '' });
        //            if (response.data.lstQC[0].RequisitionID != null) {
        //                $scope.listSprNo.push({
        //                    RequisitionID: response.data.lstQC[0].RequisitionID, SprNo: response.data.lstQC[0].RequisitionNo
        //                });

        //                $scope.SprNo = response.data.lstQC[0].RequisitionID;
        //                $("#txtSPRNo").select2("data", { id: response.data.lstQC[0].RequisitionID, text: response.data.lstQC[0].RequisitionNo });
        //            }

        //            $scope.listCurrency = [];
        //            $scope.lstCurrencyList = '';
        //            $("#ddlCurrency").select2("data", { id: '', text: '' });
        //            if (response.data.lstQC[0].CurrencyID != null) {
        //                $scope.listCurrency.push({
        //                    Id: response.data.lstQC[0].CurrencyID, CurrencyName: response.data.lstQC[0].CurrencyName
        //                });

        //                $scope.lstCurrencyList = response.data.lstQC[0].CurrencyID;
        //                $("#ddlCurrency").select2("data", { id: response.data.lstQC[0].CurrencyID, text: response.data.lstQC[0].CurrencyName });
        //            }


        //            $scope.listPONo = [];
        //            $scope.PONo = '';
        //            $("#txtPONo").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].POID != null) {
        //                $scope.listPONo.push({
        //                    POID: response.data.lstQC[0].POID, PONo: response.data.lstQC[0].PONo
        //                });

        //                $scope.PONo = response.data.lstQC[0].POID;
        //                $("#txtPONo").select2("data", { id: response.data.lstQC[0].POID, text: response.data.lstQC[0].PONo });
        //            }



        //            $scope.listGRRNo = [];
        //            $scope.lstGRRNoList = '';
        //            $("#ddlGRRNo").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].GrrID != null) {
        //                $scope.listGRRNo.push({
        //                    GrrID: response.data.lstQC[0].GrrID, GrrNo: response.data.lstQC[0].GrrNo
        //                });
        //                $scope.lstGRRNoList = response.data.lstQC[0].GrrID;
        //                $("#ddlGRRNo").select2("data", { id: response.data.lstQC[0].GrrID, text: response.data.lstQC[0].GrrNo });
        //            }


        //            $scope.LCNO = response.data.lstQC[0].LCNO;

        //            $scope.RefChallanNo = response.data.lstQC[0].RefCHNo;


        //            $scope.SPRDate = response.data.lstQC[0].SPRDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].SPRDate);
        //            $scope.PODate = response.data.lstQC[0].PODate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PODate);
        //            // $scope.PIDate = response.data.lstQC[0].PIDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PIDate);
        //            $scope.QCDate = response.data.lstQC[0].MrrQcDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].MrrQcDate);
        //            $scope.GRRDate = response.data.lstQC[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].GrrDate);


        //        },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //    }
        //}



        //**********---- Start Add details Modal ----*************** //









        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = '/Inventory/api/GRR/GetCurrency/';
            var listCurrency = finishingProcessPriceService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);



        ////  function callPaginationMthd() {
        ////**********----Pagination----***************
        //$scope.pagination = {
        //    paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
        //    ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

        //    getTotalPages: function () {
        //        return Math.ceil(this.totalItems / this.pageSize);
        //    },
        //    pageSizeChange: function () {
        //        if (this.ddlpageSize == "All")
        //            this.pageSize = $scope.pagination.totalItems;
        //        else
        //            this.pageSize = this.ddlpageSize

        //        this.pageNumber = 1
        //        $scope.loadFppRecords(1);
        //    },
        //    firstPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber = 1
        //            $scope.loadFppRecords(1);
        //        }
        //    },
        //    nextPage: function () {
        //        if (this.pageNumber < this.getTotalPages()) {
        //            this.pageNumber++;
        //            $scope.loadFppRecords(1);
        //        }
        //    },
        //    previousPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber--;
        //            $scope.loadFppRecords(1);
        //        }
        //    },
        //    lastPage: function () {
        //        if (this.pageNumber >= 1) {
        //            this.pageNumber = this.getTotalPages();
        //            $scope.loadFppRecords(1);
        //        }
        //    }
        //};
        ////  }


        ////**********----Get All Fpp Master Records----***************
        //$scope.loadFppRecords = function (isPaging) {

        //    $scope.gridOptionsFppMaster.enableFiltering = true;
        //    $scope.gridOptionsFppMaster.enableGridMenu = true;

        //     //For Loading
        //    if (isPaging == 0)
        //        $scope.pagination.pageNumber = 1;

        //    // For Loading
        //    $scope.loaderMoreMrrMaster = true;
        //    $scope.lblMessageForMrrMaster = 'loading please wait....!';
        //    $scope.result = "color-red";

        //    //Ui Grid
        //    objcmnParam = {
        //        pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
        //        pageSize: $scope.pagination.pageSize,
        //        IsPaging: $scope.pagination.pageNumber == 1 ? 0 : 1,
        //        loggeduser: LoggedUserID,
        //        loggedCompany: LoggedCompanyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };


        //    $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
        //        if (col.filters[0].term) {
        //            return 'header-filtered';
        //        } else {
        //            return '';
        //        }
        //    };

        //    $scope.gridOptionsFppMaster = {
        //        useExternalPagination: true,
        //        useExternalSorting: true,

        //        enableFiltering: true,
        //        enableRowSelection: true,
        //        enableSelectAll: true,
        //        showFooter: true,
        //        enableGridMenu: true,

        //        columnDefs: [
        //            { name: "MrrID", displayName: "MrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "CHID", displayName: "CHID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "CompanyID", displayName: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "DepartmentID", displayName: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "SupplierID", displayName: "SupplierID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "StatusID", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "StatusBy", displayName: "StatusBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "RequisitionID", displayName: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },


        //            { name: "MrrNo", displayName: $scope.grdTMrNo, title: $scope.grdTMrNo, width: '25%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "MrrDate", displayName: $scope.grdTMrDate, title: $scope.grdTMrDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "IssueNo", displayName: "Issue No", title: "Issue No", visible: $scope.CommonFIssueNReturnReceive, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "IssueDate", displayName: "Issue Date", title: "Issue Date", visible: $scope.CommonFIssueNReturnReceive, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "DepartmentName", displayName: "Department Name", title: "Department Name", visible: $scope.CommonFIssueNReturnReceive, width: '30%', headerCellClass: $scope.highlightFilteredHeader },


        //            { name: "GrrNo", displayName: "Grr No", title: "Grr No", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "GrrDate", displayName: "Grr Date", title: "Grr Date", visible: $scope.IsMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "SupplierName", displayName: "Supplier", title: "Supplier", visible: $scope.IsMrr, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
        //            //{ name: "ChallanNo", displayName: "Challan No", title: "Challan No", width: '15%', headerCellClass: $scope.highlightFilteredHeader }, 
        //            //{ name: "ChallanDate", displayName: "Challan Date", title: "Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "MrrQcNo", displayName: "Mrr QC No", title: "Mrr QC No", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "MrrQcDate", displayName: "Mrr QC Date", title: "Mrr QC Date", visible: $scope.IsMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "SprNo", displayName: "Spr No", title: "Spr No", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "SPRDate", displayName: "SPR Date", title: "SPR Date", visible: $scope.IsMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            //{ name: "PINO", displayName: "PI NO", title: "PI NO", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            //{ name: "PIDate", displayName: "PI Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "PONO", displayName: "PO NO", title: "PO NO", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "PODate", displayName: "PO Date", cellFilter: 'date:"dd-MM-yyyy"', visible: $scope.IsMrr, width: '10%', headerCellClass: $scope.highlightFilteredHeader }


        //           //// { name: "InvoiceNo", displayName: "Invoice No", title: "Invoice No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //           // { name: "CHDate", displayName: "CH Date", cellFilter: 'date:"dd-MM-yyyy"', visible: $scope.IsMrr, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //           // {
        //           //     name: 'Select',
        //           //     displayName: "Select",

        //           //     enableColumnResizing: false,
        //           //     enableFiltering: false,
        //           //     enableSorting: false,
        //           //     pinnedRight: true,

        //           //     visible: $scope.IsMrr,

        //           //     width: '10%',
        //           //     headerCellClass: $scope.highlightFilteredHeader,
        //           //     cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
        //           //                   '<a href="" title="Select" ng-click="grid.appScope.loadMasterDetailsByMrrMaster(row.entity)">' +
        //           //                     '<i class="icon-check" aria-hidden="true"></i> Select' +
        //           //                   '</a>' +
        //           //                   '</span>'
        //           // }
        //        ],

        //        onRegisterApi: function (gridApi) {
        //            $scope.gridApi = gridApi;
        //        },
        //        enableFiltering: true,
        //        enableGridMenu: true,
        //        enableSelectAll: true,
        //        exporterCsvFilename: 'Fpp.csv',
        //        exporterPdfDefaultStyle: { fontSize: 9 },
        //        exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        //        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        //        exporterPdfHeader: { text: "Fpp", style: 'headerStyle' },
        //        exporterPdfFooter: function (currentPage, pageCount) {
        //            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //        },
        //        exporterPdfCustomFormatter: function (docDefinition) {
        //            docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
        //            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //            return docDefinition;
        //        },
        //        exporterPdfOrientation: 'portrait',
        //        exporterPdfPageSize: 'LETTER',
        //        exporterPdfMaxGridWidth: 500,
        //        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //    };


        //    var apiRoute = baseUrl + 'GetFPPMasterList/';
        //    var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        //    var listgridOptionsFppMaster = finishingProcessPriceService.GetList(apiRoute, cmnParam);
        //    listgridOptionsFppMaster.then(function (response) {
        //        $scope.pagination.totalItems = response.data.recordsTotal;
        //        $scope.gridOptionsFppMaster.data = response.data.lstFppMaster; 
        //        $scope.loaderMoreFppMaster = false;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //};
        //$scope.loadFppRecords(0); 

        //**********----Load Mrr MasterForm and MrrDetails List By select Mrr Master ----***************//
        //$scope.loadMasterDetailsByMrrMaster = function (dataModel) {


        //    if (dataModel.IssueID != null) {
        //        $scope.lstIssueNoList = dataModel.IssueID;
        //        $('#ddlIssueNo').select2("data", { id: dataModel.IssueID, text: dataModel.IssueNo });

        //    }

        //    if (dataModel.DepartmentID != null) {
        //        $scope.Dept = dataModel.DepartmentID;
        //        $('#ddlDept').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });

        //    }


        //    if (dataModel.UserID != null) {
        //        $scope.ngmMRRByList = dataModel.UserID;
        //        $('#ddlMRRBy').select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });

        //    }

        //    if (dataModel.FormDepartmentID != null) {
        //        $scope.FrmDeptIDByIssueChnge = dataModel.FormDepartmentID;

        //    }



        //    if (dataModel.DepartmentID != null) {
        //        $scope.Warehouse = dataModel.DepartmentID;
        //        $('#ddlWarehouse').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });
        //    }

        //    $scope.listSupplier = [];
        //    $scope.lstSupplierList = '';
        //    $("#ddlSupplier").select2("data", { id: '', text: '' });

        //    if (dataModel.SupplierID != null) {
        //        $scope.listSupplier.push({
        //            SupplierID: dataModel.SupplierID, SupplierName: dataModel.SupplierName
        //        });

        //        $scope.lstSupplierList = dataModel.SupplierID;
        //        $("#ddlSupplier").select2("data", { id: dataModel.SupplierID, text: dataModel.SupplierName });
        //    }

        //    $scope.listSprNo = [];
        //    $scope.SprNo = '';
        //    $("#txtSPRNo").select2("data", { id: '', text: '' });
        //    if (dataModel.RequisitionID != null) {
        //        $scope.listSprNo.push({
        //            RequisitionID: dataModel.RequisitionID, SprNo: dataModel.SprNo
        //        });

        //        $scope.SprNo = dataModel.RequisitionID;
        //        $("#txtSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.SprNo });
        //    }

        //    $scope.listCurrency = [];
        //    $scope.lstCurrencyList = '';
        //    $("#ddlCurrency").select2("data", { id: '', text: '' });
        //    if (dataModel.CurrencyID != null) {
        //        $scope.listCurrency.push({
        //            Id: dataModel.CurrencyID, CurrencyName: dataModel.CurrencyName
        //        });

        //        $scope.lstCurrencyList = dataModel.CurrencyID;
        //        $("#ddlCurrency").select2("data", { id: dataModel.CurrencyID, text: dataModel.CurrencyName });
        //    }


        //    $scope.listPONo = [];
        //    $scope.PONo = '';
        //    $("#txtPONo").select2("data", { id: '', text: '' });

        //    if (dataModel.POID != null) {
        //        $scope.listPONo.push({
        //            POID: dataModel.POID, PONo: dataModel.PONO
        //        });

        //        $scope.PONo = dataModel.POID;
        //        $("#txtPONo").select2("data", { id: dataModel.POID, text: dataModel.PONO });
        //    }


        //    //$scope.listPINO = [];
        //    //$scope.PINO = '';
        //    //$("#txtPINO").select2("data", { id: '', text: '' });

        //    //if (dataModel.PIID != null) {
        //    //    $scope.listPINO.push({
        //    //        PIID: dataModel.PIID, PINo: dataModel.PINO
        //    //    });
        //    //    $scope.PINO = dataModel.PIID;
        //    //    $("#txtPINO").select2("data", { id: dataModel.PIID, text: dataModel.PINO });
        //    //}

        //    $scope.listGRRNo = [];
        //    $scope.lstGRRNoList = '';
        //    $("#ddlGRRNo").select2("data", { id: '', text: '' });

        //    if (dataModel.GrrID != null) {
        //        $scope.listGRRNo.push({
        //            GrrID: dataModel.GrrID, GrrNo: dataModel.GrrNo
        //        });
        //        $scope.lstGRRNoList = dataModel.GrrID;
        //        $("#ddlGRRNo").select2("data", { id: dataModel.GrrID, text: dataModel.GrrNo });
        //    }

        //    if (dataModel.UserID != null) {
        //        //$scope.listGRRNo.push({
        //        //    GrrID: dataModel.GrrID, GrrNo: dataModel.GrrNo
        //        //});
        //        $scope.ngmMRRByList = dataModel.UserID;
        //        $("#ddlMRRBy").select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });
        //    }

        //    $scope.LCNO = dataModel.LCNO;
        //    $scope.RefChallanNo = dataModel.RefCHNo;
        //    //var grrdate = response.data.lstQC[0].GrrDate;

        //    //  console.log(response.data.lstQC[0].GRRDate);
        //    //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

        //    $scope.SPRDate = dataModel.SPRDate == null ? "" : conversion.getDateToString(dataModel.SPRDate);
        //    $scope.PODate = dataModel.PODate == null ? "" : conversion.getDateToString(dataModel.PODate);
        //    // $scope.PIDate = dataModel.PIDate == null ? "" : conversion.getDateToString(dataModel.PIDate);
        //    $scope.QCDate = dataModel.MrrQcDate == null ? "" : conversion.getDateToString(dataModel.MrrQcDate);
        //    $scope.GRRDate = dataModel.GrrDate == null ? "" : conversion.getDateToString(dataModel.GrrDate);

        //    $scope.QCCertificateNo = dataModel.QCCertificateNo;
        //    $scope.ManualMRRNo = dataModel.ManualMRRNo;
        //    $scope.Remarks = dataModel.Remarks;
        //    $scope.Description = dataModel.Description;

        //    chkTrnsTypeFHideShow();

        //    //$scope.IsPostMRR = true;
        //    //$scope.IsPreMRR = false;

        //    $scope.lstQCNoList = dataModel.MrrQcID;
        //    $('#ddlQCNo').select2("data", { id: dataModel.MrrQcID, text: dataModel.MrrQcNo });
        //    $scope.QCDate = conversion.getDateToString(dataModel.MrrQcDate);



        //    // modal_fadeOut(); 
        //    $scope.IsShow = true;
        //    $scope.IsHiddenDetail = false;
        //    //
        //    $scope.btnMrrShowText = "Show List";
        //    $scope.IsHidden = true;
        //    //
        //    $scope.btnMrrSaveText = "Update";


        //    $scope.IsCreateIcon = false;
        //    $scope.IsListIcon = true;


        //    // $scope.listMrrMaster = [];

        //    var mrrID = dataModel.MrrID;
        //    $scope.MrrID = dataModel.MrrID;
        //    // $scope.PITypeID = dataModel.PITypeID;
        //    $scope.HMRRNO = dataModel.MrrNo;
        //    $scope.MrrDate = dataModel.MrrDate == null ? "" : conversion.getDateToString(dataModel.MrrDate);


        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };
        //    $scope.ListMrrDetails = [];

        //    var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
        //    var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(mrrID) + "]";

        //    var MrrDetails = finishingProcessPriceService.GetList(apiRoute, cmnParam);

        //    //var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
        //    //var MrrDetails = finishingProcessPriceService.getMrrDetailsListByMrrID(apiRoute, mrrID, objcmnParam);

        //    MrrDetails.then(function (response) {
        //        $scope.ListMrrDetails = response.data.lstDetailInfoByMrrID;
        //    },
        //function (error) {
        //    console.log("Error: " + error);
        //});

        //}


        //// **********----Save and Update InvMrrMaster and InvMrrDetail  Records----***************//
        $scope.save = function () {

            $("#save").prop("disabled", true);
            var HedarTokenPostPut = $scope.ProcessPriceID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

            var FppDateStringToDate = conversion.getStringToDate($scope.PriceDate);
            var fPPMaster = {
                ProcessPriceID: $scope.ProcessPriceID,
                ProcessPriceNo: "",
                FinishingProcessID: $scope.ngmFinishProcess,
                Price: $scope.Price,
                CurrencyID: $scope.lstCurrencyList,
                PriceDate: FppDateStringToDate,
                IsActive: $scope.IsActiveChkBox,
                Remarks: $scope.Remarks,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                IsDeleted: false
            };

            var menuID = $scope.UserCommonEntity.currentMenuID;
            var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;

            var apiRoute = baseUrl + 'SaveFinishProPricSetup/';

            var cmnParam = "[" + JSON.stringify(fPPMaster) + "," + JSON.stringify(menuID) + "]";

            var FppMasterNdetailsCreateUpdate = finishingProcessPriceService.GetList(apiRoute, cmnParam, HedarTokenPostPut);
            FppMasterNdetailsCreateUpdate.then(function (response) {
                var result = 0;
                if (response.data != "") {
                    //  $scope.HMRRNO = response.data;
                    // alert('Saved Successfully.');
                    Command: toastr["success"]("Save  Successfully!!!!");
                    $scope.clear();
                    // result = 1;
                }
                else if (response.data == "") {
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                    $("#save").prop("disabled", false);
                }

            },
            function (error) {

                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Save Not Successfull!!!!");
            });

        };



        //**********----Reset Record----***************//
        $scope.clear = function () {

            $scope.IsActiveChkBox = true;

            $("#save").prop("disabled", false);
            $scope.ProcessPriceID = "0";
            var date = new Date();
            $scope.PriceDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.listFinishProcess = [];
            $scope.ngmFinishProcess = '';
            $("#ddlFinishProcess").select2("data", { id: '', text: '--Select Finish Process--' });
            loadFinishProcessRecords(0);


            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $("#ddlCurrency").select2("data", { id: '', text: '--Select Currency--' });

            loadCurrencyRecords(0);
            $scope.Remarks = "";
            $scope.Price = "";
            $scope.ListFppDetails = [];


        };

    }]);

