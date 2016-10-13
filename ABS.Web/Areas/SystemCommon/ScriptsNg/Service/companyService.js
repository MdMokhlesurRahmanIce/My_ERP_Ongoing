/**
 * ng_CrudService.js
 */

app.service('companyService', function ($http) {
    //**********----Get All Record----*************** 
    var urlGet = '';
    this.GetCompanies = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getToken = function (apiRoute, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.getDemo = function (apiRoute, headerToken) {
        var request = $http({
            method: "get",
            url: apiRoute,
            headers: headerToken
        });
        return request;
    }

    //**********----Get Single Record----***************
    this.getCompanyByID = function (apiRoute, headerToken) {
        var request = $http({
            method: "get",
            url: apiRoute,
            headers: headerToken
        });
        return request;
    }

    //**********----Create New Record----***************
    this.post = function (apiRoute, Customer) {

        var request = $http({
            method: "post",
            url: apiRoute,
            data: Customer
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, Customer) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Customer
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

    //************** Post Master Details  ************
    this.postModels = function (apiRoute, modelFirst, modelSecond, headerToken) {
        debugger
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    //this.postModels = function (apiRoute, modelFirst, modelSecond) {
    //    debugger
    //    var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}


});