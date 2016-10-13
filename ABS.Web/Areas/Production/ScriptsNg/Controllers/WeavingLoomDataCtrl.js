/*
    WeavingLoomDataCtrl.js
*/
app.controller('weavingLoomDataCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/WeavingLoomData/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsWeavingLoomMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Weaving Loom Data Master Entry';
        $scope.ListTitle = 'Weaving Loom Data Records';
        $scope.ListTitleMaster = 'Weaving Loom Data Master Records';
        $scope.PageTitleDetail = 'Weaving Loom Data Detail Entry';
        $scope.btnSaveMachineStop = 'Save';

        $scope.btnAddMachineStop = 'Add';
        $scope.ModalMachineHeading = 'Machine Stop Details';

        $scope.LoomDataDetailList = [];
        $scope.TempLoomDetailList = [];

        $scope.LoomDataDetailList = [];
        $scope.LoomDataDetailListDelete = [];
        $scope.ListMachineStop = [];
        $scope.ListMachineStops = [];

        $scope.listBeamNo = [];
        $scope.listShifts = [];
        $scope.listLine = [];
        $scope.listOperator = [];
        $scope.listOfficer = [];
        $scope.listBeamQuality = [];
        $scope.SlNo = 0;

        $scope.LoomDataDetailList = [];
        $scope.TempLoomDetailList = [];
        $scope.LoomDetailIndexData = "";
        $scope.tempMachineStopDetailID = 0;
        $scope.MachineStopModal = "";
        $scope.IsNextDate = false;

        $scope.MachineSave = true;
        //$scope.IsSaveDisable = true;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsfrmShow = true;
        $scope.IsbtnAddDelShow = true;
        $scope.IsDisablebtnAdd = true;
        //$scope.IsShowSave = true;
        $scope.IsShowDetail = true;
        $scope.ModelState = "";
        $scope.ProductionDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeletePrdWeavingLoomMasterDetail'; DelMsg = 'LoomRacordNo'; EditFunc = 'getWeavingLoomMasterDetailByID';
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

        //************************************************Start Shift Dropdown******************************************************        
        $scope.loadShiftRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.loomRecordID) ? 0 : dataModel.loomRecordID;
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
        $scope.loadShiftRecords(0);
        $scope.OnChangeShift = function () {
            $scope.IsDisablebtnAdd = $scope.ShiftID == '' || $scope.ShiftID == null || angular.isUndefined($scope.ShiftID) ? true : false;
        }

        //***************************************************End Shift Dropdown******************************************************

        //************************************************Start Line Dropdown******************************************************        
        $scope.loadLineRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.loomRecordID) ? 0 : dataModel.loomRecordID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingLine/';
            var listLine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listLine.then(function (response) {
                $scope.listLine = response.data.LineList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadLineRecords(0);
        //***************************************************End Line Dropdown******************************************************

        //************************************************Start Operator Dropdown******************************************************        
        $scope.loadWeavingLoomOperatorRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.LoomRecordID) ? 0 : dataModel.LoomRecordID;
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
        $scope.loadWeavingLoomOperatorRecords(0);
        //***************************************************End Operator Dropdown******************************************************

        //************************************************Start Duty Officer Dropdown******************************************************        
        $scope.loadWeavingLoomOfficerRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.LoomRecordID) ? 0 : dataModel.LoomRecordID;
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
        $scope.loadWeavingLoomOfficerRecords(0);
        //***************************************************End Duty Officer Dropdown******************************************************        

        //************************************************Start Machine Stop Cause Dropdown******************************************************        
        $scope.LoadMachineStopCauses = function (dataModel) {
            debugger
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
        //***************************************************End Machine Stop Cause Dropdown*************************************************

        //********************************************************Start Row Add in Detail Grid***********************************************
        $scope.AddItemToList = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined($scope.LineID) || $scope.LineID == '' || $scope.LineID == null ? 0 : $scope.LineID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDataToSetWeavingLoomDetail/';
            var AddDetailList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            AddDetailList.then(function (response) {
                debugger
                $scope.SlNo = 0;
                $scope.ModelState = "Save";
                $scope.ListMachineStop = [];
                $scope.ListMachineStops = [];
                $scope.LoomDataDetailList = [];
                $scope.TempLoomDetailList = [];
                $scope.TempLoomDetailList = response.data.LoomDetailList;
                angular.forEach($scope.listShifts, function (shift) {
                    if (shift.ShiftID == $scope.ShiftID) {
                        ShiftName = shift.ShiftName;
                    }
                })

                angular.forEach($scope.TempLoomDetailList, function (getList) {
                    $scope.SlNo = $scope.SlNo + 1;
                    $scope.LoomDataDetailList.push({
                        SlNo: $scope.SlNo, LoomRecordDetailID: 0, LoomRecordID: 0, LoomStopID: 0, MachineConfigID: getList.MachineConfigID, MachineConfigNo: getList.MachineConfigNo,
                        //LineID: getList.LineID,
                        SetID: getList.SetID, SizeMRRID: getList.SizeMRRID, SetNo: getList.SetNo, ItemID: getList.ItemID, ArticleNo: getList.ArticleNo,
                        ColorID: getList.ItemColorID, ColorName: getList.ColorName, ShiftID: $scope.ShiftID, ShiftName: ShiftName, WarpStop: '', WarpCMPX: '',
                        WeftStop: '', WeftCMPX: '', OtherStop: '', StartATT: '', RPM: '', TotalStop: '', Efficiency: '', RunTime: '', Prodn: '', Remarks: '',
                        ShiftEngineerID: 0, OperatorID: 0, IsReleased: false, ModelState: $scope.ModelState //IsModalShow: false,
                    });
                })
                //$scope.IsSaveDisable = $scope.LoomDataDetailList.length > 0 ? false : true;
                if ($scope.LoomDataDetailList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true')}
                $scope.IsShow = $scope.LoomDataDetailList.length > 0 ? true : false;
                if ($scope.IsShow == false) Command: toastr["warning"]("Nothing Found....");
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**********************************************************End Row Add in Detail Grid***********************************************

        //*****************************************************Start Row Remove from Main Detail Grid****************************************
        $scope.DeleteMainDetail = function (index) {
            debugger
            $scope.LoomDetailIndexData = $scope.LoomDataDetailList[index];

            $scope.ListMachineStop = [];
            $scope.ModelState = "Delete";

            angular.forEach($scope.ListMachineStops, function (delitemS) {
                if (delitemS.SNo == $scope.LoomDetailIndexData.SlNo && delitemS.ModelState == "Update") {
                    delitemS.ModelState = $scope.ModelState;
                }
            })

            if ($scope.ListMachineStops.length > 0) {
                angular.forEach($scope.ListMachineStops, function (keepStopList) {
                    if (keepStopList.SNo != $scope.LoomDetailIndexData.SlNo || keepStopList.ModelState == $scope.ModelState) {
                        $scope.ListMachineStop.push({
                            LoomStopDetailID: keepStopList.LoomStopDetailID,
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

            $scope.ListMachineStops = [];

            if ($scope.ListMachineStop.length > 0) {
                angular.forEach($scope.ListMachineStop, function (StopItem) {
                    $scope.ListMachineStops.push({
                        LoomStopDetailID: StopItem.LoomStopDetailID,
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

            if ($scope.LoomDetailIndexData.ModelState == "Update")
                $scope.LoomDataDetailListDelete.push({
                    SlNo: $scope.LoomDetailIndexData.SlNo, LoomRecordDetailID: $scope.LoomDetailIndexData.LoomRecordDetailID,
                    LoomRecordID: $scope.LoomDetailIndexData.LoomRecordID, LoomStopID: $scope.LoomDetailIndexData.LoomStopID,
                    MachineConfigID: $scope.LoomDetailIndexData.MachineConfigID, MachineConfigNo: $scope.LoomDetailIndexData.MachineConfigNo,
                    //LineID: $scope.LoomDetailIndexData.LineID,
                    SetID: $scope.LoomDetailIndexData.SetID, SizeMRRID: $scope.LoomDetailIndexData.SizeMRRID,
                    SetNo: $scope.LoomDetailIndexData.SetNo, ItemID: $scope.LoomDetailIndexData.ItemID, ArticleNo: $scope.LoomDetailIndexData.ArticleNo,
                    ColorID: $scope.LoomDetailIndexData.ColorID, ColorName: $scope.LoomDetailIndexData.ColorName, ShiftID: $scope.LoomDetailIndexData.ShiftID,
                    ShiftName: $scope.LoomDetailIndexData.ShiftName, WarpStop: $scope.LoomDetailIndexData.WarpStop, WarpCMPX: $scope.LoomDetailIndexData.WarpCMPX,
                    WeftStop: $scope.LoomDetailIndexData.WeftStop, WeftCMPX: $scope.LoomDetailIndexData.WeftCMPX, OtherStop: $scope.LoomDetailIndexData.OtherStop,
                    StartATT: $scope.LoomDetailIndexData.StartATT, RPM: $scope.LoomDetailIndexData.RPM, TotalStop: $scope.LoomDetailIndexData.TotalStop,
                    Efficiency: $scope.LoomDetailIndexData.Efficiency, RunTime: $scope.LoomDetailIndexData.RunTime, Prodn: $scope.LoomDetailIndexData.Prodn,
                    Remarks: $scope.LoomDetailIndexData.Remarks, ShiftEngineerID: $scope.LoomDetailIndexData.ShiftEngineerID,
                    OperatorID: $scope.LoomDetailIndexData.OperatorID, IsReleased: $scope.LoomDetailIndexData.IsReleased, ModelState: $scope.ModelState //IsModalShow: false,
                });

            $scope.LoomDataDetailList.splice(index, 1);
            $scope.ListMachineStop = [];
            $scope.LoomDetailIndexData = "";
            //$scope.IsSaveDisable = $scope.LoomDataDetailList.length > 0 ? false : true;
            if ($scope.LoomDataDetailList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true');}
            $scope.IsShow = $scope.LoomDataDetailList.length > 0 ? true : false;
        }
        //*******************************************************End Row Remove from Main Detail Grid****************************************

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
            $scope.LoomDetailIndex = $scope.LoomDataDetailList.indexOf(dataModel);
            $scope.LoomDetailIndexData = dataModel;

            $scope.ListMachineStop = [];
            $scope.ClearStopModalForm();

            if ($scope.ListMachineStops.length != 0) {
                angular.forEach($scope.ListMachineStops, function (StopItem) {
                    if ($scope.LoomDetailIndexData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete") {
                        $scope.ListMachineStop.push({
                            LoomStopDetailID: StopItem.LoomStopDetailID,
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
                    LoomStopDetailID: 0,
                    tId: $scope.tempMachineStopDetailID,
                    SNo: $scope.LoomDetailIndexData.SlNo,
                    StopTime: conversion.get12HourFrom24($scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes()),
                    StartTime: conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes()),
                    BWSName: BWSName,
                    BWSID: $scope.BWSID,
                    Description: $scope.StopDescription,
                    ShiftID: $scope.LoomDetailIndexData.ShiftID,
                    StopInMin: conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime),
                    IsNextDate: $scope.IsNextDate,
                    Status: 0,
                    ModelState: $scope.ModelState
                }

                if ($scope.ListMachineStops.length != 0 && $scope.ListMachineStop.length == 0) {
                    angular.forEach($scope.ListMachineStops, function (StopItem) {
                        if ($scope.LoomDetailIndexData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete")
                            $scope.ListMachineStop.push({
                                LoomStopDetailID: StopItem.LoomStopDetailID,
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
                        MStop.ShiftID = $scope.LoomDetailIndexData.ShiftID;
                        MStop.StopInMin = conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime);
                        MStop.IsNextDate = $scope.IsNextDate;
                        MStop.Status = $scope.MachineStopModal.LoomStopDetailID > 0 ? 1 : 0;
                    }
                })
            }

            $scope.TotalNoOfStops = $scope.ListMachineStop.length;
            $scope.LoomDataDetailList[$scope.LoomDetailIndex].TotalStop = $scope.TotalNoOfStops;
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
                                savelistM.LoomStopDetailID = typelistM.LoomStopDetailID;
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
                                if (typelistM.SNo == savelistM.SNo && typelistM.Status == 0) {
                                    $scope.ListMachineStops.push({
                                        LoomStopDetailID: typelistM.LoomStopDetailID,
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
                                }
                        });
                    });
                }
                if (txtStatusM == "Save") {
                    angular.forEach($scope.ListMachineStop, function (StopItem) {
                        $scope.ListMachineStops.push({
                            LoomStopDetailID: StopItem.LoomStopDetailID,
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
            $scope.LoomDataDetailList[$scope.LoomDetailIndex].TotalStop = $scope.TotalNoOfStops;
            $scope.MachineSave = false;
        }
        //**********************************************Start Delete Item from List in Machine Stop Modal****************************************

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
                $scope.loadWeavingLoomMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadWeavingLoomMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadWeavingLoomMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadWeavingLoomMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadWeavingLoomMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadWeavingLoomMasterRecords = function (isPaging) {
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
            $scope.gridOptionsWeavingLoomMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,

                columnDefs: [
                    { name: "LoomRecordID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "LineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LoomRacordNo", displayName: "Loom Record No", title: "Loom Record No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ProductionDate", displayName: "Production Date", title: "Production Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftName", displayName: "Shift Name", title: "Shift Name", headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "LineName", displayName: "Line Name", title: "Line Name", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsWeavingLoomMaster.useExternalPagination = false;
                        $scope.gridOptionsWeavingLoomMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetWeavingLoomDataMaster/';
            ModelsArray = [objcmnParam];
            var listWeavingLoomMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listWeavingLoomMaster.then(function (response) {

                angular.forEach(response.data.ListLoomMaster, function (items) {
                    items.ProductionDate = items.ProductionDate == "1900-01-01T00:00:00" ? '' : items.ProductionDate
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsWeavingLoomMaster.data = response.data.ListLoomMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadWeavingLoomMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //***************************************************End Set Master Dynamic Grid******************************************************

        $scope.getWeavingLoomMasterDetailByID = function (Master) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(Master.LoomRecordID) ? 0 : Master.LoomRecordID;
            ModelsArray = [objcmnParam];

            var apiRouteLoomMaster = baseUrl + 'GetWeavingLoomDataMasterByID/';
            var apiRouteLoomDetail = baseUrl + 'GetWeavingLoomDataDetailByID/';
            var apiRouteStopDetail = baseUrl + 'GetStopDetailByID/';

            var listWeavingLoomMaster = crudService.postMultipleModel(apiRouteLoomMaster, ModelsArray, $scope.HeaderToken.get);
            listWeavingLoomMaster.then(function (response) {
                debugger
                $scope.LoomRecordID = response.data.SinglLoomMaster.LoomRecordID;
                $scope.ShiftID = response.data.SinglLoomMaster.ShiftID;
                angular.forEach($scope.listShifts, function (shifts) {
                    if (shifts.ShiftID == response.data.SinglLoomMaster.ShiftID) {
                        $("#ddlShiftList").select2("data", { id: 0, text: shifts.ShiftName });
                    }
                })
                $scope.Remarks = response.data.SinglLoomMaster.Remarks;
                $scope.ProductionDate = response.data.SinglLoomMaster.ProductionDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.SinglLoomMaster.ProductionDate);
                //$scope.IsShowSave = true;
                $scope.IsShowDetail = true;
            },
            function (error) {
                console.log("Error: " + error);
            });

            var listWeavingLoomDetail = crudService.postMultipleModel(apiRouteLoomDetail, ModelsArray, $scope.HeaderToken.get);
            listWeavingLoomDetail.then(function (response) {
                debugger
                $scope.LoomDataDetailList = [];
                angular.forEach(response.data.ListLoomDetail, function (getMainDetail) {
                    $scope.SlNo = getMainDetail.LoomRecordDetailID;
                    $scope.LoomDataDetailList.push({
                        SlNo: $scope.SlNo, LoomRecordDetailID: getMainDetail.LoomRecordDetailID,
                        LoomRecordID: getMainDetail.LoomRecordID, LoomStopID: getMainDetail.LoomStopID,
                        MachineConfigID: getMainDetail.MachineConfigID, MachineConfigNo: getMainDetail.MachineConfigNo,
                        //LineID: getMainDetail.LineID,
                        SetID: getMainDetail.SetID, SizeMRRID: getMainDetail.SizeMRRID, SetNo: getMainDetail.SetNo,
                        ItemID: getMainDetail.ItemID, ArticleNo: getMainDetail.ArticleNo, ColorID: getMainDetail.ColorID,
                        ColorName: getMainDetail.ColorName, ShiftID: getMainDetail.ShiftID, ShiftName: getMainDetail.ShiftName,
                        WarpStop: getMainDetail.WarpStop, WarpCMPX: getMainDetail.WarpCMPX, WeftStop: getMainDetail.WeftStop,
                        WeftCMPX: getMainDetail.WeftCMPX, OtherStop: getMainDetail.OtherStop, StartATT: getMainDetail.StartATT,
                        RPM: getMainDetail.RPM, TotalStop: getMainDetail.TotalStop, Efficiency: getMainDetail.Efficiency,
                        RunTime: getMainDetail.RunTime, Prodn: getMainDetail.Prodn, Remarks: getMainDetail.Remarks,
                        ShiftEngineerID: getMainDetail.ShiftEngineerID, OperatorID: getMainDetail.OperatorID,
                        IsReleased: getMainDetail.IsReleased, ModelState: 'Update'
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
                        LoomStopDetailID: StopItem.LoomStopDetailID,
                        tId: StopItem.LoomStopDetailID,
                        SNo: StopItem.LoomRecordDetailID,
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

            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShow = true;
            //$scope.IsSaveDisable = false;
            //$scope.IsbtnAddDelShow = true;
            $scope.IsDisablebtnAdd = true;
            $scope.frmWeavingLoomDataMasterEntry = true;
            //$scope.btnSaveText = "Update";
            //$scope.btnShowText = "Show List";
            $scope.LineID = '';
            $("#ddlLineList").select2("data", { id: 0, text: '--Select Line--' });
        }

        //******************************************************Start Master List ShowHide***************************************************        
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
            //$scope.IsShow = $scope.IsShow ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                //$scope.IsfrmShow = true;
                //$scope.IsShowSave = true;
                //$scope.IsShowDetail = true;
                //$scope.IsShow = ($scope.LoomDataDetailList.length < 1 || angular.isUndefined($scope.LoomDataDetailList.length)) ? false : true;
            }
            else {
                $scope.RefreshMasterList();
                //$scope.btnShowText = "Create";
                //$scope.pagination.pageNumber = 1;
                //$scope.loadWeavingLoomMasterRecords(1);
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
                LoomRecordID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.LoomRecordID,
                ProductionDate: $scope.ProductionDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ProductionDate),
                ShiftID: $scope.ShiftID,
                Remarks: $scope.Remarks
            };

            if ($scope.LoomDataDetailListDelete.length > 0) {
                angular.forEach($scope.LoomDataDetailListDelete, function (DelItem) {
                    $scope.LoomDataDetailList.push({
                        SlNo: DelItem.SlNo, LoomRecordDetailID: DelItem.LoomRecordDetailID,
                        LoomRecordID: DelItem.LoomRecordID, LoomStopID: DelItem.LoomStopID,
                        MachineConfigID: DelItem.MachineConfigID, MachineConfigNo: DelItem.MachineConfigNo,
                        SetID: DelItem.SetID, SizeMRRID: DelItem.SizeMRRID, SetNo: DelItem.SetNo,
                        ItemID: DelItem.ItemID, ArticleNo: DelItem.ArticleNo, ColorID: DelItem.ColorID,
                        ColorName: DelItem.ColorName, ShiftID: DelItem.ShiftID, ShiftName: DelItem.ShiftName,
                        WarpStop: DelItem.WarpStop, WarpCMPX: DelItem.WarpCMPX, WeftStop: DelItem.WeftStop,
                        WeftCMPX: DelItem.WeftCMPX, OtherStop: DelItem.OtherStop, StartATT: DelItem.StartATT,
                        RPM: DelItem.RPM, TotalStop: DelItem.TotalStop, Efficiency: DelItem.Efficiency,
                        RunTime: DelItem.RunTime, Prodn: DelItem.Prodn, Remarks: DelItem.Remarks,
                        ShiftEngineerID: DelItem.ShiftEngineerID, OperatorID: DelItem.OperatorID,
                        IsReleased: DelItem.IsReleased, ModelState: DelItem.ModelState
                    });
                })
            }

            if ($scope.LoomDataDetailList.length == 0) {
                Command: toastr["warning"]("Please input at least one Loom Data Detail.");
                return;
            }
            debugger
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [ItemMaster, $scope.LoomDataDetailList, $scope.ListMachineStops, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateWeavingLoom/';
            var SaveUpdateLoomData = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateLoomData.then(function (response) {
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

        //********************************************************Start Delete Data*********************************************************
        $scope.DeletePrdWeavingLoomMasterDetail = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.LoomRecordID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeletePrdWeavingLoomMasterDetail/';
            var delPrdWeavingLoomMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delPrdWeavingLoomMasterDetail.then(function (response) {
                debugger
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"]("Loom Racord No " + dataModel.LoomRacordNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Loom Racord No " + dataModel.LoomRacordNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Loom Racord No " + dataModel.LoomRacordNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
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
            if ($scope.LoomDataDetailList.length > 0) {
                $scope.LoomDataDetailList[$scope.LoomDetailIndex].TotalStop = $scope.ListMachineStop.length == 0 ? "" : $scope.ListMachineStop.length;
            }
            $scope.MachineSave = true;
        }

        $scope.ClearStopModalList = function (dataModel) {
            debugger
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.LoomDataDetailList[$scope.LoomDetailIndex].TotalStop = "";
        }

        $scope.clear = function () {
            debugger
            //$scope.frmWeavingLoomDataMasterEntry.$setPristine();
            //$scope.frmWeavingLoomDataMasterEntry.$setUntouched();
            $scope.ShiftID = '';
            $scope.LineID = '';
            $scope.Remarks = '';
            $scope.ProductionDate = conversion.NowDateCustom();
            $scope.LoomDataDetailList = [];
            //$scope.btnSaveText = "Save";

            //$scope.btnShowText = "Show List";
            $scope.ModelState = "";
            $scope.ChangeMachinebtnState();
            //$scope.IsSaveDisable = true;
            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsfrmShow = true;
            $scope.IsDisablebtnAdd = true;
            //$scope.IsShowSave = true;
            $scope.IsShowDetail = true;
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.LoomDataDetailListDelete = [];
            $scope.SlNo = 0;
            $scope.IsNextDate = false;

            $scope.LoomDetailIndexData = "";
            $scope.MachineSave = true;
            $("#ddlShiftList").select2("data", { id: 0, text: '--Select Shift--' });
            $("#ddlLineList").select2("data", { id: 0, text: '--Select Line--' });
            $scope.ClearStopModalForm();
        };
        //*********************************************************************End Reset*************************************************************************

    }]);