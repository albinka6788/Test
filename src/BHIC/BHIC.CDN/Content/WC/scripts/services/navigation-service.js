(
    function () {
        'use strict';

        angular.module('BHIC.Services').
            factory('navigationService', [navigationServiceFn]);

        function navigationServiceFn() {

            function _enableNavigation(lnkClass) {

                //Comment : Here get target control element
                var targetElement = $('.progress-bar').find('li#' + lnkClass);

                //add complete class on buy-policy menu if user comes directly to buy Policy
                var targetClass = targetElement.attr('class');

                if (targetClass.split(' ')[1] != 'complete') {
                    //highlight buy policy  navigation menu item
                    targetElement.addClass('complete');
                }
            }

            return {
                enableNavigation: _enableNavigation
            };
        }
    }
)();