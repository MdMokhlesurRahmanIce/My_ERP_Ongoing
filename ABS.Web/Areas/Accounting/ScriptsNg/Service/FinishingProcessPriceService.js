/**
 * CustomerSrvc.js
 */

app.service('finishingProcessPriceService', function ($http) {

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

    //this.getModelHDO = function (apiRoute, objcmnParam) {
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

    ////**********----Get Single Record----***************
    //this.getModelByID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //}

    //this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID) {
    //    // debugger
    //    var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

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


    //this.getItemMasterByGroup = function (apiRoute, objcmnParam, groupID) {

    //    var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(groupID) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: cmnParamWitGroupID,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}

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

    //this.getPIMasterListByPIActive = function (apiRoute, objcmnParam) {
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