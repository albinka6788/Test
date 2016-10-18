(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('forgotPasswordController', ['$scope', '$location', 'loadingService', 'captchaService', 'unauthorisedUserWrapperService', forgotPasswordControllerFn]);
    function forgotPasswordControllerFn($scope, $location, loadingService, captchaService, unauthorisedUserWrapperService) {
        
        // Controler funciton Start  write any methods/functions after this line.
        $('html, body').animate({ scrollTop: 0 }, 800);
            //variable declaration
            var _self = this;
            $scope.responseMsg = false;
            $scope.errormsg = "";
            $scope.status = "";
            $scope.recaptchaResponse = "";
            $scope.submitted = false;
            $scope.focusField = true;
            $('.rootText').text('');

            loadingService.hideLoader();

            function sendLink() {
                _self.EmailId = $scope.Email;
                var wrapper = { "method": "ForgotPasswordRequest", "postData": _self };
                unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                    $scope.responseMsg = response.success
                    if (response.success) {
                        _self.successMessage = response.successMessage;
                    } else if (response.redirectStatus) {
                        $location.path("/Error/");
                    } else {
                        ResetCaptcha();
                        $scope.errorMsg = response.errorMessage;
                        $scope.focusField = true;
                    }
                    loadingService.hideLoader();
                });
            }
            //Captcha Functionality
            $scope.sendForgotPasswordLink = function () {
                loadingService.showLoader();
                //sendLink();       
                var code = GetCaptchaResponse();
                if (code != '') {
                    captchaService.validateCaptcha(code).then(function (res) {
                        if (res.status)
                            sendLink();
                        else {
                            ResetCaptcha();
                            $scope.responseMsg = false;
                            $scope.errorMsg = "Please verify you're human by verifying the Captcha";
                            loadingService.hideLoader();
                        }
                    });
                }
                else {
                    $scope.responseMsg = false;
                    $scope.errorMsg = "Please verify you're human by verifying the Captcha";
                    loadingService.hideLoader();
                }
            };

            // Controler funciton End Please dont write any methods/functions after this line.       
    };

})();