(
function () {
    'use strict';
    angular.module('BHIC.LandingPage.Services').factory('sharedUserService', ['$q', '$http', sharedUserServiceFn]);
    function sharedUserServiceFn($q, $http) {
        var _user = null;
        var _policyCode = null;

        return {
            setUser: function (user) {
                _user = user
            },

            getUser: function () {
                if (_user == null && window.localStorage.getItem('isSessionCreated') != null)                
                    _user = window.localStorage.getItem('user');

                if (_user != null && window.localStorage.getItem('isSessionCreated') == null) {
                    _user = null;
                    window.localStorage.clear();
                }

                return _user;
            }
        }
    }
}
)();