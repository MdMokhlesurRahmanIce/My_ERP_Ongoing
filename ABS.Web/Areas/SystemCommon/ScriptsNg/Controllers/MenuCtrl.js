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
 

app.controller('menuCtrl', ['$scope', '$localStorage', 'menuService', '$rootScope','$http', function ($scope, $localStorage, menuService, $rootScope,$http) {
    
    // debugger
    var baseUrl = "/SystemCommon/api/Menu/SaveMenu/";
    var isExisting = 0;
    var page = 1;
    var pageSize = 50;
    var pageSizeModule = 100;
    var isPaging = 0;
    var totalData = 0;
    $scope.btnSaveUpdateText = "Save";
    $scope.loaderMore = false;
    $scope.scrollended = false;
    var inCallback = false;
 
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create Menu';
    $scope.ListTitle = 'Menu Records';
    $scope.MenuID = 0;
    $scope.ListMenues = [];
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.LgUser = $('#hUserID').val();
  
    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
        console.log($scope.UserCommonEntity);
    }
    //loadUserCommonEntity(0);
    // ****************  BroadCust      ************
   
    var lsMenuID = ($localStorage.MenuID);
    var lsMenuList = ($localStorage.ListMenues);
    //**************** Notification Approval 

    //$localStorage.notificationStorageMenuID;
    //$localStorage.notificationStorageMasterID;
    //$localStorage.notificationStorageIsApproved;
    //$localStorage.notificationStorageIsDeclained;

    var MasterID = $localStorage.notificationStorageMasterID;
    var IsApproval = $localStorage.notificationStorageIsApproved;
    var IsDelaine = $localStorage.notificationStorageIsDeclained;
    $scope.IsApproved = IsApproval;
    $scope.IsDelained = IsDelaine;

    $scope.ApprovedMethod = function () {
         
    }

    $scope.DeclinedMethod = function () {
        //alert("Declined");
    }

    //DropDown List
    $scope.ListCompany = [];
    $scope.ListModule = [];
    $scope.ListStatus = [];
    $scope.ListParentMenu = [];
    $scope.ListMenuType = [];
    //**********----Get Company DropDown On Page Load----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = menuService.GetCompanies(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data  //Set Default 
            $("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Company(0);
    //**********----Get Module DropDown On Page Load----***************
    $scope.LoadModule = function loadRecords_Module(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetModuleWithPermission/';
        pageSizeModule = 100;
        var listModule = menuService.GetModules(apiRoute, LoginCompanyID, LoginUserID, page, pageSizeModule, isPaging);
        listModule.then(function (response) {
            $scope.ListModule = response.data
            //$("#moduleDropDown").select2("data", { id: $scope.ListModule[0].ModuleID, text: $scope.ListModule[0].ModuleName });
        },
        function (error) {
            console.log("Error: " + error);
        });
        
    };
    $scope.LoadModule(0);
   // loadRecords_Module(0);
    //**********----Get Status DropDown On Page Load----***************
    function loadRecords_Status(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetStatus/';
        var listStatus = menuService.GetStatus(apiRoute, page, pageSize, isPaging);
        listStatus.then(function (response) {
            $scope.ListStatus = response.data
            $("#statusDropDown").select2("data", { id: $scope.ListStatus[0].StatusID, text: $scope.ListStatus[0].StatusName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Status(0);
    //**********----Get Menu DropDown On Page Load----***************
    function loadRecords_Menues(isPaging,modID) {
        var moduleID = modID;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetParentMenuForDropDown/';
        var listMenues = menuService.GetParentMenues(apiRoute, page, 500, isPaging, moduleID);
        listMenues.then(function (response) {
            $scope.ListParentMenu = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Menues(0,0);
    $scope.ModuleChange = function (moduleValue) {
        if (moduleValue == null) moduleValue = 0; 
        loadRecords_Menues(0, moduleValue);
        $scope.ddlMenu = 0;
        $("#menuesDropDown").select2("data", { id: 0, text: '--Select Parent--' });
    }
    //**********----Get Menu Type DropDown On Page Load----***************
    function loadRecords_MenuType(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetMenuType/';
        var listMenuTypeVar = menuService.GetMenuType(apiRoute, page, pageSize, isPaging);
        listMenuTypeVar.then(function (response) {
            $scope.ListMenuType = response.data
            $("#menuTypeDropDown").select2("data", { id: $scope.ListMenuType[0].MenuTypeID, text: $scope.ListMenuType[0].MenuTypeName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_MenuType(0);
    //*******************Upload File Demo *******************
    //$scope.callMe = function () {
    //    //alert("Called");
    //}

    //$scope.uploadedFile = function (element) {
    //    //alert("Called2");
    //     
    //    var dd = element;
    //    //alert(element.value);
    //    $scope.$apply(function ($scope) {
    //         
    //        $scope.files = element.files;
    //        //alert($scope.files[0].name);
    //        //alert($scope.files[0].value);
    //    });
    //}
    //$('#i_file').change(function (event) {
    //    //alert("Another Call");
    //    var tmppath = URL.createObjectURL(event.target.files[0]);
    //    $("img").fadeIn("fast").attr('src', URL.createObjectURL(event.target.files[0]));
    //    //alert("path");
    //    //alert(tmppath);
    //    //$("#disp_tmp_path").html("Temporary Path(Copy it and try pasting it in browser address bar) --> <strong>[" + tmppath + "]</strong>");
    //});
    $scope.menuClicked = function (asda) {
        debugger
        $scope.DocMenuID = asda;
       
        console.log("asda: " + asda);
    }
    //******************** UpLoad FIle Test *****************
    $scope.loadRecords = function (isPaging) {
        debugger
        var apiRoute = '/SystemCommon/api/Menu/GetMenues/';
        if (isPaging === 1) {
            debugger
            isPaging = 1;
            if (page > -1 && !inCallback) {
                inCallback = true;
                page++;
                //debugger
                $scope.loaderMore = true;
                $scope.lblMessage = ' Loading next page ' + page + ' of ' + pageSize + ' data...';
                $scope.result = "color-orange";

                var processMenuRecord = menuService.GetMenues(apiRoute, page, pageSize, isPaging);
                processMenuRecord.then(function (response) {
                    //debugger
                    totalData = response.data.length;
                    if (totalData === 0) {
                        debugger
                        $scope.loaderMore = false;
                        $scope.scrollended = true;
                        $scope.lblMessage = 'No more record to load....!';
                        $scope.result = "color-red";
                        inCallback = false;
                        page = -1;
                    }
                    else {
                        debugger
                        for (var model in response.data) {
                            debugger
                            $scope.ListMenues.push(response.data[model]);
                        }
                        debugger
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
            
            var processMenuRecord = menuService.GetMenues(apiRoute, page, pageSize, isPaging);
            processMenuRecord.then(function (response) {
                //debugger
                $scope.ListMenues = response.data
            },
            function (error) {
                console.log("Error: " + error);
            });
        }//default load data while pageload
    };
    $scope.loadRecords(0);

    //**********----Get All Record----***************

   //function loadRecords(isPaging) {
   //     var apiRoute = '/SystemCommon/api/Menu/GetMenues/';
   //     var listMenuesTemp = menuService.GetMenues(apiRoute, page, pageSize, isPaging);
   //     listMenuesTemp.then(function (response) {
   //         $scope.ListMenues = response.data
   //     },
   //     function (error) {
   //         console.log("Error: " + error);
   //     });
   // }
   // loadRecords(0);
   
    
    //**********----Get Single Record----***************
    $scope.getMenuForEdit = function (dataModel) {
         
        var apiRoute = '/SystemCommon/api/Menu/GetMenuByID/' + dataModel.MenuID;
        var singleModule = menuService.getMenuByID(apiRoute);
        singleModule.then(function (response) {
            var dataModelA = response.data;
            $scope.MenuID = response.data.MenuID;
            $scope.MenuCode = response.data.CustomCode;
            $scope.MenuName = response.data.MenuName;
            $scope.ddlModule = response.data.ModuleID;
            $scope.MenuPath = response.data.MenuPath;
            $scope.ReportName = response.data.ReportName;
            $scope.ReportPath = response.data.ReportPath;
            $scope.ddlMenu = response.data.ParentID;
            $scope.Sequence = response.data.Sequence;
            $scope.ddlMenuType = response.data.MenuTypeID;
            //$scope.ddlStatus = response.data.StatusID;
            //$scope.ddlCompany = response.data.CompanyID;
            $scope.ddlMenuIcon = response.data.MenuIconCss;
            $scope.btnSaveUpdateText = "Update";
            $scope.ddlModule = $scope.ddlModule; 
            if (dataModelA.ModuleID != null)
                $('#moduleDropDown').select2('data', { id: dataModelA.ModuleID, text: dataModelA.ModuleName});
            //$("#companyDropDown").select2("data", { id: dataModelA.CompanyID, text: dataModelA.CompanyName });
            //$("#moduleDropDown").select2("data", { id: dataModelA.ModuleID, text: dataModelA.ModuleName });
            //$("#statusDropDown").select2("data", { id: dataModelA.StatusID, text: dataModelA.StatusName });
            $("#menuesDropDown").select2("data", { id: dataModelA.ParentID, text: dataModelA.ParentMenuName });
            $("#menuTypeDropDown").select2("data", { id: dataModelA.MenuTypeID, text: dataModelA.MenuTypeName });
            $("#MenuIconDropDown").select2("data", { id: $scope.ddlMenuIcon, text: $scope.ddlMenuIcon });
        },
        function (error) {
            console.log("Error: " + error);
        });
        
    };

    //**********----Create New Record----***************
    $scope.save = function () {
         
        var Menu = {
            MenuID: $scope.MenuID,
            CustomCode: $scope.MenuCode,
            MenuName: $scope.MenuName,
            ModuleID: $scope.ddlModule,
            MenuPath: $scope.MenuPath,
            ReportName: $scope.ReportName,
            ReportPath: $scope.ReportPath,
            ParentID: $scope.ddlMenu,
            Sequence: $scope.Sequence,
            MenuTypeID: $scope.ddlMenuType,
            StatusID: LoginCompanyID,
            CompanyID: LoginCompanyID,
            MenuIconCss: $scope.ddlMenuIcon
        };
        isExisting = $scope.MenuID;
        if (isExisting === 0) {
            var apiRoute = '/SystemCommon/api/Menu/SaveMenu/';
            var MenuCreate = menuService.post(apiRoute, Menu);
            MenuCreate.then(function (response) {
                ShowCustomToastrMessage(response);
                loadRecords_Menues(0);
                $scope.loadRecords(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
              
            var apiRoute = '/SystemCommon/api/Menu/UpdateMenu/';
            var ModuleUpdate = menuService.put(apiRoute, Menu);
            ModuleUpdate.then(function (response) {
                loadRecords_Menues(0);
                response.data = -102;
                ShowCustomToastrMessage(response);
                $scope.loadRecords(0);
                $scope.clear();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    };

    //**********----Delete Single Record----***************
    $scope.delete = function (dataModel) {
        // 
        var IsConf = confirm('You are about to delete ' + dataModel.MenuName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/SystemCommon/api/Menu/DeleteMenu/' + dataModel.MenuID;
            var ModuleDelete = menuService.delete(apiRoute);
            ModuleDelete.then(function (response) {
                loadRecords_Menues(0);
                response.data = -101;
                ShowCustomToastrMessage(response);
                $scope.loadRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Reset Record----***************
    $scope.clear = function () {
        $scope.MenuID = 0;
        $scope.MenuCode = '';
        $scope.MenuName = '';
        $scope.ddlModule = 0;
        $scope.MenuPath = '';
        $scope.ReportName = '';
        $scope.ReportPath = '';
        $scope.ddlMenu = 0;
        $scope.Sequence = '';
        $scope.ddlMenuType = 0;
        //$scope.ddlStatus = 0;
        //$scope.ddlCompany = 0;
        $scope.ddlMenuIcon = "icon-cogs";
        $scope.btnSaveUpdateText = "Save";
        //$("#companyDropDown").select2("data", { id: $scope.ListCompany[0].CompanyID, text: $scope.ListCompany[0].CompanyName });
        $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--' });
        $("#menuesDropDown").select2("data", { id: 0, text: '--Select Parent--' });
        $("#statusDropDown").select2("data", { id: $scope.ListStatus[0].StatusID, text: $scope.ListStatus[0].StatusName });
        //$("#menuesDropDown").select2("data", { id: $scope.ListParentMenu[0].MenuID, text: $scope.ListParentMenu[0].MenuName });
        $("#menuTypeDropDown").select2("data", { id: $scope.ListMenuType[0].MenuTypeID, text: $scope.ListMenuType[0].MenuTypeName });
        $("#MenuIconDropDown").select2("data", { id: $scope.ddlMenuIcon, text: $scope.ddlMenuIcon });

    };
}]);

