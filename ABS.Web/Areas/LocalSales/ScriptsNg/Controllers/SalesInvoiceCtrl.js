app.controller('SalesInvoiceCtrl', ['$scope', 'SalesInvoice', '$filter', 'crudService', 'conversion', '$localStorage', 'uiGridConstants',
    function ($scope, SalesInvoice, $filter, crudService, conversion, $localStorage, uiGridConstants) {


        function loadUserCommonEntity(num) {
            var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = pagedata;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(1);
        $scope.DetailsTitle = "Sales Details";
        $scope.UserCommonEntity.currentTransactionTypeID;
        $scope.PageTitle = "";
        $scope.ArticleDetails = [];
        objcmnParam = {};
        $scope.SITypeID = 0;
        $scope.IsHidden = true;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 40) {
            $scope.PageTitle = "Local Sales Invoice";
            $scope.SITypeID = 1;
            $scope.ListTitle = "Local Sales List";
        }
        else if ($scope.UserCommonEntity.currentTransactionTypeID == 41) {
            $scope.PageTitle = "Damage Sales Invoice";
            $scope.SITypeID = 2;
            $scope.ListTitle = "Damage Sales List";
        }

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var baseUrl = '/LocalSales/api/SalesInvoice/';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        $scope.gridOptionslistItemMaster = [];
        $scope.DeleteSalesInvoiceList = [];
        $scope.IsHiddenDetail = true;
        $scope.drpPageTitle = "Warehouse List";
        $scope.SIID = 0;

        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmSalesInvoice'; DelFunc = 'delete'; DelMsg = 'SINo'; EditFunc = 'getSalesDetailsByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //****************************** Get Buyers ******************************
        $scope.LoadBuyers = function () {
            var apiRoute = baseUrl + 'GetUsers/';
            var UserTypeID = 2;
            var _Buyer = SalesInvoice.getUser(apiRoute, page, pageSize, isPaging, LoggedCompanyID, UserTypeID);
            _Buyer.then(function (response) {
                $scope.Buyers = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadBuyers();

        $scope.LoadSalesPersons = function () {
            var apiRoute = baseUrl + 'GetUsers/';
            var UserTypeID = 1;
            var _Buyer = SalesInvoice.getUser(apiRoute, page, pageSize, isPaging, LoggedCompanyID, UserTypeID);
            _Buyer.then(function (response) {
                $scope.SalesPersons = response.data;


            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadSalesPersons();


        $scope.LoadGrades = function () {
            $scope._Grades = [];
            var apiRoute = baseUrl + 'GetGrades/';
            var _Grade = SalesInvoice.getGrades(apiRoute, page, pageSize, isPaging);
            _Grade.then(function (response) {
                $scope._Grades = response.data;


            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadGrades();



        $scope.loadDepartmentRecords = function () {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                DepartmentID: 17 //$scope.UserCommonEntity.loggedUserDepartmentID
            };

            //  var apiRoute = '/Inventory/api/GRR/GetDepartmentDetails/';

            var apiRoute = baseUrl + 'GetDepartmentDetails/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            $scope.Departments = [];
            var listWherehouse = SalesInvoice.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listWherehouse.then(function (response) {
                $scope.Departments = response.data.ListDeptDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.selectNode = function (val) {

            $scope.txtbxWareHouse = val.Name;
            $scope.lblWareHouseID = val.ID;
        }

        $scope.treedubbleClick = function (val) {

            $scope.txtbxWareHouse = val.Name;
            $scope.lblWareHouseID = val.ID;
            $scope.modal_fadeOutTree();
        }
        $scope.modal_fadeOutTree = function () {
            $("#drpModalDept").fadeOut(200, function () {
                $('#drpModalDept').modal('hide');
            });
        }

        $scope.ItemGroup = function () {
            var apiRoute = baseUrl + 'GetItemGroup/';
            var ItemType = 1;
            var _ItemGroup = SalesInvoice.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID, ItemType);
            _ItemGroup.then(function (response) {
                $scope.ItemGroups = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.ItemGroup();
        $scope.GetItemTypeWithoutFinsihGood = function () {
            var apiRoute = baseUrl + 'GetItemTypeWithoutFinsihGood/';
            var ItemType = 1;
            var _ItemType = SalesInvoice.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID, ItemType);
            _ItemType.then(function (response) {
                $scope.ItemTypes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetItemTypeWithoutFinsihGood();
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
        $scope.loadSampleNoModalRecords = function (isPaging) {
            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            // else {
            // For Loading modal
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

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
                selectedCompany: $scope.UserCommonEntity.loggedCompnyID
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", title: "Article No", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction", headerCellClass: $scope.highlightFilteredHeader },//width: '19%',
                    { name: "Description", displayName: "Description", title: "Description", width: '17%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeight", displayName: "Weight", title: "Weight", cellFilter: 'number: 2', width: '7%', headerCellClass: $scope.highlightFilteredHeader },
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
                                      '<a href="" title="Select" ng-click="grid.appScope.getSalesInvoiceDetails(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Add' +
                                      '</a>' +
                                      '</span>'
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                },
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
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

            if ($scope.UserCommonEntity.currentTransactionTypeID == 40) {
                // $scope.listItemMaster = [];
                var groupID = $scope.ddlItemGroup;
                debugger
                // if (groupID > 0) {
                if (groupID == null || groupID == "" || groupID == undefined) {
                    groupID = 0;
                }
                var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
                var listItemMaster = SalesInvoice.getItemMasterByGroup(apiRoute, objcmnParam, groupID, $scope.HeaderToken.get);

                listItemMaster.then(function (response) {
                    //$scope.listItemMaster = response.data;
                    $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                    debugger
                    $scope.gridOptionslistItemMaster.data = response.data.objPIItemMaster;
                    debugger
                    $scope.loaderMoreItemMaster = false;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
                // **********************Start
            else if ($scope.UserCommonEntity.currentTransactionTypeID == 41) {
                debugger;
                var TypeID = $scope.ddlItemTypeID;
                if (TypeID == null || TypeID == "" || TypeID == undefined) {
                    TypeID = 0;
                }

                var apiRoute = baseUrl + 'GetItemMasterByTypeID/';
                var listItemMaster = SalesInvoice.getItemMasterByTypeID(apiRoute, objcmnParam, TypeID, $scope.HeaderToken.get);

                listItemMaster.then(function (response) {
                    //$scope.listItemMaster = response.data;
                    $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                    debugger
                    $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                    debugger
                    $scope.loaderMoreItemMaster = false;
                },
                function (error) {
                    console.log("Error: " + error);
                });

            }
            //*********************End

        };
        $scope.getSalesInvoiceDetails = function (dataModel) {
            $scope.IsHiddenDetail = false;
            debugger;

            //if ($scope.UserCommonEntity.currentTransactionTypeID == 40) {
            //    var apiRoute = baseUrl + 'GetArticleDetails/';
            //    var _ArticleDetails = SalesInvoice.getArticleDetails(apiRoute, page, pageSize, isPaging, LoggedCompanyID, dataModel.ItemID);
            //    _ArticleDetails.then(function (response) {

            //        var existItem = dataModel.ItemID;
            //        var duplicateItem = 0;
            //        angular.forEach($scope.ArticleDetails, function (item) {
            //            if (existItem == item.ItemID) {
            //                duplicateItem = 1;
            //                return false;
            //            }
            //        })
            //        if (duplicateItem === 0) {
            //            $scope.ArticleDetails.push(response.data);
            //            Command: toastr["info"]("Item Successfully Added.");
            //        } else if (duplicateItem === 1) {
            //                Command: toastr["warning"]("Item Already Exists!!!!");
            //        }
            //    },
            //    function (error) {
            //        console.log("Error: " + error);
            //    });
            //}
            //  else if ($scope.UserCommonEntity.currentTransactionTypeID == 41) {
            var apiRoute = baseUrl + 'GetArticleDetailsforDemageSale/';
            var _ArticleDetails = SalesInvoice.getArticleDetails(apiRoute, page, pageSize, isPaging, LoggedCompanyID, dataModel.ItemID);
            _ArticleDetails.then(function (response) {

                var existItem = dataModel.ItemID;
                var duplicateItem = 0;
                angular.forEach($scope.ArticleDetails, function (item) {
                    if (existItem == item.ItemID) {
                        duplicateItem = 1;
                        return false;
                    }
                })
                if (duplicateItem === 0) {
                    $scope.ArticleDetails.push(response.data);
                    Command: toastr["info"]("Item Successfully Added.");
                } else if (duplicateItem === 1) {
                        Command: toastr["warning"]("Item Already Exists!!!!");
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
            //}


        };

        $scope.deleteRow = function (index, item) {
            debugger;
            $scope.ArticleDetails.splice(index, 1);
            $scope.DeleteSalesInvoiceList.push({
                ItemID: item.ItemID, BatchID: item.BatchID == null ? 0 : item.BatchID, LotID: item.LotID == null ? 0 : item.LotID,
                SupplierID: item.SupplierID == null ? 0 : item.SupplierID, UnitID: item.UnitID == null ? 0 : item.UnitID,
                UnitPrice: item.UnitPrice == null ? 0 : item.UnitPrice, Qty: item.Qty == null ? 0 : item.Qty, Amount: item.Amount == null ? 0 : item.Amount,
                GradeID: item.GradeID == null ? 0 : item.GradeID, Status: 'Delete', SIDetailID: item.SIDetailID == null ? 0 : item.SIDetailID
            });
        }
        $scope.loadCurrentStock = function (dataEntity) {


            objcmnParam.CompanyID = LoggedCompanyID;
            objcmnParam.ItemID = dataEntity.ItemID;
            objcmnParam.DepartmentID = $scope.lblWareHouseID;
            objcmnParam.SupplierID = dataEntity.SupplierID === null ? 0 : dataEntity.SupplierID;
            objcmnParam.BatchID = dataEntity.BatchID === null ? 0 : dataEntity.BatchID;
            objcmnParam.GradeID = dataEntity.GradeID === null ? 0 : dataEntity.GradeID;
            objcmnParam.LotID = dataEntity.LotID === null ? 0 : dataEntity.LotID;

            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetCurrentStock/';
            var defaultStock = SalesInvoice.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            defaultStock.then(function (response) {
                debugger
                if (response.data.slCurrentStock != null) {
                    dataEntity.CurrentStock = response.data.slCurrentStock.CurrentStock;
                    dataEntity.UnitPrice = response.data.slCurrentStock.UnitPrice;

                }
                else {
                    dataEntity.CurrentStock = 0.00;
                    dataEntity.UnitPrice = 0.00;
                }
                //$scope.IsbtnSaveDisable = $scope.tempChemList.length > 0 ? true : false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.CalAmount = function (dataModel) {
            var Amount = parseFloat(dataModel.UnitPrice) * parseFloat(dataModel.Qty);
            dataModel.Amount = Amount;
        }
        //-------------
        $scope.Save = function () {
            debugger
            var SaleInvoice = {
                SIID: $scope.SIID,
                SINo: $scope.txtbxInvoice,
                SIDate: $scope.txtsalesDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.txtsalesDate),
                DODate: $scope.txtDeleveryDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.txtDeleveryDate),
                BuyerID: $scope.drpBuyer == null ? 0 : $scope.drpBuyer,
                UserID: $scope.drpSalesPerson == null ? 0 : $scope.drpSalesPerson,
                Remarks: $scope.txtDescription,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID,
                SITypeID: $scope.SITypeID
            }
            debugger

            $scope.SalesDetailsList = [];
            angular.forEach($scope.ArticleDetails, function (item) {
                $scope.SalesDetailsList.push({
                    ItemID: item.ItemID, BatchID: item.BatchID == null ? 0 : item.BatchID, LotID: item.LotID == null ? 0 : item.LotID,
                    SupplierID: item.SupplierID == null ? 0 : item.SupplierID, UnitID: item.UnitID == null ? 0 : item.UnitID,
                    UnitPrice: item.UnitPrice == null ? 0 : item.UnitPrice, Qty: item.Qty == null ? 0 : item.Qty, Amount: item.Amount == null ? 0 : item.Amount,
                    GradeID: item.GradeID == null ? 0 : item.GradeID, Status: item.Status, SIDetailID: item.SIDetailID == null ? 0 : item.SIDetailID
                });
            })

            ModelsArray = [SaleInvoice, $scope.SalesDetailsList, $scope.DeleteSalesInvoiceList];
            var apiRoute = baseUrl + 'SaveUpdateSalesInvoice/';
            var _SaveSalesInvoice = SalesInvoice.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _SaveSalesInvoice.then(function (response) {
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
            debugger
        }
        $scope.clear = function () {

        }

        $scope.paginationFg = {
            paginationPageSizesFg: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSizeFg: 15, pageNumberFg: 1, pageSizeFg: 15, totalItemsFg: 0,
            getTotalPagesFg: function () {
                return Math.ceil(this.totalItemsFg / this.pageSizeFg);
            },
            pageSizeChangeFg: function () {
                if (this.ddlpageSizeFg == "All")
                    this.pageSizeFg = $scope.paginationFg.totalItemsFg;
                else
                    this.pageSizeFg = this.ddlpageSizeFg
                this.pageNumberFg = 1
                $scope.LoadSalesInvoices(1);
            },
            firstPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg = 1
                    $scope.LoadSalesInvoices(1);
                }
            },
            nextPageFg: function () {
                if (this.pageNumberFg < this.getTotalPagesFg()) {
                    this.pageNumberFg++;
                    $scope.LoadSalesInvoices(1);
                }
            },
            previousPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg--;
                    $scope.LoadSalesInvoices(1);
                }
            },
            lastPageFg: function () {
                if (this.pageNumberFg >= 1) {
                    this.pageNumberFg = this.getTotalPagesFg();
                    $scope.LoadSalesInvoices(1);
                }
            }
        };

        $scope.LoadSalesInvoices = function (isPaging) {

            // For Loading
            if (isPaging == 0)
                $scope.paginationFg.pageNumberFg = 1;
            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessageItemMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.paginationFg.pageNumberFg - 1) * $scope.paginationFg.pageSizeFg),
                pageSize: $scope.paginationFg.pageSizeFg,
                IsPaging: isPaging,
                SITypeID: $scope.SITypeID,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,

            };


            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsFg = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "SIID", displayName: "SIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                     { name: "BuyerID", displayName: "BuyerID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserID", displayName: "UserID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SINo", displayName: "Invoice No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerName", displayName: "Buyer", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                     { name: "DODate", displayName: "Delevery Date", title: "Delevery Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SIDate", displayName: "Sales Date", title: "Delevery Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SalesPerson", displayName: "Sales Person", width: '20%', headerCellClass: $scope.highlightFilteredHeader },

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

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'FinishGoodFile.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Item Groups", style: 'headerStyle' },
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
            var apiRoute = baseUrl + 'GetSalesInvoiceDetails/';
            var _finishGoods = SalesInvoice.getSalesDetails(apiRoute, objcmnParam);
            _finishGoods.then(function (response) {
                $scope.paginationFg.totalItemsFg = response.data.recordsTotal;
                $scope.gridOptionsFg.data = response.data.SalesInvoicess;
                $scope.loaderMore = false;
                //$scope.finishGoods = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadSalesInvoices(1);
        $scope.RefreshMasterList = function () {
            $scope.paginationFg.pageNumberFg = 1;
            $scope.LoadSalesInvoices(1);
        }

        //*********************************************************Start Show/Hide************************************************************
        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
                //$scope.btnShowText = "Show List";
                ////$scope.IsShow = true;
                //$scope.IsSaveShow = true;
                //$scope.IsfrmShow = true;
                //$scope.IsfrmDetailShow = true;
                ////$scope.IsAddToListShow = true;
                //$scope.IsShow = $scope.tempChemList.length < 1 || angular.isUndefined($scope.tempChemList.length) ? false : true;
                //$scope.IsSaveShow = $scope.tempChemList.length < 1 || angular.isUndefined($scope.tempChemList.length) ? false : true;
            }
            else {
                //$scope.btnShowText = "Create";
                //$scope.cmnbtnShowHideEnDisable(1);
                $scope.RefreshMasterList();
                $scope.IsfrmShow = false;
                //$scope.IsfrmDetailShow = false;
                //$scope.IsShow = false;
                //$scope.IsSaveShow = false;
            }
        };
        //***********************************************************End Show/Hide************************************************************
        $scope.delete = function (delModel) {
            $scope.cmnParam();
            objcmnParam.id = delModel.SIID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteSalesInvoice/';
            var SetWiseMachineSetupMasterDetailDelete = SalesInvoice.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetWiseMachineSetupMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.RefreshMasterList();
                    Command: toastr["success"]("Data has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"]("Data Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }
        $scope.getSalesDetailsByID = function (dataModel) {
            $scope.SIID = dataModel.SIID;
            $scope.txtbxInvoice = dataModel.SINo;
            $scope.txtDescription = dataModel.Remarks;
            $scope.txtsalesDate = dataModel.SIDate == "" || dataModel.SIDate == null ? "" : conversion.getDateToString(dataModel.SIDate);
            $scope.txtDeleveryDate = dataModel.DODate == "" || dataModel.DODate == null ? "" : conversion.getDateToString(dataModel.DODate);

            $("#drpBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerName });
            $("#drpSalesPerson").select2("data", { id: dataModel.UserID, text: dataModel.SalesPerson });
            $scope.LoadSalesListForUpdate(dataModel);
        }
        $scope.LoadSalesListForUpdate = function (dataModel) {
            $scope.ArticleDetails = [];
            $scope.IsHiddenDetail = false;
            objcmnParam = {
                SITypeID: $scope.SITypeID,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                id: dataModel.SIID
            };

            var apiRoute = baseUrl + 'LoadSalesListForUpdate/';
            var _finishGoods = SalesInvoice.getSalesDetails(apiRoute, objcmnParam);
            _finishGoods.then(function (response) {
                debugger;
                $scope.ArticleDetails = response.data.ArticleDetails;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }]);