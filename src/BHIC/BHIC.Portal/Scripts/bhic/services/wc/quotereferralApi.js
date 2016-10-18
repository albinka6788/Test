(
    function () {
        'use strict';

        /**
        @factory : this function will be used to render/show questions based on exposure data which includes ZipCode,Industry and other home page key params
        @description : user can submit his response also to get Quote Status next step/page in flow to buy a online policy.
        */
        var dependencies = ['bhic-app.services'];

        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.quotereferralPageData', ['bhic-app.services.restServiceConsumer', '$q', quotereferralPageDataFn]);
        function SaveForLater(email) {
            this.Email = email || null;
        };


        //Comment : Here factory method implementation
        function quotereferralPageDataFn(restServiceConsumer, q)
        {
            //declration portion
            var _getQuoteReferralValidation = _getQuoteReferralValidation;
            var _getBusinessTypes = _getBusinessTypes;
            var _submitQuoteReferralData = _submitQuoteReferralData;

            //implementation portion
            function _getQuoteReferralValidation()
            {
                var deferred = q.defer();
               
                restServiceConsumer.getData('/WcHome/GetReferralValidations')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            function _getBusinessTypes() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/GetBusinessTypes')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            function _submitQuoteReferralData(wcQuoteViewModel, config)
            {
                var deferred = q.defer();
                //restServiceConsumer.postData("/WcHome/ResultsReferQPost", wcQuoteViewModel).then(function (data) {
                //    deferred.resolve(data);
                //    return data;
                //});

                //Comment : Here create quote referral object to post data in Viewmodel form
                var _wcQuoteViewModel = {
                    //Comment : Here "Business Information" object
                    BusinessType: wcQuoteViewModel.businessInfo.businessType,
                    BusinessName: wcQuoteViewModel.businessInfo.businessName,

                    //Comment : Here "Business Contact Information" object
                    Email: wcQuoteViewModel.businessContactInfo.businessContactEmail,
                    ContactFirstName: wcQuoteViewModel.businessContactInfo.businessContactFName,
                    ContactLastName: wcQuoteViewModel.businessContactInfo.businessContactLName,
                    ContactPhone: wcQuoteViewModel.businessContactInfo.businessContactPhone,

                    //Comment : Here "Business Mailing Address" object
                    MailAddr1: wcQuoteViewModel.businessMailingInfo.addressLine1,
                    MailAddr2: wcQuoteViewModel.businessMailingInfo.addressLine2,                    
                    MailCity: wcQuoteViewModel.businessMailingInfo.city,
                    MailState: wcQuoteViewModel.businessMailingInfo.state,
                    MailZip: wcQuoteViewModel.businessMailingInfo.zipCode,
                };

                restServiceConsumer.postData("/WcHome/ResultsReferQPost", _wcQuoteViewModel,config).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });                

                return deferred.promise;
            }

            //Comment : Here expose factory methods & properties
            return {
                getQuoteReferralValidation: _getQuoteReferralValidation,
                getBusinessTypes:_getBusinessTypes,
                submitQuoteReferralData: _submitQuoteReferralData
            };
        };
    }
)();