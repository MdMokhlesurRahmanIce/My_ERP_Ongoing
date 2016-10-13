
/**
 * CustomerCtrl.js
 */
app.controller('tokenCtrl', ['$scope', '$http', '$sessionStorage', '$localStorage', '$rootScope',
function ($scope, $http, tokenService, $sessionStorage, $localStorage, $rootScope) {
    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();
    var IP = $('#hClientIP').val();

    $scope.tokenManager = {
        generateSecurityToken: function (generateSecurityParam) {
            var model = {
                username: LUserID,
                key: generateSecurityParam.methodtype,
                ip: $('#hClientIP').val(),
                userAgent: navigator.userAgent.replace(/ \.NET.+;/, '')
            };
            var message = [model.username, model.ip, model.userAgent].join(':');
            var hash = CryptoJS.HmacSHA256(message, model.key);
            var token = CryptoJS.enc.Base64.stringify(hash);
            var tokenId = [model.username, model.key, generateSecurityParam.MenuID, generateSecurityParam.CompanyID].join(':');
            var tokenGenerated = CryptoJS.enc.Utf8.parse([token, tokenId].join(':'));
            var previousReturn = CryptoJS.enc.Base64.stringify(tokenGenerated);
            return previousReturn;
        },
    };

    $scope.menuManager = {
        LoadPageMenu: function (queryPath) {
            var responseData = null;
            var isDashboard = true;
            var DashboardControllerName = 'dashboard';
            var requestedUrl = window.location.pathname;
            try {
                var urlArray = requestedUrl.split("/");
                var controllerName = urlArray[2];
                if (controllerName.toLowerCase() != DashboardControllerName) isDashboard = false;

            } catch (e) {

            }

            var headerToken = '1';
            $.ajax({
                method: "post",
                url: '/SystemCommon/api/SystemCommonLayout/GetMenuPermission/',
                data: "[" + JSON.stringify({ MenuPath: requestedUrl, loggedCompanyID: LCompanyID, loggedUserID: LUserID }) + "]",
                async: false,
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    responseData = data;
                    console.log(responseData);
                },
                headers: headerToken
            });
            var retrunObject = {};
            if (isDashboard) {
                angular.forEach(responseData, function (value, key) {
                    retrunObject.loggedCompnyID = LCompanyID;
                    retrunObject.loggedUserID = LUserID;
                    retrunObject.loggedUserBranchID = 0;
                    retrunObject.loggedUserDepartmentID = 0;
                    retrunObject.currentModuleID = value.ModuleID;
                    retrunObject.currentMenuID = value.MenuID;
                    retrunObject.currentTransactionTypeID = 0;
                    retrunObject.TransactionTypeName = "";
                    retrunObject.MenuList = responseData;
                    retrunObject.ChildMenues = value.ChildMenues;
                    retrunObject.EnableView = value.EnableView;
                    retrunObject.EnableInsert = value.EnableInsert;
                    retrunObject.EnableUpdate = value.EnableUpdate;
                    retrunObject.EnableDelete = value.EnableDelete;

                });
            }
            else {
                angular.forEach(responseData, function (value, key) {
                    if (value.ChildMenues.length > 0) {
                        angular.forEach(value.ChildMenues, function (childMenuValue, childMenuKey) {
                            retrunObject.loggedCompnyID = LCompanyID;
                            retrunObject.loggedUserID = LUserID;
                            retrunObject.loggedUserBranchID = childMenuValue.DepartmentID;
                            retrunObject.loggedUserDepartmentID = childMenuValue.DepartmentID;
                            retrunObject.currentModuleID = childMenuValue.ModuleID;
                            retrunObject.currentMenuID = childMenuValue.MenuID;
                            retrunObject.currentTransactionTypeID = childMenuValue.TransactionTypeID;
                            retrunObject.TransactionTypeName = childMenuValue.TransactionTypeName;
                            retrunObject.MenuList = responseData;
                            retrunObject.ChildMenues = value.ChildMenues;
                            retrunObject.EnableView = childMenuValue.EnableView;
                            retrunObject.EnableInsert = childMenuValue.EnableInsert;
                            retrunObject.EnableUpdate = childMenuValue.EnableUpdate;
                            retrunObject.EnableDelete = childMenuValue.EnableDelete;
                        });
                    }

                    //if (value.EnableDelete) {
                    //}
                });

            }
            return retrunObject;
        }
    }

}]);