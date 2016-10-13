/**
 * DashboardCtrl.js
 */
app.controller('dashboardCtrl', ['$scope', 'dashboardService', '$rootScope', '$localStorage','$http',
function ($scope, dashboardService, $rootScope, $localStorage,$http) {
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


    //************************ Start Getting DC Data *****************************//
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;

        //$scope.totalNoOfDCDaily = '';
        //$scope.totalPIAmount = '';

        function loadIPDailyRecords(isPaging) {
            debugger
            var apiRoute = '/Commercial/api/DC/' + 'GetDCDailyData/';
            var listPIDataDaily = dashboardService.getModel(apiRoute, page, pageSize, isPaging);
            listPIDataDaily.then(function (response) {

                if (response.data.length > 0) {
                    $scope.totalNoOfDCDaily = response.data[0].NoOfDC;
                    $scope.totalDCAmountDaily = response.data[0].QuantityYds;
                }
                else {
                    $scope.totalNoOfDCDaily = 0;
                    $scope.totalDCAmountDaily = 0;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadIPDailyRecords(0);


        function loadIPDMonthlyRecords(isPaging) {
            debugger
            var apiRoute = '/Commercial/api/DC/' + 'GetDCMonthlyData/';
            var listPIDataMonthly = dashboardService.getModel(apiRoute, page, pageSize, isPaging);
            listPIDataMonthly.then(function (response) {

                if (response.data.length > 0) {
                    $scope.totalNoOfDCMonthly = response.data[0].NoOfDC;
                    $scope.totalDCAmountMonthly = response.data[0].QuantityYds;
                }
                else {
                    $scope.totalNoOfDCMonthly = 0;
                    $scope.totalDCAmountMonthly = 0;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadIPDMonthlyRecords(0);

    //************************ End Getting DC Data *****************************//

        $rootScope.IsDashboard = 0;
    }]);

