(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('sessionController', ['$scope', '$http', '$rootScope', '$location', 'loadingService', 'sharedUserService', sessionControllerFn]);
    function sessionControllerFn($scope, $http, $rootScope, $location, loadingService, sharedUserService) {

        // Controler funciton Start  write any methods/functions after this line.
        // Define $scope methods variables etc.
        var _self = this;
        loadingService.hideLoader();
        $('.pageHeading, .sidebar-left').css('display', 'none');

        $scope.logout = function () {
            loadingService.hideLoader();
            sharedUserService.setUser(null);
        }

        $scope.RedirectToLogin = function () {
            loadingService.showLoader();
            window.location.href = '~/#/Login';
        }

    }
})();
