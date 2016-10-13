/**
* InvPICtrl.js
*/
 

app.controller('invPICtrl', ['$scope', 'invPIService', 'crudService', 'conversion', '$filter', 'uiGridConstants',
    function ($scope, invPIService, crudService, conversion, $filter, uiGridConstants) {


        $scope.gridOptionsActivePI = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        var baseUrl = '/Inventory/api/InvPI/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.CurrentDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.LastDateOfShipment = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        $scope.LgUser = $('#hUserID').val();

        $scope.PITypeID = 5;
        $scope.MenuID = 26;
        $scope.PIID = "0";

        $scope.btnInvPISaveText = "Save";
        $scope.btnInvPIShowText = "Show PI Info List";
        $scope.btnInvPIReviseText = "Update";
        $scope.PageTitle = 'Proforma Invoice Creation';
        $scope.ListTitle = 'PI Records';
        $scope.ListTitleInvPIMasters = 'PI Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleInvPIDeatails = 'Listed Item of Proforma Invoice (PI)';

        $scope.listSprNo = [];

        $scope.ListInvPIDetails = [];
        $scope.ListInvPIMaster = [];  
        $scope.listSalesPerson = [];
        $scope.showDtgrid = 0;
        $scope.listBuyer = [];
        $scope.lstBuyerList = '';
        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnInvPIShowText = "Show PI Info List";
                $scope.IsShow = true;
            }
            else {
                $scope.btnInvPIShowText = "Hide PI Info List";
                $scope.IsShow = false;
                $scope.IsHidden = false;
            }
        }

        function LoadSPRNO() {

            // $("#ddlGRRNo").select2('data').text; // $scope.lstGRRNoList; 
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };
            var apiRoute = baseUrl + 'LoadSPRNO/';
            var allSPRNO = invPIService.getLoadSPRNO(apiRoute, objcmnParam);
            console.warn(allSPRNO);
            allSPRNO.then(function (response) {
                $scope.listSprNo = response.data.lstSprono;
            },
            function (error) {
                console.log("Error: " + error);
            }); 
        } 
        LoadSPRNO();


        //var defaultBankID = "";

        //function LoadBankAdvising() {

        //    var apiRoute = baseUrl + 'GetBankAdvisingList/';
        //    var companyID = defaultCompanyID != "" ? defaultCompanyID : $scope.LoggedCompanyID;
        //    defaultCompanyID = "";
        //    $scope.listBankAdvising = [];
        //    $scope.listBankBranch = [];
        //    $scope.lstBankAdvisingList = '';
        //    $scope.lstBankBranchList = '';
        //    $("#ddlBankAdvising").select2("data", { id: '', text: '--Select Bank  Advise--' });
        //    $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
        //    var listBankAdvising = invPIService.getBankAdvising(apiRoute, companyID);
        //    listBankAdvising.then(function (response) {
        //        $scope.listBankAdvising = response.data;
        //        angular.forEach($scope.listBankAdvising, function (item) {
        //            if (item.IsDefaultBankAdvising == true) {
        //               // defaultBankID = item.BankID;
                        
        //               // $scope.lstBankAdvisingList = item.BankID;
        //               // $("#ddlBankAdvising").select2("data", { id: item.BankID, text: item.BankName });

        //                $scope.LoadBranchByBankID();
        //                return false;
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}

        //LoadBankAdvising();

        //$scope.LoadBranchByBankID = function () { 
        //    var apiRoute = baseUrl + 'GetBranchListByBankID/';
        //    var bankID = $scope.lstBankAdvisingList;
        //    var bankID = defaultBankID != "" ? defaultBankID : $scope.lstBankAdvisingList;
        //   defaultBankID = "";
        //    $scope.listBankBranch = [];
        //    $scope.lstBankBranchList = '';
        //    $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
        //    var listBankBranch = invPIService.getBranchByBankID(apiRoute, bankID);
        //    listBankBranch.then(function (response) {
        //        $scope.listBankBranch = response.data;
        //        //angular.forEach($scope.listBankBranch, function (item) {
        //        //    if (item.IsDefaultBankBranch == true) {
        //        //        // debugger
        //        //        //  $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

        //        //        $scope.lstBankBranchList = item.BranchID;
        //        //        $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

        //        //        return false;
        //        //    }
        //       // });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}


        ////**********---- Get All Sample No Records ----*************** //
        //function loadSampleNoRecords(isPaging) {
        //    $scope.loaderMoreForSampleNo = true;
        //    $scope.lblMessageForSampleNo = '';
        //    $scope.result = "color-red";

        //    var apiRoute = baseUrl + 'GetPISampleNo/';
        //    var listSampleNo = crudService.getModel(apiRoute, page, pageSize, isPaging);
        //    listSampleNo.then(function (response) {
        //        $scope.listSampleNo = response.data;

        //        $scope.loaderMoreForSampleNo = false;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadSampleNoRecords(0);

        //**********---- Get All Buyer Records ----*************** //
        function loadSuplierRecords(isPaging) {

            var apiRoute = baseUrl + 'GetInvPIBuyer/';
            var listSupplier = invPIService.getInvPISuplier(apiRoute);
            listSupplier.then(function (response) {
                $scope.listSupplier = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSuplierRecords(0);

        ////**********----Pagination Item Master List popup----***************
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
        //        $scope.loadSampleNoModalRecords(1);
        //    },
        //    firstPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber = 1
        //            $scope.loadSampleNoModalRecords(1);
        //        }
        //    },
        //    nextPage: function () {
        //        if (this.pageNumber < this.getTotalPagesItemMaster()) {
        //            this.pageNumber++;
        //            $scope.loadSampleNoModalRecords(1);
        //        }
        //    },
        //    previousPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber--;
        //            $scope.loadSampleNoModalRecords(1);
        //        }
        //    },
        //    lastPage: function () {
        //        if (this.pageNumber >= 1) {
        //            this.pageNumber = this.getTotalPagesItemMaster();
        //            $scope.loadSampleNoModalRecords(1);
        //        }
        //    }
        //};

        ////**********----Get All Item Record by  select Sample No----***************//
        //$scope.loadSampleNoModalRecords = function (isPaging) {
        //    if ($scope.lstSampleNoList == undefined) {
        //        Command: toastr["warning"]("Select Sample/Article No !!!!");
        //    }
        //    else {
        //        // For Loading modal
        //        $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

        //        $scope.gridOptionslistItemMaster.enableFiltering = true;
        //        $scope.gridOptionslistItemMasterenableGridMenu = true;

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
        //            loggeduser: LoginUserID,
        //            loggedCompany: LoginCompanyID,
        //            menuId: 5,
        //            tTypeId: 25
        //        };

        //        $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
        //            if (col.filters[0].term) {
        //                return 'header-filtered';
        //            } else {
        //                return '';
        //            }
        //        };

        //        $scope.gridOptionslistItemMaster = {
        //            columnDefs: [
        //                { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "ItemName", displayName: "Description", title: "Description", headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Construction", displayName: "Construction", title: "Construction", headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "WeightPerUnit", displayName: "Weight", title: "Weight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "ColorName", displayName: "Color Name", title: "Color Name", headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Width", displayName: "Width", title: "Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                {
        //                    name: 'Edit',
        //                    displayName: "Edit",
        //                    width: '6%',
        //                    enableColumnResizing: false,
        //                    enableFiltering: false,
        //                    enableSorting: false,
        //                    headerCellClass: $scope.highlightFilteredHeader,
        //                    cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
        //                                  '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
        //                                    '<i class="icon-check" aria-hidden="true"></i> Select' +
        //                                  '</a>' +
        //                                  '</span>'
        //                }
        //            ],
        //            onRegisterApi: function(gridApi) {
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
        //        var groupID = $scope.lstSampleNoList;
        //        if (groupID > 0) {
        //            var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
        //            var listItemMaster = crudService.getItemMasterByGroup(apiRoute, objcmnParam, groupID);
        //            listItemMaster.then(function (response) {
        //                //$scope.listItemMaster = response.data;
        //                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
        //                $scope.gridOptionslistItemMaster.data = response.data.objPIItemMaster;
        //                $scope.loaderMoreItemMaster = false;
        //            },
        //            function (error) {
        //                console.log("Error: " + error);
        //            });
        //        }
        //        else if (groupID == 0 || groupID == "") {
        //                Command: toastr["warning"]("Select Sample/Article No !!!!");
        //        }
        //    }
        //};

        ////**********----Get Company Record and filter by LoginCompanyID and cascading with Advising bank and branch record ----***************//
        //var defaultCompanyID = "";

        //function loadCompanyRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetPICompany/';
        //    $scope.listBankAdvising = [];
        //    $scope.listBankBranch = [];
        //    $scope.lstBankAdvisingList = '';
        //    $scope.lstBankBranchList = '';
        //    $("#ddlBankAdvising").select2("data", { id: '', text: '--Select Bank  Advise--' });
        //    $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });

        //    var listCompany = crudService.getUserWiseCompany(apiRoute, LoginUserID);
        //    listCompany.then(function (response) {
        //        $scope.listCompany = response.data;
        //        angular.forEach($scope.listCompany, function (item) {
        //            if (item.CompanyID == LoginCompanyID) {
        //                defaultCompanyID = item.CompanyID;
        //                $scope.lstCompanyList = item.CompanyID;
        //                $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
        //                $scope.LoadBankAdvisingByCompanyID();

        //                return false;
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadCompanyRecords(0);

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
        //        $scope.loadActivePIRecords(1);
        //    },
        //    firstPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber = 1
        //            $scope.loadActivePIRecords(1);
        //        }
        //    },
        //    nextPage: function () {
        //        if (this.pageNumber < this.getTotalPages()) {
        //            this.pageNumber++;
        //            $scope.loadActivePIRecords(1);
        //        }
        //    },
        //    previousPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber--;
        //            $scope.loadActivePIRecords(1);
        //        }
        //    },
        //    lastPage: function () {
        //        if (this.pageNumber >= 1) {
        //            this.pageNumber = this.getTotalPages();
        //            $scope.loadActivePIRecords(1);
        //        }
        //    }
        //};

        ////**********----Get All Active PI Records----***************
        //$scope.loadActivePIRecords = function (isPaging) {

        //    $scope.gridOptionsActivePI.enableFiltering = true;
        //    // For Loading
        //    if (isPaging == 0)
        //        $scope.pagination.pageNumber = 1;

        //    // For Loading
        //    $scope.loaderMore = true;
        //    $scope.lblMessage = 'loading please wait....!';
        //    $scope.result = "color-red";

        //    //Ui Grid
        //    objcmnParam = {
        //        pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
        //        pageSize: $scope.pagination.pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: LoginUserID,
        //        loggedCompany: LoginCompanyID,
        //        menuId: 5,
        //        tTypeId: 25
        //    };

        //    $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
        //        if (col.filters[0].term) {
        //            return 'header-filtered';
        //        } else {
        //            return '';
        //        }
        //    };

        //    $scope.gridOptionsActivePI = {
        //        columnDefs: [
        //            { name: "PIID", displayName: "PI ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "PINO", displayName: "PI NO", title: "PI NO", headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "ComboNameShipment", displayName: "Shipment", title: "Shipment", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "ComboNameSight", displayName: "Sight", title: "Sight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "ComboNameValidity", displayName: "Validity", title: "Validity", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "BuyerFirstName", displayName: "Buyer Name", title: "Buyer Name", headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "PIDate", displayName: "PI Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            {
        //                name: 'Edit',
        //                displayName: "Edit",
        //                enableColumnResizing: false,
        //                enableFiltering: false,
        //                enableSorting: false,
        //                width: '6%',
        //                headerCellClass: $scope.highlightFilteredHeader,
        //                cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
        //                              '<a href="" title="Select" ng-click="grid.appScope.loadPIMasterDetailsByActivePI(row.entity)">' +
        //                                '<i class="icon-check" aria-hidden="true"></i> Select' +
        //                              '</a>' +
        //                              '</span>'
        //            }
        //        ],

        //        enableFiltering: true,
        //        enableGridMenu: true,
        //        enableSelectAll: true,
        //        exporterCsvFilename: 'ActivePIMaster.csv',
        //        exporterPdfDefaultStyle: { fontSize: 9 },
        //        exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        //        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        //        exporterPdfHeader: { text: "ActivePIMaster", style: 'headerStyle' },
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


        //    var apiRoute = baseUrl + 'GetPIMasterByPIActive/';
        //    var listActivePIMaster = crudService.getPIMasterListByPIActive(apiRoute, objcmnParam);
        //    listActivePIMaster.then(function (response) {
        //        $scope.pagination.totalItems = response.data.recordsTotal;
        //        $scope.gridOptionsActivePI.data = response.data.objVmPIMaster;
        //        $scope.loaderMoreActivePIMaster = false;
        //        $scope.loaderMore = false;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //};

        ////**********---- Get Shipment Record ----*************** //
        //function loadShipmentRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetPIShipment/';
        //    var listShipment = crudService.getModel(apiRoute, page, pageSize, isPaging);
        //    listShipment.then(function (response) {
        //        $scope.listShipment = response.data
        //        angular.forEach($scope.listShipment, function (item) {
        //            if (item.IsDefault == true) {

        //                // $("#ddlShipment").select2("data", { id: item.ComboID, text: item.ComboName });
        //                $scope.lstShipmentList = item.ComboID;
        //                $("#ddlShipment").select2("data", { id: item.ComboID, text: item.ComboName });

        //                return false;
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadShipmentRecords(0);

        ////**********---- Get Validity Record ----*************** //

        //function loadValidityRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetPIValidity/';
        //    var listValidity = crudService.getModel(apiRoute, page, pageSize, isPaging);
        //    listValidity.then(function (response) {
        //        $scope.listValidity = response.data
        //        angular.forEach($scope.listValidity, function (item) {
        //            if (item.IsDefault == true) {
        //                //  $("#ddlValidity").select2("data", { id: item.ComboID, text: item.ComboName });

        //                $scope.lstValidityList = item.ComboID;
        //                $("#ddlValidity").select2("data", { id: item.ComboID, text: item.ComboName });
        //                return false;
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadValidityRecords(0);

        ////**********---- Get Sight Record ----*************** //

        //function loadSightRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetPISight/';
        //    var listSight = crudService.getModel(apiRoute, page, pageSize, isPaging);
        //    listSight.then(function (response) {
        //        $scope.listSight = response.data
        //        angular.forEach($scope.listSight, function (item) {
        //            if (item.IsDefault == true) {
        //                // $("#ddlSight").select2("data", { id: item.ComboID, text: item.ComboName });
        //                $scope.lstSightList = item.ComboID;
        //                $("#ddlSight").select2("data", { id: item.ComboID, text: item.ComboName });
        //                return false;
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadSightRecords(0);

        ////**********---- Get Sales Person Records and filter by LoginUserID ----*************** //

        //function loadSalesPersonRecords(isPaging) {
        //    $scope.listSalesPerson = [];
        //    var apiRoute = baseUrl + 'GetPISalesPerson/';
        //    var listSalesPerson = crudService.getModel(apiRoute, page, pageSize, isPaging);
        //    listSalesPerson.then(function (response) {
        //        // $scope.listSalesPerson = response.data;
        //        //  $scope.listSalesPerson = $filter('filter')($scope.listSalesPerson, { UserID: LCompanyID });
        //        angular.forEach(response.data, function (item) {
        //            if (item.UserID == LoginUserID) {
        //                $scope.listSalesPerson.push({ UserID: item.UserID, UserFullName: item.UserFullName });

        //                // $("#ddlSalesPerson").select2("data", { id: item.UserID, text: item.UserFullName });
        //                $scope.lstSalesPersonList = item.UserID;
        //                $("#ddlSalesPerson").select2("data", { id: item.UserID, text: item.UserFullName });
        //            }
        //        });
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadSalesPersonRecords(0);



        ////**********----get Item  Record from itemList popup ----***************//

        //$scope.getListItemMaster = function (dataModel) {

        //    $scope.IsHiddenDetail = false;
        //    var existItem = dataModel.ItemID;
        //    var duplicateItem = 0;
        //    angular.forEach($scope.ListPIDetails, function (item) {
        //        if (existItem == item.ItemID) {
        //            duplicateItem = 1;
        //            return false;
        //        }
        //    });

        //    if (duplicateItem === 0) {
        //        $scope.ListPIDetails.push({
        //            PIID: 0, PIDetailID: 0, ItemID: dataModel.ItemID, ItemName: dataModel.ItemName, CompanyID: dataModel.CompanyID, Description: dataModel.Description, CuttableWidth: dataModel.CuttableWidth, Construction: dataModel.Construction, BuyerStyle: '', Quantity: 0.00, ExRate: 0, UnitPrice: 0.00, Amount: 0.00, CreateBy: LoginUserID, IsActive: true
        //        });
        //    }
        //    else if (duplicateItem === 1) {
        //            Command: toastr["warning"]("Item Already Exists!!!!");
        //    }
        //    $scope.showDtgrid = $scope.ListPIDetails.length;
        //}

        ////**********----Load PI Master Form and PI Details List By select Active PI Master ----***************//
        //$scope.loadPIMasterDetailsByActivePI = function (dataModel) {

        //    // debugger;
        //    modal_fadeOut();

        //    $scope.IsShow = true;
        //    $scope.IsHiddenDetail = false;
        //    //
        //    $scope.btnPIShowText = "Show PI List";
        //    $scope.IsHidden = true;
        //    //

        //    $scope.btnPISaveText = "Revise";
        //    $scope.listPIMaster = [];
        //    var activePI = dataModel.PIID;
        //    $scope.PIID = dataModel.PIID;
        //    $scope.PITypeID = dataModel.PITypeID;
        //    $scope.HPINO = dataModel.PINO;
        //    $scope.PIDate = conversion.getDateToString(dataModel.PIDate);
        //    $scope.Negotiation = dataModel.NegoDay;
        //    $scope.OverdueInterest = dataModel.ODInterest;
        //    $scope.Remarks = dataModel.Remarks;
        //    $scope.Discount = dataModel.Discount;

        //    //  ......  Load Advising bank...............//
        //    var apiRoute = baseUrl + 'GetBankAdvisingListByCompanyID/';
        //    var companyID = dataModel.CompanyID;
        //    var listBankAdvising = crudService.getBankAdvisingByCompanyID(apiRoute, companyID);
        //    listBankAdvising.then(function (response) {
        //        $scope.listBankAdvising = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });

        //    //  ......  Load  bank branch ...............// 
        //    var apiRoute = baseUrl + 'GetBranchListByBankID/';
        //    var bankID = dataModel.BankID;
        //    var listBankBranch = crudService.getBranchByBankID(apiRoute, bankID);
        //    listBankBranch.then(function (response) {
        //        $scope.listBankBranch = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });

        //    $scope.lstSightList = dataModel.SightID;
        //    $("#ddlSight").select2("data", { id: dataModel.SightID, text: dataModel.ComboNameSight });

        //    $scope.lstShipmentList = dataModel.ShipmentID;
        //    $("#ddlShipment").select2("data", { id: dataModel.ShipmentID, text: dataModel.ComboNameShipment });

        //    $scope.lstValidityList = dataModel.ValidityID;
        //    $("#ddlValidity").select2("data", { id: dataModel.ValidityID, text: dataModel.ComboNameValidity });

        //    // $("#ddlCompany").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });

        //    $scope.lstCompanyList = dataModel.CompanyID;
        //    $("#ddlCompany").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });

        //    console.log($scope.lstBuyerList);
        //    $scope.lstBuyerList = dataModel.BuyerID;
        //    $("#ddlBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerFirstName });

        //    $scope.lstSalesPersonList = dataModel.EmployeeID;
        //    $("#ddlSalesPerson").select2("data", { id: dataModel.EmployeeID, text: dataModel.SalesPersonFirstName });

        //    $scope.lstBankAdvisingList = dataModel.BankID;
        //    $("#ddlBankAdvising").select2("data", { id: dataModel.BankID, text: dataModel.BankName });

        //    $scope.lstBankBranchList = dataModel.BranchID;
        //    $("#ddlBankBranch").select2("data", { id: dataModel.BranchID, text: dataModel.BranchName });

        //    $scope.bool = false;

        //    //get details PI
        //    var apiRoute = baseUrl + 'GetPIDetailsListByActivePI/';
        //    var listPIDetails = crudService.getPIDetailsByActivePIID(apiRoute, activePI, objcmnParam);
        //    listPIDetails.then(function (response) {
        //        $scope.ListPIDetails = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}

        ////**********----delete  Record from ListPIDetails----***************//

        //$scope.deleteRow = function (index) {
        //    // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
        //    $scope.ListPIDetails.splice(index, 1);
        //    $scope.showDtgrid = $scope.ListPIDetails.length;
        //};

        ////**********----Create Calculation----***************//
        //$scope.calculation = function (dataModel) {
        //    $scope.ListPIDetails1 = [];
        //    angular.forEach($scope.ListPIDetails, function (item) {
        //        var amountInDec = parseFloat(parseFloat(item.Quantity) * parseFloat(item.UnitPrice)).toFixed(2);
        //        $scope.ListPIDetails1.push({
        //            PIID: item.PIID, PIDetailID: item.PIDetailID, ItemID: item.ItemID, ItemName: item.ItemName, CompanyID: item.CompanyID, Description: item.Description, CuttableWidth: item.CuttableWidth, Construction: item.Construction, BuyerStyle: item.BuyerStyle, Quantity: item.Quantity, ExRate: item.ExRate, UnitPrice: item.UnitPrice, Amount: amountInDec, CreateBy: item.CreateBy, IsActive: item.IsActive

        //        });
        //        $scope.ListPIDetails = $scope.ListPIDetails1;
        //    });
        //}

        ////**********----Save and Update SalPIMaster and SalPIDetail  Records----***************//
        //$scope.save = function () {
        //    $("#save").prop("disabled", true);
        //    var NewStringToDate = conversion.getStringToDate($scope.PIDate);

        //    var itemMaster = {
        //        PIID: $scope.PIID,
        //        PINO: $scope.HPINO,
        //        PIDate: NewStringToDate,
        //        // Description: $scope.lstLCList,
        //        Discount: $scope.Discount,
        //        SightID: $scope.lstSightList,
        //        ShipmentID: $scope.lstShipmentList,
        //        ValidityID: $scope.lstValidityList,
        //        BuyerID: $scope.lstBuyerList,
        //        EmployeeID: $scope.lstSalesPersonList,
        //        CompanyID: $scope.lstCompanyList,
        //        NegoDay: $scope.Negotiation,
        //        ODInterest: $scope.OverdueInterest,
        //        Remarks: $scope.Remarks,
        //        IsActive: true,
        //        CreateBy: LoginUserID,
        //        AdvisingBankID: $scope.lstBankAdvisingList,
        //        BranchID: $scope.lstBankBranchList,
        //        //  changes for menu id and transaction Id
        //        PITypeID: $scope.PITypeID //5,
        //        // MenuID: $scope.MenuID //26

        //    };
        //    var menuID = $scope.MenuID;
        //    var itemMasterDetail = $scope.ListPIDetails;
        //    var chkAmount = 1;
        //    var chkBuyerStyle = 1;
        //    angular.forEach($scope.ListPIDetails, function (item) {

        //        if (item.Amount <= 0) {
        //            chkAmount = 0;
        //        }

        //        if (item.BuyerStyle == '') {
        //            chkBuyerStyle = 0;
        //        }
        //    });

        //    if ($scope.ListPIDetails.length > 0) {

        //        if (chkAmount == 1 && chkBuyerStyle == 1) {
        //            var apiRoute = baseUrl + 'SaveUpdatePIItemMasterNdetails/';
        //            var PIItemMasterNdetailsCreateUpdate = crudService.postMasterDetail(apiRoute, itemMaster, itemMasterDetail, menuID);
        //            PIItemMasterNdetailsCreateUpdate.then(function (response) {
        //                var result = 0;
        //                if (response.data != "") {
        //                    $scope.HPINO = response.data;
        //                    // alert('Saved Successfully.');
        //                    Command: toastr["success"]("Save  Successfully!!!!");
        //                    $scope.clear();
        //                    // result = 1;
        //                }
        //                else if (response.data == "") {
        //                        Command: toastr["warning"]("Save Not Successfull!!!!");
        //                    $("#save").prop("disabled", false);
        //                }
        //                // 
        //                // ShowCustomToastrMessageResult(result);
        //            },
        //            function (error) {
        //                // console.log("Error: " + error);
        //                $("#save").prop("disabled", false);
        //                Command: toastr["warning"]("Save Not Successfull!!!!");
        //            });
        //        }
        //        else if (chkAmount == 0) {
        //            $("#save").prop("disabled", false);
        //            Command: toastr["warning"]("Amount Must Not Zero Or Empty !!!!");

        //        }
        //        else if (chkBuyerStyle == 0) {
        //            $("#save").prop("disabled", false);
        //            Command: toastr["warning"]("Buyer Style Must Not Empty !!!!");
        //        }
        //        else if (chkBuyerStyle == 0 && chkAmount == 0) {
        //            $("#save").prop("disabled", false);
        //            Command: toastr["warning"]("Buyer Style And Amount Must Not Empty/Zero !!!!");
        //        }
        //    }
        //    else if ($scope.ListPIDetails.length <= 0) {
        //        $("#save").prop("disabled", false);
        //        Command: toastr["warning"]("PI Detail Must Not Empty!!!!");
        //    }
        //};

        ////**********----Reset Record----***************//
        //$scope.clear = function () {

        //    $scope.PIID = '0';
        //    $scope.showDtgrid = 0;//$scope.ListPIDetails.length;

        //    $scope.IsHidden = true;
        //    $scope.IsShow = true;
        //    $scope.IsHiddenDetail = true;
        //    $scope.btnPIShowText = "Show PI Info List";
        //    $scope.btnPISaveText = "Save";
        //    $scope.listPIMaster = [];
        //    $scope.ListPIDetails = [];
        //    $scope.bool = true;
            
        //    $scope.PITypeID = 5;
        //    $scope.MenuId = 26;
        //    // $scope.HPINO = '';
        //    $scope.listBuyer = [];

        //    $scope.Negotiation = 15;
        //    $scope.OverdueInterest = '';
        //    $scope.Remarks = '';
        //    $scope.Discount = 0;
        //    $scope.loadActivePIRecords(0);
        //    loadSampleNoRecords(0);
        //    loadBuyerRecords(0);
        //    loadCompanyRecords(0);
        //    loadShipmentRecords(0);
        //    loadValidityRecords(0);
        //    loadSightRecords(0);
        //    loadSalesPersonRecords(0);

        //    // console.log($scope.lstBuyerList);
        //    $scope.lstBuyerList = '';
        //    $scope.lstSampleNoList = '';

        //    $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });
        //    $("#ddlSampleNo").select2("data", { id: '', text: '--Select Sample/Article No--' });


        //    var date = new Date();
        //    $scope.PIDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        //};

    }]);


//function modal_fadeOut() {
//    $("#PIModal").fadeOut(200, function () {
//        $('#PIModal').modal('hide');
//    });
//}
