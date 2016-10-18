(
    function () {
        'use strict';
        angular.module('BHIC.Directives')
        .directive('bhicTooltip', loadingControlFn);
        function loadingControlFn() {
            return {
                restrict: 'EA',
                link: link
            };
            function link(scope, elem, attrs)
            {
                elem.tooltip();
                //if (!scope.tooltipAlreadyAdded || scope.tooltipAlreadyAdded == false)
                //{
                //    scope.tooltipAlreadyAdded = true;
                //    elem.tooltip();
                //}  
            }
        }
    }
)();
