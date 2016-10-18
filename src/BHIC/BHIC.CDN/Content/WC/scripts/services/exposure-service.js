(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular.module('BHIC.WC.Services')
            .factory('exposureService', ['restServiceConsumer', '$q', '$resource', 'coreUtils', quoteServiceFn]);

        function quoteServiceFn(restServiceConsumer, $q, resource, coreUtils) {

            /*Public Methods*/

            var _getSubIndustries = function (industryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Exposure/GetSubIndustries?IndustryId=' + industryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getClass = function (subIndustryId) {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Exposure/GetClassDescriptions?SubIndustryId=' + subIndustryId).then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _submitExposureData = function (data) {
                var deferred = $q.defer();
                var url = resource("/Exposure/SubmitExposureDetails");
                var promise = url.save({ request: data, fromSaveForLater: false }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;

            };

            var _validateMinExposurePayroll = function (classDescriptionId, classDescKeywordId, exposureAmt, classCode, directOK, industryId, subIndustryId) {
                var deferred = $q.defer();
                var minPayrollResource = resource('/Exposure/ValidateExposureAmount');
                var promise = minPayrollResource.save({ exposureAmt: exposureAmt, classDescriptionId: classDescriptionId, classDescKeywordId: classDescKeywordId, classCode: classCode, directSalesOK: directOK, industryId: industryId, subIndustryId: subIndustryId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };

            var _getCompanionCodes = function (classDescId, payrollAmount) {
                if (classDescId && classDescId != '') {
                    var deferred = $q.defer();
                    var getCompanionClass = resource("/Exposure/GetCompanionClasses");
                    var promise = getCompanionClass.get({ classDescId: classDescId, payrollAmount: payrollAmount }).$promise;
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
                var getQuoteViewModel = (quoteId) ? resource("/Exposure/GetQuoteViewModel?quoteId=" + quoteId) : resource("/Exposure/GetQuoteViewModel");
                var deferred = $q.defer();
                var promise = getQuoteViewModel.get({ quoteId: quoteId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };


            var _getPrimaryClassCodeData = function (stateCode, classDescId) {
                var getPrimaryClassCodeData = resource("/Exposure/GetPrimaryClassData");
                var deferred = $q.defer();
                var promise = getPrimaryClassCodeData.get({ stateCode: stateCode, classDescId: classDescId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            };
            var _getStateType = function () {
                var deferred = $q.defer();
                restServiceConsumer.getData('/Exposure/GetStateType').then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }
            var saveForLaterResource = resource('/Exposure/SaveForLaterComplete/');

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
                getSubIndustries: _getSubIndustries,
                getClass: _getClass,
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