/**
 * ItemSalesCtrl.js
 */
app.directive('select2', function ($timeout) {
    return {
        restrict: 'AC',
        link: function (scope, element, attrs) {
            $timeout(function () {
                element.show();
                $(element).select2();
            });
        }
    };
})
app.controller('itemSalesCtrl', function ($scope, crudService, $filter) {
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;

    $scope.PanelTitle = 'Create Sale';
    $scope.DataPanelTitle = 'Choose items';
    $scope.SDataPanelTitle = 'Sold items';

    $scope.ListProductOutlet = [];
    $scope.selection = [];
    $scope.ListProduct = [];
    var ChoosedItems = {};

    //**********----Get All Record----***************
    function loadRecords_ot(isPaging) {
        var apiRoute = '/Sample/api/ItemSale/GetOutlet/';
        var listOutlet = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listOutlet.then(function (response) {
            $scope.ListProductOutlet = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_ot(0);

    $scope.loadRecords_pt = function (isPaging) {
        $scope.ListProductType = [];
        $scope.ddlProductType = 0;
        var id = $scope.ddlProductOutlet;
        var apiRoute = '/Sample/api/ItemSale/GetItemType/' + id + '/';
        var listType = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listType.then(function (response) {
            $scope.ListProductType = response.data
            $scope.ddlProductType = $scope.ListProductType[1].TypeID;
            $("#ProductType").select2("data", { id: $scope.ListProductType[1].TypeID, text: $scope.ListProductType[1].TypeName });
            $scope.loadRecords_pr(0);
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    //**********----Get All Record----***************
    $scope.loadRecords_pr = function (isPaging) {
        $scope.ListProduct = [];
        var id = $scope.ddlProductType;
        var apiRoute = '/Sample/api/ItemSale/GetItem/' + id + '/';
        var listItem = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listItem.then(function (response) {
            $scope.ListProduct = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    //**********----Create New Record----***************
    $scope.save = function () {
        ChoosedItems = {
            OutletID: $scope.ddlProductOutlet,
            TypeID: $scope.ddlProductType,
            Items: $scope.selection.toString()
        };

        isExisting = 0;
        if (isExisting === 0) {
            var apiRoute = '/Sample/api/ItemSale/SaveSale/';
            var CustomerCreate = crudService.post(apiRoute, ChoosedItems);
            CustomerCreate.then(function (response) {

                if (response.data === 1) { Command: toastr["success"]("Sale Made Succesfully!!!!"); }
                else { Command: toastr["error"]("Sale Doesn't Made!!!!"); }

                loadRecords_slp(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {

        }
    };

    //**********----Get All Record----***************
    function loadRecords_slp(isPaging) {
        //debugger
        var apiRoute = '/Sample/api/ItemSale/GetSoldItems/';
        var listOutlet = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listOutlet.then(function (response) {
            $scope.ListSProduct = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_slp(0);

    //******=========Multiple Checkbox=========******
    $scope.toggleSelection = function toggleSelection(ProductID) {
        var idx = $scope.selection.indexOf(ProductID);
        if (idx > -1) {
            $scope.selection.splice(idx, 1);  // is currently selected
        }
        else {
            $scope.selection.push(ProductID); // is newly selected
        }
    };

    //**********----Reset Record----***************
    $scope.clear = function () {
        ChoosedItems = {};
        ChoosedItems.Items = '';
    };

});

