/*
*    Created By: Shamim Uddin;
*    Create Date: 14-6-2016 (dd-mm-yy); Updated Date: 0-0-2000 (dd-mm-yy);
*    Name: 'SizeBeamIssueController';
*    Type: $scope;
*    Purpose: This Controller Use Two Department Sizing and Weaving For Production;
*    Service Injected: '$scope', 'conversion', 'SizeBeamIssueService','localStorage';
*/

app.controller('SizeBeamIssueController', ['$scope', 'SizeBeamIssueService', '$localStorage', 'conversion','PublicService',
function ($scope, SizeBeamIssueService, $localStorage, conversion, PublicService) {

    // ---------Defult Seting--------- 
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;
    var baseUrl = '/Production/api/SizeBeamIssue/';
    $scope.btnSaveUpdateText = "Save";
    $scope.btnShowHide = "Show List"
    $scope.PageTitle = 'Size Beam Issue';
    $scope.ListTitle = 'Size Beam  List';
    $scope.MenuID = 0;
    $scope.drpArticalNo = 0;
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.mainFromHS = true;
    $scope.SetDetailsDiv = true;
    function loadUserCommonEntity(num) {
        $scope.UserCommonEntity = {};
        $scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
        $scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
        $scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
        $scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
        $scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
        $scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
        $scope.UserCommonEntity.MenuList = $localStorage.MenuList;
        $scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
        console.log($scope.UserCommonEntity);
    }
    loadUserCommonEntity(0);
    $scope.departmentId = $scope.UserCommonEntity.loggedUserBranchID;


    //---------Set Departments---------
    $scope.Departments = [{
        Name: 'Sizing',
        ID: 10
    }, {
        Name: 'Weaving',
        ID: 11
    }];

    $scope.SetControllerDepartmentWise = function () {
        debugger
        if ($scope.Departments[0].ID == $scope.departmentId) {
            $scope.weavingReceiveDate = true;
            $scope.noteForWeaving = true;

            $scope.rbfDate = true;
            $scope.rtotalFabric = true;
            $scope.rremark = true;
            $scope.tbfDate = true;
            $scope.ttotalFabric = true;
            $scope.tremark = true;
            $scope.rLoom = true;
            $scope.dLoom = true;


        }
        else if ($scope.Departments[1].ID == $scope.departmentId) {
            $scope.sizingIssueDate = true;
            $scope.boteForSizing = true;

            $scope.rbfDate = false;
            $scope.rtotalFabric = false;
            $scope.rremark = false;
            $scope.tbfDate = false;
            $scope.ttotalFabric = false;
            $scope.tremark = false;
            $scope.rLoom = false;
            $scope.dLoom = false;
            $scope.ngsizingGPL = true;
            $scope.ngShade = true;
        }

    }

    $scope.SetControllerDepartmentWise();



    ////-------------- Sizing Machine -------------------
    //$scope.GetMachines = function () {
    //    debugger;
    //    var ItemTypeID = 4;
    //    var ItemGroupID = 48;
    //    var apiRoute = baseUrl + 'GetMachineByTypeAndGroupID/'; // 
    //    var _machine = SizeBeamIssueService.getMachine(apiRoute, page, pageSize, isPaging, LoginCompanyID, ItemTypeID, ItemGroupID);
    //    _machine.then(function (response) {
    //        $scope.Machines = response.data;
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //$scope.GetMachines();

    //-----------Finish Good--------------------
    $scope.GetAllArticalByItemType = function () {
        var Type = 1; // Finish Good's ID 1
        var apiRoute = baseUrl + 'GetAllArticalByItemType/';
        var _artical = SizeBeamIssueService.getArtical(apiRoute, page, pageSize, isPaging, LoginCompanyID, Type);
        _artical.then(function (response) {
            $scope.Articals = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetAllArticalByItemType();

    $scope.GetSetNoByArticalNo = function () {
        $scope.setNos = [];
        debugger
        var apiRoute = baseUrl + 'GetSetNoByArticalNo/'; // 
        var _setNo = SizeBeamIssueService.getAllById(apiRoute, page, pageSize, isPaging, $scope.drpArticalNo, LoginCompanyID);
        _setNo.then(function (response) {
            //if (response.data[0].ArticalNo != null) {
            $scope.setNos = response.data;
            //}
            //else
            //{
            //    Command: toastr["error"]("There is no Set for this Artical");
            //}
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.GetSetDetailsBySetNo = function () {
        var apiRoute = baseUrl + 'GetSetDeatailBySetNo/'; // 
        var _setNo = SizeBeamIssueService.getSetDetails(apiRoute, page, pageSize, isPaging, $scope.drpSetNo, LoginCompanyID);
        _setNo.then(function (response) {
            if (response.data != null) {
                $scope.txtbxTEnds = response.data.TEnds;
                $scope.txtbxSetLength = response.data.SetLength;
                $scope.txtbxWarpLot = response.data.WarpLot;
                $scope.txtbxWarpCount = response.data.WarpCount;
                $scope.txtbxWarpSupplier = response.data.SupplierFullName;
                $scope.txtbxWarpRatio = response.data.WarpRatio;
                $scope.txtbxWeftLot = response.data.WeftRatio;
                $scope.txtbxWeftLot = response.data.WeftRatio;
                $scope.txtbxWeftCount = response.data.WeftCount;
                $scope.txtbxColor = response.data.ColorName;
                $scope.txtbxPI = response.data.PINO;
                $scope.txtbxBuyer = response.data.BuyerFullName;
                $scope.txtbxMachine = response.data.MachineName;
                $scope.SetDetailsDiv = false;
                $scope.GetSizingMRRMasterDetailBySetID();
                $scope.GetLoom();
                $scope.SetControllerDepartmentWise();
            }

        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.GetLoom = function () {

        var apiRoute = baseUrl + 'GetLoom/'; // 
        var _SizingMrrMasterDetail = SizeBeamIssueService.getLoom(apiRoute, LoginCompanyID);
        _SizingMrrMasterDetail.then(function (response) {
            if (response.data != null) {

                $scope.Looms = response.data;
            }

        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.GetSizingMRRMasterDetailBySetID = function () {
        var apiRoute = baseUrl + 'GetSizingMRRMasterDetailBySetID/'; // 
        var _SizingMrrMasterDetail = SizeBeamIssueService.getSetDetails(apiRoute, page, pageSize, isPaging, $scope.drpSetNo, LoginCompanyID);
        _SizingMrrMasterDetail.then(function (response) {
            if (response.data != null) {
                $scope.hidSizeMRRID = response.data[0].SIzeMRRID;
                $scope.SizingMrrMasterDetail = response.data;
            }

        },
        function (error) {
            console.log("Error: " + error);
        });


    }

    $scope.getSizeBeamIssueMaster = function () {
        var apiRoute = baseUrl + 'GetSizeBeamIssueDetails/'; // 
        var _SizingMrrMasterDetail = SizeBeamIssueService.getAll(apiRoute, page, pageSize, isPaging, LoginCompanyID);
        _SizingMrrMasterDetail.then(function (response) {
            if (response.data != null) {
                debugger;
                $scope.SizingBeamIssueMaster = response.data;
            }

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.getSizeBeamIssueMaster();

    $scope.save = function () {
        debugger
        var PrdSizingBeamIssue = {
            BeamIssueID: $scope.BeamIssueID,
            TransactionTypeID: 10000, //$scope.UserCommonEntity.currentTransactionTypeID,
            ItemID: $scope.drpArticalNo,
            Setlength: $scope.txtbxSetLength,
            SetID: $scope.drpSetNo,
            SizeMRRID: $scope.hidSizeMRRID,

            SizeDepartmentID: '',
            IsIssuedSize: '',
            SizeIssueDate: '',
            SizeIssueBy: '',
            SizeIssueRemarks: '',

            IsReceivedWeaving: '',
            WeavingReceiveDate: '',
            WeavingReceiveBy: '',
            WeavingReceivedRemarks: '',
            WeavingDepartmentID: '',

            CompanyID: LoginCompanyID,
            CreateBy: LoginUserID,
            IsDeleted: false,
            GPL: '',
            Shade:''

        }
        //--------Sizing-----------------------
        if ($scope.Departments[0].ID == $scope.departmentId) {

            PrdSizingBeamIssue.GPL = $scope.txtbxGPL;
            PrdSizingBeamIssue.Shade = $scope.txtbxShade;

            PrdSizingBeamIssue.SizeDepartmentID = $scope.departmentId,
            PrdSizingBeamIssue.IsIssuedSize = true
            PrdSizingBeamIssue.SizeIssueDate = conversion.getStringToDate($scope.txtbxSizingIssueDate) === "" ? null : conversion.getStringToDate($scope.txtbxSizingIssueDate);

            PrdSizingBeamIssue.SizeIssueBy = LoginUserID,
            PrdSizingBeamIssue.SizeIssueRemarks = $scope.txtbxNoteForSizing
        }
            //--------Weaving-----------------------
        else if ($scope.Departments[1].ID == $scope.departmentId) {
            PrdSizingBeamIssue.WeavingDepartmentID = $scope.departmentId,
            PrdSizingBeamIssue.WeavingReceiveDate = conversion.getStringToDate($scope.txtbxWeavingReceiveeDate) === "" ? null : conversion.getStringToDate($scope.txtbxWeavingReceiveeDate);
            PrdSizingBeamIssue.WeavingReceiveBy = LoginUserID,
            PrdSizingBeamIssue.WeavingReceivedRemarks = $scope.txtbxNoteForWeaving,
            PrdSizingBeamIssue.IsReceivedWeaving = true

        }

        var PrdSizingBeamIssueDetail = $scope.SizingMrrMasterDetail;



        var apiRoute = baseUrl + 'SaveSizeBeamIssue/';
        var SaveInternalIssue = SizeBeamIssueService.postSizeBeamIssue(apiRoute, PrdSizingBeamIssueDetail, PrdSizingBeamIssue);
        SaveInternalIssue.then(function (response) {
            if (response.data === 1) {
                Command: toastr["info"]("Saved Successfully!!!!");
            }
            else {
                Command: toastr["error"]("Error !!!!");
            }

        }, function (error) {
            console.log("Error: " + error);
        });

    }

    $scope.GetSizeBeamIssueDetail = function (model) {
        //alert(model.SetID);
        if ($scope.Departments[0].ID == $scope.departmentId) {

            //$scope.weavingReceiveDate = false;
            //$scope.noteForWeaving = false;

            //$scope.rbfDate = false;
            //$scope.rtotalFabric = false;
            //$scope.rremark = false;
            //$scope.tbfDate = false;
            //$scope.ttotalFabric = false;
            //$scope.tremark = false;
            //$scope.rLoom = false;
            //$scope.dLoom = false;
            //$scope.SetDetailsDiv = true;
            $scope.drpSetNo = model.SetID;
            $scope.getSizeBeamIssuemasterDetailByBeamIssueID(model.BeamIssueID);
            $scope.GetSizingMRRMasterDetailByBeamIssueID(model.BeamIssueID);
            $scope.SetControllerDepartmentWise();

        }
        else if ($scope.Departments[1].ID == $scope.departmentId) {

            //$scope.weavingReceiveDate = true;
            //$scope.noteForWeaving = true;

            //$scope.rbfDate = true;
            //$scope.rtotalFabric = true;
            //$scope.rremark = true;
            //$scope.tbfDate = true;
            //$scope.ttotalFabric = true;
            //$scope.tremark = true;
            //$scope.rLoom = true;
            //$scope.dLoom = true;

            $scope.drpSetNo = model.SetID;
            $scope.getSizeBeamIssuemasterDetailByBeamIssueID(model.BeamIssueID);
            $scope.GetSizingMRRMasterDetailByBeamIssueID(model.BeamIssueID);
            $scope.SetControllerDepartmentWise();
        }
    }

    $scope.GetSizingMRRMasterDetailByBeamIssueID = function (BeamIssueId) {
        var apiRoute = baseUrl + 'GetSizingMRRMasterDetailByBeamIssueID/'; // 
        var _setNo = SizeBeamIssueService.getSizeBeamIssueDetails(apiRoute, page, pageSize, isPaging, BeamIssueId, LoginCompanyID);
        _setNo.then(function (response) {
            if (response.data != null) {
                debugger

                for (i = 0; i < response.data.length; i++) {

                    var bsDate = new Date(response.data[i].BSDate);
                    var lesonedaybsDate = bsDate - 1;
                    response.data[i].BSDate = new Date(lesonedaybsDate);

                    var bmDate = new Date(response.data[i].BMDate);
                    var lesonedaybmDate = bmDate - 1;
                    response.data[i].BMDate = new Date(lesonedaybmDate);

                    if (response.data[i].BFDate != null) {
                        var bfDate = new Date(response.data[i].BFDate);
                        var lesonedaybfDate = bfDate - 1;
                        response.data[i].BFDate = new Date(lesonedaybfDate);
                    }




                }
                $scope.SizingMrrMasterDetail = response.data;

            }

        },
        function (error) {
            console.log("Error: " + error);
        });


    }




    $scope.getSizeBeamIssuemasterDetailByBeamIssueID = function (BeamIssueId) {
        var apiRoute = baseUrl + 'GetSizeBeamIssuemasterDetailByBeamIssueID/'; // 
        var _setNo = SizeBeamIssueService.getSizeBeamIssueDetails(apiRoute, page, pageSize, isPaging, BeamIssueId, LoginCompanyID);
        _setNo.then(function (response) {
            if (response.data != null) {
                $scope.BeamIssueID = response.data.BeamIssueID;
                $scope.txtbxTEnds = response.data.TEnds;
                $scope.txtbxSetLength = response.data.SetLength;
                $scope.txtbxWarpLot = response.data.WarpLot;
                $scope.txtbxWarpCount = response.data.WarpCount;
                $scope.txtbxWarpSupplier = response.data.SupplierFullName;
                $scope.txtbxWarpRatio = response.data.WarpRatio;
                $scope.txtbxWeftLot = response.data.WeftRatio;
                $scope.txtbxWeftLot = response.data.WeftRatio;
                $scope.txtbxWeftCount = response.data.WeftCount;
                $scope.txtbxColor = response.data.ColorName;
                $scope.txtbxPI = response.data.PINO;
                $scope.txtbxBuyer = response.data.BuyerFullName;
                $scope.txtbxMachine = response.data.MachineName;
                $scope.txtbxNoteForSizing = response.data.SizeIssueRemarks;
                $scope.txtbxGPL = response.data.GPL;
                $scope.txtbxShade = response.data.Shade;

                if (response.data.SizeIssueDate != null) {
                    $scope.txtbxSizingIssueDate = conversion.getDateToString(response.data.SizeIssueDate);
                }
                $scope.SetDetailsDiv = false;
                $scope.SetDrpDwn(response);
                $scope.GetLoom();
            }

        },
        function (error) {
            console.log("Error: " + error);
        });

    }

    $scope.SetDrpDwn = function (response) {
        $scope.drpArticalNo = response.data.ItemID;
        if (response.data.ItemName != 'N/A') {
            $scope.ArticleNo = response.data.ArticleNo;
            //$("#drpArticalNo").select2("data", { id: 0, text: response.data.ArticleNo });
        } else {
            //$("#drpArticalNo").select2("data", { id: 0, text: '-- Select Article No --' });
            $scope.ArticleNo = "";
        }
        $scope.GetSetNoByArticalNo();
        $scope.drpSetNo = response.data.SetID;
        if (response.data.ItemName != 'N/A') {
            $("#drpSetNo").select2("data", { id: 0, text: response.data.SetNo });
        } else {
            $("#drpSetNo").select2("data", { id: 0, text: '-- Select Set No --' });
        }
    }
    //********************************************** Item Modal Code *****************************************
    $scope.gridOptionslistItemMaster = [];
    $scope.modalSearchItemName = "";
    $scope.IsCallFromSearch = false;
    $scope.getListItemMaster = function (model) {
        var ItemID = model.ItemID;
        var ItemName = model.ItemName;
        $scope.drpArticalNo = model.ItemID;
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