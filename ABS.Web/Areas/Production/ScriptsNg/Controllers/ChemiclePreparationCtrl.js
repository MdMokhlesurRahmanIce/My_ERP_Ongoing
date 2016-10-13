/**
 * ChemicalPreparationCtrl.js
 */
app.controller('ChemicalPreparationCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/ChemiclePreparation/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsFinChemMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 20;
        var isPaging = 0;
        var inCallback = false;
        var totalData = 0;
        message = '';
        $scope.IsfrmShow = true;
        //$scope.IsShowSave = true;
        $scope.IsShowDetail = true;
        $scope.IsSaveDisable = true;
        $scope.IsShow = false;
        $scope.IsHidden = true;
        $scope.PageTitleMaster = 'Chemical Preparation Entry';
        //$scope.btnSaveText = 'Save';
        //$scope.btnShowList = 'Show List';
        $scope.PageTitleDetail = 'Chemical Preparation Detail Entry';
        $scope.ListTitleMaster = 'Chemical Preparation List';
        $scope.ChemDetailList = [];
        $scope.PreparationDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'FinChemicalStupNo'; EditFunc = 'getFinChemMasterByID, AddItemToList';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************  

        //***********************************************Start Load ddl Finishing Type from Finishing Process***************************
        $scope.loadFinishingProcessType = function (isPaging) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFinishingProcessType/';
            var ddlfinishtype = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ddlfinishtype.then(function (response) {
                $scope.listFinishProcessType = response.data.ListType;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadFinishingProcessType(0);
        //*************************************************End Load ddl Finishing Type from Finishing Process****************************

        $scope.loadArticalAsFinChemical = function () {
            $scope.cmnParam();
            objcmnParam.ItemType = 5;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetChemical/';
            var ddlitemtype = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ddlitemtype.then(function (response) {
                $scope.listChemical = response.data.ListChemical;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadArticalAsFinChemical();

        //***********************************************************Start Add ToList****************************************************
        //***********************************************Start Load ddl Chemical from cmnItemmaster**************************************        
        $scope.AddItemToList = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel) ? 0 : dataModel.FinChemicalStupID;
            if (objcmnParam.id != 0) {
                $scope.ChemDetailList = [];
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetFiniChemicalPrepDetailByID/';
                var listChemDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listChemDetail.then(function (response) {
                    $scope.ChemDetailList = response.data.ListFinChemByID;
                    $scope.IsShow = $scope.ChemDetailList.length > 0 ? true : false;
                    if ($scope.ChemDetailList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                debugger
                //$scope.AutoSlNo = $scope.AutoSlNo + 1;
                $scope.ChemDetailList.push({
                    FinChemicalStupDetailID: 0, FinChemicalStupID: 0, ChemicalID: 0, MinQty: "", MaxQty: "", UnitID: 0, UOMName: ''
                })
                $scope.IsShow = $scope.ChemDetailList.length > 0 ? true : false;
                if ($scope.ChemDetailList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            }
        };

        $scope.numberChangeEvent = function (dataModel) {
            debugger
            dataModel.MinQty = conversion.setMaxMin(dataModel.MinQty, 0, 0);
            dataModel.MaxQty = conversion.setMaxMin(dataModel.MaxQty, 0, 0);
        }
        //************************************************End Load ddl Chemical from cmnItemmaster***************************************
        //***********************************************************End Add ToList******************************************************

        //*****************************************Start Delete Item From List**************************************
        $scope.deleteRow = function (index) {
            $scope.ChemDetailList.splice(index, 1);
            $scope.IsShow = $scope.ChemDetailList.length > 0 ? true : false;
            $scope.IsSaveDisable = $scope.ChemDetailList.length > 0 ? false : true;
        };
        //*****************************************End Delete Item From List**************************************

        //*******************************************************Start Load Unit from cmnUOM*********************************************
        $scope.loadChemicalUnitRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.ChemicalID == null || dataModel.ChemicalID == '' ? 0 : dataModel.ChemicalID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetUnitSingle/';
            var listUnit = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listUnit.then(function (response) {
                //$scope.ListUnit = response.data.ListUnit;
                if (response.data.ListUnit != null) {
                    dataModel.UnitID = response.data.ListUnit.UOMID;
                    dataModel.UOMName = response.data.ListUnit.UOMName;
                }
                else {
                    dataModel.UnitID = null;
                    dataModel.UOMName = "";
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //********************************************************End Load Unit from cmnUOM**********************************************

        //**************************************************Start Finishing Chemical Master Dynamic Grid*********************************
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
                $scope.loadFinChemicalPrepMaster(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadFinChemicalPrepMaster(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadFinChemicalPrepMaster(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadFinChemicalPrepMaster(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadFinChemicalPrepMaster(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadFinChemicalPrepMaster = function (isPaging) {
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
            $scope.gridOptionsFinChemMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "FinChemicalStupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingProcessID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinChemicalStupNo", displayName: "Finishing Chemical Setup No", title: "Finishing Chemical Setup No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingProcessName", displayName: "Finishing Process Name", title: "Finishing Process Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PreparationDate", displayName: "Preparation Date", title: "Preparation Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", title: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsFinChemMaster.useExternalPagination = false;
                        $scope.gridOptionsFinChemMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFinChemicalPreparationMaster/';
            var listChemicalMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemicalMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsFinChemMaster.data = response.data.ListFinChem;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadFinChemicalPrepMaster(0);
        }
        $scope.RefreshMasterList();
        //***************************************************End Finishing Chemical Master Dynamic Grid***************************************

        $scope.getFinChemMasterByID = function (dataModel) {
            $scope.FinChemicalStupID = dataModel.FinChemicalStupID;
            $scope.FinishingProcessID = dataModel.FinishingProcessID;
            $scope.Remarks = dataModel.Remarks;
            $scope.PreparationDate = dataModel.PreparationDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.PreparationDate);
            //$scope.btnSaveText = "Update";
            //$scope.btnShowList = "Show List";
            //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
            //$scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShowDetail = true;
            //$scope.IsSaveDisable = false;
            $("#finishingType").select2("data", { id: 0, text: dataModel.FinishingProcessName });
        }

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowList = "Show List";
                //$scope.IsShowSave = true;
                //$scope.IsfrmShow = true;
                //$scope.IsfrmDetailShow = true;
                //$scope.IsShow = $scope.ChemDetailList.length < 1 || angular.isUndefined($scope.ChemDetailList.length) ? false : true;
                //$scope.IsShowSave = $scope.ChemDetailList.length < 1 || angular.isUndefined($scope.ChemDetailList.length) ? false : true;
            }
            else {
                //$scope.btnShowList = "Create";
                //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                $scope.IsShowDetail = false;
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var FinChemicalMaster = {
                FinChemicalStupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.FinChemicalStupID,
                FinishingProcessID: $scope.FinishingProcessID,
                PreparationDate: $scope.PreparationDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.PreparationDate),
                Remarks: angular.isUndefined($scope.Remarks) || $scope.Remarks == '' ? '' : $scope.Remarks
            };

            //*************----Detail Data---**************
            debugger
            if ($scope.ChemDetailList.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [FinChemicalMaster, $scope.ChemDetailList, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateFiniChemicalMasterDetail/';
            var FiniChemCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            FiniChemCreateUpdate.then(function (response) {
                if (response != '') {
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
            var apiRoute = baseUrl + 'DelUpdateFiniChemicalMasterDetail/';
            var SetWiseMachineSetupMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetWiseMachineSetupMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.clear();
                    Command: toastr["success"](delModel.FinChemicalStupNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](delModel.FinChemicalStupNo + " Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"](delModel.FinChemicalStupNo + " Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*********************************************************End Save/Update/Delete**********************************************************

        //**************************************************************Start Reset/Clear**********************************************************
        $scope.clear = function () {
            debugger
            $scope.frmChemicalPreparations.$setPristine();
            $scope.frmChemicalPreparations.$setUntouched();
            $scope.FinChemicalStupID = 0;
            $scope.FinishingProcessID = "";
            $scope.Remarks = "";
            $scope.PreparationDate = conversion.NowDateCustom();
            //$scope.btnSaveText = "Save";
            //$scope.btnShowList = "Show List";
            //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
            $scope.IsShow = false;
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShowDetail = true;
            $scope.IsSaveDisable = true;
            $scope.gridOptionsFinChemMaster.data = [];
            $scope.ChemDetailList = [];
            $("#finishingType").select2("data", { id: 0, text: "--Select Finishing Type--" });
        };
        //**************************************************************End Reset/Clear**********************************************************
    }]);


//function modal_fadeOut() {
//    $("#userModal").fadeOut(200, function () {
//        $('#userModal').modal('hide');
//    });
//}