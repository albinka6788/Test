
(
    function () {
        'use strict';
        angular.module('bhic-app.wc.purchaseGet', ['bhic-app.services', 'bhic-app.wc.controls'])
            .controller('purchaseGetCtrl', ['$scope', '$timeout', '$window', 'bhic-app.wc.services.purchasePolicyPageData', purchaseGetControllerFn]);

        function purchaseGetControllerFn($scope, $timeout, $window, purchasepolicyPageDataProvider) {

            function _init() {
                _getPurchaseData();
                _getBusinessTypes();
                $scope.invalidZipCityState = true;
                $scope.showZipCodeTextInput = false;
            };

            $scope.textIdTypes = [{
                Text: 'Federal Tax ID Number (TIN)',
                Value: 'E'
            }, {
                Text: 'Social Security Number (SSN)',
                Value: 'S'
            }
            ];
            $scope.selectedTax;
            $scope.purchaseModel = {

                policy: {
                    policyDate: ''
                },

                personalContact: {
                    sameAsContact: '',
                    email: '',
                    confirmEmail: '',
                    firstName: '',
                    lastName: '',
                    phoneNumber: ''
                },

                businessContact: {
                    bEmail: '',
                    bConfirmEmail: '',
                    bFirstName: '',
                    bLastName: '',
                    bPhoneNumber: ''
                },

                account: {
                    password: '',
                    confirmPassword: ''
                },

                businessInfo: {
                    businessName: '',
                    businessType: {
                        Description: '',
                        Code: ''
                    },
                    taxIdType: {
                        Text: '',
                        Value: ''
                    },
                    taxIdOrSSN: ''
                },

                mailingAddress: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    state: '',
                    zip: '',
                }
            };
            /*Discussion pending for getting the state and Zip,
            removal of hardcoding state and zip to be done as 
            soon as API is exposed*/
            //Comment : Here get list of business type
            var _getPurchaseData = function () {

                var promise = purchasepolicyPageDataProvider.getPurchaseData();

                promise.then(function (data) {

                    if (data.success == true && data.purchaseModel.length > 0) {
                        //Comment : Here get purchase model string data into jsonObject
                        var purchaseModelData = angular.fromJson(data.purchaseModel);

                        //Comment : Here copy retrieved jsonObject into controller model object
                        //$scope.purchaseModel = angular.copy(purchaseModelData);
                        //$scope.purchaseModel.policy.policyDate = purchaseModelData.Policy.PolicyDate;
                        $scope.purchaseModel.policy.policyDate = new Date(purchaseModelData.Policy.PolicyDate);

                        $scope.purchaseModel.personalContact.sameAsContact = purchaseModelData.PersonalContact.SameAsContact;
                        $scope.purchaseModel.personalContact.email = purchaseModelData.PersonalContact.Email;
                        $scope.purchaseModel.personalContact.confirmEmail = purchaseModelData.PersonalContact.ConfirmEmail;
                        $scope.purchaseModel.personalContact.firstName = purchaseModelData.PersonalContact.FirstName;
                        $scope.purchaseModel.personalContact.lastName = purchaseModelData.PersonalContact.LastName;
                        $scope.purchaseModel.personalContact.phoneNumber = purchaseModelData.PersonalContact.PhoneNumber;

                        $scope.purchaseModel.businessContact.bEmail = purchaseModelData.BusinessContact.Email;
                        $scope.purchaseModel.businessContact.bConfirmEmail = purchaseModelData.BusinessContact.ConfirmEmail;
                        $scope.purchaseModel.businessContact.bFirstName = purchaseModelData.BusinessContact.FirstName;
                        $scope.purchaseModel.businessContact.bLastName = purchaseModelData.BusinessContact.LastName;
                        $scope.purchaseModel.businessContact.bPhoneNumber = purchaseModelData.BusinessContact.PhoneNumber;

                        $scope.purchaseModel.account.password = purchaseModelData.Account.Password;
                        $scope.purchaseModel.account.confirmPassword = purchaseModelData.Account.ConfirmPassword;

                        $scope.purchaseModel.businessInfo.businessName = purchaseModelData.BusinessInfo.BusinessName;
                        $scope.purchaseModel.businessInfo.businessType = purchaseModelData.BusinessInfo.BusinessType;
                        $scope.purchaseModel.businessInfo.taxIdType = purchaseModelData.BusinessInfo.TaxIdType;
                        $scope.selectedTax = purchaseModelData.BusinessInfo.TaxIdType.Value;
                        $scope.purchaseModel.businessInfo.taxIdOrSSN = purchaseModelData.BusinessInfo.TaxIdOrSSN;

                        $scope.purchaseModel.mailingAddress.addressLine1 = purchaseModelData.MailingAddress.AddressLine1;
                        $scope.purchaseModel.mailingAddress.addressLine2 = purchaseModelData.MailingAddress.AddressLine2;
                        $scope.purchaseModel.mailingAddress.city = purchaseModelData.MailingAddress.City;
                        $scope.purchaseModel.mailingAddress.state = purchaseModelData.MailingAddress.State;
                        $scope.purchaseModel.mailingAddress.zip = purchaseModelData.MailingAddress.Zip;

                        console.log($scope.purchaseModel);
                    }
                    else {
                        $scope.purchaseModel.policy.policyDate = data.exposureDate;
                        $scope.purchaseModel.mailingAddress.state = data.stateName;
                        $scope.purchaseModel.mailingAddress.zip = data.zipCode;
                    }
                    $scope.invalidZipCityState = $scope.purchaseModel.mailingAddress.city == "" ? true : false;
                });
            };


            //Comment : Here get list of business type
            var _getBusinessTypes = function () {

                var promise = purchasepolicyPageDataProvider.getBusinessTypes();

                promise.then(function(data) {

                    //Comment : Here get all errors then only get/set required errors for this form
                    $scope.businessTypes = data.businessTypes;
                });
            };

            var _submit = function () {
                if(!$scope.invalidZipCityState && $scope.purchaseInputForm.personalContact.$valid && $scope.purchaseInputForm.passwordInfo.$valid && $scope.purchaseInputForm.businessContact.$valid) {
                    var promise = purchasepolicyPageDataProvider.submitPurchaseData($scope.purchaseModel);
                    promise.then(function(data) {
                        $window.location.href = '/Landing/Wchome/VerifyOrderGet';
                    });
                }
            };

            var _validateStateCityZip = function () {
                $scope.invalidZipCityState = true;
                var promise = purchasepolicyPageDataProvider.validateStateCityZip($scope.purchaseModel.mailingAddress.zip, $scope.purchaseModel.mailingAddress.city, $scope.purchaseModel.mailingAddress.state);
                promise.then(function(response) {
                    $timeout(function() {
                        $scope.invalidZipCityState = response == "True" ? true : false;
                    })
                });
            }
            $scope.stateChanged = function (personalContact) {
                if(personalContact.sameAsContact == true) {
                    $scope.purchaseModel.businessContact.bEmail = personalContact.email;
                    $scope.purchaseModel.businessContact.bConfirmEmail = personalContact.confirmEmail;
                    $scope.purchaseModel.businessContact.bFirstName = personalContact.firstName;
                    $scope.purchaseModel.businessContact.bLastName = personalContact.lastName;
                    $scope.purchaseModel.businessContact.bPhoneNumber = personalContact.phoneNumber;
                }
                else {
                    $scope.purchaseModel.businessContact.bEmail = '';
                    $scope.purchaseModel.businessContact.bConfirmEmail = '';
                    $scope.purchaseModel.businessContact.bFirstName = '';
                    $scope.purchaseModel.businessContact.bLastName = '';
                    $scope.purchaseModel.businessContact.bPhoneNumber = '';
                }
            }
            var _updateTaxSelection = function (selectedTaxId) {
                $timeout(function() {
                    $scope.purchaseModel.businessInfo.taxIdType = $.grep($scope.textIdTypes, function (e) { return e.Value == selectedTaxId })[0];
                    $scope.purchaseInputForm.businessInfo.taxIdType = $scope.purchaseModel.businessInfo.taxIdType;
                });
            };
            var _goToHomePage = function () {
                $window.location.href = '/Landing/WcHome';
            }


            angular.extend($scope, {
                init: _init(),
                submitPurchase: _submit,
                validateStateCityZip: _validateStateCityZip,
                updateTaxSelection: _updateTaxSelection,
                goToHomePage: _goToHomePage
            });
        };
    }
)();