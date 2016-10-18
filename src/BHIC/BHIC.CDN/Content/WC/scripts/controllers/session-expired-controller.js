(
    function ()
    {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module("BHIC.WC.Controllers").controller('sessionExpiredController', ['$scope', '$timeout', '$location', 'loadingService', 'appConfig', sessionExpiredControllerFn]);

        //controller function
        function sessionExpiredControllerFn(scope, timeout, location, loadingService, appConfig)
        {
            var self = this;

            //Comment : Here default count down timer seconds
            var _timeCountDown = 20;
            var _updateTimerAfter = 1000;

            function _init()
            {
                loadingService.hideLoader();
                //console.log('Controller called !');
                timeout(redirectUser, _updateTimerAfter);
            };

            var redirectUser = function()
            {                
                self.timeCountDown--;
                scope.$apply();

                //Comment : Here time spent then redirect user to home page
                if (self.timeCountDown == 0)
                {
                    loadingService.hideLoader();
                    //location.path("/Home");
                    window.location.href = appConfig.appBaseUrl;
                    //window.location.href = '/PurchasePath/Home/Index';
                }
                else {
                    loadingService.hideLoader();
                    //console.log(self.timeCountDown);
                    timeout(redirectUser, _updateTimerAfter);
                }
            }

            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(self, {
                init: _init,
                timeCountDown: _timeCountDown
            });

        };
    }
)();