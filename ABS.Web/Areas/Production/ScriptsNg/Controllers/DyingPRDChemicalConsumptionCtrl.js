
app.controller('DyingPRDChemicalConsumptionCtrl', ['$scope', 'commonComboBoxGetDataService', 'DyingPRDChemicalConsumptionService', 'crudService', 'conversion', '$localStorage',
    function ($scope, commonComboBoxGetDataService, DyingPRDChemicalConsumptionService, crudService, conversion, $localStorage) {
        var baseUrl = '/Production/api/DyingPRDChemicalConsumption/';
        var productionCommonUrl = '/Production/api/ProductionDDL/';
        var partialUrl = '/Production/api/SizingChamicaleConsumption/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.gridOptionsChemConsump = [];

        var companyID = $('#hCompanyID').val();
        var loggedUser = $('#hUserID').val();
        $scope.departmentID = 0;
        $scope.departmentName = "";
        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        //$scope.ToogleDiv = 1;
        $scope.IsHidden = true;
        $scope.IsShow = false;

        //$scope.ToogleShowListButtonName = "New";
        $scope.ListSetNo = [];
        $scope.ListOperations = [];
        $scope.ListUOM = [];
        $scope.ListTank = [];
        $scope.ListConsumptionDetails = [];
        $scope.ListChemicalItem = [];
        $scope.ListBatch = [];

        $scope.ListConsumptionChemicalMaster = [];
        $scope.ListConsumptionChemicalDetails = [];
        //$scope.ListConsumptionMaster = [];
        $scope.DyingConsumptionID = 0;
        //$scope.btnSaveUpdateText = "Save";
        $scope.PageTitle = 'Chemical Consumption';
        $scope.ListDetail = 'Consumption Details';
        $scope.ListMaster = 'Consumption List';
        $scope.ItemID = 0;
        $scope.Date = conversion.NowDateCustom();

        $scope.loaderMore = false;
        $scope.lblMessage = 'loading please wait....!';
        $scope.result = "color-red";

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'deleteConsumption'; DelMsg = 'DyingConsumptionNo'; EditFunc = 'EditDyingConsumption';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************

        //************************************************ Start Switch between show and hide ***********************************        
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
        //************************************************* End Switch between show and hide ************************************

        //**********************Get All Consumption *******************************************************
        function loadConsumption_All(number) {
            var apiRoute = baseUrl + 'GetAllConsumption/';
            var processLoadConsumption = DyingPRDChemicalConsumptionService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processLoadConsumption.then(function (response) {
                $scope.ListConsumptionMaster = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadConsumption_All(0);

        function loadConsumption_BYID(number) {
            var ConsumptionID = 119;
            var apiRoute = baseUrl + 'GetChemicalConsumptionByID/';

            var processLoadConsumption = DyingPRDChemicalConsumptionService.getConsumptionDetailsByID(apiRoute, companyID, loggedUser, ConsumptionID, $scope.HeaderToken.get);
            processLoadConsumption.then(function (response) {

                $scope.ListConsump = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        // loadConsumption_BYID(0);

        $scope.EditDyingConsumption = function (datamodel) {
            //$scope.ToogleDiv == 0;
            //$scope.ShowList();
            var ConsumptionID = datamodel.DyingConsumptionID;
            var apiRoute = baseUrl + 'GetConsumptionByID/';
            var processLoadMaster = DyingPRDChemicalConsumptionService.getDetailsByID(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ConsumptionID, $scope.HeaderToken.get);
            processLoadMaster.then(function (response) {
                var masterdata = response.data;
                $scope.DyingConsumptionID = masterdata.DyingConsumptionID;
                $scope.indigoStart = masterdata.IndigoStart;
                $scope.indigoStop = masterdata.IndigoStop;
                $scope.blackStart = masterdata.BlackStart;
                $scope.blackStop = masterdata.BlackStop;
                $scope.articleNo = masterdata.ArticleNo;
                $scope.ddlSetNo = masterdata.SetID;
                $scope.ItemID = masterdata.ItemID;

                if (masterdata.SetID != null)
                    $("#ddlSetNo").select2('val', masterdata.SetID.toString());

                $scope.ddlOperation = masterdata.OperationID;
                if (masterdata.OperationID != null)
                    $("#ddlOperation").select2('val', masterdata.OperationID.toString());
                $scope.Date = masterdata.Date != null && masterdata.Date != '' ? conversion.getDateToString(masterdata.Date) : '';

                var apiRoute = baseUrl + 'GetChemicalConsumptionByID/';
                var processLoadDetails = DyingPRDChemicalConsumptionService.getConsumptionDetailsByID(apiRoute, companyID, loggedUser, ConsumptionID, $scope.HeaderToken.get);
                processLoadDetails.then(function (response) {
                    debugger
                    $scope.ListConsumptionDetails = response.data;
                    $scope.IsShow = $scope.ListConsumptionDetails.length > 0 ? true : false;
                    if ($scope.ListConsumptionDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************Get Department By Company And Logged User *****************************
        function GetDepartmentByCompayUserID(companyID, UserID) {
            var apiRoute = productionCommonUrl + 'GetDepartmentByCompayUserID/';
            var process = commonComboBoxGetDataService.GetDepartment(apiRoute, companyID, UserID, $scope.HeaderToken.get);
            process.then(function (response) {
                try {
                    $scope.departmentID = response.data[0].DepartmentID;
                    $scope.departmentName = response.data[0].DepartmentName;
                } catch (e) {
                    departmentID = 0;
                    departmentName = "";
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetDepartmentByCompayUserID(companyID, loggedUser);
        //**************** Load Set NO Dropdown ************************
        function loadDropdown_SetNo(item) {
            var ItemID = item;
            var apiRoute = productionCommonUrl + 'GetDyingSetNoByItemID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListSetNo = response.data;

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadDropdown_SetNo(0);
        //****************Set No Change Events ************************
        $scope.ChangeSetNo = function (model) {

            $scope.articleNo = model.ArticleNo;
            $scope.ItemID = model.ItemID;
        };
        function loadDropdown_Process(item) {
            var ItemID = item;
            var apiRoute = productionCommonUrl + 'GetDyingOperationByProcessID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListProcess = response.data;

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadDropdown_Process(0);
        //**************************** ADD Item To List ****************************************
        $scope.AddItemToList = function () {
            //Insertion Add  Begin
            $scope.EntityState = "Insert";
            $scope.ListConsumptionDetails.push({
                DyingConsumptionDetailID: $scope.ListConsumptionDetails.length + 1,
                DyingConsumptionID: $scope.DyingConsumptionID,
                MachinePartID: 0,
                OperationID: 0,
                Temp: '',
                ConsumptionChemicalMID: 0,
                ConsumptionChemicalSum: '',
                Volume: '',
                UOMID: 0,
                IsDeleted: false,
                EntityState: $scope.EntityState,
                //PrdDyingConsumptionChemicalM: { ConsumptionChemicalMID: 1, TotalChemical: 100, PrdDyingConsumptionChemicals: [{ ConsumptionChemicalMID: 1, Qty: 200 }] },
            });
            $scope.IsShow = $scope.ListConsumptionDetails.length > 0 ? true : false;
            if ($scope.ListConsumptionDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            //Insertion Add End
        }
        //**************** Load Tank Dropdown ************************
        function loadDropdown_Tank(item) {
            var MachineID = item;
            var apiRoute = productionCommonUrl + 'GetMachinePartByMachineID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, MachineID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListTank = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadDropdown_Tank(0);
        //**************** Load UOM Dropdown ************************
        function loadDropdown_DetailsOperation(isPaging) {
            var apiRoute = productionCommonUrl + 'GetOperation/';
            var processUOM = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processUOM.then(function (response) {
                $scope.ListOperations = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadDropdown_DetailsOperation(0);
        //**************** Load UOM Dropdown ************************
        function loadDropdown_UOM(isPaging) {
            var apiRoute = productionCommonUrl + 'GetUOM/';
            var processUOM = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processUOM.then(function (response) {
                $scope.ListUOM = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadDropdown_UOM(0);
        //**************** Populate Chemical Array ************************
        //Load ChemicalList Based On Details 
        function loadChemical(item) {
            var ItemTypeID = item;
            var apiRoute = productionCommonUrl + 'GetItmeByItemType/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemTypeID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListChemicalItem = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**************** Populate Batch Array ************************
        function loadBatch(item) {
            var BatchID = item;
            var apiRoute = productionCommonUrl + 'GetBatchByItemID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, BatchID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListBatch = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadBatch(0);

        //************************* Get GetChemicalPopUp(dataModel.ConsumptionChemicalMID)***************
        $scope.GetChemicalPopUp = function (Entity, consumptionChemicalMID, dyingConsumptionDetailID) {

            $scope.ListChemicalItem = [];
            if ($scope.DyingConsumptionID == 0) {
                var ItemTypeID = 5;
                var apiRoute = productionCommonUrl + 'GetItmeByItemType/';
                var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemTypeID, $scope.HeaderToken.get);
                processDropdownLoad.then(function (response) {

                    if (consumptionChemicalMID == 0) {
                        $scope.ListChemicalItem = response.data;
                        $scope.ListChemicalItem.ConsumptionChemicalMID = consumptionChemicalMID;
                        $scope.ListChemicalItem.DyingConsumptionDetailID = dyingConsumptionDetailID;                        
                        $scope.ListChemicalItem.EntityState = "Insert";
                        $scope.ListChemicalItem.push({ ItemID: 0, ItemName: 'Total', Quantity: 0 });
                    }
                    else {

                        $scope.ListChemicalItem = response.data;
                        angular.forEach($scope.ListConsumptionDetails, function (value, key) {
                            if (value.ConsumptionChemicalMID == consumptionChemicalMID) {
                                $scope.ListChemicalItem.ConsumptionChemicalMID = consumptionChemicalMID;
                                $scope.ListChemicalItem.DyingConsumptionDetailID = dyingConsumptionDetailID;
                                $scope.ListChemicalItem.EntityState = "Insert";
                                $scope.ListChemicalItem.push({ ItemID: 0, ItemName: 'Total', Quantity: value.ConsumptionChemicalSum });
                            }

                        });
                    }

                    angular.forEach($scope.ListChemicalItem, function (gots) {                        
                            gots.IsQtyReadOnly = true;
                    })

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

            if ($scope.DyingConsumptionID > 0) {
                debugger
                var ItemTypeID = 5;
                var apiRoute = productionCommonUrl + 'GetItmeByItemType/';
                var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemTypeID, $scope.HeaderToken.get);
                processDropdownLoad.then(function (response) {

                    if (Entity.ConsumptionChemicalMID == 0) {
                        $scope.ListChemicalItem = response.data;
                        $scope.ListChemicalItem.ConsumptionChemicalMID = Entity.ConsumptionChemicalMID;
                        $scope.ListChemicalItem.DyingConsumptionDetailID = Entity.DyingConsumptionDetailID;
                        $scope.ListChemicalItem.EntityState = "Insert";
                        $scope.ListChemicalItem.push({ ItemID: 0, ItemName: 'Total', Quantity: 0 });
                    }
                    else {

                        $scope.ListChemicalItem = response.data;
                        $scope.ListChemicalItem.ConsumptionChemicalMID = Entity.ConsumptionChemicalMID;
                        $scope.ListChemicalItem.DyingConsumptionDetailID = Entity.DyingConsumptionDetailID;

                        angular.forEach(Entity.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals, function (gotIt) {
                            angular.forEach($scope.ListChemicalItem, function (gots) {
                                if (gotIt.ChemicalID == gots.ItemID && gotIt.ConsumptionChemicalMID == Entity.ConsumptionChemicalMID) {
                                    gots.BatchID = gotIt.BatchID;
                                    gots.SupplierID = gotIt.SupplierID;
                                    gots.Quantity = gotIt.Qty;
                                    gots.UnitID = gotIt.UnitID;
                                    gots.UOMName = gotIt.UOMName;
                                    gots.UnitPrice = gotIt.UnitPrice;
                                    gots.CurrentStock = gotIt.CurrentStock;
                                    gots.IsQtyReadOnly = false;
                                }
                            })
                        })
                        $scope.ListChemicalItem.EntityState = Entity.EntityState;
                        $scope.ListChemicalItem.push({ ItemID: 0, ItemName: 'Total', Quantity: Entity.ConsumptionChemicalSum });
                    }

                },
                function (error) {
                    console.log("Error: " + error);
                });

            }
        }
        //************************* Get GetChemicalPopUp(dataModel.ConsumptionChemicalMID)***************
        $scope.SumQuantity = function (item) {
            var result = 0;
            angular.forEach($scope.ListChemicalItem, function (value, key) {
                if (value.ItemID != 0) {
                    result = parseInt(result) + parseInt(value.Quantity);
                }
            });
            angular.forEach($scope.ListChemicalItem, function (value, key) {
                if (value.ItemID == 0) {
                    value.Quantity = result;
                }
            });
        }

        //*******************************************************Start Load Current Stock**************************************************
        $scope.loadCurrentStock = function (dataEntity) {
            debugger
            if (dataEntity.Supplier.length > 0) {
                if (dataEntity.SupplierID === undefined || dataEntity.SupplierID === null || dataEntity.SupplierID == 0) {
                    //Command: toastr["warning"]("Please Select Supplier!");
                    return;
                }
            }
            if (dataEntity.Batch.length > 0) {
                if (dataEntity.BatchID === undefined || dataEntity.BatchID === null || dataEntity.BatchID==0) {
                    //Command: toastr["warning"]("Please Select Batch!");
                    return;
                }
            }

            $scope.cmnParam();
            objcmnParam.id = dataEntity.ItemID;
            objcmnParam.ItemType = dataEntity.SupplierID === undefined ? 0 : dataEntity.SupplierID;
            objcmnParam.ItemGroup = dataEntity.BatchID === undefined ? 0 : dataEntity.BatchID;
            objcmnParam.UserType = dataEntity.UnitID === undefined ? 0 : dataEntity.UnitID;
            ModelsArray = [objcmnParam];
            var apiRoute = partialUrl + 'GetCurrentStock/';
            var defaultStock = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            defaultStock.then(function (response) {
                debugger
                dataEntity.Quantity = 0;
                dataEntity.CurrentStock = '';
                dataEntity.UnitPrice = 0;
                dataEntity.IsQtyReadOnly = true;

                if (response.data.singleCStock.CurrentStock > 0 && response.data.singleCStock.CurrentStock != null) {
                    dataEntity.CurrentStock = response.data.singleCStock.CurrentStock;
                    dataEntity.UnitPrice = response.data.singleCStock.UnitPrice;
                    dataEntity.IsQtyReadOnly = false;
                }
                //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //********************************************************End Load Current Stock***************************************************

        //*******************************************************Start Load Current Stock**************************************************
        $scope.CalculateStock = function (dataEntity) {
            debugger
            if (dataEntity.Quantity > dataEntity.CurrentStock || dataEntity.Quantity < 0 || dataEntity.Quantity === null || dataEntity.Quantity == '') {
                dataEntity.Quantity = 0;
                Command: toastr["warning"]("Your Inputed Quantity is invalid!");
            }
        }

        //***************************************************Start Set Master Dynamic Grid******************************************************

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
                $scope.loadAllChemConsumpMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllChemConsumpMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllChemConsumpMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllChemConsumpMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllChemConsumpMasterRecords(1);
                }
            }
        };

        $scope.loadAllChemConsumpMasterRecords = function (isPaging) {
            $scope.gridOptionsChemConsump.enableFiltering = true;

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
            $scope.gridOptionsChemConsump = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DyingConsumptionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DyingConsumptionNo", displayName: "Dying Consumption No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IndigoStart", displayName: "Indigo Start", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IndigoStop", displayName: "Indigo Stop", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BlackStart", displayName: "Black Start", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BlackStop", displayName: "Black Stop", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OperationName", displayName: "Operation Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Date", displayName: "Consumption Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptionsChemConsump.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsChemConsump.useExternalPagination = false;
                        $scope.gridOptionsChemConsump.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetAllConsumption/';
            var listChemProcessMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemProcessMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsChemConsump.data = response.data.finalList;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllChemConsumpMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //*************************************************End Show Chemical Process List Information Dynamic Grid*******************************************************

        //**********----Save----***************
        $scope.Save = function () {
            $scope.lblMessage = 'please wait....!';
            $scope.result = "color-red";
            var varDyingSetID = 0;
            if (angular.isObject($scope.ddlSetNo)) {
                varDyingSetID = $scope.ddlSetNo.DyingSetID;
            }
            else {
                varDyingSetID = $scope.ddlSetNo;
            }

            var varDyingProcessID = 0;
            if (angular.isObject($scope.ddlOperation)) {
                varDyingProcessID = $scope.ddlOperation.DyingProcessID;
            }
            else {
                varDyingProcessID = $scope.ddlOperation;
            }

            var master = {};
            var details = {};
            if ($scope.DyingConsumptionID == 0) {
                $scope.EntityState = "Insert";
            }
            else {
                $scope.EntityState = "Update";
            }
            try {

                master = {
                    DyingConsumptionID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.DyingConsumptionID,
                    DyingConsumptionNo: '',
                    SetID: varDyingSetID,
                    ItemID: $scope.ItemID,
                    Date: $scope.Date == '' ? null : conversion.getStringToDate($scope.Date),
                    IndigoStart: $scope.indigoStart,
                    IndigoStop: $scope.indigoStop,
                    BlackStart: $scope.blackStart,
                    BlackStop: $scope.blackStop,
                    OperationID: varDyingProcessID,
                    CompanyID: companyID,

                    EntityState: $scope.EntityState
                };
            } catch (e) {
                alert(e);
            }
            var EmptyList = false;
            try {
                details = $scope.ListConsumptionDetails;
                if ($scope.ListConsumptionDetails.length == 0) EmptyList = true;
                angular.forEach($scope.ListConsumptionDetails, function (value, key) {
                    if (value.ConsumptionChemicalMID == 0 && value.IsDeleted == false) {
                        EmptyList = true;
                        Command: toastr["info"]("Please Choose Chemical For All Details ");
                        return;
                    }
                });


            } catch (e) {
                alert(e);
            }

            if (EmptyList) return;

            //Invoke Method
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [master, details, $scope.UserCommonEntity];
            var apiRoute = baseUrl + 'Save/';
            $scope.loaderMore = true;
            var porcessMasterDetails = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            porcessMasterDetails.then(function (response) {
                response.data = response;
                if (response.data.result >= 1) {
                    $scope.clear();
                    response.data = 1;
                    ShowCustomToastrMessage(response);
                    //$scope.NewInstance();

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
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************** Modal Events ********************************
        $scope.Done = function () {
            $("#chemicalInfoModal").fadeIn(200, function () {
                $('#chemicalInfoModal').modal('hide');
            });

            angular.forEach($scope.ListConsumptionDetails, function (value, key) {
                if (value.DyingConsumptionDetailID == $scope.ListChemicalItem.DyingConsumptionDetailID) {
                    if (value.ConsumptionChemicalMID == 0 || value.ConsumptionChemicalMID > 0) {
                        if (value.ConsumptionChemicalMID == 0) {
                            value.ConsumptionChemicalMID = value.DyingConsumptionDetailID;
                        }


                        value.PrdDyingConsumptionChemicalM = {};

                        var targetObject = {};
                        var targetObjectDetails = {};
                        var targetObjectDetailsList = [];
                        debugger
                        var quantityCal = 0;
                        targetObject.ConsumptionChemicalMID = value.ConsumptionChemicalMID;
                        angular.forEach($scope.ListChemicalItem, function (value, key) {
                            if (value.ItemID != 0) {
                                quantityCal = parseInt(quantityCal) + parseInt(value.Quantity);
                            }
                        });
                        value.ConsumptionChemicalSum = quantityCal;
                        targetObject.TotalChemical = quantityCal;
                        targetObject.IsDeleted = false;
                        targetObject.EntityState = $scope.ListChemicalItem.EntityState;


                        debugger
                        angular.forEach($scope.ListChemicalItem, function (value, key) {

                            if (value.ItemID != 0) {
                                targetObjectDetails = {}
                                debugger
                                targetObjectDetails.ConsumptionChemicalID = 0;
                                targetObjectDetails.ConsumptionChemicalMID = targetObject.ConsumptionChemicalMID;
                                targetObjectDetails.ChemicalID = value.ItemID;
                                //targetObjectDetails.DepartmentID = $scope.departmentID;//null
                                targetObjectDetails.BatchID = value.BatchID;
                                targetObjectDetails.UnitID = value.UnitID;
                                targetObjectDetails.SupplierID = value.SupplierID;
                                targetObjectDetails.UnitPrice = value.UnitPrice;
                                

                                targetObjectDetails.IsDeleted = false;
                                targetObjectDetails.EntityState = $scope.ListChemicalItem.EntityState;
                                targetObjectDetails.Qty = value.Quantity;
                                targetObjectDetailsList.push(targetObjectDetails);

                            }
                        });

                        targetObject.PrdDyingConsumptionChemicals = targetObjectDetailsList;
                        value.PrdDyingConsumptionChemicalM = targetObject;
                    }
                }
            });

            var abc = $scope.ListConsumptionDetails;

        };
        $scope.Refresh = function () {

            angular.forEach($scope.ListChemicalItem, function (value, key) {
                //value.DepartmentID = null;
                value.BatchID = null;
                value.SupplierID = null;
                //value.UOM = null;
                value.Quantity = 0;
            });
            //$('.modalBatch').prop('selectedIndex', 0);
            //$('.modalUom').prop('selectedIndex', 0);
            //$('.modalQty').val('0');
        };

        //************************ Delete ******************
        $scope.deleteDetails = function (dataModel) {
            //var IsConf = confirm('You are about to delete Chemical ' + dataModel.ConsumptionChemicalSum + '. Are you sure?');
            //if (IsConf) {
            angular.forEach($scope.ListConsumptionDetails, function (value, key) {
                if (value.DyingConsumptionDetailID == dataModel.DyingConsumptionDetailID) {
                    value.IsDeleted = true;
                }
            });
            //}
        }
        $scope.deleteConsumption = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.DyingConsumptionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteConsumption/';
            var processLoadConsumption = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            processLoadConsumption.then(function (response) {
                if (response.data.result >= 1) {
                    $scope.RefreshMasterList();
                    response.data = -101;
                    ShowCustomToastrMessage(response);
                    //$scope.NewInstance();
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

        //************************************** Clear Instance *****************************************
        $scope.clear = function Clear(frm) {
            //try {
            //    frm.$setPristine();
            //    frm.$setUntouched();
            //} catch (e) {
            //}

            $scope.frmPrdDyingConsumptionMaster.$setPristine();
            $scope.frmPrdDyingConsumptionMaster.$setUntouched();

            $scope.ddlSetNo = null;
            $("#ddlSetNo").select2('val', '--Select Set No--');

            $scope.ddlOperation = null;
            $("#ddlOperation").select2('val', '--Select Operation--');

            $scope.indigoStart = "";
            $scope.articleNo = "";
            $scope.indigoStop = "";
            $scope.Date = conversion.NowDateCustom();
            $scope.blackStart = "";
            $scope.departmentName = "";
            $scope.blackStop = "";
            $scope.DyingConsumptionID = 0;
            $scope.ListConsumptionDetails = [];
            $scope.ListChemicalItem = [];
            GetDepartmentByCompayUserID(companyID, loggedUser);
        };
        //************ Reset Form *******************
        //$scope.NewInstance = function () {
        //    $scope.Clear(frmPrdDyingConsumptionMaster);
        //    $scope.DyingConsumptionID = 0;
        //    //$scope.btnSaveUpdateText = "Save";
        //    $scope.ListConsumptionDetails = [];
        //    $scope.ListChemicalItem = [];
        //    //loadConsumption_All(0);
        //}

    }]);


