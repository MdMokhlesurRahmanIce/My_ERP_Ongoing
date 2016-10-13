/*
*    Created By: Shamim Uddin;
*    Create Date: 2-6-2016 (dd-mm-yy); Updated Date: 2-6-2016 (dd-mm-yy);
*    Name: 'FabricDetailController';
*    Type: $scope;
*    Purpose: Finish Good  For RND;
*    Service Injected: '$scope', 'FinishGoodSerivce','uiGridConstants','filter';
*/

//app.controller('FinishGoodController', function ($scope, FinishGoodSerivce) {
app.controller('FinishGoodController', ['$scope', 'FinishGoodSerivce', '$filter', 'crudService', 'conversion', 'uiGridConstants',
    function ($scope, FinishGoodSerivce, $filter, crudService, conversion, uiGridConstants) {
        var baseUrl = '/Production/api/FinishGood/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsFg = [];

        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.loaderMore = false;
        $scope.ItemID = 0;
        var ItemTypeID = 1; // Finish Good
        //$scope.btnFinisFoodShow = "Save";
        //$scope.btnRawMaterialShowText = "New";
        $scope.PageTitle = 'Fabric Development Info';
        $scope.ListTitle = 'Fabric Development List';
        $scope.btnSaveWarpYan = 'Save';
        $scope.btnMWYAdd = 'Add';
        $scope.btnMWrpYarnReset = 'Reset';
        $scope.ModalHeading = 'Count Setting';
        $scope.ListWarpYarn = [];
        $scope.AcDetailID = 0;
        var countType = "";
        //$scope.grdShowHide = true;
        //$scope.MainForm = false;
        $scope.IsHidden = true;
        $scope.drpWarpYarnCount = 0;
        $scope.drpWeftCount = 0;
        //$scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        //console.log($scope.UserCommonEntity);
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmFinishGood'; DelFunc = 'delete'; DelMsg = 'ArticleNo'; EditFunc = 'getFinishGoodByItemId';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //$scope.ShowHide = function () {
        //    $scope.clear();
        //    if ($scope.btnRawMaterialShowText === "New") {
        //        $scope.btnRawMaterialShowText = "Show List";
        //        $scope.grdShowHide = false;
        //        $scope.MainForm = true;
        //    }
        //    else {
        //        $scope.btnRawMaterialShowText = "New";
        //        $scope.grdShowHide = true;
        //        $scope.MainForm = false;
        //    }
        //}

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
            }
        }

        $scope.GetItemGroups = function () {
            var apiRoute = baseUrl + 'GetItemGroups/';
            var itemGroupes = FinishGoodSerivce.getAllItemGroup(apiRoute, page, pageSize, isPaging, ItemTypeID, LoggedCompanyID);
            itemGroupes.then(function (response) {
                $scope.itemGroupes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetItemGroups();
        //Pagination
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
                $scope.LoadFinishGoods(1);
            },
            firstPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg = 1
                    $scope.LoadFinishGoods(1);
                }
            },
            nextPageFg: function () {
                if (this.pageNumberFg < this.getTotalPagesFg()) {
                    this.pageNumberFg++;
                    $scope.LoadFinishGoods(1);
                }
            },
            previousPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg--;
                    $scope.LoadFinishGoods(1);
                }
            },
            lastPageFg: function () {
                if (this.pageNumberFg >= 1) {
                    this.pageNumberFg = this.getTotalPagesFg();
                    $scope.LoadFinishGoods(1);
                }
            }
        };

        //ui-Grid Call
        $scope.LoadFinishGoods = function (isPaging) {
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = $scope.paginationFg.pageNumberFg;
            objcmnParam.pageSize = $scope.paginationFg.pageSizeFg;
            objcmnParam.IsPaging = isPaging;
            objcmnParam.ItemType = ItemTypeID;
            //objcmnParam = {
            //    pageNumber: $scope.paginationFg.pageNumberFg,
            //    pageSize: $scope.paginationFg.pageSizeFg,
            //    IsPaging: isPaging,
            //    loggeduser: LoggedUserID,
            //    loggedCompany: LoggedCompanyID,
            //    menuId: 5,
            //    tTypeId: 26,
            //    ItemType: ItemTypeID
            //};

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
                    { name: "ItemID", displayName: "Item ID", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "PDL Ref", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeigth", displayName: "Fini Wt", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WarpYarnCount", displayName: "Warp Y.Count", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WarpYarnRatio", displayName: "Warp SL Lot", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WarpYarnRatioLot", displayName: "Warp SL lot", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftYarnCount", displayName: "Weft Y.Count", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftpYarnRatio", displayName: "Weft Ratio", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftYarnRatioLot", displayName: "Weft SL LOT", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftsupplierFullName", displayName: "Weft Supplier", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GerigeEPIxPPI", displayName: "Greige EPI*PPI", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Color", displayName: "Color", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FlangeNo", displayName: "Flange No.", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "TotalEnds", displayName: "Total Ends", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Length", displayName: "Length yds", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerFullName", displayName: "Buyer", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BuyerRefFullName", displayName: "Buyer ref", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Note", displayName: "Note", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remark", displayName: "Remark", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
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
            var apiRoute = baseUrl + 'GetFinishGoods/';
            var _finishGoods = FinishGoodSerivce.getAllFinishGood(apiRoute, objcmnParam);
            _finishGoods.then(function (response) {
                $scope.paginationFg.totalItemsFg = response.data.recordsTotal;
                $scope.gridOptionsFg.data = response.data.objFinishGoods;
                $scope.loaderMore = false;
                //$scope.finishGoods = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.paginationFg.pageNumberFg = 1;
            $scope.LoadFinishGoods(0);
        }
        $scope.RefreshMasterList();

        $scope.LoadYarns = function () {
            var apiRoute = baseUrl + 'GetYarns/';
            var ItemYrn = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            ItemYrn.then(function (response) {
                $scope.ItemYrns = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.LoadLots = function () {
            $("#drMpLot").select2("data", { id: 0, text: '--Select Lot--' });
            $scope.drMpLot = null;
            $scope.Lots = [];
            var apiRoute = baseUrl + 'GetLotsForYarn/' + $scope.drpMYarn;
            var _Lots = FinishGoodSerivce.getLoadByYarnID(apiRoute);
            _Lots.then(function (response) {
                if (response.data.length > 0) {
                    $scope.Lots = response.data;
                }
                else {
                    $scope.Lots = null;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //$scope.LoadLots();

        //$scope.finishWeights = function () {
        //    var apiRoute = baseUrl + 'GetFinishWeights/';
        //    var finishWeights = FinishGoodSerivce.getFiniWeight(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
        //    finishWeights.then(function (response) {
        //        $scope.finishWeights = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //$scope.finishWeights();

        $scope.LoadYarns();
        $scope.AddWarpYarn = function () {
            var IsDuplicate = false;
            var list = $scope.ListWarpYarn;
            //angular.forEach(list, function (value, key) {
            //    if (value.Yarn == $scope.drpMYarn) {
            //        IsDuplicate = true;
            //    }
            //});
            //if (!IsDuplicate) {

            obj = {
                CompnayID: LoggedCompanyID,
                LoginID: LoggedCompanyID,
                Yarn: $scope.drpMYarn,
                YarnName: $("#drpMYarn option:selected").text(),
                LotName: $("#drMpLot option:selected").text(),
                LotID: $scope.drMpLot,
                Ratio: $scope.txtMRation,
                YarnType: countType

            }
            $scope.ListWarpYarn.push(obj);
            //} else {
            //    ShowCustomToastrMessageResult(-1);
            //}
            // $scope.ClearModal();
        }
        $scope.deleteWarpYarn = function (dataModel) {
            $scope.ListWarpYarn.splice($scope.ListWarpYarn.indexOf(dataModel), 1);
            if ($scope.ListWarpYarn.length <= 0) {
                $scope.ClearModal();
            }
        }
        $scope.saveWarpYarn = function () {
            
            if ($scope.ListWarpYarn.length > 0) {
               
                var apiRoute = baseUrl + 'SaveYarn/';
                var porcessMasterDetails = FinishGoodSerivce.postList(apiRoute, $scope.ListWarpYarn);
                porcessMasterDetails.then(function (response) {
                    var responses = response;
                    response = {};
                    response.data = responses;
                    if (response.data != null) {
                        var yarnId = response.data;
                        $scope.LoadWarpcounts();
                        response.data = 1;
                        ShowCustomToastrMessage(response);
                        if (yarnId != null || yarnId != 0) {
                            $scope.LoadRatioAndLot(yarnId);
                        }
                        modal_fadeOut();
                        $scope.ListWarpYarn = [];
                    }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                Command: toastr["warning"]("There are no count items");
                return;

            }
        }
        $scope.LoadWarpYarnByYarnID = function () {

            $scope.LoadRatioAndLot($scope.drpWarpYarnCount);
            $scope.drpWarpYarnCount = 0;
        }
        $scope.LoadWeftYarnByYarnID = function () {

            $scope.LoadRatioAndLot($scope.drpWeftCount);
            $scope.drpWeftCount = 0;
        }
        $scope.LoadRatioAndLot = function (yarnId) {
            // getYarnByID          
            var apiRoute = baseUrl + 'GetYarnBYId/';
            var finishGood = FinishGoodSerivce.getYarnByID(apiRoute, yarnId, LoggedCompanyID);
            finishGood.then(function (response) {
                if (response.data.YarnType == "Warp") {
                    debugger
                    // $scope.LoadWarpcounts();
                    $scope.txtbxWarpLot = response.data.LotName;
                    $scope.txtbxWarpRatio = response.data.YarnRatio;
                    $scope.drpWarpYarnCount = response.data.Yarn;
                    $scope.drpWarpYarnCountName = response.data.YarnName;
                    //$("#drpWarpYarnCount").select2("data", { id: response.data.Yarn, text: response.data.YarnName });

                } else if (response.data.YarnType == "Weft") {
                    debugger
                    //  $scope.LoadWeftcounts(); 
                    $scope.txtbxWeftLost = response.data.LotName;
                    $scope.txtbxWeftRatio = response.data.YarnRatio;
                    $scope.drpWeftCount = response.data.Yarn;
                    $scope.drpWeftCountName = response.data.YarnName;
                    //$("#drpWeftCount").select2("data", { id: response.data.Yarn, text: response.data.YarnName });


                }
            }, function (error) {
                console.log("Error: " + error);
            });
        };
        $scope.LoadWarpcounts = function () {
            var apiRoute = baseUrl + 'GeWarps/';
            var _Warps = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _Warps.then(function (response) {
                $scope.Warps = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadWarpcounts();
        $scope.LoadWeftcounts = function () {
            var apiRoute = baseUrl + 'GetWefts/';
            var _Wefts = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _Wefts.then(function (response) {
                $scope.Wefts = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadWeftcounts();
        $scope.LoadBuyerRefs = function () {
            var apiRoute = baseUrl + 'GetBuyerReffs/';
            var _BuyerRef = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _BuyerRef.then(function (response) {
                $scope.BuyerRefs = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadBuyerRefs();
        $scope.LoadBuyers = function () {
            var apiRoute = baseUrl + 'GetBuyers/';
            var _Buyer = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _Buyer.then(function (response) {
                $scope.Buyers = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadBuyers();
        $scope.LoadSuppliers = function () {
            var apiRoute = baseUrl + 'GetSuppliers/';
            var _Supplier = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _Supplier.then(function (response) {
                $scope.Suppliers = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.LoadSuppliers();
        $scope.GetColors = function () {
            debugger;
            var apiRoute = baseUrl + 'GetColors/';
            var Colors = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Colors.then(function (response) {
                $scope.Colors = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetColors();
        $scope.GetWeftColors = function () {
            debugger;
            var apiRoute = baseUrl + 'GetColors/';
            var weftColors = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            weftColors.then(function (response) {
                $scope.weftColors = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetWeftColors();

        $scope.GetCoating = function () {
            debugger;
            var CoatingID = 1;
            var apiRoute = baseUrl + 'GetCoatingByTypeID/';
            var getCoating = FinishGoodSerivce.getAllCoatingByID(apiRoute, page, pageSize, isPaging, LoggedCompanyID, CoatingID);
            getCoating.then(function (response) {
                $scope.Coats = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetCoating();

        $scope.GetSpecialCoating = function () {
            debugger;
            var CoatingID = 2;
            var apiRoute = baseUrl + 'GetCoatingByTypeID/';
            var getSpecialCoating = FinishGoodSerivce.getAllCoatingByID(apiRoute, page, pageSize, isPaging, LoggedCompanyID, CoatingID);
            getSpecialCoating.then(function (response) {
                $scope.SpeCoats = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetSpecialCoating();

        $scope.GetOverDyed = function () {
            debugger;
            var apiRoute = baseUrl + 'GetOverdyed/';
            var OverDyeds = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            OverDyeds.then(function (response) {
                $scope.OverDyeds = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetOverDyed();

        $scope.GetFinishProcess = function () {
            var apiRoute = baseUrl + 'GetFinishProcess/';
            var _finishProcesses = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _finishProcesses.then(function (response) {
                $scope.ListMultiProcessModel = [];
                $scope.finishProcesses = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetFinishProcess();
        $scope.LoadWarpCountList = function () {
            debugger
            if ($scope.drpWarpYarnCount != 0) {
                Command: toastr["warning"]("Warp Count has been Selected");
                return;
            }
            else {
                $("#WarpYarnSetUpModal").fadeIn(200, function () { $('#WarpYarnSetUpModal').modal('show'); });
                $scope.ListWarpYarn = [];
                countType = "Warp";
                $scope.ClearAllModal();

            }
        }
        $scope.LoadWeftCountList = function () {
            debugger
            if ($scope.drpWeftCount != 0) {

                Command: toastr["warning"]("Weft Count has been Selected");
            }
            else {
                $("#WarpYarnSetUpModal").fadeIn(200, function () { $('#WarpYarnSetUpModal').modal('show'); });
                $scope.ListWarpYarn = [];
                countType = "Weft";
                $scope.ClearAllModal();
            }
        }

        $scope.ClearAllModal = function () {
            debugger;
            $scope.frmMWY.$setPristine();
            $scope.frmMWY.$setUntouched();
            $("#drpMYarn").select2("val", '');
            $("#drMpLot").select2("val", '');
            $scope.txtMRation = '';
            $scope.ListWarpYarn = [];
            //$("#drpMYarn").select2("val", '');
            //$scope.drpMYarn = 0;
            //$("#drMpLot").select2("val", '');
            //$scope.drMpLot = 0;            
            //$scope.txtMRation = '';
            //countType = "";           
            //$scope.frmMWY.$setPristine();
        }

        $scope.Save = function () {

            var FinishGood = {
                ItemTypeID: ItemTypeID,
                ItemID: $scope.ItemID,
                ArticleNo: $scope.txtArticaleNo,
                WeightPerUnit: $scope.txtbxWeightDS,
                FinishingWeight: $scope.txtFiniWeight,
                ItemGroupID: ($scope.drpItemGroup == 0 ? null : $scope.drpItemGroup),
                ItemName: $scope.txtArticaleNo,
                WarpYarnID: ($scope.drpWarpYarnCount == 0 ? null : $scope.drpWarpYarnCount),
                BuyerID: ($scope.drpBuyerName == 0 ? null : $scope.drpBuyerName),
                WeftYarnID: ($scope.drpWeftCount == 0 ? null : $scope.drpWeftCount),
                WeftSupplierID: ($scope.drpWeftSupplier == 0 ? null : $scope.drpWeftSupplier),
                GPPI: $scope.txtbxGPPI,
                ItemColorID: ($scope.drpColor == 0 ? null : $scope.drpColor),
                GEPI: $scope.txtbxGEPI,
                SetNo: $scope.txtbxSetNo,
                FlangeNo: $scope.txtbxFlangeNo,
                Weave: $scope.txtbxWeave,
                FinishingTypeID: ($scope.drpdwnFinishProcess == 0 ? null : $scope.drpdwnFinishProcess),
                Length: $scope.txtbxLength,
                TotalEnds: $scope.txtbxTotalEnds,
                Description: $scope.txtbxDescripion,
                BuyerRefID: ($scope.drpBuyerRef == 0 ? null : $scope.drpBuyerRef),
                Note: $scope.txbxNote,
                CuttableWidth: $scope.txtbxCutablewidth,
                HSCODE: $scope.txtbxHSCODE,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID,
                IsDevelopmentComplete: 0,
                AcDetailID: $scope.AcDetailID,
                // -----Last Update-----------
                CoatingID: ($scope.drpCoating == 0 ? null : $scope.drpCoating),
                SPCoatingID: ($scope.drpSpeCoating == 0 ? null : $scope.drpSpeCoating),
                OverDyedID: ($scope.drpOverDyed == 0 ? null : $scope.drpOverDyed),
                WeftColorID: ($scope.drpWeftColor == 0 ? null : $scope.drpWeftColor),

            }
            isExisting = $scope.ItemID;
            if (isExisting == 0) {

                var apiRoute = baseUrl + 'SaveFinishGood/';
                var SaveFinishGood = FinishGoodSerivce.postList(apiRoute, FinishGood, $scope.ListMultiProcessModel);
                SaveFinishGood.then(function (response) {
                    var responses = response;
                    response = {};
                    response.data = responses;
                    //$scope.LoadFinishGoods();
                    $scope.clear();
                    //$scope.txtArticaleNo = response.data;
                    response.data = 1;
                    ShowCustomToastrMessage(response);
                    //$scope.btnRawMaterialShowText = "Show List";
                    //$scope.ShowHide();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                var apiRoute = baseUrl + 'UpdateFinishGood/';
                var FinishGood = FinishGoodSerivce.putList(apiRoute, FinishGood, $scope.ListMultiProcessModel);
                FinishGood.then(function (response) {
                    var responses = response;
                    response = {};
                    response.data = responses;
                    //response.data = response;
                    //$scope.LoadFinishGoods();
                    $scope.clear();
                    response.data = -102;
                    ShowCustomToastrMessage(response);
                    //$scope.btnRawMaterialShowText = "Show List";
                    //$scope.ShowHide();
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

        }
        $scope.delete = function (dataModel) {
            //var IsConf = confirm('You are about to delete ' + dataModel.ArticleNo + '. Are you sure?');
            //if (IsConf) {

            var FinishGood = {
                ItemID: dataModel.ItemID,
                DeleteBy: LoggedUserID
            }
            var apiRoute = baseUrl + 'DeleteFinishGood/';
            var _deleteFinishGood = FinishGoodSerivce.put(apiRoute, FinishGood);
            _deleteFinishGood.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                $scope.RefreshMasterList();
                //$scope.LoadFinishGoods();
            }, function (error) {
                console.log("Error: " + error);
            });
            //}
        }
        $scope.getFinishGoodByItemId = function (dataModel) {
            var apiRoute = baseUrl + 'GetFinishGoodById/' + dataModel.ItemID;
            var finishGood = FinishGoodSerivce.getFinishGoodByID(apiRoute);
            finishGood.then(function (response) {
                //$scope.btnFinisFoodShow = "Update";
                //$scope.btnRawMaterialShowText = "Show List";
                //$scope.grdShowHide = false;
                //$scope.MainForm = true;
                $scope.ItemID = response.data.ItemID;
                $scope.txtArticaleNo = response.data.ArticleNo;
                $scope.txtbxGPPI = response.data.GPPI;
                $scope.txtbxSetNo = response.data.SetNo;
                $scope.txtbxDescripion = response.data.Remark;
                $scope.txtbxCutablewidth = response.data.CuttableWidth;
                $scope.txtbxHSCODE = response.data.HSCODE;
                $scope.txtbxFlangeNo = response.data.FlangeNo;
                $scope.txtbxLength = response.data.Length;
                $scope.txtbxWeightDS = response.data.WeightPerUnit;
                //$scope.txbxFabricName = response.data.ItemName;
                $scope.txtbxGEPI = response.data.GEPI;
                $scope.txtbxWeave = response.data.Weave;
                $scope.txtbxTotalEnds = response.data.TotalEnds;
                $scope.txbxNote = response.data.Note;
                $scope.txtbxConstruction = response.data.Remark;
                debugger;
                $scope.txtbxWarpLot = response.data.WarpYarnRatioLot;
                $scope.txtbxWarpRatio = response.data.WarpYarnRatio;
                $scope.txtbxWeftLost = response.data.WeftYarnRatioLot;
                $scope.txtbxWeftRatio = response.data.WeftpYarnRatio;
                $scope.txtFiniWeight = response.data.FinishingWeigth;

                $scope.SetDropdwn(response);
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.SetAccDetailID = function (groupID) {
            $scope.AcDetailID = 0;
            if (groupID != 0) {
                var ItemTypeID = groupID;
                var apiRoute = baseUrl + 'GetAcDetailIDByGroupID/';
                var GroupProcess = FinishGoodSerivce.getAccDetailsByGroupID(apiRoute, page, pageSize, isPaging, LoggedCompanyID, groupID);
                GroupProcess.then(function (response) {
                    $scope.AcDetailID = response.data.AcDetailID;

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        };

        $scope.SetDropdwn = function (response) {
            //$scope.drpdwnFiniWeight = response.data.FinishingWeightID;
            //if (response.data.FinishingWeigth != 0.00) {
            //    $("#drpdwnFiniWeight").select2("data", { id: 0, text: response.data.FinishingWeigth });
            //} else {
            //    $("#drpdwnFiniWeight").select2("data", { id: 0, text: '-- Select FINI Weight --' });
            //}

            $scope.drpItemGroup = response.data.ItemGroupID;

            if (response.data.ItemGroupName != 'N/A') {
                $("#drpItemGroup").select2("data", { id: 0, text: response.data.ItemGroupName });
            } else {
                $("#drpItemGroup").select2("data", { id: 0, text: '-- Select Item Group --' });
            }

            $scope.drpdwnFinishProcess = response.data.FinishingTypeID;
            if (response.data.FinishingProcessName != 'N/A') {
                $("#drpdwnFinishProcess").select2("data", { id: 0, text: response.data.FinishingProcessName });
            } else {

                $("#drpdwnFinishProcess").select2("data", { id: 0, text: '-- Select Finish Process --' });
            }

            $scope.drpWarpYarnCount = response.data.WarpYarnID;
            if (response.data.WarpYarnCount != 'N/A') {
                $scope.drpWarpYarnCountName = response.data.WarpYarnCount;
                //$("#drpWarpYarnCount").select2("data", { id: 0, text: response.data.WarpYarnCount });
            }
            else {
                $scope.drpWarpYarnCountName = "";
                $("#drpWarpYarnCount").select2("data", { id: 0, text: '-- Select Warp count --' });
            }

            $scope.drpWeftCount = response.data.WeftYarnID;
            if (response.data.WeftYarnCount != 'N/A') {
                $scope.drpWeftCountName = response.data.WeftYarnCount;
                //$("#drpWeftCount").select2("data", { id: 0, text: response.data.WeftYarnCount });
            } else {
                $scope.drpWeftCountName = "";
                //$("#drpWeftCount").select2("data", { id: 0, text: '-- Select Weft Count --' });
            }

            $scope.drpWeftSupplier = response.data.WeftSupplierId;
            if (response.data.WeftsupplierFullName != 'N/A') {
                $("#drpWeftSupplier").select2("data", { id: 0, text: response.data.WeftsupplierFullName });
            } else {
                $("#drpWeftSupplier").select2("data", { id: 0, text: '-- Select Weft Supplier --' });
            }

            $scope.drpColor = response.data.ItemColorID;
            if (response.data.Color != 'N/A') {
                $("#drpColor").select2("data", { id: 0, text: response.data.Color });
            } else {
                $("#drpColor").select2("data", { id: 0, text: '-- Select Color --' });
            }

            $scope.drpBuyerRef = response.data.BuyerRefID;
            if (response.data.BuyerRefFullName != 'N/A') {
                $("#drpBuyerRef").select2("data", { id: 0, text: response.data.BuyerRefFullName });
            } else {
                $("#drpBuyerRef").select2("data", { id: 0, text: '-- Select Buyer ref --' });
            }

            $scope.drpBuyerName = response.data.BuyerID;
            if (response.data.BuyerFullName != 'N/A') {
                $("#drpBuyerName").select2("data", { id: 0, text: response.data.BuyerFullName });
            } else {
                $("#drpBuyerName").select2("data", { id: 0, text: '-- Select Buyer Name --' });
            }

            $scope.drpCoating = response.data.CoatingID;
            if (response.data.CoatingName != 'N/A') {
                $("#drpCoating").select2("data", { id: 0, text: response.data.CoatingName });
            } else {
                $("#drpCoating").select2("data", { id: 0, text: '-- Select Coating --' });
            }

            $scope.drpSpeCoating = response.data.SPCoatingID;
            if (response.data.SpecialCoatingName != 'N/A') {
                $("#drpSpeCoating").select2("data", { id: 0, text: response.data.SpecialCoatingName });
            } else {
                $("#drpSpeCoating").select2("data", { id: 0, text: '-- Select Special Coating --' });
            }

            $scope.drpOverDyed = response.data.OverDyedID;
            if (response.data.OverDyedName != 'N/A') {
                $("#drpOverDyed").select2("data", { id: 0, text: response.data.OverDyedName });
            } else {
                $("#drpOverDyed").select2("data", { id: 0, text: '-- Select Overdyed --' });
            }

            $scope.drpWeftColor = response.data.WeftColorID;
            if (response.data.WeftColorName != 'N/A') {
                $("#drpWeftColor").select2("data", { id: 0, text: response.data.WeftColorName });
            } else {
                $("#drpWeftColor").select2("data", { id: 0, text: '-- Select Weft Color --' });
            }

            

        }

        $scope.clear = function () {
            //$scope.btnFinisFoodShow = "Save";
            $scope.IsHidden = true;
            //$scope.grdShowHide = true;
            //$scope.MainForm = false;
            ItemTypeID = 1;

            $scope.frmFinishGood.$setPristine();
            $scope.frmFinishGood.$setUntouched();
            $scope.ItemID = 0;
            $scope.txtArticaleNo = '';
            $scope.txtFiniWeight = '';
            $scope.txtbxWarpLot = '';
            $scope.txtbxWeftLost = '';
            $scope.txtbxGPPI = '';
            $scope.txtbxSetNo = '';
            $scope.txtbxDescripion = '';
            $scope.txtbxCutablewidth = '';
            $scope.txtbxFlangeNo = '';
            $scope.txtbxLength = '';
            $scope.txtbxHSCODE = '';
            $scope.txbxFabricName = '';
            $scope.txtbxWarpRatio = '';
            $scope.txtbxWeftRatio = '';
            $scope.txtbxGEPI = '';
            $scope.txtbxWeave = '';
            $scope.txtbxTotalEnds = '';
            $scope.txbxNote = '';
            $scope.txtbxConstruction = '';
            $("#drpdwnFiniWeight").select2("val", '');
            $scope.drpdwnFiniWeight = 0;
            $("#drpdwnFinishProcess").select2("val", '');
            $scope.drpdwnFinishProcess = 0;
            $scope.drpItemGroup = '';
            $("#drpItemGroup").select2("data", { id: 0, text: '-- Select Item Group --' });
            //$("#drpItemGroup").select2("val", '');
            // $scope.drpItemGroup = 0;
            //  $("#drpWarpYarnCount").select2("val", '');
            $scope.drpWarpYarnCount = 0;
            $scope.drpWarpYarnCountName = "";
            //$("#drpWeftCount").select2("val", '');
            $scope.drpWeftCount = 0;
            $scope.drpWeftCountName = "";
            $("#drpWeftSupplier").select2("val", '');
            $scope.drpWeftSupplier = 0;
            $("#drpColor").select2("val", '');
            $scope.drpColor = 0;
            $("#drpBuyerRef").select2("val", '');
            $scope.drpBuyerRef = 0;
            $("#drpBuyerName").select2("val", '');
            $scope.drpBuyerName = 0;

            $scope.drpCoating = 0;            
            $("#drpCoating").select2("val", '');
            $scope.drpSpeCoating = 0;
            $("#drpSpeCoating").select2("val", '');

            $scope.drpOverDyed = 0;
            $("#drpOverDyed").select2("val", '');
           
            $scope.drpWeftColor = 0;
            $("#drpWeftColor").select2("val", ''); 
        }

        $scope.finishProcessessettings = {
            scrollableHeight: '300px',
            scrollable: true,
        };

    }]);
function modal_fadeOut() {
    $("#WarpYarnSetUpModal").fadeOut(200, function () {
        $('#WarpYarnSetUpModal').modal('hide');
    });
}
