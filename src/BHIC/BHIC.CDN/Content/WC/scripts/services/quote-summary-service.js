(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.Services').factory('quoteSummaryService', ['$resource', '$q', quoteSummaryServiceFn]);

        function quoteSummaryServiceFn(resource, q)
        {
            //Comment : Here create resource object to make RESTful API calls
            var quoteSummaryResource = resource('/QuoteSummary/PostQuotePaymentPlan/');
            var quoteSummaryResourceForReCalculateQuote = resource('/QuoteSummary/ReCalculateWCDeductibleQuote/');

            /**
             * @description
             * saves the payment paln option for this Quote submitted by user
            */
            function _submitQuotePaymentPlan(paymentPlan) {
                var deferred = q.defer();

                var promise = quoteSummaryResource.save({ selectedPaymentPlan: paymentPlan }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
                //return quoteSummaryResource.save({ questionsList: questionsData }).$promise;
            }

            function _submitReCalculateQuote(selectedDeductible) {
                var deferred = q.defer();

                var promise = quoteSummaryResourceForReCalculateQuote.save({ selectedWCDeductible: selectedDeductible }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
                //return quoteSummaryResource.save({ questionsList: questionsData }).$promise;
            }


            //Comment : Here create resource object to make RESTful API calls
            var saveForLaterResource = resource('/QuoteSummary/SaveForLater/');

            /**
             * @description
             * saves the questionnaire (questions responses) submitted by user
            */
            function _submitSaveForLaterResponse(quoteSummaryData, selectedDeductiblePlan, emailId)
            {
                var deferred = q.defer();

                var promise = saveForLaterResource.save({ selectedPaymentPlan: quoteSummaryData, selectedDeductible: selectedDeductiblePlan, emailId: emailId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            //Comment : Here expose factory public methods 
            return {
                submitQuotePaymentPlan: _submitQuotePaymentPlan,
                submitSaveForLater: _submitSaveForLaterResponse,
                submitReCalculateQuote: _submitReCalculateQuote
            };
        }
    }
)();