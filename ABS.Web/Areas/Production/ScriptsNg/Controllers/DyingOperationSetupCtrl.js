
app.controller('DyingOperationSetupCtrl', ['$scope', 'commonComboBoxGetDataService', 'DyingOperationSetupService', 'crudService', 'conversion', '$localStorage',
    function ($scope, commonComboBoxGetDataService, DyingOperationSetupService, crudService, conversion, $localStorage) {
        var baseUrl = '/Production/api/DyingOperationSetup/';
        var dropDwonUrl = '/Production/api/ProductionDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.gridOptionsOperation = [];

        var companyID = 1;
        var loggedUser = 0;
        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;

        //$scope.ToogleDiv = 1;
        //$scope.ToogleShowListButtonName = "New";

        $scope.ChemicalSetupID = 0;
        //$scope.btnSaveUpdateText = "Save";
        $scope.PageTitle = 'Create Operation Setup';
        $scope.ListDetail = 'Operation Setup List for Save';
        $scope.ListMaster = 'Operation Setup List';

        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsShowDetail = true;

        $scope.IsChemical = false;
        $scope.Description = "";
        $scope.CodeList = [];
        $scope.ProcessTypeList = [];
        $scope.OperationList = [];
        $scope.ListUOM = [];
        $scope.ItemList = [];
        $scope.ListDyingOperationSetup = [];
        //$scope.SearchListDyingOperationSetup = [];

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteChemicalOperation'; DelMsg = 'ProcessName'; EditFunc = 'EditOperationSetup';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************

        //**************** Load Item Master Dropdown ************************
        function loadRecords_ItemMaster(item) {
            $scope.loaderMoreForSampleNo = true;
            $scope.lblMessageForSampleNo = '';
            $scope.result = "color-red";
            //Send Item ID =1
            var ItemID = 1;
            var apiRoute = dropDwonUrl + 'GetItemMasterByItemID/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.CodeList = response.data
                $scope.loaderMoreForSampleNo = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_ItemMaster(0);
        //**************** GetAll All ************************
        //function loadRecords_All(number) {
        //    var apiRoute = baseUrl + 'GetAllOperationSetup/';
        //    var processChecmicalDropdownLoad = DyingOperationSetupService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
        //    processChecmicalDropdownLoad.then(function (response) {
        //        $scope.SearchListDyingOperationSetup = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadRecords_All(0);

        //************************************************Start Show Chemical Process List Information Dynamic Grid******************************************************
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
                $scope.loadAllOperationMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllOperationMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllOperationMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllOperationMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllOperationMasterRecords(1);
                }
            }
        };

        $scope.loadAllOperationMasterRecords = function (isPaging) {
            $scope.gridOptionsOperation.enableFiltering = true;

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
            $scope.gridOptionsOperation = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "OperationSetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ChemicalItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DyingProcessID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OperationID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ChemicalArticleNo", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ProcessName", displayName: "Process Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OperationName", displayName: "Operation Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", displayName: "Unit", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MinQty", displayName: "Min Qty", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MaxQty", displayName: "Max Qty", headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptionsOperation.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsOperation.useExternalPagination = false;
                        $scope.gridOptionsOperation.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetAllOperationSetup/';
            var listChemProcessMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemProcessMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsOperation.data = response.data.finalList;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllOperationMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //*************************************************End Show Chemical Process List Information Dynamic Grid*******************************************************


        //**************** Load Process Type Dropdown ************************
        function loadRecords_DyingProcessType(processType) {
            //Send ProcessID =1
            var ProcessID = 8;
            var apiRoute = dropDwonUrl + 'GetDyingProcessByProcessID/';
            var process = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ProcessID);
            process.then(function (response) {
                $scope.ProcessTypeList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_DyingProcessType(0);
        //**************** Load Process Type Dropdown ************************
        function loadRecords_DyingOperation(ProcessID) {
            var apiRoute = dropDwonUrl + 'GetDyingOperationByProcessID/';
            var process = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ProcessID);
            process.then(function (response) {
                $scope.OperationList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_DyingOperation(0);
        //**************** Load UOM Dropdown ************************
        function loadRecords_UOMDropDown(isPaging) {
            var apiRoute = dropDwonUrl + 'GetUOM/';
            var processUOM = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
            processUOM.then(function (response) {
                $scope.ListUOM = response.data  //Set Default 
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_UOMDropDown(0);
        //**************** Load Item Master Dropdown ************************
        function loadRecords_ItemChemical(ItemID) {
            var apiRoute = dropDwonUrl + 'GetItemMasterByItemID/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.ItemList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_ItemChemical(5);
        //**************** Code change Event************************
        //$scope.checkBoxClicked = function (chkStatus) {
        //    if (chkStatus) {
        //        $scope.IsChemical = true;
        //        //$('#chkChemical').prop('checked', true);
        //        //$.uniform.update();
        //    }
        //    else {
        //        $scope.IsChemical = false;
        //        //$('#chkChemical').prop('checked', false);
        //        //$.uniform.update();
        //    }
        //};
        //**************** Code change Event************************
        $scope.DyingProcessDDLChanged = function (model) {
            angular.forEach($scope.ProcessTypeList, function (value, key) {
                if (value.DyingProcessID == model.DyingProcessID) {
                    $scope.ddlDyingOperation = null;
                    $("#dyingOperationDropDown").select2('val', '--Select Operation--');
                    $scope.OperationList = [];
                    loadRecords_DyingOperation(model.DyingProcessID);
                }
            });
        };
        //**************** Code change Event************************
        $scope.showDescription = function (model) {
            angular.forEach($scope.CodeList, function (value, key) {
                if (value.ItemColorID == model.ItemColorID) {
                    $scope.Description = value.ConcatedProperty;
                }
            });
        };
        //***************** Add List **********************
        $scope.AddList = function () {
            var ItemID = 0;
            var ArticleNo = "";
            var ChemicalItemID = 0;
            var ChemicalArticleNo = "";
            var DyingProcessID = 0;
            var ProcessName = "";
            var OperationID = 0;
            var OperationName = "";
            var UnitID = 0;
            var UOMName = "";
            if (angular.isObject($scope.ddlCode)) {
                ItemID = $scope.ddlCode.ItemID;
                ArticleNo = $scope.ddlCode.ArticleNo;
            }
            else {
                angular.forEach($scope.CodeList, function (value, key) {
                    if (value.ItemID == $scope.ddlCode) {
                        ItemID = value.ItemID;
                        ArticleNo = value.ArticleNo;
                    }
                });
            }
            if (angular.isObject($scope.ddlItem)) {
                ChemicalItemID = $scope.ddlItem.ItemID;
                ChemicalArticleNo = $scope.ddlItem.ArticleNo;
            }
            else {
                angular.forEach($scope.ItemList, function (value, key) {
                    if (value.ItemID == $scope.ddlItem) {
                        ChemicalItemID = value.ItemID;
                        ChemicalArticleNo = value.ArticleNo;
                    }
                });
            }
            if (angular.isObject($scope.ddlProcessType)) {
                DyingProcessID = $scope.ddlProcessType.DyingProcessID;
                ProcessName = $scope.ddlProcessType.ProcessName;
            }
            else {
                angular.forEach($scope.ProcessTypeList, function (value, key) {
                    if (value.DyingProcessID == $scope.ddlProcessType) {
                        DyingProcessID = value.DyingProcessID;
                        ProcessName = value.ProcessName;
                    }
                });
            }
            if (angular.isObject($scope.ddlDyingOperation)) {
                OperationID = $scope.ddlDyingOperation.OperationID;
                OperationName = $scope.ddlDyingOperation.OperationName;
            }
            else {
                angular.forEach($scope.OperationList, function (value, key) {
                    if (value.OperationID == $scope.ddlDyingOperation) {
                        OperationID = value.OperationID;
                        OperationName = value.OperationName;
                    }
                });
            }
            if (angular.isObject($scope.ddlUOM)) {
                UnitID = $scope.ddlUOM.UOMID;
                UOMName = $scope.ddlUOM.UOMName;
            }
            else {
                angular.forEach($scope.OperationList, function (value, key) {
                    if (value.UOMID == $scope.ddlUOM) {
                        UnitID = value.UOMID;
                        UOMName = value.UOMName;
                    }
                });
            }

            var obj = {
                OperationSetupID: $scope.OperationSetupID,
                ItemID: ItemID,
                ArticleNo: ArticleNo,
                ChemicalItemID: ChemicalItemID,
                ChemicalArticleNo: ChemicalArticleNo,
                DyingProcessID: DyingProcessID,
                ProcessName: ProcessName,
                OperationID: OperationID,
                OperationName: OperationName,
                MinQty: $scope.MinQty,
                MaxQty: $scope.MaxQty,
                UnitID: UnitID,
                UOMName: UOMName,
                IsDeleted: false
            };
            $scope.ListDyingOperationSetup.push(obj);
            $scope.OperationSetupID = 0;
            $scope.ClearParamsAdd();
            $scope.IsShow = $scope.ListDyingOperationSetup.length > 0 ? true : false;
            if ($scope.ListDyingOperationSetup.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        };
        //***************** Clear Data **********************
        $scope.ClearParamsAdd = function ClearParamsAdd() {
            $scope.ddlUOM = null;
            $("#unitDropDown").select2('val', '--Select Unit--');
            $scope.ddlItem = null;
            $("#itemDropdown").select2('val', '--Select Item--');
            $scope.MaxQty = 0.00;
            $scope.MinQty = 0.00;
        };
        $scope.ClearParams = function ClearParams(frmPrdDyingOperationSetup) {
            try {
                $scope.frmPrdDyingOperationSetup.$setPristine();
                $scope.frmPrdDyingOperationSetup.$setUntouched();
                frmPrdDyingOperationSetup.$setPristine();
                frmPrdDyingOperationSetup.$setUntouched();
            } catch (e) {
            }
            $scope.ddlCode = null;
            $("#DOperationcolorDropdown").select2('val', '--Select Code--');

            $scope.ddlProcessType = null;
            $("#dyingProcessDropDown").select2('val', '--Select Process--');

            $scope.ddlDyingOperation = null;
            $("#dyingOperationDropDown").select2('val', '--Select Operation--');

            $scope.ddlUOM = null;
            $("#unitDropDown").select2('val', '--Select Unit--');

            $scope.ddlItem = null;
            $("#itemDropdown").select2('val', '--Select Item--');
            //$scope.checkBoxClicked(false);
            $scope.IsChemical = false;
        };
        //**********----Delete Details ----***************
        $scope.deleteDetailsList = function (dataModel) {
            //var IsConf = confirm('You are about to delete ' + dataModel.ArticleNo + '. Are you sure?');
            //if (IsConf) {
            var list = $scope.ListDyingOperationSetup;
            angular.forEach(list, function (value, key) {
                if (value.ArticleNo == dataModel.ArticleNo && value.ProcessName == dataModel.ProcessName && value.OperationName == dataModel.OperationName) {
                    value.IsDeleted = true;
                }
            });
            //}
        }
        //**********----Save----***************
        $scope.Save = function () {
            if ($scope.ListDyingOperationSetup.length == 0) {
                Command: toastr["info"]("Please Add Details");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var master = $scope.ListDyingOperationSetup;
            var apiRoute = baseUrl + 'SaveOperationSetup/';
            var porcessMasterDetails = DyingOperationSetupService.postMasterListWithCommonEntity(apiRoute, master, $scope.UserCommonEntity, HeaderTokenPutPost);
            porcessMasterDetails.then(function (response) {
                response.data = response;
                if (response.data.result == 1) {
                    $scope.clear();
                    response.data = 1;
                    ShowCustomToastrMessage(response);
                }
                else if (response.data.result == 0) //Erro
                {
                    response.data = 0;
                    ShowCustomToastrMessage(response);
                }
                else {
                    response.data = -1;
                    ShowCustomToastrMessage(response);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //***************************** Edit Mode ***************************************
        $scope.EditOperationSetup = function (dataModel) {
            debugger
            //$scope.ToogleDiv = 0;
            //$scope.ShowList();
            //$scope.btnSaveUpdateText = "Update";
            //var list = $scope.SearchListDyingOperationSetup;
            //angular.forEach(list, function (value, key) {
            //if (value.OperationSetupID == dataModel.OperationSetupID) {
            debugger
            $scope.OperationSetupID = dataModel.OperationSetupID;
            $scope.ddlCode = dataModel.ItemID;
            $("#DOperationcolorDropdown").select2('val', dataModel.ItemID.toString());

            $scope.ddlItem = dataModel.ChemicalItemID;
            if (dataModel.ChemicalItemID != null) {
                $("#itemDropdown").select2('val', dataModel.ChemicalItemID.toString());
                $scope.IsChemical = true;
            }

            $scope.ddlProcessType = dataModel.DyingProcessID;
            $("#dyingProcessDropDown").select2('val', dataModel.DyingProcessID.toString());

            $scope.ddlDyingOperation = dataModel.OperationID;
            $("#dyingOperationDropDown").select2('val', dataModel.OperationID.toString());

            $scope.ddlUOM = dataModel.UnitID;
            if (dataModel.UnitID != null)
                $("#unitDropDown").select2('val', dataModel.UnitID.toString());

            $scope.MinQty = dataModel.MinQty;
            $scope.MaxQty = dataModel.MaxQty;
            //}
            //});
            $scope.IsShowDetail = false;
            $scope.IsShow = false;
            
            $scope.ListDyingOperationSetup = [];

            obj = {
                OperationSetupID: dataModel.OperationSetupID,
                ItemID: dataModel.ItemID,
                ArticleNo: dataModel.ArticleNo,
                ChemicalItemID: dataModel.ChemicalItemID,
                ChemicalArticleNo: dataModel.ChemicalArticleNo,
                DyingProcessID: dataModel.DyingProcessID,
                ProcessName: dataModel.ProcessName,
                OperationID: dataModel.OperationID,
                OperationName: dataModel.OperationName,
                MinQty: dataModel.MinQty,
                MaxQty: dataModel.MaxQty,
                UnitID: dataModel.UnitID,
                UOMName: dataModel.UOMName,
                IsDeleted: false
            };
            $scope.ListDyingOperationSetup.push(obj);
            if ($scope.ListDyingOperationSetup.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        }

        //************************************************ Switch between show and hide ***********************************        
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
            }
        }
        //************************************************ Switch between show and hide ***********************************        

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteChemicalOperation = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.OperationSetupID;
            var apiRoute = baseUrl + 'DeleteChemicalOperation/';
            ModelsArray = [objcmnParam];
            var delChemOperation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delChemOperation.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.ProcessName + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.ProcessName + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.ProcessName + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        //************ Reset Form *******************
        $scope.NewInstance = function () {
            $scope.ClearParams(frmPrdDyingOperationSetup);
            $scope.ChemicalSetupID = 0;
            //$scope.btnSaveUpdateText = "Save";
            $scope.ListDyingOperationSetup = [];
        }

        //************************************************ Start Reset ***********************************   
        $scope.clear = function () {
            $scope.NewInstance();
            //$scope.ClearParamsAdd();
            $scope.MaxQty = '';
            $scope.MinQty = '';
            $scope.Description = '';
            $scope.IsShowDetail = true;
        }
        //************************************************* End Reset ************************************        

        //******************************Click Event *********************
        //$scope.ShowList = function () {
        //    if ($scope.ToogleDiv == 0) {
        //        $scope.ToogleShowListButtonName = "Show List";
        //        $("#rowDetials").show(200);
        //        $("#prdDyingOperationSetup").show(200);
        //        $("#rowList").hide(200);
        //        $scope.ToogleDiv = 1;
        //        $scope.NewInstance();
        //        loadRecords_All(0);
        //        $scope.MinQty = 0;
        //        $scope.MaxQty = 0;

        //        $scope.IsChemical = false;
        //    }
        //    else {
        //        $scope.ToogleShowListButtonName = "New";
        //        $("#rowDetials").hide(200);
        //        $("#prdDyingOperationSetup").hide(200);
        //        $("#rowList").show(200);
        //        $scope.ToogleDiv = 0;
        //        $scope.NewInstance();
        //        loadRecords_All(0);
        //        $scope.btnSaveUpdateText = "Save";
        //        $scope.OperationSetupID = 0;
        //        $scope.MinQty = 0;
        //        $scope.MaxQty = 0;
        //        $scope.IsChemical = false;
        //        $scope.checkBoxClicked(false);
        //    }
        //}
        //$scope.ShowList();
    }]);


