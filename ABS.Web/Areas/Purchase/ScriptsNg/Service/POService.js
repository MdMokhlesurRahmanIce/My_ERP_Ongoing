/**
 * CustomerSrvc.js
 */

app.service('pOService', function ($http) {

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

    this.GetTerm = function (apiRoute, objcmnParam) {
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
            contentType: "application/json" ,
            headers: headerToken
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

    this.GetItmDetailByItmCode = function (apiRoute, objcmnParam, ItemCode) {
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
     

    ////**********----Get Single Record----***************
    this.getModelByID = function (apiRoute) {
        return $http.get(apiRoute);
    } 

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
     


    this.getItemMasterByGroup = function (apiRoute, objcmnParam) {

        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "]";

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