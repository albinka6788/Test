(
    function () {
        'use strict';
        angular.module('BHIC.Dashboard.Services').factory('authorisedUserWrapperService', ['$window', '$resource', '$q', '$http', '$log', 'loadingService', 'sharedUserService', authorisedUserWrapperServiceFn]);
        function authorisedUserWrapperServiceFn($window, $resource, $q, $http, $log, loadingService, sharedUserService) {

            var serviceURL = baseUrl;


            // Method is used to handle the success 
            function handleSuccess(res) {
                if (sharedUserService.getUser() == null)
                    sharedUserService.setUser(res.data.user);

                return (res.data === undefined) ? res : res.data;

            }

            // Method is used to catch the error handler from the service.
            function handleError(error) {
                //console.clear();
                //console.log(" HTTP Response Code : " + error.status);
                //console.log(" HTTP Method Type : " + error.config.method);
                //console.log(" Server API Url : " + error.config.url);
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
                var encryptedKey = window.name.split("CYB")[0];                
                try {
                    switch (wrapperObject.method) {
                        // Your Policies Related Api Calls.                  
                        case "SetPolicyNumber":
                            return $http.post(serviceURL + 'YourPolicies/SetPolicyNumber?PolicyNumber=' + queryString).then(handleSuccess, handleError);
                            break;
                        case "GetYourPolicies":
                            return $http.get(serviceURL + 'YourPolicies/GetYourPolicies').then(handleSuccess, handleError);
                            break;

                            // Saved Quotes Related Api Calls.
                        case "GetQuotes":
                            return $http.get(serviceURL + 'SavedQuotes/GetQuotes').then(handleSuccess, handleError);
                            break;

                        case "DeleteQuote":
                            return $http.post(serviceURL + 'SavedQuotes/DeleteQuote', postData).then(handleSuccess, handleError);
                            break;

                        case "CheckUserStatus":
                            return $http.get(serviceURL + 'SavedQuotes/CheckUserStatus').then(handleSuccess, handleError);
                            break;

                            // Policy Information Related Api Calls.  
                        case "GetPolicyDetailByPolicyNumber":
                            return $http.post(serviceURL + 'PolicyInformation/GetPolicyDetailByPolicyNumber', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;
                        case "GetPolicyDetail":
                            return $http.get(serviceURL + 'PolicyInformation/GetPolicyDetail').then(handleSuccess, handleError);
                            break;
                        case "GetPolicyWithTransactionCode":
                            return $http.post(serviceURL + 'PolicyInformation/GetPolicyDetailByPolicyNumber?CYBKey=' + window.name.split("CYB")[0] + '&transactionCode=' + queryString).then(handleSuccess, handleError);
                            break;
                        case "UpdatePaymentOption":
                            return $http.post(serviceURL + 'PolicyInformation/UpdatePaymentOption', postData).then(handleSuccess, handleError);
                            break;

                            // Policy Document Related Api Calls.
                        case "GetPolicyDocument":
                            return $http.post(serviceURL + 'PolicyDocument/GetPolicyDocument', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                        case "DownloadPolicyDocument":
                            return $http.get(serviceURL + 'PolicyDocument/DownloadPolicyDocument?documentId=' + documentId).then(handleSucc, handleError);
                            break;

                            // Report Claim Related Api Calls.
                        case "RequestReportClaim":
                            return $http.post(serviceURL + 'ReportClaim/RequestReportClaim', postData).then(handleSuccess, handleError);
                            break;

                            // Certificate of Insurance Related Api Calls.
                        case "GetCertificateofInsurance":
                            return $http.post(serviceURL + 'RequestCertificate/GetCertificateofInsurance', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                        case "RequestCertificate":
                            return $http.post(serviceURL + 'RequestCertificate/RequestCertificate', postData).then(handleSuccess, handleError);
                            break;

                            // Change Policy Related Api Calls.
                        case "GetLobPolicyChangeOptions":
                            return $http.post(serviceURL + 'RequestPolicyChange/GetLobPolicyChangeOptions', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                        case "SendPolicyChangeRequest":
                            return $http.post(serviceURL + 'RequestPolicyChange/SendPolicyChangeRequest', postData).then(handleSuccess, handleError);
                            break;

                            // Make Payment Related Api Calls.
                        case "GetBillingDetails":
                            //encryptedKey  = window.name.split("CYB")[0] + " " + localStorage.getItem(window.name.split("CYB")[0]);
                            return $http.post(serviceURL + 'MakePayment/GetBillingDetails', { "CYBKey": queryString }).then(handleSuccess, handleError).then(handleSuccess, handleError);
                            break;
                        case "GetBillingDetailsWithErrorId":
                            return $http.get(serviceURL + 'MakePayment/GetBillingDetails?errorId=' + queryString).then(handleSuccess, handleError).then(handleSuccess, handleError);
                            break;

                        case "UpdatePaymentAmount":
                            return $http.post(serviceURL + 'MakePayment/UpdatePaymentAmount', postData).then(handleSuccess, handleError);
                            break;

                            // Physician Panel Related Api Calls.
                        case "GetPhysicianDocument":
                            return $http.post(serviceURL + 'PhysicianPanel/GetPhysicianDocument', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                        case "DownloadPhysicianDocument":
                            return $http.get(serviceURL + 'PhysicianPanel/DownloadPhysicianDocument?documentId=' + documentId, { responseType: 'arraybuffer' }).then(handleSucc, handleError);
                            break;

                            // Edit Content Related Api Calls.
                        case "GetCurrentUserData":
                            return $http.get(serviceURL + 'EditContactInfo/GetCurrentUserData').then(handleSuccess, handleError);
                            break;

                            // Cancel Policy Related Api Calls.
                        case "CancelPolicy":
                            return $http.post(serviceURL + 'CancelPolicy/CancelPolicy', postData).then(handleSuccess, handleError);
                            break;

                            // Change Password Related Api Calls.
                        case "PasswordChangeRequest":
                            return $http.post(serviceURL + 'ChangePassword/PasswordChangeRequest', postData).then(handleSuccess, handleError);
                            break;

                            // Contact Information Related Api Calls.
                        case "UpadteContactInfo":
                            return $http.post(serviceURL + 'EditContactInfo/UpadteContactInfo', postData).then(handleSuccess, handleError);
                            break;

                        case "UpdateContactAddress":
                            return $http.post(serviceURL + 'EditContactAddress/UpdateContactAddress', postData).then(handleSuccess, handleError);
                            break;

                            // Cancel Policy Related Api Calls.
                        case "CancelPolicy":
                            return $http.post(serviceURL + 'CancelPolicy/CancelPolicy', postData).then(handleSuccess, handleError);
                            break;

                            // Signout Related Api Call.
                        case "SignOut":
                            return $http.post(serviceURL + 'Login/SignOut').then(logoutSuccess, handleError);
                            break;

                        case "GlobalSignOut":
                            return $http.post(serviceURL + 'Login/GlobalSignOut').then(logoutSuccess, handleError);
                            break;

                        // Cancel Policy Related Api Calls.
                        case "GetPolicyCancelOptions":
                            return $http.post(serviceURL + 'CancelPolicy/GetPolicyCancelOptions', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                        case "DeleteCookie":
                            return $http.post(serviceURL + 'SavedQuotes/DeleteCookie').then(handleSuccess, handleError);
                            break;
                        // Upload Document Related Api Calls.
                        case "RequestUploadDocuments":
                            return $http.post(serviceURL + 'UploadDocuments/RequestUploadDocuments', postData).then(handleSuccess, handleError);
                            break;

                        // Get Policy Specific user detail
                        case "GetPolicySpecificUserDetail":
                            return $http.post(serviceURL + 'YourPolicies/GetPolicySpecificUserDetail', { "CYBKey": encryptedKey }).then(handleSuccess, handleError);
                            break;

                            // Get Contact Information for specific policy
                        case "GetContactInformation":
                            return $http.post(serviceURL + 'EditContactInfo/GetContactInformation', { "CYBKey": postData }).then(handleSuccess, handleError);
                            break;

                        case "GetAddress":
                            return $http.post(serviceURL + 'EditContactAddress/GetAddress', { "CYBKey": queryString, contactType: postData }).then(handleSuccess, handleError);
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