/*
*    Created By: Shamim Uddin;
*    Create Date: 2-6-2016 (dd-mm-yy); Updated Date: 2-6-2016 (dd-mm-yy);
*    Name: 'InternalIssueController';
*    Type: $scope;
*    Purpose: Internal Issue For Production;
*    Service Injected: '$scope', 'conversion', 'InternalIssueService','localStorage';
*/

app.controller('InternalIssueController', ['$scope', 'InternalIssueService', '$localStorage', 'conversion','PublicService',
function ($scope, InternalIssueService, $localStorage, conversion, PublicService) {
    loadUserCommonEntity(0);
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;
    //-------Base Url of InternalIssue-------
    var baseUrl = '/Production/api/InternalIssue/';
    $scope.btnSaveUpdateText = "Save";
    $scope.btnShowHide = "Create"
    $scope.PageTitle = 'Create Internal Issue';
    $scope.ListTitle = 'Internal Issue  List';
    $scope.MenuID = 0;
    $scope.itemGroupes = {};
    $scope.drpArticalNO = 0;
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.departmentId = $scope.UserCommonEntity.loggedUserBranchID;

    //-------Set Departments-------
    $scope.Departments = [{
        Name: 'Ball Warping',
        ID: 7
    }, {
        Name: 'Dying',
        ID: 8
    }, {
        Name: 'LCB',
        ID: 9
    }];
    //-------Set Contro By Department Wise-------
    $scope.SetControlDepartmentWise = function (department) {
        if (department === $scope.Departments[0].ID)  //------Ball Warping------
        {
            $scope.txtbxDyingIssueDateReadOnly = true;
            $scope.txtbxDyingReceiveDateReadOnly = true;
            $scope.txtbxLCBIssueDateReadOnly = true;
            $scope.txtbxDyingReceiveRemarkReadOnly = true;
            $scope.txtbxLcbRemarkReadOnly = true;
            $scope.txtbxDyingIssueRemarkReadOnly = true;
            $scope.hidSetIssueBySetwise = true;
            $scope.hidformInternalIssue = true;
            $scope.hidInternalIssueDetail = false;
            $scope.btnShowHide = "Create";
        } else if (department === $scope.Departments[1].ID) //------Dying------
        {
            $scope.txtbxBallIssueDateReadOnly = true;
            $scope.txtbxLCBIssueDateReadOnly = true;
            $scope.txtbxLcbRemarkReadOnly = true;
            $scope.txtAreBallRemarkReadOnly = true;
            $scope.hidSetIssueBySetwise = true;
            $scope.hidformInternalIssue = true;
            $scope.hidInternalIssueDetail = false;
            $scope.btnShowlist = true;
        } else if (department === $scope.Departments[2].ID)//------LCB------
        {
            $scope.txtbxBallIssueDateReadOnly = true;
            $scope.txtbxDyingIssueDateReadOnly = true;
            $scope.txtbxDyingReceiveDateReadOnly = true;
            $scope.txtbxDyingReceiveRemarkReadOnly = true;
            $scope.txtAreBallRemarkReadOnly = true;
            $scope.txtbxDyingIssueRemarkReadOnly = true;
            $scope.hidSetIssueBySetwise = true;
            $scope.hidformInternalIssue = true;
            $scope.hidInternalIssueDetail = false;
            $scope.btnShowlist = true;
        }
    }

    $scope.SetControlDepartmentWise($scope.departmentId);
    //-----------Set Defult Date---------------------------
    $scope.SetDefultDate = function () {
        var date = new Date();
        $scope.txtbxBallIssueDate = conversion.NowDateCustom();
        $scope.txtbxDyingIssueDate = conversion.NowDateCustom();
        $scope.txtbxDyingReceiveDate = conversion.NowDateCustom();
        $scope.txtbxLCBIssueDate = conversion.NowDateCustom();
    }
    $scope.SetDefultDate();// Call Defult Date

    //---------Load Application Information By User Wise 
    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
        console.log($scope.UserCommonEntity);
    }



    // *******************************************************************************************************************
    $scope.GetArticals = function () {
        var apiRoute = baseUrl + 'GetArticals/'; // 
        var _artical = InternalIssueService.getAll(apiRoute, page, pageSize, isPaging, LoginCompanyID);
        _artical.then(function (response) {
            $scope.Articals = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetArticals();

    $scope.GetSetNoByArticalNo = function () {
        debugger
        var apiRoute = baseUrl + 'GetSetNoByArticalNo/'; // 
        var _setNo = InternalIssueService.getAllById(apiRoute, page, pageSize, isPaging, $scope.drpArticalNO);
        _setNo.then(function (response) {
            $scope.setNos = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetSetDetailsBySetNo = function () {
        var apiRoute = baseUrl + 'GetSetDetailsBySetNo/'; // 
        var _setdetails = InternalIssueService.getSetDetalById(apiRoute, page, pageSize, isPaging, $scope.drpSetNo);
        _setdetails.then(function (response) {
            $scope.txtbxCount = response.data.YarnCount;
            $scope.txtbxTotalEnds = response.data.TotalEnds;
            $scope.txtbxSupplier = response.data.SupplierName;
            $scope.txtbxRatio = response.data.YarnRatio;
            $scope.txtbxSetLength = response.data.SetLength;
            $scope.txtbxColor = response.data.ColorName;
            $scope.txtbxLeaseRepeat = response.data.LeaseRepeat;
            //-----------Call IssueDetails for Grd---------------

            $scope.GetIssueDetailBySetNO();
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    //------For Ball Issue--------------------------
    $scope.GetIssueDetailBySetNO = function () {
        var apiRoute = baseUrl + 'GetIssueDetailBySetNO/'; // 
        var _setdetails = InternalIssueService.getSetDetalById(apiRoute, page, pageSize, isPaging, $scope.drpSetNo);
        _setdetails.then(function (response) {
            //-----------Call Can No By DepartmentID-----------------
            $scope.GetCanNoByDeapartmentId();
            $scope.issueDetails = response.data;
            if ($scope.departmentId === 7) {
                $scope.BalMRRID = $scope.issueDetails[0].BalMRRID;
                $scope.SetSetDetailGridRopNuAndCan();
            }

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.SetSetDetailGridRopNuAndCan = function () {
        $scope.hidRopeNo = true;
        $scope.hidCanNo = true;
    }
    $scope.GetCanNoByDeapartmentId = function () {
        var DepartmentId = 8//Dying 
        var apiRoute = baseUrl + 'GetCanNoByDeapartmentId/'; // 
        var _setNo = InternalIssueService.GetCanByDepartment(apiRoute, page, pageSize, isPaging, DepartmentId);
        _setNo.then(function (response) {
            $scope.CanNos = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.save = function () {
        var PrdInternalIssue = {

            TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
            ItemID: $scope.drpArticalNO,
            SetID: $scope.drpSetNo,
            DepartmentID: $scope.departmentId,
            //----------Ball Issue--------------
            IsIssuedBall: '',
            IssBallDate: '',
            IssBallRemarks: '',
            BalMRRID: '',
            //-----------Daying Receive-----------
            IsReceivedDy: '',
            ReceivedDyDate: '',
            ReceivedDyRemarks: '',
            //-----------Daying Issue-----------
            IsIssuedDy: '',
            IssDyDate: '',
            IssDyRemarks: '',
            //-----------LCB Issue-----------
            IsReceivedLCB: '',
            ReceivedLCBDate: '',
            ReceivedLCBRemarks: '',

            CompanyID: LoginCompanyID,
            CreateBy: LoginUserID,
            IssueID: $scope.hidIssueId
        }

        if ($scope.departmentId == 7) {
            PrdInternalIssue.BalMRRID = $scope.BalMRRID;
            PrdInternalIssue.IsIssuedBall = true;
            PrdInternalIssue.IssBallDate = conversion.getStringToDate($scope.txtbxBallIssueDate) === "" ? null : conversion.getStringToDate($scope.txtbxBallIssueDate);
            PrdInternalIssue.IssBallRemarks = $scope.txtAreBallRemark;
            PrdInternalIssue.GPL = $scope.txtbxGPL;
            PrdInternalIssue.Shade = $scope.txtbxShade;
        }
        else if ($scope.departmentId == 8) {
            if ($scope.txtbxDyingIssueDate == "") {

                PrdInternalIssue.IsReceivedDy = true;
                PrdInternalIssue.ReceivedDyDate = conversion.getStringToDate($scope.txtbxDyingReceiveDate) === "" ? null : conversion.getStringToDate($scope.txtbxDyingReceiveDate);//conversion.getDateToString($scope.txtbxDyingReceiveDate);
                PrdInternalIssue.ReceivedDyRemarks = $scope.txtDyingReceiveRemark;
            }
            else {
                PrdInternalIssue.IsIssuedDy = true;
                PrdInternalIssue.IssDyDate = conversion.getStringToDate($scope.txtbxDyingIssueDate) === "" ? null : conversion.getStringToDate($scope.txtbxDyingIssueDate);//conversion.getDateToString($scope.txtbxDyingIssueDate);
                PrdInternalIssue.IssDyRemarks = $scope.txtbxDyingIssueRemark;

            }
        }
        else if ($scope.departmentId == 9) {
            PrdInternalIssue.IsReceivedLCB = true;
            PrdInternalIssue.ReceivedLCBDate = conversion.getStringToDate($scope.txtbxLCBIssueDate) === "" ? null : conversion.getStringToDate($scope.txtbxLCBIssueDate);//conversion.getDateToString($scope.txtbxLCBIssueDate);
            PrdInternalIssue.ReceivedLCBRemarks = $scope.txtAreBallRemark;

        }

        var PrdInternalIssueDetail = $scope.issueDetails;
        var apiRoute = baseUrl + 'SaveInternalIssue/';
        var SaveInternalIssue = InternalIssueService.postInternalIssue(apiRoute, PrdInternalIssueDetail, PrdInternalIssue);
        SaveInternalIssue.then(function (response) {
            if (response.data === 1) {
                $scope.SetLoadInternalIssueDetailByDepartmentWise();
                $scope.hidSetIssueBySetwise = true;
                $scope.hidformInternalIssue = true;
                $scope.hidInternalIssueDetail = false;
                $scope.SetControlDepartmentWise($scope.departmentId);
                Command: toastr["info"]("Saved Successfully!!!!");
            }
            else {
                Command: toastr["error"]("Error !!!!");
            }

        }, function (error) {
            console.log("Error: " + error);
        });

    }


    $scope.LoadInternalIssueDetail = function (IsIssuedBall, IsReceivedDy, IsIssuedDy, IsReceivedLCB) {
        var apiRoute = baseUrl + 'GetInternalIssueDetial/'; // 
        var _internalIssueDetail = InternalIssueService.getInternalIssueDetails(apiRoute, page, pageSize, isPaging, LoginCompanyID, IsIssuedBall, IsReceivedDy, IsIssuedDy, IsReceivedLCB);
        _internalIssueDetail.then(function (response) {
            $scope.InternalIssueDetails = response.data;

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.SetLoadInternalIssueDetailByDepartmentWise = function () {

        var IsIssuedBall = false;
        var IsReceivedDy = false;
        var IsIssuedDy = false;
        var IsReceivedLCB = false;

        if ($scope.departmentId == 7 || $scope.departmentId == 8) {
            IsIssuedBall = true;

        } else if ($scope.departmentId == 9) {

            IsIssuedDy = true;
        }

        $scope.LoadInternalIssueDetail(IsIssuedBall, IsReceivedDy, IsIssuedDy, IsReceivedLCB);
    }

    $scope.SetLoadInternalIssueDetailByDepartmentWise();
    debugger
    $scope.showHide = function () {
        //if ($scope.departmentId == 7 ) {
        if ($scope.btnShowHide == "Show List") {
            $scope.ShowList();
        }
        else {
            $scope.HideList();
        }
        //}
    }
    $scope.ShowList = function () {       
        $scope.btnShowHide = "Create";
        $scope.hidSetIssueBySetwise = true;
        $scope.hidformInternalIssue = true;
        $scope.hidInternalIssueDetail = false;
        debugger;
        if ($scope.departmentId === $scope.Departments[1].ID)
        {
            $scope.btnShowlist = true;
        }
        else if ($scope.departmentId === $scope.Departments[2].ID)
        {
            $scope.btnShowlist = true;
        }
    }

    $scope.HideList = function () {
        $scope.btnShowHide = "Show List";
        $scope.hidSetIssueBySetwise = false;
        $scope.hidformInternalIssue = false;
        $scope.hidInternalIssueDetail = true;

    }
    $scope.GetInternalIssueByIssuID = function (dataModel) {

        $scope.GetSetDetailsByIssueID(dataModel.IssueID);
        $scope.setControllerAfterClick();
        $scope.btnShowlist = false;
        $scope.btnShowHide = "Show List";
    }
    $scope.GetSetDetailsByIssueID = function (IssueID) {


        var apiRoute = baseUrl + 'GetSetDetailsByIssueID/'; // 
        var _setdetails = InternalIssueService.getSetDetalByIssueId(apiRoute, page, pageSize, isPaging, IssueID);
        _setdetails.then(function (response) {
            $scope.hidIssueId = response.data.IssueID;
            $scope.txtbxCount = response.data.YarnCount;
            $scope.txtbxTotalEnds = response.data.TotalEnds;
            $scope.txtbxSupplier = response.data.SupplierName;
            $scope.txtbxRatio = response.data.YarnRatio;
            $scope.txtbxSetLength = response.data.SetLength;
            $scope.txtbxColor = response.data.ColorName;
            $scope.txtbxLeaseRepeat = response.data.LeaseRepeat;
            $scope.txtbxBallIssueDate = response.data.IssBallDate;
            $scope.txtbxDyingIssueDate = response.data.IssDyDate;
            $scope.txtbxDyingReceiveDate = response.data.ReceivedDyDate;
            $scope.txtbxLCBIssueDate = response.data.ReceivedLCBDate;
            $scope.txtbxCanRemark = response.data.ReceivedLCBDate;
            $scope.txtAreBallRemark = response.data.IssBallRemark;
            $scope.txtDyingReceiveRemark = response.data.ReceivedDyRemarks;
            $scope.txtbxDyingIssueRemark = response.data.IssuDyRemarks;
            $scope.txtbxLcbRemark = response.data.ReceivedLCBRemarks;
            $scope.GetIssueDetailByIssueID(IssueID);
            $scope.SetDrpDwn(response);
            $scope.GetSetNoByArticalNo();

        },
        function (error) {
            console.log("Error: " + error);
        });

        $scope.hidSetIssueBySetwise = false;
        $scope.hidformInternalIssue = false;
    }
    // For Dying Receive
    $scope.GetIssueDetailByIssueID = function (IssueID) {

        var apiRoute = baseUrl + 'GetIssueDetailByIssueID/'; // 
        var _setdetails = InternalIssueService.getSetDetalByIssueId(apiRoute, page, pageSize, isPaging, IssueID);
        _setdetails.then(function (response) {
            //-----------Call Can No By DepartmentID-----------------
            $scope.GetCanNoByDeapartmentId();
            $scope.issueDetails = response.data;
            if ($scope.departmentId === 9) {
                $scope.txtbxRopeReadonly = true;
                $scope.ddlCanNoReadOnly = true;
            }
            else if ($scope.departmentId === 7) {
                $scope.SetSetDetailGridRopNuAndCan();
            }

            $scope.hidInternalIssueDetail = true;
            $scope.btnShowHide = "Show List";

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.SetDrpDwn = function (response) {
        $scope.drpArticalNO = response.data.ItemID;
        if (response.data.ItemName != 'N/A') {
            //$("#drpArticalNO").select2("data", { id: 0, text: response.data.ItemName });
            $scope.ArticleNo = response.data.ItemName;
        } else {
            //$("#drpArticalNO").select2("data", { id: 0, text: '--Artical No.--' });
            $scope.ArticleNo = "";
        }
        $scope.drpSetNo = response.data.SetID;
        if (response.data.SetNo != 'N/A') {
            $("#drpSetNo").select2("data", { id: 0, text: response.data.SetNo });
        } else {
            $("#drpSetNo").select2("data", { id: 0, text: '-- Select Set No --' });
        }
        if ($scope.departmentId == 8) {
            if (response.data.ReceivedDyDate == "") {
                $scope.txtbxDyingIssueDateReadOnly = true;
                $scope.txtbxDyingIssueRemarkReadOnly = true;
                $scope.hidRopeNo = true;
                $scope.hidCanNo = true;
            }
            else {
                $scope.txtbxDyingIssueDateReadOnly = false;
                $scope.txtbxDyingIssueRemarkReadOnly = false;
                $scope.txtbxDyingReceiveDateReadOnly = true;
                $scope.txtbxDyingReceiveRemarkReadOnly = true;
                //$scope.txtbxRopeReadonly = true;
                //$scope.ddlCanNoReadOnly = true;
            }
        }
    }
    $scope.setControllerAfterClick = function () {
        var department = $scope.departmentId;
        //--Ball
        if (department === 7) {

        }
            //--------------Dying--------------------
        else if (department === 8) {

            $scope.hidSetIssueBySetwise = false;
            $scope.hidformInternalIssue = false;


        }
        else if (department === 9) {

        }
    }

    //********************************************** Item Modal Code *****************************************
    $scope.gridOptionslistItemMaster = [];
    $scope.modalSearchItemName = "";
    $scope.IsCallFromSearch = false;
    $scope.getListItemMaster = function (model) {
        var ItemID = model.ItemID;
        var ItemName = model.ItemName;
        $scope.drpArticalNO = model.ItemID;
        $scope.ArticleNo = ItemName;

        $scope.GetSetNoByArticalNo();
       

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


        var apiRoute = '/SystemCommon/api/PublicApi/' + 'GetFinishedItemMaster/';
        var listItemMaster = PublicService.getItemMasterService(apiRoute, objcmnParam);
        listItemMaster.then(function (response) {
            $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
            $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
            $scope.loaderMoreItemMaster = false;
        },
        function (error) {
            console.log("Error: " + error);
        });

    };
}]);