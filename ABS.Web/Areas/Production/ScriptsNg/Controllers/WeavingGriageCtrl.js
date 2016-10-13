/*
*    Created By: Shamim Uddin;
*    Create Date: 23-6-2016 (dd-mm-yy); Updated Date: 0-0-2000 (dd-mm-yy);
     Updated By: Md. Jahangir;
     Updated On: 21-08-2016 (dd-mm-yy);
*    Name: 'WeavingGriageCtrl';
*    Type: $scope;
*    Purpose: This Controller Use for Production Receive with know about  Dropinng  ;
*    Service Injected: '$scope', 'conversion', 'WeavingGriageService','localStorage';
*/

app.controller('WeavingGriageCtrl', ['$scope', 'crudService', '$localStorage', 'conversion', 'uiGridConstants',
function ($scope, crudService, $localStorage, conversion, uiGridConstants) {
    var baseUrl = '/Production/api/WeavingGriageReceive/';
    $scope.permissionPageVisibility = true;
    $scope.UserCommonEntity = {};
    $scope.HeaderToken = {};
    objcmnParam = {};
    $scope.gridOptionsWGMaster = [];
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;

    $scope.PageTitle = 'Weaving Griage Delivery';
    $scope.ListTitle = 'Weaving Griage Delivery List';

    $scope.isIssued = false;
    $scope.WeavingMRRID = 0;
    $scope.drpIssueTo = "Finishing";
    $scope.txtbxDate = conversion.NowDateCustom();

    //***************************************************Start Common Task for all**************************************************
    frmName = 'frmWeavingGraiageReceive'; DelFunc = 'delete'; DelMsg = 'WeavingMRRNo'; EditFunc = 'getWeavingGraiageById';
    $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
    $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
    $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
    $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
    $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
    $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
    //****************************************************End Common Task for all*************************************************** 

    $scope.Machines = function () {
        $scope.cmnParam();
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetMachine/';
        var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        ListMachine.then(function (response) {
            debugger
            $scope.ListMachine = response.data.ListMachine;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.Machines()

    $scope.getWeavingMachineByIdMCID = function () {
        $scope.cmnParam();
        objcmnParam.id = $scope.drpMachine;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetWeavingMachineDetailByID/';
        var _Machine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        _Machine.then(function (response) {
            if (response.data != null) {
                $scope.txtbxArtical = response.data.ArticleNo;
                $scope.txtbxSetNo = response.data.SetNO;
                $scope.DoffingNo = response.data.DoffingNo;
                $scope.txtbxUnit = response.data.Unit;
                $scope.MachineConfigID = response.data.MachineConfigID;
                $scope.ItemID = response.data.ItemID;
                $scope.SetID = response.data.SetID;
                $scope.SizeMRRID = response.data.SizeMRRID;
                $scope.UnitID = response.data.UnitID;
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.Shift = function () {
        $scope.cmnParam();
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetShifts/';
        var listShifts = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        listShifts.then(function (response) {
            $scope.listShifts = response.data.ShiftList;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.Shift();

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
            $scope.WeavingGriageDetails(1);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.WeavingGriageDetails(1);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPages()) {
                this.pageNumber++;
                $scope.WeavingGriageDetails(1);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.WeavingGriageDetails(1);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPages();
                $scope.WeavingGriageDetails(1);
            }
        }
    };

    $scope.WeavingGriageDetails = function (isPaging) {
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
        $scope.gridOptionsWGMaster = {
            rowTemplate: $scope.UserCommonEntity.rowTemplate,
            columnDefs: [
                { name: "WeavingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "WeavingMRRNo", displayName: "Weaving MRR No", headerCellClass: $scope.highlightFilteredHeader },
                { name: "MachineConfigNo", displayName: "Machine", headerCellClass: $scope.highlightFilteredHeader },
                { name: "ArticleNo", displayName: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                { name: "SetNO", displayName: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                 { name: "DoffingNo", displayName: "Doffing No", headerCellClass: $scope.highlightFilteredHeader },
                  { name: "Unit", displayName: "Unit", headerCellClass: $scope.highlightFilteredHeader },
                   { name: "WeavingMRRDate", displayName: "Weaving MRR Date", headerCellClass: $scope.highlightFilteredHeader },
                   { name: "ShiftName", displayName: "Shift", headerCellClass: $scope.highlightFilteredHeader },
                   { name: "Griege", displayName: "Griege", headerCellClass: $scope.highlightFilteredHeader },
                   { name: "OperatorName", displayName: "Operator", headerCellClass: $scope.highlightFilteredHeader },
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
        var apiRoute = baseUrl + 'WeavingGriageDetails/';
        var _finishGoods = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        _finishGoods.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsWGMaster.data = response.data.WeavingGrages;
            $scope.loaderMore = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.RefreshMasterList = function () {
        $scope.pagination.pageNumber = 1;
        $scope.WeavingGriageDetails(0);
    };
    $scope.RefreshMasterList();

    $scope.ShowHide = function () {
        $scope.IsHidden = $scope.IsHidden ? false : true;
        if ($scope.IsHidden == true) {
            $scope.clear();
        }
        else {
            $scope.RefreshMasterList();
        }
    }

    $scope.Operators = function () {
        $scope.cmnParam();
        objcmnParam.ItemType = 1;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetOperators/';
        var listOperator = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        listOperator.then(function (response) {
            $scope.listOperator = response.data.OperatorList;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.Operators();

    $scope.Departments = [{ ID: $scope.UserCommonEntity.loggedCompnyID == 1 ? 12 : 61, Name: 'Finishing' }];

    $scope.DefultSetIssueTo = function () {
        $scope.drpIssueTo = $scope.Departments[0].ID;
        $("#drpIssueTo").select2("data", { id: $scope.Departments[0].ID, text: $scope.Departments[0].Name });
    }
    $scope.DefultSetIssueTo();

    $scope.Save = function () {
        $scope.cmnParam();
        var PrdWeavingMRRMaster = {
            WeavingMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.WeavingMRRID,
            DoffingNo: $scope.DoffingNo,
            MachineConfigID: $scope.drpMachine,
            ItemID: $scope.ItemID,
            SetID: $scope.SetID,
            SizeMRRID: $scope.SizeMRRID,
            UnitID: $scope.UnitID,
            ShiftID: $scope.drpShift,
            OperatorID: $scope.drpOperator,
            Qty: $scope.txtbxGriege,
            Remarks: $scope.txtbxRemark,
            IssuedDepartmentID: $scope.drpIssueTo,
            WeavingMRRDate: $scope.txtbxDate === "" ? null : conversion.getStringToDate($scope.txtbxDate)
        };
        var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
        ModelsArray = [PrdWeavingMRRMaster, objcmnParam];
        var apiRoute = baseUrl + 'SaveWeavingGriage/';
        var GriagSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
        GriagSaveUpdate.then(function (response) {
            if (response == 1) {
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

    //*******************************************************Start Delete Master Detail**************************************************
    $scope.delete = function (dataModel) {
        $scope.cmnParam();
        objcmnParam.id = dataModel.WeavingMRRID;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'DeleteWeavingGriageById/';
        var singleMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
        singleMachine.then(function (response) {
            if (response.data == 1) {
                $scope.RefreshMasterList();
                Command: toastr["success"](dataModel.WeavingMRRNo + " has been Deleted Successfully!!!!");
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.getWeavingGraiageById = function (model) {
        $scope.cmnParam();
        objcmnParam.id = model.WeavingMRRID;
        ModelsArray = [objcmnParam];
        var apiRoute = baseUrl + 'GetWeavingGriageDetailsById/';
        var singleMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        singleMachine.then(function (response) {
            $scope.WeavingMRRID = response.data.WeavingMRRID;
            $scope.txtbxRemark = response.data.Remarks;
            $scope.txtbxGriege = response.data.Griege;
            $scope.txtbxDate = response.data.WeavingMRRDate;
            $scope.DoffingNo = response.data.DoffingNo;
            $scope.txtbxArtical = response.data.ArticleNo;
            $scope.txtbxUnit = response.data.Unit;
            $scope.txtbxSetNo = response.data.SetNO;
            $scope.ItemID = response.data.ItemID;
            $scope.SetID = response.data.SetID;
            $scope.SizeMRRID = response.data.SizeMRRID;
            $scope.UnitID = response.data.UnitID;
            $scope.SetDrpDown(response);
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.SetDrpDown = function (response) {
        debugger;
        $scope.drpMachine = response.data.MachineConfigID;
        if (response.data.MachineConfigNo != 'N/A') {
            $("#drpMachine").select2("data", { id: 0, text: response.data.MachineConfigNo });
        } else {
            $("#drpMachine").select2("data", { id: 0, text: '-- Select Machine --' });
        }

        //$scope.getWeavingMachineByIdMCID();

        $scope.drpOperator = response.data.OperatorID;
        if (response.data.OperatorName != 'N/A') {
            $("#drpOperator").select2("data", { id: 0, text: response.data.OperatorName });
        } else {
            $("#drpOperator").select2("data", { id: 0, text: '-- Select Operator --' });
        }

        $scope.drpShift = response.data.ShiftID;
        if (response.data.ShiftName != 'N/A') {
            $("#drpShift").select2("data", { id: 0, text: response.data.ShiftName });
        } else {
            $("#drpShift").select2("data", { id: 0, text: '-- Select Shift --' });
        }
    }

    $scope.clear = function () {
        $scope.frmWeavingGraiageReceive.$setPristine();
        $scope.frmWeavingGraiageReceive.$setUntouched();
        $scope.WeavingMRRID = '';
        $scope.txtbxRemark = '';
        $scope.txtbxGriege = '';
        $scope.DoffingNo = '';

        $scope.ItemID = '';
        $scope.SetID = "";
        $scope.SizeMRRID = '';
        $scope.UnitID = "";

        $scope.txtbxArtical = '';
        $scope.txtbxUnit = '';
        $scope.txtbxSetNo = '';
        $scope.txtbxDate = conversion.NowDateCustom();
        $scope.isIssued = false;
        //$scope.drpIssueTo = '';

        $scope.drpMachine = '';
        $("#drpMachine").select2("data", { id: 0, text: '-- Select Machine --' });
        $scope.drpOperator = '';
        $("#drpOperator").select2("data", { id: 0, text: '-- Select Operator --' });
        $scope.drpShift = '';
        $("#drpShift").select2("data", { id: 0, text: '-- Select Shift --' });
    }
}]);