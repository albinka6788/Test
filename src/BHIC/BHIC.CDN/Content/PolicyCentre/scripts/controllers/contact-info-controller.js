(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('contactInfoController', ['$scope', '$location', 'loadingService', 'authorisedUserWrapperService', contactInfoControllerFn]);
    function contactInfoControllerFn($scope, $location, loadingService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.
       
        //variable declaration
        var self = this;
        nosidemenu(true);
        loadingService.hideLoader();
    }
})();