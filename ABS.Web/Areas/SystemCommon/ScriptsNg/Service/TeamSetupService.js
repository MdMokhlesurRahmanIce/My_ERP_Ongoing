/**
 * ng_CrudService.js
 */

app.service('TeamSetupService', function ($http) {

    //**********----Get All Record----*************** 
    var urlGet = '';
    this.getOrganogram = function (apiRoute, companyID, loggUserID, page, pageSize, isPaging) {
        urlGet = apiRoute + companyID + '/' + loggUserID + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getUser = function (apiRoute, companyID, loggUserID, page, pageSize, isPaging) {
        urlGet = apiRoute + companyID + '/' + loggUserID + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.postCommonMasterDetails = function (apiRoute, modelMaster, modelDetails, commonEntity) {
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(commonEntity) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails,commonEntity=Common Model }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.getDetailsByID = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, DetailsID) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + DetailsID;
        return $http.get(urlGet);
    }
});