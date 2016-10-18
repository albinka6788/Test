(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.WC.Directives').directive('numericOnly', function () {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val)
                {
                    if (angular.isUndefined(val) || val == null || val == 'null') {
                        var val = '';
                    }
                    else {
                        //Comment : Here added below logic to check "-0" value then avoid .toString()
                        if (!(val === 0 || val === '-0')) {
                            val = val.toString();
                        }
                        else {
                            val = '-0';
                        }
                    }
                    var clean = val.replace(/[^0-9]+/g, '');
                    if (val !== clean) {
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

})();

