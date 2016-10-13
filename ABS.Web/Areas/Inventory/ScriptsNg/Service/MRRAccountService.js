/**
 * CustomerSrvc.js
 */

app.service('mRRAccountService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.GetMrrTypes = function (apiRoute, mrrType, objcmnParam)
    {
        var cmnParamWithGrrNo = "[" + JSON.stringify(mrrType) + "," + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithGrrNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getMrrMasterList = function (apiRoute, objcmnParam)
    {
        var cmnParamWithGrrNo = "[" + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithGrrNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getMasterInfoByGrrNo = function (apiRoute, objcmnParam, GrrNo) {

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

    this.getMrrDetailsListByMrrID = function(apiRoute, mrrID, objcmnParam)
    {
        var cmnParamWithMrrID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(mrrID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithMrrID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getDetailInfoByGrrNo = function (apiRoute, objcmnParam, GrrNo) {

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
    this.GetQCListByGrrNo = function (apiRoute, objcmnParam, GrrNo) {

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


    this.GetList = function (apiRoute, cmnParam) {
         
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;

        //var request = $.Ajax({
        //    method: "post",
        //    url: apiRoute,
        //    data: cmnParam,
        //    dataType: "json",
        //    async: false,
        //    contentType: "application/json"
        //});
        //return request;
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



    this.getDetailInfoByQCID = function (apiRoute, objcmnParam, qcID) {

        var cmnParamWithGrrNo = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(qcID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWithGrrNo,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetSuppliers =  function (apiRoute, objcmnParam)
    {
        var finalPrms = "[" + JSON.stringify(objcmnParam)+ "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: finalPrms,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.GetIssueNo = function (apiRoute, objcmnParam) {
        var finalPrms = "[" + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: finalPrms,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }


    this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging) {

        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.postMrrMasterDetail = function (apiRoute, mrrMaster, mrrDetail, menuID, transactionTypeID) {
        var strFinal = "[" + JSON.stringify(mrrMaster) + "," + JSON.stringify(mrrDetail) + "," + JSON.stringify(menuID) + "," + JSON.stringify(transactionTypeID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,  
            dataType: "json",
            contentType: "application/json"
        }); 
        return request;
    }

    this.postSaveLot = function(apiRoute, lotMaster)
    {
        var strFinal = "[" + JSON.stringify(lotMaster) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.postSaveBatch = function(apiRoute, batchMaster)
    {
        var strFinal = "[" + JSON.stringify(batchMaster) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
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