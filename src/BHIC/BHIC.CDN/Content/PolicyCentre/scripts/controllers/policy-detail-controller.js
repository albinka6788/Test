(
    function () {
        'use strict';
        //Comment : Here module controller declaration and injection
        angular.module("BHIC.Dashboard.Controllers").controller('policyDetailController', ['$scope', '$http', '$location', 'loadingService', 'policyDetailService', policyDetailControllerFn]);
        function policyDetailControllerFn($scope, $http, $location, loadingService, policyDetailService) {
            // Controler funciton Start  write any methods/functions after this line.

            var policy = this;
            policy.policyDetail = {};

            //if refresh so activate the current link
            //sidemenustate('CancelPolicy');

            $scope.GetPolicy = function () {
                loadingService.showLoader();

                policyDetailService.requestPolicyDetail().then(function (response) {
                    policy.status = response.status;
                    if (response.status) {
                        policy.policyDetail = response.policy;
                    } else {
                        policy.errorMessage = response.errorMessage;
                    }
                    loadingService.hideLoader();
                });
            }

            $scope.GetPolicy();

            // Controler funciton End Please dont write any methods/functions after this line.
        }
    })();