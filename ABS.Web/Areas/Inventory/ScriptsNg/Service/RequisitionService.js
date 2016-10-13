/**
 * ng_CrudService.js
 */ 

app.service('RequisitionService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';

    this.getAll = function (apiRoute, page, pageSize, isPaging, headerToken) {
         
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
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
    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    this.getModelByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    this.getAllUsersGroup = function (apiRoute, objcmnParam) {
        debugger
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

    this.GetItmDetail = function (apiRoute, objcmnParam, headerToken) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
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
    
    this.GetItmDetailByItmCode = function (apiRoute, objcmnParam, ItemCode, headerToken) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ItemCode) + "]";
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
    this.GetItmDetailByItemId = function (apiRoute, objcmnParam, ItemId) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ItemId) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;

    }

    this.getByRequisitionID = function (apiRoute, RequisitionId, ComapnyId, headerToken) {
        urlGet = apiRoute + RequisitionId + '/' + ComapnyId;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    //this.getByRequisitionID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //}
    this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging) {

        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getByID = function (apiRoute, page, pageSize, isPaging, ID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ID;
        return $http.get(urlGet);
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

    this.postSPRMasterDetail = function (apiRoute, modelMaster, modelDetails, UserCommonEntity,fileList, headerToken) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(UserCommonEntity) + "," + JSON.stringify(fileList) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });       
        return request;
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, UserCommonEntity, headerToken) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(UserCommonEntity) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }
   
    this.GetRequisitionMst = function (apiRoute, objcmnParam, TransactionTypeId, headerToken) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(TransactionTypeId) + "]";
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
    //**********----Get All Company Records----***************
    this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging) {
        
        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllUsers = function (apiRoute, page, pageSize, isPaging, UserTypeID, LoginCompanyID, headerToken) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + UserTypeID + '/' + LoginCompanyID;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;       
    }

    this.getItemGroup = function (apiRoute, page, pageSize, isPaging, ItemTypeID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID;
        return $http.get(urlGet);
    }
   
    this.getunit = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getCompany = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

   
    this.getAllRawItems = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllBatch = function (apiRoute, page, pageSize, isPaging, ItemID, LoginCompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemID + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }

    this.GetAllRequisitionType = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllLotNo = function (apiRoute, page, pageSize, isPaging, ItemID, LoginCompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemID + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }

    this.getCompanyByDeptID = function (apiRoute, page, pageSize, isPaging, companyID, headerToken) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + companyID;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;

       
    }

    this.GetAllCompany = function (apiRoute, page, pageSize, isPaging, companyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging +'/'+ companyID;;
        return $http.get(urlGet);
    }
    
});