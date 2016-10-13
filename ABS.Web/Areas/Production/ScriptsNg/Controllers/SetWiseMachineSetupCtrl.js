/**
 * SetWiseMachineSetupCtrl.js
 */
app.controller('setWiseMachineSetupCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants','PublicService',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants, PublicService) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/SetWiseMachineSetup/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsMMaster = [];
        $scope.MachineDetail = [];
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var inCallback = false;
        $scope.ListItems = [];
        $scope.tempSet = [];
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsSaveShow = true;
        $scope.IsfrmShow = true;
        $scope.lstArticleList = 0;
        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitle = 'Set Wise Machine Setup Data Entry';
        $scope.ListTitleSetMaster = 'Set Wise Machine Setup Master Records';
        $scope.ListTitleSetSetupDetails = 'Set Wise Machine Setup Details Records';
        //var MessageData = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmSetWiseMachineSetupEntry'; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'SetupID'; EditFunc = 'getSetWiseMachineSetupMasterById, callGenerate';
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
            var apiRoute = baseUrl + 'GetArticle/';
            ModelsArray = [objcmnParam];
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

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadMachineRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            //objcmnParam.ItemType = 4; objcmnParam.ItemGroup = 47;
            var apiRoute = baseUrl + 'GetMachine/';
            ModelsArray = [objcmnParam];
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.ListMachine = response.data.ListMachine;
                angular.forEach($scope.ListMachine, function (tempitem) {
                    if (tempitem.MachineID == 13) {
                        $scope.lstMachineList = tempitem.MachineID;
                        $("#MachineList").select2("data", { id: 0, text: tempitem.MachineName });
                    }
                });
                if (!angular.isUndefined($scope.lstMachineList) && ($scope.lstMachineList != "" || $scope.lstMachineList != 0)) {
                    $scope.callGenerate($scope.lstMachineList);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadMachineRecords(0);
        //**************************************************End Machine Dropdown******************************************************        

        //************************************************Start Operation Dropdown******************************************************
        $scope.tempListOperation = [];
        $scope.loadOperationRecords = function (isPaging) {
            debugger
            $scope.tempListOperation = [];
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetDetailOperation/';
            ModelsArray = [objcmnParam];
            var ListOperation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListOperation.then(function (response) {
                debugger
                $scope.ListOperation = response.data.ListOperation;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadOperationRecords(1);
        //**************************************************End Operation Dropdown*************************************************        

        $scope.getSetWiseMachineSetupMasterById = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.SetupID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetWiseMachineSetupMasterByID/';
            var ListMasterByID = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMasterByID.then(function (response) {
                debugger
                $scope.SetupID = response.data.ListMasterByID.SetupID;
                $scope.lstArticleList = response.data.ListMasterByID.ItemID;
                $scope.ArticleNo = response.data.ListMasterByID.ArticleNo;
                //$("#ArticleList").select2("data", { id: 0, text: response.data.ListMasterByID.ArticleNo });

                $scope.lstMachineList = response.data.ListMasterByID.MachineID;
                $("#MachineList").select2("data", { id: 0, text: response.data.ListMasterByID.MachineName });

                $scope.Moiture = response.data.ListMasterByID.Moiture;
                $scope.KGPreMin = response.data.ListMasterByID.KGPreMin;
                $scope.Speed = response.data.ListMasterByID.Speed;
                $scope.IsHidden = true;
                $scope.IsfrmShow = true;
                //$scope.IsShow = $scope.MachineDetail.length != "" ? true : false;                
                //$scope.btnSaveText = "Update";
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //*******************************************************Start Generate Row************************************************

        //$scope.paginationMDetail = {
        //    paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
        //    ddlpageSize: 15,
        //    pageNumber: 1,
        //    pageSize: 15,
        //    totalItems: 0
        //};
        var tempSetupID = "";
        $scope.callGenerate = function (dataModel) {
            debugger
            tempSetupID = "";
            var tempMachineID = "";
            if (angular.isUndefined(dataModel) || dataModel == 0) {
                tempMachineID = $scope.lstMachineList;
            }
            else {
                if (angular.isUndefined(dataModel.SetupID) || dataModel.SetupID == "") {
                    tempMachineID = $scope.lstMachineList;
                }
                else {
                    tempSetupID = dataModel.SetupID;
                    tempMachineID = dataModel.SetupID;
                }
            }

            $scope.IsShow = angular.isUndefined(tempMachineID) ? false : true;

            $scope.generate(tempMachineID);
        };

        $scope.generate = function (tempMachineID) {
            debugger;
            $scope.cmnParam();
            objcmnParam.id = tempMachineID;
            $scope.MachineDetail = [];
            if (angular.isUndefined(tempSetupID) || tempSetupID == "") {
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetDetailBox/';
                var listBoxDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listBoxDetail.then(function (response) {
                    $scope.MachineDetail = response.data.ListBox;
                    $scope.IsShow = $scope.MachineDetail.length > 0 ? true : false;
                    //$scope.IsSaveShow = $scope.MachineDetail.length < 1 || angular.isUndefined($scope.MachineDetail.length) ? false : true;
                    if ($scope.MachineDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                },
                function (error) {
                    console.log("Error: " + error);

                });
            }
            else {
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetSetWiseMachineSetupDetailByID/';
                var listBoxDetails = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listBoxDetails.then(function (response) {
                    $scope.MachineDetail = response.data.ListDetailByID;
                    $scope.IsShow = $scope.MachineDetail.length > 0 ? true : false;
                    //$scope.IsSaveShow = $scope.MachineDetail.length < 1 || angular.isUndefined($scope.MachineDetail.length) ? false : true;
                    if ($scope.MachineDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                },
                function (error) {
                    console.log("Error: " + error);

                });
            }
        }
        //*******************************************************End Generate Row**************************************************

        //*******************************************************Start Delete Detail Row*******************************************
        $scope.deleteRow = function (index) {
            $scope.MachineDetail.splice(index, 1);
            $scope.IsShow = $scope.MachineDetail.length > 0 ? true : false;
            //$scope.IsSaveShow = $scope.MachineDetail.length > 0 ? true : false;
            if ($scope.MachineDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        };
        //********************************************************End Delete Detail Row********************************************

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
                $scope.loadSetWiseMachineSetup(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSetWiseMachineSetup(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadSetWiseMachineSetup(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSetWiseMachineSetup(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadSetWiseMachineSetup(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadSetWiseMachineSetup = function (isPaging) {
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
                    { name: "SetupID", displayName: "SetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MechineID", displayName: "MechineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineName", displayName: "Machine Name", title: "Machine Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Speed", displayName: "Speed", title: "Speed", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Moiture", displayName: "Mositure", title: "Mositure", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "KGPreMin", displayName: "KGP/Min", title: "KGP/Min", headerCellClass: $scope.highlightFilteredHeader },
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

            var apiRoute = baseUrl + 'GetSetWiseMachineSetupMaster/';
            ModelsArray = [objcmnParam];
            var listSetWiseMachineMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetWiseMachineMaster.then(function (response) {
                angular.forEach(response.data.ListSetWiseMachineMaster, function (items) {
                    items.ArticleNo = items.ItemID == 0 ? "N/A" : items.ArticleNo;
                    items.MachineName = items.MechineID == 0 ? "N/A" : items.MachineName;
                    items.Speed = items.Speed == 0 ? "N/A" : items.Speed;
                    items.Moiture = items.Moiture == 0 ? "N/A" : items.Moiture;
                    items.KGPreMin = items.KGPreMin == 0 ? "N/A" : items.KGPreMin;
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMMaster.data = response.data.ListSetWiseMachineMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadSetWiseMachineSetup(0);
        }
        $scope.RefreshMasterList();
        //***************************************************End Set Master Dynamic Grid******************************************************

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                //$scope.IsfrmShow = true;
                //$scope.IsGenerateShow = true;
                //$scope.IsShow = $scope.MachineDetail.length < 1 || angular.isUndefined($scope.MachineDetail.length) ? false : true;
                //$scope.IsSaveShow = $scope.MachineDetail.length < 1 || angular.isUndefined($scope.MachineDetail.length) ? false : true;
            }
            else {
                $scope.RefreshMasterList();
                //$scope.btnShowText = "Create";
                //$scope.pagination.pageNumber = 1;
                //$scope.loadSetWiseMachineSetup(1);
                $scope.IsfrmShow = false;
                $scope.IsShow = false;
                //$scope.IsSaveShow = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.Save = function () {
            debugger
            //MessageData = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var SetWiseMachineSetupMaster = {
                SetupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.SetupID,
                ItemID: angular.isUndefined($scope.lstArticleList) ? null : $scope.lstArticleList,
                MachineID: angular.isUndefined($scope.lstMachineList) ? null : $scope.lstMachineList,
                Speed: angular.isUndefined($scope.Speed) ? null : $scope.Speed,
                Moiture: angular.isUndefined($scope.Moiture) ? null : $scope.Moiture,
                KGPreMin: angular.isUndefined($scope.KGPreMin) ? null : $scope.KGPreMin
            };

            //*************----Detail Data---**************
            debugger
            if ($scope.MachineDetail.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [SetWiseMachineSetupMaster, $scope.MachineDetail, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateSetWiseMachineSetupMasterDetail/';
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
            objcmnParam.id = delModel.SetupID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DelUpdateSetWiseMachineSetupMasterDetail/';
            var SetWiseMachineSetupMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetWiseMachineSetupMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    //$scope.clear();
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("Data has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                //console.log("Error: " + error);
            });
        };
        //*********************************************************End Save/Update/Delete**********************************************************

        //**************************************************************Start Reset/Clear**********************************************************
        $scope.clear = function () {
            debugger
            $scope.frmSetWiseMachineSetupEntry.$setPristine();
            $scope.frmSetWiseMachineSetupEntry.$setUntouched();
            $scope.SetupID = 0;
            $scope.lstArticleList = "";
            //$scope.ListArticle = [];
            $scope.lstMachineList = "";
            //$scope.ListMachine = [];
            $scope.Moiture = "";
            $scope.KGPreMin = "";
            $scope.Speed = "";

            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsDeleted = false;

            $scope.gridOptionsMMaster.data = [];
            $scope.MachineDetail = [];
            //$scope.loadArticleRecords(1);
            $scope.loadMachineRecords(0);
            //$scope.loadOperationRecords(1);

            //$("#ArticleList").select2("data", { id: 0, text: "--Select Article No--" });
            //$("#MachineList").select2("data", { id: 0, text: "--Select Machine--" });
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