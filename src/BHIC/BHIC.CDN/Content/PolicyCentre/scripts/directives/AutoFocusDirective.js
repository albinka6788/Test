(function () {
    'use strict';

    //auto focus for input text box
    angular.module('BHIC.Dashboard.Directives').directive('autofocusfield', ['$timeout', autoFocusFn]);
    function autoFocusFn($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch(attrs.autofocusfield, function (newValue, oldValue) {
                    if (newValue) { element[0].focus(); }
                });
                element.bind("blur", function (e) {
                    $timeout(function () {
                        scope.$apply(attrs.autofocusfield + "=false");
                    }, 0);
                });
                element.bind("focus", function (e) {
                    $timeout(function () {
                        scope.$apply(attrs.autofocusfield + "=true");
                    }, 0);
                })
            }
        }
    }
})();