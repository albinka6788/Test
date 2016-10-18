(function () {
    'use strict';
    angular.module('BHIC.LandingPage.Directives').directive('numbersOnly', dateControlFn);
    function dateControlFn() {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    if (text) {
                        var transformedInput = text.replace(/[^0-9]/g, '');

                        if (transformedInput !== text) {
                            ngModelCtrl.$setViewValue(transformedInput)
                            ngModelCtrl.$render();
                        }
                        if (transformedInput.length < 5) {
                            scope.errorText = "Please enter valid zip code";
                            return scope.zipflag = false;
                        }
                        if (transformedInput.length == 5) {
                             scope.zipflag = false;
                        }
                        return (transformedInput.length == 5) ? scope.ValidateZipCode(transformedInput) : null;
                    }
                    return scope.zipflag = false;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        }
    };
})();
