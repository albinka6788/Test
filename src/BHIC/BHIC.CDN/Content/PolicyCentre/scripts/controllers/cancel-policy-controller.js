(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('cancelPolicyController', ['$scope', '$filter', '$location', 'loadingService', 'contactData', 'authorisedUserWrapperService', 'sharedUserService', cancelPolicyControllerFn]);
    function cancelPolicyControllerFn($scope, $filter, $location, loadingService, contactData, authorisedUserWrapperService, sharedUserService) {
        // Controller function Start write any methods/functions after this line.

        //variable declaration
        var _self = this;
        $scope.responseMsg = false;
        $scope.emptyString = "";
        $scope.formerrormsg = $scope.emptyString;
        $scope.errormessage = $scope.emptyString;
        $scope.reasonerrormsg = $scope.emptyString;       
        $scope.status = true;
        var minDate = new Date();
        $scope.minDate = sharedUserService.FormatDate(minDate);
        $scope.GetPolicyCancelOption = function () {
            var wrapper = { "method": "GetPolicyCancelOptions" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {

                    $scope.Reasons = response.options;
                    $scope.mreason = response.options[0];

                } else if (response.redirectStatus) {
                    $location.path("/Error/");
                } else {
                    $scope.errorMessage = response.errorMessage;
                    $scope.cancelForm.$setPristine();
                }

                loadingService.hideLoader();

            });
        }

        $scope.GetPolicyCancelOption();


        //Default cancel form for reset
        $scope.defaultCancelForm = function () {
            $scope.effectivedate = $scope.emptyString;
            $scope.mreason = $scope.Reasons[0];
            $scope.phoneerrormsg = $scope.emptyString;
            $scope.description = $scope.emptyString;
            $scope.submitted = false;
        };

        // Pre-populate the contact details of the user at form load
        $scope.getContactInfo = function () {
            //$scope.name = contactData.user.FirstName + ' ' + contactData.user.LastName;
            $scope.name = contactData.user.Name;
            $scope.phone = contactData.user.PhoneNumber;
            $scope.email = contactData.user.Email;
            $scope.maxDate = sharedUserService.convertToDate(contactData.ExpiryDate);
        };

        // method to use cancellation request form submission.  mreason
        $scope.cancelPolicyForm = function () {
            if ($scope.mreason.id != 0) {
                loadingService.showLoader();
                $scope.reasonerrormsg = $scope.emptyString;
                $scope.errorMessage = $scope.emptyString;
                $scope.effectivedate = $filter('date')($scope.effectivedate, "MM/dd/yyyy");
                _self.RequestedEffectiveDate = new Date($scope.effectivedate);
                _self.EffectiveDate = $scope.effectivedate;
                _self.Phone = $scope.phone;
                _self.Name = $scope.name;
                _self.Email = $scope.email;
                _self.ReasonID = $scope.mreason.id;
                _self.Description = $scope.description;
                _self.PolicyId = window.name.split("CYB")[0];                
                var wrapper = { "method": "CancelPolicy", "postData": _self };
                authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                    $scope.status = response.success;
                    if (response.success) {
                        $scope.responseMsg = true;
                        $scope.defaultCancelForm();
                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {
                        loadingService.hideLoader();
                        $scope.errorMessage = response.errorMessage;
                        $scope.cancelForm.$setPristine();
                    }
                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                });
            }

        };
    }
})();