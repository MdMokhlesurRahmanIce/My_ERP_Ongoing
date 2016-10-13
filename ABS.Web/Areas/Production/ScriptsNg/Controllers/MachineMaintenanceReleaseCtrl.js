/**
 * MachineMaintenanceReleaseCtrl.js
 */
app.controller('MachineMaintenanceReleaseCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/MachineMaintenanceRelease/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsMCMntMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        //$scope.btnSaveText = "Save";
        //$scope.btnShowList = "Show List";
        $scope.PageTitle = 'Maintenance Release Entry';
        $scope.ListTitleMaster = 'Maintenance Release List';
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        $scope.ReleaseDate = conversion.NowDateCustom();
        MaintenanceNo = '';
        $scope.MachineConfigID = "";
        $scope.DepartmentID = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmMachineMaintenanceReleaseEntry'; DelFunc = 'DeleteMMRList'; DelMsg = 'MaintenanceNo'; EditFunc = 'getMachineMaintenanceByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadMaintenanceRecords = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.IsTrue = MaintenanceNo == "" ? true : false;
            var apiRoute = baseUrl + 'GetMaintenanceMachine/';
            ModelsArray = [objcmnParam];
            var ListMaintenance = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMaintenance.then(function (response) {
                debugger
                $scope.ListMaintenance = response.data.ListMaintenance;
                MaintenanceNo = '';
                //if (MaintenanceNo != "")
                //{
                //    $("#ddlMaintenance").select2("data", { id: 0, text: MaintenanceNo });
                //    MaintenanceNo = "";
                //}
                //else
                //{
                //    $("#ddlMaintenance").select2("data", { id: 0, text: '--Select Maintenance No--' });
                //}
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadMaintenanceRecords();
        //**************************************************End Machine Dropdown******************************************************

        //************************************************Start Machine Data******************************************************
        $scope.ChangeMaintenanceRecords = function (MaintenanceID) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = MaintenanceID;
            var apiRoute = baseUrl + 'GetMaintenanceMachineData/';
            ModelsArray = [objcmnParam];
            var ListMaintenance = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMaintenance.then(function (response) {
                debugger
                $scope.DepartmentID = '';
                $scope.DepartmentName = '';
                $scope.MachineConfigID = '';
                $scope.MachineConfigNo = '';

                $scope.DepartmentID = response.data.MData.DepartmentID;
                $scope.DepartmentName = response.data.MData.OrganogramName;
                $scope.MachineConfigID = response.data.MData.MachineConfigID;
                $scope.MachineConfigNo = response.data.MData.MachineConfigNo;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************************************End Machine Data******************************************************

        //Pagination
        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
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
                $scope.loadAllMachineMaintenanceRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllMachineMaintenanceRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllMachineMaintenanceRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllMachineMaintenanceRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllMachineMaintenanceRecords(1);
                }
            }
        };

        $scope.loadAllMachineMaintenanceRecords = function (isPaging) {
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize;
            objcmnParam.pageSize = $scope.pagination.pageSize;
            objcmnParam.IsPaging = isPaging;
            objcmnParam.IsTrue = false;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsMCMntMaster = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "MaintenanceID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineConfigID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MaintenanceNo", displayName: "Maintenance No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineConfigNo", displayName: "MachineConfig No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OrganogramName", displayName: "Department", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ReleaseDate", displayName: "Release Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ReleaseRemarks", displayName: "Release Remarks", title: "Release Remarks", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        width: '13%',
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsMCMntMaster.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsMCMntMaster.useExternalPagination = false;
                        $scope.gridOptionsMCMntMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetMntMachineMaintenanceOrde/';
            var listMMO = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listMMO.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMCMntMaster.data = response.data.MMMOList;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllMachineMaintenanceRecords(0);
        }
        $scope.RefreshMasterList();

        $scope.getMachineMaintenanceByID = function (dataModel) {
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            //$scope.btnSaveText = "Update";
            //$scope.btnShowList = "Show List";

            MaintenanceNo = dataModel.MaintenanceNo;
            $scope.loadMaintenanceRecords(0);
            $scope.MaintenanceID = dataModel.MaintenanceID;
            $("#MaintenanceList").select2("data", { id: dataModel.MaintenanceID, text: dataModel.MaintenanceNo });
            $scope.MachineConfigID = dataModel.MachineConfigID;
            $scope.DepartmentID = dataModel.DepartmentID;
            $scope.DepartmentName = dataModel.OrganogramName;
            $scope.ReleaseRemarks = dataModel.ReleaseRemarks;
            $scope.ReleaseDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.ReleaseDate);
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                //$scope.btnShowList = "Create";
                //$scope.pagination.pageNumber = 1;
                //$scope.loadAllMachineMaintenanceRecords(0);
                //$scope.IsShowSave = false;
                $scope.IsShow = false;
            }
        }

        $scope.Save = function () {
            //message = $scope.btnDefectTypeSaveText == "Save" ? "Saved" : "Updated";
            var DefectTypeInfo = {
                MaintenanceID: $scope.UserCommonEntity.message == 'Saved' ? 0 : $scope.MaintenanceID,
                MachineConfigID: $scope.MachineConfigID,
                ReleaseRemarks: $scope.ReleaseRemarks,
                ReleaseDate: $scope.ReleaseDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ReleaseDate)
            };
            $scope.cmnParam();
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [DefectTypeInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateMachineMaintenanceRelease/';
            var MMRSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            MMRSaveUpdate.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
        function (error) {
            console.log("Error: " + error);
            Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
        });
        };

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteMMRList = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.MaintenanceID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateMachineMaintenanceOrder/';
            var delDefectList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDefectList.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"](dataModel.MaintenanceNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.MaintenanceNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.MaintenanceNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmMachineMaintenanceReleaseEntry.$setPristine();
            $scope.frmMachineMaintenanceReleaseEntry.$setUntouched();
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.MaintenanceID = "";
            $scope.MachineConfigID = "";
            $scope.MachineConfigNo = "";
            $scope.DepartmentID = "";
            $scope.DepartmentName = "";
            $scope.ReleaseDate = conversion.NowDateCustom();
            $scope.ReleaseRemarks = '';
            //$scope.btnSaveText = "Save";
            //$scope.btnShowList = "Show List";
            $scope.loadMaintenanceRecords(0);
            $("#MaintenanceList").select2("data", { id: 0, text: '--Select Maintenance No--' });
        };

    }]);
