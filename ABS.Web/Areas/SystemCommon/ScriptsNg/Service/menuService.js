/**
 * ng_CrudService.js
 */

app.service('menuService', function ($http) {
   
     
    var urlGet = '';
    
    //**********----Get All Company Records----***************
    this.GetCompanies = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //**********----Get All Module Records----***************
    this.GetModules = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.GetModules = function (apiRoute,companyID,userID, page, pageSize, isPaging) {
        urlGet = apiRoute + companyID + '/' +userID + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    //**********----Get All Status Records----***************
    this.GetStatus = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.GetParentMenues = function (apiRoute, page, pageSize, isPaging,moduleID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + moduleID;
        return $http.get(urlGet);
    }
    this.GetDrpParentMenues = function (apiRoute, page, pageSize, isPaging, moduleID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + moduleID;
        return $http.get(urlGet);
    }
    //**********----Get All Menu Records----***************
    this.GetDrpMenues = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //**********----Get All Menu Type Records----***************
    this.GetMenuType = function (apiRoute, page, pageSize, isPaging) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    //**********----Get All Record----***************
    this.GetMenues = function (apiRoute, page, pageSize, isPaging) {
         
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    //**********----Get All Company Records----***************
    this.GetDrpOrganograms = function (apiRoute,companyID,logid, page, pageSize, isPaging) {
        urlGet = apiRoute + companyID + '/' + logid + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getMenuByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    ////**********----ApprovalSetup Create Update----***************
    //this.postApprovalMasterDetail = function (apiRoute, workFlowMaster, workFlowMasterDetail) {
    //    // debugger
    //    var strFinal = "[" + JSON.stringify(workFlowMaster) + "," + JSON.stringify(workFlowMasterDetail) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: strFinal,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}


    //**********----Create New Record----***************
    this.post = function (apiRoute, Customer) {
        
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Customer
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, Customer) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Customer
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