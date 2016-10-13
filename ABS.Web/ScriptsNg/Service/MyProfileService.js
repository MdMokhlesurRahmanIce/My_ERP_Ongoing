app.service('crdMyProfileSerive', function ($http) {
    var urlGet = '';
    this.getCurrentUserPassword = function (apiRoute, companyID, loggedUser) {
        // debugger
        urlGet = apiRoute + companyID + '/' + loggedUser;
        return $http.get(urlGet);
    }
    this.post = function (apiRoute, Model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    }
});