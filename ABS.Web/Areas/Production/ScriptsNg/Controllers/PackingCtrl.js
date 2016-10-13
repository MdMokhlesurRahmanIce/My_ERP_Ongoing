/*
  PackingCtrl.js
 */
app.controller('PackingCtrl', ['$scope', 'crudService', 'conversion', '$localStorage', 'uiGridConstants','PublicService',
    function ($scope, crudService, conversion, $localStorage, uiGridConstants, PublicService) {
        var baseUrl = '/Production/api/PackingList/';
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

        $scope.PageTitle = 'Packing List Master Entry';
        $scope.ListTitleDetail = 'Packing List Detail Entry';
        $scope.ListTitle = 'Packing List';
        $scope.PackingDetailList = [];
        $scope.PackingDate = conversion.NowDateCustom();
        FinishingMRRNo = "";

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdatePackingMasterDetail'; DelMsg = 'PackingNo'; EditFunc = 'getPackingMasterDetailByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //************************************************Start Article Dropdown******************************************************
        $scope.loadPIRecords = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.IsTrue = true;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPINO/';
            var ListPINO = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListPINO.then(function (response) {
                $scope.ListPINo = response.data.ListPINo;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadPIRecords(0);
        //**************************************************End Article Dropdown******************************************************

        //************************************************Start Article Dropdown******************************************************
        $scope.loadPIBasedData = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = $scope.PIID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPIBasedData/';
            var ListPIData = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListPIData.then(function (response) {
                $scope.CINO = response.data.PIData.CINO;
                $scope.BuyerName = response.data.PIData.BuyerName;
                $scope.LCNo = response.data.PIData.LCNo;
                $scope.ExportLCNo = response.data.PIData.ExportLCNo;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //**************************************************End Article Dropdown******************************************************

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

        //************************************************Start Style Dropdown******************************************************
        $scope.loadStyleNo = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetStyleNo/';
            var listStyles = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listStyles.then(function (response) {
                $scope.Styles = response.data.Styles;
                if (FinishingMRRNo != "") {
                    $("#drpStyleNO").select2("data", { id: 0, text: FinishingMRRNo });
                    FinishingMRRNo = "";
                }
                else {
                    $("#drpStyleNO").select2("data", { id: 0, text: "-- Select Style No --" });
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //$scope.loadStyleNo(0);
        //**************************************************End Style Dropdown******************************************************

        //*******************************************************Start Add Detail**************************************************
        $scope.AddItemToList = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel == 0 ? $scope.FinishingMRRID : 0;
            objcmnParam.ItemType = dataModel == 0 ? 0 : dataModel;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPackingListDetailByID/';
            var _artical = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _artical.then(function (response) {
                $scope.PackingDetailList = response.data.PDetail;
                $scope.IsShow = $scope.PackingDetailList.length > 0 ? true : false;
                if ($scope.PackingDetailList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
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
                $scope.loadPackingMasterList(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadPackingMasterList(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadPackingMasterList(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadPackingMasterList(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadPackingMasterList(1);
                }
            }
        };

        $scope.loadPackingMasterList = function (isPaging) {
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
                    { name: "PackingID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CINO", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PackingNo", displayName: "Packing No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PINO", displayName: "PI No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRNo", displayName: "Finishing MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerName", displayName: "Buyer", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCNo", displayName: "LC No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ExportLCNo", visible: false, displayName: "Export LC No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PackingDate", displayName: "Packing Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
            var apiRoute = baseUrl + 'GetPackingMasterList/';
            var listPackingMaster = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listPackingMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptions.data = response.data.ListPMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadPackingMasterList(0);
        }
        $scope.RefreshMasterList();
        //********************************************************End List Master***************************************************

        //*******************************************************Start Edit By ID**************************************************
        $scope.getPackingMasterDetailByID = function (dataModel) {
            $scope.PackingID = dataModel.PackingID;
            $scope.PackingNo = dataModel.PackingNo;
            $scope.ItemID = dataModel.ItemID;
            $scope.ArticleNo = dataModel.ArticleNo;
            //$("#ArticleList").select2("data", { id: dataModel.ItemID, text: dataModel.ArticleNo });
            $scope.PIID = dataModel.PIID;
            $("#PIList").select2("data", { id: dataModel.PIID, text: dataModel.PINO });

            $scope.FinishingMRRID = dataModel.FinishingMRRID;
            FinishingMRRNo = dataModel.FinishingMRRNo;

            $scope.CINO = dataModel.CINO;
            $scope.BuyerName = dataModel.BuyerName;
            $scope.ExportLCNo = dataModel.ExportLCNo;
            $scope.LCNo = dataModel.LCNo;
            
            $scope.PackingDate = dataModel.PackingDate == "1900-01-01T00:00:00" ? "" : conversion.getDateToString(dataModel.PackingDate);
            $scope.Remarks = dataModel.Remarks;

            $scope.loadStyleNo(dataModel.ItemID);

            $scope.AddItemToList(dataModel.PackingID);
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
            var PackingMaster = {
                PackingID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.PackingID,
                PIID: $scope.PIID,
                FinishingMRRID: $scope.FinishingMRRID,
                ItemID: $scope.ItemID,
                Remarks: $scope.Remarks,
                PackingDate: $scope.PackingDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.PackingDate)                
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [PackingMaster, $scope.PackingDetailList, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdatePackingListMasterDetail/';
            var PackingMasterDetailSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            PackingMasterDetailSaveUpdate.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    $scope.PackingNo = response;
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
        $scope.DeleteUpdatePackingMasterDetail = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.PackingID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdatePackingMasterDetail/';
            var delPackingList = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delPackingList.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.PackingNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.PackingNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.PackingNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        //********************************************************End Delete Master Detail***************************************************

        //*******************************************************Start Reset**************************************************
        $scope.clear = function () {
            $scope.frmPackingList.$setPristine();
            $scope.frmPackingList.$setUntouched();
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $scope.PackingNo = "";
            $scope.CINO = "";
            $scope.BuyerName = "";
            $scope.LCNo = "";
            $scope.ExportLCNo = "";
            $scope.PIID = "";
            $("#PIList").select2("data", { id: 0, text: "--Select PI No--" });
            $scope.ItemID = "";
            $("#ArticleList").select2("data", { id: 0, text: "--Select Article No--" });
            $scope.FinishingMRRID = "";
            $("#drpStyleNO").select2("data", { id: 0, text: "--Select Style No--" });
            $scope.Remarks = "";
            $scope.PackingDate = conversion.NowDateCustom();
            $scope.PackingDetailList = [];
            $scope.gridOptions.data = [];
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
            $scope.loadStyleNo(ItemID);

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