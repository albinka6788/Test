(
    function () {
        'use strict';
        angular.module('bhic-app.wc.purchaseGet')
        .directive('contactInfoControl', ['$timeout', contactInfoControlFn]);
        function contactInfoControlFn($timeout) {
            return {
                restrict: 'EA',
                templateUrl: '/Scripts/bhic/directives/wc/quoteInput/contactInputControl/contactInputControlTemplate.html',
                link: link
            };
            function link(scope, elem, attrs) {
                function _init() {
                    


                };
                scope.copyContactDetailToBusiness = function () {

                    $timeout(function () {
                        angular.copy(scope.purchaseInputForm.businessContact.businessConfirmEmail, scope.purchaseInputForm.personalContact.ConfirmUserEmail);
                        angular.copy(scope.businessContact.businessConfirmEmail.$viewValue, scope.personalContact.ConfirmUserEmail.$viewValue);
                        console.log(scope.personalContact.ConfirmUserEmail);
                    });

                    //scope.businessContact.businessEmail.modelValue = scope.personalContact.UserEmail.modelValue;
                    //scope.businessContact.businessContactFirstName.modelValue = scope.personalContact.UserFirstName.modelValue;
                    //scope.businessContact.businessContactLastName.modelValue = scope.personalContact.UserLastName.modelValue;
                    //scope.businessContact.businessContactPhone.modelValue = scope.personalContact.UserPhoneNumber.modelValue;

                };
                scope.emptyBusinessDetails = function () {
                    console.log(scope);
                };
                _init();
            }
        }

    }
)();