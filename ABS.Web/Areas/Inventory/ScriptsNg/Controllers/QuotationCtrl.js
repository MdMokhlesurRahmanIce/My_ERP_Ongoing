/**
 * QuotationCtrl.js
 */


app.controller('quotationCtrl', ['$scope', 'quotationService', 'conversion', '$filter', 'uiGridConstants', '$localStorage',
    function ($scope, quotationService, conversion, $filter, uiGridConstants, $localStorage) {

        $scope.gridOptionsQuotationMaster = [];
        var objcmnParam = {};

        var baseUrl = '/Inventory/api/Quotation/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.IsListIcon = true; 
        $scope.IsCreateIcon = false;
        $scope.Invalid = true;
        var date = new Date();
        $scope.QuotationDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.DeliveryDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.tTypeID = 5;
        $scope.MenuID = 26;
        $scope.PIID = "0";

        $scope.btnQuotationSaveText = "Save";
        $scope.btnQuotationShowText = "Show List";
        $scope.btnQuotationReviseText = "Revise";
        $scope.PageTitle = 'Quotation Creation';
        $scope.ListTitle = 'Quotation Records';
        $scope.ListTitleQuotationMasters = 'Quotation Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleQuotationDeatails = 'Listed Item of Quotation';
        var UserTypeID = 3;
        $scope.ListPIDetails = [];
        $scope.ListActivePIMaster = [];
        $scope.Negotiation = 15;
        $scope.Discount = 0;
        $scope.listSalesPerson = [];
        $scope.showDtgrid = 0;
        $scope.listBuyer = [];
        $scope.lstBuyerList = '';
        //*************---Show and Hide Order---**********//

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;      
        $scope.IsShowD = false;
        $scope.ShowHide = function () {

            $scope.IsShow = $scope.IsShow ? false : true;
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnQuotationShowText = "Show List";
                $scope.IsShow = true;             
                $scope.IsShowD = $scope.ListReqItems.length > 0 ? true : false;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnQuotationShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsShowD = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                loadQuotationMasterRecords(0);
            }
        }

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

        var LoggedUser = $scope.UserCommonEntity.loggedUserID;
        var LoggedCompanyID = $scope.UserCommonEntity.loggedCompnyID;
        var DepartmentId = $scope.UserCommonEntity.loggedUserDepartmentID;

        //******=========Multiple Checkbox=========******
        $scope.toggleSelection = function toggleSelection() {
            $scope.selectedAll = false;
        };

        //******=========Single Checkbox=========******
        $scope.checkAll = function () {
            if ($scope.selectedAll) {
                $scope.selectedAll = true;
            } else {
                $scope.selectedAll = false;
            }
            angular.forEach($scope.ListReqItems, function (dataModel) {
                dataModel.Selected = $scope.selectedAll;               
            });

            //$scope.calculation(dataModel);
        };

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

            var apiRoute = '/Purchase/api/PurchaseOrder/GetOrderType/';
            var ComboType = "OrderType";

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ComboType) + "]";
            $scope.listOrderType = [];
            var listOrderType = quotationService.GetFrList(apiRoute, cmnParam);
            listOrderType.then(function (response) {
                $scope.listOrderType = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadOrderType(0);

        $scope.save = function () {
            if ($('#ddlSupplier').val() == "") {
                Command: toastr["warning"]("Please select Supplier!!!!");
                $('#ddlSupplier').focus()
                return;
            }            
            if ($('#QuotationType').val() == "") {
                Command: toastr["warning"]("Please select Quotation Type!!!!");
                $('#QuotationType').focus()
                return;
            }
            if ($('#ddlCurrency').val() == "") {
                Command: toastr["warning"]("Please select Currency!!!!");
                $('#ddlCurrency').focus()
                return;
            }
            var QuotationDate = conversion.getDateToString($scope.QuotationDate);
            var DeliveryDate = conversion.getDateToString($scope.DeliveryDate);
           
            var QuotationMaster = {
                QuotationDate: QuotationDate,
                RequisitionID: $scope.SPR,
                QuotationTypeID: $scope.QuotType,
                DeliveryDate: DeliveryDate,
                PartyID: $scope.Supplier,
                CurrencyID:$scope.CurrencyID,
                CompanyID: LoggedCompanyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                IsConfirm: true,
                CreateBy: LoggedUser,
                CreateOn: new Date(),
                IsDeleted: false
            };
            var QuotationDetail = [];
            QuotationDetail = $filter('filter')($scope.ListReqItems, { Selected: 'true' });
            if (!QuotationDetail.length > 0)
            {
                Command: toastr["warning"]("Please select Item!!!!");
                return;
            }
            var menuID = $localStorage.currentMenuID;
            var apiRoute = baseUrl + 'SaveQuotationMasterDetails/';
            var QuotationMasterDetailsCreate = quotationService.postMasterDetail(apiRoute, QuotationMaster, QuotationDetail, menuID, $scope.HeaderToken.get);
            QuotationMasterDetailsCreate.then(function (response) {
                if (response.data != "") {
                    $scope.CustomQuotationNo = response.data;
                    Command: toastr["success"]("Save  Successfully!!!!");
                }
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        $scope.clear = function () {
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsHiddenDetail = true;
            $scope.btnQuotationShowText = "Show List";
            $scope.ListReqItems = [];
            $scope.Purpose = '';
            $scope.bool = true;
            $scope.Invalid = true;
            $scope.selectedAll = false;
            angular.forEach($scope.ListReqItems, function (dataModel) {
                dataModel.Selected = $scope.selectedAll;
            });
            $("#ddlSPR").select2("data", { id: 0, text: '--Select SPR No--' });
            $("#ddlSupplier").select2("data", { id: 0, text: '--Select Supplier--' });
            $("#ddlCurrency").select2("data", { id: 0, text: '--Select Currency--' });
            var date = new Date();
            $scope.QuotationDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        };


        $scope.GetAmount = function (dataModel) {
            $scope.ListReqItems1 = [];
            angular.forEach($scope.ListReqItems, function (item) {
                var Amount = parseFloat(item.Qty) * parseFloat(item.UnitPrice);
                $scope.ListReqItems1.push({
                    ItemID: item.ItemID, UnitID: item.UnitID, ItemName: item.ItemName, UnitName: item.UnitName, Qty: item.Qty, UnitPrice: item.UnitPrice, Amount: Amount, FreightCharge: dataModel.FreightCharge, FOBValue: dataModel.FOBValue
                    , TransportTypeID: dataModel.TransportType, LoadingLocationID: dataModel.LocationId, DischargeLocationID: dataModel.DischargeLocationId
                });
                $scope.ListReqItems = $scope.ListReqItems1;
            });
        }

        $scope.GetRequisitionItemList = function () {
            $scope.ListReqItems = [];
            var ItemList = "";
            var apiRoute = baseUrl + 'GetRequisitonDetailByRequisitionID/';
            var RequisitionID = $scope.SPR;
            var CompanyId = $localStorage.loggedCompnyID;
            $scope.Invalid = true;
            if (RequisitionID != "") {
                ItemList = quotationService.getItemDetailsByRequisitionID(apiRoute, RequisitionID, CompanyId, $scope.HeaderToken.get);
                ItemList.then(function (response) {
                    $scope.ListReqItems = response.data;                   
                    $scope.SPRDate = conversion.getDateToString(response.data[0].RequisitionDate);
                    $scope.Invalid = $scope.ListReqItems.length > 0 ? false : true;
                },
                function (error) {
                    console.log("Error: " + error);
                });
                $scope.IsShowD = true;
              
            }
        }


       
        $scope.getTranportType = function () {
            $scope.lstTranportType = [];
            $scope.lstTranportType = [
                                    { id: '1', value: 'CFR' }];
        }
        $scope.getTranportType(0);



        function loadLoadingLocation(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $localStorage.loggedUserID,
                loggedCompany: $localStorage.loggedCompnyID,
                menuId: $localStorage.currentMenuID,
                tTypeId: 25
            };

            var apiRoute = '/Inventory/api/GRR/GetLocation/';
            var listLocation = quotationService.GetLocation(apiRoute, objcmnParam);
            listLocation.then(function (response) {
                $scope.listLoadLocation = response.data.objLocation;
                $scope.listDischargeLocation = response.data.objLocation;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadLoadingLocation(0);


        //Pagination Quotation Master
        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                this.pageNumber = 1;
                if (this.ddlpageSize == "All") {
                    this.ddlpageSize = $scope.pagination.totalItems;
                }
                else {
                    this.pageSize = this.ddlpageSize;
                }

                loadIssueMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadQuotationMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadQuotationMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadQuotationMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadQuotationMasterRecords(1);
                }
            }
        };

        function loadQuotationMasterRecords(isPaging) {

            $scope.loaderMoreIssueMaster = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";


            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $localStorage.loggedUserID,
                loggedCompany: $localStorage.loggedCompnyID,
                menuId: $localStorage.currentMenuID,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsQuotationMaster = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,               
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                columnDefs: [
                    { name: "QuotationID", displayName: "QuotationID", visible: false, title: "QuotationID", width: '5%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QuotationNo", displayName: "Quotation No", title: "Quotation No", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QuotationDate", displayName: "Quotation Date", cellFilter: 'date:"dd-MM-yyyy"', width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PartyName", displayName: "Party Name", title: "Party Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Currency", displayName: "Currency", title: "Currency", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    
                    {
                        name: 'Action',
                        displayName: "Action",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '10%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                   '<a href="" title="Edit" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                     '<i class="icon-edit"></i> Edit' +
                                   '</a>' +
                                   '</span>'

                        //cellTemplate: '<span class="label label-warning label-mini">' +
                        //                     '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                        //                         '<i class="icon-pencil" ng-click="grid.appScope.GetReqDetailByRequisitionID(row.entity)"></i>' +
                        //                     '</a>' +
                        //                 '</span>'
                        //+'<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //  '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.DeleteUpdateMasterDetail(row.entity)">' +
                        //    '<i class="glyphicon glyphicon-trash" aria-hidden="true"></i> Delete' +
                        //  '</a>' +
                        //  '</span>'
                    }
                ],
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Quotation.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Quotation", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetQuotationMaster/';
            var listQuotationMaster = quotationService.getQuotationMasterList(apiRoute, objcmnParam);
            listQuotationMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsQuotationMaster.data = response.data.objQuotationMaster;
                //  $scope.loaderMoreIssueMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadQuotationMasterRecords(0);


        function loadSPRNo() {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.MenuID,
                tTypeId: $scope.tTypeID
            };

            //var apiRoute = baseUrl + 'GetSPRNo/';

            var apiRoute = baseUrl + 'GetSPR/';
            var listSprNo = quotationService.GetloadSPRNo(apiRoute, objcmnParam);
            listSprNo.then(function (response) {
                $scope.listSprNo = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSPRNo();

        $scope.LoadItemBySPRNoChange = function () {
            $scope.IsHiddenDetail = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.MenuID,
                tTypeId: $scope.tTypeID
            };
        }
        //**********----Get Company Record and filter by LoginCompanyID and cascading with Advising bank and branch record ----***************//
        var defaultCompanyID = "";
        function loadCompanyRecords(isPaging) {
            var apiRoute = baseUrl + 'GetCompany/';
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: $scope.MenuID,
                tTypeId: $scope.tTypeID
            };

            var listCompany = quotationService.getUserWiseCompany(apiRoute, objcmnParam);

            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    if (item.CompanyID == LoggedCompanyID) {
                        //  defaultCompanyID = item.CompanyID;
                        $scope.lstCompanyList = item.CompanyID;
                        $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
                        //  $scope.LoadDeptByCompanyID();
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCompanyRecords(0);

        $scope.GetCurrency = function () {

            var apiRoute = baseUrl + '/GetCurrency/';
            var Currencys = quotationService.GetList(apiRoute, page, pageSize, isPaging);
            Currencys.then(function (response) {
                $scope.Currency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetCurrency(0);

        $scope.UserLst = [];
        function GetAllUsers() {
            var Route = baseUrl + '/GetAllUsers/';
            var UserLst = quotationService.getAllUsers(Route, page, pageSize, isPaging, UserTypeID, LoggedCompanyID);
            UserLst.then(function (response) {
                $scope.UserLst = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllUsers(0);
    }]);
