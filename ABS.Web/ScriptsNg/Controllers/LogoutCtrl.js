/**
 * LogoutCtrl.js
 */

app.controller('LogoutController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.LogOut = function () {
        debugger
        $http({
            method: 'POST',
            url: '/Account/LogOut',
        }).
        success(function (data, status, headers, config) {
            if (data.status === 1) {
                $window.location = '/';
            }
        }).
        error(function (data, status, headers, config) {

        });
    };
}])
app.config(function ($locationProvider) {
    $locationProvider.html5Mode(true);
});
