/**
 * OutputUnitCtrl.js
 */
app.controller('OutputUnitCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/OutputUnit/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsOutputUnit = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var message = "";
        $scope.listOutputUnit = [];
        //$scope.btnOutputUnitSaveText = "Save";
        //$scope.btnOutputUnitShowText = "Show List";
        $scope.PageTitle = 'Output Unit Creation';
        $scope.ListTitle = 'Output Unit Information';
        $scope.ListTitleOutputUnitMaster = 'Output Unit List';
        //$scope.IsShowSave = true;
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmOutputUnit'; DelFunc = 'DeleteUpdateOutPut'; DelMsg = 'BWSName'; EditFunc = 'getOutputByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        $scope.loadOutputUnitRecords = function (isPaging) {
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetOutputName/';
            var listOutputUnit = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listOutputUnit.then(function (response) {
                $scope.listOutputUnit = response.data.ListOutputUnit;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadOutputUnitRecords(0);

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
                $scope.loadAllOutputUnitRecords(0);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllOutputUnitRecords(0);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllOutputUnitRecords(0);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllOutputUnitRecords(0);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllOutputUnitRecords(0);
                }
            }
        };

        $scope.loadAllOutputUnitRecords = function (isPaging) {
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
            $scope.gridOptionsOutputUnit = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "OutputID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ProcessID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OutputNo", displayName: "Output No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OutputName", title: "Output Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Description", displayName: "Description", headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptions.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptions.useExternalPagination = false;
                        $scope.gridOptions.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetOutputUnitInfo/';
            var listOutputUnit = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listOutputUnit.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsOutputUnit.data = response.data.objOutputUnit;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllOutputUnitRecords(0);
        }
        $scope.RefreshMasterList();

        $scope.getOutputByID = function (dataModel) {
            debugger
            //$scope.btnOutputUnitSaveText = "Update";
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.OutputID = dataModel.OutputID;
            $scope.lstOutputUnitList = dataModel.ProcessID;
            $scope.Description = dataModel.Description;
            $scope.OutputName = dataModel.OutputName;
            $scope.OutputNo = dataModel.OutputNo;

            angular.forEach($scope.listOutputUnit, function (outp) {
                if (outp.OrganogramID == dataModel.ProcessID) {
                    $("#ddlOutputUnit").select2("data", { id: 0, text: outp.ProcessOutput });
                }
            })
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnOutputUnitShowText = "Show List";
                //$scope.IsShow = true;
                //$scope.IsShowSave = true;
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
            }
        }

        $scope.Save = function () {
            debugger
            //message = $scope.btnOutputUnitSaveText == "Save" ? "Saved" : "Updated";
            var OutputUnitInfo = {
                OutputID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.OutputID,
                OutputNo: $scope.OutputNo,
                ProcessID: $scope.lstOutputUnitList,
                OutputName: $scope.OutputName,
                Description: $scope.Description
            };
            $scope.cmnParam();
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [OutputUnitInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateOutputUnit/';
            var OutputUnitSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            OutputUnitSaveUpdate.then(function (response) {
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
        };

        //********---------Delete Data-------**************
        $scope.DeleteUpdateOutPut = function (delModel) {
            $scope.cmnParam();
            objcmnParam.id = delModel.OutputID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateOutPutList/';
            var ConfigDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            ConfigDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.clear();
                    Command: toastr["success"](delModel.BWSName + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](delModel.BWSName + " Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"](delModel.BWSName + " Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };

        $scope.copyvalue = function () {
            $scope.OutputName = $scope.OutputNo
        }

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmOutputUnit.$setPristine();
            $scope.frmOutputUnit.$setUntouched();
            $scope.OutputID = 0;
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.lstOutputUnitList = "";
            $scope.Description = '';
            $scope.OutputName = '';
            $scope.OutputNo = '';
            //$scope.btnOutputUnitSaveText = "Save";
            //$scope.btnOutputUnitShowText = "Show List";
            $("#ddlOutputUnit").select2("data", { id: 0, text: '--Select Output--' });
        };
    }]);
