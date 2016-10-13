/**
 *  
 */

app.service('commonComboBoxGetDataService', function ($http) {
    var urlGet = '';
    //**********----Get All Company Records----***************
    this.GetAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
   
});