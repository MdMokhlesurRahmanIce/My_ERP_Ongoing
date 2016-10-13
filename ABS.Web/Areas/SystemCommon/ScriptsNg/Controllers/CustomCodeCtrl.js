
app.controller('CustomCodeCtrl', function ($scope, $http, customCodeService, commonComboBoxGetDataService) {

    $scope.gridOptionsCustomCodeList = [];
    var objcmnParam = {};

    //Default Parameters Begin
    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();

    var isExisting = 0;
    var page = 1;
    var pageSize = 1000;
    var isPaging = 0;
    var totalData = 0;

    //List Declaration
    $scope.ListCompany = [];
    $scope.ListMenues = [];
    $scope.ListBranch = [];

    //Depended Events 
    //1 AddCustomCodeDetails(Add Button Push Operation) 
    //2 using Details In Save Envents
    //3 clearDetails Assign null 
    //4 EditCustomCode
    //5 Delete Details

    $scope.ListCustomCodeDetails = [];

    //Depended Events On Load 
    $scope.ListCustomCodeList = [];

    //Default Parameters End
    //Load Default Index param in scope Begin 
    $scope.ToogleDiv = 0;
    $scope.ToogleShowListButtonName = "Show List";

    //Depended Events Edit Clear
    $scope.RecordID = 0;
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create Custom Code';
    $scope.ListTitle = 'Records';
    //PageLoad
    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
        console.log($scope.UserCommonEntity);
    }
    loadUserCommonEntity(0);
    //Load Default Index param in scope End
    //**********----Get Mennues DropDown On Page Load----***************
    function loadRecords_Menues(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetMenues/';
        var processMenues = commonComboBoxGetDataService.GetAll(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
        processMenues.then(function (response) {
            $scope.ListMenues = response.data  //Set Default 
            $("#menuDropDown").select2("data", { id: $scope.ListMenues[0].MenuID, text: $scope.ListMenues[0].MenuName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Menues(0);

    //**********----Get Company DropDown On Page Load----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = commonComboBoxGetDataService.GetAll(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data  //Set Default 
            $("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);

    //**********----Get Branch DropDown On Page Load----***************
    function loadRecords_Branch(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetBranch/';
        var processBranch = commonComboBoxGetDataService.GetAll(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
        processBranch.then(function (response) {
            $scope.ListBranch = response.data  //Set Default 
            $("#BranchDropDown").select2("data", { id: $scope.ListBranch[0].OrganogramID, text: $scope.ListBranch[0].OrganogramName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Branch(0);

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
    //**********----Get Custom Code List----***************
    function loadRecords_CustomCodeList(isPaging) {
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

        $scope.gridOptionsCustomCodeList = {
            columnDefs: [
                { name: "CompanyName", displayName: "Company Name", headerCellClass: $scope.highlightFilteredHeader },
                { name: "MenuName", title: "Menu Name", headerCellClass: $scope.highlightFilteredHeader },
                { name: "OrganogramName", title: "Branch", headerCellClass: $scope.highlightFilteredHeader },
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
                                            '<i class="icon-pencil" ng-click="grid.appScope.EditCustomCode(row.entity)"></i>' +
                                        '</a>' +
                                    '</span>' +
                                    '<span class="label label-danger label-mini">' +
                                        '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.deleteMasterList(row.entity)">' +
                                            '<i class="icon-trash"></i>' +
                                        '</a>' +
                                    '</span>'
                }
            ],

            enableFiltering: true,
            enableGridMenu: true,
            enableSelectAll: true,
            exporterCsvFilename: 'CustomeCode.csv',
            exporterPdfDefaultStyle: { fontSize: 9 },
            exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "Custome Code List", style: 'headerStyle' },
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

        var apiRoute = '/SystemCommon/api/CustomCode/GetAllCustomCode/';
        var processCustomCode = customCodeService.getAll(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
        processCustomCode.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsCustomCodeList.data = response.data.CustomCodeList;
            $scope.ListCustomCodeList = response.data.CustomCodeList;
            $scope.loaderMore = false;
             

        },
        function (error) {
            console.log("Error: " + error);
        });
        //var apiRoute = '/SystemCommon/api/CustomCode/GetAllCustomCode/';
        //var processMenues = customCodeService.getAll(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
        //processMenues.then(function (response) {
        //    $scope.ListCustomCodeList = response.data  //Set Default 
        //},
        //function (error) {
        //    console.log("Error: " + error);
        //});
    }
    loadRecords_CustomCodeList(0);

    //**********----Add Event----***************
    $scope.AddCustomCodeDetails = function (dataModel) {
        var IsDuplicate = false;
        var list = $scope.ListCustomCodeDetails;
        angular.forEach(list, function (value, key) {
            if (value.ParameterName == dataModel && value.IsDeleted == false) {
                IsDuplicate = true;
            }
        });
        if (!IsDuplicate) {
            var obj = {
                ParameterName: $scope.ddlParameter,
                Sequence: $scope.Sequence,
                Length: $scope.Length,
                Seperator: $scope.Seperator,
                IsDeleted: false
            };
            $scope.ListCustomCodeDetails.push(obj);

        }
        else {
            ShowCustomToastrMessageResult(-1);
        }
        $scope.clearDetailsParameter();
    }

    //******************************Click Event *********************
    //Event Show List 
    $scope.ShowList = function () {
        if ($scope.ToogleDiv == 0) {
            $scope.ToogleShowListButtonName = "Show List";
            $("#rowDetials").show(200);
            $("#cmnCustomCode").show(200);
            $("#rowList").hide(200);
            $scope.ToogleDiv = 1;
        }
        else {
            $scope.ToogleShowListButtonName = "Hide List";
            $("#rowDetials").hide(200);
            $("#cmnCustomCode").hide(200);
            $("#rowList").show(200);
            $scope.ToogleDiv = 0;
        }
    }
    $scope.ShowList();

    //Edit List 
    $scope.EditCustomCode = function (dataModel) {
        //Load Master 
        //$scope.ListCustomCodeList Load On Page Loading
        //debugger
        var list = $scope.ListCustomCodeList;
        angular.forEach(list, function (value, key) {
            if (value.RecordID == dataModel.RecordID) {
                $scope.RecordID = value.RecordID;
                $('#MenuDropDown').select2('val', value.MenuID.toString());
                $scope.ddlMenuMaster = value.MenuID;
                $('#CompanyDropDown').select2('val', value.CompanyID.toString());
                $scope.ddlCompanyMaster = value.CompanyID;
                if (value.OrganogramID == null) {
                    $('#BranchDropDown').select2('val', '-1');
                    $scope.OrganogramID = null;
                }
                else {
                    $('#BranchDropDown').select2('val', value.OrganogramID.toString());
                    $scope.OrganogramID = value.OrganogramID;
                }
                $scope.Prefix = value.Prefix;
                $scope.Suffix = value.Suffix;
                $scope.IsCompany = value.IsCompany;
                $scope.IsOrganogramCode = value.IsOrganogramCode;
            }
        });
        //Load Details By ID
        try {
            var apiRoute = '/SystemCommon/api/CustomCode/GetCustomCodeDetailsByID/';

            var processdetails = customCodeService.getDetailsByID(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging, dataModel.RecordID);
            processdetails.then(function (response) {

                $scope.clearDetails();
                $scope.ListCustomCodeDetails = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        catch (e) {

        }
        $scope.ToogleDiv = 0;
        $scope.ShowList();
        $scope.btnSaveUpdateText = "Update";

    }

    //**********----Save----***************
    $scope.Save = function () {
        //debugger
        var customCodeMaster = {
            RecordID: $scope.RecordID,
            MenuID: $scope.ddlMenuMaster,
            CompanyID: $scope.ddlCompanyMaster,
            OrganogramID: $scope.ddlBranchMaster,
            Prefix: $scope.Prefix,
            Suffix: $scope.Suffix,
            IsCompany: $scope.IsCompany,
            IsOrganogramCode: $scope.IsOrganogramCode
        };
        var customCodeDetails = $scope.ListCustomCodeDetails;
        isExisting = $scope.RecordID;
        if (isExisting === 0) {
            //debugger
            var apiRoute = '/SystemCommon/api/CustomCode/SaveCustomCode/';
            var porcessMasterDetails = customCodeService.postMasterDetail(apiRoute, customCodeMaster, customCodeDetails);
            porcessMasterDetails.then(function (response) {
                //debugger
                if (response.data == 1) {
                    $scope.NewInstance();
                    ShowCustomToastrMessage(response);
                }
                else if (response.data == 0) {
                    ShowCustomToastrMessage(response);
                }
                else {
                    ShowCustomToastrMessage(response);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            var apiRoute = '/SystemCommon/api/CustomCode/SaveCustomCode/';
            var porcessMasterDetails = customCodeService.postMasterDetail(apiRoute, customCodeMaster, customCodeDetails);
            porcessMasterDetails.then(function (response) {
                if (response.data == 1) {
                    response.data = -102;
                    $scope.NewInstance();
                    ShowCustomToastrMessage(response);
                }
                else if (response.data == 0) {
                    ShowCustomToastrMessage(response);
                }
                else {
                    ShowCustomToastrMessage(response);
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //************ Reset Form *******************
    $scope.NewInstance = function () {
        $scope.clearMaster();
        $scope.clearDetailsParameter();
        $scope.clearDetails();
        $scope.ToogleDiv = 0;
        $scope.ShowList();
        $scope.RecordID = 0;
        $scope.IsCompany = false;
        $scope.IsOrganogramCode = false;
        $scope.btnSaveUpdateText = "Save";
        loadRecords_CustomCodeList(0);
    }

    //**********----Clear Details Panel----***************
    $scope.clearDetailsParameter = function () {
        $('#ParameterDropDown').select2('val', '-1');
        $scope.Sequence = '';
        $scope.Length = '';
        $scope.Seperator = '';
    };

    //**********----Clear Master Panel----***************
    $scope.clearMaster = function () {
        $('#MenuDropDown').select2('val', '-1');
        $('#CompanyDropDown').select2('val', '-1');
        $('#BranchDropDown').select2('val', '-1');
        $scope.Prefix = '';
        $scope.Suffix = '';
        $scope.IsCompany = false;
        $scope.IsOrganogramCode = false;
    };

    //**********----Clear Mater Panel----***************
    $scope.clearDetails = function () {
        $scope.ListCustomCodeDetails = [];
    };

    //**********----Delete Details ----***************
    $scope.deleteDetailsList = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.ParameterName + '. Are you sure?');
        if (IsConf) {
            var list = $scope.ListCustomCodeDetails;
            angular.forEach(list, function (value, key) {
                if (value.ParameterName == dataModel.ParameterName) {
                    value.IsDeleted = true;
                }
            });
        }
    }

    //******************* Delete From Master List ********************
    $scope.deleteMasterList = function (dataModel) {
        debugger
        var IsConf = confirm('You are about to delete ' + dataModel.MenuName + '. Are you sure?');
        if (IsConf) {
            var list = $scope.ListCustomCodeList;
            angular.forEach(list, function (value, key) {
                if (value.RecordID == dataModel.RecordID) {
                    value.IsDeleted = true;
                }
            });
            debugger
            var apiRoute = '/SystemCommon/api/CustomCode/DeleteMaster/' + dataModel.RecordID;
            var customCodeDeleteProcess = customCodeService.post(apiRoute);
            customCodeDeleteProcess.then(function (response) {
                debugger
                if (response.data == 1) {
                    response.data = -101;
                    loadRecords_CustomCodeList(0);
                    //$scope.NewInstance();
                    ShowCustomToastrMessage(response);
                }
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }
});