/*
*   SizingOutputEntryCtrl.js
*/
app.controller('sizingOutputEntryCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/SizingOutputEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsSizingMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Sizing Output Master Entry';
        $scope.ListTitle = 'Sizing Output Records';
        $scope.ListTitleMaster = 'Sizing Master Records';
        $scope.PageTitleDetail = 'Sizing Output Detail Entry';
        $scope.btnSaveMachineStop = 'Save';
        $scope.btnSaveMachineBreaks = 'Save';
        $scope.btnAddMachineStop = 'Add';
        $scope.ModalMachineHeading = 'Machine Stop Details';
        $scope.BrekageModalHeading = 'Breakage Detail';
        $scope.ListSizeDetails = [];
        $scope.ListSizeDetailsDelete = [];
        $scope.ListMachineStop = [];
        $scope.ListMachineStops = [];
        var BreakType = "";
        $scope.listBeamNo = [];
        $scope.listShifts = [];
        $scope.listOperator = [];
        $scope.listOfficer = [];
        $scope.listBeamQuality = [];
        $scope.HoldDataModel = [];
        $scope.SlNo = 0;
        $scope.ListBreakTypesToSave = [];
        $scope.ItemID = 0;
        $scope.MainDetailIndexWiseData = "";
        $scope.tempMachineStopDetailID = 0;
        $scope.MachineStopModal = "";
        $scope.IsNextDate = false;

        $scope.MachineSave = true;
        //$scope.IsSaveDisable = true;
        //$scope.IsShowSave = true;
        $scope.IsShowDetail = true;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsfrmShow = true;
        $scope.IsbtnAddDelShow = true;
        $scope.ModelState = "";
        $scope.SizeMRRDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeletePrdSizingMasterDetail'; DelMsg = 'SizeMRRNo'; EditFunc = 'loadSetInfoRecords, loadMachineRecords, SizingDetailByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        $scope.LoadTimer = function () {
            debugger
            $scope.StopTime = conversion.NowTime();
            $scope.StartTime = conversion.NowTime();
            $scope.IsNextDate = false;
        }

        $scope.loadSetRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetNo/';
            var listSetNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetNo.then(function (response) {
                $scope.listSetNo = response.data.ListSet;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSetRecords(0);

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadMachineRecords = function (dataM) {
            debugger
            $scope.cmnParam();
            //objcmnParam.ItemType = 4; objcmnParam.ItemGroup = 48;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetMachine/';
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.ListMachine = response.data.ListMachine;
                //angular.forEach($scope.ListMachine, function (tempitem) {
                //    if (tempitem.MachineID == angular.isUndefined(dataM.MachineID) ? 13 : dataM.MachineID) {
                //        $scope.MachineID = tempitem.MachineID;
                //        $("#MachineList").select2("data", { id: 0, text: tempitem.MachineName });
                //    }
                //});
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadMachineRecords(0);
        //**************************************************End Machine Dropdown******************************************************

        //************************************************Start SetNo wise single Records******************************************************
        $scope.loadSetInfoRecords = function (lstSetNoList) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = (angular.isUndefined($scope.lstSetNoList) || $scope.lstSetNoList == "") && angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.SizeMRRID) ? 0 : lstSetNoList.SizeMRRID;
            objcmnParam.id = (angular.isUndefined($scope.lstSetNoList) || $scope.lstSetNoList == "") && angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.SetID) ? $scope.lstSetNoList : lstSetNoList.SetID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetInformation/';
            var ListSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSetInformation.then(function (response) {
                if (response.data.model == null) {
                    $scope.clear();
                }
                else {
                    $scope.ClearMasterData();
                    $scope.lstSetNoList = response.data.model.SetID;
                    $scope.ArticleNo = response.data.model.ArticleNo;
                    $scope.SetLength = response.data.model.SetLength;
                    $scope.ColorName = response.data.model.ColorName;
                    $scope.BuyerName = response.data.model.BuyerName;
                    $scope.YarnCount = response.data.model.YarnCount;
                    $scope.YarnRatioLot = response.data.model.YarnRatioLot;
                    $scope.NoOfBall = response.data.model.NoOfBall;
                    $scope.LeaseReapet = response.data.model.LeaseRepeat;
                    $scope.PINO = response.data.model.PINO;
                    $scope.TotalEnds = response.data.model.TotalEnds;
                    $scope.YarnRatio = response.data.model.YarnRatio;
                    $scope.EndsPerCreel = response.data.model.EndsPerCreel;
                    $scope.SupplierName = response.data.model.SupplierName;
                    $scope.ItemID = response.data.model.ItemID;
                    debugger
                    if (response.data.model.SizeMRRID != null) {
                        $scope.SizeMRRID = response.data.model.SizeMRRID;
                        $scope.IsHidden = true;
                        $scope.IsfrmShow = true;
                        $scope.IsShow = true;
                        //$scope.IsSaveDisable = false;
                        $scope.cmnbtnShowHideEnDisable('false');
                        $scope.IsbtnAddDelShow = true;
                        //$scope.btnSaveText = "Update";
                        $scope.btnSaveMachineBreaks = "Update";
                        //$scope.btnShowText = "Show List";
                        $scope.Description = response.data.model.Description;
                        $scope.SizeMRRDate = response.data.model.SizeMRRDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.model.SizeMRRDate);
                        debugger
                        angular.forEach($scope.ListMachine, function (machine) {
                            if (machine.MachineID == response.data.model.MachineID) {
                                debugger
                                $scope.MachineID = machine.MachineID;
                                $("#MachineList").select2("data", { id: 0, text: machine.MachineName });
                            }
                        });

                        angular.forEach($scope.listSetNo, function (set) {
                            if (set.SetID == lstSetNoList.SetID) {
                                $("#ddlSetNoList").select2("data", { id: 0, text: set.SetNo });
                            }
                        });
                        //$scope.IsShowSave = true;
                        $scope.IsShowDetail = true;
                    }
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.ClearMasterData = function () {
            $scope.lstSetNoList = "";
            $scope.ArticleNo = "";
            $scope.SetLength = "";
            $scope.ColorName = "";
            $scope.BuyerName = "";
            $scope.YarnCount = "";
            $scope.YarnRatioLot = "";
            $scope.NoOfBall = "";
            $scope.LeaseReapet = "";
            $scope.PINO = "";
            $scope.TotalEnds = "";
            $scope.YarnRatio = "";
            $scope.EndsPerCreel = "";
            $scope.SupplierName = "";
            $scope.ItemID = "";
        }

        //**************************************************End SetNo wise single Records******************************************************

        //*********************************************************Start Beam Dropdown*********************************************************

        $scope.loadSizingBeamRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBeams/';
            var listBeamtNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBeamtNo.then(function (response) {
                $scope.listBeamNo = response.data.BeamList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSizingBeamRecords(0);
        //***************************************************End Beam Dropdown******************************************************

        //************************************************Start Shift Dropdown******************************************************        
        $scope.loadSizingShiftRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
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
        $scope.loadSizingShiftRecords(0);
        //***************************************************End Shift Dropdown******************************************************

        //************************************************Start Operator Dropdown******************************************************        
        $scope.loadSizingOperatorRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
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
        $scope.loadSizingOperatorRecords(0);
        //***************************************************End Operator Dropdown******************************************************

        //************************************************Start Duty Officer Dropdown******************************************************        
        $scope.loadSizingOfficerRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetOperators/';
            var listOfficer = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listOfficer.then(function (response) {
                $scope.listOfficer = response.data.OperatorList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSizingOfficerRecords(0);
        //***************************************************End Duty Officer Dropdown******************************************************

        //************************************************Start Beam Quality Dropdown******************************************************        
        $scope.loadSizingBeamQualityRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.SizeMRRID) ? 0 : dataModel.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBeamQuality/';
            var listBeamQuality = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBeamQuality.then(function (response) {
                $scope.listBeamQuality = response.data.BeamQualityList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSizingBeamQualityRecords(0);
        //***************************************************End Beam Quality Dropdown******************************************************

        //************************************************Start Machine Stop Cause Dropdown******************************************************        
        $scope.LoadMachineStopCauses = function (dataModel) {
            debugger
            $scope.HoldDataModel = dataModel;
            $scope.cmnParam();
            objcmnParam.ItemType = 29;
            objcmnParam.id = angular.isUndefined(dataModel.tId) ? 0 : dataModel.tId;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetLoadMachineStopCauses/';
            var CauseList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            CauseList.then(function (response) {
                debugger
                $scope.CauseList = response.data.CausesList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //***************************************************End Machine Stop Cause Dropdown******************************************************

        //********************************************************Start Row Add in Detail Grid***********************************************        
        $scope.AddItemToList = function () {
            debugger
            $scope.ModelState = "Save";
            $scope.ListSizeDetails.push({
                SlNo: $scope.SlNo + 1, SizeMRRDetailID: 0, SizeMRRID: 0, OutputUnitID: 0,
                SizeMachineStopMasterID: 0, SizeBreakageMasterID: 0, BeamQualityID: 0,
                ShiftID: 0, OperatorID: 0, ShiftEngineerID: 0, TotalStop: '', TotalBCBreakage: '',
                TotalHSBreakage: '', SqueezingActual: '', OverallStretch: '', SqueezingSTD: '',
                BeamLength: '', BeginTime: conversion.NowTime(), EndTime: conversion.NowTime(),
                StartTime: '', StopTime: '', MachineSpeed: '', Description: '', IsModalShow: false,
                ModelState: $scope.ModelState
            });
            $scope.SlNo = $scope.SlNo + 1;
            //$scope.IsSaveDisable = true;
            $scope.cmnbtnShowHideEnDisable('true');
            $scope.IsShow = true;
        }
        //**********************************************************End Row Add in Detail Grid***********************************************

        //*****************************************************Start Row Remove from Main Detail Grid****************************************
        $scope.DeleteMainDetail = function (index) {
            debugger
            $scope.MainDetailIndexWiseData = $scope.ListSizeDetails[index];

            $scope.ListMachineStop = [];
            $scope.ListBreakTypes = [];
            $scope.ModelState = "Delete";

            angular.forEach($scope.ListMachineStops, function (delitemS) {
                if (delitemS.SNo == $scope.MainDetailIndexWiseData.SlNo && delitemS.ModelState == "Update") {
                    delitemS.ModelState = $scope.ModelState;
                }
            })

            if ($scope.ListMachineStops.length > 0) {
                angular.forEach($scope.ListMachineStops, function (keepStopList) {
                    if (keepStopList.SNo != $scope.MainDetailIndexWiseData.SlNo || keepStopList.ModelState == $scope.ModelState) {
                        $scope.ListMachineStop.push({
                            SizeMachineStopID: keepStopList.SizeMachineStopID,
                            tId: keepStopList.tId,
                            SNo: keepStopList.SNo,
                            StopTime: keepStopList.StopTime,
                            StartTime: keepStopList.StartTime,
                            BWSName: keepStopList.BWSName,
                            BWSID: keepStopList.BWSID,
                            Description: keepStopList.Description,
                            ShiftID: keepStopList.ShiftID,
                            MachineID: keepStopList.MachineID,
                            StopInMin: keepStopList.StopInMin,
                            IsNextDate: keepStopList.IsNextDate,
                            Status: keepStopList.Status,
                            ModelState: keepStopList.ModelState
                        });
                    }
                })
            }

            angular.forEach($scope.ListBreakTypesToSave, function (delitemB) {
                if (delitemB.SlNo == $scope.MainDetailIndexWiseData.SlNo && delitemB.ModelState == "Update") {
                    delitemB.ModelState = $scope.ModelState;
                }
            })

            if ($scope.ListBreakTypesToSave.length > 0) {
                angular.forEach($scope.ListBreakTypesToSave, function (KeepBreakList) {
                    if (KeepBreakList.SlNo != $scope.MainDetailIndexWiseData.SlNo || KeepBreakList.ModelState == $scope.ModelState) {
                        $scope.ListBreakTypes.push({
                            SizeBreakageID: KeepBreakList.SizeBreakageID,
                            SlNo: KeepBreakList.SlNo,
                            BWSID: KeepBreakList.BWSID,
                            BWSName: KeepBreakList.BWSName,
                            BreakageType: KeepBreakList.BreakageType,
                            MachineID: KeepBreakList.MachineID,
                            NoOfBreakage: KeepBreakList.NoOfBreakage,
                            ModelState: KeepBreakList.ModelState
                        });
                    }
                })
            }

            $scope.ListMachineStops = [];
            $scope.ListBreakTypesToSave = [];

            if ($scope.ListMachineStop.length > 0) {
                angular.forEach($scope.ListMachineStop, function (StopItem) {
                    $scope.ListMachineStops.push({
                        SizeMachineStopID: StopItem.SizeMachineStopID,
                        tId: StopItem.tId,
                        SNo: StopItem.SNo,
                        StopTime: StopItem.StopTime,
                        StartTime: StopItem.StartTime,
                        BWSName: StopItem.BWSName,
                        BWSID: StopItem.BWSID,
                        Description: StopItem.Description,
                        ShiftID: StopItem.ShiftID,
                        MachineID: StopItem.MachineID,
                        StopInMin: StopItem.StopInMin,
                        IsNextDate: StopItem.IsNextDate,
                        Status: StopItem.Status,
                        ModelState: StopItem.ModelState
                    })
                })
            }

            if ($scope.ListBreakTypes.length > 0) {
                angular.forEach($scope.ListBreakTypes, function (BreakList) {
                    $scope.ListBreakTypesToSave.push({
                        SizeBreakageID: BreakList.SizeBreakageID,
                        SlNo: BreakList.SlNo,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
                        BreakageType: BreakList.BreakageType,
                        MachineID: BreakList.MachineID,
                        NoOfBreakage: BreakList.NoOfBreakage,
                        ModelState: BreakList.ModelState
                    });
                })
            }

            if ($scope.MainDetailIndexWiseData.ModelState == "Update")
                $scope.ListSizeDetailsDelete.push({
                    SlNo: $scope.MainDetailIndexWiseData.SlNo, SizeMRRDetailID: $scope.MainDetailIndexWiseData.SizeMRRDetailID, SizeMRRID: $scope.MainDetailIndexWiseData.SizeMRRID,
                    OutputUnitID: $scope.MainDetailIndexWiseData.OutputUnitID, SizeMachineStopMasterID: $scope.MainDetailIndexWiseData.SizeMachineStopMasterID,
                    SizeBreakageMasterID: $scope.MainDetailIndexWiseData.SizeBreakageMasterID, BeamQualityID: $scope.MainDetailIndexWiseData.BeamQualityID,
                    ShiftID: $scope.MainDetailIndexWiseData.ShiftID, OperatorID: $scope.MainDetailIndexWiseData.OperatorID,
                    ShiftEngineerID: $scope.MainDetailIndexWiseData.ShiftEngineerID, TotalStop: $scope.MainDetailIndexWiseData.TotalStop,
                    TotalBCBreakage: $scope.MainDetailIndexWiseData.TotalBCBreakage, TotalHSBreakage: $scope.MainDetailIndexWiseData.TotalHSBreakage,
                    SqueezingActual: $scope.MainDetailIndexWiseData.SqueezingActual, OverallStretch: $scope.MainDetailIndexWiseData.OverallStretch,
                    SqueezingSTD: $scope.MainDetailIndexWiseData.SqueezingSTD, BeamLength: $scope.MainDetailIndexWiseData.BeamLength,
                    BeginTime: $scope.MainDetailIndexWiseData.BeginTime, EndTime: $scope.MainDetailIndexWiseData.EndTime, MachineSpeed: $scope.MainDetailIndexWiseData.MachineSpeed,
                    Description: $scope.MainDetailIndexWiseData.Description, IsModalShow: $scope.MainDetailIndexWiseData.IsModalShow, ModelState: $scope.ModelState
                });

            $scope.ListSizeDetails.splice(index, 1);
            $scope.ListMachineStop = [];
            $scope.ListBreakTypes = [];
            $scope.MainDetailIndexWiseData = "";

            angular.forEach($scope.ListSizeDetails, function (ItemCheck) {
                //$scope.IsSaveDisable = ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeginTime != "" && ItemCheck.BeamLength != "" ? false : true;
                if (ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeginTime != "" && ItemCheck.BeamLength != "") { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }

            })
            //$scope.IsSaveDisable = $scope.ListSizeDetails.length > 0 ? false : true;
            if ($scope.ListSizeDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            $scope.IsShow = $scope.ListSizeDetails.length > 0 ? true : false;
        }
        //*******************************************************End Row Remove from Main Detail Grid****************************************

        //***********************************Start Modal ShowHide Validation Based on ShiftID & OutputUnitID*********************************
        $scope.ModalShowHide = function (dataModel) {
            debugger
            $scope.UnitIDCount = [];

            if (dataModel.ShiftID != 0 && dataModel.OutputUnitID != 0) {

                angular.forEach($scope.ListSizeDetails, function (ItemUnit) {
                    if (ItemUnit.OutputUnitID == dataModel.OutputUnitID) {
                        $scope.UnitIDCount.push(ItemUnit.OutputUnitID);
                    }
                })

                if ($scope.UnitIDCount.length <= 1) {
                    dataModel.IsModalShow = true;
                }
                else {
                    dataModel.IsModalShow = false;
                    Command: toastr["warning"]("Size Beam Number Should be Unique");
                }
            }

            angular.forEach($scope.ListSizeDetails, function (ItemCheck) {
                //$scope.IsSaveDisable = ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeamLength != "" ? false : true;
                if (ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeamLength != "") { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            })
        }
        //**************************************End Modal ShowHide Validation Based on ShiftID & OutputUnitID************************************

        //*******************************************************Start Machine Stop Modal********************************************************
        $scope.modal_ConfShow = function (confdata) {
            debugger
            $scope.UserCommonEntity.EnableYes = false;
            $scope.UserCommonEntity.EnableConf = true;
            $scope.UserCommonEntity.DelMsgs = confdata;
            $('#CmnDeleteModal').modal({ show: true, backdrop: "static", keyboard: "false" });
        }

        $scope.OnChangeMachineTime = function () {
            debugger
            if ($scope.StopTime.getHours() > $scope.StartTime.getHours() && $scope.StopTime.getDate() == $scope.StartTime.getDate()) {
                var ConfMessage = 'Are you sure Machine Start time ' + conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes()) + ' will be the next day?';
                $scope.modal_ConfShow(ConfMessage);
                $scope.IsConfirm = function () {
                    debugger
                    $scope.IsNextDate = true;
                    $scope.StartTime = conversion.ChangeDateTime($scope.IsNextDate, $scope.StartTime.getHours(), $scope.StartTime.getMinutes());
                }
            }
            else if ($scope.StopTime.getHours() <= $scope.StartTime.getHours() && $scope.StopTime.getMinutes() <= $scope.StartTime.getMinutes() && $scope.StopTime.getDate() <= $scope.StartTime.getDate()) {
                $scope.IsNextDate = false;
                $scope.StartTime = conversion.ChangeDateTime($scope.IsNextDate, $scope.StartTime.getHours(), $scope.StartTime.getMinutes());
            }
        }

        $scope.LoadMachineDetail = function (dataModel) {
            debugger
            $scope.SizeMRRDetailIndex = $scope.ListSizeDetails.indexOf(dataModel);
            $scope.MainDetailIndexWiseData = dataModel;

            $scope.ListMachineStop = [];
            $scope.ClearStopModalForm();

            if ($scope.ListMachineStops.length != 0) {
                angular.forEach($scope.ListMachineStops, function (StopItem) {
                    if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete") {
                        $scope.ListMachineStop.push({
                            SizeMachineStopID: StopItem.SizeMachineStopID,
                            tId: StopItem.tId,
                            SNo: StopItem.SNo,
                            StopTime: conversion.get12HourFrom24(StopItem.StopTime),
                            StartTime: conversion.get12HourFrom24(StopItem.StartTime),
                            BWSName: StopItem.BWSName,
                            BWSID: StopItem.BWSID,
                            Description: StopItem.Description,
                            ShiftID: StopItem.ShiftID,
                            MachineID: StopItem.MachineID,
                            StopInMin: StopItem.StopInMin,
                            IsNextDate: StopItem.IsNextDate,
                            Status: StopItem.Status,
                            ModelState: StopItem.ModelState
                        })
                        dataModel.TotalStop = $scope.ListMachineStop.length;
                    }
                })
            }
            else {
                $scope.ListMachineStop = [];
                $scope.ClearStopModalForm();
            }
        }
        //*********************************************************End Machine Stop Modal********************************************************

        //***********************************************Start Add Item to List in Machine Stop Modal********************************************
        $scope.AddMachineStop = function (dataModel) {
            debugger
            if ($scope.StopTime.getTime() >= $scope.StartTime.getTime()) {
                Command: toastr["warning"]("Start time must greater than Stop time.");
                $scope.ChangeMachinebtnState();
                return;
            }
            $scope.ModelState = "Save";

            angular.forEach($scope.CauseList, function (cause) {
                if (cause.BWSID == $scope.BWSID) {
                    BWSName = cause.BWSName;
                }
            })

            debugger
            if ($scope.btnAddMachineStop == 'Add') {
                $scope.tempMachineStopDetailID = $scope.tempMachineStopDetailID + 1;

                obj = {
                    SizeMachineStopID: 0,
                    tId: $scope.tempMachineStopDetailID,
                    SNo: $scope.MainDetailIndexWiseData.SlNo,
                    StopTime: conversion.get12HourFrom24($scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes()),
                    StartTime: conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes()),
                    BWSName: BWSName,
                    BWSID: $scope.BWSID,
                    Description: $scope.StopDescription,
                    ShiftID: $scope.HoldDataModel.ShiftID,
                    MachineID: $scope.MachineID,
                    StopInMin: conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime),
                    IsNextDate: $scope.IsNextDate,
                    Status: 0,
                    ModelState: $scope.ModelState
                }

                if ($scope.ListMachineStops.length != 0 && $scope.ListMachineStop.length == 0) {
                    angular.forEach($scope.ListMachineStops, function (StopItem) {
                        if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete")
                            $scope.ListMachineStop.push({
                                SizeMachineStopID: StopItem.SizeMachineStopID,
                                tId: StopItem.tId,
                                SNo: StopItem.SNo,
                                StopTime: conversion.get12HourFrom24(StopItem.StopTime),
                                StartTime: conversion.get12HourFrom24(StopItem.StartTime),
                                BWSName: StopItem.BWSName,
                                BWSID: StopItem.BWSID,
                                Description: StopItem.Description,
                                ShiftID: StopItem.ShiftID,
                                MachineID: StopItem.MachineID,
                                StopInMin: StopItem.StopInMin,
                                IsNextDate: StopItem.IsNextDate,
                                Status: StopItem.Status,
                                ModelState: StopItem.ModelState
                            })
                    })
                }
                debugger
                $scope.ListMachineStop.push(obj);
            }
            else {
                angular.forEach($scope.ListMachineStop, function (MStop) {
                    if (MStop.SNo == $scope.MachineStopModal.SNo && MStop.tId == $scope.MachineStopModal.tId) {
                        MStop.StopTime = conversion.get12HourFrom24($scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes());
                        MStop.StartTime = conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes());
                        MStop.BWSID = $scope.BWSID;
                        MStop.BWSName = BWSName;
                        MStop.Description = $scope.StopDescription;
                        MStop.ShiftID = $scope.HoldDataModel.ShiftID;
                        MStop.MachineID = $scope.MachineID;
                        MStop.StopInMin = conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime);
                        MStop.IsNextDate = $scope.IsNextDate;
                        MStop.Status = $scope.MachineStopModal.SizeMachineStopID > 0 ? 1 : 0;
                    }
                })
            }

            $scope.TotalNoOfStops = $scope.ListMachineStop.length;
            $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
            $scope.MachineSave = false;
            $scope.LoadTimer();
            $scope.ChangeMachinebtnState();
        }
        //*************************************************End Add Item to List in Machine Stop Modal********************************************   
        $scope.ChangeMachinebtnState = function () {
            $scope.btnAddMachineStop = 'Add';
            $scope.btnSaveMachineStop = 'Save';
        }
        $scope.EditMachineStop = function (dataModel) {
            debugger
            $scope.btnAddMachineStop = 'Modify';
            $scope.btnSaveMachineStop = 'Update';

            $scope.MachineStopModal = dataModel;
            $scope.BWSID = dataModel.BWSID;
            angular.forEach($scope.CauseList, function (cause) {
                if (cause.BWSID == dataModel.BWSID) {
                    $("#ddlCauses").select2("data", { id: 0, text: cause.BWSName });
                }
            })

            if (dataModel.IsNextDate == true) {
                var Start = "1900-01-02T" + conversion.get24HourFromPM(dataModel.StartTime);
                $scope.IsNextDate = true;
            }
            else {
                var Start = "1900-01-01T" + conversion.get24HourFromPM(dataModel.StartTime);
            }

            var Stop = "1900-01-01T" + conversion.get24HourFromPM(dataModel.StopTime);
            debugger
            $scope.StartTime = conversion.getDateTimeToTimeSpan(Start);
            $scope.StopTime = conversion.getDateTimeToTimeSpan(Stop);
            $scope.StopDescription = dataModel.Description;
        }

        $scope.saveMachineStop = function () {
            debugger
            var txtStatusM = "Save";
            var c = 0;
            if ($scope.ListMachineStop.length > 0) {
                if ($scope.ListMachineStops.length > 0) {
                    angular.forEach($scope.ListMachineStops, function (savelistM) {
                        angular.forEach($scope.ListMachineStop, function (typelistM) {
                            if (savelistM.SNo == typelistM.SNo && savelistM.tId == typelistM.tId) {//&& savelistM.Status==1
                                savelistM.SizeMachineStopID = typelistM.SizeMachineStopID;
                                savelistM.tId = typelistM.tId;
                                savelistM.SNo = typelistM.SNo;
                                savelistM.StopTime = conversion.get24HourFromPM(typelistM.StopTime);
                                savelistM.StartTime = conversion.get24HourFromPM(typelistM.StartTime);
                                savelistM.BWSName = typelistM.BWSName;
                                savelistM.BWSID = typelistM.BWSID;
                                savelistM.Description = typelistM.Description;
                                savelistM.ShiftID = typelistM.ShiftID;
                                savelistM.MachineID = typelistM.MachineID;
                                savelistM.StopInMin = typelistM.StopInMin;
                                savelistM.IsNextDate = typelistM.IsNextDate;
                                savelistM.Status = typelistM.Status;
                                savelistM.ModelState = typelistM.ModelState;
                                txtStatusM = "Update";
                            }
                            else
                                if (typelistM.SNo == savelistM.SNo && typelistM.Status == 0) {
                                    $scope.ListMachineStops.push({
                                        SizeMachineStopID: typelistM.SizeMachineStopID,
                                        tId: typelistM.tId,
                                        SNo: typelistM.SNo,
                                        StopTime: conversion.get24HourFromPM(typelistM.StopTime),
                                        StartTime: conversion.get24HourFromPM(typelistM.StartTime),
                                        BWSName: typelistM.BWSName,
                                        BWSID: typelistM.BWSID,
                                        Description: typelistM.Description,
                                        ShiftID: typelistM.ShiftID,
                                        MachineID: typelistM.MachineID,
                                        StopInMin: typelistM.StopInMin,
                                        IsNextDate: typelistM.IsNextDate,
                                        Status: 1,
                                        ModelState: typelistM.ModelState
                                    })
                                    typelistM.Status = 1;
                                }
                        });
                    });
                }
                if (txtStatusM == "Save") {
                    angular.forEach($scope.ListMachineStop, function (StopItem) {
                        $scope.ListMachineStops.push({
                            SizeMachineStopID: StopItem.SizeMachineStopID,
                            tId: StopItem.tId,
                            SNo: StopItem.SNo,
                            StopTime: conversion.get24HourFromPM(StopItem.StopTime),
                            StartTime: conversion.get24HourFromPM(StopItem.StartTime),
                            BWSName: StopItem.BWSName,
                            BWSID: StopItem.BWSID,
                            Description: StopItem.Description,
                            ShiftID: StopItem.ShiftID,
                            MachineID: StopItem.MachineID,
                            StopInMin: StopItem.StopInMin,
                            IsNextDate: StopItem.IsNextDate,
                            Status: 1,
                            ModelState: StopItem.ModelState
                        })
                    })
                }
            }
        }

        //**********************************************Start Delete Item from List in Machine Stop Modal****************************************
        $scope.DeleteMachineStop = function (dataModel) {
            debugger
            $scope.ModelState = "Delete";
            $scope.ListMachineStop.splice($scope.ListMachineStop.indexOf(dataModel), 1);
            if (dataModel.Status == 1) {
                if (dataModel.ModelState == "Save") {
                    $scope.ListMachineStops.splice($scope.ListMachineStops.indexOf(dataModel), 1);
                }
                else if (dataModel.ModelState == "Update") {
                    angular.forEach($scope.ListMachineStops, function (Del) {
                        if (Del.SNo == dataModel.SNo && Del.tId == dataModel.tId) {
                            Del.ModelState = $scope.ModelState;
                        }
                    });
                }
            }
            if ($scope.ListMachineStop.length <= 0) {
                $scope.ClearStopModalForm();
            }
            $scope.TotalNoOfStops = $scope.ListMachineStop.length;
            $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
            $scope.MachineSave = false;
        }
        //**********************************************Start Delete Item from List in Machine Stop Modal****************************************

        //*********************************************************Start Machine Breakage Modal**************************************************        
        $scope.LoadBreakageDetailHS = function (dataModels) {
            debugger
            BreakType = 'HS'
            $scope.LoadBreakageDetail(dataModels);
        }
        $scope.LoadBreakageDetailBC = function (dataModels) {
            debugger
            BreakType = 'BC'
            $scope.LoadBreakageDetail(dataModels);
        }

        $scope.LoadBreakageDetail = function (dataModels) {
            debugger
            BreakType = BreakType;
            $scope.ListBreakTypes = [];
            $scope.TotalBreaks = '';
            $scope.ModelState = "Save";
            $scope.SizeMRRDetailIndex = $scope.ListSizeDetails.indexOf(dataModels);
            $scope.MainDetailIndexWiseData = dataModels;

            if ($scope.ListBreakTypesToSave.length != 0) {
                var ttlBreak = 0;
                angular.forEach($scope.ListBreakTypesToSave, function (BreakListHold) {
                    if (BreakListHold.SlNo == $scope.MainDetailIndexWiseData.SlNo && BreakListHold.BreakageType == BreakType) {
                        $scope.ListBreakTypes.push({
                            SizeBreakageID: BreakListHold.SizeBreakageID, SlNo: BreakListHold.SlNo, BWSID: BreakListHold.BWSID, BWSName: BreakListHold.BWSName,
                            BreakageType: BreakListHold.BreakageType, MachineID: BreakListHold.MachineID, NoOfBreakage: BreakListHold.NoOfBreakage,
                            ModelState: BreakListHold.ModelState
                        });

                        ttlBreak = ttlBreak + BreakListHold.NoOfBreakage;
                        $scope.TotalBreaks = ttlBreak;
                    }
                })
            }
            if ($scope.ListBreakTypes.length == 0) {
                $scope.cmnParam();
                objcmnParam.ItemType = 27;
                objcmnParam.id = angular.isUndefined(dataModels.SizeMRRID) ? 0 : dataModels.SizeMRRID;
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetLoadMachineBrekages/';
                var ListBreakTypes = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBreakTypes.then(function (response) {
                    debugger
                    angular.forEach(response.data.BrekagesList, function (BreakList) {
                        $scope.ListBreakTypes.push({
                            SizeBreakageID: 0, SlNo: $scope.MainDetailIndexWiseData.SlNo, BWSID: BreakList.BWSID, BWSName: BreakList.BWSName,
                            BreakageType: BreakType, MachineID: $scope.MachineID, NoOfBreakage: "", ModelState: $scope.ModelState
                        });
                    })
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.TotalMachineBreaks = function (dataModel) {
            debugger
            ////// Sum the breakages///////////////       
            $scope.qtyTotalBreaks = 0;
            angular.forEach($scope.ListBreakTypes, function (item) {
                if (item.NoOfBreakage > 0) {
                    var NoOfBreakage = item.NoOfBreakage;
                    $scope.qtyTotalBreaks = NoOfBreakage + $scope.qtyTotalBreaks;
                }
            });
            $scope.TotalBreaks = $scope.qtyTotalBreaks;
            if (BreakType == "HS") {
                $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalHSBreakage = $scope.TotalBreaks;
            }
            else {
                $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalBCBreakage = $scope.TotalBreaks;
            }
        }

        $scope.saveMachineBrekages = function () {
            debugger
            var txtStatus = "Save";
            if ($scope.ListBreakTypes.length > 0) {
                if ($scope.ListBreakTypesToSave.length > 0) {
                    angular.forEach($scope.ListBreakTypesToSave, function (savelist) {
                        angular.forEach($scope.ListBreakTypes, function (typelist) {
                            if (savelist.SlNo == typelist.SlNo && savelist.BreakageType == typelist.BreakageType && savelist.BWSID == typelist.BWSID) {
                                savelist.NoOfBreakage = typelist.NoOfBreakage;
                                txtStatus = "Update";
                            }
                        });
                    });
                }
                if (txtStatus == "Save") {
                    angular.forEach($scope.ListBreakTypes, function (BreakList) {
                        $scope.ListBreakTypesToSave.push({
                            SizeBreakageID: BreakList.SizeBreakageID, SlNo: BreakList.SlNo, BWSID: BreakList.BWSID, BWSName: BreakList.BWSName,
                            BreakageType: BreakList.BreakageType, MachineID: BreakList.MachineID, NoOfBreakage: BreakList.NoOfBreakage, ModelState: BreakList.ModelState
                        });
                    })
                }
            }
        }
        //***************************************************End Machine Breakage Modal******************************************************

        //***************************************************Start Set Master Dynamic Grid******************************************************
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
                $scope.loadSizingMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSizingMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadSizingMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSizingMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadSizingMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadSizingMasterRecords = function (isPaging) {
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
            $scope.gridOptionsSizingMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "SizeMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeMRRNo", displayName: "Size MRR No", title: "Size MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeMRRDate", displayName: "Size MRR Date", title: "Set Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Articale No", title: "Articale No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineName", displayName: "Machine Name", title: "Machine Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", title: "Set No", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsSizingMaster.useExternalPagination = false;
                        $scope.gridOptionsSizingMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetSizingMRRMaster/';
            ModelsArray = [objcmnParam];
            var listSizingMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSizingMaster.then(function (response) {

                angular.forEach(response.data.ListSizingMaster, function (items) {
                    if (items.SizeMRRDate == "1900-01-01T00:00:00") {
                        items.SizeMRRDate = "N/A";
                    }
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsSizingMaster.data = response.data.ListSizingMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.loadSizingMasterRecords(0);
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadSizingMasterRecords(0);
        }
        //***************************************************End Set Master Dynamic Grid******************************************************

        $scope.SizingDetailByID = function (Master) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(Master.SizeMRRID) ? 0 : Master.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRouteSizeDetail = baseUrl + 'GetSizingDetailByID/';
            var apiRouteStopDetail = baseUrl + 'GetStopDetailByID/';
            var apiRouteBreakDetail = baseUrl + 'GetBreakageDetailByID/';

            var listSizingDetail = crudService.postMultipleModel(apiRouteSizeDetail, ModelsArray, $scope.HeaderToken.get);
            listSizingDetail.then(function (response) {
                debugger
                $scope.ListSizeDetails = [];
                angular.forEach(response.data.ListSizingDetail, function (getMainDetail) {
                    $scope.SlNo = getMainDetail.SizeMRRDetailID;
                    $scope.ListSizeDetails.push({
                        SlNo: getMainDetail.SizeMRRDetailID, SizeMRRDetailID: getMainDetail.SizeMRRDetailID, SizeMRRID: getMainDetail.SizeMRRID,
                        OutputUnitID: getMainDetail.OutputUnitID, SizeMachineStopMasterID: getMainDetail.SizeMachineStopMasterID,
                        SizeBreakageMasterID: getMainDetail.SizeBreakageMasterID, BeamQualityID: getMainDetail.BeamQualityID,
                        ShiftID: getMainDetail.ShiftID, OperatorID: getMainDetail.OperatorID, ShiftEngineerID: getMainDetail.ShiftEngineerID,
                        TotalStop: getMainDetail.TotalStop, TotalBCBreakage: getMainDetail.TotalBCBreakage, TotalHSBreakage: getMainDetail.TotalHSBreakage,
                        SqueezingActual: getMainDetail.SqueezingActual, OverallStretch: getMainDetail.OverallStretch,
                        SqueezingSTD: getMainDetail.SqueezingSTD, BeamLength: getMainDetail.BeamLength, MachineSpeed: getMainDetail.MachineSpeed,
                        Description: getMainDetail.Description, IsModalShow: true, BeginTime: conversion.getDateTimeToTimeSpan('1900-01-01T' + getMainDetail.StartTime),
                        EndTime: conversion.getDateTimeToTimeSpan('1900-01-01T' + getMainDetail.StopTime), ModelState: "Update"
                    });
                })
            },
            function (error) {
                console.log("Error: " + error);
            });

            var listStopDetail = crudService.postMultipleModel(apiRouteStopDetail, ModelsArray, $scope.HeaderToken.get);
            listStopDetail.then(function (response) {
                debugger
                $scope.ListMachineStops = [];
                angular.forEach(response.data.ListStopDetail, function (StopItem) {
                    $scope.ListMachineStops.push({
                        SizeMachineStopID: StopItem.SizeMachineStopID,
                        tId: StopItem.SizeMachineStopID,
                        SNo: StopItem.SizeMRRDetailID,
                        StopTime: StopItem.StopTime,
                        StartTime: StopItem.StartTime,
                        BWSName: StopItem.BWSName,
                        BWSID: StopItem.BWSID,
                        Description: StopItem.Description,
                        ShiftID: StopItem.ShiftID,
                        MachineID: StopItem.MachineID,
                        StopInMin: StopItem.StopInMin,
                        IsNextDate: StopItem.IsNextDate,
                        Status: 1,
                        ModelState: "Update"
                    })
                })
            },
            function (error) {
                console.log("Error: " + error);
            });

            var listBreakDetail = crudService.postMultipleModel(apiRouteBreakDetail, ModelsArray, $scope.HeaderToken.get);
            listBreakDetail.then(function (response) {
                $scope.ListBreakTypesToSave = [];
                angular.forEach(response.data.ListBreakageDetail, function (BreakList) {
                    $scope.ListBreakTypesToSave.push({
                        SizeBreakageID: BreakList.SizeBreakageID,
                        SlNo: BreakList.SizeMRRDetailID,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
                        BreakageType: BreakList.BreakageType,
                        MachineID: BreakList.MachineID,
                        NoOfBreakage: BreakList.NoOfBreakage == 0 ? '' : BreakList.NoOfBreakage,
                        ModelState: "Update"
                    });
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //******************************************************Start Master List ShowHide***************************************************        
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                //$scope.IsfrmShow = true;
                //$scope.IsShowSave = true;
                //$scope.IsShowDetail = true;
                //$scope.IsShow = ($scope.ListSizeDetails.length < 1 || angular.isUndefined($scope.ListSizeDetails.length)) ? false : true;
            }
            else {
                //$scope.btnShowText = "Create";
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
                $scope.IsShowDetail = false;
            }
        };

        //********************************************************End Master List ShowHide**************************************************        

        //********************************************************End Save All Data*********************************************************
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var ItemMaster = {
                SizeMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.SizeMRRID,
                MachineID: $scope.MachineID,
                SizeMRRDate: $scope.SizeMRRDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.SizeMRRDate),
                ItemID: $scope.ItemID,
                SetID: $scope.lstSetNoList,
                Description: $scope.Description
            };

            if ($scope.ListSizeDetailsDelete.length > 0) {
                angular.forEach($scope.ListSizeDetailsDelete, function (DelItem) {
                    $scope.ListSizeDetails.push({
                        SlNo: DelItem.SlNo, SizeMRRDetailID: DelItem.SizeMRRDetailID, SizeMRRID: DelItem.SizeMRRID, OutputUnitID: DelItem.OutputUnitID,
                        SizeMachineStopMasterID: DelItem.SizeMachineStopMasterID, SizeBreakageMasterID: DelItem.SizeBreakageMasterID, BeamQualityID: DelItem.BeamQualityID,
                        ShiftID: DelItem.ShiftID, OperatorID: DelItem.OperatorID, ShiftEngineerID: DelItem.ShiftEngineerID, TotalStop: DelItem.TotalStop,
                        TotalBCBreakage: DelItem.TotalBCBreakage, TotalHSBreakage: DelItem.TotalHSBreakage, SqueezingActual: DelItem.SqueezingActual,
                        OverallStretch: DelItem.OverallStretch, SqueezingSTD: DelItem.SqueezingSTD, BeamLength: DelItem.BeamLength, BeginTime: DelItem.BeginTime,
                        EndTime: DelItem.EndTime, MachineSpeed: DelItem.MachineSpeed, Description: DelItem.Description, IsModalShow: DelItem.IsModalShow,
                        ModelState: DelItem.ModelState
                    });
                })
            }

            angular.forEach($scope.ListSizeDetails, function (NewItem) {
                NewItem.StartTime = NewItem.BeginTime.getHours() + ":" + NewItem.BeginTime.getMinutes();
                NewItem.StopTime = NewItem.EndTime.getHours() + ":" + NewItem.EndTime.getMinutes();
            })

            if ($scope.ListSizeDetails.length == 0) {
                Command: toastr["warning"]("Please input at least one Size Detail.");
                return;
            }
            debugger
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateSizing/';
            ModelsArray = [ItemMaster, $scope.ListSizeDetails, $scope.ListMachineStops, $scope.ListBreakTypesToSave, objcmnParam];
            var SaveUpdateSizeMRR = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateSizeMRR.then(function (response) {
                if (response.result != "") {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
            });
        };
        //********************************************************End Save All Data*********************************************************

        //********************************************************Start Delete Data*********************************************************
        $scope.DeletePrdSizingMasterDetail = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.SizeMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeletePrdSizingMasterDetail/';
            var delPrdSizingMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delPrdSizingMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("Size MRR No " + dataModel.SizeMRRNo + " has been deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Size MRR No " + dataModel.SizeMRRNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Size MRR No " + dataModel.SizeMRRNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
            //}
        }
        //*********************************************************End Delete Data**********************************************************

        //*********************************************************************Start Reset**********************************************************************
        $scope.ClearStopModalForm = function () {
            debugger
            $scope.ChangeMachinebtnState();
            $scope.LoadTimer();
            $scope.BWSID = "";
            $("#ddlCauses").select2("data", { id: '', text: '--Select Cause--' });
            $scope.StopDescription = "";
            $scope.ListMachineStop = [];
            if ($scope.ListSizeDetails.length > 0) {
                $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalStop = $scope.ListMachineStop.length == 0 ? "" : $scope.ListMachineStop.length;
            }
            $scope.MachineSave = true;
        }

        $scope.ClearStopModalList = function (dataModel) {
            debugger
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.ListSizeDetails[$scope.SizeMRRDetailIndex].TotalStop = "";
        }

        $scope.clear = function () {
            debugger
            $scope.frmSizingEntry.$setPristine();
            $scope.frmSizingEntry.$setUntouched();
            $scope.ArticleNo = '';
            $scope.SetLength = '';
            $scope.ColorName = '';
            $scope.BuyerName = '';
            $scope.Description = '';
            $scope.YarnCount = '';
            $scope.YarnRatioLot = '';
            $scope.NoOfBall = '';
            $scope.LeaseReapet = '';
            $scope.PINO = '';
            $scope.TotalEnds = '';
            $scope.YarnRatio = '';
            $scope.EndsPerCreel = '';
            $scope.SupplierName = '';
            $scope.SizeMRRDate = conversion.NowDateCustom();
            $scope.ListSizeDetails = [];
            //$scope.btnSaveText = "Save";
            $scope.btnSaveMachineBreaks = "Save";
            //$scope.btnShowText = "Show List";
            $scope.ModelState = "";
            $scope.ChangeMachinebtnState();
            $scope.lstSetNoList = "";
            $scope.MachineID = "";
            //$scope.IsSaveDisable = true;
            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsfrmShow = true;
            $scope.IsbtnAddDelShow = true;
            //$scope.IsShowSave = true;
            $scope.IsShowDetail = true;
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.ListSizeDetailsDelete = [];
            var BreakType = "";
            $scope.HoldDataModel = [];
            $scope.SlNo = 0;
            $scope.IsNextDate = false;
            $scope.ListBreakTypesToSave = [];
            $scope.ItemID = 0;
            $scope.MainDetailIndexWiseData = "";
            $scope.MachineSave = true;
            $("#ddlSetNoList").select2("data", { id: 0, text: '--Select Set No--' });
            $("#MachineList").select2("data", { id: 0, text: '--Select Machine--' });
            $scope.ClearStopModalForm();
        };
        //*********************************************************************End Reset*************************************************************************

    }]);