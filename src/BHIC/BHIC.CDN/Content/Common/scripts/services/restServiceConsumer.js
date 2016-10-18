(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular
            .module('BHIC.Services')
            .factory('restServiceConsumer', ['$http', '$q', '$resource', '$log', 'coreUtils','appConfig', restServiceConsumerFn]);

        function restServiceConsumerFn(http, q, $resource, $log, coreUtils, appConfig)
        {
            var baseDomainUrl = appConfig.appBaseDomain;

            var _method = {
                GET: "GET",
                POST: "POST"
            };

            var _serverUrls = {}
            var _getDataFromUrl = function (url, params, data, type, dataType)
            {
                //debugger;                
                url = baseDomainUrl + url;

                var deferred = q.defer();
                var urlType = url.substr(url.lastIndexOf('.') + 1);

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

            var _postData = function (url, data, config)
            {
                //debugger;
                url = baseDomainUrl + url;

                var deferred = q.defer();

                //Comment : here in case config header not passed then set it to null
                config = config || null;

                if (config != null) {
                    http.post(url, data, config).success(function (data) {
                        deferred.resolve(data);
                    });
                }
                else {
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
            };

            function _createRequestObject(url, isArray, method) {
                return {
                    method: method,
                    isArray: isArray,
                    url: url,
                    transformResponse: [function (data, headersGetter) {
                        return { data: data };
                    }].concat(http.defaults.transformResponse)
                }
            }

            //Comment : Here create resource object to make RESTful API calls
            var validTaxIdNumberResource = $resource(baseDomainUrl + '/Questions/IsValidTaxIdNumber/');

            /**
             * @description
             * common method to get the validation status for supplied taxid-number (FEIN/SSN/TIN) across application
            */
            function _isValidTaxIdNumber(feinNumber, saveInSession)
            {
                var deferred = q.defer();
                //GUIN-238 : saveInSession in following promise will denote if feinNumber/taxId value will be updated in session or not 
                var promise = validTaxIdNumberResource.save({ taxIdNumber: coreUtils.ConvertToNumeric(feinNumber), saveInSession: saveInSession }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }
            

            //Comment : Here create resource object to make RESTful API calls
            var validZipCodeResource = $resource(baseDomainUrl + '/BusinessInfo/ValidateZipCodeLobs/');

            /**
             * @description
             * common method to get the validating ZipCode and get other related data like LobList,StateCode
            */
            function _validateZipCodeLobs(zipCode)
            {
                var deferred = q.defer();
 
                var promise = validZipCodeResource.save({ zipCode: (typeof zipCode != 'undefined') ? zipCode : '' }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            //Comment : Here create resource object to make RESTful API calls
            var saveForLaterResource = $resource(baseDomainUrl + '/Exposure/SaveForLater/');

            /**
                * @description
                * common method to submit SaveForLater data and send mail with link to revoke quote across application
            */
            function _sendSaveForLaterLink(retrieveQuotePageName, userEmailId)
            {
                var deferred = q.defer();

                var promise = saveForLaterResource.save({ pageName: retrieveQuotePageName, emailId: userEmailId }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            return {

                landing: $resource({}, {}, {
                    query: { method: "GET", url: ":url", isArray: false },
                    create: {
                        method: "POST",
                        url: appConfig.appBaseDomain + "/Home/Index?zipCode=:zipCode",
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)

                    },
                    get: {
                        method: "GET",
                        isArray: false,
                        url: appConfig.appBaseDomain + "/Home/IsValidZip?zipCode=:zipCode",
                        transformRequest: [function (data, headersGetter) {
                        }].concat(http.defaults.transformRequest),
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    },
                    remove: { method: "DELETE", url: ":url" },
                    update: { method: "PUT", url: ":url" },
                    getQuotePageDefaults: {
                        method: "GET",
                        isArray: false,
                        url: appConfig.appCdnDomain + "/Content/WC/data/quote-page-defaults.js",
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    },
                    getSessionData: {
                        method: "GET",
                        isArray: false,
                        url: appConfig.appBaseDomain + "/Quote/GetQuoteViewModel",
                        params: { quoteId: coreUtils.GetQuoteIdFromUrl() },
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    },
                    getQuotePurchasePageDefaults: {
                        method: "GET",
                        isArray: false,
                        url: appConfig.appBaseDomain + "/PurchaseQuote/GetBusinessTypes",
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    }
                }),
                purchaseQuote: $resource({}, {}, {
                    getQuotePurchasePageDefaults: {
                        method: "POST",
                        isArray: false,
                        url: appConfig.appBaseDomain + "/PurchaseQuote/GetQuotePurchasePageDefaults?quoteId=:quoteId",
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    },
                    validateCityStateZip: {
                        method: "POST",
                        url: appConfig.appBaseDomain + "/PurchaseQuote/InValidateStateCityZip",
                        params: {},
                        transformResponse: [function (data, headersGetter) {
                            return { data: data };
                        }].concat(http.defaults.transformResponse)
                    }
                }
                ),

                getData: _getDataFromUrl,
                postData: _postData,
                //Comment : Here common service point to validate taxid-numbers across application
                isValidTaxIdNumber: _isValidTaxIdNumber,
                sendSaveForLaterLink: _sendSaveForLaterLink,
                validateZipCodeLobs: _validateZipCodeLobs
            };
        }
    }
)();