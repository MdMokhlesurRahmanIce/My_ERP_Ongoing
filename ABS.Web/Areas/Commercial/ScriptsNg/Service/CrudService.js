/**
 * CustomerSrvc.js
 */

app.service('crudService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    //this.getModelDc = function (apiRoute, objcmnParam) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}

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


    this.getModelDc = function (apiRoute, objcmnParam, headerToken) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
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

    //**********----Get Single Record----***************
    this.getModelByID = function (apiRoute, headerToken) {
        urlGet = apiRoute;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    //**********----Create New Record----***************
    //this.post = function (apiRoute, model) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: model
    //    });
    //    return request;
    //}


    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, objCmnParam, headerToken) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(objCmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        //console.log(request);
        return request;
    }


    //this.postMasterDetail = function (apiRoute, modelMaster, modelDetails) {
    //    // debugger
    //    var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    //console.log(request);
    //    return request;
    //}
    //**********----Update the Record----***************
    //this.put = function (apiRoute, Model) {
    //    var request = $http({
    //        method: "put",
    //        url: apiRoute,
    //        data: Model
    //    });
    //    return request;
    //}

    //**********----Delete the Record----***************
    //this.delete = function (apiRoute) {
    //    var request = $http({
    //        method: "delete",
    //        url: apiRoute
    //    });
    //    return request;
    //}
});