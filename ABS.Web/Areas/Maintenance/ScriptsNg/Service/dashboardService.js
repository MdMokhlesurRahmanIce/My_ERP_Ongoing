/**
 * dashboardService.js
 */

app.service('dashboardService', function ($http) {
    //**********----Get Record Total----***************
    this.getTotal= function (apiRoute) {
        return $http.get(apiRoute);
    }

    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

});