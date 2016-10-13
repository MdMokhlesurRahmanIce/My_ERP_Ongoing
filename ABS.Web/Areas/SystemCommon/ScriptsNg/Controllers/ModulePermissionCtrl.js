/**
 * modulePermissionCtrl.js
 */

app.controller('modulePermissionCtrl', function ($scope, crudService, menuService, moduleService) {
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var inCallback = false;
    var totalData = 0;
    var companyID = 1;
    var loggedUser = 0;

    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Module Permission';
    $scope.ListTitle = 'Permission Record';
    $scope.ModulePermissionID = 0;

    $scope.ListModule = [];
    $scope.ListCompany = [];
    $scope.ListStatus = [];
    $scope.ListModulePermission = [];

    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();

    //****************Get All ******************
    function loadModulePermissionRecords(isPaging) {
        var apiRoute = '/SystemCommon/api/ModulePermission/GetAllModulePermission/';
        var processlistModulePermission = crudService.getAllIncludingCompanyuser(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
        processlistModulePermission.then(function (response) {
            $scope.ListModulePermission = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadModulePermissionRecords(0);

    //**********----Get Module DropDown On Page Load----***************
    $scope.LoadModule = function (isPaging) {
         
        var companyID = $scope.ddlCompany;
        var userID = LUserID;

        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetModules/';
        var listModule = moduleService.GetModules(apiRoute, page, pageSize, isPaging);
        listModule.then(function (response) {
            $scope.ListModule = response.data; 
            $("#moduleDropDown").select2("data", { id: $scope.ListModule[0].ModuleID, text: $scope.ListModule[0].ModuleName });
            if ($scope.ListModule== null)
            {
                $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--'});
            }
           
            //$("#moduleDropDown").select2("data", { id: $scope.ListModule[0].ModuleID, text: $scope.ListModule[0].ModuleName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    $scope.LoadModule(1);

    //**********----Get Company DropDown On Page Load----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = menuService.GetCompanies(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data  //Set Default 
            //$("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);

    //**********----Get Status DropDown On Page Load----***************
    function loadRecords_Status(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetStatus/';
        var listStatus = menuService.GetStatus(apiRoute, page, pageSize, isPaging);
        listStatus.then(function (response) {
            $scope.ListStatus = response.data
            //$("#statusDropDown").select2("data", { id: $scope.ListStatus[0].StatusID, text: $scope.ListStatus[0].StatusName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Status(0);


    //**********----Create New Record----***************
    $scope.save = function () {
        var ModulePermissionEntity = {
            ModulePermissionID: $scope.ModulePermissionID,
            CustomCode: $scope.CustomCode,
            ModuleID: $scope.ddlModule,
            CompanyID: $scope.ddlCompany
        };
        isExisting = $scope.ModulePermissionID;
        if (isExisting === 0) {
            var apiRoute = '/SystemCommon/api/ModulePermission/SaveModulePermission/';
            var processModulePermission = crudService.post(apiRoute, ModulePermissionEntity);
            processModulePermission.then(function (response) {
                //Manipulate Message based on response Data
                ShowCustomToastrMessage(response);
                loadModulePermissionRecords(0);
                $scope.clear();
                modal_fadeOut();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            var apiRoute = '/SystemCommon/api/Company/UpdateCompany2/';
            var CompanyUpdate = companyService.put(apiRoute, Company);
            CompanyUpdate.then(function (response) {
                response.data = -102;
                ShowCustomToastrMessage(response);
                loadRecords(0);
                $scope.clear();
                modal_fadeOut();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

    };

    //**********----Delete Single Record----***************
    $scope.delete = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.ModuleName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/SystemCommon/api/ModulePermission/DeletePermission/' + dataModel.ModulePermissionID;
            var ModuleDelete = menuService.delete(apiRoute);
            ModuleDelete.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                loadModulePermissionRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }
    //**********----Reset Record----***************
    $scope.clear = function () {
        $scope.ModulePermissionID = 0;
        $scope.CustomCode = "";
        $scope.btnSaveUpdateText = "Save";
        $("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        $("#moduleDropDown").select2("data", { id: $scope.ListModule[0].ModuleID, text: $scope.ListModule[0].ModuleName });
       
    };
});

function modal_fadeOut() {
    $("#modulepModal").fadeOut(200, function () {
        $('#modulepModal').modal('hide');
    });
}


