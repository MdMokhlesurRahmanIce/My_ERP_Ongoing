/**
* GRRCtrl.js   //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('gRRCtrl', ['$scope', 'gRRService', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, gRRService, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsChallanMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;  

        //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;

        // $scope.IsSPRType = true;

        $scope.IsSPRType = false;
        $scope.IsLoanTypeOrOthers = true;

        // $scope.IsItemTypeFinishedGoods = false;

        // $scope.IsItemTypeRMOthers = true;  //  for  Default Raw Material or Others

        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";
        $scope.HGRRNo = "";

        var baseUrl = '/Inventory/api/GRR/';
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
 
        $scope.GrrID = "0";

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'GRR Creation';
        $scope.ListTitle = 'GRR Records';
        $scope.ListTitleGRRMasters = 'GRR Information';
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


            ////Coming from SideNavCrl
            //$scope.UserCommonEntity = {};
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


        $scope.listMrrBatchNo = [];
        // -------Get All Batch //--------
        function GetAllBatchNo() {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data;
                $scope.listMrrBatchNo = response.data;
                $scope.listBatchInLot = response.data;
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
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadLotRecords(0);


        //**********---- Get Wherehouse Records ----*************** //


        function loadWherehouse(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
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


        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateNo = function () {

            var getMNo = $scope.RefChallanNo;

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
                var getDuplicateNo = gRRService.GetList(apiRoute, cmnParam);
                getDuplicateNo.then(function (response) {
                    if (response.data.length > 0) {
                        $scope.RefChallanNo = "";
                        Command: toastr["warning"]("Ref Challan No Already Exists.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                Command: toastr["warning"]("Please Enter Ref Challan No.");
            }

        }

        //######## End Check Duplicate No ################//



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

                $scope.lstCurrencyList = response.data[0].Id; 
                $("#ddlCurrency").select2("data", { id: response.data[0].Id, text: response.data[0].CurrencyName });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);

        ////**********---- Get All  ChallanType  Records ----*************** //
        //function loadChallanTrnsTypes(isPaging) {

        //    var apiRoute = baseUrl + 'GetChallanTrnsTypes/';
        //    var listChallanType = gRRService.getModel(apiRoute, page, pageSize, isPaging);
        //    listChallanType.then(function (response) {
        //        $scope.listChallanType = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadChallanTrnsTypes(0);

        //function loadLoadingLocation(isPaging) {
        //objcmnParam = {
        //    pageNumber: page,
        //    pageSize: pageSize,
        //    IsPaging: isPaging,
        //    loggeduser: $scope.UserCommonEntity.loggedUserID,
        //    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //    menuId: $scope.UserCommonEntity.currentMenuID,
        //    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //};
        //    var apiRoute = baseUrl + 'GetLocation/';
        //    var listLocation = gRRService.GetLocation(apiRoute, objcmnParam);
        //    listLocation.then(function (response) {
        //        $scope.listLoadLocation = response.data.objLocation;
        //        $scope.listDischargeLocation = response.data.objLocation;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //loadLoadingLocation(0);

        function loadPackingUnit(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            var apiRoute = baseUrl + 'GetPackingUnit/';
            var listPackingUnit = gRRService.GetPackingUnit(apiRoute, objcmnParam);
            listPackingUnit.then(function (response) {
                $scope.listPackingUnit = response.data.objPackingUnit;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadPackingUnit(0);

        function loadWeightUnit(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            var apiRoute = baseUrl + 'GetWeightUnit/';
            var listWeightUnit = gRRService.GetWeightUnit(apiRoute, objcmnParam);
            listWeightUnit.then(function (response) {
                $scope.listWeightUnit = response.data.objWeightUnit;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadWeightUnit(0);

        //function loadDischargeLocation(isPaging) {
        //objcmnParam = {
        //    pageNumber: page,
        //    pageSize: pageSize,
        //    IsPaging: isPaging,
        //    loggeduser: $scope.UserCommonEntity.loggedUserID,
        //    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //    menuId: $scope.UserCommonEntity.currentMenuID,
        //    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //};
        //    var apiRoute = baseUrl + 'GetLocation/';
        //    var listSprNo = gRRService.GetLocation(apiRoute, objcmnParam);
        //    listSprNo.then(function (response) {
        //        $scope.listSprNo = response.data.objSPRNo;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //loadDischargeLocation(0);
        $scope.getItmDetailsByItmCode = function () {

            $scope.ListChallanDetailsForSearch = [];

            var ItemCode = $scope.ItemCode;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            if (ItemCode.trim() != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItmCode/';
                var listItemSerch = gRRService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemCode);
                listItemSerch.then(function (response) {
                    if (response.data.length > 0) {
                        $scope.ListChallanDetailsForSearch = response.data;
                    }
                    else
                        Command: toastr["warning"]("Item Not Found.");


                    //GRRID: 0, CHDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    //PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName, HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName,
                    //Qty: dataModel.Qty, UnitPrice: dataModel.UnitPrice, PackingQty: dataModel.PackingQty, PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight,
                    //GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit, ExistQty: dataModel.ExistQty, Amount: 0.00, 
                    //AditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (ItemCode.trim() == "") {

                    Command: toastr["warning"]("Please Enter Item Code.");
            }

        }

        $scope.AddItem = function () {

            $scope.GrrID = "0";

            $scope.btnSaveText = "Save";


            if ($scope.ListChallanDetailsForSearch.length > 0) {
                $scope.IsHiddenDetail = false;

                var existItem = $scope.ListChallanDetailsForSearch[0].ItemID;
                var duplicateItem = 0;
                angular.forEach($scope.ListChallanDetails, function (item) {
                    if (existItem == item.ItemID) {
                        duplicateItem = 1;
                        return false;
                    }
                });

                if (duplicateItem === 0) {

                    // $scope.ListChallanDetailsForSearch.data[0].ItemID
                    $scope.ListChallanDetails.push({
                        GrrID: 0, GrrDetailID: 0, ItemID: $scope.ListChallanDetailsForSearch[0].ItemID, CompanyID: $scope.ListChallanDetailsForSearch[0].CompanyID,
                        UnitID: $scope.ListChallanDetailsForSearch[0].UnitID,
                        LotID: '', BatchID: '', PackingUnitID: '', WeightUnitID: '', ItemCode: $scope.ListChallanDetailsForSearch[0].ItemCode,
                        ItemName: $scope.ListChallanDetailsForSearch[0].ItemName, HSCODE: $scope.ListChallanDetailsForSearch[0].HSCODE,
                        UOMName: $scope.ListChallanDetailsForSearch[0].UOMName, Qty: 0.00, UnitPrice: 0.00, PackingQty: 0.00, PackingUnit: '', NetWeight: '',
                        GrossWeight: 0.00, WeightUnit: 0.00, ExistQty: 0.00, Amount: 0.00,
                        AdditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00

                    });
                    $scope.ListChallanDetailsForSearch = [];
                }
                else if (duplicateItem === 1) {
                        Command: toastr["warning"]("Item Already Exists!!!!");
                }
            }
            else {
                    Command: toastr["warning"]("Item Not Found!!!!");
            }

        }

        
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

            // $scope.IsItemTypeFinishedGoods = $scope.ItemType == "1" ? true : false;
            // $scope.IsItemTypeRMOthers = $scope.ItemType != "1" ? true : false;

            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            //   else {
            // For Loading modal
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

            $scope.gridOptionslistItemMaster.enableFiltering = true;
            $scope.gridOptionslistItemMaster.enableGridMenu = true;

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
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };


            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            // $scope.IsItemTypeFinishedGoods
            // $scope.IsItemTypeRMOthers
            $scope.gridOptionslistItemMaster = {
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", visible: true, title: "Article No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemCode", displayName: "Item Code", visible: false, title: "Item Code", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Item Name", title: "Item Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", displayName: "Unit Name", title: "Unit Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HSCODE", displayName: "HSCODE", visible: false, title: "HSCODE", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", visible: false, title: "C.Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", visible: false, title: "Construction", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", visible: false, title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeightPerUnit", displayName: "Weight", visible: false, title: "Weight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color Name", visible: false, title: "Color Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeName", displayName: "Size", visible: false, title: "Size", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Width", displayName: "Width", visible: false, title: "Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Add" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                        '<i class="icon-plus" aria-hidden="success"></i> Add' +
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
            //var groupID = $scope.lstSampleNoList;
            //if (groupID > 0) {
            var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
             
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listItemMaster = gRRService.GetList(apiRoute, cmnParam);
            listItemMaster.then(function (response) {
                //$scope.listItemMaster = response.data;
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
            //}
            //else if (groupID == 0 || groupID == "") {
            //        Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            //}
        };


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

            $scope.gridOptionsChallanMaster = {
                 
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,


                columnDefs: [
                    { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CHTypeID", displayName: "CHTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserID", displayName: "PartyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "ComboID", displayName: "TransactionTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "LoadingPortID", displayName: "LoadingPortID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "DischargePortID", displayName: "DischargePortID", visible: false, headerCellClass: $scope .highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CompanyID", displayName: "CompanyID", title: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "DepartmentID", displayName: "DepartmentID", title: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionID", displayName: "RequisitionID", title: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    //{ name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "GrrNo", displayName: "Grr No", title: "Grr No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrDate", displayName: "Grr Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserFullName", displayName: "Party", title: "Supplier", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHNo", displayName: "Ref ChallanNo", title: "Ref ChallanNo", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHDate", displayName: " Ref Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "Spr No", title: "Spr No", visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINo", displayName: "PI No", title: "PI No", width: '10%', visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PONo", displayName: "PO No", title: "PO No", width: '10%', visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCorVoucherorLcafNo", displayName: "L/C No", title: "L/C No", visible: $scope.IsSPRType, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyName", displayName: "Currency", title: "Currency", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "ComboName", displayName: "Transaction Type", title: "Transaction Type", width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '10%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,

                        headerCellClass: $scope.highlightFilteredHeader,

                        cellTemplate: '<span class="label label-success label-mini">' +
                                   '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                                         '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                                     '</a>' +
                                 '</span>' +
                                 '<span class="label label-success label-mini">' +
                                   '<a href="" title="Edit" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                     '<i class="icon-edit"></i> Edit' +
                                   '</a>' +
                                   '</span>'


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

            var apiRoute = baseUrl + 'GetGrrMasterList/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify($scope.IsSPRType) + "]";

            var listChallanMaster = gRRService.GetList(apiRoute, cmnParam);
            // var listChallanMaster = gRRService.getChallanMasterList(apiRoute, objcmnParam);
            listChallanMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsChallanMaster.data = response.data.lstVmChallanMaster;
                $scope.loaderMoreChallanMaster = false;
                $scope.lblMessageForChallanMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        $scope.loadChallanRecords(0);

        //**********----get Item Details Record from itemList popup ----***************//
        $scope.getListItemMaster = function (dataModel) {

            $scope.GrrID = "0"; 
            $scope.btnSaveText = "Save";


            $scope.IsHiddenDetail = false;

            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListChallanDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListChallanDetails.push({
                    GrrID: 0, GrrDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName,
                    HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName, Qty: dataModel.Qty, UnitPrice: 0.00, PackingQty: dataModel.PackingQty,
                    PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight, GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit,
                    ExistQty: dataModel.ExistQty, Amount: 0.00, AdditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00
                });
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }


        }

        //**********----Load Challan MasterForm and Challan Details List By select Challan Master ----***************//

        $scope.loadChallaMasterDetailsByGrrNo = function (dataModel) {


            modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            // 

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnSaveText = "Update";

            $scope.HGRRNo = dataModel.GrrNo;
            $scope.GrrID = dataModel.GrrID;
            $scope.GRRDate = conversion.getDateToString(dataModel.GrrDate);
            $scope.RefChallanDate = conversion.getDateToString(dataModel.RefCHDate);
            $scope.RefChallanNo = dataModel.RefCHNo;

            $scope.Description = dataModel.Description;
            $scope.Remarks = dataModel.Remarks;
            //  this page  grr loan  

            $scope.IsSPRType = false;
            $scope.IsLoanTypeOrOthers = true;

            //$scope.lstPartyList = dataModel.UserID;
            //$('#ddlParty').select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });

            $scope.listParty = [];
            $scope.lstPartyList = '';
            $("#ddlParty").select2("data", { id: '', text: '' });

            if (dataModel.UserID != null) {
                $scope.listParty.push({
                    UserID: dataModel.UserID, UserFullName: dataModel.UserFullName
                });
                $scope.lstPartyList = dataModel.UserID;
                $("#ddlParty").select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });
            }


            $scope.Warehouse = dataModel.DepartmentID;
            $('#ddlWarehouse').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });

            //$scope.SprNo = dataModel.RequisitionID;
            //$('#ddlSPRNo').select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });

            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '' });
            if (dataModel.RequisitionID != null) {
                $scope.listSprNo.push({
                    RequisitionID: dataModel.RequisitionID, RequisitionNo: dataModel.RequisitionNo
                });
                $scope.SprNo = dataModel.RequisitionID;
                $("#ddlSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });
            }



            //$scope.listPONo = [];
            //$scope.listPONo = dataModel;
            //$scope.PONo = dataModel.POID;
            //$('#ddlPONo').select2("data", { id: dataModel.POID, text: dataModel.PONo });


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
                    Id: dataModel.CurrencyID, CurrencyName: dataModel.CurrencyName
                });

                $scope.lstCurrencyList = dataModel.CurrencyID;
                $("#ddlCurrency").select2("data", { id: dataModel.CurrencyID, text: dataModel.CurrencyName });
            }


            $scope.LCNo = dataModel.LCorVoucherorLcafNo==null?"":dataModel.LCorVoucherorLcafNo;

            
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

         

            var apiRoute = baseUrl + 'GetGrrDetailByGrrID/';
            var grrid = dataModel.GrrID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(grrid) + "]";

            var ListGrrDetails = gRRService.GetList(apiRoute, cmnParam);

            //  var ListChallanDetails = gRRService.GetChallanDetailByChallanID(apiRoute, objcmnParam, grrid);

            ListGrrDetails.then(function (response) {
                $scope.ListChallanDetails = response.data.lstChallanDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----delete  Record from ListPIDetails----***************//

        $scope.deleteRow = function (index) {
            // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
            $scope.ListChallanDetails.splice(index, 1);
            // $scope.showDtgrid = $scope.ListPIDetails.length;
        };

        //**********----Create Calculation----***************//
        //$scope.calculation = function (dataModel) {
        //    $scope.ListChallanDetails1 = [];
        //    angular.forEach($scope.ListChallanDetails, function (item) {
        //        var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);
        //        $scope.ListChallanDetails1.push({

        //            GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //            PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //            Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //            GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //            AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount

        //        });
        //        $scope.ListChallanDetails = $scope.ListChallanDetails1;
        //    });
        //}

        //$scope.calDisAmntNIsPercent = function (dataModel) {
        //    $scope.ListChallanDetails1 = [];
        //    angular.forEach($scope.ListChallanDetails, function (item) {
        //        var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);

        //        if (item.IsPercent == true) {
        //            var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(parseFloat(parseFloat(item.DisAmount) / 100) * parseFloat(amountInDec))).toFixed(2);
        //            $scope.ListChallanDetails1.push({

        //                GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //                PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //                Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //                GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //                AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount

        //            });
        //        }
        //        else if (item.IsPercent == false) {
        //            var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(item.DisAmount)).toFixed(2);
        //            $scope.ListChallanDetails1.push({

        //                GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //                PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //                Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //                GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //                AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount

        //            });
        //        }
        //        $scope.ListChallanDetails = $scope.ListChallanDetails1;
        //    });

        //}

        $scope.LoadLotModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListChallanDetails.indexOf(dataModel);
        }

        $scope.LoadBatchModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListChallanDetails.indexOf(dataModel);
        }

        //// **********----Save Batch using popup----***************//
        $scope.SaveBatch = function () {

            var ManufacDate = conversion.getStringToDate($scope.ManufactureDate);
            var ExpDate = conversion.getStringToDate($scope.ExpaireDate);
            var batchMaster = {
                BatchID: 0,
                BatchNo: $scope.BatchNo,
                BatchTypeID: $scope.lstBatchTypeList,
                // BatchDate: BatchDateStringToDate,
                ItemID: $scope.ItemIDFlotNbatchSave,
                Description: $scope.BatchDecription,
                ManufacturingDate: ManufacDate,
                ExpiryDate: ExpDate,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            };

            //var apiRoute = '/Inventory/api/SPRReceive/SaveBatch/';
            // var SaveNewBatch = sPRReceiveService.postSaveBatch(apiRoute, batchMaster);

            var apiRoute = baseUrl + '/SaveBatch/';
            var cmnParam = "[" + JSON.stringify(batchMaster) + "]";
            var SaveNewBatch = gRRService.GetList(apiRoute, cmnParam);

            SaveNewBatch.then(function (response) {
                var result = 0;
                if (response.data != "") {
                    //$scope.HMRRNO = response.data;
                    //varBatchID = response.data;
                    //varBatchName = $scope.BatchNo;

                    $scope.listMrrBatchNo.push({

                        BatchID: response.data,
                        BatchNo: $scope.BatchNo

                    });

                    $scope.ListChallanDetails[$scope.hfIndex] = {

                        GrrID: $scope.ListChallanDetails[$scope.hfIndex].GrrID,
                        GrrDetailID: $scope.ListChallanDetails[$scope.hfIndex].GrrDetailID,
                        ItemID: $scope.ListChallanDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListChallanDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListChallanDetails[$scope.hfIndex].UnitID,
                        LotID: $scope.ListChallanDetails[$scope.hfIndex].LotID,
                        BatchID: response.data,
                        PackingUnitID: $scope.ListChallanDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListChallanDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListChallanDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListChallanDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListChallanDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListChallanDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListChallanDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListChallanDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListChallanDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListChallanDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListChallanDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListChallanDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListChallanDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListChallanDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListChallanDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListChallanDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListChallanDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListChallanDetails[$scope.hfIndex].TotalAmount
                    };

                    $scope.hfIndex = "";
                    $scope.ItemIDFlotNbatchSave = "0";


                    //$scope.Batch = response.data;
                    //$('#ddlBatch').select2("data", { id: response.data, text: $scope.BatchNo });

                    Command: toastr["success"]("Save  Successfully!!!!");
                    modal_fadeOut_Batch();
                    $scope.EmptyBatchSetupModal();
                }
                else if (response.data == "") {
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Save Not Successfull!!!!");
            });
        }


        //// **********----Save Lot using popup----***************//
        $scope.SaveLot = function () {
            var lotMaster = {
                LotID: 0,
                LotNo: $scope.LotNo,
                LotTypeID: $scope.lstLotTypeList,
                ItemID: $scope.ItemIDFlotNbatchSave,
                BatchID: $scope.lstBatchInLotList,
                Description: $scope.LotDecription,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            };
             
           // var SaveNewLot = sPRReceiveService.postSaveLot(apiRoute, lotMaster); 
            // var ChallanItemMasterNdetailsCreateUpdate = gRRService.GetList(apiRoute, cmnParam);

            var apiRoute = baseUrl + '/SaveLot/';
            var cmnParam = "[" + JSON.stringify(lotMaster) + "]";
            var SaveNewLot = gRRService.GetList(apiRoute, cmnParam);

            SaveNewLot.then(function (response) {
                var result = 0;
                if (response.data != "") {

                    //$scope.listLot.push({ 
                    //    LotID: response.data,
                    //    LotNo: $scope.LotNo

                    //});

                    $scope.listMrrLot.push({
                        LotID: response.data,
                        LotNo: $scope.LotNo
                    });

                    // $scope.listWorkFlowDetail[$scope.hfIndex] = { WorkFlowID: $scope.WorkFlowID, WorkFlowDetailID: $scope.WorkFlowDetailID, StatusID: $("#ddlStatus").select2('data').id, StatusName: $("#ddlStatus").select2('data').text, EmployeeID: $("#ddlStatusBy").select2('data').id, UserFirstName: $("#ddlStatusBy").select2('data').text, MenuName: $("#ddlMenu").select2('data').text, CompanyID: $scope.lstCompanyList, Sequence: $scope.Sequence };

                    $scope.ListChallanDetails[$scope.hfIndex] = {

                        GrrID: $scope.ListChallanDetails[$scope.hfIndex].GrrID,
                        GrrDetailID: $scope.ListChallanDetails[$scope.hfIndex].GrrDetailID,
                        ItemID: $scope.ListChallanDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListChallanDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListChallanDetails[$scope.hfIndex].UnitID,
                        LotID: response.data, //.LotID,
                        BatchID: $scope.ListChallanDetails[$scope.hfIndex].BatchID,
                        PackingUnitID: $scope.ListChallanDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListChallanDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListChallanDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListChallanDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListChallanDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListChallanDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListChallanDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListChallanDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListChallanDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListChallanDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListChallanDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListChallanDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListChallanDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListChallanDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListChallanDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListChallanDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListChallanDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListChallanDetails[$scope.hfIndex].TotalAmount
                    };

                    $scope.hfIndex = "";
                    $scope.ItemIDFlotNbatchSave = "0";

                    //$scope.lstLot = response.data;
                    //$('#ddlLotNo').select2("data", { id: response.data, text: $scope.LotNo });

                    //  $scope.lstLot = response.data;
                    //  $('#ddlMrrLot').select2("data", { id: response.data, text: $scope.LotNo });


                    Command: toastr["success"]("Save  Successfully!!!!");
                    modal_fadeOut_Lot();
                    $scope.EmptyLotSetupModal();
                    // $scope.clear(); 
                }
                else if (response.data == "") {
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Save Not Successfull!!!!");
            });
        }


        //**********----Save and Update InvRChallanMaster and InvRChallanDetail  Records----***************//
        $scope.save = function () {
            $("#save").prop("disabled", true);
            var NewStringToDate = conversion.getStringToDate($scope.GRRDate);
            var RefCHDateToDate = conversion.getStringToDate($scope.RefChallanDate);

            //  , CHID,  ,  ,  ,  , , , , IsMrrCompleted, ,


            var itemMaster = {
                GrrID: $scope.GrrID,
                GrrNo: $scope.HGRRNo,
                GrrDate: NewStringToDate,
                RefCHNo: $scope.RefChallanNo,
                RefCHDate: RefCHDateToDate,
                // TypeID: $scope.lstChallanTypeList,
                // CHTypeID: $scope.lstChallanTypeList,
                SupplierID: $scope.lstPartyList,
                CurrencyID: $scope.lstCurrencyList,
                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID, //$scope.lstChallanTypeList,
                RequisitionID: $scope.SprNo,
                POID: $scope.PONo,
               //  PIID: $scope.PINO,
               // LoadingPortID: $scope.LoadingLocation,
               // DischargePortID: $scope.DischargeLocation,
                Remarks: $scope.Remarks,
                Description: $scope.Description,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.Warehouse, //$scope.UserCommonEntity.loggedUserDepartmentID,

                IsDeleted: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID,


                // MenuID: $scope.MenuID //26

            };

            var fileList = [];
            angular.forEach($scope.files, function (item) {
                this.push(item.name);
            }, fileList);

            //if (fileList.length == 0) {
            //    $("#save").prop("disabled", false);
            //    Command: toastr["warning"]("Please attach GRR document.");
            //    return;
            //}


            var menuID = $scope.UserCommonEntity.currentMenuID;
            var itemMasterDetail = $scope.ListChallanDetails;
            var chkAmount = 1;
            angular.forEach($scope.ListChallanDetails, function (item) {

                if (item.Qty <= 0) {
                    chkAmount = 0;
                }
            });

            if ($scope.ListChallanDetails.length > 0) {

                if (chkAmount == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateChallanMasterNdetails/';
                    //  var apiRoute = baseUrl + 'GetGrrMasterList/';

                    var cmnParam = "[" + JSON.stringify(itemMaster) + "," + JSON.stringify(itemMasterDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(fileList) + "]";

                    var ChallanItemMasterNdetailsCreateUpdate = gRRService.GetList(apiRoute, cmnParam);

                    // var ChallanItemMasterNdetailsCreateUpdate = gRRService.postMasterDetail(apiRoute, itemMaster, itemMasterDetail, menuID, fileList);
                    ChallanItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {

                            $scope.HGRRNo = response.data;

                            ///// start file upload/////////////

                            var data = new FormData();

                            for (var i in $scope.files) {
                                data.append("uploadedFile", $scope.files[i]);
                            }
                            data.append("uploadedFile", response.data);
                            // ADD LISTENERS.
                            var objXhr = new XMLHttpRequest();
                            //objXhr.addEventListener("progress", updateProgress, false);
                            //objXhr.addEventListener("load", transferComplete, false);


                            var apiRoute = baseUrl + 'UploadFiles/';
                            objXhr.open("POST", apiRoute);
                            objXhr.send(data);
                            // debugger;
                            document.getElementById('file').value = '';
                            $scope.files = [];

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
                    Command: toastr["warning"]("GRR Quantity Must Not Zero Or Empty !!!!");

                }

            }
            else if ($scope.ListChallanDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("GRR Detail Must Not Empty!!!!");
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
            var TransTypeID= $scope.UserCommonEntity.currentTransactionTypeID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id +'/'+ TransTypeID;
            var ListFileDetails = gRRService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----Reset Record----***************//

        $scope.EmptyLotSetupModal = function () {

            $scope.ItemIDFlotNbatchSave = "0";
            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.LotID = '';
            $scope.LotNo = '';
            $scope.LotDecription = '';
            $scope.lstBatchInLotList = '';
            $scope.listBatchInLot = [];
            GetAllBatchNo();
        }

        $scope.EmptyBatchSetupModal = function () {

            $scope.ItemIDFlotNbatchSave = "0";

            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.ManufactureDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.ExpaireDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.BatchID = '';
            $scope.BatchNo = '';
            $scope.BatchDecription = '';
        }


        $scope.clear = function () {


            $scope.IsSPRType = false;
            $scope.IsLoanTypeOrOthers = true;
            //  $scope.IsSPRType = true;
            var date = new Date();
            $scope.GRRDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.RefChallanNo = "";

            $scope.ChallanTypeID = 5;
            $scope.MenuID = 93;
            $scope.GrrID = "0";
            
            $scope.listParty = [];
            $scope.lstPartyList = "";
            $('#ddlParty').select2("data", { id: "", text: "--Select Supplier--" });


            $scope.listChallanType = [];
            $scope.lstChallanTypeList = "";
            $('#ddlChallanType').select2("data", { id: "", text: "--Select Challan Type--" });


            //$scope.listLoadLocation = [];
            //$scope.LoadingLocation = "";
            //$('#txtLoadingLocation').select2("data", { id: "", text: "--Select Loading Location--" });

            //$scope.listDischargeLocation = [];
            //$scope.DischargeLocation = '';
            //$('#txtDischargeLocation').select2("data", { id: '', text: '--Select Discharge Location--' });

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

            //$scope.listPINO = [];
            //$scope.PINO = "";
            //$('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });

           // $scope.listLCNo = [];
            $scope.LCNo = "";
           // $('#ddlLCNo').select2("data", { id: '', text: '--Select LC No--' });


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

          //  loadChallanTrnsTypes(0);
          //  loadLoadingLocation(0);
            loadPackingUnit(0);
            loadWeightUnit(0);
            $scope.loadChallanRecords(0);
            $scope.getTypes();
            //  $scope.getItemGroupsByType();

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
