app.service('StockEntryService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';

    //**************--GetAll
    this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging) {

        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllItems = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getUserWiseCompany = function (apiRoute, LoginUserID, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: LoginUserID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getModelSampleNo = function (apiRoute, companyID, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + companyID + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.getItemMasterByGroup = function (apiRoute, objcmnParam, groupID, headerToken) {

        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(groupID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getAllItemGroup = function (apiRoute, page, pageSize, isPaging, ItemTypeID, CompanyId) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID + '/' + CompanyId;
        return $http.get(urlGet);
    }

    this.getAllUsers = function (apiRoute, page, pageSize, isPaging, UserTypeID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + UserTypeID;
        return $http.get(urlGet);
    }

    this.getAllUnits = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.GetLot = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllRawgg = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getStockItems = function (apiRoute, objcmnParam) {
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