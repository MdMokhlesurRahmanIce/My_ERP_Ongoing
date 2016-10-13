/**
 * UserSrvc.js
 */

app.service('userService', function ($http) {

    //**********----Create New Record----***************
    this.post = function (apiRoute, model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: model
        });
        return request;
    }
});