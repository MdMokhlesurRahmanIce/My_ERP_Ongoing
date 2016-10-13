app.service('RowMaterialService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';


    this.getAllRawMaterial = function (apiRoute, objcmnParam) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    //**************--GetAll
    this.getAllItemGroup = function (apiRoute, page, pageSize, isPaging, ItemTypeID, CompanyId) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID + '/' + CompanyId;
        return $http.get(urlGet);
    }

    this.getAll = function (apiRoute, page, pageSize, isPaging,CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }

    this.getRawMaterialByID = function (apiRoute) {
        return $http.get(apiRoute);
    }
    this.getItemParentesById = function (apiRoute, page, pageSize, isPaging, TypeId, CompanyId) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + TypeId + '/' + CompanyId;
        return $http.get(urlGet);
    }


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

});