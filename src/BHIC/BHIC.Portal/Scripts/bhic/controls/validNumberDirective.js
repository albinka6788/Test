(function () {
    'use strict';

    //Put any required dependencies
    var dependencies = [];

    //valid number directive definition
    angular.module('bhic-app.wc.controls').directive('validNumber', [validNumberControlFn]);

    function validNumberControlFn() {
        return {
            require: '?ngModel',
            link: link,
            scope: {
                data:"="
            }
        };
    
        function link(scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }
            ngModelCtrl.$parsers.push(function (val) {
                var clean;
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                if (attrs.decimalPlaces && attrs.decimalPlaces > 0) {
                    clean = val.replace(/[^0-9\.]/g, '');
                    var decimalCheck = clean.split('.');
                    if (!angular.isUndefined(decimalCheck[1])) {
                        decimalCheck[1] = decimalCheck[1].slice(0, parseInt(attrs.decimalPlaces));
                        clean = decimalCheck[0] + '.' + decimalCheck[1];
                    }
                }
                else {
                    clean = val.replace(/[^0-9]/g, '');
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                scope.data = clean;
                return clean;
            });
            if (attrs.minlength){
                $(element).blur(function () {
                    if ($(this).val().length !== parseInt(attrs.minLength)) {
                        $(this).addClass('ng-invalid').removeClass('ng-valid');
                    }
                    else {
                        $(this).addClass('ng-valid').removeClass('ng-invalid');
                    }
                });
            }
            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        };

    }
})();
