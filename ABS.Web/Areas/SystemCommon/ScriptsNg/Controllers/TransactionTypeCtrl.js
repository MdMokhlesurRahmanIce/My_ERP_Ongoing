app.controller('transactionTypeController', function ($scope, TransactionTypeService) {
    
    debugger
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;
    var baseUrl = '/SystemCommon/api/TransactionType/';
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create Transaction Type';
    $scope.ListTitle = 'Transaction Type  Records';
    $scope.MenuID = 0;
    $scope.itemGroupes = {};
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.ItemGroupID = 0;
    var parent = "";

    function loadMenues(isPaging) {
        var apiRoute = baseUrl +' GetMenues/';
        var menues = TransactionTypeService.getAll(apiRoute, page, pageSize, isPaging, LoginCompanyID);
        itemTypes.then(function (response) {
            // 
            $scope.Menues = response.data
            //Set Default 
           // $("#ddlitemtype").select2("data", { id: $scope.ItemTypes[0].ItemTypeID, text: $scope.ItemTypes[0].ItemTypeName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadMenues(0);

   // --------------------------------------------------------------


    $scope.LoadParentesByItemType = function () {
        
        var ItemTypeID = $scope.ddlitemtype;
        var apiRoute = baseUrl+'GetItemParentes/';
        var itemGroupes = ItemGroupService.getItemParentesById(apiRoute, page, pageSize, isPaging, ItemTypeID);
        itemGroupes.then(function (response) {           
            $scope.itemGroupes = response.data;          
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.save = function () {
        
        var ItemGroup = {
            ItemGroupID:$scope.ItemGroupID,
            ItemGroupName: $scope.ModGroupName,
            ItemTypeID: $scope.ddlitemtype,
            ParentID: $scope.ddlItemGroup,
            IsActive: $scope.IsActive,
            CompanyID: LoginCompanyID,
            // when save CreateBy Equal CreateBy ang When Update CreateBy Equal UpdatedBy 
            CreateBy: LoginUserID,
            IsDeleted: false
        };

        isExisting = $scope.ItemGroupID;
        if (isExisting == 0) {
            
            var apiRoute = baseUrl + 'SaveItemGroup/';
            var SaveConsumption = ItemGroupService.post(apiRoute, ItemGroup);
            SaveConsumption.then(function (response) {
                debugger;
                ShowCustomToastrMessage(response);
                $scope.clear();
                LoadItemGroups(1);
            }, function (error) {
                console.log("Error: " + error);
            });
        }
        else
        {
            var apiRoute = baseUrl + '/UpdateItemGroup/';
            var CompanyUpdate = ItemGroupService.put(apiRoute, ItemGroup);
            CompanyUpdate.then(function (response) {
                response.data = -102;
                ShowCustomToastrMessage(response);
                $scope.clear();
                LoadItemGroups(1);
            },
            function (error) {
                console.log("Error: " + error);
            });
           

        }
       

    }

    function LoadItemGroups(isPaging) {
        debugger
        var apiRoute = baseUrl + 'GetAllItemGroups/';
        var itemGroup = ItemGroupService.getAllItemGroups(apiRoute, page, pageSize, isPaging, LoginCompanyID);
        itemGroup.then(function (response) {
            $scope.ItemGroups = response.data
        },
        function (error) {
            // debugger
            console.log("Error: " + error);
        });
    }

    LoadItemGroups(1);

    
    $scope.clear=function()
    {
        $scope.btnSaveUpdateText = "Save";
        LoadItemTypes(0);
        $scope.ddlitemtype = 0;
        $scope.LoadParentesByItemType();
       
        $("#ddlitemtype").select2("data", { id: 0, text: '--Select--' });
        $("#ddlItemParent").select2("data", { id: 0, text: '--Select--' });    
        $scope.ModGroupName = '';  
       $("#isActive").prop('checked', false); // Unchecks the box

    }

    $scope.delete = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.ItemGroupName + '. Are you sure?');
        if (IsConf) {

            var apiRoute = baseUrl + 'DeleteItemGroup/';
            var deleteConsumption = ItemGroupService.put(apiRoute, dataModel);
            deleteConsumption.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                $scope.clear();
                LoadItemGroups(1);
            }, function (error) {
                console.log("Error: " + error);
            });


        }

    }

    $scope.getItemGroupForEdit = function (dataModel)
    {
        
        var apiRoute = baseUrl + 'GetItemGroupById/' + dataModel.ItemGroupID;
        var singleitemGroup = ItemGroupService.getItemGroupByID(apiRoute);
        singleitemGroup.then(function (response) {            
            $scope.ItemGroupID = response.data.ItemGroupID
            $scope.ddlitemtype = response.data.Type
            $scope.ModGroupName = response.data.ItemGroupName
            $scope.ddlitemtype = response.data.TypeId
            parent = response.data.Parent
            $scope.LoadParentesByItemType();           
            $scope.btnSaveUpdateText = "Update";
            $("#ddlitemtype").select2("data", { id: 0, text: response.data.Type });
            
           
            if (parent != null) {
               
                $("#ddlItemParent").select2("data", { id: 0, text: parent });
            }
            else {
                $("#ddlItemParent").select2("data", { id: 0, text: "--Select--" });

            }
            if(response.data.IsActive=="Yes")
            {
                $("#isActive").prop('checked', true);
            }else
            {
                $("#isActive").prop('checked', false);
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
        
       
    }

    

    
   
});