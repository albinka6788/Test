(
    function () {
        'use sctrict';
        /* This service will be used to get data from the Url on the
           basis of different inputs provided to the method
        */
        var dependencies = ['bhic-app.services'];
        angular
            .module('bhic-app.wc.services', dependencies)
            .factory('bhic-app.wc.services.userProfile', ['bhic-app.services.restServiceConsumer', '$q', userProfileFn]);

        function userProfileFn(restServiceConsumer, $q) {
            function User(firstname, lastname, phonenumber, policycode, email, password) {
                this.FirstName = firstname || null;
                this.LastName = lastname || null;
                this.PhoneNumber = phonenumber || null;
                this.PolicyCode = policycode || null;
                this.Email = email || null;
                this.Password = password || null;
            };
            function Login(email, password) {
                this.Email = email || null;
                this.Password = password || null;
            };
            function ForgotPassword(email) {
                this.Email = email || null;
            };

            var _submitUserData = function (data) {

                var model = new User(data.firstname, data.lastname, data.phonenumber, data.policycode, data.email, data.password);
                var deferred = $q.defer();

                restServiceConsumer.postData("/WcAccount/Register", model).
                    then(function (response) {
                        //  window.location.href = response;
                        deferred.resolve(response);
                        return response;
                    });
                return deferred.promise;
            }

            var _loginUser = function (data) {

                var model = new Login(data.email, data.password);
                var deferred = $q.defer();

                restServiceConsumer.postData("/Account/WcAccount/Login", model).
                    then(function (response) {
                        deferred.resolve(response);
                        return response;
                        //window.location.href = response;
                        //$scope.registrationShow = true;
                    });
                return deferred.promise;
            }

            var _forgotPassword = function (data) {
                var model = new ForgotPassword(data.email);
                var deferred = $q.defer();

                restServiceConsumer.postData("/Account/WcAccount/ForgotPassword", model).
                    then(function (response) {
                        deferred.resolve(response);
                        return response;
                        //window.location.href = response;
                        //$scope.registrationShow = true;
                    });
                return deferred.promise;
            };

            return {
                submitUserData: _submitUserData,
                loginUser: _loginUser,
                forgotPassword: _forgotPassword,
            };
        }
    }
)();