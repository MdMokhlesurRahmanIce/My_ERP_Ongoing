/**
 * DashboardCtrl.js
 */
app.controller('dashboardCtrl', ['$scope', 'dashboardService', '$rootScope', '$localStorage', '$http',
function ($scope, dashboardService, $rootScope, $localStorage, $http) {
    $scope.totalUser = '';
    var LoggedUserID = $('#hUserID').val();
    var LoggedCompanyID = $('#hCompanyID').val();

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


    //************************ Start Getting PI Data *****************************//
    var page = 0;
    var pageSize = 100;
    var isPaging = 0;

    //$scope.totalNoOfPI = '';
    //$scope.totalPIAmount = '';

    function loadIPDailyRecords(isPaging) {
        debugger
        var apiRoute = '/Sales/api/PI/' + 'GetPIDailyData/';
        var listPIDataDaily = dashboardService.getModel(apiRoute, page, pageSize, isPaging);
        listPIDataDaily.then(function (response) {

            if (response.data.length > 0) {
                $scope.totalNoOfPIDaily = response.data[0].NoOfPI;
                $scope.totalPIAmountDaily = response.data[0].Amount;
            }
            else {
                $scope.totalNoOfPIDaily = 0;
                $scope.totalPIAmountDaily = 0;
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadIPDailyRecords(0);


    function loadIPDMonthlyRecords(isPaging) {
        debugger
        var apiRoute = '/Sales/api/PI/' + 'GetPIMonthlyData/';
        var listPIDataMonthly = dashboardService.getModel(apiRoute, page, pageSize, isPaging);
        listPIDataMonthly.then(function (response) {

            if (response.data.length > 0) {
                $scope.totalNoOfPIMonthly = response.data[0].NoOfPI;
                $scope.totalPIAmountMonthly = response.data[0].Amount;
            }
            else {
                $scope.totalNoOfPIMonthly = 0;
                $scope.totalPIAmountMonthly = 0;
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadIPDMonthlyRecords(0);

    //************************ End Getting PI Data *****************************//

    $rootScope.IsDashboard = 0;
}]);

