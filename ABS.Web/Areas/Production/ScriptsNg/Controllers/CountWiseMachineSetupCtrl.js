/**
 * CountWiseMachineSetupCtrl.js
 */
app.controller('countWiseMachineSetupCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/CountWiseMechineSetup/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsMSetup = [];
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var inCallback = false;
        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitle = 'Count wise Machine Setup Entry';
        $scope.ListTitleSetMaster = 'Count wise Machine Setup Records';
        $scope.ListTitleSetSetupDetails = 'Set Setup Details Records';
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsSaveShow = true;
        $scope.IsGenerateShow = true;
        $scope.IsfrmShow = true;
        var MessageData = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmMachineSetupEntry'; DelFunc = 'DeleteMachineSetup'; DelMsg = 'Count'; EditFunc = 'getMachinSetById';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Count Dropdown******************************************************
        $scope.loadCountRecords = function (CountModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = CountModel == 0 ? 0 : CountModel.Count;
            var apiRoute = baseUrl + 'GetCount/';
            ModelsArray = [objcmnParam];
            var ListCount = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListCount.then(function (response) {
                debugger
                $scope.ListCount = response.data.ListCount;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadCountRecords(0);
        //**************************************************End Count Dropdown******************************************************        

        //************************************************Start Selected Item Data (Single)****************************************
        $scope.getMachinSetById = function (dataModels) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModels.BallMachineSetupID;
            var apiRoute = baseUrl + 'GetMachinSetup/';
            ModelsArray = [objcmnParam];
            var ListMachineSetData = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachineSetData.then(function (response) {
                debugger
                $scope.BallMachineSetupID = response.data.ListMachineSet[0].BallMachineSetupID;

                $scope.lstCountList = response.data.ListMachineSet[0].Count;
                $("#CountList").select2("data", { id: response.data.ListMachineSet[0].Count, text: response.data.ListMachineSet[0].Count });

                $scope.Jog = response.data.ListMachineSet[0].Jog;
                $scope.RFront = response.data.ListMachineSet[0].RFront;

                $scope.RRear = response.data.ListMachineSet[0].RRear;
                $scope.AGM = response.data.ListMachineSet[0].Agm;
                $scope.Empty = response.data.ListMachineSet[0].Empty;
                $scope.Speed = response.data.ListMachineSet[0].Speed;

                $scope.IsHidden = true;
                //$scope.IsSaveShow = true;
                //$scope.btnSaveText = "Update";
                //$scope.btnShowText = "Show List";
                $scope.IsfrmShow = true;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //************************************************End Selected Item Data (Single)**************************************** 

        //*************-------Check Duplicate Bill No---------***************    
        $scope.loadCountToCheckDuplicate = function (isPaging) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = $scope.lstCountList;
            var MessageData = "";
            var ListCount = "";
            var apiRoute = baseUrl + 'GetCountToCheck/';
            ModelsArray = [objcmnParam];
            var ListCountCheck = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListCountCheck.then(function (response) {
                ListCount = response.data.ListCountExist.length;
                debugger
                if (ListCount > 0) {
                    MessageData = $scope.lstCountList;
                    $scope.lstCountList = "";
                    $scope.loadCountRecords(0);
                    $("#CountList").select2("data", { id: 0, text: "--Select Count--" });
                    Command: toastr["warning"]("Your Inputed Count: " + MessageData + " is already exist!");
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //*************************------End------***************************

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
                $scope.loadMachineSetupRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadMachineSetupRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadMachineSetupRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadMachineSetupRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadMachineSetupRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadMachineSetupRecords = function (isPaging) {
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
            $scope.gridOptionsMSetup = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "BallMachineSetupID", displayName: "BallMachineSetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Count", displayName: "Count", title: "Count", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Jog", displayName: "Jog", title: "Jog", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RFront", displayName: "R. Front", title: "R. Front", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RRear", displayName: "R. Rear", title: "R. Rear", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Agm", displayName: "AGM", title: "AGM", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Empty", displayName: "Empty", title: "Empty", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Speed", displayName: "Speed", title: "Speed", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsMSetup.useExternalPagination = false;
                        $scope.gridOptionsMSetup.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetMachinSetup/';
            ModelsArray = [objcmnParam];
            var listSetMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMSetup.data = response.data.ListMachineSet;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadMachineSetupRecords(0);
        }
        $scope.RefreshMasterList();
        //***************************************************End Set Master Dynamic Grid******************************************************

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
            //$scope.IsShow = $scope.IsShow ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                //$scope.IsfrmShow = true;
                //$scope.IsSaveShow = true;

            }
            else {
                //$scope.btnShowText = "Create";
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                //$scope.IsSaveShow = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.Save = function () {
            debugger
            //*************----Master Data---**************
            //MessageData = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            var MachineSetup = {
                LUserID: $scope.UserCommonEntity.loggedUserID,
                LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                LMenuID: $scope.UserCommonEntity.currentMenuID,
                LTransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,

                BallMachineSetupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.BallMachineSetupID,
                Count: $scope.lstCountList,
                Jog: $scope.Jog,
                RFront: $scope.RFront,
                RRear: $scope.RRear,
                Agm: $scope.AGM,
                Empty: $scope.Empty,
                Speed: $scope.Speed
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateCountWiseMachineSetup/';
            ModelsArray = [MachineSetup];
            var CountWiseMachineSetupCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            CountWiseMachineSetupCreateUpdate.then(function (response) {
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
        $scope.DeleteMachineSetup = function (delModel) {
            var DeletedData = {
                LUserID: $scope.UserCommonEntity.loggedUserID,
                LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                BallMachineSetupID: delModel.BallMachineSetupID,
                IsDeleted: true
            }

            var apiRoute = baseUrl + 'SaveUpdateCountWiseMachineSetup/';
            ModelsArray = [DeletedData];
            var MachineSetupDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            MachineSetupDelete.then(function (response) {
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
            $scope.frmMachineSetupEntry.$setPristine();
            $scope.frmMachineSetupEntry.$setUntouched();
            $scope.Jog = "";
            $scope.RFront = "";
            $scope.RRear = "";
            $scope.AGM = "";
            $scope.Empty = "";
            $scope.Speed = "";

            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            //$scope.IsSaveShow = true;
            $scope.IsDeleted = false;

            $scope.gridOptionsMSetup.data = [];
            $scope.lstCountList = "";
            $("#CountList").select2("data", { id: 0, text: "--Select Count--" });
        };
        //**************************************************************End Reset/Clear**********************************************************
    }]);