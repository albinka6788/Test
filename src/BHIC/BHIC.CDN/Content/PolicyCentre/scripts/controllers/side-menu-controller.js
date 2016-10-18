(function () {
    'use strict';
    angular.module("BHIC.Dashboard.Controllers")
        .controller('sideMenuController',
                    ['$scope',
                    '$location',
                     sideMenuFn])
    function sideMenuFn($scope, $location) {
        var cssClass = this;


        $scope.GetCssClass = function () {
            $("ul.sidebar-menu li a").removeClass("activeLink");
            
        }

        $scope.GetCssClass();

    }

})();
