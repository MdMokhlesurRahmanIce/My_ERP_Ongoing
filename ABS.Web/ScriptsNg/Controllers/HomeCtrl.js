/**
* HomeCtrl.js
*/

app.controller('HomeCtrl', ['$scope', '$interval', 'dashboardService',
    function ($scope, $interval, dashboardService) {

        $scope.totalUser = '';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();


        //*************************************
        //----------Clock----------------------
        //*************************************
        $scope.format = 'MMM d, y h:mm:ss a';
        $scope.blood_1 = 100;
        $scope.blood_2 = 120;

        var stop;

        $scope.fight = function () {
            if (angular.isDefined(stop)) return;

            stop = $interval(function () {
                if ($scope.blood_1 > 0 && $scope.blood_2 > 0) {
                    $scope.blood_1 = $scope.blood_1 - 3;
                    $scope.blood_2 = $scope.blood_2 - 4;
                } else {
                    $scope.stopFight();
                }
            }, 100);
        };

        $scope.stopFight = function () {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
            }
        };

        $scope.resetFight = function () {
            $scope.blood_1 = 100;
            $scope.blood_2 = 120;
        };

        $scope.$on('$destroy', function () {
            $scope.stopFight();
        });

       
        //*************************************
        //----------total User----------------------
        //*************************************
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
        
        $scope.ShowModal=function()
        {
            debugger;
            $("#modalPasswordChange").fadeIn(200, function () { $('#modalPasswordChange').modal('show'); });
        }

    }])




