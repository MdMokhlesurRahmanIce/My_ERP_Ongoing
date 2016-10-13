app.controller('DCCtrl', ['$scope', 'crudService', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsDc = [];
        var objcmnParam = {};

        var baseUrl = '/Commercial/api/DC/';
        var lCControllerUrl = '/Sales/api/LC/';

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        $scope.FullFormateDate = [];
        $scope.listDCMaster = [];

        $scope.btnDCSaveText = "Save";
        $scope.btnDCShowText = "Show List";
        $scope.PageTitle = 'Delivery Challan Creation';
        $scope.ListTitle = 'Delivery Challan Information';
        $scope.ListTitleDCMaster = 'Delivery Challan List';
        $scope.ItemColorID = 0;
        $scope.HLCNo = "";
        $scope.HLCDate = "";

        $scope.total = 0.0;
        $scope.qtyTotal = 0.0;
        $scope.ListDCInfoDetails = [];

        //***************************************** Start Load User Common Entity ****************************************
        $scope.loadUserCommonEntity = function (num) {
            debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
            debugger
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
            //console.log($scope.UserCommonEntity);
            //console.log($scope.HeaderToken.get);
        }
        $scope.loadUserCommonEntity(0);
        //****************************************** End Load User Common Entity *****************************************

        var defaultCompanyID = "";
        function loadCompanyRecords(isPaging) {
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
        loadCompanyRecords(0);

        var date = new Date();
        $scope.DCDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();


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

        $scope.loadFDONoRecords = function (isPaging) {

            $scope.listFDO = [];
            $scope.lstFDONo = '';
            $scope.ShowTotalFDOQty = '';

            if (angular.isUndefined($scope.lstCompanyList))
            {
                $scope.lstCompanyList = $scope.UserCommonEntity.loggedCompnyID;
            }

            objcmnParam = {
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList
            };

            var apiRoute = baseUrl + 'GetAllFDONo/';
            var listFDO = crudService.getModelDc(apiRoute, objcmnParam, $scope.HeaderToken.get);

            listFDO.then(function (response) {
                $scope.listFDO = response.data.objListFDONo;

                $("#FDOList").select2("data", { id: 0, text: '--Select FDO--' });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadFDONoRecords(0);

        ////// to add to list to table


        $scope.AddItemToList = function () {

            $scope.IsDetailShow = true;

           // var newAddTotal = parseFloat($scope.qtyTotal) + parseFloat($scope.QuantityYds);
            //if ($scope.ShowTotalFDOQty >= newAddTotal) {
                $scope.ListDCInfoDetails.push({ FDOID: $scope.lstFDONo, FDONo: $("#FDOList").select2('data').text, TruckNo: $scope.TruckNo, QuantityYds: $scope.QuantityYds, Roll: $scope.Roll });
                $scope.TruckNo = '';
                $scope.Roll = '';
                $scope.QuantityYds = '';

                $scope.getTotalYds();
            //}
            //else {
            //    // alert('Challan Quantity can not be greater that FDO quantity');
            //    Command: toastr["warning"]("Challan Quantity can not be greater that FDO quantity !!!!");
            //}
        }

        $scope.getTotalYds = function () {

            $scope.qtyTotal = 0.0;
            $scope.rollTotal = 0.0;

            angular.forEach($scope.ListDCInfoDetails, function (item) {
                var productQY = item.QuantityYds;
                var productRoll = item.Roll;
                $scope.qtyTotal = parseFloat(productQY) + parseFloat($scope.qtyTotal);
                $scope.rollTotal = parseFloat(productRoll) + parseFloat($scope.rollTotal);
            });
        }

        $scope.save = function () {

            //if ($scope.qtyTotal != $scope.ShowTotalFDOQty) {
            //    Command: toastr["warning"]("Challan Quantity and FDO quantity must be equal !");
            //    return;
            //}

            var StringDCDate = $scope.DCDate;
            var SplitedDate = StringDCDate.split("-");
            var Day = SplitedDate[0];
            var Month = SplitedDate[1];
            var Year = SplitedDate[2];
            var DCDate = Month + "-" + Day + "-" + Year;

            var DCInfo = {
                DCID: $scope.DCID,

                BankID: $scope.lstBankList,
               // FDOID: $scope.lstFDONo,

                DCNo: $scope.DCNo,
                TransactionTypeID: 4,
              //  QuantityYds: $scope.QuantityYds,
               // Roll: $scope.Roll,
               // TruckNo: $scope.TruckNo,
                CompanyID: $scope.lstCompanyList,
                CreateBy: $scope.UserCommonEntity.loggedUserID,

                DCDate: $filter('date')(new Date(), DCDate)

            };

            var DCDetailList = [];
            DCDetailList = $scope.ListDCInfoDetails;


            objcmnParam = {
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList
            };


            var apiRoute = baseUrl + 'SaveUpdateDC/';

            var DCInfoSaveUpdate = crudService.postMasterDetail(apiRoute, DCInfo, DCDetailList, objcmnParam, $scope.HeaderToken.post);
            DCInfoSaveUpdate.then(function (response) {
                if (response.data != "") {
                    $scope.DCNo = response.data;
                    $scope.loadFDONoRecords(0);
                    Command: toastr["success"]("Save  Successfully!!!!");
                    $scope.IsDetailShow = false;
                }
                $scope.clear();
            },
        function (error) {
            console.log("Error: " + error);
        });
        };

        ////**********----Reset Record----***************
        $scope.clear = function () {

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.btnDCShowText = "Show List";

            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShow 

            $("#BankList").select2("data", { id: 0, text: '--Select Bank--' });
            $("#FDOList").select2("data", { id: 0, text: '--Select FDO--' });

            var date = new Date();
            $scope.DCDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.ListDCInfoDetails = [];
            $scope.TruckNo = '';
            $scope.QuantityYds = '';
            $scope.Roll = '';
            $scope.qtyTotal = 0;
            $scope.rollTotal = '';
            $scope.ShowTotalFDOQty = '';

            $scope.btnDCSaveText = "Save";
        };

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsDetailShow = false;

        $scope.ShowHide = function () {

            $scope.IsHidden = $scope.IsHidden ? false : true;
            $scope.IsShow = $scope.IsShow ? false : true;
            if ($scope.IsHidden == true) {
                $scope.btnDCShowText = "Show List";
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnDCShowText = "Create";
                $scope.pagination.pageNumber = 1;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.IsDetailShow = false;
                $scope.gridOptionsDc.enableFiltering = true;
                loadAllDCMasterRecords(0);
            }
        }

        $scope.loadTotalFDOQty = function (lstFDONo) {
            //$scope.ListPIInfoDetails = [];
            var id = $scope.lstFDONo;

            var apiRoute = baseUrl + 'GetFDOQty/' + id;

            var fDOQty = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            fDOQty.then(function (response) {
                $scope.ShowTotalFDOQty = response.data[0].QuantitYds;
                $scope.TotalFDOQty = response.data[0].QuantitYds;

                $scope.TruckNo = response.data[0].TruckNo;
                $scope.QuantityYds = response.data[0].QuantitYds;
                $scope.Roll = response.data[0].RollNo;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.deleteRow = function (index) {
            // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
            $scope.ListDCInfoDetails.splice(index, 1);
            $scope.getTotalYds();

        };

        //************************************************Start Show DC List Information Dynamic Grid******************************************************
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
                loadAllDCMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadAllDCMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadAllDCMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadAllDCMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadAllDCMasterRecords(1);
                }
            }
        };

        loadAllDCMasterRecords = function (isPaging) {
            $scope.gridOptionsDc.enableFiltering = true;

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
            $scope.gridOptionsDc = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                columnDefs: [
                    { name: "DCID", displayName: "DC ID", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DCNo", title: "DC No", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DCDate", displayName: "DC Date", cellFilter: 'date:"dd-MM-yyyy"', width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QuantityYds", displayName: "Total Quantity (Yards)", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: 100,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.getLCDetailInfo(row.entity)">' +
                                        '<i class="icon-check"></i> Select' +
                                      '</a>' +
                                      '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsDc.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsDc.useExternalPagination = false;
                        $scope.gridOptionsDc.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            //var apiRoute = baseUrl + 'GetBallWarpingMaster/';
            //var listBallWarpingMaster = crudService.getDynamicGrid(apiRoute, objcmnParam);
            //listBallWarpingMaster.then(function (response) {
            //    $scope.pagination.totalItems = response.data.recordsTotal;
            //    $scope.gridOptionsDc.data = response.data.objvmBallWarping;
            //    $scope.loaderMore = false;
            //},
            //function (error) {
            //    console.log("Error: " + error);
            //});


            var apiRoute = baseUrl + 'GetDCMaster/';
            var listDCMaster = crudService.getModelDc(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listDCMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsDc.data = response.data.objSalDCMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        loadAllDCMasterRecords(0);
        //************************************************End Show DC List Information Dynamic Grid******************************************************

    }]);
