
/**
 * ABS_App.js
 */
var app;
(function () {
    'use strict';
    //app = angular.module("ABS_App", ["ngStorage"]);
    //app = angular.module('ABS_App_Sc', ['ngStorage', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.grouping', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid.exporter']);
    app = angular.module('ABS_App_Sc', ['ngStorage', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.grouping', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid.exporter', 'angularTreeview']);
})();