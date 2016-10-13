/**
 * ShiftCtrl.js
 */

app.controller('shiftCtrl', function ($scope, $http, crudService, $filter) {

    var isExisting = 0;
    var page = 1;
    var pageSize = 20;
    var isPaging = 0;
    var inCallback = false;
    var totalData = 0;

    var LUserID = $('#hUserID').val();
    var LCompanyID = $('#hCompanyID').val();
    $scope.EditUserID = 0;

    $scope.PanelTitle = 'New User';
    $scope.DataPanelTitle = 'Existing User';

    $scope.loaderMore = false;
    $scope.scrollended = false;

});

function modal_fadeOut() {
    $("#shiftModal").fadeOut(200, function () {
        $('#shiftModal').modal('hide');
    });
}