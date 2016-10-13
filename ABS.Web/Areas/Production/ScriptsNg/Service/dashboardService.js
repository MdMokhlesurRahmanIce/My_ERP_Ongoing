/**
 * dashboardService.js
 */

app.service('dashboardService', function ($http) {
    //**********----Get Record Total----***************
    this.getTotal= function (apiRoute) {
        return $http.get(apiRoute);
    }

});