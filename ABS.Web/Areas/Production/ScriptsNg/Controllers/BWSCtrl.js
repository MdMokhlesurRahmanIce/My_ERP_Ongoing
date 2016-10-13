/**
 * BWSCtrl.js
 */
app.controller('BWSCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/Configuration/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsBWS = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var message = "";
        $scope.listBWSMaster = [];
        //$scope.btnSaveText = "Save";
        //$scope.btnShowList = "Show List";
        $scope.PageTitle = 'Breakage, Wastage, Stop Creation';
        $scope.ListTitle = 'Breakage, Wastage, Stop Information';
        $scope.ListTitleBWSMaster = 'Breakage, Wastage, Stop List';
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmBWS'; DelFunc = 'DeleteUpdateBWSlist'; DelMsg = 'BWSName'; EditFunc = 'getBWSInfoByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        $scope.loadDepartmentRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDepartment/';
            var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDept.then(function (response) {
                debugger
                $scope.listDept = response.data.ListDept;
                angular.forEach(response.data.ListDept, function (dept) {
                    if ($scope.UserCommonEntity.loggedUserDepartmentID == dept.OrganogramID) {
                        $scope.lstDeptList = dept.OrganogramID;
                        $("#DeptList").select2("data", { id: 0, text: dept.OrganogramName });
                    }
                })
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadDepartmentRecords(0);

        $scope.loadBWSTypeRecords = function () {
            $scope.cmnParam();
            objcmnParam.ParamName = 'BWS';
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBWSType/';
            var listBWSType = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBWSType.then(function (response) {
                $scope.listBWSType = response.data.objListBWSType;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadBWSTypeRecords();

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
                $scope.loadAllBWSMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllBWSMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllBWSMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllBWSMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllBWSMasterRecords(1);
                }
            }
        };

        $scope.loadAllBWSMasterRecords = function (isPaging) {
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
            $scope.gridOptionsBWS = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "BWSID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '27%', 
                    { name: "BWSType", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '27%', 
                    { name: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '27%', 
                    { name: "BWSNo", displayName: "BWS No", title: "BWS No", headerCellClass: $scope.highlightFilteredHeader },//width: '30%',
                    { name: "BWSName", displayName: "BWS Name", title: "BWS Name", headerCellClass: $scope.highlightFilteredHeader },//width: '30%',
                    { name: "Description", displayName: "Description", title: "Description", headerCellClass: $scope.highlightFilteredHeader },//width: '15%', 
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
                    return getPage(1, $scope.gridOptionsBWS.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsBWS.useExternalPagination = false;
                        $scope.gridOptionsBWS.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetBWSInfo/';
            var listBWSMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listBWSMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsBWS.data = response.data.objBWSMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllBWSMasterRecords(0);
        }
        $scope.RefreshMasterList();

        $scope.getBWSInfoByID = function (dataModel) {
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            //$scope.btnSaveText = "Update";
            //$scope.btnShowList = "Show List";
            //$scope.cmnbtnShowHideEnDisable(2);            
            $scope.BWSID = dataModel.BWSID;
            $scope.BWSName = dataModel.BWSName;
            $scope.lstDeptList = dataModel.DepartmentID;
            $scope.lstBWSTypeList = dataModel.BWSType;
            $scope.Description = dataModel.Description;

            angular.forEach($scope.listDept, function (dept) {
                if (dept.OrganogramID == dataModel.DepartmentID) {
                    $("#DeptList").select2("data", { id: dataModel.DepartmentID, text: dept.OrganogramName });
                }
            })

            angular.forEach($scope.listBWSType, function (typ) {
                if (typ.ComboID == dataModel.BWSType) {
                    $("#ddlBWSType").select2("data", { id: dataModel.BWSType, text: typ.ComboName });
                }
            })
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowList = "Show List";                
                //$scope.IsShow = true;
                //$scope.IsShowSave = true;                
            }
            else {
                //$scope.cmnbtnShowHideEnDisable(1);
                $scope.RefreshMasterList();
                $scope.IsShow = false;
                //$scope.IsShowSave = false;                
            }
        }

        $scope.Save = function () {
            debugger
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();
            var BWSInfo = {
                BWSID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.BWSID,
                BWSNo: $scope.BWSNo,
                DepartmentID: $scope.lstDeptList,
                BWSType: $scope.lstBWSTypeList,
                BWSName: $scope.BWSName,
                Description: $scope.Description,
                IsDeleted: false
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [BWSInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateBWS/';
            var BWSSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            BWSSaveUpdate.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
        function (error) {
            console.log("Error: " + error);
        });
        };

        //********---------Delete Data-------************** 
        $scope.DeleteUpdateBWSlist = function (delModel) {
            $scope.cmnParam(); objcmnParam.id = delModel.BWSID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdatePrdBWSlist/';
            var ConfigDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            ConfigDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("BWS Name " + delModel.BWSName + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("BWS Name " + delModel.BWSName + " Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"]("BWS Name " + delModel.BWSName + " Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*********************************************************End Save/Update/Delete**********************************************************

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmBWS.$setPristine();
            $scope.frmBWS.$setUntouched();
            //$scope.cmnbtnShowHideEnDisable(0);
            $scope.BWSName = '';
            $scope.lstDeptList = "";
            $scope.lstBWSTypeList = "";
            $scope.Description = '';
            //$scope.btnSaveText = "Save";
            //$scope.btnShowList = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $("#DeptList").select2("data", { id: 0, text: '--Select Department--' });
            $("#ddlBWSType").select2("data", { id: 0, text: '--Select Type--' });
        };

    }]);