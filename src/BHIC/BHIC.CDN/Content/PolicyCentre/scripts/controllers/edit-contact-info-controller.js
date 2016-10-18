(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('editContactInfoController', ['$scope', '$window', '$timeout', '$filter', '$location', 'loadingService', 'sharedUserService', 'authorisedUserWrapperService', editContactInfoControllerFn]);
    function editContactInfoControllerFn($scope, $window, $timeout, $filter, $location, loadingService, sharedUserService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.       
        //variable declaration
        var _self = this;
        $scope.emptyString = "";
        $scope.responseMsg = false;
        $scope.addressResponseMsg = false;
        $scope.contactInfo = true;
        $scope.focusField = true;
        $scope.hasErrorMain = false;
        $scope.errormsg = $scope.emptyString;
        $scope.errorMessage = $scope.emptyString;
        $scope.drpPolicyList = $scope.emptyString;
        $scope.combinePolicyCodeAndDate = $scope.emptyString;
        $scope.showContactInformation = false;
        $scope.policyCode = $scope.emptyString;
        $scope.policyStatus = $scope.emptyString;
        loadingService.hideLoader();
        nosidemenu(true);

        $scope.defaultForm = function () {
            $scope.name = $scope.emptyString;
            $scope.phone = $scope.emptyString;
            $scope.email = $scope.emptyString;
            $scope.submitted = false;
        };

        //Method will used for Contact Info form submission
        $scope.updateContactInfo = function () {
            if ($scope.policyCode.length <= 0) {
                if ($scope.drpPolicyList.length > 0) {
                    $scope.radiobuttonerror = "Please select a policy";
                } else {
                    $scope.radiobuttonerror = "You don't have any policy to perform further action.";
                }
                $('html, body').animate({ scrollTop: 0 }, 800);
                $("#radioFocus").focus();

                return;
            }
            var reqnumber = sharedUserService.CheckPhoneNumber($scope.phone);
            if (reqnumber.length < 10) {
                $scope.phoneerrormsg = "Please Enter Valid Phone Number!";
                $('html, body').animate({ scrollTop: 0 }, 800);
                $("#phonenumber").focus();
                return;
            }

            $scope.submitted = true;
            $scope.errorMessage = $scope.emptyString;
            $scope.responseMsg = false;
            var _self = {};

            loadingService.showLoader();
            _self.Name = $scope.name;
            _self.PhoneNumber = Number(reqnumber);
            _self.Email = $scope.email;
            _self.ContactId = $scope.contactId;
            _self.PolicyCode = $scope.policyCode;
            _self.PhoneId = $scope.phoneId;
            _self.QuoteId = $scope.quoteId;
            _self.PhoneType = $scope.phoneType;

            var wrapper = { "method": "UpadteContactInfo", "postData": _self };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.responseMsg = true;

                    $scope.drpPolicyList.forEach(function (obj) {
                        if (obj.Id == $scope.policyCode) {
                            obj.Id = response.CYBKey;
                            $scope.policyCode = response.CYBKey;
                        }
                    });

                    $scope.phoneerrormsg = $scope.emptyString;
                    $scope.contactInfoForm.$setPristine();
                }
                else {
                    $scope.errorMessage = response.errorMessage;
                }
                scrollTop();
                loadingService.hideLoader();
                $scope.focusField = true;
            });
        };

        //Method will used for Combining date and Policy code
        $scope.combinePolicyCodeAndDate = function (list) {
            var returnList = [];
            var checkedstatus = false;
            if (list.length > 0) {
                var lob = '';
                $.each(list, function (ind, obj) {
                    if (obj.PolicyCode.substring(2, 4) == "WC") {
                        lob = "Workers' Compensation";

                    }
                    else {
                        lob = "Business Owner's Policy";
                    }

                    if ($window.localStorage.getItem('editcontactInfo') != null) {
                        if (obj.CYBPolicyNumber == $window.localStorage.getItem('editcontactInfo')) {
                            checkedstatus = true;
                        } else {
                            checkedstatus = false;
                        }
                    } else {
                        checkedstatus = false;
                    }

                    returnList.push({
                        Id: obj.CYBPolicyNumber,
                        LOB: lob,
                        checked: false,
                        PolicyCode: ' (' + obj.PolicyCode + ', ' + $filter('date')(new Date(parseInt(obj.PolicyBegin.match(/\d/g).join(""))), 'MM/dd/yyyy') + ' to ' + $filter('date')(new Date(parseInt(obj.PolicyExpires.match(/\d/g).join(""))), 'MM/dd/yyyy') + ')',
                        PolicyStatus: obj.Status
                    });
                });
            }
            return returnList;
        }

        //Method will used for Combining date and Policy code
        $scope.ContactAddress = function (data) {
            $scope.contactInfo = true;
            $scope.contactAdd = false;
            $scope.submitted = false;
            $scope.drpPolicyList = null;
            angular.forEach(data, function (value, key) {
                $scope.drpPolicyList = $scope.combinePolicyCodeAndDate(data);
            });

            loadingService.hideLoader();
        }

        $scope.getPageData = function () {
            loadingService.showLoader();
            var wrapper = { "method": "GetYourPolicies" };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.ContactAddress(response.yourPolicies);
                } else {
                    $scope.errorMessage = "No Policies found for the user. Please try again.";
                    loadingService.hideLoader();
                }
            });
        }

        //fetch session data to show the default values
        $scope.getPageData();

        //Method to fetch the contact Information for a policy once user clicks on radio button
        $scope.getContactInformation = function (cybPolicyNumber) {
            $scope.responseMsg = false;
            $scope.hasErrorMain = false;
            $scope.radiobuttonerror = $scope.emptyString;
            loadingService.showLoader();
            var wrapper = { "method": "GetContactInformation", "postData": cybPolicyNumber };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.name = response.user.Name;
                    $scope.phone = response.user.PhoneNumber;
                    $scope.email = response.user.EmailID;
                    $scope.contactId = response.user.ContactId;
                    $scope.quoteId = response.user.QuoteId;
                    $scope.phoneId = response.user.PhoneId;
                    $scope.phoneType = response.user.PhoneType;
                    $scope.showContactInformation = true;
                    $scope.policyStatus = response.user.status;
                    $("#name").focus();
                    loadingService.hideLoader();
                }
                else {
                    $scope.defaultForm();
                    $scope.contactId = $scope.emptyString;
                    $scope.showContactInformation = false;
                    loadingService.hideLoader();
                }
            });
        }

        $scope.EditAddress = function () {
            loadingService.showLoader();
            if ($scope.policyCode == $scope.emptyString) {
                $scope.radiobuttonerror = "Please select a policy";
                scrollTop();

            } else {
                if ($scope.policyStatus.indexOf('No Coverage') > -1 || $scope.policyStatus.indexOf('Expired') > -1 || $scope.policyStatus.indexOf('Cancelled') > -1) {
                    $scope.hasErrorMain = true;
                    scrollTop();
                } else {
                    $window.localStorage.setItem('editcontactInfo', $scope.policyCode);
                    $location.path("/EditContactAddress");
                }
            }
            loadingService.hideLoader();
        }
    }
})();