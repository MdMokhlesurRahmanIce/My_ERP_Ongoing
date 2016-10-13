$(function () {

    HelperDropDowns();
    HelperCascadingDropDowns();
    InitButtons();

    $('.datetimepicker').datepicker({
        format: "dd/mm/yyyy",
        todayHighlight: true,
        autoclose: true
    });
    $('.datetimepicker').datepicker('setDate', new Date());


    $('.datetimepickeredit').datepicker({
        format: "dd/mm/yyyy",
        todayHighlight: true,
        autoclose: true
    });



    //$('.select2').select2({
    //    placeholder: "Select",
    //    allowClear: true
    //});


});

function InitButtons() {
    $('.AddRow:not(.AddRowInited)').on("click", function () {
        var url = $(this).attr('data-url');
        var container = $(this).attr('data-container');
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            success: function (html) {
                $("#" + container).append(html);
            }
        });
        return false;
    }).addClass("AddRowInited");

    $('.RemoveRow:not(.RemoveRowInited)').on("click", function () {
        $(this).parents("div.row:first").remove();
        return false;
    }).addClass("RemoveRowInited");
}
function HelperDropDowns() {
    var dropdownElements = $('select.Dropdown:not(.DropdownInited)');
    $.each(dropdownElements, function (index, element) {
        var dropdownEl = $(element);
        dropdownEl.append($('<option></option>').val('').html(''));
        dropdownEl.select2("val", '');
        $(dropdownEl).select2({
            placeholder: "Select",
            allowClear: true
        });
        var url = dropdownEl.attr('data-url');
        var selected = dropdownEl.attr('data-selected');
        var dataCache = dropdownEl.attr('data-cache') ? true : false;
        $.ajax({
            url: url,
            type: 'GET',
            cache: dataCache,
            success: function (jsonData, textStatus, XMLHttpRequest) {
                var listitems = "<option value=''> </option>";

                var selectedValue = "";
                var selectedText = "";

                $.each(jsonData, function (i, item) {
                    if (selected && selected == item.Value) {
                        listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        selectedValue = item.Value;
                        selectedText = item.Text;
                    }
                    else {
                        listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    }
                });
                dropdownEl.html(listitems).addClass("DropdownInited");

                if (selected && selected == selectedValue) {
                    dropdownEl.select2("data", { id: selectedValue, text: selectedText });
                }

            }
        });

    });

}



function HelperCascadingDropDowns() {
    var dependentElements = $('select.Cascading:not(.DropdownInited)');
    $.each(dependentElements, function (index, element) {
        var dependentEl = $(element);
        dependentEl.append($('<option></option>').val('').html(''));
        dependentEl.select2("val", '');
        $(dependentEl).select2({
            placeholder: "Select",
            allowClear: true
        });
        var parentEl = $('#' + dependentEl.attr('data-parent'));
        var url = dependentEl.attr('data-url');
        var selected = dependentEl.attr('data-selected');
        var dataCache = dependentEl.attr('data-cache') ? true : false;
        var loadDropDownItems = function () {
            dependentEl.select2("val", '');
            if (!parentEl.val()) {
                dependentEl.empty();
                dependentEl.append($('<option></option>').val('').html(''));
                dependentEl.select2("val", '');
                if (selected) {
                    setTimeout(loadDropDownItems, 300);
                }
                return;
            }
            $.ajax({
                url: url + parentEl.val(),
                type: 'GET',
                cache: dataCache,
                success: function (jsonData, textStatus, XMLHttpRequest) {

                    var listitems = '<option></option>';
                    var selectedValue = "";
                    var selectedText = "";

                    $.each(jsonData, function (i, item) {
                        if (selected && selected == item.Value) {
                            listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                            selectedValue = item.Value;
                            selectedText = item.Text;
                        }
                        else {
                            listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        }
                    });
                    dependentEl.html(listitems).addClass("DropdownInited");

                    if (selected && selected == selectedValue) {
                        dependentEl.select2("data", { id: selectedValue, text: selectedText });
                    }

                }
            });
        };
        parentEl.change(loadDropDownItems);
        if (selected) {
            loadDropDownItems();
        }

    });
}


function CloseModal(el, dataAction, dataUrl) {
    if (dataAction == "formsubmit") {
        $("#" + dataUrl).submit();
    }
    else if (dataAction == "refreshparent") {
        window.parent.location = window.parent.location;
    }
    else if (dataAction == "refreshself") {
        window.location = window.location;
    }
    else if (dataAction == "redirect") {
        window.location = dataUrl;
    }
    else if (dataAction == "function") {
        eval(dataUrl);
    }
    var win = $(el).closest(".modal");
    win.modal("hide");
    setTimeout(function () {
        win.next(".modal-backdrop").remove();
        win.remove();
    }, 500);
}
function UYResult(msg, status, dataAction, dataUrl)
{
    var html = '' +
            '<div id="userUpdate" class="modal fade">' +
                '<div class="modal-dialog modal-sm">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header modal-title-' + status + '">' +
                            '<h4 class="modal-title">ABS</h4>' +
                        '</div>' +
                        '<div class="modal-body">' +
                            '<p>' + msg + '</p>' +
                        '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-primary" data-dismiss="modal" onclick="CloseModal(this, \'' + dataAction + '\',\'' + dataUrl + '\')">OK</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';

    var dialogWindow = $(html).appendTo('body');
    dialogWindow.modal({ backdrop: 'static' });
}

//function onBegin() {
//    $("#loading").fadeIn();
//    var opts = {
//        lines: 11, // The number of lines to draw
//        length: 11, // The length of each line
//        width: 5, // The line thickness
//        radius: 15, // The radius of the inner circle
//        color: '#4682B4', // #rgb or #rrggbb
//        speed: 1, // Rounds per second
//        trail: 60, // Afterglow percentage
//        shadow: false, // Whether to render a shadow
//        hwaccel: false // Whether to use hardware acceleration
//    };
//    var target = document.getElementById('loading');
//    var spinner = new Spinner(opts).spin(target);
//    $('#loading').show();
//}
//function onComplete() {
//    $("#loading").hide();
//}
$(document).ajaxStart(function () {
   /* $("#loading").fadeIn();
    var opts = {
        lines: 15, // The number of lines to draw
        length: 19, // The length of each line
        width: 5, // The line thickness
        radius: 15, // The radius of the inner circle
        color: '#4682B4', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('loading');
    var spinner = new Spinner(opts).spin(target);
    $('#loading').show();*/
    NProgress.start();

});

$(document).ajaxStop(function () {
    //$('#loading').hide();
    NProgress.done();

});


function isNumeric(keyCode) {
    return ((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 190 || (keyCode >= 96 && keyCode <= 105) || keyCode == 110);
}


function sumDebit(txtDebit, txtCredit) {
    var debitValue = $("#" + txtDebit).val();
    if (debitValue.length > 0) {
        $("#" + txtCredit).prop('disabled', true);
    }
    else {
        $("#" + txtCredit).prop('disabled', false);
        $("#" + txtDebit).val("0");
    }

    var table = $("#tbl tbody");
    var totalAmountSum = 0.0;
    table.find('tr').each(function (i) {
        var $tds = $(this).find('td');
        var debitAmount = $tds.find("input.debitAmount").val();
        totalAmountSum = totalAmountSum + parseFloat(debitAmount);
    });

    $("#TotalDebitAmount").attr("readonly", false);
    $('#TotalDebitAmount').val(totalAmountSum.toFixed(2));
    $("#TotalDebitAmount").attr("readonly", true);
}


function sumCredit(txtCredit, txtDebit) {
    var ccno = $("#" + txtCredit).val();
    if (ccno.length > 0) {
        $("#" + txtDebit).prop('disabled', true);

    }
    else {
        $("#" + txtDebit).prop('disabled', false);
        $("#" + txtCredit).val("0");
    }

    var table = $("#tbl tbody");
    var totalAmountSum = 0.0;
    table.find('tr').each(function (i) {
        var $tds = $(this).find('td');
        var creditAmount = $tds.find("input.creditAmount").val();
        totalAmountSum = totalAmountSum + parseFloat(creditAmount);
    });

    $('#TotalCreditAmount').val(totalAmountSum.toFixed(2));
}


function UYAsk(msg, dataAction, dataUrl) {
    var html = '' +
            '<div id="userConfirm" class="modal fade">' +
                '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header">' +
                            '<h4 class="modal-title">ABS</h4>' +
                        '</div>' +
                    '<div class="modal-body">' +
                        msg +
                    '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-primary" onclick="CloseModal(this, \'' + dataAction + '\',\'' + dataUrl + '\')">YES</button>' +
                            '<button type="button" class="btn btn-primary btn-close" onclick="CloseModal(this)">NO</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';
    var dialogWindow = $(html).appendTo('body');
    dialogWindow.modal({ backdrop: 'static' });
}


function DeleteRow(lnk) {

    var row = lnk.parentNode.parentNode;
    var rowIndex = row.rowIndex - 1;
    var myTableee = document.getElementById('tbl').tBodies[0];
    var tabRowCount = myTableee.rows.length;

    if (tabRowCount > 2) {
        var myTable = document.getElementById('tbl');
        myTable.deleteRow(rowIndex + 1);
        
        var table = $("#tbl tbody");
        var totalAmountSumD = 0.0;
        table.find('tr').each(function (i) {
            var $tds = $(this).find('td');
            var debitAmount = $tds.find("input.debitAmount").val();
            totalAmountSumD = totalAmountSumD + parseFloat(debitAmount);
        });

        $("input[id$='TotalDebitAmount']").attr("readonly", false);
        $("input[id$='TotalDebitAmount']").val(totalAmountSumD);
        $("input[id$='TotalDebitAmount']").attr("readonly", true);


        var totalAmountSumc = 0.0;
        table.find('tr').each(function (i) {
            var $tds = $(this).find('td');
            var creditAmount = $tds.find("input.creditAmount").val();
            totalAmountSumc = totalAmountSumc + parseFloat(creditAmount);
        });
        $("input[id$='TotalCreditAmount']").attr("readonly", false);
        $("input[id$='TotalCreditAmount']").val(totalAmountSumc);
        $("input[id$='TotalCreditAmount']").attr("readonly", true);


       
        UYResult("Row deleted successfully");
    } else {
        UYResult("At least two row needed");
    }
}
function CheckDuplicateRow1(accountLedgerId, x) {
  

    if ($('select[name="' + accountLedgerId + '"] option:selected').val() != '') {
        //  alert($('select[name="' + accountLedgerId + '"] option:selected').val());
        //   alert(x.parentNode.parentNode.rowIndex);


        var table = $("#tbl tbody");
        table.find('tr').each(function () {
            var $tds = $(this).find('td');
            var accountLedger = $tds.find("select.accountLedger").val();
        

            if (x.parentNode.parentNode.rowIndex != this.rowIndex) {
                if ($('select[name="' + accountLedgerId + '"] option:selected').val() == accountLedger) {
                    UYResult("Duplicate Account Ledger Not Allow in Different Row");
                    $('select[name="' + accountLedgerId + '"]').select2("val", '');
                }
            }
        });
    }

}


