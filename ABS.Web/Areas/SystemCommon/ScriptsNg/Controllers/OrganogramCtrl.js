
app.controller('OrganogramCtrl', function ($scope, crudService, menuService) {

    var isExisting = 0;
    var page = 1;
    var pageSize = 100;
    var isPaging = 0;
    var totalData = 0;

    var LoggedUserID = $('#hUserID').val();
    var LoggedCompanyID = $('#hCompanyID').val();
    
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create Organogram';
    $scope.ListTitle = 'Organogram Records';
    $scope.OrganogramID = 0;
    $scope.ListCompany = [];
    $scope.ListStatus = [];
    $scope.ListParentOrganogram = [];
    $scope.ListOrganogram = [];

    //**********----Get Company DropDown On Page Load----***************
    function loadRecords_Company(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetCompany/';
        var listCompany = menuService.GetCompanies(apiRoute, page, pageSize, isPaging);
        listCompany.then(function (response) {
            $scope.ListCompany = response.data  //Set Default 
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
            $("#statusDropDown").select2("data", { id: $scope.ListStatus[0].StatusID, text: $scope.ListStatus[0].StatusName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Status(0);

    //*******************organogram  parent DropDown On Page Load-- ***********
    function loadRecords_Organogram(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
        var listOrganograms = menuService.GetDrpOrganograms(apiRoute, LoggedUserID, LoggedCompanyID, page, pageSize, isPaging);
        listOrganograms.then(function (response) {
            $scope.ListParentOrganogram = response.data
            //$("#parentDropDown").select2("data", { id: $scope.ListParentOrganogram[0].OrganogramID, text: $scope.ListParentOrganogram[0].OrganogramName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    loadRecords_Organogram(0);

    //**********----Get All Record----***************
    function loadRecords_OrganogramAll(isPaging) {
        var apiRoute = '/SystemCommon/api/SystemCommonDDL/GetOrganogram/';
        var listorganogram = menuService.GetDrpOrganogram(apiRoute, page, pageSize, isPaging);
        listorganogram.then(function (response) {
            $scope.ListOrganogram = response.data
             
        },
        function (error) {
            console.log("Error: " + error);
        });
    }
    //loadRecords_OrganogramAll(0);

    function loadRecords(isPaging) {
        var apiRoute = '/SystemCommon/api/Organogram/GetOrganograms/';
        var listOrganogram = crudService.getAllIncludingCompanyuser(apiRoute, LoggedCompanyID, LoggedUserID, page, pageSize, isPaging);
        listOrganogram.then(function (response) {
            $scope.ListOrganogram = response.data
            console.log($scope.ListOrganogram);
        },
        function (error) {
            console.log("Error: " + error);
        });
    }

    loadRecords(0);

    //**********----Get Single Record----***************
    $scope.getorganogramForEdit = function (dataModel) {
        ////debugger
        var apiRoute = '/SystemCommon/api/Organogram/GetOrganogramByID/' + dataModel.OrganogramID;
        var singleModle = crudService.getByID(apiRoute);
        singleModle.then(function (response) {
            $scope.OrganogramID = response.data.OrganogramID;
            $scope.CustomCode = response.data.CustomCode;
            $scope.OrganogramName = response.data.OrganogramName;
            $scope.ParentID = response.data.ParentID;
            $scope.ddlStatus = response.data.StatusID;
            $scope.ddlCompany = response.data.CompanyID;
            console.log(response.data.IsBranch);
            $scope.IsBranch = response.data.IsBranch;
            $scope.IsDepartment = response.data.IsDepartment;
            //$scope.IsDefault = response.data.IsDefault;
            //$scope.IsCostCenter = response.data.IsCostCenter;
            $scope.btnSaveUpdateText = "Update";
            $("#parentDropDown").select2("data", { id: response.data.ParentID, text: response.data.ParentName });
            $("#companyDropDown").select2("data", { id: response.data.CompanyID, text: response.data.CompanyName });
            $("#statusDropDown").select2("data", { id: response.data.StatusID, text: response.data.StatusName });
        },
        function (error) {
            console.log("Error: " + error);
        });
    };

    //***** Toggole Click*****
    $scope.IsBranchChecked = function (IsBranch) {
        if (IsBranch)
            $scope.IsDepartment = false;
    }
    $scope.IsDepartmentChecked = function (IsDepartment) {
        if (IsDepartment)
            $scope.IsBranch = false;
    }
    //**********----Create New Record----***************
    $scope.save = function () {
        var Organogram = {
            OrganogramID: $scope.OrganogramID,
            CustomCode: $scope.CustomCode,
            OrganogramName: $scope.OrganogramName,
            ParentID: $scope.ddlParent,
            IsBranch: $scope.IsBranch,
            IsDepartment: $scope.IsDepartment,
            ProcessOutput: $scope.ProcessOutput,
            CompanyID: $scope.ddlCompany
        };
        if (Organogram.ParentID == -1) Organogram.ParentID == null;
        isExisting = $scope.OrganogramID;
        if (isExisting === 0) {
            var apiRoute = '/SystemCommon/api/Organogram/SaveOrganogram/';
            var Organogram = crudService.post(apiRoute, Organogram);
            Organogram.then(function (response) {
                ShowCustomToastrMessage(response);
                loadRecords(0);
                $scope.clear();
                modal_fadeOut();
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            var apiRoute = '/SystemCommon/api/Organogram/UpdateOrganogram/';
            var ModuleUpdate = crudService.put(apiRoute, Organogram);
            ModuleUpdate.then(function (response) {
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
        var IsConf = confirm('You are about to delete ' + dataModel.OrganogramName + '. Are you sure?');
        if (IsConf) {
            var apiRoute = '/SystemCommon/api/Organogram/DeleteOrganogram/' + dataModel.OrganogramID;
            var ModelDelete = crudService.delete(apiRoute);
            ModelDelete.then(function (response) {
                response.data = -101;
                ShowCustomToastrMessage(response);
                loadRecords(0);
                $scope.clear();
            }, function (error) {
                console.log("Error: " + error);
            });
        }
    }

    //**********----Reset Record----***************
     

    $scope.clear = function () {
        try {
            $scope.frmOrganogram.$setPristine();
            $scope.frmOrganogram.$setUntouched();
            frmOrganogram.$setPristine();
            frmOrganogram.$setUntouched();
        } catch (e) {
        }
        $scope.ddlCompany = null;
        $("#companyDropDown").select2('val', '--Select Color--');

        $scope.ParentID = null;
        $("#parentDropDown").select2('val', '--Select Process--');

        $scope.OrganogramID = 0;
        $scope.CustomCode = '';
        $scope.OrganogramName = '';
        $scope.IsCostCenter = '';
        $scope.IsDefault = '';
        $scope.IsBranch = false;
        $scope.IsDepartment = false;
        $scope.btnSaveUpdateText = "Save";
        loadRecords_Organogram(0);

    };
});


function modal_fadeOut() {
    $("#organModal").fadeOut(200, function () {
        $('#organModal').modal('hide');
    });
}