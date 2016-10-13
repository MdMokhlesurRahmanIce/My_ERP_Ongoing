/**
 * CustomerSrvc.js
 */

app.service('crudService', function ($http, conversion) {

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
    this.getModelSampleNo = function (apiRoute, companyID, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + companyID + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
    this.getModelByBuyer = function (apiRoute, buyerID, companyID, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + buyerID + '/' + companyID + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.getModelHDO = function (apiRoute, objcmnParam, headerToken) {
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

    this.postModels = function (apiRoute, modelFirst, modelSecond, headerToken) {
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

    //**********----Get Single Record----***************
    this.getModelByID = function (apiRoute, headerToken) {
        var request = $http({
            method: "get",
            url: apiRoute,
            headers: headerToken
        });
        return request;
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID, headerToken) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

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

    this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo, objcmnParam, headerToken) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "," + JSON.stringify(objcmnParam) + "]";

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

    this.getBankAdvisingByCompanyID = function (apiRoute, companyID, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: companyID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getModelByCompanyID = function (apiRoute, id, companyID, headerToken) {

        var strFinal = "[" + JSON.stringify(id) + "," + JSON.stringify(companyID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            //data: JSON.stringify({ id: id, companyID: companyID }),
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getBranchByBankID = function (apiRoute, bankID, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: bankID,
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

    this.getPIMasterByActivePIID = function (apiRoute, activePI, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getPIDetailsByActivePIID = function (apiRoute, activePI, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
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
    this.getLCMaster = function (apiRoute, objcmnParam, headerToken) {
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

    this.getPIMasterListByPIActive = function (apiRoute, objcmnParam, headerToken) {
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

    //**********----Create New Record----***************   
    this.post = function (apiRoute, model, headerToken) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: model,
            headers: headerToken
        });
        return request;
    }

    this.postNotification = function (apiRoute, Customer) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Customer
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

    this.postMultipleModel = function (apiRoute, model, headerToken) {
        debugger
        var strFinal = conversion.getDynamicModels(model);

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        //console.log(request);
        return request;
    }
});