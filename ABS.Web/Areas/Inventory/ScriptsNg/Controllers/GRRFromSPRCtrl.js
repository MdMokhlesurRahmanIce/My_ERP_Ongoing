/**
* GRRCtrl.js   //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;
*/

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('gRRFromSPRCtrl', ['$scope', 'gRRService', 'crudService', 'RequisitionService', 'mRRService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, gRRService, crudService, RequisitionService, mRRService, conversion, $filter, $localStorage, uiGridConstants) {


        $scope.gridOptionsChallanMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};
        $scope.ManualGRRNo = "";
        $scope.ChkRefChallanNoWhnSave = "";
        $scope.ChkRefChallanNoWhnSaveChallan = "";
        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        //   GRR  For Loan   $scope.IsLoanTypeOrOthers = true;

        // $scope.IsSPRType = true;

        $scope.IsSPRType = true;
        $scope.IsLoanTypeOrOthers = false;

        $scope.drpPageTitle = "Warehouse List";
        $scope.DepartmentID = "";

        // $scope.IsItemTypeFinishedGoods = false; 
        // $scope.IsItemTypeRMOthers = true;  //  for  Default Raw Material or Others

        $scope.ItemIDFlotNbatchSave = "0";
        $scope.hfIndex = "";
        $scope.HGRRNo = "";

        var baseUrl = '/Inventory/api/GRR/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.GRRDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $scope.GrrID = "0";

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        

        $scope.ListChallanDetails = [];
        $scope.ListChallanDetailsForSearch = [];

        $scope.listSalesPerson = [];

        //  $scope.btnModal = "Add";

        $scope.btnLotModal = "Save";
        $scope.btnBatchModal = "Save";
        $scope.listMrrLot = [];
        $scope.listMrrBatchNo = [];
        $scope.LotSetup = "Lot Setup";
        $scope.BatchSetup = "Batch Setup";

        function loadUserCommonEntity(num) {
            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            //  console.clear();
            //  debugger
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

            //////Coming from SideNavCrl
            ////$scope.UserCommonEntity = {};
            //$scope.UserCommonEntity.loggedCompnyID = $localStorage.loggedCompnyID;
            //$scope.UserCommonEntity.loggedUserID = $localStorage.loggedUserID;
            //$scope.UserCommonEntity.loggedUserBranchID = $localStorage.loggedUserBranchID;
            //$scope.UserCommonEntity.loggedUserDepartmentID = $localStorage.loggedUserDepartmentID;
            //$scope.UserCommonEntity.currentModuleID = $localStorage.currentModuleID;
            //$scope.UserCommonEntity.currentMenuID = $localStorage.currentMenuID;
            //$scope.UserCommonEntity.currentTransactionTypeID = $localStorage.currentTransactionTypeID;
            //$scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            //$scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            //console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);

        $scope.grdTGrrNo = "";
        $scope.grdTGrrDate = "";
        $scope.IsGrrFromSpr = false;
        $scope.IsGrrFromSprLoan = false;
        $scope.IsGrrFromLoanReturnIssue = false;
        $scope.CommnFrLoanReturnIssueNSprLoan = false;
        $scope.grdTSupplier = "";
        $scope.grdTSPRNo = ""


        function chkTrnsTypeFHideShow() {
            if ($scope.UserCommonEntity.currentTransactionTypeID == 18) {  //  GRR  from  SPR
                 
                $scope.IsGrrFromSpr = true; 
                $scope.IsGrrFromSprLoan = false;
                $scope.IsGrrFromLoanReturnIssue = false;
                $scope.CommnFrLoanReturnIssueNSprLoan = false;
                 
                $scope.grdTGrrNo = "GRR No";
                $scope.grdTGrrDate = "GRR Date";

                $scope.grdTSupplier = "Supplier";
                $scope.grdTSPRNo = "SPR No"

                // for master load


                $scope.PageTitle = 'GRR Creation';
                $scope.ListTitle = 'GRR Records';
                $scope.ListTitleGRRMasters = 'GRR Information';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleGRRDeatails = 'Listed Item of GRR';

            }
            else if ($scope.UserCommonEntity.currentTransactionTypeID == 36) { // GRR  from Loan SPR

                $scope.IsGrrFromSpr = false;
                $scope.IsGrrFromSprLoan = true;
                $scope.IsGrrFromLoanReturnIssue = false;
                $scope.CommnFrLoanReturnIssueNSprLoan = true;

                $scope.grdTGrrNo = "Loan GRR No";
                $scope.grdTGrrDate = "Loan GRR Date";

                $scope.grdTSupplier = "Supplier Company";
                $scope.grdTSPRNo = "Loan Order No"

                $scope.PageTitle = 'Loan GRR Creation';
                $scope.ListTitle = 'Loan GRR Records';
                $scope.ListTitleGRRMasters = 'Loan GRR Information';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleGRRDeatails = 'Listed Item of Loan GRR';
            }

            else if ($scope.UserCommonEntity.currentTransactionTypeID == 45) { // GRR  from Loan return issue no

                $scope.IsGrrFromSpr = false;
                $scope.IsGrrFromSprLoan = false;
                $scope.IsGrrFromLoanReturnIssue = true;

                $scope.CommnFrLoanReturnIssueNSprLoan = true;

                $scope.grdTGrrNo = "Loan Return GRR No";
                $scope.grdTGrrDate = "Loan Return GRR Date";

                $scope.grdTSupplier = "Supplier Company";
                $scope.grdTSPRNo = "Loan Order No"

                $scope.PageTitle = 'Loan Return GRR Creation';
                $scope.ListTitle = 'Loan Return GRR Records';
                $scope.ListTitleGRRMasters = 'Loan Return GRR Information';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleGRRDeatails = 'Listed Item of Loan Return GRR';
            }



            //else if ($scope.UserCommonEntity.currentTransactionTypeID == 10) {  // for  loan
            //    $scope.IsIssueReceive = false;
            //    $scope.CommonFIssueNReturnReceive = false;
            //    $scope.IsMrr = true;
            //    $scope.CommonFLoanReceiveNMrr = true;
            //    $scope.IsLoanReceive = false;
            //    $scope.IsReturnReceive = false;

            //    $scope.grdTMrNo = "MRR No";
            //    $scope.grdTMrDate = "MRR Date";
            //    $scope.grdTSprDate = "SPR Date";
            //    $scope.grdTSprNo = "SPR No";


            //    $scope.PageTitle = 'MRR Creation';
            //    $scope.ListTitle = 'MRR Records';
            //    $scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
            //    $scope.ListTitleSampleNo = 'Sample Info';
            //    $scope.ListTitleMrrDeatails = 'MRR Information (Details)';

            //}
          
        } 
        chkTrnsTypeFHideShow();








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
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;

                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.loadChallanRecords(0);
            }
        }

        $scope.listLotType = [
         { LotTypeID: 1, LotTypeName: 'Internal' },
         { LotTypeID: 2, LotTypeName: 'External' }
        ];

        $scope.listBatchType = [
          { BatchTypeID: 1, BatchTypeName: 'Internal' },
          { BatchTypeID: 2, BatchTypeName: 'External' }
        ];

        $scope.lstBatchTypeList = "2";
        $("#ddlBatchType").select2("data", { id: "2", text: "External" });

        $scope.lstLotTypeList = "2";
        $("#ddlLotType").select2("data", { id: "2", text: "External" });

        $scope.getTypes = function () {
            $scope.Types = [
                {
                    Item: 'Finish Good'
                    , Value: 1 // For Raw Material
                },
                {
                    Item: 'Raw Material'
                , Value: 2 // For Raw Material
                },
                {
                    Item: 'Yarn'
                    , Value: 3 // For Yarn
                },
            {
                Item: 'Chemical'
                , Value: 5 // For Chemical
            }, {
                Item: 'Fixed Asset'
                , Value: 4 // For Fixed Asset
            }];

            $scope.ItemType = "2";
            $('#ddlItemType').select2("data", { id: '2', text: 'Raw Material' });


        }
        $scope.getTypes();

        var UserTypeID = 1;
        $scope.UserLst = [];
        function GetAllUsers() {
            var apiRoute = '/Inventory/api/SPR/GetAllUsers/';
            var UserLst = RequisitionService.getAllUsers(apiRoute, page, pageSize, isPaging, UserTypeID, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
            UserLst.then(function (response) {
                $scope.UserLst = response.data
                angular.forEach($scope.UserLst, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {
                        $scope.GRRBy = item.UserID;
                        $("#ddlGRRBy").select2("data", { id: item.UserID, text: item.UserFullName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllUsers(0);

        //*******************   Item Group   On Page Load--  ***********

        //$scope.getItemGroupsByType = function () {

        //    $scope.listSampleNo = "";
        //    $scope.lstSampleNoList = "";
        //    $('#ddlSampleNo').select2("data", { id: '2', text: '--Select Item Group--' });

        //    var apiRoute = '/SystemCommon/api/RawMaterial/GetItemGroups/';
        //    if ($scope.ItemType != "") {
        //        var itemGroupes = gRRService.getAllItemGroup(apiRoute, page, pageSize, isPaging, $scope.ItemType, LoggedCompanyID);
        //        itemGroupes.then(function (response) {
        //            $scope.listSampleNo = response.data;
        //        },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //    }
        //    else {
        //        $scope.listSampleNo = "";
        //    }
        //}

        //$scope.getItemGroupsByType();


        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateGrrNo = function () {

            var getMNo = $scope.ManualGRRNo;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            if (getMNo.trim() != "") {
                var apiRoute = baseUrl + 'ChkDuplicateGrrNo/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(getMNo) + "]";
                var getDuplicateNo = RequisitionService.GetList(apiRoute, cmnParam);

                getDuplicateNo.then(function (response) {

                    $scope.ChkManualMRRNoWhnSave = "";

                    if (response.data.length > 0) {

                        $scope.ChkManualMRRNoWhnSave = response.data[0].ManualGrrNo
                        $scope.ManualGRRNo = "";
                        Command: toastr["warning"]("Manual GRR No Already Exists.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                Command: toastr["warning"]("Please Enter Manual GRR No.");
            }

        }

        //######## End Check Duplicate No ################//

        function LoanReturnIssueNoRecords(isPaging) {

            if ($scope.IsGrrFromLoanReturnIssue == true) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };

                var apiRoute = baseUrl + 'GetLoanReturnIssueNo/';

                var IssueTypeID = "35";

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(IssueTypeID) + "]";

                var Issues = gRRService.GetList(apiRoute, cmnParam);
                Issues.then(function (response) {
                    $scope.listIssueNo = response.data.lstIssue;


                    //console.log($scope.listBuyer);
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        LoanReturnIssueNoRecords(0);


        $scope.getItemDetailByIssueNo = function ()
        {
            var sprID = "";
            var issueNo = $scope.lstIssueNoList;

            angular.forEach($scope.listIssueNo, function (item) {
                if (issueNo == item.IssueID) {

                    $scope.listCompany = [];
                    $scope.lstCompanyList = '';
                    $("#ddlCompany").select2("data", { id: '', text: '' });
                    //  if (response.data.lstQC[0].CurrencyID != null) { 
                    $scope.listCompany.push({
                        CompanyID: item.FromCompanyID, CompanyName: item.FromCompanyName
                    });

                    $scope.lstCompanyList = item.FromCompanyID;
                    $("#ddlCompany").select2("data", { id: item.FromCompanyID, text: item.FromCompanyName });

                    sprID = item.RequisitionID;

                    //  $scope.SPRDate = item.SPRDate == null ? "" : conversion.getDateToString(item.SPRDate);

                    return false;
                }
            });
             
            $scope.GrrID = "0";
            $scope.btnSaveText = "Save";

            $scope.ListChallanDetails = [];
            $scope.IsHiddenDetail = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };

           
            var apiRoute = baseUrl + 'GetItemDetailFGrrByIssueID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(issueNo) + "]";
            var itemByIssueNo = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            itemByIssueNo.then(function (response) {
                $scope.listPONo = [];
                $scope.PONo = '';
                $("#ddlPONo").select2("data", { id: '', text: '' });

                if (response.data[0].POID != null) {
                    $scope.listPONo.push({
                        POID: response.data[0].POID, PONo: response.data[0].PONo
                    });
                    $scope.PONo = response.data[0].POID;
                    $("#ddlPONo").select2("data", { id: response.data[0].POID, text: response.data[0].PONo });
                } 

                $scope.listPINO = [];
                $scope.PINo = '';
                $("#ddlPINO").select2("data", { id: '', text: '' });

                if (response.data[0].PIID != null) {
                    $scope.listPINO.push({
                        PIID: response.data[0].PIID, PINo: response.data[0].PINo
                    });
                    $scope.PINo = response.data[0].PIID;
                    $("#ddlPINO").select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });
                }
                 
                $scope.listLCNo = [];
                $scope.LCNo = '';
                $("#ddlLCNo").select2("data", { id: '', text: '' });

                if (response.data[0].POID != null) {
                    $scope.listLCNo.push({
                        POID: response.data[0].POID, LCorVoucherorLcafNo: response.data[0].LCorVoucherorLcafNo
                    });
                    $scope.LCNo = response.data[0].POID;
                    $("#ddlLCNo").select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });
                }
                 
                $scope.DischargeLocation = "";
                $scope.LoadingLocation = "";

                $scope.ListChallanDetails = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });
        }




        $scope.loadSPRRecords = function () {
            if ($scope.IsSPRType == true && $scope.IsGrrFromLoanReturnIssue==false) {

                $scope.IsHiddenDetail = true;

                //$scope.listSprNo = [];
                //$scope.SprNo = "";
                //$('#ddlSPRNo').select2("data", { id: '', text: '--Select SPR No--' });

                $scope.listPONo = [];
                $scope.PONo = "";
                $('#ddlPONo').select2("data", { id: '', text: '--Select PO No--' });

                //$scope.listPINO = [];
                //$scope.PINO = "";
                //$('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });

                $scope.listLCNo = [];
                $scope.LCNo = "";
                $('#ddlLCNo').select2("data", { id: '', text: '--Select LC No--' });

                $scope.DischargeLocation = "";
                $scope.LoadingLocation = "";

                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

                };

                var ReqTypeID=0;
                if( $scope.IsGrrFromSpr==true)
                {
                    ReqTypeID=8;
                }
                else if ($scope.IsGrrFromSprLoan == true)
                {
                    ReqTypeID = 33;
                }

                var apiRoute = baseUrl + 'GetSPRNo/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ReqTypeID) + "]";

                var listSprNo = gRRService.GetList(apiRoute, cmnParam);

                listSprNo.then(function (response) {
                    $scope.listSprNo = response.data.objSPRNo;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }
        $scope.loadSPRRecords();


        $scope.listMrrBatchNo = [];
        // -------Get All Batch //--------
        function GetAllBatchNo() {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data;
                $scope.listMrrBatchNo = response.data;
                $scope.listBatchInLot = response.data;
                //if (varBatchID != "" && varBatchName != "")
                //{
                //    $scope.Batch = varBatchID;
                //    $('#ddlBatch').select2("data", { id: varBatchID, text: varBatchName });

                //    varBatchID = "";
                //    varBatchName = "";
                //}

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllBatchNo();


        $scope.listMrrLot = [];

        //**********----Get All Lot Record----***************
        function loadLotRecords(isPaging) {
            var apiRoute = '/Inventory/api/StockEntry/GetLotNo/';
            var lisLotNo = RequisitionService.getAllLotNo(apiRoute, page, pageSize, isPaging);
            lisLotNo.then(function (response) {
                $scope.listLot = response.data;
                $scope.listMrrLot = response.data;
                //if (varLotID != "" && varLotName != "") {
                //    $scope.listLot = [];
                //    $scope.lstLot = "";
                //    $('#ddlLotNo').select2("data", { id: "", text: "" });
                //    $scope.listLot = response.data;
                //    $scope.lstLot = varLotID;
                //    $('#ddlLotNo').select2("data", { id: varLotID, text: varLotName });

                //   // varLotID = "";
                //  //  varLotName = "";
                //}

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadLotRecords(0);


        //**********---- Get Wherehouse Records ----*************** //

        $scope.selectNode = function (val) {
            
            $scope.Department = val.Name;
            $scope.DepartmentID = val.ID;
        }

        $scope.treedubbleClick = function (val) {
            
            $scope.Department = val.Name;
            $scope.DepartmentID = val.ID;
            $scope.modal_fadeOutTree();
        }

        $scope.modal_fadeOutTree = function () {
            $("#drpModalDept").fadeOut(200, function () {
                $('#drpModalDept').modal('hide');
            });
        }

        $scope.loadDepartmentRecords = function () {
       
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID 
            }; 
    
            var apiRoute = baseUrl + 'GetDepartmentDetails/'; 
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            $scope.Departments = [];
            var listWherehouse = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listWherehouse.then(function (response) {
                
                $scope.Departments = response.data.ListDeptDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        function loadLoggedDept()
        {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID
            };
    
            var apiRoute = baseUrl + 'GetLoggedDeptName/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            $scope.Departments = [];
            var lggDeptName = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            lggDeptName.then(function (response) {

                $scope.Department = response.data.dept[0].Name;
                $scope.DepartmentID = response.data.dept[0].ID;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadLoggedDept();


        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateNo = function () {

            var getMNo = $scope.RefChallanNo;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            if (getMNo.trim() != "") {
                var apiRoute = baseUrl + 'ChkDuplicateNo/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(getMNo) + "]";
                var getDuplicateNo = gRRService.GetList(apiRoute, cmnParam);
                getDuplicateNo.then(function (response) {

                    $scope.ChkRefChallanNoWhnSaveChallan = "";

                    if (response.data.length > 0) {

                        $scope.ChkRefChallanNoWhnSaveChallan = response.data[0].RefCHNo;

                        $scope.RefChallanNo = "";
                        Command: toastr["warning"]("Ref Challan No Already Exists.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                    Command: toastr["warning"]("Please Enter Ref Challan No.");
            }

        }

        //######## End Check Duplicate No ################//


        //**********---- Get All Party Records ----*************** //
        function loadPartyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetParty/';
            var listParty = gRRService.getModel(apiRoute, page, pageSize, isPaging);
            listParty.then(function (response) {
                $scope.listParty = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadPartyRecords(0);

        //**********---- Get All Currency Records ----*************** //
        function loadCurrencyRecords(isPaging) {

            var apiRoute = baseUrl + 'GetCurrency/';
            var listCurrency = gRRService.getModel(apiRoute, page, pageSize, isPaging);
            listCurrency.then(function (response) {
                $scope.listCurrency = response.data;

                $scope.lstCurrencyList = response.data[0].Id;
                $("#ddlCurrency").select2("data", { id: response.data[0].Id, text: response.data[0].CurrencyName });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadCurrencyRecords(0);

        //$scope.listCurrency = [];
        //$scope.lstCurrencyList = '';
        //$("#ddlCurrency").select2("data", { id: '', text: '' });
        //if (response.data.lstQC[0].CurrencyID != null) {
        //    $scope.listCurrency.push({
        //        Id: response.data.lstQC[0].CurrencyID, CurrencyName: response.data.lstQC[0].CurrencyName
        //    });

        //    $scope.lstCurrencyList = response.data.lstQC[0].CurrencyID;
        //    $("#ddlCurrency").select2("data", { id: response.data.lstQC[0].CurrencyID, text: response.data.lstQC[0].CurrencyName });
        //}


        ////**********---- Get All  ChallanType  Records ----*************** //
        //function loadChallanTrnsTypes(isPaging) {

        //    var apiRoute = baseUrl + 'GetChallanTrnsTypes/';
        //    var listChallanType = gRRService.getModel(apiRoute, page, pageSize, isPaging);
        //    listChallanType.then(function (response) {
        //        $scope.listChallanType = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadChallanTrnsTypes(0);

        function loadLoadingLocation(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };
            var apiRoute = baseUrl + 'GetLocation/';
            var listLocation = gRRService.GetLocation(apiRoute, objcmnParam);
            listLocation.then(function (response) {
                $scope.listLoadLocation = response.data.objLocation;
                $scope.listDischargeLocation = response.data.objLocation;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadLoadingLocation(0);

        function loadPackingUnit(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };
            var apiRoute = baseUrl + 'GetPackingUnit/';
            var listPackingUnit = gRRService.GetPackingUnit(apiRoute, objcmnParam);
            listPackingUnit.then(function (response) {
                $scope.listPackingUnit = response.data.objPackingUnit;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadPackingUnit(0);

        function loadWeightUnit(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            var apiRoute = baseUrl + 'GetWeightUnit/';
            var listWeightUnit = gRRService.GetWeightUnit(apiRoute, objcmnParam);
            listWeightUnit.then(function (response) {
                $scope.listWeightUnit = response.data.objWeightUnit;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadWeightUnit(0);

        //function loadDischargeLocation(isPaging) {
        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //       loggeduser: $scope.UserCommonEntity.loggedUserID,
        //            loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
       //            menuId: $scope.UserCommonEntity.currentMenuID,
        //          tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };
        //    var apiRoute = baseUrl + 'GetLocation/';
        //    var listSprNo = gRRService.GetLocation(apiRoute, objcmnParam);
        //    listSprNo.then(function (response) {
        //        $scope.listSprNo = response.data.objSPRNo;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //loadDischargeLocation(0);
        $scope.getItmDetailsByItmCode = function () {

            $scope.ListChallanDetailsForSearch = [];

            var ItemCode = $scope.ItemCode;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };

            if (ItemCode.trim() != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItmCode/'; 
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ItemCode) + "]"; 
                var listItemSerch = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
                listItemSerch.then(function (response) {
                    if (response.data[0].ItemID > 0) {
                        $scope.ListChallanDetailsForSearch = response.data;
                    }
                    else
                        Command: toastr["warning"]("Item Not Exist."); 
                    //GRRID: 0, CHDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    //PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName, HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName,
                    //Qty: dataModel.Qty, UnitPrice: dataModel.UnitPrice, PackingQty: dataModel.PackingQty, PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight,
                    //GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit, ExistQty: dataModel.ExistQty, Amount: 0.00, 
                    //AditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (ItemCode.Trim() == "") {

                    Command: toastr["warning"]("Please Enter Item Code.");
            }

        }

        $scope.AddItem = function () {

            $scope.GrrID = "0";

            $scope.btnSaveText = "Save";

            $scope.IsHiddenDetail = false;

            var existItem = $scope.ListChallanDetailsForSearch[0].ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListChallanDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {

                // $scope.ListChallanDetailsForSearch.data[0].ItemID
                $scope.ListChallanDetails.push({
                    GrrID: 0, GrrDetailID: 0, ItemID: $scope.ListChallanDetailsForSearch[0].ItemID, CompanyID: $scope.ListChallanDetailsForSearch[0].CompanyID,
                    UnitID: $scope.ListChallanDetailsForSearch[0].UnitID,
                    LotID: '', BatchID: '', PackingUnitID: '', WeightUnitID: '', ItemCode: $scope.ListChallanDetailsForSearch[0].ItemCode,
                    ItemName: $scope.ListChallanDetailsForSearch[0].ItemName, HSCODE: $scope.ListChallanDetailsForSearch[0].HSCODE,
                    UOMName: $scope.ListChallanDetailsForSearch[0].UOMName, Qty: 0.00, UnitPrice: 0.00, PackingQty: 0.00, PackingUnit: '', NetWeight: '',
                    GrossWeight: 0.00, WeightUnit: 0.00, ExistQty: 0.00, Amount: 0.00,
                    AdditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00

                });
                $scope.ListChallanDetailsForSearch = [];
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
        }

         

        //**********---- Load  Item Details By SPRNo Change ----***************//

        $scope.LoadItemBySPRNoChange = function () {

            var loanSprNo = $scope.SprNo;

            angular.forEach($scope.listSprNo, function (item) {
                if (loanSprNo == item.RequisitionID) {

                    $scope.listCompany = [];
                    $scope.lstCompanyList = '';
                    $("#ddlCompany").select2("data", { id: '', text: '' });
                    //  if (response.data.lstQC[0].CurrencyID != null) { 
                    $scope.listCompany.push({
                        CompanyID: item.FromCompanyID, CompanyName: item.FromCompanyName
                    });

                    $scope.lstCompanyList = item.FromCompanyID;
                    $("#ddlCompany").select2("data", { id: item.FromCompanyID, text: item.FromCompanyName });

                    $scope.SPRDate = item.SPRDate == null ? "" : conversion.getDateToString(item.SPRDate);

                    return false;
                }
            });




            $scope.GrrID = "0";
            $scope.btnSaveText = "Save";

            $scope.ListChallanDetails = [];
            $scope.IsHiddenDetail = false;

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };

            var sprID = $scope.SprNo;
            var apiRoute = baseUrl + 'GetItemDetailBySPRID/'; 
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(sprID) + "]"; 
            var itemBySprNo = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get); 
            itemBySprNo.then(function (response) { 
                $scope.listPONo = [];
                $scope.PONo = '';
                $("#ddlPONo").select2("data", { id: '', text: '' });

                if (response.data[0].POID != null) {
                    $scope.listPONo.push({
                        POID: response.data[0].POID, PONo: response.data[0].PONo
                    });
                    $scope.PONo = response.data[0].POID;
                    $("#ddlPONo").select2("data", { id: response.data[0].POID, text: response.data[0].PONo });
                }

                //$scope.listPONo = response.data[0];
                //$scope.PONo = response.data[0].POID;
                //$('#ddlPONo').select2("data", { id: response.data[0].POID, text: response.data[0].PONo });


                $scope.listPINO = [];
                $scope.PINo = '';
                $("#ddlPINO").select2("data", { id: '', text: '' });

                if (response.data[0].PIID != null) {
                    $scope.listPINO.push({
                        PIID: response.data[0].PIID, PINo: response.data[0].PINo
                    });
                    $scope.PINo = response.data[0].PIID;
                    $("#ddlPINO").select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });
                }


                //$scope.listPINO = response.data[0];
                //$scope.PINO = response.data[0].PIID;
                //$('#ddlPINO').select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });


                $scope.listLCNo = [];
                $scope.LCNo = '';
                $("#ddlLCNo").select2("data", { id: '', text: '' });

                if (response.data[0].POID != null) {
                    $scope.listLCNo.push({
                        POID: response.data[0].POID, LCorVoucherorLcafNo: response.data[0].LCorVoucherorLcafNo
                    });
                    $scope.LCNo = response.data[0].POID;
                    $("#ddlLCNo").select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });
                }

                //$scope.listLCNo = response.data[0].POID;
                //$scope.LCNo = response.data[0].LCorVoucherorLcafNo;
                //$('#ddlLCNo').select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });


                $scope.DischargeLocation = "";
                $scope.LoadingLocation = "";

                $scope.ListChallanDetails = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        //**********---- Load  Item Details By PONo Change ----***************//

        //$scope.LoadItemByPONoChange = function () {

        //    $scope.GrrID = "0";

        //    $scope.btnSaveText = "Save";

        //    $scope.ListChallanDetails = [];
        //    $scope.IsHiddenDetail = false;

        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId:$scope.UserCommonEntity.currentTransactionTypeID

        //    };

        //    var poID = $scope.PONo;
        //    var apiRoute = baseUrl + 'GetItemDetailByPOID/';

        //    var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(poID) + "]";

        //    var itemByPONo = gRRService.GetList(apiRoute, cmnParam);

        //    // var itemBySprNo = gRRService.GetItemDetailBySPRID(apiRoute, objcmnParam, sprID);
        //    itemByPONo.then(function (response) {


        //        $scope.ListChallanDetails = response.data;

        //        $scope.listSprNo = [];
        //        $scope.SprNo = '';
        //        $("#ddlSPRNo").select2("data", { id: '', text: '' });

        //        if (response.data[0].RequisitionID != null) {
        //            $scope.listSprNo.push({
        //                RequisitionID: response.data[0].RequisitionID, RequisitionNo: response.data[0].RequisitionNo
        //            });
        //            $scope.SprNo = response.data[0].RequisitionID;
        //            $("#ddlSPRNo").select2("data", { id: response.data[0].RequisitionID, text: response.data[0].RequisitionNo });
        //        }

        //        $scope.listParty = [];
        //        $scope.lstPartyList = '';
        //        $("#ddlSupplier").select2("data", { id: '', text: '' });

        //        if (response.data[0].SupplierID != null) {
        //            $scope.listParty.push({
        //                UserID: response.data[0].SupplierID, UserFullName: response.data[0].SupplierName
        //            });
        //            $scope.lstPartyList = response.data[0].SupplierID;
        //            $("#ddlParty").select2("data", { id: response.data[0].SupplierID, text: response.data[0].SupplierName });
        //        }


        //        $scope.listCurrency = [];
        //        $scope.lstCurrencyList = '';
        //        $("#ddlCurrency").select2("data", { id: '', text: '' });
        //        if (response.data[0].CurrencyID != null) {
        //            $scope.listCurrency.push({
        //                Id: response.data[0].CurrencyID, CurrencyName: response.data[0].CurrencyName
        //            });

        //            $scope.lstCurrencyList = response.data[0].CurrencyID;
        //            $("#ddlCurrency").select2("data", { id: response.data[0].CurrencyID, text: response.data[0].CurrencyName });
        //        }


        //        //$scope.listPONo = response.data[0];
        //        //$scope.PONo = response.data[0].POID;
        //        //$('#ddlPONo').select2("data", { id: response.data[0].POID, text: response.data[0].PONo });


        //        //$scope.listPINO = [];
        //        //$scope.PINo = '';
        //        //$("#ddlPINO").select2("data", { id: '', text: '' });

        //        //if (response.data[0].PIID != null) {
        //        //    $scope.listPINO.push({
        //        //        PIID: response.data[0].PIID, PINo: response.data[0].PINo
        //        //    });
        //        //    $scope.PINo = response.data[0].PIID;
        //        //    $("#ddlPINO").select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });
        //        //}


        //        //$scope.listPINO = response.data[0];
        //        //$scope.PINO = response.data[0].PIID;
        //        //$('#ddlPINO').select2("data", { id: response.data[0].PIID, text: response.data[0].PINo });


        //        //$scope.listLCNo = [];
        //        //$scope.LCNo = '';
        //        //$("#ddlLCNo").select2("data", { id: '', text: '' });

        //        //if (response.data[0].POID != null) {
        //        //    $scope.listLCNo.push({
        //        //        POID: response.data[0].POID, LCorVoucherorLcafNo: response.data[0].LCorVoucherorLcafNo
        //        //    });
        //        //   $scope.LCNo = response.data[0].POID;
        //        //    $("#ddlLCNo").select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });
        //        //}

        //        //$scope.listLCNo = response.data[0].POID;
        //        //$scope.LCNo = response.data[0].LCorVoucherorLcafNo;
        //        //$('#ddlLCNo').select2("data", { id: response.data[0].POID, text: response.data[0].LCorVoucherorLcafNo });

        //        $scope.LCNo = response.data[0].LCorVoucherorLcafNo;
        //        $scope.DischargeLocation = "";
        //        $scope.LoadingLocation = "";


        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}



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

            // $scope.IsItemTypeFinishedGoods = $scope.ItemType == "1" ? true : false;
            // $scope.IsItemTypeRMOthers = $scope.ItemType != "1" ? true : false;

            //if ($scope.lstSampleNoList == undefined) {
            //    Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            //   else {
            // For Loading modal
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

            $scope.gridOptionslistItemMaster.enableFiltering = true;
            $scope.gridOptionslistItemMaster.enableGridMenu = true;

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
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };


            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            // $scope.IsItemTypeFinishedGoods
            // $scope.IsItemTypeRMOthers
            $scope.gridOptionslistItemMaster = {
                columnDefs: [
                    { name: "UniqueCode", displayName: "Sample ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", displayName: "Item ID", title: "Item ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "Company ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Article No", visible: false, title: "Article No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemCode", displayName: "Item Code", title: "Item Code", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", displayName: "Item Name", title: "Item Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "HSCODE", displayName: "HSCODE", title: "HSCODE", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CuttableWidth", displayName: "C.Width", title: "C.Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Construction", displayName: "Construction", title: "Construction", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Weave", displayName: "Weave", title: "Weave", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeightPerUnit", displayName: "Weight", title: "Weight", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color Name", title: "Color Name", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeName", displayName: "Size", title: "Size", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Width", displayName: "Width", title: "Width", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Select',
                        displayName: "Select",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                                        '<i class="icon-check" aria-hidden="true"></i> Select' +
                                      '</a>' +
                                      '</span>'
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                },
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
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

            // $scope.listItemMaster = [];
            //var groupID = $scope.lstSampleNoList;
            //if (groupID > 0) {
            var apiRoute = baseUrl + 'GetItemMasterByGroupID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listItemMaster = gRRService.getItemMasterByGroup(apiRoute, cmnParam, $scope.HeaderToken.get);
            listItemMaster.then(function (response) {
                //$scope.listItemMaster = response.data;
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objItemMaster;
                $scope.loaderMoreItemMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
            //}
            //else if (groupID == 0 || groupID == "") {
            //        Command: toastr["warning"]("Select Sample/Article No !!!!");
            //}
            //}
        };


        //**********----Pagination Master Challan----***************
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
                $scope.loadChallanRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadChallanRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadChallanRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadChallanRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadChallanRecords(1);
                }
            }
        };

        //**********----Get All Challan Master Records----***************
        $scope.loadChallanRecords = function (isPaging) {

            $scope.gridOptionsChallanMaster.enableFiltering = true;
            $scope.gridOptionsChallanMaster.enableGridMenu = true;
            //$scope.gridOptionsLC.enableFiltering = true;

            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMoreChallanMaster = true;
            $scope.lblMessageForChallanMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };



            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsChallanMaster = {

                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                 

                columnDefs: [
                    { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader }, 
                    { name: "UserID", displayName: "PartyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader }, 
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader }, 
                    { name: "RequisitionID", displayName: "RequisitionID", title: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader }, 
                    { name: "GrrNo", displayName: $scope.grdTGrrNo, title: $scope.grdTGrrNo, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ManualGrrNo", displayName: "Manual GrrNo", title: "Manual GrrNo", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrDate", displayName: $scope.grdTGrrDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PONo", displayName: "PO No", title: "PO No", width: '10%', visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LCorVoucherorLcafNo", displayName: "L/C No", title: "L/C No", visible: $scope.IsSPRType, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: $scope.grdTSPRNo , title:$scope.grdTSPRNo , width: '20%', visible: $scope.IsSPRType, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UserFullName", displayName: $scope.grdTSupplier, title: $scope.grdTSupplier, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHNo", displayName: "Ref ChallanNo", title: "Ref ChallanNo", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RefCHDate", displayName: " Ref Challan Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader }, 
                    { name: "CurrencyName", displayName: "Currency", title: "Currency", width: '10%', headerCellClass: $scope.highlightFilteredHeader }, 
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '13%',
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,

                        cellTemplate: '<span class="label label-success label-mini">' +
                                   '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                                         '<i class="icon-search" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;Files</i>' +
                                     '</a>' +
                                 '</span>' +
                                 '<span class="label label-success label-mini">' +
                                   '<a href="" title="Edit" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                                     '<i class="icon-edit"></i> Edit' +
                                   '</a>' +
                                   '</span>'


                        //cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //              '<a href="" title="Select" ng-click="grid.appScope.loadChallaMasterDetailsByGrrNo(row.entity)">' +
                        //                '<i class="icon-check" aria-hidden="true"></i> Select' +
                        //              '</a>' +
                        //              '</span>'
                    }

                ],

                exporterAllDataFn: function () {
                    return getPage(1, $scope.gridOptionsChallanMaster.totalItems, paginationOptions.sort)
                    .then(function () {
                        $scope.gridOptionsChallanMaster.useExternalPagination = false;
                        $scope.gridOptionsChallanMaster.useExternalSorting = false;
                        getPage = null;
                    });
                },
            };

            var apiRoute = baseUrl + 'GetGrrMasterList/';
             
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify($scope.IsSPRType) + "]";

            var listChallanMaster = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get); 
            listChallanMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsChallanMaster.data = response.data.lstVmChallanMaster;
                $scope.loaderMoreChallanMaster = false;
                $scope.lblMessageForChallanMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        $scope.loadChallanRecords(0);

        //**********----get Item Details Record from itemList popup ----***************//
        $scope.getListItemMaster = function (dataModel) {

            $scope.IsHiddenDetail = false;

            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListChallanDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListChallanDetails.push({
                    GrrID: 0, GrrDetailID: 0, ItemID: dataModel.ItemID, CompanyID: dataModel.CompanyID, UnitID: dataModel.UnitID, LotID: dataModel.LotID, BatchID: dataModel.BatchID,
                    PackingUnitID: dataModel.PackingUnitID, WeightUnitID: dataModel.WeightUnitID, ItemCode: dataModel.ItemCode, ItemName: dataModel.ItemName,
                    HSCODE: dataModel.HSCODE, UOMName: dataModel.UOMName, Qty: dataModel.Qty, UnitPrice: 0.00, PackingQty: dataModel.PackingQty,
                    PackingUnit: dataModel.PackingUnit, NetWeight: dataModel.NetWeight, GrossWeight: dataModel.GrossWeight, WeightUnit: dataModel.WeightUnit,
                    ExistQty: dataModel.ExistQty, Amount: 0.00, AdditionalQty: 0.00, DisAmount: 0.00, IsPercent: false, TotalAmount: 0.00, AvailableGrrQty: item.AvailableGrrQty
                });
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }


        }

        //**********----Load Challan MasterForm and Challan Details List By select Challan Master ----***************//

        $scope.loadChallaMasterDetailsByGrrNo = function (dataModel) {

            $("#ddlSPRNo").prop("disabled", true);

            $("#dtGRRDate").prop("disabled", true);
            $("#dtGRRDate").prop("readonly", true);

            modal_fadeOut();

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            // 

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnSaveText = "Update";

            $scope.HGRRNo = dataModel.GrrNo;
            $scope.GrrID = dataModel.GrrID;
            $scope.GRRDate = conversion.getDateToString(dataModel.GrrDate);
            $scope.RefChallanDate = conversion.getDateToString(dataModel.RefCHDate);
            $scope.RefChallanNo = dataModel.RefCHNo;
            $scope.ManualGRRNo = dataModel.ManualGrrNo;
            $scope.Description = dataModel.Description;
            $scope.Remarks = dataModel.Remarks;
            //  this page  grr spr   

            $scope.IsSPRType = true;
            $scope.IsLoanTypeOrOthers = false;

            //$scope.lstPartyList = dataModel.UserID;
            //$('#ddlParty').select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });

            $scope.DepartmentID = dataModel.DepartmentID;
            $scope.Department = dataModel.DepartmentName;
          //  $('#ddlWarehouse').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });


            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });
            if (dataModel.FromCompanyID != null) {
                $scope.listCompany.push({
                    CompanyID: dataModel.FromCompanyID, CompanyName: dataModel.FromCompanyName
                });

                $scope.lstCompanyList = dataModel.FromCompanyID;
                $("#ddlCompany").select2("data", { id: dataModel.FromCompanyID, text: dataModel.FromCompanyName });
            }


            $scope.listParty = [];
            $scope.lstPartyList = '';
            $("#ddlParty").select2("data", { id: '', text: '' });

            if (dataModel.UserID != null) {
                $scope.listParty.push({
                    UserID: dataModel.UserID, UserFullName: dataModel.UserFullName
                });
                $scope.lstPartyList = dataModel.UserID;
                $("#ddlParty").select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });
            } 

            // $scope.SprNo = dataModel.RequisitionID;
            //  $('#ddlSPRNo').select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });

            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#ddlSPRNo").select2("data", { id: '', text: '' });
            if (dataModel.RequisitionID != null) {
                $scope.listSprNo.push({
                    RequisitionID: dataModel.RequisitionID, RequisitionNo: dataModel.RequisitionNo
                });
                $scope.SprNo = dataModel.RequisitionID;
                $("#ddlSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.RequisitionNo });
            } 
            //$scope.PONo = dataModel.POID;
            //$('#ddlPONo').select2("data", { id: dataModel.POID, text: dataModel.PONo });
             
            $scope.listPONo = [];
            $scope.PONo = '';
            $("#ddlPONo").select2("data", { id: '', text: '' });

            if (dataModel.POID != null) {
                $scope.listPONo.push({
                    POID: dataModel.POID, PONo: dataModel.PONo
                });
                $scope.PONo = dataModel.POID;
                $("#ddlPONo").select2("data", { id: dataModel.POID, text: dataModel.PONo });
            }  
            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $("#ddlCurrency").select2("data", { id: '', text: '' }); 
            if (dataModel.CurrencyID != null) {
                $scope.listCurrency.push({
                    Id: dataModel.CurrencyID, CurrencyName: dataModel.CurrencyName
                });

                $scope.lstCurrencyList = dataModel.CurrencyID;
                $("#ddlCurrency").select2("data", { id: dataModel.CurrencyID, text: dataModel.CurrencyName });
            }

            $scope.LCNo = dataModel.LCorVoucherorLcafNo; 
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID

            };


            var apiRoute = baseUrl + 'GetGrrDetailByGrrID/';
            var grrid = dataModel.GrrID;
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(grrid) + "]"; 
            var ListGrrDetails = gRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
             
            ListGrrDetails.then(function (response) {
                $scope.ListChallanDetails = response.data.lstChallanDetail;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----delete  Record from ListPIDetails----***************//

        $scope.deleteRow = function (index) { 
            $scope.ListChallanDetails.splice(index, 1); 
        };

        //**********----Create Calculation----***************//
        //$scope.calculation = function (dataModel) {
        //    $scope.ListChallanDetails1 = [];
        //    angular.forEach($scope.ListChallanDetails, function (item) {
        //        var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);
        //        $scope.ListChallanDetails1.push({

        //            GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //            PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //            Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //            GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //            AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount, AvailableGrrQty: item.AvailableGrrQty

        //        });
        //        $scope.ListChallanDetails = $scope.ListChallanDetails1;
        //    });
        //}



        //$scope.calDisAmntNIsPercent = function (dataModel) {
        //    $scope.ListChallanDetails1 = [];
        //    angular.forEach($scope.ListChallanDetails, function (item) {

        //        var chkQty = item.ExistQty + item.AvailableGrrQty;

        //        if (chkQty >= item.Qty) {

        //            var amountInDec = parseFloat(parseFloat(item.Qty) * parseFloat(item.UnitPrice)).toFixed(2);

        //            if (item.IsPercent == true) {
        //                var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(parseFloat(parseFloat(item.DisAmount) / 100) * parseFloat(amountInDec))).toFixed(2);
        //                $scope.ListChallanDetails1.push({

        //                    GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID,
        //                    BatchID: item.BatchID, PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName,
        //                    HSCODE: item.HSCODE, UOMName: item.UOMName, Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit,
        //                    NetWeight: item.NetWeight, GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //                    AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount,
        //                    AvailableGrrQty: item.AvailableGrrQty

        //                });
        //            }
        //            else if (item.IsPercent == false) {
        //                var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(item.DisAmount)).toFixed(2);
        //                $scope.ListChallanDetails1.push({

        //                    GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID,
        //                    BatchID: item.BatchID, PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName,
        //                    HSCODE: item.HSCODE, UOMName: item.UOMName, Qty: item.Qty, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit,
        //                    NetWeight: item.NetWeight,GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: amountInDec,

        //                    AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: TtlAmount,
        //                    AvailableGrrQty: item.AvailableGrrQty

        //                });
        //            }
        //        }
        //        else if (chkQty < item.Qty)
        //        {
        //            Command: toastr["warning"]("GRR Quantity Must Not Greater Than SPR Quantity !!!!");

        //            $scope.ListChallanDetails1.push({

        //                GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID, CompanyID: item.CompanyID, UnitID: item.UnitID, LotID: item.LotID, BatchID: item.BatchID,

        //                PackingUnitID: item.PackingUnitID, WeightUnitID: item.WeightUnitID, ItemCode: item.ItemCode, ItemName: item.ItemName, HSCODE: item.HSCODE, UOMName: item.UOMName,
        //                Qty: 0, UnitPrice: item.UnitPrice, PackingQty: item.PackingQty, PackingUnit: item.PackingUnit, NetWeight: item.NetWeight,
        //                GrossWeight: item.GrossWeight, WeightUnit: item.WeightUnit, ExistQty: item.ExistQty, Amount: item.Amount,

        //                AdditionalQty: item.AdditionalQty, DisAmount: item.DisAmount, IsPercent: item.IsPercent, TotalAmount: item.TotalAmount, AvailableGrrQty: item.AvailableGrrQty

        //            });
        //        }
        //        $scope.ListChallanDetails = $scope.ListChallanDetails1;
        //    }); 
        //}


        $scope.calDisAmntNIsPercent = function (dataModel) {
            // $scope.ListChallanDetails1 = []; 
            var chkQty = dataModel.ExistQty + dataModel.AvailableGrrQty;
            if (chkQty >= dataModel.Qty && dataModel.Qty > 0 &&  dataModel.Qty  !=null)
            {
                var amountInDec = parseFloat(parseFloat(dataModel.Qty) * parseFloat(dataModel.UnitPrice)).toFixed(2);
                if (dataModel.IsPercent == true) {
                    var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(parseFloat(parseFloat(dataModel.DisAmount) / 100) * parseFloat(amountInDec))).toFixed(2);

                        dataModel.Amount=amountInDec;
                        dataModel.TotalAmount =TtlAmount;
                    }
                    else if (item.IsPercent == false) {
                        var TtlAmount = parseFloat(parseFloat(amountInDec) - parseFloat(dataModel.DisAmount)).toFixed(2);
                        dataModel.Amount=amountInDec;
                        dataModel.TotalAmount =TtlAmount;
                    }
            }

            else if (chkQty < dataModel.Qty) {
                Command: toastr["warning"]("GRR Quantity Must Not Greater Than SPR Quantity !!!!"); 
                dataModel.Qty=0; 
            }

            else if (dataModel.Qty < 0) {
                Command: toastr["warning"]("GRR Quantity Must Not Be Negative !!!!"); 
                dataModel.Qty=0; 
            }

            else if ( dataModel.Qty ==null) {
                    Command: toastr["warning"]("GRR Quantity Must Not Be Empty !!!!");
                dataModel.Qty = 0;
            }
        } 

        $scope.LoadLotModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListChallanDetails.indexOf(dataModel);
        }

        $scope.LoadBatchModal = function (dataModel) {
            $scope.ItemIDFlotNbatchSave = dataModel.ItemID;
            $scope.hfIndex = $scope.ListChallanDetails.indexOf(dataModel);
        }

        //// **********----Save Batch using popup----***************//
        $scope.SaveBatch = function () {

            var ManufacDate = conversion.getStringToDate($scope.ManufactureDate);
            var ExpDate = conversion.getStringToDate($scope.ExpaireDate);
            var batchMaster = {
                BatchID: 0,
                BatchNo: $scope.BatchNo,
                BatchTypeID: $scope.lstBatchTypeList,
                // BatchDate: BatchDateStringToDate,
                ItemID: $scope.ItemIDFlotNbatchSave,
                Description: $scope.BatchDecription,
                ManufacturingDate: ManufacDate,
                ExpiryDate: ExpDate,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            };

            var apiRoute = baseUrl + '/SaveBatch/';
            var cmnParam = "[" + JSON.stringify(batchMaster) + "]";
            var SaveNewBatch = gRRService.GetList(apiRoute, cmnParam);

            SaveNewBatch.then(function (response) {
                var result = 0;
                if (response.data != "") {
                    //$scope.HMRRNO = response.data;
                    //varBatchID = response.data;
                    //varBatchName = $scope.BatchNo;

                    $scope.listMrrBatchNo.push({

                        BatchID: response.data,
                        BatchNo: $scope.BatchNo

                    });

                    $scope.ListChallanDetails[$scope.hfIndex] = {

                        GrrID: $scope.ListChallanDetails[$scope.hfIndex].GrrID,
                        GrrDetailID: $scope.ListChallanDetails[$scope.hfIndex].GrrDetailID,
                        ItemID: $scope.ListChallanDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListChallanDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListChallanDetails[$scope.hfIndex].UnitID,
                        LotID: $scope.ListChallanDetails[$scope.hfIndex].LotID,
                        BatchID: response.data,
                        PackingUnitID: $scope.ListChallanDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListChallanDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListChallanDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListChallanDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListChallanDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListChallanDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListChallanDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListChallanDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListChallanDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListChallanDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListChallanDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListChallanDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListChallanDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListChallanDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListChallanDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListChallanDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListChallanDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListChallanDetails[$scope.hfIndex].TotalAmount,
                        AvailableGrrQty: $scope.ListChallanDetails[$scope.hfIndex].AvailableGrrQty
                    };

                    $scope.hfIndex = "";
                    $scope.ItemIDFlotNbatchSave = "0";

                    //$scope.Batch = response.data;
                    //$('#ddlBatch').select2("data", { id: response.data, text: $scope.BatchNo });

                    Command: toastr["success"]("Save  Successfully!!!!");
                    modal_fadeOut_Batch();
                    $scope.EmptyBatchSetupModal();
                }
                else if (response.data == "") {
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Save Not Successfull!!!!");
            });
        }


        //// **********----Save Lot using popup----***************//
        $scope.SaveLot = function () {
            var lotMaster = {
                LotID: 0,
                LotNo: $scope.LotNo,
                LotTypeID: $scope.lstLotTypeList,
                ItemID: $scope.ItemIDFlotNbatchSave,
                BatchID: $scope.lstBatchInLotList,
                Description: $scope.LotDecription,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            }; 

            var apiRoute = baseUrl + '/SaveLot/';
            var cmnParam = "[" + JSON.stringify(lotMaster) + "]";
            var SaveNewLot = gRRService.GetList(apiRoute, cmnParam);

            SaveNewLot.then(function (response) {
                var result = 0;
                if (response.data != "") {

                    //$scope.listLot.push({ 
                    //    LotID: response.data,
                    //    LotNo: $scope.LotNo

                    //});

                    $scope.listMrrLot.push({
                        LotID: response.data,
                        LotNo: $scope.LotNo
                    });

                    // $scope.listWorkFlowDetail[$scope.hfIndex] = { WorkFlowID: $scope.WorkFlowID, WorkFlowDetailID: $scope.WorkFlowDetailID, StatusID: $("#ddlStatus").select2('data').id, StatusName: $("#ddlStatus").select2('data').text, EmployeeID: $("#ddlStatusBy").select2('data').id, UserFirstName: $("#ddlStatusBy").select2('data').text, MenuName: $("#ddlMenu").select2('data').text, CompanyID: $scope.lstCompanyList, Sequence: $scope.Sequence };

                    $scope.ListChallanDetails[$scope.hfIndex] = {

                        GrrID: $scope.ListChallanDetails[$scope.hfIndex].GrrID,
                        GrrDetailID: $scope.ListChallanDetails[$scope.hfIndex].GrrDetailID,
                        ItemID: $scope.ListChallanDetails[$scope.hfIndex].ItemID,
                        CompanyID: $scope.ListChallanDetails[$scope.hfIndex].CompanyID,
                        UnitID: $scope.ListChallanDetails[$scope.hfIndex].UnitID,
                        LotID: response.data, //.LotID,
                        BatchID: $scope.ListChallanDetails[$scope.hfIndex].BatchID,
                        PackingUnitID: $scope.ListChallanDetails[$scope.hfIndex].PackingUnitID,
                        WeightUnitID: $scope.ListChallanDetails[$scope.hfIndex].WeightUnitID,
                        ItemCode: $scope.ListChallanDetails[$scope.hfIndex].ItemCode,
                        ItemName: $scope.ListChallanDetails[$scope.hfIndex].ItemName,
                        HSCODE: $scope.ListChallanDetails[$scope.hfIndex].HSCODE,
                        UOMName: $scope.ListChallanDetails[$scope.hfIndex].UOMName,
                        Qty: $scope.ListChallanDetails[$scope.hfIndex].Qty,
                        UnitPrice: $scope.ListChallanDetails[$scope.hfIndex].UnitPrice,
                        PackingQty: $scope.ListChallanDetails[$scope.hfIndex].PackingQty,
                        PackingUnit: $scope.ListChallanDetails[$scope.hfIndex].PackingUnit,
                        NetWeight: $scope.ListChallanDetails[$scope.hfIndex].NetWeight,
                        GrossWeight: $scope.ListChallanDetails[$scope.hfIndex].GrossWeight,
                        WeightUnit: $scope.ListChallanDetails[$scope.hfIndex].WeightUnit,
                        ExistQty: $scope.ListChallanDetails[$scope.hfIndex].ExistQty,
                        Amount: $scope.ListChallanDetails[$scope.hfIndex].Amount,

                        AdditionalQty: $scope.ListChallanDetails[$scope.hfIndex].AdditionalQty,
                        DisAmount: $scope.ListChallanDetails[$scope.hfIndex].DisAmount,
                        IsPercent: $scope.ListChallanDetails[$scope.hfIndex].IsPercent,
                        TotalAmount: $scope.ListChallanDetails[$scope.hfIndex].TotalAmount,
                        AvailableGrrQty: $scope.ListChallanDetails[$scope.hfIndex].AvailableGrrQty
                    };

                    $scope.hfIndex = "";
                    $scope.ItemIDFlotNbatchSave = "0";

                    //$scope.lstLot = response.data;
                    //$('#ddlLotNo').select2("data", { id: response.data, text: $scope.LotNo });

                    //  $scope.lstLot = response.data;
                    //  $('#ddlMrrLot').select2("data", { id: response.data, text: $scope.LotNo });


                    Command: toastr["success"]("Save  Successfully!!!!");
                    modal_fadeOut_Lot();
                    $scope.EmptyLotSetupModal();
                    // $scope.clear(); 
                }
                else if (response.data == "") {
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Save Not Successfull!!!!");
            });
        }


        //**********----Save and Update InvRChallanMaster and InvRChallanDetail  Records----***************//
        $scope.save = function () {

            var issueID = $scope.lstIssueNoList == null ? 0 : $scope.lstIssueNoList;

            if ($scope.ManualGRRNo == "" || typeof ($scope.ManualGRRNo) === "undefined") {
                Command: toastr["warning"]("Please enter manual GRR No!!!!");
                $('#txtGRRNo').focus()
                return;
            }
            if ($scope.ChkRefChallanNoWhnSave == "") {

                if ($scope.ChkRefChallanNoWhnSaveChallan == "") {             
                    var HedarTokenPostPut = $scope.GrrID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

                    $("#save").prop("disabled", true);                                                     
                    var NewStringToDate = conversion.getStringToDate($scope.GRRDate);
                    var RefCHDateToDate = conversion.getStringToDate($scope.RefChallanDate);


                    var itemMaster = {
                        GrrID: $scope.GrrID,
                        GrrNo: $scope.HGRRNo,
                        ManualGrrNo: $scope.ManualGRRNo,
                        GrrDate: NewStringToDate,
                        RefCHNo: $scope.RefChallanNo,
                        RefCHDate: RefCHDateToDate,
                        // TypeID: $scope.lstChallanTypeList,
                        // CHTypeID: $scope.lstChallanTypeList,
                        SupplierID: $scope.lstPartyList,
                        CurrencyID: $scope.lstCurrencyList,
                        TransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID, //$scope.lstChallanTypeList,
                        RequisitionID: $scope.SprNo,
                        IssueID: issueID,
                        POID: $scope.PONo,
                        //  PIID: $scope.PINO,
                        LoadingPortID: $scope.LoadingLocation,
                        DischargePortID: $scope.DischargeLocation,
                        Remarks: $scope.Remarks,
                        Description: $scope.Description,
                        CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                        DepartmentID: $scope.DepartmentID, //$scope.UserCommonEntity.loggedUserDepartmentID, 
                        IsDeleted: false,
                        CreateBy: $scope.GRRBy 


                        // MenuID: $scope.MenuID //26

                    };

                    var fileList = [];
                    angular.forEach($scope.files, function (item) {
                        this.push(item.name);
                    }, fileList);

                    //if (fileList.length == 0) {
                    //    $("#save").prop("disabled", false);
                    //    Command: toastr["warning"]("Please attach GRR document.");
                    //    return;
                    //}


                    var menuID = $scope.UserCommonEntity.currentMenuID;
                    var itemMasterDetail = $scope.ListChallanDetails;
                    var chkQty = 1;
                    angular.forEach($scope.ListChallanDetails, function (item) {

                        if (item.Qty <= 0 || item.Qty == "") {
                            chkQty = 0;
                        }
                    });

                    if ($scope.ListChallanDetails.length > 0) {

                        if (chkQty == 1) {
                            var apiRoute = baseUrl + 'SaveUpdateChallanMasterNdetails/'; 
                            var cmnParam = "[" + JSON.stringify(itemMaster) + "," + JSON.stringify(itemMasterDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(fileList) + "]";

                            var ChallanItemMasterNdetailsCreateUpdate = gRRService.GetList(apiRoute, cmnParam, HedarTokenPostPut); 
                            ChallanItemMasterNdetailsCreateUpdate.then(function (response) {
                                var result = 0;
                                if (response.data != "") {

                                    
                                    ///// start file upload/////////////

                                    var data = new FormData();

                                    for (var i in $scope.files) {
                                        data.append("uploadedFile", $scope.files[i]);
                                    }
                                    data.append("uploadedFile", response.data);
                                    // ADD LISTENERS.
                                    var objXhr = new XMLHttpRequest();
                                    //objXhr.addEventListener("progress", updateProgress, false);
                                    //objXhr.addEventListener("load", transferComplete, false);


                                    var apiRoute = baseUrl + 'UploadFiles/';
                                    objXhr.open("POST", apiRoute);
                                    objXhr.send(data);
                                    // debugger;
                                    document.getElementById('file').value = '';
                                    $scope.files = [];

                                    /////////// end file upload /////////////////



                                    Command: toastr["success"]("Save  Successfully!!!!");
                                    $scope.clear();
                                    $scope.HGRRNo = response.data;

                                }
                                else if (response.data == "") {
                                        Command: toastr["warning"]("Save Not Successfull!!!!");
                                    $("#save").prop("disabled", false);
                                }
                            },
                            function (error) {
                                $("#save").prop("disabled", false);
                                Command: toastr["warning"]("Save Not Successfull!!!!");
                            });
                        }
                        else if (chkQty == 0) {
                            $("#save").prop("disabled", false);
                            Command: toastr["warning"]("GRR Quantity Must Not Zero Or Empty !!!!");

                        }

                    }
                    else if ($scope.ListChallanDetails.length <= 0) {
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("GRR Detail Must Not Empty!!!!");
                    }
                }
                else if ($scope.ChkRefChallanNoWhnSaveChallan != "") {

                        Command: toastr["warning"]("Ref Challan No Already Exists.");
                }
            }
            else if ($scope.ChkRefChallanNoWhnSave != "") {

                    Command: toastr["warning"]("Ref GRR No Already Exists.");

            }

        };

        $scope.files = [];
        $scope.getFileDetails = function (e) {
            $scope.$apply(function () {

                /// file validation //////////////
                $scope.file = e.files[0];

                if ($scope.file.size > 200000000) {
                    // alert('file size should not be greater than 200 MB');
                    Command: toastr["warning"]("file size should not be greater than 200 MB!!!!");
                    return;
                }

                var allowed = ["jpeg", "png", "gif", "jpg", "pdf"];
                var found = false;
                //var img;
                //img = new Image();
                allowed.forEach(function (extension) {
                    if ($scope.file.type.match('image/' + extension)) {
                        found = true;
                    }
                    if ($scope.file.type.match('application/' + 'pdf')) {
                        found = true;
                    }
                    //if ($scope.file.type.match('application/' + 'msword')) {
                    //    found = true;
                    //}
                });
                if (!found) {
                    //alert('file type should be .jpeg, .png, .jpg, .gif, .pdf, .doc');
                    Command: toastr["warning"]("file type should be .jpeg, .png, .jpg, .gif, .pdf!!!!");
                    return;
                }

                //// file validation /////////////


                // STORE THE FILE OBJECT IN AN ARRAY.
                for (var i = 0; i < e.files.length; i++) {
                    $scope.files.push(e.files[i])
                }
            });
        };

        $scope.getFileInfo = function (dataModel) {
            $scope.ListFileDetails = [];
            var id = dataModel.GrrID;
            // var apiRoute = baseUrl + 'GetFileDetailsById/' + id;
            var TransTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id + '/' + TransTypeID;

            var ListFileDetails = gRRService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----Reset Record----***************//

        $scope.EmptyLotSetupModal = function () {

            $scope.ItemIDFlotNbatchSave = "0";
            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.LotID = '';
            $scope.LotNo = '';
            $scope.LotDecription = '';
            $scope.lstBatchInLotList = '';
            $scope.listBatchInLot = [];
            GetAllBatchNo();
        }

        $scope.EmptyBatchSetupModal = function () {

            $scope.ItemIDFlotNbatchSave = "0";

            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.ManufactureDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.ExpaireDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.BatchID = '';
            $scope.BatchNo = '';
            $scope.BatchDecription = '';
        }


        $scope.clear = function () {

            $("#ddlSPRNo").prop("disabled", false);
            $("#dtGRRDate").prop("disabled", false);
            $("#dtGRRDate").prop("readonly", false);
            $scope.ManualGRRNo = "";
            $scope.IsSPRType = true;
            $scope.IsLoanTypeOrOthers = false;
            //  $scope.IsSPRType = true;
            var date = new Date();
            $scope.GRRDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.RefChallanDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.RefChallanNo = ""; 
            $scope.GrrID = "0";

            chkTrnsTypeFHideShow();


            $scope.HGRRNo = "";
            GetAllUsers(0); 

            $scope.listSampleNo = [];
            $scope.lstSampleNoList = '';
            $('#ddlSampleNo').select2("data", { id: '', text: '--Select Sample No--' });

            $scope.listParty = [];
            $scope.lstPartyList = "";
            $('#ddlParty').select2("data", { id: "", text: "--Select Party--" });


            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $('#ddlCurrency').select2("data", { id: '', text: '--Select Currency--' });

            $scope.listSprNo = [];
            $scope.SprNo = "";
            $('#ddlSPRNo').select2("data", { id: '', text: '--Select--' });

            $scope.listPONo = [];
            $scope.PONo = "";
            $('#ddlPONo').select2("data", { id: '', text: '--Select PO No--' });

            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });
         
            //$scope.listPINO = [];
            //$scope.PINO = "";
            //$('#ddlPINO').select2("data", { id: '', text: '--Select PI No--' });

            //$scope.listLCNo = [];
            //$scope.LCNo = "";
            //$('#ddlLCNo').select2("data", { id: '', text: '--Select LC No--' });

            $scope.LCNo = "";

           // $scope.listWarehouse = [];
            $scope.DepartmentID = '';
            $scope.Department = '';
           // $('#ddlWarehouse').select2("data", { id: '', text: '--Select Wherehouse--' });
             
            $scope.Remarks = ""; 
            $scope.Description = ""; 
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.ListChallanDetails = [];

            // $scope.loadPORecords();

            $scope.loadSPRRecords(); 
            loadPartyRecords(0);
            loadCurrencyRecords(0);
            loadLoggedDept();
           // loadWherehouse(0);

            //  loadChallanTrnsTypes(0);
            //  loadLoadingLocation(0);
            loadPackingUnit(0);
            loadWeightUnit(0);
            $scope.loadChallanRecords(0);
            $scope.getTypes();
            //  $scope.getItemGroupsByType();

        };

    }]);


function modal_fadeOut() {
    $("#ChallanMasterModal").fadeOut(200, function () {
        $('#ChallanMasterModal').modal('hide');
    });
}

function modal_fadeOut_Lot() {
    $("#LotSetupModal").fadeOut(200, function () {
        $('#LotSetupModal').modal('hide');
    });
}

function modal_fadeOut_Batch() {
    $("#BatchSetupModal").fadeOut(200, function () {
        $('#BatchSetupModal').modal('hide');
    });
}
