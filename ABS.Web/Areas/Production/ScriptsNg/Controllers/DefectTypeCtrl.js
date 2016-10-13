/**
 * DefectTypeCtrl.js
 */
app.controller('DefectTypeCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/DefectType/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDefectType = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.listDefectTypeMaster = [];
        //$scope.btnDefectTypeSaveText = "Save";
        //$scope.btnDefectTypeShowText = "Show List";
        $scope.PageTitle = 'Defect Name Creation';
        $scope.ListTitle = 'Defect Name Information';
        $scope.ListTitleDefectTypeMaster = 'Defect Name List';
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmDefectType'; DelFunc = 'DeleteDefectList'; DelMsg = 'DefectName'; EditFunc = 'getDefectTypeByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        $scope.loadDefectTypeRecords = function (isPaging) {
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDefectType/';
            var listDefectType = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDefectType.then(function (response) {
                $scope.listDefectType = response.data.TypeList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadDefectTypeRecords(0);

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
                $scope.loadAllDefectTypeMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllDefectTypeMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllDefectTypeMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllDefectTypeMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllDefectTypeMasterRecords(1);
                }
            }
        };

        $scope.loadAllDefectTypeMasterRecords = function (isPaging) {
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
            $scope.gridOptionsDefectType = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DefectID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DefectTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DefectNo", displayName: "Defect No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DefectName", title: "Defect Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Description", displayName: "Description", headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptions.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptions.useExternalPagination = false;
                        $scope.gridOptions.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDefectTypeInfo/';
            var listDefectTypeMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDefectTypeMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsDefectType.data = response.data.objDefectTypeMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllDefectTypeMasterRecords(0);
        }
        $scope.RefreshMasterList();


        $scope.getDefectTypeByID = function (dataModel) {
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            //$scope.btnDefectTypeSaveText = "Update";
            //$scope.btnDefectTypeShowText = "Show List";

            $scope.DefectID = dataModel.DefectID;
            $scope.Description = dataModel.Description;
            $scope.DefectName = dataModel.DefectName;
            $scope.DefectNo = dataModel.DefectNo;
            $scope.lstDefectTypeList = dataModel.DefectTypeID;

            angular.forEach($scope.listDefectType, function (typ) {
                if (typ.DefectTypeID == dataModel.DefectTypeID) {
                    $("#ddlDefectType").select2("data", { id: 0, text: typ.DefectTypeName });
                }
            })
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnDefectTypeShowText = "Show List";
                //$scope.IsShowSave = true;
                //$scope.IsShow = true;
            }
            else {
                $scope.RefreshMasterList();
                //$scope.btnDefectTypeShowText = "Create";
                //$scope.pagination.pageNumber = 1;
                //$scope.loadAllDefectTypeMasterRecords(0);
                //$scope.IsShowSave = false;
                $scope.IsShow = false;
            }
        }

        $scope.Save = function () {
            //message = $scope.btnDefectTypeSaveText == "Save" ? "Saved" : "Updated";
            var DefectTypeInfo = {
                DefectID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.DefectID,
                DefectNo: $scope.DefectNo,
                DefectTypeID: $scope.lstDefectTypeList,
                DefectName: $scope.DefectName,
                Description: $scope.Description
            };
            $scope.cmnParam();
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [DefectTypeInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateDefectType/';
            var DefectTypeSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DefectTypeSaveUpdate.then(function (response) {
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
        $scope.DeleteDefectList = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.DefectID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateDefectList/';
            var delDefectList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDefectList.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"](dataModel.DefectName + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.DefectName + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.DefectName + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmDefectType.$setPristine();
            $scope.frmDefectType.$setUntouched();
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.lstDefectTypeList = "";
            $("#ddlDefectType").select2("data", { id: 0, text: '--Select Type--' });
            $scope.Description = '';
            $scope.DefectName = '';
            $scope.DefectNo = '';
            //$scope.btnDefectTypeSaveText = "Save";
            //$scope.btnDefectTypeShowText = "Show List";
        };

    }]);
