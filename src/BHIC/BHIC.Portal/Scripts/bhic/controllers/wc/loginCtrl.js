(
function () {
    'use strict';

    function loginController($scope, userProfile, restServiceConsumer) {

        var self = this;

        var _loginModel = {
            email: '',
            password: ''
        };

        var _forgotPasswordModel={
            email:'',
        };

        var _submit = function (loginModel) {
            userProfile.loginUser(loginModel).
            then(function (response) {
                window.location.href = '/Account/WcAccount/LoginSuccess';
                $scope.loginStatus = "error occured";
            });
        };

        var _forgotPasswordSubmit = function (forgotPasswordModel)
        {
            userProfile.forgotPassword(forgotPasswordModel).
                       then(function (response) {
                           window.location.href = '/Account/WcAccount/ForgotPasswordConfirmation';
                       });
        };

            angular.extend(self, {
                submitUser: _submit,
                loginModel: _loginModel,
                forgotPasswordModel: _forgotPasswordModel,
                forgotPasswordSubmit: _forgotPasswordSubmit,
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
        .controller('loginCtrl', ['$scope', 'bhic-app.wc.services.userProfile', 'bhic-app.services.restServiceConsumer', loginController]);
    }
)();