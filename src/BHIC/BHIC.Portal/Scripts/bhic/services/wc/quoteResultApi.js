(
    function () {
        /**
        @factory : this API/function will be used to get,set purchase page details 
        @description : user can submit his contact all other business information to move forward on VerifyOrder step/page in to buy a online policy.
        */
        var dependencies = ['bhic-app.services'];

        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.quoteResultData', ['bhic-app.services.restServiceConsumer', '$q', quoteResultDataFn]);

        //Comment : Here factory method implementation
        function quoteResultDataFn(restServiceConsumer, q) {

            var _getQuoteResult = _getQuoteResult;
           
            function _getQuoteResult() {
                var deferred = q.defer();

                restServiceConsumer.getData('/WcHome/FetchQuoteResult')
                    .then(function (data) {
                        deferred.resolve(data);
                    });
                return deferred.promise;
            }

            function _submitQuoteResult(dataModel) {
                //Comment : Here create mapping of data according to server ViewModel object
                //var _wcQuoteViewModel = {};

                var deferred = q.defer();
                restServiceConsumer.postData("/Landing/Wchome/PostQuoteResult", dataModel).then(function (data) {

                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }
            
            //Comment : Here expose factory methods & properties
            return {
                getQuoteResult: _getQuoteResult,
                submitQuoteResult:_submitQuoteResult
            };
        };
    }
)();