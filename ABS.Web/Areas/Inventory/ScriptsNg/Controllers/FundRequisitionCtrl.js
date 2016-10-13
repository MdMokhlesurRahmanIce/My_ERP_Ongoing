/**
* PICtrl.js
*/

//app.controller('RequisitionCtrl', function ($scope, FundRequisitionService, conversion) {

app.controller('FundRequisitionCtrl', ['$scope', 'FundRequisitionService', '$filter', 'uiGridConstants', 'conversion', '$localStorage',
    function ($scope, FundRequisitionService, $filter, uiGridConstants, conversion, $localStorage) {

        function loadUserCommonEntity(num) {
            //Coming from SideNavCrl
            $scope.UserCommonEntity = {};
            $scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
            $scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
            $scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
            $scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
            $scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
            $scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
            $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
            $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);

        $scope.gridOptionsRequisition = [];
        var objcmnParam = {};

        var baseUrl = '/Inventory/api/FundRequisition/';
        var isExisting = 0;
        var page = 1;
        var pageSize = 50;
        var isPaging = 0;
        var totalData = 0;
        var ItemTypeID = 1;
        var IsValidQty = false;
        var date = new Date();
        $scope.AvailableDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.EstimatedDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.ListBuyer = [];
        $scope.bool = true;
        $scope.MenuID = 103;
        var LoginUserID = $('#hUserID').val();
        var LoginCompanyID = $('#hCompanyID').val();

        $scope.IssueId = "0";
        $scope.btnIssueSaveText = "Save";
        $scope.btnIssueShowText = "Show Fund List";
        $scope.PageTitle = 'Fund Requisition';
        $scope.ListTitle = 'Issue Records';
        $scope.ListTitleActivePIMasters = 'Issue  Information (Masters)';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleDeatails = 'SPR View';
        $scope.ListTitleRequisitioneatails = 'Issue Item List';       
        $scope.ListTitleQCMasters = 'Issue Information (Masters)';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleQCDeatails = 'Issue Information (Details)';

        $scope.ListPIDetails = [];
        $scope.ListActivePIMaster = [];
        $scope.OverdueInterest = 2;
        $scope.ListRequisitionDetail = [];

        //*************---Show and Hide Order---**********

        //**************************************
        // Show and Hide Order
        //**************************************

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnIssueShowText = "Show Issue List";
                $scope.IsShow = true;
            }
            else {
                $scope.btnIssueShowText = "Hide Issue List";
                $scope.IsShow = false;
                $scope.IsHidden = false;

                loadIssueMasterRecords(0);
            }
        }


        var ListCompany = [];
        function loadRecords_Company(isPaging) {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
            var listCompany = FundRequisitionService.getCompany(apiRoute, page, pageSize, isPaging);
            listCompany.then(function (response) {
                $scope.ListCompany = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Company(0);


        function loadRecords_OrganogramDept(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganogramsDept = FundRequisitionService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
            listOrganogramsDept.then(function (response) {
                $scope.listOrganogramsDepartment = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_OrganogramDept(0);

        //////**********---- Get Requisition No ----***************
        $scope.getRequisition = function () {
            var apiRoute = '/Inventory/api/Issue/GetRequisitionNo/';
            var listRequisition = FundRequisitionService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
            listRequisition.then(function (response) {
                $scope.lstRequisition = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.getRequisition(0);

        //////**********---- Get Bank ----***************
        $scope.getBank = function () {
            var apiRoute = baseUrl + 'GetAllBank/';
            var listBank = FundRequisitionService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
            listBank.then(function (response) {
                $scope.lstBank = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.getBank(0);

        //**********----get Item  Record from datail info ----***************//
        $scope.ListRequisitionDetails = [];

        $scope.getListItemDetails = function () {

            $scope.IsHiddenDetail = false;
            var existItem = $scope.ddlItem;
            var duplicateItem = 0;
            angular.forEach($scope.ListRequisitionDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {

                $scope.ListRequisitionDetails.push({

                    ItemID: $scope.ddlItem,
                    ItemName: $("#ddlItem").select2('data').text,
                    UnitID: $scope.Unit,
                    UnitName: $("#ddlUnit").select2('data').text,
                    LotID: $scope.lstLot,
                    LotNo: $("#ddlLotNo").select2('data').text,
                    BatchID: $scope.Batch,
                    BatchNo: $("#ddlBatch").select2('data').text,
                    Qty: $scope.Qty,
                    UnitPrice: $scope.UnitPrice,
                    Amount: parseFloat($scope.Qty * $scope.UnitPrice)
                });


                $scope.EmptyRequisitionDetail();
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Added!!!!");
            }
            $scope.showDtgrid = $scope.ListRequisitionDetails.length;
        }
        //////**********----delete  Record from ListRequisitionDetails----***************


        var defaultCompanyID = "";

        function loadCompanyRecords(isPaging) {
            var apiRoute = '/Sales/api/PI/GetPICompany/';                   
            var listCompany = FundRequisitionService.getUserWiseCompany(apiRoute, LoginUserID);
            listCompany.then(function (response) {
                $scope.listCompany = response.data;
                angular.forEach($scope.listCompany, function (item) {
                    if (item.CompanyID == LoginCompanyID) {
                        defaultCompanyID = item.CompanyID;
                        $scope.lstCompanyList = item.CompanyID;
                        $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCompanyRecords(0);

        $scope.ListRequisitionDetails = [];
        $scope.deleteRow = function (index) {
            $scope.ListRequisitionDetails.splice(index, 1);
        };

        //////**********---- Get Currency No ----***************
        $scope.getCurrency = function () {
                    
            var apiRoute = '/Inventory/api/Challan/GetCurrency/';
            var listCurrency = FundRequisitionService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.LstCurrency = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.getCurrency(0);

        //////**********---- Get Unit  ----***************
        $scope.getCurrency = function () {

            var apiRoute = '/Inventory/api/StockEntry/GetUnits/';
            var listunit = FundRequisitionService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
            listunit.then(function (response) {
                $scope.LstUnits = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.getCurrency(0);


      
        //////**********----Delete New Record----***************
        $scope.deleteRow = function (index) {
            $scope.ListRequisitionItems.splice(index, 1);
            $scope.showDtgrid = $scope.ListRequisitionItems.length;
        };

        //////**********----Create New Record----***************
        $scope.save = function () {
           
            var AvailableDate = conversion.getDateToString($scope.AvailableDate);
            var EstimatedDate = conversion.getDateToString($scope.EstimatedDate);

            var FundRequisition = {
                FRID : $scope.FRID,
                CompanyID: $scope.Company,
                DepartmentID: $scope.Department,
                FRDate: $scope.FRDate,
                PIID: 1,
                RequisitionID: $scope.SPRNo,
                Purpose: $scope.Purpose,
                ExpanseBreakDown: $scope.Expense,
                FundShouldAvailableOn: AvailableDate,
                NegotiateOrderQty: $scope.OrderQty,
                UnitPrice: $scope.UnitPrice,
                TotalAmount: $scope.TotalValue,
                UnitID: $scope.Unit,
                CurrecncyID: $scope.Currency,
                EstimatedDate: EstimatedDate,
                AvgLeadTime: $scope.LeadTime,
                AdvisingBankID: $scope.AdvisingBank,
                StatusDate: new Date(),
                CompanyID: LoginCompanyID,
                CreateBy: LoginUserID,               
                CreateOn: new Date(),
                IsDeleted: false
            };
            debugger
            var menuID = $scope.MenuID;
            var apiRoute = baseUrl + 'SaveFundRequisition/';
            var FundRequisitionCreateUpdate = FundRequisitionService.postMasterDetail(apiRoute, FundRequisition, menuID);
            FundRequisitionCreateUpdate.then(function (response) {
                if (response.data != "") {
                    //if (response.data == 2) {
                    //    Command: toastr["success"]("Updated  Successfully!!!!");
                    //}
                    //else {
                    $scope.FRNo = response.data;
                    Command: toastr["success"]("Save  Successfully!!!!");
                   // }
                }
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //Pagination Issue Master
        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                //if (this.ddlpageSize > 1) {
                //    this.pageSize = this.ddlpageSize
                //    this.pageNumber = 1
                //    loadActivePIRecords(1);
                //}
                this.pageNumber = 1;
                if (this.ddlpageSize == "All") {
                    this.ddlpageSize = $scope.pagination.totalItems;
                }
                else {
                    this.pageSize = this.ddlpageSize;
                }

                loadIssueMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadIssueMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadIssueMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadIssueMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadIssueMasterRecords(1);
                }
            }
        };
        //-------------- Edit Issue Master-----------------------------//

        $scope.getIssueMasterById = function (dataModel) {

            $scope.btnIssueSaveText = "Update";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.btnIssueShowText = "Show Issue Info";

            var Issueid = '';
            Issueid = dataModel.IssueID;
            $scope.IssueId = dataModel.IssueID;
            var apiRoute = baseUrl + 'GetIssueMasterByIssueId/' + Issueid;
            var ListIssueInfoMaster = FundRequisitionService.getByID(apiRoute);
            ListIssueInfoMaster.then(function (response) {

                $scope.IssueNo = response.data.IssueNo;
                $scope.IssueBy = response.data.IssueBy;
                $scope.RequistionNo = response.data.RequisitionID;
                $scope.Company = response.data.ToCompanyID;
                $scope.ParentDept = response.data.ToDepartmentID;

                $scope.IssueDate = conversion.getDateToString(response.data.IssueDate);
                $("#ddlIssueBy").select2("data", { id: response.data.IssueBy, text: response.data.IssueByName });
                $("#ddlRequisitionNo").select2("data", { id: response.data.RequisitionID, text: response.data.RequisitionNo });
                $("#ddlCompany").select2("data", { id: response.data.ToCompanyID, text: response.data.ToCompany });
                $("#ddlParentDept").select2("data", { id: response.data.ToDepartmentID, text: response.data.ToDepartment });
                $scope.Purpose = response.data.Comments;
                $scope.IsShow = true;

            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShow = true;
            });
        }

        //-------------- Edit Issue Detail-----------------------------//

        $scope.GetIssueDetailByIssueId = function (dataModel) {

            var Issueid = '';
            Issueid = dataModel.IssueID;
            $scope.ListRequisitionItems = [];
            var apiRoute = baseUrl + 'GetIssueDetailByIssueId/' + Issueid;
            var ListIssueDetailInfoMaster = FundRequisitionService.getByID(apiRoute);
            ListIssueDetailInfoMaster.then(function (response) {
                $scope.ListRequisitionItems = response.data;
            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShow = true;
            });
        }

        //**********---- Get All Issue Master Record ----*************** //
        function loadIssueMasterRecords(isPaging) {

            $scope.loaderMoreIssueMaster = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";


            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LoginUserID,
                loggedCompany: LoginCompanyID,
                menuId: 79,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsIssueMaster = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [
                    { name: "IssueID", displayName: "Issue ID", visible: false, title: "Issue ID", width: '5%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IssueNo", displayName: "Issue No", title: "Issue No", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IssueDate", displayName: "Issue Date", cellFilter: 'date:"dd-MM-yyyy"', width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IssueByName", displayName: "IssueBy", title: "IssueBy", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "Requisition No", title: "Requisition No", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '10%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                      '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                            '<i class="glyphicon glyphicon-edit" ng-click="grid.appScope.getIssueMasterById(row.entity); grid.appScope.GetIssueDetailByIssueId(row.entity)">&nbsp;Edit</i>' +
                                        '</a>' +
                                    '</span>'
                        //+'<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //  '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.DeleteUpdateMasterDetail(row.entity)">' +
                        //    '<i class="glyphicon glyphicon-trash" aria-hidden="true"></i> Delete' +
                        //  '</a>' +
                        //  '</span>'
                    }
                ],
                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsIssueMaster.totalItems, pagination.sort)
                    .then(function () {
                        debugger
                        $scope.gridOptionsIssueMaster.useExternalPagination = false;
                        $scope.gridOptionsIssueMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = '';//baseUrl + 'GetIssueMasterList/';
            var listQCMaster = FundRequisitionService.getIssueMasterList(apiRoute, objcmnParam);
            listQCMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsIssueMaster.data = response.data.objVmIssue;
                $scope.loaderMoreIssueMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        loadIssueMasterRecords(0);


      

        ////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.FRID = "0";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnIssueSaveText = "Save";
            $scope.FundRequisition = {};
            $scope.bool = true;
            $scope.FRNo = '';
            $scope.UnitPrice = '';
            $scope.TotalValue = '';
            $scope.RequiredAmount = '';
            $scope.EstimatedDate = $scope.EstimatedDate; 
            $scope.AvailableDate = $scope.AvailableDate;
            $scope.LeadTime = '';
            $scope.Expense = '';
            $scope.OrderQty = '';
            $("#ddlCompany").select2("data", { id: 0, text: '--Select Company--' });
            $("#ddlSPRNo").select2("data", { id: 0, text: '--Select SPR No--' });
            $("#ddlCurrency").select2("data", { id: 0, text: '--Select Currency--' });
            $("#ddlUnit").select2("data", { id: 0, text: '--Select Unit--' });
            $("#ddlPINo").select2("data", { id: 0, text: '--Select PINo--' });
            $("#ddlAdvisingBank").select2("data", { id: 0, text: '--Select Bank--' });
            $scope.RequiredAmount = '';
            var date = new Date();
            $scope.IssueDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.Purpose = '';
            $scope.IsLoan = false;
            $scope.ListRequisitionItems = [];
        };



    }]);



