/// <reference path="loadingControlTemplate.html" />
(
    function () {
        'use strict';
        angular.module('BHIC.Directives')
        .directive('loader', ['appConfig', loadingControlFn]);

        function loadingControlFn(appConfig)
        {
            return {
                restrict: 'EA',
                templateUrl: appConfig.appCdnDomain + '/Content/Common/scripts/directives/loadingControl/loadingControlTemplate.html',
                link: link
            };
            function link(scope, elem, attrs) {
                function _init() {
                    scope.appCdnDomain = appConfig.appCdnDomain;
                };
                _init();
            }
        }
    }
)();
