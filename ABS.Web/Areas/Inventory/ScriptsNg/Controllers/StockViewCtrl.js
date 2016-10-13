


app.controller('StockViewCtrl', ['$scope', 'StockEntryService', '$filter', 'uiGridConstants','$localStorage',
function ($scope, StockEntryService, $filter, uiGridConstants,$localStorage) {

        //function loadUserCommonEntity(num) {
        //    $scope.UserCommonEntity = {};
        //    $scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
        //    $scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
        //    $scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
        //    $scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
        //    $scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
        //    $scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
        //    $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
        //    $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
        //    $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
        //    console.log($scope.UserCommonEntity);
        //}
    //loadUserCommonEntity(0);

    //***************************************** Start Load User Common Entity ****************************************
    $scope.loadUserCommonEntity = function (num) {
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
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
        console.log($scope.UserCommonEntity);
        //console.log($scope.HeaderToken.get);
    }
    $scope.loadUserCommonEntity(0);
    //****************************************** End Load User Common Entity *****************************************

        $scope.gridOptionsSV = [];
        var objcmnParam = {};

        var baseUrl = '/Inventory/api/StockInfo/';
        var LoggedUserID = $scope.UserCommonEntity.loggedUserID;
        var LoggedCompanyID = $scope.UserCommonEntity.loggedCompnyID;
   
        var isExisting = 0;
        var page = 1;
        var pageSize = 50;
        var isPaging = 0;
        var totalData = 0;
        $scope.IsShow = true;
        $scope.IsListShow = true;
        $scope.ItemID = 0;
        var UserTypeID = 3;

        
        $scope.btnShowText = "Show Stock";
        $scope.PageTitle = 'Stock View';
        $scope.ListTitle = 'Item List';
        $scope.ListTitleSampleNo = 'Item Info';

        $scope.IsVisible = false;
        $scope.ShowHide = function () {
            $scope.IsVisible = $scope.ShowSupplier;
        }

    //*******************Item Item Type  On Page Load-- ***********

        $scope.getTypes = function () {
            $scope.Types = [
                {
                    Item: 'Finish Good'
                    ,Value: 1 // For Raw Material
                },
                {
                Item: 'Raw Material'
                , Value: 2 // For Raw Material
                },
                {
                    Item: 'Yarn'
                    ,Value: 3 // For Yarn
                },
            {
                Item: 'Chemical'
                , Value: 5 // For Chemical
            }, {
                Item: 'Fixed Asset'
                , Value: 4 // For Fixed Asset
            }];
        }
        $scope.getTypes();

    //*******************Item Group   On Page Load-- ***********

        $scope.getItemGroupsByType = function () {
            var apiRoute = '/SystemCommon/api/RawMaterial/GetItemGroups/';
            if ($scope.ItemType != "") {
                var itemGroupes = StockEntryService.getAllItemGroup(apiRoute, page, pageSize, isPaging, $scope.ItemType, LoggedCompanyID);
                itemGroupes.then(function (response) {
                    $scope.itemGroupes = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                $scope.itemGroupes = "";
            }
        }
    // -------Get All Users //--------
        $scope.UserLst = [];

        function GetAllUsers() {

            var apiRoute = '/Inventory/api/Requisition/GetAllUsers/';
            var UserLst = StockEntryService.getAllUsers(apiRoute, page, pageSize, isPaging, UserTypeID);
            UserLst.then(function (response) {
                $scope.UserLst = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllUsers();
       
   
    //*******************organogram  DropDown On Page Load-- ***********
        function loadRecords_Organogram(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganograms = StockEntryService.GetDrpOrganograms(apiRoute, LoggedCompanyID, LoggedUserID, page, pageSize, isPaging);
            listOrganograms.then(function (response) {
                $scope.ListOrganogram = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Organogram(0);

    //******************** Empty Controls*************************//

        $scope.EmptyItemDetail = function () {
            $("#ddlDepartment").select2("data", { id: 0, text: '--Select Department--' });
            $("#ddlItemType").select2("data", { id: 0, text: '--Select Item Type--' });
            $("#ddlItemGroup").select2("data", { id: 0, text: '--Select Group--' });
            $("#ddlSupplier").select2("data", { id: 0, text: '--Select Supplier--' });
            $scope.ShowSupplier = false;
        }

        //Pagination
        $scope.pagination = {
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
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.LoadAllItems();
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.LoadAllItems();
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.LoadAllItems();
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.LoadAllItems();
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.LoadAllItems();
                }
            }
        };

        //ui-Grid Call
        $scope.LoadAllItems = function () {
            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0,
                ItemType: $scope.ItemType,
                ItemGroup: $scope.ItemGroup,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID

            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsSV = {
                columnDefs: [                   
                    { name: "Item", title: "ItemName", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ReceiveQty", title: "Receive Qty", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                     { name: "IssueQty", title: "Issue Qty", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                      { name: "CurrentStock", title: "Current Stock", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                      { name: "LastReceiveDate", title: "Last Receive Date", width: '15%', cellFilter: 'date:\'yyyy-MM-dd\'', headerCellClass: $scope.highlightFilteredHeader },
                      { name: "LastIssueDate", title: "Last Issue Date", width: '15%', cellFilter: 'date:\'yyyy-MM-dd\'', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: "Unit", title: "Unit", width: '15%', visible:false,
                        filters: [{ condition: uiGridConstants.filter.GREATER_THAN, placeholder: 'Minimum' }, { condition: uiGridConstants.filter.LESS_THAN, placeholder: 'Maximum' }],
                        headerCellClass: $scope.highlightFilteredHeader
                    },
                    {
                        name: "CurrentStock", title: "CurrentStock", width: '10%',
                        filters: [{ condition: uiGridConstants.filter.GREATER_THAN, placeholder: 'Minimum' }, { condition: uiGridConstants.filter.LESS_THAN, placeholder: 'Maximum' }],
                        headerCellClass: $scope.highlightFilteredHeader
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'CurrentStock.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Item Groups", style: 'headerStyle' },
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


            var apiRoute = baseUrl + 'GetAllStockItems/';
            var _finishGoods = StockEntryService.getStockItems(apiRoute, objcmnParam);
            _finishGoods.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsSV.data = response.data.objListItem;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.LoadAllItems();



        $scope.Show = function GetStock() {
            var apiRoute = baseUrl + 'GetAllStockItems/';
            objcmnParam = {
                pageNumber: 0, //(($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: 0, //$scope.pagination.pageSize,
                IsPaging: 0, //isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0,
                ItemType: $scope.ItemType,
                ItemGroup: $scope.ItemGroup,
                //DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID
                DepartmentID: $scope.Department

            };
            var listStock = StockEntryService.getStockItems(apiRoute, objcmnParam);
            listStock.then(function (response) {
                $scope.ListStockItems = response.data
                $scope.gridOptionsSV.data = response.data.objListItem;
            },
            function (error) {
                console.log("Error: " + error);
            });
            $scope.EmptyItemDetail();
        }


    }]);





