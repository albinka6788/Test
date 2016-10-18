(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('yourPoliciesController', ['$scope', '$http', '$rootScope', '$location', '$filter', 'yourPoliciesData', 'loadingService', 'authorisedUserWrapperService', yourPoliciesControllerFn]);
    function yourPoliciesControllerFn($scope, $http, $rootScope, $location, $filter, yourPoliciesData, loadingService, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.

        // Define $scope methods variables etc.
        var _self = this;
        loadingService.hideLoader();
        $scope.yourPolicies = yourPoliciesData.yourPolicies;

        $scope.convertToDate = function (inputString) {
            var convertedDate = new Date(parseInt(inputString.match(/\d/g).join("")));
            return $filter('date')(convertedDate, 'MM/dd/yyyy');
        }

        $scope.GoTo = function (PolicyCode, redirect, sender) {
            loadingService.showLoader();
            window.name = PolicyCode + "CYB" + sender.policy.PolicyCode + "CYB" + sender.policy.Status + "CYB" + sender.policy.LOB;
            //window.name = PolicyCode;
            sideMenuReload = true;
            $(".sidebar-menu .policyNumber").html("(" + sender.policy.PolicyCode + ")");
            (redirect == 'PI') ? $location.path('/PolicyInformation') : redirect == 'RC' ? $location.path('/ReportClaim') : $location.path('/MakePayment/' + PolicyCode + '1');
        }

        $scope.CheckForLastPolicy = function (isLastItem) {
            if (isLastItem) {
                setTimeout(function () {
                    formatBox()
                }, 250);
            }
        }

        // Controler funciton End Please dont write any methods/functions after this line.
    }
})();
