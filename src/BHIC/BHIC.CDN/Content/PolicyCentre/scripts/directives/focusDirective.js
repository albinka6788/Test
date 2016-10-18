(
    function () {
        'use strict';

        //directive declaration
        angular.module('BHIC.Dashboard.Directives').directive('focus', ['$timeout', setFocusFn]);

        //directive implementation logic method
        function setFocusFn($timeout) {
            return {
                scope: {
                    trigger: '@focus'
                },
                link: function (scope, element) {
                    scope.$watch('trigger', function (value) {
                        if (value === "true") {
                            $timeout(function () {
                                element[0].focus();
                            });
                        }
                    });
                }
            };
        }
    }
)();
