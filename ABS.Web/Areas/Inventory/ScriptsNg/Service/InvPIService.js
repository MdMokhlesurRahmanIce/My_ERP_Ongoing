/**
 * InvPIService.js
 */

app.service('invPIService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getModelHDO = function (apiRoute, objcmnParam) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.postModels = function (apiRoute, modelFirst, modelSecond) {
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    //**********----Get Single Record----***************
    this.getModelByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.getBankAdvisingByCompanyID = function (apiRoute, companyID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: companyID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getModelByCompanyID = function (apiRoute, id, companyID) {

        var strFinal = "[" + JSON.stringify(id) + "," + JSON.stringify(companyID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            //data: JSON.stringify({ id: id, companyID: companyID }),
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getBranchByBankID = function (apiRoute, bankID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: bankID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }


    this.getItemMasterByGroup = function (apiRoute, objcmnParam, groupID) {

        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(groupID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getPIMasterByActivePIID = function (apiRoute, activePI) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getPIDetailsByActivePIID = function (apiRoute, activePI) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.getUserWiseCompany = function (apiRoute, LoginUserID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: LoginUserID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.getLoadSPRNO = function (apiRoute, objcmnParam) {
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
     
    this.getBankAdvising = function (apiRoute, companyID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: companyID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getBranchByBankID = function (apiRoute, bankID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: bankID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
      
    this.getInvPISuplier = function (apiRoute) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: '',
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }


    this.getPIMasterListByPIActive = function (apiRoute, objcmnParam) {
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