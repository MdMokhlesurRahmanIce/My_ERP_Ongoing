/**
 * QuotationSrvc.js
 */

app.service('quotationService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';

    this.GetList = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllUsers = function (apiRoute, page, pageSize, isPaging, UserTypeID, LoginCompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + UserTypeID + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getItemByID = function (apiRoute) {
        return $http.get(apiRoute);
    }
    this.getByQuotationID = function (apiRoute, QuotationId, CompanyID, headerToken) {
        urlGet = apiRoute + QuotationId + '/' + CompanyID;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
     
    }

    this.GetFrList = function (apiRoute, cmnParam, headerToken) {
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

    this.getItemDetailsByRequisitionID = function (apiRoute, RequisitionID, CompanyId) {
        urlGet = apiRoute + RequisitionID + '/' + CompanyId;
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    }

    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getQuotationMasterList = function (apiRoute, objcmnParam) {
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

    this.getDynamicGrid = function (apiRoute, objcmnParam) {
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

    this.GetloadSPRNo = function (apiRoute, objcmnParam) {
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

    this.getPostservice = function (apiRoute, objcmnParam, headerToken) {       
        var strFinal = "[" + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, 
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });       
        return request;
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID,headerToken) {
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

    //this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo) {
    //    // debugger
    //    var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "]";

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

    

    this.getDeptByCompanyID = function (apiRoute, objcmnParam, companyID) {

        var strFinal = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(companyID) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
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


    this.GetLocation = function (apiRoute, objcmnParam) {
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

    this.getUserWiseCompany = function (apiRoute, objcmnParam) {
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

    this.getLCMaster = function (apiRoute, objcmnParam) {
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

    this.getPIMasterListByPIActive = function (apiRoute, objcmnParam) {
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
    //**********----Create New Record----***************
    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.postDefectInfo = function (apiRoute, modelMaster) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelMaster) + "]";

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

    this.getModelDefectType = function (apiRoute, objcmnParam) {
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
    this.postList = function (apiRoute, modelDetails) {
        // debugger
        var strFinal = "[" + JSON.stringify(modelDetails) + "]";

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

    this.getModelOutputUnit = function (apiRoute, objcmnParam) {
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
});