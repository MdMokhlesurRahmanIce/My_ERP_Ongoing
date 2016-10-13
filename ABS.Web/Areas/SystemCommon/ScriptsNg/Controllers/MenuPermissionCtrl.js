/**
Summery : 
1.Get DropDown Selected Value and Name on change DropDown List (add $filter if you use filter)
        see Section DropDown Change events
/**
 *  
 ***angular foreach *** Search EditCaller 
  //This Filter Has A problem it get frist item that match with input value (if id is 11 then it may return only 1 )
        // SeletedModuleName = $filter('filter')(moduleList, { ModuleID: ddlModule })[0].ModuleName; 

 */

app.controller('MenuPermissionCtrl', function ($scope, crudService, menuService, $filter) {
    //debugger
    var isExisting = 0;
    var page = 1;
    var pageSize = 1000;
    var isPaging = 0;
    var totalData = 0;
    var companyID = 1;
    var loggedUser = 0;
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();

    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create User Permission';
    $scope.ListTitle = 'Permission Records';
    $scope.UserPermissionID = 0;
    $scope.ListUserGroup = [];
    $scope.ListUser = [];
    $scope.ListOrganogram = [];
    $scope.ListModuleForPermission = [];
    $scope.ListCompany = [];
    $scope.ListStatus = [];
    $scope.ListPermission = [];
    $scope.ListCompanyDetailsGrid = [];
    //Create  Array For Filter Criteria
    $scope.Params = {
        ModuleID: 0,
        CompanyID: 0,
        UserGroupID: 0,
        UserID: 0,
        OrganogramID:0
    };
    var pModuleID = 0;
    var pUserGroupID = 0;
    var pUserID = 0;
    var pOrgannogramID = 0;

    $scope.EnableAllView = false;
    $scope.EnableAllInsert = false;
    $scope.EnableAllUpdate = false;
    $scope.EnableAllDelete = false;
    //**********----Get Company DropDown On Page Load----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = menuService.GetCompanies(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data  //Set Default 
            $("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);

    //**********----Get UserGroup DropDown On Page Load----***************
    function loadRecords_UserGroup(isPaging) {
        //debugger
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetUserGroup/';
        var processListUserGroup = crudService.getAllIncludingCompanyLog(apiRoute,companyID,loggedUser, page, pageSize, isPaging);
        processListUserGroup.then(function (response) {
           // //debugger
            $scope.ListUserGroup = response.data  //Set Default 
          //  $("#userGroupDropDown").select2("data", { id: $scope.ListUserGroup[0].UserGroupID, text: $scope.ListUserGroup[0].GroupName });
            $("#userGroupDropDown").select2("data", { id: 0, text: '--Select User Group--' });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_UserGroup(0);
    

    //**********----Get User DropDown On Page Load----***************
    function loadRecords_User(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetUser/';
        var processListUser = crudService.getAllIncludingCompanyLog(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
        processListUser.then(function (response) {
            $scope.ListUser = response.data;
           // $("#userDropDown").select2("data", { id: $scope.ListUser[0].UserID, text: $scope.ListUser[0].UserFullName });
            $("#userDropDown").select2("data", { id: 0, text: '--Select User--' });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_User(0);

    //**********----Get Organogram DropDown On Page Load----***************
    function loadRecords_Organogram(isPaging) {
        //debugger
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
        var processLisOrganogram = crudService.getAllIncludingCompanyLog(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
        processLisOrganogram.then(function (response) {
           // //debugger
            $scope.ListOrganogram = response.data;
            $scope.ddlOrganogramMaster = $scope.ListOrganogram[0].OrganogramID;
            //$("#organogramDropDown").select2("data", { id: $scope.ListOrganogram[0].OrganogramID, text: $scope.ListOrganogram[0].OrganogramName });
            $("#organogramDropDown").select2("data", { id: 0, text: '--Select Organogram--' });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Organogram(0);
    //**********----Get Module DropDown On Page Load----***************
    $scope.LoadModule = function loadRecords_Module(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetModuleWithPermission/';
        var listModuleProcess = menuService.GetModules(apiRoute, LoginCompanyID,LoginUserID, page, pageSize, isPaging);
        listModuleProcess.then(function (response) {
            //debugger
            $scope.ListModuleForPermission = response.data
            //$("#moduleDropDown").select2("data", { id: $scope.ListModuleForPermission[0].ModuleID, text: $scope.ListModuleForPermission[0].ModuleName });
            $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--' });
        },
        function (error) {
            console.log("Error: " + error);
        });
    };
    $scope.LoadModule(0);
    
    //**********----Get Status DropDown On Page Load----***************
    function loadRecords_Status(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetStatus/';
        var listStatus = menuService.GetStatus(apiRoute, page, pageSize, isPaging);
        listStatus.then(function (response) {
            $scope.ListStatus = response.data
            $("#statusDropDown").select2("data", { id: $scope.ListStatus[0].StatusID, text: $scope.ListStatus[0].StatusName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Status(0);
    //****************Generate Button click Event*******************
    $scope.Generate = function (ddlModule, moduleList) {
        //debugger
        var selectedID = ddlModule;
        var SeletedName = "";
        try {
            for (i = 0; i < moduleList.length; i++) {
                if (ddlModule == moduleList[i].ModuleID) {
                    SeletedName = moduleList[i].ModuleName;
                    break;
                }
            }
        } catch (e) {

        }
       
        if (selectedID >= 1) {
            // //debugger
            var bbd = $scope.Params;
            // //debugger
            var mod = $scope.Params.ModuleID;
            $scope.Params.ModuleID = selectedID;
        }
        $scope.BindPermissionGrid();
       
    };
    //**********----DropDown Change events---***************
    $scope.ModuleChanged = function (ddlModule, moduleList) {
        ////debugger
        var selectedID = ddlModule;
        var SeletedName = "";
        for (i = 0; i < moduleList.length; i++) {
            if(ddlModule==moduleList[i].ModuleID)
            {
                SeletedName = moduleList[i].ModuleName;
                break;
            }
        }
        if (selectedID >=1)
        {
           // //debugger
            var bbd = $scope.Params;
           // //debugger
            var mod = $scope.Params.ModuleID;
            $scope.Params.ModuleID = selectedID;
        }
        $scope.BindPermissionGrid();
        //This Filter Has A problem it get frist item that match with input value (if id is 11 then it may return only 1 )
       // SeletedModuleName = $filter('filter')(moduleList, { ModuleID: ddlModule })[0].ModuleName; 
    };
    $scope.SetInputModule = function (selectedItem) {
        ////debugger
       
        if (selectedItem == null) {
            selectedItem = 0;
        }
        var selectedID = selectedItem;
        $scope.Params.ModuleID = selectedID;
    };
    $scope.SetInputUserGroup = function (selectedItem) {
        ////debugger
        if (selectedItem == null) {
            selectedItem = 0;
        }
         
        var selectedID = selectedItem;
        $scope.Params.UserGroupID = selectedID;
        if (selectedItem != null) {
            $scope.Params.UserID = 0;
            //$("#userDropDown").select2("data", { id: $scope.ListUser[0].UserID, text: $scope.ListUser[0].UserFullName });
            $("#userDropDown").select2("data", { id: 0, text: '--Select User--' });
        }
       
    };
    $scope.SetInputUser = function (selectedItem) {
        ////debugger
        if (selectedItem == null) {
            selectedItem = 0;
        }
        var selectedID = selectedItem;
        $scope.Params.UserID = selectedID;
        if (selectedItem != null) {
            $scope.Params.UserGroupID = 0;
            //$("#userGroupDropDown").select2("data", { id: $scope.ListUserGroup[0].UserGroupID, text: $scope.ListUserGroup[0].GroupName });
            $("#userGroupDropDown").select2("data", { id: 0, text: '--Select User Group--' });
            
        }
    };
    $scope.SetInputOrganogram = function (selectedItem) {
        ////debugger
        if (selectedItem == null) {
            selectedItem = 0;
            return;
        }
        var selectedID = selectedItem;
        $scope.Params.OrganogramID = selectedID;
    };
    $scope.CompanyChange = function (ddlCompany) {
        ////debugger
        var selectedID = $scope.ddlCompany;
        var SeletedName = $filter('filter')($scope.ListCompany, { CompanyID: $scope.ddlCompany })[0].CompanyName;
        for (i = 0; i < $scope.ListCompany.length; i++) {
            if ($scope.ddlCompany == $scope.ListCompany[i].CompanyID) {
                SeletedName = $scope.ListCompany[i].CompanyName;
                break;
            }
        }
        $scope.BindPermissionGrid();
    };
    $scope.BindPermissionGrid = function () {
        pModuleID = $scope.Params.ModuleID || 0;
        pUserGroupID = $scope.Params.UserGroupID || 0;
        pUserID = $scope.Params.UserID || 0;
        pOrgannogramID = $scope.Params.OrganogramID || 0 ;
        companyID = $('#hCompanyID').val();
        pModule = $scope.Params.ModuleID || 0 ;
        //console.info("--0909---");
        //console.info($scope.Params);
        //return;

        var apiRoute = '/SystemCommon/api/MenuPermission/GetMenuPermissionByParam/';
        var listPermissionParam = crudService.getAllByParam(apiRoute, companyID, loggedUser, page, pageSize, isPaging, pModuleID, pUserGroupID, pUserID, pOrgannogramID);
        //var listPermissionParam = crudService.getAllByParam(apiRoute, companyID, loggedUser, page, pageSize, isPaging, 3, -1, -1, -1);
        listPermissionParam.then(function (response) {
            
            $scope.ListPermission = response.data
            SetAllGridCheckBox($scope.ListPermission);
        },
        function (error) {
            console.log("Error: " + error);
        });
    };
    $scope.EditCaller = function (dataModel) {
        //debugger
        angular.forEach(dataModel, function (value, key) {
            /* do something for all key: value pairs . Its Works Fine */
            //debugger
            value.ModuleID = $scope.Params.ModuleID;
            value.UserGroupID = $scope.Params.UserGroupID;
            value.UserID = $scope.Params.UserID;
            value.OrganogramID = $scope.Params.OrganogramID;
            value.StatusID = 1;
            value.companyID = $('#hCompanyID').val();
        });

        //Save Begin
        var apiRoute = '/SystemCommon/api/MenuPermission/SaveMenuPermission/';
        var MenuPermissionCreate = crudService.post(apiRoute,dataModel);
        MenuPermissionCreate.then(function (response) {
            ShowCustomToastrMessage(response);
        },
        function (error) {
            console.log("Error: " + error);
        });
        //Save End


    };
    // *******************Grid Check Box evets **********************
    function SetAllGridCheckBox(permission) {
        var InsertView = true;
        var UpdateView = true;
        var DeleteView = true;
        var OnlyView = true;
        angular.forEach($scope.ListPermission, function (value, key) {
            if (!value.EnableView) { OnlyView = false; }
            if (!value.EnableInsert) { InsertView = false; }
            if (!value.EnableUpdate) { UpdateView = false; }
            if (!value.EnableDelete) { DeleteView = false; }
        });
        if (OnlyView) { $scope.EnableAllView = true; $('#chkEnableAllView').find('span').addClass('checked'); } else { $scope.EnableAllView = false; $('#chkEnableAllView').find('span').removeClass('checked'); }
        if (InsertView) { $scope.EnableAllInsert = true; $('#chkEnableAllInsert').find('span').addClass('checked'); } else { $scope.EnableAllInsert = false; $('#chkEnableAllInsert').find('span').removeClass('checked'); }
        if (UpdateView) { $scope.EnableAllUpdate = true; $('#chkEnableAllUpdate').find('span').addClass('checked'); } else { $scope.EnableAllUpdate = false; $('#chkEnableAllUpdate').find('span').removeClass('checked'); }
        if (DeleteView) { $scope.EnableAllDelete = true; $('#chkEnableAllDelete').find('span').addClass('checked'); } else { $scope.EnableAllDelete = false; $('#chkEnableAllDelete').find('span').removeClass('checked'); }
    }

    //******************view CheckBox*********************************
    $scope.gridViewCheckBoxClicked = function (status) {
        debugger
        if (!status) {
            $scope.EnableAllView = false;
        }
        SetAllGridCheckBox(1);
    }
    $scope.EnableAllViewClicked = function (EnableAllView) {
        angular.forEach($scope.ListPermission, function (value, key) {
            if (EnableAllView) {
                value.EnableView = true;
                $scope.EnableAllView = true;
                $('#chkEnableAllView').find('span').addClass('checked');
            }
            else {
                value.EnableView = false;
                $scope.EnableAllView = false;
                $('#chkEnableAllView').find('span').removeClass('checked');
            }
        });
    }
    //******************Insert CheckBox*********************************
    $scope.gridCheckBoxClicked = function (status) {
        if (!status) {
            $scope.EnableAllInsert = false;
        }
        SetAllGridCheckBox(1);
    }
    $scope.EnableAllInsertClicked = function (EnableAllInsert) {
        angular.forEach($scope.ListPermission, function (value, key) {
            debugger
            if (EnableAllInsert) {
                value.EnableInsert = true;
                $scope.EnableAllInsert = true;
            }
            else {
                value.EnableInsert = false;
                $scope.EnableAllInsert = false;
            }
        });
    }
    //******************Update CheckBox*********************************
    $scope.gridUpdateCheckBoxClicked = function (status) {
        if (!status) {
            $scope.EnableAllUpdate = false;
        }
        SetAllGridCheckBox(1);
    }
    $scope.EnableAllUpdateClicked = function (EnableAllUpdate) {
        angular.forEach($scope.ListPermission, function (value, key) {
            if (EnableAllUpdate) {
                value.EnableUpdate = true;
                $scope.EnableAllUpdate = true;
            }
            else {
                value.EnableUpdate = false;
                $scope.EnableAllUpdate = false;
            }
        });
    }

    //******************Delete CheckBox*********************************
    $scope.gridDeleteCheckBoxClicked = function (status) {
        if (!status) {
            $scope.EnableAllDelete = false;
        }
        SetAllGridCheckBox(1);
    }
    $scope.EnableAllDeleteClicked = function (EnableAllDelete) {
        angular.forEach($scope.ListPermission, function (value, key) {
            if (EnableAllDelete) {
                value.EnableDelete = true;
                $scope.EnableAllDelete = true;
            } 
            else {
                value.EnableDelete = false;
                $scope.EnableAllDelete = false;
            }
        });
    }
    //***************************Roclick**************
    $scope.checkRowClick=function(number,status) {
        if (status)
        {
            $scope.ListPermission[number].EnableView = true;
            $scope.ListPermission[number].EnableInsert = true;
            $scope.ListPermission[number].EnableUpdate = true;
            $scope.ListPermission[number].EnableDelete = true;
        }
        else {
            $scope.ListPermission[number].EnableView = false;
            $scope.ListPermission[number].EnableInsert = false;
            $scope.ListPermission[number].EnableUpdate = false;
            $scope.ListPermission[number].EnableDelete = false;
        }
        SetAllGridCheckBox(1);
       
    }

    $scope.checkAllRowClick = function (status) {
        if (status) {
            $scope.checkRow = $scope.checkRowClick(1,status);
        }
        else
        {
            $scope.checkRow = $scope.checkRowClick(1, false);
        }
        SetAllGridCheckBox(1);
      
    }
    
    //**********----Reset Record----***************
    $scope.clear = function () {
        //debugger
        $scope.ListPermission = [];
        $("#userGroupDropDown").select2("data", { id: 0, text: '--Select User Group--' });
        $("#userDropDown").select2("data", { id: 0, text: '--Select User--' });
        $("#organogramDropDown").select2("data", { id: 0, text: '--Select Organogram--' });
        $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--' });
    };
});




