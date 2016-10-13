/**
 * SetSetup.js
 */
app.controller('setSetupCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        $scope.StatusID = 0;
        $scope.IsApproved = false;
        var baseUrl = '/Production/api/SetSetup/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsSetMaster = [];
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var inCallback = false;

        //$scope.IsCreateIcon = false;
        //$scope.IsListIcon = true;

        $scope.SetCount = "";
        $scope.LastSet = "";
        $scope.tempSetList = [];
        $scope.ListItems = [];
        $scope.tempSet = [];
        //var PrevSet = 0;
        lastSetNum = 0;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitle = 'Set Setup Data Entry';
        $scope.ListTitleSetMaster = 'Set Setup Master Records';
        $scope.ListTitleSetSetupDetails = 'Set Setup Details Records';
        $scope.SetDate = conversion.NowDateCustom();
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsSaveShow = false;
        $scope.IsGenerateShow = true;
        $scope.IsfrmShow = true;
        var MessageData = "";
        $scope.IsReadOnly = true;
        $scope.ResetListData = 0;
        refSetID = '';
        refSetDate = '';
        Isclear = 0;
        lastSetNum = 0;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'ArticleNo'; EditFunc = 'getSetMasterById, callGenerate';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 


        //************************************************Start PI Dropdown******************************************************

        //$("#ItemList").on("mousewheel", function (event) {
        //        debugger
        //        var a = 'A';
        //        var b = 'B';
        //        var c = 'C';
        //})

        $scope.loadPIRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPI/';
            var ListPI = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListPI.then(function (response) {
                debugger
                $scope.ListPI = response.data.PIList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadPIRecords(1);
        //**************************************************End PI Dropdown******************************************************

        //************************************************Start Item Dropdown******************************************************        
        $scope.loadItemRecords = function (isPaging) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" || $scope.lstPIList == null ? 0 : $scope.lstPIList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetItem/';
            var ListItem = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListItem.then(function (response) {
                debugger
                $scope.ListItem = response.data.ItemList;
                if (Isclear == 0) { $scope.ResetListData = 0; $scope.reset(); }
                if (objcmnParam.id != 0) {
                    angular.forEach($scope.ListItem, function (tempitems) {
                        if (tempitems.PIID == objcmnParam.id) {
                            $scope.BuyerID = tempitems.BuyerID;
                            $scope.BuyerName = tempitems.BuyerName;
                        }
                    });
                }
                Isclear = 0;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadItemRecords(0);
        //**************************************************End Item Dropdown******************************************************

        //************************************************Start Selected Item Data (Single)****************************************
        $scope.loadSelectedItemData = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.id = $scope.lstItemList == null || $scope.lstItemList == "" || angular.isUndefined($scope.lstItemList) ? 0 : $scope.lstItemList;
            objcmnParam.ItemType = $scope.lstPIList == null || $scope.lstPIList == "" || angular.isUndefined($scope.lstPIList) ? 0 : $scope.lstPIList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSelectedItemData/';
            var ListItemsData = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListItemsData.then(function (response) {
                debugger
                if (objcmnParam.id != 0) {
                    if (angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" || $scope.lstPIList == null) {
                        $scope.Length = 0;
                        $scope.BuyerName = "";
                        $scope.BuyerID = "";
                    }
                    else {
                        $scope.Length = response.data.objItemDataById.Length;
                    }

                    $scope.YarnID = response.data.objItemDataById.YarnID;
                    $scope.WeftYarnID = response.data.objItemDataById.WeftYarnID;
                    $scope.Weave = response.data.objItemDataById.Weave;
                    $scope.YarnCount = response.data.objItemDataById.YarnCount;
                    $scope.YarnRatio = response.data.objItemDataById.YarnRatio;
                    $scope.YarnRatioLot = response.data.objItemDataById.YarnRatioLot;
                    $scope.TotalEnds = response.data.objItemDataById.TotalEnds;
                    $scope.ColorID = response.data.objItemDataById.ColorID;
                    $scope.ColorName = response.data.objItemDataById.ColorName;
                    $scope.BallNo = response.data.objItemDataById.BallNo;

                    $scope.loadRefSetRecords($scope.lstItemList);
                }
                else {
                    $scope.ResetListData = 1;
                    $scope.reset();
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //************************************************Start Selected Item Data (Single)****************************************

        //****************************************************Start Supplier DropdownList******************************************
        $scope.loadSupplierRecords = function (UserTypeID) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = UserTypeID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSupplier/';
            var ListSupplier = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListSupplier.then(function (response) {
                $scope.ListSupplier = response.data.SupplierList;

            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadSupplierRecords(3);
        //****************************************************End Supplier DropdownList********************************************

        //****************************************************Start Reference Set DropdownList*************************************
        $scope.loadRefSetRecords = function (lstItemList) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = lstItemList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetRefSet/';
            var ListRefSet = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListRefSet.then(function (response) {
                $scope.ListRefSet = response.data.RefSetList;
                lastSetNum = response.data.recordsTotal;

                if (refSetID != '' && refSetID != 0) {
                    angular.forEach($scope.ListRefSet, function (ref) {
                        if (ref.SetID == refSetID) {
                            $scope.lstRefSetList = ref.SetID;
                            $("#RefSetList").select2("data", { id: 0, text: ref.SetNo });
                            $scope.RefSetDate = conversion.getDateToString(refSetDate) == '01-01-1900' ? '' : conversion.getDateToString(refSetDate);
                        }
                    });
                    refSetID = '';
                }
                else {
                    $("#RefSetList").select2("data", { id: 0, text: "--Select Set No--" });
                    $scope.RefSetDate = "";
                }


            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //$scope.loadRefSetRecords(1);
        //****************************************************End Reference Set DropdownList***************************************

        //****************************************************Start Get Reference Set Date*************************************
        $scope.getReferenceWiseDate = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.id = $scope.lstRefSetList;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetWiseDate/';
            var ReferenceWiseDate = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ReferenceWiseDate.then(function (response) {
                $scope.RefSetDate = conversion.getDateToString(response.data.objSetWiseDate.RefSetDate);
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        //****************************************************End Get Reference Set Date***************************************

        //************************************************Start Selected Item Data (Single)****************************************
        $scope.getSetMasterById = function (dataModels) {
            debugger

            $scope.cmnParam();
            objcmnParam.id = dataModels.SetMasterID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetSetMasterByID/';
            var ListMasterData = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMasterData.then(function (response) {
                debugger

                $scope.SetMasterID = response.data.objSingleSetMaster.SetMasterID;
                debugger
                if (response.data.objSingleSetMaster.PIID != 0) {
                    $scope.lstPIList = response.data.objSingleSetMaster.PIID;
                    $("#PIList").select2("data", { id: 0, text: response.data.objSingleSetMaster.PINO });
                }
                else {
                    $("#PIList").select2("data", { id: 0, text: "--Select PI No--" });
                }

                $scope.BuyerID = response.data.objSingleSetMaster.BuyerID == 0 ? null : response.data.objSingleSetMaster.BuyerID;
                $scope.BuyerName = response.data.objSingleSetMaster.BuyerName;
                debugger
                if (response.data.objSingleSetMaster.ItemID != 0) {
                    $scope.lstItemList = response.data.objSingleSetMaster.ItemID;
                    $("#ItemList").select2("data", { id: 0, text: response.data.objSingleSetMaster.ArticleNo });
                }
                else {
                    $("#ItemList").select2("data", { id: 0, text: "--Select Article No--" });
                }

                $scope.Length = response.data.objSingleSetMaster.PIItemlength;
                $scope.SetLength = response.data.objSingleSetMaster.SetLength;
                $scope.YarnID = response.data.objSingleSetMaster.YarnID;
                $scope.WeftYarnID = response.data.objSingleSetMaster.WeftYarnID;
                $scope.Weave = response.data.objSingleSetMaster.Weave;
                $scope.YarnCount = response.data.objSingleSetMaster.YarnCount;
                $scope.YarnRatio = response.data.objSingleSetMaster.YarnRatio;
                $scope.YarnRatioLot = response.data.objSingleSetMaster.YarnRatioLot;

                if (response.data.objSingleSetMaster.SupplierID != 0) {
                    $scope.lstSuppList = response.data.objSingleSetMaster.SupplierID;
                    $("#SuppList").select2("data", { id: 0, text: response.data.objSingleSetMaster.SupplierName });
                }
                else {

                    $("#SuppList").select2("data", { id: 0, text: "--Select Supplier--" });
                }

                $scope.TotalEnds = response.data.objSingleSetMaster.TotalEnds;
                $scope.ColorID = response.data.objSingleSetMaster.ColorID;
                $scope.ColorName = response.data.objSingleSetMaster.ColorName;
                //$scope.MachineSpeed = response.data.objSingleSetMaster.MachineSpeed;
                $scope.BallNo = response.data.objSingleSetMaster.NoOfBall;
                $scope.EndsPerCreel = response.data.objSingleSetMaster.EndsPerCreel;
                $scope.LeaseRepeat = response.data.objSingleSetMaster.LeaseRepeat;
                $scope.SetDate = conversion.getDateToString(response.data.objSingleSetMaster.SetDate) == '01-01-1900' ? '' : conversion.getDateToString(response.data.objSingleSetMaster.SetDate);

                debugger
                refSetID = response.data.objSingleSetMaster.RefSetID;
                refSetDate = response.data.objSingleSetMaster.RefSetDate;

                $scope.Remarks = response.data.objSingleSetMaster.Description;
                $scope.MachineSpeed = response.data.objSingleSetMaster.MachineSpeed;

                $scope.loadRefSetRecords(response.data.objSingleSetMaster.ItemID);

                //$scope.IsHidden = true;
                //$scope.IsShow = true;
                //$scope.cmnbtnShowHideEnDisable('false');
                ////$scope.IsSaveShow = true;
                ////$scope.btnSaveText = "Update";
                ////$scope.btnShowText = "Show List";
                //$scope.IsGenerateShow = false;
                //$scope.IsfrmShow = true;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //************************************************Start Selected Item Data (Single)****************************************

        //*********************************************Start Row Number, Set No and Last Set***************************************        

        $scope.SetCalculation = function () {
            debugger
            $scope.tempSetList = [];
            $scope.IsGenerateShow = true;
            $scope.cmnbtnShowHideEnDisable('true');
            //$scope.IsSaveShow = false;
            $scope.IsShow = false;


            if ($scope.Length > $scope.SetLength) {
                $scope.SetCount = $scope.Length / $scope.SetLength;
                $scope.SetCount = Math.floor($scope.SetCount);
                $scope.LastSet = $scope.Length % $scope.SetLength;
            }
            else {
                $scope.SetCount = 1;
                $scope.LastSet = 0;
            }
        }
        //***********************************************End Row Number, Set No and Last Set***************************************

        //*******************************************************Start Generate Row************************************************

        $scope.callGenerate = function (dataModel) {
            debugger

            //$scope.loadItemListRecords(0);
            $scope.tempSetList = [];
            var tempSetMasterID = "";
            if (angular.isUndefined(dataModel)) {
                tempSetMasterID = 0;
            }
            else {
                tempSetMasterID = dataModel.SetMasterID;
            }

            //if (angular.isUndefined($scope.ListRefSet) || $scope.ListRefSet.length < 1) {
            //    PrevSet = 0;
            //}
            //else {
            //    PrevSet = $scope.ListRefSet.length;
            //}

            //$scope.IsShow = true;
            //$scope.cmnbtnShowHideEnDisable('false');
            ////$scope.IsSaveShow = true;
            //$scope.IsGenerateShow = false;
            $scope.generate(tempSetMasterID);
        };

        $scope.generate = function (tempSetMasterID) {
            debugger
            $scope.cmnParam();
            if (tempSetMasterID != 0) {
                objcmnParam.id = tempSetMasterID;
                var apiRoute = baseUrl + 'GetSetDetailByID/';
                ModelsArray = [objcmnParam];
                var listSetDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                listSetDetail.then(function (response) {
                    $scope.tempSetList = response.data.ListSetDetail;

                    $scope.IsShow = $scope.tempSetList.length > 0 ? true : false;
                    if ($scope.tempSetList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); }
                    $scope.IsfrmShow = $scope.tempSetList.length > 0 ? true : false;
                    $scope.IsHidden = $scope.tempSetList.length > 0 ? true : false;
                    $scope.IsGenerateShow = $scope.tempSetList.length > 0 ? false : true;
                },
                function (error) {
                    console.log("Error: " + error);

                });

            }
            else {
                debugger
                var ArticleNos = '';
                debugger
                angular.forEach($scope.ListItem, function (tempitem) {
                    if ($scope.lstItemList == tempitem.ItemID) {
                        ArticleNos = tempitem.ArticleNo;
                    }
                });
                debugger
                var PINOs = '';
                if (!angular.isUndefined($scope.lstPIList) || $scope.lstPIList != "") {
                    angular.forEach($scope.ListPI, function (temppi) {
                        if ($scope.lstPIList == temppi.PIID) {
                            PINOs = temppi.PINO;
                        }
                    });
                }
                debugger
                var SupplierName = "";
                if (!angular.isUndefined($scope.ListSupplier) || $scope.ListSupplier != "") {
                    angular.forEach($scope.ListSupplier, function (tempsup) {
                        if ($scope.lstSuppList == tempsup.UserID) {
                            SupplierName = tempsup.UserName;
                        }
                    });
                }

                var SetNos = "SetNo-";

                for (var i = 1; i <= $scope.SetCount; i++) {
                    $scope.tempSetList.push({
                        SetID: 0, SetNo: SetNos + ( lastSetNum + i), ItemID: $scope.lstItemList, ArticleNo: ArticleNos, //PrevSet
                        ColorID: angular.isUndefined($scope.ColorID) ? null : $scope.ColorID,
                        ColorName: angular.isUndefined($scope.ColorName) ? "" : $scope.ColorName,
                        PIID: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? null : $scope.lstPIList,
                        PINO: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? "" : PINOs,
                        SetLength: $scope.SetLength, YarnID: $scope.YarnID,
                        WeftYarnID: $scope.WeftYarnID, Weave: angular.isUndefined($scope.Weave) ? "" : $scope.Weave,
                        YarnCount: $scope.YarnCount, YarnRatio: $scope.YarnRatio, YarnRatioLot: $scope.YarnRatioLot,
                        SupplierID: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? null : $scope.lstSuppList,
                        SupplierName: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? "" : SupplierName,
                        TotalEnds: angular.isUndefined($scope.TotalEnds) ? null : $scope.TotalEnds,
                        MachineSpeed: angular.isUndefined($scope.MachineSpeed) ? null : $scope.MachineSpeed,
                        EndsPerCreel: angular.isUndefined($scope.EndsPerCreel) ? null : $scope.EndsPerCreel,
                        LeaseRepeat: angular.isUndefined($scope.LeaseRepeat) ? null : $scope.LeaseRepeat,
                        BuyerID: angular.isUndefined($scope.BuyerID) || $scope.BuyerID == "" ? null : $scope.BuyerID,
                        SetDate: angular.isUndefined($scope.SetDate) ? "1/1/1900" : conversion.getStringToDate($scope.SetDate),
                        Description: angular.isUndefined($scope.Remarks) ? "" : $scope.Remarks,
                        NoOfBall: angular.isUndefined($scope.BallNo) ? null : $scope.BallNo
                    })
                }
                if ($scope.LastSet != 0) {
                    $scope.tempSetList.push({
                        SetID: 0, SetNo: SetNos + (parseInt($scope.tempSetList[$scope.tempSetList.length - 1].SetNo.split("-")[1]) + 1),
                        ItemID: $scope.lstItemList, ArticleNo: ArticleNos,
                        ColorID: angular.isUndefined($scope.ColorID) ? null : $scope.ColorID,
                        ColorName: angular.isUndefined($scope.ColorName) ? "" : $scope.ColorName,
                        PIID: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? null : $scope.lstPIList,
                        PINO: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? "" : PINOs,
                        SetLength: $scope.LastSet, YarnID: $scope.YarnID,
                        WeftYarnID: $scope.WeftYarnID, Weave: angular.isUndefined($scope.Weave) ? "" : $scope.Weave,
                        YarnCount: $scope.YarnCount, YarnRatio: $scope.YarnRatio, YarnRatioLot: $scope.YarnRatioLot,
                        SupplierID: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? null : $scope.lstSuppList,
                        SupplierName: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? "" : SupplierName,
                        TotalEnds: angular.isUndefined($scope.TotalEnds) ? null : $scope.TotalEnds,
                        MachineSpeed: angular.isUndefined($scope.MachineSpeed) ? null : $scope.MachineSpeed,
                        EndsPerCreel: angular.isUndefined($scope.EndsPerCreel) ? null : $scope.EndsPerCreel,
                        LeaseRepeat: angular.isUndefined($scope.LeaseRepeat) ? null : $scope.LeaseRepeat,
                        BuyerID: angular.isUndefined($scope.BuyerID) || $scope.BuyerID == "" ? null : $scope.BuyerID,
                        SetDate: angular.isUndefined($scope.SetDate) ? "1/1/1900" : conversion.getStringToDate($scope.SetDate),
                        Description: angular.isUndefined($scope.Remarks) ? "" : $scope.Remarks,
                        NoOfBall: angular.isUndefined($scope.BallNo) ? null : $scope.BallNo
                    })
                }
                $scope.IsShow = $scope.tempSetList.length > 0 ? true : false;
                if ($scope.tempSetList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); }
                $scope.IsfrmShow = $scope.tempSetList.length > 0 ? true : false;
                $scope.IsHidden = $scope.tempSetList.length > 0 ? true : false;
                $scope.IsGenerateShow = $scope.tempSetList.length > 0 ? false : true;
            }
        }
        //*******************************************************End Generate Row**************************************************

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
                $scope.loadSetSetupMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSetSetupMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadSetSetupMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSetSetupMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadSetSetupMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadSetSetupMasterRecords = function (isPaging) {
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
            $scope.gridOptionsSetMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "SetMasterID", displayName: "SetMasterID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINo", displayName: "PI NO", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "NoOfBall", displayName: "No Of Ball", title: "No Of Ball", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIItemlength", displayName: "PI Length", title: "PI Length", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierName", displayName: "Supplier Name", title: "Supplier Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerName", displayName: "Buyer Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefSetNo", displayName: "Ref. Set No", title: "Ref. Set No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetDate", displayName: "Set Date", title: "Set Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefSetDate", displayName: "Ref. Set Date", title: "Ref. Set Date", width: '10%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", title: "Remarks", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsSetMaster.useExternalPagination = false;
                        $scope.gridOptionsSetMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetSetSetupMaster/';
            ModelsArray = [objcmnParam];
            var listSetMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listSetMaster.then(function (response) {
                angular.forEach(response.data.ListSetMaster, function (items) {
                    if (items.RefSetDate == "1900-01-01T00:00:00") {
                        items.RefSetDate = "N/A";
                    }
                    if (items.PIItemlength == 0) {
                        items.PIItemlength = "N/A";
                    }
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsSetMaster.data = response.data.ListSetMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadSetSetupMasterRecords(0);
        }
        $scope.RefreshMasterList();

        //***************************************************End Set Master Dynamic Grid******************************************************

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
            //$scope.IsShow = $scope.IsShow ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                //$scope.IsCreateIcon = false;
                //$scope.IsListIcon = true;
                ////$scope.IsShow = true;
                //$scope.IsfrmShow = true;
                //$scope.IsGenerateShow = true;
                //$scope.IsShow = $scope.tempSetList.length < 1 || angular.isUndefined($scope.tempSetList.length) ? false : true;
                //$scope.IsSaveShow = $scope.tempSetList.length < 1 || angular.isUndefined($scope.tempSetList.length) ? false : true;;
                //$scope.IsGenerateShow = $scope.tempSetList.length < 1 || angular.isUndefined($scope.tempSetList.length) ? true : false;;
            }
            else {
                //$scope.btnShowText = "Create";
                //$scope.IsCreateIcon = true;
                //$scope.IsListIcon = false;
                $scope.pagination.pageNumber = 1;
                $scope.loadSetSetupMasterRecords(1);
                $scope.IsfrmShow = false;
                $scope.IsShow = false;
                //$scope.IsSaveShow = false;
                $scope.IsGenerateShow = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.Save = function () {
            debugger
            //MessageData = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var SetSetupMaster = {
                SetMasterID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.SetMasterID,
                PIID: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? null : $scope.lstPIList,
                SetDate: angular.isUndefined($scope.SetDate) || $scope.SetDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.SetDate),
                ItemID: $scope.lstItemList,
                PIItemlength: angular.isUndefined($scope.Length) || $scope.Length == "" ? 0 : $scope.Length,
                SupplierID: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? null : $scope.lstSuppList,
                BuyerID: angular.isUndefined($scope.BuyerID) || $scope.BuyerID == "" ? null : $scope.BuyerID,
                RefSetID: angular.isUndefined($scope.lstRefSetList) || $scope.lstRefSetList == "" ? null : $scope.lstRefSetList,
                RefSetDate: angular.isUndefined($scope.RefSetDate) || $scope.RefSetDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.RefSetDate),
                Description: $scope.Remarks,
                StatusID: $scope.StatusID
            };

            //*************----Detail Data---**************
            debugger
            $scope.tempSetLists = [];
            angular.forEach($scope.tempSetList, function (set) {
                $scope.tempSetLists.push({
                    SetID: set.SetID, SetNo: set.SetNo, ItemID: set.ItemID, ColorID: set.ColorID, ColorName: set.ColorName,
                    PIID: set.PIID, PINO: set.PINO, SetLength: set.SetLength, YarnID: set.YarnID, WeftYarnID: set.WeftYarnID,
                    Weave: set.Weave, YarnCount: set.YarnCount, YarnRatio: set.YarnRatio, YarnRatioLot: set.YarnRatioLot,
                    SupplierID: set.SupplierID, SupplierName: set.SupplierName, TotalEnds: set.TotalEnds, MachineSpeed: set.MachineSpeed,
                    EndsPerCreel: set.EndsPerCreel, LeaseRepeat: set.LeaseRepeat, BuyerID: set.BuyerID, SetDate: set.SetDate,
                    Description: set.Description, NoOfBall: set.NoOfBall
                })
            });

            if ($scope.tempSetLists.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [SetSetupMaster, $scope.tempSetLists, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateSetSetupMasterDetail/';
            var SetSetupDetailsCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SetSetupDetailsCreateUpdate.then(function (response) {
                if (response.result != '') {
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
            $scope.cmnParam();
            objcmnParam.id = delModel.SetMasterID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DelUpdateSetMasterDetail/';
            var SetMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.clear();
                    Command: toastr["success"]("Data has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*********************************************************End Save/Update/Delete**********************************************************

        //**************************************************************Start Reset/Clear**********************************************************
        $scope.clear = function () {
            $scope.frmSetSetupEntry.$setPristine();
            $scope.frmSetSetupEntry.$setUntouched();
            //$scope.IsCreateIcon = false;
            //$scope.IsListIcon = true;
            //$scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            Isclear = 1;
            $scope.YarnID = "";
            $scope.WeftYarnID = "";
            $scope.Weave = "";
            $scope.YarnCount = "";
            $scope.YarnRatio = "";
            $scope.YarnRatioLot = "";
            $scope.TotalEnds = "";
            $scope.ColorID = "";
            $scope.ColorName = "";
            $scope.BallNo = "";
            $scope.EndsPerCreel = "";
            $scope.LeaseRepeat = "";
            $scope.MachineSpeed = "";
            $scope.SetLength = "";
            $scope.BuyerName = "";

            $scope.lstPIList = "";
            $scope.lstItemList = "";
            $scope.Length = "";
            $scope.lstSuppList = "";
            $scope.BuyerID = "";
            $scope.lstRefSetList = "";
            $scope.Remarks = "";

            $scope.SetDate = conversion.NowDateCustom();
            $scope.RefSetDate = "";

            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            //$scope.IsSaveShow = false;
            $scope.IsGenerateShow = true;
            $scope.IsDeleted = false;

            $scope.gridOptionsSetMaster.data = [];

            $scope.SetCount = "";
            $scope.LastSet = "";
            $scope.tempSetList = [];
            //PrevSet = 0;
            lastSetNum = 0;

            //$scope.ListPI = [];
            $scope.ListItem = [];
            //$scope.ListSupplier = [];
            $scope.ListRefSet = [];

            $scope.loadItemRecords(0);
            //$scope.loadSupplierRecords(1);
            //$scope.loadRefSetRecords(1);
            //$scope.loadPIRecords(1);

            $("#ItemList").select2("data", { id: 0, text: "--Select Article No--" });
            $("#SuppList").select2("data", { id: 0, text: "--Select Supplier--" });
            $("#RefSetList").select2("data", { id: 0, text: "--Select Set No--" });
            $("#PIList").select2("data", { id: 0, text: "--Select PI No--" });

        };

        $scope.reset = function () {
            $scope.frmSetSetupEntry.$setPristine();
            $scope.frmSetSetupEntry.$setUntouched();
            //$scope.IsCreateIcon = false;
            //$scope.IsListIcon = true;
            //$scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = false;

            $scope.YarnID = "";
            $scope.WeftYarnID = "";
            $scope.Weave = "";
            $scope.YarnCount = "";
            $scope.YarnRatio = "";
            $scope.YarnRatioLot = "";
            $scope.TotalEnds = "";
            $scope.ColorID = "";
            $scope.ColorName = "";
            $scope.BallNo = "";
            $scope.EndsPerCreel = "";
            $scope.LeaseRepeat = "";
            $scope.MachineSpeed = "";
            $scope.SetLength = "";
            $scope.Length = "";
            $scope.lstSuppList = "";
            $scope.lstRefSetList = "";
            $scope.ListRefSet = [];
            $scope.Remarks = "";
            $scope.SetDate = conversion.NowDateCustom();
            $scope.RefSetDate = "";

            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.IsfrmShow = true;
            $scope.cmnbtnShowHideEnDisable('true');
            //$scope.IsSaveShow = false;
            $scope.IsGenerateShow = true;
            $scope.IsDeleted = false;

            $scope.gridOptionsSetMaster.data = [];

            $scope.SetCount = "";
            $scope.LastSet = "";
            $scope.tempSetList = [];
            //PrevSet = 0;
            lastSetNum = 0;
            if ($scope.ResetListData == 0) {
                $scope.BuyerName = "";
                $scope.BuyerID = "";
                $scope.lstItemList = "";
                $("#ItemList").select2("data", { id: 0, text: "--Select Article No--" });
            }
            $("#SuppList").select2("data", { id: 0, text: "--Select Supplier--" });
            $("#RefSetList").select2("data", { id: 0, text: "--Select Set No--" });
        }
        //**************************************************************End Reset/Clear**********************************************************

        //**************************** Approve Notification ********************************
        debugger
        var ApprovalModel = $localStorage.notificationStorageModel;
        var ApprovalMenuID = $localStorage.notificationStorageMenuID;

        //IsApproval Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
        var IsApproval = $localStorage.notificationStorageIsApproved;

        //IsDelaine Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
        var IsDelaine = $localStorage.notificationStorageIsDeclained;
        try {
            $scope.APModalPageTitle = ApprovalModel.CustomCode;
            $scope.DCModalPageTitle = ApprovalModel.CustomCode;
        }
        catch (e) {

        }
        $scope.IsApproved = IsApproval;
        $scope.IsDelained = IsDelaine;

        //Page Display will be false after execution of approved/declined event
        $scope.PageDisplay = true;
        if ($scope.IsApproved) {
            debugger
            $scope.UserCommonEntity.EnableSavebtn = false;
            Isclear = 1;
            var dataModels = {};
            dataModels.SetMasterID = ApprovalModel.TransactionID;
            $scope.getSetMasterById(dataModels);
            $scope.callGenerate(dataModels);

            //Code That Refresh Local Storage
            $localStorage.notificationStorageMenuID = 0;
            $localStorage.notificationStorageMasterID = 0;
            $localStorage.notificationStorageIsApproved = false;
            $localStorage.notificationStorageIsDeclained = false;

        }
        //Approved Or Declained Operation
        $scope.ApprovedMethod = function () {
            //Save Method Test 
            $scope.StatusID = ApprovalModel.WFDStatusID;

            $scope.cmnParam();
            var SetSetupMaster = {
                SetMasterID: $scope.SetMasterID,
                PIID: angular.isUndefined($scope.lstPIList) || $scope.lstPIList == "" ? null : $scope.lstPIList,
                SetDate: angular.isUndefined($scope.SetDate) || $scope.SetDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.SetDate),
                ItemID: $scope.lstItemList,
                PIItemlength: angular.isUndefined($scope.Length) || $scope.Length == "" ? 0 : $scope.Length,
                SupplierID: angular.isUndefined($scope.lstSuppList) || $scope.lstSuppList == "" ? null : $scope.lstSuppList,
                BuyerID: angular.isUndefined($scope.BuyerID) || $scope.BuyerID == "" ? null : $scope.BuyerID,
                RefSetID: angular.isUndefined($scope.lstRefSetList) || $scope.lstRefSetList == "" ? null : $scope.lstRefSetList,
                RefSetDate: angular.isUndefined($scope.RefSetDate) || $scope.RefSetDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.RefSetDate),
                Description: $scope.Remarks,
                StatusID: $scope.StatusID
            };

            //*************----Detail Data---**************
            debugger
            $scope.tempSetLists = [];
            angular.forEach($scope.tempSetList, function (set) {
                $scope.tempSetLists.push({
                    SetID: set.SetID, SetNo: set.SetNo, ItemID: set.ItemID, ColorID: set.ColorID, ColorName: set.ColorName,
                    PIID: set.PIID, PINO: set.PINO, SetLength: set.SetLength, YarnID: set.YarnID, WeftYarnID: set.WeftYarnID,
                    Weave: set.Weave, YarnCount: set.YarnCount, YarnRatio: set.YarnRatio, YarnRatioLot: set.YarnRatioLot,
                    SupplierID: set.SupplierID, SupplierName: set.SupplierName, TotalEnds: set.TotalEnds, MachineSpeed: set.MachineSpeed,
                    EndsPerCreel: set.EndsPerCreel, LeaseRepeat: set.LeaseRepeat, BuyerID: set.BuyerID, SetDate: set.SetDate,
                    Description: set.Description, NoOfBall: set.NoOfBall
                })
            });

            if ($scope.tempSetLists.length == 0) {
                Command: toastr["warning"]("Please Generate row!!!!");
                return;
            }
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [SetSetupMaster, $scope.tempSetLists, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateSetSetupMasterDetail/';
            var SetSetupDetailsCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SetSetupDetailsCreateUpdate.then(function (response) {
                if (response.result != '') {
                    $scope.clear();

                    //Approval Method Call
                    ApprovalModel.Comments = $scope.commentsModle;
                    ApprovalModel.CreatorID = $('#hUserID').val();
                    ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
                    ApprovalModel.LoggedUserID = $('#hUserID').val();
                    $scope.commentsModle = "";
                    modal_fadeOut();
                    var apiRoute = '/SystemCommon/api/SystemCommonLayout/ApproveNotification/';
                    var approvalProcess = crudService.postNotification(apiRoute, ApprovalModel);
                    approvalProcess.then(function (response) {
                        if (response.data == 200) {
                            $scope.IsApproveChoosen = false;
                            $scope.IsDeclinChoosen = true;
                            $scope.UserCommonEntity.EnableSavebtn = true;
                            $scope.StatusID = 0;
                            //Hide Form
                            $scope.PageDisplay = false;
                            ShowCustomToastrMessage(response);
                            $scope.clear();
                            //  modal_fadeOut_Company();
                        }
                    },
                    function (error) {
                        ("Error: " + error);
                    });
                    //Approval Method Call
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                console.log("Error: " + error);
            })
            //Save Method Test

        }
        $scope.DeclinedMethod = function () {
            ApprovalModel.Comments = $scope.commentsModle;
            ApprovalModel.CreatorID = $('#hUserID').val();
            ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
            ApprovalModel.LoggedUserID = $('#hUserID').val();
            $scope.commentsModle = "";
            modal_fadeOutDeclained();
            var apiRoute = '/SystemCommon/api/SystemCommonLayout/DeclainedNotification/';
            var declaineProcess = crudService.postNotification(apiRoute, ApprovalModel);
            declaineProcess.then(function (response) {
                if (response.data == 201) {
                    $scope.IsApproveChoosen = false;
                    $scope.IsDeclinChoosen = true;

                    $scope.UserCommonEntity.EnableSavebtn = true;
                    $scope.StatusID = 0;
                    //Hide Form
                    $scope.PageDisplay = false;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    //  modal_fadeOut_Company();
                }
            },
            function (error) {
                ("Error: " + error);
            });
        }
    }]);



function modal_fadeOut() {
    $("#approveNotificationModal").fadeOut(200, function () {
        $('#approveNotificationModal').modal('hide');
    });
}
function modal_fadeOutDeclained() {
    $("#declainedNotificationModal").fadeOut(200, function () {
        $('#declainedNotificationModal').modal('hide');
    });
}