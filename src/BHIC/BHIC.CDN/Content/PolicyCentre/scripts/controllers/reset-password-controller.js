(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('resetPasswordController', ['$scope', '$location', 'loadingService', '$timeout', 'unauthorisedUserWrapperService', 'sharedUserService', 'emailId', 'captchaService', resetPasswordControllerFn]);
    function resetPasswordControllerFn($scope, $location, loadingService, $timeout, unauthorisedUserWrapperService, sharedUserService, emailId, captchaService) {

        // Controler funciton Start  write any methods/functions after this line.
        $('html, body').animate({ scrollTop: 0 }, 800);
        //variable declaration
        var _self = this;
        $scope.status = false;
        $scope.isResponse = false;
        loadingService.hideLoader();

        $scope.Email = (emailId.decryptedEmail == "") ? null : emailId.decryptedEmail;
        $scope.status = (emailId.decryptedEmail == "") ? true : false;

        var clearText = function () {
            $scope.Password = "";
            $scope.confirmPassword = "";
        }

        //function to reset the password link 
        $scope.ResetPassword = function () {
            if ($scope.Password != $scope.confirmPassword) {
                return;
            }
            loadingService.showLoader();
            var code = GetCaptchaResponse();
            if (code != '') {
                captchaService.validateCaptcha(code).then(function (res) {
                    if (res.status)
                        $scope.Reset();
                    else {
                        ResetCaptcha();
                        loadingService.hideLoader();
                    }
                });
            }
            else {
                loadingService.hideLoader();
                $scope.prCapthcaError = "Please verify you're human by verifying the Captcha.";
            }

        };

        $scope.Reset = function () {
            var resetUser = {};
            resetUser.EmailID = $location.$$search.queryKey;
            resetUser.NewPassword = $scope.Password;
            resetUser.ConfirmPassword = $scope.confirmPassword;            
            var wrapper = { "method": "ResetPasswordRequest", "postData": resetUser };
            unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                $scope.status = response.success;
                if (response.success) {
                    clearText();
                } else {
                    $scope.errorMessage = response.errorMessage;
                    $scope.ResetPwdForm.$setPristine();
                    ResetCaptcha();
                }
                $scope.isResponse = true;
                loadingService.hideLoader();
            });
        }

        // Controler funciton End Please dont write any methods/functions after this line.

    };

})();