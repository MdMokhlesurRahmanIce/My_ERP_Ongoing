/**
 * MachineMaintenanceOrderCtrl.js
 */
app.controller('MachineMaintenanceOrderCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/MachineMaintenanceOrder/';
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
        $scope.PageTitle = 'Maintenance Order Entry';
        $scope.ListTitleMaster = 'Maintenance Order List';
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        $scope.MaintenanceDate = conversion.NowDateCustom();
        MachineName = '';
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmMachineMaintenanceOrderEntry'; DelFunc = 'DeleteMMOList'; DelMsg = 'MaintenanceNo'; EditFunc = 'getMachineMaintenanceByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //****************************************************Start Load Department ddl*************************************************
        $scope.loadDepartmentRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDepartment/';
            var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDept.then(function (response) {
                debugger
                $scope.listDept = response.data.ListDept;
                angular.forEach(response.data.ListDept, function (dept) {
                    if ($scope.UserCommonEntity.loggedUserDepartmentID == dept.OrganogramID) {
                        $scope.lstDeptList = dept.OrganogramID;
                        $("#DeptList").select2("data", { id: 0, text: dept.OrganogramName });
                    }
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadDepartmentRecords(0);
        //****************************************************Start Load Department ddl************************************************* 

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadMachineRecords = function (DeptListID) {
            debugger
            $scope.cmnParam();
            objcmnParam.IsTrue = MachineName == "" ? false : true;
            objcmnParam.DepartmentID = angular.isUndefined(DeptListID) || DeptListID == "" || DeptListID == 0 || DeptListID == null ? $scope.UserCommonEntity.loggedUserDepartmentID : DeptListID;
            var apiRoute = baseUrl + 'GetMachine/';
            ModelsArray = [objcmnParam];
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.ListMachine = response.data.ListMachine;
                if (MachineName != "") {
                    $("#ddlMachine").select2("data", { id: 0, text: MachineName });
                    MachineName = "";
                }
                else {
                    $("#ddlMachine").select2("data", { id: 0, text: '--Select Machine--' });
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************************************End Machine Dropdown******************************************************


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
            objcmnParam.IsTrue = true;

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
                    { name: "MaintenanceDate", displayName: "Maintenance Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Reason", displayName: "Reason", title: "Reason", headerCellClass: $scope.highlightFilteredHeader },
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

            MachineName = dataModel.MachineConfigNo;
            $scope.loadMachineRecords(dataModel.DepartmentID);

            $scope.MaintenanceNo = dataModel.MaintenanceNo;
            $scope.MaintenanceID = dataModel.MaintenanceID;
            $scope.MachineID = dataModel.MachineConfigID;
            $scope.lstDeptList = dataModel.DepartmentID;
            $("#DeptList").select2("data", { id: dataModel.DepartmentID, text: dataModel.OrganogramName });
            $scope.Reason = dataModel.Reason;
            $scope.MaintenanceDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.MaintenanceDate);
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                //$scope.btnShowList = "Create";
                $scope.RefreshMasterList();
                //$scope.IsShowSave = false;
                $scope.IsShow = false;
            }
        }

        $scope.Save = function () {
            //message = $scope.btnDefectTypeSaveText == "Save" ? "Saved" : "Updated";
            var DefectTypeInfo = {
                MaintenanceID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.MaintenanceID,
                MachineConfigID: $scope.MachineID,
                DepartmentID: $scope.lstDeptList,
                Reason: $scope.Reason,
                MaintenanceDate: $scope.MaintenanceDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.MaintenanceDate)
            };
            $scope.cmnParam();
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [DefectTypeInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateMachineMaintenanceOrder/';
            var MMOSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            MMOSaveUpdate.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    $scope.MaintenanceNo = response.data;
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
        $scope.DeleteMMOList = function (dataModel) {
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
            $scope.frmMachineMaintenanceOrderEntry.$setPristine();
            $scope.frmMachineMaintenanceOrderEntry.$setUntouched();
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.MachineID = "";
            $scope.MaintenanceDate = conversion.NowDateCustom();
            $scope.MaintenanceNo = '';
            $scope.Reason = '';
            //$scope.btnSaveText = "Save";
            //$scope.btnShowList = "Show List";
            $scope.loadDepartmentRecords(0);
            $scope.loadMachineRecords(0);
        };

    }]);
