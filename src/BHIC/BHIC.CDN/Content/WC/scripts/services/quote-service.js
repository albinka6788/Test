(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular.module('BHIC.WC.Services')
            .factory('quoteService', ['restServiceConsumer', '$q', '$resource', 'coreUtils', quoteServiceFn]);

        function quoteServiceFn(restServiceConsumer, $q, resource, coreUtils) {

            /*Public Methods*/
            var _getIndustries = function () {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Quote/GetIndustries')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            };

            var _getSubIndustries = function (industryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Quote/GetSubIndustries?IndustryId=' + industryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getClass = function (subIndustryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Quote/GetClassDescriptions?SubIndustryId=' + subIndustryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getQuotePageDefaults = function () {
                var deferred = $q.defer();
                restServiceConsumer.landing.getQuotePageDefaults().$promise.then(function (data) {
                    deferred.resolve(data);
                });
                return deferred.promise;
            };

            var _submitExposureData = function (data) {
                var deferred = $q.defer();
                //data.InceptionDate = new Date(data.InceptionDate).toUTCString();
                restServiceConsumer.postData("/Quote/SaveLandingPageData", data).then(function (response) {
                    deferred.resolve(response);
                });
                return deferred.promise;
            };

            var _validateMinExposurePayroll = function (classDescriptionId, classDescKeywordId, exposureAmt, classCode, directOK, industryId, subIndustryId) {
                var deferred = $q.defer();
                var minPayrollResource = resource('/Quote/ValidateExposureAmount');
                var promise = minPayrollResource.save({ exposureAmt: exposureAmt, classDescriptionId: classDescriptionId, classDescKeywordId: classDescKeywordId, classCode: classCode, directSalesOK: directOK, industryId: industryId, subIndustryId: subIndustryId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                //return deferred.promise;

                //restServiceConsumer.getData('/Quote/ValidateExposureAmount?exposureAmt=' + exposureAmt + '&classDescriptionId=' + classDescriptionId + '&classDescKeywordId=' + classDescKeywordId).then(function (data) {
                //    deferred.resolve(data);
                //    return data;
                //});
                return deferred.promise;
            };

            var _getCompanionCodes = function (classDescId, payrollAmount) {
                if (classDescId && classDescId != '') {
                    var deferred = $q.defer();
                    var getCompanionClass = resource("/Quote/GetCompanionClasses");
                    var promise = getCompanionClass.save({ classDescId: classDescId, payrollAmount: payrollAmount }).$promise;
                    promise.then(function (data) {
                        deferred.resolve(data);
                        return data;
                    });

                    return deferred.promise;
                }
                else
                    return null;
            };

            var _getQuoteViewModel = function (quoteId) {
                var getQuoteViewModel = (quoteId) ? resource("/Quote/GetQuoteViewModel?quoteId=" + quoteId) : resource("/Quote/GetQuoteViewModel");
                var deferred = $q.defer();
                var promise = getQuoteViewModel.save({ quoteId: quoteId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };


            var _getPrimaryClassCodeData = function (stateCode, classDescId) {
                var getPrimaryClassCodeData = resource("/Quote/GetPrimaryClassData");
                var deferred = $q.defer();
                var promise = getPrimaryClassCodeData.save({ stateCode: stateCode, classDescId: classDescId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };
            var _getStateType = function () {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Quote/GetStateType').then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }
            var saveForLaterResource = resource('/Quote/SaveForLaterComplete/');

            /**
             * @description
             * saves the questionnaire (questions responses) submitted by user
            */
            function _submitSaveForLaterExposureData(data, emailId) {
                var deferred = $q.defer();

                var promise = saveForLaterResource.save({ request: data, emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }
            return {
                getIndustries: _getIndustries,
                getSubIndustries: _getSubIndustries,
                getClass: _getClass,
                getQuotePageDefaults: _getQuotePageDefaults,
                submitExposureData: _submitExposureData,
                validateMinExposurePayroll: _validateMinExposurePayroll,
                getCompanionCodes: _getCompanionCodes,
                getQuoteViewModel: _getQuoteViewModel,
                getStateType: _getStateType,
                submitSaveForLaterExposureData: _submitSaveForLaterExposureData,
                getPrimaryClassCodeData: _getPrimaryClassCodeData
            };
        }
    }
)();