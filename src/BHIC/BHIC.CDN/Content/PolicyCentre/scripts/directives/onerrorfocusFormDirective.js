(function () {
    'use strict';

    //currency control directive definition
    angular.module('BHIC.Dashboard.Directives').directive('onerrorfocusForm', ['$parse', onerrorFocusFn]);
    function onerrorFocusFn() {
        return {
            restrict: 'A',
            link: function (scope, elem) {

                // set up event handler on the form element
                elem.on('submit', function () {

                    // find the first invalid element
                    var firstInvalid = elem[0].querySelector('.ng-invalid');

                    // if we find one, set focus
                    if (firstInvalid) {
                        firstInvalid.focus();
                    }
                });
            }
        };
    }
})();
