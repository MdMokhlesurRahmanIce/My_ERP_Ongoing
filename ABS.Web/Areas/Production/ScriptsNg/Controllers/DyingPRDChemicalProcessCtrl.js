
app.controller('DyingPRDChemicalProcessCtrl', ['$scope', 'DyingPRDChemicalProcessService','conversion',
    function ($scope, DyingPRDChemicalProcessService, conversion) {

        // API URL 
        var baseUrl = '/Production/api/DyingPRDChemicalProcess/';
        var productionCommonUrl = '/Production/api/ProductionDDL/';
        // API URL 

        // Region For Scope Default Parameter Initialization Begin
        var page = 1;
        var pageSize = 50;
        var isPaging = 0;
        var loggedUser = $('#hUserID').val();
        var companyID = $('#hCompanyID').val();

        $scope.PageTitle = 'Chemical Process Master Entry';
        $scope.btnShowList = 'Show List';
        $scope.btnSaveText = 'Save';
        // Region For Scope Default Parameter Initialization End


        //Initialization of Current Page variable  Begin
        $scope.modelItemID = 0;
        $scope.modelArticleNo = "";
        $scope.modelPIID = 0;
        $scope.modelPINo = "";
        $scope.modelBuyerID = 0;
        $scope.modelBuyerName = "";
        $scope.modelSupplierID = 0;
        $scope.modelSupplierName = "";


        $scope.modelStartTime = conversion.NowTime();
        $scope.modelEndTime = conversion.NowTime();
        $scope.modelTime = conversion.NowTime();
        $scope.modelConsumptionDate = conversion.NowDateCustom();
        //Initialization of Current Page variable End

        // Object And Array Initialization Begin
        $scope.ListSetNo = [];
        $scope.ListMachine = [];
        $scope.ListReference = [];
        $scope.ListShift = [];
        // Object And Array Initialization End


        // Page Load Begin

        function loadRecords_SetNo() {
            var apiRoute = productionCommonUrl + 'GetDyeingSetAll/';
            var processSetNo = DyingPRDChemicalProcessService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
            processSetNo.then(function (response) {
                $scope.ListSetNo = response.data;
            },
            function (error) {
                console.log("Error: " + error + "Error Method loadRecords_SetNo");
            });
        }
        loadRecords_SetNo();

        function loadRecords_Machine() {
            
            var apiRoute = productionCommonUrl + 'GetDyingMachineForChemicalProcess/';
            var processMachine = DyingPRDChemicalProcessService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
            processMachine.then(function (response) {
                $scope.ListMachine = response.data;
                loadRecords_Reference(558);
            },
            function (error) {
                console.log("Error: " + error + "Error Method loadRecords_SetNo");
            });
        }
        loadRecords_Machine();

        function loadRecords_Reference(ItemID) {
            if(ItemID = 0) return;
            var apiRoute = productionCommonUrl + 'GetDyingReferenceByItemID/';
            var processReference = DyingPRDChemicalProcessService.getByID(apiRoute, companyID, loggedUser, page, pageSize, isPaging, ItemID);
            processReference.then(function (response) {
                $scope.ListReference = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        

        function loadRecords_Shift() {
            var apiRoute = productionCommonUrl + 'GetShift/';
            var processShift = DyingPRDChemicalProcessService.getAll(apiRoute, companyID, loggedUser, page, pageSize, isPaging);
            processShift.then(function (response) {
                $scope.ListShift = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        loadRecords_Shift();
        // Page Load End

        //Change Events Begin
        $scope.SetChanged = function (SetID)
        {
            try {
                //Set ID not Selected Then
                if (SetID === undefined)
                {
                    $scope.modelItemID = 0;
                    $scope.modelArticleNo = "";
                    $scope.modelPIID = 0;
                    $scope.modelPINo = "";
                    $scope.modelBuyerID = 0;
                    $scope.modelBuyerName = "";
                    $scope.modelSupplierID = 0;
                    $scope.modelSupplierName = "";
                    return;
                }
                angular.forEach($scope.ListSetNo, function (value, key) {
                    if (value.SetID == SetID)
                    {
                        $scope.modelItemID = value.ItemID;
                        $scope.modelArticleNo = value.ItemName;
                        $scope.modelPIID = value.PIID;
                        $scope.modelPINo = value.PINo;
                        $scope.modelBuyerID = value.BuyerID;
                        $scope.modelBuyerName = value.BuyerName;
                        $scope.modelSupplierID = value.SupplierID;
                        $scope.modelSupplierName = value.SupplierName;
                    }
                });
            } catch (errorMessage) {
                console.log(errorMessage + "Error Method SetChanged");
            }
        }
        //Change Events End

        //Click Events Begin
        $scope.Say = function ()
        {
            alert("Declined");
        }



        // Click Events End




    }]);


