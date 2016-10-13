

app.controller('sideNavCtrl', ['$scope', 'sideNavService','$localStorage','$rootScope',function ($scope, sideNavService,$localStorage,$rootScope) {
    var moduleID = $rootScope.TopNavigationBarModuleID; // It will come From Dynamic Top Navigation Bar Click Event
    //***************For Menu Permission ******************
     
    $scope.sessionUserCompanyID = 0;
    $scope.sessionLoggedUserID = 0;
    $scope.sessionUserCompanyID = $('#hCompanyID').val();
    $scope.sessionLoggedUserID = $('#hUserID').val();
    //*************** For Menu Permission *****************
    $scope.loaderWait = false;
    $scope.ListMenues = [];
    loadRecords_Menues(moduleID);
    $scope.moduleValue = function (dataModuleID) {
        loadRecords_Menues(dataModuleID);
    };
    //**********----Get Side Menus---***************
    function loadRecords_Menues(dataModuleID) {
        var ModuleID = dataModuleID;
        var companyID = $scope.sessionUserCompanyID;
        var loggedUser = $scope.sessionLoggedUserID;
        var apiRoute = '/SystemCommon/api/SystemCommonLayout/GetSideMenu/';
        $scope.loaderWait = true;
        $scope.LoadMessage = 'Loading Please wait...!';

        var processMenues = sideNavService.GetSideMenu(apiRoute, companyID, loggedUser, ModuleID);
        processMenues.then(function (response) {
            $scope.ListMenues = response.data;
            $scope.loaderWait = false;
        },
        function (error) {
            console.log("Error: " + error);
            $scope.LoadMessage = 'Loading Problem...!';
        });
    }

    //****************** BroadCust *************************
    $scope.menuStorage = function (menuID, menuList) {
        $localStorage.MenuID = menuID;
        $localStorage.ListMenues = menuList;
        //
        $localStorage.LoggedUserID=$scope.sessionLoggedUserID;
        //Code That Hide Approval and Delcline Button
        $localStorage.notificationStorageMenuID = 0;
        $localStorage.notificationStorageMasterID = 0;
        $localStorage.notificationStorageIsApproved = false;
        $localStorage.notificationStorageIsDeclained = false;
    }

    $scope.SideBarMenuClicked = function (childMenu, menuList, ChildMenues) {
        
        $localStorage.loggedCompnyID = $scope.sessionUserCompanyID
        $localStorage.loggedUserID = $scope.sessionLoggedUserID;
        $localStorage.loggedUserBranchID = 1; //It will Come from Logged User Branch
        $localStorage.loggedUserBranchID = childMenu.DepartmentID; //It will Come from Logged User Branch
        
        $localStorage.loggedUserDepartmentID = childMenu.DepartmentID; //It will Come from Logged User Department
        
        $localStorage.currentModuleID = moduleID; // It will come From Dynamic Top Navigation Bar Click Event
        $localStorage.currentMenuID = childMenu.MenuID;
        $localStorage.currentTransactionTypeID = childMenu.TransactionTypeID;
        $localStorage.MenuList = menuList;
        $localStorage.ChildMenues = ChildMenues;
        //
        
        //Code That Hide Approval and Delcline Button
        $localStorage.notificationStorageMenuID = 0;
        $localStorage.notificationStorageMasterID = 0;
        $localStorage.notificationStorageIsApproved = false;
        $localStorage.notificationStorageIsDeclained = false;

    }


}]);

