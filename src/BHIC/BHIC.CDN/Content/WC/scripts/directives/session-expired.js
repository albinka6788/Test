
(
    function ()
    {
        'use strict';
        
        //directive declaration
        angular.module('BHIC.WC.Directives').directive('sessionExpired', ['$location', 'loadingService', 'appConfig', sessionExpiredFn]);

        //directive implementation logic method
        function sessionExpiredFn($location,loadingService, appConfig)
        {
            return {
                restrict: 'AE',
                replace: true,
                controller: function ($scope,$location, loadingService)
                {
                    var self = this;

                    //Comment : Here default count down timer seconds
                    var _timeCountDown = 10;
                    var _updateTimerAfter = 1000;

                    function _init()
                    {
                        
                        loadingService.hideLoader();
                        //console.log('Controller called !');
                        setTimeout(redirectUser, _updateTimerAfter);
                    };

                    var redirectUser = function()
                    {                
                        self.timeCountDown--;
                        $scope.$apply();

                        //Comment : Here time spent then redirect user to home page
                        if (self.timeCountDown == 0)
                        {
                            loadingService.hideLoader();
                            //location.path("/Home");
                            window.location.href = appConfig.appBaseUrl;
                            //window.location.href = appConfig.appBaseDomain + '/Home/Index'; //Old Line
                            //window.location.href = '/PurchasePath/Home/Index'; //Oldest line                            
                        }
                        else {
                            loadingService.hideLoader();
                            //console.log(self.timeCountDown);
                            setTimeout(redirectUser, _updateTimerAfter);
                        }
                    }

                    //Comment : Here extend properties & function to access publicy using controller scope
                    angular.extend(self, {
                        init: _init,
                        timeCountDown: _timeCountDown
                    });

                    //Comment : Here if it will work on browser then restrict browser <- & -> buttons clicks
                    $scope.$on('$locationChangeStart', function (event, next, current)
                    {
                        // Here you can take the control and call your own functions:
                        // Prevent the browser default action (Going back):
                        event.preventDefault();
                    });
                },
                controllerAs: 'sessionExpiredCtrl',
                template: ['<div class="container" data-ng-init="sessionExpiredCtrl.init()">                                                         ',
                           '    <div class="alert alert-warning alert-big">                                                                          ',
                           '        <p class="text-bold alert-text-big"> Session Expired! </p>                                                       ',
                           '        <p class="alert-text"> Your session has timed out. You will be redirected to Home page in a short while.&nbsp;   ',
                           '        <b><i data-ng-bind-template="{{sessionExpiredCtrl.timeCountDown}} secs..."></i></b></p>                                 ',
                           '    </div>                                                                                                               ',
                           '</div>'].join(''),

                /*template: ['<div class="container" data-ng-init="sessionExpiredCtrl.init()">                                                  ',
                           '        <div class="row">                                                                                               ',
                           '            <div class="lg-col-8">                                                                                      ',
                           '                <div class="content">                                                                                   ',
                           '                    <h1 class="text-bold">Session Expired</h1>                                                          ',
                           '                    <div class="alert alert-info" style="color:#000">                                                   ',
                           '                        <p>                                                                                             ',
                           '                            Your session has timed out. You will be redirected to Home page in a short while. &nbsp;    ',
                           '            <b>                                                                                                         ',
                           '                <i data-ng-bind-template="{{sessionExpiredCtrl.timeCountDown}} secs..."></i>                             ',
                           '            </b>                                                                                                        ',
                           '        </p>                                                                                                            ',
                           '    </div>                                                                                                              ',
                           '    <br />                                                                                                              ',
                           '    <br />                                                                                                              ',
                           '    <br />                                                                                                              ',
                           '    <br />                                                                                                              ',
                           '</div>                                                                                                                  ',
                           '</div>                                                                                                                  ',
                           '</div>                                                                                                                  ',
                           '</div>'].join(''),*/

            }
        }
    }
)();