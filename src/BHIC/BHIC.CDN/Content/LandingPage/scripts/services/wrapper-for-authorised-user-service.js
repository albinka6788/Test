(
    function () {
        'use strict';
        angular.module('BHIC.LandingPage.Services').factory('authorisedUserWrapperService', ['$window', '$q', '$http', 'loadingService', 'sharedUserService', authorisedUserWrapperServiceFn]);
        function authorisedUserWrapperServiceFn($window, $q, $http, loadingService, sharedUserService) {

            var baseUrl = "LandingPage/";
            var serviceURL = baseUrl;


            // Method is used to handle the success 
            function handleSuccess(res) {
                //if (sharedUserService.getUser() == null)
                //    sharedUserService.setUser(res.data.user);

                //return (res.data === undefined) ? res : res.data;

                return res.data;
            }

            // Method is used to catch the error handler from the service.
            function handleError(error) {
                console.clear();
                console.log(" HTTP Response Code : " + error.status);
                console.log(" HTTP Method Type : " + error.config.method);
                console.log(" Server API Url : " + error.config.url);
                loadingService.hideLoader();

            }

            // Method is used only for logout success.
            function logoutSuccess(res) {
                var retValue = false;
                return (res.data.success)
                     ? { success: true }
                     : { success: retValue };
            }

            // Register Server API's
            function RegisteredAPI(wrapperObject) {
                var queryString = wrapperObject.queryString;
                var postData = wrapperObject.postData;
                var encryptedKey = window.name;
                try {
                    switch (wrapperObject.method) {
                        // Server Api Calls.                  
                        case "Authentication":
                            return $http.post(serviceURL + 'Login/CheckAuthentication', "{'postData':'" + JSON.stringify(postData) + "'}").then(handleSuccess, handleError);
                            break;
                        case "GetAllLandingPages":
                            return $http.get(serviceURL + 'Login/GetAllLandingPages').then(handleSuccess, handleError);
                            break;
                        case "GetDefaultLists":
                            return $http.get(serviceURL + 'Login/GetDefaultLists').then(handleSuccess, handleError);
                            break;
                        case "GetLandingPageDetailsByTokenId":
                            return $http.get(serviceURL + 'Login/GetLandingPageDetailsByTokenId?TokenId=' + queryString).then(handleSuccess, handleError);
                            break;
                        case "InsertOrUpdateLandingPage":
                            return $http.post(serviceURL + 'Login/AddOrUpdateLandingPage', postData).then(handleSuccess, handleError);
                            break;
                        case "GetTemplateByTokenId":
                            return $http.get(serviceURL + 'Login/GetLandingPageDetailsByTokenId?TokenId=' + queryString).then(handleSuccess, handleError);
                            break;
                        case "GetAd":
                            return $http.post(serviceURL + 'Login/GetAd', "{'postData':'" + JSON.stringify(postData) + "'}").then(handleSuccess, handleError);
                            break;
                        case "DeleteLandingPages":
                            return $http.post(serviceURL + 'Login/DeleteLandingPages', "{'postData':'" + JSON.stringify(postData) + "'}").then(handleSuccess, handleError);
                            break;
                        case "GetCurrentEnvironment":
                            return $http.get(serviceURL + 'Login/GetCurrentEnvironment').then(handleSuccess, handleError);
                            break;
                        case "GetValidZipDetail":
                            return $http.get(serviceURL + 'Home/GetValidZipDetail?zipCode=' + queryString).then(handleSuccess, handleError);
                            break;
                        case "Logout":
                            return $http.post(serviceURL + 'Login/Logout').then(logoutSuccess, handleError);
                            break;
                    }
                } catch (e) {
                    console.clear();
                    console.log(" HTTP Response Code : " + e.message);
                    loadingService.hideLoader();
                }
            }

            // Method is used to Check the user exists & call the apis.
            function _getCurrentUser(wrapperObject) {
                return (!$window.localStorage.getItem("isSessionKilled"))
                            ? RegisteredAPI(wrapperObject)
                            : window.location.href = baseUrl + "/";
            }


            // Registering Angular Common Api for all the controllers
            return {
                AuthorisedUserApiCall: _getCurrentUser
            };
        }
    }
)();