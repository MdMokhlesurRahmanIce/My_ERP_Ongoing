/// <reference path="../Service/CrudService.js" />
/// <reference path="ChemicalSetupCtrl.js" />
app.controller('TeamSetupCtrl', ['$scope', 'TeamSetupService', 
    function ($scope, TeamSetupService) {

    $scope.gridOptionsMSetup = [];
    $scope.gridOptionsMSetup.enableCellEditOnFocus = true;
    var objcmnParam = {};

    var baseUrl = '/SystemCommon/api/TeamSetup/';
    var dropDwonUrl = '/SystemCommon/api/SystemCommonDDL/';
    var companyID = 1;
    var loggedUser = 0;
    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    $scope.ChemicalSetupID = 0;
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Team Setup';
    $scope.masterID = 0;
    $scope.DetailsID = 0;

    $scope.ToogleDiv = 1;
    $scope.ToogleShowListButtonName = "New";
    
    $scope.TeamDeatils = [];
    $scope.userList = [];
    $scope.ListTeam = [];
//*************** Load User CommonEntity**************************
    function loadUserCommonEntity(num) {
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;
    }
    loadUserCommonEntity(0);
        //**********----Load View List----***************
    function loadRecords_TeamView(isPaging) {

        var apiRoute = baseUrl + 'GetTeam/';
        var processMenues = TeamSetupService.getOrganogram(apiRoute, $scope.UserCommonEntity.loggedCompnyID, $scope.UserCommonEntity.loggedUserID, page, pageSize, isPaging);
        processMenues.then(function (response) {
            $scope.ListTeam = response.data;
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_TeamView(0);
    //******************************Click Event *********************
    //Event Show List 
    $scope.ShowList = function () {
        if ($scope.ToogleDiv == 0) {
            $scope.ToogleShowListButtonName = "Show List";
            $scope.btnSaveUpdateText = "Save";
            $("#rowDetials").show(200);
            $("#teamSetup").show(200);
            $("#teamSetupADD").show(200);
            $("#rowList").hide(200);
            $scope.ToogleDiv = 1;
            $scope.clearMaster();
            $scope.clearDetailsParameter();
            $scope.clearDetails();
        }
        else {
            $scope.ToogleShowListButtonName = "New";
            $scope.btnSaveUpdateText = "Save";
            $("#rowDetials").hide(200);
            $("#teamSetup").hide(200);
            $("#teamSetupADD").hide(200);
            $("#rowList").show(200);
            $scope.ToogleDiv = 0;
        }
        $scope.masterID = 0;
        $scope.DetailsID = 0;
    }
    $scope.ShowList();
        //************************** Load DorpDown ***************************
    function loadRecords_Organogram(isPaging) {
        var apiRoute = dropDwonUrl + 'GetOrganogram/';
        var listOrganograms = TeamSetupService.getOrganogram(apiRoute, $scope.UserCommonEntity.loggedCompnyID, $scope.UserCommonEntity.loggedUserID, page, pageSize, isPaging);
        listOrganograms.then(function (response) {
            $scope.organogramList = response.data; 
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Organogram(0);

    function loadRecords_User(isPaging) {
        var apiRoute =dropDwonUrl+'GetUser/';
        var processListUser = TeamSetupService.getUser(apiRoute, $scope.UserCommonEntity.loggedCompnyID, $scope.UserCommonEntity.loggedUserID, page, pageSize, isPaging);
        processListUser.then(function (response) {
            $scope.userList = response.data; 
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_User(0);
    //************ Reset Form *******************
    $scope.NewInstance = function () {
        $scope.clearMaster();
        $scope.clearDetailsParameter();
        $scope.clearDetails();
        $scope.ToogleDiv = 0;
        $scope.ShowList();
        $scope.ChemicalSetupID = 0;
        $scope.btnSaveUpdateText = "Save";
        loadRecords_TeamView(0);
        $scope.masterID = 0;
        $scope.DetailsID = 0;
    }
    //************ Load User Common Entity **********************
    
    //**********----Clear Details Param----***************
    $scope.clearDetailsParameter = function () {
        $scope.ddlUser = null;
        $("#userDropdown").select2('val', '--Select User--');
        $scope.Sequence = null;
        try {
            $scope.frmTeamSetupAdd.$setPristine();
            $scope.frmTeamSetupAdd.$setUntouched();
        } catch (e) {

        }

    };
    //**********----Clear Master Panel----***************
    $scope.clearMaster = function () {  
        try {
            $scope.frmTeamSetup.$setPristine();
            $scope.frmTeamSetup.$setUntouched();
        } catch (e) {

        }
        $scope.TeamName = "";
        $scope.ddlDepartment = null;
        $("#departmentDropdown").select2('val', '--Select Department--'); 
    };
    //**********----Clear Mater Panel----***************
    $scope.clearDetails = function () {
        $scope.TeamDeatils = [];
    };
   //******************* Clear Master Details ***********************
    $scope.AddDetails = function () {
        var vTeamDetailID = 0;
        var vTeamID=0;
        var vUserID=0;
        var vUserName=0;
        var vSecuence=0;
        var vCompanyID=0;
        var vCreateBy=null;
        var vUpdateBy=null;
        var vDeleteBy = null;
        var vEntityMode = "Inserted";
        var vIsDeleted = false;

        vUserName = $scope.ddlUser.UserFullName;
        vUserID = $scope.ddlUser.UserID
        vSequence = $scope.Sequence;
        vCompanyID = $scope.UserCommonEntity.loggedCompnyID;
        vCreateBy = $scope.UserCommonEntity.loggedUserID;
         
        if ($scope.TeamDeatils.length == 0) vTeamDetailID = 1;
        else
        {
            var MaxValue = 1;
            angular.forEach($scope.TeamDeatils, function (value, key) {
                if (value.TeamDetailID > MaxValue) {
                    MaxValue = value.TeamDetailID;
                }
                vTeamDetailID = MaxValue + 1;
            });
        }
       
        $scope.TeamDeatils
        var detailsObject =
            {
                TeamDetailID: vTeamDetailID,
                TeamID: vTeamID,
                UserID: vUserID,
                UserName: vUserName,
                Sequence: vSequence,
                CompanyID: vCompanyID,
                CreateBy: vCreateBy,
                UpdateBy: vUpdateBy,
                DeleteBy: vDeleteBy,
                EntityMode: vEntityMode,
                IsDeleted: vIsDeleted
            }
        $scope.TeamDeatils.push(detailsObject);
        $scope.clearDetailsParameter();
         

    };
    $scope.EditTeamSetup = function (dataModel) {

        $scope.ToogleDiv = 1;
        $scope.ShowList();
        $scope.btnSaveUpdateText = "Update";
        $scope.ToogleDiv = 0;
        $scope.ShowList();
        $scope.btnSaveUpdateText = "Update";
        
        var list = $scope.ListTeam;
        angular.forEach(list, function (value, key) {
            if (value.TeamID == dataModel.TeamID) {
                $scope.masterID = value.TeamID;
                $scope.TeamName = value.TeamName; 
                $scope.ddlDepartment = value.DepartmentID;
                if (value.DepartmentID != null)
                    $('#departmentDropdown').select2('val', value.DepartmentID.toString());
            }
        });
        // Load Details By ID
        try {
            var apiRoute = baseUrl + 'GetDetailsByMasterID/';
            var processdetails = TeamSetupService.getDetailsByID(apiRoute, companyID, loggedUser, page, pageSize, isPaging, dataModel.TeamID);
            processdetails.then(function (response) {
                $scope.clearDetails();
                $scope.TeamDeatils = response.data; 
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        catch (e) {
        }
       

    }
        //**********----Delete Details ----***************
    $scope.deleteDetailsList = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.UserName + '. Are you sure?');
        if (IsConf) {
            var list = $scope.TeamDeatils;
             
            angular.forEach(list, function (value, key) {
                 
                if (value.TeamDetailID == dataModel.TeamDetailID) {
                     
                    value.IsDeleted = true;
                    if (value.EntityMode == "Updated")
                    {
                        value.EntityMode = "Updated";
                    }
                    else
                    {
                        $scope.TeamDeatils = $scope.TeamDeatils.filter(function (item) {
                            return item.TeamDetailID !== dataModel.TeamDetailID;
                        });
                    }
                    
                }
            });
            //var Sequence = 1;
            //angular.forEach($scope.TeamDeatils, function (value, key) { 
            //        value.Sequence = Sequence++;  
            //});

            console.log($scope.TeamDeatils);
        }
    }
        //**********----Save----***************
    $scope.DeleteTeam = function (dataModel)
    {
        var IsConf = confirm('You are about to delete ' + dataModel.TeamName + '. Are you sure?');
        if (IsConf) {
            var master = {
                TeamID: dataModel.TeamID,
                IsDeleted: true
            };
            var detailsObject =
                       {
                           TeamDetailID: 1,
                           TeamID: 1,
                           UserID: 1,

                           IsDeleted: true
                       }
        }
       
        $scope.TeamDeatils.push(detailsObject);

        var apiRoute = baseUrl + 'DeleteTeam/';
        var porcessMasterDetails = TeamSetupService.postCommonMasterDetails(apiRoute, master, $scope.TeamDeatils, $scope.UserCommonEntity);
        porcessMasterDetails.then(function (response) {

            if (response.data.result == 3) {
                response.data = -101;
                loadRecords_TeamView(0);
                ShowCustomToastrMessage(response);
            }
            else {
                response.data =0;
                ShowCustomToastrMessage(response);
            }
            $scope.TeamDeatils = [];

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.Save = function () {
        debugger
        if ($scope.TeamDeatils.length == 0) {
            Command: toastr["info"]("Please Add Details");
            return;
        }
        var vTeamID = 0;
        var vDepartmentID = 0;
        var vTeamName = "";
        var vCompanyID = 0;
        var vCreateBy = null;
        var vUpdateBy = null;
        var vDeleteBy = null;
        var vEntityMode = "Inserted";
        var vIsDeleted = false;
        if ($scope.masterID > 0) {
            vTeamID = $scope.masterID;
            vEntityMode="Updated"
        }
        
        vTeamName = $scope.TeamName;
        if (angular.isObject($scope.ddlDepartment)) {
            vDepartmentID = $scope.ddlDepartment.OrganogramID;
        }
        else {
            vDepartmentID = $scope.ddlDepartment;
        }
        
        vCompanyID = $scope.UserCommonEntity.loggedCompnyID;
        if (vTeamID == 0) {
            vCreateBy = $scope.UserCommonEntity.loggedUserID;
        }
         
        var master = {
            TeamID: vTeamID,
            TeamName: vTeamName,
            DepartmentID: vDepartmentID,
            CompanyID: vCompanyID,
            CreateBy: vCreateBy,
            UpdateBy: vUpdateBy,
            DeleteBy: vDeleteBy,
            EntityMode: vEntityMode,
            IsDeleted: vIsDeleted
        };

        var details = $scope.TeamDeatils;
        isExisting = 0;
        if (isExisting === 0) {
            debugger
            var apiRoute = baseUrl + 'SaveTeam/';
            var porcessMasterDetails = TeamSetupService.postCommonMasterDetails(apiRoute, master, details, $scope.UserCommonEntity);
            porcessMasterDetails.then(function (response) {
                debugger
                if (response.data.result == 1) {
                    response.data = 1;
                    $scope.NewInstance();
                    ShowCustomToastrMessage(response);
                }
                else if (response.data.result == 2) {
                    response.data = -102;
                    $scope.NewInstance();
                    ShowCustomToastrMessage(response);
                }
                else if (response.data.result == 0) //Erro
                {
                    response.data = 0;
                    ShowCustomToastrMessage(response);
                }
                else {
                    response.data = -1;
                    ShowCustomToastrMessage(response);
                }
                 
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        
    }
}]);