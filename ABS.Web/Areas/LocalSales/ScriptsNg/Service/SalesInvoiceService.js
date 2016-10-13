app.service('SalesInvoice', function ($http,conversion, $q) {

    this.getUser = function (apiRoute, page, pageSize, isPaging, ComapnyID, UserTypeID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID + '/' + UserTypeID;
        return $http.get(urlGet);
    }
    this.getAll = function (apiRoute, page, pageSize, isPaging, ComapnyID, ItemType) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID + '/' + ItemType;
        return $http.get(urlGet);
    }

    this.getArticleDetails = function (apiRoute, page, pageSize, isPaging, ComapnyID, ItemID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID + '/' + ItemID;
        return $http.get(urlGet);
    }

    this.getGrades = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
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
    this.GetList = function (apiRoute, cmnParam, headerToken) {

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json",
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
    this.getItemMasterByTypeID = function (apiRoute, objcmnParam, TypeID, headerToken) {

        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(TypeID) + "]";

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

    this.postMultipleModel = function (apiRoute, model, headerToken) {
        debugger
        var strFinal = conversion.getDynamicModels(model);
        var RequestType = strFinal.Token.post.AuthorizedToken == headerToken.AuthorizedToken || strFinal.Token.put.AuthorizedToken == headerToken.AuthorizedToken ? $.ajax : $http;
        var request = RequestType({
            method: "post",
            url: apiRoute,
            data: strFinal.model,
            async: false,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getSalesDetails = function (apiRoute, objcmnParam) {
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
    
});