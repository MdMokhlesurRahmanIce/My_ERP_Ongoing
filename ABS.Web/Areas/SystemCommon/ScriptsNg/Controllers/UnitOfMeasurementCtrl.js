/**
 * UnitOfMeasurementCtrl.js
 */

app.controller('unitOfMeasurementCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        var baseUrl = '/SystemCommon/api/UnitOfMeasurement/';
        var isExisting = 0;
        var page = 1;
        var pageSize = 10;
        var isPaging = 0;
        var totalData = 0;
        $scope.btnUnitOfMeasurementSaveText = "Save";
        $scope.PageTitle = 'Create Unit of Measurement';
        $scope.ListTitle = 'Unit of Measurement Records';

        $scope.UOMID = 0;

        loadUnitOfMeasurementRecords(0);
        loadUOMGroupRecords(0);
        //
        $scope.CustomCode = 1;
        $scope.UOMGroup = 0;
        //


        //**********----Get Record of UOMGroup ----***************
        function loadUOMGroupRecords(isPaging) {
            debugger
            var apiRoute = baseUrl + 'GetUOMGroup/';
            var listUOMGroup = crudService.getAll(apiRoute, page, pageSize, isPaging);
            listUOMGroup.then(function (response) {
                $scope.listUOMGroup = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----Get All Record----***************
        function loadUnitOfMeasurementRecords(isPaging) {
            var apiRoute = baseUrl + 'GetUnitOfMeasurement/';
            var listUnitOfMeasurement = crudService.getAll(apiRoute, page, pageSize, isPaging);
            listUnitOfMeasurement.then(function (response) {
                $scope.listUnitOfMeasurement = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        //**********----Get Single Record----***************
        $scope.getUnitOfMeasurement = function (dataModel) {

            var apiRoute = baseUrl + 'GetUnitOfMeasurementById/' + dataModel.UOMID;
            var singleUnitOfMeasurement = crudService.getByID(apiRoute);
            singleUnitOfMeasurement.then(function (response) {
                $scope.UOMID = response.data.UOMID;
                $scope.CustomCode = response.data.CustomCode;
                $scope.UOMName = response.data.UOMName;
                $scope.UOMShortName = response.data.UOMShortName;//new Date(response.data.SaleDate);
                // $scope.UOMGroupID = response.data.UOMGroupID;
                $scope.UOMGroup = response.data.UOMGroupID;
                $scope.btnUnitOfMeasurementSaveText = "Update";
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //**********----Create New Record----***************
        $scope.save = function () {
            var UnitOfMeasurement = {
                UOMID: $scope.UOMID,
                CustomCode: $scope.CustomCode,
                UOMName: $scope.UOMName,
                UOMShortName: $scope.UOMShortName,
                UOMGroupID: $scope.UOMGroup//$scope.UOMGroup.UOMGroupID//$scope.UOMGroupID
            };

            //isExisting = $scope.SaleID;

            //if (isExisting === 0) {
            var apiRoute = baseUrl + 'SaveUpdateUnitOfMeasurement/';
            var UnitOfMeasurementCreateUpdate = crudService.post(apiRoute, UnitOfMeasurement);
            UnitOfMeasurementCreateUpdate.then(function (response) {
                loadUnitOfMeasurementRecords(0);
                loadUOMGroupRecords(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        };

        //**********----Delete Single Record----***************
        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.UOMName + '. Are you sure?');
            if (IsConf) {
                var apiRoute = baseUrl + 'DeleteUnitOfMeasurement/' + dataModel.UOMID;
                var UnitOfMeasurementDelete = crudService.delete(apiRoute);
                UnitOfMeasurementDelete.then(function (response) {
                    loadUnitOfMeasurementRecords(0);
                    loadUOMGroupRecords(0);
                    $scope.clear();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        //**********----Reset Record----***************
        $scope.clear = function () {
            $scope.UOMID = 0;
            $scope.UOMGroup = 0;
            $scope.CustomCode = '';
            $scope.UOMName = '';
            $scope.UOMShortName = '';
            $scope.btnUnitOfMeasurementSaveText = "Save";
        };
    }]);

