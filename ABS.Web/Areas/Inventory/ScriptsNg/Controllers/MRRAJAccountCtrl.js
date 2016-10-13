/**
* MRRCtrl.js
*/


app.controller('mRRAJAccountCtrl', ['$scope', 'mRRAccountService', 'qcService', 'RequisitionService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, mRRAccountService, qcService, RequisitionService, conversion, $filter, $localStorage, uiGridConstants) {

        $scope.gridOptionsMrrMaster = [];
        $scope.gridOptionslistItemMaster = [];
        var objcmnParam = {};
        //*************---Show and Hide Order---**********//
      
        $scope.btnMrrShowText = "Create";
        $scope.IsShow = false;
        $scope.IsHidden = false;
        $scope.IsHiddenDetail = true;
        $scope.IsCreateIcon = true;
        $scope.IsListIcon = false;

        $scope.ChkApprovalOrUpdateMode = false;

        $scope.IsApproveChoosen = false;
        $scope.IsDeclinChoosen = false;

        $scope.IsApprovalAsync = false;
        $scope.IsDeclinedAsync = false;

        $scope.CallApproveAfterSuccessUpdate = false;



        function loadUserCommonEntity(num) {

            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
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

            if ($scope.UserCommonEntity.currentMenuID==177)
            {
                $scope.UserCommonEntity.currentTransactionTypeID = 10;
            }
            else 
                $scope.UserCommonEntity.currentTransactionTypeID = 29; // $localStorage.currentTransactionTypeID; // for transactiontyeid not duplicate 
            //$scope.UserCommonEntity.MenuList = $localStorage.MenuList;
            //$scope.UserCommonEntity.ChildMenues = $localStorage.ChildMenues;
            //console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);


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
         
        $scope.IsMrr = false;
        $scope.IsCommonMrrNLoanReceive = false;
        $scope.IsLoanReceive = false;

        $scope.grdTMrNo = "";
        $scope.grdTMrDate = "";
        $scope.grdTSprDate = "";
        $scope.grdTSprNo = "";
        $scope.grdTQCNo = "";
        $scope.grdTQCDate = "";

        function chkTrnsTypeFHideShow() {
           
                $scope.IsMrr = true; 
                $scope.IsCommonMrrNLoanReceive = true; 
                $scope.IsLoanReceive = false;

                $scope.grdTMrNo = "MRR No";
                $scope.grdTMrDate = "MRR Date";

                $scope.grdTGrrNo = "GRR No";
                $scope.grdTGrrDate = "GRR Date";

                $scope.grdTSprDate = "SPR Date";
                $scope.grdTSprNo = "SPR No";
                $scope.grdTQCNo = "QC No";
                $scope.grdTQCDate = "QC Date";
                $scope.grdTSupplier = "Supplier Name";

                $scope.PageTitle = 'MRR Creation';
                $scope.ListTitle = 'MRR Records';
                $scope.ListTitleMrrMasters = 'MRR  Information (Masters)';
                $scope.ListTitleSampleNo = 'Sample Info';
                $scope.ListTitleMrrDeatails = 'MRR Information (Details)';
                 
            
        }

        chkTrnsTypeFHideShow();
         

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



        var baseUrl = '/Inventory/api/MRRAJAccount/';
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
        $scope.TransactionTypeID = 10;
        $scope.MenuID = 85;

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
            var listuser = mRRAccountService.GetList(apiRoute, cmnParam);
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


        //function loadWherehouse(isPaging) {
        //    objcmnParam = {
        //        pageNumber: page,
        //        pageSize: pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };

        //    var apiRoute = baseUrl + 'GetWherehouseList/';

        //    var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        //    $scope.listWarehouse = [];
        //    var listWherehouse = mRRAccountService.GetList(apiRoute, cmnParam);
        //    listWherehouse.then(function (response) {
        //        //  $scope.listWarehouse = response.data.lstWherehouse;
        //        // $scope.listWarehouse = response.data.lstWherehouse[1].ListDpt;

        //        $scope.listWarehouse.push({ OrganogramID: response.data.lstWherehouse[1].ListDpt[5].OrganogramID, OrganogramName: response.data.lstWherehouse[1].ListDpt[5].OrganogramName })

        //        //    .lstWherehouse[1].ListDpt;
        //        //$scope.listWarehouseLocation = response.data.lstWherehouse.ListDpt.ListBrn;
        //        //$scope.listSelfNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf;
        //        //$scope.listRackNo = response.data.lstWherehouse.ListDpt.ListBrn.ListShelf.ListRack;
        //        // console.log(response.data.lstWherehouse);
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });

        //}
        //loadWherehouse(0);

        function loadRecords_OrganogramDept(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganogramsDept = mRRAccountService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
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
                var Issues = mRRAccountService.GetIssueNo(apiRoute, objcmnParam);
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
                var getDuplicateNo = mRRAccountService.GetList(apiRoute, cmnParam);
                getDuplicateNo.then(function (response) {
                    if (response.data.length > 0) {
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

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listMRRQCNo = mRRAccountService.GetList(apiRoute, cmnParam);
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

                var QCDetails = mRRAccountService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);;
                QCDetails.then(function (response) {
                    $scope.ListMrrDetails = response.data.lstQC;
                    $scope.QCCertificateNo = response.data.lstQC[0].QCCertificateNo;


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


        //**********---- QC Detail from QCNo  Changes ----***************//

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

                var issueDetails = mRRAccountService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);;
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
        //    var listGRRNo = mRRAccountService.getModel(apiRoute, page, pageSize, isPaging);
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
        //    var Suppliers = mRRAccountService.GetSuppliers(apiRoute, objcmnParam);
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
        //    var listCurrency = mRRAccountService.getModel(apiRoute, page, pageSize, isPaging);
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

                if ($scope.IsMrr == true && item.MrrValidQty < item.Qty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty < item.Qty) {  //item.QCQty < item.Qty) {
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

                else if ($scope.IsMrr == true  && item.MrrValidQty >= item.Qty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty >= item.Qty) {  //item.QCQty > item.Qty) {
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

                if ($scope.IsMrr == true  && item.MrrValidAdditonalQty < item.AdditionalQty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty < item.Qty) {  //item.QCQty < item.Qty) {
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

                else if ($scope.IsMrr == true  && item.MrrValidAdditonalQty >= item.AdditionalQty) { //($scope.IsPostMRR == true && $scope.IsPreMRR == false && item.AvailableQty >= item.Qty) {  //item.QCQty > item.Qty) {
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


        //**********----Create calculationAccountsUPrice Accounts----***************//
        $scope.calculationAccountsUPrice = function (dataModel) {
            $scope.ListMrrDetails1 = [];
            // var getItemID = dataModel.ItemID;
            angular.forEach($scope.ListMrrDetails, function (item) {

                var newAmnt = item.UnitPrice * item.Qty;

                $scope.ListMrrDetails1.push({
                    MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                    MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                    LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                    UOMName: item.UOMName, UnitPrice: item.UnitPrice, GrrQty: item.GrrQty, Amount: newAmnt, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                    Qty: item.Qty, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,

                    GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                    AdditionalQty: item.AdditionalQty, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty

                });
                
            });
            $scope.ListMrrDetails = $scope.ListMrrDetails1;
        }

        //**********----Create calculationAccountsAmount Accounts----***************//
        $scope.calculationAccountsAmount = function (dataModel) {
            $scope.ListMrrDetails1 = [];
            // var getItemID = dataModel.ItemID;
            angular.forEach($scope.ListMrrDetails, function (item) {

                var newUnitPrice = item.Amount / item.Qty;

                $scope.ListMrrDetails1.push({
                    MrrID: item.MrrID, MrrDetailID: item.MrrDetailID,
                    MrrQcID: item.MrrQcID, MrrQcDetailID: item.MrrQcDetailID, GrrID: item.GrrID, GrrDetailID: item.GrrDetailID, ItemID: item.ItemID,
                    LotID: item.LotID, BatchID: item.BatchID, UnitID: item.UnitID, ItemName: item.ItemName, ItemCode: item.ItemCode, BatchNo: item.BatchNo, LotNo: item.LotNo,
                    UOMName: item.UOMName, UnitPrice: newUnitPrice, GrrQty: item.GrrQty, Amount: item.Amount, QcRemainingQty: item.QcRemainingQty, QCQty: item.QCQty,
                    Qty: item.Qty, CurrentStock: item.CurrentStock, AvailableQty: item.AvailableQty, MrrValidQty: item.MrrValidQty, ReceiveQuantity: item.ReceiveQuantity,

                    GrrAdditionalQty: item.GrrAdditionalQty, QCAdditionalQty: item.QCAdditionalQty,
                    AdditionalQty: item.AdditionalQty, AvailableAdditonalQty: item.AvailableAdditonalQty, MrrValidAdditonalQty: item.MrrValidAdditonalQty

                });
            });
            $scope.ListMrrDetails = $scope.ListMrrDetails1;
        }

        //  Pagination MRR List Master
        //  function for call  when clear or save  or page load not click n open popup

        //  function callPaginationMthd() {
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

            // callPaginationMthd();

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


            //        $scope.IsMrr = false;
            //$scope.IsCommonMrrNLoanReceive = true;
            //$scope.IsLoanReceive = true;

            //$scope.grdTMrNo = "Loan MRR No";
            //$scope.grdTMrDate = "Loan MRR Date"; 
            //$scope.grdTGrrNo = "Loan GRR No";
            //$scope.grdTGrrDate = "Loan GRR Date"; 
            //$scope.grdTQCNo = "Loan QC No";
            //$scope.grdTQCDate = "Loan QC Date";
            //$scope.grdTSprDate = "Loan Date";
            //$scope.grdTSprNo = "Loan Order No"; 
            //$scope.grdTSupplier = "From Company"; 


                    { name: "MrrNo", displayName: $scope.grdTMrNo, title: $scope.grdTMrNo, width: '25%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ManualMRRNo", displayName: "Manual MRRNo", title: "Manual MRRNo", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrDate", displayName: $scope.grdTMrDate, title: $scope.grdTMrDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueNo", displayName: "Issue No", title: "Issue No", visible: false, width: '25%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "IssueDate", displayName: "Issue Date", title: "Issue Date", visible: false, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "DepartmentName", displayName: "Department Name", title: "Department Name", visible: false, width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                     
                    { name: "GrrNo", displayName: $scope.grdTGrrNo, title: $scope.grdTGrrNo, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "GrrDate", displayName: $scope.grdTGrrDate, title: $scope.grdTGrrDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SupplierName", displayName: $scope.grdTSupplier, title: $scope.grdTSupplier, visible:$scope.IsMrr, width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "FromCompanyName", displayName: $scope.grdTSupplier, title: $scope.grdTSupplier, visible: $scope.IsLoanReceive, width: '20%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "MrrQcNo", displayName: $scope.grdTQCNo, title: $scope.grdTQCNo, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MrrQcDate", displayName: $scope.grdTQCDate, title: $scope.grdTQCDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "SprNo", displayName: $scope.grdTSprNo, title: $scope.grdTSprNo, width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPRDate", displayName: $scope.grdTSprDate, title: $scope.grdTSprDate, cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                     
                    { name: "PONO", displayName: "PO NO", title: "PO NO", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PODate", displayName: "PO Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                     
                    { name: "CHDate", displayName: "CH Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },

                    {
                        name: 'Action',
                        displayName: "Action",

                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,
                         
                        width: '12%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                      '<a href="" title="Edit" ng-click="grid.appScope.loadMasterDetailsByMrrMaster(row.entity)">' +
                                        '<i class="icon-edit" aria-hidden="true"></i> Price Update' +
                                      '</a>' +
                                      '</span>'


                    }
                    //,
                    //{
                    //    field: "IsAccountsCompleted",
                    //    displayName: "IsAccountsCompleted",
                    //    width: '12%',
                    //    cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                    //        var cellValue = grid.getCellValue(row, col);
                    //        if (uniqueCellInfoArr.indexOf(cellValue) == 'false') {
                    //            return 'blue';
                    //        }
                    //        else{
                    //            return 'pink';
                    //        }
                    //    }

                    //    //cellClass: function (grid, row, col, rowIndex, colIndex) {
                    //    //    //var val = 'true';
                    //    //    var val = grid.getCellValue(row, col);
                    //    //    if (val == 'false') {
                    //    //        return 'blue';
                    //    //    }
                    //    //    else if (val == 'true') {
                    //    //        return 'pink';
                    //    //    }
                    //    //}
                    //}

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


            //var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
            //var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(mrrID) + "]";

            //var MrrDetails = mRRAccountService.GetList(apiRoute, cmnParam);

            var apiRoute = baseUrl + 'GetMrrMasterListAccount/';

            // sequence............  storeCom, accountsCom, ApprovedCom
            var cmnParam = null;
        

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";

            var listgridOptionsMrrMaster = mRRAccountService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);
            listgridOptionsMrrMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsMrrMaster.data = response.data.lstMrrMaster;
                // $scope.lblMessageForMrrMaster = false;
                $scope.loaderMoreMrrMaster = false;

                //console.log(response.data.lstMrrMaster);
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
      
        $scope.loadMrrRecords(0);



        //**********-#######################################---Load Mrr MasterForm and MrrDetails List By select Mrr Master ----***************//




        $scope.loadMasterDetailsByMrrMaster = function (dataModel) {

            var mrrID ="";
      
            if($scope.ChkApprovalOrUpdateMode == false)
            {
                 mrrID = dataModel.MrrID;
                 $scope.MrrID = dataModel.MrrID;
            }
            else if ($scope.ChkApprovalOrUpdateMode == true) {
                mrrID = dataModel.TransactionID;
                $scope.MrrID = dataModel.TransactionID;
            }
            
            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnMrrShowText = "Show List";
            $scope.IsHidden = true;
            //
            $scope.btnMrrSaveText = "Update";
             
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
              

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

            var MrrDetails = mRRAccountService.GetList(apiRoute, cmnParam, $scope.HeaderToken.get);

            //var apiRoute = baseUrl + 'GetMrrDetailsListByMrrID/';
            //var MrrDetails = mRRAccountService.getMrrDetailsListByMrrID(apiRoute, mrrID, objcmnParam);

            MrrDetails.then(function (response) {

                $scope.ListMrrDetails = response.data.lstDetailInfoByMrrID;

                // $("#ddlQCNo").prop("disabled", true);

                //if (response.data.lstDetailInfoByMrrID[0].IssueID != null) {
                //    $scope.lstIssueNoList = response.data.lstDetailInfoByMrrID[0].IssueID;
                //    $('#ddlIssueNo').select2("data", { id: response.data.lstDetailInfoByMrrID[0].IssueID, text: response.data.lstDetailInfoByMrrID[0].IssueNo });

                //}

                //if (response.data.lstDetailInfoByMrrID[0].DepartmentID != null) {
                //    $scope.Dept = response.data.lstDetailInfoByMrrID[0].DepartmentID;
                //    $('#ddlDept').select2("data", { id: response.data.lstDetailInfoByMrrID[0].DepartmentID, text: response.data.lstDetailInfoByMrrID[0].DepartmentName });

                //}


                if (response.data.lstDetailInfoByMrrID[0].UserID != null) {
                    $scope.ngmMRRByList = response.data.lstDetailInfoByMrrID[0].UserID;
                    $('#ddlMRRBy').select2("data", { id: response.data.lstDetailInfoByMrrID[0].UserID, text: response.data.lstDetailInfoByMrrID[0].UserFullName });

                }

                //if (response.data.lstDetailInfoByMrrID[0].FormDepartmentID != null) {
                //    $scope.FrmDeptIDByIssueChnge = response.data.lstDetailInfoByMrrID[0].FormDepartmentID;

                //}



                if (response.data.lstDetailInfoByMrrID[0].DepartmentID != null) {
                    $scope.Warehouse = response.data.lstDetailInfoByMrrID[0].DepartmentID;
                    $('#ddlWarehouse').select2("data", { id: response.data.lstDetailInfoByMrrID[0].DepartmentID, text: response.data.lstDetailInfoByMrrID[0].DepartmentName });
                }

                $scope.listSupplier = [];
                $scope.lstSupplierList = '';
                $("#ddlSupplier").select2("data", { id: '', text: '' });

                if (response.data.lstDetailInfoByMrrID[0].SupplierID != null) {
                    $scope.listSupplier.push({
                        SupplierID: response.data.lstDetailInfoByMrrID[0].SupplierID, SupplierName: response.data.lstDetailInfoByMrrID[0].SupplierName
                    });

                    $scope.lstSupplierList = response.data.lstDetailInfoByMrrID[0].SupplierID;
                    $("#ddlSupplier").select2("data", { id: response.data.lstDetailInfoByMrrID[0].SupplierID, text: response.data.lstDetailInfoByMrrID[0].SupplierName });
                }

                $scope.listSprNo = [];
                $scope.SprNo = '';
                $("#txtSPRNo").select2("data", { id: '', text: '' });
                if (response.data.lstDetailInfoByMrrID[0].RequisitionID != null) {
                    $scope.listSprNo.push({
                        RequisitionID: response.data.lstDetailInfoByMrrID[0].RequisitionID, SprNo: response.data.lstDetailInfoByMrrID[0].SprNo
                    });

                    $scope.SprNo = response.data.lstDetailInfoByMrrID[0].RequisitionID;
                    $("#txtSPRNo").select2("data", { id: response.data.lstDetailInfoByMrrID[0].RequisitionID, text: response.data.lstDetailInfoByMrrID[0].SprNo });
                }

                $scope.listCurrency = [];
                $scope.lstCurrencyList = '';
                $("#ddlCurrency").select2("data", { id: '', text: '' });
                if (response.data.lstDetailInfoByMrrID[0].CurrencyID != null) {
                    $scope.listCurrency.push({
                        Id: response.data.lstDetailInfoByMrrID[0].CurrencyID, CurrencyName: response.data.lstDetailInfoByMrrID[0].CurrencyName
                    });

                    $scope.lstCurrencyList = response.data.lstDetailInfoByMrrID[0].CurrencyID;
                    $("#ddlCurrency").select2("data", { id: response.data.lstDetailInfoByMrrID[0].CurrencyID, text: response.data.lstDetailInfoByMrrID[0].CurrencyName });
                }


                //$scope.listPONo = [];
                //$scope.PONo = '';
                //$("#txtPONo").select2("data", { id: '', text: '' });

                //if (response.data.lstDetailInfoByMrrID[0].POID != null) {
                //    $scope.listPONo.push({
                //        POID: response.data.lstDetailInfoByMrrID[0].POID, PONo: response.data.lstDetailInfoByMrrID[0].PONO
                //    });

                //    $scope.PONo = response.data.lstDetailInfoByMrrID[0].POID;
                //    $("#txtPONo").select2("data", { id: response.data.lstDetailInfoByMrrID[0].POID, text: response.data.lstDetailInfoByMrrID[0].PONO });
                //}


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

                //$scope.listGRRNo = [];
                //$scope.lstGRRNoList = '';
                //$("#ddlGRRNo").select2("data", { id: '', text: '' });

                //if (response.data.lstDetailInfoByMrrID[0].GrrID != null) {
                //    $scope.listGRRNo.push({
                //        GrrID: response.data.lstDetailInfoByMrrID[0].GrrID, GrrNo: response.data.lstDetailInfoByMrrID[0].GrrNo
                //    });
                //    $scope.lstGRRNoList = response.data.lstDetailInfoByMrrID[0].GrrID;
                //    $("#ddlGRRNo").select2("data", { id: response.data.lstDetailInfoByMrrID[0].GrrID, text: response.data.lstDetailInfoByMrrID[0].GrrNo });
                //}



                $scope.LCNO = response.data.lstDetailInfoByMrrID[0].LCNO;
                $scope.RefChallanNo = response.data.lstDetailInfoByMrrID[0].RefCHNo;
                //var grrdate = response.data.lstQC[0].GrrDate;

                //  console.log(response.data.lstQC[0].GRRDate);
                //  $scope.GRRDate = conversion.getDateToString(response.data.lstQC[0].GrrDate);

                $scope.SPRDate = response.data.lstDetailInfoByMrrID[0].SPRDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].SPRDate);
                //$scope.PODate = response.data.lstDetailInfoByMrrID[0].PODate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].PODate);
                //// $scope.PIDate = dataModel.PIDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].PIDate);
                //$scope.QCDate = response.data.lstDetailInfoByMrrID[0].MrrQcDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].MrrQcDate);
                //$scope.GRRDate = response.data.lstDetailInfoByMrrID[0].GrrDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].GrrDate);

               // $scope.QCCertificateNo = response.data.lstDetailInfoByMrrID[0].QCCertificateNo;
                $scope.ManualMRRNo = response.data.lstDetailInfoByMrrID[0].ManualMRRNo;
                $scope.Remarks = response.data.lstDetailInfoByMrrID[0].Remarks;
                $scope.Description = response.data.lstDetailInfoByMrrID[0].Description;


                //$scope.lstQCNoList = response.data.lstDetailInfoByMrrID[0].MrrQcID;
                //$('#ddlQCNo').select2("data", { id: response.data.lstDetailInfoByMrrID[0].MrrQcID, text: response.data.lstDetailInfoByMrrID[0].MrrQcNo });
                //$scope.QCDate = conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].MrrQcDate);

                $scope.HMRRNO = response.data.lstDetailInfoByMrrID[0].MrrNo;
                $scope.MrrDate = response.data.lstDetailInfoByMrrID[0].MrrDate == null ? "" : conversion.getDateToString(response.data.lstDetailInfoByMrrID[0].MrrDate);

                $scope.ChkApprovalOrUpdateMode = false;
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
            var SaveNewBatch = mRRAccountService.postSaveBatch(apiRoute, batchMaster);
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
            var SaveNewLot = mRRAccountService.postSaveLot(apiRoute, lotMaster);
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

            // $("#save").prop("disabled", true);

            //var varIsStoreCompleted = false;
            //var varIsApproved = false;
            //var varIsAccountsCompleted = false

            //var deptID = null;

            //if ($scope.CommonFIssueNReturnReceive) {
            //    deptID = $scope.Dept;

            //    varIsStoreCompleted = false;
            //    varIsApproved = false;
            //    varIsAccountsCompleted = false;
            //}
            //else if ($scope.IsCommonMrrAccountsDept) {
            //    deptID = $scope.Warehouse;
            //}

            var HedarTokenPostPut = $scope.MrrID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

            var MrrDateStringToDate = conversion.getStringToDate($scope.MrrDate);
            var varIsApproved = false;
            if ($scope.IsApproveChoosen) {

                varIsApproved = true; 
                $scope.IsApproveChoosen = false; //  clear IsApproveChoosen if not approved and after that try to update 
            }
            


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
                DepartmentID: $scope.Warehouse,
                StatusID: 1,
                StatusBy: $scope.UserCommonEntity.loggedUserID,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                UserID: $scope.ngmMRRByList, //$scope.UserCommonEntity.loggedUserID


                IsStoreCompleted: true,
                IsAccountsCompleted: true,
                IsApproved: varIsApproved

            };

            var chkMrrAmount = 1;
            var MssVar = "";

            angular.forEach($scope.ListMrrDetails, function (item) {
                if ((item.Amount <= 0)) {
                    chkMrrAmount = 0;
                    MssVar = " Amount ";
                }
            });



            var menuID = $scope.UserCommonEntity.currentMenuID;
            var transactionTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var mrrDetail = $scope.ListMrrDetails;


            if ($scope.ListMrrDetails.length > 0) {

                if (chkMrrAmount == 1) {
                    var apiRoute = baseUrl + 'SaveUpdateMrrMasterNdetails/';

                    var cmnParam = "[" + JSON.stringify(mrrMaster) + "," + JSON.stringify(mrrDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify($scope.UserCommonEntity) + "]";

                    var MrrMasterNdetailsCreateUpdate = mRRAccountService.GetList(apiRoute, cmnParam, HedarTokenPostPut); //mRRAccountService.postMrrMasterDetail(apiRoute, mrrMaster, mrrDetail, menuID, transactionTypeID);
                    MrrMasterNdetailsCreateUpdate.then(function (response) {
                        var result = 0;
                        if (response.data != "0" && response.data != "" && response.data != "-1" && response.data != "-2") {
                            
                            // alert('Saved Successfully.');
                         
                            $scope.clear(); 
                            $scope.HMRRNO = response.data;

                            if ($scope.IsApprovalAsync == false && $scope.IsDeclinedAsync == false)
                            {
                                Command: toastr["success"]("Save  Successfully!!!!");
                            }
                            $scope.CallApproveAfterSuccessUpdate = true;
                            if($scope.IsApprovalAsync)
                            {
                                $scope.ApprovedMethod();
                                $scope.IsApprovalAsync = false;
                            }
                            if ($scope.IsDeclinedAsync) {
                                $scope.DeclinedMethod();
                                $scope.IsDeclinedAsync = false;
                            }

                            // result = 1;
                        }
                        else if (response.data == "-1") {
                            $scope.CallApproveAfterSuccessUpdate = false;
                            $scope.IsApprovalAsync = false;
                            $scope.IsDeclinedAsync = false;
                                Command: toastr["warning"]("Item Not Tagged with Ledger!!!!");
                            $("#save").prop("disabled", false);
                        }
                        else if (response.data == "-2") {
                            $scope.CallApproveAfterSuccessUpdate = false;
                            $scope.IsApprovalAsync = false;
                            $scope.IsDeclinedAsync = false;
                            Command: toastr["warning"]("Supplier Not Tagged with Ledger!!!!");
                            $("#save").prop("disabled", false);
                        }

                        else if (response.data == "") {
                            $scope.CallApproveAfterSuccessUpdate = false;
                            $scope.IsApprovalAsync = false;
                            $scope.IsDeclinedAsync = false;
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
                else if (chkMrrAmount == 0) {
                    $("#save").prop("disabled", false);
                    Command: toastr["warning"]("" + MssVar + " Must Not Zero Or Empty !!!!");
                }
            }
            else if ($scope.ListMrrDetails.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("MRR Detail Must Not Empty!!!!");
            }
        };



        //**********----Reset Record----***************//
        $scope.clear = function () {
             
            $scope.IsApproveChoosen = false;
            $scope.IsDeclinChoosen = false;
            $scope.ChkApprovalOrUpdateMode = false;
            $scope.CallApproveAfterSuccessUpdate = false;

            $("#save").prop("disabled", false);


            // $scope.btnModal == "Add";
            $scope.MrrID = "0";
            $scope.HMRRNO = "";

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

            $scope.listCompany = [];
            $scope.lstCompanyList = '';
            $("#ddlCompany").select2("data", { id: '', text: '' });

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

            chkTrnsTypeFHideShow();


            loadQCNo(0);

           // loadWherehouse(0);
            loadRecords_OrganogramDept(0);
            loadIssueRecords(0);

            loadRecords_OrganogramDept(0);
            //  loadCurrencyRecords(0);
            loadMrrByRecords(0);

            $scope.loadMrrRecords(0);

        };


        //**************************** #################################   Approve Notification ################################### ********************************


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

            $scope.ChkApprovalOrUpdateMode = true; //  my check
            $scope.loadMasterDetailsByMrrMaster(ApprovalModel);

            //$scope.getCompanyForEdit(ApprovalModel);

        }

        //Approved Or Declained Operation
        $scope.IsApprovalAsync = false;
        $scope.ApprovedMethod = function () {
            $scope.IsApprovalAsync = true;
            $scope.IsApproveChoosen = true;
            $scope.IsDeclinChoosen = false;

            if ($scope.CallApproveAfterSuccessUpdate == false) {
                $scope.save();
            } 

            if ($scope.CallApproveAfterSuccessUpdate == true) {

                $scope.IsApprovalAsync = false;
                $scope.IsApproveChoosen = false;
                $scope.IsDeclinChoosen = false;

                ApprovalModel.Comments = $scope.commentsModle;
                ApprovalModel.CreatorID = $scope.UserCommonEntity.loggedUserID;
                ApprovalModel.LoggedCompanyID = $scope.UserCommonEntity.loggedCompnyID;
                ApprovalModel.LoggedUserID = $scope.UserCommonEntity.loggedUserID;
                $scope.commentsModle = "";
                modal_fadeOutApproved();
                var apiRoute = '/Inventory/api/InventoryLayout/ApproveNotification/';
                var approvalProcess = mRRAccountService.post(apiRoute, ApprovalModel);
                approvalProcess.then(function (response) {
                    if (response.data == 200) {
                        $scope.IsApprovalAsync = false;
                        //$scope.IsApproveChoosen = true;
                        //$scope.IsDeclinChoosen = false;
                        //$scope.save();
                        //Hide Form
                        $scope.PageDisplay = false;

                        $scope.CallApproveAfterSuccessUpdate == false;

                        ShowCustomToastrMessage(response);
                        // modal_fadeOut_Company();
                    }
                },
                function (error)
                {
                    $scope.CallApproveAfterSuccessUpdate == false;
                    ("Error: " + error);

                });
            }
        }

        $scope.IsDeclinedAsync = false;
        $scope.DeclinedMethod = function () {
            $scope.IsDeclinedAsync = true;
            $scope.IsApproveChoosen = false;
            $scope.IsDeclinChoosen = true;
            if ($scope.CallApproveAfterSuccessUpdate == false) {
                $scope.save();
            }

            if ($scope.CallApproveAfterSuccessUpdate == true) {

                $scope.IsDeclinedAsync = false;
                $scope.IsApproveChoosen = false;
                $scope.IsDeclinChoosen = false;

                ApprovalModel.Comments = $scope.commentsModle;
                ApprovalModel.CreatorID = $scope.UserCommonEntity.loggedUserID;
                ApprovalModel.LoggedCompanyID = $scope.UserCommonEntity.loggedCompnyID;
                ApprovalModel.LoggedUserID = $scope.UserCommonEntity.loggedUserID;
                $scope.commentsModle = "";
                modal_fadeOutDeclained();
                var apiRoute = '/Inventory/api/InventoryLayout/DeclainedNotification/';
                var declaineProcess = mRRAccountService.post(apiRoute, ApprovalModel);
                declaineProcess.then(function (response) {
                    if (response.data == 201) {
                        $scope.IsDeclinedAsync = false;
                        //$scope.IsApproveChoosen = false;
                        //$scope.IsDeclinChoosen = true;
                        //$scope.save();
                        $scope.CallApproveAfterSuccessUpdate == false;
                        //Hide Form
                        $scope.PageDisplay = false;
                        ShowCustomToastrMessage(response);
                        // modal_fadeOut_Company();
                    }
                },
                function (error) {
                    $scope.CallApproveAfterSuccessUpdate == false;
                    ("Error: " + error);
                });
            }
        }

    }]);

function modal_fadeOutApproved() {
    $("#approveNotificationModal").fadeOut(200, function () {
        $('#approveNotificationModal').modal('hide');
    });
}
function modal_fadeOutDeclained() {
    $("#declainedNotificationModal").fadeOut(200, function () {
        $('#declainedNotificationModal').modal('hide');
    });
}

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

