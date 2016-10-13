/**
/**
 * CustomerCtrl.js
 */


app.directive('whenScrolled', function () {
     
    return function (scope, elm, attr) {
        var raw = elm[0];
        elm.bind('scroll', function () {
            if (raw.scrollTop + raw.offsetHeight >= raw.scrollHeight) {
                 
                scope.$apply(attr.whenScrolled);
            }
        });
    };
})


app.controller('moduleCtrl', function ($scope, moduleService, $filter, $http) {
    //  
    var isExisting = 0;
    var page = 1;
    var pageSize = 20;
    var isPaging = 0;
    var totalData = 0;

    $scope.btnSaveUpdateText = "Save";
    $scope.loaderMore = false;
    $scope.scrollended = false;
    var inCallback = false;

    $scope.PageTitle = 'Create Module';
    $scope.ListTitle = 'Module Records';
    $scope.ModID = 0;
    $scope.ListCompany = [];
    $scope.listModule = [];

    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();

    function loadUserCommonEntity(num) {
        debugger
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
        
    }
    loadUserCommonEntity(0);
    $scope.loadRecordsModule = function (isPaging) {
        var apiRoute = '/SystemCommon/api/Module/GetModules/';
        if (isPaging === 1) {
            isPaging = 1;
            if (page > -1 && !inCallback) {
                inCallback = true;
                page++;

                $scope.loaderMore = true;
                $scope.lblMessage = ' Loading next page ' + page + ' of ' + pageSize + ' data...';
                $scope.result = "color-orange";
                 
                var processModuleRecord = moduleService.GetModulesNew(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
                processModuleRecord.then(function (response) {
                    
                    totalData = response.data.length;
                    if (totalData === 0) {
                        $scope.loaderMore = false;
                        $scope.scrollended = true;
                        $scope.lblMessage = 'No more record to load....!';
                        $scope.result = "color-red";
                        inCallback = false;
                        page = -1;
                    }
                    else {
                        
                        for (var model in response.data) {
                            $scope.listModule.push(response.data[model]);
                        }

                        $scope.loaderMore = false;
                        inCallback = false;
                    }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }//unlimited load data while scroll
        else {
            var processModuleRecord = moduleService.GetModulesNew(apiRoute, LCompanyID, LUserID, page, pageSize, isPaging);
            processModuleRecord.then(function (response) {
                $scope.listModule = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }//default load data while pageload
        
    };
    $scope.loadRecordsModule(0);


    //******=========Get All Data with Paging=========******
    $scope.loadData = function (IsPaging) {
        var geturl = '/SystemCommon/api/Module/GetModules/';
        if (IsPaging === 1) {
            IsPaging = 1;
            if (page > -1 && !inCallback) {
                inCallback = true;
                page++;

                $scope.loaderMore = true;
                $scope.LoadMessage = ' Loading page ' + page + ' of ' + pageSize + ' data...';
                $scope.result = "color-green";

                $http({
                    method: 'GET',
                    url: geturl + '/' + page + '/' + pageSize + '/' + isPaging,
                })
                .success(function (data) {
                  
                    totalData = data.length;
                    if (totalData === 0) {
                        $scope.loaderMore = false;
                        $scope.scrollended = true;
                        $scope.LoadMessage = 'No more data...!';
                        $scope.result = "color-red";
                        inCallback = false;
                        page = -1;
                    }
                    else {
                        for (model in data) {
                            $scope.listModule.push(data[model]);
                        }
                        $scope.loaderMore = false;
                        inCallback = false;
                    }

                }).error(function (error) {
                    alert('Error Loading..');
                })
            }
        }//unlimited load data while scroll
        else {
            IsPaging = 0; $scope.loaderMore = true;
            $scope.LoadMessage = ' Loading page ' + page + ' of ' + pageSize + ' data...';
            $scope.result = "color-green";

            $http({
                method: 'GET',
                url: geturl + '/' + page + '/' + pageSize + '/' + isPaging,
            }).
            success(function (data) { 
                $scope.listModule = data;
                $scope.loaderMore = false;
            }).
            error(function () {
                
                $scope.message = 'Unexpected Error while loading data!!';
                
            });
        }//default load data while pageload

    };

    //**********----Get All Record----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/Module/GetCompany/';
        var listCompany = moduleService.GetModules(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            // 
            $scope.ListCompany = response.data
            //Set Default 
            //$("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);

    function loadRecords(isPaging) {
        var apiRoute = '/SystemCommon/api/Module/GetModules/';
        var listModule = moduleService.GetModules(apiRoute, page, pageSize, isPaging);
        listModule.then(function (response) {
            $scope.listModule = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }


    //**********----Get Single Record----***************
    $scope.getModuleForEdit = function (dataModel) {
        var apiRoute = '/SystemCommon/api/Module/GetModuleByID/' + dataModel.ModuleID;
        var singleModule = moduleService.getModuleByID(apiRoute);
        singleModule.then(function (response) {
            
            $scope.PageTitle = 'Edit Module';
            $scope.ModID = response.data.ModuleID;
            $scope.ModCode = response.data.CustomCode;
            $scope.ModName = response.data.ModuleName;
            $scope.ModDesc = response.data.Description;
            $scope.ddlModeIcon = response.data.ImageURL;
            $scope.Modepath = response.data.ModulePath;
            
            $scope.Modequence = response.data.Sequence;
            $scope.btnSaveUpdateText = "Update";
            $scope.ddlCompany = dataModel.CompanyID;
            $("#companyDropDown").select2("data", { id: dataModel.CompanyID, text: dataModel.CompanyName });
            $("#ModeIconDropDown").select2("data", { id: $scope.ddlModeIcon, text: $scope.ddlModeIcon });
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    //**********----Create New Record----***************
    $scope.save = function () {

        var Module = {
            ModuleID: $scope.ModID,
            CustomCode: $scope.ModCode,
            ModuleName: $scope.ModName,
            Description: $scope.ModDesc,
            Sequence: $scope.Modequence,
            CompanyID: LCompanyID,
            ModulePath: $scope.Modepath,
            ImageURL: $scope.ddlModeIcon
        };
        isExisting = $scope.ModID;
        if (isExisting === 0) {
            var apiRoute = '/SystemCommon/api/Module/SaveModule/';
            var ModuleCreate = moduleService.post(apiRoute, Module);
            ModuleCreate.then(function (response) {
                //Manipulate Message based on response Data
                $scope.loadRecordsModule(0);
                ShowCustomToastrMessage(response);
                $scope.clear();
                modal_fadeOut();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            var apiRoute = '/SystemCommon/api/Module/UpdateModule/';
            var ModuleUpdate = moduleService.put(apiRoute, Module);
            ModuleUpdate.then(function (response) {
                response.data = -102;
                ShowCustomToastrMessage(response);
                $scope.loadRecordsModule(0);
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
        var IsConf = confirm('You are about to delete ' + dataModel.CompanyName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/SystemCommon/api/Module/DeleteModule/' + dataModel.ModuleID;
            var ModuleDelete = moduleService.delete(apiRoute);
            ModuleDelete.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                $scope.clear();
                $scope.loadRecordsModule(0);
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Reset Record----***************

    $scope.clear = function () {

        $scope.ModID = 0;
        $scope.ModCode = '';
        $scope.ModName = '';
        $scope.ModDesc = '';
        $scope.Modequence = '';
        $scope.Modepath = '';
        $scope.btnSaveUpdateText = "Save";
        $scope.ddlModeIcon = "icon-cogs";
        $("#ModeIconDropDown").select2("data", { id: $scope.ddlModeIcon, text: $scope.ddlModeIcon });
        $("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
    };
});

function modal_fadeOut() {
    $("#moduleModal").fadeOut(200, function () {
        $('#moduleModal').modal('hide');
    });
}