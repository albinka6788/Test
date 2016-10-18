(
    function () {
        'use strict';
        angular.module('bhic-app.wc.purchaseGet')
        .directive('businessInfoControl', businessInfoControlFn);
        function businessInfoControlFn() {
            return {
                restrict: 'EA',
                templateUrl: '/Scripts/bhic/directives/wc/businessInput/businessInputControlTemplate.html',
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


