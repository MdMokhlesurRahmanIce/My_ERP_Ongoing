/**
* POCtrl.js   //    
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('ComparativeStatementCtrl', ['$scope', 'ComparativeStatementService', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, ComparativeStatementService, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        $scope.gridOptionsCSMaster = [];
        var objcmnParam = {};

        $scope.gridOptionsPOMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};
        $scope.gridOptionsItemPop = [];
        var objcmnParamItemPop = {};
        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.HPONo = "";
        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";

        var baseUrl = '/Purchase/api/ComparativeStatement/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.CSDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.IsShowD = false;
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'Comparative Statement';
        $scope.ListTitle = 'PO Records';
        $scope.ListTitleGRRMasters = 'PO Information';
        $scope.ListTitleSampleNo = 'Sample Info';
        $scope.ListTitleGRRDeatails = 'Listed Item of PO';
        $scope.PanelTitle = "Quotation Detail Info";
        $scope.ListPODetails = [];
        $scope.ListPODetailsForSearch = [];

        //***************************************** Start Load User Common Entity ****************************************
        $scope.loadUserCommonEntity = function (num) {
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);           
            console.clear();         
            //Coming from SideNavCrl  
            $scope.permissionPageVisibility = true;
            $scope.generateSecurityParam = {};
            $scope.generateSecurityParam.MenuID = $scope.UserCommonEntity.currentMenuID;
            $scope.generateSecurityParam.CompanyID = $scope.UserCommonEntity.loggedCompnyID;

            $scope.HeaderToken = {};
            $scope.generateSecurityParam.methodtype = 'get';
            $scope.HeaderToken.get = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'put';
            $scope.HeaderToken.put = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'post';
            $scope.HeaderToken.post = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'delete';
            $scope.HeaderToken.delete = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            console.log($scope.UserCommonEntity);
            //console.log($scope.HeaderToken.get);
        }
        $scope.loadUserCommonEntity(0);
        //****************************************** End Load User Common Entity *****************************************

        //******=========Single Checkbox=========******
        $scope.checkAll = function (dataModel) {
            var chkQuotation = dataModel.QuotationID;
            var QuotationId = "";
            $scope.CSDetail = [];
            angular.forEach($scope.ListQuotation, function (dataModel) {
                 
                if (dataModel.QuotationID != chkQuotation)
                {
                    dataModel.Selected = false;
                }
                 else
                {
                    dataModel.Selected = true;
                    QuotationId = dataModel.QuotationID;
                    $scope.QuotationId = QuotationId;

                }
               
            });
            var apiRoute = baseUrl + 'GetQuotationInfoDetail/';
            var listQuotation = ComparativeStatementService.getByQuotationId(apiRoute, QuotationId, $scope.UserCommonEntity.loggedCompnyID);
            listQuotation.then(function (response) {
                $scope.CSDetail = response.data;
            },
           function (error) {
               console.log("Error: " + error);
           });
        };



        //**********----Save and Update Comparative Statement and Comparative Statement   Records----***************//
        $scope.save = function (dataModel) {
          
            var NewStringToDate = conversion.getStringToDate($scope.CSDate);
            var CSMaster = {
                CSDate: NewStringToDate,
                RequisitionID: $scope.SPR,
                QuotationID: $scope.QuotationId,
                Description: $scope.Description,             
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                IsDeleted: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                CreateOn: new Date()
            };
          
            var CSDetail = $scope.CSDetail;
            if (typeof (CSDetail) === "undefined")
            {
                Command: toastr["warning"]("Please select one quotation!!!!");
                return;
            }
            var menuID = $scope.UserCommonEntity.currentMenuID;
            var apiRoute = baseUrl + 'SaveUpdateCSMasterNdetails/';
            var CSMasterDetailsCreate = ComparativeStatementService.postMasterDetail(apiRoute, CSMaster, CSDetail, menuID);
            CSMasterDetailsCreate.then(function (response) {
                var result = 0;              
                if (response.data != "") {
                    if (response.data == "Update") {
                        $scope.clear();
                        Command: toastr["success"]("Update  Successfully!!!!");
                    }
                    else if (response.data == "-1") {
                            Command: toastr["warning"]("Save failed!!!!");
                    }
                    else {
                        $scope.clear();
                        $scope.ComparativeStatementNo = response.data;
                        Command: toastr["success"]("Save  Successfully!!!!");
                    }
                }
            },
            function (error) {
                console.log("Error: " + error);
            });

        };


        function loadSPRNo() {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0 //$scope.tTypeID
            };

            //  var apiRoute = '/Inventory/api/Quotation/GetSPRNo/';

            var apiRoute = baseUrl + 'GetSPR/';
            var listSprNo = ComparativeStatementService.GetSPRNo(apiRoute, objcmnParam);
            listSprNo.then(function (response) {
                $scope.listSprNo = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadSPRNo();

        //****************************************** Start Get Quotation Item Detail *****************************************

        $scope.loadQuotationDetailPop_Records =  function (dataModel)
        {

         $scope.QuotaionItemInfo = "";
         var apiRoute = baseUrl + 'GetQuotationInfoDetail/';
         var listQuotation = ComparativeStatementService.getByQuotationId(apiRoute, dataModel.QuotationID, $scope.UserCommonEntity.loggedCompnyID);
         listQuotation.then(function (response) {
         $scope.QuotaionItemInfo  = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });

        }

        //****************************************** Hide Modal Popup *****************************************
        $scope.HidePopUp = function()
        {
            $('#HDOMasterModal').modal('hide');           
        }

        //****************************************** Get Quotation info *****************************************

        $scope.GetQuotationList = function () {
            $scope.ListQuotation = [];
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                menuId: $localStorage.currentMenuID,
                id:  $scope.SPR,
                tTypeId: 0
            };
            var ItemList = "";
            var apiRoute = baseUrl + 'GetQuotationRequisitionID/';
            var RequisitionID = $scope.SPR;        
            $scope.Invalid = true;
            $scope.IsShowD = false;
            if (RequisitionID != "") {
                ItemList = ComparativeStatementService.getQuotationByRequisitionID(apiRoute, objcmnParam);
                ItemList.then(function (response) {
                    $scope.IsShowD = true;
                    $scope.IsHidden = true;
                    $scope.ListQuotation = response.data;

                },
                function (error) {
                    console.log("Error: " + error);
                });
              
            }
        }
        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
                $scope.IsShowD = false;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                loadCSMasterRecords(0);
            }
        }
  
        $scope.clear = function () {             
            var date = new Date();
            $scope.CSDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.Description = "";
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsShowD = false;
            $scope.IsHiddenDetail = true;
            loadSPRNo();
        };


           //Pagination Quotation Master
        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 0, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                this.pageNumber = 1;
                if (this.ddlpageSize == "All") {
                    this.ddlpageSize = $scope.pagination.totalItems;
                }
                else {
                    this.pageSize = this.ddlpageSize;
                }

                loadCSMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadCSMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadCSMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadCSMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadCSMasterRecords(1);
                }
            }
        };

        function loadCSMasterRecords(isPaging) {

            $scope.loaderMore = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";


            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $localStorage.currentMenuID,
                tTypeId: 0
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsCSMaster = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                columnDefs: [
                    { name: "CSID", displayName: "CSID", visible: false, title: "QuotationID", width: '5%', headerCellClass: $scope.highlightFilteredHeader },                   
                    { name: "CSNo", displayName: "CSNo", title: "CSNo", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CSDate", displayName: "CSDate", cellFilter: 'date:"dd-MM-yyyy"', width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Description", displayName: "Description", title: "Description", width: '47%', headerCellClass: $scope.highlightFilteredHeader },                                  
                    {
                        name: 'Action',
                        displayName: "Action",
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '10%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                   '<a href="" title="Edit" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                     '<i class="icon-edit"></i> Edit' +
                                   '</a>' +
                                   '</span>'

                        //cellTemplate: '<span class="label label-warning label-mini">' +
                        //                     '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                        //                         '<i class="icon-pencil" ng-click="grid.appScope.GetReqDetailByRequisitionID(row.entity)"></i>' +
                        //                     '</a>' +
                        //                 '</span>'
                        //+'<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //  '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.DeleteUpdateMasterDetail(row.entity)">' +
                        //    '<i class="glyphicon glyphicon-trash" aria-hidden="true"></i> Delete' +
                        //  '</a>' +
                        //  '</span>'
                    }
                ],
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Quotation.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Quotation", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetCSMaster/';
            var listQuotationMaster = ComparativeStatementService.getQuotationMasterList(apiRoute, objcmnParam);
            listQuotationMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsCSMaster.data = response.data.objCSMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCSMasterRecords(0);


    }]);


