
app.controller('preCostingCtrl', ['$scope', 'preCostingService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, preCostingService, conversion, $filter, $localStorage, uiGridConstants) {
        var partialUrl = '/Production/api/ChemiclePreparation/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.gridOptionsCostMaster = [];

        $scope.gridOptionslistItemMaster = [];

        $scope.gridFinishingPriceChange = [];

        $scope.ListFppDetails = [];
        $scope.ListDying = [];
        $scope.ListSizing = [];
        $scope.ListFinishing = [];
        $scope.ListTotal = [];
        $scope.ListYarn = [];
        

        function loadUserCommonEntity(num) {

            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);

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
            $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID; // for transactiontyeid not duplicate 
            $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);


        $scope.PageTitle = 'Pre Cost Creation';
        $scope.ListTitle = 'Pre Cost Records';
        $scope.ListTitleMasters = 'Pre Cost Information (Masters)';
        $scope.ListTitleDeatails = 'Pre Cost Information (Details)';


        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        
        $scope.IsHiddenDying = true;
        $scope.IsHiddenFinishing = true;
        $scope.IsHiddenSizing = true;
        $scope.IsHiddenYarn = true;
        $scope.IsHiddenTotal = true;

        $scope.TitleDyingGrid = "Dying Records";
        $scope.TitleFinishingGrid = "Finishing Records";
        $scope.TitleSizingGrid = "Sizing Records";
        $scope.TitleYarnGrid = "Yarn Records";
        $scope.TitleTotalGrid = "Total Records";


        var baseUrl = '/Accounting/api/PreCosting/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

  
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        var date = new Date();
        $scope.CostingDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
         
        $scope.CostingID = "0"; 
        $scope.ItemIDByBOMNOChange = "0";
     

        $scope.dyngTotal = "0";
        $scope.sizTotal = "0";
        $scope.yarnTotal = "0";
        $scope.finishTotal = "0";
        $scope.ttlTotal = "0";


        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.btnReviseText = "Update";

        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        $scope.cmnParam = function () {
            objcmnParam = conversion.cmnParams($scope.UserCommonEntity);
        }
        //****************************************************End Common Task for all***************************************************


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
                $scope.loadPreCostRecords(0);
            }
        }

        //*************************************************End Load ddl Finishing Type from Finishing Process****************************

        function loadBomNArticle() {
            objcmnParam = {
                        pageNumber: page,
                        pageSize: pageSize,
                        IsPaging: isPaging,
                        loggeduser: $scope.UserCommonEntity.loggedUserID,
                        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                        menuId: $scope.UserCommonEntity.currentMenuID,
                        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                    };

            var apiRoute = baseUrl + 'GetBomNArticleNo/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var BomNArticle = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
             
            BomNArticle.then(function (response) {
                $scope.listBOMnArticleNo = response.data.lstBomNArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBomNArticle();

     
        $scope.loadSelectedItemData = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;
            $scope.IsHidden = true;

            $scope.ListDying = [];
            $scope.ListSizing = [];
            $scope.ListFinishing = [];
            $scope.ListTotal = [];
            $scope.ListYarn = [];

            $scope.dyngTotal = "0";
            $scope.sizTotal = "0";
            $scope.yarnTotal = "0";
            $scope.finishTotal = "0";
            $scope.ttlTotal = "0";

            var itemID = 0;
            var existBomID = $scope.ngmBOMArticleNo;

            angular.forEach($scope.listBOMnArticleNo, function (item) {
                if (existBomID == item.BOMID) {
                    itemID = item.ItemID;
                    $scope.ItemIDByBOMNOChange = item.ItemID;
                    return false;
                }
            });



            if (itemID > 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetItemDetailByItemID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemID) + "]";

                var ItemDetail = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemDetail.then(function (response) {
                    $scope.CuttableWidth = response.data.lstBOM[0].CuttableWidth == null ? "" : response.data.lstBOM[0].CuttableWidth;
                    $scope.WeftYarn = response.data.lstBOM[0].WeftYarn == null ? "" : response.data.lstBOM[0].WeftYarn;
                    $scope.WarpYarn = response.data.lstBOM[0].WarpYarn == null ? "" : response.data.lstBOM[0].WarpYarn;
                    $scope.Construction = response.data.lstBOM[0].Construction == null ? "" : response.data.lstBOM[0].Construction;
                    $scope.Weightoz = response.data.lstBOM[0].WeightPerUnit == null ? "" : response.data.lstBOM[0].WeightPerUnit;
                    $scope.Color = response.data.lstBOM[0].ColorName == null ? "" : response.data.lstBOM[0].ColorName;
                    $scope.FinishingWidth = response.data.lstBOM[0].FinishingWidth == null ? "" : response.data.lstBOM[0].FinishingWidth;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {

                Command: toastr["warning"]("Select Article No !!!!");
            }
        }

         
        $scope.LoadDyingGrd = function () {

            $scope.IsHiddenDying = false;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;
            $scope.IsHidden = true;

            var bomID = $scope.ngmBOMArticleNo==undefined?"0":$scope.ngmBOMArticleNo;
             
            if (bomID > 0 && $scope.ListDying.length==0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetDyingByBomID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + ","+ JSON.stringify(bomID) + "]";

                var ItemDying = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemDying.then(function (response) {
                    $scope.ListDying = response.data.lstDying;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else if (bomID==0) {

                Command: toastr["warning"]("Select BOM No !!!!");
            }
        }


        $scope.LoadSizingGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = false;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;
            $scope.IsHidden = true;

            var bomID = $scope.ngmBOMArticleNo == undefined ? "0" : $scope.ngmBOMArticleNo;

            if (bomID > 0 && $scope.ListSizing.length == 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetSizingByBomID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(bomID)+ "]";

                var ItemSizing = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemSizing.then(function (response) {

                    $scope.ListSizing = response.data.lstSizing;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else if (bomID == 0) {

                Command: toastr["warning"]("Select BOM No !!!!");
            }
        }

        $scope.LoadFinishingGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = false;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;
            $scope.IsHidden = true;
             
            var itemID =  $scope.ItemIDByBOMNOChange;
            if (itemID > 0 && $scope.ListFinishing.length == 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetFinishingByItemID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemID) + "]";

                var ItemFinishing = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemFinishing.then(function (response) {
                    $scope.ListFinishing = response.data.lstFinishing;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else if (itemID == 0) {

                Command: toastr["warning"]("Select BOM No!!!!");
            }
        }

        $scope.LoadYarnGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = false;
            $scope.IsHiddenTotal = true;
            $scope.IsHidden = true;


            var itemID = $scope.ItemIDByBOMNOChange;
            if (itemID > 0 && $scope.ListYarn.length == 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetYarnByItemID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemID) + "]";

                var ItemYarn = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemYarn.then(function (response) {
                    $scope.ListYarn = response.data.lstYarn;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else if (itemID == 0) {
                Command: toastr["warning"]("Select BOM No!!!!");
            }
        }

 
        $scope.LoadTotalGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = false;

            $scope.IsHidden = true;

            $scope.CalTtlTotal();
        }

        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = '/Inventory/api/GRR/GetCurrency/';
            var listCurrency = preCostingService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);



        //  function callPaginationMthd() {
        //**********----Pagination----***************
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
                $scope.loadPreCostRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadPreCostRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadPreCostRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadPreCostRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadPreCostRecords(1);
                }
            }
        };
        //  }


        //**********----Get All pre cost Master Records----***************
        $scope.loadPreCostRecords = function (isPaging) {

            $scope.gridOptionsCostMaster.enableFiltering = true;
            $scope.gridOptionsCostMaster.enableGridMenu = true;

            //For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMoreBOMMaster = true;
            $scope.lblMessageForBOMMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: $scope.pagination.pageNumber == 1 ? 0 : 1,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
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

            $scope.gridOptionsCostMaster = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [

                    { name: "CostingID", displayName: "Costing ID", title: "Costing ID", visible: false, width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BOMID", displayName: "BOM ID", title: "BOM ID", visible: false, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "CostingNo", displayName: "Costing No", title: "Costing No", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CostingDate", displayName: "Costing Date", title: "Costing Date", cellFilter: 'date:"dd-MM-yyyy"', width: '15%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "BOMNO", displayName: "BOM No", title: "BOM No", width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "ItemName", displayName: "Item Name", title: "Item Name", width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "CurrencyName", displayName: "Currency", title: "Description", width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Action',
                        displayName: "Action",

                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,

                        width: '15%',
                        headerCellClass: $scope.highlightFilteredHeader
                        ,
                        cellTemplate: '<span class="label label-success label-mini" ng-if="grid.appScope.UserCommonEntity.EnableUpdate">' +
                                      '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.loadMasterDetailsByPreCostMaster(row.entity)">' +
                                            '<i class="glyphicon glyphicon-edit">&nbsp;Edit</i>' +
                                        '</a>' +
                                    '</span>' +
                                    '<span class="label label-warning label-mini" style="text-align:center !important" ng-if="grid.appScope.UserCommonEntity.EnableDelete">' +
                                            '<a href="javascript:void(0);" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="bs-tooltip" title="Delete" ng-href="#CmnDeleteModal" ng-click="grid.appScope.loadDelModel(row.entity)">' +
                                                '<i class="glyphicon glyphicon-trash" aria-hidden="true">&nbsp;Delete</i>' +
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
                exporterCsvFilename: 'Fpp.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "BOM", style: 'headerStyle' },
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


            var apiRoute = baseUrl + 'GetCostMasterList/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listgridOptionsBOMMaster = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listgridOptionsBOMMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsCostMaster.data = response.data.lstCostMaster;
                $scope.loaderMoreBOMMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
        $scope.loadPreCostRecords(0);

        //     **********----Load PreCost MasterForm and PreCost Details List By select PreCost Master ----***************//

        $scope.loadMasterDetailsByPreCostMaster = function (dataModel) {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;

            $scope.ListDying = [];
            $scope.ListSizing = [];

            $scope.IsShow = true;
            //
            $scope.btnrShowText = "Show List";
            $scope.IsHidden = true;
            //
            $scope.btnSaveText = "Update";

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
             
            };

            //objcmnParam1 = {
            //    pageNumber: page,
            //    pageSize: pageSize,
            //    IsPaging: isPaging,
            //    loggeduser: $scope.UserCommonEntity.loggedUserID,
            //    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
            //    menuId: $scope.UserCommonEntity.currentMenuID,
            //    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
            //    ItemType: 5,
            //    ItemGroup: 53
            //};
            // $scope.ListAllDetails = [];

            var apiRoute = baseUrl + 'GetDetailListByPrCostID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(dataModel.CostingID) + "]";
            var allDetails = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

            allDetails.then(function (response) {

                $("#ddlBomArticleNo").prop("disabled", true);

                // $scope.ListAllDetails = response.data.lstDetailInfoByBOMID; 
                $scope.ListDying = response.data.lstDetailInfoByCostID[0].ForDying;
                $scope.ListSizing = response.data.lstDetailInfoByCostID[0].ForSizing;
                $scope.ListYarn = response.data.lstDetailInfoByCostID[0].ForYarn; 
                $scope.ListFinishing = response.data.lstDetailInfoByCostID[0].ForFinishing; 
                $scope.ListTotal = response.data.lstDetailInfoByCostID[0].ForDetail;
                $scope.ItemIDByBOMNOChange = response.data.lstDetailInfoByCostID[0].ItemID;

                var dnTtl = 0;

                angular.forEach($scope.ListDying, function (Item) {
                    dnTtl = dnTtl + Item.Amount;
                });
                $scope.dyngTotal = conversion.roundNumber(dnTtl, 2);

               
                var szTtl = 0;
                angular.forEach($scope.ListSizing, function (Item) {
                    szTtl = szTtl + Item.Amount;
                });
                $scope.sizTotal = conversion.roundNumber(szTtl, 2);

                var yrnTtl = 0;
                angular.forEach($scope.ListYarn, function (Item) {
                    yrnTtl = yrnTtl + Item.Amount;
                });
                $scope.yarnTotal = conversion.roundNumber(yrnTtl, 2);
                 
                var fnsTtl = 0;
                angular.forEach($scope.ListFinishing, function (Item) {
                    fnsTtl = fnsTtl + Item.UnitPrice;
                }); 
                $scope.finishTotal = conversion.roundNumber(fnsTtl, 2);

                //var detTtl = 0;
                //angular.forEach($scope.ListTotal, function (Item) {
                //    detTtl = detTtl + Item.Amount;
                //});

                // $scope.ttlTotal = $scope.ListTotal[0].SizeCost + $scope.ListTotal[0].OverHeadCost + $scope.ListTotal[0].FinishingCost + $scope.ListTotal[0].DyingCost + $scope.ListTotal[0].YarnCost;

                $scope.ListTotal[0].ttlTotal = $scope.ListTotal[0].SizeCost + $scope.ListTotal[0].OverHeadCost + $scope.ListTotal[0].FinishingCost + $scope.ListTotal[0].DyingCost + $scope.ListTotal[0].YarnCost;
      




                if (response.data.lstDetailInfoByCostID[0].BOMID != null) {
                    $scope.ngmBOMArticleNo = response.data.lstDetailInfoByCostID[0].BOMID;
                    $('#ddlBomArticleNo').select2("data", { id: response.data.lstDetailInfoByCostID[0].BOMID, text: response.data.lstDetailInfoByCostID[0].BomArticleNo });
                }

                if (response.data.lstDetailInfoByCostID[0].CurrencyID != null) {
                    $scope.lstCurrencyList = response.data.lstDetailInfoByCostID[0].CurrencyID;
                    $('#ddlCurrency').select2("data", { id: response.data.lstDetailInfoByCostID[0].CurrencyID, text: response.data.lstDetailInfoByCostID[0].CurrencyName });
                }


                $scope.CostingID = response.data.lstDetailInfoByCostID[0].CostingID == null ? "" : response.data.lstDetailInfoByCostID[0].CostingID;
                $scope.HCostingNo = response.data.lstDetailInfoByCostID[0].CostingNo == null ? "" : response.data.lstDetailInfoByCostID[0].CostingNo;
                $scope.CostingDate = response.data.lstDetailInfoByCostID[0].CostingDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByCostID[0].CostingDate);

                $scope.CuttableWidth = response.data.lstDetailInfoByCostID[0].CuttableWidth == null ? "" : response.data.lstDetailInfoByCostID[0].CuttableWidth;

                $scope.WeftYarn = response.data.lstDetailInfoByCostID[0].WeftYarn == null ? "" : response.data.lstDetailInfoByCostID[0].WeftYarn;

                $scope.WarpYarn = response.data.lstDetailInfoByCostID[0].WarpYarn == null ? "" : response.data.lstDetailInfoByCostID[0].WarpYarn;
                $scope.Description = response.data.lstDetailInfoByCostID[0].Description == null ? "" : response.data.lstDetailInfoByCostID[0].Description;

                $scope.Construction = response.data.lstDetailInfoByCostID[0].Construction == null ? "" : response.data.lstDetailInfoByCostID[0].Construction;

                $scope.Weightoz = response.data.lstDetailInfoByCostID[0].WeightPerUnit == null ? "" : response.data.lstDetailInfoByCostID[0].WeightPerUnit;

                $scope.Color = response.data.lstDetailInfoByCostID[0].ColorName == null ? "" : response.data.lstDetailInfoByCostID[0].ColorName;

                $scope.FinishingWidth = response.data.lstDetailInfoByCostID[0].FinishingWidth == null ? "" : response.data.lstDetailInfoByCostID[0].FinishingWidth;

            },
        function (error) {
            console.log("Error: " + error);
        });
        }

        //********---------Delete Data-------**************
        $scope.loadDelModel = function (EntityModel) {
            // debugger
            $scope.SelectedItemName = "You are about to delete " + EntityModel.CostingNo + ". Are you sure?";
            $scope.CmnDelModel = EntityModel;
            $scope.IsConfShow = true;
            $scope.IsConfirmShow = false;
        }
        $scope.ConfirmYes = function () {
            // debugger
            $scope.DeleteUpdateMasterDetail($scope.CmnDelModel);
        }
        $scope.DeleteUpdateMasterDetail = function (delModel) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };

            var apiRoute = baseUrl + 'DeletePreCosting/';
            var costingID = delModel.CostingID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(costingID) + "]";

            var costDelete = preCostingService.GetList(apiRoute, cmnParam, $scope.HeaderToken.delete);

            costDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.clear();
                    Command: toastr["success"]("Data has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };

        $scope.calculationDyng = function (dataModel) {

            if (dataModel.LastPurchasePrice >= 0) {
                dataModel.Amount = conversion.roundNumber(dataModel.Qty * dataModel.LastPurchasePrice, 2)
                var dnTtl = 0;
                angular.forEach($scope.ListDying, function (Item) {
                    dnTtl = dnTtl + parseFloat(Item.Amount);
                });
                $scope.dyngTotal = conversion.roundNumber(dnTtl, 2);
            }
            $scope.CalTtlTotal();
        
        }

        $scope.calculationSiz = function (dataModel) {
          
            if (dataModel.LastPurchasePrice >= 0) {
                dataModel.Amount = conversion.roundNumber(dataModel.Qty * dataModel.LastPurchasePrice,2)
                var szTtl = 0;
                angular.forEach($scope.ListSizing, function (Item) {
                    szTtl = szTtl + parseFloat(Item.Amount);
                });
                $scope.sizTotal = conversion.roundNumber(szTtl, 2);
            }
            $scope.CalTtlTotal(); 
        }

        $scope.calculationFinish = function (dataModel) {
         
            if (dataModel.UnitPrice >= 0) {
                // dataModel.Amount = dataModel.Qty * dataModel.LastPurchasePrice
                var fnsTtl = 0;
                angular.forEach($scope.ListFinishing, function (Item) {
                    fnsTtl = fnsTtl + Item.UnitPrice;
                });
                $scope.finishTotal = conversion.roundNumber(fnsTtl, 2);
            }
            $scope.CalTtlTotal();
        }


        $scope.calculationYarn = function (dataModel)
        {
            if (dataModel.LastPurchasePrice >= 0)
            {
                dataModel.Amount = conversion.roundNumber(dataModel.Qty * dataModel.LastPurchasePrice,2)
                var yrnTtl = 0;
                angular.forEach($scope.ListYarn, function (Item) {
                    yrnTtl = yrnTtl +parseFloat(Item.Amount);
                });
                $scope.yarnTotal = conversion.roundNumber(yrnTtl, 2);
            }
            $scope.CalTtlTotal();
        }


        $scope.CalTtlTotal = function ()
        {
            if ($scope.ListTotal.length == 0) {
                var ovrHeadCost = 0;

                angular.forEach($scope.ListTotal, function (Item) {
                    ovrHeadCost = Item.OverHeadCost;
                });

                var totals = parseFloat($scope.yarnTotal) + parseFloat($scope.sizTotal) + parseFloat($scope.dyngTotal) + parseFloat($scope.finishTotal) + parseFloat(ovrHeadCost);

                $scope.ListTotal = [];

                $scope.ListTotal.push({ CostingDetailID: 0, CostingID: 0, YarnCost: $scope.yarnTotal, SizeCost: $scope.sizTotal, DyingCost: $scope.dyngTotal, FinishingCost: $scope.finishTotal, OverHeadCost: ovrHeadCost, UnitPrice: 0.00, CompanyID: $scope.UserCommonEntity.loggedCompnyID, IsDeleted: false, CreateBy: $scope.UserCommonEntity.loggedUserID, ttlTotal: conversion.roundNumber(totals,2) })
            }

            else if ($scope.ListTotal.length> 0) {

                var ovrHeadCost = 0;
                var varCostingDetailID = $scope.ListTotal[0].CostingDetailID;
                var varCostingID = $scope.ListTotal[0].CostingID;

                angular.forEach($scope.ListTotal, function (Item) {
                    ovrHeadCost = Item.OverHeadCost;
                });

                var totals = parseFloat($scope.yarnTotal) + parseFloat($scope.sizTotal) + parseFloat($scope.dyngTotal) + parseFloat($scope.finishTotal) + parseFloat(ovrHeadCost);

                $scope.ListTotal = [];

                $scope.ListTotal.push({ CostingDetailID: varCostingDetailID, CostingID: varCostingID, YarnCost: $scope.yarnTotal, SizeCost: $scope.sizTotal, DyingCost: $scope.dyngTotal, FinishingCost: $scope.finishTotal, OverHeadCost: ovrHeadCost, UnitPrice: 0.00, CompanyID: $scope.UserCommonEntity.loggedCompnyID, IsDeleted: false, CreateBy: $scope.UserCommonEntity.loggedUserID, ttlTotal: conversion.roundNumber(totals,2) })
            }
            
        }

        $scope.calculationOverHead =  function (dataModel)
        {
            var ovrhd = dataModel.OverHeadCost;
                $scope.ListTotal = [];
                var totals = $scope.yarnTotal + $scope.sizTotal + $scope.dyngTotal + $scope.finishTotal + ovrhd;

                $scope.ListTotal.push({ CostingDetailID: dataModel.CostingDetailID, CostingID: dataModel.CostingID, YarnCost: $scope.yarnTotal, SizeCost: $scope.sizTotal, DyingCost: $scope.dyngTotal, FinishingCost: $scope.finishTotal, OverHeadCost: ovrhd, UnitPrice: 0.00, CompanyID: $scope.UserCommonEntity.loggedCompnyID, IsDeleted: false, CreateBy: $scope.UserCommonEntity.loggedUserID, ttlTotal: conversion.roundNumber(totals, 2) })
          
        }

        //// **********----Save and Update   Records----***************//
        $scope.save = function () {
 
            var msgDyng = "";
            var msgSiz = "";
            var msgYarn = "";
            var msgFinishing = "";
            var msgTotal = "";
             

            msgDyng = $scope.ListDying.length == 0 ? " Dying " : "";
            msgSiz = $scope.ListSizing.length == 0 ? " Size " : "";

            msgYarn = $scope.ListYarn.length == 0 ? " Yarn " : "";
            msgFinishing = $scope.ListFinishing.length == 0 ? " Finishing " : "";
            msgTotal = $scope.ListTotal.length == 0 ? " Total " : "";

            $("#save").prop("disabled", true);

            var HedarTokenPostPut = $scope.CostingID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;
           
            var costDateStringToDate = conversion.getStringToDate($scope.CostingDate);
            var costMaster = {
                CostingID: $scope.CostingID,
                CostingNo: $scope.HCostingNo,
                CostingDate: costDateStringToDate,
                BOMID: $scope.ngmBOMArticleNo,
                CurrencyID: $scope.lstCurrencyList,
                ItemID: $scope.ItemIDByBOMNOChange, 
                Description: $scope.Description, 
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                IsDeleted: false
            };

            if (msgDyng == "" && msgSiz == "" && msgYarn == "" && msgFinishing == "" && msgTotal=="") {
                var menuID = $scope.UserCommonEntity.currentMenuID;
                var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
         

                var apiRoute = baseUrl + 'SaveNUpdatePreCosting/';

                var cmnParam = "[" + JSON.stringify(costMaster) + "," + JSON.stringify(menuID) + "," + JSON.stringify($scope.ListDying) + "," + JSON.stringify($scope.ListSizing) + "," + JSON.stringify($scope.ListFinishing) + "," + JSON.stringify($scope.ListYarn) + "," + JSON.stringify($scope.ListTotal) + "]";

                var costCreateUpdate = preCostingService.GetList(apiRoute, cmnParam, HedarTokenPostPut);
                costCreateUpdate.then(function (response) {

                    if (response.data != "") {

                        Command: toastr["success"]("Save  Successfully!!!!");
                        $scope.clear();
                        $scope.HCostingNo = response.data;
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
            else
                Command: toastr["warning"]("Please Enter Minimum One " + msgDyng + " " + msgSiz +" "+ msgYarn + " " +msgFinishing +" "+msgTotal +" Quantity");

        };



        //**********----Reset Record----***************//
        $scope.clear = function () {

            $("#ddlBomArticleNo").prop("disabled", false);
            $("#save").prop("disabled", false);

            $scope.CostingID = "0";

            $scope.ItemIDByBOMNOChange = "0";
      
            $scope.ListDying = [];
            $scope.ListSizing = [];
            $scope.ListFinishing = [];
            $scope.ListTotal = [];
            $scope.ListYarn = [];

            $scope.dyngTotal = "0";
            $scope.sizTotal = "0";
            $scope.yarnTotal = "0";
            $scope.finishTotal = "0";
            $scope.ttlTotal = "0";
             

            var date = new Date();
            $scope.CostingDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;
            $scope.IsHiddenYarn = true;
            $scope.IsHiddenTotal = true;

            $scope.listBOMnArticleNo = [];

            $scope.ngmBOMArticleNo = "";
            $('#ddlBomArticleNo').select2("data", { id: "", text: "--Select Article No--" });
          
            loadBomNArticle();

           // $scope.listCurrency = [];
            $scope.lstCurrencyList = "";
            $('#ddlCurrency').select2("data", { id: "", text: "--Select Currency--" });

           // loadCurrencyRecords(0);


            $scope.HCostingNo = "";
  
            $scope.CuttableWidth = "";

            $scope.WeftYarn = "";

            $scope.WarpYarn = "";
            $scope.Description = "";

            $scope.Construction = "";

            $scope.Weightoz = "";

            $scope.Color = "";

            $scope.FinishingWidth = "";

        };

    }]);

