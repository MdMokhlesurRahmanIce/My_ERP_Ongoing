/**
 * UserTypeCtrl.js
 */

//app.controller('userTypeCtrl', function ($scope, crudService, $filter) {

app.controller('userTypeCtrl', ['$scope', 'crudService', 'uiGridConstants',
    function ($scope, crudService, uiGridConstants) {

        $scope.gridOptionsUsersType = [];
        var objcmnParam = {};

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        var LUserID = $('#hUserID').val();
        var LCompanyID = $('#hCompanyID').val();
        $scope.UserTypeID = 0;

        $scope.PanelTitle = 'Create New Type';
        $scope.DataPanelTitle = 'User Type';

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
                $scope.loadRecordsUser(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadRecordsUser(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadRecordsUser(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadRecordsUser(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadRecordsUser(1);
                }
            }
        };

        //**********----Get All Record----***************
        function loadRecords_type(isPaging) {

            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: $scope.pagination.pageNumber,
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LUserID,
                loggedCompany: LCompanyID,
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

            $scope.gridOptionsUsersType = {
                columnDefs: [
                    { name: "UserTypeID", displayName: "User TypeID", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserTypeName", title: "User TypeName", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '7%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                                            '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                                '<i class="icon-pencil" ng-click="grid.appScope.getUserType(row.entity)"></i>' +
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
                exporterCsvFilename: 'UsersType.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "User Type", style: 'headerStyle' },
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

            var apiRoute = '/SystemCommon/api/User/GetUserType/';
            var listAllUserType = crudService.getAllUsersType(apiRoute, objcmnParam);
            listAllUserType.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsUsersType.data = response.data.listUsersType;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_type(0);

        //**********----Create New Record----***************
        $scope.save = function () {
            UserType = {
                CompanyID: LCompanyID,
                LoggedUser: LUserID,

                UserTypeID: $scope.UserTypeID,
                UserTypeName: $scope.UserTypeName,
                ParentID: 1
            };

            isExisting = $scope.UserTypeID;
            if (isExisting === 0) {
                var apiRoute = '/SystemCommon/api/User/SaveUserType/';
                var CreateUserType = crudService.post(apiRoute, UserType);
                CreateUserType.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("Type Saved Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Creating User!!!!"); }

                    loadRecords_type(0);
                    $scope.clear();
                    modal_fadeOut();
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                var apiRoute = '/SystemCommon/api/User/UpdateUserType/';
                var UpdateUserType = crudService.put(apiRoute, UserType);
                UpdateUserType.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("Type Updated Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Creating User!!!!"); }

                    loadRecords_type(0);
                    $scope.clear();
                    modal_fadeOut();

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        };

        //******=========Get Single Data=========******
        $scope.getUserType = function (dataModel) {
            debugger
            $scope.PanelTitle = 'Update Type';
            $scope.UserTypeID = dataModel.UserTypeID
            $scope.UserTypeName = dataModel.UserTypeName
        };

        //**********----Delete Single Record----***************
        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.UserTypeName + '. Are you sure?');
            if (IsConf) {
                var apiRoute = '/SystemCommon/api/User/DeleteUserType/' + dataModel.UserTypeID + '/' + LCompanyID + '/' + LUserID;
                var UserTypeDelete = crudService.delete(apiRoute);
                UserTypeDelete.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("Type Deleted Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Deleting!!!!"); }

                    loadRecords_type(0);
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        $scope.clear = function () {
            $scope.PanelTitle = 'Create New Type';
            $scope.UserTypeID = 0;
            $scope.UserTypeName = '';
        }
    }]);

function modal_fadeOut() {
    $("#userModal").fadeOut(200, function () {
        $('#userModal').modal('hide');
    });
}