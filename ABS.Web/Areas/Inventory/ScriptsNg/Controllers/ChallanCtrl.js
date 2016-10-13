/**
* ChallanCtrl.js
*/


app.controller('challanCtrl', ['$scope', 'challanService', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, challanService, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsChallanMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        $scope.IsSPRType = false;
        $scope.IsLoanTypeOrOthers = true;

       // $scope.IsItemTypeFinishedGoods = false;

       // $scope.IsItemTypeRMOthers = true;  //  for  Default Raw Material or Others

        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";

        var baseUrl = '/Inventory/api/Challan/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.CurrentDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.ChallanTypeID = 5;
        $scope.MenuID = 93;
        $scope.CHID = "0";

        $scope.btnChallanSaveText = "Save";
        $scope.btnChallanShowText = "Show List";
        $scope.btnChallanReviseText = "Update";
        $scope.PageTitle = 'GRR Creation';
        $scope.ListTitle = 'GRR Records';
        $scope.ListTitleChallanMasters = 'GRR Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleChallanDeatails = 'Listed Item of GRR';

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
                $scope.btnChallanShowText = "Show Challan List";
                $scope.IsShow = true;
            }
            else {
                $scope.btnChallanShowText = "Hide Challan List";
                $scope.IsShow = false;
                $scope.IsHidden = false;
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
        //        var itemGroupes = challanService.getAllItemGroup(apiRoute, page, pageSize, isPaging, $scope.ItemType, LoggedCompanyID);
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
        // -------Get All Batch //--------
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

        //**********---- Get All Party Records ----*************** //
        function loadPartyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetParty/';
            var listParty = challanService.getModel(apiRoute, page, pageSize, isPaging);
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
            var listCurrency = challanService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);

        //**********---- Get All  ChallanType  Records ----*************** //
        function loadChallanTrnsTypes(isPaging) {

            var apiRoute = baseUrl + 'GetChallanTrnsTypes/';
            var listChallanType = challanService.getModel(apiRoute, page, pageSize, isPaging);
            listChallanType.then(function (response) {
                $scope.listChallanType = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadChallanTrnsTypes(0);

        //function loadLoadingLocation(isPaging) {
        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: LoggedUserID,
        //        loggedCompany: LoggedCompanyID,
        //        menuId: 76,
        //        tTypeId: 25
        //    };
        //    var apiRoute = baseUrl + 'GetLocation/';
        //    var listLocation = challanService.GetLocation(apiRoute, objcmnParam);
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
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };
            var apiRoute = baseUrl + 'GetPackingUnit/';
            var listPackingUnit = challanService.GetPackingUnit(apiRoute, objcmnParam);
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
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };
            var apiRoute = baseUrl + 'GetWeightUnit/';
            var listWeightUnit = challanService.GetWeightUnit(apiRoute, objcmnParam);
            listWeightUnit.then(function (response) {
                $scope.listWeightUnit = response.data.objWeightUnit;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadWeightUnit(0);

        //function loadDischargeLocation(isPaging) {
        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: LoggedUserID,
        //        loggedCompany: LoggedCompanyID,
        //        menuId: 76,
        //        tTypeId: 25
        //    };
        //    var apiRoute = baseUrl + 'GetLocation/';
        //    var listSprNo = challanService.GetLocation(apiRoute, objcmnParam);
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
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };

            if (ItemCode.trim() != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItmCode/';
                var listItemSerch = challanService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemCode);
                listItemSerch.then(function (response) {
                    if (response.data[0].ItemID > 0) {
                        $scope.ListChallanDetailsForSearch = response.data;
                    }
                    else
                        Command: toastr["warning"]("Item Not Exist.");


                    //CHID: 0, CHDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    //PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName, HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName,
                    //Qty: dataModel.Qty, UnitPrice: dataModel.UnitPrice, PackingQty: dataModel.PackingQty, PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight,
                    //GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit, ExistQty: dataModel.ExistQty, Amount: 0.00, 
                    //AditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (ItemCode.Trim() == "")
            {

                Command: toastr["warning"]("Please Enter Item Code.");
            }

        }

        $scope.AddItem = function ()
        {
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
                    CHID: 0, CHDetailID: 0, ItemID: $scope.ListChallanDetailsForSearch[0].ItemID, CompanyID: $scope.ListChallanDetailsForSearch[0].CompanyID,
                    UnitID: $scope.ListChallanDetailsForSearch[0].UnitID,
                    LotID: '', BatchID: '', PackingUnitID: '', WeightUnitID: '', ItemCode: $scope.ListChallanDetailsForSearch[0].ItemCode,
                    ItemName: $scope.ListChallanDetailsForSearch[0].ItemName, HSCODE: $scope.ListChallanDetailsForSearch[0].HSCODE,
                    UOMName: $scope.ListChallanDetailsForSearch[0].UOMName, Qty: 0.00, UnitPrice: 0.00, PackingQty: 0.00, PackingUnit: '', NetWeight: '',
                    GrossWeight: 0.00, WeightUnit: 0.00, ExistQty: 0.00, Amount: 0.00, 
                    AditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00

                });
                $scope.ListChallanDetailsForSearch = [];
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
        }

        //**********---- Load By Challan Transaction Type ----***************//

        $scope.LoadByChallanTransType = function () {

            var ctType = $("#ddlChallanType").select2('data').text;
            $scope.ListChallanDetails = [];
            if (ctType == "Loan") {
                $scope.IsSPRType = false;
                $scope.IsLoanTypeOrOthers = true;
                $scope.ListChallanDetails = [];

                // Load Sample/Article No 

                //$scope.loaderMoreForSampleNo = true;
                //$scope.lblMessageForSampleNo = '';
                //$scope.result = "color-red";

                //var apiRoute = baseUrl + 'GetItemSampleNo/';
                //var listSampleNo = challanService.getModel(apiRoute, page, pageSize, isPaging);
                //listSampleNo.then(function (response) {
                //    $scope.listSampleNo = response.data;

                //    $scope.loaderMoreForSampleNo = false;
                //},
                //function (error) {
                //    console.log("Error: " + error);
                //});
            }
            else if (ctType == "SPR") {
                $scope.IsSPRType = true;
                $scope.IsLoanTypeOrOthers = false;
                $scope.IsHiddenDetail = true;

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

                $scope.DischargeLocation = "";
                $scope.LoadingLocation = "";

                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: LoggedUserID,
                    loggedCompany: LoggedCompanyID,
                    menuId: 76,
                    tTypeId: 25
                };
                var apiRoute = baseUrl + 'GetSPRNo/';
                var listSprNo = challanService.GetSPRNo(apiRoute, objcmnParam);
                listSprNo.then(function (response) {
                    $scope.listSprNo = response.data.objSPRNo;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }

        //**********---- Load  Item Details By SPRNo Change ----***************//

        $scope.LoadItemBySPRNoChange = function () {
            $scope.ListChallanDetails = [];
            $scope.IsHiddenDetail = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 76,
                tTypeId: 25
            };

            var sprID = $scope.SprNo;
            var apiRoute = baseUrl + 'GetItemDetailBySPRID/';
            var itemBySprNo = challanService.GetItemDetailBySPRID(apiRoute, objcmnParam, sprID);
            itemBySprNo.then(function (response) {

                $scope.listPONo = response.data[0];
                $scope.PONo = response.data[0].POID;
                $('#ddlPONo').select2("data", { id: response.data[0].POID, text: response.data[0].PONo });

                $scope.listPINO = response.data[0];
                $scope.PINO = response.data[0].PIID;
                $('#ddlPINO').select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });

                $scope.listLCNo = response.data[0].POID;
                $scope.LCNo = response.data[0].LCorVoucherorLcafNo;
                $('#ddlLCNo').select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });

                $scope.DischargeLocation = "";
                $scope.LoadingLocation = "";

                $scope.ListChallanDetails = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });
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

            if ($scope.lstSampleNoList == undefined) {
                Command: toastr["warning"]("Select Sample/Article No !!!!");
            }
            else {
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

                // $scope.IsItemTypeFinishedGoods
                // $scope.IsItemTypeRMOthers
                $scope.gridOptionslistItemMaster = {
                    columnDefs: [
                        { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ArticleNo", displayName: "Article No", visible: false, title: "Article No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ItemCode", displayName: "Item Code",  title: "Item Code", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ItemName", displayName: "Item Name", title: "Item Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "HSCODE", displayName: "HSCODE", title: "HSCODE", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "CuttableWidth", displayName: "C.Width",  title: "C.Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "Construction", displayName: "Construction",  title: "Construction", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "Weave", displayName: "Weave",  title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "WeightPerUnit", displayName: "Weight",title: "Weight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SizeName", displayName: "Size", title: "Size", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "Width", displayName: "Width",  title: "Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                        {
                            name: 'Select',
                            displayName: "Select",
                            width: '6%',
                            enableColumnResizing: false,
                            enableFiltering: false,
                            enableSorting: false,
                            headerCellClass: $scope.highlightFilteredHeader,
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
                //var groupID = $scope.lstSampleNoList;
                //if (groupID > 0) {
                    var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
                    var listItemMaster = challanService.getItemMasterByGroup(apiRoute, objcmnParam);
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
            }
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
                $scope.loadChallanRecords();
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadChallanRecords();
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadChallanRecords();
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadChallanRecords();
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadChallanRecords();
                }
            }
        };

        //**********----Get All Challan Master Records----***************
        $scope.loadChallanRecords = function () {

            $scope.gridOptionsChallanMaster.enableFiltering = true;
            $scope.gridOptionsChallanMaster.enableGridMenu = true;
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
                    { name: "CHID", displayName: "CHID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHTypeID", displayName: "CHTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserID", displayName: "PartyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Id", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ComboID", displayName: "TransactionTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LoadingPortID", displayName: "LoadingPortID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DischargePortID", displayName: "DischargePortID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "CompanyID", title: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", displayName: "DepartmentID", title: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionID", displayName: "RequisitionID", title: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "RequisitionNo", title: "RequisitionNo", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHNo", displayName: "Challan No", title: "Challan No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHDate", displayName: "Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHNo", displayName: "Ref ChallanNo", title: "Ref ChallanNo", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHDate", displayName: " Ref ChallanDate", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINo", displayName: "PI No", title: "PI No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PONo", displayName: "PO No", title: "PO No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCorVoucherorLcafNo", displayName: "L/C No", title: "L/C No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyName", displayName: "Currency", title: "Currency", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ComboName", displayName: "Transaction Type", title: "Transaction Type", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserFullName", displayName: "Party", title: "Party", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.loadChallaMasterDetailsByChallanNo(row.entity)">' +
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

            var apiRoute = baseUrl + 'GetChallanMasterList/';
            var listChallanMaster = challanService.getChallanMasterList(apiRoute, objcmnParam);
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


        //**********----get Item Details Record from itemList popup ----***************//
        $scope.getListItemMaster = function (dataModel) {

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
                    CHID: 0, CHDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName, HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName,
                    Qty: dataModel.Qty, UnitPrice: dataModel.UnitPrice, PackingQty: dataModel.PackingQty, PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight,
                    GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit, ExistQty: dataModel.ExistQty, Amount: 0.00,

                    AditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00
                });
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }


        }

        //**********----Load Challan MasterForm and Challan Details List By select Challan Master ----***************//

        $scope.loadChallaMasterDetailsByChallanNo = function (dataModel) {


            modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnChallanShowText = "Show Challan List";
            $scope.IsHidden = true;
            // 
            $scope.btnChallanSaveText = "Update";

            $scope.HCHNo = dataModel.CHNo;
            $scope.CHID = dataModel.CHID;
            $scope.CurrentDate = conversion.getDateToString(dataModel.CHDate);
            $scope.RefChallanDate = conversion.getDateToString(dataModel.RefCHDate);
            $scope.RefChallanNo = dataModel.RefCHNo;

            $scope.lstPartyList = dataModel.UserID;
            $('#ddlParty').select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });

            $scope.lstCurrencyList = dataModel.Id;
            $('#ddlCurrency').select2("data", { id: dataModel.Id, text: dataModel.CurrencyName });

            $scope.lstChallanTypeList = dataModel.ComboID;
            $('#ddlChallanType').select2("data", { id: dataModel.ComboID, text: dataModel.ComboName });


            if (dataModel.ComboName == "SPR") {
                $scope.IsSPRType = true;
                $scope.IsLoanTypeOrOthers = false;
 
                //$scope.LoadingLocation = dataModel.LoadingPortID;
                //$('#txtLoadingLocation').select2("data", { id: dataModel.LoadingPortID, text: dataModel.LoadingLocation });

                
                //$scope.DischargeLocation = dataModel.DischargePortID;
                //$('#txtDischargeLocation').select2("data", { id: dataModel.DischargePortID, text: dataModel.DischargeLocation });

                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: LoggedUserID,
                    loggedCompany: LoggedCompanyID,
                    menuId: 76,
                    tTypeId: 25
                };
                var apiRoute = baseUrl + 'GetSPRNo/';
                var listSprNo = challanService.GetSPRNo(apiRoute, objcmnParam);
                listSprNo.then(function (response) {
                    $scope.listSprNo = response.data.objSPRNo;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });

                // $scope.listSprNo = [];
                //  $scope.listSprNo = dataModel;
                $scope.SprNo = dataModel.RequisitionID;
                $('#ddlSPRNo').select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });

                $scope.listPONo = [];
                $scope.listPONo = dataModel;
                $scope.PONo = dataModel.POID;
                $('#ddlPONo').select2("data", { id: dataModel.POID, text: dataModel.PONo });

                $scope.listPINO = [];
                $scope.listPINO = dataModel;
                $scope.PINO = dataModel.PIID;
                $('#ddlPINO').select2("data", { id: dataModel.PIID, text: dataModel.PINo });

                $scope.listLCNo = [];
                $scope.listLCNo = dataModel;
                $scope.LCNo = dataModel.LCorVoucherorLcafNo;
                $('#ddlLCNo').select2("data", { id: dataModel.POID, text: dataModel.LCorVoucherorLcafNo });

                //$scope.DischargeLocation = dataModel.DischargePortID;
                //$scope.LoadingLocation = dataModel.LoadingPortID;
            }
            else if (dataModel.ComboName == "Loan") {

                $scope.IsSPRType = false;
                $scope.IsLoanTypeOrOthers = true;

                //$scope.listLoadLocation = [];
                //$scope.LoadingLocation = "";
                //$('#txtLoadingLocation').select2("data", { id: "", text: "" });

                //$scope.listDischargeLocation = [];
                //$scope.DischargeLocation = '';
                //$('#txtDischargeLocation').select2("data", { id: '', text: '' });

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


            }
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.MenuID,
                tTypeId: 25
            };

            var apiRoute = baseUrl + 'GetChallanDetailByChallanID/';
            var challanID = dataModel.CHID;
            var ListChallanDetails = challanService.GetChallanDetailByChallanID(apiRoute, objcmnParam, challanID);
            ListChallanDetails.then(function (response) {
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
        $scope.calculation = function (dataModel) {
            $scope.ListChallanDetails1 = [];
            angular.forEach($scope.ListChallanDetails, function (item) {
                var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);
                $scope.ListChallanDetails1.push({

                    CHID: item.CHID, CHDetailID: item.CHDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

                    PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
                    Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
                    GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

                    AditionalQty: item.AditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount

                });
                $scope.ListChallanDetails = $scope.ListChallanDetails1;
            });
        }

        //$scope.calDisAmount = function (dataModel) {
        //    $scope.ListChallanDetails1 = [];
        //    angular.forEach($scope.ListChallanDetails, function (item) {
        //        var TtlAmount = parseFloat(parseFloat(item.Amount) - parseFloat(item.DisAmount)).toFixed(2);

        //        $scope.ListChallanDetails1.push({

        //            CHID: item.CHID, CHDetailID: item.CHDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //            PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //            Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //            GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: item.Amount,

        //            AditionalQty: item.AditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount

        //        });
        //        $scope.ListChallanDetails = $scope.ListChallanDetails1;
        //    });
        //}

        $scope.calDisAmntNIsPercent = function (dataModel) {
            $scope.ListChallanDetails1 = [];
            angular.forEach($scope.ListChallanDetails, function (item) {
                var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);

                if (item.IsPercent == true) {
                    var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(parseFloat(parseFloat(item.DisAmount) / 100) * parseFloat(amountInDec))).toFixed(2);
                    $scope.ListChallanDetails1.push({

                        CHID: item.CHID, CHDetailID: item.CHDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

                        PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
                        Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
                        GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

                        AditionalQty: item.AditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount

                    });
                }
                else if (item.IsPercent == false) {
                    var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(item.DisAmount)).toFixed(2);
                    $scope.ListChallanDetails1.push({

                        CHID: item.CHID, CHDetailID: item.CHDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

                        PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
                        Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
                        GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

                        AditionalQty: item.AditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount

                    });
                }
                $scope.ListChallanDetails = $scope.ListChallanDetails1;
            });

        }

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

            var apiRoute = '/Inventory/api/SPRReceive/SaveBatch/';
            var SaveNewBatch = sPRReceiveService.postSaveBatch(apiRoute, batchMaster);
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

                        CHID: $scope.ListChallanDetails[$scope.hfIndex].CHID,
                        CHDetailID: $scope.ListChallanDetails[$scope.hfIndex].CHDetailID,
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

                        AditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AditionalQty,
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

            var apiRoute = '/Inventory/api/SPRReceive/SaveLot/';
            var SaveNewLot = sPRReceiveService.postSaveLot(apiRoute, lotMaster);
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

                        CHID: $scope.ListChallanDetails[$scope.hfIndex].CHID,
                        CHDetailID: $scope.ListChallanDetails[$scope.hfIndex].CHDetailID,
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

                        AditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AditionalQty,
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
            var NewStringToDate = conversion.getStringToDate($scope.CurrentDate);
            var RefCHDateToDate = conversion.getStringToDate($scope.RefChallanDate);
            var itemMaster = {
                CHID: $scope.CHID,
                CHNo: $scope.HCHNo,
                CHDate: NewStringToDate,
                RefCHNo: $scope.RefChallanNo,
                RefCHDate: RefCHDateToDate,
                TypeID: $scope.lstChallanTypeList,
                // CHTypeID: $scope.lstChallanTypeList,
                PartyID: $scope.lstPartyList,
                CurrencyID: $scope.lstCurrencyList,
                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID, //$scope.lstChallanTypeList,
                RequisitionID: $scope.SprNo,
                POID: $scope.PONo,
                PIID: $scope.PINO,
                LoadingPortID: $scope.LoadingLocation,
                DischargePortID: $scope.DischargeLocation,
                Remarks: $scope.Remarks,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,

                IsDeleted: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID,


                // MenuID: $scope.MenuID //26

            };
            var menuID = $scope.UserCommonEntity.currentMenuID;
            var itemMasterDetail = $scope.ListChallanDetails;
            var chkAmount = 1;
            angular.forEach($scope.ListChallanDetails, function (item) {

                if (item.Amount <= 0) {
                    chkAmount = 0;
                }
            });

            if ($scope.ListChallanDetails.length > 0) {

                if (chkAmount == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateChallanMasterNdetails/';
                    var ChallanItemMasterNdetailsCreateUpdate = challanService.postMasterDetail(apiRoute, itemMaster, itemMasterDetail, menuID);
                    ChallanItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {
                            $scope.HCHNo = response.data;
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
                Command: toastr["warning"]("Challan Detail Must Not Empty!!!!");
            }
        };



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
            var date = new Date();
            $scope.CurrentDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.RefChallanNo = "";

            $scope.ChallanTypeID = 5;
            $scope.MenuID = 93;
            $scope.CHID = "0";
            $scope.HCHNo = "";

            $scope.listParty = [];
            $scope.lstPartyList = "";
            $('#ddlParty').select2("data", { id: "", text: "--Select Party--" });


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

            $scope.listPINO = [];
            $scope.PINO = "";
            $('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });

            $scope.listLCNo = [];
            $scope.LCNo = "";
            $('#ddlLCNo').select2("data", { id: '', text: '--Select LC No--' });

            $scope.Remarks = "";


            $scope.btnChallanSaveText = "Save";
            $scope.btnChallanShowText = "Show PI Info List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.ListChallanDetails = [];
            loadPartyRecords(0);
            loadCurrencyRecords(0);
            loadChallanTrnsTypes(0);
            loadLoadingLocation(0);
            loadPackingUnit(0);
            loadWeightUnit(0);
            $scope.loadChallanRecords();
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
