/**
 * CustomerCtrl.js
 */

app.controller('customerCtrl', function ($scope, crudService) {
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;

    $scope.PanelTitle = 'Create Customer';
    $scope.DataPanelTitle = 'Data Customer';
    $scope.CustID = 0;

    loadRecords(0);

    //**********----Get All Record----***************
    function loadRecords(isPaging) {
        debugger
        var apiRoute = '/Sample/api/Customer/GetCustomers/';
        var listCustomer = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listCustomer.then(function (response) {
            $scope.ListCustomer = response.data

        },
        function (error) {
            debugger
            console.log("Error: " + error);
        });
    }

    //**********----Get Single Record----***************
    $scope.getCustomer = function (dataModel) {

        var apiRoute = '/Sample/api/Customer/GetCustomerByID/' + dataModel.CustomerID;
        var singleCustomer = crudService.getByID(apiRoute);
        singleCustomer.then(function (response) {
            $scope.CustID = response.data.CustomerID
            $scope.CustName = response.data.Name
            $scope.CustEmail = response.data.Email;
            $scope.CustMobile = response.data.Mobile;
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    //**********----Create New Record----***************
    $scope.save = function () {
        debugger
        var Customer = {
            CustomerID: $scope.CustID,
            Name: $scope.CustName,
            Email: $scope.CustEmail,
            Mobile: $scope.CustMobile
        };

        isExisting = $scope.CustID;

        if (isExisting === 0) {
            var apiRoute = '/Sample/api/Customer/SaveCustomer/';
            var CustomerCreate = crudService.post(apiRoute, Customer);
            CustomerCreate.then(function (response) {
                loadRecords(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            var apiRoute = '/Sample/api/Customer/UpdateCustomer/';
            var CustomerUpdate = crudService.put(apiRoute, Customer);
            CustomerUpdate.then(function (response) {
                loadRecords(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

    };

    //**********----Delete Single Record----***************
    $scope.delete = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.Name + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/Sample/api/Customer/DeleteCustomer/' + dataModel.CustomerID;
            var CustomerDelete = crudService.delete(apiRoute);
            CustomerDelete.then(function (response) {
                loadRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Reset Record----***************
    $scope.clear = function () {
        $scope.CustID = 0;
        $scope.CustName = '';
        $scope.CustEmail = '';
        $scope.CustMobile = '';
    };
});

