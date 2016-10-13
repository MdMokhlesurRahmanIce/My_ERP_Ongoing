/**
*ProductionEntryCtrl.js
*/
app.controller('productionEntryCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/ProductionEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsFinishingMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Production Entry';
        $scope.ListTitleMaster = 'Production Records';
        $scope.PageTitleDetail = 'Production Shrinkage Entry';
        $scope.ListShrinkage = [];
        $scope.IsNextDate = false;
        //$scope.IsSaveDisable = true;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsShowSave = true;
        $scope.IsfrmShow = true;
        $scope.ModelState = "";
        var message = "";
        $scope.FinishingMRRDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteFinishingMasterDetail'; DelMsg = 'FinishingMRRNo'; EditFunc = 'loadDataForEdit';
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
        $scope.LoadTimer();

        $scope.loadSetRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? 0 : dataModel.FinishingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingSetNo/';
            var listSetNo = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetNo.then(function (response) {
                $scope.listSetNo = response.data.AllWSetNo;
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
            //objcmnParam.ItemType = 4; objcmnParam.ItemGroup = 49;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetMachine/';
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.ListMachine = response.data.ListMachine;
                angular.forEach($scope.ListMachine, function (tempitem) {
                    if (tempitem.MachineID == angular.isUndefined(dataM.MachineID) ? 13 : dataM.MachineID) {
                        $scope.MachineID = tempitem.MachineID;
                        $("#MachineList").select2("data", { id: 0, text: tempitem.MachineName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadMachineRecords(0);
        //**************************************************End Machine Dropdown******************************************************
        //************************************************Start Machine Dropdown******************************************************
        $scope.loadFinishTypeRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? dataModel : dataModel.FinishingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFinishingType/';
            var listFinishType = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listFinishType.then(function (response) {
                $scope.ListFinishType = response.data.AllFinishingType;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //$scope.loadFinishTypeRecords(0);
        //**************************************************End Machine Dropdown******************************************************

        //************************************************Start SetNo wise single Records******************************************************

        $scope.loadWeavingSetInfoRecords = function (lstSetNoList) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(lstSetNoList) ? 0 : angular.isUndefined(lstSetNoList.WeavingMRRID) ? $scope.lstSetNoList : lstSetNoList.WeavingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetWeavingSetInformation/';
            var ListWeavingSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListWeavingSetInformation.then(function (response) {
                debugger
                $scope.lstSetNoList = "";
                $scope.SetID = "";
                $scope.SizeMRRID = "";
                $scope.ItemID = "";
                $scope.ArticleNo = "";
                $scope.BuyerID = "";
                $scope.BuyerName = "";
                $scope.PIID = "";
                $scope.PINO = "";
                //$scope.FInishTypeID = "";
                $scope.ListShrinkage = [];

                if (response.data != null) {
                    $scope.lstSetNoList = response.data.WeavingMRRID;
                    $scope.SetID = response.data.SetID;
                    $scope.SizeMRRID = response.data.SizeMRRID;
                    $scope.ItemID = response.data.ItemID;
                    $scope.ArticleNo = response.data.ArticleNo;
                    $scope.BuyerID = response.data.BuyerID;
                    $scope.BuyerName = response.data.BuyerName;
                    $scope.PIID = response.data.PIID;
                    $scope.PINO = response.data.PINO;
                    //$scope.FInishTypeID = response.data.FinishingTypeID;
                    //$scope.FinishTypeName = response.data.FinishingProcessName;
                    //angular.forEach($scope.ListFinishType, function (Ftype) {
                    //    if (response.data.FinishingTypeID == Ftype.FInishTypeID) {
                    //        $("#ddlFinishType").select2("data", { id: 0, text: Ftype.FInishTypeName });
                    //    }
                    //})
                    var SetNo = '';
                    angular.forEach($scope.listSetNo, function (LSet) {
                        if (response.data.WeavingMRRID == LSet.WeavingMRRID) {
                            SetNo = LSet.WeavingMRRNo;
                        }
                    })
                    $scope.loadFinishTypeRecords(response.data.WeavingMRRID);

                    $scope.ListShrinkage.push({
                        FinishingMRRShrinkageID: 0, FinishingMRRID: 0, ReqWeight: response.data.ReqWeight, FiniWeight: '', AWWeight: 0, ItemID: response.data.ItemID,
                        ArticleNo: response.data.ArticleNo, SetID: response.data.SetID, SizeMRRID: response.data.SizeMRRID, SetNo: SetNo,
                        WeavingMRRID: response.data.WeavingMRRID, GreigeEPIPPI: response.data.GreigeEPIPPI, FiniEPI: '', FiniPPI: '', AWEPIPPI: '',
                        CuttableWidth: response.data.CuttableWidth, FiniWidth: '', AWWidth: 0, WReqd: response.data.WReqd, LShrinkage: '', WShrinkage: '', FiniSkew: '',
                        IsFSPercent: false, AWSkew: 0, IsAWPercent: false, MovSkew: 0, IsMovPercent: false
                    })

                    //$scope.loadFinishTypeRecords(response.data.WeavingMRRID);
                }
                //if (response.data == null) {
                //    $scope.FInishTypeID = "";
                //    $("#ddlFinishType").select2("data", { id: 0, text: "--Select Finish Type--" });
                //}
                //$scope.IsSaveDisable = $scope.ListShrinkage.length > 0 ? false : true;
                if ($scope.ListShrinkage.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                $scope.IsShow = $scope.ListShrinkage.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************************************End SetNo wise single Records********************************************

        //************************************************Start Shift Dropdown******************************************************        
        $scope.loadFinishingShiftRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? 0 : dataModel.FinishingMRRID;
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
        $scope.loadFinishingShiftRecords(0);
        //***************************************************End Shift Dropdown******************************************************

        //************************************************Start Unit Dropdown******************************************************
        $scope.loadUOMRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetUnit/';
            ModelsArray = [objcmnParam];
            var ListUOM = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListUOM.then(function (response) {
                debugger
                $scope.ListUOM = response.data.ListUOM;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadUOMRecords(1);
        //**************************************************End Unit Dropdown******************************************************  

        //************************************************Start Operator Dropdown******************************************************        
        $scope.loadFinishingOperatorRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? 0 : dataModel.FinishingMRRID;
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
        $scope.loadFinishingOperatorRecords(0);
        //***************************************************End Operator Dropdown******************************************************

        //************************************************Start Duty Officer Dropdown******************************************************        
        $scope.loadFinishingOfficerRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            objcmnParam.id = angular.isUndefined(dataModel.FinishingMRRID) ? 0 : dataModel.FinishingMRRID;
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
        $scope.loadFinishingOfficerRecords(0);
        //***************************************************End Duty Officer Dropdown****************************************************** 

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
                $scope.loadFinishingMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadFinishingMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadFinishingMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadFinishingMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadFinishingMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadFinishingMasterRecords = function (isPaging) {
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
            $scope.gridOptionsFinishingMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "FinishingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeavingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingTypeID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StartTime", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "EndTime", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OperatorID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftEngineerID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRShrinkageID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ReqWeight", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AWWeight", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GreigeEPIPPI", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FiniEPI", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FiniPPI", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AWEPIPPI", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FiniWidth", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AWWidth", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WReqd", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LShrinkage", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WShrinkage", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FiniSkew", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AWSkew", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MovSkew", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsFSPercent", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsAWPercent", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsMovPercent", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "FinishingMRRNo", displayName: "Finishing MRR No", title: "Finishing MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRDate", displayName: "Finishing MRR Date", title: "Finishing MRR Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeavingMRRNo", displayName: "Set No", title: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerName", displayName: "Buyer Name", title: "Buyer Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINO", displayName: "PI NO", title: "PI NO", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FInishTypeName", displayName: "Finishing Type Name", title: "Finishing Type Name", headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "FinishingProcessName", displayName: "Finishing Process Name", title: "Finishing Process Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Length", displayName: "Length", title: "Length", headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsFinishingMaster.useExternalPagination = false;
                        $scope.gridOptionsFinishingMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFinishingMRRMaster/';
            var listFinishingMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listFinishingMaster.then(function (response) {
                angular.forEach(response.data.ListFinishingMaster, function (items) {
                    if (items.FinishingMRRDate == "1900-01-01T00:00:00") {
                        items.FinishingMRRDate = "";
                    }
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsFinishingMaster.data = response.data.ListFinishingMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.loadFinishingMasterRecords(1);
        //***************************************************End Set Master Dynamic Grid******************************************************

        $scope.loadDataForEdit = function (Master) {
            debugger
            $scope.clear();

            $scope.StyleNo = Master.FinishingMRRNo;
            $scope.FinishingMRRID = Master.FinishingMRRID;
            $scope.SetID = Master.SetID;
            $scope.SizeMRRID = Master.SizeMRRID;
            $scope.ItemID = Master.ItemID;
            $scope.ArticleNo = Master.ArticleNo;
            $scope.BuyerID = Master.BuyerID;
            $scope.BuyerName = Master.BuyerName;
            $scope.PIID = Master.PIID;
            $scope.PINO = Master.PINO;
            $scope.Length = Master.Length;
            $scope.lstSetNoList = Master.WeavingMRRID;
            //$scope.FinishTypeName = Master.FinishingProcessName;
            $("#ddlSetNoList").select2("data", { id: 0, text: Master.WeavingMRRNo });
            $scope.FInishTypeID = Master.FinishingTypeID;
            $("#ddlFinishType").select2("data", { id: 0, text: Master.FInishTypeName });

            $scope.UnitID = Master.UnitID;
            angular.forEach($scope.ListUOM, function (LUnit) {
                if (Master.UnitID == LUnit.UOMID) {
                    $("#UnitListD").select2("data", { id: 0, text: LUnit.UOMName });
                }
            })
            $scope.MachineID = Master.MachineID;
            angular.forEach($scope.ListMachine, function (LMac) {
                if (Master.MachineID == LMac.MachineID) {
                    $("#MachineList").select2("data", { id: 0, text: LMac.MachineName });
                }
            })
            $scope.OperatorID = Master.OperatorID;
            angular.forEach($scope.listOperator, function (LOP) {
                if (Master.OperatorID == LOP.UserID) {
                    $("#ddlOperatorList").select2("data", { id: 0, text: LOP.UserFullName });
                }
            })
            $scope.ShiftEngineerID = Master.ShiftEngineerID;
            angular.forEach($scope.listOfficer, function (LOF) {
                if (Master.ShiftEngineerID == LOF.UserID) {
                    $("#ddlDutyOfficerList").select2("data", { id: 0, text: LOF.UserFullName });
                }
            })
            $scope.ShiftID = Master.ShiftID;
            angular.forEach($scope.listShifts, function (LS) {
                if (Master.ShiftID == LS.ShiftID) {
                    $("#ddlShiftList").select2("data", { id: 0, text: LS.ShiftName });
                }
            })

            $scope.Remarks = Master.Remarks;
            $scope.StartTime = conversion.getDateTimeToTimeSpan('1900-01-01T' + Master.StartTime);
            $scope.StopTime = conversion.getDateTimeToTimeSpan('1900-01-01T' + Master.EndTime);
            $scope.FinishingMRRDate = Master.FinishingMRRDate == "" ? "" : conversion.getDateToString(Master.FinishingMRRDate);

            $scope.ListShrinkage.push({
                FinishingMRRShrinkageID: Master.FinishingMRRShrinkageID, FinishingMRRID: Master.FinishingMRRID, ReqWeight: Master.ReqWeight, FiniWeight: Master.FiniWeight,
                AWWeight: Master.AWWeight, ItemID: Master.ItemID, ArticleNo: Master.ArticleNo, SetID: Master.SetID, SizeMRRID: Master.SizeMRRID, SetNo: Master.WeavingMRRNo,
                WeavingMRRID: Master.WeavingMRRID, GreigeEPIPPI: Master.GreigeEPIPPI, FiniEPI: Master.FiniEPI, FiniPPI: Master.FiniPPI, AWEPIPPI: Master.AWEPIPPI,
                CuttableWidth: Master.CuttableWidth, FiniWidth: Master.FiniWidth, AWWidth: Master.AWWidth, WReqd: Master.WReqd, LShrinkage: Master.LShrinkage,
                WShrinkage: Master.WShrinkage, FiniSkew: Master.FiniSkew, IsFSPercent: Master.IsFSPercent, AWSkew: Master.AWSkew, IsAWPercent: Master.IsAWPercent,
                MovSkew: Master.MovSkew, IsMovPercent: Master.IsMovPercent
            })

            $scope.loadFinishTypeRecords(Master.WeavingMRRID);
            //$scope.cmnParam();
            //objcmnParam.id = Master.WeavingMRRID;
            //ModelsArray = [objcmnParam];
            //var apiRoute = baseUrl + 'GetWeavingSetInformation/';            
            //var ListWeavingSetInformation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            //ListWeavingSetInformation.then(function (response) {
            //    $scope.FinishTypeName = response.data.FinishingProcessName;
            //},
            //function (error) {
            //    console.log("Error: " + error);

            //});

            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            //$scope.IsSaveDisable = false;
            //$scope.btnSaveText = "Update";
            //$scope.btnShowText = "Show List";
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
                //$scope.IsShow = ($scope.ListShrinkage.length < 1 || angular.isUndefined($scope.ListShrinkage.length)) ? false : true;
            }
            else {
                //$scope.btnShowText = "Create";
                $scope.pagination.pageNumber = 1;
                $scope.loadFinishingMasterRecords(0);
                $scope.IsfrmShow = false;
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
            }
        };

        //********************************************************End Master List ShowHide**************************************************        

        //********************************************************End Save All Data*********************************************************
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";            
            $scope.cmnParam();
            var ItemMaster = {
                FinishingMRRID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.FinishingMRRID, FinishingMRRNo: $scope.StyleNo, FinishingTypeID: $scope.FInishTypeID,
                FinishingMRRDate: $scope.FinishingMRRDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.FinishingMRRDate),
                ItemID: $scope.ItemID, SetID: $scope.SetID, SizeMRRID: $scope.SizeMRRID, WeavingMRRID: $scope.lstSetNoList,
                BuyerID: $scope.BuyerID, PIID: $scope.PIID, MachineID: $scope.MachineID, ShiftID: $scope.ShiftID,
                StartTime: $scope.StartTime.getHours() + ":" + $scope.StartTime.getMinutes(),
                EndTime: $scope.StopTime.getHours() + ":" + $scope.StopTime.getMinutes(),
                Length: $scope.Length, UnitID: $scope.UnitID, OperatorID: $scope.OperatorID, ShiftEngineerID: $scope.ShiftEngineerID,
                Remarks: $scope.Remarks
            };

            if ($scope.ListShrinkage.length < 1) {
                Command: toastr["warning"]("Please input at least one Shrinkage.");
                return;
            }
            debugger
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateFinishing/';
            ModelsArray = [ItemMaster, $scope.ListShrinkage, objcmnParam];
            var SaveUpdateProductionMRR = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateProductionMRR.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    if ($scope.UserCommonEntity.message == "Saved")
                        $scope.StyleNo = response.data;
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
        $scope.DeleteFinishingMasterDetail = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.FinishingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteFinishingMasterDetail/';
            var delFinishingMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delFinishingMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"]("Finishing MRR No " + dataModel.FinishingMRRNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Finishing MRR No " + dataModel.FinishingMRRNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Finishing MRR No " + dataModel.FinishingMRRNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });

        }

        //*********************************************************************Start Reset**********************************************************************
        $scope.clear = function () {
            debugger
            $scope.frmFinishingEntry.$setPristine();
            $scope.frmFinishingEntry.$setUntouched();
            $scope.StyleNo = "";
            $scope.lstSetNoList = "";
            $scope.SetID = "";
            $scope.SizeMRRID = "";
            $scope.ItemID = "";
            $scope.ArticleNo = "";
            $scope.BuyerID = "";
            $scope.BuyerName = "";
            $scope.PIID = "";
            $scope.PINO = "";
            $scope.FInishTypeID = "";
            $scope.UnitID = "";
            $scope.MachineID = "";
            $scope.OperatorID = "";
            $scope.ShiftEngineerID = "";
            $scope.ShiftID = "";
            $scope.StartTime = conversion.NowTime();
            $scope.StopTime = conversion.NowTime();
            $scope.Remarks = "";
            $scope.Length = "";

            $scope.FinishingMRRDate = conversion.NowDateCustom();
            $scope.ListShrinkage = [];
            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            //$scope.IsSaveDisable = true;
            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsfrmShow = true;
            $("#ddlSetNoList").select2("data", { id: 0, text: '--Select Set No--' });
            $("#UnitListD").select2("data", { id: 0, text: '--Select Unit--' });
            $("#MachineList").select2("data", { id: 0, text: '--Select Machine--' });
            $("#ddlFinishType").select2("data", { id: 0, text: '--Select Finish Type--' });
            $("#ddlOperatorList").select2("data", { id: 0, text: '--Select Operator--' });
            $("#ddlDutyOfficerList").select2("data", { id: 0, text: '--Select Shift Engineer--' });
            $("#ddlShiftList").select2("data", { id: 0, text: '--Select Shift--' });
        };
        //*********************************************************************End Reset*************************************************************************
    }]);