/**
 * HeadOfficeSalesDeliveryOrderEntryCtrl.js
 */
app.controller('headOfficeSalesDeliveryOrderEntryCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Sales/api/HeadOfficeSalesDeliveryOrderEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        //*******************Dynamic Grid****************************
        $scope.gridOptionsHDOM = [];
        $scope.gridOptionsLCD = [];
        $scope.gridOptionsPID = [];
        $scope.gridOptionsProd = [];
        var PIMasterIDHold = "";
        //*************************End******************************
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var inCallback = false;
        var lstBuyerListHold = "";
        var IsTrue = false;
        $scope.IsAllApprove = "";
        $scope.ReviseProdIndex = "";
        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.ListBuyer = [];
        $scope.ListLCNo = [];
        $scope.listDOMaster = [];
        $scope.btnSaleSaveText = "Save";
        $scope.btnSaleReviseText = "Revise";
        $scope.btnSaleShowText = "Show List";
        $scope.PageTitle = 'Head Office Delivery Order Creation';
        $scope.ListTitle = 'Head Office Delivery Order Records';
        $scope.ListTitleLCMaster = 'LC Information';
        $scope.ListTitleDOMaster = 'Head Office Delivery Order List';
        $scope.ListTitleLCDetails = 'PI Information';
        $scope.ListTitlePIDeatails = '';
        $scope.ProductModalHeading = "New Product";
        $scope.ItemColorID = 0;
        $scope.HDONo = "";
        $scope.HDODate = conversion.NowDateCustom();
        $scope.B2BLCDate = conversion.NowDateCustom();
        $scope.IsHideRevise = true;
        $scope.IsHideSave = false;
        $scope.IsHidden = true;
        $scope.IsShowM = false;
        $scope.IsShowD = false;
        $scope.IsShowR = false;
        $scope.IsfrmShow = true;
        $scope.IsbtnAddDisable = true;
        var keepQty = 0;
        var keepUPrice = 0;
        var keepAmt = 0;
        var message = "";
        $scope.IsApproveOrDecline = 0;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        //****************************************************End Common Task for all***************************************************

        //************************************************Start PI Detail Dynamic Grid******************************************************
        $scope.CallloadLCInfo = function (LCModel) {
            IsTrue = false;
            var tempLCID = "";
            $scope.paginationLC.pageNumber = 1;
            if (LCModel != 0) {
                tempLCID = LCModel.LCID;
                if (angular.isUndefined(tempLCID)) {
                    tempLCID = LCModel;
                }
            }
            else {
                tempLCID = LCModel;
            }

            $scope.loadLCInfo(tempLCID);
        }
        //Pagination
        $scope.paginationLC = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumber: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationLC.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadLCInfo();
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadLCInfo();
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadLCInfo();
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadLCInfo();
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadLCInfo();
                }
            }
        };

        $scope.loadLCInfo = function (tempLCID) {
            //$scope.pagination.pageNumberFdod = 1;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";


            objcmnParam = {
                pageNumber: (($scope.paginationLC.pageNumber - 1) * $scope.paginationLC.pageSize),
                pageSize: $scope.paginationLC.pageSize,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList,
                IsTrue: true
            };
            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsLCD = {
                enableGridMenu: true,

                //enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                columnDefs: [
                    { name: "PIID", displayName: "PI ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '7%',
                    { name: "PINo", title: "PI No", headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    { name: "PIDate", title: "PI Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    { name: "SightName", title: "Sight Name", headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    { name: "ShipmentName", title: "Shipment Name", headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    { name: "TtlAmount", displayName: "Total Amount", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    {
                        name: 'Detail',
                        displayName: "Detail",
                        enableColumnResizing: false,
                        //enableFiltering: false,
                        enableSorting: false,
                        width: '7%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                      '<a href="javascript:void(0);" ng-href="#PIModal" data-toggle="modal" class="bs-tooltip" title="Detail">' +
                                            '<i class="glyphicon glyphicon-search" ng-click="grid.appScope.CallgetPIDetails(row.entity)">&nbsp;Detail</i>' +
                                        '</a>' +
                                    '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationLC.totalItems, pagination.sort)
                    .then(function () {
                        $scope.gridOptionsLCD.useExternalPagination = false;
                        $scope.gridOptionsLCD.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            if (tempLCID != 0) {
                IsTrue = false;
                IsTrue = true;
                objcmnParam.id = tempLCID;
                objcmnParam.IsTrue = IsTrue;
            }
            else {
                objcmnParam.id = $scope.lstLCList;
                objcmnParam.IsTrue = IsTrue;
            }
            //For LCDetail 
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetLCDetailByID/';
            var ListLCInfoDetails = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListLCInfoDetails.then(function (response) {

                $scope.paginationLC.totalItems = response.data.recordsTotal;
                $scope.gridOptionsLCD.data = response.data.objLCDetail;
                $scope.loaderMore = false;

                if ($scope.gridOptionsLCD.data != "") {
                    $scope.IsShowM = true;
                    $scope.IsShowD = true;
                    $scope.IsShowR = false;
                    $scope.IsHidden = true;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });

            debugger
            //var id = '';

            if (tempLCID != 0) {
                IsTrue = false;
                IsTrue = true;
                objcmnParam.id = tempLCID;
                objcmnParam.IsTrue = IsTrue;
            }
            else {
                objcmnParam.id = $scope.lstLCList;
                objcmnParam.IsTrue = IsTrue;
            }
            ModelsArray = [objcmnParam];
            var apiRouteMaster = baseUrl + 'GetLCMasterById/';
            var ListLCInfoMastersByID = crudService.postMultipleModel(apiRouteMaster, ModelsArray, $scope.HeaderToken.get);
            ListLCInfoMastersByID.then(function (response) {
                debugger
                $scope.ListLCInfoMaster = [];
                $scope.ListLCInfoMaster = response.data.objLCMasterById;
                //$scope.ListTitlePIDeatails = $scope.gridOptionsPID.data[0].PINO;
                //$scope.ListTitlePIDeatails = $scope.gridOptionsPID.data[0].PINO;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadLCInfo(0);
        //***************************************************End PI Detail Dynamic Grid************************************************        

        //************************************************Start PI Detail Dynamic Grid******************************************************
        $scope.CallgetPIDetails = function (dataModel) {
            var tempDataModel = [];
            tempDataModel = dataModel;//.PIID;
            $scope.paginationPI.pageNumber = 1;
            $scope.getPIDetails(tempDataModel);
        }
        //Pagination
        $scope.paginationPI = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumber: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationPI.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.getPIDetails(PIMasterIDHold);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.getPIDetails(PIMasterIDHold);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.getPIDetails(PIMasterIDHold);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.getPIDetails(PIMasterIDHold);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.getPIDetails(PIMasterIDHold);
                }
            }
        };

        $scope.getPIDetails = function (dataModel) {

            $scope.gridOptionsPID.enableFiltering = true;
            $scope.gridOptionsPID.showColumnFooter = true;
            // $scope.gridOptionsPID.showGridFooter = true;

            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";


            objcmnParam = {
                pageNumber: (($scope.paginationPI.pageNumber - 1) * $scope.paginationPI.pageSize),
                pageSize: $scope.paginationPI.pageSize,
                IsPaging: 1,    //isPaging
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
            $scope.gridOptionsPID = {
                enableGridMenu: true,
                //enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                // showGridFooter: true,
                showColumnFooter: true,
                columnDefs: [
                    { name: "PIDetailID", displayName: "PI DetailID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '7%',
                    { name: "PIID", displayName: "PIID", width: '7%', visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    { name: "Description", displayName: "Description", title: "Description", headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    {
                        name: "CuttableWidth", displayName: "Cuttable Width", title: "Cuttable Width", headerCellClass: $scope.highlightFilteredHeader,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: none;color: #000000">Total:</div>'
                    },
                    {
                        name: "Quantity", displayName: "Quantity", title: "Quantity", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },
                    {
                        name: "UnitPrice", displayName: "Unit Price", title: "Unit Price", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader
                    },
                    {
                        name: "Amount", displayName: "Amount", title: "Amount", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationPI.totalItems, pagination.sort)
                    .then(function () {
                        $scope.gridOptionsPID.useExternalPagination = false;
                        $scope.gridOptionsPID.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            if (dataModel > 0) {
                PIMasterIDHold = dataModel;
            }
            else if (dataModel == 0) {
                PIMasterIDHold = dataModel;
            }
            else {
                PIMasterIDHold = dataModel.PIID;
            }

            objcmnParam.id = PIMasterIDHold;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPIDetailsById/';
            var ListPIDetails = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListPIDetails.then(function (response) {
                debugger
                $scope.paginationPI.totalItems = response.data.recordsTotal;
                $scope.gridOptionsPID.data = response.data.objPIDetail;
                $scope.ListTitlePIDeatails = $scope.gridOptionsPID.data[0].PINO;

                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.getPIDetails(0);
        //***************************************************End PI Detail Dynamic Grid******************************************************

        //***************************************************Start HDO Master List******************************************************
        //Pagination
        $scope.paginationHDOM = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumber: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationHDOM.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadAllDOMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllDOMasterRecords(1);
                }
            }
        };

        $scope.loadAllDOMasterRecords = function (isPaging) {
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";


            objcmnParam = {
                pageNumber: (($scope.paginationHDOM.pageNumber - 1) * $scope.paginationHDOM.pageSize),//--------------will start
                pageSize: $scope.paginationHDOM.pageSize,
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
            $scope.gridOptionsHDOM = {
                //useExternalPagination: true,
                //useExternalSorting: true,
                enableGridMenu: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,

                columnDefs: [
                    { name: "HDOID", displayName: "HDO ID", visible: false, width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserId", displayName: "Buyer ID", visible: false, width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCID", displayName: "LCID", visible: false, width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", displayName: "PIID", visible: false, width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsAllApproved", visible: false, width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HDONo", displayName: "HDO No", title: "HDO No", width: '18%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HDODate", displayName: "HDO Date", title: "HDO Date", cellFilter: 'date:"dd-MM-yyyy"', width: '9%', headerCellClass: $scope.highlightFilteredHeader },
                   // { name: "AdoNo", displayName: "AHDO No", title: "AHDO No", headerCellClass: $scope.highlightFilteredHeader },//width: '8%', 
                    // { name: "AdoQty", displayName: "AHDO Quantity", title: "AHDO Quantity", headerCellClass: $scope.highlightFilteredHeader },//width: '12%', 
                     { name: "BuyerName", displayName: "Buyer Name", title: "Buyer Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "B2BLCNo", displayName: "B2B LC No", title: "B2B LCNo", width: '9%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "B2BLCDate", displayName: "B2B LC Date", title: "B2B LC Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "Remarks", displayName: "Remarks", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Status", displayName: "App. Status", width: '14%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DecUserName", displayName: "Dec. By", width: '14%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        visible: $scope.UserCommonEntity.EnableUpdate == false && $scope.UserCommonEntity.EnableDelete == false ? false : true,
                        //width: '11%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-info label-mini" ng-hide="true" ng-if="grid.appScope.UserCommonEntity.EnableUpdate">' +
                                            '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Revised" ng-click="grid.appScope.getHDoMasterByIdRevise(row.entity); grid.appScope.loadLCRecords(row.entity)">' +
                                                '<i class="glyphicon glyphicon-edit" aria-hidden="true">&nbsp;Revise</i>' +
                                            '</a>' +
                                      '</span>' +
                                      '<span class="label label-warning label-mini" style="text-align:center !important" ng-if="grid.appScope.UserCommonEntity.EnableDelete">' +
                                            '<a href="javascript:void(0);" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="bs-tooltip" title="Delete" ng-href="#CmnDeleteModal" ng-click="grid.appScope.loadDelModel(row.entity)">' +
                                                '<i class="glyphicon glyphicon-trash" aria-hidden="true">&nbsp;Delete</i>' +
                                            '</a>' +
                                      '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationHDOM.totalItems, pagination.sort)
                    .then(function () {
                        $scope.gridOptionsHDOM.useExternalPagination = false;
                        $scope.gridOptionsHDOM.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDOMaster/';
            var listDOMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDOMaster.then(function (response) {
                //console.log(response);
                //$scope.listDOMaster = response.data;
                $scope.paginationHDOM.totalItems = response.data.recordsTotal;
                $scope.gridOptionsHDOM.data = response.data.objDOMaster;
                $scope.lblMessage = '';
                $scope.loaderMore = false;

                angular.forEach($scope.gridOptionsHDOM.data, function (dataModel) {
                    if (dataModel.B2BLCDate == "1900-01-01T00:00:00") {
                        dataModel.B2BLCDate = "";
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);

            });
        }

        $scope.loadAllDOMasterRecords(1);
        //***************************************************End HDO Master List******************************************************

        //*************************************Start Get Sales DO Master Data By ID On Revise******************************
        var EntityType = "";
        $scope.getHDoMasterByIdRevise = function (dataModel) {
            debugger
            EntityType = "0";
            $scope.IsHideRevise = dataModel.IsAllApproved == true ? true : false;
            $scope.IsAllApprove = dataModel.IsAllApproved;
            $scope.IsHideSave = true;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShowM = true;
            $scope.IsShowD = false;
            $scope.IsShowR = true;
            $scope.btnSaleShowText = "Show List";
            $scope.getHDoMasterById(dataModel);
            $scope.getLCMasterById(dataModel);
            $scope.getReviseDetailByID(dataModel);

        }
        //**************************************End Get Sales DO Master Data By ID On Revise*******************************
        //**************************************Start Get Sales DO Master Data By ID On Edit*******************************
        $scope.getHDoMasterByIdEdit = function (dataModel) {
            debugger
            EntityType = "1";
            $scope.IsAllApprove = "";
            $scope.btnSaleSaveText = "Update";
            $scope.IsHideRevise = true;
            $scope.IsHideSave = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShowM = true;
            $scope.IsShowD = true;
            $scope.IsShowR = false;
            $scope.btnSaleShowText = "Show List";
            $scope.getHDoMasterById(dataModel);
        }
        //***************************************End Get Sales DO Master Data By ID On Edit********************************

        //******************Start Get Sales DO Master Data By ID On Revise(common call by another method)*******************
        $scope.getLCMasterById = function (dataModel) {
            objcmnParam = {
                pageNumber: 1,
                pageSize: 10,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList,
                id: dataModel.LCID,
                IsTrue: true
            };
            ModelsArray = [objcmnParam];
            var apiRouteMaster = baseUrl + 'GetLCMasterById/';
            var ListLCInfoMasterByID = crudService.postMultipleModel(apiRouteMaster, ModelsArray, $scope.HeaderToken.get);
            ListLCInfoMasterByID.then(function (response) {
                debugger
                $scope.ListLCInfoMaster = [];
                $scope.ListLCInfoMaster = response.data.objLCMasterById;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.ListReviseDetail = [];
        $scope.OldTotalAmount = 0;
        $scope.getReviseDetailByID = function (dataModel) {
            objcmnParam = {
                pageNumber: 0,
                pageSize: 15,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList,
                id: dataModel.HDOID,
                IsTrue: true
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetProductRevised/';
            var ListReviseDetails = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListReviseDetails.then(function (response) {
                debugger
                $scope.CurrentTotalAmount = 0;
                $scope.OldTotalAmount = 0;
                $scope.ListReviseDetail = response.data.objHDDetail;
                angular.forEach($scope.ListReviseDetail, function (rev) {
                    $scope.CurrentTotalAmount = $scope.CurrentTotalAmount + rev.Amount;
                    $scope.OldTotalAmount = $scope.OldTotalAmount + rev.Amount;
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //*******************End Get Sales DO Master Data By ID On Revise(common call by another method)********************

        //******************************************Start Add New Item List On Revise***************************************
        $scope.AddItemToList = function () {
            debugger
            $scope.ListReviseDetail.push({
                PIDetailID: 0, PIID: 0, ItemID: 0, ItemName: "", PINO: "", Construction: "", CuttableWidth: "", Quantity: "", UnitPrice: "", Amount: ""
            });
        }
        //*******************************************End Add New Item List On Revise****************************************

        //******************************************Start Delete Item From List On Revise***********************************
        $scope.DeleteProductList = function (dataModel) {
            debugger
            $scope.ListReviseDetail.splice($scope.ListReviseDetail.indexOf(dataModel), 1);
            $scope.delamt = 0;
            angular.forEach($scope.ListReviseDetail, function (rev) {
                $scope.delamt = $scope.delamt + rev.Amount;
            })
            $scope.CurrentTotalAmount = $scope.delamt;
            debugger
            $scope.CurrentTotalAmount = $scope.ListReviseDetail.length < 1 ? 0 : $scope.CurrentTotalAmount;
            $scope.IsbtnAddDisable = $scope.CurrentTotalAmount < $scope.OldTotalAmount ? false : true;
        }
        //*******************************************End Delete Item From List On Revise************************************

        //************************************Start Get New Item Data(Info) to list On Revise*******************************
        $scope.getNewProductData = function (dataEntity) {
            debugger
            keepQty = $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity;
            keepUPrice = $scope.ListReviseDetail[$scope.ReviseProdIndex].UnitPrice;
            keepAmt = $scope.ListReviseDetail[$scope.ReviseProdIndex].Amount;

            $scope.ListReviseDetail[$scope.ReviseProdIndex].UnitPrice = dataEntity.UnitPrice;
            $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity = 1;//angular.isUndefined($scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity == "") || $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity == "" ? 1 : $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity;
            $scope.calculationRevisedAmtNewLoad();

            $scope.ListReviseDetail[$scope.ReviseProdIndex].ItemID = dataEntity.ItemID;
            $scope.ListReviseDetail[$scope.ReviseProdIndex].ItemName = dataEntity.ItemName;
            $scope.ListReviseDetail[$scope.ReviseProdIndex].Construction = dataEntity.Construction;
            $scope.ListReviseDetail[$scope.ReviseProdIndex].CuttableWidth = dataEntity.CuttableWidth;
        }
        //*************************************End Get New Item Data(Info) to list On Revise********************************

        //****************************Start Calculation based on Qty*UnitPrice in list On Revise****************************
        $scope.calculationRevisedAmt = function (dataModel) {//dataModel
            $scope.CurrentTotalAmount = 0;
            $scope.ReviseProdIndex = $scope.ListReviseDetail.indexOf(dataModel);

            keepUPrice = $scope.ListReviseDetail[$scope.ReviseProdIndex].UnitPrice;

            angular.forEach($scope.ListReviseDetail, function (rev) {
                rev.Amount = rev.Quantity * rev.UnitPrice;
                $scope.CurrentTotalAmount = $scope.CurrentTotalAmount + rev.Amount;
            })
            $scope.IsbtnAddDisable = $scope.CurrentTotalAmount < $scope.OldTotalAmount ? false : true;
            if ($scope.CurrentTotalAmount > $scope.OldTotalAmount) {
                var holdamt = 0;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].UnitPrice = keepUPrice;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity = 1;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].Amount = keepUPrice;
                angular.forEach($scope.ListReviseDetail, function (rev) {
                    holdamt = holdamt + rev.Amount;
                })
                $scope.CurrentTotalAmount = holdamt;
                Command: toastr["warning"]("Current total Amount Can't be greater than Previous total Amount");
            }
        }

        $scope.calculationRevisedAmtNewLoad = function () {//dataModel
            debugger
            $scope.CurrentTotalAmount = 0;
            //$scope.ReviseProdIndex = $scope.ListReviseDetail.indexOf(dataModel);
            angular.forEach($scope.ListReviseDetail, function (rev) {
                rev.Amount = rev.Quantity * rev.UnitPrice;
                $scope.CurrentTotalAmount = $scope.CurrentTotalAmount + rev.Amount;
            })
            $scope.IsbtnAddDisable = $scope.CurrentTotalAmount < $scope.OldTotalAmount ? false : true;
            if ($scope.CurrentTotalAmount > $scope.OldTotalAmount) {
                var holdamt = 0;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].UnitPrice = keepUPrice;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].Quantity = keepQty;
                $scope.ListReviseDetail[$scope.ReviseProdIndex].Amount = keepAmt;
                angular.forEach($scope.ListReviseDetail, function (rev) {
                    holdamt = holdamt + rev.Amount;
                })
                $scope.CurrentTotalAmount = holdamt;
                Command: toastr["warning"]("Current total Amount Can't be greater than Previous total Amount");
                return;
            }
        }
        //*****************************End Calculation based on Qty*UnitPrice in list On Revise*****************************

        //***************************************Start Get Single Data For Edit/Revise**************************************
        $scope.getHDoMasterById = function (dataModel) {
            //var id = '';
            //if ('TransactionID' in dataModel) {
            //    id = dataModel.TransactionID;
            //}
            //else {
            //    id = dataModel.HDOID;
            //}
            objcmnParam = {
                pageNumber: 0,
                pageSize: 15,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                id: 'TransactionID' in dataModel ? dataModel.TransactionID : dataModel.HDOID
            };

            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDOMasterById/';
            var ListHDOInfoMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListHDOInfoMaster.then(function (response) {
                $scope.HDOID = response.data.objDoMasterById.HDOID;
                $scope.lstCompanyList = response.data.objDoMasterById.CompanyID;
                $scope.lstBuyerList = response.data.objDoMasterById.UserId;
                lstBuyerListHold = $scope.lstBuyerList;
                $scope.lstLCList = response.data.objDoMasterById.LCID;
                $("#CompanyList").select2("data", { id: 0, text: response.data.objDoMasterById.CompanyName });
                $("#BuyerList").select2("data", { id: 0, text: response.data.objDoMasterById.UserName });
                $("#LCList").select2("data", { id: 0, text: response.data.objDoMasterById.LCNo });
                $scope.HDONo = response.data.objDoMasterById.HDONo;
                $scope.lstAdoNoList = response.data.objDoMasterById.AdoNo;
                $scope.AdoQty = response.data.objDoMasterById.AdoQty;
                $scope.BtbLcNo = response.data.objDoMasterById.B2BLCNo;
                $scope.HDODate = response.data.objDoMasterById.HDODate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.objDoMasterById.HDODate);
                $scope.B2BLCDate = response.data.objDoMasterById.B2BLCDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.objDoMasterById.B2BLCDate);
                $scope.UpNo = response.data.objDoMasterById.UpNo;
                $scope.Beneficiary = response.data.objDoMasterById.Beneficiary;
                $scope.Remarks = response.data.objDoMasterById.Remarks;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //****************************************End Get Single Data For Edit/Revise***************************************

        //****************************************Start New Product List For Revise Data************************************
        //Pagination
        $scope.paginationProd = {
            paginationPageSizes: [5, 10, 15, 75, 100, 500, 1000, "All"],
            ddlpageSize: 5, pageNumberProd: 1, pageSize: 5, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationProd.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumberProd = 1
                $scope.loadAllProductRecords(1);
            },
            firstPage: function () {
                if (this.pageNumberProd > 1) {
                    this.pageNumberProd = 1
                    $scope.loadAllProductRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumberProd < this.getTotalPages()) {
                    this.pageNumberProd++;
                    $scope.loadAllProductRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumberProd > 1) {
                    this.pageNumberProd--;
                    $scope.loadAllProductRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumberProd >= 1) {
                    this.pageNumberProd = this.getTotalPages();
                    $scope.loadAllProductRecords(1);
                }
            }
        };

        $scope.CallAllProductRecords = function (dataModel) {
            debugger
            $scope.ReviseProdIndex = $scope.ListReviseDetail.indexOf(dataModel);
            $scope.paginationProd.pageNumberProd = 1;
            $scope.loadAllProductRecords(1);
        }

        $scope.loadAllProductRecords = function (isPaging) {
            debugger;
            //$scope.gridOptionsHDO.enableFiltering = true;
            if (isPaging == 0)
                $scope.paginationProd.pageNumber = 1;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            objcmnParam = {
                pageNumber: (($scope.paginationProd.pageNumberProd - 1) * $scope.paginationProd.pageSize),//--------------will start
                pageSize: $scope.paginationProd.pageSize,
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
            $scope.gridOptionsProd = {
                enableGridMenu: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                columnDefs: [
                    { name: "ItemID", displayName: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "Cuttable Width", title: "Cuttable Width", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitPrice", displayName: "Unit Price", title: "Unit Price", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '10%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                      '<a href="javascript:void(0);" data-toggle="modal" data-dismiss="modal" class="bs-tooltip" title="Select">' +
                                            '<i class="glyphicon glyphicon-check" ng-click="grid.appScope.getNewProductData(row.entity)">&nbsp;Select</i>' +
                                        '</a>' +
                                    '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationProd.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsProd.useExternalPagination = false;
                        $scope.gridOptionsProd.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetProducts/';
            var listProduct = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listProduct.then(function (response) {
                $scope.UnitPrice = 0;
                $scope.tempProdList = [];
                $scope.paginationProd.totalItems = response.data.recordsTotal;

                angular.forEach(response.data.ListProduct, function (Lproduct) {
                    $scope.UnitPrice = Math.round((Math.random() * 10) * 10);
                    $scope.tempProdList.push({
                        ItemID: Lproduct.ItemID, ItemName: Lproduct.ItemName, Construction: Lproduct.Description,
                        CuttableWidth: Lproduct.CuttableWidth, UnitPrice: $scope.UnitPrice
                    });
                })
                $scope.gridOptionsProd.data = $scope.tempProdList;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }

        $scope.loadAllProductRecords(1);
        //*****************************************End New Product List For Revise Data***********************************

        //******************************************Start Show/Hide Master List*****************************************
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
            debugger
            if ($scope.IsHidden == true) {
                $scope.btnSaleShowText = "Show List";
                $scope.IsfrmShow = true;
                if (EntityType == "1") {
                    $scope.IsShowM = true;
                    $scope.IsShowD = true;
                    $scope.IsShowR = false;
                    $scope.IsHideRevise = true;
                    $scope.IsHideSave = false;
                }
                else if (EntityType == "") {
                    $scope.IsShowM = false;
                    $scope.IsShowD = false;
                    $scope.IsShowR = false;
                    $scope.IsHideRevise = true;
                    $scope.IsHideSave = false;
                }
                if (EntityType == "0") {
                    $scope.IsShowM = true;
                    $scope.IsShowD = false;
                    $scope.IsShowR = true;
                    $scope.IsHideRevise = $scope.IsAllApprove == true ? true : false;
                    $scope.IsHideSave = true;
                }
                else if (EntityType == "") {
                    $scope.IsShowM = false;
                    $scope.IsShowD = false;
                    $scope.IsShowR = false;
                    $scope.IsHideRevise = true;
                    $scope.IsHideSave = false;
                }
            }
            else {
                $scope.btnSaleShowText = "Create";
                $scope.paginationHDOM.pageNumber = 1;
                $scope.loadAllDOMasterRecords(1);
                $scope.IsfrmShow = false;
                $scope.IsShowM = false;
                $scope.IsShowD = false;
                $scope.IsShowR = false;
                $scope.IsHideSave = true;
                $scope.IsHideRevise = true;
            }
        };
        //********************************************End Show/Hide Master List*****************************************

        //******************************************Start Get All Company Record On Load*****************************************
        $scope.loadCompanyRecords = function (isPaging) {
            objcmnParam = {
                pageNumber: 1,
                pageSize: 10,
                IsPaging: 1,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetCompany/';
            var listCompany = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);//, page, pageSize, isPaging
            listCompany.then(function (response) {
                $scope.listCompany = response.data.objListCompany;
                angular.forEach($scope.listCompany, function (com) {
                    if (com.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                        $scope.lstCompanyList = com.CompanyID;
                        $("#CompanyList").select2("data", { id: 0, text: com.CompanyName });
                    }
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadCompanyRecords(0);
        var isCompanyChanged = 0;
        $scope.onChangedCompany = function () {
            debugger
            isCompanyChanged = 1;
            $scope.clear();
            $scope.loadLCRecords(0);
        }
        //********************************************End Get All Company Record On Load*****************************************

        //******************************************Start Get All Buyer On Load*****************************************
        $scope.loadBuyerRecords = function (isPaging) {
            objcmnParam = {
                pageNumber: 1,
                pageSize: 15,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBuyer/';
            var ListBuyer = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListBuyer.then(function (response) {
                $scope.ListBuyer = response.data.BuyerList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadBuyerRecords(1);
        //********************************************End Get All Buyer On Load*****************************************

        //******************************Start Get LC Master By Buyer ID Change on Buyer Dropdownlist****************************
        $scope.loadLCRecords = function (dataModel) {
            debugger
            //var id = "";
            IsTrue = false;
            //if (dataModel != 0) {
            //    id = dataModel.UserId;
            //    lstBuyerListHold = dataModel.UserId;
            //    IsTrue = true;
            //}
            //else {
            //    id = $scope.lstBuyerList;
            //    IsTrue = false;
            //}

            lstBuyerListHold = dataModel != 0 ? dataModel.UserId : "";
            objcmnParam = {
                pageNumber: 1,
                pageSize: 15,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.lstCompanyList,
                menuId: $scope.UserCommonEntity.currentMenuID,
                id: dataModel == 0 ? $scope.lstBuyerList : dataModel.UserId,
                IsTrue: dataModel == 0 ? false : true
            };

            if (objcmnParam.id > 0) {
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetLCByBuyerId/';
                var ListLCNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListLCNo.then(function (response) {
                    debugger
                    $scope.ListLCNo = [];
                    $scope.ListLCNo = response.data.objLCBYBId;
                    if ($scope.ListLCNo == "") {
                        $("#LCList").select2("data", { id: 0, text: "--Select LC--" });
                        $scope.ListLCNo = [];
                        $scope.lstLCList = "";
                        $scope.ListLCInfoMaster = [];
                        //$scope.ListLCInfoDetails = [];
                        $scope.gridOptionsPID.data = [];
                        $scope.IsShowM = false;
                        $scope.IsShowD = false;
                        $scope.IsShowR = false;
                    }
                    if ($scope.ListLCNo != "") {
                        if (EntityType == "") {
                            if (!angular.isUndefined($scope.lstBuyerList) && $scope.lstBuyerList != lstBuyerListHold) {
                                $("#LCList").select2("data", { id: 0, text: "--Select LC--" });
                                lstBuyerListHold = $scope.lstBuyerList;
                                $scope.ListLCInfoMaster = [];
                                $scope.ListLCInfoDetails = [];
                                $scope.IsShowM = false;
                                $scope.IsShowD = false;
                                $scope.IsShowR = false;
                            }
                        }
                    }


                },
            function (error) {
                console.log("Error: " + error);
            });
            }
            else {
                $("#LCList").select2("data", { id: 0, text: "--Select LC--" });
                $scope.ListLCNo = [];
                $scope.lstLCList = "";
                $scope.ListLCInfoMaster = [];
                $scope.ListLCInfoDetails = [];
                $scope.IsShowM = false;
            }
        }
        //*******************************End Get LC Master By Buyer ID Change on Buyer Dropdownlist*****************************

        //********************************************Start Save Data***********************************************
        $scope.save = function () {
            //debugger            
            message = $scope.btnSaleSaveText == "Save" ? "Saved" : "Updated";

            var DeliveryOrders = {
                UserId: $scope.lstBuyerList,
                HDOID: message == "Saved" ? 0 : $scope.HDOID,
                CompanyID: $scope.lstCompanyList,

                HDONo: $scope.HDONo,
                LCID: $scope.lstLCList,
                AHDONo: angular.isUndefined($scope.AdoNo) || $scope.AdoNo == "" ? "ADO-" + Math.round((Math.random() * 10) * 10) : $scope.AdoNo,
                AHDOQTY: angular.isUndefined($scope.AdoQty) || $scope.AdoQty == "" ? Math.round((Math.random() * 10) * 10) : $scope.AdoQty,
                B2BLCNo: angular.isUndefined($scope.BtbLcNo) ? "" : $scope.BtbLcNo,
                HDODate: conversion.getStringToDate($scope.HDODate),
                B2BLCDate: $scope.B2BLCDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.B2BLCDate),
                UpNo: angular.isUndefined($scope.UpNo) ? "" : $scope.UpNo,
                Beneficiary: $scope.Beneficiary,
                Remarks: $scope.Remarks
            };
            ModelsArray = [DeliveryOrders, $scope.UserCommonEntity];
            var apiRoute = baseUrl + 'SaveUpdateHeadOfficeSalesDeliveryOrder/';
            var DeliorderCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.post);
            DeliorderCreateUpdate.then(function (response) {
                //debugger
                if (response.data.result != "") {
                    $scope.clear();
                    //debugger
                    $scope.HDONo = response.data.result;
                    Command: toastr["success"]("Data " + message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                console.log("Error: " + error);
                Command: toastr["warning"]("Data Not " + message + ", Please Check and Try Again!");
            });
        };
        //*********************************************End Save Data************************************************

        //******************************************Start Revise Data***********************************************
        $scope.revise = function () {
            debugger

            objcmnParam = {
                pageNumber: 1,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList,
            };

            var DeliveryOrderRevise = {
                UserId: $scope.lstBuyerList,
                HDOID: $scope.HDOID,
                CompanyID: $scope.lstCompanyList,

                HDONo: $scope.HDONo,
                LCID: $scope.lstLCList,
                AdoNo: angular.isUndefined($scope.AdoNo) || $scope.AdoNo == "" ? "ADO-" + Math.round((Math.random() * 10) * 10) : $scope.AdoNo,
                AdoQty: angular.isUndefined($scope.AdoQty) || $scope.AdoQty == "" ? Math.round((Math.random() * 10) * 10) : $scope.AdoQty,
                B2BLCNo: angular.isUndefined($scope.BtbLcNo) ? "" : $scope.BtbLcNo,
                B2BLCDate: $scope.B2BLCDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.B2BLCDate),
                UpNo: angular.isUndefined($scope.UpNo) ? "" : $scope.UpNo,
                Beneficiary: $scope.Beneficiary,
                Remarks: $scope.Remarks
            };

            $scope.ListReviseDetailForRevise = [];
            angular.forEach($scope.ListReviseDetail, function (curr) {
                if (curr.ItemID != 0) {
                    $scope.ListReviseDetailForRevise.push({
                        ItemID: curr.ItemID, ItemName: curr.ItemName, Construction: curr.Construction, CuttableWidth: curr.CuttableWidth,
                        Quantity: curr.Quantity, UnitPrice: curr.UnitPrice, Amount: curr.Amount
                    });
                }
            })

            if ($scope.ListReviseDetailForRevise.length == 0) {
                Command: toastr["warning"]("Product Not Found.");
                return;
            }

            var apiRoute = baseUrl + 'RevisedHeadOfficeSalesDeliveryOrder/';
            ModelsArray = [DeliveryOrderRevise, $scope.ListReviseDetailForRevise, objcmnParam];
            var DeliveryOrderRevised = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.post);
            DeliveryOrderRevised.then(function (response) {
                //debugger
                if (response.data.result != "") {
                    $scope.clear();
                    //debugger
                    $scope.HDONo = response.data.result;
                    Command: toastr["success"]("Data Revised Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not Revised, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Data Not Revised, Please Check and Try Again!");
                console.log("Error: " + error);

            });
        };
        //*******************************************End Revise Data************************************************

        //******************************************Start Delete Data***********************************************
        $scope.loadDelModel = function (EntityModel) {
            debugger
            $scope.UserCommonEntity.EnableYes = true;
            $scope.UserCommonEntity.EnableConf = false;
            $scope.UserCommonEntity.rowEntity = EntityModel;
            $scope.UserCommonEntity.DelMsgs = "You are about to delete " + EntityModel.HDONo + ". Are you sure?";
        }
        $scope.CmnMethod = function (MethodName, num) {
            debugger
            $scope.DeleteUpdateMasterDetail($scope.UserCommonEntity.rowEntity);
        }
        $scope.DeleteUpdateMasterDetail = function (dataModel) {

            var DeletedData = {
                HDOID: dataModel.HDOID,
                IsDeleted: true
            }
            ModelsArray = [DeletedData, $scope.UserCommonEntity];
            var apiRoute = baseUrl + 'SaveUpdateHeadOfficeSalesDeliveryOrder/';
            var HDOMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            HDOMasterDetailDelete.then(function (response) {
                $scope.clear();
                Command: toastr["success"]("Data has been Deleted Successfully!!!!");
            }, function (error) {
                Command: toastr["warning"]("Data not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*******************************************End Delete Data************************************************

        //*************************************** Start Approve Notification Related All Task ******************************
        //*************************************** Start Approve Notification ********************************

        var ApprovalModel = $localStorage.notificationStorageModel;
        var ApprovalMenuID = $localStorage.notificationStorageMenuID;

        //IsApproval Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
        var IsApproval = $localStorage.notificationStorageIsApproved;

        //IsDelaine Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
        var IsDelaine = $localStorage.notificationStorageIsDeclained;
        try {
            $scope.APModalPageTitle = ApprovalModel.CustomCode;
            $scope.DCModalPageTitle = ApprovalModel.CustomCode;
        }
        catch (e) {

        }

        $scope.IsApproved = IsApproval;
        $scope.IsDelained = IsDelaine;

        //Page Display will be false after execution of approved/declined event
        $scope.PageDisplay = true;          
        
        if ($scope.IsApproved) {
            debugger
            IsTrue = true;
            $scope.ShowHide();
            //console.log(ApprovalModel);
            $scope.getHDoMasterByIdEdit(ApprovalModel);
            debugger
            objcmnParam = {
                pageNumber: 1,
                pageSize: 15,
                IsPaging: 1,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                id: ApprovalModel.TransactionID
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDOMasterById/';
            var ListGetLCMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListGetLCMaster.then(function (response) {
                $scope.CallloadLCInfo(response.data.objDoMasterById.LCID);
            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShowM = true;
            });
        }
        else {

        }

        //Approved Or Declined Operation-Approved
        //$scope.ApprovedMethod = function () {
        //    debugger
        //    ApprovalModel.Comments = $scope.commentsModle;
        //    ApprovalModel.CreatorID = $('#hUserID').val();
        //    ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
        //    $scope.commentsModle = "";
        //    modal_fadeOut();
        //    var apiRoute = '/Sales/api/SalesLayout/ApproveNotification/';
        //    var approvalProcess = crudService.post(apiRoute, ApprovalModel, $scope.HeaderToken.post);
        //    approvalProcess.then(function (response) {
        //        if (response.data == 200) {
        //            //Hide Form
        //            $scope.PageDisplay = false;
        //            debugger
        //            $scope.IsApproveOrDecline = 0;
        //            //var ApHDOID = ApprovalModel.TransactionID;
        //            $scope.UpdateMasterByApprove(ApprovalModel.TransactionID);
        //            $scope.clear();
        //            ShowCustomToastrMessage(response);
        //            modal_fadeOut_Company();
        //        }
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //Approved Or Declined Operation-Declined
        $scope.ApprovedMethod = function () {

            ApprovalModel.Comments = $scope.commentsModle;
            ApprovalModel.CreatorID = $('#hUserID').val();
            ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
            ApprovalModel.LoggedUserID = $('#hUserID').val();
            $scope.commentsModle = "";
            modal_fadeOut();
            var apiRoute = '/Sales/api/SalesLayout/ApproveNotification/';
            var approvalProcess = crudService.postNotification(apiRoute, ApprovalModel);
            approvalProcess.then(function (response) {
                if (response.data == 200) {
                    //Hide Form
                    $scope.PageDisplay = false;
                    $scope.IsApproveOrDecline = 0;
                    $scope.UpdateMasterByApprove(ApprovalModel.TransactionID);
                    $scope.clear();
                    ShowCustomToastrMessage(response);
                    //modal_fadeOut_Company();
                }
            },
            function (error) {
                ("Error: " + error);
            });
        }
        //$scope.DeclinedMethod = function () {
        //    ApprovalModel.Comments = $scope.commentsDeclained;
        //    ApprovalModel.CreatorID = $('#hUserID').val();
        //    ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
        //    $scope.commentsDeclained = "";
        //    modal_fadeOutDeclained();
        //    var apiRoute = '/Sales/api/SalesLayout/DeclainedNotification/';
        //    var declaineProcess = crudService.post(apiRoute, ApprovalModel, $scope.HeaderToken.post);
        //    declaineProcess.then(function (response) {
        //        if (response.data == 201) {
        //            //Hide Form
        //            $scope.PageDisplay = false;
        //            debugger
        //            $scope.IsApproveOrDecline = 1;
        //            //var ApHDOID = ApprovalModel.TransactionID;
        //            $scope.UpdateMasterByApprove(ApprovalModel.TransactionID);
        //            $scope.clear();
        //            ShowCustomToastrMessage(response);
        //            modal_fadeOut_Company();
        //        }
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        $scope.DeclinedMethod = function () {
            ApprovalModel.Comments = $scope.commentsModle;
            ApprovalModel.CreatorID = $('#hUserID').val();
            ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
            ApprovalModel.LoggedUserID = $('#hUserID').val();
            $scope.commentsModle = "";
            modal_fadeOutDeclained();
            debugger
            var apiRoute = '/Sales/api/SalesLayout/DeclainedNotification/';
            var declaineProcess = crudService.postNotification(apiRoute, ApprovalModel);
            declaineProcess.then(function (response) {
                if (response.data == 201) {
                    //Hide Form
                    $scope.PageDisplay = false;
                    $scope.IsApproveOrDecline = 1;
                    $scope.UpdateMasterByApprove(ApprovalModel.TransactionID);
                    $scope.clear();
                    ShowCustomToastrMessage(response);
                    //modal_fadeOut_Company();
                }
            },
            function (error) {
                ("Error: " + error);
            });
        }

        function modal_fadeOut() {
            $("#approveNotificationModal").fadeOut(200, function () {
                $('#approveNotificationModal').modal('hide');
            });
        }
        function modal_fadeOutDeclained() {
            $("#declainedNotificationModal").fadeOut(200, function () {
                $('#declainedNotificationModal').modal('hide');
            });
        }
        //**************************************** End Approve Notification *********************************
        //**************************************** Start Approve Related Method *****************************
        $scope.UpdateMasterByApprove = function (dataModel) {
            debugger
            objcmnParam = {
                pageNumber: 1,
                pageSize: 10,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                ItemType: $scope.IsApproveOrDecline,
                id: dataModel
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'NotificationApproval/';
            var AppByID = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.put);
            AppByID.then(function (response) {
                debugger
                $scope.ShowHide();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************************** End Approve Related Method ******************************
        //***************************************** End Approve Notification Related All Task ******************************

        //********************************************Start Reset Record*****************************************
        $scope.clear = function () {
            $scope.frmHeadOfficeDeliveryOrder.$setPristine();
            $scope.frmHeadOfficeDeliveryOrder.$setUntouched();
            $scope.AdoNo = "";
            $scope.AdoQty = "";
            $scope.BtbLcNo = "";
            EntityType = "";
            $scope.HDONo = "";
            $scope.B2BLCDate = conversion.NowDateCustom();
            $scope.UpNo = "";
            $scope.Beneficiary = "";
            $scope.Remarks = "";
            $scope.btnSaleSaveText = "Save";
            $scope.btnSaleShowText = "Show List";
            $scope.IsHideRevise = true;
            $scope.IsHideSave = false;
            $scope.IsShowM = false;
            $scope.IsShowD = false;
            $scope.IsShowR = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsDeleted = false;
            $scope.IsbtnAddDisable = true;
            $scope.IsAllApprove = "";
            $scope.ListPIDetails = "";
            $scope.ListLCNo = "";
            $scope.listDOMaster = "";
            $scope.ListLCInfoMaster = "";
            $scope.ListLCInfoDetails = "";
            $scope.lstLCList = "";
            lstBuyerListHold = "";
            IsTrue = false;

            if (isCompanyChanged == 0) {
                $scope.lstCompanyList = "";
                $scope.lstBuyerList = "";

                $scope.loadCompanyRecords(0);
                $("#BuyerList").select2("data", { id: 0, text: "--Select Party--" });
            }
            $("#LCList").select2("data", { id: 0, text: "--Select LC--" });
            $scope.gridOptionsHDOM.data = [];
            $scope.gridOptionsLCD.data = [];
            $scope.gridOptionsPID.data = [];
            isCompanyChanged = 0;
        };
        //*********************************************End Reset Record******************************************
    }]);