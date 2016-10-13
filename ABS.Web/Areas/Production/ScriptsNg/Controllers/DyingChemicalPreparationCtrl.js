/// <reference path="../Service/CrudService.js" />
/// <reference path="Dying ChemicalSetupCtrl.js" />
app.controller('DyingChemicalPreparationCtrl', ['$scope', 'commonComboBoxGetDataService', 'ChemicalSetupService', 'crudService', 'conversion', '$localStorage',
    function ($scope, commonComboBoxGetDataService, ChemicalSetupService, crudService, conversion, $localStorage) {

        var baseUrl = '/Production/api/ChemicalSetup/';
        var dropDwonUrl = '/Production/api/ProductionDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.gridOptions = [];
        //$scope.gridOptionsMSetup = [];
        //$scope.gridOptionsMSetup.enableCellEditOnFocus = true;

        var companyID = 1;
        var loggedUser = 0;
        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        $scope.ChemicalSetupID = 0;
        //$scope.btnSaveUpdateText = "Save";
        $scope.PageTitle = 'Create Chemical Preparation Master';
        $scope.ListDetail = 'Chemical Preparation Detail';
        $scope.ListMaster = 'Chemical Preparation List';

        $scope.IsHidden = true;
        $scope.IsShow = false;

        $scope.ListChemicalSetupGrid = [];
        $scope.ListColor = [];
        $scope.ListUOM = [];
        $scope.ListChemical = [];
        $scope.ListChemicalSetupDetail = [];

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteDyingChemPreparation'; DelMsg = 'ColorName'; EditFunc = 'EditChemicalSetup';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************

        //************************************************ Switch between show and hide ***********************************        
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
        //************************************************ Switch between show and hide ***********************************               
        //$scope.tempSet = [];

        //Page Load Color
        //**************** Load Color Dropdown ************************
        function loadRecords_DyingcolorDropdown(isPaging) {
            var apiRoute = dropDwonUrl + 'GetColor/';
            var processColor = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processColor.then(function (response) {
                $scope.ListColor = response.data;  //Set Default
                //angular.forEach($scope.ListColor, function (tems) {
                //    $scope.tempSet.push({ ItemColorID: tems.ItemColorID, ColorName: tems.ColorName });
                //});
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_DyingcolorDropdown(0);
        //**************** Load UOM Dropdown ************************
        function loadRecords_UOMDropDown(isPaging) {
            var apiRoute = dropDwonUrl + 'GetUOM/';
            var processUOM = commonComboBoxGetDataService.GetAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging, $scope.HeaderToken.get);
            processUOM.then(function (response) {
                $scope.ListUOM = response.data;  //Set Default
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_UOMDropDown(0);
        //**************** Load Checmical Dropdown ************************
        function loadRecords_ChecmicalDropDown(isPaging) {
            //For Chemical Group ID is 5
            var GroupID = 5;
            var apiRoute = dropDwonUrl + 'GetChemicalItem/';
            var processChecmicalDropdownLoad = commonComboBoxGetDataService.GetItem(apiRoute, companyID, loggedUser, page, pageSize, isPaging, GroupID, $scope.HeaderToken.get);
            processChecmicalDropdownLoad.then(function (response) {
                $scope.ListChemical = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_ChecmicalDropDown(0);
        //**********----Load View List----***************
        //function loadRecords_ChemicalSetupView(isPaging) {

        //    var apiRoute = baseUrl + 'GetChemicalSetupList/';
        //    var processMenues = ChemicalSetupService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
        //    processMenues.then(function (response) {
        //        $scope.ListChemicalSetupGrid = response.data  //Set Default 

        //        //$scope.gridOptionsMSetup.data = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadRecords_ChemicalSetupView(0);

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
                $scope.loadAllDyingChemPrepMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllDyingChemPrepMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllDyingChemPrepMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllDyingChemPrepMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllDyingChemPrepMasterRecords(1);
                }
            }
        };

        $scope.loadAllDyingChemPrepMasterRecords = function (isPaging) {
            $scope.gridOptions.enableFiltering = true;

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
            $scope.gridOptions = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "ChemicalSetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Qty", displayName: "Quantity", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UnitName", displayName: "Unit", headerCellClass: $scope.highlightFilteredHeader },
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
                    return getPage(1, $scope.gridOptions.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptions.useExternalPagination = false;
                        $scope.gridOptions.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetChemicalSetupList/';
            var listChemPrepMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listChemPrepMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptions.data = response.data.finalList;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllDyingChemPrepMasterRecords(0);
        }
        $scope.RefreshMasterList();
        //*************************************************End Show Chemical Process List Information Dynamic Grid*******************************************************


        //**********----Save----***************
        $scope.Save = function () {
            if ($scope.ListChemicalSetupDetail.length == 0) {
                Command: toastr["info"]("Please Add Details");
                return;
            }
            var varColorID = 0;
            var UnitID = 0;
            if (angular.isObject($scope.ddlColor)) {
                varColorID = $scope.ddlColor.ItemColorID;
            }
            else {
                varColorID = $scope.ddlColor;
            }

            if (angular.isObject($scope.ddlUOM)) {
                UnitID = $scope.ddlUOM.UOMID;
            }
            else {
                UnitID = $scope.ddlUOM;
            }

            var master = {
                ChemicalSetupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.ChemicalSetupID,
                ColorID: varColorID,
                Qty: $scope.Qty,
                DepartmentID: $scope.UserCommonEntity.loggedUserBranchID,
                UserID: $scope.UserCommonEntity.loggedUserID,
                UnitID: UnitID,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                IsDeleted: false
            };

            var details = $scope.ListChemicalSetupDetail;
            //isExisting = 0;
            //if (isExisting === 0) {
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [master, details, $scope.UserCommonEntity];
            var apiRoute = baseUrl + 'SaveChemicalPrepartion/';
            var ChemPrepMasterDetails = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            ChemPrepMasterDetails.then(function (response) {
                response.data = response;
                if (response.data.result == 1) {
                    $scope.clear();
                    response.data = 1;
                    ShowCustomToastrMessage(response);
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
        //**********----Add Event----***************
        $scope.AddDetails = function () {
            var varItemID = 0;
            var varArticleNo = "";
            var varQty = 0;
            var varUnitID = 0;
            var varUOMName = "";
            var listChemical = $scope.ListChemical;
            var listUOM = $scope.ListUOM;

            if (angular.isObject($scope.ddlChemical)) {
                varItemID = $scope.ddlChemical.ItemID;
                varArticleNo = $scope.ddlChemical.ArticleNo;
            }
            else {
                angular.forEach($scope.ListChemical, function (value, key) {
                    if (value.ItemID == $scope.ddlChemical) {
                        varItemID = value.ItemID;
                        varArticleNo = value.ArticleNo;
                    }
                });
            }

            if (angular.isObject($scope.ddlUOMDetails)) {
                varUnitID = $scope.ddlUOMDetails.UOMID;
                varUOMName = $scope.ddlUOMDetails.UOMName;
            }
            else {
                angular.forEach($scope.ListChemical, function (value, key) {
                    if (value.UOMID == $scope.ddlUOMDetails) {
                        varUnitID = value.UOMID;
                        varUOMName = value.UOMName;
                    }
                });
            }


            if (true) {
                var obj = {
                    //ChemicalSetupDetailID:$scope.
                    ItemID: varItemID,
                    ArticleNo: varArticleNo,
                    Qty: $scope.QtyAdd,
                    UnitID: varUnitID,
                    UnitName: varUOMName,
                    IsDeleted: false
                };
                $scope.ListChemicalSetupDetail.push(obj);
                $scope.IsShow = $scope.ListChemicalSetupDetail.length > 0 ? true : false;
                if ($scope.ListChemicalSetupDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            }
            else {
                ShowCustomToastrMessageResult(-1);
            }
            $scope.clearDetailsParameter();
        }
        //***************************** Edit Mode ***************************************
        $scope.EditChemicalSetup = function (dataModel) {
            debugger
            $scope.ChemicalSetupID = dataModel.ChemicalSetupID;
            $scope.ddlColor = dataModel.ColorID;
            if (dataModel.ColorID != null)
                $("#DyingcolorDropdown").select2('val', dataModel.ColorID.toString());
            $scope.Qty = dataModel.Qty;
            $scope.ddlUOM = dataModel.UnitID;
            if (dataModel.UnitID != null)
                $('#masterunitDropDown').select2('val', dataModel.UnitID.toString());

            try {
                var apiRoute = baseUrl + 'GetDetailsByMasterID/';
                var processdetails = ChemicalSetupService.getDetailsByID(apiRoute, companyID, loggedUser, page, pageSize, isPaging, dataModel.ChemicalSetupID, $scope.HeaderToken.get);
                processdetails.then(function (response) {
                    $scope.clearDetails();
                    $scope.ListChemicalSetupDetail = response.data;
                    $scope.IsShow = $scope.ListChemicalSetupDetail.length > 0 ? true : false;
                    if ($scope.ListChemicalSetupDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                },
                function (error) {
                    console.log("Error: " + error);
                });

            }
            catch (e) {
            }
        }
        //**********----Delete Details ----***************
        $scope.deleteDetailsList = function (dataModel) {
            var list = $scope.ListChemicalSetupDetail;
            angular.forEach(list, function (value, key) {
                if (value.ArticleNo == dataModel.ArticleNo && value.Qty == dataModel.Qty) {
                    value.IsDeleted = true;
                }
            });

            $scope.IsShow = $scope.ListChemicalSetupDetail.length > 0 ? true : false;
            if ($scope.ListChemicalSetupDetail.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        }

        $scope.hasDetailsValue = function () {

            if ($scope.ListChemicalSetupDetail.length >= 1) {
                return false;
            }
            else {
                return true;
            }

        };

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteDyingChemPreparation = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.ChemicalSetupID;
            var apiRoute = baseUrl + 'DeleteChemicalPreparationMasterDetail/';
            ModelsArray = [objcmnParam];
            var delChemOperation = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delChemOperation.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.ColorName + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.ColorName + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.ColorName + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        //**********----Clear Details Param----***************
        $scope.clearDetailsParameter = function () {
            $scope.ddlChemical = null;
            $("#chemicalDropdown").select2('val', '--Select Chemical--');
            $scope.ddlUOMDetails = null;
            $("#detailsunitDropDown").select2('val', '--Select unit--');
            $scope.QtyAdd = 0;
            try {
                $scope.frmPrdDyingChemicalSetupAdd.$setPristine();
                $scope.frmPrdDyingChemicalSetupAdd.$setUntouched();
            } catch (e) {

            }

        };
        //**********----Clear Master Panel----***************
        $scope.clearMaster = function () {
            try {
                $scope.frmPrdDyingChemicalSetup.$setPristine();
                $scope.frmPrdDyingChemicalSetup.$setUntouched();
            } catch (e) {

            }
            $scope.ddlColor = null;
            $("#DyingcolorDropdown").select2('val', '--Select Color--');
            $scope.ddlUOM = null;
            $("#masterunitDropDown").select2('val', '--Select Unit--');
            $scope.Qty = 0;
        };
        //**********----Clear Mater Panel----***************
        $scope.clearDetails = function () {
            $scope.ListChemicalSetupDetail = [];
        };

        //************ Reset Form *******************
        $scope.clear = function () {
            $scope.clearMaster();
            $scope.clearDetailsParameter();
            $scope.clearDetails();
            //$scope.ToogleDiv = 0;
            //$scope.ShowList();
            $scope.ChemicalSetupID = 0;

            $scope.IsHidden = true;
            $scope.IsShow = false;
            //$scope.btnSaveUpdateText = "Save";
            //loadRecords_ChemicalSetupView(0);
        }

    }]);