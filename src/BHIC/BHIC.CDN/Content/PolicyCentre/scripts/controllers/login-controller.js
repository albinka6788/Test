
(function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('loginController', ['$scope', '$rootScope', '$http', '$timeout', '$location', '$window', 'loadingService', 'unauthorisedUserWrapperService', 'captchaService', loginControllerFn]);
    function loginControllerFn($scope, $rootScope, $http, $timeout, $location, $window, loadingService, unauthorisedUserWrapperService, captchaService) {
        // Controler funciton Start  write any methods/functions after this line.
        var _self = this;
        $scope.valid = false;
        $scope.status = "";
        $scope.formerrormsg = "";
        $scope.resendemail = "";
        $scope.verifiedemail = false;
        $scope.afterverification = false;
        $scope.focusField = true;
        $('.rootText').text('');
        loadingService.hideLoader();


        //Default form
        $scope.defaultLoginForm = function () {
            $scope.valid = false;
            $scope.resendemail = $scope.Email;
            $scope.afterverification = false;
            $scope.Email = "";
            $scope.Password = "";
            $scope.submitted = false;
            $scope.loginform.$setPristine();
            $('#txtemail').focus();
        };

        //Resend the Mail functionality

        $scope.resendMailFn = function () {
            _self.user = {};
            _self.user.Email = $scope.resendemail;
            loadingService.showLoader();
            var wrapper = { "method": "ResendMail", "postData": _self.user };
            unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.verifiedemail = false;
                    $scope.afterverification = true;
                    $scope.Email = "";
                    $scope.Password = "";
                    $('html, body').animate({ scrollTop: 0 }, 800);
                }
                loadingService.hideLoader();
                $scope.focusField = true;
            });
        }


        $scope.resetMsg = function () { $scope.valid = false; };

        //setPageforLogin(true);
        $scope.checkUserAutntication = function () {
            if ($scope.captcha) {
                var code = GetCaptchaResponse();
                if (code == "") {
                    $scope.valid = true;
                    $scope.responseMsg = "Please verify you're human by verifying the Captcha.";
                    loadingService.hideLoader();
                }
                else {
                    captchaService.validateCaptcha(code).then(function (res) {
                        if (res.status)
                            captchaResponse()
                        else {
                            ResetCaptcha();
                            $scope.valid = true;
                            $scope.responseMsg = "Please verify you're human by verifying the Captcha.";
                            loadingService.hideLoader();
                        }
                    });
                }
            }
            else {
                captchaResponse();
            }
        }

        function captchaResponse() {
            _self.user = {};
            _self.user.Email = $scope.Email;
            _self.user.Password = $scope.Password;
            $scope.captcha = false;
            loadingService.showLoader();
            var wrapper = { "method": "UserLogin", "postData": _self.user };
            unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success && response.isEmailverified) {
                    $window.localStorage.setItem('isSessionCreated', true)
                    $window.localStorage.setItem('user', JSON.stringify(response.user));
                    window.location.href = baseUrl + "Dashboard/Index";
                }
                else if (response.success && !response.isEmailverified) {
                    $scope.verifiedemail = true;
                    $scope.defaultLoginForm();
                    loadingService.hideLoader();
                    $scope.focusField = true;
                }
                else {
                    ResetCaptcha();
                    loadingService.hideLoader();
                    $scope.valid = true;
                    $scope.responseMsg = response.errorMessage;
                    $scope.captcha = true;
                    $scope.focusField = true;
                }
            });
        }

        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
