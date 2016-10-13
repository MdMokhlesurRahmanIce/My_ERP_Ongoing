/**
*
*/

//app.controller('RequisitionCtrl', function ($scope, IssueService, conversion) {

app.controller('IssueCtrl', ['$scope', 'IssueService', '$filter', 'uiGridConstants', 'conversion','$localStorage',
    function ($scope, IssueService, $filter, uiGridConstants, conversion, $localStorage) {



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

    $scope.gridOptionsRequisition = [];
    var objcmnParam = {};

    var baseUrl = '/Inventory/api/Issue/';
    var isExisting = 0;
    var page = 1;
    var pageSize = 50;
    var isPaging = 0;
    var totalData = 0;
    var ItemTypeID = 1;
    var IsValidQty = false;
    $scope.ShowItemModal = false;
    var date = new Date();
    $scope.IsListIcon = true;
    $scope.IsCreateIcon = false;
    $scope.IsInvalid = true;
    var varGroupID = "";
    $scope.IssueDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
    
    $scope.FullFormateDate = [];
    $scope.ListCompany = [];
    $scope.ListBuyer = [];
    $scope.bool = true;
    $scope.MenuID = $scope.UserCommonEntity.currentMenuID;
    var LoginUserID = $scope.UserCommonEntity.loggedUserID; 
    var LoginCompanyID = $scope.UserCommonEntity.loggedCompnyID;
  
   
        //*************---Conditionally show hide element---**********

    if ($scope.UserCommonEntity.currentTransactionTypeID == 13)
     {
        $scope.PageTitle = 'Issue';
        $scope.ListTitlePIDeatails = 'Issue Item (Details)';
        $scope.LabelIssueText = "Issue No:";
        $scope.LabelIssueby = "Issue By:";
        $scope.LabelIssuedate = "Issue Date:";
        $scope.LabelFromDept = "From Dept:";
        $scope.labelToDept = "To Dept:";
        $scope.divReqNo = false;
        $scope.divToCompany = false;
        $scope.divMRR = true;
        $scope.IsRequired = true;
        $scope.Hide = true;
        $scope.divChallan = true;
        $scope.divGRR = true;
        $scope.divRemarksSecondary = true;
        $scope.ShowItemModal = false;
     }

    if ($scope.UserCommonEntity.currentTransactionTypeID == 14)
     {
         $scope.PageTitle = 'Internal Transfer';
         $scope.ListTitlePIDeatails = 'Item Details';
         $scope.LabelIssueText = "Transfer No:";
         $scope.LabelIssueby = "Transfer By:";
         $scope.LabelIssuedate = "Trns Date:";
         $scope.LabelFromDept = "Transfer From:";
         $scope.labelToDept = "Transfer To:";
         $scope.divReqNo = true;
         $scope.divToCompany = true;
         $scope.divMRR = true;
         $scope.IsRequired = false;
         $scope.divChallan = true;
         $scope.divGRR = true;
         $scope.divRemarksSecondary = true;
         $scope.ShowItemModal = true;
     }

    if ($scope.UserCommonEntity.currentTransactionTypeID == 15)
     {
         $scope.PageTitle = 'Internal Return';
         $scope.ListTitlePIDeatails = 'Item Details';
         $scope.LabelIssueText = "Return No:";
         $scope.LabelIssueby = "Return By:";
         $scope.LabelIssuedate = "Return Date:";
         $scope.LabelFromDept = "Return From:";
         $scope.labelToDept = "Return To:";
         $scope.divReqNo = true;
         $scope.divToCompany = true;
         $scope.divMRR = false;
         $scope.IsRequired = false;
         $scope.IsHide = true;
         $scope.divChallan = true;
         $scope.divGRR = true;
         $scope.divRemarksSecondary = true;
         $scope.ShowItemModal = true;
     }

    if ($scope.UserCommonEntity.currentTransactionTypeID == 16)
     {
         $scope.PageTitle = 'MRR Return';
         $scope.ListTitlePIDeatails = 'Item Details';
         $scope.LabelIssueText = "Return No:";
         $scope.LabelIssueby = "Return By:";
         $scope.LabelIssuedate = "Return Date:";
         $scope.LabelFromDept = "Return From:";
         $scope.labelToDept = "Return To:";
         $scope.divReqNo = true;
         $scope.divToCompany = true;        
         $scope.IsRequired = false;
         $scope.IsHide = true;
         $scope.divChallan = false;
         $scope.divGRR = false;
         $scope.divMRR = true;
         $scope.divToDept = true;
         $scope.divFromDept = false;
         $scope.divRemarksSecondary = true;
         $scope.ShowItemModal = true;
     }

    if ($scope.UserCommonEntity.currentTransactionTypeID == 17) {
         $scope.PageTitle = 'Damage Goods Entry';
         $scope.ListTitlePIDeatails = 'Item Details';    
         $scope.LabelIssueText = "Damage No:";
         $scope.LabelIssueby = "Entry By:";
         $scope.LabelIssuedate = "Date:";
         $scope.LabelFromDept = "Damage From:";
         $scope.divReqNo = true;
         $scope.divToCompany = true;
         $scope.IsRequired = false;
         $scope.IsHide = true;      
         $scope.divToDept = true;
         $scope.divFromDept = false;
         $scope.divChallan = true;
         $scope.divGRR = true;
         $scope.divMRR = true;
         $scope.divRemarksSecondary = false;
         $scope.divRemarksPrimary = true;
         $scope.ShowItemModal = true;
     }
    $scope.IssueId = "0";
    $scope.btnSaveText = "Save";
    $scope.btnShowText = "Show List";
    $scope.ListTitle = 'Issue Records';
    $scope.ListTitleActivePIMasters = 'Issue  Information (Masters)';
    $scope.ListTitleSampleNo = 'Sample Info';
   // $scope.ListTitlePIDeatails = 'Issue Information (Details)';
    $scope.ListTitleRequisitioneatails = 'Issue Item List';
        // $scope.ListTitleRequisitionMaster = 'Requisition List';
    $scope.ListTitleQCMasters = 'Issue Information (Masters)';
    $scope.ListTitleSampleNo = 'Sample Info';
    $scope.ListTitleQCDeatails = 'Issue Information (Details)';
    $scope.btnModal = "Add";
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
    $scope.IsShowD = false;
    $scope.IsHiddenDetail = true;
    $scope.ShowHide = function () {
        $scope.IsHidden = $scope.IsHidden == true ? false : true;
        $scope.IsHiddenDetail = true;
        if ($scope.IsHidden == true) {
            $scope.btnShowText = "Show List";
            $scope.IsShowD = $scope.ListRequisitionDetails.length > 0 ? true : false;
            $scope.IsShow = true;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
          
        }
        else {
            $scope.btnShowText = "Create";
            $scope.btnSaveText = "Save";
            $scope.IsInvalid = true;
            $scope.IsShow = false;
            $scope.IsHidden = false;
            $scope.IsListIcon = false;
            $scope.IsCreateIcon = true;
            $scope.IsShowD = false;
            loadIssueMasterRecords(0);
        }
    }


    var ListCompany = [];
    function loadRecords_Company(isPaging) {
        
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = IssueService.getCompany(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data 
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);

    $scope.UserLst = [];
    function GetAllUsers() {
        var apiRoute = '/Inventory/api/SPR/GetAllUsers/';
        var UserLst = IssueService.getAllUsers(apiRoute, page, pageSize, isPaging, 1, LoginCompanyID, $scope.HeaderToken.get);
        UserLst.then(function (response) {
            $scope.UserLst = response.data
            angular.forEach($scope.UserLst, function (item) {
                if (item.UserID == LoginUserID) {
                    $scope.IssueBy = item.UserID;
                    $("#ddlIssueBy").select2("data", { id: item.UserID, text: item.UserFullName });
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
        var listOrganograms = IssueService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging);
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
        var listOrganogramsDept = IssueService.GetDrpOrganograms(apiRoute, 1, 0, page, pageSize, isPaging, $scope.HeaderToken.get);
        listOrganogramsDept.then(function (response) {           
            $scope.listOrganogramsDepartment = response.data            
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_OrganogramDept(0);   
    
    //**********----get Item  Record from datail info ----***************//
    $scope.ListRequisitionDetails = [];
    $scope.deleteRowDetail = function (index) {
        $scope.ListRequisitionDetails.splice(index, 1);
    };
    $scope.ListRequisitionDetails = [];
  
        //**********----Edit List ----***************//

    $scope.EditRequisitionItems = function (dataModel) {
        $scope.btnModal = "Edit";
        $scope.hfIndex = $scope.ListRequisitionDetails.indexOf(dataModel);
        angular.forEach($scope.ListRequisitionDetails, function (item, key) {
            if (item.ItemID == dataModel.ItemID) {
                $scope.RequisitionDetailID = item.RequisitionDetailID;
                $scope.RequisitionID = item.RequisitionID;
                $scope.lstItemGroup = item.GroupID;
                $("#ddlItemGroup").select2("data", { id: item.GroupID, text: item.GroupName });
                varGroupID = item.GroupID;
                $scope.LaodItemByGroupID(0);
                varGroupID = "";
                $scope.ddlItem = item.ItemID;
                $("#ddlItem").select2("data", { id: item.ItemID, text: item.ItemName });
                $scope.UnitID = item.UnitID;
                $("#ddlUnit").select2("data", { id: item.UnitID, text: item.UnitName });
                $scope.Qty = item.Qty;
                $("#ddlLotNo").select2("data", { id: item.LotID, text: item.LotNo });
                LotID: $scope.lstLot;
                $("#ddlBatch").select2("data", { id: item.BatchID, text: item.BatchNo });
                BatchID: $scope.Batch;
                $scope.UnitPrice = item.UnitPrice;
            }
        });
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

                $scope.ListRequisitionDetails.push({
                    RequisitionDetailID: $scope.RequisitionDetailID,
                    RequisitionID: $scope.RequisitionID,
                    GroupID: $scope.lstItemGroup,
                    GroupName: $("#ddlItemGroup").select2('data').text,
                    ItemID: $scope.ddlItem,
                    ItemName: $("#ddlItem").select2('data').text,
                    UnitID: $scope.UnitID,
                    UnitName: $("#ddlUnit").select2('data').text,
                    LotID: $scope.lstLot,
                    LotNo: $("#ddlLotNo").select2('data').text == "--Select Lot No--" ? "" : $("#ddlLotNo").select2('data').text,
                    BatchID: $scope.Batch,
                    BatchNo: $("#ddlBatch").select2('data').text == "--Select Batch--" ? "" : $("#ddlBatch").select2('data').text,
                    IssueQty: $scope.Qty,
                    UnitPrice: $scope.UnitPrice,
                    Amount: parseFloat($scope.Qty * $scope.UnitPrice)
                });

                $scope.EmptyRequisitionDetail();
            }
            else if (duplicateItem === 1) {
                    Command: toastr["warning"]("Item Already Added!!!!");
            }
        }
        else if ($scope.ddlItem > 0 && $scope.btnModal == "Edit") {
            $scope.ListRequisitionDetails[$scope.hfIndex] = {
                RequisitionDetailID: $scope.RequisitionDetailID,
                RequisitionID: $scope.RequisitionID,
                GroupID: $scope.lstItemGroup,
                GroupName: $("#ddlItemGroup").select2('data').text,
                ItemID: $scope.ddlItem,
                ItemName: $("#ddlItem").select2('data').text,
                UnitID: $scope.UnitID,
                UnitName: $("#ddlUnit").select2('data').text,
                LotID: $scope.lstLot,
                LotNo: $("#ddlLotNo").select2('data').text == "--Select Lot No--" ? "" : $("#ddlLotNo").select2('data').text,
                BatchID: $scope.Batch,
                BatchNo: $("#ddlBatch").select2('data').text == "--Select Batch--" ? "" : $("#ddlBatch").select2('data').text,
                IssueQty: $scope.Qty,
                UnitPrice: $scope.UnitPrice,
                Amount: parseFloat($scope.Qty * $scope.UnitPrice)
            };
            $scope.EmptyRequisitionDetail();
        }

        $scope.showDtgrid = $scope.ListRequisitionDetails.length;
    }

    //////**********----delete  Record from ListRequisitionDetails----***************

        //**********----Get Unit Price Record and filter by ItemID ----***************// 

    $scope.LaodItemRateNUnit = function LoadItemPrice(isPaging) {
        var Url = '/Inventory/api/Requisition/';
        var ItemID = $scope.ddlItem;
        if (ItemID != "") {
            var apiRoute = Url + 'LaodItemRateNUnit/';
            var listItemPrice = IssueService.getItemGroup(apiRoute, page, pageSize, isPaging, ItemID);
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
        var Url = '/Inventory/api/Requisition/';
        var GroupID = varGroupID == "" ? $scope.lstItemGroup : varGroupID;
        if (GroupID != "") {
            var apiRoute = Url + 'GetItemListByGroupID/';
            var listItem = IssueService.getItemGroup(apiRoute, page, pageSize, isPaging, GroupID);
            listItem.then(function (response) {
                $scope.listItems = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }

        //////**********---- Get MRR No ----***************
    //$scope.getMRR = function () {
    //    var apiRoute = baseUrl + 'GetMRRList/';
    //    var listMRR = IssueService.getMRRlst(apiRoute, page, pageSize, isPaging);
    //    listMRR.then(function (response) {
    //        $scope.lstMRR = response.data;
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //$scope.getMRR(0);


        //////**********---- Get Requisition No ----***************
    var RequisitionTypeID = 6;
    $scope.getRequisition = function () {
        var apiRoute = baseUrl + 'GetRequisitionNo/';
        var listRequisition = IssueService.getReqisitionlst(apiRoute, page, pageSize, isPaging, $scope.UserCommonEntity.loggedCompnyID, RequisitionTypeID);
        listRequisition.then(function (response) {
            $scope.lstRequisition = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.getRequisition(0);


        //////**********---- Get Challan No ----***************
    //$scope.getChallan = function () {
    //    var apiRoute = baseUrl + 'GetChallanList/';
    //    var listChallanList = IssueService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
    //    listChallanList.then(function (response) {
    //        $scope.lstChallan = response.data;
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //$scope.getChallan(0);

        //////**********---- Get GRR No ----***************
    //$scope.getGRR = function () {
    //    var apiRoute = baseUrl + 'GetGRRList/';
    //    var listGRRList = IssueService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
    //    listGRRList.then(function (response) {
    //        $scope.lstGRR = response.data;
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //$scope.getGRR(0);


      //////**********----Get Requisition Item List----***************
   
    $scope.GetRequisitionItemList = function()
    {
        $scope.ListRequisitionItems = [];
        var ItemList = "";
        var apiRoute = baseUrl + 'GetRequisitionItemList/';
        var RequisitionID = $scope.RequistionNo;
        $scope.IsInvalid = true;
        $("#ddlParentDept").select2("data", { id: 0, text: '--Select Department--' });
        $scope.IsShowD = false;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 13) {
            if (RequisitionID != "") {             
                ItemList = IssueService.getItemDetailsByRequisitionID(apiRoute, page, pageSize, isPaging, RequisitionID, 0, $scope.HeaderToken.get);
                ItemList.then(function (response) {
                    $scope.ListRequisitionItems = response.data;
                    $scope.IsShowD = $scope.ListRequisitionItems.length > 0 ? true : false;
                    $scope.IsInvalid = $scope.ListRequisitionItems.length > 0 ? false : true;
                    $scope.Company = response.data[0].ToCompanyID;
                    $scope.ParentDept = response.data[0].ToDepartmentID;               
                    $("#ddlParentDept").select2("data", { id: response.data[0].ToDepartmentID, text: response.data[0].ToDepartmentName });
                    $scope.IsInvalid = false;

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        
        }
        if ($scope.UserCommonEntity.currentTransactionTypeID == 15)
            {
                var MrrID = $scope.MRR;
                if (MrrID != "") {
                    ItemList = IssueService.getItemDetailsByRequisitionID(apiRoute, page, pageSize, isPaging, 0, MrrID, $scope.HeaderToken.get);
                    ItemList.then(function (response) {
                        $scope.ListRequisitionItems = response.data;
                        $scope.ParentDept = response.data[0].DepartmentID;
                        $("#ddlParentDept").select2("data", { id: response.data[0].DepartmentID, text: response.data[0].FromDepartment });
                    },
                    function (error) {
                        console.log("Error: " + error);
                    });
                }
            }
        }
   
        //////**********----Get editable Item List----***************

    //$scope.GetRequisitionItemList = function () {
    //    $scope.ListRequisitionItems = [];
    //    var apiRoute = baseUrl + 'GetRequisitionItemList/';
    //    var RequisitionID = $scope.RequistionNo;
    //    if (RequisitionID != "") {
    //        var ItemList = IssueService.getItemDetailsByRequisitionID(apiRoute, page, pageSize, isPaging, RequisitionID);
    //        ItemList.then(function (response) {
    //            $scope.ListRequisitionItems = response.data;
    //            $scope.Company = response.data[0].ToCompanyID;
    //            $scope.ParentDept = response.data[0].ToDepartmentID;
    //            $("#ddlCompany").select2("data", { id: response.data[0].ToCompanyID, text: response.data[0].ToCompanyName });
    //            $("#ddlParentDept").select2("data", { id: response.data[0].ToDepartmentID, text: response.data[0].ToDepartmentName });
                
    //        },
    //        function (error) {
    //            console.log("Error: " + error);
    //        });
    //    }
    //}

     //////**********----Get Issue Amount----***************

    $scope.GetIssueAmount = function (dataModel) {
        $scope.ListRequisitionItems1 = [];  
        angular.forEach($scope.ListRequisitionItems, function (item)
        {
            if (item.PendingIssueQty < parseFloat(item.IssueQty))
            {
                Command: toastr["warning"]("Issue qty can't be greater than pending qty!!!!");
                IsValidQty = true;              
                $scope.ListRequisitionItems1.push({
                    IssueDetailID: item.IssueDetailID, IssueID: item.IssueID, ItemID: item.ItemID, UnitID: item.UnitID, ItemName: item.ItemName, UOMName: item.UOMName, LotID: item.LotID, BatchID: item.BatchID, RequisitionQty: item.RequisitionQty, IssuedQty: item.IssuedQty, PendingIssueQty: item.PendingIssueQty, UnitPrice: item.UnitPrice, IssueQty: 0.00, Amount: 0.00

                });
                $scope.ListRequisitionItems = $scope.ListRequisitionItems1;
            }
            else if (item.PendingIssueQty >= item.IssueQty)
                {
            var IssueAmt = parseFloat(item.IssueQty) * parseFloat(item.UnitPrice);
            $scope.ListRequisitionItems1.push({
                IssueDetailID: item.IssueDetailID, IssueID: item.IssueID, ItemID: item.ItemID, UnitID: item.UnitID, ItemName: item.ItemName, UOMName: item.UOMName, LotID: item.LotID, BatchID: item.BatchID, RequisitionQty: item.RequisitionQty, IssuedQty: item.IssuedQty, PendingIssueQty: item.PendingIssueQty, UnitPrice: item.UnitPrice, IssueQty: item.IssueQty, Amount: IssueAmt

            });
            $scope.ListRequisitionItems = $scope.ListRequisitionItems1;
           }
        });
       
    }
        //////**********----Delete New Record----***************
    $scope.deleteRow = function (index) {      
        $scope.ListRequisitionItems.splice(index, 1);
        $scope.showDtgrid = $scope.ListRequisitionItems.length;
        $scope.IsShowD = $scope.ListRequisitionItems.length > 0 ? true : false;
        $scope.IsInvalid = $scope.ListRequisitionItems.length > 0 ? false : true;       
    };
    //////**********----Create New Record----***************
    $scope.save = function () {

        var IssueDate = conversion.getDateToString($scope.IssueDate);

        var IssueMaster = {
            IssueID: $scope.IssueId,
            IssueTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
            IssueDate: IssueDate,
            IssueBy: $scope.IssueBy,
            ChallanID: $scope.Challan,
            GRRID: $scope.GRR,
            MRRID: $scope.MRR,
            RequisitionID: $scope.RequistionNo,
            RequisitionNo: $("#ddlRequisitionNo").select2('data').text,
            ToCompanyID: $scope.Company,
            ToDepartmentID: $scope.ParentDept,
            IsLoan: $scope.IsLoan,
            Comments: $scope.Purpose,
            StatusID: 1,
            StatusBy: $scope.StatusBy,
            StatusDate: new Date(),
            CompanyID: LoginCompanyID,
            DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
            CreateBy: LoginUserID,
            CreateOn: new Date(),
            IsDeleted: false
        };
        var IssueDetail = "";
        if ($scope.UserCommonEntity.currentTransactionTypeID == 13)
            IssueDetail = $scope.ListRequisitionItems;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 14)
            IssueDetail = $scope.ListRequisitionDetails;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 15)
            IssueDetail = $scope.ListRequisitionItems;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 16)
            IssueDetail = $scope.ListRequisitionDetails;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 17)
            IssueDetail = $scope.ListRequisitionDetails;


        var chkAmount = 1;
        if ($scope.UserCommonEntity.currentTransactionTypeID == 13 || $scope.UserCommonEntity.currentTransactionTypeID == 15) {
            angular.forEach($scope.ListRequisitionItems, function (item) {
                if ((item.IssueQty <= 0) || typeof (item.IssueQty) === "undefined") {
                    chkAmount = 0;
                }
            });
        }
        else {
            angular.forEach($scope.ListRequisitionDetails, function (item) {
                if ((item.IssueQty <= 0) || typeof (item.IssueQty) === "undefined") {
                    chkAmount = 0;
                }
            });
        }
        if (chkAmount == 1) {
            var menuID = $scope.MenuID;
            var apiRoute = baseUrl + 'SaveIssueMasterDetails/';
            var IssueMasterDetailsCreate = IssueService.postMasterDetail(apiRoute, IssueMaster, IssueDetail, menuID, $scope.HeaderToken.get);
            IssueMasterDetailsCreate.then(function (response) {
                if (response.data != "") {
                    if (response.data == 2) {
                        Command: toastr["success"]("Updated  Successfully!!!!");
                    }
                    else {
                        $scope.IssueNo = response.data;
                        Command: toastr["success"]("Save  Successfully!!!!");
                    }
                }
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }   
        else if (chkAmount == 0) {
            Command: toastr["warning"]("SPR Quantity Must Not Zero Or Empty !!!!");

        }
    }
     

        //Pagination Issue Master
    $scope.pagination = {
        paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
        ddlpageSize: 10, pageNumber: 0, pageSize: 10, totalItems: 0,

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
      
        $scope.IsInvalid = false;
        $scope.btnSaveText = "Update";
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.btnShowText = "Show List";
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
      
        var IssueId = '';
        IssueId = dataModel.IssueID;
        $scope.IssueId = dataModel.IssueID;

        var apiRoute = baseUrl + 'GetIssueMasterByIssueId/';
        var ListIssueInfoMaster = IssueService.getByIssueID(apiRoute, IssueId, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
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

    //    //**********----Get All Lot Record----***************
    //function loadLotRecords(isPaging) {
    //    var apiRoute = '/Inventory/api/StockEntry/GetLotNo/';
    //    var lisLotNo = IssueService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
    //    lisLotNo.then(function (response) {
    //        $scope.listLot = response.data
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //loadLotRecords(0);

        // -------Get All Batch //--------
    //function GetAllBatchNo() {

    //    var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetAllBatch/';
    //    var BatchList = IssueService.getReqisitionlst(apiRoute, page, pageSize, isPaging);
    //    BatchList.then(function (response) {
    //        $scope.lstBatchList = response.data
    //    },
    //    function (error) {
    //        console.log("Error: " + error);
    //    });
    //}
    //GetAllBatchNo();

        //-------------- Edit Issue Detail-----------------------------//

    $scope.GetIssueDetailByIssueId = function (dataModel) {
 
        var Issueid = '';
        Issueid = dataModel.IssueID;
        $scope.ListRequisitionItems = [];
        var apiRoute = baseUrl + 'GetIssueDetailByIssueId/';
        var ListIssueDetailInfoMaster = IssueService.getByIssueID(apiRoute, Issueid, $scope.UserCommonEntity.loggedCompnyID, $scope.HeaderToken.get);
        ListIssueDetailInfoMaster.then(function (response) {         
            $scope.ListRequisitionItems = response.data;
            $scope.IsShowD = true;
        },
        function (error) {
            $scope.console.log("Error: " + error);
            $scope.IsShow = true;
            $scope.IsShowD = false;
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
            tTypeId: 13
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
                { name: "IssueID", displayName: "Issue ID", visible:false,  title: "Issue ID", width: '5%', headerCellClass: $scope.highlightFilteredHeader },
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

        var apiRoute = baseUrl + 'GetIssueMasterList/'; 
        var listIssueMaster = IssueService.getIssueMasterList(apiRoute, objcmnParam, $scope.HeaderToken.get);
        listIssueMaster.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsIssueMaster.data = response.data.objVmIssue;
            $scope.loaderMoreIssueMaster = false;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    loadIssueMasterRecords(0);


    $scope.GetIssueDetailInfo = function(IssueId)
    {
        var apiRoute = baseUrl + 'GetIssueDetailInfo/';
        var listIssueDetail = IssueService.getIssueMasterList(apiRoute, IssueId, $scope.HeaderToken.get);
        listIssueDetail.then(function (response) {
        },
        function(error)
        {
            console.log("Error: " + error);
        });
    }

        ////**********----Reset Record----***************
    $scope.clear = function () {
        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IssueId = "0";
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsInvalid = true;
        $scope.btnSaveText = "Save";
        $scope.IsHiddenDetail = true;
        $scope.btnShowText = "Show List";            
        $scope.ListRequisitionDetails = [];
        $scope.bool = true;    
        $("#ddlRequisitionNo").select2("data", { id: 0, text: '--Select Req No--' });      
        $("#ddlIssueBy").select2("data", { id: 0, text: '--Select Person--' });
        $("#ddlParentDept").select2("data", { id: 0, text: '--Select Department--' });
        var date = new Date();
        $scope.IssueDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.Purpose = '';
        $scope.IsLoan = false;
        $scope.IsShowD = false;
        $scope.ListRequisitionItems = [];
    };

  

}]);



