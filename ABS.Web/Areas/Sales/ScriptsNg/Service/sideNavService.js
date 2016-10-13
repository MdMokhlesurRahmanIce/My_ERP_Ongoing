
app.service('sideNavService', function ($http) {

    var urlGet = '';
    //*****************  method For Layout Side Menu ********************
    this.GetSideMenu = function (apiRoute, companyID, loggedUser, ModuleID) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + ModuleID;
        return $http.get(urlGet);
    }

    //Get----get single data from server
    this.getByID = function (ngRouteAction) {
        return $http.get(ngRouteAction);
    }
     

});