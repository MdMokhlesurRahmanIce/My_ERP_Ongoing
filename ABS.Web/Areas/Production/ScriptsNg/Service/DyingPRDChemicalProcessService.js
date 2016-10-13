app.service('DyingPRDChemicalProcessService', function ($http) {
    var urlGet = '';
    //**********----Get All Record----***************
    this.getAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
        //var request = $http({
        //    method: "get",
        //    url: urlGet,
        //    headers: headerToken
        //});
        //return request;
    }

    this.getByID = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging , itemID) {
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + itemID;
        return $http.get(urlGet);
    }
     
});