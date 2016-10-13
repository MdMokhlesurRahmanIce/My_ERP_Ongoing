/**
 * validNumber.js
 */
app.directive('validNumber', function () {
     
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }
            
            ngModelCtrl.$parsers.push(function (val) {
                 debugger
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^-0-9\.]/g, '');
                var negativeCheck = clean.split('-');
                var decimalCheck = clean.split('.');
                if (!angular.isUndefined(negativeCheck[1])) {
                    negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                    clean = negativeCheck[0] + '-' + negativeCheck[1];
                    if (negativeCheck[0].length > 0) {
                        clean = negativeCheck[0];
                    }
                }
                if (angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = "00";
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }
                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 2);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }

                function isPositiveInteger(n) {
                    return n >>> 0 === parseFloat(n);
                }
                
              
                debugger
                if (val == '') {
                    clean = "0.00";
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});


app.directive('nksOnlyNumber', function () {
    return {
        restrict: 'EA',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                var spiltArray = String(newValue).split("");
                alert("Do");
                if (attrs.allowNegative == "false") {
                    if (spiltArray[0] == '-') {
                        newValue = newValue.replace("-", "");
                        ngModel.$setViewValue(newValue);
                        ngModel.$render();
                    }
                }

                if (attrs.allowDecimal == "false") {
                    newValue = parseInt(newValue);
                    ngModel.$setViewValue(newValue);
                    ngModel.$render();
                }

                if (attrs.allowDecimal != "false") {
                    if (attrs.decimalUpto) {
                        var n = String(newValue).split(".");
                        if (n[1]) {
                            var n2 = n[1].slice(0, attrs.decimalUpto);
                            newValue = [n[0], n2].join(".");
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }
                    }
                }


                if (spiltArray.length === 0) return;
                if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                if (spiltArray.length === 2 && newValue === '-.') return;

                /*Check it is number or not.*/
                if (isNaN(newValue)) {
                    ngModel.$setViewValue(oldValue);
                    ngModel.$render();
                }
            });
        }
    };
});
