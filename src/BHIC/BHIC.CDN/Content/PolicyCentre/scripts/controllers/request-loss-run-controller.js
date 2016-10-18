(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('requestLossRunController', ['$scope', '$location', 'loadingService', 'authorisedUserWrapperService', requestLossRunControllerFn]);
    function requestLossRunControllerFn($scope, $location, loadingService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.
        loadingService.hideLoader();
    }
})();