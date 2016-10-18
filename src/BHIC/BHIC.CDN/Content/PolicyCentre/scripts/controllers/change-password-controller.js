(
function () {
    'use strict';
    //Comment : Here module controller declaration and injection
    angular.module("BHIC.Dashboard.Controllers").controller('changePasswordController', ['$scope', '$location', 'loadingService', 'authorisedUserWrapperService', changePasswordControllerFn]);
    function changePasswordControllerFn($scope, $location, loadingService, authorisedUserWrapperService) {
        // Controler funciton Start  write any methods/functions after this line.
      
        //variable declaration
        var _self = this;
        $scope.responseMsg = false;
        $scope.focusField = true;
        $scope.emptyString = "";
        $scope.errorMessage = $scope.emptyString;
        $scope.status = $scope.emptyString;
        nosidemenu(true);
        loadingService.hideLoader();


        //defaultForm
        $scope.defaulPasswordForm = function () {
            $scope.oldPassword = $scope.emptyString;
            $scope.newPassword = $scope.emptyString;
            $scope.confirmNewPassword = $scope.emptyString;
            $scope.submitted = false;
        };

        // method to for changePassword form submission.
        $scope.changePassword = function () {
            if ($scope.newPassword != $scope.confirmNewPassword) {
                $('html, body').animate({ scrollTop: 0 }, 800);
                $("#confNewpassord").focus();
                return;
            }
            $scope.errorMessage = $scope.emptyString;
            $scope.responseMsg = false;
            var _self = {};
            loadingService.showLoader();
            _self.OldPassword = $scope.oldPassword;
            _self.NewPassword = $scope.newPassword;
            _self.ConfirmPassword = $scope.confirmNewPassword;
            var wrapper = { "method": "PasswordChangeRequest", "postData": _self };
            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    $scope.responseMsg = true;
                    $scope.defaulPasswordForm();
                    $scope.changePasswordForm.$setPristine();
                    _self.successMessage = response.successMessage;
                }
                else {
                    $scope.errorMessage = response.errorMessage;                    
                }
                loadingService.hideLoader();
                $('html, body').animate({ scrollTop: 0 }, 800);
                $scope.focusField = true;
            });
        };
    };
})();