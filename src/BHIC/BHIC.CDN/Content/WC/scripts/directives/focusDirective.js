(
    function () {
        'use strict';

        //directive declaration
        angular.module('BHIC.WC.Directives').directive('focus', ['$timeout', setFocusFn]);

        //directive implementation logic method
        function setFocusFn($timeout) {
            return {
                link: function (scope, element, attributes) {
                    scope.$watch(attributes.focus, function (value) {
                        if (value == true) {
                            $timeout(function () {
                                element[0].focus();
                                return scope[attributes.focus] = false;
                            });
                        }
                    });
                }
            };
        }
    }
)();