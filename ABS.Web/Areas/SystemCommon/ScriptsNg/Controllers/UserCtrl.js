/*
*    Created By: Shashangka Shekhar;
*    Create Date: 22-5-2016 (dd-mm-yy); Updated Date: 22-5-2016 (dd-mm-yy);
*    Name: 'crudService';
*    Type: $http service;
*    Purpose: Register New member with validaing user input;
*    Service Injected: '$scope', 'crudService', '$http', '$filter';
*/

//app.controller('userCtrl', function ($scope, $http, crudService, $filter) {
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('userCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion',
function ($scope, crudService, uiGridConstants, $q, $http, conversion) {

    $scope.gridOptionsUsers = [];
    var objcmnParam = {};

    $scope.onlyNumbers = /^\d+$/;
    var isExisting = 0;
    var page = 1;
    var pageSize = 20;
    var isPaging = 0;
    var inCallback = false;
    var totalData = 0;

    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();
    $scope.EditUserID = 0;
    $scope.IsOnlineAccount = true;
    $scope.PanelTitle = 'New User';
    $scope.DataPanelTitle = 'Existing User';
    $scope.listUser = [];
    var User = {};
    var OnlineAccount = 0;

    $scope.loaderMore = false;
    $scope.loaderUpload = false;
    $scope.scrollended = false;

    $scope.SelectedFileAvatar = [];
    $scope.SelectedFileSignature = [];

    $scope.signaturename = null;
    $scope.avatarname = null;

    $scope.UserType = 0;
    $scope.UserType = $('#hUserType').val();

    //**********----Get All Record----***************
    $scope.listUserTitle = [
        { TitleId: 1, Title: 'Mr.' },
        { TitleId: 2, Title: 'Mrs.' }
    ];

    $scope.listUserGender = [
        { GenderId: 1, Gender: 'Male' },
        { GenderId: 2, Gender: 'Female' },
        { GenderId: 3, Gender: 'Others' }
    ];

    $scope.listUserReligion = [
         { ReligionId: 1, Religion: 'Islam' },
         { ReligionId: 2, Religion: 'Christianity' },
         { ReligionId: 3, Religion: 'Hinduism' },
         { ReligionId: 4, Religion: 'Buddhism' },
         { ReligionId: 5, Religion: 'Others' }
    ];

    $scope.listBloodGroup = [
       { BloodGroupID: 1, BloodGroup: 'A+ (A-Positive)' },
       { BloodGroupID: 2, BloodGroup: 'O+ (O-Positive)' },
       { BloodGroupID: 3, BloodGroup: 'B+ (B-Positive)' },
       { BloodGroupID: 4, BloodGroup: 'AB+ (AB-Positive)' },
       { BloodGroupID: 5, BloodGroup: 'A- (A-Negative)' },
       { BloodGroupID: 6, BloodGroup: 'O- (O-Negative)' },
       { BloodGroupID: 7, BloodGroup: 'B- (B-Negative)' },
       { BloodGroupID: 8, BloodGroup: 'AB- (AB-Negative)' }
    ];

    $scope.listJobContractType = [
        { JobContractTypeID: 1, JobContractType: 'Permanent' },
        { JobContractTypeID: 2, JobContractType: 'Probational' },
        { JobContractTypeID: 3, JobContractType: 'Contractual' },
        { JobContractTypeID: 4, JobContractType: 'Others' }
    ];

    $scope.listCountry = [];
    $scope.listState = [];
    $scope.listCity = [];
    $scope.listDesignation = [];
    $scope.listDepartment = [];

    $scope.Duplicate = function () {
        if ($scope.IsDuplicate) {
            $scope.Address2 = $scope.Address1;
        }
    };
    $scope.Duplicate1 = function () {
        if ($scope.IsDuplicate1) {
            $scope.Address2_2 = $scope.Address1_1;
        }
    };
    //else {
    //    $scope.Address1 = angular.copy($scope.Address2);
    //}



    function loadLedger(isPaging) {

        objcmnParam = {
            pageNumber: page,
            pageSize: pageSize,
            IsPaging: isPaging,
            loggeduser: LUserID,
            loggedCompany: LCompanyID,
            menuId: 5,
            tTypeId: 25
        };

        var apiRoute = '/SystemCommon/api/ItemGroup/GetLedger/';
        var acc1Id = 1;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(acc1Id) + "]";
        var LedgerList = crudService.GetList(apiRoute, cmnParam);
        LedgerList.then(function (response) {
            $scope.LedgerList = response.data;

        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadLedger(0);


    //**********----Get All Record----***************
    function loadRecords_designation(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetDesignation/' + LCompanyID + '/' + LUserID + '/';
        var ddlCountry = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCountry.then(function (response) {
            $scope.listDesignation = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_designation(0);

    //**********----Get All Record----***************
    function loadRecords_department(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetDepartment/' + LCompanyID + '/' + LUserID + '/';
        var ddlCountry = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCountry.then(function (response) {
            $scope.listDepartment = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_department(0);

    //**********----Get All Record----***************
    function loadRecords_countryforPresent(isPaging) {
        debugger;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCountry/' + LCompanyID + '/' + LUserID + '/';
        var ddlCountry = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCountry.then(function (response) {
            debugger;
            $scope.listCountryforpresentAddress = response.data;

            $scope.ddlCountry = response.data[17].CountryID;
            $("#userCountry").select2("data", { id: response.data[17].CountryID, text: response.data[17].CountryName });

            $scope.loadRecords_stateforPresentAddress(0);
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }
    loadRecords_countryforPresent(0);

    //**********----Get All Record----***************
    $scope.loadRecords_stateforPresentAddress = function (isPaging) {

        var countryId = $scope.ddlCountry;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetState/' + countryId + '/' + LCompanyID + '/' + LUserID + '/';
        var ddlStates = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlStates.then(function (response) {
            $scope.listStateforPresent = response.data
            debugger;
            //$scope.ddlState = response.data[0].StateID;
            //$("#userState").select2("data", { id: response.data[0].StateID, text: response.data[0].StateName });

            //$scope.ddlState1 = response.data[0].StateID;
            //$("#userState1").select2("data", { id: response.data[0].StateID, text: response.data[0].StateName });

            //$scope.loadRecords_cityforPresentAddress(0);
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }

    //**********----Get All Record----***************
    $scope.ngchloadRecords_cityforPresentAddress = function (isPagin) {
        $scope.ddlCity = 0;
        $("#userCity").select2("val", '');
        $scope.loadRecords_cityforPresentAddress(0);
    }
    $scope.loadRecords_cityforPresentAddress = function (isPaging) {
        debugger;

        var stateId = $scope.ddlState;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCity/' + stateId + '/' + LCompanyID + '/' + LUserID + '/';
        var ddlCities = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCities.then(function (response) {
            debugger;
            $scope.listCityforpresentAddress = response.data

            //$scope.ddlCity = response.data[0].CityID;
            //$("#userCity").select2("data", { id: response.data[0].CityID, text: response.data[0].CityName });

            //$scope.ddlCity1 = response.data[0].CityID;
            //$("#userCity1").select2("data", { id: response.data[0].CityID, text: response.data[0].CityName });
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }

    function loadRecords_countryforPermanent(isPaging) {
        debugger;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCountry/' + LCompanyID + '/' + LUserID + '/';
        var ddlCountry = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCountry.then(function (response) {
            debugger;
            $scope.listCountryforpermanent = response.data;

            $scope.ddlCountry1 = response.data[17].CountryID;
            $("#userCountry1").select2("data", { id: response.data[17].CountryID, text: response.data[17].CountryName });

            $scope.loadRecords_stateforPermanentAddress(0);
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }
    loadRecords_countryforPermanent(0);

    $scope.loadRecords_stateforPermanentAddress = function (isPaging) {

        debugger;
        var countryId1 = $scope.ddlCountry1;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetState/' + countryId1 + '/' + LCompanyID + '/' + LUserID + '/';
        var ddlStates = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlStates.then(function (response) {
            $scope.listStateforPermanentAddress = response.data

            //$scope.ddlState = response.data[0].StateID;
            //$("#userState").select2("data", { id: response.data[0].StateID, text: response.data[0].StateName });

            //$scope.ddlState1 = response.data[0].StateID;
            //$("#userState1").select2("data", { id: response.data[0].StateID, text: response.data[0].StateName });

            //   $scope.loadRecords_cityforPermanentAddress(0);
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }
    $scope.ngchloadRecords_cityforPermanentAddress = function (isPaging) {
        $scope.ddlCity1 = 0;
        $("#userCity1").select2("val", '');
        $scope.loadRecords_cityforPermanentAddress(0);
    }
    $scope.loadRecords_cityforPermanentAddress = function (isPaging) {


        var stateId = $scope.ddlState1;
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCity/' + stateId + '/' + LCompanyID + '/' + LUserID + '/';
        var ddlCities = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlCities.then(function (response) {
            $scope.listCityforpremanentAddress = response.data

            //$scope.ddlCity = response.data[0].CityID;
            //$("#userCity").select2("data", { id: response.data[0].CityID, text: response.data[0].CityName });

            //$scope.ddlCity1 = response.data[0].CityID;
            //$("#userCity1").select2("data", { id: response.data[0].CityID, text: response.data[0].CityName });
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }


    //**********----Get All Record----***************
    function loadRecords_grp(isPaging) {
        var apiRoute = '/SystemCommon/api/User/GetUserGroupddl/' + LCompanyID + '/' + LUserID + '/' + $scope.UserType + '/';
        var ddlUserGroup = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlUserGroup.then(function (response) {
            $scope.ListUserGroup = response.data
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }
    loadRecords_grp(0);

    //**********----Get All Record----***************
    function loadRecords_typ(isPaging) {
        var apiRoute = '/SystemCommon/api/User/GetUserTypeddl/' + LCompanyID + '/' + LUserID + '/';
        var ddlUserType = crudService.getAll(apiRoute, page, pageSize, isPaging);
        ddlUserType.then(function (response) {
            $scope.ListUserType = response.data;
        },
        function (error) {
            console.warn("Error: " + error);
        });
    }
    //loadRecords_typ(0);

    //Pagination
    $scope.pagination = {
        paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
        ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

        getTotalPages: function () {
            return Math.ceil(this.totalItems / this.pageSize);
        },
        pageSizeChange: function () {
            if (this.ddlpageSize == "All")
                this.pageSize = $scope.pagination.totalItems;
            else
                this.pageSize = this.ddlpageSize

            this.pageNumber = 1
            $scope.loadRecordsUser(1);
        },
        firstPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber = 1
                $scope.loadRecordsUser(1);
            }
        },
        nextPage: function () {
            if (this.pageNumber < this.getTotalPages()) {
                this.pageNumber++;
                $scope.loadRecordsUser(1);
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.loadRecordsUser(1);
            }
        },
        lastPage: function () {
            if (this.pageNumber >= 1) {
                this.pageNumber = this.getTotalPages();
                $scope.loadRecordsUser(1);
            }
        }
    };

    //**********----Get All Record----***************
    $scope.loadRecordsUser = function (isPaging) {

        // For Loading
        $scope.loaderMore = true;
        $scope.lblMessage = 'loading please wait....!';
        $scope.result = "color-red";

        //Ui Grid
        objcmnParam = {
            pageNumber: $scope.pagination.pageNumber,
            pageSize: $scope.pagination.pageSize,
            IsPaging: isPaging,
            loggeduser: LUserID,
            loggedCompany: LCompanyID,
            menuId: 5,
            tTypeId: 25,
            UserType: $scope.UserType
        };

        $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
            if (col.filters[0].term) {
                return 'header-filtered';
            } else {
                return '';
            }
        };

        $scope.gridOptionsUsers = {
            columnDefs: [
                { name: "UserID", displayName: "User ID", width: '7%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "CustomCode", displayName: "CustomCode", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                { name: "UserFullName", title: "User Name", headerCellClass: $scope.highlightFilteredHeader },
                { name: "Designation", title: "Designation", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "Department", title: "Department", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "UserType", title: "User Type", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                { name: "GroupName", title: "Group", width: '12%', headerCellClass: $scope.highlightFilteredHeader },
                {
                    name: 'Edit',
                    displayName: "Edit",
                    width: '7%',
                    enableColumnResizing: false,
                    enableFiltering: false,
                    enableSorting: false,
                    headerCellClass: $scope.highlightFilteredHeader,
                    cellTemplate: '<span class="label label-warning label-mini">' +
                                        '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                                            '<i class="icon-pencil" ng-click="grid.appScope.getUser(row.entity)"></i>' +
                                        '</a>' +
                                    '</span>' +
                                    '<span class="label label-danger label-mini">' +
                                        '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                            '<i class="icon-trash"></i>' +
                                        '</a>' +
                                    '</span>'
                }
            ],

            enableFiltering: true,
            enableGridMenu: true,
            enableSelectAll: true,
            exporterCsvFilename: 'Users.csv',
            exporterPdfDefaultStyle: { fontSize: 9 },
            exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "User", style: 'headerStyle' },
            exporterPdfFooter: function (currentPage, pageCount) {
                return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
            },
            exporterPdfCustomFormatter: function (docDefinition) {
                docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
                docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                return docDefinition;
            },
            exporterPdfOrientation: 'portrait',
            exporterPdfPageSize: 'LETTER',
            exporterPdfMaxGridWidth: 500,
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        };

        var apiRoute = '/SystemCommon/api/User/GetUser/';
        var listAllUser = crudService.getAllUsers(apiRoute, objcmnParam);
        listAllUser.then(function (response) {
            $scope.pagination.totalItems = response.data.recordsTotal;
            $scope.gridOptionsUsers.data = response.data.listUsers;
            $scope.loaderMore = false;
            //$scope.listUser = response.data
        },
        function (error) {
            console.log("Error: " + error);
        });
    };
    $scope.loadRecordsUser(0);

    //File Upload
    $scope.selectFileAvatar = function (element) {
        debugger
        if (element.files[0] == undefined) return;
        for (var i = 0; i < element.files.length; i++) {
            $scope.SelectedFileAvatar.push(element.files[i])
        }

        var formData = new FormData();
        //formData.append("file", $scope.SelectedFileFinger[0]);
        for (var i in $scope.SelectedFileAvatar) {
            formData.append("uploadedFile", $scope.SelectedFileAvatar[i]);
        }

        var defer = $q.defer();
        $http.post("/SystemCommon/api/User/UploadAvatar/", formData,
            {

                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (response) {
            debugger
            $scope.avatarname = response

            //Preview Image
            var reader = new FileReader();
            reader.onload = function (event) {
                debugger
                $scope.avatar_source = event.target.result;
                $scope.$apply();
                document.getElementById("fileAvatar").disabled = true;
            }
            // when the file is read it triggers the onload event above.
            reader.readAsDataURL(element.files[0]);
            //defer.resolve(response)
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });
        //console.log(defer.promise);
    }
    $scope.selectFileSignature = function (element) {

        if (element.files[0] == undefined) return;
        for (var i = 0; i < element.files.length; i++) {
            $scope.SelectedFileSignature.push(element.files[i])
        }

        var formData = new FormData();
        //formData.append("file", $scope.SelectedFileFinger[0]);
        for (var i in $scope.SelectedFileSignature) {
            formData.append("uploadedFile", $scope.SelectedFileSignature[i]);
        }

        var defer = $q.defer();
        $http.post("/SystemCommon/api/User/UploadSignature/", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (response) {
            $scope.signaturename = response

            //Preview Image
            var reader = new FileReader();
            reader.onload = function (event) {
                $scope.signature_source = event.target.result;
                $scope.$apply();
                document.getElementById("fileSignature").disabled = true;
            }
            // when the file is read it triggers the onload event above.
            reader.readAsDataURL(element.files[0]);
            //defer.resolve(response)
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });
        //console.log(defer.promise);
    }

    $scope.clearFileAvatar = function () {
        if ($scope.avatarname != null) {
            var fileDetails = {
                ImageUrl: $scope.avatarname
            }
            var apiRoute = '/SystemCommon/api/User/DeleteAvatar/';
            var fileDelete = crudService.post(apiRoute, fileDetails);
            fileDelete.then(function (response) {
                $scope.avatar_source = '';
                $('#fileAvatar').val('');
                $scope.SelectedFileAvatar = [];
                $scope.avatarname = null;
                fileDetails = {};
                document.getElementById("fileAvatar").disabled = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }
    $scope.clearFileSignature = function () {
        if ($scope.signaturename != null) {
            var fileDetails = {
                SignatUrl: $scope.signaturename
            }
            var apiRoute = '/SystemCommon/api/User/DeleteSignature/';
            var fileDelete = crudService.post(apiRoute, fileDetails);
            fileDelete.then(function (response) {
                $scope.signature_source = '';
                $('#fileSignature').val('');
                $scope.SelectedFileSignature = [];
                $scope.signaturename = null;
                fileDetails = {};
                document.getElementById("fileSignature").disabled = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Create New Record----***************
    $scope.save = function () {
        debugger
        if ($scope.IsOnlineAccount)
            OnlineAccount = 1;

        //create user object
        User = {
            CompanyID: LCompanyID,
            LoggedUser: LUserID,
            UserID: $scope.EditUserID,

            //Login Account
            LoginID: $scope.LoginID,
            LoginEmail: $scope.Email,
            LoginPhone: $scope.Phone,

            //User Type
            //UserTypeID: $scope.ddlUserType,
            UserTypeID: $scope.UserType,
            UserGroupID: $scope.ddlUserGroup,
            UserTitleID: $scope.ddlUsertitle,


            //User Info
            UserFirstName: $scope.FirstName,
            UserMiddleName: $scope.MiddleName,
            UserLastName: $scope.LastName,
            GenderID: $scope.ddlGender,
            ReligionID: $scope.ddlUserReligion,

            FathersName: $scope.FatherName,
            MothersName: $scope.MotherName,
            SpouseNane: $scope.SpouseName,

            // Present Address
            //User Contact1
            ParAddress1: $scope.ParAddress1,
            ParAddress2: $scope.ParAddress2,
            ParCountryID: $scope.ddlCountry,
            ParStateID: $scope.ddlState,
            ParCityID: $scope.ddlCity,

            //Permanent Address
            //User Contact2 //Newly Added
            PreAddress1: $scope.PreAddress1,
            PreAddress2: $scope.PreAddress2,
            PreCountryID: $scope.ddlCountry1,
            PreStateID: $scope.ddlState1,
            PreCityID: $scope.ddlCity1,

            //User Identity
            UniqueIdentity: '',
            //BloodGroup: $scope.ddlBloodGroup.BloodGroupID,
            BloodGroup: $scope.ddlBloodGroup.BloodGroup,

            Height: $scope.Height == "" ? 0.0 : $scope.Height,
            DOB: $scope.DOB == "" ? "1/1/1900" : conversion.getStringToDate($scope.DOB),
            PassportNO: $scope.PassportNo,
            NID: $scope.NationalID,

            ImageUrl: $scope.avatarname,
            FingerUrl: '',
            SignatUrl: $scope.signaturename,

            //User Job Contract
            DesignationID: $scope.ddlDesignation,
            DepartmentID: $scope.ddlDepartment,
            JobContractTypeID: $scope.ddlContractType,


            // Ledger
            AcDetailID: $scope.ngmLedger,

            //Open User Login Account
            IsOnlineAccount: OnlineAccount
        };

        console.log(User);

        //-------Defult Company ------------

        $scope.defultCompanylist = [];

        for (i = 0; i < $scope.SelectCompnayList.length; i++) {
            var bl = false;
           debugger
            if ($scope.companylist[i].id == $scope.defultCompany) {
                    bl = true;
            }
            else
            {
                bl = false;
            }

            if (bl) {
                obj = {
                    id: $scope.companylist[i].id,
                    label: $scope.companylist[i].label,
                    IsDefult: true
                }
                
            } else {
                obj = {
                    id: $scope.companylist[i].id,
                    label: $scope.companylist[i].label,
                    IsDefult: false
                }
            }
            $scope.defultCompanylist.push(obj);
            
        }


        var PmCompany = $scope.defultCompanylist;


        debugger;
        var apiRoute = '/SystemCommon/api/User/SaveUser/';
        var UserCreate = crudService.postList(apiRoute, User, PmCompany);
        UserCreate.then(function (response) {
            if (response.data === 1) {
                Command: toastr["info"]("User Saved Successfully!!!!");
                modal_fadeOut();
                $scope.loadRecordsUser(0);
                $scope.clear();
            }
            else if (response.data === 0) {
                    Command: toastr["error"]("Error while Creating User!!!!");
            }
            else {
                Command: toastr["error"]("Error Undefined!!!!");
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    //******=========Get Single Data=========******
    $scope.getUser = function (dataModel) {
        debugger;
        $scope.PanelTitle = 'Update User';
        $scope.EditUserID = dataModel.UserID;

        var apiRoute = '/SystemCommon/api/User/GetUserByID/' + dataModel.UserID + '/' + LCompanyID + '/' + LUserID;
        var UserDetails = crudService.getByID(apiRoute);
        UserDetails.then(function (response) {
            if (response.data != null) {
                console.log(response.data);

                $scope.ddlUserType = '';

                $scope.ddlUserGroup = response.data.UserGroupID;
                $("#userGroup").select2("data", { id: response.data.UserGroupID, text: response.data.UserGroup });

                $scope.LoginID = response.data.LoginID;
                $scope.Email = response.data.Email;
                $scope.Phone = response.data.LoginPhone;

                $scope.ddlUsertitle = response.data.UserTitleID;
                $("#userTitle").select2("data", { id: response.data.UserTitleID, text: response.data.UserTitle });

                $scope.FirstName = response.data.UserFirstName;
                $scope.MiddleName = response.data.UserMiddleName;
                $scope.LastName = response.data.UserLastName;

                $scope.ddlGender = response.data.GenderID;
                $("#userGender").select2("data", { id: response.data.GenderID, text: response.data.Gender });

                $scope.ddlUserReligion = response.data.ReligionID;
                $("#userReligion").select2("data", { id: response.data.GenderID, text: response.data.Religion });

                //$scope.DOB = response.data.DOB;
                if (response.data.DOB != null) {
                    $scope.DOB = conversion.getDateToString(response.data.DOB);
                }

                $scope.FatherName = response.data.FathersName;
                $scope.MotherName = response.data.MothersName;
                $scope.SpouseName = response.data.SpouseNane;
                $scope.Height = response.data.Height;

                $scope.ddlBloodGroup = response.data.BloodGroupID;
                $("#bloodGroup").select2("data", { id: response.data.BloodGroupID, text: response.data.BloodGroup });

                $scope.PassportNo = response.data.PassportNO;
                $scope.NationalID = response.data.NID;
                $scope.PreAddress1 = response.data.PreAddress1;
                $scope.PreAddress2 = response.data.PreAddress2;
                $scope.ParAddress1 = response.data.ParAddress1;
                $scope.ParAddress2 = response.data.ParAddress2;
                debugger;
                //--------------Present Address----------
                loadRecords_countryforPresent(0);
                $scope.ddlCountry = response.data.ParCountryID;
                $("#userCountry").select2("data", { id: response.data.ParCountryID, text: response.data.ParCountryName });

                $scope.loadRecords_stateforPresentAddress(0);
                $scope.ddlState = response.data.ParStateID;
                $("#userState").select2("data", { id: response.data.ParStateID, text: response.data.ParStateName });

                $scope.loadRecords_cityforPresentAddress(0);
                $scope.ddlCity = response.data.ParCityID;
                $("#userCity").select2("data", { id: response.data.ParCityID, text: response.data.ParCityName });

                //-------------Permanent Address-----------------------------
                debugger;
                loadRecords_countryforPermanent(0);
                $scope.ddlCountry1 = response.data.PreCountryID;
                $("#userCountry1").select2("data", { id: response.data.PreCountryID, text: response.data.PreCountryName });

                $scope.loadRecords_stateforPermanentAddress(0);
                $scope.ddlState1 = response.data.PreStateID;
                $("#userState1").select2("data", { id: response.data.PreStateID, text: response.data.PreStateName });

                $scope.loadRecords_cityforPermanentAddress(0);
                $scope.ddlCity1 = response.data.PreCityID;
                $("#userCity1").select2("data", { id: response.data.PreCityID, text: response.data.PreCityName });


                debugger;

                // $scope.Zip = response.data.Zip;
                $scope.ddlDepartment = response.data.DepartmentID;
                if (response.data.ImageUrl == "../../Content/img/noavatar-male.png") {
                    document.getElementById("fileAvatar").disabled = false;
                    $scope.avatar_source = response.data.ImageUrl;
                    $scope.avatarname = null;

                } else {
                    document.getElementById("fileAvatar").disabled = true;
                    $scope.avatar_source = response.data.ImageUrl;
                    $scope.avatarname = response.data.ImageName;

                }
                if (response.data.SignatUrl == "../../Content/img/signature.png") {
                    document.getElementById("fileSignature").disabled = false;
                    $scope.signature_source = response.data.SignatUrl;
                    $scope.signaturename = null;
                } else {
                    document.getElementById("fileSignature").disabled = true;
                    $scope.signature_source = response.data.SignatUrl;
                    $scope.signaturename = response.data.SignatName;
                }


                $("#userdepartment").select2("data", { id: response.data.DepartmentID, text: response.data.Department });

                $scope.ddlContractType = response.data.JobContractTypeID;
                $("#userContractType").select2("data", { id: response.data.JobContractTypeID, text: response.data.JobContractType });

                $scope.ddlDesignation = response.data.DesignationID;
                $("#userDesignation").select2("data", { id: response.data.DesignationID, text: response.data.Designation });

                $scope.ngmLedger = response.data.AcDetailID;
                $("#ddlLedger").select2("data", { id: response.data.AcDetailID, text: response.data.ACName });


                $scope.frmUser.$setPristine();
                $scope.frmUser.$setUntouched();
                debugger;
                $scope.GetCompanyPermissionListByuserID($scope.EditUserID);
            }
            else { Command: toastr["error"]("Error while Loading!!!!"); }
        }, function (error) {
            console.log("Error: " + error);
        });
    };

    $scope.GetCompanyPermissionListByuserID = function (UserID) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompanyPermissionListByUserID/' + UserID;
        var company = crudService.getByID(apiRoute);
        company.then(function (response) {
            $scope.SelectCompnayList = response.data;
            $scope.selectedList = response.data;

            debugger;
            for (i = 0; i < $scope.selectedList.length; i++) {
                if ($scope.selectedList[i].IsDefult == true) {
                    $scope.defultCompany = $scope.selectedList[i].id;
                    $("#defultCompany").select2("data", { id: $scope.selectedList[i].id, text: $scope.selectedList[i].label });

                }
            }
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    //**********----Delete Single Record----***************
    $scope.delete = function (dataModel) {
        var IsConf = confirm('You are about to delete ' + dataModel.UserFullName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/SystemCommon/api/User/DeleteUser/' + dataModel.UserID + '/' + LCompanyID + '/' + LUserID;
            var UserDelete = crudService.delete(apiRoute);
            UserDelete.then(function (response) {
                if (response.data === 1) { Command: toastr["info"]("User Deleted Successfully!!!!"); }
                else { Command: toastr["error"]("Error while Deleting!!!!"); }
                $scope.loadRecordsUser(0);
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }
    $scope.GetCompanys = function () {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompanyForMutl/';
        var _company = crudService.getAll(apiRoute, page, pageSize, isPaging);
        _company.then(function (response) {
            $scope.SelectCompnayList = [];
            $scope.companylist = response.data;
            // $scope.ListMultiProcessModel = item;     
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    $scope.GetCompanys();

    $scope.clear = function () {
        debugger;
        $scope.frmUser.$setPristine();
        $scope.frmUser.$setUntouched();

        $scope.PanelTitle = 'New User';
        $scope.UserGroupID = 0;
        $scope.GroupName = '';
        $scope.LoginID = '';
        $scope.FirstName = '';
        $scope.MiddleName = '';
        $scope.LastName = '';
        $scope.Email = '';
        $scope.Phone = '';
        $scope.FatherName = '';
        $scope.MotherName = '';
        $scope.SpouseName = '';
        $scope.Address1 = '';
        $scope.Address2 = '';
        $scope.Address1_1 = '';
        $scope.Address2_2 = '';
        $scope.Height = '';
        $scope.DOB = '';
        $scope.PassportNo = '';
        $scope.NationalID = '';

        $("#userGender").select2("val", '');
        $scope.ddlGender = 0;

        $("#userReligion").select2("val", '');
        $scope.ddlUserReligion = 0;

        $("#userType").select2("val", '');
        $scope.ddlUserType = 0;

        $("#userGroup").select2("val", '');
        $scope.ddlUserGroup = 0;

        $("#userTitle").select2("val", '');
        $scope.ddlUsertitle = 0;

        $("#bloodGroup").select2("val", '');
        $scope.ddlBloodGroup = 0;

        //$("#userState").select2("val", '');
        //$scope.ddlState = 0;

        //$("#userCity").select2("val", '');
        //$scope.ddlCity = 0;

        $("#userdepartment").select2("val", '');
        $scope.ddlDepartment = 0;

        $("#userContractType").select2("val", '');
        $scope.ddlContractType = 0;

        $("#userDesignation").select2("val", '');
        $scope.ddlDesignation = 0;

        $("#ddlLedger").select2("val", '');
        $scope.ngmLedger = 0;
        $scope.SelectCompnayList = [];
        $scope.signature_source = "../../Content/img/signature.png";
        $scope.avatar_source = "../../Content/img/noavatar-male.png";
        $scope.avatarname = null;
        $scope.signaturename = null;
        debugger;
        $scope.PreAddress1 = "";
        $scope.PreAddress2 = "";
        $scope.ParAddress1 = "";
        $scope.ParAddress2 = "";

        //------Present Address-------       

        $scope.loadRecords_stateforPresentAddress(0);
        $scope.ddlState = 0;
        $("#userState").select2("val", '');

        $scope.loadRecords_cityforPresentAddress(0);
        $scope.ddlCity = 0;
        $("#userCity").select2("val", '');

        //-------Permanent Address------

        $scope.loadRecords_stateforPermanentAddress(0);
        $scope.ddlState1 = 0;
        $("#userState1").select2("val", '');

        $scope.loadRecords_cityforPermanentAddress(0);
        $scope.ddlCity1 = 0;
        $("#userCity1").select2("val", '');

        $scope.defultCompany = 0;
        $("#defultCompany").select2("val", '');
    };

    $scope.AddDefultCompany = function () {
        debugger;
        $scope.SelectCompnayList;
        $scope.companylist;
        $scope.selectedList = [];
        for (i = 0; i < $scope.companylist.length; i++) {
            var bl = false;
            for (j = 0; j < $scope.SelectCompnayList.length; j++) {
                if ($scope.companylist[i].id == $scope.SelectCompnayList[j].id) {
                    bl = true;
                }
            }

            if (bl) {
                obj = {
                    id: $scope.companylist[i].id,
                    label: $scope.companylist[i].label,
                }
                $scope.selectedList.push(obj);
            }
        }

    }

}]);

function modal_fadeOut() {
    $("#userModal").fadeOut(200, function () {
        $('#userModal').modal('hide');
    });
}