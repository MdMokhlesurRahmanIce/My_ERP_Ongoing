/**
 * InternalIssueService.js
 */

app.service('FebricInspectionSer', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';


    this.getAll = function (apiRoute, page, pageSize, isPaging, CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }

    this.getFebricInspectionByInspectionID = function (apiRoute, page, pageSize, isPaging, InspactionID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + InspactionID;
        return $http.get(urlGet);
    }

    this.GetOperator = function (apiRoute, page, pageSize, isPaging, CompanyID, Type) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + Type;
        return $http.get(urlGet);
    }
    this.GetFaInspByStyle = function (apiRoute, page, pageSize, isPaging, CompanyID, FinishingMRRID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + FinishingMRRID;
        return $http.get(urlGet);
    }
    this.getLoom = function (apiRoute, CompanyID) {

        urlGet = apiRoute + CompanyID;
        return $http.get(urlGet);
    }

    this.getArtical = function (apiRoute, page, pageSize, isPaging, CompanyID, Type) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + Type;
        return $http.get(urlGet);
    }

    this.getSetDetails = function (apiRoute, page, pageSize, isPaging, SetNo, LoginCompanyID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + SetNo + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }

    this.getSizeBeamIssueDetails = function (apiRoute, page, pageSize, isPaging, BeamIssueId, LoginCompanyID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + BeamIssueId + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }


    this.getMachine = function (apiRoute, page, pageSize, isPaging, CompanyID, ItemTypeID, ItemGroupID) {
        debugger
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID + '/' + ItemTypeID + '/' + ItemGroupID;
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

    this.postFebricInspection = function (apiRoute, FebricInspectionDetailsList, DeleteFebricIndpection, FinishingMaster) {
        // debugger
        var strFinal = "[" + JSON.stringify(FebricInspectionDetailsList) + "," + JSON.stringify(DeleteFebricIndpection) + "," + JSON.stringify(FinishingMaster) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }
  
    this.getFabricInspectionDetails = function (apiRoute, objcmnParam) {
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