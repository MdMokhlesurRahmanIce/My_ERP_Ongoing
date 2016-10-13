app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);

app.controller('LCCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Sales/api/LC/';
        var deliveryController = '/Sales/api/HeadOfficeSalesDeliveryOrderEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsLC = [];
        $scope.gridOptionsPID = [];

        $scope.loaderMore = false;
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.bool = true;
        $scope.ListCompany = [];
        $scope.ListBuyer = [];
        $scope.listLCMaster = [];

        $scope.btnSaleSaveText = "Save";
        $scope.btnSaleShowText = "Show List";
        $scope.PageTitle = 'Letter of Credit (LC) Creation';
        $scope.ListTitle = 'Pending PI for this Buyer';
        $scope.ListTitleLCMaster = 'LC List';
        $scope.IsHideSave = false;
        $scope.ItemColorID = 0;
        $scope.HLCNo = "";
        $scope.HLCDate = "";
        $scope.ListTitlePIDeatails = '';
        $scope.LCDate = conversion.NowDateCustom();
        $scope.ExpiryDate = conversion.NowDateCustom();
        $scope.ShipmentDate = conversion.NowDateCustom();
        $scope.ExportLCNoDate = conversion.NowDateCustom();
        $scope.RefNoDate = conversion.NowDateCustom();
        $scope.CircularNoDate = conversion.NowDateCustom();
        var defaultCompanyID = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);        
        //****************************************************End Common Task for all***************************************************

        //**********----Get All Record----***************
        $scope.loadCompanyRecords = function (isPaging) {
            var apiRoute = '/Sales/api/PI/GetPICompany/';
            var listCompany = crudService.getUserWiseCompany(apiRoute, $scope.UserCommonEntity.loggedUserID, $scope.HeaderToken.get);
            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    if (item.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                        defaultCompanyID = item.CompanyID;
                        $scope.lstCompanyList = item.CompanyID;
                        $("#CompanyList").select2("data", { id: item.CompanyID, text: item.CompanyName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadCompanyRecords(0);

        var isCompanyChanged = 0;
        $scope.onChangedCompany = function () {            
            isCompanyChanged = 1;
            $scope.clear();
            $scope.selectedAll = false;
            $scope.loadPendingPIRecords(0);
        }

        function loadBuyerRecords(isPaging) {

            var apiRoute = baseUrl + 'GetBuyer/';
            var ListBuyer = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            ListBuyer.then(function (response) {
                $scope.ListBuyer = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBuyerRecords(0);

        function loadBankRecords(isPaging) {

            var apiRoute = baseUrl + 'GetBank/';
            var ListBank = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            ListBank.then(function (response) {
                $scope.ListBank = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBankRecords(0);

        $scope.loadLCBankBranch = function (dataModel) {
            $scope.ListLCBankBranch = [];
            $scope.lstLCBankBranchList = '';
            $("#LCBankBranchList").select2("data", { id: '', text: '--Select Branch--' });
            debugger
            var id = $scope.lstLCBankList;
            var apiRoute = baseUrl + 'GetBankBranchById/' + id;
            var ListLCBankBranch = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListLCBankBranch.then(function (response) {
                $scope.ListLCBankBranch = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.loadMasterBankBranch = function (dataModel) {

            $scope.ListMasterBankBranch = [];
            $scope.lstMasterBankBranchList = '';
            $("#MasterBankBranchList").select2("data", { id: '', text: '--Select Branch--' });

            var id = $scope.lstMasterBankList;
            var apiRoute = baseUrl + 'GetBankBranchById/' + id;
            var ListMasterBankBranch = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListMasterBankBranch.then(function (response) {
                $scope.ListMasterBankBranch = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.loadPendingPIRecords = function (lstBuyerList) {
            $scope.IsPIListShow = true;
            $scope.ListPIInfoDetails = [];
            $scope.selectedAll = false;
            var id = $scope.lstBuyerList;
            var companyId = $scope.lstCompanyList;

            var apiRoute = baseUrl + 'GetPendingPI/';

            var ListPIInfoDetails = crudService.getModelByCompanyID(apiRoute, id, companyId, $scope.HeaderToken.get);
            ListPIInfoDetails.then(function (response) {
                $scope.ListPIInfoDetails = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        function loadSightRecords(isPaging) {
            var apiRoute = baseUrl + 'GetPISight/';
            var listSight = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listSight.then(function (response) {
                $scope.listSight = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSightRecords(0);

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsPIListShow = false;
        $scope.ShowHide = function () {
            debugger
            $scope.IsHidden = $scope.IsHidden ? false : true;
            $scope.IsShow = $scope.IsShow ? false : true;
            if ($scope.IsHidden == true) {
                $scope.btnSaleShowText = "Show List";
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;

                $scope.clear(); // When at list page entry data should be cleared.
            }
            else {
                $scope.btnSaleShowText = "Create";
                $scope.pagination.pageNumber = 1;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.IsPIListShow = false;
                $scope.IsPIListShow = false;
                loadAllLCMasterRecords(0);

                $scope.btnSaleSaveText = "Save";

                /// for clearing form data and make save button disable /////////////////////
                $scope.ODInterest = '';
                $scope.GarmentsQTY = '';
                $scope.LCAmount = '';
                $scope.lstMasterBankBranchList = '';
                $scope.lstLCBankBranchList = '';
                $scope.lstLCBankList = '';
                $scope.lstMasterBankList = '';
                $scope.lstSightList = '';

                $("#LCOpenBankList").select2("data", { id: '', text: '--Select Bank--' });
                $("#MasterBankList").select2("data", { id: '', text: '--Select Bank--' });
                $("#ddlSight").select2("data", { id: '', text: '--Select Sight--' });
                $("#LCBankBranchList").select2("data", { id: '', text: '--Select Branch--' });
                $("#MasterBankBranchList").select2("data", { id: '', text: '--Select Branch--' });

                $scope.dtShipmentDate = conversion.NowDateCustom();
                $scope.dtLCDate = conversion.NowDateCustom();
                $scope.dtExpiryDate = conversion.NowDateCustom();
                $scope.dtCircularDate = conversion.NowDateCustom();
                $scope.dtExportLCDate = conversion.NowDateCustom();
                $scope.dtReferenceDate = conversion.NowDateCustom();

                $scope.ListPIInfoDetails = [];
            }
        }
        
        //************************************************Start Show LC List Information Dynamic Grid******************************************************
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
                loadAllLCMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadAllLCMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadAllLCMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadAllLCMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadAllLCMasterRecords(1);
                }
            }
        };

        loadAllLCMasterRecords = function (isPaging) {
            debugger
            $scope.gridOptionsLC.enableFiltering = true;
            $scope.gridOptionsLC.showColumnFooter = true;
          //  $scope.gridOptionsLC.showGridFooter = true;

            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                id: 0
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsLC = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
               // showGridFooter: true,
                showColumnFooter: true,
                columnDefs: [
                    { name: "LCID", displayName: "LC ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCReferenceNo", displayName: "Ref No", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCNo", displayName: "LC No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCDate", displayName: "LC Date", cellFilter: 'date:"dd-MM-yyyy"', width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerName", displayName: "Buyer Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShipmentDate", displayName: "Shipment Date", width: '12%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: "SightName", displayName: "Sight", width: '8%', headerCellClass: $scope.highlightFilteredHeader,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: none;color: #000000">Total:</div>'
                    },

                    {
                        name: "LCAmount", displayName: "Amount($)", width: '10%', cellFilter: 'number: 2', headerCellClass: $scope.highlightFilteredHeader,
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                        aggregationHideLabel: true
                    },                   
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: 100,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                      '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                                            '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                                        '</a>' +
                                    '</span>' +
                                    '<span class="label label-warning label-mini" ng-if="grid.appScope.UserCommonEntity.EnableUpdate">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.getLCDetailInfo(row.entity)">' +
                                        '<i class="icon-check"></i> Select' +
                                      '</a>' +
                                      '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsLC.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsLC.useExternalPagination = false;
                        $scope.gridOptionsLC.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetLCMaster/';
            var listLCMaster = crudService.getLCMaster(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listLCMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsLC.data = response.data.objvmSalLCDetail;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadAllLCMasterRecords(0);
        //************************************************End Show LC  List Information Dynamic Grid******************************************************

        $scope.save = function () {
            $("#save").prop("disabled", true);
            var LCInfo = {
                LCID: $scope.LCID,
                LCOpenBank: $scope.lstLCBankList,
                LCMasterBank: $scope.lstMasterBankList,
                LCOpenBranch: $scope.lstLCBankBranchList,
                LCMasterBranch: $scope.lstMasterBankBranchList,
                DocPrepDays: $scope.DocPrepDays,
                MasterLCNO: $scope.MasterLCNO,
                ODInterest: $scope.ODInterest,
                Sight: $scope.lstSightList,
                TransactionTypeID: 1,

                GarmentsQTY: $scope.GarmentsQTY,
                IRCNo: $scope.IRCNo,
                LCAmount: $scope.LCAmount,
                SalesContractNo: $scope.SalesContractNo,
                CircularNo: $scope.CircularNo,
                Remarks: $scope.Remarks,
                VATRegNo: $scope.VATRegNo,
                CentralBankRefNo: $scope.CentralBankRefNo,
                CompanyID: $scope.lstCompanyList,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                BuyerID: $scope.lstBuyerList,
                LCNo: $scope.LCNo,
                ExportLCNo: $scope.ExportLCNo,
                ReferenceNo: $scope.ReferenceNo,
                LCReferenceNo: $scope.LCReferenceNo,
                IsActive: true,
                ExpiryDate: $scope.ExpiryDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ExpiryDate),
                ExportLCNoDate: $scope.ExportLCNoDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ExportLCNoDate),
                ShipmentDate: $scope.ShipmentDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ShipmentDate),
                LCDate: $scope.LCDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.LCDate),
                CircularNoDate: $scope.CircularNoDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.CircularNoDate),
                RefNoDate: $scope.RefNoDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.RefNoDate)

            };

            var fileList = [];
            angular.forEach($scope.files, function (item) {
                this.push(item.name);
            }, fileList);

            //if (fileList.length == 0) {
            //    $("#save").prop("disabled", false);
            //    Command: toastr["warning"]("Please attach LC document.");
            //    return;
            //}

            var LCDetailList = [];
            LCDetailList = $filter('filter')($scope.ListPIInfoDetails, { Selected: 'true' });

            if (LCDetailList.length == 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Please select Proforma Invoice.");
                return;
            }

            objcmnParam = {
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
            };

            var apiRoute = baseUrl + 'saveLC/';
            var LCInfoSaveUpdate = crudService.saveLC(apiRoute, LCInfo, LCDetailList, fileList, objcmnParam, $scope.HeaderToken.post);
            LCInfoSaveUpdate.then(function (response) {
                if (response.data != "") {
                    ///// start file upload/////////////
                    var data = new FormData();
                    for (var i in $scope.files) {
                        data.append("uploadedFile", $scope.files[i]);
                    }
                    data.append("uploadedFile", response.data);
                    // ADD LISTENERS.
                    var objXhr = new XMLHttpRequest();
                    var apiRoute = baseUrl + 'UploadFiles/';
                    objXhr.open("POST", apiRoute);
                    objXhr.send(data);
                    debugger;
                    document.getElementById('file').value = '';
                    $scope.files = [];
                    /////////// end file upload /////////////////
                    $scope.LCReferenceNo = response.data;
                    Command: toastr["success"]("Save  Successfully!!!!");
                }
                $scope.clear();
            },
        function (error) {
            $("#save").prop("disabled", false);
            Command: toastr["warning"]("Save Not Successfull!!!!");
            $scope.files = [];
        });
        };

        //************************************************Start PI Detail Dynamic Grid******************************************************
        $scope.CallgetPIDetails = function (dataModel) {
            var tempDataModel = [];

            if (dataModel.length != 0) {
                tempDataModel = dataModel.PIID;
            }
            else {
                tempDataModel = 0;
            }
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
            debugger;
            //$scope.pagination.pageNumberFdod = 1;
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

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
               // showGridFooter: true,
                showColumnFooter: true,
                columnDefs: [
                   { name: "PIDetailID", displayName: "PI DetailID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '7%',
                   { name: "PIID", displayName: "PIID", width: '7%', visible: false, headerCellClass: $scope.highlightFilteredHeader },
                   { name: "ItemName", displayName: "Article No", title: "Article No", width: '13%', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                   { name: "Construction", displayName: "Construction", title: "Construction", width: '21%', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                   { name: "Description", displayName: "Description", title: "Description", width: '23%', headerCellClass: $scope.highlightFilteredHeader },//width: '10%', 
                   {
                       name: "CuttableWidth", displayName: "Cuttable Width", title: "Cuttable Width", width: '12%', headerCellClass: $scope.highlightFilteredHeader,
                       footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: none;color: #000000">Total:</div>'
                   },
                   {
                       name: "Quantity", displayName: "Quantity", title: "Quantity", cellFilter: 'number: 2', width: '8%', headerCellClass: $scope.highlightFilteredHeader,
                       aggregationType: uiGridConstants.aggregationTypes.sum,
                       footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                       aggregationHideLabel: true
                   },
                   {
                       name: "UnitPrice", displayName: "Unit Price", title: "Unit Price", cellFilter: 'number: 2', width: '8%', headerCellClass: $scope.highlightFilteredHeader
                   },
                   {
                       name: "Amount", displayName: "Amount", title: "Amount", cellFilter: 'number: 2', width: '7%', headerCellClass: $scope.highlightFilteredHeader,
                       aggregationType: uiGridConstants.aggregationTypes.sum,
                       footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | number:2 }}</div>',
                       aggregationHideLabel: true
                   }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.paginationPI.totalItems, pagination.sort)
                    .then(function () {
                        debugger
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
            var apiRoute = deliveryController + 'GetPIDetailsById/';
            var ListPIDetails = crudService.getModelHDO(apiRoute, objcmnParam, $scope.HeaderToken.get);
            ListPIDetails.then(function (response) {

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

        $scope.getFileInfo = function (dataModel) {
            $scope.ListFileDetails = [];
            var id = dataModel.LCID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id;
            var ListFileDetails = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
        function (error) {
            console.log("Error: " + error);
        });
        }

        $scope.toggleSelection = function () {
            var k = 0;
            var index = 0;
            var TotalLCAmount = 0;
            angular.forEach($scope.ListPIInfoDetails, function (dataModel) {
                if (dataModel.Selected) {
                    debugger

                    index = $scope.ListPIInfoDetails.indexOf(dataModel);

                    TotalAmount = $scope.ListPIInfoDetails[index].TotalAmount;
                    TotalLCAmount = TotalLCAmount + TotalAmount;
                    k = k + 1;
                }
            });
            $scope.LCAmount = TotalLCAmount;
        };

        $scope.checkAll = function () {
            if ($scope.selectedAll) {
                $scope.selectedAll = true;
            } else {
                $scope.selectedAll = false;
            }
            var k = 0;
            var TotalLCAmount = 0;
            angular.forEach($scope.ListPIInfoDetails, function (dataModel) {
                dataModel.Selected = $scope.selectedAll;
                // code for lcamount

                if ($scope.selectedAll) {
                    TotalAmount = $scope.ListPIInfoDetails[k].TotalAmount;
                    TotalLCAmount = TotalLCAmount + TotalAmount;
                    k = k + 1;
                }
                else {
                    TotalLCAmount = 0;
                }
                // end code for lcamount
            });
            $scope.LCAmount = TotalLCAmount;
        };

        $scope.getLCDetailInfo = function (dataModel) {
            $scope.IsShow = true;
            $scope.IsHidden = true;
            $scope.IsPIListShow = true;
            $scope.btnSaleShowText = "Show List";
            $scope.btnSaleSaveText = "Amendment";

            $scope.IsHideSave = dataModel.IsHDOCompleted == true ? true : false;//Hiding amendment option

            $scope.bool = false;
            // for loading lc open branch

            var LCOpenBankID = dataModel.LCOpenBank;

            //var id = LCOpenBankID;
            var apiRoute = baseUrl + 'GetBankBranchById/' + LCOpenBankID;
            var ListLCBankBranch = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListLCBankBranch.then(function (response) {
                $scope.ListLCBankBranch = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });

            // for loading lc master branch
            var LCMasterBankID = dataModel.LCMasterBank;
            var apiRoute = baseUrl + 'GetBankBranchById/' + LCMasterBankID;
            var ListMasterBankBranch = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListMasterBankBranch.then(function (response) {
                $scope.ListMasterBankBranch = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });

            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: 1,    //isPaging
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList
            };


            objcmnParam.id = dataModel.LCID;

            $scope.ListLCInfoMaster = [];
            $scope.ListLCInfoDetails = [];
            var LCID = dataModel.LCID;
            var apiRouteMaster = baseUrl + 'GetLCMasterById/' + LCID;
            var apiRouteDetails = baseUrl + 'GetLCDetailByID/';
            //---------------------------------------
            var ListLCInfoMaster = crudService.getModelByID(apiRouteMaster, $scope.HeaderToken.get);
            ListLCInfoMaster.then(function (response) {
                $scope.ListLCInfoMaster = response.data;

                $scope.LCID = response.data[0].LCID;
                $scope.ODInterest = response.data[0].ODInterest;
                $scope.LCNo = response.data[0].LCNo;
                $scope.GarmentsQTY = response.data[0].GarmentsQTY;
                $scope.SalesContractNo = response.data[0].SalesContractNo;
                $scope.IRCNo = response.data[0].IRCNo;
                $scope.CircularNo = response.data[0].CircularNo;
                $scope.LCReferenceNo = response.data[0].LCReferenceNo;

                $scope.LCAmount = response.data[0].LCAmount;
                $scope.DocPrepDays = response.data[0].DocPrepDays;
                $scope.ExportLCNo = response.data[0].ExportLCNo;
                $scope.MasterLCNO = response.data[0].MasterLCNO;
                $scope.ReferenceNo = response.data[0].ReferenceNo;
                $scope.Remarks = response.data[0].Remarks;
                $scope.CentralBankRefNo = response.data[0].CentralBankRefNo;
                $scope.VATRegNo = response.data[0].VATRegNo;

                if (response.data[0].LCMasterBank != 0) {
                    $scope.lstMasterBankList = response.data[0].LCMasterBank;
                    $("#MasterBankList").select2("data", { id: response.data[0].LCMasterBank, text: response.data[0].MasterBankName });
                }
                if (response.data[0].LCMasterBranch != 0) {
                    $scope.lstMasterBankBranchList = response.data[0].LCMasterBranch;
                    $("#MasterBankBranchList").select2("data", { id: response.data[0].LCMasterBranch, text: response.data[0].MasterBranchName });
                }

                $scope.lstCompanyList = response.data[0].CompanyID;
                $("#CompanyList").select2("data", { id: response.data[0].CompanyID, text: response.data[0].CompanyName });

                $scope.lstLCBankList = response.data[0].LCOpenBank;
                $("#LCOpenBankList").select2("data", { id: response.data[0].LCOpenBank, text: response.data[0].LCOpenBankName });


                $scope.lstSightList = response.data[0].Sight;
                $("#ddlSight").select2("data", { id: response.data[0].Sight, text: response.data[0].SightName });

                $scope.lstBuyerList = response.data[0].BuyerID;
                $("#BuyerList").select2("data", { id: response.data[0].BuyerID, text: response.data[0].UserFullName });

                $scope.lstLCBankBranchList = response.data[0].LCOpenBranch;
                $("#LCBankBranchList").select2("data", { id: response.data[0].LCOpenBranch, text: response.data[0].LCOpenBranchName });              
                $scope.ExpiryDate = response.data[0].ExpiryDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].ExpiryDate);
                $scope.ShipmentDate = response.data[0].ShipmentDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].ShipmentDate);
                $scope.CircularNoDate = response.data[0].CircularNoDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].CircularNoDate);
                $scope.LCDate = response.data[0].LCDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].LCDate);
                $scope.ExportLCNoDate = response.data[0].ExportLCNoDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].ExportLCNoDate);
                $scope.RefNoDate = response.data[0].RefNoDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data[0].RefNoDate);
            },
            function (error) {
                console.log("Error: " + error);
            });

            //---------------------------------------
            //var ListPIInfoDetails = crudService.getModelByID(apiRouteDetails);

            var ListPIInfoDetails = crudService.getModelHDO(apiRouteDetails, objcmnParam, $scope.HeaderToken.get);

            ListPIInfoDetails.then(function (response) {
                $scope.ListPIInfoDetails = response.data.objLCById;
                var k = 0;
                angular.forEach($scope.ListPIInfoDetails, function (dataModel) {
                    if ($scope.ListPIInfoDetails[k].LCReferenceNo != '' && $scope.ListPIInfoDetails[k].LCReferenceNo != null) {
                        dataModel.Selected = true;
                        dataModel.Showing = true;

                        // angular.element(document.getElementById('modalCreateBtn'))[0].disabled = true;
                    }
                    else if ($scope.ListPIInfoDetails[k].LCReferenceNo == '' || $scope.ListPIInfoDetails[k].LCReferenceNo == null) {
                        dataModel.Selected = false;
                        dataModel.Showing = false;

                        // angular.element(document.getElementById('modalCreateBtn'))[0].disabled = true;
                    }
                    k = k + 1;
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.HideShowMasterSales = function (gg) {
            var selectedValue = gg;

            Command: toastr["success"]("'" + gg + "'");

        };

        $scope.clearMasterLC = function () {
            $scope.MasterLCNO = '';
        }
        $scope.clearSalesContact = function () {
            $scope.SalesContractNo = '';
        }

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
        $scope.clear = function () {

            $scope.IsPIListShow = false;
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.btnSaleShowText = "Show List";
            $scope.IsHideSave = false;
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.bool = true;
            $scope.LCID = 0;
            $scope.LCNo = '';
            $scope.ODInterest = '';
            $scope.GarmentsQTY = '';
            $scope.SalesContractNo = '';
            $scope.IRCNo = '';
            $scope.CircularNo = '';
            $scope.LCAmount = '';
            $scope.DocPrepDays = '';
            $scope.ExportLCNo = '';
            $scope.MasterLCNO = '';
            $scope.ReferenceNo = '';
            $scope.Remarks = '';
            $scope.CentralBankRefNo = '';
            $scope.VATRegNo = '';
            $scope.lstMasterBankBranchList = '';
            $scope.lstLCBankBranchList = '';
            $scope.lstLCBankList = '';
            $scope.lstMasterBankList = '';
            $scope.lstSightList = '';

            $("#LCOpenBankList").select2("data", { id: '', text: '--Select Bank--' });
            $("#MasterBankList").select2("data", { id: '', text: '--Select Bank--' });
            $("#ddlSight").select2("data", { id: '', text: '--Select Sight--' });
            $("#LCBankBranchList").select2("data", { id: '', text: '--Select Branch--' });
            $("#MasterBankBranchList").select2("data", { id: '', text: '--Select Branch--' });
            
            if (isCompanyChanged == 0) {
                $scope.lstCompanyList = "";
                $scope.lstBuyerList = '';
                $scope.loadCompanyRecords(0);
                $("#BuyerList").select2("data", { id: '', text: '--Select Party--' });
            }

            $scope.dtShipmentDate = conversion.NowDateCustom();
            $scope.dtLCDate = conversion.NowDateCustom();
            $scope.dtExpiryDate = conversion.NowDateCustom();
            $scope.dtCircularDate = conversion.NowDateCustom();
            $scope.dtExportLCDate = conversion.NowDateCustom();
            $scope.dtReferenceDate = conversion.NowDateCustom();

            $scope.ListPIInfoDetails = [];
            $scope.btnSaleSaveText = "Save";
            isCompanyChanged = 0;
        };
    }]);