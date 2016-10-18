(function () {
    'use strict';

    //valid number directive definition
    angular.module('bhic-app.wc.purchaseGet').directive('numericValidation', [numericValidationControlFn]);

    function numericValidationControlFn() {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                if (!ngModel) return;
                ngModel.$parsers.unshift(function (inputValue) {
                    var digits = inputValue.split('').filter(function (s) { return (!isNaN(s) && s != ' '); }).join('');
                    ngModel.$viewValue = digits;
                    ngModel.$render();
                    return digits;
                });
            }
        };
    }
})();