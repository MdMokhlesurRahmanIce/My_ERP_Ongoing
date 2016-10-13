/*
*   WastageEntryCtrl.js
*/
app.controller('wastageEntryCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/WastageEntry/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsWastageMaster = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        //$scope.btnSaveText = "Save";
        //$scope.btnShowText = "Show List";
        $scope.PageTitleMaster = 'Wastage Master Entry';
        $scope.ListTitle = 'Wastage Records';
        $scope.ListTitleMaster = 'Wastage Master Records';
        $scope.PageTitleDetail = 'Wastage Detail Entry';
        $scope.ListWastageDetails = [];
        $scope.ListWastageDetailsDelete = [];
        $scope.MainDetailIndexWiseData = "";
        //$scope.IsSaveDisable = true;
        $scope.IsHidden = true;
        $scope.IsShow = false;
        $scope.IsfrmShow = true;
        $scope.IsShowDetail = true;
        //$scope.IsShowSave = true;
        $scope.ModelState = "";
        $scope.DepartmentID = "";
        $scope.drpPageTitle = "Department List";
        $scope.WastageDate = conversion.NowDateCustom();
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteWastageMasterDetail'; DelMsg = 'WastageNo'; EditFunc = 'WastageMasterDetailByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 
        debugger
        $scope.LoadTimer = function () {
            debugger
            $scope.StopTime = conversion.NowTime();
            $scope.StartTime = conversion.NowTime();
            $scope.IsNextDate = false;
        }

        //$scope.loadDepartmentRecords = function (dataModel) { 
        //debugger
        //$scope.cmnParam();
        //objcmnParam.id = angular.isUndefined(dataModel.WastageID) ? 0 : dataModel.WastageID;        
        //    ModelsArray = [objcmnParam];
        //    var apiRoute = baseUrl + 'GetDepartment/';
        //    var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
        //    listDept.then(function (response) {
        //        debugger
        //        $scope.ListDept = response.data.ListDept;
        //        angular.forEach(response.data.ListDept, function (dept) {
        //            if ($scope.UserCommonEntity.loggedUserDepartmentID == dept.OrganogramID) {
        //                $scope.DepartmentID = dept.OrganogramID;
        //                $("#ddlDepartment").select2("data", { id: 0, text: dept.OrganogramName });
        //            }
        //        })
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //$scope.loadDepartmentRecords(0);

        $scope.selectNode = function (val) {
            debugger
            $scope.Department = val.Name;
            $scope.DepartmentID = val.ID;
        }

        $scope.treedubbleClick = function (val) {
            debugger
            $scope.Department = val.Name;
            $scope.DepartmentID = val.ID;
            $scope.modal_fadeOutTree();
        }

        $scope.modal_fadeOutTree = function () {
            $("#drpModalDept").fadeOut(200, function () {
                $('#drpModalDept').modal('hide');
            });
        }

        $scope.loadDepartmentRecords = function () {
            debugger
            $scope.Departments = [];
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDepartmentDetails/';            
            var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDept.then(function (response) {
                debugger
                $scope.Departments = response.data.ListDeptDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadItemRecords = function (dataM) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 6;            
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetArticle/';            
            var ListItem = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListItem.then(function (response) {
                debugger
                $scope.ListArticle = response.data.ListArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadItemRecords(0);
        //**************************************************End Machine Dropdown******************************************************

        $scope.loadWastageUnitRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = angular.isUndefined(dataModel.ItemID) ? 0 : dataModel.ItemID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetUnitSingle/';            
            var listUnit = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listUnit.then(function (response) {
                //$scope.ListUnit = response.data.ListUnit;
                dataModel.UnitID = response.data.ListUnit.UOMID;
                dataModel.UOMName = response.data.ListUnit.UOMName;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //$scope.loadWastageUnitRecords(0);

        //************************************************Start SetNo wise single Records******************************************************

        //**************************************************End SetNo wise single Records******************************************************

        //*********************************************************Start Beam Dropdown*********************************************************



        //***************************************************End Beam Dropdown******************************************************

        //************************************************Start Shift Dropdown******************************************************        

        //***************************************************End Machine Stop Cause Dropdown******************************************************

        //********************************************************Start Row Add in Detail Grid***********************************************        
        $scope.AddItemToList = function () {
            debugger
            $scope.ModelState = "Save";
            $scope.ListWastageDetails.push({
                WastageDetailID: 0, WastageID: 0, ItemID: 0, Qty: '', UnitID: 0, UOMName: '', Remarks: '',
                ModelState: $scope.ModelState
            });
            //$scope.IsSaveDisable = false;
            $scope.cmnbtnShowHideEnDisable('false');
            $scope.IsShow = true;
        }
        //**********************************************************End Row Add in Detail Grid***********************************************

        //*****************************************************Start Row Remove from Main Detail Grid****************************************
        $scope.DeleteMainDetail = function (index) {
            debugger
            $scope.MainDetailIndexWiseData = $scope.ListWastageDetails[index];
            $scope.ModelState = "Delete";

            if ($scope.MainDetailIndexWiseData.ModelState == "Update")
                $scope.ListWastageDetailsDelete.push({
                    WastageDetailID: $scope.MainDetailIndexWiseData.WastageDetailID, WastageID: $scope.MainDetailIndexWiseData.WastageID,
                    ItemID: $scope.MainDetailIndexWiseData.ItemID, Qty: $scope.MainDetailIndexWiseData.Qty, UnitID: $scope.MainDetailIndexWiseData.UnitID,
                    UOMName: $scope.MainDetailIndexWiseData.UOMName, Remarks: $scope.MainDetailIndexWiseData.Remarks, ModelState: $scope.ModelState = "Delete"
                });

            $scope.ListWastageDetails.splice(index, 1);
            $scope.MainDetailIndexWiseData = "";
            //$scope.IsSaveDisable = $scope.ListWastageDetails.length > 0 ? false : true;
            if ($scope.ListWastageDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            $scope.IsShow = $scope.ListWastageDetails.length > 0 ? true : false;
        }
        //*******************************************************End Row Remove from Main Detail Grid****************************************

        //***************************************************Start Set Master Dynamic Grid******************************************************
        //Pagination
        $scope.pagination = {
            paginationPageWastages: [15, 25, 50, 75, 100, 500, 1000, "All"],
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
                $scope.loadWastageMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadWastageMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadWastageMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadWastageMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadWastageMasterRecords(1);
                }
            }
        };

        //**********----Get All Sales DO Master Record----***************
        $scope.loadWastageMasterRecords = function (isPaging) {
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
            $scope.gridOptionsWastageMaster = {
                enableGridMenu: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "WastageID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WastageNo", displayName: "Wastage No", title: "Wastage No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WastageDate", displayName: "Wastage Date", title: "Wastage Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                        $scope.gridOptionsWastageMaster.useExternalPagination = false;
                        $scope.gridOptionsWastageMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetWastageMaster/';
            ModelsArray = [objcmnParam];
            var listWastageMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listWastageMaster.then(function (response) {

                angular.forEach(response.data.WastageMaster, function (items) {
                    if (items.WastageDate == "1900-01-01T00:00:00") {
                        items.WastageDate = "";
                    }
                });

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsWastageMaster.data = response.data.WastageMaster;

                $scope.lblMessage = '';
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);

            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadWastageMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //***************************************************End Set Master Dynamic Grid******************************************************

        $scope.WastageMasterDetailByID = function (Master) {
            debugger
            //var WastageMasterID = angular.isUndefined(Master.WastageID) ? 0 : Master.WastageID;
            debugger
            $scope.WastageID = Master.WastageID;
            $scope.DepartmentID = "";
            $scope.WastageDate = Master.WastageDate == "" ? "" : conversion.getDateToString(Master.WastageDate);
            $scope.Remarks = Master.Remarks;

            $scope.cmnParam();
            objcmnParam.id = Master.DepartmentID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDepartmentByID/';
            var singleDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            singleDept.then(function (response) {
                debugger
                $scope.DepartmentID = response.data.SingleDept.OrganogramID;
                $scope.Department = response.data.SingleDept.OrganogramName;
            },
            function (error) {
                console.log("Error: " + error);
            });
            
            objcmnParam.id = Master.WastageID;
            var apiRouteWastageDetail = baseUrl + 'GetWastageDetailByID/';
            ModelsArray = [objcmnParam];
            var listWastageDetail = crudService.postMultipleModel(apiRouteWastageDetail, ModelsArray, $scope.HeaderToken.get);
            listWastageDetail.then(function (response) {
                debugger
                $scope.ListWastageDetails = [];
                angular.forEach(response.data.WastageDetailByID, function (WDetail) {
                    $scope.ListWastageDetails.push({
                        WastageDetailID: WDetail.WastageDetailID, WastageID: WDetail.WastageID, ItemID: WDetail.ItemID,
                        Qty: WDetail.Qty, UnitID: WDetail.UnitID, UOMName: WDetail.UOMName, Remarks: WDetail.Remarks, ModelState: 'Update'
                    });
                })

                if ($scope.ListWastageDetails.length > 0) {
                    $scope.IsHidden = true;
                    $scope.IsfrmShow = true;
                    $scope.IsShow = true;
                    //$scope.IsShowSave = true;
                    $scope.IsShowDetail = true;
                    $scope.cmnbtnShowHideEnDisable('false');
                    //$scope.IsSaveDisable = false;
                    //$scope.btnSaveText = "Update";
                    //$scope.btnShowText = "Show List";
                }
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
                //$scope.IsShowDetail = true;
                //$scope.IsShowSave = true;
                //$scope.IsShow = ($scope.ListWastageDetails.length < 1 || angular.isUndefined($scope.ListWastageDetails.length)) ? false : true;
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                $scope.IsShowDetail = false;
                //$scope.IsShowSave = false;
                $scope.IsShow = false;
            }
        };

        //********************************************************End Master List ShowHide**************************************************        

        //********************************************************End Save All Data*********************************************************
        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();

            var ItemMaster = {
                WastageID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.WastageID,
                DepartmentID: $scope.DepartmentID,
                WastageDate: $scope.WastageDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.WastageDate),
                Remarks: $scope.Remarks
            };

            if ($scope.ListWastageDetailsDelete.length > 0) {
                angular.forEach($scope.ListWastageDetailsDelete, function (DelItem) {
                    $scope.ListWastageDetails.push({
                        WastageDetailID: DelItem.WastageDetailID, WastageID: DelItem.WastageID, ItemID: DelItem.ItemID,
                        Qty: DelItem.Qty, UnitID: DelItem.UnitID, Remarks: DelItem.Remarks, ModelState: DelItem.ModelState
                    });
                })
            }

            if ($scope.ListWastageDetails.length == 0) {
                Command: toastr["warning"]("Please input at least one Wastage Detail.");
                return;
            }
            debugger
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var apiRoute = baseUrl + 'SaveUpdateWastage/';
            ModelsArray = [ItemMaster, $scope.ListWastageDetails, objcmnParam];
            var SaveUpdateWastage = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateWastage.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check ang Try again!");
                }
            },
            function (error) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check ang Try again!");
            });
        };
        //********************************************************End Save All Data*********************************************************

        //********************************************************Start Delete Data*********************************************************
        $scope.DeleteWastageMasterDetail = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.WastageID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteWastageMasterDetail/';
            var delPrdWastageMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delPrdWastageMasterDetail.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"]("Wastage No " + dataModel.WastageNo + " has been deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Wastage No " + dataModel.WastageNo + " not Deleted, Please Check ang Try again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Wastage No " + dataModel.WastageNo + " not Deleted, Please Check ang Try again!");
                console.log("Error: " + error);
            });
        }
        //*********************************************************End Delete Data**********************************************************

        //*********************************************************************Start Reset**********************************************************************
        $scope.clear = function () {
            debugger
            $scope.frmWastageEntry.$setPristine();
            $scope.frmWastageEntry.$setUntouched();
            $scope.Departments = [];
            $scope.WastageID = 0;
            $scope.DepartmentID = '';
            $scope.Department = '';
            $scope.WastageDate = conversion.NowDateCustom();
            $scope.Remarks = "";
            $scope.ListWastageDetails = [];
            //$scope.btnSaveText = "Save";
            //$scope.btnShowText = "Show List";
            $scope.ModelState = "";
            //$scope.IsSaveDisable = true;
            $scope.IsHidden = true;
            $scope.IsShow = false;
            $scope.IsShowDetail = true;
            $scope.IsfrmShow = true;
            //$scope.IsShowSave = true;
            $scope.ListWastageDetailsDelete = [];
            $scope.MainDetailIndexWiseData = "";
            $scope.CmnDelModel = "";
            $("#ddlDepartment").select2("data", { id: 0, text: '--Select Department--' });
        };
        //*********************************************************************End Reset*************************************************************************

    }]);