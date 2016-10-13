 
/**
* MRRCtrl.js
*/


app.controller('billOfMaterialCtrl', ['$scope', 'billOfMaterialService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, billOfMaterialService, conversion, $filter, $localStorage, uiGridConstants) {
        var partialUrl = '/Production/api/ChemiclePreparation/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.gridOptionsBOMMaster = [];

        $scope.gridOptionslistItemMaster = [];

        $scope.gridFinishingPriceChange = [];

        $scope.ListFppDetails = [];
        $scope.ListDying = [];
        $scope.ListSizing = [];
        $scope.ListFinishing = [];

        $scope.IsActiveChkBox = true;

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


        $scope.PageTitle = 'Bill Of Material Creation';
        $scope.ListTitle = 'Bill Of Material Records';
        $scope.ListTitleMasters = 'Bill Of Material Information (Masters)';
        $scope.ListTitleDeatails = 'Bill Of Material Information (Details)';


        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.IsHiddenDying = true;
        $scope.IsHiddenFinishing = true;
        $scope.IsHiddenSizing = true;

        $scope.TitleDyingGrid = "Dying Records";
        $scope.TitleFinishingGrid = "Finishing Records";
        $scope.TitleSizingGrid = "Sizing Records";



        var baseUrl = '/Accounting/api/BillOfMaterial/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.BOMDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.BOMID = "0";
        $scope.dyngTotal = "0";
        $scope.sizTotal = "0";


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
                $scope.loadBOMRecords(0);
            }
        }

        //*************************************************End Load ddl Finishing Type from Finishing Process****************************

        function loadArticalAsFinChemical() {
            $scope.cmnParam();

            ModelsArray = [objcmnParam];
            var apiRoute = '/Production/api/ChemiclePreparation/GetChemical/';
            var ddlitemtype = billOfMaterialService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ddlitemtype.then(function (response) {
                $scope.listArticleNo = response.data.ListChemical;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadArticalAsFinChemical();

     

        $scope.loadSelectedItemData = function () {

            var itemID = $scope.ngmArticleNo;
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

                var ItemDetail = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
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

            //$scope.ListDying = [];
            //$scope.ListSizing = [];
            //$scope.ListFinishing = [];

            var gridLenth = $scope.ListDying.length;
            var itemTypeID = 5;
            var itemGroupID = 51;
            if (gridLenth == 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetItemDetailFDying/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemTypeID) + "," + JSON.stringify(itemGroupID) + "]";

                var ItemDying = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemDying.then(function (response) {
                    $scope.ListDying = response.data.lstDying;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //else {

            //    Command: toastr["warning"]("Select Article No !!!!");
            //}
        }


        $scope.LoadSizingGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = false;

            //$scope.ListDying = [];
            //$scope.ListSizing = [];
            //$scope.ListFinishing = [];

            var itemTypeID = 5;
            var itemGroupID = 53;

            var gridLenth = $scope.ListSizing;
            if (gridLenth == 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetItemDetailFSizing/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemTypeID) + "," + JSON.stringify(itemGroupID) + "]";

                var ItemSizing = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemSizing.then(function (response) {

                    $scope.ListSizing = response.data.lstSizing;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.LoadFinishingGrd = function () {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = false;
            $scope.IsHiddenSizing = true;

            //$scope.ListDying = [];
            //$scope.ListSizing = [];
            //$scope.ListFinishing = [];

            //var gridLenth = $scope.ListFinishing;
            // if (gridLenth != 0) {
            var itemID = $scope.ngmArticleNo;
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

                var apiRoute = baseUrl + 'GetFinishingByItemID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(itemID) + "]";

                var ItemFinishing = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                ItemFinishing.then(function (response) {
                    $scope.ListFinishing = response.data.lstFinishing;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {

                Command: toastr["warning"]("Select Article No !!!!");
            }
        }



        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = '/Inventory/api/GRR/GetCurrency/';
            var listCurrency = billOfMaterialService.getModel(apiRoute, page, pageSize, isPaging);
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
                $scope.loadBOMRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadBOMRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadBOMRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadBOMRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadBOMRecords(1);
                }
            }
        };
        //  }


        //**********----Get All BOM Master Records----***************
        $scope.loadBOMRecords = function (isPaging) {

            $scope.gridOptionsBOMMaster.enableFiltering = true;
            $scope.gridOptionsBOMMaster.enableGridMenu = true;

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

            $scope.gridOptionsBOMMaster = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [

                    { name: "BOMID", displayName: "BOM ID", title: "BOM No", visible: false, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "BOMNO", displayName: "BOM No", title: "BOM No", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BOMDate", displayName: "BOM Date", title: "BOM Date", cellFilter: 'date:"dd-MM-yyyy"', width: '15%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "ItemName", displayName: "Item Name", title: "Item Name", width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "Description", displayName: "Description", title: "Description", width: '25%', headerCellClass: $scope.highlightFilteredHeader },

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
                                      '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.loadMasterDetailsByBOMMaster(row.entity)">' +
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


            var apiRoute = baseUrl + 'GetBOMMasterList/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listgridOptionsBOMMaster = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listgridOptionsBOMMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsBOMMaster.data = response.data.lstBOMMaster;
                $scope.loaderMoreBOMMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
        $scope.loadBOMRecords(0);

        //     **********----Load BOM MasterForm and BOM Details List By select BOM Master ----***************//

        $scope.loadMasterDetailsByBOMMaster = function (dataModel) {

            $scope.IsHiddenDying = true;
            $scope.IsHiddenFinishing = true;
            $scope.IsHiddenSizing = true;

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
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                ItemType: 5,
                ItemGroup: 51
            };

            objcmnParam1 = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                ItemType: 5,
                ItemGroup: 53
            };
            // $scope.ListAllDetails = [];

            var apiRoute = baseUrl + 'GetDetailListByBOMID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(objcmnParam1) + "," + JSON.stringify(dataModel.BOMID) + "]";
            var allDetails = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

            allDetails.then(function (response) {

                $("#ddlArticleNo").prop("disabled", true);

                // $scope.ListAllDetails = response.data.lstDetailInfoByBOMID; 
                $scope.ListDying = response.data.lstDetailInfoByBOMID[0].ForDying;
                $scope.ListSizing = response.data.lstDetailInfoByBOMID[0].ForSizing;

                var dynTtl = 0;
                angular.forEach($scope.ListDying, function (dyngItem) {
                    dynTtl = dynTtl + dyngItem.Qty;
                });
                $scope.dyngTotal = dynTtl; 

                var sizTtl = 0;
                angular.forEach($scope.ListSizing, function (dyngItem) {
                    sizTtl = sizTtl + dyngItem.Qty;
                });
                $scope.sizTotal = sizTtl;


                if (response.data.lstDetailInfoByBOMID[0].ItemID != null) {
                    $scope.ngmArticleNo = response.data.lstDetailInfoByBOMID[0].ItemID;
                    $('#ddlArticleNo').select2("data", { id: response.data.lstDetailInfoByBOMID[0].ItemID, text: response.data.lstDetailInfoByBOMID[0].ArticleNo });
                }

                $scope.BOMID = response.data.lstDetailInfoByBOMID[0].BOMID == null ? "" : response.data.lstDetailInfoByBOMID[0].BOMID;
                $scope.HBOMNo = response.data.lstDetailInfoByBOMID[0].BOMNO == null ? "" : response.data.lstDetailInfoByBOMID[0].BOMNO;
                $scope.BOMDate = response.data.lstDetailInfoByBOMID[0].BOMDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByBOMID[0].BOMDate);

                $scope.CuttableWidth = response.data.lstDetailInfoByBOMID[0].CuttableWidth == null ? "" : response.data.lstDetailInfoByBOMID[0].CuttableWidth;

                $scope.WeftYarn = response.data.lstDetailInfoByBOMID[0].WeftYarn == null ? "" : response.data.lstDetailInfoByBOMID[0].WeftYarn;

                $scope.WarpYarn = response.data.lstDetailInfoByBOMID[0].WarpYarn == null ? "" : response.data.lstDetailInfoByBOMID[0].WarpYarn;
                $scope.Description = response.data.lstDetailInfoByBOMID[0].Description == null ? "" : response.data.lstDetailInfoByBOMID[0].Description;

                $scope.Construction = response.data.lstDetailInfoByBOMID[0].Construction == null ? "" : response.data.lstDetailInfoByBOMID[0].Construction;

                $scope.Weightoz = response.data.lstDetailInfoByBOMID[0].WeightPerUnit == null ? "" : response.data.lstDetailInfoByBOMID[0].WeightPerUnit;

                $scope.Color = response.data.lstDetailInfoByBOMID[0].ColorName == null ? "" : response.data.lstDetailInfoByBOMID[0].ColorName;

                $scope.FinishingWidth = response.data.lstDetailInfoByBOMID[0].FinishingWidth == null ? "" : response.data.lstDetailInfoByBOMID[0].FinishingWidth;

            },
        function (error) {
            console.log("Error: " + error);
        });
        }

        //********---------Delete Data-------**************
        $scope.loadDelModel = function (EntityModel) {
            // debugger
            $scope.SelectedItemName = "You are about to delete " + EntityModel.BOMNO + ". Are you sure?";
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

            var apiRoute = baseUrl + 'DeleteBOM/';
            var bomID = delModel.BOMID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(bomID) + "]";

            var bomDelete = billOfMaterialService.GetList(apiRoute, cmnParam, $scope.HeaderToken.delete);

            //var SetMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            bomDelete.then(function (response) {
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
            //if (dataModel.Qty <= 0 || dataModel.Qty == "") {
            //    dataModel.Qty = 0;
            //    Command: toastr["warning"]("Please Enter Dying Quantity.");
            //}

            //else
                if (dataModel.Qty > 0) {
                var dynTtl = 0;
                angular.forEach($scope.ListDying, function (dyngItem) {
                    dynTtl = dynTtl + dyngItem.Qty;
                });
                $scope.dyngTotal = dynTtl;
            }
        }

        $scope.calculationSiz = function (dataModel) {
            //if (dataModel.Qty <= 0 || dataModel.Qty == "") {
            //    dataModel.Qty = 0;
            //    Command: toastr["warning"]("Please Enter Size Quantity.");
            //}

            //else
                if (dataModel.Qty > 0) {
                var sizTtl = 0;
                angular.forEach($scope.ListSizing, function (dyngItem) {
                    sizTtl = sizTtl + dyngItem.Qty;
                });
                $scope.sizTotal = sizTtl;
            }
        }


        //// **********----Save and Update   Records----***************//
        $scope.save = function () {

            //var checkQtyFDyng = $scope.ListDying.length == 0 ? 0 : 1;
            //var checkQtyFSize = $scope.ListSizing.length == 0 ? 0 : 1;

            $scope.newListDying = [];
            $scope.newListSizing = [];
            var msgDyng = "";
            var msgSiz = "";

            angular.forEach($scope.ListDying, function (dyngItem) {
                if (dyngItem.Qty > 0) {
                    $scope.newListDying.push(dyngItem);
                }
            });

            angular.forEach($scope.ListSizing, function (sizeItem) {
                if (sizeItem.Qty > 0) {
                    $scope.newListSizing.push(sizeItem);
                }
            });

            msgDyng = $scope.newListDying.length == 0 ? " Dying " : "";
            msgSiz = $scope.newListSizing.length == 0 ? " Size " : "";


            $("#save").prop("disabled", true);

            var HedarTokenPostPut = $scope.BOMID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;
            var bomDateStringToDate = conversion.getStringToDate($scope.BOMDate);
            var bomMaster = {
                BOMID: $scope.BOMID,
                BOMNO: $scope.HBOMNo,
                BOMDate: bomDateStringToDate,
                Description: $scope.Description,
                ItemID: $scope.ngmArticleNo,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                IsDeleted: false
            };

            if (msgDyng == "" && msgSiz == "") {
                var menuID = $scope.UserCommonEntity.currentMenuID;
                var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
                var DyingData = $scope.ListDying;
                var DataSizing = $scope.ListSizing;

                var apiRoute = baseUrl + 'SaveNUpdateBOM/';

                var cmnParam = "[" + JSON.stringify(bomMaster) + "," + JSON.stringify(menuID) + "," + JSON.stringify($scope.newListDying) + "," + JSON.stringify($scope.newListSizing) + "]";

                var bomCreateUpdate = billOfMaterialService.GetList(apiRoute, cmnParam, HedarTokenPostPut);
                bomCreateUpdate.then(function (response) {

                    if (response.data != "") {

                        Command: toastr["success"]("Save Successfully!!!!");
                        $scope.clear();
                        $scope.HBOMNo = response.data;
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
                Command: toastr["warning"]("Please Enter Minimum One " + msgDyng + " " + msgSiz + " Quantity");

        };



        //**********----Reset Record----***************//
        $scope.clear = function () {

            $("#ddlArticleNo").prop("disabled", false);
            $("#save").prop("disabled", false);
            $scope.BOMID = "0";
            $scope.dyngTotal = "0";
            $scope.sizTotal = "0";
            var date = new Date();
            $scope.BOMDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

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

            $scope.ListDying = [];
            $scope.ListSizing = [];

            $scope.newListDying = [];
            $scope.newListSizing = [];


            $scope.listArticleNo = [];
            $scope.ngmArticleNo = "";
            $('#ddlArticleNo').select2("data", { id: "", text: "--Select Article No--" });

            loadArticalAsFinChemical();

            $scope.HBOMNo = "";

            var date = new Date();
            $scope.BOMDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

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



