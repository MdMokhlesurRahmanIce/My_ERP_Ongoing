
app.service('WeavingGriageService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';
    this.GetList = function (apiRoute, page, pageSize, isPaging, CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }
    this.GetOperator = function (apiRoute, page, pageSize, isPaging, CompanyID,Type) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + Type;
        return $http.get(urlGet);
    }

    this.getMachine = function (apiRoute, page, pageSize, isPaging, CompanyID, ItemTypeID, ItemGroupID) {
        debugger
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + ItemTypeID + '/' + ItemGroupID;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getItemByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    this.getAllWeavingGraige = function (apiRoute, objcmnParam) {
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

    this.getAllMachineInfoes = function (apiRoute, objcmnParam) {
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
    //**********----Create New Record----***************
    this.post = function (apiRoute, model) {

        var request = $http({
            method: "post",
            url: apiRoute,
            data: model
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, model) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: model
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