/**
* GRRCtrl.js   //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;
*/ 

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);

app.controller('costTransportCtrl', ['$scope', 'gRRService', '$http', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, gRRService, $http, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsChallanMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        //GRR  For Loan   $scope.IsLoanTypeOrOthers = true;

        //$scope.IsSPRType = true;

        $scope.IsSPRType = false;
        $scope.IsLoanTypeOrOthers = true;

        // $scope.IsItemTypeFinishedGoods = false;

        // $scope.IsItemTypeRMOthers = true;  //  for  Default Raw Material or Others

        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";

        var baseUrl = '/Inventory/api/Cost/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.GRRDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.ChallanTypeID = 5;
        $scope.MenuID = 93;
        $scope.GrrID = "0";

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'Cost Entry';
        $scope.ListTitle = 'Cost Records';
        $scope.ListTitleGRRMasters = 'Cost Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleGRRDeatails = 'Listed Item of GRR';

        $scope.ListChallanDetails = [];
        $scope.ListChallanDetailsForSearch = [];

        $scope.listSalesPerson = [];

        //  $scope.btnModal = "Add";

        $scope.btnLotModal = "Save";
        $scope.btnBatchModal = "Save";
        $scope.listMrrLot = [];
        $scope.listMrrBatchNo = [];
        $scope.LotSetup = "Lot Setup";
        $scope.BatchSetup = "Batch Setup";

        function loadUserCommonEntity(num) {
            //Coming from SideNavCrl
            $scope.UserCommonEntity = {};
            $scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
            $scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
            $scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
            $scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
            $scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
            $scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
            $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
            $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);

        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;

                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.loadChallanRecords(0);
            }
        }

        $scope.listLotType = [
         { LotTypeID: 1, LotTypeName: 'Internal' },
         { LotTypeID: 2, LotTypeName: 'External' }
        ];

        $scope.listBatchType = [
          { BatchTypeID: 1, BatchTypeName: 'Internal' },
          { BatchTypeID: 2, BatchTypeName: 'External' }
        ];

        $scope.lstBatchTypeList = "2";
        $("#ddlBatchType").select2("data", { id: "2", text: "External" });

        $scope.lstLotTypeList = "2";
        $("#ddlLotType").select2("data", { id: "2", text: "External" });

        $scope.getTypes = function () {
            $scope.Types = [
                {
                    Item: 'Finish Good'
                    , Value: 1 // For Raw Material
                },
                {
                    Item: 'Raw Material'
                , Value: 2 // For Raw Material
                },
                {
                    Item: 'Yarn'
                    , Value: 3 // For Yarn
                },
            {
                Item: 'Chemical'
                , Value: 5 // For Chemical
            }, {
                Item: 'Fixed Asset'
                , Value: 4 // For Fixed Asset
            }];

            $scope.ItemType = "2";
            $('#ddlItemType').select2("data", { id: '2', text: 'Raw Material' });


        }
        $scope.getTypes();

        //*******************   Item Group   On Page Load--  ***********

        //$scope.getItemGroupsByType = function () {

        //    $scope.listSampleNo = "";
        //    $scope.lstSampleNoList = "";
        //    $('#ddlSampleNo').select2("data", { id: '2', text: '--Select Item Group--' });

        //    var apiRoute = '/SystemCommon/api/RawMaterial/GetItemGroups/';
        //    if ($scope.ItemType != "") {
        //        var itemGroupes = gRRService.getAllItemGroup(apiRoute, page, pageSize, isPaging, $scope.ItemType, LoggedCompanyID);
        //        itemGroupes.then(function (response) {
        //            $scope.listSampleNo = response.data;
        //        },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //    }
        //    else {
        //        $scope.listSampleNo = "";
        //    }
        //}

        //$scope.getItemGroupsByType();


        $scope.listMrrBatchNo = [];
        //------- Get All Batch //--------
        function GetAllBatchNo() {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data;
                $scope.listMrrBatchNo = response.data;
                $scope.listBatchInLot = response.data;
                //if (varBatchID != "" && varBatchName != "")
                //{
                //    $scope.Batch = varBatchID;
                //    $('#ddlBatch').select2("data", { id: varBatchID, text: varBatchName });

                //    varBatchID = "";
                //    varBatchName = "";
                //}

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllBatchNo();


        $scope.listMrrLot = [];

        //**********----Get All Lot Record----***************
        function loadLotRecords(isPaging) {
            var apiRoute = '/Inventory/api/StockEntry/GetLotNo/';
            var lisLotNo = RequisitionService.getAllLotNo(apiRoute, page, pageSize, isPaging);
            lisLotNo.then(function (response) {
                $scope.listLot = response.data;
                $scope.listMrrLot = response.data;
                //if (varLotID != "" && varLotName != "") {
                //    $scope.listLot = [];
                //    $scope.lstLot = "";
                //    $('#ddlLotNo').select2("data", { id: "", text: "" });
                //    $scope.listLot = response.data;
                //    $scope.lstLot = varLotID;
                //    $('#ddlLotNo').select2("data", { id: varLotID, text: varLotName });

                //   // varLotID = "";
                //  //  varLotName = "";
                //}

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadLotRecords(0);


        // **********---- Get Wherehouse Records ----*************** //


        function loadWherehouse(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: 76,
                tTypeId: 25
            };

            var apiRoute = '/Inventory/api/MRR/GetWherehouseList/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            $scope.listWarehouse = [];
            var listWherehouse = mRRService.GetList(apiRoute, cmnParam);
            listWherehouse.then(function (response) {
                //  $scope.listWarehouse = response.data.lstWherehouse;
                // $scope.listWarehouse = response.data.lstWherehouse[1].ListDpt;

                $scope.listWarehouse.push({ OrganogramID: response.data.lstWherehouse[1].ListDpt[5].OrganogramID, OrganogramName: response.data.lstWherehouse[1].ListDpt[5].OrganogramName })

                //    .lstWherehouse[1].ListDpt;
                //$scope.listWarehouseLocation = response.data.lstWherehouse.ListDpt.ListBrn;
                //$scope.listSelfNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf;
                //$scope.listRackNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf.ListRack;
                // console.log(response.data.lstWherehouse);
            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadWherehouse(0);



        //**********---- Get All Party Records ----*************** //
        function loadPartyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetParty/';
            var listParty = gRRService.getModel(apiRoute, page, pageSize, isPaging);
            listParty.then(function (response) {
                $scope.listParty = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadPartyRecords(0);

        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetCurrency/';
            var listCurrency = gRRService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);

        
        //**********---- Accessment Cost ----***************************************************************************//
        
        $scope.AccessmentDetail= [];

        $scope.FirstTimeRowLoad = function () {
            if ($scope.AccessmentDetail.length <= 0) {
                for (var i = 0; i <= 3; i++) {
                    $scope.AccessmentDetail.push({ AccessmentCostDetailID:0,  TaxID:0, TaxValue:0});
                }
            }
        };
        $scope.FirstTimeRowLoad();

        $scope.addNewRow = function () {
            $scope.AccessmentDetail.push({ AccessmentCostDetailID: 0,  TaxID: 0, TaxValue: 0 });
        };

        $scope.RowRemove = function (index) {
           // $scope.AccessmentDetail.splice($scope.AccessmentDetail.indexOf($scope.Accessment), 1);

            $scope.AccessmentDetail.splice(index, 1);

        };
        

        // --------------------------- Clearing Cost ------------------------------------------

        $scope.ClearingDetail = [];

        $scope.FirstTimeRowLoadClearing = function () {
            if ($scope.ClearingDetail.length <= 0) {
                for (var i = 0; i <= 3; i++) {
                    $scope.ClearingDetail.push({ ClearingCostDetailID: 0, ConsumerChargeTypeID: 0, Amount: 0 });
                }
            }
        };
        $scope.FirstTimeRowLoadClearing();


        $scope.addNewRowClearing = function () {
            $scope.ClearingDetail.push({ ClearingCostDetailID: 0, ConsumerChargeTypeID: 0, Amount: 0 });
        };


        $scope.RowRemoveClearing = function (index) {
            // $scope.AccessmentDetail.splice($scope.AccessmentDetail.indexOf($scope.Accessment), 1);

            $scope.ClearingDetail.splice(index, 1);

        };



        // --------------------------- Transport Cost ------------------------------------------

        $scope.TransportDetail = [];

        $scope.FirstTimeRowLoadTransport = function () {
            if ($scope.TransportDetail.length <= 0) {
                for (var i = 0; i <= 3; i++) {
                    $scope.TransportDetail.push({ TransportCostDetailID: 0, VehicleID: 0, FarePerVehicle: 0 });
                }
            }
        };
        $scope.FirstTimeRowLoadTransport();


        $scope.addNewRowTransport = function () {
            $scope.TransportDetail.push({ TransportCostDetailID: 0, VehicleID: 0, FarePerVehicle: 0 });
        };


        $scope.RowRemoveTransport = function (index) {
            // $scope.AccessmentDetail.splice($scope.AccessmentDetail.indexOf($scope.Accessment), 1);
            $scope.TransportDetail.splice(index, 1);
        };




        $scope.GetList = function (apiRoute, cmnParam) {
            var request = $http({
                method: "post",
                url: apiRoute,
                data: cmnParam,
                dataType: "json",
                contentType: "application/json"
            });
            return request;
        }

        //**********----Get All Record----***************
        var urlGet = '';
        $scope.getModel = function (apiRoute, page, pageSize, isPaging) {
            urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
            return $http.get(urlGet);
        }


        function loadTaxGroup(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var apiRoute = baseUrl + 'GetTaxCategory/';
            var listPurchaseTaxCategory = $scope.GetList(apiRoute, cmnParam);
            listPurchaseTaxCategory.then(function (response) {
                $scope.listPurchaseTaxCategory = response.data.objPurchaseTaxCategory;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadTaxGroup(0);

        
        $scope.GetTaxTypeByTaxCategoryId = function () {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: 0,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId:0 // Accessment.TaxCategoryID
            };

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var apiRoute = baseUrl + 'GetTaxTypeByCategoryId/';
            var listPurchaseTax = $scope.GetList(apiRoute, cmnParam);
            listPurchaseTax.then(function (response) {
                $scope.listPurchaseTax = response.data.objPurchaseTax;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.GetTaxTypeByTaxCategoryId();


        $scope.GetPOList = function () {
            
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: 0,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 0// Accessment.TaxCategoryID
            };

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var apiRoute = baseUrl + 'GetPurchaseOrderList/';
            var listPONo = $scope.GetList(apiRoute, cmnParam);
            listPONo.then(function (response) {
                $scope.listPONo = response.data.objPurchasePOMaster;
                console.log($scope.listPONo);
            },
                function (error) {
                    console.log("Error: " + error);
                });

        };
        $scope.GetPOList();
        
        $scope.LoadInfoByPOID = function () {
            
            objcmnParam = {
                CompanyID:1,
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: 0,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: $scope.PONo
            };

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var apiRoute = baseUrl + 'GetCostInfoByPOID/';
            var listCostInfo = $scope.GetList(apiRoute, cmnParam);
            listCostInfo.then(function (response) {
                
                $scope.listCostInfo = response.data.lstVmCostInfo;

                $scope.listSprNo = [];
                $scope.SprNo = '';
                $("#ddlSPRNo").select2("data", { id: '', text: '' });
                if ($scope.listCostInfo[0].RequisitionID != null) {
                    $scope.listSprNo.push({
                        RequisitionID: $scope.listCostInfo[0].RequisitionID, RequisitionNo: $scope.listCostInfo[0].RequisitionNo
                    });
                    $scope.SprNo = $scope.listCostInfo[0].RequisitionID;

                    $("#ddlSPRNo").select2("data", { id: $scope.listCostInfo[0].RequisitionID, text: $scope.listCostInfo[0].RequisitionNo });
                    $("#ddlLCNo").select2("data", { id: 0, text: $scope.listCostInfo[0].LCorVoucherorLcafNo });
                }

            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

      //  $scope.LoadInfoByPOID();


        function loadDischargeLocation(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };

            var objcmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            
            var apiRoute = baseUrl + 'GetLocation/';

            var listLocation = gRRService.GetList(apiRoute, objcmnParam);
            listLocation.then(function (response) {
                $scope.listLocation = response.data.objLocation;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadDischargeLocation(0);


        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetCurrency/';

            var listCurrency = gRRService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);
        

        //**********----Pagination Master Challan----***************
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
                $scope.loadChallanRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadChallanRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadChallanRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadChallanRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadChallanRecords(1);
                }
            }
        };

        //**********----Get All Challan Master Records----***************
        $scope.loadChallanRecords = function (isPaging) {

            $scope.gridOptionsChallanMaster.enableFiltering = true;
            $scope.gridOptionsChallanMaster.enableGridMenu = true; 
            //$scope.gridOptionsLC.enableFiltering = true;

            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMoreChallanMaster = true;
            $scope.lblMessageForChallanMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 5,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsChallanMaster = {
               
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [
                    { name: "AccessmentCostID", displayName: "AccessmentCostID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CHTypeID", displayName: "CHTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "UserID", displayName: "PartyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "ComboID", displayName: "TransactionTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "LoadingPortID", displayName: "LoadingPortID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "DischargePortID", displayName: "DischargePortID", visible: false, headerCellClass: $scope .highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CompanyID", displayName: "CompanyID", title: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "DepartmentID", displayName: "DepartmentID", title: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionID", displayName: "RequisitionID", title: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AccessmentRefDate", visible: false, displayName: "AccessmentRefDate", cellFilter: 'date:"dd-MM-yyyy"', width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AccessmentNo", displayName: "Accessment No", title: "Accessment No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "UserFullName", displayName: "Party", title: "Supplier", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "RefCHNo", displayName: "Ref ChallanNo", title: "Ref ChallanNo", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "RefCHDate", displayName: " Ref Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "SPR No", title: "SPR No", width: '20%', visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "PINo", displayName: "PI No", title: "PI No", width: '10%', visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PONo", displayName: "PO No", title: "PO No", width: '20%', visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "LCorVoucherorLcafNo", displayName: "L/C No", title: "L/C No", visible: $scope.IsSPRType, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CurrencyName", displayName: "Currency", title: "Currency", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "ComboName", displayName: "Transaction Type", title: "Transaction Type", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AccessmentDate", displayName: "Accessment Date", cellFilter: 'date:"dd-MM-yyyy"', width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Select',
                        displayName: "Select",
                        width: '10%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,

                        cellTemplate: '<span class="label label-warning label-mini">' +
                                   '<a href="" title="Select" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                     '<i class="icon-check"></i> Select' +
                                   '</a>' +
                                   '</span>'

                        //'<span class="label label-success label-mini">' +
                        //           '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                        //                 '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                        //             '</a>' +
                        //         '</span>' +

                        //cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //              '<a href="" title="Select" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                        //                '<i class="icon-check" aria-hidden="true"></i> Select' +
                        //              '</a>' +
                        //              '</span>'
                    }

                ],
                
                    exporterAllDataFn: function () {
                        return getPage(1, $scope.gridOptionsChallanMaster.totalItems, paginationOptions.sort)
                        .then(function () {
                            $scope.gridOptionsChallanMaster.useExternalPagination = false;
                            $scope.gridOptionsChallanMaster.useExternalSorting = false;
                            getPage = null;
                        });
                    },
            };
            

            var apiRoute = baseUrl + 'GetCostAccessmentMaster/';
            
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listChallanMaster = gRRService.GetList(apiRoute, cmnParam);

            listChallanMaster.then(function (response) {

                $scope.gridOptionsChallanMaster.data = response.data.lstVmCostMaster;
                $scope.loaderMoreChallanMaster = false;
                $scope.lblMessageForChallanMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        $scope.loadChallanRecords(0);
        
        //**********----Load accessment master detail by accessment no----***************
        
        $scope.loadChallaMasterDetailsByGrrNo = function (dataModel) {

            modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            // 
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;


            //alert(dataModel.AccessmentRefDate);

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.btnSaveText = "Update";

            $scope.HGRRNo = dataModel.AccessmentNo;
            $scope.AccessmentCostID = dataModel.AccessmentCostID;
            $scope.AccessmentDate = conversion.getDateToString(dataModel.AccessmentDate);
            $scope.AccessmentRefNo = dataModel.AccessmentRefNo;
            
            $scope.AccessmentRefDate = conversion.getDateToString(dataModel.AccessmentRefDate);
            $scope.CustomRefNo = dataModel.CustomRefNo;
            $scope.DeclarantRefNo = dataModel.DeclarantRefNo;
            $scope.BondAmount = dataModel.BondAmount;
            $scope.BondDue = dataModel.BondDue;

            $scope.SROAmount = dataModel.SROAmount;
            $scope.SRODue = dataModel.SRODue;

            $scope.AccessmentDescription = dataModel.AccessmentDescription;
            $scope.DocUrl = dataModel.DocUrl;
            $scope.LCorVoucherorLcafNo = dataModel.AccessmentDescription;


            $scope.listLocation = [];
            $scope.CountryID = '';
            $("#ddlCountry").select2("data", { id: '', text: '' });
            if (dataModel.CountryID != null) {
                $scope.listLocation.push({
                    CountryID: dataModel.CountryID, text: dataModel.CountryName
                });
                $scope.CountryID = dataModel.CountryID;
                $("#ddlCountry").select2("data", { id: dataModel.CountryID, text: dataModel.CountryName });
            }

      
            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '' });
            if (dataModel.RequisitionID != null) {
                $scope.listSprNo.push({
                    RequisitionID: dataModel.RequisitionID, text: dataModel.RequisitionNo
                });
                $scope.SprNo = dataModel.RequisitionID;
                $("#ddlSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });
            }


            $scope.listPONo = [];
            $scope.PONo = '';
            $("#ddlPONo").select2("data", { id: '', text: '' });

            if (dataModel.POID != null) {
                $scope.listPONo.push({
                    POID: dataModel.POID, PONo: dataModel.PONo
                });
                $scope.PONo = dataModel.POID;
                $("#ddlPONo").select2("data", { id: dataModel.POID, text: dataModel.PONo });
            }
            

            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $("#ddlCurrency").select2("data", { id: '', text: '' });

            if (dataModel.CurrencyID != null) {
                $scope.listCurrency.push({
                    Id: dataModel.CurrencyID, text: dataModel.CurrencyName
                });

                $scope.lstCurrencyList = dataModel.CurrencyID;
                $("#ddlCurrency").select2("data", { id: dataModel.CurrencyID, text: dataModel.CurrencyName });
            }
            
            $scope.listLCNo = [];
            $scope.LCNo = '';
            $("#ddlLCNo").select2("data", { id: '', text: '' });

            if (dataModel.POID != null) {
                $scope.listLCNo.push({
                    POID: dataModel.POID, CurrencyName: dataModel.LCorVoucherorLcafNo
                });

                $scope.LCNo = dataModel.POID;
                $("#ddlLCNo").select2("data", { id: dataModel.POID, text: dataModel.LCorVoucherorLcafNo });
            }

            objcmnParam = {
                CompanyID: LoggedCompanyID,
                LoggedUser: LoggedUserID,
                PageNo: isPaging,
                RowCountPerPage: LoggedUserID,
                IsPaging: isPaging,
                tTypeId: dataModel.AccessmentCostID,
            };

            var apiRoute = baseUrl + 'GetCostAccessmentDetailByCostAccessmentId/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var ListGrrDetails = gRRService.GetList(apiRoute, cmnParam);
            
            ListGrrDetails.then(function (response) {
                $scope.AccessmentDetail = response.data.lstVmCostDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }


        //**********----Save and Update InvRChallanMaster and InvRChallanDetail  Records----***************//
        $scope.save = function () {
            $("#save").prop("disabled", true);
            var AccessmentDate = conversion.getStringToDate($scope.AccessmentDate);
            var AccessmentRefDate = conversion.getStringToDate($scope.AccessmentRefDate);
                      

            var itemMaster = {
                AccessmentCostID:$scope.AccessmentCostID,
                AccessmentNo:$scope.AccessmentNo,
                AccessmentDate: AccessmentDate,
                POID: $scope.PONo,
                RequisitionID: $scope.SprNo,
                CurrencyID: $scope.lstCurrencyList,
                CustomOfficeID: $scope.CustomOfficeID,
                AccessmentRefNo:$scope.AccessmentRefNo,
                AccessmentRefDate:AccessmentRefDate,
                CustomRefNo:$scope.CustomRefNo,
                DeclarantRefNo:$scope.DeclarantRefNo,
                BondAmount:$scope.BondAmount,
                BondDue:$scope.BondDue,
                SROAmount:$scope.SROAmount,
                SRODue:$scope.SRODue,
                AccessmentDescription: $scope.AccessmentDescription,
                DocUrl:$scope.DocUrl,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                IsDeleted: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
            };


            //var fileList = [];
            //angular.forEach($scope.files, function (item) {
            //    this.push(item.name);
            //}, fileList);

            //if (fileList.length == 0) {
            //    $("#save").prop("disabled", false);
            //    Command: toastr["warning"]("Please attach GRR document.");
            //    return;
            //}
            
            var menuID = $scope.UserCommonEntity.currentMenuID;
            var itemMasterDetail = $scope.AccessmentDetail;
            var chkAmount = 1;
            angular.forEach($scope.AccessmentDetail, function (item) {
                if (item.TaxValue <= 0) {
                    chkAmount = 0;
                }
            });


            if ($scope.AccessmentDetail.length > 0) {

                if (chkAmount == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateAccessmentCostMasterNdetails/';

                    var cmnParam = "[" + JSON.stringify(itemMaster) + "," + JSON.stringify(itemMasterDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(fileList) + "]";
                    var ChallanItemMasterNdetailsCreateUpdate = gRRService.GetList(apiRoute, cmnParam);

                    ChallanItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {

                            $scope.HGRRNo = response.data;

                            ///// start file upload/////////////

                            //var data = new FormData();

                            //for (var i in $scope.files) {
                            //    data.append("uploadedFile", $scope.files[i]);
                            //}

                            //data.append("uploadedFile", response.data);
                            //var objXhr = new XMLHttpRequest();
                     
                            
                            //var apiRoute = baseUrl + 'UploadFiles/';
                            //objXhr.open("POST", apiRoute);
                            //objXhr.send(data);

                            //document.getElementById('file').value = '';
                            //$scope.files = [];

                            /////////// end file upload /////////////////
                            
                            Command: toastr["success"]("Save  Successfully!!!!");
                            $scope.clear();
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
                }
                else if (chkAmount == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Amount Must Not Zero Or Empty !!!!");
                }
            }
            else if ($scope.ListChallanDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Accessment Detail Must Not Empty!!!!");
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
            var id = dataModel.GrrID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id;
            var ListFileDetails = gRRService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        
        $scope.clear = function () {
            
            $scope.IsSPRType = false;
            $scope.IsLoanTypeOrOthers = true;
            var date = new Date();
            $scope.GRRDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.RefChallanNo = "";

            $scope.ChallanTypeID = 5;
            $scope.MenuID = 93;
            $scope.GrrID = "0";
            $scope.HGRRNo = "";

            $scope.listParty = [];
            $scope.lstPartyList = "";
            $('#ddlParty').select2("data", { id: "", text: "--Select Party--" });

            $scope.listChallanType = [];
            $scope.lstChallanTypeList = "";
            $('#ddlChallanType').select2("data", { id: "", text: "--Select Challan Type--" });

            $scope.listSampleNo = [];
            $scope.lstSampleNoList = '';
            $('#ddlSampleNo').select2("data", { id: '', text: '--Select Sample No--' });

            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $('#ddlCurrency').select2("data", { id: '', text: '--Select Currency--' });

            $scope.listSprNo = [];
            $scope.SprNo = "";
            $('#ddlSPRNo').select2("data", { id: '', text: '--Select SPR No--' });

            $scope.listPONo = [];
            $scope.PONo = "";
            $('#ddlPONo').select2("data", { id: '', text: '--Select PO No--' });

            $scope.listPINO = [];
            $scope.PINO = "";
            $('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });

            $scope.listLCNo = [];
            $scope.LCNo = "";
            $('#ddlLCNo').select2("data", { id: '', text: '--Select LC No--' });
            
            $scope.listWarehouse = [];
            $scope.Warehouse = '';
            $('#ddlWarehouse').select2("data", { id: '', text: '--Select Wherehouse--' });


            $scope.Remarks = "";
            $scope.Description = "";
            
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.ListChallanDetails = [];
            loadPartyRecords(0);
            loadCurrencyRecords(0);
            loadWherehouse(0);
            loadPackingUnit(0);
            loadWeightUnit(0);
            $scope.loadChallanRecords(0);
            $scope.getTypes();

        };

    }]);

function modal_fadeOut() {
    $("#ChallanMasterModal").fadeOut(200, function () {
        $('#ChallanMasterModal').modal('hide');
    });
}

function modal_fadeOut_Lot() {
    $("#LotSetupModal").fadeOut(200, function () {
        $('#LotSetupModal').modal('hide');
    });
}

function modal_fadeOut_Batch() {
    $("#BatchSetupModal").fadeOut(200, function () {
        $('#BatchSetupModal').modal('hide');
    });
}
