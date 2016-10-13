/// <reference path="../Service/CrudService.js" />


app.controller('ConsumptionController', function ($scope, crudService) {
    var baseUrl = '/Production/api/Consumption/';
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;
    $scope.btnConsumptionSaveText = "Save";
    $scope.PageTitle = 'Create Consumption';
    $scope.ListTitle = 'Consumption Records';
    $scope.ConsumptionTypeId = 0;

    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.getConsumptionById = function (dataModel) {
        debugger
        var apiRoute = baseUrl + 'GetConsumptionById/' + dataModel.ConsumptionTypeID;
        var singleConsumption = crudService.getItemByID(apiRoute);
        singleConsumption.then(function (response) {
            $scope.ConsumptionTypeId = response.data.ConsumptionTypeID
            $scope.TypeName = response.data.ConsumptionTypeName
            $scope.btnConsumptionSaveText = "Update";
        },
        function (error) {
            console.log("Error: " + error);
        });
    };
    function LoadConsumptions(isPaging) {
        debugger
        var apiRoute = baseUrl + 'GetConsumptions/';
        var consumption = crudService.GetList(apiRoute, page, pageSize, isPaging);
        consumption.then(function (response) {
            $scope.Consumptions = response.data
        },
        function (error) {
            // debugger
            console.log("Error: " + error);
        });
    }
    LoadConsumptions(1);


    $scope.save = function () {
        debugger;


        isExisting = $scope.ConsumptionTypeId;
        if (isExisting == 0) {
            var cousumption = {
                ConsumptionTypeName: $scope.TypeName,
                CompanyID: LoginCompanyID,
                CreateBy: LoginUserID,
                IsDeleted: false
            };

            var apiRoute = baseUrl + 'SaveConsumption/';
            var SaveConsumption = crudService.post(apiRoute, cousumption);
            SaveConsumption.then(function (response) {
                debugger;
                ShowCustomToastrMessage(response);
                $scope.clear();
                LoadConsumptions(1);
            }, function (error) {
                console.log("Error: " + error);
            });
        }
        else {

            var cousumption = {
                ConsumptionTypeID: $scope.ConsumptionTypeId,
                ConsumptionTypeName: $scope.TypeName,
                CompanyID: LoginCompanyID,
                UpdateBy: LoginUserID
            };

            debugger
            var apiRoute = baseUrl + '/UpdateConsumption/';
            var CompanyUpdate = crudService.put(apiRoute, cousumption);
            CompanyUpdate.then(function (response) {
                response.data = -102;
                ShowCustomToastrMessage(response);
                LoadConsumptions(1);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });

        }

    }
    $scope.delete = function (dataModel) {

        //var cousumption = {
        //    ConsumptionTypeID: $scope.ConsumptionTypeId,
        //    ConsumptionTypeName: $scope.TypeName,            
        //    DeleteBy: LoginUserID
        //};

        debugger
        var IsConf = confirm('You are about to delete ' + dataModel.ConsumptionTypeName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = baseUrl + 'DeleteConsumption/';
            var deleteConsumption = crudService.put(apiRoute, dataModel);
            deleteConsumption.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                LoadConsumptions(1);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }
    $scope.clear = function () {
        $scope.ConsumptionTypeId = 0;
        $scope.TypeName = '';
        $scope.btnConsumptionSaveText = "Save";

    };

});