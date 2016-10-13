/**
 *  
 */

app.service('commonComboBoxGetDataService', function ($http) {
    var urlGet = '';
    //**********----Get All Company Records----***************
    this.GetAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, headerToken) {
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
   

    this.GetItem = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, GroupID, headerToken) {
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + GroupID;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.GetMachine = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemTypeID, ItemGroupID, headerToken) {
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID + '/' + ItemGroupID;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }


    this.GetDepartment = function (apiRoute, companyID, loggedUser, headerToken) {
        urlGet = apiRoute + companyID + '/' + loggedUser ;
        //return $http.get(urlGet);
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
     
});