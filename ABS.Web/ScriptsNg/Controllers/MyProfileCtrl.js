app.controller('myProfileCtrl', ['$scope','crdMyProfileSerive',
function ($scope, crdMyProfileSerive) {
    
        $scope.PasswordChageTitle = "Change Password";
        var LUserID = $('#hUserID').val();
        var LCompanyID = $('#hCompanyID').val();

        $scope.ShowPasswordChangeModal = function () {         
            debugger;
            $("#modalPasswordChange").fadeIn(200, function () { $('#modalPasswordChange').modal('show'); });
        }

        $scope.CheckCurrentPassword=function()
        {
            var apiRoute = '/SystemCommon/api/User/GetCurrentUserPassword/';
            var _getPassword = crdMyProfileSerive.getCurrentUserPassword(apiRoute, LCompanyID, LUserID);
            _getPassword.then(function (response) {            
                $scope.currentPassword = response.data;
                if( $scope.currentPassword==$scope.txtCurrentPassword)
                {

                }
                else {
                    $scope.txtCurrentPassword = "";
                    Command: toastr["error"]("Your Current Password Not Match");
                }

                // $scope.ListMultiProcessModel = item;     
            },
            function (error) {
                console.log("Error: " + error);
            });

        }

        $scope.CheckNewConf=function()
        {
            if ($scope.txtbxNewPassword == $scope.txtbxConfirmpassword)
            {

            }else{
                $scope.txtbxNewPassword = "";
                $scope.txtbxConfirmpassword = "";
                Command: toastr["error"]("Your New Password And Confirm Password Not Match");
            }
        }

        $scope.ChangePaswordSave=function()
        {
            UserUserAuthenticationGroup = {
                UserID: LUserID,
                Password:$scope.txtbxNewPassword,
                UpdateBy: LUserID                
            };

            var apiRoute = '/SystemCommon/api/User/ChangePassword/';
            var CreateUserGroup = crdMyProfileSerive.post(apiRoute, UserUserAuthenticationGroup);
            CreateUserGroup.then(function (response) {

                if (response.data === 1) { Command: toastr["info"]("Password Changed Successfully!!!!"); }                        
                $scope.clear();
                $("#modalPasswordChange").fadeIn(200, function () { $('#modalPasswordChange').modal('hide'); });
            },
            function (error) {
                console.log("Error: " + error);
            });

        }

        $scope.clear=function()
        {
            $scope.txtCurrentPassword = "";
            $scope.txtbxNewPassword = "";
            $scope.txtbxConfirmpassword = "";
        }
    }]);