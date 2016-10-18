(
    function () {
        'use strict';

        angular.module('BHIC.Services').
            factory('policyPaymentService', ['$resource', '$q', '$http', policyPaymentServiceFn]);

        function policyPaymentServiceFn(resource, q) {

            var paymentPlanResource = resource('/PolicyPurchase/GetPaymentPlan');

            function _getPaymentPlans() {

                var deferred = q.defer();

                var promise = paymentPlanResource.get().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var changePlanResource = resource('/PolicyPurchase/UpdatePaymentSessionDetails');

            function _updatePaymentPlans(selectedPlanId) {

                var deferred = q.defer();

                var promise = changePlanResource.get({ selectedPlanId: selectedPlanId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            return {
                getPaymentPlans: _getPaymentPlans,
                updatePaymentPlans: _updatePaymentPlans
            };
        }

    }
)();