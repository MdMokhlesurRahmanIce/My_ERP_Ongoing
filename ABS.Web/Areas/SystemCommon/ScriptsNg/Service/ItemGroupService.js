/**
 * ng_CrudService.js
 */

app.service('ItemGroupService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';
   

    this.getAll = function (apiRoute, page, pageSize, isPaging,CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }
    this.getItemGroupByID = function (apiRoute,GID,CompanyID) {

        urlGet = apiRoute + GID + '/' + CompanyID;
        return $http.get(urlGet);
    }
    this.checkedItemGroupCode = function (apiRoute, GroupCode) {

        urlGet = apiRoute + GroupCode;
        return $http.get(urlGet);
    }

    //**********----Get Single Record----***************
    this.getItemParentesById = function (apiRoute, page, pageSize, isPaging, TypeId, CompanyId) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + TypeId + '/' + CompanyId;
        return $http.get(urlGet);
    }

    this.GetList = function (apiRoute, cmnParam) {

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getAllItemGroups = function (apiRoute, objcmnParam) {
        debugger
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;

        //urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        //return $http.get(urlGet);
    }
    
    //this.getItemGroupByID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //}

    //**********----Create New Record----***************
    this.post = function (apiRoute, Model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, Model) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Model
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