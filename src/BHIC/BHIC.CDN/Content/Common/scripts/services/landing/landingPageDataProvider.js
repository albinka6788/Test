(
    function () {
        'use strict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        angular
            .module('BHIC.Services')
            .factory('landingPageData', ['$resource', '$q','appConfig', landingPageDataFn]);

        function landingPageDataFn(resource, q, appConfig) {

            //console.log('Able to get app Config varibale : ' + appConfig.appBaseDomain);
            var appBaseDomainUrl = appConfig.appBaseDomain;

            var zipCodeResource = resource(appBaseDomainUrl + '/Home/GetValidZipDetail');

            function _getValidZip(zipCode) {
                var deferred = q.defer();

                var promise = zipCodeResource.get({ zipCode: zipCode }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var lobResource = resource(appBaseDomainUrl + '/Home/GetLobList');

            function _getLobList(state) {
                var deferred = q.defer();

                var promise = lobResource.get({ stateCode: state }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var saveZipCodeResource = resource(appBaseDomainUrl + '/Home/Index');

            function _saveZipAndState(zipCode, state, lobId) {
                var deferred = q.defer();

                var promise = saveZipCodeResource.save({ zipCode: zipCode, state: state, lobId: lobId }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var sessionResource = resource(appBaseDomainUrl + '/Home/GetStateAndZipFromSession');

            function _getSessionData() {

                var deferred = q.defer();

                var promise = sessionResource.get().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            //Comment : Here added log-out service call
            var signOutResource = resource(appBaseDomainUrl + '/Home/SignOut');

            function _signOutUser() {
                var deferred = q.defer();

                var promise = signOutResource.get().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var bopPathResource = resource(appBaseDomainUrl + '/Home/GetBOPPath');

            function _getBOPUrl(zipCode, state) {
                var deferred = q.defer();

                var promise = bopPathResource.get({ zipCode: zipCode, state: state }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var saveCASessionResource = resource(appBaseDomainUrl + '/Home/SaveCASession');

            function _saveCASession(zipCode) {
                var deferred = q.defer();

                var promise = saveCASessionResource.get({ zipCode: zipCode }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }

            var getNewQuoteResource = resource(appBaseDomainUrl + '/Home/GetNewQuote');

            function _getNewQuote(zipCode)
            {
                var deferred = q.defer();

                var promise = getNewQuoteResource.save({ quoteId: '' }).$promise;
                promise.then(function (data)
                {
                    deferred.resolve(data);
                    return data;
                });

                return deferred.promise;
            }


            return {
                getValidZip: _getValidZip,
                getLobList: _getLobList,
                saveZipAndState: _saveZipAndState,
                getSessionData: _getSessionData,
                signOutUser: _signOutUser,
                getBOPUrl: _getBOPUrl,
                saveCASession: _saveCASession,
                getNewQuote: _getNewQuote
            };
        }
    }
)();