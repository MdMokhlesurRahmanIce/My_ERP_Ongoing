/**
 * LongChainBeamCtrl.js
 */
app.controller('longChainBeamCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/LongChainBeamEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsLCBMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Long Chain Beam Entry';
        $scope.ListTitle = 'Long Chain Beam Records';
        $scope.ListTitleMaster = 'Long Chain Beam Master Records';
        $scope.PageTitleDetail = 'Long Chain Beam Detail';
        $scope.btnSaveMachineStop = 'Save';
        $scope.btnSaveMachineBreaks = 'Save';
        $scope.btnAddMachineStop = 'Add';
        $scope.ModalMachineHeading = 'Machine Stop Details';
        $scope.BrekageModalHeading = 'Breakage Detail';
        $scope.ListLCBDetails = [];
        $scope.ListLCBDetailsDelete = [];
        $scope.ListMachineStop = [];
        $scope.ListMachineStops = [];
        $scope.listBeamNo = [];
        $scope.listShifts = [];
        $scope.listOperator = [];
        $scope.listOfficer = [];
        $scope.listBeamQuality = [];
        $scope.HoldDataModel = [];
        $scope.SlNo = 0;
        $scope.ListBreakTypesToSave = [];
        $scope.ItemID = "";
        $scope.PIID = "";
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
        $scope.LCBMRRDate = conversion.NowDateCustom();
        var message = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeletePrdLCBMasterDetail'; DelMsg = 'LCBMRRNo'; EditFunc = 'LCBDetailByID, loadSetInfoRecords';
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
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;

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
            var apiRoute = baseUrl + 'GetMachine/';
            ModelsArray = [objcmnParam];
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.ListMachine = response.data.ListMachine;
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
            objcmnParam.ItemType = (angular.isUndefined($scope.lstSetNoList) || $scope.lstSetNoList == "") && angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.LCBMRRID) ? 0 : lstSetNoList.LCBMRRID;
            objcmnParam.id = (angular.isUndefined($scope.lstSetNoList) || $scope.lstSetNoList == "") && angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.SetID) ? $scope.lstSetNoList : lstSetNoList.SetID;

            var apiRoute = baseUrl + 'GetSetInformation/';
            ModelsArray = [objcmnParam];
            var ListSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSetInformation.then(function (response) {
                if (response.data == null) {
                    $scope.clear();
                }
                else {
                    $scope.ClearMasterData();
                    $scope.lstSetNoList = response.data.SetID;
                    $scope.ArticleNo = response.data.ArticleNo;
                    $scope.SetLength = response.data.SetLength;
                    $scope.ColorName = response.data.ColorName;
                    $scope.BuyerName = response.data.BuyerName;
                    $scope.YarnCount = response.data.YarnCount;
                    $scope.YarnRatioLot = response.data.YarnRatioLot;
                    $scope.NoOfBall = response.data.NoOfBall;
                    $scope.LeaseReapet = response.data.LeaseRepeat;
                    $scope.PINO = response.data.PINO;
                    $scope.TotalEnds = response.data.TotalEnds;
                    $scope.YarnRatio = response.data.YarnRatio;
                    $scope.EndsPerCreel = response.data.EndsPerCreel;
                    $scope.SupplierName = response.data.SupplierName;
                    $scope.ItemID = response.data.ItemID;
                    $scope.PIID = response.data.PIID;
                    debugger
                    if (response.data.LCBMRRID != null && response.data.LCBMRRID != 0) {
                        $scope.LCBMRRID = response.data.LCBMRRID;
                        $scope.IsHidden = true;
                        $scope.IsfrmShow = true;
                        $scope.IsShow = true;
                        //$scope.IsSaveDisable = false;
                        $scope.IsbtnAddDelShow = true;
                        //$scope.btnSaveText = "Update";
                        $scope.btnSaveMachineBreaks = "Update";
                        //$scope.btnShowText = "Show List";
                        $scope.Description = response.data.Description;

                        $scope.LCBMRRDate = response.data.LCBMRRDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.LCBMRRDate);

                        debugger
                        angular.forEach($scope.listSetNo, function (set) {
                            if (set.SetID == lstSetNoList.SetID) {
                                $("#ddlSetNoList").select2("data", { id: 0, text: set.SetNo });
                            }
                        });

                        //$scope.IsShowSave = true;
                        $scope.IsShowDetail = true;
                        //$scope.cmnbtnShowHideEnDisable(2);
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
            $scope.PIID = "";
        }

        //**************************************************End SetNo wise single Records******************************************************

        //*********************************************************Start Beam Dropdown*********************************************************

        $scope.loadLCBBeamRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;

            var apiRoute = baseUrl + 'GetBeams/';
            ModelsArray = [objcmnParam];
            var listBeamtNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBeamtNo.then(function (response) {
                $scope.listBeamNo = response.data.BeamList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadLCBBeamRecords(0);
        //***************************************************End Beam Dropdown******************************************************

        //************************************************Start Shift Dropdown******************************************************        
        $scope.loadLCBShiftRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;
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
        $scope.loadLCBShiftRecords(0);
        //***************************************************End Shift Dropdown******************************************************

        //************************************************Start Operator Dropdown******************************************************        
        $scope.loadLCBOperatorRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;
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
        $scope.loadLCBOperatorRecords(0);
        //***************************************************End Operator Dropdown******************************************************

        //************************************************Start Duty Officer Dropdown******************************************************        
        $scope.loadLCBOfficerRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;
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
        $scope.loadLCBOfficerRecords(0);
        //***************************************************End Duty Officer Dropdown******************************************************

        //************************************************Start Beam Quality Dropdown******************************************************        
        $scope.loadLCBBeamQualityRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.LCBMRRID) ? 0 : dataModel.LCBMRRID;
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
        $scope.loadLCBBeamQualityRecords(0);
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
            $scope.ListLCBDetails.push({
                SlNo: $scope.SlNo + 1, LCBMRRDetailID: 0, LCBMRRID: 0, OutputUnitID: 0,
                LCBMachineStopMasterID: 0, LCBBreakageMasterID: 0, BeamQualityID: 0,
                ShiftID: 0, OperatorID: 0, ShiftEngineerID: 0, TotalStop: '', TotalBreakage: '',
                MachineID: 0, DDate: conversion.NowDateDefault(), DDateString: '',
                BeamLength: '', BeginTime: conversion.NowTime(), EndTime: conversion.NowTime(),
                StartTime: '', StopTime: '', MachineSpeed: '', Description: '', IsModalShow: false,
                ModelState: $scope.ModelState
            });
            $scope.SlNo = $scope.SlNo + 1;
            $scope.cmnbtnShowHideEnDisable("true");
            //$scope.cmnButtonCall(0, 0);
            $scope.IsShow = true;
        }
        //**********************************************************End Row Add in Detail Grid***********************************************

        //*****************************************************Start Row Remove from Main Detail Grid****************************************
        $scope.DeleteMainDetail = function (index) {
            debugger
            $scope.MainDetailIndexWiseData = $scope.ListLCBDetails[index];

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
                            LCBMachineStopID: keepStopList.LCBMachineStopID,
                            tId: keepStopList.tId,
                            SNo: keepStopList.SNo,
                            StopTime: keepStopList.StopTime,
                            StartTime: keepStopList.StartTime,
                            BWSName: keepStopList.BWSName,
                            BWSID: keepStopList.BWSID,
                            Description: keepStopList.Description,
                            ShiftID: keepStopList.ShiftID,
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
                            LCBBreakageID: KeepBreakList.LCBBreakageID,
                            SlNo: KeepBreakList.SlNo,
                            BWSID: KeepBreakList.BWSID,
                            BWSName: KeepBreakList.BWSName,
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
                        LCBMachineStopID: StopItem.LCBMachineStopID,
                        tId: StopItem.tId,
                        SNo: StopItem.SNo,
                        StopTime: StopItem.StopTime,
                        StartTime: StopItem.StartTime,
                        BWSName: StopItem.BWSName,
                        BWSID: StopItem.BWSID,
                        Description: StopItem.Description,
                        ShiftID: StopItem.ShiftID,
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
                        LCBBreakageID: BreakList.LCBBreakageID,
                        SlNo: BreakList.SlNo,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
                        NoOfBreakage: BreakList.NoOfBreakage,
                        ModelState: BreakList.ModelState
                    });
                })
            }

            if ($scope.MainDetailIndexWiseData.ModelState == "Update")
                $scope.ListLCBDetailsDelete.push({
                    SlNo: $scope.MainDetailIndexWiseData.SlNo, LCBMRRDetailID: $scope.MainDetailIndexWiseData.LCBMRRDetailID, LCBMRRID: $scope.MainDetailIndexWiseData.LCBMRRID,
                    OutputUnitID: $scope.MainDetailIndexWiseData.OutputUnitID, LCBMachineStopMasterID: $scope.MainDetailIndexWiseData.LCBMachineStopMasterID,
                    LCBBreakageMasterID: $scope.MainDetailIndexWiseData.LCBBreakageMasterID, BeamQualityID: $scope.MainDetailIndexWiseData.BeamQualityID,
                    ShiftID: $scope.MainDetailIndexWiseData.ShiftID, OperatorID: $scope.MainDetailIndexWiseData.OperatorID,
                    ShiftEngineerID: $scope.MainDetailIndexWiseData.ShiftEngineerID, TotalStop: $scope.MainDetailIndexWiseData.TotalStop,
                    TotalBreakage: $scope.MainDetailIndexWiseData.TotalBreakage, MachineID: $scope.MainDetailIndexWiseData.MachineID,
                    DDate: $scope.MainDetailIndexWiseData.DDate, DDateString: $scope.MainDetailIndexWiseData.DDateString, BeamLength: $scope.MainDetailIndexWiseData.BeamLength,
                    BeginTime: $scope.MainDetailIndexWiseData.BeginTime, EndTime: $scope.MainDetailIndexWiseData.EndTime, MachineSpeed: $scope.MainDetailIndexWiseData.MachineSpeed,
                    Description: $scope.MainDetailIndexWiseData.Description, IsModalShow: $scope.MainDetailIndexWiseData.IsModalShow, ModelState: $scope.ModelState
                });

            $scope.ListLCBDetails.splice(index, 1);
            $scope.ListMachineStop = [];
            $scope.ListBreakTypes = [];
            $scope.MainDetailIndexWiseData = "";

            angular.forEach($scope.ListLCBDetails, function (ItemCheck) {
                if (ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeginTime != "" && ItemCheck.BeamLength != "") { $scope.cmnbtnShowHideEnDisable("false"); } else { $scope.cmnbtnShowHideEnDisable("true"); }
            })
            if ($scope.ListLCBDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            //$scope.IsbtnSaveDisable = $scope.ListLCBDetails.length > 0 ? false : true;
            $scope.IsShow = $scope.ListLCBDetails.length > 0 ? true : false;
        }
        //*******************************************************End Row Remove from Main Detail Grid****************************************

        //***********************************Start Modal ShowHide Validation Based on ShiftID & OutputUnitID*********************************
        $scope.ModalShowHide = function (dataModel) {
            debugger
            $scope.UnitIDCount = [];

            if (dataModel.ShiftID != 0 && dataModel.OutputUnitID != 0 && dataModel.MachineID != 0) {

                angular.forEach($scope.ListLCBDetails, function (ItemUnit) {
                    if (ItemUnit.OutputUnitID == dataModel.OutputUnitID) {
                        $scope.UnitIDCount.push(ItemUnit.OutputUnitID);
                    }
                })

                if ($scope.UnitIDCount.length <= 1) {
                    dataModel.IsModalShow = true;
                }
                else {
                    dataModel.IsModalShow = false;
                    Command: toastr["warning"]("LCB Beam Number Should be Unique");
                }
            }

            angular.forEach($scope.ListLCBDetails, function (ItemCheck) {
                if (ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeamLength != "") { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                //$scope.IsbtnSaveDisable = ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeamLength != "" ? false : true;
            })
        }
        //**************************************End Modal ShowHide Validation Based on ShiftID & OutputUnitID************************************

        //*******************************************************Start Machine Stop Modal******************************************************** 
        $scope.modal_ConfShow = function (confdata) {
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
            $scope.LCBMRRDetailIndex = $scope.ListLCBDetails.indexOf(dataModel);
            $scope.MainDetailIndexWiseData = dataModel;

            $scope.ListMachineStop = [];
            $scope.ClearStopModalForm();

            if ($scope.ListMachineStops.length != 0) {
                angular.forEach($scope.ListMachineStops, function (StopItem) {
                    if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete") {
                        $scope.ListMachineStop.push({
                            LCBMachineStopID: StopItem.LCBMachineStopID,
                            tId: StopItem.tId,
                            SNo: StopItem.SNo,
                            StopTime: conversion.get12HourFrom24(StopItem.StopTime),
                            StartTime: conversion.get12HourFrom24(StopItem.StartTime),
                            BWSName: StopItem.BWSName,
                            BWSID: StopItem.BWSID,
                            Description: StopItem.Description,
                            ShiftID: StopItem.ShiftID,
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
                    LCBMachineStopID: 0,
                    tId: $scope.tempMachineStopDetailID,
                    SNo: $scope.MainDetailIndexWiseData.SlNo,
                    StopTime: conversion.get12HourFrom24($scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes()),
                    StartTime: conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes()),
                    BWSName: BWSName,
                    BWSID: $scope.BWSID,
                    Description: $scope.StopDescription,
                    ShiftID: $scope.HoldDataModel.ShiftID,
                    StopInMin: conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime),
                    IsNextDate: $scope.IsNextDate,
                    Status: 0,
                    ModelState: $scope.ModelState
                }

                if ($scope.ListMachineStops.length != 0 && $scope.ListMachineStop.length == 0) {
                    angular.forEach($scope.ListMachineStops, function (StopItem) {
                        if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete")
                            $scope.ListMachineStop.push({
                                LCBMachineStopID: StopItem.LCBMachineStopID,
                                tId: StopItem.tId,
                                SNo: StopItem.SNo,
                                StopTime: conversion.get12HourFrom24(StopItem.StopTime),
                                StartTime: conversion.get12HourFrom24(StopItem.StartTime),
                                BWSName: StopItem.BWSName,
                                BWSID: StopItem.BWSID,
                                Description: StopItem.Description,
                                ShiftID: StopItem.ShiftID,
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
                        MStop.StopInMin = conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime);
                        MStop.IsNextDate = $scope.IsNextDate;
                        MStop.Status = $scope.MachineStopModal.SizeMachineStopID > 0 ? 1 : 0;
                    }
                })
            }

            $scope.TotalNoOfStops = $scope.ListMachineStop.length;
            $scope.ListLCBDetails[$scope.LCBMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
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
                                savelistM.LCBMachineStopID = typelistM.LCBMachineStopID;
                                savelistM.tId = typelistM.tId;
                                savelistM.SNo = typelistM.SNo;
                                savelistM.StopTime = conversion.get24HourFromPM(typelistM.StopTime);
                                savelistM.StartTime = conversion.get24HourFromPM(typelistM.StartTime);
                                savelistM.BWSName = typelistM.BWSName;
                                savelistM.BWSID = typelistM.BWSID;
                                savelistM.Description = typelistM.Description;
                                savelistM.ShiftID = typelistM.ShiftID;
                                savelistM.StopInMin = typelistM.StopInMin;
                                savelistM.IsNextDate = typelistM.IsNextDate;
                                savelistM.Status = typelistM.Status;
                                savelistM.ModelState = typelistM.ModelState;
                                txtStatusM = "Update";
                            }
                            else
                                //angular.forEach($scope.ListMachineStops, function (StopsItems) {
                                if (typelistM.SNo == savelistM.SNo && typelistM.Status == 0) {
                                    $scope.ListMachineStops.push({
                                        LCBMachineStopID: typelistM.LCBMachineStopID,
                                        tId: typelistM.tId,
                                        SNo: typelistM.SNo,
                                        StopTime: conversion.get24HourFromPM(typelistM.StopTime),
                                        StartTime: conversion.get24HourFromPM(typelistM.StartTime),
                                        BWSName: typelistM.BWSName,
                                        BWSID: typelistM.BWSID,
                                        Description: typelistM.Description,
                                        ShiftID: typelistM.ShiftID,
                                        StopInMin: typelistM.StopInMin,
                                        IsNextDate: typelistM.IsNextDate,
                                        Status: 1,
                                        ModelState: typelistM.ModelState
                                    })
                                    typelistM.Status = 1;
                                    //})
                                }

                        });
                    });
                }
                if (txtStatusM == "Save") {
                    angular.forEach($scope.ListMachineStop, function (StopItem) {
                        $scope.ListMachineStops.push({
                            LCBMachineStopID: StopItem.LCBMachineStopID,
                            tId: StopItem.tId,
                            SNo: StopItem.SNo,
                            StopTime: conversion.get24HourFromPM(StopItem.StopTime),
                            StartTime: conversion.get24HourFromPM(StopItem.StartTime),
                            BWSName: StopItem.BWSName,
                            BWSID: StopItem.BWSID,
                            Description: StopItem.Description,
                            ShiftID: StopItem.ShiftID,
                            StopInMin: StopItem.StopInMin,
                            IsNextDate: StopItem.IsNextDate,
                            Status: 1,
                            ModelState: StopItem.ModelState
                        })
                    })
                }
            }
            //$scope.MachineSave = true;
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
            $scope.ListLCBDetails[$scope.LCBMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
            $scope.MachineSave = false;
        }
        //**********************************************Start Delete Item from List in Machine Stop Modal****************************************

        //*********************************************************Start Machine Breakage Modal**************************************************        
        $scope.LoadBreakageDetail = function (dataModels) {
            debugger
            $scope.ListBreakTypes = [];
            $scope.TotalBreaks = '';
            $scope.ModelState = "Save";
            $scope.LCBMRRDetailIndex = $scope.ListLCBDetails.indexOf(dataModels);
            $scope.MainDetailIndexWiseData = dataModels;

            if ($scope.ListBreakTypesToSave.length != 0) {
                var ttlBreak = 0;
                angular.forEach($scope.ListBreakTypesToSave, function (BreakListHold) {
                    if (BreakListHold.SlNo == $scope.MainDetailIndexWiseData.SlNo) {
                        $scope.ListBreakTypes.push({
                            LCBBreakageID: BreakListHold.LCBBreakageID, SlNo: BreakListHold.SlNo, BWSID: BreakListHold.BWSID,
                            BWSName: BreakListHold.BWSName, NoOfBreakage: BreakListHold.NoOfBreakage, ModelState: BreakListHold.ModelState
                        });

                        ttlBreak = ttlBreak + BreakListHold.NoOfBreakage;
                        $scope.TotalBreaks = ttlBreak;
                    }
                })
            }
            if ($scope.ListBreakTypes.length == 0) {
                $scope.cmnParam();
                objcmnParam.ItemType = 27;
                objcmnParam.id = angular.isUndefined(dataModels.LCBMRRID) ? 0 : dataModels.LCBMRRID;
                ModelsArray = [objcmnParam];
                var apiRoute = baseUrl + 'GetLoadMachineBrekages/';
                var ListBreakTypes = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBreakTypes.then(function (response) {
                    debugger
                    angular.forEach(response.data.BrekagesList, function (BreakList) {
                        $scope.ListBreakTypes.push({
                            LCBBreakageID: 0, SlNo: $scope.MainDetailIndexWiseData.SlNo, BWSID: BreakList.BWSID,
                            BWSName: BreakList.BWSName, NoOfBreakage: "", ModelState: $scope.ModelState
                        });
                    })
                },
                function (error) {
                    console.log("Error: " + error);
                });
                //$scope.ClearStopModalForm();
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
            $scope.ListLCBDetails[$scope.LCBMRRDetailIndex].TotalBreakage = $scope.TotalBreaks;

        }

        $scope.saveMachineBrekages = function () {
            debugger
            var txtStatus = "Save";
            if ($scope.ListBreakTypes.length > 0) {
                if ($scope.ListBreakTypesToSave.length > 0) {
                    angular.forEach($scope.ListBreakTypesToSave, function (savelist) {
                        angular.forEach($scope.ListBreakTypes, function (typelist) {
                            if (savelist.SlNo == typelist.SlNo && savelist.BWSID == typelist.BWSID) {
                                savelist.NoOfBreakage = typelist.NoOfBreakage;
                                txtStatus = "Update";
                            }
                        });
                    });
                }
                if (txtStatus == "Save") {
                    angular.forEach($scope.ListBreakTypes, function (BreakList) {
                        $scope.ListBreakTypesToSave.push({
                            LCBBreakageID: BreakList.LCBBreakageID, SlNo: BreakList.SlNo, BWSID: BreakList.BWSID,
                            BWSName: BreakList.BWSName, NoOfBreakage: BreakList.NoOfBreakage, ModelState: BreakList.ModelState
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
                $scope.loadLCBMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadLCBMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadLCBMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadLCBMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadLCBMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadLCBMasterRecords = function (isPaging) {
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
            $scope.gridOptionsLCBMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "LCBMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCBMRRNo", displayName: "LCB MRR No", title: "LCB MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCBMRRDate", displayName: "LCB MRR Date", title: "Set Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Articale No", title: "Articale No", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsLCBMaster.useExternalPagination = false;
                        $scope.gridOptionsLCBMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetLCBMRRMaster/';
            var listLCBMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listLCBMaster.then(function (response) {
                angular.forEach(response.data.ListLCBMaster, function (items) {
                    if (items.LCBMRRDate == "1900-01-01T00:00:00") {
                        items.LCBMRRDate = "N/A";
                    }
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsLCBMaster.data = response.data.ListLCBMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.loadLCBMasterRecords(0);
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadLCBMasterRecords(0);
        }
        //***************************************************End Set Master Dynamic Grid******************************************************

        $scope.LCBDetailByID = function (Master) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(Master.LCBMRRID) ? 0 : Master.LCBMRRID;
            var apiRouteLCBDetail = baseUrl + 'GetLCBDetailByID/';
            var apiRouteStopDetail = baseUrl + 'GetStopDetailByID/';
            var apiRouteBreakDetail = baseUrl + 'GetBreakageDetailByID/';
            ModelsArray = [objcmnParam];
            var listLCBDetail = crudService.postMultipleModel(apiRouteLCBDetail, ModelsArray, $scope.HeaderToken.get);
            listLCBDetail.then(function (response) {
                debugger
                $scope.ListLCBDetails = [];
                angular.forEach(response.data.ListLCBDetail, function (getMainDetail) {
                    $scope.SlNo = getMainDetail.LCBMRRDetailID;
                    $scope.ListLCBDetails.push({
                        SlNo: getMainDetail.LCBMRRDetailID, LCBMRRDetailID: getMainDetail.LCBMRRDetailID, LCBMRRID: getMainDetail.LCBMRRID,
                        OutputUnitID: getMainDetail.OutputUnitID, LCBMachineStopMasterID: getMainDetail.LCBMachineStopMasterID,
                        LCBBreakageMasterID: getMainDetail.LCBBreakageMasterID, BeamQualityID: getMainDetail.BeamQualityID,
                        ShiftID: getMainDetail.ShiftID, OperatorID: getMainDetail.OperatorID, ShiftEngineerID: getMainDetail.ShiftEngineerID,
                        TotalStop: getMainDetail.TotalStop, TotalBreakage: getMainDetail.TotalBreakage, MachineID: getMainDetail.MachineID,
                        DDate: conversion.getDateTimeToTimeSpan(getMainDetail.DDate), DDateString: '', BeamLength: getMainDetail.BeamLength,
                        MachineSpeed: getMainDetail.MachineSpeed, Description: getMainDetail.Description, IsModalShow: true,
                        BeginTime: conversion.getDateTimeToTimeSpan('1900-01-01T' + getMainDetail.StartTime),
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
                        LCBMachineStopID: StopItem.LCBMachineStopID,
                        tId: StopItem.LCBMachineStopID,
                        SNo: StopItem.LCBMRRDetailID,
                        StopTime: StopItem.StopTime,
                        StartTime: StopItem.StartTime,
                        BWSName: StopItem.BWSName,
                        BWSID: StopItem.BWSID,
                        Description: StopItem.Description,
                        ShiftID: StopItem.ShiftID,
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
                        LCBBreakageID: BreakList.LCBBreakageID,
                        SlNo: BreakList.LCBMRRDetailID,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
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
                //$scope.IsCreateIcon = false;
                //$scope.IsListIcon = true;
                //$scope.IsfrmShow = true;
                //$scope.IsShowSave = true;
                //$scope.IsShowDetail = true;
                //$scope.IsShow = ($scope.ListLCBDetails.length < 1 || angular.isUndefined($scope.ListLCBDetails.length)) ? false : true;
            }
            else {
                //$scope.btnShowText = "Create";
                //$scope.IsCreateIcon = true;
                //$scope.IsListIcon = false;
                //$scope.cmnbtnShowHideEnDisable(1);
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
                LCBMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.LCBMRRID,
                PIID: $scope.PIID,
                LCBMRRDate: $scope.LCBMRRDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.LCBMRRDate),
                ItemID: $scope.ItemID,
                SetID: $scope.lstSetNoList,
                Description: $scope.Description
            };

            if ($scope.ListLCBDetailsDelete.length > 0) {
                angular.forEach($scope.ListLCBDetailsDelete, function (DelItem) {
                    $scope.ListLCBDetails.push({
                        SlNo: DelItem.SlNo, LCBMRRDetailID: DelItem.LCBMRRDetailID, LCBMRRID: DelItem.LCBMRRID, OutputUnitID: DelItem.OutputUnitID,
                        LCBMachineStopMasterID: DelItem.LCBMachineStopMasterID, LCBBreakageMasterID: DelItem.LCBBreakageMasterID, BeamQualityID: DelItem.BeamQualityID,
                        ShiftID: DelItem.ShiftID, OperatorID: DelItem.OperatorID, ShiftEngineerID: DelItem.ShiftEngineerID, TotalStop: DelItem.TotalStop,
                        TotalBreakage: DelItem.TotalBreakage, MachineID: DelItem.MachineID, DDate: DelItem.DDate, DDateString: DelItem.DDateString,
                        BeamLength: DelItem.BeamLength, BeginTime: DelItem.BeginTime, EndTime: DelItem.EndTime, MachineSpeed: DelItem.MachineSpeed,
                        Description: DelItem.Description, IsModalShow: DelItem.IsModalShow, ModelState: DelItem.ModelState
                    });
                })
            }
            debugger
            angular.forEach($scope.ListLCBDetails, function (NewItem) {
                NewItem.StartTime = NewItem.BeginTime.getHours() + ":" + NewItem.BeginTime.getMinutes();
                NewItem.StopTime = NewItem.EndTime.getHours() + ":" + NewItem.EndTime.getMinutes();
                NewItem.DDateString = NewItem.DDate.getMonth() + 1 + "/" + NewItem.DDate.getDate() + "/" + NewItem.DDate.getFullYear();
            })

            if ($scope.ListLCBDetails.length == 0) {
                Command: toastr["warning"]("Please input at least one LCB Detail.");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateLCB/';
            ModelsArray = [ItemMaster, $scope.ListLCBDetails, $scope.ListMachineStops, $scope.ListBreakTypesToSave, objcmnParam];
            var SaveUpdateLCBMRR = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateLCBMRR.then(function (response) {
                if (response != "") {
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

        $scope.DeletePrdLCBMasterDetail = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.LCBMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeletePrdLCBMasterDetail/';
            var delPrdLCBMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delPrdLCBMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("LCB MRR No " + result + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("LCB MRR No " + dataModel.LCBMRRNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("LCB MRR No " + dataModel.LCBMRRNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }


        //*********************************************************************Start Reset**********************************************************************
        $scope.ClearStopModalForm = function () {
            debugger
            $scope.ChangeMachinebtnState();
            $scope.LoadTimer();
            $scope.BWSID = "";
            $("#ddlCauses").select2("data", { id: '', text: '--Select Cause--' });
            $scope.StopDescription = "";
            $scope.ListMachineStop = [];
            if ($scope.ListLCBDetails.length > 0) {
                $scope.ListLCBDetails[$scope.LCBMRRDetailIndex].TotalStop = $scope.ListMachineStop.length == 0 ? "" : $scope.ListMachineStop.length;
            }
            $scope.MachineSave = true;
        }

        $scope.ClearStopModalList = function (dataModel) {
            debugger
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.ListLCBDetails[$scope.LCBMRRDetailIndex].TotalStop = "";
        }

        $scope.clear = function () {
            $scope.frmLCBEntry.$setPristine();
            $scope.frmLCBEntry.$setUntouched();
            //$scope.cmnbtnShowHideEnDisable(0);            
            //$scope.IsCreateIcon = false;
            //$scope.IsListIcon = true;
            //$scope.btnShowText = "Show List";            
            $scope.IsHidden = true;
            $scope.IsShow = false;
            debugger
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
            $scope.LCBMRRDate = conversion.NowDateCustom();
            $scope.ListLCBDetails = [];
            //$scope.btnSaveText = "Save";
            $scope.btnSaveMachineBreaks = "Save";
            //$scope.btnShowText = "Show List";
            $scope.ModelState = "";
            $scope.ChangeMachinebtnState();
            $scope.lstSetNoList = "";
            //$scope.IsbtnSaveDisable = true;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            //$scope.IsShowSave = true;
            $scope.IsShowDetail = true;
            $scope.IsbtnAddDelShow = true;
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.ListLCBDetailsDelete = [];
            $scope.HoldDataModel = [];
            $scope.SlNo = 0;
            $scope.IsNextDate = false;
            $scope.ListBreakTypesToSave = [];
            $scope.ItemID = "";
            $scope.PIID = "";
            $scope.MainDetailIndexWiseData = "";
            $scope.MachineSave = true;
            $("#ddlSetNoList").select2("data", { id: 0, text: '--Select Set No--' });
            $scope.ClearStopModalForm();
        };
        //*********************************************************************End Reset*************************************************************************

    }]);