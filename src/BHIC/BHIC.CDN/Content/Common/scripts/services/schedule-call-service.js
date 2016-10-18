(
    function () {
        'use strict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular
            .module('BHIC.Services')
            .factory('scheduleCallService', ['$resource', '$q', 'appConfig', scheduleCallServiceFn]);

        function scheduleCallServiceFn(resource, $q, appConfig) {

            var appBaseDomainUrl = appConfig.appBaseDomain;

            var scheduleCallResource = resource(appBaseDomainUrl + '/Home/SaveScheduleCallData');

            function _saveScheduleCall(fullName, contactNumber, selectedRequestTime, scheduleCallTime) {

                var deferred = $q.defer();
                var promise = scheduleCallResource.save({ fullName: fullName, contact: contactNumber, selectedRequestTime: selectedRequestTime, scheduleCallTime: scheduleCallTime }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }

            return {
                saveScheduleCall: _saveScheduleCall
            };
        }
    }
)();