/*
    DFSMachineConfigurationCtrl.js
*/
app.controller('DFSMachineConfigurationCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
    //**************************************************Start Vairiable Initialize**************************************************
    var baseUrl = '/Production/api/DFSMachineConfiguration/';
    $scope.permissionPageVisibility = true;
    $scope.UserCommonEntity = {};
    $scope.HeaderToken = {};
    objcmnParam = {};
    $scope.gridOptionsDFSMaster = [];
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;
    $scope.ListDFSMachineConfigDetails = [];
    //$scope.btnSaveText = "Save";
    //$scope.btnShowList = "Show List"
    $scope.PageTitle = 'Machine Configuration';
    $scope.ListTitle = 'Machine Configuration List';
    $scope.MachineConfigID = 0;
    //$scope.IsSaveDisable = true;
    $scope.IsHidden = true;
    $scope.IsShow = false;
    $scope.IsfrmShow = true;
    $scope.IsShowDetail = true;
    //$scope.IsShowSave = true;
    message = '';
    //***************************************************End Vairiable Initialize***************************************************

    //***************************************************Start Common Task for all**************************************************
    frmName = ''; DelFunc = 'DeleteUpdateWeavingMachineConfig'; DelMsg = 'MachineConfigNo'; EditFunc = 'getDFSMachineConfigMasterByID, AddItemToList';
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
    $scope.ResetDetail = function () {
        $scope.ListDFSMachineConfigDetails = [];
        $scope.cmnbtnShowHideEnDisable('true');
        $scope.IsShow = false;
    }

    //-------------- Sizing Machine -------------------
    $scope.GetMachines = function () {
        debugger
        $scope.cmnParam();
        objcmnParam.ItemGroup = 40;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetMachines/';
        var _machine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        _machine.then(function (response) {
            $scope.Machines = response.data.ListMachine;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetMachines();

    //********************************************************Start Row Add in Detail Grid***********************************************        
    $scope.AddItemToList = function () {
        debugger
        $scope.ListDFSMachineConfigDetails.push({
            MachineConfigDetailID: 0, MachineConfigID: 0, MachineID: 0, Description: ""
        });
        $scope.cmnbtnShowHideEnDisable('false');
        $scope.IsShow = true;
    }
    //**********************************************************End Row Add in Detail Grid***********************************************

    //***********************************************Start Load ddl Chemical from cmnItemmaster**************************************        
    $scope.AddItemToList = function (dataModel) {
        debugger
        $scope.cmnParam();
        objcmnParam.id = angular.isUndefined(dataModel) ? 0 : dataModel.MachineConfigID;
        if (objcmnParam.id != 0) {
            $scope.ListDFSMachineConfigDetails = [];
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingMachineConfigById/';
            var listDFSDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDFSDetail.then(function (response) {
                $scope.ListDFSMachineConfigDetails = response.data.MConfigByID;
                $scope.IsShow = $scope.ListDFSMachineConfigDetails.length > 0 ? true : false;
                //$scope.IsSaveDisable = $scope.ListDFSMachineConfigDetails.length > 0 ? false : true;
                if ($scope.ListDFSMachineConfigDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            debugger
            //$scope.AutoSlNo = $scope.AutoSlNo + 1;
            $scope.ListDFSMachineConfigDetails.push({
                MachineConfigDetailID: 0, MachineConfigID: 0, MachineID: 0, Description: ""
            })
            $scope.IsShow = $scope.ListDFSMachineConfigDetails.length > 0 ? true : false;
            //$scope.IsSaveDisable = $scope.ListDFSMachineConfigDetails.length > 0 ? false : true;
            if ($scope.ListDFSMachineConfigDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        }
    };
    //************************************************End Load ddl Chemical from cmnItemmaster***************************************
    //***********************************************************End Add ToList******************************************************

    //*****************************************Start Delete Item From List**************************************
    $scope.deleteRow = function (index) {
        $scope.ListDFSMachineConfigDetails.splice(index, 1);
        $scope.IsShow = $scope.ListDFSMachineConfigDetails.length > 0 ? true : false;
        //$scope.IsSaveDisable = $scope.ListDFSMachineConfigDetails.length > 0 ? false : true;
        if ($scope.ListDFSMachineConfigDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
    };
    //*****************************************End Delete Item From List**************************************

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
            $scope.LoadDFSMachineConfigurations(1);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.LoadDFSMachineConfigurations(1);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPages()) {
                this.pageNumber++;
                $scope.LoadDFSMachineConfigurations(1);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.LoadDFSMachineConfigurations(1);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPages();
                $scope.LoadDFSMachineConfigurations(1);
            }
        }
    };

    //ui-Grid Call
    $scope.LoadDFSMachineConfigurations = function (isPaging) {
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
        $scope.gridOptionsDFSMaster = {
            //useExternalPagination: true,
            //useExternalSorting: true,
            //enableFiltering: true,
            //enableRowSelection: true,
            //enableSelectAll: true,
            //showFooter: true,
            //enableGridMenu: true,
            rowTemplate: $scope.UserCommonEntity.rowTemplate,
            columnDefs: [
                { name: "MachineConfigID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "OrganogramName", displayName: "Department Name", headerCellClass: $scope.highlightFilteredHeader },
                { name: "MachineConfigNo", displayName: "Machine Config No", headerCellClass: $scope.highlightFilteredHeader },
                { name: "Remarks", displayName: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
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
            enableFiltering: true,
            enableGridMenu: true,
            enableSelectAll: true,
            exporterCsvFilename: 'FinishGoodFile.csv',
            exporterPdfDefaultStyle: { fontSize: 9 },
            exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "Item Groups", style: 'headerStyle' },
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
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetWeavingMachineConfigMaster/';
        var _finishGoods = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        _finishGoods.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsDFSMaster.data = response.data.weavingMachines;
            $scope.loaderMore = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.RefreshMasterList = function () {
        $scope.pagination.pageNumber = 1;
        $scope.LoadDFSMachineConfigurations(0);
    }
    $scope.RefreshMasterList();

    $scope.getDFSMachineConfigMasterByID = function (model) {
        //$scope.btnSaveText = "Update";
        //$scope.btnShowList = "Show List"
        //$scope.IsSaveDisable = false;
        $scope.IsHidden = true;
        //$scope.IsShow = true;
        $scope.IsfrmShow = true;
        $scope.IsShowDetail = true;
        //$scope.IsShowSave = true;

        $scope.MachineConfigID = model.MachineConfigID;
        $scope.MachineConfigNo = model.MachineConfigNo;
        $scope.txtbxRemark = model.Remarks;
        $scope.lstDeptList = model.DepartmentID;
        $("#DeptList").select2("data", { id: model.DepartmentID, text: model.OrganogramName });
    }

    $scope.ShowHide = function () {
        $scope.IsHidden = $scope.IsHidden ? false : true;
        if ($scope.IsHidden == true) {
            $scope.clear();
        }
        else {
            $scope.RefreshMasterList();
            $scope.IsfrmShow = false;
            $scope.IsShowDetail = false;
            $scope.IsShow = false;
            //$scope.IsShowSave = false;
        }
    }

    //*********************************************************Start Save/Update/Delete**********************************************************
    $scope.Save = function () {
        //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
        $scope.cmnParam();
        var DFSMachinConf = {
            MachineConfigID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.MachineConfigID,
            MachineConfigNo: $scope.MachineConfigNo,
            DepartmentID: $scope.lstDeptList,
            Remarks: $scope.txtbxRemark
        }

        if ($scope.ListDFSMachineConfigDetails.length == 0) {
            Command: toastr["warning"]("Please Generate at least 1 row");
            return;
        }

        var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
        ModelsArray = [DFSMachinConf, $scope.ListDFSMachineConfigDetails, objcmnParam];
        var apiRoute = baseUrl + 'SaveUpdateWeavingMasterDetail/';
        var MachineConfigSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
        MachineConfigSaveUpdate.then(function (response) {
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
    });

    }


    $scope.DeleteUpdateWeavingMachineConfig = function (delModel) {
        $scope.cmnParam(); objcmnParam.id = delModel.MachineConfigID;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'DeleteUpdateWeavingMasterDetail/';
        var ConfigDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
        ConfigDelete.then(function (response) {
            if (response.data.result != '') {
                //$scope.clear();
                $scope.RefreshMasterList();
                Command: toastr["success"](delModel.MachineConfigNo + " has been Deleted Successfully!!!!");
            }
            else {
                Command: toastr["warning"](delModel.MachineConfigNo + " Not Deleted, Please Check and Try Again!");
            }
        }, function (error) {
            Command: toastr["warning"](delModel.MachineConfigNo + " Not Deleted, Please Check and Try Again!");
            console.log("Error: " + error);
        });
    };
    //*********************************************************End Save/Update/Delete**********************************************************

    //*******************************************************************Start Reset***********************************************************
    $scope.clear = function () {
        $scope.frmDFSMachineConfiguration.$setPristine();
        $scope.frmDFSMachineConfiguration.$setUntouched();
        $scope.ListDFSMachineConfigDetails = [];
        $scope.gridOptionsDFSMaster.data = [];
        $scope.MachineConfigNo = "";
        $scope.txtbxRemark = "";
        //$scope.btnSaveText = "Save";
        //$scope.btnShowList = "Show List"
        //$scope.IsSaveDisable = true;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsfrmShow = true;
        $scope.IsShowDetail = true;
        //$scope.IsShowSave = true;
        $scope.loadDepartmentRecords(0);
    };
    //********************************************************************End Reset************************************************************
}]);