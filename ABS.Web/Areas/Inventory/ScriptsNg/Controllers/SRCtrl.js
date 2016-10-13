/**
* PICtrl.js
*/

//app.controller('RequisitionCtrl', function ($scope, RequisitionService, conversion) {

app.controller('SRCtrl', ['$scope', 'RequisitionService', '$filter', 'uiGridConstants', 'conversion', '$localStorage',
    function ($scope, RequisitionService, $filter, uiGridConstants, conversion, $localStorage) {

        //Rq
        $scope.gridOptionsRequisition = [];
        var objcmnParam = {};

        //SRItemPop
        $scope.gridOptionsSRItemPop = [];
        var objcmnParamSRItemPop = {};

        $scope.gridOptionslistItemMaster = [];
        $scope.gridOptionsHDO = [];

        var baseUrl = '/Inventory/api/SPR/';
        var isExisting = 0;
        var page = 1;
        var pageSize = 50;
        var isPaging = 0;
        var totalData = 0;
        var ItemTypeID = 1;
        var TransactionTypeId = 6
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
        var date = new Date();
        $scope.RequistionDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
        $scope.EstimateDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

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

        $scope.PageTitle = 'Store Requisition Creation';
        $scope.ListTitle = 'Requisition Records';
        $scope.ListTitleActivePIMasters = 'Requisition  Information (Masters)';
        $scope.ListTitleSampleNo = 'Item Details';
        $scope.ListTitlePIDeatails = 'Requisition Information (Details)';
        $scope.ListTitleRequisitioneatails = 'SR Item List';
        $scope.ListTitleRequisitionMaster = 'Requisition List';
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

        var LoginUserID = $scope.UserCommonEntity.loggedUserID;
        var LoginCompanyID = $scope.UserCommonEntity.loggedCompnyID;


        //**************************************
        // Show and Hide Order
        //**************************************

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
                $scope.btnSaveText = "Save";
                $scope.IsInvalid = true;               
                $scope.IsShow = false;
                $scope.IsShowD = false;
                $scope.IsHidden = false;
                $scope.IsListIcon = false;
                $scope.IsCreateIcon = true;
                loadRequisition_Records(0);

            }
        }


        function loadGradeName() {

            var apiRoute = baseUrl + '/GetGrade/';
            var listGrade = RequisitionService.GetAllRequisitionType(apiRoute, page, pageSize, isPaging);
            listGrade.then(function (response) {
                $scope.listGrade = response.data                
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        // -------Get All Batch //--------
        function GetAllSupplier(dataModel) {

            var apiRoute = baseUrl + '/GetAllSupplier/';
            var SupList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging, dataModel.ItemID, LoginCompanyID);
            SupList.then(function (response) {
                $scope.lstSupplierList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        // -------Get All Batch //--------
        function GetAllBatchNo(dataModel) {

            var apiRoute =  baseUrl +'/GetAllBatch/';
            var BatchList = RequisitionService.getAllBatch(apiRoute, page, pageSize, isPaging, dataModel.ItemID, LoginCompanyID);
            BatchList.then(function (response) {
                $scope.lstBatchList = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //GetAllBatchNo();

        //**********----Get All Lot Record----***************
        function loadLotRecords(dataModel) {
            var apiRoute = baseUrl + '/GetLotNo/';
            var lisLotNo = RequisitionService.getAllLotNo(apiRoute, page, pageSize, isPaging, dataModel.ItemID, LoginCompanyID);
            lisLotNo.then(function (response) {
                $scope.listLot = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        //loadLotRecords(0);
       
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


        $scope.ListItemDetails = [];
        $scope.GetItemDetailsByItemID = function (dataModel) {
            var ItemId = dataModel.ItemID;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: 6
            };

            if (ItemId != "") {
                var apiRoute = baseUrl + 'GetItmDetailByItemId/';
                var listItemSerch = RequisitionService.GetItmDetailByItmCode(apiRoute, objcmnParam, ItemId, $scope.HeaderToken.get);
                listItemSerch.then(function (response) {
                    $scope.ListItemDetails = response.data;
                    $scope.IsAddDisable = false;
                    $scope.IsAddDisable = true;
                    $scope.getItmDetailsByItemId(dataModel);
                   // GetAllSupplier(dataModel);
                    //loadLotRecords(dataModel);
                    //GetAllBatchNo(dataModel);
                    //loadGradeName();
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
            //else if (ItemCode.Trim == "") {

            //        Command: toastr["warning"]("Please Enter Item Code.");
            //}

        }

        //*****************************************End Delete*******************************************
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
          //  GetAllSupplier(dataModel);
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
        }

        //**********----Get Item Detail by Item Code ----***************// 
        $scope.ListItemDetailsForSearch = [];
        $scope.getItmDetailsByItmCode = function () {

            var ItemCode = $scope.ItemCode;
            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: LoginUserID,
                loggedCompany: LoginCompanyID,
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
                    LotID: $scope.lstLot,                
                    LotNo: $("#ddlLotNo").select2('data').text == "--Select Lot No--" ? "" : $("#ddlLotNo").select2('data').text,
                    BatchID: $scope.Batch,               
                    BatchNo: $("#ddlBatch").select2('data').text == "--Select Batch--" ? "": $("#ddlBatch").select2('data').text,                  
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

        $scope.itemGroupes = [];
        $scope.GetItemGroups = function () {

            var apiRoute = baseUrl + '/GetAllItemGroup/';
            var itemGroups = RequisitionService.getAll(apiRoute, page, pageSize, isPaging, $scope.HeaderToken.get);
            itemGroups.then(function (response) {
                $scope.itemGroupes = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetItemGroups(0);


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
            RequisitionId = dataModel.RequisitionID;
            $scope.RequisitionId = dataModel.RequisitionID;
            $scope.requisitionID = dataModel.RequisitionID;
            var apiRoute = baseUrl + 'GetRequisitonMasterByRequisitionID/';
            var ListRequisitionInfoMaster = RequisitionService.getByRequisitionID(apiRoute, RequisitionId, LoginCompanyID, $scope.HeaderToken.get);
            ListRequisitionInfoMaster.then(function (response) {
                $scope.CustomrequisitionNo = response.data.RequisitionNo;
                $scope.RequisitionBy = response.data.RequisitionBy;
                $scope.RequistionDate = conversion.getDateToString(response.data.RequisitionDate);
                $scope.EstimateDate = conversion.getDateToString(response.data.EstDate);
                $("#ddlRequisitionBy").select2("data", { id: response.data.RequisitionBy, text: response.data.RequisitionByName });              
                $("#ddlDepartiment").select2("data", { id: response.data.ToDepartmentID, text: response.data.ToDepartmentName });
                $scope.Department = response.data.ToDepartmentID;
                $scope.Purpose = response.data.Purpose;
                $scope.Remarks = response.data.Remarks;
                $scope.IsShow = true;
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
            RequisitionId = dataModel.RequisitionID;
            var apiRoute = baseUrl + 'GetRequisitonDetailByRequisitionID/';
            var ListRequisitionInfoMaster = RequisitionService.getByRequisitionID(apiRoute, RequisitionId,LoginCompanyID, $scope.HeaderToken.get);
            ListRequisitionInfoMaster.then(function (response) {
                $scope.ListRequisitionDetailstemp = response.data;
                angular.forEach($scope.ListRequisitionDetailstemp, function (item) {
                    $scope.ListRequisitionDetails.push({
                        RequisitionID: item.RequisitionID, RequisitionDetailID: item.RequisitionDetailID,StockTransitID:item.StockTransitID, ArticleNo: item.ArticleNo, ItemName: item.ItemName,
                        UnitName: item.UnitName, ItemID: item.ItemID, UnitID: item.UnitID, Qty: item.Qty, MrrNo: item.MrrNo, UnitPrice: item.UnitPrice,                      
                        LotID: item.LotID,
                        BatchID: item.BatchID,
                        CurrentStock: item.CurrentStock, 
                        ModelState: 'Update'
                    })
                })
                $scope.IsShowD = $scope.ListRequisitionDetailstemp.length > 0 ? true : false;
                $scope.IsInvalid = $scope.ListRequisitionDetails.length > 0 ? false : true;
            },
            function (error) {
                console.log("Error: " + error);
                $scope.IsShow = true;
            });

        }


        //**********----Get Company Record and filter by CompanyID and cascading with Department ----***************//   
        //$scope.LoadDepartmentByCompanyID =
        function loadRecords_Departement(isPaging) {
            $scope.ListDepartment = [];
            var CompanyID = $localStorage.loggedCompnyID;
            if (CompanyID != "") {
                $("#ddlParentDept").select2("data", { id: 0, text: '--Select Department--' });
                var apiRoute = baseUrl + 'GetDepartmentByCompanyID/';
                var listDepartment = RequisitionService.getCompanyByDeptID(apiRoute, page, pageSize, isPaging, CompanyID, $scope.HeaderToken.get);
                listDepartment.then(function (response) {
                    $scope.ListDepartment = response.data
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }
        loadRecords_Departement(0);

        var ListCompany = [];
        function loadRecords_Company(isPaging) {

            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
            var listCompany = RequisitionService.getCompany(apiRoute, page, pageSize, isPaging);
            listCompany.then(function (response) {
                $scope.ListCompany = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Company(0);

       

        // -------Get All Users //--------
        $scope.UserLst = [];
        function GetAllUsers() {
            var apiRoute = '/Inventory/api/SPR/GetAllUsers/';
            var UserLst = RequisitionService.getAllUsers(apiRoute, page, pageSize, isPaging, UserTypeID, LoginCompanyID, $scope.HeaderToken.get);
            UserLst.then(function (response) {
                $scope.UserLst = response.data
                angular.forEach($scope.UserLst, function (item) {
                    if (item.UserID == LoginUserID) {
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
                        Qty: $scope.Qty,
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
                    Qty: $scope.Qty,
                    UnitPrice: $scope.UnitPrice,
                    Amount: parseFloat($scope.Qty * $scope.UnitPrice)
                };
                $scope.EmptyRequisitionDetail();
            }

            $scope.showDtgrid = $scope.ListRequisitionDetails.length;
        }


        //////**********----Day difference between two dates----***************


        $scope.GetDays = function () {

            var firstdate = $scope.RequistionDate;
            var seconddate = $scope.EstimateDate;

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
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                loadRequisition_Records(1);
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
                loggeduser: LoginUserID,
                loggedCompany: LoginCompanyID,
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
                    { name: "RequisitionNo", title: "Requisition No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionDate", title: "Requisition Date", cellFilter: 'date:"dd-MM-yyyy"', width: '10%',  headerCellClass: $scope.highlightFilteredHeader },
                    { name: "RequisitionByName", title: "Requisition By", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OrganogramName", title: "Department ", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Remarks", title: "Remarks", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "Purpose", title: "Purpose", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '7%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-success label-mini">' +
                                 '<a href="" title="Edit" ng-click="grid.appScope.GetRequisitonMasterByReqID(row.entity);grid.appScope.GetReqDetailByRequisitionID(row.entity)">' +
                                   '<i class="icon-edit"></i> Edit' +
                                 '</a>' +
                                 '</span>'

                        //cellTemplate: '<span class="label label-warning label-mini">' +
                        //                    '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                        //                        '<i class="icon-pencil" ng-click="grid.appScope.GetRequisitonMasterByReqID(row.entity);grid.appScope.GetReqDetailByRequisitionID(row.entity)"></i>' +
                        //                    '</a>' +
                        //                '</span>'
                                        // + '<span class="label label-danger label-mini">' +
                                        //    '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                        //        '<i class="icon-trash"></i>' +
                                        //    '</a>' +
                                        //'</span>'
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

            var apiRoute = baseUrl + 'GetRequisitionMaster/';
            var listReqMaster = RequisitionService.GetRequisitionMst(apiRoute, objcmnParam, TransactionTypeId,$scope.HeaderToken.get);
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
                return Math.ceil(this.totalItemsPop / this.pageSize);
            },
            pageSizeChangeItemPop: function () {
                if (this.ddlpageSizeItemPop == "All") {
                    this.pageNumber = 1
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
                if (this.pageNumber < this.getTotalPages()) {
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
                    this.pageNumber = this.getTotalPages();
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
                    { name: "ArticleNo", displayName: "Article No", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
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
                                     '<a href="" title="Add" ng-click="grid.appScope.GetItemDetailsByItemID(row.entity);">' +
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

            if ($('#ddlDepartiment').val() == '')
            {
                Command: toastr["warning"]("Please select SR dept!!!!");
                return;
            }
            if ($('#txtPurpose').val() == "") {
                Command: toastr["warning"]("Please enter purpose!!!!");
                $('#txtPurpose').focus()
                return;
            }
            var RequisitinDate = conversion.getStringToDate($scope.RequistionDate);
            var EstimateDate = conversion.getStringToDate($scope.EstimateDate);

            var RequisitionMaster = {
                RequisitionID: $scope.requisitionID,
                RequisitionDate: RequisitinDate,
                RequisitionTypeID: 6,                  // Transaction Type 8 for SPR
                RequisitionBy: $scope.RequisitionBy,
                Remarks: $scope.Remarks,
                Description: $scope.Description,
                Purpose: $scope.Purpose,             
                ToDepartmentID: $scope.Department,
                EstDate: EstimateDate,
                StatusID: 1,
                StatusBy: 1,
                StatusDate: new Date(),
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                DepartmentID: $scope.UserCommonEntity.loggedUserDepartmentID,
                IsComplete: false,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                CreateOn: new Date(),
                IsDeleted: false
            };
            var chkAmount = 1;
            var ChkStock = 1;
            angular.forEach($scope.ListRequisitionDetails, function (item) {
                if ((item.Qty <= 0) || typeof (item.Qty) === "undefined") {
                    chkAmount = 0;
                }
                if (item.Qty > item.CurrentStock)
                {
                    ChkStock = 0;
                }
            });
            
            

            var RequisitionDetail = $scope.ListRequisitionDetails;
            var UserCommonEntity = $scope.UserCommonEntity;
            //var menuID = $scope.MenuID;
            if (chkAmount == 1) {
                if (ChkStock == 1)
                    {
                var apiRoute = baseUrl + 'SaveSRequisitionMasterDetails/';
                var RequisitionMasterDetailsCreate = RequisitionService.postMasterDetail(apiRoute, RequisitionMaster, RequisitionDetail, UserCommonEntity, $scope.HeaderToken.get);
                RequisitionMasterDetailsCreate.then(function (response) {
                    var result = 0;

                    //if (response.data != "") {
                    //    $scope.CustomrequisitionNo = response.data;
                    //    Command: toastr["success"]("Save  Successfully!!!!");
                    //}

                    if (response.data != "") {
                        if (response.data == "Update") {
                            $scope.clear();
                            Command: toastr["success"]("Update  Successfully!!!!");
                        }
                        else if (response.data == "-1") {
                                Command: toastr["warning"]("Save not Successfully done!!!!");
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
                else if(ChkStock == 0)
                {
                    Command: toastr["warning"]("SR Quantity Can not be greater than Stock Quantity !!!!");
                }
            }
            else if (chkAmount == 0) {
                    Command: toastr["warning"]("SR Quantity Must Not Zero Or Empty !!!!");

            }
        };

        //////**********----Reset Record----***************
        $scope.clear = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsInvalid = true;         
            $scope.IsAddDisable = true;
            $scope.IsShowD = false;
            $scope.IsHiddenDetail = true;
            $scope.requisitionID = "0";
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.ListRequisitionDetails = [];
            $scope.RequisitionMaster = [];
            $scope.Description = "";
            $scope.Remarks = "";
            $scope.Purpose = "";
            $scope.ItemCode = "";
            $scope.IsInvalid = true;
            $("#ddlDepartiment").select2("data", { id: 0, text: '--Select Department--' });
        };

    }]);



