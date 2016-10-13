app.service('PublicService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';
    this.getItemMasterService= function (apiRoute, objcmnParam) {
        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json" 
        });
        return request;
    }

});