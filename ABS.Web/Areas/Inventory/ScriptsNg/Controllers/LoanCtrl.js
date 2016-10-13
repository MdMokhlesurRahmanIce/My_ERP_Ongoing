/**
* PICtrl.js
*/

//app.controller('RequisitionCtrl', function ($scope, RequisitionService, conversion) {


app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);

app.controller('LoanCtrl', ['$scope', 'RequisitionService', '$filter', 'uiGridConstants', 'conversion', '$localStorage',
    function ($scope, RequisitionService, $filter, uiGridConstants, conversion, $localStorage) {

        //Rq
        $scope.gridOptionsRequisition = [];
        var objcmnParam = {};
        var TransactionTypeId = 33;
        $scope.ChkManualMRRNoWhnSave = "";
        //ItemPop
        $scope.gridOptionsItemPop = [];
        var objcmnParamItemPop = {};

        $scope.gridOptionslistItemMaster = [];
        $scope.gridOptionsHDO = [];

        var baseUrl = '/Inventory/api/SPR/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 3000;
        var isPaging = 0;
        var totalData = 0;
        var ItemTypeID = 1;
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
        $scope.ChkApprovalOrUpdateMode = false;
        var date = new Date();
        $scope.RequistionDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.EstDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.ListBuyer = [];
        $scope.bool = true;

        $scope.MenuID = $localStorage.currentMenuID;

        var DepartmentId = $localStorage.loggedUserDepartmentID;
        var varGroupID = "";
        var UserTypeID = 1;
        $scope.requisitionID = "0";
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'Loan Creation';
        $scope.ListTitle = 'Loan Records';
        $scope.ListTitleActivePIMasters = 'Loan  Information (Masters)';
        $scope.ListTitleSampleNo = 'Item Details';
        $scope.ListTitlePIDeatails = 'Laon Information (Details)';
        $scope.ListTitleRequisitioneatails = 'Loan Item List';
        $scope.ListTitleRequisitionMaster = 'Loan List';
        $scope.PanelTitle = 'Item Info';
        $scope.btnModal = "Add";
        $scope.ListPIDetails = [];
        $scope.ListActivePIMaster = [];
        $scope.OverdueInterest = 2;
        $scope.ListRequisitionDetail = [];
        $scope.ListRequisitionDetails = [];
        //*************---Show and Hide Order---**********

        //***************************************** Start Load User Common Entity ****************************************
        $scope.loadUserCommonEntity = function (num) {
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
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




        //**************************************
        // Show and Hide Order
        //**************************************
        //getItmDetailsByItmCode()
        $scope.IsApproveChoosen = false;
        $scope.IsDeclinChoosen = false;
        $scope.IsInvalid = true;
        $scope.IsAddDisable = true;
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsShowD = false;
        $scope.IsHiddenDetail = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsShowD = $scope.ListRequisitionDetails.length > 0 ? true : false;
                $scope.IsListIcon = true;
                $scope.IsCreateIcon = false;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.btnSaveText = "Save";
                $scope.IsInvalid = true;
                $scope.IsShowD = false;
                $scope.IsHidden = false;
                $scope.IsListIcon = false;
                $scope.IsCreateIcon = true;
                loadRequisition_Records(0);

            }
        }

        //######## Start Load Company ################//

        function loadRecords_Company(isPaging) {
            $scope.ListCompany = [];
            $("#ddlCompany").select2("data", { id: 0, text: '--Select Company--'});
            var apiRoute = baseUrl + 'GetCompany/';
            var listComp = RequisitionService.GetAllCompany(apiRoute, page, pageSize, isPaging, $scope.UserCommonEntity.loggedCompnyID);
            listComp.then(function (response) {
                $scope.ListCompany = response.data              
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Company(0)

        //######## Start Check Duplicate No ################//

        $scope.ChkDuplicateNo = function () {

            var getMNo = $scope.ManualSPRNo;

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
                var getDuplicateNo = RequisitionService.GetList(apiRoute, cmnParam);

                getDuplicateNo.then(function (response) {

                    $scope.ChkManualMRRNoWhnSave = "";

                    if (response.data.length > 0) {

                        $scope.ChkManualMRRNoWhnSave = response.data[0].ManualRequisitionNo
                        $scope.ManualSPRNo = "";
                        Command: toastr["warning"]("Manual Loan No Already Exists.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (getMNo.trim() == "") {

                Command: toastr["warning"]("Please Enter Manual Laon No.");
            }

        }

        //######## End Check Duplicate No ################//

        //**********----File Updload----***************//
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
            debugger
            var id = dataModel.RequisitionID;
            var TransTypeID = $scope.UserCommonEntity.currentTransactionTypeID;
            var apiRoute = baseUrl + 'GetFileDetailsById/' + id + '/' + TransTypeID;

            var ListFileDetails = RequisitionService.getModelByID(apiRoute);
            ListFileDetails.then(function (response) {
                $scope.ListFileDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----get Item  Record from itemList popup ----***************//

        $scope.getItmDetailsByItemId = function (dataModel) {

            $scope.IsHiddenDetail = false;
            $scope.IsShowD = true;
            var existItem = dataModel.ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListRequisitionDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    return false;
                }
            });
            var Currstock = $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].CurrentStock : 0;
            var TransitQty = $scope.ListItemDetails[0].length > 0 ? $scope.ListItemDetails[0].transitQty : 0;
            CurrentStock = Currstock - TransitQty;
            if (duplicateItem === 0) {
                $scope.ListRequisitionDetails.push({
                    RequisitionID: 0,
                    RequisitionDetailID: 0,
                    ArticleNo: dataModel.ArticleNo,
                    ItemID: dataModel.ItemID,
                    ItemName: dataModel.ItemName,
                    UnitID: dataModel.UOMID,
                    UnitName: dataModel.UOMName,
                    Qty: 0,
                    PurchaseRate: $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].PurchaseRate : 0,
                    UnitPrice: $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].UnitPrice : 0,
                    MrrNo: $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].MrrNo : 0,
                    MrrDate: $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].MrrDate : '',
                    MRRQty: $scope.ListItemDetails.length > 0 ? $scope.ListItemDetails[0].MRRQty : 0,
                    CurrentStock: CurrentStock,
                    ModelState: 'Save'
                });
                $scope.IsInvalid = $scope.ListRequisitionDetails.length > 0 ? false : true;
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
            // $scope.showDtgrid = $scope.ListRequisitionDetails.length;
        }

        //**********----Get Item Detail by Item Code ----***************// 
        $scope.ListItemDetailsForSearch = [];
        $scope.getItmDetailsByItmCode = function () {

            var ItemCode = $scope.ItemCode;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $localStorage.loggedUserDepartmentID,
                menuId: $localStorage.currentMenuID,
                tTypeId: 25
            };

            if (ItemCode.Trim != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItmCode/';
                var listItemSerch = RequisitionService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemCode, $scope.HeaderToken.get);
                listItemSerch.then(function (response) {
                    if (response.data[0].ItemID > 0) {
                        $scope.ListItemDetailsForSearch = response.data;
                        $scope.IsAddDisable = false;
                    }
                    else {
                        $scope.IsAddDisable = true;
                        Command: toastr["warning"]("Item Not Exist.");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            else if (ItemCode.Trim == "") {

                    Command: toastr["warning"]("Please Enter Item Code.");
            }

        }

        //**********----Get Item Detail by Item Code ----***************// 
        $scope.ListItemDetails = [];
        $scope.GetItemDetailsByItemID = function (dataModel) {
            var ItemId = dataModel.ItemID;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID:  $scope.UserCommonEntity.loggedUserDepartmentID,
                menuId:  $scope.UserCommonEntity.currentMenuID,
                tTypeId: 25
            };

            if (ItemId != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItemId/';
                var listItemSerch = RequisitionService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemId, $scope.HeaderToken.get);
                listItemSerch.then(function (response) {
                    $scope.ListItemDetails = response.data;
                    $scope.IsAddDisable = false;
                    $scope.IsAddDisable = true;
                    $scope.getItmDetailsByItemId(dataModel);
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            //else if (ItemCode.Trim == "") {

            //        Command: toastr["warning"]("Please Enter Item Code.");
            //}

        }



        //**********----Add Items To List ----***************// 

        $scope.AddItem = function () {
            $scope.IsHiddenDetail = false;

            var existItem = $scope.ListItemDetailsForSearch[0].ItemID;
            var duplicateItem = 0;
            angular.forEach($scope.ListRequisitionDetails, function (item) {
                if (existItem == item.ItemID) {
                    duplicateItem = 1;
                    $scope.IsAddDisable = true;
                    return false;
                }
            });
            if (duplicateItem === 0) {

                $scope.ListRequisitionDetails.push({
                    ArticleNo: $scope.ListItemDetailsForSearch[0].ArticleNo,
                    ItemID: $scope.ListItemDetailsForSearch[0].ItemID,
                    ItemName: $scope.ListItemDetailsForSearch[0].ItemName,
                    UnitID: $scope.ListItemDetailsForSearch[0].UOMID,
                    UnitName: $scope.ListItemDetailsForSearch[0].UOMName,
                    Qty: 0,
                    CurrentRate: $scope.ListItemDetailsForSearch[0].CurrentRate,
                    UnitPrice: $scope.ListItemDetailsForSearch[0].UnitPrice,
                    MrrNo: $scope.ListItemDetailsForSearch[0].MrrNo,
                    MrrDate: $scope.ListItemDetailsForSearch[0].MrrDate,
                    MRRQty: $scope.ListItemDetailsForSearch[0].MRRQty,
                    CurrentStock: $scope.ListItemDetailsForSearch[0].CurrentStock,
                    ModelState: 'Save'
                });
                $scope.IsShowD = true;
                $scope.IsAddDisable = true;
                $scope.IsInvalid = $scope.ListRequisitionDetails.length > 0 ? false : true;
                $scope.ItemCode = "";
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Exists!!!!");
            }
        }



        //**********----Get Unit Price Record and filter by ItemID ----***************// 

        $scope.LaodItemRateNUnit = function LoadItemPrice(isPaging) {

            var ItemID = $scope.ddlItem;
            if (ItemID != "") {
                var apiRoute = baseUrl + 'LaodItemRateNUnit/';
                var listItemPrice = RequisitionService.getByID(apiRoute, page, pageSize, isPaging, ItemID, $scope.HeaderToken.get);
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


        //**********----Get Item Record and filter by GroupID ----***************// 

        $scope.LaodItemByGroupID = function LoadItemList(isPaging) {
            $scope.listItems = [];
            var GroupID = varGroupID == "" ? $scope.lstItemGroup : varGroupID;
            if (GroupID != "") {
                var apiRoute = baseUrl + 'GetItemListByGroupID/';
                var listItem = RequisitionService.getByID(apiRoute, page, pageSize, isPaging, GroupID);
                listItem.then(function (response) {
                    $scope.listItems = response.data
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        //**********----Get Requisition Master info ----***************// 

        $scope.GetRequisitonMasterByReqID = function (dataModel) {

            $scope.btnSaveText = "Update";
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.btnShowText = "Show List";
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            var RequisitionId = '';
            //if ($scope.ChkApprovalOrUpdateMode == false) {
            //    RequisitionId = dataModel.RequisitionID;
            //    $scope.requisitionID = dataModel.RequisitionId;
            //}
            //else if ($scope.ChkApprovalOrUpdateMode == true) {
            //    RequisitionId = dataModel.TransactionID;
            //    $scope.requisitionID = dataModel.TransactionID;

            //}

            RequisitionId = dataModel.RequisitionID;
            $scope.RequisitionId = dataModel.RequisitionID;
            $scope.requisitionID = dataModel.RequisitionID;
            var apiRoute = baseUrl + 'GetRequisitonMasterByRequisitionID/';
            var ListRequisitionInfoMaster = RequisitionService.getByRequisitionID(apiRoute, RequisitionId, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
            ListRequisitionInfoMaster.then(function (response) {
                $scope.CustomrequisitionNo = response.data.RequisitionNo;
                $scope.RequisitionBy = response.data.RequisitionBy;
                $scope.ManualSPRNo = response.data.ManualRequisitionNo;
                $scope.RequistionDate = conversion.getDateToString(response.data.RequisitionDate);
                $scope.EstDate = conversion.getDateToString(response.data.EstDate);
                $("#ddlRequisitionBy").select2("data", { id: response.data.RequisitionBy, text: response.data.RequisitionByName });
                $("#ddlDepartment").select2("data", { id: response.data.OrganogramID, text: response.data.ToDepartmentName });
                $scope.Purpose = response.data.Purpose;
                $scope.Remarks = response.data.Remarks;
                $scope.IsShow = true;
                $scope.ChkApprovalOrUpdateMode = false;
            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShow = true;
            });
        }


        //**********----Get Requisition Detail info ----***************// 

        $scope.GetReqDetailByRequisitionID = function (dataModel) {
            $scope.ListRequisitionDetailstemp = [];
            $scope.ListRequisitionDetails = [];
            $scope.IsShowD = false;
            var RequisitionId = '';
            //if ($scope.ChkApprovalOrUpdateMode == false) {
            //    RequisitionId = dataModel.RequisitionID;
            //    $scope.requisitionID = dataModel.RequisitionId;
            //}
            //else if ($scope.ChkApprovalOrUpdateMode == true) {
            //    RequisitionId = dataModel.TransactionID;
            //    $scope.requisitionID = dataModel.TransactionID;
            //}
            RequisitionId = dataModel.RequisitionID;
            var apiRoute = baseUrl + 'GetRequisitonDetailByRequisitionID/';
            var ListRequisitionInfoMaster = RequisitionService.getByRequisitionID(apiRoute, RequisitionId, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
            ListRequisitionInfoMaster.then(function (response) {
                $scope.ListRequisitionDetailstemp = response.data;
                angular.forEach($scope.ListRequisitionDetailstemp, function (item) {
                    $scope.ListRequisitionDetails.push({
                        RequisitionID: item.RequisitionID, RequisitionDetailID: item.RequisitionDetailID, ArticleNo: item.ArticleNo, ItemName: item.ItemName,
                        UnitName: item.UnitName, ItemID: item.ItemID, UnitID: item.UnitID, Qty: item.Qty, MrrNo: item.MrrNo, UnitPrice: item.UnitPrice, MrrDate: '',
                        CurrentStock: item.CurrentStock, ModelState: 'Update'
                    })
                })
                $scope.IsShowD = $scope.ListRequisitionDetailstemp.length > 0 ? true : false;
                $scope.IsInvalid = $scope.ListRequisitionDetails.length > 0 ? false : true;
                $scope.ChkApprovalOrUpdateMode == false;
            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShow = true;
            });

        }


        //**********----Get Company Record and filter by CompanyID and cascading with Department ----***************//   

        function loadRecords_Departement(isPaging) {
            $scope.ListDepartment = [];
            $("#ddlDepartment").select2("data", { id: 0, text: '--Select Department--' });
            var apiRoute = baseUrl + 'GetDepartmentByCompanyID/';
            var listDepartment = RequisitionService.getCompanyByDeptID(apiRoute, page, pageSize, isPaging, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
            listDepartment.then(function (response) {
                $scope.ListDepartment = response.data
                angular.forEach($scope.ListDepartment, function (item) {
                    if (item.OrganogramID == $scope.UserCommonEntity.loggedUserDepartmentID) {
                        $scope.Department = item.OrganogramID;
                        $("#ddlDepartment").select2("data", { id: item.OrganogramID, text: item.OrganogramName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Departement(0)

        

        ////**********----Get All Lot Record----***************
        //function loadLotRecords(isPaging) {
        //    var apiRoute = '/Inventory/api/StockEntry/GetLotNo/';
        //    var lisLotNo = RequisitionService.getAllLotNo(apiRoute, page, pageSize, isPaging);
        //    lisLotNo.then(function (response) {
        //        $scope.listLot = response.data
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //loadLotRecords(0);

        // -------Get All Users //--------
        $scope.UserLst = [];
        function GetAllUsers() {
            var apiRoute = '/Inventory/api/SPR/GetAllUsers/';
            var UserLst = RequisitionService.getAllUsers(apiRoute, page, pageSize, isPaging, UserTypeID, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
            UserLst.then(function (response) {
                $scope.UserLst = response.data
                angular.forEach($scope.UserLst, function (item) {
                    if (item.UserID == $scope.UserCommonEntity.loggedUserID) {
                        $scope.RequisitionBy = item.UserID;
                        $("#ddlRequisitionBy").select2("data", { id: item.UserID, text: item.UserFullName });
                    }
                });
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllUsers(0);



        //*******************organogram  parent DropDown On Page Load-- ***********
        function loadRecords_Organogram(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganograms = RequisitionService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
            listOrganograms.then(function (response) {
                $scope.ListParentOrganogram = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Organogram(0);



        function loadRecords_OrganogramDept(isPaging) {
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
            var listOrganogramsDept = RequisitionService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
            listOrganogramsDept.then(function (response) {
                $scope.listOrganogramsDepartment = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_OrganogramDept(0);

        // -------Get All raw items //--------
        $scope.GetRawItems = function () {

            var apiRoute = '/Inventory/api/StockEntry/GetItemList/';
            var items = RequisitionService.getAllRawItems(apiRoute, page, pageSize, isPaging);
            items.then(function (response) {
                $scope.items = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetRawItems();


        $scope.lstBatchList = [];

        // -------Get All Batch //--------
        function GetAllBatchNo() {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        GetAllBatchNo();


        //////**********----Day difference between two dates----***************


        $scope.GetDays = function () {

            var firstdate = $scope.RequistionDate;
            var seconddate = $scope.EstDate;

            var dt1 = firstdate.split('-'),
                dt2 = seconddate.split('-'),
                one = new Date(dt1[2], dt1[1] - 1, dt1[0]),
                two = new Date(dt2[2], dt2[1] - 1, dt2[0]);

            var millisecondsPerDay = 1000 * 60 * 60 * 24;
            var millisBetween = two.getTime() - one.getTime();
            var days = millisBetween / millisecondsPerDay;
            var diffDays = Math.floor(days);
            if (diffDays < 0) {
                $scope.Estday = 0;
                Command: toastr["warning"]("Back date not allowed!!!!");
                $scope.EstDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
                return false;
            }
            else {
                $scope.Estday = diffDays + 1;
            }

        }
        //////**********----delete  Record from ListRequisitionDetails----***************

        $scope.ListRequisitionDetails = [];
        $scope.deleteRow = function (index) {
            $scope.ListRequisitionDetails.splice(index, 1);
            $scope.IsInvalid = $scope.ListRequisitionDetails.length > 0 ? false : true;
            $scope.IsShowD = $scope.ListRequisitionDetails.length > 0 ? true : false;
            $scope.btnSaveText = "Save";

        };
        //Grid-1-Requisition
        //Pagination
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 0, pageSize: 15, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            //pageSizeChange: function () {
            //    if (this.ddlpageSize == "All")
            //        this.pageSize = $scope.pagination.totalItems;
            //    else
            //        this.pageSize = this.ddlpageSize

            //    this.pageNumber = 1
            //    loadRequisition_Records(1);
            //},

            pageSizeChange: function () {
                if (this.ddlpageSize == "All") {
                    this.pageNumber = 1
                    this.pageSize = $scope.pagination.totalItems;
                    loadRequisition_Records(1);
                }
                else {
                    this.pageSize = this.ddlpageSize
                    this.pageNumber = 1
                    loadRequisition_Records(1);
                }


            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadRequisition_Records(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadRequisition_Records(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadRequisition_Records(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadRequisition_Records(1);
                }
            }
        };

        //**********----Get All Record----***************
        function loadRequisition_Records(isPaging) {

            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: $scope.pagination.pageNumber,
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $localStorage.currentMenuID,
                tTypeId: 25
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsRequisition = {
                columnDefs: [
                    { name: "RequisitionID", displayName: "Requisition ID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionNo", displayName: "Loan No", title: "Loan No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ManualRequisitionNo", displayName: "Manual Loan No", title: "Manual Loan No", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionDate", displayName: "Laon Date", title: "Laon Date", cellFilter: 'date:"dd-MM-yyyy"', width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "OrganogramName", title: "SPR Department", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "EstDate", title: "Estimate Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionByName", displayName: "Laon By", title: "Laon By", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", title: "Remarks", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Purpose", title: "Purpose", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '10%',
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
                                  '<a href="" title="Edit" ng-click="grid.appScope.GetRequisitonMasterByReqID(row.entity);grid.appScope.GetReqDetailByRequisitionID(row.entity)">' +
                                    '<i class="icon-edit"></i> Edit' +
                                  '</a>' +
                                  '</span>'

                        //cellTemplate: '<span class="label label-warning label-mini">' +
                        //                    '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                        //                        '<i class="icon-pencil" ng-click="grid.appScope.GetRequisitonMasterByReqID(row.entity);grid.appScope.GetReqDetailByRequisitionID(row.entity)"></i>' +
                        //                    '</a>' +
                        //                '</span>' 
                        //+ '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                        //   '<a href="javascript:void(0);" ng-href="#CmnDeleteModal" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="bs-tooltip" title="Delete" ng-click="grid.appScope.loadDelModel(row.entity)">' +
                        //         '<i class="glyphicon glyphicon-trash" aria-hidden="true"></i> Delete' +
                        //   '</a>' +
                        // '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Requisition.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Requisition", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetRequisitionMasterSPR/';
            debugger
            var listReqMaster = RequisitionService.GetRequisitionMst(apiRoute, objcmnParam, $scope.UserCommonEntity.currentTransactionTypeID, $scope.HeaderToken.get);
            listReqMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsRequisition.data = response.data.objRequisitionMaster;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRequisition_Records(0);



        //Grid-2
        //Pagination
        $scope.paginationItemPop = {
            paginationPageSizesItemPop: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSizeItemPop: 15,
            pageNumber: 1,
            pageSize: 15,
            totalItemsPop: 0,

            getTotalPagesItemPop: function () {
                return Math.ceil(this.totalItemsPop / this.ddlpageSizeItemPop);
            },
            //pageSizeChangeItemPop: function () {
            //    if (this.ddlpageSizeItemPop == "All") {
            //        this.pageNumber = 1
            //        this.pageSize = $scope.paginationItemPop.totalItemsPop;
            //        loadItemPop_Records(1);
            //    }
            //    else {
            //        this.pageSize = this.ddlpageSizeItemPop
            //        this.pageNumber = 1
            //        loadItemPop_Records(1);
            //    }
            //},

            pageSizeChangeItemPop: function () {
                if (this.ddlpageSizeItemPop == "All") {
                    this.pageNumber = 1
                    this.pageSize = 0
                    this.pageSize = $scope.paginationItemPop.totalItemsPop;
                    loadItemPop_Records(1);
                }
                else {
                    this.pageSize = this.ddlpageSizeItemPop
                    this.pageNumber = 1
                    loadItemPop_Records(1);
                }


            },
            firstPageItemPop: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadItemPop_Records(1);
                }
            },
            nextPageItemPop: function () {
                if (this.pageNumber < this.getTotalPagesItemPop()) {
                    this.pageNumber++;
                    loadItemPop_Records(1);
                }
            },
            previousPageItemPop: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadItemPop_Records(1);
                }
            },
            lastPageItemPop: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPagesItemPop();
                    loadItemPop_Records(1);
                }
            }
        };

        //**********----Get All Record----***************
        function loadItemPop_Records(isPaging) {

            // For Loading   
            $scope.loaderMore = true;
            $scope.lblMessageItemPop = 'loading please wait....!';
            $scope.resultItemPop = "color-red";

            //Ui Grid
            objcmnParamItemPop = {
                pageNumber: $scope.paginationItemPop.pageNumber,
                pageSize: $scope.paginationItemPop.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 0
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsItemPop = {
                columnDefs: [
                    { name: "ItemID", displayName: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CurrentRate", displayName: "CurrentRate", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "Item Code", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemName", title: "Item Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "UOMName", title: "Unit", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '7%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                     '<a href="" title="Add" ng-click="grid.appScope.GetItemDetailsByItemID(row.entity)">' +//grid.appScope.getItmDetailsByItemId(row.entity)"
                                       '<i class="icon-check" aria-hidden="true"></i> Add' +
                                     '</a>' +
                                     '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Item.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Item", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetItmDetail/';
            var listItemMaster = RequisitionService.GetItmDetail(apiRoute, objcmnParamItemPop, $scope.HeaderToken.get);
            listItemMaster.then(function (response) {
                $scope.paginationItemPop.totalItemsPop = response.data.recordsTotal;
                $scope.gridOptionsItemPop.data = response.data.objItem;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadItemPop_Records(0);


        //////**********----Create New Record----***************
        $scope.save = function () {


            if ($scope.ChkManualMRRNoWhnSave == "") {
                
                var RequisitinDate = conversion.getStringToDate($scope.RequistionDate);
                var EstDate = conversion.getStringToDate($scope.EstDate);
               
                if ($scope.ManualSPRNo == "" || typeof ($scope.ManualSPRNo) === "undefined")
                {
                    Command: toastr["warning"]("Please enter manual Laon No!!!!");
                    $('#ManualSPR').focus()
                    return;
                }

                if ($('#ddlCompany').val() == "" || typeof ($('#ddlCompany').val()) === "undefined") {
                    Command: toastr["warning"]("Please select company!!!!");
                    $('#ddlCompany').focus()
                    return;
                }

                if ($scope.Purpose == "" || typeof ($scope.Purpose) === "undefined") {
                    Command: toastr["warning"]("Please enter purpose!!!!");
                    $('#txtPurpose').focus()
                    return;
                }
               

                var RequisitionMaster = {
                    RequisitionID: $scope.requisitionID,
                    RequisitionDate: RequisitinDate,
                    ManualRequisitionNo: $scope.ManualSPRNo,
                    RequisitionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,                  // Transaction Type 8 for SPR
                    RequisitionBy: $scope.RequisitionBy,
                    Remarks: $scope.Remarks,
                    Description: $scope.Description,
                    Purpose: $scope.Purpose,
                    ToCompanyID: $scope.Company,
                    ToDepartmentID: $scope.Department,
                    EstDate: EstDate,
                    StatusID: 1,
                    StatusBy: $scope.UserCommonEntity.loggedUserID,
                    StatusDate: new Date(),
                    CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                    DepartmentID: $scope.Department,
                    IsComplete: false,
                    IsGrrComplete: false,
                    CreateBy: $scope.UserCommonEntity.loggedUserID,
                    CreateOn: new Date(),
                    IsDeleted: false
                    
                };

                var fileList = [];
                angular.forEach($scope.files, function (item) {
                    this.push(item.name);
                }, fileList);

                var chkAmount = 1;
                angular.forEach($scope.ListRequisitionDetails, function (item) {
                    if ((item.Qty <= 0) || typeof (item.Qty) === "undefined") {
                        chkAmount = 0;
                    }
                });

                var RequisitionDetail = $scope.ListRequisitionDetails;
                var UserCommonEntity = $scope.UserCommonEntity;
                if (chkAmount == 1) {
                    var apiRoute = baseUrl + 'SaveRequisitionMasterDetails/';
                    var RequisitionMasterDetailsCreate = RequisitionService.postSPRMasterDetail(apiRoute, RequisitionMaster, RequisitionDetail, UserCommonEntity, fileList, $scope.HeaderToken.get);
                    RequisitionMasterDetailsCreate.then(function (response) {
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


                            if (response.data == "Update") {
                                $scope.clear();
                                Command: toastr["success"]("Update  Successfully!!!!");
                            }
                            else if (response.data == "-1") {
                                    Command: toastr["warning"]("Save failed!!!!");
                            }
                            else {
                                $scope.clear();
                                $scope.CustomrequisitionNo = response.data;
                                Command: toastr["success"]("Save  Successfully!!!!");
                            }
                        }
                    },
                    function (error) {
                        console.log("Error: " + error);
                    });
                }
                else if (chkAmount == 0) {
                        Command: toastr["warning"]("Loan Quantity Must Not Zero Or Empty !!!!");

                }
            }
            else if ($scope.ChkManualMRRNoWhnSave != "") {

                    Command: toastr["warning"]("Manual Loan No Already Exists.");
            }
        };

        //****************************************Start Delete******************************************
        $scope.loadDelModel = function (EntityModel) {

            $scope.SelectedItemName = "You are about to delete " + EntityModel.RequisitionNo + ". Are you sure?";
            $scope.CmnDelModel = EntityModel;
            $scope.IsConfShow = true;
            $scope.IsConfirmShow = false;
        }
        $scope.ConfirmYes = function () {

            $scope.SprDelete($scope.CmnDelModel);
        }

        $scope.SprDelete = function (dataModel) {
            var ID = dataModel.RequisitionID;
            objcmnParam = {
                pageNumber: 1,//--------------will start
                pageSize: 10,
                IsPaging: isPaging,
                ItemType: 0,
                ItemGroup: 0,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID == null || angular.isUndefined($scope.UserCommonEntity.loggedUserDepartmentID) ? 1 : $scope.UserCommonEntity.loggedUserDepartmentID,
                id: ID
            };

            //var IsConf = confirm('You are about to delete ' + dataModel.LCBMRRNo + '. Are you sure?');
            //if (IsConf) {
            var apiRoute = baseUrl + 'SprUpdateDelete/';
            var delSprMasterDetail = RequisitionService.GetItmDetail(apiRoute, objcmnParam);
            delSprMasterDetail.then(function (response) {
                if (response.data != "") {
                    $scope.clear();
                    Command: toastr["success"]("SPR No " + dataModel.RequisitionNo + " has been deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("SPR No " + dataModel.RequisitionNo + " not deleted!!!!");
                }
            },
            function (error) {
                Command: toastr["warning"]("SPR No " + dataModel.RequisitionNo + " not deleted!!!!");
                console.log("Error: " + error);
            });
            //}
        }
        //*****************************************End Delete*******************************************

        //////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.IsApproveChoosen = false;
            $scope.IsDeclinChoosen = false;
            $scope.ChkApprovalOrUpdateMode = false;
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.IsInvalid = true;
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.ManualSPRNo = "";
            $scope.IsAddDisable = true;
            $scope.IsShowD = false;
            $scope.IsHiddenDetail = true;
            $scope.btnShowText = "Show List";
            $scope.requisitionID = "0";
            $scope.btnSaveText = "Save";
            $scope.ListRequisitionDetails = [];
            $scope.RequisitionMaster = [];
            $scope.Description = "";
            $scope.Remarks = "";
            $scope.Purpose = "";
            $scope.ItemCode = "";
            $scope.IsInvalid = true;
            $scope.EstDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            loadRecords_Departement(0);
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
            $scope.ChkApprovalOrUpdateMode = true;
            $scope.GetRequisitonMasterByReqID(ApprovalModel);
            $scope.GetReqDetailByRequisitionID(ApprovalModel);

            //modal_fadeIn_Company();
        }

        //Approved Or Declained Operation
        $scope.ApprovedMethod = function () {

            ApprovalModel.Comments = $scope.commentsModle;
            ApprovalModel.CreatorID = $('#hUserID').val();
            ApprovalModel.LoggedCompanyID = $('#hCompanyID').val();
            ApprovalModel.LoggedUserID = $('#hUserID').val();
            $scope.commentsModle = "";
            modal_fadeOut();
            var apiRoute = '/Inventory/api/InventoryLayout/ApproveNotification/';
            var approvalProcess = RequisitionService.post(apiRoute, ApprovalModel);
            approvalProcess.then(function (response) {
                if (response.data == 200) {
                    $scope.IsApproveChoosen = false;
                    $scope.IsDeclinChoosen = true;
                    //Hide Form
                    $scope.PageDisplay = false;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    //  modal_fadeOut_Company();
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
            var apiRoute = '/Inventory/api/InventoryLayout/DeclainedNotification/';
            var declaineProcess = RequisitionService.post(apiRoute, ApprovalModel);
            declaineProcess.then(function (response) {
                if (response.data == 201) {
                    $scope.IsApproveChoosen = false;
                    $scope.IsDeclinChoosen = true;
                    //Hide Form
                    $scope.PageDisplay = false;
                    ShowCustomToastrMessage(response);
                    $scope.clear();
                    //  modal_fadeOut_Company();
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


