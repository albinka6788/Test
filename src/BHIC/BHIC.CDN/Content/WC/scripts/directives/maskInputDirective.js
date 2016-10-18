(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.Directives').directive('maskInput', dateControlFn);
    function dateControlFn() {
        return {
            restrict: 'AE',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                var blankValue = attrs.maskFormat.replace(/\9/g, '_');
                
                element.inputmask("mask", { "mask": attrs.maskFormat })
                .on("blur", function (e) {
                    var value = element.val();
                    if (attrs.required) {
                        if (value == "" || value == blankValue) {
                            ngModelCtrl.$setValidity('required', false);
                        }
                        else
                            ngModelCtrl.$setValidity('required', true);
                    }
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(value);
                    });
                });
                
            }
        };
    }
})();
