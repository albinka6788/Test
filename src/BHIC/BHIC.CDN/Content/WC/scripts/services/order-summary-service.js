(
    function () {
        'use strict';

        angular.module('BHIC.Services').
            factory('orderSummaryService', ['$resource', '$q', '$http', orderSummaryServiceFn]);

        function orderSummaryServiceFn(resource, q) {

            var orderSummaryResource = resource('/OrderSummary/GetOrderSummary');

            function _getOrderSummary() {

                var deferred = q.defer();

                var promise = orderSummaryResource.get().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }
       
            return {
                getOrderSummary: _getOrderSummary
            };
        }
    }
)();