(
    function () {
        'use strict';

        function registrationController($scope, userProfile, restServiceConsumer) {

            var self = this;
            var _userModel =
             {
                 email: '',
                 password: '',
                 confirmPassword: '',
                 firstName: '',
                 lastName: '',
                 phoneNumber: '',
                 policyCode: ''
             };

            var _submit = function (userModel) {

                userProfile.submitUserData(userModel).
                then(function (response) {
                    window.location.href = '/Account/WcAccount/DisplayEmail';
                });
            };

            angular.extend(self, {
                submitUser: _submit,
                userModel: _userModel,
            });
        };

        /**
          * @module : bhic-app.wc
          * @moduledesc : Worker compensation (wc) application module will register this controllers to take quesionnaire page related activities
                       this will generate dynamic list of question before generating quote based question response it will show quote in next step
          * @ngInject : inject $scope module into controller for certain Moel-View communication
       */
        angular
           .module('bhic-app.wc')
        .controller('registrationCtrl', ['$scope', 'bhic-app.wc.services.userProfile', 'bhic-app.services.restServiceConsumer', registrationController]);
    }
)();