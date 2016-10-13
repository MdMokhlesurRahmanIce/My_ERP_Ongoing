/*
    WeavingMachineConfigurtion.js
*/
app.controller('WeavingMachineConfigurtion', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
    //**************************************************Start Vairiable Initialize**************************************************
    var baseUrl = '/Production/api/WeavingMachineConfigurtion/';
    $scope.permissionPageVisibility = true;
    $scope.UserCommonEntity = {};
    $scope.HeaderToken = {};
    objcmnParam = {};
    $scope.gridOptionsFg = [];
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;

    //$scope.btnSaveUpdateText = "Save";
    //$scope.btnShowHide = "Show List"
    $scope.PageTitle = 'Machine Configuration';
    $scope.ListTitle = 'Machine Configuration List';
    $scope.MachineConfigID = 0;
    $scope.IsHidden = true;
    $scope.IsShow = true;
    //$scope.IsShowSave = true;
    message = '';
    lineName = '';
    //***************************************************End Vairiable Initialize***************************************************

    //***************************************************Start Common Task for all**************************************************
    frmName = 'frmMachineConfiguration'; DelFunc = 'DeleteUpdateWeavingMachineConfig'; DelMsg = 'MachineConfigNo'; EditFunc = 'getWeavingMachineConfigurationInfo';
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

    $scope.loadLineRecords = function (DeptListID) {
        debugger
        $scope.cmnParam();
        objcmnParam.ItemType = angular.isUndefined(DeptListID) || DeptListID == "" || DeptListID == 0 || DeptListID == null ? $scope.UserCommonEntity.loggedUserDepartmentID : DeptListID;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetLines/';
        var listLine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        listLine.then(function (response) {
            $scope.listLine = response.data.LineList;
            if (lineName != '') {                
                $("#ddlLineList").select2("data", { id: 0, text: lineName });
                lineName = '';
            }
            else
            {
                $("#ddlLineList").select2("data", { id: 0, text: '--Select Line--' });
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.loadLineRecords(0);

    //-------------- Sizing Machine -------------------
    $scope.GetMachines = function () {
        debugger
        $scope.cmnParam();
        objcmnParam.ItemType = 4;
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
            $scope.LoadWeavingMachineConfigurations(1);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.LoadWeavingMachineConfigurations(1);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPages()) {
                this.pageNumber++;
                $scope.LoadWeavingMachineConfigurations(1);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.LoadWeavingMachineConfigurations(1);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPages();
                $scope.LoadWeavingMachineConfigurations(1);
            }
        }
    };

    //ui-Grid Call
    $scope.LoadWeavingMachineConfigurations = function (isPaging) {
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
        $scope.gridOptionsFg = {
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
                { name: "MachineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "OrganogramName", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "LineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "MachineConfigNo", displayName: "Machine Config No", headerCellClass: $scope.highlightFilteredHeader },
                { name: "LineName", displayName: "Line Name", headerCellClass: $scope.highlightFilteredHeader },
                { name: "ItemName", displayName: "Machine Name", headerCellClass: $scope.highlightFilteredHeader },
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
        var apiRoute = baseUrl + 'GetWeavingMachineConfigurations/';
        var _finishGoods = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        _finishGoods.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsFg.data = response.data.weavingMachines;
            $scope.loaderMore = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.RefreshMasterList = function () {
        $scope.pagination.pageNumber = 1;
        $scope.LoadWeavingMachineConfigurations(0);
    }
    $scope.RefreshMasterList();

    $scope.getWeavingMachineConfigurationInfo = function (model) {
        //$scope.btnSaveUpdateText = "Update";
        //$scope.btnShowHide = "Show List"
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        $scope.loadLineRecords(model.DepartmentID);

        $scope.MachineConfigID = model.MachineConfigID;
        $scope.MachineConfigNo = model.MachineConfigNo;
        $scope.txtbxRemark = model.Remarks;
        $scope.lstDeptList = model.DepartmentID;
        $("#DeptList").select2("data", { id: model.DepartmentID, text: model.OrganogramName });
        $scope.drpdwnMachine = model.MachineID;
        $("#drpdwnMachine").select2("data", { id: model.MachineID, text: model.ItemName });
        $scope.LineID = model.LineID;
        lineName= model.LineName;
    }

    $scope.ShowHide = function () {
        $scope.IsHidden = $scope.IsHidden ? false : true;
        if ($scope.IsHidden == true) {
            $scope.clear();
            //$scope.btnShowHide = "Show List";
            //$scope.IsShow = true;
            //$scope.IsShowSave = true;
        }
        else {
            $scope.RefreshMasterList();
            //$scope.btnShowHide = "Create";
            //$scope.pagination.pageNumber = 1;
            //$scope.LoadWeavingMachineConfigurations(0);
            $scope.IsShow = false;
            //$scope.IsShowSave = false;
        }
    }

    //*********************************************************Start Save/Update/Delete**********************************************************
    $scope.Save = function () {
        //message = $scope.btnSaveUpdateText == "Save" ? "Saved" : "Updated";
        $scope.cmnParam();
        var WeavingMachinConf = {
            MachineConfigID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.MachineConfigID,
            MachineConfigNo: $scope.MachineConfigNo,
            DepartmentID: $scope.lstDeptList,
            MachineID: $scope.drpdwnMachine,
            LineID: $scope.LineID,
            Remarks: $scope.txtbxRemark
        }
        var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
        ModelsArray = [WeavingMachinConf, objcmnParam];
        var apiRoute = baseUrl + 'SaveWeavingMachineConfi/';
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
        var apiRoute = baseUrl + 'DeleteWeavingMachineConfig/';
        var ConfigDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
        ConfigDelete.then(function (response) {
            if (response.data.result != '') {
                $scope.clear();
                Command: toastr["success"]("BWS Name " + delModel.MachineConfigNo + " has been Deleted Successfully!!!!");
            }
            else {
                Command: toastr["warning"]("BWS Name " + delModel.MachineConfigNo + " Not Deleted, Please Check and Try Again!");
            }
        }, function (error) {
            Command: toastr["warning"]("BWS Name " + delModel.MachineConfigNo + " Not Deleted, Please Check and Try Again!");
            console.log("Error: " + error);
        });
    };
    //*********************************************************End Save/Update/Delete**********************************************************

    //*******************************************************************Start Reset***********************************************************
    $scope.clear = function () {
        $scope.frmMachineConfiguration.$setPristine();
        $scope.frmMachineConfiguration.$setUntouched();
        $scope.gridOptionsFg.data = [];
        $scope.MachineConfigNo = "";
        $scope.drpdwnMachine="";
        $scope.LineID = "";
        $scope.txtbxRemark = "";
        //$scope.btnSaveUpdateText = "Save";
        //$scope.btnShowHide = "Show List"
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        $scope.loadDepartmentRecords(0);
        $scope.loadLineRecords(0);
        $("#drpdwnMachine").select2("data", { id: 0, text: '--Select Machine--' });
        $("#ddlLineList").select2("data", { id: 0, text: '--Select Line--' });
    };
    //********************************************************************End Reset************************************************************
}]);