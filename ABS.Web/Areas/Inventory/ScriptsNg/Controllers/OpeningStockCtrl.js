/**
* GRRCtrl.js   //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('openingStockCtrl', ['$scope', 'openingStockService', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, openingStockService, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsChallanMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;  
 
        $scope.IsStoreCompleted = true;
        $scope.IsApproved = true;
        $scope.IsAccountsCompleted = true;


        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";
        $scope.HMRRNo = "";

        var baseUrl = '/Inventory/api/OpeningStock/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.OpeningStockDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

     
        $scope.MrrID = "0";

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'Opening Stock Creation';
        $scope.ListTitle = 'Opening Stock Records';
        $scope.ListTitleGRRMasters = 'Opening Stock Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleGRRDeatails = 'Listed Item of Opening Stock';

        $scope.ListMrrDetails = [];
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

         

        function loadStockByRecords(isPaging) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = '/Inventory/api/QC/GetUser/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listuser = mRRService.GetList(apiRoute, cmnParam);
            listuser.then(function (response) {
                $scope.listStockBy = response.data;
                angular.forEach($scope.listStockBy, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {

                        $scope.ngmStockByList = item.UserID;
                        $("#ddlStockBy").select2("data", { id: item.UserID, text: item.UserFullName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadStockByRecords(0);


        //*******************   Item Group   On Page Load--  ***********

        //$scope.getItemGroupsByType = function () {

        //    $scope.listSampleNo = "";
        //    $scope.lstSampleNoList = "";
        //    $('#ddlSampleNo').select2("data", { id: '2', text: '--Select Item Group--' });

        //    var apiRoute = '/SystemCommon/api/RawMaterial/GetItemGroups/';
        //    if ($scope.ItemType != "") {
        //        var itemGroupes = openingStockService.getAllItemGroup(apiRoute, page, pageSize, isPaging, $scope.ItemType, LoggedCompanyID);
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


        //**********---- Get Wherehouse Records ----*************** //
         
 
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
              //  var listItemSerch = openingStockService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemCode);

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ItemCode) + "]";

                //   var itemBySprNo = gRRService.GetList(apiRoute, cmnParam);

                var listItemSerch = openingStockService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

                listItemSerch.then(function (response) {
                    if (response.data.length > 0) {
                        $scope.ListChallanDetailsForSearch = response.data;
                    }
                    else
                        Command: toastr["warning"]("Item Not Found.");

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

            $scope.MrrID = "0";

            $scope.btnSaveText = "Save";


            if ($scope.ListChallanDetailsForSearch.length > 0) {
                $scope.IsHiddenDetail = false;

                var existItem = $scope.ListChallanDetailsForSearch[0].ItemID;
                var duplicateItem = 0;
                angular.forEach($scope.ListMrrDetails, function (item) {
                    if (existItem == item.ItemID) {
                        duplicateItem = 1;
                        return false;
                    }
                });

                if (duplicateItem === 0) {

                    // $scope.ListChallanDetailsForSearch.data[0].ItemID
                    $scope.ListMrrDetails.push({
                        MrrID: 0, MrrDetailID: 0, ItemID: $scope.ListChallanDetailsForSearch[0].ItemID, CompanyID: $scope.ListChallanDetailsForSearch[0].CompanyID,
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
                    { name: "ItemCode", displayName: "Item Code", visible: false,  title: "Item Code", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Item Name", title: "Item Name",   headerCellClass: $scope.highlightFilteredHeader },
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
                        width: '8%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Add" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                        '<i class="icon-plus" aria-hidden="true"></i> Add' +
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

            var listItemMaster = openingStockService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
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
                   { name: "MrrID", displayName: "MrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHID", displayName: "CHID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", displayName: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierID", displayName: "SupplierID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StatusID", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StatusBy", displayName: "StatusBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "RequisitionID", displayName: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },


                    { name: "MrrNo", displayName: "MRR No", title: "MRR No", width: '52%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrDate", displayName: "MRR Date", title: "MRR Date", cellFilter: 'date:"dd-MM-yyyy"', width: '40%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueNo", displayName: "Issue No", title: "Issue No", visible: false, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueDate", displayName: "Issue Date", title: "Issue Date", visible: false, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "DepartmentName", displayName: "Department Name", title: "Department Name", visible: false, width: '30%', headerCellClass: $scope.highlightFilteredHeader },


                    { name: "GrrNo", displayName: "GRR No", title: "GRR No", visible: false, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrDate", displayName: "GRR Date", title: "GRR Date", visible: false, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierName", displayName: "Supplier", title: "Supplier", visible: false, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "ChallanNo", displayName: "Challan No", title: "Challan No", width: '15%', headerCellClass: $scope.highlightFilteredHeader }, 
                    //{ name: "ChallanDate", displayName: "Challan Date", title: "Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "MrrQcNo", displayName: "QC No", title: "QC No", visible: false, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrQcDate", displayName: "QC Date", title: "QC Date", visible: false, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "SprNo", displayName: "SPR No", title: "SPR No", visible: false, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPRDate", displayName: "SPR Date", title: "SPR Date", visible: false, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "PINO", displayName: "PI NO", title: "PI NO", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "PIDate", displayName: "PI Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "PONO", displayName: "PO NO", title: "PO NO", visible: false, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PODate", displayName: "PO Date", cellFilter: 'date:"dd-MM-yyyy"', visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                   // { name: "InvoiceNo", displayName: "Invoice No", title: "Invoice No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHDate", displayName: "CH Date", cellFilter: 'date:"dd-MM-yyyy"', visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },


                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '10%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,
                       // visible:false,
                        headerCellClass: $scope.highlightFilteredHeader,

                        //cellTemplate: '<span class="label label-success label-mini">' +
                        //           '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                        //                 '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                        //             '</a>' +
                        //         '</span>' +
                        //         '<span class="label label-warning label-mini">' +
                        //           '<a href="" title="Select" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                        //             '<i class="icon-check"></i> Select' +
                        //           '</a>' +
                        //           '</span>'


                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Edit" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                        '<i class="icon-edit" aria-hidden="true"></i> Edit' +
                                      '</a>' +
                                      '</span>'
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

            var apiRoute = '/Inventory/api/MRR/GetMrrMasterList/';

            var TransTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify($scope.IsStoreCompleted) + "," + JSON.stringify($scope.IsAccountsCompleted) + "," + JSON.stringify($scope.IsApproved) + "]";
           // var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listChallanMaster = openingStockService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            // var listChallanMaster = openingStockService.getChallanMasterList(apiRoute, objcmnParam);
            listChallanMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsChallanMaster.data = response.data.lstMrrMaster;
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

            $scope.MrrID = "0"; 
            $scope.btnSaveText = "Save";


            $scope.IsHiddenDetail = false;

            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListMrrDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListMrrDetails.push({
                    MrrID: 0, MrrDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
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

            $("#save").prop("disabled", true); //  not permitted to update

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

            $scope.HMRRNo = dataModel.MrrNo;
            $scope.MrrID = dataModel.MrrID;
            $scope.OpeningStockDate = conversion.getDateToString(dataModel.MrrDate);

            $scope.Description = dataModel.Description;
            $scope.Remarks = dataModel.Remarks;
          
          



            //$scope.listPONo = [];
            //$scope.listPONo = dataModel;
            //$scope.PONo = dataModel.POID;
            //$('#ddlPONo').select2("data", { id: dataModel.POID, text: dataModel.PONo });


            //$scope.listPONo = [];
            //$scope.PONo = '';
            //$("#ddlPONo").select2("data", { id: '', text: '' });

            //if (dataModel.POID != null) {
            //    $scope.listPONo.push({
            //        POID: dataModel.POID, PONo: dataModel.PONo
            //    });
            //    $scope.PONo = dataModel.POID;
            //    $("#ddlPONo").select2("data", { id: dataModel.POID, text: dataModel.PONo });
            //}


            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = '/Inventory/api/MRR/GetMrrDetailsListByMrrID/';
            var MrrID = dataModel.MrrID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(MrrID) + "]";

            var ListStockDetails = openingStockService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

            //  var ListMrrDetails = openingStockService.GetChallanDetailByChallanID(apiRoute, objcmnParam, MrrID);

            ListStockDetails.then(function (response) {
                $scope.ListMrrDetails = response.data.lstDetailInfoByMrrID;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----delete  Record from ListPIDetails----***************//

        $scope.deleteRow = function (index) {
            // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
            $scope.ListMrrDetails.splice(index, 1);
            // $scope.showDtgrid = $scope.ListPIDetails.length;
        };

        //**********----Create Calculation----***************//
        $scope.calculationAmount = function (dataModel) {

            dataModel.UnitPrice = conversion.roundNumber((dataModel.Amount / dataModel.Qty),2);

            //$scope.ListChallanDetails1 = [];
            //angular.forEach($scope.ListMrrDetails, function (item) {

            //    var UnitPriceInDec = item.Amount /item.Qty;

            //    $scope.ListChallanDetails1.push({

            //        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

            //        PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
            //        Qty: item.Qty, UnitPrice: UnitPriceInDec, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
            //        GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: item.Amount,

            //        AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount

            //    });
            //    $scope.ListMrrDetails = $scope.ListChallanDetails1;
            //});
        }

        $scope.calculationUnitPrice = function (dataModel) {
             
            dataModel.Amount = conversion.roundNumber((dataModel.UnitPrice * dataModel.Qty), 2);

            //$scope.ListChallanDetails1 = [];
            //angular.forEach($scope.ListMrrDetails, function (item) {

            //    var AmountInDec = item.UnitPrice * item.Qty

            //    $scope.ListChallanDetails1.push({

            //        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

            //        PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
            //        Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
            //        GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: AmountInDec,

            //        AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount

            //    });
            //    $scope.ListMrrDetails = $scope.ListChallanDetails1;
            //});
        }

        $scope.calculationQuantity = function (dataModel) {
            //$scope.ListChallanDetails1 = [];
            dataModel.UnitPrice = 0.00;
            dataModel.Amount = 0.00;
            //angular.forEach($scope.ListMrrDetails, function (item) {

            //   // var AmountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);

            //    $scope.ListChallanDetails1.push({

            //        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

            //        PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
            //        Qty: item.Qty, UnitPrice: 0.00, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
            //        GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: 0.00,

            //        AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount

            //    });
            //    $scope.ListMrrDetails = $scope.ListChallanDetails1;
            //});
        }


 

        $scope.LoadLotModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListMrrDetails.indexOf(dataModel);
        }

        $scope.LoadBatchModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListMrrDetails.indexOf(dataModel);
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
            var SaveNewBatch = openingStockService.GetList(apiRoute, cmnParam);

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

                    $scope.ListMrrDetails[$scope.hfIndex] = {

                        MrrID: $scope.ListMrrDetails[$scope.hfIndex].MrrID,
                        MrrDetailID: $scope.ListMrrDetails[$scope.hfIndex].MrrDetailID,
                        ItemID: $scope.ListMrrDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListMrrDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListMrrDetails[$scope.hfIndex].UnitID,
                        LotID: $scope.ListMrrDetails[$scope.hfIndex].LotID,
                        BatchID: response.data,
                        PackingUnitID: $scope.ListMrrDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListMrrDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListMrrDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListMrrDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListMrrDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListMrrDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListMrrDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListMrrDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListMrrDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListMrrDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListMrrDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListMrrDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListMrrDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListMrrDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListMrrDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListMrrDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListMrrDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListMrrDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListMrrDetails[$scope.hfIndex].TotalAmount
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
            // var ChallanItemMasterNdetailsCreateUpdate = openingStockService.GetList(apiRoute, cmnParam);

            var apiRoute = baseUrl + '/SaveLot/';
            var cmnParam = "[" + JSON.stringify(lotMaster) + "]";
            var SaveNewLot = openingStockService.GetList(apiRoute, cmnParam);

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

                    $scope.ListMrrDetails[$scope.hfIndex] = {

                        MrrID: $scope.ListMrrDetails[$scope.hfIndex].MrrID,
                        MrrDetailID: $scope.ListMrrDetails[$scope.hfIndex].MrrDetailID,
                        ItemID: $scope.ListMrrDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListMrrDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListMrrDetails[$scope.hfIndex].UnitID,
                        LotID: response.data, //.LotID,
                        BatchID: $scope.ListMrrDetails[$scope.hfIndex].BatchID,
                        PackingUnitID: $scope.ListMrrDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListMrrDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListMrrDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListMrrDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListMrrDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListMrrDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListMrrDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListMrrDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListMrrDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListMrrDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListMrrDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListMrrDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListMrrDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListMrrDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListMrrDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListMrrDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListMrrDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListMrrDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListMrrDetails[$scope.hfIndex].TotalAmount
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

            var HedarTokenPostPut = $scope.MrrID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

            if ($scope.MrrID == 0) {
                var MrrDateStringToDate = conversion.getStringToDate($scope.OpeningStockDate);
                var mrrMaster = {
                    MrrID: $scope.MrrID,
                    MrrNo: $scope.HMRRNo,
                    MrrDate: MrrDateStringToDate,
                    MrrTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                    //  ManualMRRNo: $scope.ManualMRRNo,
                    // MrrID: $scope.lstGRRNoList,
                    //  RequisitionID: $scope.SprNo,
                    //   ReqNo: $("#txtSPRNo").select2('data').text,

                    //  POID: $scope.PONo,
                    //  PONo: $("#txtPONo").select2('data').text,
                    //  MrrQcID: $scope.lstQCNoList,
                    Remarks: $scope.Remarks,
                    Description: $scope.Description,
                    //  IssueID: $scope.lstIssueNoList, 
                    //  SupplierID: $scope.lstSupplierList,
                    //  CurrencyID: $scope.lstCurrencyList,
                    //  FromDepartmentID: $scope.FrmDeptIDByIssueChnge,
                    DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                    // DepartmentID: deptID,
                    StatusID: 1,
                    StatusBy: $scope.UserCommonEntity.loggedUserID,
                    CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                    CreateBy: $scope.UserCommonEntity.loggedUserID,
                    UserID: $scope.ngmStockByList, //$scope.UserCommonEntity.loggedUserID
                    IsStoreCompleted: true,
                    IsApproved: true,
                    IsAccountsCompleted: true
                };


                var menuID = $scope.UserCommonEntity.currentMenuID;
                var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
                var mrrDetail = $scope.ListMrrDetails;
                var chkQty = 1;
                angular.forEach($scope.ListMrrDetails, function (item) {
                    if (item.UnitPrice <= 0 && item.Qty<=0 && item.Amount<=0) {
                        chkQty = 0;
                    }
                });

                if ($scope.ListMrrDetails.length > 0) {

                    if (chkQty == 1) {
                        var apiRoute = baseUrl + 'SaveUpdateMrrMasterNdetails/';

                        var cmnParam = "[" + JSON.stringify(mrrMaster) + "," + JSON.stringify(mrrDetail) + "," + JSON.stringify(menuID) + "]";

                        var MrrMasterNdetailsCreateUpdate = mRRService.GetList(apiRoute, cmnParam, HedarTokenPostPut); //mRRService.postMrrMasterDetail(apiRoute, mrrMaster, mrrDetail, menuID, transactionTypeID);
                        MrrMasterNdetailsCreateUpdate.then(function (response) {
                            var result = 0;
                            if (response.data != "") {
                                $scope.HMRRNo = response.data;
                                // alert('Saved Successfully.');
                                Command: toastr["success"]("Save  Successfully!!!!");
                                $scope.clear();
                                // result = 1;
                            }
                            else if (response.data == "") {
                                    Command: toastr["warning"]("Save Not Successfull!!!!");
                                $("#save").prop("disabled", false);
                            }
                            // 
                            // ShowCustomToastrMessageResult(result);
                        },
                        function (error) {
                            // console.log("Error: " + error);
                            $("#save").prop("disabled", false);
                            Command: toastr["warning"]("Save Not Successfull!!!!");
                        });
                    }
                    else if (chkQty == 0) {
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Quantity/Unit Price/Amount Must Not Zero Or Empty !!!!");
                    }
                }
                else if ($scope.ListMrrDetails.length <= 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Stock Detail Must Not Empty!!!!");
                }
            }
            else  
                Command: toastr["warning"]("Update is not permitted !!!!");
            

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
             
            $("#save").prop("disabled", false);
            $scope.ListMrrDetails = [];

            var date = new Date();
            $scope.OpeningStockDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
           
            $scope.MrrID = "0";
           
             
            $scope.listStockBy = [];
            $scope.ngmStockByList = "";
            $("#ddlStockBy").select2("data", { id: "", text: "" });

            //$scope.listPINO = [];
            //$scope.PINO = "";
            //$('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });
             

            $scope.Remarks = "";

            $scope.Description = ""; 

            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.ListMrrDetails = [];
            loadStockByRecords(0);
            $scope.loadChallanRecords(0);
            

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
