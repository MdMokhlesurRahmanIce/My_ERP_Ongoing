/**
* MRRCtrl.js
*/


app.controller('mRRCtrl', ['$scope', 'mRRService', 'qcService', 'RequisitionService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, mRRService, qcService, RequisitionService, conversion, $filter, $localStorage, uiGridConstants) {

        $scope.gridOptionsMrrMaster = [];
        $scope.gridOptionslistItemMaster = [];
        $scope.ChkManualMRRNoWhnSave = "";


        var objcmnParam = {};
        //*************---Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnMrrShowText = "Show List";
                $scope.IsShow = true;

                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnMrrShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;

                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                $scope.loadMrrRecords(0);
            }
        }


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


            ////Coming from SideNavCrl
            //$scope.UserCommonEntity = {};
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

        //$scope.ChkLDeptWithLComForMRRACC = 0;

        //function SetChkLDeptWithLComForMRRACC(){
        //    if ($scope.UserCommonEntity.loggedCompnyID == 1 && $scope.UserCommonEntity.loggedUserDepartmentID == 16) {
        //        $scope.ChkLDeptWithLComForMRRACC = 1;
        //    }
        //    else if ($scope.UserCommonEntity.loggedCompnyID == 2 && $scope.UserCommonEntity.loggedUserDepartmentID == 32) {
        //        $scope.ChkLDeptWithLComForMRRACC = 1;
        //    }
        //}
        //SetChkLDeptWithLComForMRRACC();

        $scope.IsIssueReceive = false;
        $scope.IsMrr = false; 
        $scope.IsReturnReceive = false;
        $scope.IsLoanReceive = false;

        $scope.CommonFLoanReceiveNMrr = false;

        $scope.CommonFIssueNReturnReceive = false;
        $scope.TrnsTypeFLoadDDLIssue = 0;
    
        
        $scope.grdTSprDate = "";
        $scope.grdTSprNo = "";

        $scope.FrmDeptIDByIssueChnge = null;
        function chkTrnsTypeFHideShow() {
            if ($scope.UserCommonEntity.currentTransactionTypeID == 24) {  //  Issue Receive 
                 
                $scope.TrnsTypeFLoadDDLIssue = 13;
                $scope.IsIssueReceive = true;
                $scope.CommonFIssueNReturnReceive = true;
                $scope.IsMrr = false;
                $scope.CommonFLoanReceiveNMrr = false;
                $scope.IsLoanReceive = false;
                $scope.IsReturnReceive = false;
                $scope.grdTMrNo = "Receive No";
                $scope.grdTMrDate = "Receive Date";
                

                // for master load
             

                $scope.PageTitle = 'Issue Receive Creation';
                $scope.ListTitle = 'Issue Receive Records';
                $scope.ListTitleMrrMasters = 'Issue Receive  Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleMrrDeatails = 'Issue Receive Information (Details)';
            }
            else if ($scope.UserCommonEntity.currentTransactionTypeID == 25) {  //  Return Receive 
                $scope.TrnsTypeFLoadDDLIssue = 16;
                $scope.IsIssueReceive = false;
                $scope.CommonFIssueNReturnReceive = true;
                $scope.IsMrr = false;
                $scope.CommonFLoanReceiveNMrr = false;
                $scope.IsLoanReceive = false;
                $scope.IsReturnReceive = true;

                $scope.grdTMrNo = "Return No";
                $scope.grdTMrDate = "Return Date";

                // for master load
                //$scope.IsStoreCompleted = false;
                //$scope.IsApproved = false;
                //$scope.IsAccountsCompleted = false;

                $scope.PageTitle = 'Return Receive Creation';
                $scope.ListTitle = 'Return Receive Records';
                $scope.ListTitleMrrMasters = 'Return Receive  Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleMrrDeatails = 'Return Receive Information (Details)';
            }

            else if ($scope.UserCommonEntity.currentTransactionTypeID == 10) {  // for  store mrr
                $scope.IsIssueReceive = false;
                $scope.CommonFIssueNReturnReceive = false;
                $scope.IsMrr = true;
                $scope.CommonFLoanReceiveNMrr = true;
                $scope.IsLoanReceive = false; 
                $scope.IsReturnReceive = false;

                $scope.grdTMrNo = "MRR No";
                $scope.grdTMrDate = "MRR Date";
                $scope.grdTSprDate = "SPR Date";
                $scope.grdTSprNo = "SPR No";


                // for master load
                //$scope.IsStoreCompleted = true;
                //$scope.IsApproved = false;
                //$scope.IsAccountsCompleted = false;


                $scope.PageTitle = 'MRR Creation';
                $scope.ListTitle = 'MRR Records';
                $scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleMrrDeatails = 'MRR Information (Details)';

                //if ($scope.ChkLDeptWithLComForMRRACC == 1) // for accounts
                //{
                //    $scope.IsMrrAccountsDept = true;
                //    $scope.IsMrr = false;

                //    $scope.IsHiddenDetail = true;
                //    $scope.btnMrrShowText = "Create";
                //    $scope.IsShow = false;
                //    $scope.IsHidden = false; 
                //    $scope.IsCreateIcon = true;
                //    $scope.IsListIcon = false;
                //    // $scope.loadMrrRecords();

                //    // for master load
                //    $scope.IsStoreCompleted = true;
                //    $scope.IsApproved = false;
                //    $scope.IsAccountsCompleted = true;
                     
                //}
                //else if ($scope.ChkLDeptWithLComForMRRACC != 1) {
                //    $scope.IsMrrAccountsDept = false;
                //    $scope.IsMrr = true;

                //    // for master load
                //    $scope.IsStoreCompleted = true;
                //    $scope.IsApproved = false;
                //    $scope.IsAccountsCompleted = false;
                //}
            }
            else if ($scope.UserCommonEntity.currentTransactionTypeID == 32) {  //  for loan receive 
               
                $scope.IsIssueReceive = false;
                $scope.CommonFIssueNReturnReceive = false;
                $scope.IsMrr = false;
                $scope.CommonFLoanReceiveNMrr = true;
                $scope.IsLoanReceive = true;
                $scope.IsReturnReceive = false;
                 

                $scope.grdTMrNo = "Loan Receive No";
                $scope.grdTMrDate = "Loan Receive Date";

                $scope.grdTSprDate = "Loan Date";
                $scope.grdTSprNo = "Loan Order No";

                // for master load
                //$scope.IsStoreCompleted = false;
                //$scope.IsApproved = false;
                //$scope.IsAccountsCompleted = false;

                $scope.PageTitle = 'Loan Receive Creation';
                $scope.ListTitle = 'Loan Receive Records';
                $scope.ListTitleMrrMasters = 'Loan Receive  Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleMrrDeatails = 'Loan Receive Information (Details)';
            }
        }

        chkTrnsTypeFHideShow();

        $scope.FrmDeptIDByIssueChnge = null;

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.listWarehouse = [];

        $scope.listMRRType = [];
        $scope.listMRRQCNo = [];
        $scope.listLot = [];

        $scope.btnModal = "Add";
        $scope.btnLotModal = "Save";
        $scope.btnBatchModal = "Save";
        $scope.listMrrLot = [];
        $scope.listMrrBatchNo = [];
        $scope.LotSetup = "Lot Setup";
        $scope.BatchSetup = "Batch Setup";
        $scope.IsItem = false;
        var varLotID = "";
        var varLotName = "";
        var varBatchID = "";
        var varBatchName = "";

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



        var baseUrl = '/Inventory/api/MRR/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        var date = new Date();
        $scope.MrrDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.ManufactureDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.ExpDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        // $scope.LgUser = $('#hUserID').val();

        //   only qc item valid for  mrr  so  IsPostMRR  always = true
        //$scope.IsPostMRR = true;
        //$scope.IsPreMRR = false;
        $scope.MrrID = "0";


        //$scope.TransactionTypeID = 10;
        //$scope.MenuID = 85;



        $scope.btnMrrSaveText = "Save";
        $scope.btnMrrShowText = "Show List";
        $scope.btnMrrReviseText = "Update";
        //$scope.PageTitle = 'Create MRR';
        //$scope.ListTitle = 'MRR Records';
        //$scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
        //$scope.ListTitleSampleNo = 'Sample Info';
        //$scope.ListTitleMrrDeatails = 'MRR Information (Details)';

        $scope.ListMrrDetails = [];
        $scope.ListMrrMaster = [];

        


        //**********---- Get Mrrby Records ----*************** //

        function loadMrrByRecords(isPaging) {

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = '/Inventory/api/QC/GetUser/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listuser = mRRService.GetList(apiRoute, cmnParam);
            listuser.then(function (response) {
                $scope.listMRRBy = response.data;
                angular.forEach($scope.listMRRBy, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {

                        $scope.ngmMRRByList = item.UserID;
                        $("#ddlMRRBy").select2("data", { id: item.UserID, text: item.UserFullName });

                        return false;
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadMrrByRecords(0);



        //**********---- Get Wherehouse Records ----*************** //


        function loadWherehouse(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = baseUrl + 'GetWherehouseList/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            $scope.listWarehouse = [];
            var listWherehouse = mRRService.GetList(apiRoute, cmnParam);
            listWherehouse.then(function (response) {
                //  $scope.listWarehouse = response.data.lstWherehouse;
                // $scope.listWarehouse = response.data.lstWherehouse[1].ListDpt;
                if($scope.UserCommonEntity.loggedCompnyID==1)
                    $scope.listWarehouse.push({ OrganogramID: response.data.lstWherehouse[1].ListDpt[5].OrganogramID, OrganogramName: response.data.lstWherehouse[1].ListDpt[5].OrganogramName })
                else if($scope.UserCommonEntity.loggedCompnyID==12)
                $scope.listWarehouse.push({ OrganogramID: response.data.lstWherehouse[1].ListDpt[5].OrganogramID, OrganogramName: response.data.lstWherehouse[1].ListDpt[5].OrganogramName })

                //    .lstWherehouse[1].ListDpt;
                //$scope.listWarehouseLocation = response.data.lstWherehouse.ListDpt.ListBrn;
                //$scope.listSelfNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf;
                //$scope.listRackNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf.ListRack;
                // console.log(response.data.lstWherehouse);
            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadWherehouse(0);

        function loadRecords_OrganogramDept(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganogramsDept = mRRService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
            listOrganogramsDept.then(function (response) {
                $scope.listOrganogramsDepartment = response.data;

                angular.forEach($scope.listOrganogramsDepartment, function (item) {
                    if (item.OrganogramID == $scope.UserCommonEntity.loggedUserDepartmentID) {
                        $scope.Dept = item.OrganogramID;
                        $('#ddlDept').select2("data", { id: item.OrganogramID, text: item.OrganogramName });

                        return false;
                    }
                });

            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_OrganogramDept(0);

        function loadIssueRecords(isPaging) {

            if ($scope.TrnsTypeFLoadDDLIssue > 0) {
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.TrnsTypeFLoadDDLIssue
                };

                var apiRoute = baseUrl + 'GetIssueNo/';
                var Issues = mRRService.GetIssueNo(apiRoute, objcmnParam);
                Issues.then(function (response) {
                    $scope.listIssueNo = response.data.lstIssue;


                    //console.log($scope.listBuyer);
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        loadIssueRecords(0);

        ////#########  load DDL  LoadSprLoanOrderNo ###############//

        //function LoadSprLoanOrderNo()
        //{
        //    if ($scope.UserCommonEntity.currentTransactionTypeID == 32)
        //        {


        //            objcmnParam = {
        //                pageNumber: page,
        //                pageSize: pageSize,
        //                IsPaging: isPaging,
        //                loggeduser: $scope.UserCommonEntity.loggedUserID,
        //                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //                menuId: $scope.UserCommonEntity.currentMenuID,
        //                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //            };

        //            var apiRoute = baseUrl + 'GetSprLoanList/';
        //            var loanTypeSpr = "9";
        //            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(loanTypeSpr) + "]";

        //            var listGetSprLoan = mRRService.GetList(apiRoute, cmnParam);
        //            listGetSprLoan.then(function (response) {
        //                $scope.listSprNo = response.data.lstSprLoan;

        //            },
        //                function (error) {
        //                    console.log("Error: " + error);
        //                    });
        //    }
        //}
        //LoadSprLoanOrderNo();

        //$scope.LoadLoanItemBSprNo = function()
        //{
        //    var loanSprNo = $scope.SprNo;

        //    angular.forEach($scope.listSprNo, function (item) {
        //        if (loanSprNo == item.RequisitionID) {
                    
        //            $scope.listCompany = [];
        //            $scope.lstCompanyList = '';
        //            $("#ddlCompany").select2("data", { id: '', text: '' });
        //            //  if (response.data.lstQC[0].CurrencyID != null) { 
        //            $scope.listCompany.push({
        //                CompanyID: item.CompanyID, CompanyName: item.CompanyName
        //                });

        //            $scope.lstCompanyList = item.CompanyID;
        //            $("#ddlCompany").select2("data", { id: item.CompanyID, text: item.CompanyName });

        //            $scope.SPRDate = item.SPRDate == null ? "" : conversion.getDateToString(item.SPRDate);

        //            return false;
        //        }
        //    });


        //    $scope.MrrID = "0";
        //    $scope.btnMrrSaveText = "Save";

        //    $scope.ListMrrDetails = [];
          
        //    $scope.IsMrr = false;
        //    $scope.CommonFLoanReceiveNMrr = true;
        //    $scope.IsIssueReceive = false;
        //    $scope.IsReturnReceive = false;
        //    $scope.CommonFIssueNReturnReceive = false;

        //    $scope.IsHiddenDetail = false;

        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        selectedCompany:$scope.lstCompanyList,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };


        //    if (loanSprNo != "") {
        //        var apiRoute = baseUrl + 'GetDetailInfoByLoanSprID/';

        //        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(loanSprNo) + "]";

        //        var loanItemDetails = mRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);;
        //        loanItemDetails.then(function (response) {
        //            $scope.ListMrrDetails = response.data.lstQC;
        //            $scope.QCCertificateNo = response.data.lstQC[0].QCCertificateNo;


        //            $scope.listSupplier = [];
        //            $scope.lstSupplierList = '';
        //            $("#ddlSupplier").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].SupplierID != null) {
        //                $scope.listSupplier.push({
        //                    SupplierID: response.data.lstQC[0].SupplierID, SupplierName: response.data.lstQC[0].SupplierName
        //                });

        //                $scope.lstSupplierList = response.data.lstQC[0].SupplierID;
        //                $("#ddlSupplier").select2("data", { id: response.data.lstQC[0].SupplierID, text: response.data.lstQC[0].SupplierName });
        //            }

        //            if ($scope.UserCommonEntity.currentTransactionTypeID != 32) {  //  for loan receive 
        //                $scope.listSprNo = [];
        //                $scope.SprNo = '';
        //                $("#txtSPRNo").select2("data", { id: '', text: '' });
        //                if (response.data.lstQC[0].RequisitionID != null) {
        //                    $scope.listSprNo.push({
        //                        RequisitionID: response.data.lstQC[0].RequisitionID, SprNo: response.data.lstQC[0].RequisitionNo
        //                    });

        //                    $scope.SprNo = response.data.lstQC[0].RequisitionID;
        //                    $("#txtSPRNo").select2("data", { id: response.data.lstQC[0].RequisitionID, text: response.data.lstQC[0].RequisitionNo });
        //                }
        //            }

        //            $scope.listCurrency = [];
        //            $scope.lstCurrencyList = '';
        //            $("#ddlCurrency").select2("data", { id: '', text: '' });
        //            if (response.data.lstQC[0].CurrencyID != null) {
        //                $scope.listCurrency.push({
        //                    Id: response.data.lstQC[0].CurrencyID, CurrencyName: response.data.lstQC[0].CurrencyName
        //                });

        //                $scope.lstCurrencyList = response.data.lstQC[0].CurrencyID;
        //                $("#ddlCurrency").select2("data", { id: response.data.lstQC[0].CurrencyID, text: response.data.lstQC[0].CurrencyName });
        //            }


        //            $scope.listPONo = [];
        //            $scope.PONo = '';
        //            $("#txtPONo").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].POID != null) {
        //                $scope.listPONo.push({
        //                    POID: response.data.lstQC[0].POID, PONo: response.data.lstQC[0].PONo
        //                });

        //                $scope.PONo = response.data.lstQC[0].POID;
        //                $("#txtPONo").select2("data", { id: response.data.lstQC[0].POID, text: response.data.lstQC[0].PONo });
        //            }


        //            $scope.listWarehouse = [];
        //            $scope.Warehouse = '';
        //            $("#ddlWarehouse").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].DepartmentID != null) {
        //                $scope.listWarehouse.push({
        //                    OrganogramID: response.data.lstQC[0].DepartmentID, OrganogramName: response.data.lstQC[0].DepartmentName
        //                });

        //                $scope.Warehouse = response.data.lstQC[0].DepartmentID;
        //                $("#ddlWarehouse").select2("data", { id: response.data.lstQC[0].DepartmentID, text: response.data.lstQC[0].DepartmentName });
        //            } 
        //            $scope.listGRRNo = [];
        //            $scope.lstGRRNoList = '';
        //            $("#ddlGRRNo").select2("data", { id: '', text: '' });

        //            if (response.data.lstQC[0].GrrID != null) {
        //                $scope.listGRRNo.push({
        //                    GrrID: response.data.lstQC[0].GrrID, GrrNo: response.data.lstQC[0].GrrNo
        //                });
        //                $scope.lstGRRNoList = response.data.lstQC[0].GrrID;
        //                $("#ddlGRRNo").select2("data", { id: response.data.lstQC[0].GrrID, text: response.data.lstQC[0].GrrNo });
        //            }


        //            $scope.LCNO = response.data.lstQC[0].LCNO;

        //            $scope.RefChallanNo = response.data.lstQC[0].RefCHNo;



        //            //var grrdate = response.data.lstQC[0].GrrDate;

        //            //  console.log(response.data.lstQC[0].GRRDate);
        //            //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

        //            $scope.SPRDate = response.data.lstQC[0].SPRDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].SPRDate);
        //            $scope.PODate = response.data.lstQC[0].PODate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PODate);
        //            // $scope.PIDate = response.data.lstQC[0].PIDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PIDate);
        //            $scope.QCDate = response.data.lstQC[0].MrrQcDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].MrrQcDate);
        //            $scope.GRRDate = response.data.lstQC[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].GrrDate);


        //        },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //    }

        //}

        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateNo = function () {
             
            var getMNo = $scope.ManualMRRNo;

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
                var getDuplicateNo = mRRService.GetList(apiRoute, cmnParam);

                getDuplicateNo.then(function (response) {

                    $scope.ChkManualMRRNoWhnSave = "";

                    if (response.data.length > 0) {

                        $scope.ChkManualMRRNoWhnSave = response.data[0].ManualMRRNo
                        $scope.ManualMRRNo = "";
                        Command: toastr["warning"]("Manual MRR No Already Exists.");
                    } 
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                Command: toastr["warning"]("Please Enter MRR No.");
            }

        }

        //######## End Check Duplicate No ################//



        //##############################################################################################  start  tree dropdown   ######################################################################// 






        //##############################################################################################  end  tree dropdown   ######################################################################// 



        //**********---- Get QCNo Records ----*************** //


        function loadQCNo(isPaging) {
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var apiRoute = baseUrl + 'GetQCList/';
            var TransType = 0;
            if($scope.IsMrr==true)
            {
                TransType = 19;
            }
            else if ($scope.IsLoanReceive == true) {
                TransType = 37;
            }

            var cmnParam = "[" + JSON.stringify(objcmnParam) + ',' + JSON.stringify(TransType) + "]";

            var listMRRQCNo = mRRService.GetList(apiRoute, cmnParam);
            listMRRQCNo.then(function (response) {
                $scope.listMRRQCNo = response.data.lstQCByGrrNo;

            },
                function (error) {
                    console.log("Error: " + error);
                });

        }
        loadQCNo(0);


        //**********---- QC Detail from QCNo  Changes ----***************//

        $scope.getItemDetailByQCNo = function () {

            $scope.MrrID = "0";
            $scope.btnMrrSaveText = "Save";

            $scope.ListMrrDetails = [];
            var qcNo = $scope.lstQCNoList;
           
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


            if (qcNo != "") {
                var apiRoute = baseUrl + 'GetDetailInfoByQCID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(qcNo) + "]";

                var QCDetails = mRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);;
                QCDetails.then(function (response) {
                    $scope.ListMrrDetails = response.data.lstQC;
                    $scope.QCCertificateNo = response.data.lstQC[0].QCCertificateNo;

                    if (response.data.lstQC[0].FromCompanyID != null) {
                        $scope.listCompany = [];
                        $scope.lstCompanyList = '';
                        $("#ddlCompany").select2("data", { id: '', text: '' });
                        //  if (response.data.lstQC[0].CurrencyID != null) { 
                        $scope.listCompany.push({
                            CompanyID: response.data.lstQC[0].FromCompanyID, CompanyName: response.data.lstQC[0].FromCompanyName
                        });

                        $scope.lstCompanyList = response.data.lstQC[0].FromCompanyID;
                        $("#ddlCompany").select2("data", { id: response.data.lstQC[0].FromCompanyID, text: response.data.lstQC[0].FromCompanyName });
                    }


                    $scope.listSupplier = [];
                    $scope.lstSupplierList = '';
                    $("#ddlSupplier").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].SupplierID != null) {
                        $scope.listSupplier.push({
                            SupplierID: response.data.lstQC[0].SupplierID, SupplierName: response.data.lstQC[0].SupplierName
                        });

                        $scope.lstSupplierList = response.data.lstQC[0].SupplierID;
                        $("#ddlSupplier").select2("data", { id: response.data.lstQC[0].SupplierID, text: response.data.lstQC[0].SupplierName });
                    }

                    $scope.listSprNo = [];
                    $scope.SprNo = '';
                    $("#txtSPRNo").select2("data", { id: '', text: '' });
                    if (response.data.lstQC[0].RequisitionID != null) {
                        $scope.listSprNo.push({
                            RequisitionID: response.data.lstQC[0].RequisitionID, SprNo: response.data.lstQC[0].RequisitionNo
                        });

                        $scope.SprNo = response.data.lstQC[0].RequisitionID;
                        $("#txtSPRNo").select2("data", { id: response.data.lstQC[0].RequisitionID, text: response.data.lstQC[0].RequisitionNo });
                    }

                    $scope.listCurrency = [];
                    $scope.lstCurrencyList = '';
                    $("#ddlCurrency").select2("data", { id: '', text: '' });
                    if (response.data.lstQC[0].CurrencyID != null) {
                        $scope.listCurrency.push({
                            Id: response.data.lstQC[0].CurrencyID, CurrencyName: response.data.lstQC[0].CurrencyName
                        });

                        $scope.lstCurrencyList = response.data.lstQC[0].CurrencyID;
                        $("#ddlCurrency").select2("data", { id: response.data.lstQC[0].CurrencyID, text: response.data.lstQC[0].CurrencyName });
                    }


                    $scope.listPONo = [];
                    $scope.PONo = '';
                    $("#txtPONo").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].POID != null) {
                        $scope.listPONo.push({
                            POID: response.data.lstQC[0].POID, PONo: response.data.lstQC[0].PONo
                        });

                        $scope.PONo = response.data.lstQC[0].POID;
                        $("#txtPONo").select2("data", { id: response.data.lstQC[0].POID, text: response.data.lstQC[0].PONo });
                    }


                    $scope.listWarehouse = [];
                    $scope.Warehouse = '';
                    $("#ddlWarehouse").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].DepartmentID != null) {
                        $scope.listWarehouse.push({
                            OrganogramID: response.data.lstQC[0].DepartmentID, OrganogramName: response.data.lstQC[0].DepartmentName
                        });

                        $scope.Warehouse = response.data.lstQC[0].DepartmentID;
                        $("#ddlWarehouse").select2("data", { id: response.data.lstQC[0].DepartmentID, text: response.data.lstQC[0].DepartmentName });
                    }



                    //$scope.listPINO = [];
                    //$scope.PINO = '';
                    //$("#txtPINO").select2("data", { id: '', text: '' });

                    //if (response.data.lstQC[0].PIID != null) {
                    //    $scope.listPINO.push({
                    //        PIID: response.data.lstQC[0].PIID, PINo: response.data.lstQC[0].PINo
                    //    });
                    //    $scope.PINO = response.data.lstQC[0].PIID;
                    //    $("#txtPINO").select2("data", { id: response.data.lstQC[0].PIID, text: response.data.lstQC[0].PINo });
                    //}

                    $scope.listGRRNo = [];
                    $scope.lstGRRNoList = '';
                    $("#ddlGRRNo").select2("data", { id: '', text: '' });

                    if (response.data.lstQC[0].GrrID != null) {
                        $scope.listGRRNo.push({
                            GrrID: response.data.lstQC[0].GrrID, GrrNo: response.data.lstQC[0].GrrNo
                        });
                        $scope.lstGRRNoList = response.data.lstQC[0].GrrID;
                        $("#ddlGRRNo").select2("data", { id: response.data.lstQC[0].GrrID, text: response.data.lstQC[0].GrrNo });
                    }


                    $scope.LCNO = response.data.lstQC[0].LCNO;

                    $scope.RefChallanNo = response.data.lstQC[0].RefCHNo;



                    //var grrdate = response.data.lstQC[0].GrrDate;

                    //  console.log(response.data.lstQC[0].GRRDate);
                    //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

                    $scope.SPRDate = response.data.lstQC[0].SPRDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].SPRDate);
                    $scope.PODate = response.data.lstQC[0].PODate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PODate);
                    // $scope.PIDate = response.data.lstQC[0].PIDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PIDate);
                    $scope.QCDate = response.data.lstQC[0].MrrQcDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].MrrQcDate);
                    $scope.GRRDate = response.data.lstQC[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].GrrDate);

                     
                },
            function (error) {
                console.log("Error: " + error);
            });
            }
        }


        //**********---- item Detail from IssueNo  Changes ----***************//

        $scope.getItemDetailByIssueNo = function () {

            $scope.MrrID = "0";
            $scope.btnMrrSaveText = "Save";

            $scope.ListMrrDetails = [];
            var issueNo = $scope.lstIssueNoList;
 

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


            if (issueNo != "") {
                var apiRoute = baseUrl + 'GetDetailInfoByIssueID/';

                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(issueNo) + "]";

                var issueDetails = mRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);;
                issueDetails.then(function (response) {
                    $scope.ListMrrDetails = response.data.lstIssue;
                    $scope.QCCertificateNo = response.data.lstIssue[0].QCCertificateNo;

                    if (response.data.lstIssue[0].FromDepartmentID != null) {
                        $scope.FrmDeptIDByIssueChnge = response.data.lstIssue[0].FromDepartmentID
                    }
                     
                    $scope.listSupplier = [];
                    $scope.lstSupplierList = '';
                    $("#ddlSupplier").select2("data", { id: '', text: '' });


                    if (response.data.lstIssue[0].SupplierID != null) {
                        $scope.listSupplier.push({
                            SupplierID: response.data.lstIssue[0].SupplierID, SupplierName: response.data.lstIssue[0].SupplierName
                        });

                        $scope.lstSupplierList = response.data.lstIssue[0].SupplierID;
                        $("#ddlSupplier").select2("data", { id: response.data.lstIssue[0].SupplierID, text: response.data.lstIssue[0].SupplierName });
                    }

                    $scope.listSprNo = [];
                    $scope.SprNo = '';
                    $("#txtSPRNo").select2("data", { id: '', text: '' });
                    if (response.data.lstIssue[0].RequisitionID != null) {
                        $scope.listSprNo.push({
                            RequisitionID: response.data.lstIssue[0].RequisitionID, SprNo: response.data.lstIssue[0].RequisitionNo
                        });

                        $scope.SprNo = response.data.lstIssue[0].RequisitionID;
                        $("#txtSPRNo").select2("data", { id: response.data.lstIssue[0].RequisitionID, text: response.data.lstIssue[0].RequisitionNo });
                    }

                    $scope.listCurrency = [];
                    $scope.lstCurrencyList = '';
                    $("#ddlCurrency").select2("data", { id: '', text: '' });
                    if (response.data.lstIssue[0].CurrencyID != null) {
                        $scope.listCurrency.push({
                            Id: response.data.lstIssue[0].CurrencyID, CurrencyName: response.data.lstIssue[0].CurrencyName
                        });

                        $scope.lstCurrencyList = response.data.lstIssue[0].CurrencyID;
                        $("#ddlCurrency").select2("data", { id: response.data.lstIssue[0].CurrencyID, text: response.data.lstIssue[0].CurrencyName });
                    }


                    $scope.listPONo = [];
                    $scope.PONo = '';
                    $("#txtPONo").select2("data", { id: '', text: '' });

                    if (response.data.lstIssue[0].POID != null) {
                        $scope.listPONo.push({
                            POID: response.data.lstIssue[0].POID, PONo: response.data.lstIssue[0].PONo
                        });

                        $scope.PONo = response.data.lstIssue[0].POID;
                        $("#txtPONo").select2("data", { id: response.data.lstIssue[0].POID, text: response.data.lstIssue[0].PONo });
                    }


                    //$scope.listPINO = [];
                    //$scope.PINO = '';
                    //$("#txtPINO").select2("data", { id: '', text: '' });

                    //if (response.data.lstQC[0].PIID != null) {
                    //    $scope.listPINO.push({
                    //        PIID: response.data.lstQC[0].PIID, PINo: response.data.lstQC[0].PINo
                    //    });
                    //    $scope.PINO = response.data.lstQC[0].PIID;
                    //    $("#txtPINO").select2("data", { id: response.data.lstQC[0].PIID, text: response.data.lstQC[0].PINo });
                    //}

                    $scope.listGRRNo = [];
                    $scope.lstGRRNoList = '';
                    $("#ddlGRRNo").select2("data", { id: '', text: '' });

                    if (response.data.lstIssue[0].GrrID != null) {
                        $scope.listGRRNo.push({
                            GrrID: response.data.lstIssue[0].GrrID, GrrNo: response.data.lstIssue[0].GrrNo
                        });
                        $scope.lstGRRNoList = response.data.lstIssue[0].GrrID;
                        $("#ddlGRRNo").select2("data", { id: response.data.lstIssue[0].GrrID, text: response.data.lstIssue[0].GrrNo });
                    }


                    $scope.LCNO = response.data.lstIssue[0].LCNO;

                    $scope.RefChallanNo = response.data.lstIssue[0].RefCHNo;



                    //var grrdate = response.data.lstQC[0].GrrDate;

                    //  console.log(response.data.lstQC[0].GRRDate);
                    //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

                    $scope.SPRDate = response.data.lstIssue[0].SPRDate == null ? "" : conversion.getDateToString(response.data.lstIssue[0].SPRDate);
                    $scope.PODate = response.data.lstIssue[0].PODate == null ? "" : conversion.getDateToString(response.data.lstIssue[0].PODate);
                    // $scope.PIDate = response.data.lstQC[0].PIDate == null ? "" : conversion.getDateToString(response.data.lstQC[0].PIDate);
                    $scope.QCDate = response.data.lstIssue[0].MrrQcDate == null ? "" : conversion.getDateToString(response.data.lstIssue[0].MrrQcDate);
                    $scope.GRRDate = response.data.lstIssue[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstIssue[0].GrrDate);
                    // $scope.GRRDate = response.data.lstIssue[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstIssue[0].GrrDate);




                },
            function (error) {
                console.log("Error: " + error);
            });
            }
        }



        ////**********---- Get GRRNo Records ----*************** //
        //function loadGRRNo(isPaging) {

        //    var apiRoute = baseUrl + 'GetGRRNo/';
        //    var listGRRNo = mRRService.getModel(apiRoute, page, pageSize, isPaging);
        //    listGRRNo.then(function (response) {
        //        $scope.listGRRNo = response.data; 
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadGRRNo(0);


        //**********---- Start Add details Modal ----*************** //

        //**********----Get Item Record and filter by GroupID ----***************// 

        //  var baseUrl = '/Inventory/api/Requisition/';

        $scope.LaodItemByGroupID = function LoadItemList(isPaging) {
            $scope.listItems = [];
            var GroupID = $scope.lstItemGroup;
            if (GroupID != "") {
                var apiRoute = '/Inventory/api/Requisition/GetItemListByGroupID/';
                var listItem = RequisitionService.getByID(apiRoute, page, pageSize, isPaging, GroupID);
                listItem.then(function (response) {
                    $scope.listItems = response.data
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        //$scope.itemGroupes = [];
        //$scope.GetItemGroups = function () {

        //    var apiRoute = '/Inventory/api/Requisition/GetAllItemGroup/';
        //    var itemGroups = RequisitionService.getAll(apiRoute, page, pageSize, isPaging);
        //    itemGroups.then(function (response) {
        //        $scope.itemGroupes = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //$scope.GetItemGroups(0);


        $scope.lstBatchList = [];

        // -------Get All Batch //--------
        function GetAllBatchNo() {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data;
                // $scope.listMrrBatchNo = response.data;
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


        //**********----Get All Lot Record----***************
        function loadLotRecords(isPaging) {
            var apiRoute = '/Inventory/api/StockEntry/GetLotNo/';
            var lisLotNo = RequisitionService.getAllLotNo(apiRoute, page, pageSize, isPaging);
            lisLotNo.then(function (response) {
                $scope.listLot = response.data;
                // $scope.listMrrLot = response.data;
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

        //**********----Get All unit Record----***************

        //function loadUnitRecords(isPaging) {
        //    var apiRoute = '/SystemCommon/api/RawMaterial/GetUnits/';
        //    var listunit = RequisitionService.getunit(apiRoute, page, pageSize, isPaging);
        //    listunit.then(function (response) {
        //        $scope.listunit = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadUnitRecords(0);

        //**********----Get Unit Price Record and filter by ItemID ----***************// 

        $scope.LaodItemRateNUnit = function LoadItemPrice(isPaging) {

            $scope.IsItem = false;
            var ItemID = $scope.ddlItem;
            if (ItemID > 0) {

                $scope.IsItem = true;

                var apiRoute = '/Inventory/api/Requisition/LaodItemRateNUnit/';
                var listItemPrice = RequisitionService.getByID(apiRoute, page, pageSize, isPaging, ItemID);
                listItemPrice.then(function (response) {
                    $scope.UnitPrice = response.data[0].UnitPrice;
                    $scope.UnitID = response.data[0].UnitID;
                    $scope.UOMName = response.data[0].UOMName;
                    $("#ddlUnit").select2("data", { id: $scope.UnitID, text: $scope.UOMName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.getListItemDetails = function () {

            $scope.IsHiddenDetail = false;
            var existItem = $scope.ddlItem;
            var duplicateItem = 0;
            if ($scope.btnModal == "Add") {
                angular.forEach($scope.ListRequisitionDetails, function (item) {
                    if (existItem == item.ItemID) {
                        duplicateItem = 1;
                        return false;
                    }
                });

                if (duplicateItem === 0) {

                    //$scope.ListRequisitionDetails.push({
                    //    RequisitionDetailID: $scope.RequisitionDetailID,
                    //    RequisitionID: $scope.RequisitionID,
                    //    GroupID: $scope.lstItemGroup,
                    //    GroupName: $("#ddlItemGroup").select2('data').text,
                    //    ItemID: $scope.ddlItem,
                    //    ItemName: $("#ddlItem").select2('data').text,
                    //    UnitID: $scope.UnitID,
                    //    UnitName: $("#ddlUnit").select2('data').text,
                    //    LotID: $scope.lstLot,
                    //    LotNo: $("#ddlLotNo").select2('data').text == "--Select Lot No--" ? "" : $("#ddlLotNo").select2('data').text,
                    //    BatchID: $scope.Batch,
                    //    BatchNo: $("#ddlBatch").select2('data').text == "--Select Batch--" ? "" : $("#ddlBatch").select2('data').text,
                    //    Qty: $scope.Qty,
                    //    UnitPrice: $scope.UnitPrice,
                    //    Amount: parseFloat($scope.Qty * $scope.UnitPrice)
                    //});

                    $scope.ListMrrDetails.push({
                        MrrID: 0, MrrDetailID: 0, MrrQcID: 0, MrrQcDetailID: 0, GrrID: 0, GrrDetailID: 0, ItemID: $scope.ddlItem, LotID: $scope.lstLot, BatchID: $scope.Batch,
                        UnitID: $scope.UnitID, ItemName: $("#ddlItem").select2('data').text, ItemCode: '', BatchNo: $("#ddlBatch").select2('data').text == "--Select Batch--" ? "" : $("#ddlBatch").select2('data').text,
                        LotNo: $("#ddlLotNo").select2('data').text == "--Select Lot No--" ? "" : $("#ddlLotNo").select2('data').text, UOMName: $("#ddlUnit").select2('data').text,
                        UnitPrice: $scope.UnitPrice, GrrQty: 0, Amount: parseFloat($scope.Qty * $scope.UnitPrice), QcRemainingQty: 0, QCQty: 0, Qty: $scope.Qty, CurrentStock: 0

                    });

                    $scope.EmptyRequisitionDetail();
                }
                else if (duplicateItem === 1) {
                        Command: toastr["warning"]("Item Already Added!!!!");
                }
            }
            else if ($scope.btnModal == "") {
                    Command: toastr["warning"]("Item Not Added With Existing GRR/MRR !!!!");
            }

        }


        $scope.EmptyRequisitionDetail = function () {
            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Group--' });
            $("#ddlItem").select2("data", { id: '', text: '--Select Item--' });
            $("#ddlUnit").select2("data", { id: '', text: '--Select Unit--' });
            $("#ddlLotNo").select2("data", { id: '', text: '--Select Lot No--' });
            $("#ddlBatch").select2("data", { id: '', text: '--Select Batch--' });
            $scope.Qty = ''
            $scope.UnitPrice = ''
            $scope.btnModal = "Add";
            $scope.IsItem = false;

        }

        $scope.EmptyLotSetupModal = function () {
            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.LotID = '';
            $scope.LotNo = '';
            $scope.LotDecription = '';
            $scope.lstBatchInLotList = '';
            $scope.listBatchInLot = [];
            GetAllBatchNo();
        }

        $scope.EmptyBatchSetupModal = function () {
            $("#ddlBatchInLot").select2("data", { id: '', text: '--Select Batch--' });
            $scope.ManufactureDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.ExpaireDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

            $scope.BatchID = '';
            $scope.BatchNo = '';
            $scope.BatchDecription = '';
        }

        //**********---- End Add details Modal ----***************//

        //**********---- Get All Suppliers Records ----*************** //

        //function loadSuppliersRecords(isPaging) {

        //     objcmnParam = {
        //    pageNumber: page,
        //    pageSize: pageSize,
        //    IsPaging: isPaging,
        //    loggeduser: $scope.UserCommonEntity.loggedUserID,
        //    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //    menuId: $scope.UserCommonEntity.currentMenuID,
        //    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //};

        //    var apiRoute = baseUrl + 'GetSuppliers/';
        //    var Suppliers = mRRService.GetSuppliers(apiRoute, objcmnParam);
        //    Suppliers.then(function (response) {
        //        $scope.listSupplier = response.data.lstSuppliers;
        //        //console.log($scope.listBuyer);
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadSuppliersRecords(0);



        ////**********---- Get All Currency Records ----*************** //
        //function loadCurrencyRecords(isPaging) {

        //    var apiRoute = '/Inventory/api/Challan/GetCurrency/';
        //    var listCurrency = mRRService.getModel(apiRoute, page, pageSize, isPaging);
        //    listCurrency.then(function (response) {
        //        $scope.listCurrency = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadCurrencyRecords(0);



        ////**********----delete  Record from ListMrrDetails----***************// 
        $scope.deleteRow = function (index) {
            $scope.ListMrrDetails.splice(index, 1);
        };

        //**********----Create Calculation----***************//
        $scope.calculation = function (dataModel) {
            $scope.ListMrrDetails1 = [];
            // var getItemID = dataModel.ItemID;
            angular.forEach($scope.ListMrrDetails, function (item) {
             
                if ($scope.IsMrr == true && $scope.IsIssueReceive == false && $scope.IsReturnReceive == false && item.MrrValidQty < item.Qty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty < item.Qty) {  //item.QCQty < item.Qty) {
                    Command: toastr["warning"]("MRR Qty must not greater than QCQty !!!!");
                    $scope.ListMrrDetails1.push({
                        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                        LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, UnitPrice: item.UnitPrice, GrrQty: item.GrrQty, Amount: 0.00, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                        Qty: 0.00, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,

                        GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                        AdditionalQty: item.AdditionalQty, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty

                    });
                }

                else if ($scope.IsMrr == true && $scope.IsIssueReceive == false && $scope.IsReturnReceive == false && item.MrrValidQty >= item.Qty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty >= item.Qty) {  //item.QCQty > item.Qty) {
                    $scope.ListMrrDetails1.push({
                        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                        LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, UnitPrice: item.UnitPrice, GrrQty: item.GrrQty, Amount: 0.00, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                        Qty: item.Qty, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,

                        GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                        AdditionalQty: item.AdditionalQty, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty

                    });
                } 
            });
            $scope.ListMrrDetails = $scope.ListMrrDetails1;
        }



        //**********----Create calculationAdditionalQty----***************//
        $scope.calculationAdditionalQty = function (dataModel) {
            $scope.ListMrrDetails1 = [];
            // var getItemID = dataModel.ItemID;
            angular.forEach($scope.ListMrrDetails, function (item) {
              
                if ($scope.IsMrr == true && $scope.IsIssueReceive == false && $scope.IsReturnReceive == false && item.MrrValidAdditonalQty < item.AdditionalQty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty < item.Qty) {  //item.QCQty < item.Qty) {
                    Command: toastr["warning"]("MRR Additional Quantity must not greater than QC Additional Quantity !!!!");
                    $scope.ListMrrDetails1.push({
                        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                        LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, UnitPrice: item.UnitPrice, GrrQty: item.GrrQty, Amount: item.Amount, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                        Qty: item.Qty, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,
                         
                        GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                        AdditionalQty: 0.00, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty


                    });
                }

                else if ($scope.IsMrr == true && $scope.IsIssueReceive == false && $scope.IsReturnReceive == false && item.MrrValidAdditonalQty >= item.AdditionalQty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty >= item.Qty) {  //item.QCQty > item.Qty) {
                    $scope.ListMrrDetails1.push({
                        MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                        MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                        LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                        UOMName: item.UOMName, UnitPrice: item.UnitPrice, GrrQty: item.GrrQty, Amount: newAmnt, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                        Qty: item.Qty, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,

                        GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                        AdditionalQty: item.AdditionalQty, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty

                    });
                }
            });
            $scope.ListMrrDetails = $scope.ListMrrDetails1;
        }



        //**********----Pagination----***************
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
                $scope.loadMrrRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadMrrRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadMrrRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadMrrRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadMrrRecords(1);
                }
            }
        };
        //  }

    
        //**********----Get All Mrr Master Records----***************
        $scope.loadMrrRecords = function (isPaging) {

            
            $scope.gridOptionsMrrMaster.enableFiltering = true;
            $scope.gridOptionsMrrMaster.enableGridMenu = true;

            // For Loading
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

             //For Loading
            $scope.loaderMoreMrrMaster = true;
            $scope.lblMessageForMrrMaster = 'loading please wait....!';
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

            $scope.gridOptionsMrrMaster = {
                useExternalPagination: true,
                useExternalSorting: true,

                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,

                columnDefs: [
                    { name: "MrrID", displayName: "MrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CHID", displayName: "CHID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyID", displayName: "CompanyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrencyID", displayName: "CurrencyID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DepartmentID", displayName: "DepartmentID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrID", displayName: "GrrID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PIID", displayName: "PIID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "POID", displayName: "POID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierID", displayName: "SupplierID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StatusID", displayName: "StatusID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "StatusBy", displayName: "StatusBy", visible: false, headerCellClass: $scope.highlightFilteredHeader },

                    { name: "RequisitionID", displayName: "RequisitionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },


                    { name: "MrrNo", displayName: $scope.grdTMrNo, title: $scope.grdTMrNo, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "ManualMRRNo", displayName: "Manual MRRNo", title: "Manual MRRNo",  width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrDate", displayName: $scope.grdTMrDate, title: $scope.grdTMrDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueNo", displayName: "Issue No", title: "Issue No", visible: $scope.CommonFIssueNReturnReceive, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueDate", displayName: "Issue Date", title: "Issue Date", visible: $scope.CommonFIssueNReturnReceive, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "DepartmentName", displayName: "Department Name", title: "Department Name", visible: $scope.CommonFIssueNReturnReceive, width: '30%', headerCellClass: $scope.highlightFilteredHeader },


                    { name: "GrrNo", displayName: "GRR No", title: "GRR No", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrDate", displayName: "GRR Date", title: "GRR Date", visible: $scope.IsMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierName", displayName: "Supplier", title: "Supplier", visible: $scope.IsMrr, width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "FromCompanyName", displayName: "From Company", title: "From Company", visible: $scope.IsLoanReceive, width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "MrrQcNo", displayName: "QC No", title: "QC No", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrQcDate", displayName: "QC Date", title: "QC Date", visible: $scope.IsMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "SprNo", displayName: $scope.grdTSprNo, title: $scope.grdTSprNo, visible: $scope.CommonFLoanReceiveNMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPRDate", displayName: $scope.grdTSprDate, title: $scope.grdTSprDate, visible: $scope.CommonFLoanReceiveNMrr, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                  


                    { name: "PONO", displayName: "PO NO", title: "PO NO", visible: $scope.IsMrr, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PODate", displayName: "PO Date", cellFilter: 'date:"dd-MM-yyyy"', visible: $scope.IsMrr, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                   
                    { name: "CHDate", displayName: "CH Date", cellFilter: 'date:"dd-MM-yyyy"', visible: $scope.IsMrr, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Action',
                        displayName: "Action",

                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,

                        //visible: $scope.IsMrr,

                        visible: $scope.CommonFLoanReceiveNMrr,

                        width: '8%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Edit" ng-click="grid.appScope.loadMasterDetailsByMrrMaster(row.entity)">' +
                                        '<i class="icon-edit" aria-hidden="true"></i> Edit' +
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
                exporterCsvFilename: 'Mrr.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Mrr", style: 'headerStyle' },
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


            var apiRoute = baseUrl + 'GetMrrMasterList/';
             

             var cmnParam = "[" + JSON.stringify(objcmnParam) +"]";

             var listgridOptionsMrrMaster = mRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listgridOptionsMrrMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMrrMaster.data = response.data.lstMrrMaster;
                
                $scope.loaderMoreMrrMaster = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
        
        $scope.loadMrrRecords(0);

        //**********----Load Mrr MasterForm and MrrDetails List By select Mrr Master ----***************//
        $scope.loadMasterDetailsByMrrMaster = function (dataModel) {

            $("#ddlQCNo").prop("disabled", true);

         


            if (dataModel.IssueID != null) {
                $scope.lstIssueNoList = dataModel.IssueID;
                $('#ddlIssueNo').select2("data", { id: dataModel.IssueID, text: dataModel.IssueNo });

            }

            if (dataModel.DepartmentID != null) {
                $scope.Dept = dataModel.DepartmentID;
                $('#ddlDept').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });

            }


            if (dataModel.UserID != null) {
                $scope.ngmMRRByList = dataModel.UserID;
                $('#ddlMRRBy').select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });

            }

            if (dataModel.FormDepartmentID != null) {
                $scope.FrmDeptIDByIssueChnge = dataModel.FormDepartmentID;

            }



            if (dataModel.DepartmentID != null) {
                $scope.Warehouse = dataModel.DepartmentID;
                $('#ddlWarehouse').select2("data", { id: dataModel.DepartmentID, text: dataModel.DepartmentName });
            }


            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });

            if (dataModel.FromCompanyID != null) {
                
                //  if (response.data.lstQC[0].CurrencyID != null) { 
                $scope.listCompany.push({
                    CompanyID: dataModel.FromCompanyID, CompanyName: dataModel.FromCompanyName
                });

                $scope.lstCompanyList = dataModel.FromCompanyID;
                $("#ddlCompany").select2("data", { id: dataModel.FromCompanyID, text: dataModel.FromCompanyName });
            }



            $scope.listSupplier = [];
            $scope.lstSupplierList = '';
            $("#ddlSupplier").select2("data", { id: '', text: '' });

            if (dataModel.SupplierID != null) {
                $scope.listSupplier.push({
                    SupplierID: dataModel.SupplierID, SupplierName: dataModel.SupplierName
                });

                $scope.lstSupplierList = dataModel.SupplierID;
                $("#ddlSupplier").select2("data", { id: dataModel.SupplierID, text: dataModel.SupplierName });
            }

            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#txtSPRNo").select2("data", { id: '', text: '' });
            if (dataModel.RequisitionID != null) {
                $scope.listSprNo.push({
                    RequisitionID: dataModel.RequisitionID, SprNo: dataModel.SprNo
                });

                $scope.SprNo = dataModel.RequisitionID;
                $("#txtSPRNo").select2("data", { id: dataModel.RequisitionID, text: dataModel.SprNo });
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


            $scope.listPONo = [];
            $scope.PONo = '';
            $("#txtPONo").select2("data", { id: '', text: '' });

            if (dataModel.POID != null) {
                $scope.listPONo.push({
                    POID: dataModel.POID, PONo: dataModel.PONO
                });

                $scope.PONo = dataModel.POID;
                $("#txtPONo").select2("data", { id: dataModel.POID, text: dataModel.PONO });
            }


            //$scope.listPINO = [];
            //$scope.PINO = '';
            //$("#txtPINO").select2("data", { id: '', text: '' });

            //if (dataModel.PIID != null) {
            //    $scope.listPINO.push({
            //        PIID: dataModel.PIID, PINo: dataModel.PINO
            //    });
            //    $scope.PINO = dataModel.PIID;
            //    $("#txtPINO").select2("data", { id: dataModel.PIID, text: dataModel.PINO });
            //}

            $scope.listGRRNo = [];
            $scope.lstGRRNoList = '';
            $("#ddlGRRNo").select2("data", { id: '', text: '' });

            if (dataModel.GrrID != null) {
                $scope.listGRRNo.push({
                    GrrID: dataModel.GrrID, GrrNo: dataModel.GrrNo
                });
                $scope.lstGRRNoList = dataModel.GrrID;
                $("#ddlGRRNo").select2("data", { id: dataModel.GrrID, text: dataModel.GrrNo });
            }

            if (dataModel.UserID != null) {
                //$scope.listGRRNo.push({
                //    GrrID: dataModel.GrrID, GrrNo: dataModel.GrrNo
                //});
                $scope.ngmMRRByList = dataModel.UserID;
                $("#ddlMRRBy").select2("data", { id: dataModel.UserID, text: dataModel.UserFullName });
            }

            $scope.LCNO = dataModel.LCNO;
            $scope.RefChallanNo = dataModel.RefCHNo;
            //var grrdate = response.data.lstQC[0].GrrDate;

            //  console.log(response.data.lstQC[0].GRRDate);
            //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

            $scope.SPRDate = dataModel.SPRDate == null ? "" : conversion.getDateToString(dataModel.SPRDate);
            $scope.PODate = dataModel.PODate == null ? "" : conversion.getDateToString(dataModel.PODate);
            // $scope.PIDate = dataModel.PIDate == null ? "" : conversion.getDateToString(dataModel.PIDate);
            $scope.QCDate = dataModel.MrrQcDate == null ? "" : conversion.getDateToString(dataModel.MrrQcDate);
            $scope.GRRDate = dataModel.GrrDate == null ? "" : conversion.getDateToString(dataModel.GrrDate);

            $scope.QCCertificateNo = dataModel.QCCertificateNo;
            $scope.ManualMRRNo = dataModel.ManualMRRNo;
            $scope.Remarks = dataModel.Remarks;
            $scope.Description = dataModel.Description;

            

            //$scope.IsPostMRR = true;
            //$scope.IsPreMRR = false;

            $scope.lstQCNoList = dataModel.MrrQcID;
            $('#ddlQCNo').select2("data", { id: dataModel.MrrQcID, text: dataModel.MrrQcNo });
            $scope.QCDate = conversion.getDateToString(dataModel.MrrQcDate);



            // modal_fadeOut(); 
            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnMrrShowText = "Show List";
            $scope.IsHidden = true;
            //
            $scope.btnMrrSaveText = "Update";


            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;


            // $scope.listMrrMaster = [];

            var mrrID = dataModel.MrrID;
            $scope.MrrID = dataModel.MrrID;
            // $scope.PITypeID = dataModel.PITypeID;
            $scope.HMRRNO = dataModel.MrrNo;
            $scope.MrrDate = dataModel.MrrDate == null ? "" : conversion.getDateToString(dataModel.MrrDate);


            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };
            $scope.ListMrrDetails = [];

            var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(mrrID) + "]";

            var MrrDetails = mRRService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

            //var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
            //var MrrDetails = mRRService.getMrrDetailsListByMrrID(apiRoute, mrrID, objcmnParam);

            MrrDetails.then(function (response) {
                $scope.ListMrrDetails = response.data.lstDetailInfoByMrrID;
            },
        function (error) {
            console.log("Error: " + error);
        });

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
                ItemID: $scope.ddlItem,
                Description: $scope.BatchDecription,
                ManufacturingDate: ManufacDate,
                ExpiryDate: ExpDate,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            };

            var apiRoute = baseUrl + 'SaveBatch/';
            var SaveNewBatch = mRRService.postSaveBatch(apiRoute, batchMaster);
            SaveNewBatch.then(function (response) {
                var result = 0;
                if (response.data != "") {
                    //$scope.HMRRNO = response.data;
                    varBatchID = response.data;
                    varBatchName = $scope.BatchNo;

                    //$scope.lstBatchList = [];
                    //$scope.Batch = "";
                    //$('#ddlBatch').select2("data", { id: '', text: '' });

                    $scope.lstBatchList.push({

                        BatchID: response.data,
                        BatchNo: $scope.BatchNo

                    });


                    // GetAllBatchNo();
                    $scope.Batch = response.data;
                    $('#ddlBatch').select2("data", { id: response.data, text: $scope.BatchNo });
                    Command: toastr["success"]("Save  Successfully!!!!");
                    modal_fadeOut_Batch();
                    $scope.EmptyBatchSetupModal();
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

        //// **********----Save Lot using popup----***************//
        $scope.SaveLot = function () {
            // LotID, LotNo, LotTypeID, ItemID, BatchID, Description, CompanyID, CreateBy, CreateOn, CreatePc, UpdateBy, UpdateOn, UpdatePc, IsDeleted, DeleteBy, DeleteOn, DeletePc
            var MrrDateStringToDate = conversion.getStringToDate($scope.MrrDate);
            var lotMaster = {
                LotID: 0,
                LotNo: $scope.LotNo,
                LotTypeID: $scope.lstLotTypeList,
                ItemID: $scope.ddlItem,
                BatchID: $scope.lstBatchInLotList,
                Description: $scope.LotDecription,
                CompanyID: LoggedCompanyID,
                CreateBy: LoggedUserID
            };

            var apiRoute = baseUrl + 'SaveLot/';
            var SaveNewLot = mRRService.postSaveLot(apiRoute, lotMaster);
            SaveNewLot.then(function (response) {
                var result = 0;
                if (response.data != "") {
                    //$scope.HMRRNO = response.data; 

                    varLotID = response.data;
                    varLotName = $scope.LotNo;

                    //$scope.listLot = [];

                    $scope.listLot.push({

                        LotID: response.data,
                        LotNo: $scope.LotNo

                    });

                    // $scope.lstLot = "";
                    //$('#ddlLotNo').select2("data", { id:'', text: '' });

                    // loadLotRecords(0);

                    $scope.lstLot = response.data;
                    $('#ddlLotNo').select2("data", { id: response.data, text: $scope.LotNo });

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

        //// **********----Save and Update InvMrrMaster and InvMrrDetail  Records----***************//
        $scope.save = function () {

            if ($scope.ChkManualMRRNoWhnSave == "") {

                var HedarTokenPostPut = $scope.MrrID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

            $("#save").prop("disabled", true);
             
            var varIsStoreCompleted = false;
            var varIsApproved = false;
            var varIsAccountsCompleted = false

            var deptID = null;

            if ($scope.CommonFIssueNReturnReceive) {
                deptID = $scope.Dept;

                varIsStoreCompleted = false;
                varIsApproved = false;
                varIsAccountsCompleted = false;
            }
            else if ($scope.IsMrr) {
                deptID = $scope.Warehouse;
                varIsStoreCompleted = true;
            }
            else if ($scope.IsLoanReceive) {
                deptID = $scope.Warehouse;
                varIsStoreCompleted = true;
            }

            var MrrDateStringToDate = conversion.getStringToDate($scope.MrrDate);



            var mrrMaster = {
                MrrID: $scope.MrrID,
                MrrNo: $scope.HMRRNO,
                MrrDate: MrrDateStringToDate,
                MrrTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                ManualMRRNo: $scope.ManualMRRNo,
                GrrID: $scope.lstGRRNoList,
                RequisitionID: $scope.SprNo,
                ReqNo: $("#txtSPRNo").select2('data').text,
                // ChallanNo: $("#txtChallanNo").select2('data').text,
                //  PIID: $scope.PINO,
                // CIID: $scope.InvoiceNo,
                POID: $scope.PONo,
                PONo: $("#txtPONo").select2('data').text,
                MrrQcID: $scope.lstQCNoList,
                Remarks: $scope.Remarks,
                Description: $scope.Description,
                IssueID: $scope.lstIssueNoList,
                // CHID: $scope.ChallanNo, 

                SupplierID: $scope.lstSupplierList,
                CurrencyID: $scope.lstCurrencyList,
                FromDepartmentID: $scope.FrmDeptIDByIssueChnge,
                // DepartmentID: $scope.Warehouse, //$scope.UserCommonEntity.loggedUserDepartmentID,
                DepartmentID: deptID,
                StatusID: 1,
                StatusBy: $scope.UserCommonEntity.loggedUserID,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                UserID: $scope.ngmMRRByList, //$scope.UserCommonEntity.loggedUserID


                IsStoreCompleted : varIsStoreCompleted, 
                IsAccountsCompleted: varIsAccountsCompleted,
                IsApproved: varIsApproved

            };

            var chkMrrQty = 1;
            var MssVar = "";
             
                angular.forEach($scope.ListMrrDetails, function (item) {
                    if ((item.Qty <= 0) && ($scope.IsMrrAccountsDept == false)) {
                        chkMrrQty = 0;
                        MssVar = " Quantity ";
                    }
                    //else if ((item.Amount <= 0) && ($scope.IsMrrAccountsDept == true)) {
                    //    chkMrrQty = 0;
                    //    MssVar = " Amount ";
                    //}
                });
    


            var menuID = $scope.UserCommonEntity.currentMenuID;
            var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var mrrDetail = $scope.ListMrrDetails;
            

            if ($scope.ListMrrDetails.length > 0) {

                if (chkMrrQty == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateMrrMasterNdetails/';

                    var cmnParam = "[" + JSON.stringify(mrrMaster) + "," + JSON.stringify(mrrDetail) + "," + JSON.stringify(menuID) + "]";

                    var MrrMasterNdetailsCreateUpdate = mRRService.GetList(apiRoute, cmnParam, HedarTokenPostPut); //mRRService.postMrrMasterDetail(apiRoute, mrrMaster, mrrDetail, menuID, transactionTypeID);
                    MrrMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "") {
                            $scope.HMRRNO = response.data;
                            // alert('Saved Successfully.');
                            Command: toastr["success"]("Save  Successfully!!!!");
                            $scope.clear();
                            // result = 1;
                        }
                        else if (response.data == "") {
                                Command: toastr["warning"]("Save Not Successfull!!!!");
                            $("#save").prop("disabled", false);
                        }
                        // 
                        // ShowCustomToastrMessageResult(result);
                    },
                    function (error) {
                        // console.log("Error: " + error);
                        $("#save").prop("disabled", false);
                        Command: toastr["warning"]("Save Not Successfull!!!!");
                    });
                }
                else if (chkMrrQty == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("" + MssVar + " Must Not Zero Or Empty !!!!");
                }
            }
            else if ($scope.ListMrrDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("MRR Detail Must Not Empty!!!!");
            }

            }

            else if ($scope.ChkManualMRRNoWhnSave != "") {

                Command: toastr["warning"]("Manual MRR No Already Exists.");

                // Command: toastr["warning"]("Please Enter QC Certificate No.");
            }
        };



        //**********----Reset Record----***************//
        $scope.clear = function () {

            $("#save").prop("disabled", false);

            $("#ddlQCNo").prop("disabled", false);

        
            // $scope.btnModal == "Add";
            $scope.MrrID = "0";

            chkTrnsTypeFHideShow();
            $scope.FrmDeptIDByIssueChnge = null;
            //varLotID = "";
            //varLotName = "";
            //varBatchID = "";
            //varBatchName = "";

            // $scope.listMRRType = [];

            //  $scope.listMRRQCNo = [];

            var date = new Date();
            $scope.MrrDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            $scope.IsPostMRR = true;
            $scope.IsPreMRR = false;
            $scope.IsItem = false;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.btnMrrSaveText = "Save";
            $scope.btnMrrShowText = "Show List";

            //  $scope.btnMrrReviseText = "Update";

            //$scope.PageTitle = 'Create MRR';
            //$scope.ListTitle = 'MRR Records';
            //$scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
            //$scope.ListTitleSampleNo = 'Sample Info';
            //$scope.ListTitleMrrDeatails = 'MRR Information (Details)';

            $scope.ListMrrDetails = [];
            $scope.ListMrrMaster = [];
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.listIssueNo = [];
            $scope.lstIssueNoList = '';
            $("#ddlIssueNo").select2("data", { id: '', text: '--Select Issue No--' });

            $scope.listOrganogramsDepartment = [];
            $scope.Dept = '';
            $("#ddlDept").select2("data", { id: '', text: '--Select Department--' });

            $scope.listMRRQCNo = [];
            $scope.lstQCNoList = '';
            $("#ddlQCNo").select2("data", { id: '', text: '--Select QC No--' });


            $scope.listSupplier = [];
            $scope.lstSupplierList = '';
            $("#ddlSupplier").select2("data", { id: '', text: '' });

            $scope.listWarehouse = [];
            $scope.Warehouse = '';
            $('#ddlWarehouse').select2("data", { id: '', text: '--Select Wherehouse--' });


            $scope.listSprNo = [];
            $scope.SprNo = '';
            $("#txtSPRNo").select2("data", { id: '', text: '' });

            $scope.listCurrency = [];
            $scope.lstCurrencyList = '';
            $("#ddlCurrency").select2("data", { id: '', text: '' });

            $scope.listPONo = [];
            $scope.PONo = '';
            $("#txtPONo").select2("data", { id: '', text: '' });


            //$scope.listPINO = [];
            //$scope.PINO = '';
            //$("#txtPINO").select2("data", { id: '', text: '' });

            $scope.listGRRNo = [];
            $scope.lstGRRNoList = '';
            $("#ddlGRRNo").select2("data", { id: '', text: '' });

            $scope.listMRRBy = [];
            $scope.ngmMRRByList = "";
            $("#ddlMRRBy").select2("data", { id: "", text: "" });

            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });



            $scope.LCNO = "";
            $scope.RefChallanNo = "";
            $scope.SPRDate = "";
            $scope.PODate = "";
            // $scope.PIDate = "";
            $scope.QCDate = "";
            $scope.GRRDate = "";

            $scope.QCCertificateNo = "";
            $scope.ManualMRRNo = "";

            $scope.Remarks = "";
            $scope.Description = "";



            loadQCNo(0);

            loadWherehouse(0);
            loadRecords_OrganogramDept(0);
            loadIssueRecords(0);

            loadRecords_OrganogramDept(0);
            //  loadCurrencyRecords(0);
            loadMrrByRecords(0);

            $scope.loadMrrRecords(0);

        };

    }]);

function modal_fadeOut() {
    $("#MrrModal").fadeOut(200, function () {
        $('#MrrModal').modal('hide');
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

