app.controller('pICtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Sales/api/PI/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsActivePI = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;
        $scope.IsDOCompleted = "";
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        
        $scope.PIDate = conversion.NowDateCustom();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        $scope.PIID = "0";

        $scope.TotalQuantity = 0.00;
        $scope.TotalAmount = 0.00;

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.btnPIReviseText = "Revise";
        $scope.PageTitle = 'Proforma Invoice Creation';
        $scope.ListTitle = 'PI Records';
        $scope.ListTitleActivePIMasters = 'PI Information';
       // $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitlePIDeatails = 'Listed Item of Proforma Invoice (PI)';

        $scope.ListPIDetails = [];
        $scope.ListActivePIMaster = [];
        $scope.Negotiation = 15;
        $scope.OverdueInterest = 14;
        $scope.Discount = 0;
        $scope.listSalesPerson = [];
        $scope.showDtgrid = 0;
        $scope.listBuyer = [];
        $scope.lstBuyerList = '';
        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        //****************************************************End Common Task for all***************************************************

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            //$scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnPIShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsHiddenDetail = $scope.ListPIDetails.length > 0 ? false : true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
             //   $scope.IsbtSaveReviseShow = $scope.IsLcComplete == true ? false : true;
                $scope.IsbtSaveReviseShow = $scope.IsDOCompleted == true ? false : true;

                $scope.clear(); // When at list page entry data should be cleared.
            }
            else {
                $scope.loadActivePIRecords(0);
                $scope.btnPIShowText = "Create";                
                $scope.IsShow = false;
                $scope.IsHiddenDetail = true;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                //$scope.IsbtSaveReviseShow = $scope.IsLcComplete == true ? false : true;
                $scope.IsbtSaveReviseShow = $scope.IsDOCompleted == true ? false : true;

                $scope.btnPISaveText = "Save";
                
                $scope.PIID = '0';
                $scope.showDtgrid = 0;//$scope.ListPIDetails.length;

                $scope.listPIMaster = [];
                $scope.ListPIDetails = [];
                $scope.listBuyer = [];
                loadIncotermRecords(0);
                $scope.Negotiation = 15;
                $scope.OverdueInterest = 14;
                $scope.IncotermDescription = '';
                $scope.Remarks = '';
                $scope.Discount = 0;
               // loadSampleNoRecords(0);
                loadBuyerRecords(0);

                $scope.lstBuyerList = '';
               // $scope.lstSampleNoList = '';

                $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

               // $("#ddlSampleNo").select2("data", { id: '', text: '--Select Sample/Article No--' });
                
                $scope.PIDate = conversion.NowDateCustom();
            }
        }

        //**********---- Get All Sample No Records ----*************** //
        //function loadSampleNoRecords(isPaging) {
        //    $scope.loaderMoreForSampleNo = true;
        //    $scope.lblMessageForSampleNo = '';
        //    $scope.result = "color-red";

        //    var apiRoute = baseUrl + 'GetPISampleNo/';
        //    var listSampleNo = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
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
        function loadBuyerRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPIBuyer/';
            var listBuyer = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listBuyer.then(function (response) {
                $scope.listBuyer = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBuyerRecords(0);

        //**********----Pagination Item Master List popup----***************
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

        //**********----Get All Item Record by  select Sample No----***************//
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
        //            loggeduser: $scope.UserCommonEntity.loggedUserID,
        //            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //            menuId: $scope.UserCommonEntity.currentMenuID,
        //            tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
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
        //                { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
        //                //{ name: "ItemName", displayName: "Description", title: "Description", headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "WeightPerUnit", displayName: "Weight", title: "Weight", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //                { name: "Width", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
        //                {
        //                    name: 'Action',
        //                    displayName: "Action",
        //                    width: '6%',
        //                    enableColumnResizing: false,
        //                    enableFiltering: false,
        //                    enableSorting: false,
        //                    headerCellClass: $scope.highlightFilteredHeader,
        //                    visible: $scope.UserCommonEntity.EnableUpdate,
        //                    cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
        //                                  '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
        //                                    '<i class="icon-check" aria-hidden="true"></i> Add' +
        //                                  '</a>' +
        //                                  '</span>'
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
        //        var groupID = $scope.lstSampleNoList;
        //        if (groupID > 0) {
        //            var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
        //            var listItemMaster = crudService.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);
        //            listItemMaster.then(function (response) {
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

        //**********----Get Company Record and filter by LoginCompanyID and cascading with Advising bank and branch record ----***************//
        var defaultCompanyID = "";

        function loadCompanyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPICompany/';
            $scope.listBankAdvising = [];
            $scope.listBankBranch = [];
            $scope.lstBankAdvisingList = '';
            $scope.lstBankBranchList = '';
            $("#ddlBankAdvising").select2("data", { id: '', text: '--Select Bank  Advise--' });
            $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });

            var listCompany = crudService.getUserWiseCompany(apiRoute, $scope.UserCommonEntity.loggedUserID, $scope.HeaderToken.get);
            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    if (item.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                        defaultCompanyID = item.CompanyID;
                        $scope.lstCompanyList = item.CompanyID;
                        $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
                        $scope.LoadBankAdvisingByCompanyID();

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCompanyRecords(0);

        var defaultBankID = "";

        $scope.LoadBankAdvisingByCompanyID = function () {

            var apiRoute = baseUrl + 'GetBankAdvisingListByCompanyID/';
            var companyID = defaultCompanyID != "" ? defaultCompanyID : $scope.lstCompanyList;
            defaultCompanyID = "";
            $scope.listBankAdvising = [];
            $scope.listBankBranch = [];
            $scope.lstBankAdvisingList = '';
            $scope.lstBankBranchList = '';
            $("#ddlBankAdvising").select2("data", { id: '', text: '--Select Bank  Advise--' });
            $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
            var listBankAdvising = crudService.getBankAdvisingByCompanyID(apiRoute, companyID, $scope.HeaderToken.get);
            listBankAdvising.then(function (response) {
                $scope.listBankAdvising = response.data;
                angular.forEach($scope.listBankAdvising, function (item) {
                    if (item.IsDefaultBankAdvising == true) {
                        defaultBankID = item.BankID;
                        // $("#ddlBankAdvising").select2("data", { id: item.BankID, text: item.BankName });

                        $scope.lstBankAdvisingList = item.BankID;
                        $("#ddlBankAdvising").select2("data", { id: item.BankID, text: item.BankName });

                        $scope.LoadBranchByBankID();
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });


            // for refresh buyer and booking
           // $scope.listBuyer = [];
            $scope.lstBuyerList = '';
            $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

            $scope.listBooking = [];
            $scope.lstBookingNG = '';
            $("#ddlBookingList").select2("data", { id: '', text: '--Select Booking--' });
        }

        $scope.LoadBranchByBankID = function () {
            // var ki = defaultBankID;
            var apiRoute = baseUrl + 'GetBranchListByBankID/';
            var bankID = defaultBankID != "" ? defaultBankID : $scope.lstBankAdvisingList;
            defaultBankID = "";
            $scope.listBankBranch = [];
            $scope.lstBankBranchList = '';
            $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
            var listBankBranch = crudService.getBranchByBankID(apiRoute, bankID, $scope.HeaderToken.get);
            listBankBranch.then(function (response) {
                $scope.listBankBranch = response.data;
                angular.forEach($scope.listBankBranch, function (item) {
                    if (item.IsDefaultBankBranch == true) {
                        // 
                        //  $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

                        $scope.lstBankBranchList = item.BranchID;
                        $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.LoadDetailByBookingID = function () {
            $scope.IsHiddenDetail = false;
            var groupID = $scope.lstBookingNG;
            var apiRoute = baseUrl + 'GetBookingDetailByID/';
            var listItemMaster = crudService.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);
            listItemMaster.then(function (response) {
                $scope.ListPIDetails = response.data.objPIItemMaster;


                /// for adding total quantity and amount
                debugger
                $scope.TotalQuantity = 0;
                $scope.TotalAmount = 0;
                angular.forEach($scope.ListPIDetails, function (item) {
                    debugger
                    $scope.TotalQuantity = parseFloat($scope.TotalQuantity) + parseFloat(item.Quantity);
                    $scope.TotalAmount = parseFloat($scope.TotalAmount) + parseFloat(item.Amount);
                })

                //$scope.showDtgrid = $scope.ListPIDetails.length;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----Pagination----***************
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadActivePIRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadActivePIRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadActivePIRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadActivePIRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadActivePIRecords(1);
                }
            }
        };

        //**********----Get All Active PI Records----***************
        $scope.loadActivePIRecords = function (isPaging) {

            $scope.gridOptionsActivePI.enableFiltering = true;
            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
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

            $scope.gridOptionsActivePI = {
                columnDefs: [
                    { name: "PIID", displayName: "PI ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsLcCompleted", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINO", displayName: "PI NO", title: "PI NO", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIDate", displayName: "PI Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerFirstName", displayName: "Buyer Name", title: "Buyer Name", headerCellClass: $scope.highlightFilteredHeader },

                    { name: "ComboNameShipment", displayName: "Shipment", title: "Shipment", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ComboNameSight", displayName: "Sight", title: "Sight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ComboNameValidity", displayName: "Validity", title: "Validity", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCStatus", displayName: "LC Status", title: "LC Status", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HDOStatus", displayName: "HDO Status", title: "HDO Status", width: '10%', headerCellClass: $scope.highlightFilteredHeader },


                    {
                        name: 'Select',
                        displayName: "Select",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '6%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.loadPIMasterDetailsByActivePI(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Select' +
                                      '</a>' +
                                      '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'ActivePIMaster.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "ActivePIMaster", style: 'headerStyle' },
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


            var apiRoute = baseUrl + 'GetPIMasterByPIActive/';
            var listActivePIMaster = crudService.getPIMasterListByPIActive(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listActivePIMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsActivePI.data = response.data.objVmPIMaster;
                $scope.loaderMoreActivePIMaster = false;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //**********---- Get Shipment Record ----*************** //
        function loadShipmentRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPIShipment/';
            var listShipment = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listShipment.then(function (response) {
                $scope.listShipment = response.data
                angular.forEach($scope.listShipment, function (item) {
                    if (item.IsDefault == true) {

                        // $("#ddlShipment").select2("data", { id: item.ComboID, text: item.ComboName });
                        $scope.lstShipmentList = item.ComboID;
                        $("#ddlShipment").select2("data", { id: item.ComboID, text: item.ComboName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadShipmentRecords(0);

        //**********---- Get Validity Record ----*************** //

        function loadValidityRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPIValidity/';
            var listValidity = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listValidity.then(function (response) {
                $scope.listValidity = response.data
                angular.forEach($scope.listValidity, function (item) {
                    if (item.IsDefault == true) {
                        //  $("#ddlValidity").select2("data", { id: item.ComboID, text: item.ComboName });

                        $scope.lstValidityList = item.ComboID;
                        $("#ddlValidity").select2("data", { id: item.ComboID, text: item.ComboName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadValidityRecords(0);

        //**********---- Get PI Status Record ----*************** //

        function loadPIStatusRecord(isPaging) {

            var apiRoute = baseUrl + 'GetPIStatus/';
            var listPIStatus = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listPIStatus.then(function (response) {
                $scope.listPIStatus = response.data
                angular.forEach($scope.listPIStatus, function (item) {
                    if (item.IsDefault == true) {
                        //  $("#ddlValidity").select2("data", { id: item.ComboID, text: item.ComboName });

                        $scope.lstPIStatusNG = item.ComboID;
                        $("#ddlPIStatus").select2("data", { id: item.ComboID, text: item.ComboName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadPIStatusRecord(0);

        //**********---- Get Incerm Record ----*************** //

        function loadIncotermRecords(isPaging) {

            var apiRoute = baseUrl + 'GetIncoterm/';
            var listIncoterm = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listIncoterm.then(function (response) {
                $scope.listIncoterm = response.data
                angular.forEach($scope.listIncoterm, function (item) {
                    if (item.IsDefault == true) {
                        $scope.lstIncoNG = item.IncoTermID;
                        $("#ddlIncoterm").select2("data", { id: item.IncoTermID, text: item.IncotermName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadIncotermRecords(0);


        //**********---- Get Booking Record ----*************** //

        $scope.LoadBookingNoByBuyerID = function () {

            $scope.listBooking = [];
            $scope.lstBookingNG = '';
            $("#ddlBookingList").select2("data", { id: '', text: '--Select Booking--' });


            var buyerID = $scope.lstBuyerList;
            var companyID = $scope.lstCompanyList;
            var apiRoute = baseUrl + 'GetBookingList/';
            var listBooking = crudService.getModelByBuyer(apiRoute, buyerID, companyID, page, pageSize, isPaging, $scope.HeaderToken.get);
            listBooking.then(function (response) {
                $scope.listBooking = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        

        //function loadBookingRecords(isPaging) {

        //    var apiRoute = baseUrl + 'GetBookingList/';
        //    var listBooking = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
        //    listBooking.then(function (response) {
        //        $scope.listBooking = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadBookingRecords(0);



        //**********---- Get Acceptable Quantity Record ----*************** //

        function loadAcceptableQuantity(isPaging) {

            var apiRoute = baseUrl + 'GetAcceptableQuantity/';
            var listAcceptableQuantity = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listAcceptableQuantity.then(function (response) {
                $scope.listAcceptableQuantity = response.data
                angular.forEach($scope.listAcceptableQuantity, function (item) {
                    if (item.IsDefault == true) {
                        $scope.listAccepQty = item.ComboID;
                        $("#ddlAcceptableQuantity").select2("data", { id: item.ComboID, text: item.ComboName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadAcceptableQuantity(0);

        //**********---- Get Sight Record ----*************** //

        function loadSightRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPISight/';
            var listSight = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listSight.then(function (response) {
                $scope.listSight = response.data
                angular.forEach($scope.listSight, function (item) {
                    if (item.IsDefault == true) {
                        // $("#ddlSight").select2("data", { id: item.ComboID, text: item.ComboName });
                        $scope.lstSightList = item.ComboID;
                        $("#ddlSight").select2("data", { id: item.ComboID, text: item.ComboName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSightRecords(0);

        //**********---- Get Sales Person Records and filter by LoginUserID ----*************** //

        function loadSalesPersonRecords(isPaging) {
            $scope.listSalesPerson = [];
            var apiRoute = baseUrl + 'GetPISalesPerson/';
            var listSalesPerson = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listSalesPerson.then(function (response) {
                // $scope.listSalesPerson = response.data;
                //  $scope.listSalesPerson = $filter('filter')($scope.listSalesPerson, { UserID: LCompanyID });
                angular.forEach(response.data, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {
                        $scope.listSalesPerson.push({ UserID: item.UserID, UserFullName: item.UserFullName });

                        // $("#ddlSalesPerson").select2("data", { id: item.UserID, text: item.UserFullName });
                        $scope.lstSalesPersonList = item.UserID;
                        $("#ddlSalesPerson").select2("data", { id: item.UserID, text: item.UserFullName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSalesPersonRecords(0);



        //**********----get Item  Record from itemList popup ----***************//

        $scope.getListItemMaster = function (dataModel) {

            $scope.IsHiddenDetail = false;
            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListPIDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListPIDetails.push({
                    PIID: 0, PIDetailID: 0, ItemID: dataModel.ItemID, ArticleNo: dataModel.ArticleNo, ItemName: dataModel.ItemName,
                    CompanyID: dataModel.CompanyID, Description: dataModel.Description, CuttableWidth: dataModel.CuttableWidth,
                    Construction: dataModel.Construction, BuyerStyle: '', Quantity: 0.00, ExRate: 0, UnitPrice: 0.00, Amount: 0.00,
                    CreateBy: $scope.UserCommonEntity.loggedUserID, IsActive: true
                });
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
            $scope.showDtgrid = $scope.ListPIDetails.length;
        }

        //******************************* Get Item Construction Type ****************************************************//

        $scope.loadSalesItemConstructionType = function (dataModel) {

            var apiRoute = baseUrl + 'GetSalesItemConstructionType/';
            var listConstructionType = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listConstructionType.then(function (response) {
                $scope.listConstructionType = response.data
                angular.forEach($scope.listConstructionType, function (item) {
                    
                    if (item.IsDefault == true) {
                        $scope.listConstType = item.ComboID;
                        $("#ddlConstructionType").select2("data", { id: item.ComboID, text: item.ComboName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSalesItemConstructionType(0);

        //**********----Load PI Master Form and PI Details List By select Active PI Master ----***************//
        $scope.loadPIMasterDetailsByActivePI = function (dataModel) {
            

            //$scope.IsbtSaveReviseShow = dataModel.IsLcCompleted == false ? true : false;
            //$scope.IsLcComplete = dataModel.IsLcCompleted;

            $scope.IsbtSaveReviseShow = dataModel.IsDOCompleted == false ? true : false;
            $scope.IsDOCompleted = dataModel.IsDOCompleted;

            // ;
            modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnPIShowText = "Show List";
            $scope.IsHidden = true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            //

            $scope.btnPISaveText = "Revise";
            $scope.listPIMaster = [];
            var activePI = dataModel.PIID;
            $scope.PIID = dataModel.PIID;
            $scope.TransactionTypeID = dataModel.TransactionTypeID;
            $scope.HPINO = dataModel.PINO;
            $scope.IncotermDescription = dataModel.IncotermDescription;
            $scope.PIDate = conversion.getDateToString(dataModel.PIDate);
            $scope.Negotiation = dataModel.NegoDay;
            $scope.OverdueInterest = dataModel.ODInterest;
            $scope.Remarks = dataModel.Remarks;
            $scope.Discount = dataModel.Discount;

            //  ......  Load Advising bank...............//
            var apiRoute = baseUrl + 'GetBankAdvisingListByCompanyID/';
            var companyID = dataModel.CompanyID;
            var listBankAdvising = crudService.getBankAdvisingByCompanyID(apiRoute, companyID, $scope.HeaderToken.get);
            listBankAdvising.then(function (response) {
                $scope.listBankAdvising = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });

            //  ......  Load  bank branch ...............// 
            var apiRoute = baseUrl + 'GetBranchListByBankID/';
            var bankID = dataModel.BankID;
            var listBankBranch = crudService.getBranchByBankID(apiRoute, bankID, $scope.HeaderToken.get);
            listBankBranch.then(function (response) {
                $scope.listBankBranch = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });

            $scope.lstBookingNG = dataModel.BookingID;
            $("#ddlBookingList").select2("data", { id: dataModel.BookingID, text: dataModel.BookingNo });

            $scope.lstSightList = dataModel.SightID;
            $("#ddlSight").select2("data", { id: dataModel.SightID, text: dataModel.ComboNameSight });

            $scope.lstShipmentList = dataModel.ShipmentID;
            $("#ddlShipment").select2("data", { id: dataModel.ShipmentID, text: dataModel.ComboNameShipment });

            $scope.lstValidityList = dataModel.ValidityID;
            $("#ddlValidity").select2("data", { id: dataModel.ValidityID, text: dataModel.ComboNameValidity });

            $scope.lstPIStatusNG = dataModel.StatusID;
            $("#ddlPIStatus").select2("data", { id: dataModel.StatusID, text: dataModel.comboNamePIStatus });

            
            $scope.listAccepQty = dataModel.AcceptanceID;
            $("#ddlAcceptableQuantity").select2("data", { id: dataModel.AcceptanceID, text: dataModel.ComboNameAcceptance });

            $scope.lstIncoNG = dataModel.IncotermID;
            $("#ddlIncoterm").select2("data", { id: dataModel.IncotermID, text: dataModel.IncotermName });

            $scope.lstCompanyList = dataModel.CompanyID;
            $("#ddlCompany").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });

            $scope.lstBuyerList = dataModel.BuyerID;
            $("#ddlBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerFirstName });

            $scope.lstSalesPersonList = dataModel.EmployeeID;
            $("#ddlSalesPerson").select2("data", { id: dataModel.EmployeeID, text: dataModel.SalesPersonFirstName });

            $scope.lstBankAdvisingList = dataModel.BankID;
            $("#ddlBankAdvising").select2("data", { id: dataModel.BankID, text: dataModel.BankName });

            $scope.lstBankBranchList = dataModel.BranchID;
            $("#ddlBankBranch").select2("data", { id: dataModel.BranchID, text: dataModel.BranchName });

            $scope.bool = false;

            //get details PI
            var apiRoute = baseUrl + 'GetPIDetailsListByActivePI/';
            var listPIDetails = crudService.getPIDetailsByActivePIID(apiRoute, activePI, $scope.HeaderToken.get);
            listPIDetails.then(function (response) {
                $scope.ListPIDetails = response.data;

                // adding quantity and amount.

                $scope.TotalQuantity = 0;
                $scope.TotalAmount = 0;
                angular.forEach($scope.ListPIDetails, function (item) {
                    debugger
                    $scope.TotalQuantity = parseFloat($scope.TotalQuantity) + parseFloat(item.Quantity);
                    $scope.TotalAmount = parseFloat($scope.TotalAmount) + parseFloat(item.Amount);
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----delete  Record from ListPIDetails----***************//

        //$scope.deleteRow = function (index) {
        //    // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
        //    $scope.ListPIDetails.splice(index, 1);
        //    $scope.showDtgrid = $scope.ListPIDetails.length;
        //};

        //**********----Create Calculation----***************//
        $scope.calculation = function (dataModel) {
            $scope.ListPIDetails1 = [];
            angular.forEach($scope.ListPIDetails, function (item) {
                debugger
                var amountInDec = parseFloat(parseFloat(item.Quantity) * parseFloat(item.UnitPrice == null || item.UnitPrice == '' ? 0 : item.UnitPrice)).toFixed(2);
                $scope.ListPIDetails1.push({
                    PIID: item.PIID, PIDetailID: item.PIDetailID, ItemID: item.ItemID, ArticleNo: item.ArticleNo, ItemConstructionTypeID: item.ItemConstructionTypeID,
                    ItemName: item.ItemName, CompanyID: item.CompanyID, Description: item.Description, CuttableWidth: item.CuttableWidth,
                    Construction: item.Construction, BuyerStyle: item.BuyerStyle, Quantity: item.Quantity, ExRate: item.ExRate,
                    UnitPrice: item.UnitPrice, Amount: amountInDec, CreateBy: item.CreateBy, IsActive: item.IsActive

                });
                $scope.ListPIDetails = $scope.ListPIDetails1;

                // adding quantity and amount

                $scope.TotalQuantity = 0;
                $scope.TotalAmount = 0;
                angular.forEach($scope.ListPIDetails, function (item) {
                    debugger
                    $scope.TotalQuantity = parseFloat($scope.TotalQuantity) + parseFloat(item.Quantity);
                    $scope.TotalAmount = parseFloat($scope.TotalAmount) + parseFloat(item.Amount==null || item.Amount==''?0:item.Amount);
                })




            });
        }

        //**********----Save and Update SalPIMaster and SalPIDetail  Records----***************//
        $scope.save = function () {
            $("#save").prop("disabled", true);            
            
            var itemMaster = {
                PIID: $scope.PIID,
                PINO: $scope.HPINO,
                PIDate: conversion.getStringToDate($scope.PIDate),
                // Description: $scope.lstLCList,
                Discount: $scope.Discount,
                SightID: $scope.lstSightList,
                ShipmentID: $scope.lstShipmentList,
                ValidityID: $scope.lstValidityList,
                StatusID:$scope.lstPIStatusNG,

                IncotermID: $scope.lstIncoNG,
                IncotermDescription: $scope.IncotermDescription,
                AcceptanceID: $scope.listAccepQty,
                BuyerID: $scope.lstBuyerList,
                EmployeeID: $scope.lstSalesPersonList,
                CompanyID: $scope.lstCompanyList,
                NegoDay: $scope.Negotiation,
                ODInterest: $scope.OverdueInterest,
                Remarks: $scope.Remarks,
                IsActive: true,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                AdvisingBankID: $scope.lstBankAdvisingList,
                BranchID: $scope.lstBankBranchList,
                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                BookingID: $scope.lstBookingNG
            };
            var menuID = $scope.UserCommonEntity.currentMenuID;
            var itemMasterDetail = $scope.ListPIDetails;
            var chkAmount = 1;
            var chkBuyerStyle = 1;
            angular.forEach($scope.ListPIDetails, function (item) {

                if (item.Amount <= 0) {
                    chkAmount = 0;
                }

                if (item.BuyerStyle == '') {
                    chkBuyerStyle = 0;
                }
            });

            if ($scope.ListPIDetails.length > 0) {

                if (chkAmount == 1 && chkBuyerStyle == 1) {
                    var apiRoute = baseUrl + 'SaveUpdatePIItemMasterNdetails/';
                    var PIItemMasterNdetailsCreateUpdate = crudService.postMasterDetail(apiRoute, itemMaster, itemMasterDetail, menuID, $scope.HeaderToken.post);
                    PIItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {
                            $scope.HPINO = response.data;
                            // alert('Saved Successfully.');
                            Command: toastr["success"]("Save  Successfully!!!!");
                            $scope.clear();
                            // result = 1;
                        }
                        else if (response.data == "") {
                                Command: toastr["warning"]("Save Not Successful!!!!");
                            $("#save").prop("disabled", false);
                        }
                        // 
                        // ShowCustomToastrMessageResult(result);
                    },
                    function (error) {
                        // console.log("Error: " + error);
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Save Not Successful!!!!");
                    });
                }
                else if (chkAmount == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Please input rate !!!!");

                }
                else if (chkBuyerStyle == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Buyer Style Must Not Empty !!!!");
                }
                else if (chkBuyerStyle == 0 && chkAmount == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Please input Buyer Style and Rate !!!!");
                }
            }
            else if ($scope.ListPIDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("PI Detail Must Not Empty!!!!");
            }
        };

        //**********----Reset Record----***************//
        $scope.clear = function () {
            //$scope.IsLcComplete = "";
            $scope.IsDOCompleted = "";
            //  $scope.IsbtSaveReviseShow = $scope.IsLcComplete == true ? false : true;
            $scope.IsbtSaveReviseShow = $scope.IsDOCompleted == true ? false : true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.PIID = '0';
            $scope.showDtgrid = 0;//$scope.ListPIDetails.length;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnPIShowText = "Show List";
            $scope.btnPISaveText = "Save";
            $scope.listPIMaster = [];
            $scope.ListPIDetails = [];
            $scope.bool = true;
            $scope.listBuyer = [];

            //$scope.lstIncoNG = '';
            //$("#ddlIncoterm").select2("data", { id: '', text: '--Select Inco.--' });
            //$scope.listIncoterm = [];
            loadIncotermRecords(0);


            $scope.Negotiation = 15;
            $scope.OverdueInterest = 14;
            $scope.IncotermDescription = '';
            $scope.Remarks = '';
            $scope.Discount = 0;
            $scope.loadActivePIRecords(0);
            // loadSampleNoRecords(0);
            loadBuyerRecords(0);
            loadCompanyRecords(0);
            loadShipmentRecords(0);
            loadValidityRecords(0);
            loadPIStatusRecord(0);
            loadSightRecords(0);
            loadSalesPersonRecords(0);

            // console.log($scope.lstBuyerList);
            $scope.lstBuyerList = '';
            //   $scope.lstSampleNoList = '';

            $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

            $scope.lstBookingNG = '';
            $("#ddlBookingList").select2("data", { id: '', text: '--Select Booking--' });

            //   $("#ddlSampleNo").select2("data", { id: '', text: '--Select Sample/Article No--' });

            $scope.PIDate = conversion.NowDateCustom();
        };

    }]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}
