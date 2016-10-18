(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('policyInformationController', ['$scope', '$rootScope', '$routeParams', '$location', 'loadingService', 'policyInformationData', 'authorisedUserWrapperService', policyInformationControllerFn]);
        function policyInformationControllerFn($scope, $rootScope, $routeParams, $location, loadingService, policyInformationData, authorisedUserWrapperService) {

            // Controler funciton Start  write any methods/functions after this line.          

            $scope.key = window.name.split("CYB")[0];
			$scope.link1 = "#/MakePayment/" + $scope.key + "1";
            $scope.link2 = "#/MakePayment/" + $scope.key + "2";
            if (policyInformationData.success) {
                $scope.policy = policyInformationData.policy;
            } else if (policyInformationData.redirectStatus) {
                $location.path("/Error/");
                loadingService.hideLoader();
            }

            loadingService.hideLoader();
            // Controler funciton End Please dont write any methods/functions after this line.

        }
    })();
