/**
 * BallWarpingCtrl.js
 */
app.controller('BallWarpingCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/BallWarping/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsBallWarping = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.loaderMore = false;
        //$scope.btnSaveText = "Save";
        //$scope.btnShowList = "Show List";
        $scope.BallWarpingPageTitle = 'Ball Warping Creation';
        $scope.BallWarpingListTitle = 'Ball Warping Information';
        $scope.ListTitleBallWarpingDetail = 'Ball Warping Detail';
        $scope.ListTitleBallWarpingInfo = "All Ball Warping Records";
        $scope.ListTitleBallWarpingMachineSetup = 'Machine Setup Data';
        $scope.ModalConsumption = "Consumption";
        $scope.btnSaveConsumption = "Save"
        $scope.btnSaveMachineStop = 'Save';
        $scope.ModalMachineHeading = 'Machine Stop Details';
        $scope.btnAddMachineStop = 'Add';
        $scope.btnSaveMachineBreaks = 'Save';
        $scope.BrekageModalHeading = 'Breakage Detail';

        $scope.MachineID = "";
        $scope.ListMachineStop = [];
        $scope.ListMachineStops = [];
        $scope.MainDetailIndexWiseData = "";
        $scope.tempMachineStopDetailID = 0;
        $scope.HoldDataModel = [];
        $scope.SlNo = 0;
        $scope.ListBreakTypesToSave = [];
        $scope.ListConsumption = [];
        $scope.ListConsumptionToSave = [];
        $scope.ItemID = "";
        //$scope.IsbtnSaveDisable = true;
        $scope.IsShow = false;
        $scope.IsShowDetail = true;
        $scope.IsShowDetailMachine = false;
        $scope.IsHidden = true;
        $scope.IsfrmShow = true;
        $scope.MachineStopModal = "";
        $scope.IsNextDate = false;
        $scope.MachineSave = true;
        $scope.IsConRead = true;
        $scope.ModelState = "";
        var YdsNum = 1.0936;
        var QtyNum = 0.453592;
        var constraint = 840;
        $scope.BalMRRDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteBallMrrMasterDetail'; DelMsg = 'BalMRRNo'; EditFunc = 'getBallWarpingDetailInfo';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //************************************************ load all set no which is not completed ball***********************************
        $scope.loadSetRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetSetNo/';
            ModelsArray = [objcmnParam];
            var listSetNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetNo.then(function (response) {
                $scope.listSetNo = response.data.ListSet;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadSetRecords(0);


        $scope.loadSetInformation = function (lstSetNoList) {
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined($scope.lstSetNoList) ? 0 : $scope.lstSetNoList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetInformation/';
            var ListSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSetInformation.then(function (response) {
                debugger
                $scope.IsShow = false;
                $scope.ListballInfoDetails = [];
                $scope.ListMachineSetup = [];
                $scope.IsShowDetail = true;
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
                $scope.PIID = "";
                $scope.TotalEnds = "";
                $scope.YarnRatio = "";
                $scope.EndsPerCreel = "";
                $scope.SupplierName = "";
                $scope.ItemID = "";
                debugger
                if (response.data.model.length > 0) {
                    $scope.ListMachineSetup = response.data.model;
                    $scope.lstSetNoList = response.data.model[0].SetID;
                    $scope.ArticleNo = response.data.model[0].ArticleNo;
                    $scope.ItemID = response.data.model[0].ItemID;
                    $scope.SetLength = response.data.model[0].SetLength;
                    $scope.ColorName = response.data.model[0].ColorName;
                    $scope.BuyerName = response.data.model[0].BuyerName;
                    $scope.YarnCount = response.data.model[0].YarnCount;
                    $scope.YarnRatioLot = response.data.model[0].YarnRatioLot;
                    $scope.NoOfBall = response.data.model[0].NoOfBall;
                    $scope.LeaseReapet = response.data.model[0].LeaseRepeat;
                    $scope.PINO = response.data.model[0].PINO;
                    $scope.PIID = response.data.model[0].PIID;
                    $scope.TotalEnds = response.data.model[0].TotalEnds;
                    $scope.YarnRatio = response.data.model[0].YarnRatio;
                    $scope.EndsPerCreel = response.data.model[0].EndsPerCreel;
                    $scope.SupplierName = response.data.model[0].SupplierName;
                }
                $scope.IsShowDetailMachine = $scope.ListMachineSetup.length > 0 ? true : false;
                if ($scope.ListMachineSetup.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //************************************************ Start Reset ***********************************
        $scope.ClearStopModalForm = function () {
            $scope.LoadTimer();
            $scope.BWSID = "";
            $("#ddlCauses").select2("data", { id: '', text: '--Select Cause--' });
            $scope.StopDescription = "";
            $scope.ListMachineStop = [];
            if ($scope.ListMachineStop.length == 0) {
                $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].TotalStop = "";
            }
            else {
                $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].TotalStop = $scope.ListMachineStop.length;
            }
        }

        $scope.ClearStopModalList = function (dataModel) {
            $scope.ListMachineStop = [];
            $scope.ListMachineStops = [];
            $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].TotalStop = "";
        }
        //************************************************ Switch between show and hide ***********************************        
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowList = "Show List";
                //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
                //$scope.cmnButtonCall(0, 0);
                //$scope.IsfrmShow = true;
                //$scope.IsShowSave = true;
                //$scope.IsShowDetail = true;
                //$scope.IsShowDetailMachine = angular.isUndefined($scope.ListMachineSetup) || $scope.ListMachineSetup.length < 1 ? false : true;
                //$scope.IsShow = angular.isUndefined($scope.ListballInfoDetails) || $scope.ListballInfoDetails.length < 1 ? false : true;
            }
            else {
                //$scope.btnShowList = "Create";
                //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);                
                $scope.RefreshMasterList();
                //$scope.IsfrmShow = false;
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
                //$scope.IsShowDetail = false;
                $scope.IsShowDetailMachine = false;
            }
        }
        //************************************************Start Show Ball Warping List Information Dynamic Grid******************************************************
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
                $scope.loadAllBallWarpingMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllBallWarpingMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllBallWarpingMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllBallWarpingMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllBallWarpingMasterRecords(1);
                }
            }
        };

        $scope.loadAllBallWarpingMasterRecords = function (isPaging) {
            $scope.gridOptionsBallWarping.enableFiltering = true;

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
            $scope.gridOptionsBallWarping = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "BalMRRID", displayName: "BalMRR ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BalMRRNo", displayName: "BalMRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "YarnRatioLot", displayName: "Yarn Ratio Lot", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Item Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BalMRRDate", displayName: "BalMRR Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "NoOfBall", displayName: "No Of Ball", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetLength", displayName: "Set Length", headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptionsBallWarping.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsBallWarping.useExternalPagination = false;
                        $scope.gridOptionsBallWarping.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBallWarpingMaster/';
            var listBallWarpingMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBallWarpingMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsBallWarping.data = response.data.objvmBallWarping;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllBallWarpingMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //************************************************End Show Ball Warping List Information Dynamic Grid******************************************************

        //************************************************ Load Ball Warping detail when click on ADD button as per set no ***********************************
        $scope.loadAllBallDetailDropDown = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 4;
            objcmnParam.ItemGroup = 50;
            ModelsArray = [objcmnParam];
            var apiRouteDetails = baseUrl + 'GetBallWarpingDetail/';
            var ListballInfoDetails = crudService.postMultipleModel(apiRouteDetails, ModelsArray, $scope.HeaderToken.get);
            ListballInfoDetails.then(function (response) {
                $scope.OutputNoList = response.data.objBallDetail[0].OutputNos;
                $scope.MachineNoList = response.data.objBallDetail[0].MachineNos;
                $scope.ShiftNameList = response.data.objBallDetail[0].ShiftNames;
                $scope.OperatorList = response.data.objBallDetail[0].Operators;
                $scope.DutyOfficerList = response.data.objBallDetail[0].DutyOfficers;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadAllBallDetailDropDown();

        $scope.AddItemToList = function () {
            debugger
            //********************************Start Load Item Wise Stock**************************            
            $scope.ListItemWiseStock = [];
            $scope.cmnParam();
            debugger
            for (var i = 0; i < $scope.ListMachineSetup.length; i++) {
                debugger
                $scope.tempListMachineSetup = {};
                $scope.tempListMachineSetup = { ItemID: $scope.ListMachineSetup[i].YarnCountID, LotID: $scope.ListMachineSetup[i].LotID, SupplierID: $scope.ListMachineSetup[i].SupplierID };
                ModelsArray = [$scope.tempListMachineSetup, objcmnParam];
                var apiRoute = baseUrl + 'GetItemWiseStock/';
                var ListItemStock = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListItemStock.then(function (response) {
                    if (response.data.ItemStockList != null)
                        $scope.ListItemWiseStock.push(response.data.ItemStockList);
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //*********************************End Load Item Wise Stock***************************




            $scope.ModelState = "Save";
            $scope.ListballInfoDetails = [];
            $scope.cmnParam();
            objcmnParam.ItemType = 4;
            objcmnParam.ItemGroup = 50;
            ModelsArray = [objcmnParam];
            var apiRouteDetails = baseUrl + 'GetBallWarpingDetail/';
            var ListballInfoDetails = crudService.postMultipleModel(apiRouteDetails, ModelsArray, $scope.HeaderToken.get);
            ListballInfoDetails.then(function (response) {
                debugger
                for (var j = 0; j < $scope.ListMachineSetup.length; j++) {
                    var YRatio = $scope.ListMachineSetup[j].Ratio;
                    var ArticleYarnCount = $scope.ListMachineSetup[j].ArticleYarnCount;
                    var YarnCountID = $scope.ListMachineSetup[j].YarnCountID;
                    var LotID = $scope.ListMachineSetup[j].LotID;
                    var SupplierID = $scope.ListMachineSetup[j].SupplierID;
                    var Unit = $scope.ListMachineSetup[j].Unit;
                    var UOMName = $scope.ListMachineSetup[j].UOMName == null ? '' : $scope.ListMachineSetup[j].UOMName;

                    for (var k = 0; k < YRatio; k++) {
                        response.data.objBallDetail[0].cnt = k;

                        $scope.ListballInfoDetails.push({
                            cnt: k, SlNo: $scope.SlNo + 1, SupplierID: SupplierID, YarnCountID: YarnCountID, LotID: LotID, ArticleYarnCount: ArticleYarnCount,
                            BeginTime: conversion.NowTime(), EndTime: conversion.NowTime(), WarpingDate: conversion.NowDateDefault(),
                            StartTime: '', StopTime: '', Consumption: '', Unit: Unit, UOMName: UOMName, ModelState: $scope.ModelState
                        });
                        $scope.SlNo = $scope.SlNo + 1;
                    }
                }
                $scope.IsShowDetail = true;
                $scope.IsShow = $scope.ListballInfoDetails.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.LoadTimer = function () {
            debugger
            $scope.StopTime = conversion.NowTime();
            $scope.StartTime = conversion.NowTime();
            $scope.IsNextDate = false;
        }

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

        //***********************************Start Modal ShowHide Validation Based on ShiftID & OutputUnitID*********************************
        $scope.ModalShowHide = function (dataModel) {
            angular.forEach($scope.ListballInfoDetails, function (ItemCheck) {
                if ((!angular.isUndefined(ItemCheck.ShiftID) && ItemCheck.ShiftID != null && ItemCheck.ShiftID != 0) && (!angular.isUndefined(ItemCheck.OutputUnitID) && ItemCheck.OutputUnitID != null && ItemCheck.OutputUnitID != 0)
                    && (!angular.isUndefined(ItemCheck.LengthPerBall) && ItemCheck.LengthPerBall != null && ItemCheck.LengthPerBall != "") && (!angular.isUndefined(ItemCheck.MachineID) && ItemCheck.MachineID != null && ItemCheck.MachineID != 0)
                    && ItemCheck.BeginTime != "" && ItemCheck.EndTime != "") {
                    if (ItemCheck.EndTime.getTime() > ItemCheck.BeginTime.getTime()) {
                        $scope.cmnbtnShowHideEnDisable('false');
                        ItemCheck.IsModalShow = true;
                    }
                    else {
                        $scope.cmnbtnShowHideEnDisable('true');
                        ItemCheck.IsModalShow = false;
                    }
                }
                else {
                    ItemCheck.IsModalShow = false;
                }
            })
        }
        //**************************************End Modal ShowHide Validation Based on ShiftID & OutputUnitID************************************

        //************************************************Start Machine Stop Cause Dropdown******************************************************        
        $scope.LoadMachineStopCauses = function (dataModel) {
            $scope.HoldDataModel = dataModel;
            $scope.cmnParam();
            objcmnParam.ItemType = 29; objcmnParam.id = angular.isUndefined(dataModel.tId) ? 0 : dataModel.tId;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetLoadMachineBrekages/';
            var CauseList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            CauseList.then(function (response) {
                debugger
                $scope.CauseList = response.data.BrekagesList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //***************************************************End Machine Stop Cause Dropdown******************************************************

        //*******************************************************Start Machine Stop Modal********************************************************        
        $scope.LoadMachineDetail = function (dataModel) {
            debugger
            $scope.BallMRRDetailIndex = $scope.ListballInfoDetails.indexOf(dataModel);
            $scope.MainDetailIndexWiseData = dataModel;

            $scope.ListMachineStop = [];
            $scope.ClearStopModalForm();

            if ($scope.ListMachineStops.length != 0) {
                angular.forEach($scope.ListMachineStops, function (StopItem) {
                    if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete") {
                        $scope.ListMachineStop.push({
                            BallMachineStopID: StopItem.BallMachineStopID,
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
            var BWSName = "";
            angular.forEach($scope.CauseList, function (cause) {
                if (cause.BWSID == $scope.BWSID) {
                    BWSName = cause.BWSName;
                }
            })
            debugger
            if ($scope.btnAddMachineStop == 'Add') {
                $scope.tempMachineStopDetailID = $scope.tempMachineStopDetailID + 1;

                obj = {
                    BallMachineStopID: 0,
                    tId: $scope.tempMachineStopDetailID,
                    SNo: $scope.MainDetailIndexWiseData.SlNo,
                    StopTime: conversion.get12HourFrom24($scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes()),
                    StartTime: conversion.get12HourFrom24($scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes()),
                    BWSName: BWSName,
                    BWSID: $scope.BWSID,
                    Description: $scope.StopDescription,
                    ShiftID: $scope.HoldDataModel.ShiftID,
                    MachineID: $scope.MainDetailIndexWiseData.MachineID,
                    StopInMin: conversion.getMinutesBetweenDates($scope.StartTime, $scope.StopTime),
                    IsNextDate: $scope.IsNextDate,
                    Status: 0,
                    ModelState: $scope.ModelState
                }

                if ($scope.ListMachineStops.length != 0 && $scope.ListMachineStop.length == 0) {
                    angular.forEach($scope.ListMachineStops, function (StopItem) {
                        if ($scope.MainDetailIndexWiseData.SlNo == StopItem.SNo && StopItem.ModelState != "Delete")
                            $scope.ListMachineStop.push({
                                BallMachineStopID: StopItem.BallMachineStopID,
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
                        if ($scope.MachineStopModal.BallMachineStopID > 0) {
                            MStop.Status = 1;
                        }
                        else {
                            MStop.Status = 0;
                        }
                    }
                })
            }

            $scope.TotalNoOfStops = $scope.ListMachineStop.length;
            $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
            $scope.MachineSave = false;
            $scope.LoadTimer();
            $scope.ChangeMachinebtnState();
        }
        //*************************************************End Add Item to List in Machine Stop Modal********************************************   
        $scope.ChangeMachinebtnState = function () {
            if ($scope.btnSaveMachineText == "Save") {
                $scope.btnAddMachineStop = 'Add';
                $scope.btnSaveMachineStop = 'Save';
            }
        }
        $scope.EditMachineStop = function (dataModel) {
            if ($scope.btnSaveMachineText == "Save") {
                $scope.btnAddMachineStop = 'Modify';
                $scope.btnSaveMachineStop = 'Update';
            }

            $scope.MachineStopModal = dataModel;
            $scope.BWSID = dataModel.BWSID;
            angular.forEach($scope.CauseList, function (cause) {
                if (cause.BWSID == dataModel.BWSID) {
                    $("#ddlCauses").select2("data", { id: 0, text: cause.BWSName });
                }
            })

            if (dataModel.IsNextDate == true) {
                var Start = "1900-01-02T" + conversion.get24HourFromPM(dataModel.StartTime) + ":00";
                $scope.IsNextDate = true;
            }
            else {
                var Start = "1900-01-01T" + conversion.get24HourFromPM(dataModel.StartTime) + ":00";
            }

            var Stop = "1900-01-01T" + conversion.get24HourFromPM(dataModel.StopTime) + ":00";
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
                            if (savelistM.SNo == typelistM.SNo && savelistM.tId == typelistM.tId) {
                                savelistM.BallMachineStopID = typelistM.BallMachineStopID;
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
                                        BallMachineStopID: typelistM.BallMachineStopID,
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
                            BallMachineStopID: StopItem.BallMachineStopID,
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
            $scope.ListBallDetails[$scope.BallMRRDetailIndex].TotalStop = $scope.TotalNoOfStops;
        }
        //**********************************************Start Delete Item from List in Machine Stop Modal****************************************

        //*********************************************************Start Machine Breakage Modal**************************************************
        $scope.LoadBreakageDetail = function (dataModels) {
            $scope.ListBreakTypes = [];

            $scope.TotalBreaks = '';
            $scope.ModelState = "Save";
            $scope.BallMRRDetailIndex = $scope.ListballInfoDetails.indexOf(dataModels);
            $scope.MainDetailIndexWiseData = dataModels;

            if ($scope.ListBreakTypesToSave.length != 0) {
                var ttlBreak = 0;
                angular.forEach($scope.ListBreakTypesToSave, function (BreakListHold) {
                    if (BreakListHold.SlNo == $scope.MainDetailIndexWiseData.SlNo) {
                        $scope.ListBreakTypes.push({
                            SlNo: BreakListHold.SlNo, BWSID: BreakListHold.BWSID, BWSName: BreakListHold.BWSName,
                            MachineID: BreakListHold.MachineID, NoOfBreakage: BreakListHold.NoOfBreakage, ModelState: BreakListHold.ModelState
                        });

                        ttlBreak = ttlBreak + BreakListHold.NoOfBreakage;
                        $scope.TotalBreaks = ttlBreak;
                    }
                })
            }
            if ($scope.ListBreakTypes.length == 0) {
                $scope.cmnParam();
                objcmnParam.ItemType = 27;
                var apiRoute = baseUrl + 'GetLoadMachineBrekages/';
                ModelsArray = [objcmnParam];
                var ListBreakTypes = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBreakTypes.then(function (response) {
                    angular.forEach(response.data.BrekagesList, function (BreakList) {
                        $scope.ListBreakTypes.push({
                            SlNo: $scope.MainDetailIndexWiseData.SlNo, BWSID: BreakList.BWSID, BWSName: BreakList.BWSName,
                            MachineID: $scope.HoldDataModel.MachineID, NoOfBreakage: "", ModelState: $scope.ModelState
                        });
                    })
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.TotalMachineBreaks = function (dataModel) {
            ////// Sum the breakages///////////////       
            $scope.qtyTotalBreaks = 0;
            angular.forEach($scope.ListBreakTypes, function (item) {
                if (item.NoOfBreakage > 0) {
                    var NoOfBreakage = item.NoOfBreakage;
                    $scope.qtyTotalBreaks = NoOfBreakage + $scope.qtyTotalBreaks;
                }
            });
            $scope.TotalBreaks = $scope.qtyTotalBreaks;

            $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].TotalBreakage = $scope.TotalBreaks;
        }

        $scope.saveMachineBrekages = function () {
            var txtStatus = "Save";
            if ($scope.ListBreakTypes.length > 0) {
                if ($scope.ListBreakTypesToSave.length > 0) {
                    angular.forEach($scope.ListBreakTypesToSave, function (savelist) {
                        angular.forEach($scope.ListBreakTypes, function (typelist) {
                            if (savelist.SlNo == typelist.SlNo && savelist.BWSID == typelist.BWSID) {
                                savelist.NoOfBreakage = typelist.NoOfBreakage
                                txtStatus = "Update";
                            }
                        });
                    });
                }
                if (txtStatus == "Save") {
                    angular.forEach($scope.ListBreakTypes, function (BreakList) {
                        $scope.ListBreakTypesToSave.push({
                            SlNo: BreakList.SlNo, BWSID: BreakList.BWSID, BWSName: BreakList.BWSName,
                            MachineID: BreakList.MachineID, NoOfBreakage: BreakList.NoOfBreakage, ModelState: BreakList.ModelState
                        });
                    })
                }
            }
        }
        //***************************************************End Machine Breakage Modal******************************************************

        //*********************************************************Start Consumption Modal**************************************************
        var countQty = 0;
        var unitPrice = 0;
        $scope.CompareCurrentQtyWithStock = function (dataModels) {
            countQty = 0;
            unitPrice = 0;
            var tempQty = conversion.roundNumber((((dataModels.LengthPerBall * YdsNum) / constraint) * QtyNum), 6);
            if ($scope.ListItemWiseStock.length == 0) {
                Command: toastr["warning"]("No Stock Information Found for Selected Item");
                return;
            }
            else {
                angular.forEach($scope.ListItemWiseStock, function (StockCheck) {
                    if (StockCheck.ItemID == dataModels.YarnCountID) {
                        countQty = countQty + StockCheck.CurrentStock;
                        unitPrice = StockCheck.UnitPrice;
                    }
                })
            }


            if ($scope.ListConsumptionToSave.length > 0) {
                angular.forEach($scope.ListConsumptionToSave, function (ConCheck) {
                    if (ConCheck.YarnCountID == dataModels.YarnCountID) {
                        countQty = countQty - ConCheck.Qty;
                    }
                })
            }

            if (tempQty > countQty) {
                Command: toastr["warning"]("Quantity can't be greater than Current Stock");
                return;
            }
        }

        $scope.LoadConsumption = function (dataModels) {            
            debugger
            $scope.ListConsumption = [];
            $scope.BallMRRDetailIndex = $scope.ListballInfoDetails.indexOf(dataModels);
            $scope.MainDetailIndexWiseData = dataModels;

            if ($scope.ListConsumptionToSave.length > 0) {
                angular.forEach($scope.ListConsumptionToSave, function (ConList) {
                    if (ConList.SlNo == $scope.MainDetailIndexWiseData.SlNo || ConList.BallConsumptionID == $scope.MainDetailIndexWiseData.BallConsumptionID) {
                        $scope.ListConsumption.push({
                            SlNo: ConList.SlNo, BallConsumptionID: ConList.BallConsumptionID,
                            YarnCountID: ConList.YarnCountID, SupplierID: ConList.SupplierID,
                            LotID: ConList.LotID, Unit: ConList.Unit, LengthM: ConList.LengthM,
                            LengthYds: conversion.roundNumber(ConList.LengthYds, 2), YarnCount: ConList.YarnCount, Constraint: ConList.Constraint,
                            UOMName: ConList.UOMName, Qty: conversion.roundNumber(ConList.Qty, 6), UnitPrice: conversion.roundNumber(ConList.UnitPrice, 4),
                            Amount: conversion.roundNumber(ConList.Amount, 4), Remarks: ConList.Remarks, ModelState: ConList.ModelState
                        });
                        $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].Consumption = conversion.roundNumber(ConList.Qty, 6);
                    }
                })
            }
            
            debugger
            if ($scope.ListConsumption.length == 0) {
                debugger
                $scope.CompareCurrentQtyWithStock(dataModels);
                debugger
                $scope.ListConsumption.push({
                    SlNo: $scope.MainDetailIndexWiseData.SlNo, BallConsumptionID: 0,
                    YarnCountID: $scope.MainDetailIndexWiseData.YarnCountID, SupplierID: $scope.MainDetailIndexWiseData.SupplierID,
                    LotID: $scope.MainDetailIndexWiseData.LotID, Unit: $scope.MainDetailIndexWiseData.Unit, LengthM: $scope.MainDetailIndexWiseData.LengthPerBall,
                    LengthYds: conversion.roundNumber(($scope.MainDetailIndexWiseData.LengthPerBall * YdsNum), 2), YarnCount: $scope.YarnCount,
                    Constraint: constraint, UOMName: $scope.MainDetailIndexWiseData.UOMName, Qty: '', UnitPrice: conversion.roundNumber(unitPrice, 4),
                    Amount: '', Remarks: '', ModelState: "Save"
                });

                angular.forEach($scope.ListConsumption, function (Con) {
                    Con.Qty = conversion.roundNumber(((Con.LengthYds / Con.Constraint) * QtyNum), 6);
                    Con.Amount = conversion.roundNumber((Con.UnitPrice * Con.Qty), 4);
                    $scope.ListballInfoDetails[$scope.BallMRRDetailIndex].Consumption = conversion.roundNumber(Con.Qty, 6);
                });
            }
        }


        $scope.OnChangeBallLength = function (dataModel) {
            debugger
            $scope.CompareCurrentQtyWithStock(dataModel);
            debugger
            $scope.BallMRRDetailIndex = $scope.ListballInfoDetails.indexOf(dataModel);
            $scope.MainDetailIndexWiseData = dataModel;
            if ($scope.ListConsumptionToSave.length > 0) {
                angular.forEach($scope.ListConsumptionToSave, function (ConList) {
                    if ((ConList.SlNo == $scope.MainDetailIndexWiseData.SlNo || ConList.BallConsumptionID == $scope.MainDetailIndexWiseData.BallConsumptionID) && ConList.LengthM != $scope.MainDetailIndexWiseData.LengthPerBall) {
                        ConList.LengthM = $scope.MainDetailIndexWiseData.LengthPerBall;
                        ConList.LengthYds = conversion.roundNumber(($scope.MainDetailIndexWiseData.LengthPerBall * YdsNum), 2);
                        ConList.Qty = conversion.roundNumber(((($scope.MainDetailIndexWiseData.LengthPerBall * YdsNum) / ConList.Constraint) * QtyNum), 6);
                        ConList.UnitPrice = conversion.roundNumber(unitPrice, 4);
                        ConList.Amount = conversion.roundNumber((ConList.Qty*unitPrice), 4);
                    }
                })
            }
        }

        $scope.saveConsumption = function () {
            debugger
            var txtStatus = "Save";
            if ($scope.ListConsumption.length > 0) {
                if ($scope.ListConsumptionToSave.length > 0) {
                    angular.forEach($scope.ListConsumptionToSave, function (savelist) {
                        angular.forEach($scope.ListConsumption, function (typelist) {
                            if (savelist.SlNo == typelist.SlNo) {
                                savelist.LengthM = typelist.LengthM;
                                savelist.LengthYds = typelist.LengthYds;
                                savelist.Qty = typelist.Qty;
                                savelist.UnitPrice = typelist.UnitPrice;
                                savelist.Amount = typelist.Amount;
                                savelist.Remarks = typelist.Remarks;
                                txtStatus = "Update";
                            }
                        });
                    });
                }
                if (txtStatus == "Save") {
                    angular.forEach($scope.ListConsumption, function (ConList) {
                        $scope.ListConsumptionToSave.push({
                            SlNo: ConList.SlNo, BallConsumptionID: ConList.BallConsumptionID,
                            YarnCountID: ConList.YarnCountID, SupplierID: ConList.SupplierID,
                            LotID: ConList.LotID, Unit: ConList.Unit, LengthM: ConList.LengthM,
                            LengthYds: ConList.LengthYds, YarnCount: ConList.YarnCount, Constraint: ConList.Constraint,
                            UOMName: ConList.UOMName, Qty: ConList.Qty, UnitPrice: ConList.UnitPrice, Amount: ConList.Amount,
                            Remarks: ConList.Remarks, ModelState: ConList.ModelState
                        });
                    })
                }
            }
        }
        //***************************************************End Machine Breakage Modal******************************************************

        //************************************** Load Data at Edit mode ********************************//
        $scope.getBallWarpingDetailInfo = function (dataModel) {
            debugger
            //$scope.IsfrmShow = true;
            $scope.IsHidden = true;
            //$scope.IsbtnSaveDisable = $scope.UserCommonEntity.EnableEView == true ? true : false;
            //$scope.IsShowSave = true;
            //$scope.btnSaveText = "Update";
            //$scope.btnShowList = "Show List";
            //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
            $scope.bool = false;
            $scope.cmnParam();
            objcmnParam.id = dataModel.BalMRRID;

            ModelsArray = [objcmnParam];
            var apiRouteMaster = baseUrl + 'GetBallWarpingMasterById/';
            var apiRouteBallDetail = baseUrl + 'GetBallDetailByID/';
            var apiRouteStopDetail = baseUrl + 'GetStopDetailByID/';
            var apiRouteBreakDetail = baseUrl + 'GetBreakageDetailByID/';
            var apiRouteConsumption = baseUrl + 'GetBallConsumptionByID/';

            var ListBallInfoMaster = crudService.postMultipleModel(apiRouteMaster, ModelsArray, $scope.HeaderToken.get);
            ListBallInfoMaster.then(function (response) {
                //$scope.ListBallInfoMaster = response.data.BWMById;
                $scope.ListMachineSetup = response.data.BWMById;
                $scope.BalMRRID = response.data.BWMById[0].BalMRRID;
                $scope.ArticleNo = response.data.BWMById[0].ArticleNo;
                $scope.ItemID = response.data.BWMById[0].ItemID;
                $scope.SetLength = response.data.BWMById[0].SetLength;
                $scope.ColorName = response.data.BWMById[0].ColorName;
                $scope.BuyerName = response.data.BWMById[0].BuyerName;
                $scope.Description = response.data.BWMById[0].Description;
                $scope.YarnCount = response.data.BWMById[0].YarnCount;
                $scope.YarnRatioLot = response.data.BWMById[0].YarnRatioLot;

                $scope.NoOfBall = response.data.BWMById[0].NoOfBall;
                $scope.LeaseReapet = response.data.BWMById[0].LeaseRepeat;
                $scope.PINO = response.data.BWMById[0].PINO;
                $scope.PIID = response.data.BWMById[0].PIID;
                $scope.TotalEnds = response.data.BWMById[0].TotalEnds;
                $scope.YarnRatio = response.data.BWMById[0].YarnRatio;
                $scope.EndsPerCreel = response.data.BWMById[0].EndsPerCreel;
                $scope.SupplierName = response.data.BWMById[0].SupplierName;

                $scope.lstSetNoList = response.data.BWMById[0].SetID;
                $("#ddlSetNoList").select2("data", { id: response.data.BWMById[0].SetID, text: response.data.BWMById[0].SetNo });
                $scope.BalMRRDate = response.data.BWMById[0].BalMRRDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(response.data.BWMById[0].BalMRRDate);

                $scope.IsShowDetailMachine = $scope.ListMachineSetup.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });

            var ListBallDetail = crudService.postMultipleModel(apiRouteBallDetail, ModelsArray, $scope.HeaderToken.get);
            ListBallDetail.then(function (response) {
                debugger
                $scope.ListballInfoDetails = [];
                angular.forEach(response.data.ListBallDetail, function (getMainDetail) {
                    $scope.SlNo = getMainDetail.BalMRRDetailID;
                    $scope.ListballInfoDetails.push({
                        SlNo: getMainDetail.BalMRRDetailID, BalMRRDetailID: getMainDetail.BalMRRDetailID, BalMRRID: getMainDetail.BalMRRID,
                        OutputUnitID: getMainDetail.OutputUnitID, BalMachineStopID: getMainDetail.BalMachineStopID, YarnCountID: getMainDetail.YarnCountID,
                        BallBreackageMasterID: getMainDetail.BallBreackageMasterID, ShiftID: getMainDetail.ShiftID, OperatorID: getMainDetail.OperatorID,
                        ShiftEngineerID: getMainDetail.ShiftEngineerID, TotalStop: getMainDetail.TotalStop, TotalBreakage: getMainDetail.TotalBreakage,
                        WarpingDate: conversion.getDateTimeToTimeSpan(getMainDetail.WarpingDate), StrWarpingDate: '', LotID: getMainDetail.LotID,
                        ArticleYarnCount: getMainDetail.ArticleYarnCount, BallConsumptionID: getMainDetail.BallConsumptionID, Consumption: getMainDetail.Qty,
                        LengthPerBall: getMainDetail.LengthPerBall == 0 ? "" : getMainDetail.LengthPerBall, Remarks: getMainDetail.Remarks,
                        MachineID: getMainDetail.MachineID, MachineSpeed: getMainDetail.MachineSpeed == 0 ? "" : getMainDetail.MachineSpeed,
                        BeginTime: conversion.getDateTimeToTimeSpan('1900-01-01T' + getMainDetail.StartTime), SupplierID: getMainDetail.SupplierID,
                        EndTime: conversion.getDateTimeToTimeSpan('1900-01-01T' + getMainDetail.StopTime), Unit: getMainDetail.Unit, UOMName: getMainDetail.UOMName,
                        ModelState: "Update", IsModalShow: true
                    });
                })
                $scope.IsShowDetail = true;
                $scope.IsShow = $scope.ListballInfoDetails.length > 0 ? true : false;
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
                        BallMachineStopID: StopItem.BalMachineStopID,
                        tId: StopItem.BalMachineStopID,
                        SNo: StopItem.BalMRRDetailID,
                        StopTime: StopItem.StopTime,
                        StartTime: StopItem.StartTime,
                        BWSName: StopItem.BWSName,
                        BWSID: StopItem.BWSID,
                        Description: StopItem.Description,
                        ShiftID: StopItem.ShiftID,
                        MachineID: StopItem.MachineID,
                        StopInMin: StopItem.StopInMin,
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
                        BallBreakageID: BreakList.BallBreackageID,
                        SlNo: BreakList.BalMRRDetailID,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
                        MachineID: BreakList.MachineID,
                        NoOfBreakage: BreakList.NoOfBreakage,
                        ModelState: "Update"
                    });
                })
            },
            function (error) {
                console.log("Error: " + error);
            });

            var listConsumption = crudService.postMultipleModel(apiRouteConsumption, ModelsArray, $scope.HeaderToken.get);
            listConsumption.then(function (response) {
                $scope.ListConsumptionToSave = [];
                angular.forEach(response.data.ConsumptionList, function (ConList) {
                    $scope.ListConsumptionToSave.push({
                        SlNo: ConList.BallConsumptionID, BallConsumptionID: ConList.BallConsumptionID,
                        YarnCountID: ConList.YarnCountID, SupplierID: ConList.SupplierID,
                        LotID: ConList.LotID, Unit: ConList.Unit, LengthM: ConList.LengthM,
                        LengthYds: ConList.LengthYds, YarnCount: ConList.YarnCount, Constraint: 840,
                        UOMName: ConList.UOMName, Qty: ConList.Qty, UnitPrice: ConList.UnitPrice, Amount: ConList.Amount,
                        Remarks: ConList.Remarks, ModelState: "Update"
                    });
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //*****************************************************Start Row Remove from Main Detail Grid****************************************
        $scope.DeleteMainDetail = function (index) {
            $scope.MainDetailIndexWiseData = $scope.ListballInfoDetails[index];

            $scope.ListMachineStop = [];
            $scope.ListBreakTypes = [];

            if ($scope.ListMachineStops.length > 0) {
                angular.forEach($scope.ListMachineStops, function (keepStopList) {
                    if (keepStopList.SNo != $scope.MainDetailIndexWiseData.SlNo) {
                        $scope.ListMachineStop.push({
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
                            Status: keepStopList.Status
                        });
                    }
                })
            }

            if ($scope.ListBreakTypesToSave.length > 0) {
                angular.forEach($scope.ListBreakTypesToSave, function (KeepBreakList) {
                    if (KeepBreakList.SlNo != $scope.MainDetailIndexWiseData.SlNo) {
                        $scope.ListBreakTypes.push({
                            SlNo: KeepBreakList.SlNo,
                            BWSID: KeepBreakList.BWSID,
                            BWSName: KeepBreakList.BWSName,
                            MachineID: KeepBreakList.MachineID,
                            NoOfBreakage: KeepBreakList.NoOfBreakage
                        });
                    }
                })
            }

            $scope.ListMachineStops = [];
            $scope.ListBreakTypesToSave = [];

            if ($scope.ListMachineStop.length > 0) {
                angular.forEach($scope.ListMachineStop, function (StopItem) {
                    $scope.ListMachineStops.push({
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
                        Status: StopItem.Status
                    })
                })
            }

            if ($scope.ListBreakTypes.length > 0) {
                angular.forEach($scope.ListBreakTypes, function (BreakList) {
                    $scope.ListBreakTypesToSave.push({
                        SlNo: BreakList.SlNo,
                        BWSID: BreakList.BWSID,
                        BWSName: BreakList.BWSName,
                        MachineID: BreakList.MachineID,
                        NoOfBreakage: BreakList.NoOfBreakage
                    });
                })
            }

            $scope.ListballInfoDetails.splice(index, 1);
            $scope.ListMachineStop = [];
            $scope.ListBreakTypes = [];
            $scope.MainDetailIndexWiseData = "";

            angular.forEach($scope.ListballInfoDetails, function (ItemCheck) {
                if (ItemCheck.ShiftID != 0 && ItemCheck.OutputUnitID != 0 && ItemCheck.BeginTime != "" && ItemCheck.EndTime != "" && ItemCheck.BeamLength != "" && ItemCheck.EndTime.getTime() > ItemCheck.BeginTime.getTime()) {
                    //$scope.IsbtnSaveDisable = false;
                    $scope.cmnbtnShowHideEnDisable('false');
                }
                else {
                    //$scope.IsbtnSaveDisable = true;
                    $scope.cmnbtnShowHideEnDisable('true');
                }
            })
        }
        //*******************************************************End Row Remove from Main Detail Grid****************************************

        //**************************************Start Save or Update Ball MRR Data ********************************//
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var ItemMaster = {
                BalMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.BalMRRID,
                PIID: $scope.PIID,
                SetID: $scope.lstSetNoList,
                Description: $scope.Description,
                ItemID: $scope.ItemID,
                BalMRRDate: $scope.BalMRRDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.BalMRRDate)
            };

            angular.forEach($scope.ListballInfoDetails, function (NewItem) {
                NewItem.StartTime = NewItem.BeginTime.getHours() + ":" + NewItem.BeginTime.getMinutes();
                NewItem.StopTime = NewItem.EndTime.getHours() + ":" + NewItem.EndTime.getMinutes();
                NewItem.StrWarpingDate = NewItem.WarpingDate.getMonth() + 1 + "/" + NewItem.WarpingDate.getDate() + "/" + NewItem.WarpingDate.getFullYear();
                NewItem.LengthPerBall = NewItem.LengthPerBall == "" ? null : NewItem.LengthPerBall;
                NewItem.MachineSpeed = NewItem.MachineSpeed == "" ? null : NewItem.MachineSpeed;
            })

            if ($scope.ListballInfoDetails.length < 1) {
                Command: toastr["warning"]("Please input at least one ball information .");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateBallMRR/';
            ModelsArray = [ItemMaster, $scope.ListballInfoDetails, $scope.ListMachineStops, $scope.ListBreakTypesToSave, $scope.ListConsumptionToSave, objcmnParam];
            var SaveUpdateBallMRR = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateBallMRR.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    $scope.LCReferenceNo = response.data;
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
        //**************************************End Save or Update Ball MRR Data ********************************

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteBallMrrMasterDetail = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.BalMRRID;
            var apiRoute = baseUrl + 'DeleteUpdateBallMrrMasterDetail/';
            ModelsArray = [objcmnParam];
            var delBallMrrMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delBallMrrMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("Ball MRR No " + dataModel.BalMRRNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Ball MRR No " + dataModel.BalMRRNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Ball MRR No " + dataModel.BalMRRNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
            //}
        }

        //********************************************************End Delete Master Detail***************************************************

        //**************************************Start Clear All Modal ********************************
        $scope.ClearAllModal = function () {
            $scope.frmMacStop.$setPristine();
            $scope.frmMacStop.$setUntouched();
        }
        //**************************************End Clear All Modal ********************************

        //*************************************************************Start Reset***********************************************************
        $scope.clear = function () {
            $scope.frmBallWarpingEntry.$setPristine();
            $scope.frmBallWarpingEntry.$setUntouched();
            //$scope.defaultDisable();
            //$scope.btnShowList = "Show List";
            //$scope.btnSaveText = "Save";
            //conversion.ChangeIconClass($scope.btnShowList, $scope.btnSaveText);
            $scope.IsHidden = true;
            $scope.IsShow = false;
            //$scope.IsfrmShow = true;
            //$scope.IsbtnSaveDisable = true;
            //$scope.IsShowDetail = true;
            $scope.IsShowDetailMachine = false;
            //$scope.IsShowSave = true;            
            $scope.lstSetNoList = "";
            $("#ddlSetNoList").select2("data", { id: 0, text: '--Select Set No--' });
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
            $scope.PIID = '';
            $scope.ItemID = '';
            $scope.TotalEnds = '';
            $scope.YarnRatio = '';
            $scope.EndsPerCreel = '';
            $scope.SupplierName = '';
            $scope.btnSaveConsumption = "Save"
            $scope.BalMRRDate = conversion.NowDateCustom();
            $scope.ListballInfoDetails = [];
            $scope.ListMachineSetup = [];
        };
        //**************************************************************End Reset************************************************************
    }]);
