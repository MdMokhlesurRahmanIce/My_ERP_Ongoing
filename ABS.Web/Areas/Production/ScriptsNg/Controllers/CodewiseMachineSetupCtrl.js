app.controller('CodewiseMachineSetupCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants','PublicService',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants, PublicService) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/Production/api/CodewiseMachineSetup/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsCodewiseMachineSetup = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.lstArticleList = 0;
        $scope.listCodewiseMachineSetupMaster = [];
        //$scope.btnSaveText = "Save";
        //$scope.btnShowList = "Show List";
        $scope.PageTitle = 'Codewise Machine Setup Creation';
        $scope.ListTitle = 'Codewise Machine Setup Information';
        $scope.ListTitleCodewiseMachineSetupMaster = 'Codewise Machine Setup List';
        $scope.IsHidden = true;
        $scope.IsShow = true;
        //$scope.IsShowSave = true;
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmCodewiseMachineSetup'; DelFunc = 'DeleteUpdateWeavingMachineSetup'; DelMsg = 'Selvedge'; EditFunc = 'getMachineSetupInfo';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Article Dropdown******************************************************
        $scope.loadArticleRecords = function (isPaging) {
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetArticle/';
            ModelsArray = [objcmnParam];
            var ListArticle = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListArticle.then(function (response) {
                $scope.ListArticle = response.data.ListArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadArticleRecords(1);
        //**************************************************End Article Dropdown******************************************************

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
                $scope.loadAllCodewiseMachineSetupMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAllCodewiseMachineSetupMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAllCodewiseMachineSetupMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAllCodewiseMachineSetupMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAllCodewiseMachineSetupMasterRecords(1);
                }
            }
        };

        $scope.loadAllCodewiseMachineSetupMasterRecords = function (isPaging) {
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
            $scope.gridOptionsCodewiseMachineSetup = {
                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "MachineSetupID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Selvedge", displayName: "Selvedge", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Brackrest", displayName: "Brackrest", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShadeAngle", displayName: "ShadeAngle", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SFHight", displayName: "SFHight", headerCellClass: $scope.highlightFilteredHeader },
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
            var apiRoute = baseUrl + 'GetMachineSetupList/';
            var listCodewiseMachineSetupMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listCodewiseMachineSetupMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                debugger
                $scope.gridOptionsCodewiseMachineSetup.data = response.data.masterDataList;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAllCodewiseMachineSetupMasterRecords(0);
        }
        $scope.RefreshMasterList();

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowList = "Show List";                
                //$scope.IsShow = true;
                //$scope.IsShowSave = true;
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
                //$scope.IsShowSave = false;
            }
        }

        $scope.getMachineSetupInfo = function (dataModel) {
            $scope.ListLCInfoMaster = [];
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            //$scope.btnShowList = "Show List";
            //$scope.btnSaveText = "Update";

            $scope.MachineSetupID = dataModel.MachineSetupID;
            $scope.Selvedge = dataModel.Selvedge;
            $scope.Brackrest = dataModel.Brackrest;
            $scope.ShadeAngle = dataModel.ShadeAngle;
            $scope.SFHight = dataModel.SFHight;
            $scope.lstArticleList = dataModel.ItemID;

            $scope.cmnParam();
            objcmnParam.id = dataModel.ItemID;
            ModelsArray = [objcmnParam];
            var apiRouteMaster = baseUrl + 'GetArticle/';
            var ListLCInfoMaster = crudService.postMultipleModel(apiRouteMaster, ModelsArray, $scope.HeaderToken.get);
            ListLCInfoMaster.then(function (response) {
                // $scope.ListLCInfoMaster = response.data;
                //$scope.MachineSetupID = response.data.objMachineSetupInfo[0].MachineSetupID;
                //$scope.Selvedge = response.data.objMachineSetupInfo[0].Selvedge;
                //$scope.Brackrest = response.data.objMachineSetupInfo[0].Brackrest;
                //$scope.ShadeAngle = response.data.objMachineSetupInfo[0].ShadeAngle;
                //$scope.SFHight = response.data.objMachineSetupInfo[0].SFHight;
                //$scope.lstArticleList = response.data.ListArticle[0].ItemID;
                $scope.lstArticleList = response.data.ListArticle[0].ItemID;
                $scope.ArticleNo = response.data.ListArticle[0].ArticleNo;
                //$("#ArticleList").select2("data", { id: response.data.ListArticle[0].ItemID, text: response.data.ListArticle[0].ArticleNo });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.Save = function () {
            //message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();

            var CodewiseMachineSetupInfo = {
                MachineSetupID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.MachineSetupID,
                Selvedge: $scope.Selvedge,
                ItemID: $scope.lstArticleList,
                Brackrest: $scope.Brackrest,
                ShadeAngle: $scope.ShadeAngle,
                SFHight: $scope.SFHight
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [CodewiseMachineSetupInfo, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateCodewiseMachineSetup/';
            var CodewiseMachineSetupSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            CodewiseMachineSetupSaveUpdate.then(function (response) {
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

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteUpdateWeavingMachineSetup = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.MachineSetupID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateWeavingMachineSetup/';
            var delDefectList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDefectList.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"](dataModel.Selvedge + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.Selvedge + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.Selvedge + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.frmCodewiseMachineSetup.$setPristine();
            $scope.frmCodewiseMachineSetup.$setUntouched();
            $scope.IsShow = true;
            //$scope.IsShowSave = true;
            $scope.IsHidden = true;
            $scope.lstArticleList = "";
            $scope.ArticleNo = "";
            //$("#ArticleList").select2("data", { id: 0, text: "--Select Article No--" });
            $scope.MachineSetupID = 0;
            $scope.Selvedge = '';
            $scope.Brackrest = '';
            $scope.ShadeAngle = '';
            $scope.SFHight = '';
            //$scope.btnSaveText = "Save";
        };

        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;
        $scope.getListItemMaster = function (model) {
            var ItemID = model.ItemID;
            var ItemName = model.ItemName;
            $scope.lstArticleList = model.ItemID;
            $scope.ArticleNo = ItemName; 

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


            var apiRoute = '/SystemCommon/api/PublicApi/' + 'GetFinishedItemMaster/';
            var listItemMaster = PublicService.getItemMasterService(apiRoute, objcmnParam);
            listItemMaster.then(function (response) {
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

        };
    }]);
