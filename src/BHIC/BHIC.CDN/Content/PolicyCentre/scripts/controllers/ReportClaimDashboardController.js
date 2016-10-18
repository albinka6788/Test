(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('reportClaimDashboardController', ['$scope', '$location', 'loadingService', reportClaimDashboardControllerFn]);
    function reportClaimDashboardControllerFn($scope, $location, loadingService) {

        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        var formdata, count = 0, valid = true;
        loadingService.hideLoader();

        // Controler funciton End Please dont write any methods/functions after this line.

        $scope.navigate = function (url) {
            loadingService.showLoader();
            $location.path(url);
        }
    }

})();