/**
 * InternalIssueService.js
 */

app.service('InternalIssueService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';


    this.getAll = function (apiRoute, page, pageSize, isPaging, CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }

    this.getInternalIssueDetails = function (apiRoute, page, pageSize, isPaging, CompanyID, IsIssuedBall, IsReceivedDy, IsIssuedDy, IsReceivedLCB) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + IsIssuedBall + '/' + IsReceivedDy + '/' + IsIssuedDy + '/' + IsReceivedLCB;
        return $http.get(urlGet);
    }

    //**********----Get Single Record----***************
    this.getAllById = function (apiRoute, page, pageSize, isPaging, ItemID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemID;
        return $http.get(urlGet);
    }
    this.getSetDetalById = function (apiRoute, page, pageSize, isPaging, SetNo) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + SetNo;
        return $http.get(urlGet);
    }

    this.getSetDetalByIssueId = function (apiRoute, page, pageSize, isPaging, IssueID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + IssueID;
        return $http.get(urlGet);
    }

    
    this.GetCanByDepartment = function (apiRoute, page, pageSize, isPaging, DepartmentID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + DepartmentID;
        return $http.get(urlGet);
    }

    this.postInternalIssue = function (apiRoute, PrdInternalIssueDetail, PrdInternalIssue) {
        // debugger
        var strFinal = "[" + JSON.stringify(PrdInternalIssueDetail) + "," + JSON.stringify(PrdInternalIssue) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.getAllItemGroups = function (apiRoute, objcmnParam) {
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

        //urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        //return $http.get(urlGet);
    }

    this.getItemGroupByID = function (apiRoute) {
        return $http.get(apiRoute);
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