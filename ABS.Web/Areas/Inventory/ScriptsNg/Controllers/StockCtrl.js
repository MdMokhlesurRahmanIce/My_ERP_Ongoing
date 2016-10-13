app.controller('StockCtrl', ['$scope', 'StockEntryService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, StockEntryService, conversion, $filter, $localStorage, uiGridConstants) {

    $scope.gridOptionsSE = [];
    var objcmnParam = {};

    var baseUrl = '/Inventory/api/StockEntry/';

    $scope.Materials = [];
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;
    $scope.IsShow = true;
    $scope.IsListShow = true;
    $scope.ItemID = 0;
    var ItemTypeID = 1;
    $scope.ListCompany = [];
    $scope.btnRawMaterialText = "Save";
    $scope.btnRawMaterialShowText = "Show  List";
    $scope.PageTitle = 'Stock Entry';
    $scope.ListTitle = 'Item List';

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

    $scope.BookingDate = conversion.NowDateCustom();

    $scope.FullFormateDate = [];
    $scope.ListCompany = [];
    $scope.bool = true;
    $scope.BookingID = "0";   

    $scope.ListBookingDetails = [];
    $scope.ListActivePIMaster = [];
    $scope.Discount = 0;
    $scope.listBuyerReference = [];
    $scope.showDtgrid = 0;
    $scope.listBuyer = [];
    $scope.lstBuyerList = '';
        //*************---Show and Hide Order---**********//
    $scope.IsHidden = true;
    $scope.IsShow = true;
    $scope.IsHiddenDetail = true;

    $scope.permissionPageVisibility = true;
    $scope.UserCommonEntity = {};
    $scope.HeaderToken = {};

        //***************************************************Start Common Task for all**************************************************
    $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
    $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        //****************************************************End Common Task for all***************************************************


    //************************************************Start Common Settings related Data******************************************************

    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
        console.log($scope.UserCommonEntity);
    }
    loadUserCommonEntity(0);
   
    //************************************************End Common Settings related Data******************************************************

    var defaultCompanyID = "";

    function loadCompanyRecords(isPaging) {

        var apiRoute = '/Sales/api/Booking/' + 'GetPICompany/';

        var listCompany = StockEntryService.getUserWiseCompany(apiRoute, $scope.UserCommonEntity.loggedUserID, $scope.HeaderToken.get);
        listCompany.then(function (response) {
            $scope.listCompany = response.data;
            angular.forEach($scope.listCompany, function (item) {
                if (item.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                    defaultCompanyID = item.CompanyID;
                    $scope.lstCompanyList = item.CompanyID;
                    $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
                    return false;
                }
            });
            $scope.LoadItemGroupByCompanyID();
            $scope.loadAllMasterRecords(0);
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadCompanyRecords(0);

    $scope.LoadItemGroupByCompanyID = function () {

        $scope.listSampleNo = [];
        $scope.lstSampleNoList = '';
        $("#ddlItemGroup").select2("data", { id: '', text: '--Select Item Group--' });


        $scope.loaderMoreForSampleNo = true;
        $scope.lblMessageForSampleNo = '';
        $scope.result = "color-red";
        debugger
        var apiRoute = '/Sales/api/Booking/' + 'GetPISampleNo/';
        var companyID = $scope.lstCompanyList;
        var listSampleNo = StockEntryService.getModelSampleNo(apiRoute, companyID, page, pageSize, isPaging, $scope.HeaderToken.get);
        listSampleNo.then(function (response) {
            $scope.listSampleNo = response.data;

            $scope.loaderMoreForSampleNo = false;
            $scope.loadAllMasterRecords(0);
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
        //**********---- Get All Sample No Records ----*************** //
    //function loadSampleNoRecords(isPaging) {

    //    // loadCompanyRecords(0); //loading company records first.

    //    $scope.loaderMoreForSampleNo = true;
    //    $scope.lblMessageForSampleNo = '';
    //    $scope.result = "color-red";
    //    debugger
    //    var apiRoute = '/Sales/api/Booking/' + 'GetPISampleNo/';
    //    var companyID = 1;
    //    var listSampleNo = StockEntryService.getModelSampleNo(apiRoute, companyID, page, pageSize, isPaging, $scope.HeaderToken.get);
    //    listSampleNo.then(function (response) {
    //        $scope.listSampleNo = response.data;

    //        $scope.loaderMoreForSampleNo = false;
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //loadSampleNoRecords(0);

        //**********----Pagination Item Master List popup----***************
    $scope.paginationItemMaster = {
        paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
        ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

        getTotalPagesItemMaster: function () {
            return Math.ceil(this.totalItems / this.pageSize);
        },
        pageSizeChange: function () {
            if (this.ddlpageSize == "All")
                this.pageSize = $scope.paginationItemMaster.totalItems;
            else
                this.pageSize = this.ddlpageSize

            this.pageNumber = 1
            $scope.loadSampleNoModalRecords(1);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.loadSampleNoModalRecords(1);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPagesItemMaster()) {
                this.pageNumber++;
                $scope.loadSampleNoModalRecords(1);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.loadSampleNoModalRecords(1);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPagesItemMaster();
                $scope.loadSampleNoModalRecords(1);
            }
        }
    };

        //**********----Get All Item Record by  select Sample No----***************//
    $scope.loadSampleNoModalRecords = function (isPaging) {
        $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

        $scope.gridOptionslistItemMaster.enableFiltering = true;
        $scope.gridOptionslistItemMasterenableGridMenu = true;

        // For Loading
        if (isPaging == 0)
            $scope.paginationItemMaster.pageNumber = 1;
        // For Loading
        $scope.loaderMoreItemMaster = true;
        $scope.lblMessageItemMaster = 'loading please wait....!';
        $scope.result = "color-red";

        //Ui Grid
        objcmnParam = {
            pageNumber: (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize),
            pageSize: $scope.paginationItemMaster.pageSize,
            IsPaging: isPaging,
            loggeduser: $scope.UserCommonEntity.loggedUserID,
            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
            menuId: $scope.UserCommonEntity.currentMenuID,
            tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
            selectedCompany: $scope.lstCompanyList
        };

        $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
            if (col.filters[0].term) {
                return 'header-filtered';
            } else {
                return '';
            }
        };

        $scope.gridOptionslistItemMaster = {
            columnDefs: [
                { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "FinishingWeight", displayName: "Weight", title: "Weight", cellFilter: 'number: 2', width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "FinishingWidth", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                {
                    name: 'Action',
                    displayName: "Action",
                    width: '6%',
                    enableColumnResizing: false,
                    enableFiltering: false,
                    enableSorting: false,
                    headerCellClass: $scope.highlightFilteredHeader,
                    visible: $scope.UserCommonEntity.EnableUpdate,
                    cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                  '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                    '<i class="icon-check" aria-hidden="true"></i> Select' +
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

        // $scope.listItemMaster = [];
        var groupID = $scope.lstSampleNoList;
        debugger
        // if (groupID > 0) {
        if (groupID == null || groupID == "" || groupID == undefined) {
            groupID = 0;
        }
        var apiRoute = '/Sales/api/Booking/' + 'GetItemMasterByGroupID/';
        var listItemMaster = StockEntryService.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);
        listItemMaster.then(function (response) {
            //$scope.listItemMaster = response.data;
            $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
            $scope.gridOptionslistItemMaster.data = response.data.objPIItemMaster;
            $scope.loaderMoreItemMaster = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
        //  }
        //else if (groupID == 0 || groupID == "") {
        //        Command: toastr["warning"]("Select Sample/Article No !!!!");
        //}
        //}
    };


    $scope.getListItemMaster = function (dataModel) {
        $scope.ItemID = dataModel.ItemID;
        $scope.ItemDescription = dataModel.ArticleNo + ' - ' + dataModel.Description;
        Command: toastr["info"]("Item Selected.");
    }
    

    $scope.GetFinishItemDescription = function () {
        var apiRoute = baseUrl + 'GetFinishItemDescription/';
        var items = StockEntryService.getAllItems(apiRoute, page, pageSize, isPaging);
        items.then(function (response) {
            $scope.items = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetFinishItemDescription();

    function loadGradeName(isPaging) {

        var apiRoute = baseUrl + 'GetGrade/';
        var listGrade = StockEntryService.GetLot(apiRoute, page, pageSize, isPaging);
        listGrade.then(function (response) {
            $scope.listGrade = response.data
            angular.forEach($scope.listGrade, function (item) {
                if (item.IsDefault == true) {

                    $("#ddlLotNo").select2("data", { id: item.ItemGradeID, text: item.GradeName });

                    return false;
                }
            });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadGradeName(0);

    $scope.save = function () {
        var ItemMaster = {
            ItemID: $scope.ItemID,

            GradeID: $scope.ItemGradeList,
            ReceiveQty: $scope.Qty,
            CompanyID: $scope.lstCompanyList
        }
        var apiRoute = baseUrl + 'SaveStockEntry/';
        var saveStockEntry = StockEntryService.post(apiRoute, ItemMaster);
        saveStockEntry.then(function (response) {
            if (response.data != "") {
                debugger
                $scope.loadAllMasterRecords(0);
                if (response.data == 1) {
                    Command: toastr["success"]("updated  Successfully!!!!");
                }
                else {
                    Command: toastr["success"]("Saved  Successfully!!!!");
                }
                $scope.clear();
            }

        }, function (error) {
            console.log("Error: " + error);
            Command: toastr["warning"]("Save Not Successfully!!!!");
        });
    }

    $scope.pagination = {
        paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
        ddlpageSize: 10,
        pageNumber: 1,
        pageSize: 10,
        totalItems: 0,

        getTotalPages: function () {
            return Math.ceil(this.totalItems / this.pageSize);
        },
        pageSizeChange: function () {
            if (this.ddlpageSize == "All")
                this.pageSize = $scope.pagination.totalItems;
            else
                this.pageSize = this.ddlpageSize

            this.pageNumber = 1
            $scope.loadAllMasterRecords(0);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.loadAllMasterRecords(0);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPages()) {
                this.pageNumber++;
                $scope.loadAllMasterRecords(0);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.loadAllMasterRecords(0);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPages();
                $scope.loadAllMasterRecords(0);
            }
        }
    };

    $scope.loadAllMasterRecords = function (isPaging) {
        $scope.loaderMore = true;
        $scope.lblMessage = 'loading please wait....!';
        $scope.result = "color-red";
        $scope.pagination.pageNumber = 1;
        objcmnParam = {
            pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
            pageSize: $scope.pagination.pageSize,
            IsPaging: isPaging,
            loggeduser: $scope.UserCommonEntity.loggedUserID,
            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
            menuId: $scope.UserCommonEntity.currentMenuID,
            tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
            selectedCompany: $scope.lstCompanyList
        };

        $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
            if (col.filters[0].term) {
                return 'header-filtered';
            } else {
                return '';
            }
        };
        $scope.gridOptionsSE = {
            useExternalPagination: true,
            useExternalSorting: true,
            enableFiltering: true,
            enableRowSelection: true,
            enableSelectAll: true,
            showFooter: true,
            enableGridMenu: true,
            columnDefs: [
                { name: "ItemName", displayName: "Item Description", width: '50%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "GradeName", title: "GradeID", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "CurrentStock", displayName: "Current Stock", width: '15%', headerCellClass: $scope.highlightFilteredHeader }
            ],
            exporterAllDataFn: function () {
                return getPage(1, $scope.gridOptions.totalItems, paginationOptions.sort)
                .then(function () {
                    $scope.gridOptions.useExternalPagination = false;
                    $scope.gridOptions.useExternalSorting = false;
                    getPage = null;
                });
            },
        };

        var apiRoute = baseUrl + 'GetItemList/';
        var listDefectTypeMaster = StockEntryService.getStockItems(apiRoute, objcmnParam);
        listDefectTypeMaster.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsSE.data = response.data.objStockMaster;
            $scope.loaderMore = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.pagination.pageNumber = 1;
    $scope.gridOptionsSE.enableFiltering = true;


    $scope.clear = function () {
        $scope.ItemID = 0;
        $scope.btnRawMaterialText = "Save";         
        $scope.ItemDescription = '',
        $scope.ddlGrade = '',
        $scope.Qty = '';

        $("#ddlItem").select2("data", { id: 0, text: '--Select--' });
        $("#ddlGrade").select2("data", { id: 0, text: '--Select--' });

        $scope.LoadItemGroupByCompanyID();
    }
     
    }]);