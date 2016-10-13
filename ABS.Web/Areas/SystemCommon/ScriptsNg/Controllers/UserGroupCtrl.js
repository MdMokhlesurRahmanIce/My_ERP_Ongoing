/**
 * UserGroupCtrl.js
 */

//app.controller('userGroupCtrl', function ($scope, crudService, $filter) {

app.controller('userGroupCtrl', ['$scope', 'crudService', 'uiGridConstants',
    function ($scope, crudService, uiGridConstants) {

        $scope.gridOptionsUsersGroup = [];
        var objcmnParam = {};

        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;

        var LUserID = $('#hUserID').val();
        var LCompanyID = $('#hCompanyID').val();
        $scope.UserGroupID = 0;

        $scope.PanelTitle = 'Create New Group';
        $scope.DataPanelTitle = 'User Group';

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
        function loadRecords_group(isPaging) {

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

            $scope.gridOptionsUsersGroup = {
                columnDefs: [
                    { name: "UserGroupID", displayName: "User GroupID", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GroupName", title: "Group Name", headerCellClass: $scope.highlightFilteredHeader },
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
                                                '<i class="icon-pencil" ng-click="grid.appScope.getUserGroup(row.entity)"></i>' +
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
                exporterCsvFilename: 'UsersGroup.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "User Group", style: 'headerStyle' },
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

            var apiRoute = '/SystemCommon/api/User/GetUserGroup/';
            var listAllUser = crudService.getAllUsersGroup(apiRoute, objcmnParam);
            listAllUser.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsUsersGroup.data = response.data.listUsersGroup;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_group(0);

        //**********----Create New Record----***************
        $scope.save = function () {
            debugger
            UserGroup = {
                CompanyID: LCompanyID,
                LoggedUser: LUserID,

                UserGroupID: $scope.UserGroupID,
                GroupName: $scope.GroupName,
                Sequence: 1
            };

            isExisting = $scope.UserGroupID;
            if (isExisting === 0) {
                var apiRoute = '/SystemCommon/api/User/SaveUserGroup/';
                var CreateUserGroup = crudService.post(apiRoute, UserGroup);
                CreateUserGroup.then(function (response) {

                    if (response.data === 1) { Command: toastr["info"]("Group Saved Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Creating Group!!!!"); }

                    loadRecords_group(0);
                    $scope.clear();
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                var apiRoute = '/SystemCommon/api/User/UpdateUserGroup/';
                var UpdateUserType = crudService.put(apiRoute, UserGroup);
                UpdateUserType.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("Group Updated Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Updating Group!!!!"); }

                    modal_fadeOut();
                    loadRecords_group(0);
                    $scope.clear();
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        };

        //******=========Get Single Data=========******
        $scope.getUserGroup = function (dataModel) {
            debugger
            $scope.PanelTitle = 'Update Group';
            $scope.UserGroupID = dataModel.UserGroupID
            $scope.GroupName = dataModel.GroupName
        };

        //**********----Delete Single Record----***************
        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.GroupName + '. Are you sure?');
            if (IsConf) {
                var apiRoute = '/SystemCommon/api/User/DeleteUserGroup/' + dataModel.UserGroupID + '/' + LCompanyID + '/' + LUserID;
                var GroupDelete = crudService.delete(apiRoute);
                GroupDelete.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("Group Deleted Successfully!!!!"); }
                    else { Command: toastr["error"]("Error while Deleting!!!!"); }
                    loadRecords_group(0);
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.clear = function () {
            $scope.PanelTitle = 'Create New Group';
            $scope.UserGroupID = 0;
            $scope.GroupName = '';
        }
    }]);


function modal_fadeOut() {
    $("#userModal").fadeOut(200, function () {
        $('#userModal').modal('hide');
    });
}