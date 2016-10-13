/**
 * Conversion.js
 */

app.service('conversion', function ($filter) {

    //**********----Get All Record----***************  

    //************************************Start Convert String to Date Formate*****************************
    this.getStringToDate = function (InputString) {
        debugger
        var newStringToDate = InputString.replace(/[/-]/g, '-');
        var StringToDate = newStringToDate;
        var SplitedDate = StringToDate.split("-");
        var Day = SplitedDate[0];
        var Month = SplitedDate[1];
        var Year = SplitedDate[2];
        var FullFormateDate = Month + "-" + Day + "-" + Year;
        var Output = $filter('date')(new Date(), FullFormateDate);
        return Output;
    }
    //**************************************End Convert String to Date Formate*****************************

    //************************************Start Convert Date Formate to String*****************************
    this.getDateToString = function (InputDate) {
        debugger
        var DateToString = InputDate;
        var SplitedDate = DateToString.split("-");
        var Year = SplitedDate[0];
        var Month = SplitedDate[1];
        var Day = SplitedDate[2].split("T");
        var Output = Day[0] + "/" + Month + "/" + Year;
        return Output;
    }
    //**************************************End Convert Date Formate to String*****************************

    //**********************************Start Calculate Day Different between two Date (Day)*********************
    this.dateCalculationInDays = function (firstdate, seconddate) {
        debugger
        var dtFirst = firstdate;
        var dtSecond = seconddate;

        var dtFirstSplit = dtFirst.split('-'),
        dtSecondSplit = dtSecond.split('-'),

        dtFirstNew = new Date(dtFirstSplit[2], dtFirstSplit[1], dtFirstSplit[0]),
        dtSecondNew = new Date(dtSecondSplit[2], dtSecondSplit[1], dtSecondSplit[0]);

        var millisecondsPerDay = 1000 * 60 * 60 * 24;
        var millisBetween = dtSecondNew.getTime() - dtFirstNew.getTime();
        var days = millisBetween / millisecondsPerDay;

        return Math.floor(days);
    };
    //************************************End Calculate Day Different between two Date (Day)*********************

    //**********************************Start Minutes Different between two time (Minute)************************
    this.TimeDifferentInMinutes = function (start, end) {//Not applicable
        debugger
        return moment.utc(moment(end).diff(moment(start))).format("mm")
    }

    this.getMinutesBetweenDates = function (startDate, endDate) {
        debugger
        var diff = startDate.getTime() - endDate.getTime();
        return (diff / 60000);
    }
    //************************************End Minutes Different between two time (Minute)************************

    this.dateCalculationInDate = function (firstdate, seconddate) {//Not Completed
        debugger
        var dtFirst = firstdate;
        var dtSecond = seconddate;

        var dtFirstSplit = dtFirst.split('-'),
        dtSecondSplit = dtSecond.split('-'),

        dtFirstNew = new Date(dtFirstSplit[2], dtFirstSplit[1], dtFirstSplit[0]),
        dtSecondNew = new Date(dtSecondSplit[2], dtSecondSplit[1], dtSecondSplit[0]);

        var millisecondsPerDay = 1000 * 60 * 60 * 24;
        var millisBetween = dtSecondNew.getTime() - dtFirstNew.getTime();
        var days = millisBetween / millisecondsPerDay;

        var day = Math.floor(days);

        return day;
    }

    this.getDateTimeToTimeSpan = function (InputDate) {
        debugger
        var DateToString = InputDate;
        var SplitedDate = DateToString.split("-");
        var Year = SplitedDate[0];
        var Month = SplitedDate[1] - 1;
        var Days = SplitedDate[2].split("T");
        var Day = Days[0];
        var Times = Days[1].split(":");
        var Hour = Times[0];
        var Minute = Times[1];

        var OutputTime = new Date(Year, Month, Day, Hour, Minute, 0);

        return OutputTime;
    }

    this.get24HourFromPM = function (InputTime) {
        debugger
        var TimeString = InputTime;
        var SplitedTime = TimeString.split(":");
        var Hour = SplitedTime[0];
        var Minutess = SplitedTime[1];
        var Minutes = Minutess.split(" ");
        var Minute = Minutes[0];
        var AMPM = Minutes[1];

        if (AMPM == "PM") {
            Hour = parseInt(Hour) == 12 ? 12 : (parseInt(Hour) + parseInt(12));
        }
        else {
            Hour = parseInt(Hour) == 12 ? 00 : Hour;
        }
        var Output24 = ('0' + Hour).slice(-2) + ":" + ('0' + Minute).slice(-2);
        return Output24;
    }

    this.get12HourFrom24 = function (InputTime) {
        debugger
        var TimeString = InputTime;
        var SplitedTime = TimeString.split(":");
        var Hour = SplitedTime[0];
        var Minute = SplitedTime[1];
        var AMPM = "";
        if (parseInt(Hour) > 11) {
            Hour = parseInt(Hour) == 12 ? 12 : (parseInt(Hour) - parseInt(12));
            AMPM = "PM";
        }
        else {
            Hour = parseInt(Hour) == 0 ? 12 : Hour;
            AMPM = "AM";
        }
        var Output12 = ('0' + Hour).slice(-2) + ":" + ('0' + Minute).slice(-2) + " " + AMPM;
        return Output12;
    }

    this.NowTime = function () {
        debugger
        var Dates = new Date();
        var todayD = "01";
        var todayMon = "00";
        var todayY = "1900";
        var todayH = Dates.getHours();
        var todayMin = Dates.getMinutes();

        var Nowtime = new Date(todayY, todayMon, todayD, todayH, todayMin, 0);
        return Nowtime;
    }

    this.GetDateTimeFromTime = function (time) {
        var tArray = time.split(":");
        var Dates = new Date();
        var todayD = "01";
        var todayMon = "00";
        var todayY = "1900";
        var todayH = tArray[0];
        var todayMin = tArray[1];
        var Nowtime = new Date(todayY, todayMon, todayD, todayH, todayMin, 0);
        return Nowtime;
    }

    this.ChangeDateTime = function (IsTrue, Hour, Minute) {
        debugger
        var ChangeY = 1900;
        var ChangeMon = 0;
        var ChangeD = IsTrue == true ? 2 : 1;
        var Day = ChangeD;
        var ChangeH = Hour;
        var ChangeMin = Minute;

        var ChangedTime = new Date(ChangeY, ChangeMon, Day, ChangeH, ChangeMin, 0);
        return ChangedTime;
    }

    this.NowDateCustom = function () {
        var date = new Date();
        var Nowdate = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
        return Nowdate;
    }

    this.NowDateDefault = function () {
        debugger
        var date = new Date();
        var Nowdate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
        return Nowdate;
    }

    this.UnlimitedSplite = function (Input) {
        debugger
        var ArrayItem = [];

        var InputItem = Input;
        var SplitedItem = InputItem.split(":");
        for (var s = 0; s < SplitedItem.length; s++) {
            ArrayItem.push({ Items: SplitedItem[s] });
        }
        debugger
        return ArrayItem;
    }

    this.getDynamicModels = function (models) {
        debugger
        var model = [];
        for (var i = 0; i < models.length; i++) {
            if (i == models.length - 1) {
                if (models.length == 1) {
                    if (models[i].length > 0) {
                        model.push("[" + JSON.stringify(models[i]));
                    }
                    else {
                        model.push("[" + JSON.stringify(models[i]) + "]");
                    }
                }
                else {
                    if (models[i].length > 0) {
                        model.push(JSON.stringify(models[i]));
                    }
                    else {
                        model.push(JSON.stringify(models[i]) + "]");
                    }
                }
            }
            else {
                if (models[i].length > 0) {
                    model.push(JSON.stringify(models[i]) + ',');
                }
                else {
                    model.push("[" + JSON.stringify(models[i]) + ',');
                }
            }
        }
        strModel = {};
        strModel.model = model.join("");
        strModel.Token = HeaderTokens;
        return strModel;
    }

    this.roundNumber = function (number, precision) {
        debugger
        precision = Math.abs(parseInt(precision)) || 0;
        var multiplier = Math.pow(10, precision);
        return (Math.round(number * multiplier) / multiplier);
    }

    this.setMaxMin = function (setNum, min, max) {
        debugger
        if (angular.isUndefined(setNum) || setNum == null || setNum < min) {
            setNum = min;
        }
        if ((!angular.isUndefined(setNum) || setNum != '' || setNum != null) && (max != 0 && setNum > max)) {
            setNum = max;
        }
        var setnums = setNum.toString();
        setnums = setnums.replace(/[-]/g, '');
        return parseInt(setnums);
    }

    this.cmnParams = function () {
        debugger
        var cmnParam = {};
        cmnParam = {
            pageNumber: 1,//--------------will start
            pageSize: 15,
            IsPaging: 0,
            loggeduser: UCEntitys.loggedUserID,
            loggedCompany: UCEntitys.loggedCompnyID,
            menuId: UCEntitys.currentMenuID,
            tTypeId: UCEntitys.currentTransactionTypeID,
            DepartmentID: UCEntitys.loggedUserDepartmentID,
            ItemType: 0,
            ItemGroup: 0,
            id: 0,
            ParamName: "",
            IsTrue: false,
            UserType: 0,
            selectedCompany: 0,
            FromDate: null,
            ToDate: null
        };
        return cmnParam;
    }

    HeaderTokens = {};
    this.Tokens = function (tokenManager) {
        generateSecurityParam = {};
        generateSecurityParam.MenuID = UCEntitys.currentMenuID;
        generateSecurityParam.CompanyID = UCEntitys.loggedCompnyID;
        HeaderToken = {};
        generateSecurityParam.methodtype = 'get';
        HeaderToken.get = { 'AuthorizedToken': tokenManager.generateSecurityToken(generateSecurityParam) };
        generateSecurityParam.methodtype = 'put';
        HeaderToken.put = { 'AuthorizedToken': tokenManager.generateSecurityToken(generateSecurityParam) };
        generateSecurityParam.methodtype = 'post';
        HeaderToken.post = { 'AuthorizedToken': tokenManager.generateSecurityToken(generateSecurityParam) };
        generateSecurityParam.methodtype = 'delete';
        HeaderToken.delete = { 'AuthorizedToken': tokenManager.generateSecurityToken(generateSecurityParam) };
        HeaderTokens = HeaderToken;
        return HeaderToken;
    }

    UCEntitys = {};
    this.UserCmnEntity = function (UCEntity, FormName, DelFunc, DelMsg, EditFunc) {
        debugger
        UCEntity.EnableSavebtn = FormName == "" ? true : false;
        UCEntity.EnableSavefrm = FormName == "" ? false : true;
        UCEntity.frmName = FormName == "" ? "" : FormName + ".$invalid";
        UCEntity.DelFunc = DelFunc;
        UCEntity.DelMsg = DelMsg;
        UCEntity.EditFunc = EditFunc;

        UCEntity.Save = "Save";
        UCEntity.ShowHide = "ShowHide";
        UCEntity.Reset = "clear";
        UCEntity.Print = "Print";

        UCEntity.EnableSave = UCEntity.EnableInsert;
        UCEntity.EnableModify = UCEntity.EnableUpdate;
        UCEntity.EnableShow = true;

        if (UCEntity.EnableSave == true && UCEntity.EnableModify == true) {
            UCEntity.EnableEView = false;
            UCEntity.EnableReset = true;
            UCEntity.EnableEdit = true;

        }
        else if (UCEntity.EnableSave == true && UCEntity.EnableModify == false) {
            UCEntity.EnableEView = true;
            UCEntity.EnableReset = true;
            UCEntity.EnableEdit = false;
        }
        else if (UCEntity.EnableSave == false && UCEntity.EnableModify == true) {
            UCEntity.EnableEView = false;
            UCEntity.EnableReset = true;
            UCEntity.EnableEdit = true;
        }
        else {
            UCEntity.EnableEView = true;
            UCEntity.EnableReset = false;
            UCEntity.EnableEdit = false;
        }
        if (UCEntity.EnableSave == true && UCEntity.EnableModify == true) {
            UCEntity.EnableSave = false;
            UCEntity.EnableModify = false;
            UCEntity.EnableSaveUpdate = true;
        }
        else {
            UCEntity.EnableSaveUpdate = false;
        }
        UCEntitys = UCEntity;
        UCEntitys.cellTemplate = cellTemplate;
        UCEntitys.rowTemplate = rowTemplate;
        UCEntitys.visible = visible;
        return UCEntity;
    }
    State = 'Start';
    this.btnBehave = function (nums, disableMode) {
        debugger
        btnPerm = {};
        if (State == 'Start') {
            EnableModify = UCEntitys.EnableModify == true ? UCEntitys.EnableModify : false;
            EnableSave = UCEntitys.EnableSave == true ? UCEntitys.EnableSave : false;
            EnableSaveUpdate = UCEntitys.EnableSaveUpdate == true ? UCEntitys.EnableSaveUpdate : false;
            State = 'End';
        }

        if (nums != 0 && nums != 1 && nums != 2 && nums != 3 && nums != 4 && nums != 5) { nums = nums.toString().toLowerCase(); }
        if (nums == 't' || nums == 'tr' || nums == 'tru' || nums == 'true' || nums == 'tu' || nums == 'tue' || nums == 'ture' || nums == 'turu' || nums == 'turue') { nums = 'true' }
        if (nums == 'f' || nums == 'fa' || nums == 'fal' || nums == 'fals' || nums == 'false' || nums == 'flse' || nums == 'fales' || nums == 'fasle' || nums == 'fles') { nums = 'false' }
        if (nums == 'true' || nums == 'false') { if (nums == 'true') { num = 4 } else if (nums == 'false') { num = 3 } } else { num = nums; }

        if (num == 0 || num == 1 || num == 2 || num == 3 || num == 4 || num == 5) {
            btnPerm.IsContinue = true;
            btnPerm.clear = num == 0 ? 1 : 0;
            btnPerm.show = num == 1 ? 1 : 0;
            btnPerm.save = num == 2 ? 1 : 0;
            btnPerm.isDefault = num == 3 ? 1 : 0;

            if (num != 4 && num != 5) {
                UCEntitys.EnableShow = btnPerm.show != 0 && (UCEntitys.EnableEView == true || EnableModify == true) && EnableSave == false ? false : true;
                UCEntitys.btnShowList = btnPerm.isDefault == 0 ? btnPerm.show == 0 ? "Show List" : "Create" : UCEntitys.btnShowList;
                UCEntitys.btnSaveText = btnPerm.isDefault == 0 ? btnPerm.save == 0 ? "Save" : "Update" : UCEntitys.btnSaveText;
                UCEntitys.message = btnPerm.isDefault == 0 ? btnPerm.save == 0 ? "Saved" : "Updated" : UCEntitys.message;

                if (UCEntitys.EnableSavebtn == true && btnPerm.clear == 0) {
                    btnPerm.IsbtnSaveDisable = disableMode;
                    if (EnableSave == true && UCEntitys.message == "Saved") {
                        btnPerm.IsbtnSaveDisable = false;
                    }
                    else if (EnableSave == true && UCEntitys.message == "Saved") {
                        btnPerm.IsbtnSaveDisable = true;
                    }
                    else if (EnableModify == true && UCEntitys.message == "Saved") {
                        btnPerm.IsbtnSaveDisable = true;
                    }
                    else if (EnableModify == true && UCEntitys.message == "Updated") {
                        btnPerm.IsbtnSaveDisable = false;
                    }
                    else if (EnableSaveUpdate == true) {
                        btnPerm.IsbtnSaveDisable = false;
                    }
                    else {

                        btnPerm.IsbtnSaveDisable = btnPerm.isDefault == 1 ? btnPerm.IsbtnSaveDisable : true;
                    }
                }

                if (UCEntitys.EnableSavebtn == true && btnPerm.clear != 0) { btnPerm.IsbtnSaveDisable = true }

                if (UCEntitys.btnShowList == 'Show List') {
                    $('#Icon-Show').removeClass('icon-plus-sign');
                    $('#Icon-Show').addClass('icon-search');
                    if (EnableSave == true && btnPerm.save == 0 && UCEntitys.message == "Saved") { UCEntitys.EnableSave = true; }
                    if (EnableModify == true && btnPerm.save != 0 && UCEntitys.message == "Updated") { UCEntitys.EnableModify = true; }
                    else if (EnableModify == true && btnPerm.save == 0 && UCEntitys.message == "Saved") { UCEntitys.EnableModify = false; }
                    if (EnableSaveUpdate == true) { UCEntitys.EnableSaveUpdate = true; }
                    //$('#saveupdate').show();
                    $('#reset').show();
                    $('#frmEntry').show();
                    $('#frmDetail').show();
                    $('#frmList').hide();
                }
                else {
                    $('#Icon-Show').removeClass('icon-search');
                    $('#Icon-Show').addClass('icon-plus-sign');
                    if (EnableSave == true) { UCEntitys.EnableSave = false; }
                    if (EnableModify == true) { UCEntitys.EnableModify = false; }
                    if (EnableSaveUpdate == true) { UCEntitys.EnableSaveUpdate = false; }
                    $('#reset').hide();
                    $('#frmEntry').hide();
                    $('#frmDetail').hide();
                    $('#frmList').show();
                }
                if (EnableSaveUpdate == true) {
                    if (UCEntitys.btnSaveText == 'Save') {
                        //$('#IconSave').removeClass('icon-edit');
                        //$('#IconSave').addClass('icon-save');
                        UCEntitys.SaveIcon = true;
                        UCEntitys.EditIcon = false;
                    }
                    else {
                        //$('#IconSave').removeClass('icon-save');
                        //$('#IconSave').addClass('icon-edit');
                        UCEntitys.SaveIcon = false;
                        UCEntitys.EditIcon = true;
                    }
                }
            }
            else {
                if (UCEntitys.EnableSavebtn == true) { btnPerm.IsbtnSaveDisable = num == 4 ? true : false; }
            }

            if (UCEntitys.EnableSavebtn == true) { UCEntitys.IsbtnSaveDisable = btnPerm.IsbtnSaveDisable; }

        }
        else {
            btnPerm.IsContinue = false;
        }
        UCEntitys.IsContinue = btnPerm.IsContinue;
        return UCEntitys;
    }

    this.ExecuteCmnFunc = function (funcEntity, cmnNum) {
        debugger
        cmnParamList = {};
        if (cmnNum != 0 && cmnNum != 2) {
            cmnParamList = this.getNumbyFunc(funcEntity);
            if (cmnNum != 3 && cmnNum != 1) { this.btnBehave(cmnParamList.num, UCEntitys.IsbtnSaveDisable); }
        }
        else if (cmnNum == 2) {
            cmnParamList = this.EditByIDService(UCEntitys.EditFunc, funcEntity);
            this.btnBehave(2, UCEntitys.IsbtnSaveDisable);
        }
        else if (cmnNum == 0) {
            cmnParamList = this.DelByIDService(UCEntitys.DelFunc, funcEntity, UCEntitys.DelMsg);

        }
        return cmnParamList;
    }

    this.getNumbyFunc = function (func) {
        debugger
        MethodNum = {};
        MethodNum.num = UCEntitys.ShowHide == func && UCEntitys.btnShowList == "Show List" ? 1 : 0;
        //MethodNum.toastr = UCEntitys.Save == func ? 1 : 0;
        MethodNum.rowEntity = UCEntitys.rowEntity != null ? UCEntitys.rowEntity : 0;
        MethodNum.MethodName = func;

        return MethodNum;
    }

    this.EditByIDService = function (Method, Entity) {
        debugger
        returnList = {};
        var MethodList = Method.replace(/[/-;,.]/g, '-');
        returnList.rowEntity = Entity;
        returnList.MethodName = MethodList.split("-");
        for (i = 0; i < returnList.MethodName.length; i++) {
            returnList.MethodName[i] = returnList.MethodName[i].trim();
        }
        return returnList;
    }

    this.DelByIDService = function (Method, Entity, DelMsg) {
        debugger
        UCEntitys.rowEntity = Entity;
        UCEntitys.DelMsgs = "You are about to delete " + Entity[DelMsg] + ". Are you sure?";
        UCEntitys.MethodName = Method.trim();
        UCEntitys.EnableYes = true;
        UCEntitys.EnableConf = false;
        $('#CmnDeleteModal').modal({ show: true, backdrop: "static", keyboard: "false" });
        return UCEntitys;
    }

    var visible = UCEntitys.EnableEdit == false && UCEntitys.EnableDelete == false && UCEntitys.EnableEView == false ? false : true;
    var rowTemplate = '<div ng-dblclick="grid.appScope.CmnMethod(row.entity,2)" ng-repeat="col in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>'
    var cellTemplate = '<span class="label label-success label-mini" ng-if="grid.appScope.UserCommonEntity.EnableEView">' +
                          '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.CmnMethod(row.entity,2)">' +
                          '<i class="glyphicon glyphicon-eye-open" aria-hidden="true">&nbsp;View</i> </a> </span>' +

                          '<span class="label label-success label-mini" ng-if="grid.appScope.UserCommonEntity.EnableEdit">' +
                          '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Edit Info" ng-click="grid.appScope.CmnMethod(row.entity,2)">' +
                          '<i class="glyphicon glyphicon-edit" aria-hidden="true">&nbsp;Edit</i></a></span>' +

                          '<span class="label label-warning label-mini" style="text-align:center !important" ng-if="grid.appScope.UserCommonEntity.EnableDelete">' +
                          '<a href="javascript:void(0);" data-toggle="modal" class="bs-tooltip" title="Delete" ng-click="grid.appScope.CmnMethod(row.entity,0)">' +
                          '<i class="glyphicon glyphicon-trash" aria-hidden="true">&nbsp;Delete</i></a></span>'

    //'<span class="label label-warning label-mini" style="text-align:center !important" ng-if="grid.appScope.UserCommonEntity.EnableDelete">' +
    //'<a href="javascript:void(0);" ng-href="#CmnDeleteModal" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="bs-tooltip" title="Delete" ng-click="grid.appScope.DelByID(row.entity,0)">' +
    //'<i class="glyphicon glyphicon-trash" aria-hidden="true">&nbsp;Delete</i></a></span>'


    //this.jsFilter = function (filterList, IDField, NameField, CompareFieldName) {
    //    debugger
    //    var NewLists = [];
    //    var filterLists = [];

    //    angular.forEach(filterList, function (item) {
    //        NewLists.push({ ID: item.IDField, Name: item.NameField });
    //    });

    //    angular.forEach(NewLists, function (item) {
    //        if (CompareFieldName == item.ID) {
    //            filterLists.push({ id: item.ID, name: item.Name })
    //        }
    //    });

    //    return filterLists;
    //}
});