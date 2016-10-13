/**
 * CrudService.js
 */

app.service('crudService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';

    this.getAll = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    }

    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        //return $http.get(apiRoute);
        var request = $http({
            method: "get",
            url: apiRoute
        });
        return request;
    }

    //**********----Create New Record----***************
    this.post = function (apiRoute, apiSaveModel) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: apiSaveModel
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, apiUpdateModel) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: apiUpdateModel
        });
        return request;
    }

    //**********----Delete the Record----***************
    this.delete = function (apiRoute) {
        var request = $http({
            method: "delete",
            url: apiRoute
        });
        return request;
    }
});