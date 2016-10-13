
/**
 * ABS_App.js
 */

var app;
(function () {
    'use strict';
    //app = angular.module("ABS_App", []);
    app = angular.module('ABS_App_Cm', ['ngStorage', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid.exporter']);
})();