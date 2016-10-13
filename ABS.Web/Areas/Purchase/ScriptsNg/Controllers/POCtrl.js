/**
* POCtrl.js   //    
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('pOCtrl', ['$scope', 'pOService', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, pOService, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsPOMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.gridOptionsItemPop = [];
        var objcmnParamItemPop = {};
      

        // $scope.IsItemTypeFinishedGoods = false;

        // $scope.IsItemTypeRMOthers = true;  //  for  Default Raw Material or Others

        $scope.HPONo = "";
        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";

        var baseUrl = '/Purchase/api/PurchaseOrder/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.PanelTitle = "Terms & Condition";
        var date = new Date();
        $scope.PODate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.LCVoucherDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.ShipmentDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.ExpireDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.ChallanTypeID = 5;
        $scope.MenuID = 93;
        $scope.POID = "0";

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'PO Creation';
        $scope.ListTitle = 'PO Records';
        $scope.ListTitleGRRMasters = 'PO Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleGRRDeatails = 'Listed Item of PO';

        $scope.ListPODetails = [];
        $scope.ListPODetailsForSearch = [];

        $scope.listSalesPerson = [];

        //  $scope.btnModal = "Add";

        $scope.btnLotModal = "Save";
        $scope.btnBatchModal = "Save";
        $scope.listMrrLot = [];
        $scope.listMrrBatchNo = [];
        $scope.LotSetup = "Lot Setup";
        $scope.BatchSetup = "Batch Setup";


        //***************************************** Start Load User Common Entity ****************************************
        $scope.loadUserCommonEntity = function (num) {
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
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
                $scope.loadPORecords(0);
            }
        }
 
        //######  Load Statement No ####################//

        function LoadStatementNo() {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };
            var apiRoute = baseUrl + 'GetStatementNo/';
            $scope.listStatementNo = [];
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listStmentNo = pOService.GetList(apiRoute, cmnParam);
            listStmentNo.then(function (response) {
                $scope.listStatementNo = response.data.objStatementNo;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        LoadStatementNo();


        //#############  Load Order Type ####################//

        function loadOrderType(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };

            var apiRoute = baseUrl + 'GetOrderType/';
            var ComboType = "OrderType";

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ComboType) + "]";
            $scope.listOrderType = [];
            var listOrderType = pOService.GetList(apiRoute, cmnParam);
            listOrderType.then(function (response) {
                $scope.listOrderType = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadOrderType(0);

        //#############  Load MoneyTransType ####################//

        function loadMoneyTransType(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };

            var apiRoute = baseUrl + 'GetMoneyTrnsType/';

            var ComboType = "MoneyTransType";
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ComboType) + "]";

            $scope.listMoneyTrnsType = [];
            var listMoneyTrnsType = pOService.GetList(apiRoute, cmnParam);
            listMoneyTrnsType.then(function (response) {
                $scope.listMoneyTrnsType = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadMoneyTransType(0);




        //**********----Get   bank and branch record ----***************//

        var defaultBankID = "";

        $scope.LoadBankAdvisingByCompanyID = function () {

            var apiRoute = '/Sales/api/PI/GetBankAdvisingListByCompanyID/';
            var companyID = $scope.UserCommonEntity.loggedCompnyID;

            $scope.listBankAdvising = [];
            $scope.listBankBranch = [];
            $scope.lstBankAdvisingList = '';
            $scope.lstBankBranchList = '';
            $("#ddlBankAdvising").select2("data", { id: '', text: '--Select Bank--' });
            $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
            var listBankAdvising = pOService.GetList(apiRoute, companyID, $scope.HeaderToken.get);
            listBankAdvising.then(function (response) {
                $scope.listBankAdvising = response.data;
                angular.forEach($scope.listBankAdvising, function (item) {
                    if (item.IsDefaultBankAdvising == true) {
                        defaultBankID = item.BankID;
                        // $("#ddlBankAdvising").select2("data", { id: item.BankID, text: item.BankName });

                        $scope.lstBankAdvisingList = item.BankID;
                        $("#ddlBankAdvising").select2("data", { id: item.BankID, text: item.BankName });

                        $scope.LoadBranchByBankID();
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.LoadBankAdvisingByCompanyID();

        $scope.LoadBranchByBankID = function () {
            // var ki = defaultBankID;
            var apiRoute = '/Sales/api/PI/GetBranchListByBankID/';
            var bankID = defaultBankID != "" ? defaultBankID : $scope.lstBankAdvisingList;
            defaultBankID = "";
            $scope.listBankBranch = [];
            $scope.lstBankBranchList = '';
            $("#ddlBankBranch").select2("data", { id: '', text: '--Select Bank  Branch--' });
            var listBankBranch = pOService.GetList(apiRoute, bankID, $scope.HeaderToken.get);
            listBankBranch.then(function (response) {
                $scope.listBankBranch = response.data;
                angular.forEach($scope.listBankBranch, function (item) {
                    if (item.IsDefaultBankBranch == true) {
                        // debugger
                        //  $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

                        $scope.lstBankBranchList = item.BranchID;
                        $("#ddlBankBranch").select2("data", { id: item.BranchID, text: item.BranchName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********---- Load  Item Details By StatementNo Change ----***************//

        $scope.LoadItemByStatementNoChange = function () {

            $scope.POID = "0";

            $scope.btnSaveText = "Save";

            $scope.ListPODetails = [];
            $scope.IsHiddenDetail = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0
            };

            var statemntID = $scope.StatementNo;
            var apiRoute = baseUrl + 'GetItemDetailByStatementNo/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(statemntID) + "]";

            var itemByStatementNo = pOService.GetList(apiRoute, cmnParam);

            // var itemBySprNo = pOService.GetItemDetailBySPRID(apiRoute, objcmnParam, sprID);
            itemByStatementNo.then(function (response) {


                $scope.ListPODetails = response.data;

                $scope.listSprNo = [];
                $scope.SprNo = '';
                $("#ddlSPRNo").select2("data", { id: '', text: '' });

                if (response.data[0].RequisitionID != null) {
                    $scope.listSprNo.push({
                        RequisitionID: response.data[0].RequisitionID, RequisitionNo: response.data[0].RequisitionNo
                    });
                    $scope.SprNo = response.data[0].RequisitionID;
                    $("#ddlSPRNo").select2("data", { id: response.data[0].RequisitionID, text: response.data[0].RequisitionNo });
                }


                $scope.listParty = [];
                $scope.lstPartyList = '';
                $("#ddlParty").select2("data", { id: '', text: '' });

                if (response.data[0].UserID != null) {
                    $scope.listParty.push({
                        UserID: response.data[0].UserID, UserFullName: response.data[0].UserFullName
                    });
                    $scope.lstPartyList = response.data[0].UserID;
                    $("#ddlParty").select2("data", { id: response.data[0].UserID, text: response.data[0].UserFullName });
                }


                $scope.listCurrency = [];
                $scope.lstCurrencyList = '';
                $("#ddlCurrency").select2("data", { id: '', text: '' });

                if (response.data[0].CurrencyID != null) {
                    $scope.listCurrency.push({
                        Id: response.data[0].CurrencyID, CurrencyName: response.data[0].CurrencyName
                    });

                    $scope.lstCurrencyList = response.data[0].CurrencyID;
                    $("#ddlCurrency").select2("data", { id: response.data[0].CurrencyID, text: response.data[0].CurrencyName });
                }


            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.ListTerms = [];
        $scope.GetTerms = function (entity) {

            if((entity.Sequence <= 0) || typeof (entity.Sequence) === "undefined")
            {
                Command: toastr["warning"]("Please enter sequence no.!!!!");
               // $('#txtSequence').focus()
                entity.Selected = false;           
                return;
            }          
            var existTerm = entity.TermID;
            var duplicateItem = 0;
            angular.forEach($scope.ListTerms, function (item) {
                if (existTerm == item.TermID) {
                    duplicateItem = 1;
                    return false;
                }
            });
            if (duplicateItem === 0)
            {
            $scope.ListTerms.push({
                TermID: entity.TermID,
                Sequence: entity.Sequence,
                Selected: entity.Selected
            });
            }
            if (duplicateItem === 1)
            {                
                for (var i = 0; i < $scope.ListTerms.length; i++) {
                    if ($scope.ListTerms[i].TermID == existTerm) {
                        $scope.ListTerms.splice(i, 1);
                        break;
                    }
                }
            }
        }

        //Grid-2
        //Pagination
        $scope.paginationItemPop = {
            paginationPageSizesItemPop: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSizeItemPop: 15,
            pageNumber: 0,
            pageSize: 15,
            totalItemsPop: 0,

            getTotalPagesItemPop: function () {
                return Math.ceil(this.totalItemsPop / this.pageSize);
            },
            pageSizeChangeItemPop: function () {
                if (this.ddlpageSizeItemPop == "All") {
                    this.pageNumber = 1
                    this.pageSize = $scope.paginationItemPop.totalItemsPop;
                    loadItemPop_Records(1);
                }
                else {
                    this.pageSize = this.ddlpageSizeItemPop
                    this.pageNumber = 1
                    loadItemPop_Records(1);
                }
            },
            firstPageItemPop: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadItemPop_Records(1);
                }
            },
            nextPageItemPop: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadItemPop_Records(1);
                }
            },
            previousPageItemPop: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadItemPop_Records(1);
                }
            },
            lastPageItemPop: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadItemPop_Records(1);
                }
            }
        };

        //**********----Get All Record----***************
        function loadItemPop_Records(isPaging) {

            // For Loading   
            $scope.loaderMore = true;
            $scope.lblMessageItemPop = 'loading please wait....!';
            $scope.resultItemPop = "color-red";

            //Ui Grid
            objcmnParamItemPop = {
                pageNumber: $scope.paginationItemPop.pageNumber,
                pageSize: $scope.paginationItemPop.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsItemPop = {
                columnDefs: [
                    { name: "TermID", displayName: "TermID", visible: false, headerCellClass: $scope.highlightFilteredHeader },                
                    { name: "Description", displayName: "Description", width: '85%', headerCellClass: $scope.highlightFilteredHeader },
                   {
                       name: 'Sequence',
                       displayName: "Sequence",
                       width: '10%',
                       cellTemplate: '<span>' +                                     
                                   '<input type="number" id="txtSequence" min="0" placeholder="0" ng-model="row.entity.Sequence" name = "txtSequence"/>' + '</span>'
                   },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '10%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span>' +                                     
                                    '<input type="checkbox" ng-click="grid.appScope.GetTerms(row.entity)" ng-model="row.entity.Selected" />' +
                                     '</span>'

                        //cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                        //             '<a href="" title="Add" ng-click="grid.appScope.GetItemDetailsByItemID(row.entity)">' +//grid.appScope.getItmDetailsByItemId(row.entity)"
                        //               '<i class="icon-check" aria-hidden="true"></i> Add' +
                        //             '</a>' +
                        //             '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Item.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Item", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetTermCondition/';
            var listItemMaster = pOService.GetTerm(apiRoute, objcmnParamItemPop);
            listItemMaster.then(function (response) {
                $scope.paginationItemPop.totalItemsPop = response.data.recordsTotal;
                $scope.gridOptionsItemPop.data = response.data.objTerms;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadItemPop_Records(0);





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
                $scope.loadPORecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadPORecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadPORecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadPORecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadPORecords(1);
                }
            }
        };

        //**********----Get All Challan Master Records----***************
        $scope.loadPORecords = function (isPaging) {

            $scope.gridOptionsPOMaster.enableFiltering = true;
            $scope.gridOptionsPOMaster.enableGridMenu = true;
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
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsPOMaster = {

                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,


                columnDefs: [
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PODetailID", displayName: "PODetailID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionID", displayName: "RequisitionID", title: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "CreateBy", displayName: "CreateBy", title: "CreateBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "PONo", displayName: "PO No", title: "PO No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PODate", displayName: "PO Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "Spr No", title: "Spr No", visible: true, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserFullName", displayName: "Party", title: "Supplier", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    //{ name: "CountryName", displayName: "Origin Country", title: "Origin Country", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "OrderTypeName", displayName: " Order Type", width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "MoneyTransactionTypeName", displayName: "Money Transaction Type", title: "Money Transaction Type", width: '15%', visible: true, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "LCorVoucherorLcafNo", displayName: "L/C No", title: "L/C No", visible: true, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "LCorVoucherorLcafDate", displayName: "LC Voucheror Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShipmentDate", displayName: "Shipment Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ExpireDate", displayName: "Expire Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BankAccountNo", displayName: "Account No", title: "Account No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "AccAmount", displayName: "Amount", title: "Amount", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BankName", displayName: "Bank Name", title: "Bank Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BranchName", displayName: "Branch Name", title: "Branch Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "CurrencyName", displayName: "Currency", title: "Currency", width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '13%',
                        pinnedRight:true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,

                        cellTemplate: '<span class="label label-success label-mini">' +
                                   '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                                         '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                                     '</a>' +
                                 '</span>' +
                                 '<span class="label label-success label-mini">' +
                                   '<a href="" title="Edit" ng-click="grid.appScope.loadPOMasterDetailsByPONo(row.entity)">' +
                                     '<i class="icon-edit"></i> Edit' +
                                   '</a>' +
                                   '</span>'


                        //cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //              '<a href="" title="Select" ng-click="grid.appScope.loadPOMasterDetailsByPONo(row.entity)">' +
                        //                '<i class="icon-check" aria-hidden="true"></i> Select' +
                        //              '</a>' +
                        //              '</span>'
                    }

                ],

                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsPOMaster.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsPOMaster.useExternalPagination = false;
                        $scope.gridOptionsPOMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetPOMasterList/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listChallanMaster = pOService.GetList(apiRoute, cmnParam);
            // var listChallanMaster = pOService.getChallanMasterList(apiRoute, objcmnParam);
            listChallanMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsPOMaster.data = response.data.lstPOMaster;
                $scope.loaderMoreChallanMaster = false;
                $scope.lblMessageForChallanMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        $scope.loadPORecords(0);

      

        //**********----Load PO MasterForm and PO Details List By select PO Master ----***************//

        $scope.loadPOMasterDetailsByPONo = function (dataModel) {


           // modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            // 

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnSaveText = "Update";

            $scope.HPONo = dataModel.PONo;
            $scope.POID = dataModel.POID;
            $scope.PODate = conversion.getDateToString(dataModel.PODate);

            $scope.StatementNo = dataModel.QuotationID;
            $("#ddlStatementNo").select2("data", { id: dataModel.QuotationID, text: dataModel.QuotationNo });

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
             

            // $scope.FoundNo = dataModel.FoundNo;

            $scope.LCBankVoucherNo = dataModel.LCorVoucherorLcafNo; 

            $scope.LCVoucherDate = conversion.getDateToString(dataModel.LCorVoucherorLcafDate); 
            $scope.ShipmentDate = conversion.getDateToString(dataModel.ShipmentDate);
            $scope.ExpireDate = conversion.getDateToString(dataModel.ExpireDate);

             
            $scope.OrderType = dataModel.OrderTypeID;
            $("#ddlOrderType").select2("data", { id: dataModel.OrderTypeID, text: dataModel.OrderTypeName });

            $scope.MoneyTrnsTypes = dataModel.MoneyTransactionTypeID;
            $("#ddlMoneyTrnsType").select2("data", { id: dataModel.MoneyTransactionTypeID, text: dataModel.MoneyTransactionTypeName });

            $scope.lstBankAdvisingList = dataModel.BankID;
            $("#ddlBankAdvising").select2("data", { id: dataModel.BankID, text: dataModel.BankName });

            $scope.LoadBankAdvisingByCompanyID();

           // $scope.LoadBranchByBankID();

            $scope.lstBankBranchList = dataModel.BankBranchID;
            $("#ddlBankBranch").select2("data", { id: dataModel.BankBranchID, text: dataModel.BranchName });
             

            $scope.AccountNo = dataModel.BankAccountNo;
            $scope.Amount = dataModel.AccAmount;

            $scope.Description = dataModel.Description;
            $scope.Remarks = dataModel.Remarks;
              
              

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

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };

            var apiRoute = baseUrl + 'GetPODetailByPOID/';
            var poid = dataModel.POID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(poid) + "]";

            $scope.ListPODetails = [];

            var ListPODetails = pOService.GetList(apiRoute, cmnParam);

            //  var ListPODetails = pOService.GetChallanDetailByChallanID(apiRoute, objcmnParam, grrid);

            ListPODetails.then(function (response) {
                $scope.ListPODetails = response.data.lstPODetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----delete  Record from ListPIDetails----***************//

        $scope.deleteRow = function (index) {
           
            $scope.ListPODetails.splice(index, 1); 
        };
         

        //**********----Save and Update PurchasePOMaster and PurchasePODetail  Records----***************//
        $scope.save = function () {
            $("#save").prop("disabled", true);
            var NewStringToDate = conversion.getStringToDate($scope.PODate);
            var lcVoucherDate = conversion.getStringToDate($scope.LCVoucherDate);
            var shipmentDate = conversion.getStringToDate($scope.ShipmentDate);
            var expireDate = conversion.getStringToDate($scope.ExpireDate);
             
            var itemMaster = {
                POID: $scope.POID,
                PONo: $scope.HPONo, 
                PODate: NewStringToDate,
                RequisitionID: $scope.SprNo,
                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                PartyID: $scope.lstPartyList,
                OrderTypeID: $scope.OrderType,
                MoneyTransactionTypeID: $scope.MoneyTrnsTypes,
                LCorVoucherorLcafNo: $scope.LCBankVoucherNo,
                LCorVoucherorLcafDate: lcVoucherDate,
                CurrencyID: $scope.lstCurrencyList,  
                QuotationID: $scope.StatementNo,
                BankID: $scope.lstBankAdvisingList,
                BankBranchID: $scope.lstBankBranchList,
                BankAccountNo: $scope.AccountNo,
                Amount: $scope.Amount,
                ShipmentDate: shipmentDate,
                ExpireDate: expireDate,  
                Remarks: $scope.Remarks,
                Description: $scope.Description,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,

                IsDeleted: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID

            };

            var fileList = [];
            angular.forEach($scope.files, function (item) {
                this.push(item.name);
            }, fileList);

            //if (fileList.length == 0) {
            //    $("#save").prop("disabled", false);
            //    Command: toastr["warning"]("Please attach PO document.");
            //    return;
            //}

            var ListTerms = $filter('filter')($scope.ListTerms, { Selected: 'true' });
            var menuID = $scope.UserCommonEntity.currentMenuID;
            var itemMasterDetail = $scope.ListPODetails;
            var chkAmount = 1;
            angular.forEach($scope.ListPODetails, function (item) {

                if (item.Amount <= 0) {
                    chkAmount = 0;
                }
            });

            if ($scope.ListPODetails.length > 0) {

                if (chkAmount == 1) {
                    var apiRoute = baseUrl + 'SaveUpdatePOMasterNdetails/';
                    //  var apiRoute = baseUrl + 'GetGrrMasterList/';

                    var cmnParam = "[" + JSON.stringify(itemMaster) + "," + JSON.stringify(itemMasterDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(fileList) + "," + JSON.stringify(ListTerms) + "]";

                    var POItemMasterNdetailsCreateUpdate = pOService.GetList(apiRoute, cmnParam);

                    // var ChallanItemMasterNdetailsCreateUpdate = pOService.postMasterDetail(apiRoute, itemMaster, itemMasterDetail, menuID, fileList);
                    POItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {

                            $scope.clear();

                            $scope.HPONo = response.data;

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
            else if ($scope.ListPODetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("PO Detail Must Not Empty!!!!");
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
            var id = dataModel.POID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id;
            var ListFileDetails = pOService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
         
        $scope.clear = function () {
              
            var date = new Date();           
            $scope.PODate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear(); 
            $scope.LCVoucherDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear(); 
            $scope.ShipmentDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.ExpireDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
              
            $scope.POID = "0";
            $scope.HPONo = "";

      
             
            $scope.listParty = [];
            $scope.lstPartyList = "";
            $('#ddlParty').select2("data", { id: "", text: "--Select Party--" });

            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '--Select SPR No--' });

            $scope.LCBankVoucherNo = ''; 
            $scope.AccountNo = '';
            $scope.Amount = ''; 
            $scope.Description = '';
            $scope.Remarks = ''; 
            $scope.ListPODetails = [];

            $scope.listStatementNo = [];
            $scope.StatementNo = '';
            $("#ddlStatementNo").select2("data", { id: '', text: '--Select Statement No--' });

            $scope.listOrderType = [];
            $scope.OrderType = '';
            $("#ddlOrderType").select2("data", { id: '', text: '--Select Order Type--' });

            $scope.listMoneyTrnsType = [];
            $scope.MoneyTrnsTypes = '';
            $("#ddlMoneyTrnsType").select2("data", { id: '', text: '--Select Money Trns. Type--' });
            
         
            LoadStatementNo();
            loadOrderType(0);
            loadMoneyTransType(0);

            $scope.LoadBankAdvisingByCompanyID();
             
            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $('#ddlCurrency').select2("data", { id: '', text: '--Select Currency--' });
             
            $scope.Remarks = "";
            $scope.Description = "";
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
        };

    }]);


//function modal_fadeOut() {
//    $("#ChallanMasterModal").fadeOut(200, function () {
//        $('#ChallanMasterModal').modal('hide');
//    });
//}

