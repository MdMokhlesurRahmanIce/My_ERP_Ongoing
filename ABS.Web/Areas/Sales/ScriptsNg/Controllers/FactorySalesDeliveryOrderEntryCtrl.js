/// <reference path="../../Views/Booking/_Partial/_formBooking.cshtml" />
/**
 * FactorySalesDeliveryOrderEntryCtrl.js
 */
app.controller('factorySalesDeliveryOrderEntryCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Sales/api/FactorySalesDeliveryOrderEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        //*******************Dynamic Grid HDO****************************
        $scope.gridOptionsHDO = [];         //----For HDO Master List
        $scope.gridOptionsFDOM = [];         //----For FDO Master List
        $scope.gridOptionsFDOD = [];         //----For FDO Details List        
        var FDOMasterIDHold = "";
        //*************************End******************************
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.ListCompany = [];
        $scope.ListBuyer = [];
        $scope.listDOMaster = [];
        $scope.btnSaleSaveText = "Save";
        $scope.btnSaleShowText = "Show List";
        $scope.PageTitle = 'Factory Delivery Order Creation';
        $scope.ListTitle = 'Factory Delivery Order Records';
        $scope.ListTitleFDOMaster = 'Factory Delivery Order List';
        $scope.ListTitleDOMaster = 'Factory Delivery Order Information';
        $scope.ListTitleHDODetails = 'Factory Delivery Order Information';
        $scope.ListTitleFDODeatails = 'FDO Information';
        $scope.HDOID = "";
        $scope.HDONo = "";
        $scope.HDODate = "";
        $scope.ListHDOInfoDetails = [];
        $scope.DtInvoice = conversion.NowDateCustom();
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenFDO = true;
        $scope.IsfrmShow = true;
        $scope.IsShowSave = true;

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        $scope.cmnParam = function () {
            objcmnParam = conversion.cmnParams($scope.UserCommonEntity);
        }
        //****************************************************End Common Task for all***************************************************

        //*************---Show and Hid Sales Delivery Order---**********        
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHiddenFDO = $scope.IsHiddenFDO ? false : true;
            if ($scope.IsHiddenFDO == true) {
                $scope.btnSaleShowText = "Show List";
                $scope.IsfrmShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
                $scope.IsShowSave = true;
                $scope.IsHidden = $scope.ListHDOInfoDetails.length > 0 ? false : true;
            }
            else {
                $scope.btnSaleShowText = "Create";
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.paginationFDOM.pageNumberFdom = 1;
                $scope.loadAllFDOMasterRecords(1);
                $scope.IsfrmShow = false;
                $scope.IsShowSave = false;
                $scope.IsHidden = true;
            }
        }

        //**********----Get All Company Record On Load----***************
        $scope.loadCompanyRecords = function (isPaging) {
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetCompany/';
            var listCompany = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listCompany.then(function (response) {
                $scope.listCompany = response.data.objListCompany;
                debugger
                angular.forEach($scope.listCompany, function (com) {
                    if (com.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                        $scope.lstCompanyList = com.CompanyID;
                        $("#CompanyList").select2("data", { id: 0, text: com.CompanyName });
                    }
                })
                $scope.loadDeptdll(0);
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadCompanyRecords(0);
        //*******************-------End------********************** 

        //**********----Load Department DDL----***************
        $scope.loadDeptdll = function (isPaging) {

            $scope.listDept = [];
            $scope.lstDeptList = '';
            $("#DeptList").select2("data", { id: '', text: '--Select Delivery Location--' });

            debugger
            $scope.cmnParam();
            objcmnParam.selectedCompany = $scope.lstCompanyList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDepartment/';
            var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDept.then(function (response) {
                debugger
                $scope.listDept = response.data.objListDept;
                angular.forEach($scope.listDept, function (dept) {
                    if (dept.OrganogramName == 'Store') {
                        $scope.lstDeptList = dept.OrganogramID;
                        $("#DeptList").select2("data", { id: 0, text: dept.OrganogramName });
                    }
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
      

        //**********----Get All HDO Master Record On Click Select----***************
        //************************************************Start HDO Master Dynamic Grid******************************************************
        $scope.CallAllDOMasterRecords = function () {
            $scope.paginationHDOM.pageNumberHdo = 1;
            $scope.loadAllDOMasterRecords(1);
        }
        //Pagination
        $scope.paginationHDOM = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumberHdo: 1,
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

                this.pageNumberHdo = 1
                $scope.loadAllDOMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumberHdo > 1) {
                    this.pageNumberHdo = 1
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumberHdo < this.getTotalPages()) {
                    this.pageNumberHdo++;
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumberHdo > 1) {
                    this.pageNumberHdo--;
                    $scope.loadAllDOMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumberHdo >= 1) {
                    this.pageNumberHdo = this.getTotalPages();
                    $scope.loadAllDOMasterRecords(1);
                }
            }
        };

        $scope.loadAllDOMasterRecords = function (isPaging) {
            debugger;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.paginationHDOM.pageNumberHdo - 1) * $scope.paginationHDOM.pageSize;
            objcmnParam.pageSize = $scope.paginationHDOM.pageSize;
            objcmnParam.selectedCompany = $scope.lstCompanyList;
            objcmnParam.IsPaging = isPaging;
            
            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsHDO = {
                enableGridMenu: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                columnDefs: [
                    { name: "HDOID", displayName: "HDO ID", width: '7%', visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HDONo", title: "HDO No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HDODate", title: "HDO Date", width: '20%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCNo", title: "LC No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserName", displayName: "Buyer Name", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
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
                                            '<i class="glyphicon glyphicon-check" ng-click="grid.appScope.getHDoDetails(row.entity)">&nbsp;Select</i>' +
                                        '</a>' +
                                    '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationHDOM.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsHDO.useExternalPagination = false;
                        $scope.gridOptionsHDO.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDOMaster/';
            var listDOMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDOMaster.then(function (response) {
                $scope.paginationHDOM.totalItems = response.data.recordsTotal;
                $scope.gridOptionsHDO.data = response.data.objDOMaster;
                $scope.lblMessage = '';
                $scope.loaderMore = false;

                angular.forEach($scope.gridOptionsHDO.data, function (dataModel) {
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
        //*************************************************End HDO Master Dynamic Grid*******************************************************

        //************************************************Start FDO Master Dynamic Grid******************************************************
        //Pagination
        $scope.paginationFDOM = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumberFdom: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationFDOM.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumberFdom = 1
                $scope.loadAllFDOMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumberFdom > 1) {
                    this.pageNumberFdom = 1
                    $scope.loadAllFDOMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumberFdom < this.getTotalPages()) {
                    this.pageNumberFdom++;
                    $scope.loadAllFDOMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumberFdom > 1) {
                    this.pageNumberFdom--;
                    $scope.loadAllFDOMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumberFdom >= 1) {
                    this.pageNumberFdom = this.getTotalPages();
                    $scope.loadAllFDOMasterRecords(1);
                }
            }
        };

        $scope.loadAllFDOMasterRecords = function (isPaging) {
            debugger;
            //$scope.pagination.pageNumberFdom = 1;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.paginationFDOM.pageNumberFdom - 1) * $scope.paginationFDOM.pageSize;
            objcmnParam.pageSize = $scope.paginationFDOM.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsFDOM = {
                enableGridMenu: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                columnDefs: [
                    { name: "FDOMasterID", displayName: "FDO MasterID", width: '7%', visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FDONo", title: "FDO No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FDODate", title: "FDO Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BillNo", title: "Bill No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BillDate", title: "Bill Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DeliveryTo", displayName: "Delivery To", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Detail',
                        displayName: "Detail",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '7%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                      '<a href="javascript:void(0);" ng-href="#FDODModal" data-toggle="modal" class="bs-tooltip" title="Detail">' +
                                            '<i class="glyphicon glyphicon-search" ng-click="grid.appScope.CallGetFDODetailRecords(row.entity)">&nbsp;Detail</i>' +
                                        '</a>' +
                                    '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationFDOM.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsFDOM.useExternalPagination = false;
                        $scope.gridOptionsFDOM.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFDOMaster/';
            var listFDOMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listFDOMaster.then(function (response) {

                $scope.paginationFDOM.totalItems = response.data.recordsTotal;
                $scope.gridOptionsFDOM.data = response.data.objFDOMaster;
                $scope.loaderMore = false;
                $scope.IsHidden = true;
                angular.forEach($scope.gridOptionsFDOM.data, function (dataModel) {
                    if (dataModel.BillDate == "1900-01-01T00:00:00") {
                        dataModel.BillDate = "";
                    }
                    if (dataModel.FDODate == "1900-01-01T00:00:00") {
                        dataModel.FDODate = "";
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.loadAllFDOMasterRecords(1);
        //***************************************************End FDO Master Dynamic Grid******************************************************

        //************************************************Start FDO Detail Dynamic Grid******************************************************
        $scope.CallGetFDODetailRecords = function (dataModel) {
            var tempDataModel = [];
            tempDataModel = dataModel;
            $scope.paginationFDOD.pageNumberFdod = 1;
            $scope.GetFDODetailRecords(tempDataModel);
        }
        //Pagination
        $scope.paginationFDOD = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumberFdod: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationFDOD.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumberFdod = 1
                $scope.GetFDODetailRecords(FDOMasterIDHold);
            },
            firstPage: function () {
                if (this.pageNumberFdod > 1) {
                    this.pageNumberFdod = 1
                    $scope.GetFDODetailRecords(FDOMasterIDHold);
                }
            },
            nextPage: function () {
                if (this.pageNumberFdod < this.getTotalPages()) {
                    this.pageNumberFdod++;
                    $scope.GetFDODetailRecords(FDOMasterIDHold);
                }
            },
            previousPage: function () {
                if (this.pageNumberFdod > 1) {
                    this.pageNumberFdod--;
                    $scope.GetFDODetailRecords(FDOMasterIDHold);
                }
            },
            lastPage: function () {
                if (this.pageNumberFdod >= 1) {
                    this.pageNumberFdod = this.getTotalPages();
                    $scope.GetFDODetailRecords(FDOMasterIDHold);
                }
            }
        };

        $scope.GetFDODetailRecords = function (dataModel) {
            debugger;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";
            $scope.gridOptionsFDOD.showColumnFooter = true;

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.paginationFDOD.pageNumberFdod - 1) * $scope.paginationFDOD.pageSize;
            objcmnParam.pageSize = $scope.paginationFDOD.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsFDOD = {
                enableGridMenu: true,
                // enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                // showGridFooter: true,
                showColumnFooter: true,
                columnDefs: [
                    { name: "FDODetailsID", displayName: "FDO DetailsID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '7%',
                    {
                        name: "RollNo", displayName: "Roll No", headerCellClass: $scope.highlightFilteredHeader,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: none;color: #000000">Total:</div>'
                    },//width: '10%', 
                    {
                        name: "QuantitYds", displayName: "Quantit (Yds)", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },//width: '10%', 
                    {
                        name: "QuantityKg", displayName: "Quantity (Kg)", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },//width: '10%', 
                    { name: "Rate", displayName: "Rate", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                    {
                        name: "Amount", displayName: "Amount($)", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },//width: '10%', 
                    {
                        name: "NetQtyKg", displayName: "Net Weight (Kg)", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },//width: '10%', 
                    {
                        name: "GrossQtyKg", displayName: "Gross Weight (Kg)", cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    }//width: '10%', 
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationFDOD.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsFDOD.useExternalPagination = false;
                        $scope.gridOptionsFDOD.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            if (dataModel > 0) {
                FDOMasterIDHold = dataModel;
            }
            else if (dataModel == 0) {
                FDOMasterIDHold = dataModel;
            }
            else {
                FDOMasterIDHold = dataModel.FDOMasterID;
            }
            $scope.cmnParam();
            objcmnParam.id = FDOMasterIDHold;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFDODetail/';
            var listFDODetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listFDODetail.then(function (response) {

                $scope.paginationFDOD.totalItems = response.data.recordsTotal;
                $scope.gridOptionsFDOD.data = response.data.objFDODetail;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.GetFDODetailRecords(0);
        //***************************************************End FDO Detail Dynamic Grid******************************************************

        //*******************-------Get DO Master & Detail By ID------**********************
        $scope.getHDoDetails = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.HDOID) || dataModel.HDOID == "" || dataModel.HDOID == null ? 0 : dataModel.HDOID;
            objcmnParam.ParamName = angular.isUndefined(dataModel.HDOID) || dataModel.HDOID == "" || dataModel.HDOID == null && dataModel != "" && !angular.isUndefined(dataModel) ? dataModel : "";
            objcmnParam.selectedCompany = $scope.lstCompanyList;
            ModelsArray = [objcmnParam];

            var apiRouteMaster = baseUrl + 'GetDOMasterById/';
            var apiRouteDetails = baseUrl + 'GetDODetailByID/';

            var ListHDOInfoMaster = crudService.postMultipleModel(apiRouteMaster, ModelsArray, $scope.HeaderToken.get);
            ListHDOInfoMaster.then(function (response) {
                if (response.data.objDoMasterById != null) {
                    debugger
                    if (response.data.objDoMasterById.DODetailCount != 0) {
                        $scope.HDOID = response.data.objDoMasterById.HDOID;
                        $scope.HoDONo = response.data.objDoMasterById.HDONo;
                        $scope.DeliveredTo = response.data.objDoMasterById.DeliveredTo;
                        $scope.UserID = response.data.objDoMasterById.UserId;
                        $scope.BuyerName = response.data.objDoMasterById.UserName;

                        angular.forEach($scope.listCompany, function (temcom) {
                            if (temcom.CompanyID == response.data.objDoMasterById.CompanyID) {
                                $scope.lstCompanyList = temcom.CompanyID;
                                $("#CompanyList").select2("data", { id: 0, text: temcom.CompanyName });
                            }
                        });

                        $scope.IsHidden = false;
                        $scope.IsHiddenFDO = true;
                        $scope.btnSaleShowText = "Show List";
                        $scope.selectedAll = false;
                    }
                    else {
                        Command: toastr["warning"]("No data found for " + response.data.objDoMasterById.HDONo);
                    }
                }
                else {
                    $scope.HDOID = "";
                    //$scope.HoDONo = "";
                    $scope.DeliveredTo = "";
                    $scope.UserID = "";
                    $scope.BuyerName = "";
                    angular.forEach($scope.listCompany, function (temcom) {
                        if (temcom.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                            $scope.lstCompanyList = temcom.CompanyID;
                            $("#CompanyList").select2("data", { id: 0, text: temcom.CompanyName });
                        }
                    });

                    $scope.IsHidden = true;
                    $scope.IsHiddenFDO = true;
                    $scope.btnSaleShowText = "Show List";
                    $scope.selectedAll = false;
                    Command: toastr["warning"]("No data found for " + objcmnParam.ParamName);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });

            //*******************-------Detail------**********************
            var ListHDOInfoDetails = crudService.postMultipleModel(apiRouteDetails, ModelsArray, $scope.HeaderToken.get);
            ListHDOInfoDetails.then(function (response) {
                $scope.ListHDOInfoDetails = response.data.objDODetailById;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //*******************-------End------**********************

        //******=========Multiple Checkbox=========******
        $scope.toggleSelection = function toggleSelection() {
            $scope.selectedAll = false;

            angular.forEach($scope.ListHDOInfoDetails, function (dataModel) {
                if (dataModel.Selected == true) {
                    dataModel.QuantityYds = dataModel.HoldQty;

                    dataModel.RemainingQty = dataModel.HoldQty - dataModel.QuantityYds;
                }
                else {
                    dataModel.QuantityYds = "";
                    dataModel.RemainingQty = dataModel.HoldQty;
                }
            });
        };

        //******=========Single Checkbox=========******
        $scope.checkAll = function () {
            if ($scope.selectedAll) {
                $scope.selectedAll = true;
            } else {
                $scope.selectedAll = false;
            }
            angular.forEach($scope.ListHDOInfoDetails, function (dataModel) {
                dataModel.Selected = $scope.selectedAll;
                if ($scope.selectedAll == true) {
                    dataModel.QuantityYds = dataModel.HoldQty;

                    dataModel.RemainingQty = dataModel.HoldQty - dataModel.QuantityYds;
                }
                else {
                    dataModel.QuantityYds = "";
                    dataModel.RemainingQty = dataModel.HoldQty;
                }
            });

            //$scope.calculation(dataModel);
        };
        //*******************-------End------**********************

        //******--Calculation in Array--********
        $scope.calculation = function (dataModel) {
            angular.forEach($scope.ListHDOInfoDetails, function (item) {
                item.RemainingQty = item.HoldQty - item.QuantityYds
            });

            angular.forEach($scope.ListHDOInfoDetails, function (dataModel) {

                if (dataModel.QuantityYds > dataModel.HoldQty) {
                    dataModel.QuantityYds = dataModel.HoldQty;
                    dataModel.RemainingQty = 0;
                }
                if (dataModel.QuantityYds < 0) {
                    dataModel.QuantityYds = 0;
                    dataModel.RemainingQty = dataModel.HoldQty;
                }
                if (angular.isUndefined(dataModel.QuantityYds)) {
                    dataModel.RemainingQty = dataModel.HoldQty;
                }

            });
        }
        //*******--------End-------*************

        //**********----------------Create New Record-----------***************
        $scope.save = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.loggedCompany = $scope.lstCompanyList;
            objcmnParam.SelectedDepartmentID = $scope.lstDeptList;

            var FactoryDeliveryOrdersMaster = {
                MHDOID: $scope.HDOID,
                MPartyID: $scope.UserID,
                MDeliveryTo: $scope.DeliveredTo,
                MBillNo: $scope.InvoiceNo,
                MBillDate: $scope.DtInvoice == "" ? "1/1/1900" : conversion.getStringToDate($scope.DtInvoice),
                TruckNo: $scope.TruckNo,
                BuyerContactName: $scope.BuyerContactName,
                BuyerContactPhoneNo: $scope.BuyerContactPhoneNo,
                DriverName: $scope.DriverName,
                DriverPhoneNo: $scope.DriverPhoneNo
            };

            //*************----Detail Data---**************
            debugger
            $scope.FDODetailsList = [];
            angular.forEach($scope.ListHDOInfoDetails, function (item) {
                if (item.Selected == true) {
                    $scope.FDODetailsList.push({
                        HDODetailID: item.HDODetailID, PIID: item.PIID, ItemID: item.ItemID, Selected: item.Selected, UnitPrice: item.UnitPrice,
                        Quantity: item.Quantity, BatchId: item.BatchId, RemainingQty: item.RemainingQty, ItemGradeID: item.ItemGradeID,
                        LotId: item.LotId, Amount: item.Amount, Roll: item.Roll, GrossQuantityKg: item.GrossQuantityKg,
                        QuantityYds: item.QuantityYds, NetQuantityKg: item.NetQuantityKg
                    });
                }
            })

            if ($scope.FDODetailsList.length < 1) {
                Command: toastr["warning"]("No Row is Selected!!!!");
                return;
            }

            ModelsArray = [FactoryDeliveryOrdersMaster, $scope.FDODetailsList, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateFactorySalesDeliveryOrderMasterDetail/';
            var FDODetailsCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.post);
            FDODetailsCreateUpdate.then(function (response) {
                if (response.data.result == '1') {
                    Command: toastr["warning"]("Input Quantity is Invalid. Please Check Stock and Try Again!");
                }
                else if (response.data.result == '') {
                        Command: toastr["warning"]("Data Not Saved, Please Check and Try Again!");
                }
                else {
                    $scope.clear();
                    $scope.FDONo = response.data.result;
                    Command: toastr["success"]("Data Saved Successfully!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Data Not Saved, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*******************-------End------**********************

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmFactoryDeliveryOrder.$setPristine();
            $scope.frmFactoryDeliveryOrder.$setUntouched();
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.btnSaleShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsShowSave = true;
            $scope.HDONo = "";
            $scope.HoDONo = "";
            $scope.BuyerName = "";
            $scope.DeliveredTo = "";
            $scope.UserId = "";
            $scope.lstDeptList = "";
            $scope.lstCompanyList = "";
            $scope.FDONo = "";
            $scope.InvoiceNo = "";
            $scope.listDOMaster = "";
            $scope.ListHDOInfoDetails = "";
            $scope.DtInvoice = conversion.NowDateCustom();
            $scope.btnSaleSaveText = "Save";
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsHiddenFDO = true;
            $scope.selectedAll = false;
            angular.forEach($scope.ListHDOInfoDetails, function (dataModel) {
                dataModel.Selected = $scope.selectedAll;
            });

            $scope.loadCompanyRecords(0);
            $scope.loadDeptdll(0);
        };
    }]);