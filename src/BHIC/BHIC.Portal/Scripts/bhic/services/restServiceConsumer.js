(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular
            .module('bhic-app.services', [])
            .factory('bhic-app.services.restServiceConsumer', ['$http', '$q', '$log', restServiceConsumerFn]);

        function restServiceConsumerFn(http, q, $log) {
            var _getDataFromUrl = function (url, params, data, type, dataType) {
                var deferred = q.defer();
                var urlType = url.substr(url.lastIndexOf('.') + 1);
                //if (urlType == 'json') {
                //    _fetchJsonData(url).success(function (response) {
                //        deferred.resolve(response);
                //    });
                //}
                if (url && params && data && type && dataType) {
                    http({
                        url: url,
                        data: data,
                        method: type || 'get',
                        type: dataType || 'json',
                        params: params
                    })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (msg, code) {
                        deferred.reject(msg);
                        $log.error(msg, code);
                    });
                }
                else if (url) {
                    http.get(url).success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (msg, code) {
                        deferred.reject(msg);
                        $log.error(msg, code);
                    });
                }
                return deferred.promise;
            };

            var _postData = function (url, data,config) {
                var deferred = q.defer();

                //Comment : here in case config header not passed then set it to null
                config = config || null;

                if (config != null)
                {
                    http.post(url, data, config).success(function (data) {
                        deferred.resolve(data);
                    });
                }
                else
                {
                    http.post(url, data).success(function (data) {
                        deferred.resolve(data);
                    });
                }
                
                return deferred.promise;
            };

            var _fetchJsonData = function (url) {
                $.getJSON(url).success(function (data) {
                    return data;
                });
            }
            return {
                getData: _getDataFromUrl,
                postData: _postData
            }
        }
    }
)();