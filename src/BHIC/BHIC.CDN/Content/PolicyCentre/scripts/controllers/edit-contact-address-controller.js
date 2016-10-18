(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('editContactAddressController', ['$scope', '$window', '$timeout', '$filter', '$location', 'loadingService', 'sharedUserService', 'authorisedUserWrapperService', editContactInfoControllerFn]);
    function editContactInfoControllerFn($scope, $window, $timeout, $filter, $location, loadingService, sharedUserService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.       
        //variable declaration
        var _self = this;        
        $scope.value = 'M';
        $scope.CYBKey = $window.localStorage.getItem('editcontactInfo');
        $scope.PolicyCode = $scope.emptyString;
        $scope.Status = $scope.emptyString;
        $scope.changeNotification = false;

        loadingService.hideLoader();
        nosidemenu(true);

        //Default Contact Address Form for reset
        $scope.defaultContactAddressForm = function () {
            $scope.emptyString = "";
            $scope.errormsg = $scope.emptyString;            
            $scope.address1 = $scope.emptyString;
            $scope.address2 = $scope.emptyString;
            $scope.city = $scope.emptyString;
            $scope.state = $scope.emptyString;
            $scope.zipCode = $scope.emptyString;
            $scope.additional = $scope.emptyString;            
            $scope.submitted = false;            
        };

        $scope.defaultContactAddressForm();
       
        //Method will used for Contact Address Form submission
        $scope.updateContactAddressInfo = function () {
            var _self = {};
            $scope.hasErrorMain = false;

            if ($scope.Status.indexOf('No Coverage') > -1 || $scope.Status.indexOf('Expired') > -1 || $scope.Status.indexOf('Cancelled') > -1) {
                $scope.hasErrorMain = true;
                scrollTop();
                return;
            }

            $scope.errormsg = $scope.emptyString;
            $scope.changeNotification = false;
            loadingService.showLoader();
            _self.Address1 = $scope.address1;
            _self.Address2 = $scope.address2;
            _self.City = $scope.city;
            _self.State = $scope.state;
            _self.ZipCode = $scope.zipCode;
            _self.Additional = $scope.additional;
            _self.PolicyCode = $scope.CYBKey;
            _self.AddressType = $scope.value == "M" ? "Mailing Address" : ($scope.value == "B" ? "Billing Address" : ($scope.value == "P" ? "Physical Address" : ($scope.value == "A" ? "Physical, Mailing as well as Billing Address" : "")));
            var wrapper = { "method": "UpdateContactAddress", "postData": _self };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.changeNotification = true;
                    if ($scope.value == "A") {
                        $scope.defaultContactAddressForm();
                    } else {
                        $scope.errormsg = $scope.emptyString;
                        $scope.submitted = false;                        
                    }
                    $scope.contactInfoAddressForm.$setPristine();
                }
                else {
                    $scope.errormsg = response.errorMessage;
                }
                $('html, body').animate({ scrollTop: 0 }, 800);
                loadingService.hideLoader();
            });
        }

        $scope.getSelectedAddress = function () {
            loadingService.showLoader();
            $scope.changeNotification = false;
            $scope.submitted = false;            
            //if ($scope.CYBKey != $scope.emptyString && $scope.value == 'A' && $scope.PolicyCode != $scope.emptyString && $scope.Status != $scope.emptyString) {
            //    $scope.address1 = $scope.emptyString;
            //    $scope.address2 = $scope.emptyString;
            //    $scope.city = $scope.emptyString;
            //    $scope.state = $scope.emptyString;
            //    $scope.zipCode = $scope.emptyString;
            //    $scope.errormsg = $scope.emptyString;
            //    loadingService.hideLoader();
            //    return;
            //}
            
            var wrapper = { "method": "GetAddress", "queryString": $scope.CYBKey, "postData": $scope.value }
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.errormsg = $scope.emptyString;
                    if ($scope.CYBKey == response.contactAddress.CYBKey) {                        
                        if ($scope.value == 'A') {
                            $scope.address1 = $scope.emptyString;
                            $scope.address2 = $scope.emptyString;
                            $scope.city = $scope.emptyString;
                            $scope.state = $scope.emptyString;
                            $scope.zipCode = $scope.emptyString;
                        } else {
                            $scope.address1 = response.contactAddress.Addr1;
                            $scope.address2 = response.contactAddress.Addr2;
                            $scope.city = response.contactAddress.City;
                            $scope.state = response.contactAddress.State;
                            $scope.zipCode = response.contactAddress.Zip;
                        }
                        //$scope.CYBKey = response.contactAddress.CYBKey; 
                        $scope.Status = response.contactAddress.PolicyStatus;
                        $scope.PolicyCode = response.contactAddress.PolicyCode;
                    }
                    else {
                        $scope.errormsg = "Key is invalid";
                    }
                } else {
                    $scope.errormsg = response.errorMessage;
                }
                loadingService.hideLoader();
                $scope.contactInfoAddressForm.$setPristine();
            });

        }

        $scope.getSelectedAddress($scope.value);
    }
})();




