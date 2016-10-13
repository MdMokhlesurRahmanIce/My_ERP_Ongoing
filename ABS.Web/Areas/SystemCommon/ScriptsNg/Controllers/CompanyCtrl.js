/**
 * CustomerCtrl.js
 */
app.controller('companyCtrl', ['$scope', 'companyService', '$localStorage', '$rootScope', '$window',
function ($scope, companyService, $localStorage, $rootScope, $window) {

    $scope.gridOptionsCompany = [];
    var objcmnParam = {};
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;
    var objcmnParam = {};
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;

    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();

    $scope.btnSaveUpdateText = "Save";
    $scope.PanelTitle = 'Create Company';
    $scope.ModalTitle = 'Create Company';
    $scope.DataPanelTitle = 'Data Company';
    $scope.CompID = 0;


    $scope.MenuID = $localStorage.MenuID;
    //************ Load User Common Entity **********************


    //function loadUserCommonEntity(num) {
    //    var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
    //    console.clear();
    //    $scope.UserCommonEntity = {}
    //    $scope.UserCommonEntity = pagedata;
    //    //$scope.permissionPageVisibility = true;
    //    //$scope.generateSecurityParam = {};
    //    //$scope.generateSecurityParam.MenuID = $localStorage.currentMenuID;
    //    //$scope.generateSecurityParam.CompanyID = $('#hCompanyID').val();
    //    ////Coming from SideNavCrl
    //    //$scope.UserCommonEntity = {};
    //    //$scope.UserCommonEntity.loggedCompnyID = $('#hCompanyID').val();
    //    //$scope.UserCommonEntity.loggedUserID = $('#hUserID').val();
    //    //$scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
    //    //$scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
    //    //$scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
    //    //$scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
    //    //$scope.UserCommonEntity.MenuList = $localStorage.MenuList;
    //    //$scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
    //    //angular.forEach($scope.UserCommonEntity.ChildMenues, function (value, key) {
    //    //    if (value.MenuID == $scope.UserCommonEntity.currentMenuID) {
    //    //        $scope.UserCommonEntity.EnableView = value.EnableView;
    //    //        $scope.UserCommonEntity.EnableInsert = value.EnableInsert;
    //    //        $scope.UserCommonEntity.EnableUpdate = value.EnableUpdate;
    //    //        $scope.UserCommonEntity.EnableDelete = value.EnableDelete;
    //    //    }
    //    //});
    //    console.log($scope.UserCommonEntity);
    //}
    //loadUserCommonEntity(0);

    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear(); 
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata; 
        console.log($scope.UserCommonEntity);
    }
    loadUserCommonEntity(0);

    $scope.generateSecurityParam = {};
    $scope.permissionPageVisibility = true;
    $scope.generateSecurityParam.MenuID = $scope.UserCommonEntity.currentMenuID;
    $scope.generateSecurityParam.CompanyID = $scope.UserCommonEntity.loggedCompnyID;
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
    function loadRecords_company(isPaging) {
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

        $scope.gridOptionsCompany = {
            columnDefs: [
                { name: "CustomCode", displayName: "Custom Code", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "CompanyShortName", title: "Company ShortName", headerCellClass: $scope.highlightFilteredHeader },
                { name: "CompanyName", title: "Company Name", headerCellClass: $scope.highlightFilteredHeader },
                {
                    name: 'Edit',
                    displayName: "Edit",
                    width: '5%',
                    enableColumnResizing: false,
                    enableFiltering: false,
                    enableSorting: false,
                    headerCellClass: $scope.highlightFilteredHeader,
                    cellTemplate: '<span class="label label-warning label-mini">' +
                                        '<a ng-href="#NewCompanyModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                            '<i class="icon-pencil" ng-click="grid.appScope.getCompanyForEdit(row.entity)"></i>' +
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
            exporterCsvFilename: 'Company.csv',
            exporterPdfDefaultStyle: { fontSize: 9 },
            exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "Company List", style: 'headerStyle' },
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


        //var apiRoute = '/SystemCommon/api/Company/GetCompanies/';
        //var listCompany = companyService.GetCompanies(apiRoute, page, pageSize, isPaging);
        //listCompany.then(function (response) {
        //    $scope.pagination.totalItems = response.data.recordsTotal;
        //    $scope.gridOptionsCompany.data = response.data.objListCompanies;
        //    $scope.loaderMore = false;
        //},
        //function (error) {
        //     ("Error: " + error);
        //});

        //Get Method Permission

        $scope.generateSecurityParam.methodtype = 'get';
        var headerToken = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

        //Service call
        var apiRoute = '/SystemCommon/api/Company/GetCompanies/';
        var cust = companyService.getToken(apiRoute, page, pageSize, isPaging, headerToken);
        cust.then(function (response) {

            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsCompany.data = response.data.objListCompanies;
            $scope.loaderMore = false;
        },
        function (error) {
            switch (error.status) {
                case 401:
                    $scope.permissionPageVisibility = false;
                    Command: toastr["warning"]("Unauthorized access");
                    $scope.authresult = 'Limited access on retrive data(Unauthorized) : HTTP-' + error.status;
                    $scope.resultStyle = { "color": "red" };
                    break;
                default:
                    $scope.authresult = error;
            }
        });
        //Get Method Permission
        //Service call End


    }
    loadRecords_company(0);

    //**********----Get Single Record----***************
    $scope.getCompanyForEdit = function (dataModel) {

        $scope.ModalTitle = 'Udate Company ' + dataModel.CustomCode;
        var id = '';
        if ('TransactionID' in dataModel) {
            id = dataModel.TransactionID;
        }
        else {
            id = dataModel.CompanyID;
        }

        //var apiRoute = '/SystemCommon/api/Company/GetCompanyByID/' + id;
        //var singleCompany = companyService.getCompanyByID(apiRoute);
        //singleCompany.then(function (response) {
        //    $scope.CompID = response.data.CompanyID
        //    $scope.CompName = response.data.CompanyName
        //    $scope.CompSName = response.data.CompanyShortName;
        //    $scope.CompCode = response.data.CustomCode;
        //    $scope.btnSaveUpdateText = "Update";
        //},
        //function (error) {
        //     ("Error: " + error);
        //});


        $scope.generateSecurityParam.methodtype = 'put';
        var headerToken = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

        //Service call
        var apiRoute = '/SystemCommon/api/Company/GetCompanyByID/' + id;
        var singleCompany = companyService.getCompanyByID(apiRoute, headerToken);
        singleCompany.then(function (response) {
            $scope.CompID = response.data.CompanyID
            $scope.CompName = response.data.CompanyName
            $scope.CompSName = response.data.CompanyShortName;
            $scope.CompCode = response.data.CustomCode;
            $scope.btnSaveUpdateText = "Update";
        },
        function (error) {
            //alert(error);
            //alert(error.status);
            switch (error.status) {
                case 401:
                    Command: toastr["warning"]("Unauthorized access");
                    $scope.authresult = 'Limited access on retrive data(Unauthorized) : HTTP-' + error.status;
                    $scope.resultStyle = { "color": "red" };
                    break;
                default:
                    $scope.authresult = error;
            }
        });


    };

    //**********----Create New Record----***************
    $scope.save = function () {
        var Company = {
            CompanyID: $scope.CompID,
            CompanyName: $scope.CompName,
            CustomCode: $scope.CompCode,
            CompanyShortName: $scope.CompSName,
            StatusID: 1,
            MenuID: $scope.MenuID
        };

        isExisting = $scope.CompID;
        if (isExisting === 0) {
            //var apiRoute = '/SystemCommon/api/Company/savecompanyparam/';
            //var CustomerCreate = companyService.postModels(apiRoute, Company, $scope.UserCommonEntity);
            //CustomerCreate.then(function (response) {
            //    //Manipulate Message based on response Data
            //    if (response.data.result == 1) {
            //        response.data = 1;
            //    }
            //    else if (response.data.result == 0) {
            //        response.data = 0;
            //    } else {
            //        response.data = 0;
            //    }
            //    ShowCustomToastrMessage(response);
            //    loadRecords_company(0);
            //    $scope.clear();
            //    modal_fadeOut_Company();
            //},
            //function (error) {
            //     ("Error: " + error);
            //});


            $scope.generateSecurityParam.methodtype = 'post';
            var headerToken = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

            //Service call
            var apiRoute = '/SystemCommon/api/Company/savecompanyparam/';
            var CustomerCreate = companyService.postModels(apiRoute, Company, $scope.UserCommonEntity, headerToken);
            CustomerCreate.then(function (response) {
                if (response.data.result == 1) {
                    response.data = 1;
                }
                else if (response.data.result == 0) {
                    response.data = 0;
                } else {
                    response.data = 0;
                }
                ShowCustomToastrMessage(response);
                loadRecords_company(0);
                $scope.clear();
                modal_fadeOut_Company();
            },
            function (error) {
                switch (error.status) {
                    case 401:
                        Command: toastr["warning"]("Unauthorized access");
                        $scope.authresult = 'Limited access on retrive data(Unauthorized) : HTTP-' + error.status;
                        $scope.resultStyle = { "color": "red" };
                        break;
                    default:
                        $scope.authresult = error;
                }
            });

        }
        else {
            //    var apiRoute = '/SystemCommon/api/Company/UpdateCompany/';
            //    var CompanyUpdate = companyService.postModels(apiRoute, Company, $scope.UserCommonEntity);
            //    CompanyUpdate.then(function (response) {
            //         (response.data);
            //        response.data = -102;
            //        ShowCustomToastrMessage(response);
            //        loadRecords_company(0);
            //        $scope.clear();
            //        modal_fadeOut_Company();
            //    },
            //    function (error) {
            //         ("Error: " + error);
            //    });
            $scope.generateSecurityParam.methodtype = 'put';
            var headerToken = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

            //Service call
            var apiRoute = '/SystemCommon/api/Company/UpdateCompany/';
            var CompanyUpdate = companyService.postModels(apiRoute, Company, $scope.UserCommonEntity, headerToken);
            CompanyUpdate.then(function (response) {
                response.data = -102;
                ShowCustomToastrMessage(response);
                loadRecords_company(0);
                $scope.clear();
                modal_fadeOut_Company();
            },
        function (error) {
            switch (error.status) {
                case 401:
                    Command: toastr["warning"]("Unauthorized access");
                    $scope.authresult = 'Limited access on retrive data(Unauthorized) : HTTP-' + error.status;
                    $scope.resultStyle = { "color": "red" };
                    break;
                default:
                    $scope.authresult = error;
            }
        });

        }



    };

    //**********----Delete Single Record----***************

    $scope.delete = function (dataModel) {

        var IsConf = confirm('You are about to delete ' + dataModel.CompanyName + '. Are you sure?');
        if (IsConf) {
            var deletedID = dataModel.CompanyID;
            //var apiRoute = '/SystemCommon/api/Company/DeleteCompany/';
            //var CompanyDelete = companyService.postModels(apiRoute, deletedID, $scope.UserCommonEntity);
            //CompanyDelete.then(function (response) {
            //    if (response.data > 0)
            //        response.data = -101;
            //    ShowCustomToastrMessage(response);
            //    loadRecords_company(0);
            //    $scope.clear();
            //}, function (error) {
            //     ("Error: " + error);
            //});

            $scope.generateSecurityParam.methodtype = 'delete';
            var headerToken = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

            //Service call
            var apiRoute = '/SystemCommon/api/Company/DeleteCompany/';
            var CompanyDelete = companyService.postModels(apiRoute, deletedID, $scope.UserCommonEntity, headerToken);
            CompanyDelete.then(function (response) {
                if (response.data > 0)
                    response.data = -101;
                ShowCustomToastrMessage(response);
                loadRecords_company(0);
                $scope.clear();
            },
        function (error) {
            switch (error.status) {
                case 401:
                    Command: toastr["warning"]("Unauthorized access");
                    $scope.authresult = 'Limited access on retrive data(Unauthorized) : HTTP-' + error.status;
                    $scope.resultStyle = { "color": "red" };
                    break;
                default:
                    $scope.authresult = error;
            }
        });
        }
    }

    //**********----Reset Record----***************
    $scope.clear = function () {
        $scope.CompID = 0;
        $scope.CompName = '';
        $scope.CompCode = '';
        $scope.CompSName = '';
        $scope.btnSaveUpdateText = "Save";
        $scope.ModalTitle = 'Create Company';
    };

    //**************************** Approve Notification ********************************
    var ApprovalModel = $localStorage.notificationStorageModel;
    var ApprovalMenuID = $localStorage.notificationStorageMenuID;

    //IsApproval Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
    var IsApproval = $localStorage.notificationStorageIsApproved;

    //IsDelaine Allowed When User go through Notifiction bar .It will be false when user navigate with side bar menu
    var IsDelaine = $localStorage.notificationStorageIsDeclained;
    try {
        $scope.APModalPageTitle = ApprovalModel.CustomCode;
        $scope.DCModalPageTitle = ApprovalModel.CustomCode;
    }
    catch (e) {

    }

    $scope.IsApproved = IsApproval;
    $scope.IsDelained = IsDelaine;

    //Page Display will be false after execution of approved/declined event
    $scope.PageDisplay = true;

    if ($scope.IsApproved) {
        $scope.getCompanyForEdit(ApprovalModel);
        modal_fadeIn_Company();
    }

    //Approved Or Declained Operation
    $scope.ApprovedMethod = function () {

        ApprovalModel.Comments = $scope.commentsModle;
        ApprovalModel.CreatorID = $('#hUserID').val();
        ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
        ApprovalModel.LoggedUserID = $('#hUserID').val();
        $scope.commentsModle = "";
        modal_fadeOut();
        var apiRoute = '/SystemCommon/api/SystemCommonLayout/ApproveNotification/';
        var approvalProcess = companyService.post(apiRoute, ApprovalModel);
        approvalProcess.then(function (response) {
            if (response.data == 200) {
                //Hide Form
                $scope.PageDisplay = false;
                ShowCustomToastrMessage(response);
                modal_fadeOut_Company();
            }
        },
        function (error) {
            ("Error: " + error);
        });
    }

    $scope.DeclinedMethod = function () {
        ApprovalModel.Comments = $scope.commentsModle;
        ApprovalModel.CreatorID = $('#hUserID').val();
        ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
        ApprovalModel.LoggedUserID = $('#hUserID').val();
        $scope.commentsModle = "";
        modal_fadeOutDeclained();
        var apiRoute = '/SystemCommon/api/SystemCommonLayout/DeclainedNotification/';
        var declaineProcess = companyService.post(apiRoute, ApprovalModel);
        declaineProcess.then(function (response) {
            if (response.data == 201) {
                //Hide Form
                $scope.PageDisplay = false;
                ShowCustomToastrMessage(response);
                modal_fadeOut_Company();
            }
        },
        function (error) {
            ("Error: " + error);
        });
    }
}]);

function modal_fadeOut() {
    $("#approveNotificationModal").fadeOut(200, function () {
        $('#approveNotificationModal').modal('hide');
    });
}
function modal_fadeOutDeclained() {
    $("#declainedNotificationModal").fadeOut(200, function () {
        $('#declainedNotificationModal').modal('hide');
    });
}
function modal_fadeOut_Company() {
    $("#NewCompanyModal").fadeOut(200, function () {
        $('#NewCompanyModal').modal('hide');
    });
}
function modal_fadeIn_Company() {
    $("#NewCompanyModal").fadeOut(200, function () {
        $('#NewCompanyModal').modal('show');
    });
}