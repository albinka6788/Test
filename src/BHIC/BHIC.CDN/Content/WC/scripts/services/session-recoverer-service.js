(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.WC.Services').factory('sessionRecovererService', ['$q', '$injector', '$location', 'loadingService', 'appConfig', sessionRecovererServiceFn]);

        function sessionRecovererServiceFn(q, injector, location, loadingService, appConfig) {
            var sessionRecoverer =
            {
                'request': function (config) {

                    //debugger;
                    if (config != null && config != undefined)
                    {
                        //Comment : Here get requested resource relative url
                        var httpRequestRelativeUrl = config.url;

                        if (httpRequestRelativeUrl != null && httpRequestRelativeUrl != undefined)
                        {
                            //Comment : Here Prepend/Set application base url(AppBaseUrl) with requesting resource 
                            //config.url = 'http://localhost:50350' + httpRequestRelativeUrl;
                            var baseDomainUrl = appConfig.appBaseDomain;

                            //Comment : Here if relative path already having Resource Absolute path then don't prepend below path (baseDomainUrl)
                            if (httpRequestRelativeUrl.indexOf('http://') > -1 || httpRequestRelativeUrl.indexOf('https://') > -1)
                            {
                                config.url = config.url;
                            }
                            else
                            {
                                config.url = baseDomainUrl + httpRequestRelativeUrl;
                            }
                        }
                    }
                    return config;
                },
                'response': function (response)
                {
                    //var deferred = q.defer();                    

                    if (response != null && response != undefined)
                    {
                        //debugger;

                        //Comment : Here response data object
                        var responseData = response.data;
                        var appSessionExpired = false;
                        //console.log(responseData);

                        //Comment : Here convert stringified JSON to JSON object
                        if (
                                typeof (responseData) == 'string' &&
                                (
                                    (responseData.indexOf('<meta name="AppSessionTimedOut" content="true">') != -1)         //In case app session or cookies has been expired
                                    || (responseData.indexOf('<meta name="AppAuthTokenNotFound" content="true">') != -1)    //In case app antoforgory toekn not found
                                )
                            )
                        {
                            appSessionExpired = true;
                        }
                        
                        /*
                        //Comment : Here convert stringified JSON to JSON object
                        if (typeof(responseData) == 'string' && (responseData.indexOf('"responseCode":419') != -1 || responseData.indexOf('"responseCode":401') != -1))
                        {
                            responseData = JSON.parse(responseData);
                        }

                        var responseCode = (responseData != null && responseData != undefined && responseData != '') ? responseData.responseCode : null;
                        //console.log(responseCode);
                        */

                        if (appSessionExpired)
                        {
                            location.path('/SessionExpired/');

                            /*switch (responseCode)
                            {
                                case 401:
                                case 419:
                                    location.path('/SessionExpired/');

                                    /*
                                    //Comment : Here abort http calls
                                    //location.path('/UnAuthorised/');
                                    var $http = injector.get('$http');
                                    $http.abort(response);
                                    
                                    rootScope.$on('$stateChangeStart', function (event, toState, toParams) 
                                    {
                                      angular.forEach($http.pendingRequests, function (request) 
                                      {
                                            $http.abort(request);
                                        });
                                    });
                                    
    
                                    //Comment : Here reject execution
                                    //responseData = new Object();
                                    //q.reject(responseData);
                            }*/
                        }
                    }
                    return response;
                },
                'responseError': function (response) {
                    // Session has expired
                    if (response.status == 419) {
                        //console.log('session expired interceptor called');
                        location.path("/AppError/");
                    }
                    return q.reject(response);
                }
            };
            return sessionRecoverer;
        }
    }
)();