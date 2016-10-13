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
        var Output = Day[0] + "-" + Month + "-" + Year;
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
        var Nowdate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
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

    //this.UserCmnEntity = function (UCEntity) {
    //    debugger
    //    return UCEntity;
    //}

    this.cmnParams = function (cmnEntity) {
        debugger
        var cmnParam = {};
        cmnParam = {
            pageNumber: 1,//--------------will start
            pageSize: 15,
            IsPaging: 0,
            loggeduser: cmnEntity.loggedUserID,
            loggedCompany: cmnEntity.loggedCompnyID,
            menuId: cmnEntity.currentMenuID,
            tTypeId: cmnEntity.currentTransactionTypeID,
            DepartmentID: cmnEntity.loggedUserDepartmentID,
            ItemType: 0,
            ItemGroup: 0,
            id: 0,
            ParamName: "",
            IsTrue: false,
            UserType: 0,
            selectedCompany: 0
        };
        return cmnParam;
    }

    HeaderTokens = {};
    this.Tokens = function (tokenManager, UCEntitys) {
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