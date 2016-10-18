(
    function () {
        /**
        @factory : this API/function will be used to get,set purchase page details 
        @description : user can submit his contact all other business information to move forward on VerifyOrder step/page in to buy a online policy.
        */
        var dependencies = ['bhic-app.services'];

        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.cacheData', ['bhic-app.services.restServiceConsumer', '$q', cacheDataFn]);

        //Comment : Here factory method implementation
        function cacheDataFn(restServiceConsumer, q) {
            
            var _getIndustryData = _getIndustryData;


            function _getIndustryData() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/GetIndustryList')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            }

            //Comment : Here expose factory methods & properties
            return {
                getIndustryData: _getIndustryData,
            };
        };
    }
)();