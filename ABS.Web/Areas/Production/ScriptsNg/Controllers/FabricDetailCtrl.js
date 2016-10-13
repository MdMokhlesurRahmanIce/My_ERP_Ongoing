/*
*    Created By: Shamim Uddin;
*    Create Date: 2-6-2016 (dd-mm-yy); Updated Date: 2-6-2016 (dd-mm-yy);
*    Name: 'FabricDetailController';
*    Type: $scope;
*    Purpose: Fabric Details For RND;
*    Service Injected: '$scope', 'ItemGroupService','conversion','uiGridConstants','uiGridGroupingConstants';
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);



app.controller('FabricDetailController', ['$scope', 'FinishGoodSerivce', '$filter', 'conversion', 'uiGridConstants', 'uiGridGroupingConstants', 'PublicService',
    function ($scope, FinishGoodSerivce, $filter, conversion, uiGridConstants, uiGridGroupingConstants, PublicService) {

        $scope.gridOptionsFg = [];
        $scope.gridOptionsCnsumption = [];
        var objcmnParamFg = {};
        var objcmnParam = {};
        var baseUrl = '/Production/api/FinishGood/';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.loaderMore = false;
        $scope.fabdevShow = true;
        $scope.FabricInfoIsShow = false;
        $scope.WeavingIsShow = false;
        $scope.FinishingIsShow = false;
        $scope.WashingIsShow = false;
        $scope.ConsumptionIsShow = false;

        $scope.IsListShow = true;
        $scope.ItemID = 0;
        var ItemTypeID = 1;
        $scope.btnFinisFoodShow = "Save";
        $scope.btnRawMaterialShowText = "New";
        $scope.PageTitle = 'Fabric Detail Info';
        $scope.WeavingTitle = 'Weaving Info';
        $scope.WashingTitle = 'Washing Info';
        $scope.FinishingTitle = "Finishing Info";
        $scope.CompusitionTitle = "Compusition";
        $scope.ConsumptionTitle = "Consumption Info";
        $scope.FabricDevelopmentList = "Fabric Detail List";
        $scope.ModalHeading = 'Count Setting';
        $scope.btnSaveWarpYan = 'Save';
        $scope.btnMWYAdd = 'Add';
        $scope.ConsumptionModalTitle = 'Yarn Consumption Setting'
        //$scope.btnMWrpYarnReset = 'Reset'; 
        $scope.developmentNoList = [];
        $scope.ListWarpYarn = [];
        $scope.resultFile = [];
        $scope.DocumentList = [];
        $scope.drpWarpYarnCount = 0;
        $scope.drpWeftCount = 0;
        $scope.txtDocName = "";
        var countType = "";
        var date = new Date();
        $scope.weavingDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

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
                $scope.LoadFinishGoods();
            },
            firstPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg = 1
                    $scope.LoadFinishGoods();
                }
            },
            nextPageFg: function () {
                if (this.pageNumberFg < this.getTotalPagesFg()) {
                    this.pageNumberFg++;
                    $scope.LoadFinishGoods();
                }
            },
            previousPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg--;
                    $scope.LoadFinishGoods();
                }
            },
            lastPageFg: function () {
                if (this.pageNumberFg >= 1) {
                    this.pageNumberFg = this.getTotalPagesFg();
                    $scope.LoadFinishGoods();
                }
            }
        };

        //ui-Grid Call
        $scope.LoadFinishGoods = function () {
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";
            objcmnParamFg = {
                pageNumber: $scope.paginationFg.pageNumberFg,
                pageSize: $scope.paginationFg.pageSizeFg,
                IsPaging: isPaging,
                loggeduser: LoggedUserID,
                loggedCompany: LoggedCompanyID,
                menuId: 5,
                tTypeId: 26,
                ItemType: ItemTypeID
            };
            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptionsFg = {

                columnDefs: [
                    { name: "ItemID", displayName: "Item ID", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "PDL Ref", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingWeigth", displayName: "Fini Wt", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WarpYarnRatio", displayName: "Warp Ratio", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WarpYarnRatioLot", displayName: "Warp SL lot", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Constraction", displayName: "Constraction", width: '11%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftpYarnRatio", displayName: "Weft Ratio", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeftYarnRatioLot", displayName: "Weft SL Lot", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GerigeEPIxPPI", displayName: "Gerige EPIxPPI", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Color", displayName: "Color", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Width", displayName: "Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "Cuttable Width", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LengthShrinkage", displayName: "Length Shrinkage", width: '13%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WidthShrinkage", displayName: "Width Shrinkage", width: '13%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Cotton", displayName: "Cotton(%)", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Spandex", displayName: "Spandex(%)", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Polyester", displayName: "Plystar(%)", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FlangeNo", displayName: "Flange No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "SetNo", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remark", displayName: "Remark", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineName", displayName: "Machine Name", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeavingDate", displayName: "Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Length", displayName: "Length", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GGSM", displayName: "GGSM", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GWidth", displayName: "GWidth", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '7%',
                        pinnedRight: true,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                                      ' <a ng-href="#GroupModal" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.getFabricDevelopmentDetailById(row.entity)">' +
                                          '<i class="icon-pencil"></i>' +
                                      ' </a>' +
                                  ' </span>' +
                                   '<span class="label label-danger label-mini">' +
                                     '  <a href="#" class="bs-tooltip" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                           '<i class="icon-trash"></i>' +
                                      ' </a>' +
                                  ' </span>'
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
            var apiRoute = baseUrl + 'GetFabricDevelopmentList/';
            var _finishGoods = FinishGoodSerivce.getAllFinishGood(apiRoute, objcmnParamFg);
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
        $scope.LoadFinishGoods();
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
        }

        $scope.saveWarpYarn = function () {
            if ($scope.ListWarpYarn.length > 0) {
                var apiRoute = baseUrl + 'SaveYarn/';
                var porcessMasterDetails = FinishGoodSerivce.postList(apiRoute, $scope.ListWarpYarn);
                porcessMasterDetails.then(function (response) {
                    if (response.data != null) {
                        debugger;
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

        //$scope.LoadRatioAndLot = function (yarnId) {
        //    // getYarnByID
        //    var apiRoute = baseUrl + 'GetYarnBYId/' + yarnId;
        //    var finishGood = FinishGoodSerivce.getYarnByID(apiRoute);
        //    finishGood.then(function (response) {
        //        if (response.data.YarnType == "Warp") {
        //            $scope.LoadWarpcounts();
        //            $scope.txtbxWarpLot = response.data.YarnRatioLot;
        //            $scope.txtbxWarpRatio = response.data.YarnRatio;
        //            $scope.drpWarpYarnCount = response.data.YarnID;
        //            $("#drpWarpYarnCount").select2("data", { id: response.data.YarnID, text: response.data.YarnCount });

        //        } else {

        //            $scope.LoadWeftcounts();
        //            $scope.txtbxWeftLost = response.data.YarnRatioLot;
        //            $scope.txtbxWeftRatio = response.data.YarnRatio;
        //            $scope.drpWeftCount = response.data.YarnID;
        //            $("#drpWeftCount").select2("data", { id: response.data.YarnID, text: response.data.YarnCount });


        //        }
        //    }, function (error) {
        //        console.log("Error: " + error);
        //    });
        //};
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

        $scope.finishWeights = function () {
            var apiRoute = baseUrl + 'GetFinishWeights/';
            var finishWeights = FinishGoodSerivce.getFiniWeight(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            finishWeights.then(function (response) {
                $scope.finishWeights = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.finishWeights();
        // Company Wise Department
        $scope.departmentId = 0;
        if (LoggedCompanyID == 1) {
            $scope.departmentId = 11;
        }
        else if (LoggedCompanyID == 2) {
            $scope.departmentId = 60;
        }
        $scope.GetMachines = function () {
            var apiRoute = baseUrl + 'GetMachines/'; // 
            var _Machine = FinishGoodSerivce.getWeavingMachines(apiRoute, page, pageSize, isPaging, $scope.departmentId);
            _Machine.then(function (response) {
                $scope.Machines = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetMachines();
        $scope.GetFinishProcess = function () {
            //var apiRoute = baseUrl + 'GetFinishProcess/';
            //var _finishProcesses = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            //_finishProcesses.then(function (response) {
            //    $scope.finishProcesses = response.data;
            //},
            //function (error) {
            //    console.log("Error: " + error);
            //});
            var apiRoute = baseUrl + 'GetFinishProcess/';
            var _finishProcesses = FinishGoodSerivce.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            _finishProcesses.then(function (response) {
                $scope.ListMultiProcessModel = [];
                $scope.finishProcesses = response.data;
                // $scope.ListMultiProcessModel = item;     
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetFinishProcess();

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


        $scope.LoadWarpCountList = function () {
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
            if ($scope.drpWeftCount != 0) {

                Command: toastr["warning"]("Weft Count has been Selected");
                return;

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
        $scope.save = function () {
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
                //------------------------Weaving Info---------------------------
                WeavingMachineID: $scope.drpmc,
                GSM: $scope.txtbxWeaveingGGSM,
                WeavingLength: $scope.txtbxWeaveingLength,
                GWidth: $scope.txtbxWeaveingGWidth,
                WeavingDate: $scope.weavingDate == "" ? "1/1/1900" : conversion.getStringToDate($scope.weavingDate),
                //WeavingDate: $scope.weavingDate,
                //-------------------------Finishing Info-------------------------
                MinLShrinkage: $scope.txtbxfinishingLengthShrinkageMin,
                MaxLshrinkage: $scope.txtbxfinishingLengthShrinkageMax,
                MinWshrinkage: $scope.txtbxFinishingWidthShrinkageMin,
                MaxWShrinkage: $scope.txtbxFinishingWidthShrinkageMax,
                Skew: $scope.txtbxfinishingSkew,
                EPI: $scope.txtbxFinishingEPI,
                PPI: $scope.txtbxFinishingPPI,
                Cotton: $scope.txtbxFinishingCotton,
                Spandex: $scope.txtbxFinishingSpandex,
                Polyester: $scope.txtbxFinishingPolystar,
                Lycra: $scope.txtbxFinishingLaycra,
                FinishingWidth: $scope.txtbxFinishingWidth,
                T4100: $scope.txtbxT4100,
                Viscos: $scope.txtbxViscos,
                Modal: $scope.txtbxModal,
                C4100: $scope.txtbxC4100,
                Tencel: $scope.txtbxTencel,
                OtherComp: $scope.txtCopusitonOther,
                //------------------------------Washing Info-------------------------------
                MinLShrinkageW: $scope.txtbxWashingLengthShrinkageMin,
                MaxLshrinkageW: $scope.txtbxWashingLengthShrinkageMax,
                MinWshrinkageW: $scope.txtbxWashingWidthShrinkageMin,
                MaxWShrinkageW: $scope.txtbxWashingWidthShrinkageMax,
                SkewW: $scope.txtbxWashingSkew,
                WEPI: $scope.txtbxWashingEPI,
                WPPI: $scope.txtbxWashingPPI,
                WashingWidth: $scope.txtbxAwWidth,
                WashingWeigth: $scope.txtbxAwWeight,
                IsDevelopmentComplete: 1,
                CoatingID: ($scope.drpCoating == 0 ? null : $scope.drpCoating),
                SPCoatingID: ($scope.drpSpeCoating == 0 ? null : $scope.drpSpeCoating),
                OverDyedID: ($scope.drpOverDyed == 0 ? null : $scope.drpOverDyed),
                WeftColorID: ($scope.drpWeftColor == 0 ? null : $scope.drpWeftColor),
            }
            isExisting = $scope.ItemID;
            if (isExisting == 0) {
                var apiRoute = baseUrl + 'SaveFinishGood/';
                var SaveFinishGood = FinishGoodSerivce.post(apiRoute, FinishGood);
                SaveFinishGood.then(function (response) {
                    var articalNo = response.data;
                    $scope.txtArticaleNo = articalNo;
                    response.data = 1;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    $scope.LoadFinishGoods();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                debugger
                var apiRoute = baseUrl + 'UpdateFinishGood/';
                var FinishGood = FinishGoodSerivce.putList(apiRoute, FinishGood, $scope.ListMultiProcessModel);
                FinishGood.then(function (response) {
                    var responses = response;
                    response = {};
                    response.data = responses;
                    debugger
                    response.data = 1;
                    var apiRoutea = baseUrl + 'uploadedFile/';
                    var a = FinishGoodSerivce.postFile(apiRoutea, $scope.DocumentList);
                    a.then(function (response) {
                        $scope.DocumentList = [];
                        debugger
                        $scope.clear();
                        $scope.LoadFinishGoods();
                        $scope.btnRawMaterialShowText = "Show List";
                        $scope.ShowHide();
                    }, function (error) {
                        console.log("Error: " + error);
                    });
                    //var data = new FormData();
                    //for (var i in $scope.files) {
                    //    data.append("uploadedFile", $scope.files[i]);
                    //}
                    //data.append("uploadedFile", response.data);
                    //// ADD LISTENERS.
                    //var objXhr = new XMLHttpRequest();
                    ////objXhr.addEventListener("progress", updateProgress, false);
                    ////objXhr.addEventListener("load", transferComplete, false);
                    //var apiRoute = '/Inventory/api/GRR/' + 'UploadFiles/';
                    //objXhr.open("POST", apiRoute);
                    //objXhr.send(data);
                    //// debugger;
                    //document.getElementById('file').value = '';
                    //$scope.files = [];

                    /////////// end file upload ///////////////////////////////

                    debugger
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    $scope.LoadFinishGoods();
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.PDLRef + '. Are you sure?');
            if (IsConf) {

                var FinishGood = {
                    ItemID: dataModel.ItemID,
                    DeleteBy: LoggedUserID
                }
                var apiRoute = baseUrl + 'DeleteFinishGood/';
                var _deleteFinishGood = FinishGoodSerivce.put(apiRoute, FinishGood);
                _deleteFinishGood.then(function (response) {
                    response.data = -101;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    $scope.LoadFinishGoods();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        $scope.LoadConsumptionCal = function () {
            $scope.ItemID;
            $scope.gridOptionsCnsumption.showColumnFooter = true;
            $scope.gridOptionsCnsumption.showGridFooter = true;
            if ($scope.ItemID > 0) {

                $scope.gridOptionsCnsumption = {
                    showGridFooter: true,
                    showColumnFooter: true,
                    columnDefs: [
                        {
                            name: "YarnType", displayName: "Consumtion Type", width: '15%', headerCellClass: $scope.highlightFilteredHeader
                        },
                        { name: "YarnSP", displayName: "Yarn SP", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ItemGroupName", displayName: "Yarn Specification", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "LotNo", displayName: "Lot No", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                        { name: "YarnCount", displayName: "Count", width: '8%', headerCellClass: $scope.highlightFilteredHeader },
                        {
                            name: "BeamRatio", displayName: "Beam Ratio", width: '10%', headerCellClass: $scope.highlightFilteredHeader,
                            footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: none;color: #000000">Total:</div>'
                        },
                        {
                            name: "WeightPerUnit", displayName: "Kg/Yd.", headerCellClass: $scope.highlightFilteredHeader,
                            aggregationType: uiGridConstants.aggregationTypes.sum,
                            aggregationHideLabel: true
                        },
                        {
                            name: "TotalEnds", displayName: "Total Ends", width: '10%', headerCellClass: $scope.highlightFilteredHeader,
                            aggregationType: uiGridConstants.aggregationTypes.sum,
                            aggregationHideLabel: true
                        },
                        {
                            name: "NoOfPick", displayName: "No.Pick", width: '7%', headerCellClass: $scope.highlightFilteredHeader,
                            aggregationType: uiGridConstants.aggregationTypes.sum,
                            aggregationHideLabel: true

                        }
                        //{ field: 'NoOfPick', displayName: 'NoOfPick (total)', width: '15%', aggregationType: uiGridConstants.aggregationTypes.sum }
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

                var apiRoute = baseUrl + 'GetConsumptionInfoByItemID/' + $scope.ItemID;
                var consumption = FinishGoodSerivce.getFinishGoodByID(apiRoute);
                consumption.then(function (response) {
                    $scope.gridOptionsCnsumption.data = response.data
                    // For Loading modal
                    $("#ModalConsumption").fadeIn(200, function () { $('#ModalConsumption').modal('show'); });

                },
            function (error) {
                console.log("Error: " + error);
            });

            }
            else {
                Command: toastr["warning"]("Please Select Fabric");
                return;
            }
        }
        $scope.getFabricDevelopmentDetailById = function (dataModel) {
            $scope.LoadDocumentList(dataModel.ItemID, $scope.UserCommonEntity.currentTransactionTypeID);
            var apiRoute = baseUrl + 'GetFabricDevelopmentDetailById/' + dataModel.ItemID;
            var finishGood = FinishGoodSerivce.getFinishGoodByID(apiRoute);
            finishGood.then(function (response) {
                $scope.btnRawMaterialShowText = "New";
                $scope.ShowHide();

                $scope.FabricInfoIsShow = true;
                $scope.WeavingIsShow = true;
                $scope.FinishingIsShow = true;
                $scope.CompusitionTitleIsShow = true;
                $scope.WashingIsShow = true;
                $scope.ConsumptionIsShow = true;
                $scope.fabdevShow = false;
                //-----------------------------------------textbox of Fabric development----------------------------
                //$scope.btnFinisFoodShow = "Update";
                $scope.ItemID = response.data.ItemID;
                $scope.txtArticaleNo = response.data.ArticleNo;
                $scope.txtbxGPPI = response.data.GPPI;
                $scope.txtbxSetNo = response.data.SetNo;
                $scope.txtbxDescripion = response.data.Remark;
                $scope.txtbxCutablewidth = response.data.CuttableWidth;
                $scope.txtbxHSCODE = response.data.HSCODE;
                $scope.txtbxFlangeNo = response.data.FlangeNo;
                $scope.txtbxLength = response.data.Lengthyds;
                $scope.txtbxWeightDS = response.data.WeightPerUnit;
                $scope.txbxFabricName = response.data.ItemName;
                $scope.txtbxGEPI = response.data.GEPI;
                $scope.txtbxWeave = response.data.Weave;
                $scope.txtbxTotalEnds = response.data.TotalEnds;
                $scope.txbxNote = response.data.Note;
                $scope.txtbxConstruction = response.data.Remark;
                $scope.txtbxWarpLot = response.data.WarpYarnRatioLot;
                $scope.txtbxWarpRatio = response.data.WarpYarnRatio;
                $scope.txtbxWeftLost = response.data.WeftYarnRatioLot;
                $scope.txtbxWeftRatio = response.data.WeftpYarnRatio;
                //-------------------------------textbox of Weaving Info --------------------------------------
                $scope.txtbxWeaveingGGSM = response.data.GGSM;
                $scope.txtbxWeaveingLength = response.data.WeavingLength;
                $scope.txtbxWeaveingGWidth = response.data.GWidth;
                if (response.data.WeavingDate != null) {
                    $scope.weavingDate = conversion.getDateToString(response.data.WeavingDate);
                }
                //-------------------------------Finishing Info-------------------------------------------------
                $scope.txtbxfinishingLengthShrinkageMin = response.data.MinLShrinkage;
                $scope.txtbxfinishingLengthShrinkageMax = response.data.MaxLshrinkage;
                $scope.txtbxFinishingWidthShrinkageMin = response.data.MinWshrinkage;
                $scope.txtbxFinishingWidthShrinkageMax = response.data.MaxWShrinkage;
                $scope.txtbxfinishingSkew = response.data.Skew;
                $scope.txtbxFinishingEPI = response.data.EPI;
                $scope.txtbxFinishingPPI = response.data.PPI;
                $scope.txtbxFinishingCotton = response.data.Cotton;
                $scope.txtbxFinishingSpandex = response.data.Spandex;
                $scope.txtbxFinishingPolystar = response.data.Polyester;
                $scope.txtbxFinishingLaycra = response.data.Lycra;
                $scope.txtbxT4100 = response.data.T400
                $scope.txtbxViscos = response.data.Viscos
                $scope.txtbxModal = response.data.Modal
                $scope.txtbxC4100 = response.data.C4100
                $scope.txtbxTencel = response.data.Tencel
                $scope.txtbxFinishingWidth = response.data.FinishingWidth;
                $scope.txtCopusitonOther = response.data.OtherComp;
                //-------------------------------Washing Info-------------------------------------------------
                $scope.txtbxWashingLengthShrinkageMin = response.data.MinLShrinkageW;
                $scope.txtbxWashingLengthShrinkageMax = response.data.MaxLshrinkageW;
                $scope.txtbxWashingWidthShrinkageMin = response.data.MinWshrinkageW;
                $scope.txtbxWashingWidthShrinkageMax = response.data.MaxWShrinkageW;
                $scope.txtbxWashingSkew = response.data.SkewW;
                $scope.txtbxWashingEPI = response.data.WEPI;
                $scope.txtbxWashingPPI = response.data.WPPI;
                $scope.txtbxAwWidth = response.data.WashingWidth;
                $scope.txtbxAwWeight = response.data.WashingWeigth;
                //--------- GetFinishing Process By ID ---------
                $scope.GetFinishingProcessByItemID($scope.ItemID);

                $scope.SetDropdwn(response);
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.getFabricDevelopmentDetailByIdInsertion = function (dataModel) {
            var apiRoute = baseUrl + 'GetFabricDevelopmentDetailsByID/' + dataModel.ItemID;
            var finishGood = FinishGoodSerivce.getFinishGoodByID(apiRoute);
            finishGood.then(function (response) {

                $scope.FabricInfoIsShow = true;
                $scope.WeavingIsShow = true;
                $scope.FinishingIsShow = true;
                $scope.CompusitionTitleIsShow = true;
                $scope.WashingIsShow = true;
                $scope.ConsumptionIsShow = true;
                $scope.fabdevShow = false;
                //-----------------------------------------textbox of Fabric development----------------------------
                //$scope.btnFinisFoodShow = "Update";
                $scope.ItemID = response.data.ItemID;
                $scope.txtArticaleNo = response.data.ArticleNo;
                $scope.txtbxGPPI = response.data.GPPI;
                $scope.txtbxSetNo = response.data.SetNo;
                $scope.txtbxDescripion = response.data.Remark;
                $scope.txtbxCutablewidth = response.data.CuttableWidth;
                $scope.txtbxHSCODE = response.data.HSCODE;
                $scope.txtbxFlangeNo = response.data.FlangeNo;
                $scope.txtbxLength = response.data.Lengthyds;
                $scope.txtbxWeightDS = response.data.WeightPerUnit;
                $scope.txbxFabricName = response.data.ItemName;
                $scope.txtbxGEPI = response.data.GEPI;
                $scope.txtbxWeave = response.data.Weave;
                $scope.txtbxTotalEnds = response.data.TotalEnds;
                $scope.txbxNote = response.data.Note;
                $scope.txtbxConstruction = response.data.Remark;
                $scope.txtbxWarpLot = response.data.WarpYarnRatioLot;
                $scope.txtbxWarpRatio = response.data.WarpYarnRatio;
                $scope.txtbxWeftLost = response.data.WeftYarnRatioLot;
                $scope.txtbxWeftRatio = response.data.WeftpYarnRatio;
                //-------------------------------textbox of Weaving Info --------------------------------------
                $scope.txtbxWeaveingGGSM = response.data.GGSM;
                $scope.txtbxWeaveingLength = response.data.WeavingLength;
                $scope.txtbxWeaveingGWidth = response.data.GWidth;
                if (response.data.WeavingDate != null) {
                    $scope.weavingDate = conversion.getDateToString(response.data.WeavingDate);
                }
                //-------------------------------Finishing Info-------------------------------------------------
                $scope.txtbxfinishingLengthShrinkageMin = response.data.MinLShrinkage;
                $scope.txtbxfinishingLengthShrinkageMax = response.data.MaxLshrinkage;
                $scope.txtbxFinishingWidthShrinkageMin = response.data.MinWshrinkage;
                $scope.txtbxFinishingWidthShrinkageMax = response.data.MaxWShrinkage;
                $scope.txtbxfinishingSkew = response.data.Skew;
                $scope.txtbxFinishingEPI = response.data.EPI;
                $scope.txtbxFinishingPPI = response.data.PPI;
                $scope.txtbxFinishingCotton = response.data.Cotton;
                $scope.txtbxFinishingSpandex = response.data.Spandex;
                $scope.txtbxFinishingPolystar = response.data.Polyester;
                $scope.txtbxFinishingLaycra = response.data.Lycra;
                $scope.txtbxT4100 = response.data.T400
                $scope.txtbxViscos = response.data.Viscos
                $scope.txtbxModal = response.data.Modal
                $scope.txtbxC4100 = response.data.C4100
                $scope.txtbxTencel = response.data.Tencel
                $scope.txtbxFinishingWidth = response.data.FinishingWidth;
                //-------------------------------Washing Info-------------------------------------------------
                $scope.txtbxWashingLengthShrinkageMin = response.data.MinLShrinkageW;
                $scope.txtbxWashingLengthShrinkageMax = response.data.MaxLshrinkageW;
                $scope.txtbxWashingWidthShrinkageMin = response.data.MinWshrinkageW;
                $scope.txtbxWashingWidthShrinkageMax = response.data.MaxWShrinkageW;
                $scope.txtbxWashingSkew = response.data.SkewW;
                $scope.txtbxWashingEPI = response.data.WEPI;
                $scope.txtbxWashingPPI = response.data.WPPI;
                $scope.txtbxAwWidth = response.data.WashingWidth;
                $scope.txtbxAwWeight = response.data.WashingWeigth;
                //--------- GetFinishing Process By ID ---------
                $scope.GetFinishingProcessByItemID($scope.ItemID);

                $scope.SetDropdwn(response);
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.SetDropdwn = function (response) {

            //-----------------------------------------drpdwn of Fabric development----------------------------
            $scope.drpdwnFiniWeight = response.data.FinishingWeightID;
            if (response.data.FinishingWeigth != 0.00) {
                $("#drpdwnFiniWeight").select2("data", { id: 0, text: response.data.FinishingWeigth });
            } else {
                $("#drpdwnFiniWeight").select2("data", { id: 0, text: '-- Select FINI Weight --' });
            }

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
            } else {
                $scope.drpWarpYarnCountName = "";
                //$("#drpWarpYarnCount").select2("data", { id: 0, text: '-- Select Warp count --' });
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
            //-------------------------------drpwn of Weaving Info --------------------------------------
            $scope.drpmc = response.data.WeavingMachineID;
            if (response.data.MachineName != 'N/A') {
                $("#drpmc").select2("data", { id: 0, text: response.data.MachineName });
            } else {
                $("#drpmc").select2("data", { id: 0, text: '--Select Machine--' });
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
            $scope.btnFinisFoodShow = "Save";
            ItemTypeID = 1,
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
            $scope.txtbxWeaveingLength = '';
            $scope.txtbxWeaveingGGSM = '';
            $scope.txtbxWeaveingGWidth = '';
            $scope.txtbxfinishingLengthShrinkageMin = '';
            $scope.txtbxFinishingWidthShrinkageMin = '';
            $scope.txtbxFinishingEPI = '';
            $scope.txtbxfinishingLengthShrinkageMax = '';
            $scope.txtbxFinishingWidthShrinkageMax = '';
            $scope.txtbxFinishingPPI = '';
            $scope.txtbxfinishingSkew = '';
            $scope.txtbxFinishingCotton = '';
            $scope.txtbxFinishingWidth = '';
            $scope.txtbxFinishingPolystar = '';
            $scope.txtbxFinishingLaycra = '';
            $scope.txtbxFinishingSpandex = '';
            $scope.txtbxT4100 = '';
            $scope.txtbxViscos = '';
            $scope.txtbxModal = '';
            $scope.txtbxTencel = '';
            $scope.txtbxC4100 = '';
            $scope.txtCopusitonOther = '';
            $scope.txtbxWashingLengthShrinkageMin = '';
            $scope.txtbxWashingLengthShrinkageMax = '';
            $scope.txtbxWashingWidthShrinkageMin = '';
            $scope.txtbxWashingWidthShrinkageMax = '';
            $scope.txtbxWashingSkew = '';
            $scope.txtbxWashingEPI = '';
            $scope.txtbxWashingPPI = '';
            $scope.txtbxAwWidth = '';
            $scope.txtbxAwWeight = '';
            $scope.txtbxWeightDS = '';
            $("#drpdwnFiniWeight").select2("data", { id: 0, text: '-- Select FINI Weight --' });
            $scope.drpdwnFiniWeight = 0;
            $("#drpdwnFinishProcess").select2("data", { id: 0, text: '-- Select Finish Process --' });
            $scope.drpdwnFinishProcess = 0;
            $scope.drpItemGroup = 0;
            $("#drpItemGroup").select2("data", { id: 0, text: '-- Select Item Group --' });
            //$("#drpItemGroup").select2("val", '');
            // $scope.drpItemGroup = 0;
            //$("#drpWarpYarnCount").select2("data", { id: 0, text: '-- Select Warp count --' });
            //$scope.drpWarpYarnCount = 0;

            $scope.drpWarpYarnCount = 0;
            $scope.drpWarpYarnCountName = "";


            //$("#drpWeftCount").select2("data", { id: 0, text: '-- Select Weft Count --' });
            //$scope.drpWeftCount = 0;

            $scope.drpWeftCount = 0;
            $scope.drpWeftCountName = "";

            $("#drpWeftSupplier").select2("data", { id: 0, text: '-- Select Weft Supplier --' });
            $scope.drpWeftSupplier = 0;
            $("#drpColor").select2("data", { id: 0, text: '-- Select Color --' });
            $scope.drpColor = 0;
            $("#drpBuyerRef").select2("data", { id: 0, text: '-- Select Buyer ref --' });
            $scope.drpBuyerRef = 0;
            $("#drpBuyerName").select2("data", { id: 0, text: '-- Select Buyer Name --' });
            $scope.drpBuyerName = 0;
            $("#drpmc").select2("data", { id: 0, text: '--Select Machine--' });
            $scope.drpmc = 0;
            //  $scope.ShowHide();

            $scope.drpCoating = 0;
            $("#drpCoating").select2("val", '');
            $scope.drpSpeCoating = 0;
            $("#drpSpeCoating").select2("val", '');

            $scope.drpOverDyed = 0;
            $("#drpOverDyed").select2("val", '');

            $scope.drpWeftColor = 0;
            $("#drpWeftColor").select2("val", '');
        }
        $scope.SaveConsumption = function () {
            var total = 0;
            var ConsumptionDetails = $scope.gridOptionsCnsumption.data;
            angular.forEach(ConsumptionDetails, function (value, key) {
                total += value.WeightPerUnit;
            });
            console.log(ConsumptionDetails);
            var ConsumptionMaster = {
                ItemID: $scope.ItemID,
                Description: $scope.txtbxmodalDescrpiton,
                Note: $scope.txtbxmodalNote,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            }
            var apiRoute = baseUrl + 'SaveConsumpiton/';
            var SaveConsumption = FinishGoodSerivce.postConsumptionmodal(apiRoute, ConsumptionDetails, ConsumptionMaster);
            SaveConsumption.then(function (response) {
                $scope.txtbxWeightDS = total;
                $("#ModalConsumption").fadeIn(200, function () { $('#ModalConsumption').modal('hide'); });
            }, function (error) {
                console.log("Error: " + error);
            });
            modal_fadeOut();
        }
        $scope.ShowHide = function () {
            $scope.fabdevShow = true;
            $scope.FabricInfoIsShow = false;
            $scope.WeavingIsShow = false;
            $scope.FinishingIsShow = false;
            $scope.CompusitionTitleIsShow = false;

            $scope.WashingIsShow = false;
            $scope.ConsumptionIsShow = false;
            //New Added
            if ($scope.btnRawMaterialShowText === "New") {
                $scope.btnRawMaterialShowText = "Show List";
                $scope.fabdevShow = false;
                $scope.FabricInfoIsShow = true;
                $scope.WeavingIsShow = true;
                $scope.FinishingIsShow = true;
                $scope.CompusitionTitleIsShow = true;
                $scope.WashingIsShow = true;
                $scope.ConsumptionIsShow = true;
                $scope.LoadDevelopmentNo(0);
            }
            else {
                $scope.clear();
                $scope.btnRawMaterialShowText = "New";
                $scope.fabdevShow = true;
                $scope.FabricInfoIsShow = false;
                $scope.WeavingIsShow = false;
                $scope.FinishingIsShow = false;
                $scope.CompusitionTitleIsShow = false;
                $scope.WashingIsShow = false;
                $scope.ConsumptionIsShow = false;
            }
            //New Added
        }
        $scope.GetFinishingProcessByItemID = function (item) {

            var apiRoute = baseUrl + 'GetFinishProcessByItem/';
            var _finishProcesses = FinishGoodSerivce.getFinishProcessByItem(apiRoute, page, pageSize, isPaging, LoggedCompanyID, item);
            _finishProcesses.then(function (response) {
                $scope.ListMultiProcessModel = response.data;
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
        //**************************** Load Devolopment ID *******************************
        $scope.LoadDevelopmentNo = function (IsDevelopmentComplete) {
            $scope.developmentNoList = [];
            var IsDevelopment = IsDevelopmentComplete;
            var apiRoute = baseUrl + 'GetDevelopmentNo/';
            var _developmentNo = FinishGoodSerivce.getFinishProcessByItem(apiRoute, page, pageSize, isPaging, LoggedCompanyID, IsDevelopment);
            _developmentNo.then(function (response) {
                $scope.developmentNoList = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.ChangeLoadDevelopment = function (dataModel) {
            var model = {};
            model.ItemID = dataModel;
            $scope.getFabricDevelopmentDetailByIdInsertion(model);
            debugger
            $scope.LoadDocumentList(model.ItemID, $scope.UserCommonEntity.currentTransactionTypeID);
        }

        //********************************** File Document***************************
        $scope.files = [];
        $scope.getFileDetails = function (e) {
            $scope.$apply(function () {
                /// file validation //////////////
                $scope.file = e.files[0];

                if ($scope.file.size > 200000000) {
                    // alert('file size should not be greater than 200 MB');
                    Command: toastr["warning"]("file size should not be greater than 200 MB!!!!");
                    return;
                }

                var allowed = ["jpeg", "png", "gif", "jpg", "pdf"];
                var found = false;
                //var img;
                //img = new Image();
                allowed.forEach(function (extension) {
                    if ($scope.file.type.match('image/' + extension)) {
                        found = true;
                    }
                    if ($scope.file.type.match('application/' + 'pdf')) {
                        found = true;
                    }
                    //if ($scope.file.type.match('application/' + 'msword')) {
                    //    found = true;
                    //}
                });
                if (!found) {
                    //alert('file type should be .jpeg, .png, .jpg, .gif, .pdf, .doc');
                    Command: toastr["warning"]("file type should be .jpeg, .png, .jpg, .gif, .pdf!!!!");
                    return;
                }

                //// file validation /////////////


                // STORE THE FILE OBJECT IN AN ARRAY.
                for (var i = 0; i < e.files.length; i++) {
                    $scope.files.push(e.files[i])
                }
                var lastOne = $scope.files[$scope.files.length - 1];
                $scope.resultFile = [];
                $scope.resultFile.push(lastOne);
                console.log($scope.resultFile);
            });
        };
        $scope.AddDocument = function () {
            var document = {};
            angular.forEach($scope.resultFile, function (value, key) {
                var arrayName = value.name.split('.');
                document.DocumentID = 0;
                document.DocumentPahtID = 0;
                document.DocName = $scope.txtDocName;
                document.DocumentName = value.name;
                document.TransactionID = $scope.ItemID;
                document.TransactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
                document.CompanyID = LoggedCompanyID;
                document.IsDeleted = false;
                document.CreateBy = LoggedUserID;
            });
            $scope.DocumentList.push(document);
            $scope.txtDocName = "";
            $scope.resultFile = [];
            $("#file").val('');
        };
        $scope.LoadDocumentList = function (itemID, transactionID) {
            $scope.DocumentList = [];
            debugger
            var apiRoute = baseUrl + 'GetDoclistByItemID/';
            var finishGood = FinishGoodSerivce.getYarnByID(apiRoute, itemID, transactionID);
            finishGood.then(function (response) {
                debugger
                $scope.DocumentList = response.data;
            }, function (error) {
                console.log("Error: " + error);
            });
        };
        $scope.deleteDocument = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.DocName + '. Are you sure?');
            if (IsConf) {
                angular.forEach($scope.DocumentList, function (value, key) {
                    if (value.DocName == dataModel.DocName) {
                        value.IsDeleted = true;
                    }
                });
            }
        }

        //********************************************** Item Modal Code *****************************************
        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;
        $scope.getListItemMaster = function (model) {
            var ItemID = model.ItemID;
            var ItemName = model.ItemName;
            $scope.ItemID = model.ItemID;
            $scope.ArticleNo = ItemName;
            $scope.drpDevelop = ItemName;
            $scope.ChangeLoadDevelopment($scope.ItemID);

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
            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            // else {
            // For Loading modal
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
                //onRegisterApi: function (gridApi) {
                //    $scope.gridApi = gridApi;
                //},
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

            // $scope.listItemMaster = [];
            //var groupID = $scope.lstSampleNoList;
            //debugger
            //// if (groupID > 0) {
            //if (groupID == null || groupID == "" || groupID == undefined) {
            //    groupID = 0;
            //}

            var apiRoute = '/SystemCommon/api/PublicApi/' + 'GetItemMasterDeveloped/';
            var listItemMaster = PublicService.getItemMasterService(apiRoute, objcmnParam);
            listItemMaster.then(function (response) {
                //$scope.listItemMaster = response.data;
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });

        };


    }]);
function modal_fadeOut() {
    $("#ModalConsumption").fadeOut(200, function () {
        $('#ModalConsumption').modal('hide');
    });
}

function modal_fadeOut() {
    $("#WarpYarnSetUpModal").fadeOut(200, function () {
        $('#WarpYarnSetUpModal').modal('hide');
    });
}