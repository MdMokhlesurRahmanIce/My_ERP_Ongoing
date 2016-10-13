/**
 * ItemSizeCtrl.js
 */

app.controller('ItemSizeCtrl', function ($scope, crudService) {
    var baseUrl = '/SystemCommon/api/ItemSize/';
    var isExisting = 0;
    var page = 1;
    var pageSize = 10;
    var isPaging = 0;
    var totalData = 0;
    $scope.btnSaleSaveText = "Save";
    $scope.PageTitle = 'Create Item Size';
    $scope.ListTitle = 'Item Size Records';
    $scope.SizeID = 0;

    loadItemSizeRecords(0);

    //**********----Get All Record----***************
    function loadItemSizeRecords(isPaging) {
        var apiRoute = baseUrl + 'GetItemSize/';
        var listItemSize = crudService.getAll(apiRoute, page, pageSize, isPaging);
        listItemSize.then(function (response) {
            $scope.listItemSize = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    //**********----Get Single Record----***************
    $scope.getItemSize = function (dataModel) {

        var apiRoute = baseUrl + 'GetItemSizeById/' + dataModel.SizeID;
        var singleItemSize = crudService.getByID(apiRoute);
        singleItemSize.then(function (response) {
            $scope.SizeID = response.data.SizeID;
            $scope.SizeName = response.data.SizeName;
            $scope.StatusID = response.data.StatusID;
            $scope.CompanyID = response.data.CompanyID;
            $scope.CreateBy = response.data.CreateBy;
            $scope.CreateOn = new Date(response.data.CreateOn);
            $scope.CreatePc = response.data.CreatePc;
            $scope.UpdateBy = response.data.UpdateBy;
            $scope.UpdateOn = new Date(response.data.UpdateOn);
            $scope.UpdatePc = response.data.UpdatePc;
            $scope.IsDeleted = response.data.IsDeleted;
            $scope.DeleteBy = response.data.DeleteBy;
            $scope.DeleteOn = new Date(response.data.DeleteOn);
            $scope.btnSaleSaveText = "Update";
            //console.log(response.data.SaleDate);
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    //**********----Create New Record----***************
    $scope.save = function () {
        var ItemSize = {
            SizeID: $scope.SizeID,
            SizeName: $scope.SizeName       
        };

        //isExisting = $scope.SaleID;

        //if (isExisting === 0) {
        var apiRoute = baseUrl + 'SaveUpdateItemSize/';
        var ItemSizeCreateUpdate = crudService.post(apiRoute, ItemSize);
        ItemSizeCreateUpdate.then(function (response) {
            loadItemSizeRecords(0);
            $scope.clear();
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    $scope.deleteUpdate = function (dataModel) {
        dataModel.IsDeleted = true;
        var IsConf = confirm('You are about to delete ' + dataModel.SizeName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = baseUrl + 'SaveUpdateItemSize/';
            var ItemSizeDeleteUpdate = crudService.post(apiRoute, dataModel);
            ItemSizeDeleteUpdate.then(function (response) {
                loadItemSizeRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    };


    //**********----Delete Single Record----***************
    $scope.delete = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.SizeName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = baseUrl + 'DeleteItemSize/' + dataModel.SizeID;
            var ItemSizeDelete = crudService.delete(apiRoute);
            ItemSizeDelete.then(function (response) {
                loadItemSizeRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Reset Record----***************
    $scope.clear = function () {
        var date = new Date();
        currentDate = ('0' + (date.getMonth() + 1)).slice(-2) + '/' + ('0' + date.getDate()).slice(-2) + '/' + date.getFullYear();

        $scope.SizeID = 0;
        $scope.SizeName = "";   
        $scope.btnSaleSaveText = "Save";
    };
});

