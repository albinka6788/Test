(
function () {
    'use strict';
    angular.module('BHIC.Dashboard.Services').factory('sharedUserService', ['$resource', '$q', '$http', '$window', '$filter', sharedUserServiceFn]);
    function sharedUserServiceFn(resource, $q, $http, $window, $filter) {
        var _user = null;
        var _policyCode = null;

        return {
            getPolicyCode: function () {
                return _policyCode
            },

            setPolicyCode: function (policyCode) {
                _policyCode = policyCode;
            },

            setUser: function (user) {
                _user = user
            },
            getUserStatus: function () {
                var serviceURL = baseUrl + "/SessionExpired/GetUserStatus";
                var getUserStatus = resource(serviceURL);
                var deferred = $q.defer();
                var promise = getUserStatus.save().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            },

            getUser: function () {
                if (_user == null && $window.localStorage.getItem('isSessionCreated') != null)
                    _user = $window.localStorage.getItem('user');

                if (_user != null && $window.localStorage.getItem('isSessionCreated') == null) {
                    _user = null;
                    $window.localStorage.clear();
                }

                return _user;
            },

            decryptCYBKey: function (code) {
                var serviceURL = baseUrl + "/PolicyInformation/DecryptPolicyData";
                var getUserStatus = resource(serviceURL);
                var deferred = $q.defer();
                var promise = getUserStatus.save({ CYBKey: code }).$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            },

            getCurrentUserData: function () {
                var serviceURL = baseUrl + "/EditContactInfo/GetCurrentUserData";
                var getUserStatus = resource(serviceURL);
                var deferred = $q.defer();
                var promise = getUserStatus.save().$promise;
                promise.then(function (data) {
                    deferred.resolve(data);
                    return data;
                });
                return deferred.promise;
            },
            //Formatting the date
            FormatDate: function (dateVal) {
                var value = new Date(dateVal);
                return $filter('date')(value, 'MM/dd/yyyy');
            },

            //Comment : Here formatting the date
            convertToDate: function (inputString) {
                var convertedDate = new Date(parseInt(inputString.match(/\d/g).join("")));
                return $filter('date')(convertedDate, 'MM/dd/yyyy');
            },

            //For validating Phone Number
            CheckPhoneNumber: function (number) {
                var formattedNumber;
                if (number.toString().length < 18) {
                    formattedNumber = number.toString();
                    return formattedNumber;
                }
                var area = number.substring(0, 3);
                var front = number.substring(4, 7);
                var middle = number.substring(8, 12);
                var end = number.substring(14, 18);

                formattedNumber = area + front + middle;
                formattedNumber = formattedNumber.replace(/_/g, '');
                if (formattedNumber.length < 10) {
                    return formattedNumber;
                }
                else {
                    formattedNumber += end;
                    formattedNumber = formattedNumber.replace(/_/g, '');
                }
                return formattedNumber;
            }
        }
    }
}
)();