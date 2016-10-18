(
    function () {
        'use strict';
        angular.module('bhic-app.wc.purchaseGet')
        .directive('passwordInfoControl', passwordInfoControlFn);
        function passwordInfoControlFn() {
            return {
                restrict: 'EA',
                templateUrl: '/Scripts/bhic/directives/wc/passwordInput/passwordInputControlTemplate.html',
                link: link
            };
            function link(scope, elem, attrs) {
                function _init() {


                };
                _init();
            }
        }

    }
)();


