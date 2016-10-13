/*
  FebricInspectionCtrl.js
 */
app.controller('FebricInspectionCtrl', ['$scope', 'crudService', 'conversion', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $localStorage, uiGridConstants) {
        var baseUrl = '/Production/api/FebricInspection/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];

        var isExisting = 0;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        //$scope.btnSaveUpdateText = "Save";
        //$scope.btnShowHide = "Show List";
        $scope.PageTitle = 'Febric Inspection';
        $scope.ListTitleDetail = 'Inspaction detail';
        $scope.ListTitle = 'Febric Inspection List';
        $scope.FebricInspectionDetailsList = [];
        //$scope.DeleteFebricIndpectionList = [];
        $scope.txtbxDate = conversion.NowDateCustom();
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.MenuID = 0;
        //var LoggedUserID = $('#hUserID').val();
        //var LoginUserID = $('#hUserID').val();
        //var LoginCompanyID = $('#hCompanyID').val();
        //$scope.IsbtnAddDelShow = true;

        //***************************************************Start Common Task for all**************************************************
        frmName = ''; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'InspactionNo'; EditFunc = 'getFebricInspectionByInspactionID, getFabricInspectionDetailsbyID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        //$scope.StyleNo = function () {

        //    var apiRoute = baseUrl + 'GetStyleNo/'; // 
        //    var _GetStyle = FebricInspectionSer.getAll(apiRoute, page, pageSize, isPaging, LoginCompanyID);
        //    _GetStyle.then(function (response) {
        //        $scope.Styles = response.data;
        //    },
        //    function (error) {
        //        console.log("Error: " + error);
        //    });
        //}
        //$scope.StyleNo();

        $scope.loadStyleNo = function (dataModel) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetStyleNo/';
            var listStyles = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listStyles.then(function (response) {
                $scope.Styles = response.data.Styles;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadStyleNo(0);

        $scope.loadShiftRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetShifts/';
            var listShifts = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listShifts.then(function (response) {
                $scope.Shifts = response.data.ShiftList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadShiftRecords(0);

        $scope.loadOperatorRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.ItemType = 1;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetOperators/';
            var listOperator = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listOperator.then(function (response) {
                $scope.Operators = response.data.OperatorList;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadOperatorRecords(0);

        //************************************************Start Machine Dropdown******************************************************
        $scope.loadMachineRecords = function (dataM) {
            debugger
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetMachine/';
            ModelsArray = [objcmnParam];
            var ListMachine = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListMachine.then(function (response) {
                debugger
                $scope.Machines = response.data.ListMachine;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadMachineRecords(0);
        //**************************************************End Machine Dropdown******************************************************

        $scope.loadPlatesRecords = function (dataModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = objcmnParam.loggedCompnyID == 1 ? 12 : 61;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetPlates/';
            var listDept = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            listDept.then(function (response) {
                debugger
                $scope.Plates = response.data.ListDept;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.loadPlatesRecords(0);

        $scope.getFabricIspectionByStyle = function () {
            $scope.cmnParam();
            objcmnParam.id = $scope.FinishingMRRID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFabricInspectionByStyle/';
            var _Pate = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _Pate.then(function (response) {
                if (response.data._fabricInspection != null) {
                    $scope.txtbxADMLCode = response.data._fabricInspection.ArticleNo;
                    $scope.txtbxSetNo = response.data._fabricInspection.SetNo;
                    $scope.txtbxColor = response.data._fabricInspection.ColorName;
                    $scope.FinishingMRRID = response.data._fabricInspection.FinishingMRRID;
                    $scope.ItemID = response.data._fabricInspection.ItemID;
                    $scope.SetID = response.data._fabricInspection.SetID;
                    $scope.WeavingMRRID = response.data._fabricInspection.WeavingMRRID;
                    $scope.SizeMRRID = response.data._fabricInspection.SizeMRRID;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.GetDefectPoints = function () {
            $scope.cmnParam();
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetDefectPoints/';
            var _defect = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _defect.then(function (response) {
                if (response.data._objDefectPoints != null) {
                    $scope.DefectPoints = response.data._objDefectPoints;
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.GetDefectPoints();

        //************************************************Start Unit Dropdown******************************************************
        $scope.loadUOMRecords = function () {
            debugger
            $scope.cmnParam();
            var apiRoute = baseUrl + 'GetUnit/';
            ModelsArray = [objcmnParam];
            var ListUOM = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            ListUOM.then(function (response) {
                debugger
                $scope.ListUOM = response.data.ListUOM;
            },
            function (error) {
                console.log("Error: " + error);
            });

        }
        $scope.loadUOMRecords();
        //**************************************************End Unit Dropdown******************************************************  

        //**************************** ADD Item To List ****************************************
        $scope.AddItemToList = function () {
            $scope.FebricInspectionDetailsList.push({ InspactionID: 0, InspactionDateilID: 0, BeamNo: 0, RollNo: '', GreigeLength: 0, UnitID: 0, Piece: 0, DefectPoint: 0, GrossWt: 0, NetWt: 0, Remarks: '' });
            $scope.IsShow = true;
            $scope.cmnbtnShowHideEnDisable('false');
        };

        //$scope.deleteFrebricInspectionDetails = function (dataModel) {
        //    if (isExisting > 0) {
        //        $scope.DeleteFebricIndpectionList.push(dataModel);
        //    }
        //    $scope.FebricInspectionDetailsList.splice($scope.FebricInspectionDetailsList.indexOf(dataModel), 1);
        //    $scope.IsShow = $scope.FebricInspectionDetailsList.length > 0 ? true : false;
        //}

        $scope.deleteRow = function (index) {
            $scope.FebricInspectionDetailsList.splice(index, 1);
            $scope.IsShow = $scope.FebricInspectionDetailsList.length > 0 ? true : false;
            if ($scope.FebricInspectionDetailsList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
        };

        //Pagination
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15,
            pageNumber: 1,
            pageSize: 15,
            totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.LoadFabricInspectionDetails(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.LoadFabricInspectionDetails(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.LoadFabricInspectionDetails(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.LoadFabricInspectionDetails(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.LoadFabricInspectionDetails(1);
                }
            }
        };

        $scope.LoadFabricInspectionDetails = function (isPaging) {
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize;
            objcmnParam.pageSize = $scope.pagination.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };
            $scope.gridOptions = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "InspactionID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ItemID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "WeivingMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SizeMRRID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MachineConfigID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "PlateID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OperatorID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "InspactionNo", displayName: "Inspaction No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ArticleNo", displayName: "ArticleNo", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "FinishingMRRNo", displayName: "MRR No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SetNo", displayName: "Set No", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ColorName", displayName: "Color Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ShiftName", displayName: "Shift Name", headerCellClass: $scope.highlightFilteredHeader },
                     { name: "PlateNo", displayName: "Plate No", headerCellClass: $scope.highlightFilteredHeader },
                     { name: "MachineConfigNo", displayName: "Machine Config No", headerCellClass: $scope.highlightFilteredHeader },
                     { name: "WeavingMRRNo", displayName: "Weaving MRR No", headerCellClass: $scope.highlightFilteredHeader },
                     { name: "UserFullName", displayName: "Operator", headerCellClass: $scope.highlightFilteredHeader },
                     { name: "FabricInspectionDate", displayName: "Date", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                     { name: "Remarks", displayName: "Remarks", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Options',
                        displayName: "Options",
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        pinnedRight: true,
                        width: '13%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'FabricInspection.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Item Groups", style: 'headerStyle' },
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
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'FabricInspectionDetails/';
            var _finishGoods = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _finishGoods.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptions.data = response.data.fabricInspectionMasgter;
                $scope.loaderMore = false;
                //$scope.finishGoods = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.LoadFabricInspectionDetails(0);
        }
        $scope.RefreshMasterList();

        $scope.getFebricInspectionByInspactionID = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.InspactionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFebricInspectionByInspectionID/';
            var _febricIndpection = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _febricIndpection.then(function (response) {
                if (response.data != null) {
                    isExisting = $scope.InspactionID = response.data.InspactionID
                    $scope.txtbxColor = response.data.ColorName;
                    $scope.txtbxADMLCode = response.data.ArticleNo;
                    $scope.ItemID = response.data.ItemID;
                    $scope.Remarks = response.data.Remarks;
                    $scope.txtbxSetNo = response.data.SetNo;
                    $scope.SetID = response.data.SetID;
                    $scope.WeavingMRRID = response.data.WeivingMRRID;
                    $scope.SizeMRRID = response.data.SizeMRRID;
                    $scope.txtbxDate = conversion.getDateToString(response.data.FabricInspectionDate);
                    $scope.SetDrpDwn(response);
                    //$scope.getFabricInspectionDetailsbyID(dataModel)
                }
            },
            function (error) {
                console.log("Error: " + error);
            });

        }

        $scope.SetDrpDwn = function (response) {

            $scope.FinishingMRRID = response.data.FinishingMRRID;
            if (response.data.FinishingMRRNo != 'N/A') {
                $("#drpStyleNO").select2("data", { id: 0, text: response.data.FinishingMRRNo });
            } else {
                $("#drpStyleNO").select2("data", { id: 0, text: '-- Select Style No --' });
            }

            $scope.drpShift = response.data.ShiftID;
            if (response.data.ShiftName != 'N/A') {
                $("#drpShift").select2("data", { id: 0, text: response.data.ShiftName });
            } else {
                $("#drpShift").select2("data", { id: 0, text: '-- Select Shift --' });
            }

            $scope.drpPlateNo = response.data.PlateID;
            if (response.data.PlateNo != 'N/A') {
                $("#drpPlateNo").select2("data", { id: 0, text: response.data.PlateNo });
            } else {
                $("#drpPlateNo").select2("data", { id: 0, text: '-- Plate No --' });
            }

            $scope.drpMachineNo = response.data.MachineConfigID;
            if (response.data.MachineConfigNo != 'N/A') {
                $("#drpMachineNo").select2("data", { id: 0, text: response.data.MachineConfigNo });
            } else {
                $("#drpMachineNo").select2("data", { id: 0, text: '-- Machine No --' });
            }

            $scope.drpOperator = response.data.OperatorID;
            if (response.data.UserFullName != 'N/A') {
                $("#drpOperator").select2("data", { id: 0, text: response.data.UserFullName });
            } else {
                $("#drpOperator").select2("data", { id: 0, text: '-- Select Operator --' });
            }

        }

        $scope.getFabricInspectionDetailsbyID = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.InspactionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'GetFebricInspectionDetailsID/';
            var _febricIndpection = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.get);
            _febricIndpection.then(function (response) {
                if (response.data != null) {
                    $scope.FebricInspectionDetailsList = response.data;
                    //$scope.IsHidden = $scope.FebricInspectionDetailsList.length > 0 ? true : false;
                    $scope.IsShow = $scope.FebricInspectionDetailsList.length > 0 ? true : false;
                    if ($scope.FebricInspectionDetailsList.length > 0) { $scope.cmnbtnShowHideEnDisable('false'); } else { $scope.cmnbtnShowHideEnDisable('true'); }
                }
            },
            function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.ShowHide = function () {
            debugger
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;

            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
            }
        };

        $scope.Save = function () {
            debugger
            $scope.cmnParam();
            var FinishingMaster = {
                InspactionID: $scope.UserCommonEntity.message == "Saved" ? 0 : $scope.InspactionID,
                FinishingMRRID: $scope.FinishingMRRID,
                ItemID: $scope.ItemID,
                SetID: $scope.SetID,
                WeivingMRRID: $scope.WeavingMRRID,
                SizeMRRID: $scope.SizeMRRID,
                MachineConfigID: $scope.drpMachineNo,
                ShiftID: $scope.drpShift,
                PlateID: $scope.drpPlateNo,
                OperatorID: $scope.drpOperator,
                Remarks: $scope.Remarks,
                Date: $scope.txtbxDate === "" ? null : conversion.getStringToDate($scope.txtbxDate)
            };
            var HeaderTokenPutPost = $scope.UserCommonEntity.message == "Saved" ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            ModelsArray = [FinishingMaster, $scope.FebricInspectionDetailsList, objcmnParam];//, $scope.DeleteFebricIndpectionList
            var apiRoute = baseUrl + 'SaveUpdateFebricInspection/';
            var SaveInternalIssue = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveInternalIssue.then(function (response) {
                if (response != '') {
                    $scope.clear();
                    Command: toastr["success"]("Data " + $scope.UserCommonEntity.message + " Successfully!!!!");
                }
                else {
                    Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"]("Data Not " + $scope.UserCommonEntity.message + ", Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }

        //*********************************************************Start Save/Update/Delete**********************************************************
        $scope.DeleteUpdateMasterDetail = function (delModel) {
            debugger
            $scope.cmnParam();
            objcmnParam.id = delModel.InspactionID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateFabricInspectionMasterDetail/';
            var SetWiseMachineSetupMasterDetailDelete = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            SetWiseMachineSetupMasterDetailDelete.then(function (response) {
                if (response.data.result != '') {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](delModel.InspactionNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](delModel.InspactionNo + " Not Deleted, Please Check and Try Again!");
                }
            }, function (error) {
                Command: toastr["warning"](delModel.InspactionNo + " Not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        };
        //*********************************************************End Save/Update/Delete**********************************************************

        $scope.clear = function () {
            $scope.frmFebricInspection.$setPristine();
            $scope.frmFebricInspection.$setUntouched();
            $scope.InspactionID = "";
            $scope.FinishingMRRID = "";
            $scope.ItemID = "";
            $scope.txtbxADMLCode = "";
            $scope.txtbxColor = "";
            $scope.txtbxSetNo = "";
            $scope.Remarks = "";
            $scope.SetID = "";
            $scope.WeavingMRRID = "";
            $scope.SizeMRRID = "";
            $scope.drpMachineNo = "";
            $scope.drpShift = "";
            $scope.drpPlateNo = "";
            $scope.drpOperator = "";
            $scope.txtbxDate = conversion.NowDateCustom();
            $scope.FebricInspectionDetailsList = [];
            //$scope.DeleteFebricIndpectionList = [];
            $scope.gridOptions.data = "";
            $scope.IsShow = false;
            $scope.IsHidden = true;
            $("#drpStyleNO").select2("data", { id: 0, text: '-- Select Style No --' });
            $("#drpShift").select2("data", { id: 0, text: '-- Select Shift --' });
            $("#drpPlateNo").select2("data", { id: 0, text: '-- Plate No --' });
            $("#drpMachineNo").select2("data", { id: 0, text: '-- Machine No --' });
            $("#drpOperator").select2("data", { id: 0, text: '-- Select Operator --' });
        }
    }]);


//function modal_fadeOut() {
//    $("#userModal").fadeOut(200, function () {
//        $('#userModal').modal('hide');
//    });
//}