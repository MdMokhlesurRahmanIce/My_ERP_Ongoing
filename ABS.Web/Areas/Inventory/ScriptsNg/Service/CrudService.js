/**
 * CustomerSrvc.js
 */



app.service('crudService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    //**********----Get Single Record----***************
    this.getModelByID = function (apiRoute) {
        return $http.get(apiRoute);
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

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "]";

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

    
    this.getItemMasterByUnique = function (apiRoute, uniqueCode) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: uniqueCode,
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
    
    this.getPIMasterListByPIActive = function (apiRoute) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: '',
            dataType: "json",
            contentType: "application/json"
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