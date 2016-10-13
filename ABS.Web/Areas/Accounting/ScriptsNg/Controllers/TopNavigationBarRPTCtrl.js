/// <reference path="../Service/CrudService.js" />
/// <reference path="../Service/CrudService.js" />
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(false);
}]);
app.controller('TopNavigationBarCtrl', ['$scope', '$localStorage', 'sideNavService', '$rootScope', '$window',
    function ($scope, $localStorage, sideNavService, $rootScope, $window) {
        //***************** ComapnyDropDownNotify *****************************
         
        $scope.ListUserWiseCompany = [];
        $scope.sessionUserCompanyID = 0;
        $scope.sessionLoggedUserID = 0;

        $scope.sessionUserCompanyID = $('#hCompanyID').text();
        $scope.sessionLoggedUserID = $('#hUserID').text();
        
        $scope.sessionUserCompanyName = "";
        $scope.ShowCompanyList = true;
        $scope.ShowCompanyChange = false;
        //$rootScope.CompanyFullName = "ABC";
        var ComapnyName = "AMBER LIFE";

        function SetCompanyName(ComapnyName) {
            //var s = ComapnyName,
            //a = s.split(' '),
            //l = a.length,
            //i = 0,
            //result = "";
            //for (; i < l; ++i) {
            //    result += a[i].charAt(0);
            //}
            //$scope.sessionUserCompanyName = result;
            $scope.sessionUserCompanyName = "ADML";
        }
        function loadRecords_UserWiseCompany(dataModuleID) {
            var userID = $scope.sessionLoggedUserID;
            var companyID = $scope.sessionUserCompanyID;
            var loggedUser = $scope.sessionLoggedUserID;
            var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetUserCompany/';
            //var processMenues = sideNavService.GetSideMenu(apiRoute, userID, companyID, loggedUser);
            var processMenues = sideNavService.GetSideMenu(apiRoute, 1, 1, 1);
            processMenues.then(function (response) {
                $scope.ListUserWiseCompany = response.data;
                $rootScope.CompanyFullName = $('#hCompanyName').val();
                SetCompanyName($('#hCompanyName').val());
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_UserWiseCompany(0);
        $scope.ChangeCompany = function (data) {
            var CompanyID = data.CompanyID;
            var CompanyName = data.CompanyName;
            var apiRoute = '/SystemCommon/Dashboard/' + 'ModifyCompanySession/' + CompanyID;
            var porcessMasterDetails = sideNavService.getByID(apiRoute);
            porcessMasterDetails.then(function (response) {
                SetCompanyName(data.CompanyName);
                $rootScope.CompanyFullName = data.CompanyName;
                $window.location.reload();
            },
           function (error) {
               console.log("Error: " + error);
           });
        }
        //********** Get TopMenus
         
        $scope.TopMenues = [];
        $scope.BaseName = "";
        $scope.BaseUrl = "";
        var requestedUrl = window.location.pathname;
        var requestedUrl = window.location.pathname;
        var moduleName = requestedUrl.toLowerCase().split('/')[1].toLowerCase();
        var modulepath = '/' + moduleName + '/' + 'dashboard';
         
        if (moduleName == 'areas')
        {
            moduleName = requestedUrl.toLowerCase().split('/')[2].toLowerCase();
            modulepath = '/' + moduleName + '/' + 'dashboard';
        }
        function loadRecords_Top(dataModuleID) {
            $.ajax({
                method: "get",
                 url: '/SystemCommon/api/SystemCommonLayout/GetTopMenu/' + $scope.sessionUserCompanyID + '/' + $scope.sessionLoggedUserID + '/' + $scope.sessionUserCompanyID,
              //  url: '/SystemCommon/api/SystemCommonLayout/GetTopMenu/' + 1 + '/' + 1 + '/' + 1,
                async: false,
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                     
                    $scope.TopMenues = data;
                    angular.forEach($scope.TopMenues, function (value, key) {
                         
                        if (value.ModulePath.toLowerCase() == modulepath.toLowerCase()) {
                             
                            $scope.BaseName = value.ModuleName;
                            $scope.BaseUrl = value.ModulePath;
                        }

                    });
                    //responseData = data;
                },
                headers: '1'
            });
        }
        loadRecords_Top(0);
    }]);
