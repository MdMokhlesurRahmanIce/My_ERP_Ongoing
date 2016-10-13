app.service('DyingPRDChemicalConsumptionService', function ($http) {
    var urlGet = '';

    //**********----Get All Record----***************

    this.getAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, headerToken) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.getConsumptionDetailsByID = function (apiRoute, companyID, loggedUser, ConsumptionID, headerToken) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + ConsumptionID;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
    //************ Get Details  By ID  *************************
    this.getDetailsByID = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, DetailsID, headerToken) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + DetailsID;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        return $http.get(apiRoute);
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
    //*********************** Get Master **************************************
    this.GetAllByMasterDetails = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging,MasterID, DetailsID) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + MasterID + '/' + DetailsID;
        return $http.get(urlGet);
    }


    //************** Post Master Details  ************

    this.putMasterCommon = function (apiRoute, modelMaster, modelDetails) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "]";
        var request = $http({
            method: "put",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    //************** Post Master Details  ************
    this.postCommonMasterDetails = function (apiRoute, modelMaster, modelDetails,commonEntity) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(commonEntity) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails,commonEntity=Common Model }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    //************** Post Master Details  ************
    this.postCommonMaster = function (apiRoute, modelMaster, commonEntity) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(commonEntity) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster,commonEntity=Common Model }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    //************** Post Master Details  ************
    



});