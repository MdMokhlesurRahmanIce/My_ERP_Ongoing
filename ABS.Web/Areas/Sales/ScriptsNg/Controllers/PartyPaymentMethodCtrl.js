/**
* PartyPaymentMethodCtrl.js
*/
app.controller('partyPaymentMethodCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
        function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
            //**************************************************Start Vairiable Initialize**************************************************
            var baseUrl = '/Sales/api/PartyPaymentMethod/';
            var partialUrl = '/Sales/api/HeadOfficeSalesDeliveryOrderEntry/';
            $scope.permissionPageVisibility = true;
            $scope.UserCommonEntity = {};
            $scope.HeaderToken = {};
            objcmnParam = {};

            var isExisting = 0;
            var page = 1;
            var pageSize = 10;
            var isPaging = 0;
            var totalData = 0;

            $scope.ListCompany = [];
            $scope.ListBuyer = [];
            $scope.ListLCNo = [];
            $scope.listDOMaster = [];

            var TabValForSaveData = 1;
            var TabId = 1;

            var DocumentValue_OD = "";
            var LIB_OD = "";
            var Discount_OD = "";
            var Percentage_OD = "";
            var PurchaseAmount_OD = 0;

            $scope.btnSaleDocSaveText = "Save";
            $scope.btnSalePurSaveText = "Save";
            $scope.btnSaleODSaveText = "Save";
            $scope.btnSaleAdjustSaveText = "Save";

            $scope.btnSaleReviseText = "Revise";
            $scope.btnSaleShowText = "Show DO Info";
            $scope.PageTitle = 'Party Payment';
            $scope.ListTitleAdjustmentDeatails = 'Adjustment Info Details';

            $scope.ShowDoc = false;
            $scope.ShowPur = true;
            $scope.ShowOD = true;
            $scope.ShowAD = true;

            $scope.ShipmentDate_Doc = conversion.NowDateCustom();
            $scope.DocsSentDateParty_Doc = conversion.NowDateCustom();
            $scope.SubmissionDatePartyBank_Doc = conversion.NowDateCustom();
            $scope.DocumentsDate_Doc = conversion.NowDateCustom();
            $scope.DocsRecieveDate_Doc = conversion.NowDateCustom();
            $scope.PartyAcceptanceDate_Doc = conversion.NowDateCustom();
            $scope.BankAcceptanceDate_Doc = conversion.NowDateCustom();
            //***************************************************End Vairiable Initialize***************************************************

            //***************************************************Start Common Task for all**************************************************
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            $scope.HeaderToken = conversion.Tokens($scope.tokenManager, $scope.UserCommonEntity);
            $scope.cmnParam = function () {
                objcmnParam = conversion.cmnParams($scope.UserCommonEntity);
            }
            //****************************************************End Common Task for all***************************************************

            //******************************End**************************************
            $scope.GetTabVal_Documents = function () {
                TabValForSaveData = 1;
                TabId = TabValForSaveData;
                $scope.ShowDoc = false;
                $scope.ShowPur = true;
                $scope.ShowOD = true;
                $scope.ShowAD = true;
                $scope.loadCompanyRecords_Doc(0);
                $scope.loadBuyerRecords_Doc(0);
                $scope.loadDocType_Doc(0);
                debugger
                var date = new Date();
                $scope.ShipmentDate_Doc = conversion.NowDateCustom();
                $scope.DocsSentDateParty_Doc = conversion.NowDateCustom();
                $scope.SubmissionDatePartyBank_Doc = conversion.NowDateCustom();
                $scope.DocumentsDate_Doc = conversion.NowDateCustom();
                $scope.DocsRecieveDate_Doc = conversion.NowDateCustom();
                $scope.PartyAcceptanceDate_Doc = conversion.NowDateCustom();
                $scope.BankAcceptanceDate_Doc = conversion.NowDateCustom();
            };
            $scope.GetTabVal_Purchase = function () {
                TabValForSaveData = 2;
                TabId = TabValForSaveData;
                $scope.ShowDoc = true;
                $scope.ShowPur = false;
                $scope.ShowOD = true;
                $scope.ShowAD = true;
                $scope.loadCompanyRecords_Pur(0);
                $scope.loadBuyerRecords_Pur(0);

                var date = new Date();
                $scope.PurchaseDate_Pur = conversion.NowDateCustom();
            };
            $scope.GetTabVal_Overdue = function () {
                TabValForSaveData = 3;
                TabId = TabValForSaveData;
                $scope.ShowDoc = true;
                $scope.ShowPur = true;
                $scope.ShowOD = false;
                $scope.ShowAD = true;
                $scope.loadCompanyRecords_OD(0);
                $scope.loadBuyerRecords_OD(0);
                $scope.loadPaymentMode_OD(0);

                var date = new Date();
                $scope.MaturityDate_OD = conversion.NowDateCustom();
                $scope.PaymentRecievedDate_OD = conversion.NowDateCustom();
                $scope.PaymentIssueDate_OD = conversion.NowDateCustom();
                $scope.AdjustmentDate_OD = conversion.NowDateCustom();
            };
            $scope.GetTabVal_Adjustment = function () {
                TabValForSaveData = 4;
                TabId = TabValForSaveData;
                $scope.ShowDoc = true;
                $scope.ShowPur = true;
                $scope.ShowOD = true;
                $scope.ShowAD = false;
                $scope.loadCompanyRecords_AD(0);
                $scope.loadBuyerRecords_AD(0);
                $scope.loadBankCharge_AD(0);

                var date = new Date();
                $scope.AdjustmentDate_AD = conversion.NowDateCustom();
            };
            //*******************************************************Start For Tab-1(Documents Info Entry)************************************************************

            //**********----Get All Company Record On Load----***************
            $scope.loadCompanyRecords_Doc = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetCompany/';
                var listCompany_Doc = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);//, page, pageSize, isPaging
                listCompany_Doc.then(function (response) {
                    $scope.listCompany_Doc = response.data.objListCompany;
                    debugger
                    var com_Doc = $filter('filter')($scope.listCompany_Doc, { CompanyID: $scope.UserCommonEntity.loggedCompnyID });
                    $scope.lstCompanyList_Doc = com_Doc[0].CompanyID;
                    debugger
                    $("#CompanyList_Doc").select2("data", { id: 0, text: com_Doc[0].CompanyName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            $scope.loadCompanyRecords_Doc(0);
            //fire on company dropdownlist change event
            //$scope.getChangedCompanyID_Doc = function () {
            //    $scope.xCompanyID_Doc = $scope.lstCompanyList_Doc;
            //}
            //*******************-------End------**********************    

            //*************-------Get All Buyer On Load---------***************
            $scope.loadBuyerRecords_Doc = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetBuyer/';
                var ListBuyer_Doc = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBuyer_Doc.then(function (response) {
                    $scope.ListBuyer_Doc = response.data.BuyerList;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            $scope.loadBuyerRecords_Doc(0);
            //*******************-------End------**********************

            //*************-------Get LC Master By Buyer ID Change on Buyer Dropdownlist---------***************
            $scope.loadLCRecords_Doc = function () {
                debugger
                $scope.ListLCNo_Doc = [];
                var id = $scope.lstBuyerList_Doc;
                var apiRoute = baseUrl + 'GetLCByBuyerId/' + id + "/" + $scope.lstCompanyList_Doc;
                var ListLCNo_Doc = crudService.getModelByID(apiRoute);
                ListLCNo_Doc.then(function (response) {
                    $scope.ListLCNo_Doc = response.data;
                    console.log(response.data);
                },
            function (error) {
                console.log("Error: " + error);
            });
            }
            //*******************-------End------**********************

            //*************-------Get All DocType On Load---------***************
            $scope.loadDocType_Doc = function (isPaging) {
                debugger
                var apiRoute = baseUrl + 'GetDocTypeOrPaymentMode/' + TabId + '/';
                var ListDocType_Doc = crudService.getModel(apiRoute, page, pageSize, isPaging);
                ListDocType_Doc.then(function (response) {
                    $scope.ListDocType_Doc = response.data;

                    debugger
                    var Doc_Doc = $filter('filter')($scope.ListDocType_Doc, { ComboName: 'Regular' });
                    $scope.lstDocumentsType_Doc = Doc_Doc[0].ComboID;
                    debugger
                    $("#documentsType_Doc").select2("data", { id: 0, text: Doc_Doc[0].ComboName });

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            $scope.loadDocType_Doc(0);
            //fire on DocType dropdownlist change event
            //$scope.getChangedComboID_Doc = function () {
            //    $scope.xComboID_Doc = $scope.lstDocumentsType_Doc;
            //}
            //*******************-------End------**********************

            //*************-------Get All Bank On Load---------***************
            $scope.loadBank_Doc = function () {
                $scope.ListBank_Doc = [];

                var id = $scope.lstLCList_Doc;

                var apiRoute = baseUrl + 'GetBankByLCID/' + id;
                var ListBank_Doc = crudService.getModelByID(apiRoute);
                ListBank_Doc.then(function (response) {
                    $scope.ListBank_Doc = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

            //fire on DocType dropdownlist change event
            //$scope.getChangedBankID_Doc = function () {
            //    $scope.xBankID_Doc = $scope.lstBankList_Doc;
            //}
            //*******************-------End------**********************

            //*************-------Check Duplicate Bill No---------***************    
            $scope.loadBillToCheckDuplicate_Doc = function (isPaging) {
                debugger
                var MessageData = "";
                var ListBillNo_Doc = [];
                var apiRoute = baseUrl + 'GetBillNo/';
                var ListBill_Doc = crudService.getModel(apiRoute, page, pageSize, isPaging);
                ListBill_Doc.then(function (response) {
                    ListBillNo_Doc = response.data;

                    angular.forEach(ListBillNo_Doc, function (item) {
                        debugger
                        if (item.BillNo == $scope.RefBillNo_Doc) {
                            MessageData = $scope.RefBillNo_Doc;
                            $scope.RefBillNo_Doc = "";
                            Command: toastr["warning"]("Your Inputed Ref Bill No: " + MessageData + " is already exist!");
                        }
                    });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //*************************------End------***************************

            //*********************************************************End For Tab-1(Documents Info Entry)************************************************************

            //********************************************************Start For Tab-2(Purchase Info Entry)************************************************************    
            //**********----Get All Company Record On Load----***************
            $scope.loadCompanyRecords_Pur = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];                
                var apiRoute = partialUrl + 'GetCompany/';
                var listCompany_Pur = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);//, page, pageSize, isPaging
                listCompany_Pur.then(function (response) {
                    $scope.listCompany_Pur = response.data.objListCompany;
                    debugger
                    var com_Pur = $filter('filter')($scope.listCompany_Pur, { CompanyID: $scope.UserCommonEntity.loggedCompnyID });
                    $scope.lstCompanyList_Pur = com_Pur[0].CompanyID;
                    debugger
                    $("#CompanyList_Pur").select2("data", { id: 0, text: com_Pur[0].CompanyName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //fire on company dropdownlist change event
            //$scope.getChangedCompanyID_Pur = function () {
            //    $scope.xCompanyID_Pur = $scope.lstCompanyList_Pur;
            //}
            //*******************-------End------**********************    

            //*************-------Get All Buyer On Load---------***************
            $scope.loadBuyerRecords_Pur = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetBuyer/';
                var ListBuyer_Pur = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBuyer_Pur.then(function (response) {
                    $scope.ListBuyer_Pur = response.data.BuyerList;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

            //$scope.getChangedPartyID_Pur = function () {
            //    $scope.xUserID_Pur = $scope.lstBuyerList_Pur;
            //}
            //*******************-------End------**********************        

            //*************-------Get All Bill Buyer wise---------***************
            $scope.loadBill_Pur = function (isPaging) {

                var id = $scope.lstBuyerList_Pur;
                var apiRoute = baseUrl + 'GetBillByBuyerID/' + id + '/' + TabId;
                var ListBill_Pur = crudService.getModelByID(apiRoute);
                ListBill_Pur.then(function (response) {
                    $scope.ListBill_Pur = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //$scope.loadDocType_Doc(0);

            //*************-------Get Docudment Value By Bill ID---------***************
            $scope.loadDocumentsValue_Pur = function () {

                var id = $scope.lstBillList_Pur;
                var apiRoute = baseUrl + 'GetDocValByBillID/' + id;
                var ListDocVal_Pur = crudService.getModelByID(apiRoute);
                ListDocVal_Pur.then(function (response) {
                    $scope.DocumentValue_Pur = response.data.DeliveryValue;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //*******************-------End------********************** 

            //*************************Take Either LIB or Discount**********************
            $scope.IsErrorShow = true;
            $scope.setLib_Pur = function () {
                $scope.Discount_Pur = "";
                if ($scope.LIB_Pur == "" || angular.isUndefined($scope.LIB_Pur) || $scope.LIB_Pur <= 0) { // && ($scope.LIB_Pur == "" || angular.isUndefined($scope.LIB_Pur))
                    $scope.IsErrorShow = false;
                    Command: toastr["warning"]("Please Input data in any field of LIB or Discount!");

                }
                else {
                    $scope.IsErrorShow = true;
                }
            };
            //$scope.LIB_Pur == "" || angular.isUndefined($scope.LIB_Pur) || $scope.LIB_Pur<=0
            $scope.setDiscount_Pur = function () {
                debugger
                $scope.LIB_Pur = "";
                if ($scope.Discount_Pur == "" || angular.isUndefined($scope.Discount_Pur) || $scope.Discount_Pur <= 0) { //($scope.Discount_Pur == "" || angular.isUndefined($scope.Discount_Pur)) && 
                    $scope.IsErrorShow = false;
                    Command: toastr["warning"]("Please Input data in any field of LIB or Discount!");
                }
                else {
                    $scope.IsErrorShow = true;
                }
            };

            //************************-------End------********************************** 

            //*************************Take Reserve Margine**********************
            $scope.setReservMargine_Pur = function () {
                if ($scope.Percentage_Pur > 100) {
                    $scope.Percentage_Pur = 100;
                }
                $scope.ReserveMargin_Pur = 100 - $scope.Percentage_Pur;
            }
            //************************-------End------**********************************

            //**********************************************************End For Tab-2(Purchase Info Entry)************************************************************

            //********************************************************Start For Tab-3(OverDue Info Entry)*************************************************************
            //**********----Get All Company Record On Load----***************
            $scope.loadCompanyRecords_OD = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetCompany/';
                var listCompany_OD = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);//, page, pageSize, isPaging
                listCompany_OD.then(function (response) {
                    $scope.listCompany_OD = response.data.objListCompany;
                    debugger
                    var com_OD = $filter('filter')($scope.listCompany_OD, { CompanyID: $scope.UserCommonEntity.loggedCompnyID });
                    $scope.lstCompanyList_OD = com_OD[0].CompanyID;
                    debugger
                    $("#CompanyList_OD").select2("data", { id: 0, text: com_OD[0].CompanyName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //fire on company dropdownlist change event
            //$scope.getChangedCompanyID_OD = function () {
            //    $scope.xCompanyID_OD = $scope.lstCompanyList_OD;
            //}
            //*******************-------End------**********************    

            //*************-------Get All Buyer On Load---------***************
            $scope.loadBuyerRecords_OD = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetBuyer/';
                var ListBuyer_OD = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBuyer_OD.then(function (response) {
                    $scope.ListBuyer_OD = response.data.BuyerList;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

            //$scope.getChangedPartyID_OD = function () {
            //    $scope.xUserID_OD = $scope.lstBuyerList_OD;
            //}
            //*******************-------End------**********************        

            //*************-------Get All Bill Buyer wise---------***************
            $scope.loadBill_OD = function (isPaging) {
                //$scope.xUserID_OD = $scope.lstBuyerList_OD;
                var id = $scope.lstBuyerList_OD;
                var apiRoute = baseUrl + 'GetBillByBuyerID/' + id + '/' + TabId;
                var ListBill_OD = crudService.getModelByID(apiRoute);
                ListBill_OD.then(function (response) {
                    $scope.ListBill_OD = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //$scope.loadDocType_Doc(0);

            //**********************Get Bill Changed ID && Document Value*********************
            $scope.getChangedBillID_OD = function () {
                DocumentValue_OD = "";
                LIB_OD = "";
                Discount_OD = "";
                Percentage_OD = "";
                PurchaseAmount_OD = 0;

                $scope.xBillMasterId_OD = $scope.lstBillList_OD;

                var id = $scope.xBillMasterId_OD;
                var apiRoute = baseUrl + 'GetDocValByBillID/' + id;
                var ListDocVal_OD = crudService.getModelByID(apiRoute);
                ListDocVal_OD.then(function (response) {
                    DocumentValue_OD = response.data.DeliveryValue;
                    LIB_OD = response.data.LIB;
                    Discount_OD = response.data.LIBDiscount;
                    Percentage_OD = response.data.ConvertionPercentage;

                    if (LIB_OD > Discount_OD || (LIB_OD > 0 && angular.isUndefined(Discount_OD))) {
                        PurchaseAmount_OD = LIB_OD;
                    }
                    else if (LIB_OD < Discount_OD || (Discount_OD > 0 && angular.isUndefined(LIB_OD))) {
                        PurchaseAmount_OD = Discount_OD;
                    }

                    $scope.setTotalODDaysParty_OD();
                    $scope.setTotalODDaysBank_OD();

                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //*******************************-------End------*********************************** 

            //*************-------Get All Payment Mode On Load---------***************
            $scope.loadPaymentMode_OD = function (isPaging) {
                debugger
                var apiRoute = baseUrl + 'GetDocTypeOrPaymentMode/' + TabId + '/';
                var ListPaymentMode_OD = crudService.getModel(apiRoute, page, pageSize, isPaging);
                ListPaymentMode_OD.then(function (response) {
                    $scope.ListPaymentMode_OD = response.data;

                    debugger
                    var PMode_Doc = $filter('filter')($scope.ListPaymentMode_OD, { ComboName: 'Cash' });
                    $scope.lstpaymentMode_OD = PMode_Doc[0].ComboID;
                    debugger
                    $("#paymentMode_OD").select2("data", { id: 0, text: PMode_Doc[0].ComboName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //fire on DocType dropdownlist change event
            //$scope.getChangedComboID_OD = function () {
            //    $scope.xComboID_OD = $scope.lstpaymentMode_OD;
            //}
            //*******************-------End------**********************

            //****************************************Take ShortFall****************************
            $scope.setShortFall_OD = function () {
                debugger
                if (DocumentValue_OD >= $scope.PaymentValue_OD && DocumentValue_OD > 0 && DocumentValue_OD != "" && $scope.PaymentValue_OD > 0 && $scope.PaymentValue_OD != "") {
                    $scope.Shortfall_OD = DocumentValue_OD - $scope.PaymentValue_OD;
                }
                else if (DocumentValue_OD < $scope.PaymentValue_OD && DocumentValue_OD > 0 && DocumentValue_OD != "" && $scope.PaymentValue_OD > 0 && $scope.PaymentValue_OD != "") {
                    $scope.PaymentValue_OD = DocumentValue_OD;
                    $scope.Shortfall_OD = 0;
                }
                else if (DocumentValue_OD == "" || $scope.PaymentValue_OD == "" && $scope.PaymentValue_OD != 0 || angular.isUndefined($scope.PaymentValue_OD)) {//angular.isUndefined(DocumentValue_OD)
                    $scope.PaymentValue_OD = "";
                    $scope.Shortfall_OD = "";
                }
                else if ($scope.PaymentValue_OD == 0) {
                    $scope.Shortfall_OD = DocumentValue_OD;
                }
            }
            //************************-------End------****************************************

            //****************************************Take OD Days By Calculating between Two Days****************************
            //**********************OD Days && OD Interest (Party)****************************
            $scope.setTotalODDaysParty_OD = function () {
                debugger
                if (PurchaseAmount_OD == "" || angular.isUndefined(PurchaseAmount_OD) || PurchaseAmount_OD == 0) {
                    $scope.MaturityDate_OD = "";
                    $scope.PaymentIssueDate_OD = "";
                }
                else {
                    $scope.TotalODDaysParty_OD = conversion.dateCalculationInDays($scope.MaturityDate_OD, $scope.PaymentIssueDate_OD);
                }

                if ($scope.TotalODDaysParty_OD < 0 && $scope.TotalODDaysParty_OD != "") {
                    $scope.TotalODDaysParty_OD = "";
                    $scope.PaymentIssueDate_OD = "";
                    Command: toastr["warning"]("Payment issue Date should be greater than or equal Maturity date!");
                }
                if ($scope.TotalODDaysParty_OD > 0 && PurchaseAmount_OD > 0) {
                    $scope.TotalODInterestParty_OD = PurchaseAmount_OD * Percentage_OD / 360 * $scope.TotalODDaysParty_OD;
                    //$scope.TotalODInterestParty_OD = conversion.Decimal2($scope.TotalODInterestParty_OD);
                    $scope.TotalODInterestParty_OD = conversion.roundNumber($scope.TotalODInterestParty_OD, 2);
                    var v = 0;
                }
                else {
                    $scope.TotalODInterestParty_OD = "";
                }
            }
            //************************-------End------****************************************

            //***********************OD Days && OD Interest (Bank)****************************
            $scope.setTotalODDaysBank_OD = function () {
                debugger
                if (PurchaseAmount_OD == "" || angular.isUndefined(PurchaseAmount_OD) || PurchaseAmount_OD == 0) {
                    $scope.PaymentRecievedDate_OD = "";
                    $scope.AdjustmentDate_OD = "";
                }
                else {
                    $scope.TotalODDaysBank_OD = conversion.dateCalculationInDays($scope.PaymentRecievedDate_OD, $scope.AdjustmentDate_OD);
                }

                if ($scope.TotalODDaysBank_OD < 0 && $scope.TotalODDaysBank_OD != "") {
                    $scope.TotalODDaysBank_OD = "";
                    $scope.AdjustmentDate_OD = "";
                    Command: toastr["warning"]("Adjustment Date should be greater than or equal Payment receive date!");
                }
                if ($scope.TotalODDaysBank_OD > 0 && PurchaseAmount_OD > 0) {
                    $scope.TotalODInterestBank_OD = PurchaseAmount_OD * Percentage_OD / 360 * $scope.TotalODDaysBank_OD;
                }
                else {
                    $scope.TotalODInterestBank_OD = "";
                }
            }
            //************************-------End------****************************************

            //*********************************************-------End------******************************************************

            //**********************************************************End For Tab-3(OverDue Info Entry)*************************************************************

            //********************************************************Start For Tab-4(Adjustment Info Entry)**********************************************************
            //**********----Get All Company Record On Load----***************
            $scope.loadCompanyRecords_AD = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetCompany/';
                var listCompany_AD = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);//, page, pageSize, isPaging
                listCompany_AD.then(function (response) {
                    $scope.listCompany_AD = response.data.objListCompany;
                    debugger
                    var com_AD = $filter('filter')($scope.listCompany_AD, { CompanyID: $scope.UserCommonEntity.loggedCompnyID });
                    $scope.lstCompanyList_AD = com_AD[0].CompanyID;
                    debugger
                    $("#CompanyList_AD").select2("data", { id: 0, text: com_AD[0].CompanyName });
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //fire on company dropdownlist change event
            //$scope.getChangedCompanyID_AD = function () {
            //    $scope.xCompanyID_AD = $scope.lstCompanyList_AD;
            //}
            //*******************-------End------**********************    

            //*************-------Get All Buyer On Load---------***************
            $scope.loadBuyerRecords_AD = function (isPaging) {
                objcmnParam = {
                    pageNumber: 1,
                    pageSize: 10,
                    IsPaging: 1,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                ModelsArray = [objcmnParam];
                var apiRoute = partialUrl + 'GetBuyer/';
                var ListBuyer_AD = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
                ListBuyer_AD.then(function (response) {
                    $scope.ListBuyer_AD = response.data.BuyerList;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }

            //$scope.getChangedPartyID_OD = function () {
            //    $scope.xUserID_OD = $scope.lstBuyerList_OD;
            //}
            //*******************-------End------**********************        

            //*************-------Get All Bill Buyer wise---------***************
            $scope.loadBill_AD = function (isPaging) {
                //$scope.xUserID_AD = $scope.lstBuyerList_AD;
                var id = $scope.lstBuyerList_AD;
                var apiRoute = baseUrl + 'GetBillByBuyerID/' + id + '/' + TabId;
                var ListBill_AD = crudService.getModelByID(apiRoute);
                ListBill_AD.then(function (response) {
                    $scope.ListBill_AD = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //$scope.loadDocType_Doc(0);

            //fire on DocType dropdownlist change event
            //$scope.getChangedBillID_AD = function () {
            //    $scope.xBillMasterId_AD = $scope.lstBillList_AD;
            //}
            //*******************-------End------**********************

            //*************-------Get All DocType On Load---------***************
            $scope.loadBankCharge_AD = function (isPaging) {
                var apiRoute = baseUrl + 'GetBankCharge/';
                var BankCharge_AD = crudService.getModel(apiRoute, page, pageSize, isPaging);
                BankCharge_AD.then(function (response) {
                    $scope.ListCharge_AD = response.data;
                },
                function (error) {
                    console.log("Error: " + error);
                });
            }
            //$scope.loadBankCharge_AD(0);
            //fire on DocType dropdownlist change event
            //$scope.getChangedComboID_AD = function () {
            //    $scope.xComboID_AD = $scope.lstChargeName_AD;
            //}
            //*******************-------End------**********************

            //*************************************Start Disable/Enable Add To List Button**************************
            $scope.IsDisable = true;
            $scope.btnAddEnable = function () {
                if ((angular.isUndefined($scope.lstChargeName_AD) || $scope.lstChargeName_AD == "") || (angular.isUndefined($scope.Amount_AD) || $scope.Amount_AD == "" || $scope.Amount_AD < 1)) {
                    $scope.IsDisable = true;
                }
                else {
                    $scope.IsDisable = false;
                }
            }
            //************************************End Disable/Enable Add To List Button****************************

            //*****************************************Start Add Item To List**************************************
            $scope.ListAdjustmentInfoDetails_AD = [];
            $scope.AddItemToList = function () {
                //var ListAdjustmentInfoDetails_AD = [];
                var ComboName = $("#chargeName_AD").select2('data').text;
                $scope.ListAdjustmentInfoDetails_AD.push({ BADetailID: 0, BankChargeID: $scope.lstChargeName_AD, ChargeAmount: $scope.Amount_AD, ComboName: ComboName });
                //$scope.ListAdjustmentInfoDetails_AD.push(ListAdjustmentInfoDetails_AD);
            }
            //*******************************************End Add Item To List**************************************

            //*****************************************Start Delete Item From List**************************************
            $scope.deleteRow = function (index) {
                // $scope.ListPIDetails.splice($scope.ListPIDetails.indexOf($scope.data), 1);
                $scope.ListAdjustmentInfoDetails_AD.splice(index, 1);
                //$scope.getTotalYds();

            };
            //*****************************************End Delete Item From List**************************************

            //**********************************************************End For Tab-4(Adjustment Info Entry)**********************************************************

            //***********************************************************Start Conditional(Tab Wise) Save*************************************************************    
            $scope.save = function () {
                debugger
                var DataForSave = {};
                //var NewStringToDate = "";
                var message = "Saved";
                //if ($scope.btnSaleSaveText == "Save") {
                //    $scope.HDOID = 0;
                //    message = "Saved";
                //}

                //To Convert String to Date

                debugger
                //********************************************Start Docs Info (Tab-1) Data Process for Save**************************************************
                if (TabValForSaveData == 1) {
                    if (angular.isUndefined($scope.ShipmentDate_Doc)) {
                        ShipmentDate_Doc = "1/1/1900";
                    }
                    else {
                        ShipmentDate_Doc = conversion.getStringToDate($scope.ShipmentDate_Doc);
                    }
                    if (angular.isUndefined($scope.DocsSentDateParty_Doc)) {
                        DocsSentDateParty_Doc = "1/1/1900";
                    }
                    else {
                        DocsSentDateParty_Doc = conversion.getStringToDate($scope.DocsSentDateParty_Doc);
                    }
                    if (angular.isUndefined($scope.SubmissionDatePartyBank_Doc)) {
                        SubmissionDatePartyBank_Doc = "1/1/1900";
                    }
                    else {
                        SubmissionDatePartyBank_Doc = conversion.getStringToDate($scope.SubmissionDatePartyBank_Doc);
                    }
                    if (angular.isUndefined($scope.DocumentsDate_Doc)) {
                        DocumentsDate_Doc = "1/1/1900";
                    }
                    else {
                        DocumentsDate_Doc = conversion.getStringToDate($scope.DocumentsDate_Doc);
                    }
                    if (angular.isUndefined($scope.DocsRecieveDate_Doc)) {
                        DocsRecieveDate_Doc = "1/1/1900";
                    }
                    else {
                        DocsRecieveDate_Doc = conversion.getStringToDate($scope.DocsRecieveDate_Doc);
                    }
                    if (angular.isUndefined($scope.PartyAcceptanceDate_Doc)) {
                        PartyAcceptanceDate_Doc = "1/1/1900";
                    }
                    else {
                        PartyAcceptanceDate_Doc = conversion.getStringToDate($scope.PartyAcceptanceDate_Doc);
                    }
                    if (angular.isUndefined($scope.BankAcceptanceDate_Doc)) {
                        BankAcceptanceDate_Doc = "1/1/1900";
                    }
                    else {
                        BankAcceptanceDate_Doc = conversion.getStringToDate($scope.BankAcceptanceDate_Doc);
                    }

                    //-----------------------
                    var DocsInfoEntry = {
                        LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                        LUserID: $scope.UserCommonEntity.loggedUserID,
                        LMenuID: $scope.UserCommonEntity.currentMenuID,
                        LTransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                        TabID: TabValForSaveData,

                        CompanyID: $scope.lstCompanyList_Doc,
                        BuyerID: $scope.lstBuyerList_Doc,
                        LCID: $scope.lstLCList_Doc,
                        DeliveryQty: $scope.DeliveryQuantity_Doc,
                        DocumentsNo: $scope.DocumentsNo_Doc,
                        ShipmentDate: ShipmentDate_Doc,
                        DocsSentDateParty: DocsSentDateParty_Doc,
                        BankID: $scope.lstBankList_Doc,
                        SubmissionDatePartyBank: SubmissionDatePartyBank_Doc,
                        ComboID: $scope.lstDocumentsType_Doc,
                        RefNo: $scope.RefNo_Doc,
                        DocumentValue: $scope.DocumentValue_Doc,
                        DocumentsDate: DocumentsDate_Doc,
                        DocsRecieveDate: DocsRecieveDate_Doc,
                        PartyAcceptanceDate: PartyAcceptanceDate_Doc,
                        RefBillNo: $scope.RefBillNo_Doc,
                        BankAcceptanceDate: BankAcceptanceDate_Doc
                    };
                    DataForSave = DocsInfoEntry;
                }
                    //********************************************End Docs Info (Tab-1) Data Process for Save**************************************************

                    //*****************************************Start Purchase Info (Tab-2) Data Process for Save***********************************************
                else if (TabValForSaveData == 2) {
                    if (angular.isUndefined($scope.PurchaseDate_Pur)) {
                        PurchaseDate_Pur = "1/1/1900";
                    }
                    else {
                        PurchaseDate_Pur = conversion.getStringToDate($scope.PurchaseDate_Pur);
                    }

                    //-----------------------
                    var PurchaseInfoEntry = {
                        LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                        LUserID: $scope.UserCommonEntity.loggedUserID,
                        LMenuID: $scope.UserCommonEntity.currentMenuID,
                        LTransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                        TabID: TabValForSaveData,

                        CompanyID: $scope.lstCompanyList_Pur,
                        BuyerID: $scope.lstBuyerList_Pur,
                        BillMasterId: $scope.lstBillList_Pur,
                        LIB: $scope.LIB_Pur,
                        ConversionRate: $scope.ConversionRate_Pur,
                        SugarPAD: $scope.SugarPAD_Pur,
                        ReserveMargin: $scope.ReserveMargin_Pur,
                        PurchaseDate: PurchaseDate_Pur,
                        Discount: $scope.Discount_Pur,
                        Percentage: $scope.Percentage_Pur,
                        LIBRateOfInterest: $scope.LIBRateOfInterest_Pur,
                        DocumentValue: $scope.DocumentValue_Pur
                    };
                    DataForSave = PurchaseInfoEntry;
                }
                    //********************************************End Purchase Info (Tab-2) Data Process for Save**************************************************
                    //*****************************************Start OverDue Info (Tab-3) Data Process for Save***********************************************
                else if (TabValForSaveData == 3) {
                    if (angular.isUndefined($scope.PaymentIssueDate_OD)) {
                        PaymentIssueDate_OD = "1/1/1900";
                    }
                    else {
                        PaymentIssueDate_OD = conversion.getStringToDate($scope.PaymentIssueDate_OD);
                    }
                    if (angular.isUndefined($scope.AdjustmentDate_OD)) {
                        AdjustmentDate_OD = "1/1/1900";
                    }
                    else {
                        AdjustmentDate_OD = conversion.getStringToDate($scope.AdjustmentDate_OD);
                    }
                    if (angular.isUndefined($scope.MaturityDate_OD)) {
                        MaturityDate_OD = "1/1/1900";
                    }
                    else {
                        MaturityDate_OD = conversion.getStringToDate($scope.MaturityDate_OD);
                    }
                    if (angular.isUndefined($scope.PaymentRecievedDate_OD)) {
                        PaymentRecievedDate_OD = "1/1/1900";
                    }
                    else {
                        PaymentRecievedDate_OD = conversion.getStringToDate($scope.PaymentRecievedDate_OD);
                    }

                    //-----------------------
                    var OverDueInfoEntry = {
                        LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                        LUserID: $scope.UserCommonEntity.loggedUserID,
                        LMenuID: $scope.UserCommonEntity.currentMenuID,
                        LTransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                        TabID: TabValForSaveData,

                        CompanyID: $scope.lstCompanyList_OD,
                        BuyerID: $scope.lstBuyerList_OD,
                        BillMasterId: $scope.lstBillList_OD,
                        PaymentIssueDate: PaymentIssueDate_OD,
                        ComboID: $scope.lstpaymentMode_OD,
                        AdjustmentDate: AdjustmentDate_OD,
                        TotalODDaysParty: $scope.TotalODDaysParty_OD,
                        TotalODInterestParty: $scope.TotalODInterestParty_OD,
                        ODAdjustment: $scope.ODAdjustment_OD,
                        MaturityDate: MaturityDate_OD,
                        PaymentValue: $scope.PaymentValue_OD,
                        Shortfall: $scope.Shortfall_OD,
                        PaymentRecievedDate: PaymentRecievedDate_OD,
                        TotalODDaysBank: $scope.TotalODDaysBank_OD,
                        TotalODInterestBank: $scope.TotalODInterestBank_OD
                    };
                    DataForSave = OverDueInfoEntry;
                }
                //********************************************End OverDue Info (Tab-3) Data Process for Save**************************************************

                var apiRoute = baseUrl + 'SaveUpdatePartyPayment/';
                var DataCreateUpdate = crudService.post(apiRoute, DataForSave);
                DataCreateUpdate.then(function (response) {
                    debugger
                    $scope.clear();
                    debugger
                    if (TabValForSaveData == 1) {
                        $scope.RefNo_Doc = response.data;
                    }
                    Command: toastr["success"]("Data " + message + " Successfully!!!!");

                },
                function (error) {
                    console.log("Error: " + error);
                    Command: toastr["warning"]("Data Not " + message + "!!!!");
                });
            };

            $scope.saveAdustment = function () {
                //var NewStringToDate = "";
                var message = "Saved";
                //if ($scope.btnSaleSaveText == "Save") {
                //    $scope.HDOID = 0;
                //    message = "Saved";
                //}

                //To Convert String to Date

                debugger
                //*****************************************Start Adjustment Info (Tab-3) Data Process for Save***********************************************

                if (angular.isUndefined($scope.AdjustmentDate_AD)) {
                    AdjustmentDate_AD = "1/1/1900";
                }
                else {
                    AdjustmentDate_AD = conversion.getStringToDate($scope.AdjustmentDate_AD);
                }

                //Master
                var AdjustmentInfoEntry = {
                    LCompanyID: $scope.UserCommonEntity.loggedCompnyID,
                    LUserID: $scope.UserCommonEntity.loggedUserID,
                    LMenuID: $scope.UserCommonEntity.currentMenuID,
                    LTransactionTypeID: $scope.UserCommonEntity.currentTransactionTypeID,
                    TabID: TabValForSaveData,

                    CompanyID: $scope.lstCompanyList_AD,
                    BuyerID: $scope.lstBuyerList_AD,
                    BillMasterId: $scope.lstBillList_AD,
                    LIBAdjustmentAmount: $scope.LIBAdjustmentAmount_AD,
                    RestRealizedAmount: $scope.RestRealizedAmount_AD,
                    PAD: $scope.PAD_AD,
                    AdjustmentDate: AdjustmentDate_AD,
                    RestRealizedAmtPercentage: $scope.RestRealizedAmtPercentage_AD,
                    ConversionRateRealized: $scope.ConversionRateRealized_AD,
                    ERQ: $scope.ERQ_AD,
                    Remarks: $scope.Remarks_AD
                };
                //Detail
                //var AdjustmentInfoEntryList = [];
                //AdjustmentInfoEntryList = ListAdjustmentInfoDetails_AD;
                ListAdjustmentInfoDetails_AD.push(AdjustmentInfoEntry);


                var apiRoute = baseUrl + 'SaveUpdateAdjustment/';
                var AdjustDataCreateUpdate = crudService.post(apiRoute, ListAdjustmentInfoDetails_AD);
                AdjustDataCreateUpdate.then(function (response) {
                    $scope.clear();
                    Command: toastr["success"]("Data " + message + " Successfully!!!!");
                },
                function (error) {
                    console.log("Error: " + error);
                    Command: toastr["warning"]("Data Not " + message + "!!!!");
                });
            };
            //********************************************End Adjustment Info (Tab-3) Data Process for Save**************************************************

            //************************************************************Start Conditional(Tab Wise) Clear**************************************************
            $scope.clear = function () {
                if (TabValForSaveData == 1) {
                    $scope.lstCompanyList_Doc = "";
                    $scope.listCompany_Doc = "";
                    $scope.lstBuyerList_Doc = "";
                    $scope.ListBuyer_Doc = "";
                    $scope.DeliveryQuantity_Doc = "";
                    $scope.DocumentsNo_Doc = "";
                    $scope.lstBankList_Doc = "";
                    $scope.ListBank_Doc = "";
                    $scope.RefBillNo_Doc = "";
                    $scope.RefNo_Doc = "";
                    $scope.lstLCList_Doc = "";
                    $scope.ListLCNo_Doc = "";
                    $scope.lstDocumentsType_Doc = "";
                    $scope.ListDocType_Doc = "";
                    $scope.DocumentValue_Doc = "";

                    var date = new Date();
                    $scope.ShipmentDate_Doc = conversion.NowDateCustom();
                    $scope.DocsSentDateParty_Doc = conversion.NowDateCustom();
                    $scope.SubmissionDatePartyBank_Doc = conversion.NowDateCustom();
                    $scope.DocumentsDate_Doc = conversion.NowDateCustom();
                    $scope.DocsRecieveDate_Doc = conversion.NowDateCustom();
                    $scope.PartyAcceptanceDate_Doc = conversion.NowDateCustom();
                    $scope.BankAcceptanceDate_Doc = conversion.NowDateCustom();

                    $scope.ShowDoc = false;
                    $scope.ShowPur = true;
                    $scope.ShowOD = true;
                    $scope.ShowAD = true;
                    $scope.loadCompanyRecords_Doc(0);
                    $scope.loadBuyerRecords_Doc(0);
                    $scope.loadDocType_Doc(0);

                    $("#CompanyList_Doc").select2("data", { id: "", text: "--Select Company--" });
                    $("#BuyerList_Doc").select2("data", { id: "", text: "--Select Party--" });
                    $("#bankList_Doc").select2("data", { id: "", text: "--Select Bank--" });
                    $("#LCList_Doc").select2("data", { id: "", text: "--Select LC--" });
                    $("#documentsType_Doc").select2("data", { id: "", text: "--Select Docs Type--" });
                }
                else if (TabValForSaveData == 2) {
                    $scope.lstCompanyList_Pur = "";
                    $scope.listCompany_Pur = "";
                    $scope.lstBillList_Pur = "";
                    $scope.ListBill_Pur = "";
                    $scope.LIB_Pur = "";
                    $scope.ConversionRate_Pur = "";
                    $scope.SugarPAD_Pur = "";
                    $scope.ReserveMargin_Pur = "";
                    $scope.lstBuyerList_Pur = "";
                    $scope.ListBuyer_Pur = "";
                    $scope.Discount_Pur = "";
                    $scope.Percentage_Pur = "";
                    $scope.LIBRateOfInterest_Pur = "";
                    $scope.DocumentValue_Pur = "";

                    var date = new Date();
                    $scope.PurchaseDate_Pur = conversion.NowDateCustom();

                    $scope.ShowDoc = true;
                    $scope.ShowPur = false;
                    $scope.ShowOD = true;
                    $scope.ShowAD = true;
                    $scope.loadCompanyRecords_Pur(0);
                    $scope.loadBuyerRecords_Pur(0);

                    $("#CompanyList_Pur").select2("data", { id: "", text: "--Select Company--" });
                    $("#BuyerList_Pur").select2("data", { id: "", text: "--Select Party--" });
                    $("#billList_Pur").select2("data", { id: "", text: "--Select Bill No--" });
                }
                else if (TabValForSaveData == 3) {
                    $scope.lstCompanyList_OD = "";
                    $scope.listCompany_OD = "";
                    $scope.lstBillList_OD = "";
                    $scope.ListBill_OD = "";
                    $scope.PaymentValue_OD = "";
                    $scope.TotalODDaysParty_OD = "";
                    $scope.TotalODInterestParty_OD = "";
                    $scope.ODAdjustment_OD = "";
                    $scope.lstBuyerList_OD = "";
                    $scope.ListBuyer_OD = "";
                    $scope.lstpaymentMode_OD = "";
                    $scope.ListPaymentMode_OD = "";
                    $scope.Shortfall_OD = "";
                    $scope.TotalODDaysBank_OD = "";
                    $scope.TotalODInterestBank_OD = "";

                    DocumentValue_OD = "";
                    LIB_OD = "";
                    Discount_OD = "";
                    Percentage_OD = "";
                    PurchaseAmount_OD = 0;

                    var date = new Date();
                    $scope.MaturityDate_OD = conversion.NowDateCustom();
                    $scope.PaymentRecievedDate_OD = conversion.NowDateCustom();
                    $scope.PaymentIssueDate_OD = conversion.NowDateCustom();
                    $scope.AdjustmentDate_OD = conversion.NowDateCustom();

                    $scope.ShowDoc = true;
                    $scope.ShowPur = true;
                    $scope.ShowOD = false;
                    $scope.ShowAD = true;
                    $scope.loadCompanyRecords_OD(0);
                    $scope.loadBuyerRecords_OD(0);
                    $scope.loadPaymentMode_OD(0);

                    $("#CompanyList_OD").select2("data", { id: "", text: "--Select Company--" });
                    $("#BuyerList_OD").select2("data", { id: "", text: "--Select Party--" });
                    $("#billList_OD").select2("data", { id: "", text: "--Select Bill No--" });
                    $("#paymentMode_OD").select2("data", { id: "", text: "--Select Payment Mode--" });
                }
                else if (TabValForSaveData == 4) {

                    $scope.lstCompanyList_AD = "";
                    $scope.lstCompanyList_AD = "";
                    $scope.lstBuyerList_AD = "";
                    $scope.ListBuyer_AD = "";
                    $scope.lstBillList_AD = "";
                    $scope.ListBill_AD = "";
                    $scope.LIBAdjustmentAmount_AD = "";
                    $scope.RestRealizedAmount_AD = "";
                    $scope.PAD_AD = "";
                    $scope.Remarks_AD = "";
                    $scope.RestRealizedAmtPercentage_AD = "";
                    $scope.ConversionRateRealized_AD = "";
                    $scope.ERQ_AD = "";
                    $scope.lstChargeName_AD = "";
                    $scope.ListCharge_AD = "";
                    $scope.Amount_AD = "";
                    $scope.IsDisable = true;

                    $scope.ListAdjustmentInfoDetails_AD = "";
                    $scope.AdustmentInfoSearch_AD = "";

                    var date = new Date();
                    $scope.AdjustmentDate_AD = conversion.NowDateCustom();

                    $scope.ShowDoc = true;
                    $scope.ShowPur = true;
                    $scope.ShowOD = true;
                    $scope.ShowAD = false;
                    $scope.loadCompanyRecords_AD(0);
                    $scope.loadBuyerRecords_AD(0);
                    $scope.loadBankCharge_AD(0);

                    $("#CompanyList_AD").select2("data", { id: "", text: "--Select Company--" });
                    $("#BuyerList_AD").select2("data", { id: "", text: "--Select Buyer--" });
                    $("#billList_AD").select2("data", { id: "", text: "--Select Bill No--" });
                    $("#chargeName_AD").select2("data", { id: "", text: "--Select Charges--" });
                }
            }
            //*************************************************************End Conditional(Tab Wise) Clear**************************************************

            //************************************************************End Conditional(Tab Wise) Save**************************************************************

        }]);