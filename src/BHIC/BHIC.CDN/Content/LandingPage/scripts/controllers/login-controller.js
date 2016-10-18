(function () {
    'use strict';
    angular.module("BHIC.LandingPage.Controllers").controller('loginController', ['$scope', '$location', 'loadingService', 'sharedUserService', 'authorisedUserWrapperService', loginControllerFn])
    function loginControllerFn($scope, $location, loadingService, sharedUserService, authorisedUserWrapperService) {

        // Controler funciton Start  write any methods/functions after this line.

        $scope.Login = function () {
            var user = { "username": $scope.uname, "password": $scope.password };
            var wrapper = { "method": "Authentication", "postData": user };

            authorisedUserWrapperService.AuthorisedUserApiCall(wrapper).then(function (response) {
                if (response.success) {
                    window.localStorage.setItem('isSessionCreated', true);
                    window.localStorage.setItem('user', JSON.stringify(user));
                    sharedUserService.setUser(user);
                    $location.path("LandingPages");
                }
                else {
                    $scope.responseMsg = "Invalid user credentials..!"
                }
                loadingService.hideLoader();
            });
        }

        $scope.eventPropagation = function (sender) {
            if (sender.which == 13) {
                if ($scope.uname === undefined || $scope.password === undefined)
                    $scope.responseMsg = "Please enter user name and password..!";
                else
                    $scope.Login();
            }
        }

        $scope.clearLogin = function () {
            window.localStorage.clear();
            sharedUserService.setUser(null);
            $location.path("/Login");
        }

        loadingService.hideLoader();
        // Controler funciton End Please dont write any methods/functions after this line.      
    }
})();
