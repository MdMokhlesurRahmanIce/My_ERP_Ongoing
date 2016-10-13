/// <reference path="../Service/CrudService.js" />
/// <reference path="ChemicalSetupCtrl.js" />

app.controller('DyingChemicalConsumptionCtrl', ['$scope', 'commonComboBoxGetDataService', 'crudService', 'conversion', 'DyingChemicalConsumptionService', '$localStorage', '$rootScope', '$filter', 'PublicService',
    function ($scope, commonComboBoxGetDataService, crudService, conversion, DyingChemicalConsumptionService, $localStorage, $rootScope, $filter, PublicService) {
        
        var baseUrl = '/Production/api/DyingChemicalConsumption/';
        var dropDwonUrl = '/Production/api/ProductionDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        var companyID = $('#hCompanyID').val();
        var loggedUser = $('#hUserID').val();

        $scope.gridOptionsChemProcess = [];

        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        //$scope.ToogleDiv = 1;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsShowDetailMachine = false;
        //$scope.ToogleShowListButtonName = "New";
        //$scope.btnSaveUpdateText = "Save";
        $scope.PageTitle = 'Chemical Process Master Entry';
        $scope.MachineSetupTitle = 'Machine Setup';
        $scope.ListDetail = 'Chemical Process Detail Entry';
        $scope.ListMaster = 'Chemical Process Master List';
        $scope.DyingMRRID = 0;
        $scope.injectedObject = {};

        //DropDown Arrays Initialization
        $scope.loadLoadingProcessArticleNo = false;
        $scope.loadLoadingProcessMultiSetNo = false;
        $scope.LoadingMessage = 'Loading...';
        $scope.ListArticleNo = [];
        $scope.ListMultiSetNo = [];
        $scope.ListSetNo = [];
        $scope.ListRefferenceNo = [];
        $scope.ListShift = [];
        $scope.ListMrrDetails = [];
        $scope.ListMachineSetupDetails = [];
        $scope.OperationList = [];
        $scope.ProcessTypeList = [];
        //$scope.ListConsumptionProcess = [];
        $scope.ConsumptionDate = conversion.NowDateCustom();
        $scope.StartTime = conversion.NowTime();
        $scope.EndTime = conversion.NowTime();
        $scope.Time = conversion.NowTime();

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteChemicalProcessMasterDetail'; DelMsg = 'DyingMRRNo'; EditFunc = 'EditDyingConsumption, loadChemProcessDetail';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************

        //$scope.ShowList = function () {
        //    if ($scope.ToogleDiv == 0) {
        //        $scope.ToogleShowListButtonName = "Show List";
        //        $("#rowDetials").show(200);
        //        $("#prdDyingMRRMaster").show(200);
        //        $("#prdDyingMRRMasterEx").show(200);

        //        $("#rowList").hide(200);
        //        $scope.ToogleDiv = 1;
        //        loadPorcess_All(0);
        //        $scope.NewInstance();
        //        $scope.clear();
        //    }
        //    else {
        //        $scope.ToogleShowListButtonName = "New";
        //        $("#rowDetials").hide(200);
        //        $("#prdDyingMRRMaster").hide(200);
        //        $("#prdDyingMRRMasterEx").hide(200);
        //        $("#rowList").show(200);
        //        $scope.ToogleDiv = 0;           
        //    }
        //}
        //$scope.ShowList();

        //************************************************ Switch between show and hide ***********************************        
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
                $scope.IsShowDetailMachine = false;
            }
        }

        //**************** Load Shift Dropdown ************************
        function loadRecords_ShiftDropdown(item) {
            var apiRoute = dropDwonUrl + 'GetShift/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.ListShift = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_ShiftDropdown(0);
        //********************SetNo Modal Content***********************
        $scope.PageTitleSetNoModel = "Set No";
        //**************** Load MachineDropdown Dropdown ************************
        function loadRecords_MachineDropdown(ItemTypeID, ItemGroupID) {
            $scope.loadLoadingProcessMachine = true;
            $scope.result = "color-red";
            // var apiRoute = dropDwonUrl + 'GetDyingItemMachineByItemTypeGroup/';
            //var processDropdownLoad = commonComboBoxGetDataService.GetMachine(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemTypeID, ItemGroupID);
            var ItemID = 0;
            var DepartmentID = $scope.UserCommonEntity.loggedUserDepartmentID;
            var apiRoute = dropDwonUrl + 'GetMachine/';
            var processDropdownLoad = commonComboBoxGetDataService.GetMachine(apiRoute, companyID, loggedUser, page, pageSize, isPaging, DepartmentID, ItemID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListMachine = response.data;
                $scope.loadLoadingProcessMachine = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_MachineDropdown(1, 204);
        $scope.MachineChanged = function (Item) {
          //  //debugger
            try {
                if ($scope.ddlMachine.MachineConfigID == undefined || $scope.ddlMachine.MachineConfigID == null)
                    item = 0;
                else
                    item = $scope.ddlMachine.MachineConfigID;
            } catch (e) {
                item = 0;
            }
            $scope.loadRecords_MachineSetupDetails($scope.ddlCode.ItemID, item);
            //var boolClear = false;
            //try {
            //    angular.forEach($scope.ListMachine, function (value, key) {
            //        if (value.ItemID == Item.ItemID) {
            //            boolClear = true;
            //            $scope.Speed = value.Speed;
            //            $scope.Moiture = value.Moiture;
            //            $scope.KGPreMin = value.KGPreMin;
            //        }
            //    });
            //} catch (e) {

            //}
            //if (!boolClear) {
            //    $scope.Speed = "";
            //    $scope.Moiture = "";
            //    $scope.KGPreMin = "";
            //}
        };

        //**************** Load Set NO Dropdown ************************

        $scope.LoadSetNo = function (item) {
            if (item != undefined) {
                // loadRecords_SetNoDropdown(item);

                try {
                    if ($scope.ddlMachine.MachineConfigID == undefined || $scope.ddlMachine.MachineConfigID == null)
                        item = 0;
                    else
                        item = $scope.ddlMachine.MachineConfigID;
                } catch (e) {
                    item = 0;
                }
                loadRecords_MachineSetupDetails($scope.ddlCode.ItemID, 0);
                loadRecords_OperationSetup($scope.ddlCode.ItemID, 1);
                $scope.loadRecords_MachineSetupDetails($scope.ddlCode.ItemID, item);
            }
            else {
                $scope.buyer = "";
                $scope.PINO = "";
                $scope.supplier = "";
                $scope.Speed = "";
                $scope.Moiture = "";
                $scope.KGPreMin = "";
            }

        }
        $scope.loadRecords_MachineSetupDetails = function (ItemID, MachineID) {
            $scope.Speed = "";
            $scope.Moiture = "";
            $scope.KGPreMin = "";
            var varItemID = ItemID;
            var varMachineID = MachineID;
            if (ItemID == null) return;
            if (MachineID == null) varMachineID = 0;
            var apiRoute = dropDwonUrl + 'GetPrdDyingMachineSetupByItemMachine/';
            var processDropdownLoad = commonComboBoxGetDataService.GetMachine(apiRoute, companyID, loggedUser, page, pageSize, isPaging, varItemID, varMachineID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                if (response.data != null) {
                    $scope.Speed = response.data.Speed;
                    $scope.Moiture = response.data.Moiture;
                    $scope.KGPreMin = response.data.KGPreMin;
                }
                else {
                    $scope.Speed = "";
                    $scope.Moiture = "";
                    $scope.KGPreMin = "";
                }

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        function loadRecords_SetNoDropdown(item) {
            $scope.loadLoadingProcessSetNo = true;
            $scope.result = "color-red";
            var ItemID = item;
            var apiRoute = dropDwonUrl + 'GetDyingSetNoByItemID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListSetNo = response.data;
                $scope.ListRefferenceNo = response.data;
                $scope.loadLoadingProcessSetNo = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }


        $scope.LoadRefferenceOnEdit = function (item, RefSetID, DyingSetNo) {
            $scope.loadLoadingProcessSetNo = true;
            $scope.result = "color-red";
            var ItemID = item;
            var apiRoute = dropDwonUrl + 'GetDyingSetNoByItemID/';
            var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processDropdownLoad.then(function (response) {
                $scope.ListRefferenceNo = response.data;
                $scope.loadLoadingProcessSetNo = false;
                $("#ddlRefferenceNoDropdown").select2("data", { id: RefSetID, text: DyingSetNo.toString() });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        // loadRecords_SetNoDropdown(2);
        $scope.SetNoChanged = function (SetNo) {
            var boolClear = false;
            try {
                angular.forEach($scope.ListSetNo, function (value, key) {
                    if (value.DyingSetID == SetNo.DyingSetID) {
                        boolClear = true;
                        $scope.buyer = value.BuyerName;
                        $scope.PINO = value.DyingPINo;
                        $scope.supplier = value.SupplierName;
                    }
                });
            } catch (e) {

            }
            if (!boolClear) {
                $scope.buyer = "";
                $scope.PINO = "";
                $scope.supplier = "";
            }
        };

        //**************** Load Set MultiSelect Dropdown ************************
        function loadRecords_SetMultiselectDropdown(item) {
            $scope.MultiSetNoModel = [];
            $scope.loadLoadingProcessMultiSetNo = true;
            $scope.result = "color-red";
            var ItemID = item;
            var apiRoute = dropDwonUrl + 'GetSetNoByItemID/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.ListMultiSetNoModel = [];
                $scope.ListMultiSetNo = response.data;
                $scope.loadLoadingProcessMultiSetNo = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

            $scope.ListMultiSetNoSettings = {
                scrollableHeight: '100px',
                scrollable: true
            };
        }
        loadRecords_SetMultiselectDropdown(558);

        function loadRecords_SetMultiselectDropdownTemp(SetID, Data) {
            $scope.example14Model = [];
            $scope.example14data = Data;

            //    [
            //    { id: 1, label: "David" },
            //    { id: 2, label: "Jhon" },
            //    { id: 3, label: "Lisa" },
            //    { id: 4, label: "Nicole" },
            //    { id: 5, label: "Danny" },
            //    { id: 6, label: "Dan" },
            //    { id: 7, label: "Dean" },
            //    { id: 8, label: "Adam" },
            //    { id: 9, label: "Uri" },
            //    { id: 10, label: "Phil" }
            //];
            $scope.example14Settings = {
                scrollableHeight: '100px',
                scrollable: true
            };
        }
        // loadRecords_SetMultiselectDropdownTemp(0);
        $scope.InvokeSetModal = function (SetID) {
            
            try {
                loadRecords_SetMultiselectDropdown($scope.ddlCode.ItemID);
            } catch (e) {

            }
            
            $("#SetNoModel").fadeIn(200, function () { $('#SetNoModel').modal('show'); });
        }
        $scope.SubmitSetNoModel = function (SetID) {
            //Insert Setup
            var apiRoute = baseUrl + 'SaveMRRSet/';
            var porcessMasterDetails = DyingChemicalConsumptionService.postCommonMaster(apiRoute, $scope.ListMultiSetNoModel, $scope.UserCommonEntity, $scope.HeaderToken.post);
            porcessMasterDetails.then(function (response) {
                //debugger
                var responseID = response.data.result;
                var ItemID = $scope.ddlCode.ItemID;
                var apiRoute = dropDwonUrl + 'GetDyingSetNoByItemID/';
                var processDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
                processDropdownLoad.then(function (response) {
                    //debugger
                    $scope.ListRefferenceNo = [];
                    $scope.ListRefferenceNo = response.data;

                    angular.forEach($scope.ListRefferenceNo, function (value, key) {
                        if (value.DyingSetID == responseID) {

                            $scope.setNo = value.DyingSetNo;
                            $scope.ddlRefferenceNo = $scope.ListRefferenceNo;
                            $scope.ddlRefferenceNo.DyingSetID = responseID;
                            //$("#ddlRefferenceNoDropdown").select2('val', value.DyingSetNo.toString());
                            $("#ddlRefferenceNoDropdown").select2("data", { id: responseID, text: value.DyingSetNo.toString() });
                            $scope.buyer = value.BuyerName;
                            $scope.PINO = value.DyingPINo;
                            $scope.supplier = value.SupplierName;

                        }
                    });
                },
                function (error) {
                    console.log("Error: " + error);
                });

            },
            function (error) {
                console.log("Error: " + error);
            });

            $("#SetNoModel").fadeIn(200, function () { $('#SetNoModel').modal('hide'); });
            $scope.ListMultiSetNoModel = [];
        }
        // loadRecords_SetMultiselectDropdown(0);
        //************************************** Dropdown Multiselect ******************************
        //********************** Load Machine Setup And  Operation By ItemID ********************

        function loadRecords_MachineSetupDetails(MasterID, DetailsID) {
            var apiRoute = baseUrl + 'GetMachineSetupDetails/';
            var processMenues = DyingChemicalConsumptionService.GetAllByMasterDetails(apiRoute, companyID, loggedUser, page, pageSize, isPaging, MasterID, DetailsID, $scope.HeaderToken.get);
            processMenues.then(function (response) {
                $scope.ListMachineSetupDetails = [];
                var headerObject = {};
                var OperationObject = {};
                var SQObject = {};
                var TempObject = {};
                var SetupDetailsObject = {};
                var SetupObject = {};
                var MachinePartObject = {};
                var MachineOperatorObject = {};
                if (response.data.length > 1) {
                    //debugger
                    //Take Header 
                    angular.forEach(response.data[0].ItemArray, function (valueMachine, keyMachine) {
                        headerObject[valueMachine] = valueMachine;
                        angular.forEach(response.data[1].ItemArray, function (valueOperator, keyOperator) {
                            //debugger
                            if (keyMachine == keyOperator)
                                OperationObject[valueMachine] = valueOperator;
                        });
                        //debugger
                        angular.forEach(response.data[2].ItemArray, function (valueSQ, keySQ) {
                            //debugger
                            if (keyMachine == keySQ)
                                SQObject[valueMachine] = valueSQ;
                        });
                        angular.forEach(response.data[3].ItemArray, function (valueTemp, keyTemp) {
                            if (keyMachine == keyTemp)
                                TempObject[valueMachine] = valueTemp;
                        });
                        angular.forEach(response.data[4].ItemArray, function (valueSetupDetails, keySetupDetails) {
                            if (keyMachine == keySetupDetails)
                                SetupDetailsObject[valueMachine] = valueSetupDetails;
                        });
                        angular.forEach(response.data[5].ItemArray, function (valueSetup, keySetup) {
                            if (keyMachine == keySetup)
                                SetupObject[valueMachine] = valueSetup;
                        });
                        //debugger
                        angular.forEach(response.data[6].ItemArray, function (valueMachinePart, keyMachinePart) {
                            //debugger
                            if (keyMachine == keyMachinePart)
                                MachinePartObject[valueMachine] = valueMachinePart;
                        });
                        angular.forEach(response.data[7].ItemArray, function (valueMachineOperator, keyMachineOperator) {
                            //debugger
                            if (keyMachine == keyMachineOperator)
                                MachineOperatorObject[valueMachine] = valueMachineOperator;
                        });


                    });
                    //debugger
                    $scope.ListMachineSetupDetails.push(headerObject);
                    $scope.ListMachineSetupDetails.push(OperationObject);
                    $scope.ListMachineSetupDetails.push(SQObject);
                    $scope.ListMachineSetupDetails.push(TempObject);
                    $scope.ListMachineSetupDetails.push(SetupDetailsObject);
                    $scope.ListMachineSetupDetails.push(SetupObject);
                    $scope.ListMachineSetupDetails.push(MachinePartObject);
                    $scope.ListMachineSetupDetails.push(MachineOperatorObject);

                    //Take Header 
                    for (var i = 0; i < response.data.length; i++) {
                    }//for loop End
                }
                //$scope.IsShow = false;
                $scope.IsShowDetailMachine = $scope.ListMachineSetupDetails.length > 0 ? true : false;
                //if ($scope.ListMachineSetupDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //loadRecords_MachineSetupDetails(1, 12);
        function LoadValues(string, IsOperaion, SerialNo) {
            var data = $scope.ListOperationSetupTemp;
            var value;
            angular.forEach($scope.ListOperationSetupTemp, function (value1, key1) {
                angular.forEach(value1, function (propValue, propKey) {
                    if (string == propValue && value1.SLNo == SerialNo) {
                        data = $scope.ListOperationSetupTemp;
                        var a = value1;
                        var b = key1;
                        var a = propValue;
                        var b = propKey;
                        value = propValue;

                        if (IsOperaion) {
                            value = value1.OperationName;
                        }
                        else {
                            var varName = value1.MinQty + "/" + value1.MaxQty;
                            value = varName;
                        }


                    }
                });
            });
            return value;
        }
        function loadRecords_OperationSetup(ItemID, CompanyID) {
            var apiRoute = baseUrl + 'GetOperationSetup/';
            var processMenues = DyingChemicalConsumptionService.GetAllByMasterDetails(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, CompanyID, $scope.HeaderToken.get);
            processMenues.then(function (response) {
                $scope.ListOperationSetup = [];
                $scope.ListOperationSetupTemp = response.data;
                // Adding SLNo Witch Indicates Row No
                var processID = [];
                var MaxRowNo = 1;
                var num = 0;
                angular.forEach($scope.ListOperationSetupTemp, function (value, key) {
                    processID.push(value.DyingProcessID);
                    processID.push(value.ProcessName);
                    var filteredData = $filter('filter')(processID, value.DyingProcessID);
                    $scope.ListOperationSetupTemp[num].SLNo = filteredData.length;
                    if (MaxRowNo < filteredData.length) MaxRowNo = filteredData.length;
                    num++;
                });
                // Adding SLNo Witch Indicates Row No End
                //Taking TopHeader
                var operationSetupHeader = {};
                var operationSetupHeaderSecond = {};
                angular.forEach($scope.ListOperationSetupTemp, function (value, key) {
                    angular.forEach(value, function (propValue, propKey) {
                        if (propKey == "ProcessName" && !_.has(operationSetupHeader, propKey)) {
                            operationSetupHeader[propValue] = propValue;
                        }
                    });
                });
                $scope.ListOperationSetup.push(operationSetupHeader);
                //Taking TopHeader End
                //Taking Second Header As A Row        
                var objectArrayCustom = [];
                var object = {};
                angular.forEach(operationSetupHeader, function (value, key) {
                    object[value + "Operation"] = value;
                    object[value + "Name"] = value;
                });
                objectArrayCustom.push(object);
                for (i = 1; i <= MaxRowNo; i++) {
                    var object2 = {};
                    angular.forEach(operationSetupHeader, function (value, key) {
                        object2[value + "Operation"] = value;
                        object2[value + "Name"] = value;
                    });
                    objectArrayCustom.push(object2);
                    //Manupulating Array 
                    angular.forEach(objectArrayCustom[i], function (value, key) {

                        var obj = objectArrayCustom[i];
                        var SerialNo = i;
                        var IsOperaion = true;
                        if (!key.endsWith("Operation")) {
                            IsOperaion = false;
                        }
                        obj[key] = LoadValues(value, IsOperaion, SerialNo);
                    });
                    //Manupulating Array 
                }
                //Taking Second Header As A Row END

                // console.log($scope.ListOperationSetupTemp);
                $scope.ListOperationSetupHeader = [];
                $scope.ListOperationSetupHeader = operationSetupHeader;

                angular.forEach(objectArrayCustom[0], function (value, key) {
                    var obj = objectArrayCustom[0];
                    if (key.endsWith("Operation")) {
                        obj[key] = "Operation";
                    }
                    else {
                        obj[key] = "Name/Qty";
                    }

                });
                $scope.ListOperationSetup = objectArrayCustom;

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //loadRecords_OperationSetup(8, 12);
        //**************** Load Article No Dropdown ************************
        function loadRecords_ArticleNoDropDown(item) {
            $scope.loadLoadingProcessArticleNo = true;
            $scope.result = "color-red";
            var ItemID = item;
            var apiRoute = dropDwonUrl + 'GetItemMasterByItemID/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.ListArticleNo = response.data;
                $scope.loadLoadingProcessArticleNo = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_ArticleNoDropDown(1);

        //**************************** ADD Mrr Details ****************************************
        //**************** Load Process Type Dropdown ************************
        function loadRecords_DyingProcessType(processType) {
            //Send ProcessID =1
            var ProcessID = 8;
            var apiRoute = dropDwonUrl + 'GetDyingProcessByProcessID/';
            var process = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ProcessID, $scope.HeaderToken.get);
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
            var process = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ProcessID, $scope.HeaderToken.get);
            process.then(function (response) {
                $scope.OperationList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_DyingOperation(0);
        $scope.AddMrrDetails = function () {
            //Insertion Add  Begin
            $scope.EntityState = "Insert";
            $scope.ListMrrDetails.push({
                DyingMRRDetailID: $scope.ListMrrDetails.length + 1,
                DyingMRRID: $scope.DyingMRRID,
                DyingProcessID: 0,
                Time: "10:00",
                OperationID: 0,
                Quantity: 0,
                UnitID: 0,
                CompanyID: companyID,
                IsDeleted: false,
                EntityState: "Insert",
                CreateBy: null,
                CreateOn: null,
                CreatePc: null,
                UpdateBy: null,
                UpdateOn: null,
                UpdatePc: null,
                DeleteBy: null,
                DeleteOn: null,
                DeletePc: null
            });

            $scope.IsShow = $scope.ListMrrDetails.length > 0 ? true : false;
            if ($scope.ListMrrDetails.length > 0) { $scope.cmnbtnShowHideEnDisable("false"); } else { $scope.cmnbtnShowHideEnDisable("true"); }
        }

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
                $scope.loadAllChemProcessMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllChemProcessMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllChemProcessMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllChemProcessMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllChemProcessMasterRecords(1);
                }
            }
        };

        $scope.loadAllChemProcessMasterRecords = function (isPaging) {
            $scope.gridOptionsChemProcess.enableFiltering = true;

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
            $scope.gridOptionsChemProcess = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DyingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DyingMRRNo", displayName: "Dying MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DyingSetNo", displayName: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "KGPreMin", displayName: "KG/Min", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Moiture", displayName: "Moiture", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Date", displayName: "Dying Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptionsChemProcess.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsChemProcess.useExternalPagination = false;
                        $scope.gridOptionsChemProcess.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetAllProcess/';
            var listChemProcessMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemProcessMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsChemProcess.data = response.data.ListChemProcess;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllChemProcessMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //*************************************************End Show Chemical Process List Information Dynamic Grid*******************************************************

        //******************************************** Save Method **************************
        $scope.Save = function () {

            $scope.EntityState = "Insert";
            if ($scope.DyingMRRID > 0) $scope.EntityState = "Update";
            var master = {};
            var details = [];
            var varMachineID = 0;
            var varItemID = 0;
            var varRefSetID = 0;
            var varShiftID = 0;

            if (angular.isObject($scope.ddlMachine)) {
                varMachineID = $scope.ddlMachine.MachineConfigID;
            }
            else {
                varMachineID = $scope.ddlMachine;
            }
            if (angular.isObject($scope.ddlCode)) {
                varItemID = $scope.ddlCode.ItemID;
            }
            else {
                varItemID = $scope.ddlCode;
            }

            if (angular.isObject($scope.ddlRefferenceNo)) {
                varRefSetID = $scope.ddlRefferenceNo.DyingSetID;
            }
            else {
                varRefSetID = $scope.ddlRefferenceNo;
            }
            if (angular.isObject($scope.Shift)) {
                varShiftID = $scope.Shift.ShiftID;
            }
            else {
                varShiftID = $scope.Shift;
            }

            try {
                master = {
                    DyingMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.DyingMRRID,
                    DyingMRRNo: $scope.DyingMRRID.toString(),
                    DyingSetNo: $scope.setNo,
                    MachineID: varMachineID,
                    ItemID: varItemID,
                    KGPreMin: $scope.KGPreMin,
                    Moiture: $scope.Moiture,
                    Speed: $scope.Speed,
                    StartTime: $scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes(),
                    EndTime: $scope.EndTime.getHours() + ":" + $scope.EndTime.getMinutes(),
                    RefSetID: varRefSetID,
                    Date: $scope.ConsumptionDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ConsumptionDate),
                    RefSetDate: $scope.ConsumptionDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ConsumptionDate),
                    ShiftID: varShiftID,
                    CompanyID: $scope.UserCommonEntity.loggedCompany,
                    EntityState: $scope.EntityState,

                };

            }
            catch (e) {
                alert(e);
            }
            try {
                details = $scope.ListMrrDetails;
                angular.forEach(details, function (value, key) {
                    if (value.EntityState == "Insert") {
                        value.Time = $scope.Time.getHours() + ":" + $scope.Time.getMinutes();
                    }
                });
            }
            catch (e) {
                alert(e);
            }

            //Invoke Method
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [master, details, $scope.UserCommonEntity];
            var apiRoute = baseUrl + 'SaveMRR/';
            var porcessMasterDetails = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            porcessMasterDetails.then(function (response) {
              //  //debugger
                if (response.result >= 1) {
                    response.result = 1;
                    //ShowCustomToastrMessageResult(response.result);
                    //$scope.NewInstance();
                    $scope.clear();
                    //loadPorcess_All(0);
                    //$scope.ShowList();
                    ShowCustomToastrMessageResult(response.result);
                }
                else if (response.result == 0) //Erro
                {
                    response.result = 0;
                    ShowCustomToastrMessageResult(response.result);
                }
                else {
                    response.result = -1;
                    ShowCustomToastrMessageResult(response.result);
                }

            },
            function (error) {
                console.log("Error: " + error);
            });
            //Invoke Method

        };

        //******************************* get Process By ID *******************************************
        $scope.EditDyingConsumption = function (datamodel) {
            //$scope.ShowList();
            var ConsumptionID = datamodel.DyingMRRID;
            var apiRoute = baseUrl + 'GetProcessByID/';
            var processLoadMaster = DyingChemicalConsumptionService.getDetailByID(apiRoute, companyID, loggedUser, ConsumptionID, $scope.HeaderToken.get);
            processLoadMaster.then(function (response) {
                var masterdata = response.data;

                $scope.DyingMRRID = masterdata.DyingMRRID;
                $scope.setNo = masterdata.DyingSetNo;
                $scope.Speed = masterdata.Speed;
                $scope.Moiture = masterdata.Moiture;
                $scope.KGPreMin = masterdata.KGPreMin;
                $scope.ConsumptionDate = conversion.getDateToString(masterdata.Date);
                $scope.StartTime = conversion.GetDateTimeFromTime(masterdata.StartTime.toString());
                $scope.EndTime = conversion.GetDateTimeFromTime(masterdata.EndTime.toString());
                $scope.ddlCode = masterdata.ItemID;

                if (masterdata.ItemID != null)
                    $("#ddlArticleNo").select2('val', masterdata.ItemID.toString());

                $scope.ddlMachine = masterdata.MachineID;
                if (masterdata.MachineID != null)
                    $("#ddlMachine").select2('val', masterdata.MachineID.toString());
                $scope.Shift = masterdata.ShiftID;
                if (masterdata.ShiftID != null)
                    $("#Shift").select2('val', masterdata.ShiftID.toString());

                $scope.LoadRefferenceOnEdit(masterdata.ItemID, masterdata.RefSetID, masterdata.DyingSetNo);
                $scope.ddlRefferenceNo = masterdata.RefSetID;
                if (masterdata.RefSetID != null)
                    $("#ddlRefferenceNoDropdown").select2("data", { id: masterdata.RefSetID, text: masterdata.DyingSetNo.toString() });

                loadRecords_OperationSetup(masterdata.ItemID, companyID);
                $scope.loadRecords_MachineSetupDetails(masterdata.ItemID, 0);
                loadRecords_MachineSetupDetails(masterdata.ItemID, 0);
                //$scope.IsHidden = true;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.loadChemProcessDetail = function (datamodel) {
            //Load Details 
            var ConsumptionID = datamodel.DyingMRRID;
            var apiRouta = baseUrl + 'GetProcessDetailsByID/';
            var processLoadDetails = DyingChemicalConsumptionService.getDetailsByID(apiRouta, companyID, loggedUser, page, pageSize, isPaging, ConsumptionID, $scope.HeaderToken.get);
            processLoadDetails.then(function (response) {
                var detailsData = response.data;
                $scope.ListMrrDetails = response.data;
                $scope.Time = conversion.getDateTimeToTimeSpan('1900-01-01T' + $scope.ListMrrDetails[$scope.ListMrrDetails.length - 1].Time);

                $scope.IsShow = $scope.ListMrrDetails.length > 0 ? true : false;
                if ($scope.ListMrrDetails.length > 0) { $scope.cmnbtnShowHideEnDisable("false"); } else { $scope.cmnbtnShowHideEnDisable("true"); }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //************************************* Clear Set **************************************************

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteChemicalProcessMasterDetail = function (dataModel) {
          //  //debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.DyingMRRID;
            var apiRoute = baseUrl + 'DeleteChemicalProcessMasterDetail/';
            ModelsArray = [objcmnParam];
            var delChemProcessMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delChemProcessMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.DyingMRRNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.DyingMRRNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.DyingMRRNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
            //}
        }

        //********************************************************End Delete Master Detail***************************************************

        //************************************** Clear Instance *****************************************
        $scope.clear = function () {
            try {
                $scope.frmPrdDyingMRRMaster.$setPristine();
                $scope.frmPrdDyingMRRMaster.$setUntouched();
            } catch (e) {
            }

            $scope.DyingMRRNo = "";
            $scope.DyingSetNo = "";
            $scope.KGPreMin = 0;
            $scope.Moiture = 0;
            $scope.Speed = 0;

            $scope.buyer = "";
            $scope.setNo = "";
            $scope.PINO = "";
            $scope.supplier = "";

            $scope.ListMrrDetails = [];
            $scope.ListMachineSetupDetails = [];
            $scope.ListOperationSetup = [];
            $scope.DyingMRRID = 0;

            $scope.ddlCode = null;
            //$("#ddlArticleNo").select2('val', '--Select Article--');
            $scope.ddlMachine = null;
            $("#ddlMachine").select2('val', '--Select Machine--');
            $scope.ddlRefferenceNo = null;
            $("#ddlRefferenceNoDropdown").select2('val', '--Select Refference--');
            $scope.Shift = null;
            $("#Shift").select2('val', '--Select Shift--');

            $scope.StartTime = conversion.NowTime();
            $scope.EndTime = conversion.NowTime();
            $scope.Time = conversion.NowTime();
            $scope.ConsumptionDate = conversion.NowDateCustom();

            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsShowDetailMachine = false;
        };

        //************ Reset Form *******************
        //$scope.NewInstance = function () {
        //    $scope.clear(frmPrdDyingMRRMaster);
        //    //$scope.btnSaveUpdateText = "Save";
        //    $scope.ListMrrDetails = [];
        //    $scope.ListMachineSetupDetails = [];
        //    $scope.ListOperationSetup = [];
        //    $scope.DyingMRRID = 0;
        //};


        //********************************************** Item Modal Code *****************************************
        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;
        $scope.getListItemMaster = function (model) {
            var ItemID = model.ItemID;
            var ItemName = model.ItemName;
            $scope.ddlCode = {};
            $scope.ddlCode.ItemID = ItemID;
            $scope.ddlCodeName = ItemName;
            $scope.LoadSetNo(ItemID);

            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('hide'); });
        }
        $scope.modalClose = function () {
            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
        }
        $scope.SearchItem = function (serachItemName) {
            $scope.IsCallFromSearch = serachItemName == "" ? false : true;
            $scope.modalSearchItemName = serachItemName.toString();
            $scope.paginationItemMaster.pageNumber = 2;
            $scope.paginationItemMaster.firstPage();
        }
        //**********----Pagination Item Master List popup----***************
        $scope.paginationItemMaster = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

            getTotalPagesItemMaster: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationItemMaster.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadSampleNoModalRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPagesItemMaster()) {
                    this.pageNumber++;
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPagesItemMaster();
                    $scope.loadSampleNoModalRecords(1);
                }
            }
        };
        //**********----Get All Item Record by  select Sample No----***************//
        $scope.loadSampleNoModalRecords = function (isPaging) {
            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            // else {
            // For Loading modal
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });
            $('#ItemModal').modal({ show: true, backdrop: "static" });
            $scope.gridOptionslistItemMaster.enableFiltering = true;
            $scope.gridOptionslistItemMasterenableGridMenu = true;

            // For Loading
            if (isPaging == 0)
                $scope.paginationItemMaster.pageNumber = 1;
            // For Loading
            $scope.loaderMoreItemMaster = true;
            $scope.lblMessageItemMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize),
                pageSize: $scope.paginationItemMaster.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                selectedCompany: $scope.UserCommonEntity.loggedCompnyID,
                serachItemName: $scope.IsCallFromSearch == true ? $scope.modalSearchItemName : "100"
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {               
                rowTemplate: '<div ng-dblclick="grid.appScope.getListItemMaster(row.entity)" ng-repeat="col in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>',
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeight", displayName: "Weight", title: "Weight", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWidth", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.EnableUpdate,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Add' +
                                      '</a>' +
                                      '</span>'
                    }
                ],
                //onRegisterApi: function (gridApi) {
                //    $scope.gridApi = gridApi;
                //},
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                exporterCsvFilename: 'ItemSample.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
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

            // $scope.listItemMaster = [];
            var groupID = $scope.lstSampleNoList;
          //  //debugger
            // if (groupID > 0) {
            if (groupID == null || groupID == "" || groupID == undefined) {
                groupID = 0;
            }

            var apiRoute = '/SystemCommon/api/PublicApi/' + 'GetItemMaster/';
            var listItemMaster = PublicService.getItemMasterService(apiRoute, objcmnParam);
            listItemMaster.then(function (response) {
                //$scope.listItemMaster = response.data;
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

        };
        //****************************************************Another ***********************************

        //$scope.gridOptionslistItemMaster = {
        //    columnDefs: [
        //        { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "Construction", displayName: "Construction", title: "Construction", width: '19%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "FinishingWeight", displayName: "Weight", title: "Weight", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //        { name: "FinishingWidth", displayName: "Width", title: "Width", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
        //        {
        //            name: 'Action',
        //            displayName: "Action",
        //            width: '6%',
        //            enableColumnResizing: false,
        //            enableFiltering: false,
        //            enableSorting: false,
        //            headerCellClass: $scope.highlightFilteredHeader,
        //            visible: $scope.UserCommonEntity.EnableUpdate,
        //            cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
        //                          '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
        //                            '<i class="icon-check" aria-hidden="true"></i> Add' +
        //                          '</a>' +
        //                          '</span>'
        //        }
        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    },
        //    enableFiltering: true,
        //    enableGridMenu: true,
        //    enableSelectAll: true,
        //    exporterCsvFilename: 'ItemSample.csv',
        //    exporterPdfDefaultStyle: { fontSize: 9 },
        //    exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        //    exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        //    exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
        //    exporterPdfFooter: function (currentPage, pageCount) {
        //        return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //    },
        //    exporterPdfCustomFormatter: function (docDefinition) {
        //        docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
        //        docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //        return docDefinition;
        //    },
        //    exporterPdfOrientation: 'portrait',
        //    exporterPdfPageSize: 'LETTER',
        //    exporterPdfMaxGridWidth: 500,
        //    exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //};

    }]);



