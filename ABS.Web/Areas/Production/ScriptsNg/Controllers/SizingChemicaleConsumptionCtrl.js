/**
 * SizingChemicaleConsumptionCtrl.js
 */
app.controller('SizingChemicaleConsumptionCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants', 'PublicService',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants, PublicService) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/SizingChamicaleConsumption/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsConsumpMaster = [];
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.lstArticleList = 0;
        $scope.tempChemList = [];
        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Sizing Chemical Consumption Entry (Master)';
        $scope.PageTitleDetail = 'Sizing Chemical Consumption Entry (Detail)';
        $scope.ListTitleMaster = 'Sizing Chemical Consumption Master Records';
        $scope.ListTitleDetail = 'Sizing Chemical Consumption Details Records';
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsSaveShow = false;
        //$scope.IsAddToListShow = true;
        $scope.IsfrmShow = true;
        $scope.IsfrmDetailShow = true;
        //$scope.IsQtyEnable = true;
        //$scope.IsUpdateListShow = false;
        $scope.ConsumptionDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'ChemicalConsumptionID'; EditFunc = 'getSizingChemicalConsumptionMasterByID, AddItemToList';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Article Dropdown******************************************************
        $scope.loadArticleRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetArticle/';
            var ListArticle = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListArticle.then(function (response) {
                debugger
                $scope.ListArticle = response.data.ListArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadArticleRecords(1);
        //**************************************************End Article Dropdown******************************************************

        //************************************************Start Set Dropdown******************************************************
        $scope.loadSetRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 5;
            objcmnParam.id = dataModel != 0 ? dataModel.ItemID : $scope.lstArticleList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetByArticalNo/';
            var ListSet = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSet.then(function (response) {
                debugger
                $scope.ListSet = response.data.ListSet;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //**************************************************End Set Dropdown****************************************************** 

        //****************************************************Start Supplier DropdownList******************************************
        $scope.loadSupplierRecords = function (UserTypeID) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = UserTypeID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSupplier/';
            var ListSupplier = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSupplier.then(function (response) {
                $scope.ListSupplier = response.data.SupplierList;

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //$scope.loadSupplierRecords(3);
        //****************************************************End Supplier DropdownList********************************************

        //****************************************************Start Supplier DropdownList******************************************
        $scope.loadBatchRecords = function () {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBatches/';
            var ListBatches = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListBatches.then(function (response) {
                $scope.ListBatches = response.data.BatcheList;

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //$scope.loadBatchRecords();
        //****************************************************End Supplier DropdownList********************************************

        $scope.getSizingChemicalConsumptionMasterByID = function (dataModel) {

            debugger
            $scope.ChemicalConsumptionID = dataModel.ChemicalConsumptionID;
            $scope.lstArticleList = dataModel.ItemID;
            $scope.ArticleNo = dataModel.ArticleNo;
            //$("#ArticleList").select2("data", { id: 0, text: dataModel.ArticleNo });

            $scope.lstSetList = dataModel.SetID;
            $("#SetList").select2("data", { id: 0, text: dataModel.SetNo });
            $scope.ConsumptionDate = dataModel.ConsumptionDate == "" || dataModel.ConsumptionDate == null ? "" : conversion.getDateToString(dataModel.ConsumptionDate);
            $scope.Remarks = dataModel.Remarks;

            //$scope.cmnbtnShowHideEnDisable(2);
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsfrmDetailShow = true;
            //$scope.IsShow = $scope.tempChemList != "" ? true : false;
            $scope.IsShow = true;
            $scope.loadSetRecords(dataModel);
            //$scope.btnSaveText = "Update";
            //$scope.btnShowText = "Show List";            
        }

        //*****************************************Start Delete Item From List**************************************
        $scope.deleteRow = function (index) {
            $scope.tempChemList.splice(index, 1);
            $scope.IsShow = $scope.tempChemList.length > 0 ? true : false;
            if ($scope.tempChemList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
        };
        //*****************************************End Delete Item From List**************************************

        //*******************************************************Start Generate Row************************************************
        ChemConID = 0; CurrentStatus = 'Entry';
        $scope.AddItemToList = function (dataModel) {
            debugger
            if (CurrentStatus != 'Modification') {
                $scope.cmnParam();
                objcmnParam.ItemType = angular.isUndefined(dataModel) || dataModel == "" || dataModel == null ? $scope.lstSetList : 0;
                objcmnParam.id = angular.isUndefined(dataModel) ? 0 : dataModel.ChemicalConsumptionID;
                ChemConID = objcmnParam.id;
                $scope.tempChemList = [];
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetSizingChemicalConsumptionDetailByID/';
                var listChemDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listChemDetail.then(function (response) {
                    debugger
                    $scope.tempChemList = response.data.ListDetailByID;
                    //$scope.tempChemList.IsQtyEnable = true;
                    angular.forEach($scope.tempChemList, function (tem) {
                        if (ChemConID > 0) {
                            tem.IsQtyReadOnly = false;
                            CurrentStatus = 'Modification';
                        }
                        else {
                            tem.IsQtyReadOnly = true;
                        }
                    })

                    $scope.IsShow = $scope.tempChemList.length > 0 ? true : false;

                    if (ChemConID == 0) {
                        $scope.cmnbtnShowHideEnDisable('true');
                    }

                    //if ($scope.tempChemList.length > 0) { if (angular.isUndefined(dataModel)) { $scope.cmnbtnShowHideEnDisable('false'); } } else { $scope.cmnbtnShowHideEnDisable('true'); }
                    //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        };
        //*******************************************************End Generate Row**************************************************

        //*******************************************************Start Load Current Stock**************************************************
        $scope.loadCurrentStock = function (dataEntity) {
            debugger
            if (dataEntity.Supplier.length > 0) {
                if (dataEntity.SupplierID === undefined || dataEntity.SupplierID === null) {
                    //Command: toastr["warning"]("Please Select Supplier!");
                    return;
                }
            }
            if (dataEntity.Batch.length > 0) {
                if (dataEntity.BatchID === undefined || dataEntity.BatchID === null) {
                    //Command: toastr["warning"]("Please Select Batch!");
                    return;
                }
            }

            $scope.cmnParam();
            objcmnParam.id = dataEntity.ChemicalID;
            objcmnParam.ItemType = dataEntity.SupplierID === undefined ? 0 : dataEntity.SupplierID;
            objcmnParam.ItemGroup = dataEntity.BatchID === undefined ? 0 : dataEntity.BatchID;
            objcmnParam.UserType = dataEntity.UnitID === undefined ? 0 : dataEntity.UnitID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetCurrentStock/';
            var defaultStock = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            defaultStock.then(function (response) {
                debugger
                dataEntity.Qty = '';
                dataEntity.CurrentStock = '';
                dataEntity.UnitPrice = '';
                dataEntity.IsQtyReadOnly = true;

                if (response.data.singleCStock.CurrentStock > 0 && response.data.singleCStock.CurrentStock != null) {
                    dataEntity.CurrentStock = response.data.singleCStock.CurrentStock;
                    dataEntity.UnitPrice = response.data.singleCStock.UnitPrice;
                    dataEntity.IsQtyReadOnly = false;
                }
                //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //********************************************************End Load Current Stock***************************************************

        //*******************************************************Start Load Current Stock**************************************************
        $scope.CalculateStock = function (dataEntity) {
            debugger
            if (dataEntity.Qty > dataEntity.CurrentStock || dataEntity.Qty < 0 || dataEntity.Qty === null || dataEntity.Qty == '') {
                Command: toastr["warning"]("Your Inputed Quantity is invalid!");
                $scope.cmnbtnShowHideEnDisable('true');
            }
            else {
                angular.forEach($scope.tempChemList, function (tem) {
                    $scope.cmnbtnShowHideEnDisable(tem.Qty == null || tem.Qty == "" ? 'true' : 'false');
                })
            }
        }

        //***************************************************Start Set Master Dynamic Grid******************************************************
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
                $scope.loadSizingChemicalSetup(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSizingChemicalSetup(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadSizingChemicalSetup(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSizingChemicalSetup(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadSizingChemicalSetup(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadSizingChemicalSetup = function (isPaging) {
            debugger;
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize;
            objcmnParam.pageSize = $scope.pagination.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsConsumpMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "ChemicalConsumptionID", displayName: "ChemicalConsumptionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", displayName: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", displayName: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", title: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OrganogramName", displayName: "Department", title: "Department", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ConsumptionDate", displayName: "Consumption Date", title: "Consumption Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", title: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '13%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.pagination.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsConsumpMaster.useExternalPagination = false;
                        $scope.gridOptionsConsumpMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSizingChemicalConsumptionMaster/';
            var listChemicalMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemicalMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsConsumpMaster.data = response.data.ListChemicalMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.loadSizingChemicalSetup(0);
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadSizingChemicalSetup(0);
        }
        //***************************************************End Set Master Dynamic Grid******************************************************

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                ////$scope.IsShow = true;
                //$scope.IsSaveShow = true;
                //$scope.IsfrmShow = true;
                //$scope.IsfrmDetailShow = true;
                ////$scope.IsAddToListShow = true;
                //$scope.IsShow = $scope.tempChemList.length < 1 || angular.isUndefined($scope.tempChemList.length) ? false : true;
                //$scope.IsSaveShow = $scope.tempChemList.length < 1 || angular.isUndefined($scope.tempChemList.length) ? false : true;
            }
            else {
                //$scope.btnShowText = "Create";
                //$scope.cmnbtnShowHideEnDisable(1);
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                $scope.IsfrmDetailShow = false;
                $scope.IsShow = false;
                //$scope.IsSaveShow = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var SetChemicalMaster = {
                ChemicalConsumptionID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.ChemicalConsumptionID,
                ItemID: angular.isUndefined($scope.lstArticleList) ? null : $scope.lstArticleList,
                SetID: angular.isUndefined($scope.lstSetList) ? null : $scope.lstSetList,
                ConsumptionDate: $scope.ConsumptionDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ConsumptionDate),
                Remarks: $scope.Remarks
            };

            //*************----Detail Data---**************
            debugger
            if ($scope.tempChemList.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }

            $scope.FinalChemList = [];
            angular.forEach($scope.tempChemList, function (take) {
                $scope.FinalChemList.push({
                    ChemicalConsumptionDetailID: take.ChemicalConsumptionDetailID, ChemicalConsumptionID: take.ChemicalConsumptionID,
                    ChemicalID: take.ChemicalID, SupplierID: take.SupplierID, BatchID: take.BatchID, UnitID: take.UnitID,
                    Qty: take.Qty, UnitPrice: take.UnitPrice, Amount: conversion.roundNumber((take.UnitPrice * take.Qty), 4)
                });
            })


            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [SetChemicalMaster, $scope.FinalChemList, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateSizingChemicalMasterDetail/';
            var SetWiseMachineSetupCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SetWiseMachineSetupCreateUpdate.then(function (response) {
                if (response.result != '') {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //********-------End------************

        //********---------Delete Data-------**************
        $scope.DeleteUpdateMasterDetail = function (delModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = delModel.ChemicalConsumptionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DelUpdateSizingChemicalMasterDetail/';
            var SetWiseMachineSetupMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetWiseMachineSetupMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.RefreshMasterList();
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
        //*********************************************************End Save/Update/Delete**********************************************************

        //**************************************************************Start Reset/Clear**********************************************************
        $scope.clear = function () {
            debugger
            $scope.frmSizingChemicaleConsumptionEntryMaster.$setPristine();
            $scope.frmSizingChemicaleConsumptionEntryMaster.$setUntouched();
            $scope.ChemicalConsumptionID = 0;
            $scope.lstArticleList = "";
            $scope.lstSetList = "";
            $scope.Remarks = "";
            CurrentStatus = 'Entry';
            $scope.ConsumptionDate = conversion.NowDateCustom();
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsfrmDetailShow = true;
            $scope.gridOptionsConsumpMaster.data = [];
            $scope.tempChemList = [];
            $scope.ArticleNo = "";
            $("#SetList").select2("data", { id: 0, text: "--Select Set No--" });
            $("#UnitList").select2("data", { id: 0, text: "--Select Unit--" });
            $("#ChemicalList").select2("data", { id: 0, text: "--Select Chemical--" });
            $("#UnitListD").select2("data", { id: 0, text: "--Select Unit--" });
        };
        //**************************************************************End Reset/Clear**********************************************************
        //********************************************** Item Modal Code *****************************************
        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;
        $scope.getListItemMaster = function (model) {
            var ItemID = model.ItemID;
            var ItemName = model.ItemName;
            $scope.lstArticleList = model.ItemID;
            $scope.ArticleNo = ItemName;

            $scope.ListSet = [];
            $("#SetList").select2("data", { id: 0, text: "--Select Set No--" });
            var dataModel = {};
            dataModel.ItemID = $scope.lstArticleList;
            $scope.loadSetRecords(dataModel);


            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('hide'); });
        }
        $scope.modalClose = function () {
            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
        }
        $scope.SearchItem = function (serachItemName) {
            $scope.IsCallFromSearch = serachItemName == "" ? false : true;
            $scope.modalSearchItemName = serachItemName.toString();
            $scope.paginationItemMaster.pageNumber = 2;
            $scope.paginationItemMaster.firstPage();
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
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });
            $('#ItemModal').modal({ show: true, backdrop: "static" });
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
                selectedCompany: $scope.UserCommonEntity.loggedCompnyID,
                serachItemName: $scope.IsCallFromSearch == true ? $scope.modalSearchItemName : "100"
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {
                rowTemplate: '<div ng-dblclick="grid.appScope.getListItemMaster(row.entity)" ng-repeat="col in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>',
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeight", displayName: "Weight", title: "Weight", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
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

                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
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


            var apiRoute = '/SystemCommon/api/PublicApi/' + 'GetFinishedItemMaster/';
            var listItemMaster = PublicService.getItemMasterService(apiRoute, objcmnParam);
            listItemMaster.then(function (response) {
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

        };
    }]);