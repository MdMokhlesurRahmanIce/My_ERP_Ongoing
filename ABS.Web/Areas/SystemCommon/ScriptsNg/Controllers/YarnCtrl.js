/*
*    Created By: Shamim Uddin;
*    Create Date: 2-6-2016 (dd-mm-yy); Updated Date: 2-6-2016 (dd-mm-yy);
*    Name: 'YarnController';
*    Type: $scope;
*    Purpose: Yarn  For RND;
*    Service Injected: '$scope', 'FinishGoodSerivce','uiGridConstants','filter';
*/

app.controller('YarnController', ['$scope', 'YarnService', '$filter', 'uiGridConstants',
    function ($scope, YarnService, $filter, uiGridConstants) {

        $scope.gridOptionsYarn = [];
        var objcmnParam = {};

        var baseUrl = '/SystemCommon/api/Yarn/';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        var ItemTypeID = 3;// 3 type is yarn
        $scope.IsShow = true;
        $scope.IsListShow = true;
        $scope.ItemID = 0;


        $scope.btnRawMaterialText = "Save";
        $scope.btnRawMaterialShowText = "New";
        $scope.PageTitle = 'Yarn Info';
        $scope.ListTitle = 'Yarn List';
        // var checkItemCode = false;
        $scope.hiditemName = "";
        $scope.grdShowHide = true;
        $scope.MainForm = false;

        $scope.GetItemGroups = function () {
            var apiRoute = baseUrl + 'GetItemGroups/';
            var itemGroupes = YarnService.getAllItemGroup(apiRoute, page, pageSize, isPaging, ItemTypeID, LoggedCompanyID);
            itemGroupes.then(function (response) {
                $scope.itemGroupes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetItemGroups();

        $scope.GetUnits = function () {
            var apiRoute = baseUrl + 'GetUnits/';
            var Units = YarnService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Units.then(function (response) {
                $scope.Units = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetUnits();

        $scope.GetColors = function () {
            var apiRoute = baseUrl + 'GetColors/';
            var Colors = YarnService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Colors.then(function (response) {
                $scope.Colors = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.GetColors();

        $scope.GetSizes = function () {
            var apiRoute = baseUrl + 'GetSizes/';
            var Sizes = YarnService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Sizes.then(function (response) {
                $scope.Sizes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetSizes();

        $scope.GetBrands = function () {
            var apiRoute = baseUrl + 'GetBrands/';
            var Brands = YarnService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Brands.then(function (response) {
                $scope.Brands = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetBrands();

        $scope.GetModels = function () {
            var apiRoute = baseUrl + 'GetModels/';
            var Models = YarnService.getAll(apiRoute, page, pageSize, isPaging, LoggedCompanyID);
            Models.then(function (response) {
                $scope.Models = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetModels();


        $scope.save = function () {
            isExisting = $scope.ItemID;
            debugger;
            if (isExisting == 0) {

                var Yarn = {
                    ItemTypeID: ItemTypeID,
                    ItemGroupID: $scope.drpItemGroup,
                    ItemName: $scope.txtbxItemName,
                    UOMID: $scope.drpUOMID,
                    ItemColorID: $scope.drpColor,
                    ItemSizeID: $scope.drpSize,
                    ItemBrandID: $scope.drpBrand,
                    ItemModelID: $scope.drpModel,
                    Note: $scope.txtbxNote,
                    Description: $scope.txtDescription,
                    CreateBy: LoggedUserID,
                    CompanyID: LoggedCompanyID,
                    Count: $scope.txtbxCount
                }

                var apiRoute = baseUrl + 'SaveYarn/';
                var saveYarn = YarnService.post(apiRoute, Yarn);
                saveYarn.then(function (response) {
                    if (response.data != "") {
                        var ArtwitUnCode = response.data;
                        var SplitedArtwitUnCode = ArtwitUnCode.split(",");
                        $scope.ArticleNo = SplitedArtwitUnCode[0];
                        $scope.txtbxUniqueCode = SplitedArtwitUnCode[1];
                        response.data = 1;
                        ShowCustomToastrMessage(response);
                        $scope.GetYarnDetails();
                        $scope.clear();
                    }

                }, function (error) {
                    console.log("Error: " + error);
                });
            }

            else {


                var Yarn = {
                    ItemID: $scope.ItemID,
                    ItemTypeID: ItemTypeID,
                    ItemGroupID: $scope.drpItemGroup,
                    ItemName: $scope.txtbxItemName,
                    UOMID: $scope.drpUOMID,
                    ItemColorID: $scope.drpColor,
                    ItemSizeID: $scope.drpSize,
                    ItemBrandID: $scope.drpBrand,
                    ItemModelID: $scope.drpModel,
                    Note: $scope.txtbxNote,
                    Description: $scope.txtDescription,
                    UpdateBy: LoggedUserID,
                    CompanyID: LoggedCompanyID,
                    Count: $scope.txtbxCount
                }
                var apiRoute = baseUrl + '/UpdateYarn/';
                var updateYarn = YarnService.put(apiRoute, Yarn);
                updateYarn.then(function (response) {
                    response.data = -102;
                    ShowCustomToastrMessage(response);
                    $scope.GetYarnDetails();
                    $scope.clear();

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

        }


        $scope.clear = function () {
            //checkItemCode = false;
            $scope.grdShowHide = true;
            $scope.MainForm = false;
            $scope.ItemID = 0;
            $scope.hiditemName = "";
            $scope.btnRawMaterialText = "Save";
            $scope.txtbxItemName = "";
            $scope.txtDescription = "";
            $scope.txtbxNote = "";
            $scope.drpItemGroup,
            $scope.txtbxItemName = "",
            $scope.drpUOMID = "",
            $scope.drpColor = "",
            $scope.drpSize = "",
            $scope.drpBrand = "",
            $scope.drpModel = "",
            $scope.txtbxCount = "";

            $("#drpUOMID").select2("data", { id: 0, text: '--Select--' });
            $("#drpBrand").select2("data", { id: 0, text: '--Select--' });
            $("#drpItemGroup").select2("data", { id: 0, text: '--Select--' });
            $("#drpColor").select2("data", { id: 0, text: '--Select--' });
            $("#drpModel").select2("data", { id: 0, text: '--Select--' });
            $("#drpSize").select2("data", { id: 0, text: '--Select--' });
        }

        $scope.ShowHide = function () {
            $scope.clear();
            if ($scope.btnRawMaterialShowText === "New") {

                $scope.btnRawMaterialShowText = "Show List";
                $scope.grdShowHide = false;
                $scope.MainForm = true;
            }
            else {
                $scope.btnRawMaterialShowText = "New";
                $scope.grdShowHide = true;
                $scope.MainForm = false;
            }
        }



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
                $scope.GetYarnDetails();
            },
            firstPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg = 1
                    $scope.GetYarnDetails();
                }
            },
            nextPageFg: function () {
                if (this.pageNumberFg < this.getTotalPagesFg()) {
                    this.pageNumberFg++;
                    $scope.GetYarnDetails();
                }
            },
            previousPageFg: function () {
                if (this.pageNumberFg > 1) {
                    this.pageNumberFg--;
                    $scope.GetYarnDetails();
                }
            },
            lastPageFg: function () {
                if (this.pageNumberFg >= 1) {
                    this.pageNumberFg = this.getTotalPagesFg();
                    $scope.GetYarnDetails();
                }
            }
        };

        $scope.GetYarnDetails = function () {
            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
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
                    { name: "ArticleNo", displayName: "Article No", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemTypeName", title: "Item Type", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemGroupName", title: "Item Group", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", title: "Item Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Count", title: "Item Count", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", title: "Item Unit", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Color", title: "Item Color", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeName", title: "Item Size", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BrandName", title: "Item Brand", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ModelName", title: "Model", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '5%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                                            '<a ng-href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                                '<i class="icon-pencil" ng-click="grid.appScope.getRawMaterialByID(row.entity)"></i>' +
                                            '</a>' +
                                        '</span>' +
                                        '<span class="label label-danger label-mini">' +
                                            '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                                '<i class="icon-trash"></i>' +
                                            '</a>' +
                                        '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Yarn.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Yarn Type", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetAllYarn/';
            var RowMaterials = YarnService.getAllYarn(apiRoute, objcmnParamFg);
            RowMaterials.then(function (response) {
                $scope.paginationFg.totalItemsFg = response.data.recordsTotal;
                $scope.gridOptionsFg.data = response.data.objFinishGoods;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });


        }
        $scope.GetYarnDetails();

        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.ItemName + '. Are you sure?');
            if (IsConf) {
                var RawMaterial = {
                    ItemID: dataModel.ItemID,
                    DeleteBy: LoggedUserID,
                    CompanyID: LoggedCompanyID
                }
                var apiRoute = baseUrl + 'DeleteYarn/';
                var deleteRowMaterial = YarnService.put(apiRoute, RawMaterial);
                deleteRowMaterial.then(function (response) {
                    response.data = -101;
                    ShowCustomToastrMessage(response);
                    $scope.GetYarnDetails();
                    $scope.clear();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.getRawMaterialByID = function (dataModel) {
            var apiRoute = baseUrl + 'GetYarn/' + dataModel.ItemID;
            var rowMaterial = YarnService.getRawMaterialByID(apiRoute);
            rowMaterial.then(function (response) {

                $scope.btnRawMaterialText = "Update";
                $scope.btnRawMaterialShowText = "Show List";
                $scope.grdShowHide = false;
                $scope.MainForm = true;
                $scope.hiditemName = response.data.ItemName;
                $scope.ItemID = response.data.ItemID;
                $scope.txtbxUniqueCode = response.data.UniqueCode;
                $scope.txtbxNote = response.data.Note;
                $scope.txtbxItemName = response.data.ItemName;
                $scope.ArticleNo = response.data.ArticleNo;
                $scope.drpItemGroup = response.data.ItemGropID;
                $scope.txtDescription = response.data.Description;
                $scope.drpUOMID = response.data.UnitId;
                $scope.drpSize = response.data.SizeId;
                $scope.drpColor = response.data.ColorId;

                $scope.drpBrand = response.data.BrandId;
                $scope.drpModel = response.data.ModelId;
                $scope.txtbxCount = response.data.Count;

                $("#drpItemGroup").select2("data", { id: 0, text: response.data.ItemGroup });
                $("#drpUOMID").select2("data", { id: 0, text: response.data.Unit });
                $("#drpColor").select2("data", { id: 0, text: response.data.Color });
                $("#drpSize").select2("data", { id: 0, text: response.data.Size });
                $("#drpBrand").select2("data", { id: 0, text: response.data.Brand });
                $("#drpModel").select2("data", { id: 0, text: response.data.Model });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //$scope.SetItemName = function () {
        //    ItemName = $("#drpItemGroup option:selected").text();
        //    Count = $scope.txtbxCount;
        //    if (ItemName != "" && Count != undefined) {
        //        $scope.txtbxItemName = ItemName + '' + Count;
        //        $scope.ArticleNo = ItemName + '' + Count;
        //    }
        //    else if (Count == null && ItemName != "") {
        //        $scope.txtbxItemName = "";
        //        $scope.ArticleNo = "";
        //    }
        //}
    }]);
