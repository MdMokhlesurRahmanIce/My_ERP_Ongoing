/**
 * SizingChemicaleSetupCtrl.js
 */
app.controller('sizingChemicaleSetupCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants','PublicService',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants, PublicService) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/SizingChemicaleSetup/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsMMaster = [];
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var inCallback = false;
        //$scope.IsSaveDisable = true;
        
        $scope.ItemID = 0;
        $scope.tempChemList = [];
        $scope.ListItems = [];
        $scope.tempSet = [];
        //var PrevSet = 0;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Sizing Chemical Setup Data Entry (Master)';
        $scope.PageTitleDetail = 'Sizing Chemical Setup Data Entry (Detail)';
        $scope.ListTitleMaster = 'Sizing Chemical Setup Master Records';
        $scope.ListTitleDetail = 'Sizing Chemical Setup Details Records';
        $scope.holdChemicalSetupDetailID = "";
        $scope.AutoSlNo = 0;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsSaveShow = false;
        //$scope.IsAddToListShow = true;
        $scope.IsfrmShow = true;
        $scope.IsfrmDetailShow = true;
        //$scope.IsUpdateListShow = false;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'SetNo'; EditFunc = 'getSizingChemicalSetupMasterByID, AddItemToList, loadSetRecords';
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

        //************************************************Start Chemical Dropdown******************************************************
        $scope.loadChemicalRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 5;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetArticle/';
            var ListChemical = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListChemical.then(function (response) {
                debugger
                $scope.ListChemical = response.data.ListArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadChemicalRecords(1);
        //**************************************************End Chemical Dropdown******************************************************        

        //************************************************Start Unit for Detail Dropdown******************************************************
        $scope.loadUOMDRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.ChemicalID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetUnitSingle/';
            var ListUOMD = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListUOMD.then(function (response) {
                debugger
                dataModel.UnitID = response.data.ListUOMSingle.UOMID;
                dataModel.UOMName = response.data.ListUOMSingle.UOMName;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //**************************************************End Unit for Detail Dropdown******************************************************


        $scope.getSizingChemicalSetupMasterByID = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.ChemicalSetupID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSizingChemicalSetupMasterByID/';
            var ListMasterByID = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMasterByID.then(function (response) {
                debugger
                $scope.ChemicalSetupID = response.data.ListMasterByID.ChemicalSetupID;
                $scope.lstArticleList = response.data.ListMasterByID.ItemID;
                $scope.ArticleNo = response.data.ListMasterByID.ArticleNo;
                //$("#ArticleList").select2("data", { id: 0, text: response.data.ListMasterByID.ArticleNo });

                $scope.lstSetList = response.data.ListMasterByID.SetID;
                $("#SetList").select2("data", { id: 0, text: response.data.ListMasterByID.SetNo });

                $scope.lstUnitList = response.data.ListMasterByID.UnitID;
                $("#UnitList").select2("data", { id: 0, text: response.data.ListMasterByID.UOMName });

                $scope.Quantity = response.data.ListMasterByID.Qty;
                $scope.KGPreMin = response.data.ListMasterByID.KGPreMin;
                $scope.Speed = response.data.ListMasterByID.Speed;

                $scope.lstChemicalList = "";
                $scope.MinQty = "";
                $scope.MaxQty = "";
                $scope.lstUnitListD = "";
                $("#ChemicalList").select2("data", { id: 0, text: "--Select Chemical--" });
                $("#UnitListD").select2("data", { id: 0, text: "--Select Unit--" });

                //$scope.cmnbtnShowHideEnDisable(2);
                $scope.IsHidden = true;
                $scope.IsfrmShow = true;
                $scope.IsfrmDetailShow = true;
                //$scope.IsShow = $scope.tempChemList != "" ? true : false;
                $scope.IsShow = true;
                //$scope.btnSaveText = "Update";
                //$scope.btnShowText = "Show List";
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //$scope.getSizingChemicalSetupDetailSingleByID = function (dataModel) {
        //    debugger
        //    if (dataModel.ChemicalSetupDetailID == 0) {
        //        $scope.holdChemicalSetupDetailID = dataModel.SlNo;
        //    }
        //    else {
        //        $scope.holdChemicalSetupDetailID = dataModel.ChemicalSetupDetailID;
        //    }
        //    $scope.lstChemicalList = dataModel.ChemicalID;
        //    $("#ChemicalList").select2("data", { id: 0, text: dataModel.ItemName });
        //    $scope.MinQty = dataModel.MinQty;
        //    $scope.MaxQty = dataModel.MaxQty;
        //    $scope.lstUnitListD = dataModel.UnitID;
        //    $("#UnitListD").select2("data", { id: 0, text: dataModel.UOMName });
        //}

        //*****************************************Start Delete Item From List**************************************
        $scope.deleteRow = function (index) {
            $scope.tempChemList.splice(index, 1);
            $scope.IsShow = $scope.tempChemList.length > 0 ? true : false;
            if ($scope.tempChemList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
        };
        //*****************************************End Delete Item From List**************************************

        //*******************************************************Start Generate Row************************************************

        $scope.AddItemToList = function (dataModel) {
            debugger
            $scope.IsShow = true;
            $scope.IsGenerateShow = false;
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel) ? 0 : dataModel.ChemicalSetupID;
            if (objcmnParam.id != 0) {
                $scope.tempChemList = [];
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetSizingChemicalSetupDetailByID/';
                var listChemDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listChemDetail.then(function (response) {
                    $scope.tempChemList = response.data.ListDetailByID;
                    if ($scope.tempChemList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                    //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                debugger
                $scope.AutoSlNo = $scope.AutoSlNo + 1;
                $scope.tempChemList.push({
                    SlNo: $scope.AutoSlNo, ChemicalSetupDetailID: 0, ChemicalSetupID: 0, ChemicalID: 0, MinQty: "", MaxQty: "", UnitID: 0, UOMName: ''
                })
                if ($scope.tempChemList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
            }
        };
        //*******************************************************End Generate Row**************************************************

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
            $scope.gridOptionsMMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "ChemicalSetupID", displayName: "ChemicalSetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", displayName: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", displayName: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitID", displayName: "UnitID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", title: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Qty", displayName: "Qty", title: "Qty", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", displayName: "Unit", title: "Unit", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsMMaster.useExternalPagination = false;
                        $scope.gridOptionsMMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSizingChemicalSetupMaster/';
            var listChemicalMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemicalMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMMaster.data = response.data.ListChemicalMaster;

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
                ChemicalSetupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.ChemicalSetupID,
                ItemID: angular.isUndefined($scope.lstArticleList) ? null : $scope.lstArticleList,
                SetID: angular.isUndefined($scope.lstSetList) ? null : $scope.lstSetList,
                Qty: angular.isUndefined($scope.Quantity) ? null : $scope.Quantity,
                UnitID: angular.isUndefined($scope.lstUnitList) ? null : $scope.lstUnitList
            };

            //*************----Detail Data---**************
            debugger
            if ($scope.tempChemList.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [SetChemicalMaster, $scope.tempChemList, objcmnParam];
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
            objcmnParam.id = delModel.ChemicalSetupID;
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
            $scope.frmSizingChemicaleSetupEntryMaster.$setPristine();
            $scope.frmSizingChemicaleSetupEntryMaster.$setUntouched();
            //$scope.cmnbtnShowHideEnDisable(0);
            $scope.ChemicalSetupID = 0;
            $scope.lstArticleList = "";
            $scope.lstSetList = "";
            $scope.lstUnitList = "";
            $scope.Quantity = "";
            $scope.ListArticle = [];
            $scope.ListSet = [];
            $scope.ListUOM = [];

            $scope.lstChemicalList = "";
            $scope.MinQty = "";
            $scope.MaxQty = "";
            $scope.lstUnitListD = "";
            $scope.ListChemical = [];
            //$scope.ListUOMD = [];
            $scope.AutoSlNo = 0;

            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.IsShow = false;
            //$scope.IsSaveShow = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsfrmDetailShow = true;
            $scope.IsDeleted = false;
            //$scope.IsSaveDisable = true;
            //$scope.IsUpdateListShow = false;
            //$scope.IsAddToListShow = true;

            $scope.gridOptionsMMaster.data = [];
            //$scope.gridOptionsChemDetail.data = [];
            $scope.tempChemList = [];
            $scope.loadArticleRecords(1);
            $scope.loadChemicalRecords(1);
            $scope.loadUOMRecords(1);
            $scope.loadUOMDRecords(1);

            //$("#ArticleList").select2("data", { id: 0, text: "--Select Article No--" });
            $scope.ArticleNo = "";
            $("#SetList").select2("data", { id: 0, text: "--Select Set No--" });
            $("#UnitList").select2("data", { id: 0, text: "--Select Unit--" });
            $("#ChemicalList").select2("data", { id: 0, text: "--Select Chemical--" });
            $("#UnitListD").select2("data", { id: 0, text: "--Select Unit--" });
        };
        //**************************************************************End Reset/Clear**********************************************************


        //*********************************************************  Article No *****************************************************************
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