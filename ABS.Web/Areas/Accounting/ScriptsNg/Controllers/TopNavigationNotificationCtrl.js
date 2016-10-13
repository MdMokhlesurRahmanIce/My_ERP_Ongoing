app.filter('timeAgo', ['$interval', function ($interval) {
    // trigger digest every 60 seconds
    $interval(function () { }, 60000);

    function fromNowFilter(time) {
        return moment(time).fromNow();
    }

    fromNowFilter.$stateful = true;
    return fromNowFilter;
}]);
app.controller('TopNavigationNotificationCtrl', ['$scope', '$localStorage', 'sideNavService', function ($scope, $localStorage, sideNavService) {
     
    $scope.topNavigationNotificationList = [];
    $scope.sessionUserCompanyID = $('#hCompanyID').val();
    $scope.sessionLoggedUserID = $('#hUserID').val();
    //**** GET NOtificationes This Event Fire for signalr client BroadCust ****************
    function loadRecords_Notificationes(dataModuleID) {
        var ModuleID = 1;
        var companyID = 1;
        var loggedUser = 1;
        var userID = $scope.sessionLoggedUserID;
        var apiRoute = '/SystemCommon/api/SystemCommonLayout/GetNotificationInfo/';
        var processMenues = sideNavService.GetSideMenu(apiRoute, companyID, loggedUser, userID);
        processMenues.then(function (response) {
            var currentLength = $scope.topNavigationNotificationList.length;
            $scope.topNavigationNotificationList = response.data;
            var Length = $scope.topNavigationNotificationList.length;
            if (Length > currentLength) {
                ShowCustomToastrMessageResult(500);
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    //This Function is Invoked When Signalr hub Initialized;
    function loadRecords_Notification(dataModuleID) {
        
        var ModuleID = 1;
        var companyID = 1;
        var loggedUser = 1;
        var userID = $scope.sessionLoggedUserID;

        var apiRoute = '/SystemCommon/api/SystemCommonLayout/GetNotificationInfo/';
        var processMenues = sideNavService.GetSideMenu(apiRoute, companyID, loggedUser, userID);
        processMenues.then(function (response) {
            
            $scope.topNavigationNotificationList = response.data;

        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    $scope.targetNotifiedID = function (model, MasterID) {
        $localStorage.notificationStorageModel = model;
        $localStorage.notificationStorageMenuID = model.MenuID;
        $localStorage.currentMenuID = model.MenuID;
        $localStorage.loggedCompnyID = $('#hUserID').val();
        $localStorage.loggedUserID = $('#hCompanyID').val();
        $localStorage.notificationStorageMasterID = model.TransactionID;
        $localStorage.notificationStorageIsApproved = true;
        $localStorage.notificationStorageIsDeclained = true;
    };
    //*********************************** Signalr Code ************************
    // Reference the hub.
    var model = $scope.topNavigationNotificationList;
    var hubNotif = $.connection.notificationHubs;

    $.connection.hub.start().done(function () {
        loadRecords_Notification(0);
    });

    hubNotif.client.updatedData = function () {
        loadRecords_Notificationes(0);
    };

    hubNotif.server.broadcastData = function (model) {
        loadRecords_Notification(0);
    };

}]);