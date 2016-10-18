(
    function () {
        'use strict';
        angular.module('BHIC.Dashboard.Services').factory('unauthorisedUserWrapperService', ['$window', '$resource', '$q', '$http', 'loadingService', 'sharedUserService', unauthorisedUserWrapperServiceFn]);
        function unauthorisedUserWrapperServiceFn($window, $resource, $q, $http, loadingService, sharedUserService) {

            var serviceURL = baseUrl;

            // Method is used to handle the success 
            function handleSuccess(res) {
                //return res.data;
                if (sharedUserService.getUser() == null)
                    sharedUserService.setUser(res.data.user)

                return res.data;

            }

            // Method is used to catch the error handler from the service.
            function handleError(error) {
                try {
                    console.clear();
                    console.log(" HTTP Response Code : " + error.status);
                    console.log(" HTTP Method Type : " + error.config.method);
                    console.log(" Server API Url : " + error.config.url);
                    loadingService.hideLoader();

                } catch (e) {
                    window.location.href = baseUrl + "/";
                }
                //return function () {
                //    return { success: false, message: error };
                //};
            }

            // Method is used for only for login success
            function loginSuccess(res) {


                return res.data;
            }

            // Register Server UnAuthorised API's
            function RegisteredUnAuthorisedAPI(wrapperObject) {
                var queryString = wrapperObject.queryString;
                var postData = wrapperObject.postData;
                try {
                    switch (wrapperObject.method) {
                        // Login Related Api Calls.
                        case "UserLogin":
                            return $http.post(serviceURL + 'Login/UserLogin', postData).then(loginSuccess, handleError);
                            break;

                            // Account Registration Api Calls.
                        case "CreateAccount":
                            return $http.post(serviceURL + 'Registration/CreateAccount', postData).then(handleSuccess, handleError);
                            break;

                        case "ResendMail":
                            return $http.post(serviceURL + 'Login/ResendMail', postData).then(handleSuccess, handleError);
                            break;

                            // Report Claim Api Calls.
                        case "RequestReportClaimFromHome":
                            return $http.post(serviceURL + 'ReportClaim/RequestReportClaimFromHome', postData).then(handleSuccess, handleError);
                            break;

                            // Password Related Api Calls.
                        case "ForgotPasswordRequest":
                            return $http.post(serviceURL + 'ForgotPassword/ForgotPasswordRequest', postData).then(handleSuccess, handleError);
                            break;

                        case "GetEmail":
                            return $http.get(serviceURL + 'ResetPassword/GetEmail?key=' + queryString).then(handleSuccess, handleError);
                            break;

                        case "ResetPasswordRequest":
                            return $http.post(serviceURL + 'ResetPassword/ResetPasswordRequest', postData).then(handleSuccess, handleError);
                            break;
                    }
                } catch (e) {
                    console.clear();
                    console.log(" HTTP Response Code : " + e.message);
                    loadingService.hideLoader();
                }
            }

            // Registering Angular Common Api for all the controllers
            return {
                UnAuthorisedUserApiCall: RegisteredUnAuthorisedAPI
            };
        }
    }
)();