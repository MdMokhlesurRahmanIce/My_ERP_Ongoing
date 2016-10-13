/**
 * CustomerSrvc.js
 */

app.service('IssueService', function ($http) {

    //**********----Get All Record----***************
    var urlGet = '';

    this.getAll = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getunit = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        return $http.get(apiRoute);
    }
    this.getAllRawItems = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getByIssueID = function (apiRoute, IssueID, CompanyID, headerToken) {
        urlGet = apiRoute + IssueID + '/' + CompanyID;
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

    this.getIssueMasterList = function (apiRoute, objcmnParam, headerToken) {
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
    this.post = function (apiRoute, Model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    }

    //**********----Create New Record----***************
    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID, headerToken) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

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

    this.getReqisitionlst = function (apiRoute, page, pageSize, isPaging, CompanyID, TransactionId) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + TransactionId;
        var request = $http({
            method: "get",
            url: urlGet
         
        });
        return request;
    }
    this.GetAllCompany = function (apiRoute, page, pageSize, isPaging, companyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + companyID;;
        return $http.get(urlGet);
    }
    this.getMRRlst = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging, headerToken) {

        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
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

    this.getItemDetailsByRequisitionID = function (apiRoute, page, pageSize, isPaging, RequisitionID, MrrID, headerToken) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + RequisitionID + '/' + MrrID;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
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

    this.getAllBatch = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllLotNo = function (apiRoute, page, pageSize, isPaging) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
});