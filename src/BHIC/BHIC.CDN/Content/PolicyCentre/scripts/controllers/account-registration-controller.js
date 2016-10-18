(function () {
    'use strict';
    angular.module("BHIC.Dashboard.Controllers").controller('accountRegistrationController', ['$scope', '$location', 'loadingService', 'captchaService', 'sharedUserService', 'unauthorisedUserWrapperService', accountRegistrationFn])
    function accountRegistrationFn($scope, $location, loadingService, captchaService, sharedUserService, unauthorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.

        //variable declaration
        var _self = this;
        $scope.hasError = false;
        $scope.hasErrorMain = false;
        $scope.focusField = true;
        $('.rootText').text('');
        $('.sidebar-left').css('display', 'none');

        loadingService.hideLoader();

        //Default form
        $scope.emptyString = "";

        $scope.defaultRegistrationForm = function () {
            $scope.fname = $scope.emptyString;
            $scope.lname = $scope.emptyString;
            $scope.orgname = $scope.emptyString;
            $scope.phone = $scope.emptyString;
            $scope.policycode = $scope.emptyString;
            $scope.email = $scope.emptyString;
            $scope.password = $scope.emptyString;
            $scope.confirmpassword = $scope.emptyString;
            $scope.phoneerrormsg = $scope.emptyString;
            $scope.submitted = false;
        };

        // Method is used to add/update the users.
        $scope.CreateOrUpdateAccount = function () {

            var number = sharedUserService.CheckPhoneNumber($scope.phone);
            if (number.length < 10) {
                $scope.phoneerrormsg = "Please Enter Valid Phone Number!";
                $('html, body').animate({ scrollTop: 0 }, 800);
                $("#reportphone").focus();
                return;
            }
            else {
                $scope.phoneerrormsg = $scope.emptyString;
            }

            loadingService.showLoader();
            var code = GetCaptchaResponse();
            if (code != '') {
                captchaService.validateCaptcha(code).then(function (res) {
                    if (res.status)
                        createAccount()
                    else {
                        ResetCaptcha();
                        $scope.responseMsg = false;
                        $scope.captchaMsg = "Please verify you're human by verifying the Captcha";
                        $scope.hasErrorMain = true;
                        loadingService.hideLoader();
                    }
                });
            }
            else {
                $scope.responseMsg = false;
                $scope.captchaMsg = "Please verify you're human by verifying the Captcha";
                $scope.hasErrorMain = true;
                loadingService.hideLoader();
            }
        }   

        function createAccount() {
            //$scope.hasError = true;
            _self.user = {};
            _self.user.Id = ($scope.userId == undefined) ? 0 : $scope.userId;

            // Method used to call service.
            if (_self.user.Id == 0) {
                _self.user.FirstName = $scope.fname;
                _self.user.LastName = $scope.lname;
                _self.user.OrganizationName = $scope.orgname;
                _self.user.PhoneNumber = sharedUserService.CheckPhoneNumber($scope.phone);
                _self.user.PolicyCode = $scope.policycode;
                _self.user.Email = $scope.email;
                _self.user.Password = $scope.password;
                _self.user.confirmpassword = $scope.confirmpassword;
                var wrapper = { "method": "CreateAccount", "postData": _self.user }
                unauthorisedUserWrapperService.UnAuthorisedUserApiCall(wrapper).then(function (response) {
                    if (response.success) {
                        $scope.resMsg = true;
                        $scope.hasErrorMain = false;
                        $scope.defaultRegistrationForm();
                        $scope.regForm.$setPristine();
                    } else {
                        $scope.errorMessage = response.errorMessage;
                        $scope.hasErrorMain = true;
                        $scope.resMsg = true;
                    }
                    ResetCaptcha();
                    $('html, body').animate({ scrollTop: 0 }, 800);
                    loadingService.hideLoader();
                    $scope.focusField = true;
                });
            } else {
                ResetCaptcha();
                $scope.errorMessage = "Please verify you're human by verifying the Captcha";
                $scope.hasErrorMain = true;
                loadingService.hideLoader();
            }
        }
        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
