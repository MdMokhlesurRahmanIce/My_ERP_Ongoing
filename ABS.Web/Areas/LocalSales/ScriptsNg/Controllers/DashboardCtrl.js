/**
 * DashboardCtrl.js
 */
app.controller('dashboardCtrl', ['$scope', 'dashboardService', '$rootScope', '$localStorage','$http',
function ($scope, dashboardService, $rootScope, $localStorage,$http) {
        $scope.totalUser = '';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

    //PageLoad
        function loadUserCommonEntity(num) {
            var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = pagedata;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);

        function loadRecords_usertotal() {
            var apiRoute = '/SystemCommon/api/Dashboard/GetUserTotal/' + LoggedCompanyID;
            var totalUser = dashboardService.getTotal(apiRoute);
            totalUser.then(function (response) {
                $scope.totalUser = response.data.totalUser;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_usertotal();
        //*****************Common **************************
        $rootScope.IsDashboard = 0;
    }]);

