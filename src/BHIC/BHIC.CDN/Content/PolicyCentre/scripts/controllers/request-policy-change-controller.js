(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('requestPolicyChangeController', ['$scope', '$filter', '$http', '$location', 'loadingService', 'policyChangeOptionsData', 'authorisedUserWrapperService', 'sharedUserService', requestPolicyChangeControllerFn]);
    function requestPolicyChangeControllerFn($scope, $filter, $http, $location, loadingService, policyChangeOptionsData, authorisedUserWrapperService, sharedUserService) {

        // Controler funciton Start  write any methods/functions after this line.

        var policy = this;
        $scope.changeOption = [];
        $scope.selectedItem = [0];
        $scope.effectivedate = {};
        var _date = new Date();
        var defaultdate = sharedUserService.FormatDate(_date.setDate(_date.getDate() + 1));
        $scope.minDate = defaultdate;

        //Default Cancel Form for reset
        $scope.defaultForm = function () {
            $scope.description = "";
            $scope.selectedDate = defaultdate;
            $scope.selectedItem = $scope.changeOption[0];
            if ($scope.effectivedate.value != "option1")
                $scope.effectivedate = { "desc": "", "value": "option1" };
            $scope.submitted = false;
            $scope.requestPolicyChangeModel.$setPristine();
        };


        $scope.GetPolicyChangeOption = function () {
            $scope.status = policyChangeOptionsData.success;
            if (policyChangeOptionsData.success) {
                $scope.effectivedate = { "desc": "", "value": "option1" };
                $scope.selectedDate = defaultdate;
                $scope.changeOption = policyChangeOptionsData.options;
                $scope.selectedItem = policyChangeOptionsData.options[0];
                $scope.description = "";
                $scope.submitted = false;
                $scope.maxDate = sharedUserService.convertToDate(policyChangeOptionsData.ExpiryDate);
            } else if (policyChangeOptionsData.redirectStatus) {
                $location.path("/Error/");
            } else {
                $scope.errorMessage = policyChangeOptionsData.errorMessage;
            }

            loadingService.hideLoader();
        }


        $scope.submitModel = function () {
            loadingService.showLoader();
            var _self = {};

            if ($scope.effectivedate.value == undefined) {
                $scope.status = false;
                $scope.errorMessage = "Please select effective date";
                loadingService.hideLoader();
                return;
            }
            if ($scope.description == "") {
                $scope.status = false;
                $scope.errorMessage = "Please add description";
                loadingService.hideLoader();
                return;
            }

            if ($scope.effectivedate.value == "option1") {
                _self.Effectivedate = $scope.selectedDate;
            }
            else {
                _self.Effectivedate = $scope.effectivedate.desc;
            }

            _self.Description = $scope.description;
            _self.SelectedID = $scope.selectedItem.id;
            _self.PolicyNumber = window.name.split("CYB")[0];

            var wrapper = { "method": "SendPolicyChangeRequest", "postData": _self }
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                $scope.status = response.success;
                if (response.success) {
                    $scope.successMessage = true;
                    $scope.defaultForm();
                } else if (response.redirectStatus) {
                    $location.path("/Error/");
                } else {
                    $scope.errorMessage = response.errorMessage;
                }
                $('html, body').animate({ scrollTop: 0 }, 800);
                loadingService.hideLoader();
            });
        }

        $scope.GetPolicyChangeOption();

        //Comment : Here reset all error validation in case user chooses OPTION-2,3
        $scope.$watch('effectivedate.value', function () {
            if ($scope.effectivedate.value == 'option2' || $scope.effectivedate.value == 'option3') {
                $scope.selectedDate = defaultdate;
                $scope.requestPolicyChangeModel.dtpicker.$setValidity('invalidFormat', true);
                $scope.requestPolicyChangeModel.dtpicker.$setValidity('invalidRange', true);
            }
        });
        // Controler funciton End Please dont write any methods/functions after this line.


    }
})();