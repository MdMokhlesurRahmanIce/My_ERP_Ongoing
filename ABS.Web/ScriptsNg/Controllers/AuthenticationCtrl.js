/**
* UserCtrl.js
*/ 

app.controller('loginCtrl', ['$scope', 'userService', '$window',
    function ($scope, userService, $window) {
        $scope.IsVisible = true; $scope.IsLogged = false; $scope.success = false; $scope.error = false;
        //**********----Create New Record----***************
        $scope.submitLogin = function () {
            var AuthenticateUser = {
                UserLogin: $scope.UserLogin,
                Password: $scope.Password
            };
            $scope.lblrmessage = 'Authenticating....';
            if (AuthenticateUser != null) {
                var urlLogin = '/Account/Login/';
                var LoginUser = userService.post(urlLogin, AuthenticateUser);
                LoginUser.then(function (response) {
                    if (response.data.status === 1) {
                        $scope.IsVisible = false;
                        $scope.IsLogged = true;
                        $scope.success = true;
                        $scope.lblrmessage = 'Login Successful. Redirecting....';
                        $window.location = '/Home';
                        //Command: toastr["info"]("Login Successful, Redirecting please wait....!");
                    }
                    else if (response.data.status === 0) {
                        $scope.IsVisible = true;
                        $scope.IsLogged = true;
                        $scope.error = true;
                        $scope.lblrmessage = 'The login is invalid! Please re-enter....';
                        Command: toastr["warning"]("The login is invalid! Please re-enter....");
                    }
                    else {
                        Command: toastr["error"]("Account Not Found!");
                    }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        };

        //**********----Create New Record----***************
        $scope.submitRecovery = function () {
            debugger;
            var RecoverUser = {
                RecoverEmail: $scope.RecEmail
            };

            if (RecoverUser != null) {
                var url = '/Account/Recover/';
                var Recovery = userService.post(url, RecoverUser);
                Recovery.then(function (response) {
                    if (response.data.status === 1) {

                        Command: toastr["success"]("Great. We have sent you an email!");
                    }
                    else if (response.data.status === 0) {

                            Command: toastr["error"]("Account Not Found!");
                    }
                    else {
                        Command: toastr["error"]("Account Not Found!");
                    }
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
        }
    }]);


