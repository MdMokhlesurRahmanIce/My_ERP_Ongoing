app.factory('dataHandler', function ($http) {
    debugger
    var GetRandomArtists = function (data, callback) {
        debugger
        //$http.post(URL, data).success(function (response) {
        //    callback(response);
        //});
        $http({
            method: "post",
            url: '/SystemCommon/api/SystemCommonLayout/GetMenuPermission/',
            data: "[" + JSON.stringify({ MenuPath: 'SystemCommon/Company', loggedCompanyID: 1, loggedUserID: 1 }) + "]",
            dataType: "json",
            contentType: "application/json"
        }).success(function (response) {
            debugger
            callback(response);
          //  return response.data;
        });

        var result = [{Name:'Sha'}]
        return { data: result }
    }
    
  
    });