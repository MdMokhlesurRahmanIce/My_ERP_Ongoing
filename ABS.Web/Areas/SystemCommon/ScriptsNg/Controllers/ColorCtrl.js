/**
 * ColorCtrl.js
 */
app.controller('colorCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/SystemCommon/api/Color/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsColor = [];
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.btnSaveText = "Save";
        $scope.PageTitle = 'Create Color';
        $scope.ListTitle = 'Color Records';
        var message = "";
        //***************************************************End Vairiable Initialize***************************************************

        //***************************************************Start Common Task for all**************************************************
        $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
        $scope.cmnParam = function () {
            objcmnParam = conversion.cmnParams($scope.UserCommonEntity);
        }
        //****************************************************End Common Task for all***************************************************

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
                $scope.loadColorRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadColorRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadColorRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadColorRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadColorRecords(1);
                }
            }
        };
        //********************************----Get All Record----***********************
        $scope.loadColorRecords = function (isPaging) {
            // For Loading
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

            $scope.gridOptionsColor = {
                columnDefs: [
                    { name: "ItemColorID", visible: false, headerCellClass: $scope.highlightFilteredHeader },//width: '10%',
                    { name: "CustomCode", displayName: "Custom Code", headerCellClass: $scope.highlightFilteredHeader },//width: '10%',
                    { name: "ColorName", title: "Color Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorCode", title: "Color Code", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '11%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate:
                                        //'<span class="label label-warning label-mini">' +
                                        //    '<a ng-href="#colorModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                        //        '<i class="icon-pencil" ng-click="grid.appScope.getColor(row.entity)"></i>' +
                                        //    '</a>' +
                                        //'</span>' +
                                        '<span class="label label-info label-mini">' +// ng-if="grid.appScope.UserCommonEntity.EnableUpdate"
                                            '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit" ng-href="#colorModal" ng-click="grid.appScope.getColor(row.entity)">' +
                                                '<i class="glyphicon glyphicon-edit" aria-hidden="true">&nbsp;Edit</i>' +
                                            '</a>' +
                                        '</span>' +
                                        '<span class="label label-warning label-mini" style="text-align:center !important" ng-if="grid.appScope.UserCommonEntity.EnableDelete">' +
                                            '<a href="javascript:void(0);" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="bs-tooltip" title="Delete" ng-href="#CmnDeleteModal" ng-click="grid.appScope.loadDelModel(row.entity)">' +
                                                '<i class="glyphicon glyphicon-trash" aria-hidden="true">&nbsp;Delete</i>' +
                                            '</a>' +
                                        '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Color.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Color", style: 'headerStyle' },
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

            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetColor/';
            var listColor = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listColor.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsColor.data = response.data.color;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadColorRecords(1);
        //*****************************----END----******************************

        //***************************----Get Single Record----***********************
        $scope.getColor = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.ItemColorID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetColorById/';
            var singleColor = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            singleColor.then(function (response) {
                $scope.ItemColorID = response.data.objColor.ItemColorID;
                $scope.CustomCode = response.data.objColor.CustomCode;
                $scope.ColorName = response.data.objColor.ColorName;
                $scope.ColorCode = response.data.objColor.ColorCode;
                $scope.btnSaveText = "Update";
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
        //********************************----END----******************************

        //**********----Create New/Update Existing Record----***************
        $scope.save = function () {
            message = $scope.btnSaveText == "Save" ? "Saved" : "Updated";
            $scope.cmnParam();

            var Color = {
                ItemColorID: message == "Saved" ? 0 : $scope.ItemColorID,
                ColorName: $scope.ColorName,
                ColorCode: $scope.ColorCode
            };
            var HeaderTokenPutPost = message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [Color, objcmnParam];
            var apiRoute = baseUrl + 'SaveUpdateColor/';
            var ColorCreateUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            ColorCreateUpdate.then(function (response) {
                if (response.result != "") {
                    $scope.clear();
                    $scope.CustomCode = response.result;
                    Command: toastr["success"]("Data " + message + " Successfully!!!!");
                    $scope.modal_fadeOut();
                }
                else {
                    Command: toastr["warning"]("Data Not " + message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                console.log("Error: " + error);
                Command: toastr["warning"]("Data Not " + message + ", Please Check and Try Again!");
            });
        };
        //********************************----END----******************************

        //********************************----Delete Record----******************************
        $scope.loadDelModel = function (EntityModel) {
            debugger
            $scope.UserCommonEntity.EnableYes = true;
            $scope.UserCommonEntity.EnableConf = false;
            $scope.UserCommonEntity.rowEntity = EntityModel;
            $scope.UserCommonEntity.DelMsgs = "You are about to delete " + EntityModel.ColorName + ". Are you sure?";
        }
        $scope.CmnMethod = function (MethodName, num) {
            debugger
            $scope.deleteUpdate($scope.UserCommonEntity.rowEntity);
        }

        $scope.deleteUpdate = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = dataModel.ItemColorID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateColor/';
            var ColorDeleteUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            ColorDeleteUpdate.then(function (response) {
                if (response.data.result != "") {
                    $scope.clear();
                    Command: toastr["success"](dataModel.ColorName + " has been Deleted Successfully!!!!");                    
                }
                else {
                    Command: toastr["warning"](dataModel.ColorName + " Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                console.log("Error: " + error);
                Command: toastr["warning"](dataModel.ColorName + "Not Deleted, Please Check and Try Again!");
            });
        };
        //********************************----END----******************************

        //***************************----Delete Single Record----*******************
        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.ColorName + '. Are you sure?');
            if (IsConf) {
                var apiRoute = baseUrl + 'DeleteColor/' + dataModel.ItemColorID;
                var ColorDelete = crudService.delete(apiRoute);
                ColorDelete.then(function (response) {
                    $scope.clear();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        //********************************----END----******************************

        $scope.modal_fadeOut = function () {
            $("#colorModal").fadeOut(200, function () {
                $('#colorModal').modal('hide');
            });
        }

        //*************************----Reset Record----*****************************
        $scope.clear = function () {
            var date = new Date();
            $scope.ItemColorID = 0;
            $scope.CustomCode = "";
            $scope.ColorName = "";
            $scope.ColorCode = "";
            $scope.btnSaveText = "Save";
            $scope.pagination.pageNumber = 1;
            $scope.loadColorRecords(1);
        };
        //********************************----END----******************************
    }]);

