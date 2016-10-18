(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('errorController', ['$scope', '$rootScope', '$routeParams', '$location', 'loadingService', 'authorisedUserWrapperService', errorControllerFn]);
        function errorControllerFn($scope, $rootScope, $routeParams, $location, loadingService, authorisedUserWrapperService) {

            // Controler funciton Start  write any methods/functions after this line.
            $('html, body').animate({ scrollTop: 0 }, 800);

            loadingService.hideLoader();

            // Controler funciton End Please dont write any methods/functions after this line.

        }
    })();
