app.controller('bookingCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************      

        $scope.openedSD = [];
        $scope.open = function (index) {
            $scope.openedSD[index] = true;
        };

        //$scope.today = function () {
        //    $scope.DeliveryStartDate = new Date();
        //};
        //$scope.today();

        //$scope.dateOptions = {
        //    'year-format': "'yy'",
        //    'starting-day': 1
        //};

        //$scope.today = function () {
        //    $scope.DeliveryDate = new Date();
        //};
        //$scope.today();


        //$scope.clear = function () {
        //    $scope.DeliveryDate = null;
        //};      

        //$scope.toggleMin = function () {
        //    $scope.minDate = ($scope.minDate) ? null : new Date();
        //};
        //$scope.toggleMin();



        /////////////////////////////////////////////////////////////////////////////////////////////////////

        var baseUrl = '/Sales/api/Booking/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsActivePI = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;
        $scope.IsDOCompleted = "";
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        $scope.BookingDate = conversion.NowDateCustom();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        $scope.BookingID = "0";

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.btnPIReviseText = "Update";
        $scope.PageTitle = 'Booking Creation';
        $scope.ListTitle = 'Booking Records';
        $scope.ListTitleActivePIMasters = 'Booking Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitlePIDeatails = 'Listed Item of Booking';

        $scope.ListBookingDetails = [];
        $scope.ListActivePIMaster = [];
        $scope.Discount = 0;
        $scope.listBuyerReference = [];
        $scope.showDtgrid = 0;
        $scope.listBuyer = [];
        $scope.lstBuyerList = '';
        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        //****************************************************End Common Task for all***************************************************

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            //$scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnPIShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsHiddenDetail = $scope.ListBookingDetails.length > 0 ? false : true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
                $scope.clear(); // When at list page entry data should be cleared.
                $scope.IsbtSaveReviseShow = $scope.IsLCCompleted == true ? false : true;
            }
            else {
                $scope.loadActivePIRecords(0);
                $scope.IsbtSaveReviseShow = $scope.IsLCCompleted == true ? false : true;

                $scope.btnPIShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHiddenDetail = true;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;

                $scope.btnPISaveText = "Save";

                $scope.BookingID = '0';
                $scope.showDtgrid = 0;//$scope.ListBookingDetails.length;

                $scope.listBookingMaster = [];
                $scope.ListBookingDetails = [];
                $scope.listBuyer = [];
                $scope.OverdueInterest = '';
                $scope.Remarks = '';
                $scope.Discount = 0;
                $scope.LoadItemGroupByCompanyID();
                loadBuyerRecords(0);

                $scope.lstBuyerList = '';
                $scope.lstSampleNoList = '';

                $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

                $("#ddlItemGroup").select2("data", { id: '', text: '--Select Sample/Article No--' });

                $scope.BookingDate = conversion.NowDateCustom();
            }
        }

        //**********---- Get All Buyer Records ----*************** //
        function loadBuyerRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPIBuyer/';
            var listBuyer = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listBuyer.then(function (response) {
                $scope.listBuyer = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBuyerRecords(0);

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
            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            // else {
            // For Loading modal
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

            $scope.gridOptionslistItemMaster.enableFiltering = true;
            $scope.gridOptionslistItemMasterenableGridMenu = true;

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
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.lstCompanyList
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction", headerCellClass: $scope.highlightFilteredHeader },//width: '19%',
                    { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeight", displayName: "Weight", title: "Weight", cellFilter: 'number: 2', width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWidth", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.EnableUpdate,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Add' +
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
            var groupID = $scope.lstSampleNoList;
            debugger
            // if (groupID > 0) {
            if (groupID == null || groupID == "" || groupID == undefined) {
                groupID = 0;
            }
            var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
            var listItemMaster = crudService.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);
            listItemMaster.then(function (response) {
                //$scope.listItemMaster = response.data;
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objPIItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
            //  }
            //else if (groupID == 0 || groupID == "") {
            //        Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            //}
        };

        //**********----Get Company Record and filter by LoginCompanyID and cascading with Advising bank and branch record ----***************//
        var defaultCompanyID = "";

        function loadCompanyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetPICompany/';

            var listCompany = crudService.getUserWiseCompany(apiRoute, $scope.UserCommonEntity.loggedUserID, $scope.HeaderToken.get);
            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    debugger
                    if (item.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
                        defaultCompanyID = item.CompanyID;
                        $scope.lstCompanyList = item.CompanyID;
                        $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
                        return false;
                    }
                });

                $scope.LoadItemGroupByCompanyID();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCompanyRecords(0);

        $scope.LoadItemGroupByCompanyID = function () {

            $scope.listSampleNo = [];
            $scope.lstSampleNoList = '';
            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Item Group--' });


            $scope.loaderMoreForSampleNo = true;
            $scope.lblMessageForSampleNo = '';
            $scope.result = "color-red";
            debugger
            var apiRoute = baseUrl + 'GetPISampleNo/';
            var companyID = $scope.lstCompanyList;
            var listSampleNo = crudService.getModelSampleNo(apiRoute, companyID, page, pageSize, isPaging, $scope.HeaderToken.get);
            listSampleNo.then(function (response) {
                $scope.listSampleNo = response.data;

                $scope.loaderMoreForSampleNo = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********---- Get All Sample No Records ----*************** //
        // $scope.loadSampleNoRecords = function(isPaging) {

        // loadCompanyRecords(0); //loading company records first.


        // }
        //loadSampleNoRecords(0);

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
                $scope.loadActivePIRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadActivePIRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadActivePIRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadActivePIRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadActivePIRecords(1);
                }
            }
        };

        //**********----Get All Active PI Records----***************
        $scope.loadActivePIRecords = function (isPaging) {

            $scope.gridOptionsActivePI.enableFiltering = true;
            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
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

            $scope.gridOptionsActivePI = {
                columnDefs: [
                    { name: "BookingID", displayName: "BookingID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BookingNo", displayName: "Booking No", title: "Booking No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BookingDate", displayName: "Booking Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerFullName", displayName: "Buyer Name", title: "Buyer Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerReferenceFullName", displayName: "Buyer Reference", title: "Buyer Reference", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Select',
                        displayName: "Select",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '6%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.loadPIMasterDetailsByActivePI(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Select' +
                                      '</a>' +
                                      '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'ActivePIMaster.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "ActivePIMaster", style: 'headerStyle' },
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


            var apiRoute = baseUrl + 'GetBookingMaster/';
            var listActivePIMaster = crudService.getPIMasterListByPIActive(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listActivePIMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsActivePI.data = response.data.objVmPIMaster;
                $scope.loaderMoreActivePIMaster = false;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //**********---- Get Sales Person Records and filter by LoginUserID ----*************** //

        function loadBuyerReferenceRecords(isPaging) {
            $scope.listBuyerReference = [];
            var apiRoute = baseUrl + 'GetBuyerReference/';
            var listBuyerReference = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            listBuyerReference.then(function (response) {
                $scope.listBuyerReference = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBuyerReferenceRecords(0);

        $scope.getListItemMaster = function (dataModel) {

            $scope.IsHiddenDetail = false;
            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListBookingDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListBookingDetails.push({
                    BookingID: 0, PIDetailID: 0, ItemID: dataModel.ItemID, ArticleNo: dataModel.ArticleNo, ItemName: dataModel.ItemName,
                    CompanyID: dataModel.CompanyID, CuttableWidth: dataModel.CuttableWidth, Description: dataModel.Description,
                    Quantity: 0.00, CreateBy: $scope.UserCommonEntity.loggedUserID, IsActive: true//,
                    //DeliveryStartDate: conversion.NowDateCustomForBooking(), DeliveryFinishDate: conversion.NowDateCustomForBooking()
                }
                    
                );
                Command: toastr["info"]("Item Successfully Added.");
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
            $scope.showDtgrid = $scope.ListBookingDetails.length;
        }

        $scope.loadPIMasterDetailsByActivePI = function (dataModel) {
            modal_fadeOut();

            $scope.IsbtSaveReviseShow = dataModel.IsLCCompleted == false ? true : false;

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnPIShowText = "Show List";
            $scope.IsHidden = true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            //

            $scope.btnPISaveText = "Update";
            $scope.listBookingMaster = [];
            var BookingID = dataModel.BookingID;
            $scope.BookingID = dataModel.BookingID;
            $scope.TransactionTypeID = dataModel.TransactionTypeID;
            $scope.BookingNo = dataModel.BookingNo;
            $scope.Description = dataModel.Description;
            $scope.BookingDate = conversion.getDateToString(dataModel.BookingDate);

            $scope.lstCompanyList = dataModel.CompanyID;
            $("#ddlCompany").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });

            $scope.lstBuyerList = dataModel.BuyerID;
            $("#ddlBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerFullName });

            $scope.lstBuyerReferenceList = dataModel.BuyerRefID;
            $("#ddlBuyerReference").select2("data", { id: dataModel.BuyerRefID, text: dataModel.BuyerReferenceFullName });

            $scope.bool = false;

            var apiRoute = baseUrl + 'GetBookingDetail/';
            var ListBookingDetails = crudService.getPIDetailsByActivePIID(apiRoute, BookingID, $scope.HeaderToken.get);
            ListBookingDetails.then(function (response) {
                debugger
                angular.forEach(response.data, function (item) {
                    item.DeliveryStartDate = conversion.getDateToString(item.DeliveryStartDate);
                    item.DeliveryFinishDate = conversion.getDateToString(item.DeliveryFinishDate);
                })

                $scope.ListBookingDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.deleteRow = function (index) {
            $scope.ListBookingDetails.splice(index, 1);
            $scope.showDtgrid = $scope.ListBookingDetails.length;
        };

        $scope.save = function () {
            $("#save").prop("disabled", true);
            var itemMaster = {
                BookingID: $scope.BookingID,
                BookingNo: $scope.BookingNo,
                Description: $scope.Description,
                BookingDate: conversion.getStringToDate($scope.BookingDate),
                BuyerID: $scope.lstBuyerList,
                BuyerRefID: $scope.lstBuyerReferenceList,
                CompanyID: $scope.lstCompanyList,
                Remarks: $scope.Remarks,
                IsActive: true,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID
            };
            var menuID = $scope.UserCommonEntity.currentMenuID;

            var chkQuantity = 1;
            angular.forEach($scope.ListBookingDetails, function (item) {
                debugger
                //item.DeliveryDate = conversion.getStringToDate(item.DeliveryDate);
                // var chkDate = 
                if (item.DeliveryStartDate.toString().length > 10) {
                    item.DeliveryStartDate = conversion.fullDateToFormattedDate(item.DeliveryStartDate);
                }

                else if (item.DeliveryStartDate.toString().length == 10) {

                    item.DeliveryStartDate = conversion.getStringToDate(item.DeliveryStartDate);
                }

                if (item.DeliveryFinishDate.toString().length > 10) {
                    item.DeliveryFinishDate = conversion.fullDateToFormattedDate(item.DeliveryFinishDate);
                }

                else if (item.DeliveryFinishDate.toString().length == 10) {

                    item.DeliveryFinishDate = conversion.getStringToDate(item.DeliveryFinishDate);
                }


                if (item.Quantity <= 0) {
                    chkQuantity = 0;
                }
            });

            if ($scope.ListBookingDetails.length > 0) {

                if (chkQuantity == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateBookingItemMasterNdetails/';
                    var PIItemMasterNdetailsCreateUpdate = crudService.postMasterDetail(apiRoute, itemMaster, $scope.ListBookingDetails,
                                                           menuID, $scope.HeaderToken.post);
                    PIItemMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {
                            $scope.BookingNo = response.data;
                            Command: toastr["success"]("Save  Successfully!!!!");
                            $scope.clear();
                        }
                        else if (response.data == "") {
                                Command: toastr["warning"]("Save Not Successful!!!!");
                            $("#save").prop("disabled", false);
                        }
                    },
                    function (error) {
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Save Not Successful!!!!");
                    });
                }
                else if (chkQuantity == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("Quantity Must Not Zero Or Empty !!!!");
                }
            }
            else if ($scope.ListBookingDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Booking Detail Must Not Empty!!!!");
            }
        };

        //**********----Reset Record----***************//
        $scope.clear = function () {

            $scope.IsbtSaveReviseShow = $scope.IsLCCompleted == true ? false : true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.BookingID = '0';
            $scope.showDtgrid = 0;//$scope.ListBookingDetails.length;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnPIShowText = "Show List";
            $scope.btnPISaveText = "Save";
            $scope.listBookingMaster = [];
            $scope.ListBookingDetails = [];
            $scope.bool = true;
            $scope.listBuyer = [];

            $scope.Description = '';
            $scope.loadActivePIRecords(0);

            $scope.LoadItemGroupByCompanyID();
            loadBuyerRecords(0);
            loadCompanyRecords(0);
            loadBuyerReferenceRecords(0);

            $scope.lstBuyerList = '';
            $scope.lstSampleNoList = '';
            $scope.lstBuyerReferenceList = '';

            $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

            $("#ddlBuyerReference").select2("data", { id: '', text: '--Select Buyer Reference--' });

            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Sample/Article No--' });

            $scope.BookingDate = conversion.NowDateCustom();
            // $scope.DeliveryDate = conversion.NowDateCustom();
        };
    }



]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}

//app.directive("datepicker", function () {
//    
//    return {
//        restrict: "A",
//        require: "ngModel",
//        link: function (scope, elem, attrs, ngModel) {
//            var updateModel = function (dateText) {
//                scope.$apply(function () {
//                    ngModel.$setViewValue(dateText);
//                });
//            };
//            var options = {
//                format: "dd/mm/yyyy",
//                onSelect: function (dateText) {
//                    updateModel(dateText);
//                }                
//            };
//            elem.datepicker(options);
//        }
//    }
//});






//app.controller('bookingCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants', 
//    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
//        //**************************************************Start Vairiable Initialize**************************************************
//        var baseUrl = '/Sales/api/Booking/';
//        $scope.permissionPageVisibility = true;
//        $scope.UserCommonEntity = {};
//        $scope.HeaderToken = {};
//        objcmnParam = {};
//        $scope.gridOptionsActivePI = [];
//        $scope.gridOptionslistItemMaster = [];

//        $scope.IsCreateIcon = false;
//        $scope.IsListIcon = true;
//        $scope.IsbtSaveReviseShow = true;
//        $scope.IsDOCompleted = "";
//        var isExisting = 0;
//        var page = 0;
//        var pageSize = 100;
//        var isPaging = 0;
//        var totalData = 0;

//        $scope.BookingDate = conversion.NowDateCustom();

//        $scope.FullFormateDate = [];
//        $scope.ListCompany = [];
//        $scope.bool = true;
//        $scope.BookingID = "0";

//        $scope.btnPISaveText = "Save";
//        $scope.btnPIShowText = "Show List";
//        $scope.btnPIReviseText = "Update";
//        $scope.PageTitle = 'Booking Creation';
//        $scope.ListTitle = 'Booking Records';
//        $scope.ListTitleActivePIMasters = 'Booking Information';
//        $scope.ListTitleSampleNo = 'Sample Info';
//        $scope.ListTitlePIDeatails = 'Listed Item of Booking';

//        $scope.ListBookingDetails = [];
//        $scope.ListActivePIMaster = [];
//        $scope.Discount = 0;
//        $scope.listBuyerReference = [];
//        $scope.showDtgrid = 0;
//        $scope.listBuyer = [];
//        $scope.lstBuyerList = '';
//        //*************---Show and Hide Order---**********//
//        $scope.IsHidden = true;
//        $scope.IsShow = true;
//        $scope.IsHiddenDetail = true;
//        //***************************************************End Vairiable Initialize***************************************************

//        //***************************************************Start Common Task for all**************************************************
//        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
//        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
//        //****************************************************End Common Task for all***************************************************


//        $scope.ShowHide = function () {
//            $scope.IsHidden = $scope.IsHidden == true ? false : true;
//            //$scope.IsHiddenDetail = true;
//            if ($scope.IsHidden == true) {
//                $scope.btnPIShowText = "Show List";
//                $scope.IsShow = true;
//                $scope.IsHiddenDetail = $scope.ListBookingDetails.length > 0 ? false : true;
//                $scope.IsCreateIcon = false;
//                $scope.IsListIcon = true;
//                $scope.clear(); // When at list page entry data should be cleared.
//            }
//            else {
//                $scope.loadActivePIRecords(0);
//                $scope.btnPIShowText = "Create";                
//                $scope.IsShow = false;
//                $scope.IsHiddenDetail = true;
//                $scope.IsHidden = false;
//                $scope.IsCreateIcon = true;
//                $scope.IsListIcon = false;

//                $scope.btnPISaveText = "Save";

//                $scope.BookingID = '0';
//                $scope.showDtgrid = 0;//$scope.ListBookingDetails.length;

//                $scope.listBookingMaster = [];
//                $scope.ListBookingDetails = [];
//                $scope.listBuyer = [];
//                $scope.OverdueInterest = '';
//                $scope.Remarks = '';
//                $scope.Discount = 0;
//                loadSampleNoRecords(0);
//                loadBuyerRecords(0);

//                $scope.lstBuyerList = '';
//                $scope.lstSampleNoList = '';

//                $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

//                $("#ddlItemGroup").select2("data", { id: '', text: '--Select Sample/Article No--' });

//                $scope.BookingDate = conversion.NowDateCustom();
//            }
//        }

//        //**********---- Get All Sample No Records ----*************** //
//        function loadSampleNoRecords(isPaging) {
//            $scope.loaderMoreForSampleNo = true;
//            $scope.lblMessageForSampleNo = '';
//            $scope.result = "color-red";

//            var apiRoute = baseUrl + 'GetPISampleNo/';
//            var listSampleNo = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
//            listSampleNo.then(function (response) {
//                $scope.listSampleNo = response.data;

//                $scope.loaderMoreForSampleNo = false;
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        }
//        loadSampleNoRecords(0);

//        //**********---- Get All Buyer Records ----*************** //
//        function loadBuyerRecords(isPaging) {

//            var apiRoute = baseUrl + 'GetPIBuyer/';
//            var listBuyer = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
//            listBuyer.then(function (response) {
//                $scope.listBuyer = response.data;
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        }
//        loadBuyerRecords(0);

//        //**********----Pagination Item Master List popup----***************
//        $scope.paginationItemMaster = {
//            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
//            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

//            getTotalPagesItemMaster: function () {
//                return Math.ceil(this.totalItems / this.pageSize);
//            },
//            pageSizeChange: function () {
//                if (this.ddlpageSize == "All")
//                    this.pageSize = $scope.paginationItemMaster.totalItems;
//                else
//                    this.pageSize = this.ddlpageSize

//                this.pageNumber = 1
//                $scope.loadSampleNoModalRecords(1);
//            },
//            firstPage: function () {
//                if (this.pageNumber > 1) {
//                    this.pageNumber = 1
//                    $scope.loadSampleNoModalRecords(1);
//                }
//            },
//            nextPage: function () {
//                if (this.pageNumber < this.getTotalPagesItemMaster()) {
//                    this.pageNumber++;
//                    $scope.loadSampleNoModalRecords(1);
//                }
//            },
//            previousPage: function () {
//                if (this.pageNumber > 1) {
//                    this.pageNumber--;
//                    $scope.loadSampleNoModalRecords(1);
//                }
//            },
//            lastPage: function () {
//                if (this.pageNumber >= 1) {
//                    this.pageNumber = this.getTotalPagesItemMaster();
//                    $scope.loadSampleNoModalRecords(1);
//                }
//            }
//        };

//        //**********----Get All Item Record by  select Sample No----***************//
//        $scope.loadSampleNoModalRecords = function (isPaging) {
//            if ($scope.lstSampleNoList == undefined) {
//                Command: toastr["warning"]("Select Sample/Article No !!!!");
//            }
//            else {
//                // For Loading modal
//                $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

//                $scope.gridOptionslistItemMaster.enableFiltering = true;
//                $scope.gridOptionslistItemMasterenableGridMenu = true;

//                // For Loading
//                if (isPaging == 0)
//                    $scope.paginationItemMaster.pageNumber = 1;
//                // For Loading
//                $scope.loaderMoreItemMaster = true;
//                $scope.lblMessageItemMaster = 'loading please wait....!';
//                $scope.result = "color-red";

//                //Ui Grid
//                objcmnParam = {
//                    pageNumber: (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize),
//                    pageSize: $scope.paginationItemMaster.pageSize,
//                    IsPaging: isPaging,
//                    loggeduser: $scope.UserCommonEntity.loggedUserID,
//                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
//                    menuId: $scope.UserCommonEntity.currentMenuID,
//                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
//                };

//                $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
//                    if (col.filters[0].term) {
//                        return 'header-filtered';
//                    } else {
//                        return '';
//                    }
//                };

//                $scope.gridOptionslistItemMaster = {
//                    columnDefs: [
//                        { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "WeightPerUnit", displayName: "Weight", title: "Weight", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
//                        { name: "Width", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
//                        {
//                            name: 'Action',
//                            displayName: "Action",
//                            width: '6%',
//                            enableColumnResizing: false,
//                            enableFiltering: false,
//                            enableSorting: false,
//                            headerCellClass: $scope.highlightFilteredHeader,
//                            visible: $scope.UserCommonEntity.EnableUpdate,
//                            cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
//                                          '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
//                                            '<i class="icon-check" aria-hidden="true"></i> Add' +
//                                          '</a>' +
//                                          '</span>'
//                        }
//                    ],
//                    onRegisterApi: function (gridApi) {
//                        $scope.gridApi = gridApi;
//                    },
//                    enableFiltering: true,
//                    enableGridMenu: true,
//                    enableSelectAll: true,
//                    exporterCsvFilename: 'ItemSample.csv',
//                    exporterPdfDefaultStyle: { fontSize: 9 },
//                    exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
//                    exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
//                    exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
//                    exporterPdfFooter: function (currentPage, pageCount) {
//                        return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
//                    },
//                    exporterPdfCustomFormatter: function (docDefinition) {
//                        docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
//                        docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
//                        return docDefinition;
//                    },
//                    exporterPdfOrientation: 'portrait',
//                    exporterPdfPageSize: 'LETTER',
//                    exporterPdfMaxGridWidth: 500,
//                    exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
//                };

//                // $scope.listItemMaster = [];
//                var groupID = $scope.lstSampleNoList;
//                if (groupID > 0) {
//                    var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
//                    var listItemMaster = crudService.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);
//                    listItemMaster.then(function (response) {
//                        //$scope.listItemMaster = response.data;
//                        $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
//                        $scope.gridOptionslistItemMaster.data = response.data.objPIItemMaster;
//                        $scope.loaderMoreItemMaster = false;
//                    },
//                    function (error) {
//                        console.log("Error: " + error);
//                    });
//                }
//                else if (groupID == 0 || groupID == "") {
//                        Command: toastr["warning"]("Select Sample/Article No !!!!");
//                }
//            }
//        };

//        //**********----Get Company Record and filter by LoginCompanyID and cascading with Advising bank and branch record ----***************//
//        var defaultCompanyID = "";

//        function loadCompanyRecords(isPaging) {

//            var apiRoute = baseUrl + 'GetPICompany/';

//            var listCompany = crudService.getUserWiseCompany(apiRoute, $scope.UserCommonEntity.loggedUserID, $scope.HeaderToken.get);
//            listCompany.then(function (response) {
//                $scope.listCompany = response.data;
//                angular.forEach($scope.listCompany, function (item) {
//                    if (item.CompanyID == $scope.UserCommonEntity.loggedCompnyID) {
//                        defaultCompanyID = item.CompanyID;
//                        $scope.lstCompanyList = item.CompanyID;
//                        $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
//                        return false;
//                    }
//                });
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        }
//        loadCompanyRecords(0);

//        //**********----Pagination----***************
//        $scope.pagination = {
//            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
//            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

//            getTotalPages: function () {
//                return Math.ceil(this.totalItems / this.pageSize);
//            },
//            pageSizeChange: function () {
//                if (this.ddlpageSize == "All")
//                    this.pageSize = $scope.pagination.totalItems;
//                else
//                    this.pageSize = this.ddlpageSize

//                this.pageNumber = 1
//                $scope.loadActivePIRecords(1);
//            },
//            firstPage: function () {
//                if (this.pageNumber > 1) {
//                    this.pageNumber = 1
//                    $scope.loadActivePIRecords(1);
//                }
//            },
//            nextPage: function () {
//                if (this.pageNumber < this.getTotalPages()) {
//                    this.pageNumber++;
//                    $scope.loadActivePIRecords(1);
//                }
//            },
//            previousPage: function () {
//                if (this.pageNumber > 1) {
//                    this.pageNumber--;
//                    $scope.loadActivePIRecords(1);
//                }
//            },
//            lastPage: function () {
//                if (this.pageNumber >= 1) {
//                    this.pageNumber = this.getTotalPages();
//                    $scope.loadActivePIRecords(1);
//                }
//            }
//        };

//        //**********----Get All Active PI Records----***************
//        $scope.loadActivePIRecords = function (isPaging) {

//            $scope.gridOptionsActivePI.enableFiltering = true;
//            // For Loading
//            if (isPaging == 0)
//                $scope.pagination.pageNumber = 1;

//            // For Loading
//            $scope.loaderMore = true;
//            $scope.lblMessage = 'loading please wait....!';
//            $scope.result = "color-red";

//            //Ui Grid
//            objcmnParam = {
//                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
//                pageSize: $scope.pagination.pageSize,
//                IsPaging: isPaging,
//                loggeduser: $scope.UserCommonEntity.loggedUserID,
//                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
//                menuId: $scope.UserCommonEntity.currentMenuID,
//                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
//            };

//            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
//                if (col.filters[0].term) {
//                    return 'header-filtered';
//                } else {
//                    return '';
//                }
//            };

//            $scope.gridOptionsActivePI = {
//                columnDefs: [
//                    { name: "BookingID", displayName: "BookingID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
//                    { name: "BookingNo", displayName: "Booking No", title: "Booking No", headerCellClass: $scope.highlightFilteredHeader },
//                    { name: "BookingDate", displayName: "Booking Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
//                    { name: "BuyerFullName", displayName: "Buyer Name", title: "Buyer Name", headerCellClass: $scope.highlightFilteredHeader },
//                    { name: "BuyerReferenceFullName", displayName: "Buyer Reference", title: "Buyer Reference", headerCellClass: $scope.highlightFilteredHeader },
//                    {
//                        name: 'Select',
//                        displayName: "Select",
//                        enableColumnResizing: false,
//                        enableFiltering: false,
//                        enableSorting: false,
//                        width: '6%',
//                        headerCellClass: $scope.highlightFilteredHeader,
//                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
//                                      '<a href="" title="Select" ng-click="grid.appScope.loadPIMasterDetailsByActivePI(row.entity)">' +
//                                        '<i class="icon-check" aria-hidden="true"></i> Select' +
//                                      '</a>' +
//                                      '</span>'
//                    }
//                ],

//                enableFiltering: true,
//                enableGridMenu: true,
//                enableSelectAll: true,
//                exporterCsvFilename: 'ActivePIMaster.csv',
//                exporterPdfDefaultStyle: { fontSize: 9 },
//                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
//                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
//                exporterPdfHeader: { text: "ActivePIMaster", style: 'headerStyle' },
//                exporterPdfFooter: function (currentPage, pageCount) {
//                    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
//                },
//                exporterPdfCustomFormatter: function (docDefinition) {
//                    docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
//                    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
//                    return docDefinition;
//                },
//                exporterPdfOrientation: 'portrait',
//                exporterPdfPageSize: 'LETTER',
//                exporterPdfMaxGridWidth: 500,
//                exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
//            };


//            var apiRoute = baseUrl + 'GetPIMasterByPIActive/';
//            var listActivePIMaster = crudService.getPIMasterListByPIActive(apiRoute, objcmnParam, $scope.HeaderToken.get);
//            listActivePIMaster.then(function (response) {
//                $scope.pagination.totalItems = response.data.recordsTotal;
//                $scope.gridOptionsActivePI.data = response.data.objVmPIMaster;
//                $scope.loaderMoreActivePIMaster = false;
//                $scope.loaderMore = false;
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        };        

//        //**********---- Get Sales Person Records and filter by LoginUserID ----*************** //

//        function loadBuyerReferenceRecords(isPaging) {
//            $scope.listBuyerReference = [];
//            var apiRoute = baseUrl + 'GetBuyerReference/';
//            var listBuyerReference = crudService.getModel(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
//            listBuyerReference.then(function (response) {
//                $scope.listBuyerReference = response.data;             
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        }
//        loadBuyerReferenceRecords(0);

//        $scope.getListItemMaster = function (dataModel) {

//            $scope.IsHiddenDetail = false;
//            var existItem = dataModel.ItemID;
//            var duplicateItem = 0;
//            angular.forEach($scope.ListBookingDetails, function (item) {
//                if (existItem == item.ItemID) {
//                    duplicateItem = 1;
//                    return false;
//                }
//            });

//            if (duplicateItem === 0) {
//                $scope.ListBookingDetails.push({
//                    BookingID: 0, PIDetailID: 0, ItemID: dataModel.ItemID, ArticleNo: dataModel.ArticleNo, ItemName: dataModel.ItemName,
//                    CompanyID: dataModel.CompanyID, CuttableWidth: dataModel.CuttableWidth, Description: dataModel.Description,
//                    Quantity: 0.00, CreateBy: $scope.UserCommonEntity.loggedUserID, IsActive: true,
//                    DeliveryDate: conversion.NowDateDefault()
//                });
//            }
//            else if (duplicateItem === 1) {
//                    Command: toastr["warning"]("Item Already Exists!!!!");
//            }
//            $scope.showDtgrid = $scope.ListBookingDetails.length;
//        }

//        $scope.loadPIMasterDetailsByActivePI = function (dataModel) {
//            modal_fadeOut();

//            $scope.IsShow = true;
//            $scope.IsHiddenDetail = false;
//            //
//            $scope.btnPIShowText = "Show List";
//            $scope.IsHidden = true;

//            $scope.IsCreateIcon = false;
//            $scope.IsListIcon = true;
//            //

//            $scope.btnPISaveText = "Update";
//            $scope.listBookingMaster = [];
//            var activePI = dataModel.BookingID;
//            $scope.BookingID = dataModel.BookingID;
//            $scope.TransactionTypeID = dataModel.TransactionTypeID;
//            $scope.BookingNo = dataModel.BookingNo;
//            $scope.Description = dataModel.Description;
//            $scope.BookingDate = conversion.getDateToString(dataModel.BookingDate);

//            $scope.lstCompanyList = dataModel.CompanyID;
//            $("#ddlCompany").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });

//            $scope.lstBuyerList = dataModel.BuyerID;
//            $("#ddlBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerFullName });

//            $scope.lstBuyerReferenceList = dataModel.BuyerRefID;
//            $("#ddlBuyerReference").select2("data", { id: dataModel.BuyerRefID, text: dataModel.BuyerReferenceFullName });

//            $scope.bool = false;

//            var apiRoute = baseUrl + 'GetPIDetailsListByActivePI/';
//            var ListBookingDetails = crudService.getPIDetailsByActivePIID(apiRoute, activePI, $scope.HeaderToken.get);
//            ListBookingDetails.then(function (response) {
//                debugger
//                angular.forEach(response.data, function (item) {
//                    item.DeliveryDate = conversion.getDateTimeToTimeSpan(item.DeliveryDate);
//                })
//                $scope.ListBookingDetails = response.data;
//            },
//            function (error) {
//                console.log("Error: " + error);
//            });
//        }

//        $scope.deleteRow = function (index) {
//            $scope.ListBookingDetails.splice(index, 1);
//            $scope.showDtgrid = $scope.ListBookingDetails.length;
//        };

//        $scope.save = function () {
//            $("#save").prop("disabled", true);            
//            var itemMaster = {
//                BookingID: $scope.BookingID,
//                BookingNo: $scope.BookingNo,
//                Description: $scope.Description,
//                BookingDate: conversion.getStringToDate($scope.BookingDate),              
//                BuyerID: $scope.lstBuyerList,
//                BuyerRefID: $scope.lstBuyerReferenceList,
//                CompanyID: $scope.lstCompanyList,
//                Remarks: $scope.Remarks,
//                IsActive: true,
//                CreateBy: $scope.UserCommonEntity.loggedUserID,
//                TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID
//            };
//            var menuID = $scope.UserCommonEntity.currentMenuID;
//           // var itemMasterDetail = $scope.ListBookingDetails;
//            var chkQuantity = 1;
//            angular.forEach($scope.ListBookingDetails, function (item) {
//                item.DeliveryDate = conversion.getStringToDate(item.DeliveryDate);
//                if (item.Quantity <= 0) {
//                    chkQuantity = 0;
//                }
//            });

//            if ($scope.ListBookingDetails.length > 0) {

//                if (chkQuantity == 1) {
//                    var apiRoute = baseUrl + 'SaveUpdateBookingItemMasterNdetails/';
//                    var PIItemMasterNdetailsCreateUpdate = crudService.postMasterDetail(apiRoute, itemMaster, $scope.ListBookingDetails,
//                                                           menuID, $scope.HeaderToken.post);
//                    PIItemMasterNdetailsCreateUpdate.then(function (response) {
//                        var result = 0;
//                        if (response.data != "") {
//                            $scope.BookingNo = response.data;
//                            Command: toastr["success"]("Save  Successfully!!!!");
//                            $scope.clear();
//                        }
//                        else if (response.data == "") {
//                                Command: toastr["warning"]("Save Not Successful!!!!");
//                            $("#save").prop("disabled", false);
//                        }
//                    },
//                    function (error) {
//                        $("#save").prop("disabled", false);
//                        Command: toastr["warning"]("Save Not Successful!!!!");
//                    });
//                }
//                else if (chkQuantity == 0) {
//                    $("#save").prop("disabled", false);
//                    Command: toastr["warning"]("Quantity Must Not Zero Or Empty !!!!");
//                }
//            }
//            else if ($scope.ListBookingDetails.length <= 0) {
//                $("#save").prop("disabled", false);
//                Command: toastr["warning"]("Booking Detail Must Not Empty!!!!");
//            }
//        };

//        //**********----Reset Record----***************//
//        $scope.clear = function () {

//            $scope.IsCreateIcon = false;
//            $scope.IsListIcon = true;

//            $scope.BookingID = '0';
//            $scope.showDtgrid = 0;//$scope.ListBookingDetails.length;

//            $scope.IsHidden = true;
//            $scope.IsShow = true;
//            $scope.IsHiddenDetail = true;
//            $scope.btnPIShowText = "Show List";
//            $scope.btnPISaveText = "Save";
//            $scope.listBookingMaster = [];
//            $scope.ListBookingDetails = [];
//            $scope.bool = true;
//            $scope.listBuyer = [];

//            $scope.Description = '';
//            $scope.loadActivePIRecords(0);
//            loadSampleNoRecords(0);
//            loadBuyerRecords(0);
//            loadCompanyRecords(0);
//            loadBuyerReferenceRecords(0);

//            $scope.lstBuyerList = '';
//            $scope.lstSampleNoList = '';
//            $scope.lstBuyerReferenceList = '';

//            $("#ddlBuyer").select2("data", { id: '', text: '--Select Buyer--' });

//            $("#ddlBuyerReference").select2("data", { id: '', text: '--Select Buyer Reference--' });

//            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Sample/Article No--' });

//            $scope.BookingDate = conversion.NowDateCustom();
//        };

//    }

//]);


//function modal_fadeOut() {
//    $("#PIModal").fadeOut(200, function () {
//        $('#PIModal').modal('hide');
//    });
//}

//app.directive("datepicker", function () {
//    debugger
//    return {
//        restrict: "A",
//        require: "ngModel",
//        link: function (scope, elem, attrs, ngModel) {
//            var updateModel = function (dateText) {
//                scope.$apply(function () {
//                    ngModel.$setViewValue(dateText);
//                });
//            };
//            var options = {
//                format: "dd/mm/yyyy",
//                onSelect: function (dateText) {
//                    updateModel(dateText);
//                }                
//            };
//            elem.datepicker(options);
//        }
//    }
//});
