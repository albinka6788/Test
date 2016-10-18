(
    function () {
        'use strict';

        //Comment : Here module controller declaration and injection
        angular.module('BHIC.Controllers').controller('logoutController', ['$scope', '$location', 'loadingService', 'landingPageData', logoutControllerFn]);

        //controller function
        function logoutControllerFn(scope, location, loadingService, landingPageDataProvider)
        {
            var self = this;

            scope.navigate = function (url) {               
                location.path(url);
            }

            //Comment : Here to navigate user to PC sing-out page
            var _policyCenterLogoutUrl = '2131';

            function _init(logoutUrl)
            {
                //debugger;                
                loadingService.hideLoader();
                if (logoutUrl != null && logoutUrl != '')
                {
                    scope.policyCenterLogoutUrl = logoutUrl;
                }
            };

            var _logOutUser = function ()
            {
                var promise = landingPageDataProvider.signOutUser();

                promise
                .then(function (data)
                {
                    if (data.resultStatus == 'OK')
                    {
                        console.log(data.resultText);
                        window.location.href = scope.policyCenterLogoutUrl;
                    }
                })
                .catch(function (exception)
                {
                    //console.log(exception.toUpperCase());
                })
                .finally(function ()
                {
                    loadingService.hideLoader();
                });
            }

            var _openScheduleCallModel = function () {
                //Comment : Here if form has been validated then show model POP-UP for further information
                var modelBox = $('[data-remodal-id=scheduleCallModel]').remodal();

                //if not undefined
                if (modelBox) {
                    modelBox.open();
                    $('.remodal-overlay').addClass("overlay-light");
                }
            }

            //_init('Prem');

            //Comment : Here extend properties & function to access publicy using controller scope
            angular.extend(scope, {
                init: _init,
                logOutUser: _logOutUser,
                policyCenterLogoutUrl: _policyCenterLogoutUrl,
                openScheduleCallModel: _openScheduleCallModel
            });

        };
    }
)();