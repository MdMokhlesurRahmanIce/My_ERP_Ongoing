/**
* itemGrouController.js
*/


//app.controller('itemGroupController', function ($scope, ItemGroupService) {

app.controller('userWiseCompanyController', ['$scope', 'UserWiseCompanyService', 'uiGridConstants',
    function ($scope, UserWiseCompanyService, uiGridConstants) {
        debugger
        $scope.gridOptionsItemGroups = [];
        var objcmnParam = {};

        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        var baseUrl = '/SystemCommon/api/ItemGroup/';
        $scope.btnSaveUpdateText = "Save";
        $scope.PageTitle = 'Create Item Group';
        $scope.ListTitle = 'Item Group  Records';
        $scope.MenuID = 0;
        $scope.itemGroupes = {};

        var LoginUserID = $('#hUserID').val();
        var LoginCompanyID = $('#hCompanyID').val();

        $scope.ItemGroupID = 0;
        var parent = "";

        var defaultCompanyID = "";
        function loadCompanyRecords(isPaging) {

            //var apiRoute = baseUrl + 'GetCompany/';
            //var listCompany = crudService.getModel(apiRoute, page, pageSize, isPaging);
            //  var baseUrl = '/Sales/api/PI/';
            var apiRoute = '/Sales/api/PI/GetPICompany/';
            var listCompany = crudService.getUserWiseCompany(apiRoute, LoggedUserID);
            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    if (item.CompanyID == LoggedCompanyID) {
                        defaultCompanyID = item.CompanyID;

                        $scope.lstCompanyList = item.CompanyID;
                        $("#CompanyList").select2("data", { id: item.CompanyID, text: item.CompanyName });
                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCompanyRecords(0);

        function LoadItemTypes(isPaging) {
            var apiRoute = baseUrl + 'GetItemTypes/';
            var itemTypes = ItemGroupService.getAll(apiRoute, page, pageSize, isPaging, LoginCompanyID);
            itemTypes.then(function (response) {
                // 
                $scope.ItemTypes = response.data
                //Set Default 
                // $("#ddlitemtype").select2("data", { id: $scope.ItemTypes[0].ItemTypeID, text: $scope.ItemTypes[0].ItemTypeName });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        LoadItemTypes(0);

        $scope.LoadParentesByItemType = function () {

            var ItemTypeID = $scope.ddlitemtype;
            var apiRoute = baseUrl + 'GetItemParentes/';
            var itemGroupes = ItemGroupService.getItemParentesById(apiRoute, page, pageSize, isPaging, ItemTypeID);
            itemGroupes.then(function (response) {
                $scope.itemGroupes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.save = function () {

            var ItemGroup = {
                ItemGroupID: $scope.ItemGroupID,
                ItemGroupName: $scope.ModGroupName,
                ItemTypeID: $scope.ddlitemtype,
                ParentID: $scope.ddlItemGroup,
                IsActive: $scope.IsActive,
                CompanyID: LoginCompanyID,
                // when save CreateBy Equal CreateBy ang When Update CreateBy Equal UpdatedBy 
                CreateBy: LoginUserID,
                IsDeleted: false
            };

            isExisting = $scope.ItemGroupID;
            if (isExisting == 0) {

                var apiRoute = baseUrl + 'SaveItemGroup/';
                var SaveConsumption = ItemGroupService.post(apiRoute, ItemGroup);
                SaveConsumption.then(function (response) {
                    debugger;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    LoadItemGroups(1);
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                var apiRoute = baseUrl + '/UpdateItemGroup/';
                var CompanyUpdate = ItemGroupService.put(apiRoute, ItemGroup);
                CompanyUpdate.then(function (response) {
                    response.data = -102;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    LoadItemGroups(1);
                },
                function (error) {
                    console.log("Error: " + error);
                });


            }
        }

        //Pagination
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                LoadItemGroups(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    LoadItemGroups(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    LoadItemGroups(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    LoadItemGroups(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    LoadItemGroups(1);
                }
            }
        };

        //*************************************
        //---------Get All Item Group--------//
        //*************************************
        function LoadItemGroups(isPaging) {
            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LoginUserID,
                loggedCompany: LoginCompanyID,
                menuId: 5,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsItemGroups = {
                columnDefs: [
                    { name: "ItemGroupID", displayName: "Group ID", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Type", title: "Type", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemGroupName", title: "Item Group", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Parent", title: "Parent", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IsActive", title: "IsActive", width: '10%', headerCellClass: $scope.highlightFilteredHeader, enableFiltering: false, enableSorting: false, },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '7%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                                       ' <a ng-href="#GroupModal" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.getItemGroupForEdit(row.entity)">' +
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
                exporterCsvFilename: 'ItemGroupsFile.csv',
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

            //Ui Grid Server Call
            var apiRoute = baseUrl + 'GetAllItemGroups/';
            var itemGroup = ItemGroupService.getAllItemGroups(apiRoute, objcmnParam);
            itemGroup.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsItemGroups.data = response.data.itemGroups;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //Ui Grid Page Call
        LoadItemGroups(1);


        $scope.clear = function () {
            $scope.btnSaveUpdateText = "Save";
            LoadItemTypes(0);
            $scope.ddlitemtype = 0;
            $scope.LoadParentesByItemType();

            $("#ddlitemtype").select2("data", { id: 0, text: '--Select--' });
            $("#ddlItemParent").select2("data", { id: 0, text: '--Select--' });
            $scope.ModGroupName = '';
            $("#isActive").prop('checked', false); // Unchecks the box

        }

        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.ItemGroupName + '. Are you sure?');
            if (IsConf) {

                var apiRoute = baseUrl + 'DeleteItemGroup/';
                var deleteConsumption = ItemGroupService.put(apiRoute, dataModel);
                deleteConsumption.then(function (response) {
                    response.data = -101;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    LoadItemGroups(1);
                }, function (error) {
                    console.log("Error: " + error);
                });


            }

        }

        $scope.getItemGroupForEdit = function (dataModel) {

            var apiRoute = baseUrl + 'GetItemGroupById/' + dataModel.ItemGroupID;
            var singleitemGroup = ItemGroupService.getItemGroupByID(apiRoute);
            singleitemGroup.then(function (response) {
                $scope.ItemGroupID = response.data.ItemGroupID
                $scope.ddlitemtype = response.data.Type
                $scope.ModGroupName = response.data.ItemGroupName
                $scope.ddlitemtype = response.data.TypeId
                parent = response.data.Parent
                $scope.LoadParentesByItemType();
                $scope.btnSaveUpdateText = "Update";
                $("#ddlitemtype").select2("data", { id: 0, text: response.data.Type });


                if (parent != null) {

                    $("#ddlItemParent").select2("data", { id: 0, text: parent });
                }
                else {
                    $("#ddlItemParent").select2("data", { id: 0, text: "--Select--" });

                }
                if (response.data.IsActive == "Yes") {
                    $("#isActive").prop('checked', true);
                } else {
                    $("#isActive").prop('checked', false);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }]);