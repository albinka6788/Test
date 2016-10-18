(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('helpfulResourcesController', ['$scope', '$location', 'loadingService', 'authorisedUserWrapperService', helpfulResourcesControllerFn]);
    function helpfulResourcesControllerFn($scope, $location, loadingService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.

        //variable declaration
        var self = this;
        nosidemenu(true);
        loadingService.hideLoader();
    }
})();