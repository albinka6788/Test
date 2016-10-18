(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.Services').factory('businessInfoService', ['restServiceConsumer', '$resource', '$q', businessInfoServiceFn]);

        function businessInfoServiceFn(restServiceConsumer, resource, q)
        {
            //Comment : Here create resource object to make RESTful API calls
            var businessInfoResource = resource('/BusinessInfo/PostBusinessInfo/');

            /**
             * @description
             * saves the BusinessInfo (companyName,zipCode,emailId) & LOB submitted by user
            */
            function _submitBusinessInfo(businessInfo)
            {
                var deferred = q.defer();

                var promise = businessInfoResource.save({ businessInfoVM: businessInfo }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            //Comment : Here expose factory public methods 
            return {
                submitBusinessInfo: _submitBusinessInfo,
            };
        }
    }
)();