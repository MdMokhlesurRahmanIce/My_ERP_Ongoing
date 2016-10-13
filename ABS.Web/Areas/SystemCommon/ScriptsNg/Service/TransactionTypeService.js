/**
 * ng_CrudService.js
 */

app.service('TransactionTypeService', function ($http) {
    //**********----Get All Record----***************
    var urlGet = '';
   

    this.getAll = function (apiRoute, page, pageSize, isPaging,CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }



   
    //**********----Get Single Record----***************
    this.getItemParentesById = function (apiRoute, page, pageSize, isPaging, TypeId) {
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + TypeId;
        return $http.get(urlGet);
    }

    this.getAllTransactionTypes = function (apiRoute, page, pageSize, isPaging, CompanyID) {

        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + CompanyID;
        return $http.get(urlGet);
    }
    
    this.getTransactionTypeByID = function (apiRoute) {
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