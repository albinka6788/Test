(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular.module('BHIC.WC.Services')
            .factory('quotePurchaseService', ['restServiceConsumer', '$q', '$resource', 'coreUtils', quotePurchaseServiceFn]);

        function quotePurchaseServiceFn(restServiceConsumer, $q, resource, coreUtils) {
            function _getQuotePurchasePageDefaults(quoteId) {
                var deferred = $q.defer();
                restServiceConsumer.purchaseQuote.getQuotePurchasePageDefaults({ quoteId: quoteId }).$promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }

            function _validateCityStateZip(zipCode, city, state) {
                var deferred = $q.defer();
                restServiceConsumer.purchaseQuote.validateCityStateZip({ zipCode: zipCode, city: city, state: state }).$promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }

            function _prepareSubmissionData(dataModel) {
                var _wcPurchaseViewModel = {

                    Policy: {
                        //PolicyDate: dataModel.policy.policyDate
                        PolicyDate: ""// new Date(dataModel.policy.policyDate).toUTCString()
                    },

                    PersonalContact: {
                        SameAsContact: dataModel.primaryContactInformation.copyPrimaryContactInfoToBusinessContact,
                        Email: dataModel.primaryContactInformation.email,
                        ConfirmEmail: dataModel.primaryContactInformation.email,
                        FirstName: dataModel.primaryContactInformation.firstName,
                        LastName: dataModel.primaryContactInformation.lastName,
                        PhoneNumber: dataModel.primaryContactInformation.phone
                    },

                    BusinessContact: {
                        Email: dataModel.businessContactInformation.email,
                        ConfirmEmail: dataModel.businessContactInformation.email,
                        FirstName: dataModel.businessContactInformation.firstName,
                        LastName: dataModel.businessContactInformation.lastName,
                        PhoneNumber: dataModel.businessContactInformation.phone
                    },

                    Account: {
                        Email: dataModel.account.email,
                        Password: dataModel.account.password,
                        ConfirmPassword: dataModel.account.confirmPassword
                    },

                    BusinessInfo: {
                        FirstName: (dataModel.selectedBusiness.BusinessTypeCode == "I" && dataModel.individual != null && dataModel.individual != undefined) ? dataModel.individual.firstName : null,
                        MiddleName: (dataModel.selectedBusiness.BusinessTypeCode == "I" && dataModel.individual != null && dataModel.individual != undefined) ? dataModel.individual.middleName : null,
                        LastName: (dataModel.selectedBusiness.BusinessTypeCode == "I" && dataModel.individual != null && dataModel.individual != undefined) ? dataModel.individual.lastName : null,
                        BusinessName: (dataModel.selectedBusiness.BusinessTypeCode != "I") ? dataModel.businessName : null,
                        BusinessType: dataModel.selectedBusiness.BusinessTypeCode,
                        TaxIdType: dataModel.selectedTaxIdType,
                        TaxIdOrSSN: dataModel.selectedTaxIdType == 'S' ? dataModel.SSN : dataModel.taxId,
                    },

                    MailingAddress: {
                        AddressLine1: dataModel.businessMailingAddress.addressLine1,
                        AddressLine2: dataModel.businessMailingAddress.addressLine2,
                        City: dataModel.businessMailingAddress.city,
                        State: dataModel.businessMailingAddress.state,
                        Zip: dataModel.businessMailingAddress.zipCode
                    },
                    SUIN: dataModel.SUIN
                };
                return _wcPurchaseViewModel;
            }

            function _submitPurchaseData(dataModel) {
                var deferred = $q.defer();
                restServiceConsumer.postData("/PurchaseQuote/Purchase", _prepareSubmissionData(dataModel)).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var saveForLaterResource = resource('/PurchaseQuote/SaveForLater/');

            function _saveForLaterPurchaseQuoteData(dataModel, emailId) {
                var deferred = $q.defer();

                var promise = saveForLaterResource.save({ request: _prepareSubmissionData(dataModel), emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            function _getSavedQuotePurchaseData(quoteId) {


                //var GetPurchaseQuoteDataUrl = resource("/PurchaseQuote/GetPurchaseQuoteData");
                var GetPurchaseQuoteDataUrl = (quoteId) ? resource("/PurchaseQuote/GetPurchaseQuoteData?quoteId=" + quoteId) : resource("/PurchaseQuote/GetPurchaseQuoteData");
                var deferred = $q.defer();

                var promise = GetPurchaseQuoteDataUrl.save({ quoteId: quoteId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            //Comment : Here method will check user account existance based on supplied EmailId
            function _isExistingUser(emailId) {
                //debugger;
                var existingUserResource = resource("/PurchaseQuote/IsExistingUser");
                var deferred = $q.defer();

                var promise = existingUserResource.save({ emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            //Comment : Here method will validate user account passwrod based on supplied EmailId and Password
            function _hasValidPassword(emailId, password) {
                //debugger;
                var validPasswordResource = resource("/PurchaseQuote/HasValidPassword");
                var deferred = $q.defer();

                var promise = validPasswordResource.save({ emailId: emailId, password: password }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            function _forgotPassword(emailId) {
                debugger;
                var validPasswordResource = resource("/PurchaseQuote/ForgotPasswordRequest");
                var deferred = $q.defer();

                var promise = validPasswordResource.save({ emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            //Comment : Here method to show and hide Control level loader in application
            function _controlLevelLoader(elem, show) {
                if (show != null) {
                    if (show == true) {
                        elem.addClass("btn-loading");
                    }
                    else {
                        elem.removeClass("btn-loading");
                    }
                }
            }

            return {
                getQuotePurchasePageDefaults: _getQuotePurchasePageDefaults,
                validateCityStateZip: _validateCityStateZip,
                submitPurchaseData: _submitPurchaseData,
                getSavedQuotePurchaseData: _getSavedQuotePurchaseData,
                saveForLaterPurchaseQuoteData: _saveForLaterPurchaseQuoteData,
                isExistingUser: _isExistingUser,
                hasValidPassword: _hasValidPassword,
                controlLevelLoader: _controlLevelLoader,
                forgotPassword: _forgotPassword
            };
        }
    }
)();