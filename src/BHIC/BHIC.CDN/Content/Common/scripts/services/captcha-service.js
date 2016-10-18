
(
    function () {
        'use strict';
        /* This service will be used to display loader while processing http request
        */
        angular
            .module('BHIC.Services')
            .factory('captchaService', ['$resource', '$q', '$http', captchaServiceFn]);

        function captchaServiceFn(resource, $q, $http) {

            var serviceURL = baseUrl + "/Login/";

            function _validateCaptcha(code) {
                var validateCaptcha = resource(serviceURL + 'Validate');
                var deferred = $q.defer();
                var promise = validateCaptcha.save({ code: code }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            }

            return {
                validateCaptcha: _validateCaptcha
            };

        }
    }
)();