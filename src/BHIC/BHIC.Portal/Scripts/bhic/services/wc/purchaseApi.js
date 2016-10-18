(
    function () {
        /**
        @factory : this API/function will be used to get,set purchase page details 
        @description : user can submit his contact all other business information to move forward on VerifyOrder step/page in to buy a online policy.
        */
        var dependencies = ['bhic-app.services'];

        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.purchasePolicyPageData', ['bhic-app.services.restServiceConsumer', '$q', purchasePolicyPageDataFn]);

        //Comment : Here factory method implementation
        function purchasePolicyPageDataFn(restServiceConsumer, q) {
            var _getQuoteReferralValidation = '';
            var _submitQuoteReferralData = '';

            var _submitPurchaseData = _submitPurchaseData;
            var _getPurchaseData = _getPurchaseData;
            var _getBusinessTypes = _getBusinessTypes;

            function _submitPurchaseData(dataModel) {
                //Comment : Here create mapping of data according to server ViewModel object
                var _wcPurchaseViewModel = {

                    Policy: {
                        //PolicyDate: dataModel.policy.policyDate
                        PolicyDate: new Date(dataModel.policy.policyDate).toUTCString()
                    },

                    PersonalContact: {
                        SameAsContact: dataModel.personalContact.sameAsContact,
                        Email: dataModel.personalContact.email,
                        ConfirmEmail: dataModel.personalContact.confirmEmail,
                        FirstName: dataModel.personalContact.firstName,
                        LastName: dataModel.personalContact.lastName,
                        PhoneNumber: dataModel.personalContact.phoneNumber
                    },

                    BusinessContact: {
                        Email: dataModel.businessContact.bEmail,
                        ConfirmEmail: dataModel.businessContact.bConfirmEmail,
                        FirstName: dataModel.businessContact.bFirstName,
                        LastName: dataModel.businessContact.bLastName,
                        PhoneNumber: dataModel.businessContact.bPhoneNumber
                    },

                    Account: {
                        Password: dataModel.account.password,
                        ConfirmPassword: dataModel.account.confirmPassword
                    },

                    BusinessInfo: {
                        BusinessName: dataModel.businessInfo.businessName,
                        BusinessType: dataModel.businessInfo.businessType,
                        TaxIdType: dataModel.businessInfo.taxIdType,
                        TaxIdOrSSN: dataModel.businessInfo.taxIdOrSSN,
                        TaxIdTypeText: dataModel.businessInfo.taxIdTypeText,
                        TaxIdOrSSNText: dataModel.businessInfo.taxIdOrSSNText
                    },

                    MailingAddress: {
                        AddressLine1: dataModel.mailingAddress.addressLine1,
                        AddressLine2: dataModel.mailingAddress.addressLine2,
                        City: dataModel.mailingAddress.city,
                        State: dataModel.mailingAddress.state,
                        Zip: dataModel.mailingAddress.zip
                    }
                };

                var deferred = q.defer();
                restServiceConsumer.postData("/Landing/Wchome/Purchase", _wcPurchaseViewModel).then(function (data) {

                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            function _getPurchaseData() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/GetPurchaseModel')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            }

            function _getBusinessTypes() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/GetBusinessTypes')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            function _validateStateCityZip(zip, city, state) {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/ValidateStateCityZip?zipCode=' + zip + '&city=' + city + '&state=' + state)
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            }
            function _createPolicy() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/CreatePolicy')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            }
            //Comment : Here expose factory methods & properties
            return {
                submitPurchaseData: _submitPurchaseData,
                getPurchaseData: _getPurchaseData,
                getBusinessTypes: _getBusinessTypes,
                validateStateCityZip: _validateStateCityZip,
                createPolicy: _createPolicy
            };

        };
    }
)();