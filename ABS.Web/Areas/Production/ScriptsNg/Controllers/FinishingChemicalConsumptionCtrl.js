app.controller('FinishingChemicalConsumptionCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/FinishingChemicalConsumption/';
        var partialUrl = '/Production/api/SizingChamicaleConsumption/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsFinishingChemicalConsumption = [];

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.listChemConsumptionMaster = [];

        $scope.PageTitle = 'Chemical Consumption Creation';
        $scope.ListTitle = 'Chemical Consumption Information';
        $scope.ListTitleChemConsumptionMaster = 'Chemical Consumption List';
        $scope.ListChemConsumptionInfoDetails = [];
        $scope.ConsumptionDate = conversion.NowDateCustom();
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsfrmShow = true;
        $scope.IsShowDetail = true;
        $scope.ItemID = "";
        Ftype = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************        
        frmName = ''; DelFunc = 'DeleteFiniChemConsumption'; DelMsg = 'FinishingConsumptionNo'; EditFunc = 'GetFiniChemConsumptionMasterByID, AddItemToList';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************

        $scope.loadSetRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? 0 : dataModel.FinishingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingSetNo/';
            var listSetNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetNo.then(function (response) {
                $scope.listSetNo = response.data.AllWSetNo;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSetRecords(0);

        $scope.loadFinishTypeRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFinishingType/';
            var listFinishType = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listFinishType.then(function (response) {
                $scope.ListFinishType = response.data.AllFinishingType;
                if (Ftype != "") {
                    $("#ddlFinishType").select2("data", { id: 0, text: Ftype });
                    Ftype = "";
                }
                else {
                    $("#ddlFinishType").select2("data", { id: 0, text: '--Select Finish Type--' });
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //************************************************Start Unit Dropdown******************************************************
        $scope.loadUOMRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetUnit/';
            ModelsArray = [objcmnParam];
            var ListUOM = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListUOM.then(function (response) {
                debugger
                $scope.ListUOM = response.data.ListUOM;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadUOMRecords(1);
        //**************************************************End Unit Dropdown******************************************************  

        $scope.loadWeavingSetInfoRecords = function (lstSetNoList) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.WeavingMRRID) ? $scope.lstSetNoList : lstSetNoList.WeavingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingSetInformation/';
            var ListWeavingSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListWeavingSetInformation.then(function (response) {
                debugger
                $scope.lstSetNoList = "";
                $scope.ItemID = "";
                $scope.ArticleNo = "";
                if (response.data != null) {
                    $scope.lstSetNoList = response.data.WeavingMRRID;
                    $scope.ItemID = response.data.ItemID;
                    $scope.ArticleNo = response.data.ArticleNo;
                    $scope.loadFinishTypeRecords(response.data.WeavingMRRID);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        ChemConID = 0; CurrentStatus = 'Entry';
        $scope.AddItemToList = function (dataModel) {
            if (CurrentStatus != 'Modification') {
                $scope.cmnParam();
                objcmnParam.ItemType = angular.isUndefined(dataModel) || dataModel == null || dataModel == '' ? $scope.FInishTypeID : 0;
                objcmnParam.id = angular.isUndefined(dataModel) ? 0 : dataModel.FinishingConsumptionID;
                ChemConID = objcmnParam.id;
                $scope.ListChemicalInfo = [];
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetFiniChemConsumptionDetailByID/';
                var listItems = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listItems.then(function (response) {                    
                    $scope.ListChemicalInfo = response.data.objMachineSetupInfo;

                    angular.forEach($scope.ListChemicalInfo, function (tem) {
                        if (ChemConID > 0) {
                            tem.IsQtyReadOnly = false;
                            CurrentStatus = 'Modification';
                        }
                        else {
                            tem.IsQtyReadOnly = true;
                        }
                    })

                    $scope.IsShow = $scope.ListChemicalInfo.length > 0 ? true : false;

                    if (ChemConID == 0) {
                        $scope.cmnbtnShowHideEnDisable('true');
                    }

                    //if ($scope.ListChemicalInfo.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                    //$scope.IsShow = $scope.ListChemicalInfo.length > 0 ? true : false;
                    //$scope.loaderMore = false;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

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
            var apiRoute = partialUrl + 'GetCurrentStock/';
            var defaultStock = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            defaultStock.then(function (response) {
                debugger
                dataEntity.AccQty = '';
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
            if (dataEntity.AccQty > dataEntity.CurrentStock || dataEntity.AccQty < 0 || dataEntity.AccQty === null || dataEntity.AccQty == '') {
                Command: toastr["warning"]("Your Inputed Quantity is invalid!");
                $scope.cmnbtnShowHideEnDisable('true');
            }
            else {
                angular.forEach($scope.ListChemicalInfo, function (tem) {
                    $scope.cmnbtnShowHideEnDisable(tem.AccQty == null || tem.AccQty == "" ? 'true' : 'false');
                })
            }
        }

        //***************************************************Start Set Master Dynamic Grid******************************************************

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            //$scope.IsShow = $scope.IsShow ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnChemConsumptionShowText = "Show List";
                //$scope.IsCreateIcon = false;
                //$scope.IsListIcon = true;
            }
            else {
                //$scope.btnChemConsumptionShowText = "Create";
                $scope.RefreshMasterList();
                //$scope.IsCreateIcon = true;
                //$scope.IsListIcon = false;
                $scope.IsfrmShow = false;
                $scope.IsShowDetail = false;
                $scope.IsShow = false;
                //$scope.gridOptionsFinishingChemicalConsumption.enableFiltering = true;                
            }
        }

        $scope.deleteRow = function (index) {
            $scope.ListChemicalInfo.splice(index, 1);
            if ($scope.ListChemicalInfo.length == 0) { $scope.cmnbtnShowHideEnDisable('true'); }
        };

        //************************************************Start Show Master List Information Dynamic Grid******************************************************//
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
                $scope.loadAllChemConsumptionMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllChemConsumptionMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllChemConsumptionMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllChemConsumptionMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllChemConsumptionMasterRecords(1);
                }
            }
        };

        $scope.loadAllChemConsumptionMasterRecords = function (isPaging) {
            $scope.gridOptionsFinishingChemicalConsumption.enableFiltering = true;
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
            $scope.gridOptionsFinishingChemicalConsumption = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "FinishingConsumptionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeavingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingConsumptionNo", displayName: "Finishing Consumption No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeavingMRRNo", displayName: "Weaving MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Articale No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ConsumptionDate", displayName: "Consumption Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FInishTypeName", displayName: "Finishing Type", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Volume", displayName: "Volume", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", displayName: "Unit", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: 100,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsFinishingChemicalConsumption.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsFinishingChemicalConsumption.useExternalPagination = false;
                        $scope.gridOptionsFinishingChemicalConsumption.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetConsumptionMasterList/';
            var listDCMaster = crudService.getDynamicGrid(apiRoute, objcmnParam);
            listDCMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsFinishingChemicalConsumption.data = response.data.ListFiniChemMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllChemConsumptionMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //************************************************End Show Master List Information Dynamic Grid********************************************************//
        $scope.GetFiniChemConsumptionMasterByID = function (dataModel) {
            debugger
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShowDetail = true;
            $scope.IsShow = true;

            $scope.FinishingConsumptionID = dataModel.FinishingConsumptionID;
            $scope.ItemID = dataModel.ItemID;
            $scope.ArticleNo = dataModel.ArticleNo;
            $scope.ChemicalVolume = dataModel.Volume;
            $scope.ConsumptionDate = dataModel.ConsumptionDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.ConsumptionDate);
            $scope.Remarks = dataModel.Remarks;

            $scope.lstSetNoList = dataModel.WeavingMRRID;
            $("#ddlSetNoList").select2("data", { id: dataModel.WeavingMRRID, text: dataModel.WeavingMRRNo });

            $scope.UnitID = dataModel.UnitID;
            $("#UnitListD").select2("data", { id: dataModel.UnitID, text: dataModel.UOMName });

            $scope.lstFinishingType = dataModel.FinishingTypeID;
            Ftype = dataModel.FInishTypeName;
            $scope.loadFinishTypeRecords(dataModel.FinishingTypeID);
        }

        $scope.Save = function () {
            debugger
            $scope.cmnParam();
            var ChemConsumptionInfo = {
                FinishingConsumptionID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.FinishingConsumptionID,
                FinishingTypeID: $scope.FInishTypeID,
                WeavingMRRID: $scope.lstSetNoList,
                ItemID: $scope.ItemID,
                Volume: $scope.ChemicalVolume,
                Remarks: $scope.Remarks,
                UnitID: $scope.UnitID,
                ConsumptionDate: $scope.ConsumptionDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ConsumptionDate)
            };

            if ($scope.ListChemicalInfo.length == 0) {
                Command: toastr["warning"]("Please add consumption detail.");
                return;
            }

            $scope.FinalChemList = [];
            angular.forEach($scope.ListChemicalInfo, function (take) {
                $scope.FinalChemList.push({
                    FinishingConsumptionDetailID: take.FinishingConsumptionDetailID, FinishingConsumptionID: take.FinishingConsumptionID,
                    ChemicalID: take.ChemicalID, SupplierID: take.SupplierID, BatchID: take.BatchID, UnitID: take.UnitID,
                    AccQty: take.AccQty, UnitPrice: take.UnitPrice, Amount: conversion.roundNumber((take.UnitPrice * take.AccQty), 4)
                });
            })

            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [ChemConsumptionInfo, $scope.FinalChemList, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateChemConsumptionInfo/';
            var ConsumptionSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            ConsumptionSaveUpdate.then(function (response) {
                debugger
                if (response != "") {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                    $scope.IsDetailShow = false;
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
        function (error) {
            //  $("#save").prop("disabled", false);
            Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
        });
        };

        $scope.DeleteFiniChemConsumption = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.FinishingConsumptionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteFiniChemConsumptionMD/';
            var delFinishingMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delFinishingMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.FinishingConsumptionNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.FinishingConsumptionNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.FinishingConsumptionNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });

        }

        $scope.clear = function () {
            $scope.frmFinishingChemCosumption.$setPristine();
            $scope.frmFinishingChemCosumption.$setUntouched();

            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsfrmShow = true;
            $scope.IsShowDetail = true;
            $scope.lstSetNoList = '';
            $scope.ItemID = "";
            $scope.UnitID = "";
            $scope.ArticleNo = "";
            $scope.FInishTypeID = "";
            $scope.dtConsumptionDate = conversion.NowDateCustom();
            $scope.ListChemicalInfo = [];
            $scope.Remarks = '';
            $scope.ChemicalVolume = '';
            $scope.FinishingConsumptionID = "";

            $("#ddlSetNoList").select2("data", { id: 0, text: '--Select Set No--' });
            $("#UnitListD").select2("data", { id: 0, text: '--Select Unit--' });
            $("#ddlFinishType").select2("data", { id: 0, text: '--Select Finish Type--' });
        };
    }]);
