/**
 * CustomerSrvc.js
 */

app.service('qcService', function ($http) {

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


    this.getAll = function (apiRoute, page, pageSize, isPaging, ComapnyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID;
        return $http.get(urlGet);
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

    this.postQCMasterDetail = function (apiRoute, qcMaster, qcDetail, menuID) { 
        var strFinal = "[" + JSON.stringify(qcMaster) + "," + JSON.stringify(qcDetail) + "," + JSON.stringify(menuID) + "]"; 
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetSPRPOLCNoByID = function (apiRoute, objcmnParam, SPRPOLC_ID) {

        var cmnParamWithSPRPOLC_ID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(SPRPOLC_ID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithSPRPOLC_ID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getChallanInvoiceReceiptNoByTypeID = function (apiRoute, objcmnParam, ChallanInvoiceReceiptType) {

        var cmnParamWithChallanInvoiceReceiptType = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ChallanInvoiceReceiptType) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithChallanInvoiceReceiptType,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getItemDetailByGrrNo = function (apiRoute, objcmnParam, GrrNo) {

        var cmnParamWithGrrNo = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(GrrNo) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithGrrNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetSPRPOLCDateByNo = function (apiRoute, SPRPOLCNo, SPRPOLCType) {
        var SPRPOLCTypeNo = "[" + JSON.stringify(SPRPOLCType) + "," + JSON.stringify(SPRPOLCNo) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: SPRPOLCTypeNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetCIRDateByNo = function (apiRoute, CIRNo, CIRType) {
        var CIRTypeNo = "[" + JSON.stringify(CIRType) + "," + JSON.stringify(CIRNo) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: CIRTypeNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getQCMasterList = function (apiRoute, objcmnParam) {
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
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getQCDetailsListByQCMasterID = function (apiRoute, mrrQCID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: mrrQCID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

});