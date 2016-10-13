app.service('FinishGoodSerivce', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';

    //**************--GetAll
 
    this.getAll = function (apiRoute, page, pageSize, isPaging,ComapnyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID;
        return $http.get(urlGet);
    }

    this.getWeavingMachines = function (apiRoute, page, pageSize, isPaging, departmentId) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + departmentId;
        return $http.get(urlGet);
    }

    this.getAllCoatingByID = function (apiRoute, page, pageSize, isPaging, ComapnyID, CTypeID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID + '/' + CTypeID;
        return $http.get(urlGet);
    }

    this.getFinishProcessByItem = function (apiRoute, page, pageSize, isPaging, ComapnyID, Item) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID + '/'+Item;
        return $http.get(urlGet);
    }

    this.getAccDetailsByGroupID = function (apiRoute, page, pageSize, isPaging, LoggedCompanyID, GroupID) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + LoggedCompanyID + '/' + GroupID;
        return $http.get(urlGet);
    }

    //***************** FINI Weight ******************
    this.getFiniWeight = function (apiRoute, page, pageSize, isPaging, CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }



    this.getAllFinishGood = function (apiRoute, objcmnParam) {
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.postConsumptionmodal = function (apiRoute, ConsumptionDetails, ConsumptionMaster) {
        // debugger
        var strFinal = "[" + JSON.stringify(ConsumptionDetails) + "," + JSON.stringify(ConsumptionMaster) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, 
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.getFinishGoodByID = function (apiRoute) {
        return $http.get(apiRoute);
    }

    //this.getYarnByID = function (apiRoute) {
    //    return $http.get(apiRoute);
    //}


    this.getYarnByID = function (apiRoute, yarnId, CompanyID) {

        urlGet = apiRoute + yarnId + '/' + CompanyID;
        return $http.get(urlGet);
    }

   
    this.getLoadByYarnID = function (apiRoute) {
        return $http.get(apiRoute);
    }
    this.getAllItemGroup = function (apiRoute, page, pageSize, isPaging, ItemTypeID, CompanyId) {
        
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ItemTypeID + '/' + CompanyId;
        return $http.get(urlGet);
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

    this.postFile = function (apiRoute, finishProcess) {
        // debugger
        var strFinal = "[" + JSON.stringify(finishProcess) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }
    this.postList = function (apiRoute, finishGood,finishProcess) {
        // debugger
        var strFinal = "[" + JSON.stringify(finishGood) + "," + JSON.stringify(finishProcess) + "]";
        //var request = $http({
        //    method: "post",
        //    url: apiRoute,
        //    data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
        //    dataType: "json",
        //    contentType: "application/json"
        //});
        //return request;
        var request = $.ajax({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            async: false,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.putList = function (apiRoute, finishGood, finishProcess) {
        // debugger
        var strFinal = "[" + JSON.stringify(finishGood) + "," + JSON.stringify(finishProcess) + "]";
        //var request = $http({
        //    method: "post",
        //    url: apiRoute,
        //    data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
        //    dataType: "json",
        //    contentType: "application/json"
        //});
        //return request;
        var request = $.ajax({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            async: false,
            dataType: "json",
            contentType: "application/json"
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