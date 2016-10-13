/**
 * CustomerSrvc.js
 */

app.service('challanService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllItemGroup = function (apiRoute, page, pageSize, isPaging, ItemTypeID, CompanyId) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID + '/' + CompanyId;
        return $http.get(urlGet);
    }


    this.GetSPRNo = function (apiRoute, objcmnParam) {
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

    this.GetItmDetailByItmCode = function (apiRoute, objcmnParam, ItemCode)
    {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ItemCode) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;

    }

    this.GetPackingUnit = function (apiRoute, objcmnParam) {
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

    this.GetWeightUnit = function (apiRoute, objcmnParam) {
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


    this.GetItemDetailBySPRID = function (apiRoute, objcmnParam, sprID) {
        var param = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(sprID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: param,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }


    //this.postModels = function (apiRoute, modelFirst, modelSecond) {
    //    var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

    ////**********----Get Single Record----***************
    //this.getModelByID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //} 

    this.postMasterDetail = function (apiRoute, itemMaster, itemMasterDetail, menuID) { 
        var strFinal = "[" + JSON.stringify(itemMaster) + "," + JSON.stringify(itemMasterDetail) + "," + JSON.stringify(menuID) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, 
            dataType: "json",
            contentType: "application/json"
        }); 
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

    //this.getBankAdvisingByCompanyID = function (apiRoute, companyID) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: companyID,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

    //this.getModelByCompanyID = function (apiRoute, id, companyID) {

    //    var strFinal = "[" + JSON.stringify(id) + "," + JSON.stringify(companyID) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        //data: JSON.stringify({ id: id, companyID: companyID }),
    //        data: strFinal,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

    //this.getBranchByBankID = function (apiRoute, bankID) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: bankID,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}


    this.getItemMasterByGroup = function (apiRoute, objcmnParam) {

        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam)+ "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetChallanDetailByChallanID = function (apiRoute, objcmnParam, challanID) {

        var cmnParamWitChallanID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(challanID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitChallanID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    
    //this.getPIMasterByActivePIID = function (apiRoute, activePI) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: activePI,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

    //this.getPIDetailsByActivePIID = function (apiRoute, activePI) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: activePI,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}
    //this.getUserWiseCompany = function (apiRoute, LoginUserID) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: LoginUserID,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}
    //this.getLCMaster = function (apiRoute, objcmnParam) {
    //    var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
    //    debugger
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: cmnParam,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

    this.getChallanMasterList = function (apiRoute, objcmnParam) {
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