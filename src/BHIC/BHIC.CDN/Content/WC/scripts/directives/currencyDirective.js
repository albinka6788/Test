(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('currencyControl', ['$timeout', '$parse', currencyControlFn]);
    function currencyControlFn($timeout, $parse) {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                function _applyMask() {
                    $timeout(function () { })
                    element.autoNumeric({
                        aSep: ',',
                        dGroup: '3',
                        altDec: null,
                        vMax: '999999999',
                        vMin: '0',
                        aForm: false
                    });
                    scope.$parent.disableSubmit = true;
                }

                element.on('change keydown paste input keyup', function ()
                {
                    //hold value of input control
                    var elementValue = $(this).val();

                    $timeout(function ()
                    {
                        ngModel.$setViewValue(elementValue);
                        ngModel.$commitViewValue();
                        _applyMask();
                    });
                });

                _applyMask();
                scope.$parent.disableSubmit = false;
            }
        };
    }
})();