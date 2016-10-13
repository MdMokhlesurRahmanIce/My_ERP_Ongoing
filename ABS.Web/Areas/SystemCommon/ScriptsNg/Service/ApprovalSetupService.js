/**
 * ng_CrudService.js
 */

app.service('approvalSetupService', function ($http) {


    this.getBranch = function (apiRoute, page, pageSize, isPaging,  CompanyId) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyId;
        return $http.get(urlGet);
    }
    this.getTeam = function (apiRoute, page, pageSize, isPaging, DepartmentID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + DepartmentID;
        return $http.get(urlGet);
    }
    this.getAll = function (apiRoute) {
        return $http.get(apiRoute);
    }
    //var urlGet = '';

    ////**********----Get All Company Records----***************
    //this.GetCompanies = function (apiRoute, page, pageSize, isPaging) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}
    ////**********----Get All Module Records----***************
    //this.GetModules = function (apiRoute, page, pageSize, isPaging) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}

    ////**********----Get All Status Records----***************
    //this.GetStatus = function (apiRoute, page, pageSize, isPaging) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}

    ////**********----Get All Menu Records----***************
    //this.GetDrpMenues = function (apiRoute, page, pageSize, isPaging) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}
    ////**********----Get All Menu Type Records----***************
    //this.GetMenuType = function (apiRoute, page, pageSize, isPaging) {
    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}

    ////**********----Get All Record----***************
    //this.GetMenues = function (apiRoute, page, pageSize, isPaging) {

    //    urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}

    ////**********----Get All Company Records----***************
    //this.GetDrpOrganograms = function (apiRoute, companyID, logid, page, pageSize, isPaging) {
    //    urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
    //    return $http.get(urlGet);
    //}
    ////**********----Get Single Record----***************
    //this.getMenuByID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //}

    //**********----ApprovalSetup Create Update----***************
    this.postApprovalMasterDetail = function (apiRoute, workFlowMaster, workFlowMasterDetail) {
        // debugger
        var strFinal = "[" + JSON.stringify(workFlowMaster) + "," + JSON.stringify(workFlowMasterDetail) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getApprovalListByWorkFlowID = function (apiRoute, workFlowID) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: workFlowID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getApprovalSetupList = function (apiRoute) {
    	var request = $http({
    		method: "post",
    		url: apiRoute,
    		data: '',
    		dataType: "json",
    		contentType: "application/json"
    	});
    	return request;
    }


    ////**********----Create New Record----***************
    //this.post = function (apiRoute, Customer) {

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: Customer
    //    });
    //    return request;
    //}

    ////**********----Update the Record----***************
    //this.put = function (apiRoute, Customer) {
    //    var request = $http({
    //        method: "put",
    //        url: apiRoute,
    //        data: Customer
    //    });
    //    return request;
    //}

    ////**********----Delete the Record----***************
    //this.delete = function (apiRoute) {
    //    var request = $http({
    //        method: "delete",
    //        url: apiRoute
    //    });
    //    return request;
    //}
});