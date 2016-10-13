/*
  FebricInspectionCtrl.js
 */
app.controller('QualityAssuaranceCtrl', ['$scope', 'crudService', 'conversion', '$localStorage', 'uiGridConstants','PublicService',
    function ($scope, crudService, conversion, $localStorage, uiGridConstants, PublicService) {
        var baseUrl = '/Production/api/QA/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];

        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        $scope.ItemID = 0;
        $scope.IsHidden = true;
        $scope.IsShow = false;

        $scope.PageTitle = 'Quality Assuarance Entry';
        $scope.ListTitleDetail = 'Quality Assuarance Entry';
        $scope.ListTitle = 'Quality Assuarance List';
        $scope.QualityAssuaranceDetailList = [];
        $scope.QADate = conversion.NowDateCustom();
        $scope.FromDate = conversion.NowDateCustom();
        $scope.ToDate = conversion.NowDateCustom();

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateQAMasterDetail'; DelMsg = 'QANo'; EditFunc = 'getQAMasterDetailByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Article Dropdown******************************************************
        $scope.loadArticleRecords = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetArticle/';
            var ListArticle = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListArticle.then(function (response) {
                $scope.ListArticle = response.data.ListArticle;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadArticleRecords(0);
        //**************************************************End Article Dropdown******************************************************  

        //************************************************Start Grade Dropdown******************************************************
        $scope.loadGradeRecords = function (dataModel) {
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetAllGrade/';
            var ListGrade = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListGrade.then(function (response) {
                $scope.ListGrade = response.data.ListGrade;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadGradeRecords(0);
        //**************************************************End Grade Dropdown******************************************************  

        //*******************************************************Start Add Detail**************************************************
        $scope.AddItemToList = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel == 0 ? $scope.ItemID : 0;
            objcmnParam.ItemType = dataModel == 0 ? 0 : dataModel;
            objcmnParam.FromDate = $scope.txtbxFormDate === "" ? null : conversion.getStringToDate($scope.FromDate);
            objcmnParam.ToDate = $scope.txtbxToDate === "" ? null : conversion.getStringToDate($scope.ToDate);
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetInspactionDetailsByIDAndDates/';
            var _artical = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _artical.then(function (response) {
                $scope.InspactionDetails = response.data;
                $scope.IsShow = $scope.InspactionDetails.length > 0 ? true : false;
                if ($scope.InspactionDetails.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //********************************************************End Add Detail***************************************************

        //*******************************************************Start List Master**************************************************
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
                $scope.loadQAMasterList(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadQAMasterList(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadQAMasterList(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadQAMasterList(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadQAMasterList(1);
                }
            }
        };

        $scope.loadQAMasterList = function (isPaging) {
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
                    { name: "QAID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QANo", displayName: "QA No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Item Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QADate", displayName: "QA Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FromDate", displayName: "From Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ToDate", displayName: "To Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", displayName: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
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
            var apiRoute = baseUrl + 'GetQAMasterList/';
            var listQAMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listQAMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptions.data = response.data.ListQAMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadQAMasterList(0);
        }
        $scope.RefreshMasterList();
        //********************************************************End List Master***************************************************

        //*******************************************************Start Edit By ID**************************************************
        $scope.getQAMasterDetailByID = function (dataModel) {
            $scope.QAID = dataModel.QAID;
            $scope.ItemID = dataModel.ItemID;
            $scope.ArticleNo = dataModel.ItemName;
            //$("#ArticleList").select2("data", { id: dataModel.ItemID, text: dataModel.ItemName });
            $scope.QADate = dataModel.QADate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.QADate);
            $scope.FromDate = dataModel.FromDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.FromDate);
            $scope.ToDate = dataModel.ToDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.ToDate);
            $scope.Remarks = dataModel.Remarks;

            $scope.AddItemToList(dataModel.QAID);
        }
        //********************************************************End Edit By ID***************************************************

        //*******************************************************Start ShowHide Master List**************************************************
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
        //********************************************************End ShowHide Master List***************************************************

        //*******************************************************Start Save Master Detail**************************************************
        $scope.Save = function () {
            $scope.cmnParam();
            var QAMaster = {
                QAID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.QAID,
                ItemID: $scope.ItemID,
                QADate: $scope.QADate == "" ? "1/1/1900" : conversion.getStringToDate($scope.QADate),
                FromDate: $scope.FromDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.FromDate),
                ToDate: $scope.ToDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.ToDate),
                Remarks: $scope.Remarks
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [QAMaster, $scope.InspactionDetails, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateQAMasterDetail/';
            var QAMasterDetailSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            QAMasterDetailSaveUpdate.then(function (response) {
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
        //********************************************************End Save Master Detail***************************************************

        //*******************************************************Start Delete Master Detail**************************************************
        $scope.DeleteUpdateQAMasterDetail = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.QAID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateQAMasterDetail/';
            var delDefectList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDefectList.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.QANo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.QANo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.QANo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        //*******************************************************Start Reset**************************************************
        $scope.clear = function () {
            $scope.frmQualityAssuarance.$setPristine();
            $scope.frmQualityAssuarance.$setUntouched();
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.QAID = "";
            $scope.Remarks = "";
            $scope.ItemID = "";
            //$("#ArticleList").select2("data", { id: 0, text: "--Select Article No--" });
            $scope.QADate = conversion.NowDateCustom();
            $scope.FromDate = conversion.NowDateCustom();
            $scope.ToDate = conversion.NowDateCustom();
            $scope.InspactionDetails = [];
        };
        //********************************************************End Reset***************************************************
        //********************************************** Item Modal Code *****************************************
        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;
        $scope.getListItemMaster = function (model) {
            var ItemID = model.ItemID;
            var ItemName = model.ItemName;
            $scope.ItemID = model.ItemID;
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